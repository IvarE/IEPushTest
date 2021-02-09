using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using CRM2013.SkanetrafikenPlugins.CreateGiftcardService;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public class ReimbursementHandler
    {
        #region Public Methods
        public void ExecuteRefundAndUpdatesStatus(Guid RefundId, IOrganizationService service)
        {
            Entity Refund = service.Retrieve("cgi_refund", RefundId, new ColumnSet(new string[] { 
                "cgi_refundtypeid", 
                "cgi_reimbursementformid", 
                "cgi_travelcard_number", 
                "cgi_accountid", 
                "cgi_contactid", 
                "cgi_refundnumber", 
                "cgi_caseid",
                "cgi_amount", 
                "cgi_mobilenumber",
                "statecode", 
                "statuscode",
                "cgi_last_valid"
            }));

            int state = ((OptionSetValue)Refund.Attributes["statecode"]).Value;
            int status = ((OptionSetValue)Refund.Attributes["statuscode"]).Value;

            //CreateAnnotation("ExecuteRefundAndUpdatesStatus message", "this is a test message", new EntityReference(Refund.LogicalName, Refund.Id));
            //Genomför eventuell utbetalning och sätt status på beslutet [Refund].
            //State value: 0 = active, corresponding Status values: 1 = New, 285050003 = Approved - Transaction Pending, 285050004 = Approved -Transaction failed, 285050005 = Approved
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved

            if ((state == 0 && status == 285050005) || (state == 0 && status == 285050004))
            {
                //285050003 = Approved - Transaction Pending
                //285050004 = Approved -Transaction failed
                //try to execute the Refund
                ReimbursementHandler rh = new ReimbursementHandler();
                if (rh.ExecuteRefundTransaction(Refund, true, service))
                {
                    //close the incident if refund is executed

                    _SetRefundDescisionStatus(RefundId, 0, 285050005, service);//Active - Approved
                    if (Refund.Attributes.Contains("cgi_caseid") && Refund.Attributes["cgi_caseid"] != null)
                    {
                        _closeIncident(((EntityReference)Refund.Attributes["cgi_caseid"]).Id, service);
                    }
                }
                else
                {
                    _SetRefundDescisionStatus(RefundId, 0, 285050004, service);//Active - Approved -Transaction failed
                    //leave incident open
                }
            }
        }

        public bool ExecuteRefundTransaction(Entity Refund, bool IsAutomatic, IOrganizationService service)
        {
            try
            {
                string EHOrderNumber = "";
                string ValueCode = "";

                //attempt to execute the refund

                if (!Refund.Contains("cgi_reimbursementformid"))
                {
                    throw new UserMessageException("Kunde inte behandla utbetalning eftersom utbetalningsform saknas.");
                }

                EntityReference rf_id = Refund.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                Entity reimbursementForm = service.Retrieve(rf_id.LogicalName, rf_id.Id, new ColumnSet("cgi_loadcard", "cgi_giftcard", "cgi_couponsms", "cgi_couponemail"));

                //Avbryt om ingen automatisk utbetalning ska exekveras
                if (!(reimbursementForm.RefundedViaLoadCard()
                    || reimbursementForm.RefundedViaGiftCard()
                    || reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaEmail()))
                {
                    if (IsAutomatic)
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms");
                    else
                        return true;
                }


                EntityReference incidentReference = Refund.GetAttributeValue<EntityReference>("cgi_caseid");
                Entity incident = service.Retrieve(incidentReference.LogicalName, incidentReference.Id, new ColumnSet("customerid", "cgi_customer_email"));

                EntityReference CustomerId = null;
                if (incident.Contains("customerid"))
                    CustomerId = incident.GetAttributeValue<EntityReference>("customerid");
                else
                    throw new UserMessageException("Ärendet saknar kund!");

                string EmailAddress = null;

                if (Refund.Contains("cgi_email"))
                {
                    EmailAddress = Refund.GetAttributeValue<string>("cgi_email");
                }
                else if (incident.Contains("cgi_customer_email"))
                {
                    EmailAddress = incident.GetAttributeValue<string>("cgi_customer_email");
                }
                else if (!(reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaLoadCard())) //If the code is sent via SMS, we don't need an email address.
                    throw new UserMessageException("Ärendet saknar epostadress!");

                string _CardSerialNumber = Refund.Contains("cgi_travelcard_number") ? Refund.GetAttributeValue<string>("cgi_travelcard_number") : null;
                decimal _Amount = Refund.Contains("cgi_amount") ? Refund.GetAttributeValue<Money>("cgi_amount").Value : 0;

                string setting_cgi_chargeorderservice;
                string setting_cgi_createcouponservice;
                string setting_cgi_createemailcouponservice;
                string setting_cgi_sendvaluecodemailservice;
                string setting_cgi_giftcardservice;
                EntityReference setting_cgi_defaultcustomeroncase;
                int cgi_valuecodevalidformonths;
                GetSettings(service, out setting_cgi_chargeorderservice, out setting_cgi_createcouponservice, out setting_cgi_createemailcouponservice, out setting_cgi_sendvaluecodemailservice, out setting_cgi_giftcardservice, out setting_cgi_defaultcustomeroncase, out cgi_valuecodevalidformonths);

                EntityReference ERRefund = new EntityReference(Refund.LogicalName, Refund.Id);

                if (reimbursementForm.RefundedViaLoadCard())
                {
                    if (_Amount == 0 || _CardSerialNumber == null)
                        return true;

                    LoadCard(_Amount, _CardSerialNumber, CustomerId.Id, EmailAddress, ERRefund, setting_cgi_chargeorderservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaGiftCard())
                {
                    if (setting_cgi_defaultcustomeroncase.Id.Equals(CustomerId.Id))
                        throw new UserMessageException("Kunde inte skapa värdekod för anonym kund!");

                    DateTime cgi_last_valid = DateTime.Now.AddMonths(6);
                    if (Refund.Contains("cgi_last_valid") && Refund["cgi_last_valid"] != null)
                        cgi_last_valid = Refund.GetAttributeValue<DateTime>("cgi_last_valid");

                    CreateGiftcard(CustomerId, EmailAddress, cgi_last_valid, _Amount, ERRefund, setting_cgi_giftcardservice, setting_cgi_sendvaluecodemailservice, ref ValueCode, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaSms())
                {
                    string cgi_mobilenumber = null;
                    if (Refund.Contains("cgi_mobilenumber"))
                        cgi_mobilenumber = Refund.GetAttributeValue<string>("cgi_mobilenumber");
                    else
                        throw new UserMessageException("Ärendet saknar telefonnummer!");

                    string smstext = String.IsNullOrEmpty(EmailAddress) ? "," : EmailAddress; //Cannot send an empty string as argument to the service, see RedMine #2437

                    CreateCouponSMS(_Amount, CustomerId.Id, smstext, cgi_mobilenumber, "0", ERRefund, setting_cgi_createcouponservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else if (reimbursementForm.RefundedViaEmail())
                {
                    CreateEmailCoupon(_Amount, _CardSerialNumber, CustomerId.Id, EmailAddress, EmailAddress, "0", ERRefund, setting_cgi_createemailcouponservice, ref EHOrderNumber, service, IsAutomatic);
                }
                else
                {
                    if (IsAutomatic)
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras baserat på vald utbetalningsform. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms, cgi_couponemail");
                }

                Refund.Attributes["cgi_ehordernumber"] = EHOrderNumber;
                Refund.Attributes["cgi_value_code"] = ValueCode;

                if (IsAutomatic)
                    service.Update(Refund); //not needed when called from pre-executing plugin

                return true;
            }
            catch (Exception ex)
            {
                if (IsAutomatic)
                {
                    try
                    {
                        if (ex is UserMessageException)
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction", "User messge:" + ex.Message + " | Original error:" + (ex.InnerException != null ? ex.InnerException.ToString() : ""), new EntityReference(Refund.LogicalName, Refund.Id), service);
                            Refund.Attributes["cgi_errormessage"] = ex.Message;
                        }
                        else
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction", ex.ToString(), new EntityReference(Refund.LogicalName, Refund.Id), service);
                            Refund.Attributes["cgi_errormessage"] = "Ett oförutsett fel inträffade. Se återbetalningens anteckningar för teknisk information.";
                        }
                        service.Update(Refund);
                    }
                    catch { }
                    return false;
                }
                else
                {
                    if (ex is UserMessageException)
                    {
                        throw new InvalidPluginExecutionException(ex.Message, ex);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Ett oförutsett fel inträffade. Försök igen eller välj en annan återbetalningsform.", ex);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void _SetRefundDescisionStatus(Guid RefundId, int state, int status, IOrganizationService service)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed


            SetStateRequest statusrequest = new SetStateRequest();
            statusrequest.EntityMoniker = new EntityReference("cgi_refund", RefundId);
            statusrequest.State = new OptionSetValue(state);
            statusrequest.Status = new OptionSetValue(status);
            SetStateResponse response = (SetStateResponse)service.Execute(statusrequest);
        }

        private void _closeIncident(Guid CaseId, IOrganizationService service)
        {
            Entity _caseresolution = new Entity("incidentresolution");
            _caseresolution.Attributes.Add("incidentid", new EntityReference("incident", CaseId));
            _caseresolution.Attributes.Add("subject", "Problemet löst.");

            CloseIncidentRequest _closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = _caseresolution,
                //RequestName = "CloseIncident",
                Status = new OptionSetValue(5)
            };
            CloseIncidentResponse _closeresponse = (CloseIncidentResponse)service.Execute(_closerequest);

        }

        private void _CreateAnnotation(string subject, string notetext, EntityReference reference, IOrganizationService service)
        {
            try
            {
                Entity annotation = new Entity("annotation");
                annotation["objectid"] = reference;
                //                annotation["objecttypecode"] = reference.LogicalName;
                annotation["subject"] = subject;
                annotation["notetext"] = notetext;
                service.Create(annotation);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(string.Format("subject={0}, notetext={1}, reference.Id={2}, reference.LogicalName = {3} Message: {4}", subject, notetext, reference.Id, reference.LogicalName, ex.Message));
            }
        }

        #region Create Coupon
        private void CreateCouponSMS(decimal amount, Guid CustomerId, string FullName, string MobilePhone, string CampaignCode, EntityReference RefundER, string CreateCouponServiceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                CreateSMSCouponService.CreateSMSCouponRequest request = new CreateSMSCouponService.CreateSMSCouponRequest();
                request.Amount = amount;
                request.AmountSpecified = true;
                request.CampaignCode = CampaignCode;
                request.Currency = "SEK";
                request.CustomerId = CustomerId.ToString();
                //request.DistributionType = 1; Field not included in request?
                request.Email = FullName; //Redmine #2437:
                request.MobilePhone = MobilePhone;

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(CreateCouponServiceUrl);

                CreateSMSCouponService.CreateSMSCouponClient client = new CreateSMSCouponService.CreateSMSCouponClient(myBinding, endPointAddress);
                CreateSMSCouponService.CreateSMSCouponResponse respons = client.CreateSMSCoupon(request);
                string Message = respons.Message;
                string ErrorMessage = respons.ErrorMessage;
                uint StatusCode = respons.StatusCode;
                bool StatusCodeSpecified = respons.StatusCodeSpecified;
                bool OrderCreated = respons.OrderCreated;
                bool OrderCreatedSpecified = respons.OrderCreatedSpecified;

                EHOrderNumber = respons.OrderNumber;

                if (IsAutomatic)
                    _CreateAnnotation("CreateCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, StatusCode, StatusCodeSpecified, EHOrderNumber), RefundER, service);
                if (!OrderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av SMS-kupong." + ErrorMessage);
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapande av SMS-kupong.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateCouponSMS, amount = {0}, CustomerId = {1}, FullName={2}, MobilePhone={3}, CampaignCode={4}, Exception: {5}", amount.ToString(), CustomerId.ToString(), FullName, MobilePhone, CampaignCode, ex.ToString())));
            }
        }

        private void CreateEmailCoupon(decimal amount, string _CardSerialNumber, Guid CustomerId, string FullName, string EmailAddress, string CampaignCode, EntityReference RefundER, string CreateCouponEmailServiceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                CreateEmailCouponService.CreateEmailCouponRequest request = new CreateEmailCouponService.CreateEmailCouponRequest();
                request.Amount = amount;
                request.AmountSpecified = true;
                request.CampaignCode = CampaignCode;
                request.Currency = "SEK";
                request.CustomerId = CustomerId.ToString();
                request.Email = EmailAddress;

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(CreateCouponEmailServiceUrl);

                CreateEmailCouponService.CreateEmailCouponClient client = new CreateEmailCouponService.CreateEmailCouponClient(myBinding, endPointAddress);
                CreateEmailCouponService.CreateEmailCouponResponse respons = client.CreateEmailCoupon(request);
                string Message = respons.Message;
                string ErrorMessage = respons.ErrorMessage;
                uint StatusCode = respons.StatusCode;
                bool StatusCodeSpecified = respons.StatusCodeSpecified;
                bool OrderCreated = respons.OrderCreated;
                bool OrderCreatedSpecified = respons.OrderCreatedSpecified;

                EHOrderNumber = respons.OrderNumber;

                if (IsAutomatic)
                    _CreateAnnotation("CreateEmailCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, StatusCode, StatusCodeSpecified, EHOrderNumber), RefundER, service);
                if (!OrderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av Email-kupong." + ErrorMessage);
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapande av Email-kupong.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateEmailCoupon, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, FullName={3}, EmailAddress={4} Exception: {5}", amount.ToString(), _CardSerialNumber, CustomerId.ToString(), FullName, EmailAddress, ex.ToString())));
            }
        }
        #endregion

        #region Create giftcard
        private void CreateGiftcard(EntityReference CustomerId, string EmailAddress, DateTime cgi_last_valid, decimal cgi_amount, EntityReference RefundER, string CreateGiftcardServiceUrl, string SendValueCodeMailServiceUrl, ref string ValueCode, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                using (CreateGiftcardService.BizTalkServiceInstance createvaluecodeclient = new CreateGiftcardService.BizTalkServiceInstance())
                {
                    createvaluecodeclient.Url = CreateGiftcardServiceUrl;

                    CreateGiftCardRequest req = new CreateGiftCardRequest();

                    req.CustomerId = null;

                    req.CampaignTrackingCode = "";
                    req.Currency = "SEK";

                    req.Sum = cgi_amount;
                    req.SumSpecified = true;

                    req.ValidTo = cgi_last_valid;
                    req.ValidToSpecified = true;
                    CreateGiftCardResponse res = createvaluecodeclient.GreateGiftCard(req);


                    string CampaignTrackingCode = res.CampaignTrackingCode;
                    ValueCode = res.Code;
                    string Currency = res.Currency;
                    string _CustomerId = res.CustomerId;
                    decimal Sum = res.Sum;
                    bool SumSpecified = res.SumSpecified;
                    DateTime ValidTo = res.ValidTo;
                    bool ValidToSpecified = res.ValidToSpecified;

                    if (IsAutomatic)
                        _CreateAnnotation("CreateGiftCardResponse:", string.Format("CampaignTrackingCode={0}, Code={1}, Currency={2}, CustomerId={3}, Sum={4}, SumSpecified={5}, ValidTo={6}, ValidToSpecified={7}"
                        , CampaignTrackingCode, ValueCode, Currency, _CustomerId, Sum, SumSpecified, ValidTo, ValidToSpecified), RefundER, service);

                    if (string.IsNullOrEmpty(res.Code))
                        throw new UserMessageException("Ett fel uppstod vid skapandet eller utskicket av värdekod via mail: CreateGiftCard tjänsten returerade ingen kod, försök igen!");

                    //skicka värdekod med epost
                    SendValueCodeMailService.SendValueCodeMailRequest request = new SendValueCodeMailService.SendValueCodeMailRequest();

                    request.Email = EmailAddress;
                    request.ValueCode = res.Code;
                    request.CustomerId = CustomerId.Id.ToString();
                    if (IsAutomatic)
                        _CreateAnnotation("SendValueCodeMailRequest:", string.Format("Email={0}, ValueCode={1}, Currency={2}", request.Email, request.ValueCode, request.CustomerId), RefundER, service);

                    BasicHttpBinding myBinding = new BasicHttpBinding();
                    myBinding.Name = "myBasicHttpBinding";
                    EndpointAddress endPointAddress = new EndpointAddress(SendValueCodeMailServiceUrl);
                    SendValueCodeMailService.SendValueCodeMailClient sendvaluecodeclient = new SendValueCodeMailService.SendValueCodeMailClient(myBinding, endPointAddress);

                    SendValueCodeMailService.SendValueCodeMailResponse response = sendvaluecodeclient.SendValueCodeMail(request);
                    if (response == null)
                    {
                        if (IsAutomatic)
                            _CreateAnnotation("Service returned null", "SendValueCodeMailResponse is null", RefundER, service);
                        else
                            throw new InvalidPluginExecutionException("SendValueCodeMail returned null. SendValueCodeMailResponse is null.");
                    }


                    string Message = response.Message;
                    string ErrorMessage = response.ErrorMessage;
                    int StatusCode = response.StatusCode;
                    bool StatusCodeSpecified = response.StatusCodeSpecified;
                    bool Success = response.Success;
                    bool SuccessSpecified = response.SuccessSpecified;


                    if (IsAutomatic)
                        _CreateAnnotation("SendValueCodeMailResponse:", string.Format("ErrorMessage={0}, Message={1}, Success={2}, SuccessSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}", ErrorMessage, Message, Success, SuccessSpecified, StatusCode, StatusCodeSpecified), RefundER, service);
                    if (!Success)
                        throw new UserMessageException("Ett fel uppstod vid utskick av värdekod via mail: " + ErrorMessage);

                }
            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid skapandet eller utskicket av värdekod via mail.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }

                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateGiftcard/SendValueCodeMail, CustomerId = {0}, EmailAddress={1}, cgi_last_valid = {2}, cgi_amount={3}. Exception: {4}", CustomerId.Id.ToString(), EmailAddress, cgi_last_valid.ToString(), cgi_amount.ToString(), ex.ToString())));
            }
        }

        #endregion

        #region Load card
        private void LoadCard(decimal amount, string _CardSerialNumber, Guid CustomerId, string EmailAddress, EntityReference RefundER, string serviceUrl, ref string EHOrderNumber, IOrganizationService service, bool IsAutomatic)
        {
            try
            {
                ChargeOrderService.CreateChargeOrderRequest ccorequest = new ChargeOrderService.CreateChargeOrderRequest();
                ccorequest.Amount = amount;
                ccorequest.AmountSpecified = true;
                ccorequest.CardNumber = _CardSerialNumber;
                ccorequest.Currency = "SEK";
                ccorequest.CustomerId = CustomerId.ToString();
                ccorequest.Email = EmailAddress;

                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "myBasicHttpBinding";
                EndpointAddress endPointAddress = new EndpointAddress(serviceUrl);
                ChargeOrderService.ChargeOrderClient client = new ChargeOrderService.ChargeOrderClient(myBinding, endPointAddress);
                ChargeOrderService.CreateChargeOrderResponse ccoresponse = client.ChargeOrder(ccorequest);

                string ErrorMessage = ccoresponse.ErrorMessage;
                string Message = ccoresponse.Message;
                bool OrderCreated = ccoresponse.OrderCreated;
                bool OrderCreatedSpecified = ccoresponse.OrderCreatedSpecified;
                EHOrderNumber = ccoresponse.OrderNumber;
                int StatusCode = ccoresponse.StatusCode;
                bool StatusCodeSpecified = ccoresponse.StatusCodeSpecified;
                if (IsAutomatic)
                    _CreateAnnotation("CreateChargeOrderResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, EHOrderNumber={4}, StatusCode={5}, StatusCodeSpecified={6}", ErrorMessage, Message, OrderCreated, OrderCreatedSpecified, EHOrderNumber, StatusCode, StatusCodeSpecified), RefundER, service);
                if (!OrderCreated)
                    throw new UserMessageException("Ett fel uppstod vid laddning av kort: " + ErrorMessage);

            }
            catch (Exception ex)
            {
                string UserMessage = "Ett fel uppstod vid laddning av kort.";
                if (ex is UserMessageException)
                {
                    UserMessage = ex.Message;
                }
                throw new UserMessageException(UserMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i LoadCard, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, EmailAddress={3}. Exception: {4}", amount, _CardSerialNumber, CustomerId, EmailAddress, ex.ToString())));
            }
        }

        #endregion

        #region Common
        private void GetSettings(IOrganizationService service, out string cgi_chargeorderservice, out  string cgi_createcouponservice, out  string cgi_createemailcouponservice, out string cgi_sendvaluecodemailservice, out string cgi_giftcardservice, out EntityReference cgi_defaultcustomeroncase, out int cgi_valuecodevalidformonths)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            _xml += "       <attribute name='cgi_cubicservice' />";
            _xml += "       <attribute name='cgi_giftcardservice' />";
            _xml += "       <attribute name='cgi_chargeorderservice' />";
            _xml += "       <attribute name='cgi_createcouponservice' />";
            _xml += "       <attribute name='cgi_createemailcouponservice' />";
            _xml += "       <attribute name='cgi_getoutstandingchargesservice' />";
            _xml += "       <attribute name='cgi_rechargecardservice' />";
            _xml += "       <attribute name='cgi_sendvaluecodemailservice' />";
            _xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            _xml += "       <attribute name='cgi_raindanceprefix' />";
            _xml += "       <attribute name='cgi_valuecodevalidformonths' />";
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";


            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_chargeorderservice") && settings["cgi_chargeorderservice"] != null)
                cgi_chargeorderservice = settings.GetAttributeValue<string>("cgi_chargeorderservice");
            else
                throw new Exception("Required setting is missing: cgi_chargeorderservice");

            if (settings.Contains("cgi_createcouponservice") && settings["cgi_createcouponservice"] != null)
                cgi_createcouponservice = settings.GetAttributeValue<string>("cgi_createcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createcouponservice");

            if (settings.Contains("cgi_createemailcouponservice") && settings["cgi_createemailcouponservice"] != null)
                cgi_createemailcouponservice = settings.GetAttributeValue<string>("cgi_createemailcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createemailcouponservice");

            if (settings.Contains("cgi_sendvaluecodemailservice") && settings["cgi_sendvaluecodemailservice"] != null)
                cgi_sendvaluecodemailservice = settings.GetAttributeValue<string>("cgi_sendvaluecodemailservice");
            else
                throw new Exception("Required setting is missing: cgi_sendvaluecodemailservice");

            if (settings.Contains("cgi_giftcardservice") && settings["cgi_giftcardservice"] != null)
                cgi_giftcardservice = settings.GetAttributeValue<string>("cgi_giftcardservice");
            else
                throw new Exception("Required setting is missing: cgi_giftcardservice");

            if (settings.Contains("cgi_defaultcustomeroncase") && settings["cgi_defaultcustomeroncase"] != null)
                cgi_defaultcustomeroncase = settings.GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
            else
                throw new Exception("Required setting is missing: cgi_defaultcustomeroncase");

            if (settings.Contains("cgi_valuecodevalidformonths") && settings["cgi_valuecodevalidformonths"] != null)
                cgi_valuecodevalidformonths = settings.GetAttributeValue<int>("cgi_valuecodevalidformonths");
            else
                throw new Exception("Required setting is missing: cgi_valuecodevalidformonths");
        }
        #endregion
        #endregion
        #endregion
    }
}