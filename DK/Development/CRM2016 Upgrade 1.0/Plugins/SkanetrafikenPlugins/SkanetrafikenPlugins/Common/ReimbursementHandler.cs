using System;
using System.Linq;
using System.ServiceModel;
using CRM2013.SkanetrafikenPlugins.CreateGiftcardService;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins.Common
{
    public class ReimbursementHandler
    {
        #region Public Methods
        public void ExecuteRefundAndUpdatesStatus(Guid refundId, IOrganizationService service)
        {
            Entity refund = service.Retrieve("cgi_refund", refundId, new ColumnSet(
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
                "cgi_last_valid",
                "cgi_email"
                ));

            int state = ((OptionSetValue)refund.Attributes["statecode"]).Value;
            int status = ((OptionSetValue)refund.Attributes["statuscode"]).Value;

            //Genomför eventuell utbetalning och sätt status på beslutet [Refund].
            //State value: 0 = active, corresponding Status values: 1 = New, 285050003 = Approved - Transaction Pending, 285050004 = Approved -Transaction failed, 285050005 = Approved
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved

            if ((state == 0 && status == 285050005) || (state == 0 && status == 285050004))
            {
                //285050003 = Approved - Transaction Pending
                //285050004 = Approved -Transaction failed
                //try to execute the Refund
                ReimbursementHandler rh = new ReimbursementHandler();
                if (rh.ExecuteRefundTransaction(refund, true, service))
                {
                    //close the incident if refund is executed

                    _SetRefundDescisionStatus(refundId, 0, 285050005, service);//Active - Approved
                    if (refund.Attributes.Contains("cgi_caseid") && refund.Attributes["cgi_caseid"] != null)
                    {
                        _closeIncident(((EntityReference)refund.Attributes["cgi_caseid"]).Id, service);
                    }
                }
                else
                {
                    _SetRefundDescisionStatus(refundId, 0, 285050004, service);//Active - Approved -Transaction failed
                    //leave incident open
                }
            }
        }

        public bool ExecuteRefundTransaction(Entity refund, bool isAutomatic, IOrganizationService service)
        {
            try
            {
                string ehOrderNumber = "";
                string valueCode = "";

                //attempt to execute the refund

                if (!refund.Contains("cgi_reimbursementformid"))
                {
                    throw new UserMessageException("Kunde inte behandla utbetalning eftersom utbetalningsform saknas.");
                }

                EntityReference rfId = refund.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                Entity reimbursementForm = service.Retrieve(rfId.LogicalName, rfId.Id, new ColumnSet("cgi_loadcard", "cgi_giftcard", "cgi_couponsms", "cgi_couponemail", "cgi_payment", "cgi_payment_abroad", "cgi_print", "ed_clearonvaluecode"));



                //Avbryt om ingen automatisk utbetalning ska exekveras
                if (!(reimbursementForm.RefundedViaLoadCard()
                    || reimbursementForm.RefundedViaGiftCard()
                    || reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaEmail()))
                {
                    //Ingen utbetalning ska exekveras

                    //Returnera vid förutsedda reimbursementForms
                    if (reimbursementForm.RefundedViaPayment()) { return true; }
                    else if (reimbursementForm.RefundedViaPaymentAbroad()) { return true; }
                    else if (reimbursementForm.RefundedViaPrint()) { return true; }


                    if (isAutomatic)
                    {
                        //returnera fel vid automatisk körning vid oförutsedd utbetalningsform
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms, cgi_couponemail, cgi_payment, cgi_payment_abroad, cgi_print");
                    }
                    //vid manuell registrering - returnera vid oförutsedd utbetalningsform
                    return true;
                }
                

                EntityReference incidentReference = refund.GetAttributeValue<EntityReference>("cgi_caseid");
                Entity incident = service.Retrieve(incidentReference.LogicalName, incidentReference.Id, new ColumnSet("customerid", "cgi_customer_email"));

                EntityReference customerId;
                if (incident.Contains("customerid"))
                    customerId = incident.GetAttributeValue<EntityReference>("customerid");
                else
                    throw new UserMessageException("Ärendet saknar kund!");

                string Incident_cgi_customer_email = null;
                if (incident.Contains("cgi_customer_email"))
                {
                    Incident_cgi_customer_email = incident.GetAttributeValue<string>("cgi_customer_email");
                }

                string refund_cgi_email = null;
                if (refund.Contains("cgi_email"))
                {
                    refund_cgi_email = refund.GetAttributeValue<string>("cgi_email");
                }

                string emailAddress = refund_cgi_email != null ? refund_cgi_email : Incident_cgi_customer_email;//använd i första hand epostadressen från beslutet

                bool IsEmailAdressIdenticalToCustomerEmailAddress = Incident_cgi_customer_email == emailAddress;

                if (emailAddress == null && !(reimbursementForm.RefundedViaSms()
                    || reimbursementForm.RefundedViaLoadCard())) //If the code is sent via SMS, we don't need an email address.
                    throw new UserMessageException("Ärendet saknar epostadress!");

                string cardSerialNumber = refund.Contains("cgi_travelcard_number") ? refund.GetAttributeValue<string>("cgi_travelcard_number") : null;
                decimal amount = refund.Contains("cgi_amount") ? refund.GetAttributeValue<Money>("cgi_amount").Value : 0;

                // temporary fix /Daniel Endeavor
                //if (refund.Contains("cgi_last_valid"))
                //{
                //    DateTime validTo = refund.GetAttributeValue<DateTime>("cgi_last_valid");
                //    //if (DateTime.Now.AddMonths(5).AddDays(26) < validTo && DateTime.Now.AddMonths(6).AddDays(1) > validTo) {
                //    //    refund["cgi_last_valid"] = validTo.AddMonths(6);

                //    //}
                //}

                if (refund.Contains("cgi_last_valid"))
                {
                    var validTo = refund.GetAttributeValue<DateTime>("cgi_last_valid");
                    refund["cgi_last_valid"] = validTo;
                }

                string settingCgiChargeorderservice;
                string settingCgiCreatecouponservice;
                string settingCgiCreateemailcouponservice;
                string settingCgiSendvaluecodemailservice;
                string settingCgiGiftcardservice;
                EntityReference settingCgiDefaultcustomeroncase;
                int? cgiValuecodevalidformonths;
                DateTime? edValuecodetodate;

                Ed_GetSettings(service, out settingCgiChargeorderservice, out settingCgiCreatecouponservice, out settingCgiCreateemailcouponservice, out settingCgiSendvaluecodemailservice, out settingCgiGiftcardservice, out settingCgiDefaultcustomeroncase, out cgiValuecodevalidformonths, out edValuecodetodate);
                //GetSettings(service, out settingCgiChargeorderservice, out settingCgiCreatecouponservice, out settingCgiCreateemailcouponservice, out settingCgiSendvaluecodemailservice, out settingCgiGiftcardservice, out settingCgiDefaultcustomeroncase, out cgiValuecodevalidformonths);

                EntityReference erRefund = new EntityReference(refund.LogicalName, refund.Id);

                if (reimbursementForm.RefundedViaLoadCard())
                {
                    if (amount == 0 || cardSerialNumber == null)
                        return true;

                    //LoadCard(amount, cardSerialNumber, customerId.Id, emailAddress, erRefund, settingCgiChargeorderservice, ref ehOrderNumber, service, isAutomatic);
                }
                else if (reimbursementForm.RefundedViaGiftCard())
                {
                    if (settingCgiDefaultcustomeroncase.Id.Equals(customerId.Id))
                        throw new UserMessageException("Kunde inte skapa värdekod för anonym kund!");


                    DateTime lastValid = new DateTime();
                    if (refund.Contains("cgi_last_valid") && refund["cgi_last_valid"] != null)
                        lastValid = refund.GetAttributeValue<DateTime>("cgi_last_valid");

                    //DateTime cgiLastValid = DateTime.Now.AddMonths(cgiValuecodevalidformonths);
                    //if (refund.Contains("cgi_last_valid") && refund["cgi_last_valid"] != null)
                    //    cgiLastValid = refund.GetAttributeValue<DateTime>("cgi_last_valid");


                    //CreateGiftcard(customerId, emailAddress, IsEmailAdressIdenticalToCustomerEmailAddress, lastValid, amount, erRefund, settingCgiGiftcardservice, settingCgiSendvaluecodemailservice, ref valueCode, service, isAutomatic);
                }
                else if (reimbursementForm.RefundedViaSms() && reimbursementForm.RefundedViaClearon() == false) // Marcus Stenswed - Only send via SMS this way when not via Clearon
                {
                    string cgiMobilenumber;
                    if (refund.Contains("cgi_mobilenumber"))
                        cgiMobilenumber = refund.GetAttributeValue<string>("cgi_mobilenumber");
                    else
                        throw new UserMessageException("Ärendet saknar telefonnummer!");

                    string smstext = String.IsNullOrEmpty(emailAddress) ? "," : emailAddress; //Cannot send an empty string as argument to the service, see RedMine #2437

                    //CreateCouponSMS(amount, customerId.Id, smstext, cgiMobilenumber, "0", erRefund, settingCgiCreatecouponservice, ref ehOrderNumber, service, isAutomatic);
                }
                else if (reimbursementForm.RefundedViaEmail() && reimbursementForm.RefundedViaClearon() == false) // Marcus Stenswed - Only send via Email this way when not via Clearon
                {
                    //CreateEmailCoupon(amount, cardSerialNumber, customerId.Id, emailAddress, emailAddress, "0", erRefund, settingCgiCreateemailcouponservice, ref ehOrderNumber, service, isAutomatic);
                }
                else if ((reimbursementForm.RefundedViaSms() || reimbursementForm.RefundedViaEmail()) && reimbursementForm.RefundedViaClearon() == true)
                {
                    // Marcus Stenswed - 2018-12-13
                    // Do nothing. ValueCode is created and sent to customer from Workflow trough Clearon
                }
                else
                {
                    if (isAutomatic)
                        throw new UserMessageException("Utbetalningsform kunde inte avgöras baserat på vald utbetalningsform. Tillåtna värden är cgi_loadcard, cgi_giftcard, cgi_couponsms, cgi_couponemail");
                }

                refund.Attributes["cgi_ehordernumber"] = ehOrderNumber;
                refund.Attributes["cgi_value_code"] = valueCode;

                if (isAutomatic)
                    service.Update(refund); //not needed when called from pre-executing plugin

                return true;
            }
            catch (Exception ex)
            {
                if (isAutomatic)
                {
                    try
                    {
                        if (ex is UserMessageException)
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction",
                                "User messge:" + ex.Message + " | Original error:" +
                                (ex.InnerException != null ? ex.InnerException.ToString() : ""),
                                new EntityReference(refund.LogicalName, refund.Id), service);
                            refund.Attributes["cgi_errormessage"] = ex.Message;
                        }
                        else
                        {
                            _CreateAnnotation("Error in ExecuteRefundTransaction", ex.ToString(),
                                new EntityReference(refund.LogicalName, refund.Id), service);
                            refund.Attributes["cgi_errormessage"] =
                                "Ett oförutsett fel inträffade. Se återbetalningens anteckningar för teknisk information.";
                        }
                        service.Update(refund);
                    }
                    catch (Exception innerEx)
                    {
                        throw new InvalidPluginExecutionException(innerEx.Message);
                    }
                    return false;
                }
                if (ex is UserMessageException)
                {
                    throw new InvalidPluginExecutionException(ex.Message, ex);
                }
                //throw new InvalidPluginExecutionException("Ett oförutsett fel inträffade. Försök igen eller välj en annan återbetalningsform.", ex);
                throw new InvalidPluginExecutionException("Ett oförutsett fel inträffade. Försök igen eller välj en annan återbetalningsform. " + ex);
            }
        }
        #endregion

        #region Private Methods
        private void _SetRefundDescisionStatus(Guid refundId, int state, int status, IOrganizationService service)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed

            SetStateRequest statusrequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference("cgi_refund", refundId),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };
            service.Execute(request: statusrequest);
        }

        private void _closeIncident(Guid caseId, IOrganizationService service)
        {
            _SetCaseReopen(caseId, service);

            Entity caseresolution = new Entity("incidentresolution");
            caseresolution.Attributes.Add("incidentid", new EntityReference("incident", caseId));
            caseresolution.Attributes.Add("subject", "Problemet löst.");

            CloseIncidentRequest closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = caseresolution,
                Status = new OptionSetValue(5)
            };
            service.Execute(closerequest);
        }
        /// <summary>
        /// Sätter cgi_case_reopen till 1
        /// </summary>
        /// <param name="IncidentId"></param>
        private void _SetCaseReopen(Guid IncidentId, IOrganizationService service)
        {
            Entity incidentEntity = new Entity("incident");
            incidentEntity.Id = IncidentId;
            incidentEntity["cgi_case_reopen"] = "1";
            service.Update(incidentEntity);
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
        private void CreateCouponSMS(decimal amount, Guid customerId, string fullName, string mobilePhone, string campaignCode, EntityReference refundEr, string createCouponServiceUrl, ref string ehOrderNumber, IOrganizationService service, bool isAutomatic)
        {
            try
            {
                CreateSMSCouponService.CreateSMSCouponRequest request =
                    new CreateSMSCouponService.CreateSMSCouponRequest
                    {
                        Amount = amount,
                        AmountSpecified = true,
                        CampaignCode = campaignCode,
                        Currency = "SEK",
                        CustomerId = customerId.ToString(),
                        Email = fullName, //Redmine #2437:
                        MobilePhone = mobilePhone
                    };


                BasicHttpBinding myBinding = new BasicHttpBinding
                {
                    Name = "myBasicHttpBinding"
                };
                EndpointAddress endPointAddress = new EndpointAddress(createCouponServiceUrl);

                CreateSMSCouponService.CreateSMSCouponClient client = new CreateSMSCouponService.CreateSMSCouponClient(myBinding, endPointAddress);
                CreateSMSCouponService.CreateSMSCouponResponse respons = client.CreateSMSCoupon(request);
                string message = respons.Message;
                string errorMessage = respons.ErrorMessage;
                uint statusCode = respons.StatusCode;
                bool statusCodeSpecified = respons.StatusCodeSpecified;
                bool orderCreated = respons.OrderCreated;
                bool orderCreatedSpecified = respons.OrderCreatedSpecified;

                ehOrderNumber = respons.OrderNumber;

                if (isAutomatic)
                    _CreateAnnotation("CreateCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", errorMessage, message, orderCreated, orderCreatedSpecified, statusCode, statusCodeSpecified, ehOrderNumber), refundEr, service);
                if (!orderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av SMS-kupong." + errorMessage);
            }
            catch (Exception ex)
            {
                string userMessage = "Ett fel uppstod vid skapande av SMS-kupong.";
                if (ex is UserMessageException)
                {
                    userMessage = ex.Message;
                }
                throw new UserMessageException(userMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateCouponSMS, amount = {0}, CustomerId = {1}, FullName={2}, MobilePhone={3}, CampaignCode={4}, Exception: {5}", amount, customerId, fullName, mobilePhone, campaignCode, ex)));
            }
        }

        private void CreateEmailCoupon(decimal amount, string cardSerialNumber, Guid customerId, string fullName, string emailAddress, string campaignCode, EntityReference refundEr, string createCouponEmailServiceUrl, ref string ehOrderNumber, IOrganizationService service, bool isAutomatic)
        {
            try
            {
                CreateEmailCouponService.CreateEmailCouponRequest request =
                    new CreateEmailCouponService.CreateEmailCouponRequest
                    {
                        Amount = amount,
                        AmountSpecified = true,
                        CampaignCode = campaignCode,
                        Currency = "SEK",
                        CustomerId = customerId.ToString(),
                        Email = emailAddress
                    };

                BasicHttpBinding myBinding = new BasicHttpBinding
                {
                    Name = "myBasicHttpBinding"
                };
                EndpointAddress endPointAddress = new EndpointAddress(createCouponEmailServiceUrl);

                CreateEmailCouponService.CreateEmailCouponClient client = new CreateEmailCouponService.CreateEmailCouponClient(myBinding, endPointAddress);
                CreateEmailCouponService.CreateEmailCouponResponse respons = client.CreateEmailCoupon(request);
                string message = respons.Message;
                string errorMessage = respons.ErrorMessage;
                uint statusCode = respons.StatusCode;
                bool statusCodeSpecified = respons.StatusCodeSpecified;
                bool orderCreated = respons.OrderCreated;
                bool orderCreatedSpecified = respons.OrderCreatedSpecified;

                ehOrderNumber = respons.OrderNumber;

                if (isAutomatic)
                    _CreateAnnotation("CreateEmailCouponResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}, EHOrderNumber={6}", errorMessage, message, orderCreated, orderCreatedSpecified, statusCode, statusCodeSpecified, ehOrderNumber), refundEr, service);
                if (!orderCreated)
                    throw new InvalidPluginExecutionException("Ett fel uppstod vid skapande av Email-kupong." + errorMessage);
            }
            catch (Exception ex)
            {
                string userMessage = "Ett fel uppstod vid skapande av Email-kupong.";
                if (ex is UserMessageException)
                {
                    userMessage = ex.Message;
                }
                throw new UserMessageException(userMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateEmailCoupon, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, FullName={3}, EmailAddress={4} Exception: {5}", amount, cardSerialNumber, customerId, fullName, emailAddress, ex)));
            }
        }
        #endregion

        #region Create giftcard
        private void CreateGiftcard(EntityReference customerId, string emailAddress, bool IsEmailAdressIdenticalToCustomerEmailAddress, DateTime cgiLastValid, decimal cgiAmount, EntityReference refundEr, string createGiftcardServiceUrl, string sendValueCodeMailServiceUrl, ref string valueCode, IOrganizationService service, bool isAutomatic)
        {
            try
            {
                using (BizTalkServiceInstance createvaluecodeclient = new BizTalkServiceInstance())
                {
                    createvaluecodeclient.Url = createGiftcardServiceUrl;

                    CreateGiftCardRequest req = new CreateGiftCardRequest
                    {
                        CustomerId = null,
                        CampaignTrackingCode = "",
                        Currency = "SEK",
                        Sum = cgiAmount,
                        SumSpecified = true,
                        ValidTo = cgiLastValid,
                        ValidToSpecified = true
                    };

                    CreateGiftCardResponse res = createvaluecodeclient.GreateGiftCard(req);


                    string campaignTrackingCode = res.CampaignTrackingCode;
                    valueCode = res.Code;
                    string currency = res.Currency;
                    string customerIdStr = res.CustomerId;
                    if (customerIdStr == null) throw new ArgumentNullException("customerId");
                    decimal sum = res.Sum;
                    bool sumSpecified = res.SumSpecified;
                    DateTime validTo = res.ValidTo;
                    bool validToSpecified = res.ValidToSpecified;

                    if (isAutomatic)
                        _CreateAnnotation("CreateGiftCardResponse:", string.Format("CampaignTrackingCode={0}, Code={1}, Currency={2}, CustomerId={3}, Sum={4}, SumSpecified={5}, ValidTo={6}, ValidToSpecified={7}"
                        , campaignTrackingCode, valueCode, currency, customerIdStr, sum, sumSpecified, validTo, validToSpecified), refundEr, service);

                    if (string.IsNullOrEmpty(res.Code))
                        throw new UserMessageException("Ett fel uppstod vid skapandet eller utskicket av värdekod via mail: CreateGiftCard tjänsten returerade ingen kod, försök igen!");

                    //skicka värdekod med epost
                    SendValueCodeMailService.SendValueCodeMailRequest request =
                        new SendValueCodeMailService.SendValueCodeMailRequest
                        {
                            Email = emailAddress,
                            ValueCode = res.Code,
                            CustomerId = IsEmailAdressIdenticalToCustomerEmailAddress ? customerId.Id.ToString() : null //avoid sending customerid if email address does not match the customeremail, 
                        };

                    if (isAutomatic)
                        _CreateAnnotation("SendValueCodeMailRequest:", string.Format("Email={0}, ValueCode={1}, CustomerId={2}", request.Email, request.ValueCode, request.CustomerId), refundEr, service);

                    BasicHttpBinding myBinding = new BasicHttpBinding
                    {
                        Name = "myBasicHttpBinding"
                    };
                    EndpointAddress endPointAddress = new EndpointAddress(sendValueCodeMailServiceUrl);
                    SendValueCodeMailService.SendValueCodeMailClient sendvaluecodeclient = new SendValueCodeMailService.SendValueCodeMailClient(myBinding, endPointAddress);

                    SendValueCodeMailService.SendValueCodeMailResponse response = sendvaluecodeclient.SendValueCodeMail(request);
                    if (response == null)
                    {
                        if (isAutomatic)
                            _CreateAnnotation("Service returned null", "SendValueCodeMailResponse is null", refundEr, service);
                        else
                            throw new InvalidPluginExecutionException("SendValueCodeMail returned null. SendValueCodeMailResponse is null.");
                    }


                    if (response != null)
                    {
                        string message = response.Message;
                        string errorMessage = response.ErrorMessage;
                        int statusCode = response.StatusCode;
                        bool statusCodeSpecified = response.StatusCodeSpecified;
                        bool success = response.Success;
                        bool successSpecified = response.SuccessSpecified;


                        if (isAutomatic)
                            _CreateAnnotation("SendValueCodeMailResponse:", string.Format("ErrorMessage={0}, Message={1}, Success={2}, SuccessSpecified={3}, StatusCode={4}, StatusCodeSpecified={5}", errorMessage, message, success, successSpecified, statusCode, statusCodeSpecified), refundEr, service);
                        if (!success)
                            throw new UserMessageException("Ett fel uppstod vid utskick av värdekod via mail: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                string userMessage = "Ett fel uppstod vid skapandet eller utskicket av värdekod via mail.";
                if (ex is UserMessageException)
                {
                    userMessage = ex.Message;
                }

                throw new UserMessageException(userMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i CreateGiftcard/SendValueCodeMail, CustomerId = {0}, EmailAddress={1}, cgi_last_valid = {2}, cgi_amount={3}. Exception: {4}", customerId.Id, emailAddress, cgiLastValid, cgiAmount, ex)));
            }
        }

        #endregion

        #region Load card
        private void LoadCard(decimal amount, string cardSerialNumber, Guid customerId, string emailAddress, EntityReference refundEr, string serviceUrl, ref string ehOrderNumber, IOrganizationService service, bool isAutomatic)
        {
            try
            {
                ChargeOrderService.CreateChargeOrderRequest ccorequest =
                    new ChargeOrderService.CreateChargeOrderRequest
                    {
                        Amount = amount,
                        AmountSpecified = true,
                        CardNumber = cardSerialNumber,
                        Currency = "SEK",
                        CustomerId = customerId.ToString(),
                        Email = emailAddress
                    };

                BasicHttpBinding myBinding = new BasicHttpBinding
                {
                    Name = "myBasicHttpBinding"
                };
                EndpointAddress endPointAddress = new EndpointAddress(serviceUrl);
                ChargeOrderService.ChargeOrderClient client = new ChargeOrderService.ChargeOrderClient(myBinding, endPointAddress);
                ChargeOrderService.CreateChargeOrderResponse ccoresponse = client.ChargeOrder(ccorequest);

                string errorMessage = ccoresponse.ErrorMessage;
                string message = ccoresponse.Message;
                bool orderCreated = ccoresponse.OrderCreated;
                bool orderCreatedSpecified = ccoresponse.OrderCreatedSpecified;
                ehOrderNumber = ccoresponse.OrderNumber;
                int statusCode = ccoresponse.StatusCode;
                bool statusCodeSpecified = ccoresponse.StatusCodeSpecified;
                if (isAutomatic)
                    _CreateAnnotation("CreateChargeOrderResponse:", string.Format("ErrorMessage={0}, Message={1}, OrderCreated={2}, OrderCreatedSpecified={3}, EHOrderNumber={4}, StatusCode={5}, StatusCodeSpecified={6}", errorMessage, message, orderCreated, orderCreatedSpecified, ehOrderNumber, statusCode, statusCodeSpecified), refundEr, service);
                if (!orderCreated)
                    throw new UserMessageException("Ett fel uppstod vid laddning av kort: " + errorMessage);

            }
            catch (Exception ex)
            {
                string userMessage = "Ett fel uppstod vid laddning av kort.";
                if (ex is UserMessageException)
                {
                    userMessage = ex.Message;
                }
                throw new UserMessageException(userMessage, new InvalidPluginExecutionException(string.Format("Ett oväntat fel uppstod i LoadCard, amount = {0}, _CardSerialNumber={1}, CustomerId = {2}, EmailAddress={3}. Exception: {4}", amount, cardSerialNumber, customerId, emailAddress, ex)));
            }
        }

        #endregion

        #region Common

        private void Ed_GetSettings(IOrganizationService service, out string cgiChargeorderservice, out string cgiCreatecouponservice, out string cgiCreateemailcouponservice, out string cgiSendvaluecodemailservice, out string cgiGiftcardservice, out EntityReference cgiDefaultcustomeroncase, out int? cgiValuecodevalidformonths, out DateTime? edValidForDate)
        {
      

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_settingid' />";
            xml += "       <attribute name='cgi_cubicservice' />";
            xml += "       <attribute name='cgi_giftcardservice' />";
            xml += "       <attribute name='cgi_chargeorderservice' />";
            xml += "       <attribute name='cgi_createcouponservice' />";
            xml += "       <attribute name='cgi_createemailcouponservice' />";
            xml += "       <attribute name='cgi_getoutstandingchargesservice' />";
            xml += "       <attribute name='cgi_rechargecardservice' />";
            xml += "       <attribute name='cgi_sendvaluecodemailservice' />";
            xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            xml += "       <attribute name='cgi_raindanceprefix' />";
            xml += "       <attribute name='cgi_valuecodevalidformonths' />";
            xml += "       <attribute name='ed_valuecodevaliddate' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";


            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_chargeorderservice") && settings["cgi_chargeorderservice"] != null)
                cgiChargeorderservice = settings.GetAttributeValue<string>("cgi_chargeorderservice");
            else
                throw new Exception("Required setting is missing: cgi_chargeorderservice");

            if (settings.Contains("cgi_createcouponservice") && settings["cgi_createcouponservice"] != null)
                cgiCreatecouponservice = settings.GetAttributeValue<string>("cgi_createcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createcouponservice");

            if (settings.Contains("cgi_createemailcouponservice") && settings["cgi_createemailcouponservice"] != null)
                cgiCreateemailcouponservice = settings.GetAttributeValue<string>("cgi_createemailcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createemailcouponservice");

            if (settings.Contains("cgi_sendvaluecodemailservice") && settings["cgi_sendvaluecodemailservice"] != null)
                cgiSendvaluecodemailservice = settings.GetAttributeValue<string>("cgi_sendvaluecodemailservice");
            else
                throw new Exception("Required setting is missing: cgi_sendvaluecodemailservice");

            if (settings.Contains("cgi_giftcardservice") && settings["cgi_giftcardservice"] != null)
                cgiGiftcardservice = settings.GetAttributeValue<string>("cgi_giftcardservice");
            else
                throw new Exception("Required setting is missing: cgi_giftcardservice");

            if (settings.Contains("cgi_defaultcustomeroncase") && settings["cgi_defaultcustomeroncase"] != null)
                cgiDefaultcustomeroncase = settings.GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
            else
                throw new Exception("Required setting is missing: cgi_defaultcustomeroncase");



            if (!settings.Contains("cgi_valuecodevalidformonths") && !settings.Contains("ed_valuecodevaliddate"))
            {
                if(settings["cgi_valuecodevalidformonths"] == null && settings["ed_valuecodevaliddate"] == null)
                    throw new Exception("'valueCodeValidForMonths' and 'valueCodeValidDate' cannot be empty at the same time. Go to settings to enter one of the fields.");
            }
            if (settings.Contains("cgi_valuecodevalidformonths") && !settings.Contains("ed_valuecodevaliddate"))
            {
                if (settings["cgi_valuecodevalidformonths"] != null)
                    cgiValuecodevalidformonths = settings.GetAttributeValue<int>("cgi_valuecodevalidformonths");
                else cgiValuecodevalidformonths = null;
            }
            else cgiValuecodevalidformonths = null;

            if (!settings.Contains("cgi_valuecodevalidformonths") && settings.Contains("ed_valuecodevaliddate"))
            {
                if (settings["ed_valuecodevaliddate"] != null)
                    edValidForDate = settings.GetAttributeValue<DateTime>("ed_valuecodevaliddate");
                else edValidForDate = null;
            }
            else edValidForDate = null;
                
            //if (settings.Contains("cgi_valuecodevalidformonths") && settings["cgi_valuecodevalidformonths"] != null)
            //    cgiValuecodevalidformonths = settings.GetAttributeValue<int>("cgi_valuecodevalidformonths");
            //else
            //    throw new Exception("Required setting is missing: cgi_valuecodevalidformonths");
        }

        private void GetSettings(IOrganizationService service, out string cgiChargeorderservice, out string cgiCreatecouponservice, out string cgiCreateemailcouponservice, out string cgiSendvaluecodemailservice, out string cgiGiftcardservice, out EntityReference cgiDefaultcustomeroncase, out int cgiValuecodevalidformonths)
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_settingid' />";
            xml += "       <attribute name='cgi_cubicservice' />";
            xml += "       <attribute name='cgi_giftcardservice' />";
            xml += "       <attribute name='cgi_chargeorderservice' />";
            xml += "       <attribute name='cgi_createcouponservice' />";
            xml += "       <attribute name='cgi_createemailcouponservice' />";
            xml += "       <attribute name='cgi_getoutstandingchargesservice' />";
            xml += "       <attribute name='cgi_rechargecardservice' />";
            xml += "       <attribute name='cgi_sendvaluecodemailservice' />";
            xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            xml += "       <attribute name='cgi_raindanceprefix' />";
            xml += "       <attribute name='cgi_valuecodevalidformonths' />";
            xml += "       <attribute name='ed_valuecodevaliddate' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";


            FetchExpression f = new FetchExpression(xml);
            EntityCollection settingscollection = service.RetrieveMultiple(f);

            Entity settings = settingscollection.Entities.First();

            if (settings.Contains("cgi_chargeorderservice") && settings["cgi_chargeorderservice"] != null)
                cgiChargeorderservice = settings.GetAttributeValue<string>("cgi_chargeorderservice");
            else
                throw new Exception("Required setting is missing: cgi_chargeorderservice");

            if (settings.Contains("cgi_createcouponservice") && settings["cgi_createcouponservice"] != null)
                cgiCreatecouponservice = settings.GetAttributeValue<string>("cgi_createcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createcouponservice");

            if (settings.Contains("cgi_createemailcouponservice") && settings["cgi_createemailcouponservice"] != null)
                cgiCreateemailcouponservice = settings.GetAttributeValue<string>("cgi_createemailcouponservice");
            else
                throw new Exception("Required setting is missing: cgi_createemailcouponservice");

            if (settings.Contains("cgi_sendvaluecodemailservice") && settings["cgi_sendvaluecodemailservice"] != null)
                cgiSendvaluecodemailservice = settings.GetAttributeValue<string>("cgi_sendvaluecodemailservice");
            else
                throw new Exception("Required setting is missing: cgi_sendvaluecodemailservice");

            if (settings.Contains("cgi_giftcardservice") && settings["cgi_giftcardservice"] != null)
                cgiGiftcardservice = settings.GetAttributeValue<string>("cgi_giftcardservice");
            else
                throw new Exception("Required setting is missing: cgi_giftcardservice");

            if (settings.Contains("cgi_defaultcustomeroncase") && settings["cgi_defaultcustomeroncase"] != null)
                cgiDefaultcustomeroncase = settings.GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
            else
                throw new Exception("Required setting is missing: cgi_defaultcustomeroncase");

            if (settings.Contains("cgi_valuecodevalidformonths") && settings["cgi_valuecodevalidformonths"] != null)
                cgiValuecodevalidformonths = settings.GetAttributeValue<int>("cgi_valuecodevalidformonths");
            else
                throw new Exception("Required setting is missing: cgi_valuecodevalidformonths");
        }
        #endregion
        #endregion
        #endregion
    }
}