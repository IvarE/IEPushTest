using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.ValueCodes
{
    public static class ValueCodeHandler
    {
        //private const string typeConst = "mobile";
        //private const string deliveryTypeConst = "api";
        public static Guid CreateMobileValueCodeGeneric(Plugin.LocalPluginContext localContext, int deliveryType, DateTime validTo, float amount, decimal periodPrice, string phoneNumber, int templateNumber, ContactEntity contact, ValueCodeApprovalEntity valueCodeApproval = null, ValueCodeTemplateEntity template = null, LeadEntity lead = null, RefundEntity refund = null, TravelCardEntity travelCard = null)
        {
            localContext.Trace($"(CreateMobileValueCodeGeneric) Entering mobile");
            return CreateValueCodeGeneric(localContext, deliveryType, validTo, amount, periodPrice, Generated.ed_valuecode_ed_typeoption.Mobile, valueCodeApproval, templateNumber, contact, phoneNumber, "", template, lead, refund, travelCard);
        }

        public static Guid CreateEmailValueCodeGeneric(Plugin.LocalPluginContext localContext, int deliveryType, DateTime validTo, float amount, decimal periodPrice, string email, int templateNumber, ContactEntity contact, ValueCodeApprovalEntity valueCodeApproval = null, ValueCodeTemplateEntity template = null, LeadEntity lead = null, RefundEntity refund = null, TravelCardEntity travelCard = null)
        {
            localContext.Trace($"(CreateEmailValueCodeGeneric) Entering email");
            return CreateValueCodeGeneric(localContext, deliveryType, validTo, amount, periodPrice, Generated.ed_valuecode_ed_typeoption.Email, valueCodeApproval, templateNumber, contact, "", email, template, lead, refund, travelCard);
        }

        public static ValueCodeTemplateEntity GetValueCodeTemplate(Plugin.LocalPluginContext localContext, ValueCodeTemplateEntity template, int? templateNumber)
        {
            if (template != null)
            {
                return template;
            }
            if (!templateNumber.HasValue)
            {
                throw new Exception("Value Code must have a template");
            }

            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(ValueCodeTemplateEntity.Fields.ed_TemplateId, ConditionOperator.Equal, templateNumber);
            template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(true), filter);

            if (template == null)
            {
                throw new Exception($"Value Code Template with the given number ({templateNumber}) does not exist");
            }

            return template;
        }

        /// <summary>
        /// Creates a value code.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="deliveryType"></param>
        /// <param name="validTo"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="valueCodeApproval"></param>
        /// <param name="voucherType"></param>
        /// <param name="contact"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="template"></param>
        /// <param name="lead"></param>
        /// <param name="refund"></param>
        /// <param name="travelCardNumber"></param>
        /// <returns></returns>
        public static Guid CreateValueCodeGeneric(Plugin.LocalPluginContext localContext, int deliveryType, DateTime validTo, float amount, decimal periodPrice, Generated.ed_valuecode_ed_typeoption type,
            ValueCodeApprovalEntity valueCodeApproval, int voucherType, ContactEntity contact, string phoneNumber, string email, ValueCodeTemplateEntity template, LeadEntity lead, RefundEntity refund, TravelCardEntity travelCard)
        {

            localContext.TracingService.Trace($"---> Entering {nameof(CreateValueCodeGeneric)}.");

            string nPhoneNumber = PhoneNumberUtility.CheckPhoneFormatCreateValueCodeGeneric(localContext, phoneNumber);

            #region old code 8042
            //if (phoneNumber != null && phoneNumber != "")
            //{
            //    if (phoneNumber.StartsWith("0046"))
            //    {
            //        phoneNumber = "+46" + phoneNumber.Substring(4);
            //    }
            //    else if (phoneNumber.StartsWith("0045"))
            //    {
            //        phoneNumber = "+45" + phoneNumber.Substring(4);
            //    }
            //    else if (phoneNumber.StartsWith("46") && phoneNumber.Length == 11)
            //    {
            //        phoneNumber = "+46" + phoneNumber.Substring(2);
            //    }
            //    else if (phoneNumber.StartsWith("45") && phoneNumber.Length == 11)
            //    {
            //        phoneNumber = "+45" + phoneNumber.Substring(2);
            //    }
            //    else if (phoneNumber.StartsWith("07") && phoneNumber.Length == 10)
            //    {
            //        phoneNumber = "+46" + phoneNumber.Substring(1);
            //    }
            //}
            #endregion

            //string apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_VoucherService);
            //Get settings entity
            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_VoucherService, ConditionOperator.NotNull);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_CRMPlusService, ConditionOperator.NotNull);
            CgiSettingEntity cgiSetting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext,
                new ColumnSet(CgiSettingEntity.Fields.ed_VoucherService, CgiSettingEntity.Fields.ed_CRMPlusService), settingFilter);

            string apiUrl = cgiSetting.ed_VoucherService;
            string fasadEndpoint = cgiSetting.ed_CRMPlusService;
            var requestToken = string.Empty;

            #region GetAccessToken from Fasad

            //-- Create mthod for reuse --
            var tokenHttpWebReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/ValueCode/GetAccessToken/voucher", fasadEndpoint));
            tokenHttpWebReq.ContentType = "text/plain; charset=utf-8";
            tokenHttpWebReq.Method = "GET";

            try
            {
                var tokenHttpResponse = (HttpWebResponse)tokenHttpWebReq.GetResponse();
                if (tokenHttpResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(string.Format("Error - Could not access required token for VoucherService. Response: " + tokenHttpResponse.StatusCode.ToString()));
                }
                else
                {
                    using (var streamReader = new StreamReader(tokenHttpResponse.GetResponseStream()))
                    {
                        //Read response token
                        requestToken = streamReader.ReadToEnd();
                    }

                    if (string.IsNullOrWhiteSpace(requestToken))
                    {
                        throw new Exception(string.Format("Error - Could not access required token for VoucherService. Returned token was null."));
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
            //-- Create mthod for reuse --

            #endregion

            localContext.Trace($"(CreateValueCodeGeneric) API: {apiUrl}");

            string InputJSON = string.Empty;

            Uri url = new Uri(apiUrl);

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest httpWebRequest = CreateRequest(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ServicePoint.Expect100Continue = true;

            localContext.Trace($"(CreateValueCodeGeneric) Create Token for Voucher Service");
            //Changed with VoucherService 2.0
            //ApiHelper.CreateTokenForVoucherService(localContext, httpWebRequest);
            httpWebRequest.Headers.Add("Authorization", "Bearer " + requestToken);

            localContext.Trace($"(CreateValueCodeGeneric) Created Token for Voucher Service");

            localContext.Trace($"(CreateValueCodeGeneric) Fetch Value Code Template");
            template = GetValueCodeTemplate(localContext, template, voucherType);
            localContext.Trace($"(CreateValueCodeGeneric) Fetched Value Code Template");

            // Call to Voucher Service
            localContext.Trace($"(CreateValueCodeGeneric) Create input JSON for Voucher Service");
            localContext.Trace($"(CreateValueCodeGeneric) validTo: {validTo}, voucherType: {template.ed_TemplateId}, amount: {amount}, phoneNumber: {nPhoneNumber}, email: {email}");


            /* ONLY FOR STATISTICS - Marcus Stenswed 2019-10-03 */
            int totalAmountSentToVoucherService = 0;
            decimal originalPeriodPrice = 0;
            float originalReskassa = 0;

            // 2019-08-13: Periodbelopp och reskassabelopp kommer endast slås ihop om det är Förlustgaranti
            if (voucherType == (int)Generated.ed_valuecodetypeglobal.Forlustgaranti)
            {
                if (amount <= 0 && periodPrice > 0)
                {
                    totalAmountSentToVoucherService = Convert.ToInt32(periodPrice);
                    localContext.TracingService.Trace($"(endast period) Belopp som skickas till VoucherService {totalAmountSentToVoucherService}");
                }
                else if (amount > 0 && periodPrice > 0)
                {
                    totalAmountSentToVoucherService = Convert.ToInt32((int)amount + (int)periodPrice);
                    localContext.TracingService.Trace($"(period + reskassa) Belopp som skickas till VoucherService {totalAmountSentToVoucherService}");
                }
                else if (amount > 0 && periodPrice <= 0)
                {
                    totalAmountSentToVoucherService = Convert.ToInt32(amount);
                    localContext.TracingService.Trace($"(endast reskassa) Belopp som skickas till VoucherService {totalAmountSentToVoucherService}");
                }

                originalPeriodPrice = periodPrice;
                originalReskassa = amount;

            }
            // Annars är det endast periodbeloppet eller reskassa
            else
            {
                if (amount <= 0 && periodPrice > 0)
                    totalAmountSentToVoucherService = Convert.ToInt32(periodPrice);
                else if ((amount > 0 && periodPrice <= 0) || (amount > 0 && periodPrice > 0))
                    totalAmountSentToVoucherService = Convert.ToInt32(amount);
                else if (amount <= 0 && periodPrice <= 0)
                    throw new InvalidPluginExecutionException($"Amount and period price cannot both be 0.");

                originalPeriodPrice = periodPrice;
                originalReskassa = amount;
            }

            /* ------------------------------------------------ */

            InputJSON = CreateInputJSONVoucherServiceGeneric(localContext, validTo, (int)template.ed_TemplateId, amount, periodPrice,
                nPhoneNumber, email, travelCard, refund);

            localContext.Trace($"(CreateValueCodeGeneric) Created input JSON: {InputJSON}");

            localContext.Trace($"(CreateValueCodeGeneric) Starting streamWriter");
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(InputJSON);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {

                VoucherCrm responseService = new VoucherCrm();

                localContext.Trace($"(CreateValueCodeGeneric) Starting GetResponse from {apiUrl}");

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                localContext.Trace($"(CreateValueCodeGeneric) Declared httpResponse");

                string response = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    localContext.Trace($"(CreateValueCodeGeneric) Inside streamReader...");
                    //Read response
                    response = streamReader.ReadToEnd();

                    localContext.Trace($"(CreateValueCodeGeneric) Read to end");
                }

                localContext.Trace($"(CreateValueCodeGeneric) Opening MemoryStream...");
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
                DataContractJsonSerializer ser = new DataContractJsonSerializer(responseService.GetType());
                responseService = ser.ReadObject(ms) as VoucherCrm;
                ms.Close();
                localContext.Trace($"(CreateValueCodeGeneric) Closing MemoryStream...");

                if (responseService == null)
                    throw new WebException($"Responsen från {apiUrl} returnerade ingenting.");

                // Create ValueCode from Voucher Service
                localContext.Trace($"(CreateValueCodeGeneric) Entering CreateValueCodeFromVoucherServiceResponseGeneric...");
                Guid valueCodeGuid = CreateValueCodeFromVoucherServiceResponseGeneric(localContext, responseService, deliveryType, template,
                    contact, lead, type, email, nPhoneNumber, refund, valueCodeApproval, voucherType, travelCard, amount, totalAmountSentToVoucherService, originalPeriodPrice, originalReskassa);

                if (valueCodeGuid == null)
                    throw new InvalidPluginExecutionException("Kunde inte skapa värde kod i CRM.");

                return valueCodeGuid;
            }
            catch (WebException we)
            {
                string resultFromService;

                HttpWebResponse response = (HttpWebResponse)we.Response;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    resultFromService = streamReader.ReadToEnd();
                    localContext.TracingService.Trace($"got http error: {response.StatusCode} Content: {resultFromService}");
                }

                throw new WebException($"Ett fel uppstod vid anrop till Voucher Service mot adressen '{apiUrl}'. Ex:{we.Message}, message:{resultFromService}");
                //throw new WebException($"Ett fel uppstod vid anrop till Voucher Service mot adressen '{apiUrl}'. Ex:{we}, message:{we.Message}");
            }
            catch (ProtocolViolationException pve)
            {
                throw new ProtocolViolationException("1" + pve.Message);
            }
            catch (NotSupportedException nse)
            {
                throw new NotSupportedException("2" + nse.Message);
            }
            catch (InvalidOperationException ope)
            {
                throw new InvalidOperationException("3" + ope.Message);
            }
            catch (InvalidPluginExecutionException ioe)
            {
                throw new InvalidPluginExecutionException("4 " + ioe.Message);
            }
        }

        private static HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json";
            return httpWebRequest;
        }

        private static string CreateInputJSON(int clearOnTemplate, string type, float amount, string deliverType, string clientsPhoneNumber, string clientsEmailAddress, string apiToken, DateTime? lastValid)
        {
            SendValueCodeRequest sendMessageRequest = new SendValueCodeRequest()
            {
                template = clearOnTemplate,
                type = type,
                amount = amount,
                delivery_type = deliverType,
                api_token = apiToken,
                clients_phone_number = (clientsPhoneNumber != null) ? clientsPhoneNumber : "Unknown",
                clients_email_address = (clientsEmailAddress != null) ? clientsEmailAddress : "Unknown",
            };

            if (lastValid != null)
            {
                sendMessageRequest.valid_days = (lastValid.Value.ToLocalTime() - DateTime.Now.ToLocalTime()).Days + 1;
            }

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SendValueCodeRequest));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, sendMessageRequest);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validDays">Id on template for creating valuecode</param>
        /// <param name="clearOnTemplate">Id on template for creating valuecode</param>
        /// <param name="type">Email or phonenumber</param>
        /// <param name="amount"></param>
        /// <param name="deliverType"></param>
        /// <param name="clientsPhoneNumber"></param>
        /// <param name="clientsEmailAddress"></param>
        /// <param name="apiToken"></param>
        /// <param name="label1"></param>
        /// <param name="value1"></param>
        /// <param name="label2"></param>
        /// <param name="value2"></param>
        /// <param name="label3"></param>
        /// <param name="value3"></param>
        /// <param name="label4"></param>
        /// <param name="value4"></param>
        /// <param name="label5"></param>
        /// <param name="value5"></param>
        /// <returns></returns>
        private static string CreateInputJSONGeneric(Plugin.LocalPluginContext localContext, Generated.ed_valuecodetype valueCodeType, int validDays, int clearOnTemplate, string type, float amount, string deliverType, string clientsPhoneNumber, string clientsEmailAddress, string apiToken, string label1, string value1, string label2, string value2, string label3, string value3, string label4, string value4, string label5, string value5)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(CreateInputJSONGeneric)}.");
            string customText = "";

            if (label1 != "" && value1 != "")
                customText += $"{label1}: {value1}\n";
            if (label2 != "" && value2 != "")
                customText += $"{label2}: {value2}\n";
            if (label3 != "" && value3 != "")
                customText += $"{label3}: {value3}\n";
            if (label4 != "" && value4 != "")
                customText += $"{label4}: {value4}\n";
            if (label5 != "" && value5 != "")
                customText += $"{label5}: {value5}";

            DataContractJsonSerializer js = null;
            MemoryStream msObj = new MemoryStream();

            localContext.TracingService.Trace($"Value Code Type: {valueCodeType}");

            if (valueCodeType == Generated.ed_valuecodetype.Utansaldo)
            {
                localContext.TracingService.Trace($"Setting up ValueCodeCouponRequest.------\nclearon:{clearOnTemplate}\tapi_token:{apiToken}\ttype:{type}\namount:{amount}\tdelivery_type:{deliverType}\t" +
                    $"email:{clientsEmailAddress}:\tmobile:{clientsPhoneNumber}\nvaliddays:{validDays}\tcustom_Text:{customText}\n-------");
                var valueCode = new ValueCodeCouponRequest()
                {

                    template = clearOnTemplate,
                    api_token = apiToken,
                    type = type,
                    amount = amount,
                    delivery_type = deliverType,
                    clients_email_address = (clientsEmailAddress != null) ? clientsEmailAddress : "Unknown",
                    clients_phone_number = (clientsPhoneNumber != null) ? clientsPhoneNumber : "Unknown",
                    valid_days = validDays,
                    custom_text = customText,

                    //ticket_reference = null,
                    //reason = null,
                    //clients_first_name = null,
                    //clients_last_name = null,
                    //clients_countries_id = -1,
                    //custom_image = null
                };

                js = new DataContractJsonSerializer(typeof(ValueCodeCouponRequest));
                js.WriteObject(msObj, valueCode);
            }

            else
            {
                localContext.TracingService.Trace($"Setting up ValueCodeVoucherRequest.");
                var valueCode = new ValueCodeVoucherRequest()
                {
                    template = clearOnTemplate,
                    amount = amount,
                    api_token = apiToken,
                    trans_id = "1",
                    ki_store_id = "1",
                    concept_id = "1",
                    store_id = "1",
                    country_code = "1",
                    store_owner_id = "1",

                    //custom_identifier = ""
                };

                js = new DataContractJsonSerializer(typeof(ValueCodeVoucherRequest));
                js.WriteObject(msObj, valueCode);
            }


            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();


            localContext.TracingService.Trace($"<--- Exiting {nameof(CreateInputJSONGeneric)}.");

            return json;

            //SendValueCodeRequest sendMessageRequest = new SendValueCodeRequest()
            //{
            //    template = clearOnTemplate,
            //    type = type,
            //    amount = amount,
            //    delivery_type = deliverType,
            //    api_token = apiToken,
            //    clients_phone_number = (clientsPhoneNumber != null) ? clientsPhoneNumber : "Unknown",
            //    clients_email_address = (clientsEmailAddress != null) ? clientsEmailAddress : "Unknown",
            //    custom_text = customText,
            //    valid_days = validDays
            //};
        }

        public static string CreateInputJSONVoucherServiceGeneric(Plugin.LocalPluginContext localContext, DateTime validTo, int voucherType, float amount, decimal periodPrice,
            string clientsPhoneNumber, string clientsEmailAddress, TravelCardEntity travelCard, RefundEntity refund)
        {
            localContext.TracingService.Trace($"---> Entering {nameof(CreateInputJSONVoucherServiceGeneric)}.");


            DataContractJsonSerializer js = null;
            MemoryStream msObj = new MemoryStream();


            ValueCodeCouponVoucherServiceRequest valueCode = new ValueCodeCouponVoucherServiceRequest();

            // 2019-08-13: Periodbelopp och reskassabelopp kommer endast slås ihop om det är Förlustgaranti
            if (voucherType == (int)Generated.ed_valuecodetypeglobal.Forlustgaranti)
            {
                if (amount <= 0 && periodPrice > 0)
                    valueCode.amount = Convert.ToInt32(periodPrice);
                else if (amount > 0 && periodPrice > 0)
                    valueCode.amount = Convert.ToInt32((int)amount + (int)periodPrice);
                else if (amount > 0 && periodPrice <= 0)
                    valueCode.amount = Convert.ToInt32(amount);
                else if (amount <= 0 && periodPrice <= 0)
                    throw new InvalidPluginExecutionException($"Amount and period price cannot both be 0.");
            }
            // Annars är det endast periodbeloppet eller reskassa
            else
            {
                if (amount <= 0 && periodPrice > 0)
                    valueCode.amount = Convert.ToInt32(periodPrice);
                else if ((amount > 0 && periodPrice <= 0) || (amount > 0 && periodPrice > 0))
                    valueCode.amount = Convert.ToInt32(amount);
                else if (amount <= 0 && periodPrice <= 0)
                    throw new InvalidPluginExecutionException($"Amount and period price cannot both be 0.");
            }


            if (clientsEmailAddress != null && clientsEmailAddress != "")
            {
                valueCode.contactAddress = clientsEmailAddress;
                // contactType 2 = Email
                valueCode.contactType = 2;
            }
            else if (clientsPhoneNumber != null && clientsPhoneNumber != "")
            {
                valueCode.contactAddress = clientsPhoneNumber;
                // contactType 1 = SMS
                valueCode.contactType = 1;
            }

            //CK - This might have changed
            valueCode.validFromDate = DateTime.Now;
            valueCode.validToDate = validTo;

            valueCode.voucherType = voucherType;

            //Created By
            RefundEntity refundInfo = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refund.Id, new ColumnSet(
                    RefundEntity.Fields.CreatedBy, RefundEntity.Fields.cgi_Caseid));
            if (refundInfo != null && refundInfo.CreatedBy != null) 
            {
                //Fetch the SystemUser
                SystemUserEntity createdByUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, refundInfo.CreatedBy.Id, 
                    new ColumnSet(SystemUserEntity.Fields.FullName));

                if (createdByUser != null && !string.IsNullOrWhiteSpace(createdByUser.FullName)) 
                {
                    valueCode.CreatedBy = createdByUser.FullName;
                }
            }

            // 1 = förseningsersätting
            if (voucherType == 1)
            {
                IncidentEntity incidentInfo = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refundInfo.cgi_Caseid.Id, new ColumnSet(
                    IncidentEntity.Fields.TicketNumber,
                    IncidentEntity.Fields.cgi_RGOLIssueId,
                    IncidentEntity.Fields.cgi_ActionDate,
                    IncidentEntity.Fields.cgi_arrival_date));

                if (incidentInfo.cgi_ActionDate != null)
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan från {incidentInfo.cgi_ActionDate.Value.Date.ToShortDateString()} med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }
                else if (incidentInfo.cgi_arrival_date != null)
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan från {incidentInfo.cgi_arrival_date.Value.Date.ToShortDateString()} med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }
                else
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }

            }
            // 2 = presentkort (med saldo)
            else if (voucherType == 2)
            {

                valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                    + "<p>Värdekoden används som betalning i vår app. Värdekoden lägger du till under \"Inställningar\"</p>";
            }
            // 3 = förlustgaranti
            else if (voucherType == 3)
            {

                TravelCardEntity travelCardInfo = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext, TravelCardEntity.EntityLogicalName, travelCard.Id, new ColumnSet(
                    TravelCardEntity.Fields.ed_PeriodPricePaidAmount, // Värde Jojo Period
                    TravelCardEntity.Fields.st_SaldoReskassa, //Värde jojo Reskassa
                    TravelCardEntity.Fields.cgi_travelcardnumber, // Kortnummer
                    TravelCardEntity.Fields.ed_ZonesString, // Zoner
                    TravelCardEntity.Fields.cgi_ValidFrom, // Period Startdatum
                    TravelCardEntity.Fields.cgi_ValidTo // Period Slutdatum
                    ));


                //TODO: "lösa in den senast...#date#" Adjust this later.
                valueCode.voucherText = $"<p>Hade du reskassa på ditt kort kan du lösa in den senast 2019-12-31. Är det ett periodkort måste värdekoden lösas in senast {travelCardInfo.cgi_ValidTo?.ToLocalTime().ToShortDateString()}.</p>" +
                    $"<p>Du kan visa QR-koden i en smartphone eller skriv ut den. </p>"
                    + $"<p>Värde Jojo Period: {travelCardInfo?.ed_PeriodPricePaidAmount?.ToString("F")} kr <br>"
                    + $"Värde Jojo Reskassa: {travelCardInfo?.st_SaldoReskassa?.ToString("F")} kr <br>"
                    + $"Kortnummer: {travelCardInfo?.cgi_travelcardnumber} <br>"
                    + $"Zoner: {travelCardInfo?.ed_ZonesString} <br>"
                    + $"Period Startdatum: {travelCardInfo?.cgi_ValidFrom?.ToLocalTime().ToShortDateString()} <br>"
                    + $"Period Slutdatum: {travelCardInfo?.cgi_ValidTo?.ToLocalTime().ToShortDateString()}. <p>";


                localContext.TracingService.Trace($"Reskassa: {amount}. Period: {periodPrice}");

                if (amount <= 0 && periodPrice > 0)
                {
                    valueCode.amount = Convert.ToInt32(periodPrice);
                    localContext.TracingService.Trace($"(endast period) Belopp som skickas till VoucherService {valueCode.amount}");
                }
                else if (amount > 0 && periodPrice > 0)
                {
                    valueCode.amount = Convert.ToInt32((int)amount + (int)periodPrice);
                    localContext.TracingService.Trace($"(period + reskassa) Belopp som skickas till VoucherService {valueCode.amount}");
                }
                else if (amount > 0 && periodPrice <= 0)
                {
                    valueCode.amount = Convert.ToInt32(amount);
                    localContext.TracingService.Trace($"(endast reskassa) Belopp som skickas till VoucherService {valueCode.amount}");
                }
                else if (amount <= 0 && periodPrice <= 0)
                    throw new InvalidPluginExecutionException($"Amount and period price cannot both be 0.");
            }
            // 4 = ersättning
            else if (voucherType == 4)
            {
                IncidentEntity incidentInfo = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refundInfo.cgi_Caseid.Id, new ColumnSet(
                    IncidentEntity.Fields.TicketNumber,
                    IncidentEntity.Fields.cgi_RGOLIssueId,
                    IncidentEntity.Fields.cgi_ActionDate,
                    IncidentEntity.Fields.cgi_arrival_date));

                valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriva ut den. <br>" +
                    $"Referens {incidentInfo.TicketNumber} <br>" +
                    "Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";

                if (incidentInfo.cgi_arrival_date != null)
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan från {incidentInfo.cgi_arrival_date.Value.Date.ToShortDateString()} med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }
                else if (incidentInfo.cgi_ActionDate != null)
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan från {incidentInfo.cgi_ActionDate.Value.Date.ToShortDateString()} med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }
                else
                {
                    valueCode.voucherText = "<p>Du kan visa QR-koden i en smartphone eller skriv ut den.</p>"
                                        + $"<p>Ersättningen avser din ansökan med referens {incidentInfo.TicketNumber} / {incidentInfo.cgi_RGOLIssueId}.</p>"
                                        + "<p>Värdekoden kan användas som betalning i vår app eller <a href='https://www.skanetrafiken.se/kopstallen'>hos våra kundcenter och utvalda serviceombud</a>. Koden kan endast användas vid köp av Skånetrafikens biljetter.</p>";
                }

            }


            js = new DataContractJsonSerializer(typeof(ValueCodeCouponRequest));
            js.WriteObject(msObj, valueCode);

            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            localContext.TracingService.Trace($"<--- Exiting {nameof(CreateInputJSONVoucherServiceGeneric)}.");
            return json;

        }


        public static string CallGetOutstandingChargesFromBiztalkAction(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            OrganizationRequest request = new OrganizationRequest("ed_GetOutstandingCharges");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["GetOutstandingChargesResponse"];
        }

        public static string CallGetCardDetailsFromBiztalkAction(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            OrganizationRequest request = new OrganizationRequest("ed_GetCardDetails");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["CardDetailsResponse"];
        }

        public static BiztalkParseCardDetailsMessage CallParseCardDetailsFromBiztalkAction(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBiztalkResponse");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            BiztalkParseCardDetailsMessage biztalkResponseObject = new BiztalkParseCardDetailsMessage();

            biztalkResponseObject.CardCategoryField = (string)response["CardCategoryField"];
            biztalkResponseObject.CardHotlistedField = (bool)response["CardHotlistedField"];
            biztalkResponseObject.CardIssuerField = (string)response["CardIssuerField"];
            biztalkResponseObject.CardNumberField = (string)response["CardNumberField"];
            biztalkResponseObject.CardTypePeriodField = (int)response["CardTypePeriodField"];
            biztalkResponseObject.CardTypeValueField = (int)response["CardTypeValueField"];
            biztalkResponseObject.CardValueProductTypeField = (string)response["CardValueProductTypeField"];

            biztalkResponseObject.BalanceField = (decimal)response["BalanceField"];
            biztalkResponseObject.ContractSerialNumberField = (string)response["ContractSerialNumberField"];
            biztalkResponseObject.CurrencyField = (string)response["CurrencyField"];
            biztalkResponseObject.PurseHotlistedField = (bool)response["PurseHotlistedField"];
            biztalkResponseObject.OutstandingDirectedAutoloadField = (bool)response["OutstandingDirectedAutoloadField"];
            biztalkResponseObject.OutstandingEnableThresholdAutoloadField = (bool)response["OutstandingEnableThresholdAutoloadField"];
            biztalkResponseObject.PeriodCardCategoryField = (string)response["PeriodCardCategoryField"];
            biztalkResponseObject.PeriodCurrencyField = (string)response["PeriodCurrencyField"];
            biztalkResponseObject.PeriodEndField = (DateTime)response["PeriodEndField"];
            biztalkResponseObject.PeriodHotlistedField = (bool)response["PeriodHotlistedField"];
            biztalkResponseObject.PeriodOutstandingDirectedAutoloadField = (bool)response["PeriodOutstandingDirectedAutoloadField"];
            biztalkResponseObject.PeriodOutstandingEnableThresholdAutoload = (bool)response["PeriodOutstandingEnableThresholdAutoload"];
            biztalkResponseObject.PeriodStartField = (DateTime)response["PeriodStartField"];
            biztalkResponseObject.PricePaidField = (decimal)response["PricePaidField"] / 100;
            biztalkResponseObject.ProductTypeField = (string)response["ProductTypeField"];
            biztalkResponseObject.WaitingPeriodsField = (string)response["WaitingPeriodsField"];
            biztalkResponseObject.ZoneListIDField = (string)response["ZoneListIDField"];
            biztalkResponseObject.ZonesListField = (string)response["ZonesListField"];

            return biztalkResponseObject;
        }

        public static string CallBlockCardBiztalkAction(Plugin.LocalPluginContext localContext, string cardNumber, int reasonCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_BlockCardBiztalk");
            request["CardNumber"] = cardNumber;
            request["ReasonCode"] = reasonCode;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["CardBlockResponse"];
        }

        public static string CallParseBlockCardFromBiztalkAction(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBlockCardResponseFromBiztalk");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string parsedCardBlockResponse = (string)response["RequestCardBlockResult"];
            return parsedCardBlockResponse;
        }

        //Change response to be an object
        public static string CallCaptureOrderAction(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            OrganizationRequest request = new OrganizationRequest("ed_CaptureOrder");
            request["CardNumber"] = cardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["CaptureOrderResponse"];
        }

        public static string CallPlaceOrderAction(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            OrganizationRequest request = new OrganizationRequest("ed_PlaceOrder");
            request["CardNumber"] = cardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["PlaceOrderResponse"];
        }

        public static GetCardProperties CallGetCardAction(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            OrganizationRequest request = new OrganizationRequest("ed_GetCard");
            request["CardNumber"] = cardNumber;

            //Handle Response Model
            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            GetCardProperties getCardProperties = new GetCardProperties();
            getCardProperties.CardNumber = (string)response["CardNumberResp"];
            getCardProperties.IsClosed = (bool)response["IsClosed"];
            getCardProperties.Amount = (decimal)response["Amount"];
            getCardProperties.ClosedReason = (string)response["ClosedReason"];
            getCardProperties.IsReserved = (bool)response["IsReserved"];
            getCardProperties.IsExpired = (bool)response["IsExpired"];
            getCardProperties.LastTransactionDate = (DateTime)response["LastTransactionDate"];

            return getCardProperties;
        }

        public static EntityReference CallCreateValueCodeAction(Plugin.LocalPluginContext localContext, int voucherType, decimal amount, decimal periodPrice, string mobile, string email,
            EntityReference refundId, EntityReference leadId, EntityReference contactId, EntityReference valueCodeApprovalId, int deliveryMethod, EntityReference travelCard)
        {
            OrganizationRequest request = new OrganizationRequest("ed_CreateValueCodeGeneric");
            request["VoucherType"] = voucherType; // 2 = InlostReskassa
            request["DeliveryType"] = deliveryMethod; // 2 = SMS
            request["Amount"] = amount; // 200kr?
            request["Mobile"] = mobile; // 0708707431
            request["Email"] = email; // EMPTY
            request["RefundId"] = refundId; // NULL
            request["LeadId"] = leadId; // NULL
            request["ContactId"] = contactId; // contact
            request["ValueCodeApprovalId"] = valueCodeApprovalId; // NULL
            request["TravelCardId"] = travelCard; // TravelCard
            request["PeriodPrice"] = periodPrice; // 0


            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (EntityReference)response["ValueCodeId"];
        }

        public static string CallSendValueCodeAction(Plugin.LocalPluginContext localContext, EntityReference valueCodeId)
        {
            OrganizationRequest request = new OrganizationRequest("ed_SendValueCode");
            request["Target"] = valueCodeId;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["Result"];
        }

        [Obsolete("This workflow in CRM doesn't work due to cache error.")]
        public static string CallCreateValueCodeApprovalAction(Plugin.LocalPluginContext localContext, decimal amount, string cardNumber, string contactId, int deliveryMethod, string email, string firstname, string lastname, string mobile, bool needsManualApproval, DateTime validTo, int typeOfValueCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_CreateValueCodeApproval");
            request["Amount"] = amount;
            request["CardNumber"] = cardNumber;
            request["ContactId"] = contactId;
            request["DeliveryMethod"] = deliveryMethod;
            request["Email"] = email;
            request["Firstname"] = firstname;
            request["Lastname"] = lastname;
            request["Mobile"] = mobile;
            request["NeedsManualApproval"] = needsManualApproval;
            request["ValidTo"] = validTo;
            request["TypeOfValueCode"] = typeOfValueCode;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["ValueCodeApprovalId"];
        }

        public static string CallCreateValueCodeApprovalActionTemp(Plugin.LocalPluginContext localContext, decimal amount, string cardNumber, string contactId, int deliveryMethod, string email, string firstname, string lastname, string mobile, bool needsManualApproval, DateTime validTo, int typeOfValueCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_CreateValueCodeApprovalTemp");
            request["Amount"] = amount;
            request["CardNumber"] = cardNumber;
            request["ContactId"] = contactId;
            request["DeliveryMethod"] = deliveryMethod;
            request["Email"] = email;
            request["Firstname"] = firstname;
            request["Lastname"] = lastname;
            request["Mobile"] = mobile;
            request["NeedsManualApproval"] = needsManualApproval;
            request["ValidTo"] = validTo;
            request["TypeOfValueCode"] = typeOfValueCode;


            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (string)response["ValueCodeApprovalId"];
        }

        public static EntityReference CallApproveValueCodeApprovalAction(Plugin.LocalPluginContext localContext, EntityReference approval)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ApproveValueCodeApproval");
            request["ValueCodeApprovalId"] = approval;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            return (EntityReference)response["ValueCodeId"];
        }

        public static void CallDeclineValueCodeApprovalAction(Plugin.LocalPluginContext localContext, EntityReference approval)
        {
            OrganizationRequest request = new OrganizationRequest("ed_DeclineValueCode");
            request["ValueCodeApprovalId"] = approval;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);
        }

        private static T GetAPIResponse<T>(HttpWebRequest httpWebRequest) where T : class, new()
        {
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw;
            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string responseJson = streamReader.ReadToEnd();

                T responseObj = new T();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseJson)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(responseObj.GetType());
                    responseObj = ser.ReadObject(ms) as T;
                    ms.Close();
                }
                return responseObj;
            }
        }

        private static Guid CreateValueCodeFromResponse(Plugin.LocalPluginContext localContext, ValueCodeResponseMessage response, ValueCodeTemplateEntity template, ContactEntity contact, LeadEntity lead, IncidentEntity incident, RefundEntity refund, Generated.ed_valuecode_ed_typeoption type, string email, string phoneNumber)
        {
            /*
            ValueCodeTemplateEntity template = new ValueCodeTemplateEntity();

            if (response.couponDetails.template.coupon_templates_id.ToString() != "")
            {
                FilterExpression templateFilter = new FilterExpression(LogicalOperator.And);
                templateFilter.AddCondition(ValueCodeTemplateEntity.Fields.ed_TemplateId, ConditionOperator.Equal, response.couponDetails.template.coupon_templates_id.ToString());
                templateFilter.AddCondition(ValueCodeTemplateEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_valuecodetemplateState.Active);

                template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(ValueCodeTemplateEntity.Fields.Id), templateFilter);

                if (template == null)
                {
                    template = CreateTemplateFromResponse(localContext, response);                    
                }
            }*/

            DateTime? nullDate = null;

            QueryExpression queryCampaign = new QueryExpression
            {
                EntityName = CampaignEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, response.couponDetails.template.coupon_templates_campaign_number)
                        }
                }
            };

            CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, queryCampaign);

            string lastDate = response.couponDetails.coupons_last_redemption_date.date + " Z";

            ValueCodeEntity valueCode = new ValueCodeEntity
            {
                ed_Link = response.couponLink,
                ed_AdminsId = response.couponDetails.coupons_admins_id,
                ed_Amount = new Money(response.couponDetails.coupons_amount),
                ed_Campaign = (campaign != null) ? campaign.ToEntityReference() : null,
                ed_ClientsId = response.couponDetails.coupons_clients_id,
                ed_CodeId = response.couponDetails.coupons_id.ToString(),
                ed_CompaniesId = response.couponDetails.coupons_companies_id,
                ed_CreatedTimestamp = (response.couponDetails.coupons_created_timestamp != "") ? Convert.ToDateTime(response.couponDetails.coupons_created_timestamp) : nullDate,
                ed_CustomImage = "",
                ed_CustomText = "",
                ed_DeliveryType = response.couponDetails.coupons_delivery_type,
                ed_Ean = response.couponDetails.coupons_ean,
                ed_Image = response.couponDetails.coupons_image,
                ed_LastRedemptionDate = (response.couponDetails.coupons_last_redemption_date.date != "") ? Convert.ToDateTime(lastDate, DateTimeFormatInfo.CurrentInfo) : nullDate,
                ed_Reason = response.couponDetails.coupons_reason,
                ed_Sent = response.couponDetails.coupons_sent,
                ed_Status = response.couponDetails.coupons_status,
                ed_TemplatesId = response.couponDetails.coupons_templates_id,
                ed_TicketReference = "",
                ed_Type = response.couponDetails.coupons_type,
                ed_TypeOption = type,
                ed_name = response.couponDetails.coupons_id.ToString(),
                ed_ValueCodeTemplate = template.ToEntityReference(),
                ed_Contact = (contact != null) ? contact.ToEntityReference() : null,
                ed_Lead = (lead != null) ? lead.ToEntityReference() : null,
                ed_Case = (incident != null) ? incident.ToEntityReference() : null,
                ed_Refund = (refund != null) ? refund.ToEntityReference() : null,
                ed_RgolId = (incident != null && incident.cgi_RGOLIssueId != null) ? incident.cgi_RGOLIssueId : null,
                ed_CaseNumber = (incident != null && incident.TicketNumber != null) ? incident.TicketNumber : null,
                ed_MobileNumber = phoneNumber,
                ed_Email = email
            };

            //valueCode.ed_LastRedemptionDate = new DateTime(valueCode.ed_LastRedemptionDate.Value.Year, valueCode.ed_LastRedemptionDate.Value.Month, valueCode.ed_LastRedemptionDate.Value.Day, valueCode.ed_LastRedemptionDate.Value.Hour, valueCode.ed_LastRedemptionDate.Value.Minute, valueCode.ed_LastRedemptionDate.Value.Second, DateTimeKind.Utc);

            Guid valueCodeId = XrmHelper.Create(localContext.OrganizationService, valueCode);

            return valueCodeId;

        }

        private static Guid CreateValueCodeFromResponseGeneric(Plugin.LocalPluginContext localContext, ValueCodeResponseMessage response, ValueCodeTemplateEntity template, ContactEntity contact, LeadEntity lead, Generated.ed_valuecode_ed_typeoption type, string email, string phoneNumber, RefundEntity refund, ValueCodeApprovalEntity valueCodeApproval)
        {

            localContext.TracingService.Trace($"(CreateValueCodeFromResponseGeneric) started.");

            //if (valueCodeApproval == null)
            //    throw new InvalidPluginExecutionException("Value Code Approval is null.");

            DateTime? nullDate = null;

            QueryExpression queryCampaign = new QueryExpression
            {
                EntityName = CampaignEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, response.couponDetails.template.coupon_templates_campaign_number)
                        }
                }
            };

            CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, queryCampaign);
            localContext.TracingService.Trace($"Fetched campaign: {campaign?.Id}");


            string lastDate = response.couponDetails.coupons_last_redemption_date.date + " Z";

            string rgolId = "";
            string caseNumber = "";
            if (refund?.cgi_Caseid != null)
            {
                IncidentEntity incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refund.cgi_Caseid, new ColumnSet(
                    IncidentEntity.Fields.TicketNumber, IncidentEntity.Fields.cgi_RGOLIssueId));

                rgolId = incident.cgi_RGOLIssueId;
                caseNumber = incident.TicketNumber;
            }

            localContext.TracingService.Trace($"Setting up ValueCodeEntity.");

            ValueCodeEntity valueCode = new ValueCodeEntity
            {
                ed_Link = response.couponLink,
                ed_AdminsId = response.couponDetails.coupons_admins_id,
                ed_Amount = new Money(response.couponDetails.coupons_amount),
                ed_Campaign = (campaign != null) ? campaign.ToEntityReference() : null,
                ed_ClientsId = response.couponDetails.coupons_clients_id,
                ed_CodeId = response.couponDetails.coupons_id.ToString(),
                ed_CompaniesId = response.couponDetails.coupons_companies_id,
                ed_CreatedTimestamp = (response.couponDetails.coupons_created_timestamp != "") ? Convert.ToDateTime(response.couponDetails.coupons_created_timestamp) : nullDate,
                ed_CustomImage = "",
                ed_CustomText = "",
                ed_DeliveryType = response.couponDetails.coupons_delivery_type,
                ed_Ean = response.couponDetails.coupons_ean,
                ed_Image = response.couponDetails.coupons_image,
                ed_LastRedemptionDate = (response.couponDetails.coupons_last_redemption_date.date != "") ? Convert.ToDateTime(lastDate, DateTimeFormatInfo.CurrentInfo) : nullDate,
                ed_Reason = response.couponDetails.coupons_reason,
                ed_Sent = response.couponDetails.coupons_sent,
                ed_Status = response.couponDetails.coupons_status,
                ed_TemplatesId = response.couponDetails.coupons_templates_id,
                ed_TicketReference = "",
                ed_Type = response.couponDetails.coupons_type,
                ed_TypeOption = type,
                ed_name = response.couponDetails.coupons_id.ToString(),
                ed_ValueCodeTemplate = template.ToEntityReference(),
                ed_Contact = (contact != null) ? contact.ToEntityReference() : null,
                ed_Lead = (lead != null) ? lead.ToEntityReference() : null,
                ed_Case = (refund?.cgi_Caseid != null) ? refund.cgi_Caseid : null,
                ed_Refund = (refund != null) ? refund.ToEntityReference() : null,
                ed_RgolId = (rgolId != null) ? rgolId : null,
                ed_CaseNumber = (caseNumber != null) ? caseNumber : null,
                ed_MobileNumber = phoneNumber,
                ed_Email = email,
                ed_ValueCodeApprovalId = valueCodeApproval?.ToEntityReference()
            };



            Guid valueCodeId = XrmHelper.Create(localContext.OrganizationService, valueCode);
            localContext.TracingService.Trace($"Created ValueCodeEntity.");

            return valueCodeId;

        }

        private static Guid CreateValueCodeFromVoucherServiceResponseGeneric(Plugin.LocalPluginContext localContext, VoucherCrm response, int deliveryType, ValueCodeTemplateEntity template,
            ContactEntity contact, LeadEntity lead, Generated.ed_valuecode_ed_typeoption type, string email, string phoneNumber, RefundEntity refund,
            ValueCodeApprovalEntity valueCodeApproval, int voucherType, TravelCardEntity travelCard, float amount, int totalAmountSentToVoucherService, decimal originalPeriodPrice, float originalReskassa)
        {

            localContext.TracingService.Trace($"---> Entering CreateValueCodeFromVoucherServiceResponseGeneric.");

            localContext.TracingService.Trace($"{response}");

            DateTime lastDate = new DateTime();

            string rgolId = "";
            string caseNumber = "";
            if (refund != null)
            {
                if (refund?.cgi_Caseid != null)
                {
                    IncidentEntity incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refund.cgi_Caseid, new ColumnSet(
                        IncidentEntity.Fields.TicketNumber, IncidentEntity.Fields.cgi_RGOLIssueId));

                    rgolId = incident.cgi_RGOLIssueId;
                    caseNumber = incident.TicketNumber;
                }
            }

            localContext.TracingService.Trace($"Setting up ValueCodeEntity.");

            Guid valueCodeId = Guid.Empty;

            if (response != null)
            {
                if (response.validToDate != null)
                    lastDate = Convert.ToDateTime(response.validToDate);

                ValueCodeEntity valueCode = new ValueCodeEntity();

                if (totalAmountSentToVoucherService != null)
                    valueCode.ed_TotalAmountCalculatedBeforeVoucherService = totalAmountSentToVoucherService;
                else
                    valueCode.ed_TotalAmountCalculatedBeforeVoucherService = 9999999;

                if (originalReskassa != null)
                    valueCode.ed_OriginalAmount = (int)originalReskassa;
                else
                    valueCode.ed_OriginalAmount = 9999999;

                if (originalPeriodPrice != null)
                    valueCode.ed_OriginalAmountPeriodPrice = originalPeriodPrice;
                else
                    valueCode.ed_OriginalAmountPeriodPrice = 9999999;

                if (response.amount != null)
                    valueCode.ed_AmountFromVoucherService = response.amount;
                else
                    valueCode.ed_AmountFromVoucherService = 9999999;


                if (deliveryType == (int)Generated.ed_valuecodedeliverytypeglobal.Email)
                    valueCode.ed_ValueCodeDeliveryTypeGlobal = Generated.ed_valuecodedeliverytypeglobal.Email;

                if (deliveryType == (int)Generated.ed_valuecodedeliverytypeglobal.SMS)
                    valueCode.ed_ValueCodeDeliveryTypeGlobal = Generated.ed_valuecodedeliverytypeglobal.SMS;

                if (voucherType == (int)Generated.ed_valuecodetypeglobal.Ersattningsarende)
                    valueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Ersattningsarende;

                if (voucherType == (int)Generated.ed_valuecodetypeglobal.Forlustgaranti)
                    valueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forlustgaranti;

                if (voucherType == (int)Generated.ed_valuecodetypeglobal.Forseningsersattning)
                    valueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.Forseningsersattning;

                if (voucherType == (int)Generated.ed_valuecodetypeglobal.InlostReskassa)
                    valueCode.ed_ValueCodeTypeGlobal = Generated.ed_valuecodetypeglobal.InlostReskassa;

                valueCode.ed_Amount = new Money((decimal)amount);

                if (response.voucherCode != null)
                    valueCode.ed_CodeId = response.voucherCode;

                valueCode.ed_Link = response.voucherViewer;

                valueCode.ed_CustomImage = "";
                valueCode.ed_CustomText = "";

                //CK - Detta skickar de till oss
                if (response.validToDate != null)
                {
                    valueCode.ed_LastRedemptionDate = Convert.ToDateTime(response.validToDate);
                    valueCode.ed_ValidUntil = Convert.ToDateTime(response.validToDate);
                }

                //CK - Changed
                valueCode.ed_Status = response.eanCode.ToString();

                valueCode.ed_TicketReference = "";

                if (type != null)
                    valueCode.ed_TypeOption = type;

                if (response.voucherId != null)
                {
                    valueCode.ed_name = response.voucherId.ToString();
                    valueCode.ed_ValueCodeVoucherId = response.voucherId.ToString();
                }

                if (template != null)
                    valueCode.ed_ValueCodeTemplate = template.ToEntityReference();


                valueCode.ed_Lead = (lead != null) ? lead.ToEntityReference() : null;
                valueCode.ed_Case = (refund?.cgi_Caseid != null) ? refund.cgi_Caseid : null;
                valueCode.ed_Refund = (refund != null) ? refund.ToEntityReference() : null;
                valueCode.ed_RgolId = (rgolId != null) ? rgolId : null;
                valueCode.ed_CaseNumber = (caseNumber != null) ? caseNumber : null;

                if (phoneNumber != null)
                    valueCode.ed_MobileNumber = phoneNumber;

                if (email != null)
                    valueCode.ed_Email = email;

                if (valueCodeApproval != null)
                    valueCode.ed_ValueCodeApprovalId = valueCodeApproval?.ToEntityReference();

                if (contact != null)
                    valueCode.ed_Contact = contact.ToEntityReference();

                if (travelCard != null)
                    valueCode.ed_TravelCard = travelCard?.ToEntityReference();



                valueCodeId = XrmHelper.Create(localContext.OrganizationService, valueCode);
                localContext.TracingService.Trace($"Created ValueCodeEntity.");
            }

            localContext.TracingService.Trace($"<--- Exiting CreateValueCodeFromVoucherServiceResponseGeneric.");
            return valueCodeId;

        }


        private static ValueCodeTemplateEntity CreateTemplateFromResponse(Plugin.LocalPluginContext localContext, ValueCodeResponseMessage response)
        {
            var template = new ValueCodeTemplateEntity
            {
                ed_Amount = decimal.Parse(response.couponDetails.template.coupon_templates_amount, CultureInfo.InvariantCulture),
                ed_CampaignNumber = response.couponDetails.template.coupon_templates_campaign_number.ToString(),
                ed_CompaniesId = response.couponDetails.template.coupon_templates_companies_id,
                ed_Description = response.couponDetails.template.coupon_templates_description,
                ed_EmailText = response.couponDetails.template.coupon_templates_coupon_email_text,
                ed_SMSSender = response.couponDetails.template.coupon_templates_email_sender,
                ed_SMSText = response.couponDetails.template.coupon_templates_coupon_sms_text,
                ed_TemplateId = response.couponDetails.template.coupon_templates_id,
                ed_Type = response.couponDetails.template.coupon_templates_type,
                ed_TypeOption = (response.couponDetails.template.coupon_templates_type == "mobile") ? Schema.Generated.ed_valuecodetemplate_ed_typeoption.Mobile : Schema.Generated.ed_valuecodetemplate_ed_typeoption.Email,
                ed_ValidAt = response.couponDetails.template.coupon_templates_valid_at,
                ed_ValidDate = Convert.ToDateTime(response.couponDetails.template.coupon_templates_valid_date),
                ed_ValueCodeText = response.couponDetails.template.coupon_templates_coupon_text,
                ed_name = response.couponDetails.template.coupon_templates_name
            };


            template.Id = XrmHelper.Create(localContext.OrganizationService, template);

            return template;
        }


        #region DataContracts

        [DataContract]
        public class BaseValueCode
        {
            [DataMember]
            public int template { get; set; }
            [DataMember]
            public string api_token { get; set; }
            [DataMember]
            public float amount { get; set; }

        }

        [DataContract]
        public class ValueCodeCancelRequest
        {
            [DataMember]
            public string voucherId { get; set; }
            [DataMember]
            public string cancelledBy { get; set; }

        }

        [DataContract]
        [KnownType(typeof(Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.ValueCodeCouponVoucherServiceRequest))]
        public class ValueCodeCouponVoucherServiceRequest
        {
            [DataMember]
            public int amount { get; set; }
            [DataMember]
            public string contactAddress { get; set; }
            [DataMember]
            public int contactType { get; set; }
            [DataMember]
            public string tag { get; set; }
            [DataMember]
            public int travellerId { get; set; }
            [DataMember]
            public DateTime validFromDate { get; set; }
            [DataMember]
            public DateTime validToDate { get; set; }
            [DataMember]
            public int voucherType { get; set; }
            [DataMember]
            public string voucherText { get; set; }
            [DataMember]
            public string CreatedBy { get; set; }
        }


        [DataContract]
        public class ValueCodeCouponRequest : BaseValueCode
        {

            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string delivery_type { get; set; }
            [DataMember]
            public string clients_phone_number { get; set; }
            [DataMember]
            public string clients_email_address { get; set; }
            [DataMember]
            public int valid_days { get; set; }
            [DataMember]
            public string custom_text { get; set; }

            //[DataMember]
            //public string ticket_reference { get; set; }
            //[DataMember]
            //public string reason { get; set; }
            //[DataMember]
            //public string clients_first_name { get; set; }
            //[DataMember]
            //public string clients_last_name { get; set; }
            //[DataMember]
            //public int clients_countries_id { get; set; }
            //[DataMember]
            //public string custom_image { get; set; }
        }

        [DataContract]
        public class ValueCodeVoucherRequest : BaseValueCode
        {
            #region Required
            [DataMember]
            public string trans_id { get; set; }
            [DataMember]
            public string ki_store_id { get; set; }
            [DataMember]
            public string concept_id { get; set; }
            [DataMember]
            public string store_id { get; set; }
            [DataMember]
            public string country_code { get; set; }
            [DataMember]
            public string store_owner_id { get; set; }

            #endregion

            //[DataMember]
            //public string custom_identifier { get; set; }
        }

        [DataContract]
        public class SendValueCodeRequest
        {
            [DataMember]
            public int template { get; set; }
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public string delivery_type { get; set; }
            [DataMember]
            public string clients_phone_number { get; set; }
            [DataMember]
            public string clients_email_address { get; set; }
            [DataMember]
            public float amount { get; set; }
            [DataMember]
            public string api_token { get; set; }
            [DataMember]
            public int valid_days { get; set; }
            [DataMember]
            public string custom_text { get; set; }
        }

        #endregion


        [DataContract]
        public class VoucherCrm : ValueCodeVoucherServiceResponseMessage
        {
            [DataMember]
            public string voucherViewer { get; set; }
            [DataMember]
            public string voucherText { get; set; }
        }

        [DataContract]
        public class ValueCodeVoucherServiceResponseMessage
        {
            [DataMember]
            public decimal amount { get; set; }
            [DataMember]
            public string created { get; set; }
            [DataMember]
            public string tag { get; set; }

            //CK - De kommer skicka det till oss
            [DataMember]
            public string validFromDate { get; set; }
            //CK - De kommer skicka det till oss
            [DataMember]
            public string validToDate { get; set; }
            [DataMember]
            public string voucherCode { get; set; }
            [DataMember]
            public string voucherId { get; set; }
            [DataMember]
            public int? voucherType { get; set; }
            [DataMember]
            public decimal? remainingAmount { get; set; }
            [DataMember]
            public DateTime? disabled { get; set; }

            //CK - This has been changed
            [DataMember]
            public long eanCode { get; set; }
            [DataMember]
            public string redeemStoreId { get; set; }
        }


        public class ValueCodeResponseMessage
        {
            public string couponLink { get; set; }
            public Coupondetails couponDetails { get; set; }
        }

        public class Coupondetails
        {
            public int coupons_companies_id { get; set; }
            public int coupons_admins_id { get; set; }
            public int coupons_clients_id { get; set; }
            public int coupons_templates_id { get; set; }
            public object coupons_ticket_reference { get; set; }
            public int coupons_amount { get; set; }
            public string coupons_reason { get; set; }
            public string coupons_type { get; set; }
            public string coupons_delivery_type { get; set; }
            public string coupons_status { get; set; }
            public string coupons_sent { get; set; }
            public object coupons_custom_image { get; set; }
            public object coupons_custom_text { get; set; }
            public string coupons_ean { get; set; }
            public Coupons_Last_Redemption_Date coupons_last_redemption_date { get; set; }
            public string coupons_provider_unique_code { get; set; }
            public string coupons_created_timestamp { get; set; }
            public int coupons_id { get; set; }
            public string coupons_image { get; set; }
            public Company company { get; set; }
            public Template template { get; set; }
        }

        public class Coupons_Last_Redemption_Date
        {
            public string date { get; set; }
            public int timezone_type { get; set; }
            public string timezone { get; set; }
        }

        public class Company
        {
            public int companies_id { get; set; }
            public string companies_name { get; set; }
            public string companies_active { get; set; }
            public string companies_sms_sender { get; set; }
            public string companies_email_sender { get; set; }
            public string companies_api_postback_url { get; set; }
            public string companies_contact_email { get; set; }
            public object companies_custom_domain { get; set; }
        }

        public class Template
        {
            public int coupon_templates_id { get; set; }
            public int coupon_templates_companies_id { get; set; }
            public int coupon_templates_campaign_number { get; set; }
            public string coupon_templates_sms_sender { get; set; }
            public string coupon_templates_amount { get; set; }
            public string coupon_templates_valid_date { get; set; }
            public string coupon_templates_valid_at { get; set; }
            public string coupon_templates_name { get; set; }
            public string coupon_templates_description { get; set; }
            public string coupon_templates_coupon_email_text { get; set; }
            public string coupon_templates_coupon_sms_text { get; set; }
            public string coupon_templates_coupon_text { get; set; }
            public string coupon_templates_has_image { get; set; }
            public string coupon_templates_image_extension { get; set; }
            public string coupon_templates_has_mobile_image { get; set; }
            public string coupon_templates_mobile_image_extension { get; set; }
            public string coupon_templates_is_dynamic { get; set; }
            public string coupon_templates_type { get; set; }
            public string coupon_templates_active { get; set; }
            public string coupon_templates_email_sender { get; set; }
            public int coupon_templates_custom_image_is_allowed { get; set; }
            public int coupon_templates_custom_text_is_allowed { get; set; }
            public int coupon_templates_uses_qr_code { get; set; }
            public string coupon_templates_qr_text { get; set; }
        }

        public class BiztalkParseCardDetailsMessage
        {
            public string CardNumberField { get; set; }

            public string CardIssuerField { get; set; }

            public bool CardHotlistedField { get; set; }

            public int CardTypePeriodField { get; set; }

            public int CardTypeValueField { get; set; }

            public string CardValueProductTypeField { get; set; }

            public string CardKindField { get; set; }

            public string CardCategoryField { get; set; }

            public decimal BalanceField { get; set; }

            public string CurrencyField { get; set; }

            public bool OutstandingDirectedAutoloadField { get; set; }

            public bool OutstandingEnableThresholdAutoloadField { get; set; }

            public bool PurseHotlistedField { get; set; }

            public string PeriodCardCategoryField { get; set; }

            public string ProductTypeField { get; set; }

            public DateTime PeriodStartField { get; set; }

            public DateTime PeriodEndField { get; set; }

            public string WaitingPeriodsField { get; set; }

            public string ZoneListIDField { get; set; }

            public string ZonesListField { get; set; }

            public decimal PricePaidField { get; set; }

            public string ContractSerialNumberField { get; set; }

            public string PeriodCurrencyField { get; set; }

            public bool PeriodHotlistedField { get; set; }

            public bool PeriodOutstandingDirectedAutoloadField { get; set; }

            public bool PeriodOutstandingEnableThresholdAutoload { get; set; }

        }

        [DataContract]
        public class GetCardProperties
        {
            [DataMember(Name = "cardNumber")]
            public string CardNumber { get; set; }

            [DataMember(Name = "isClosed")]
            public bool IsClosed { get; set; }

            [DataMember(Name = "amount")]
            public decimal Amount { get; set; }

            [DataMember(Name = "closedReason")]
            public string ClosedReason { get; set; }

            [DataMember(Name = "isReserved")]
            public bool IsReserved { get; set; }

            [DataMember(Name = "isExpired")]
            public bool IsExpired { get; set; }

            [DataMember(Name = "lastTransactionDate")]
            public DateTime LastTransactionDate { get; set; }
        }

        /// <summary>
        /// Model for PurseDetails from Biztalk
        /// </summary>
        /// <see cref="CardDetails2PurseDetails"/>
        /// <seealso cref="http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"/>
        public class BiztalkPurseDetails
        {
            //byte
            public string CardCategoryField { get; set; }

            //decimal
            public decimal BalanceField { get; set; }

            //string
            public string CurrencyField { get; set; }

            //bool
            public bool OutstandingDirectedAutoloadField { get; set; }

            //bool
            public bool OutstandingEnableThresholdAutoloadField { get; set; }

            //bool
            public bool HotlistedField { get; set; }
        }

        /// <summary>
        /// Model for CardInformation from Biztalk
        /// </summary>
        /// <see cref="CardDetails2CardInformation"/>
        /// <seealso cref="http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"/>
        public class BiztalkCardInformation
        {
            //int
            public string CardNumberField { get; set; }

            //string
            public string CardIssuerField { get; set; }

            //bool
            public bool CardHotlistedField { get; set; }

            //ushort
            public int CardTypePeriodField { get; set; }

            //ushort
            public int CardTypeValueField { get; set; }

            //string
            public string CardValueProductTypeField { get; set; }

            //byte
            public string CardKindField { get; set; }
        }

        /// <summary>
        /// Model for PeriodDetails from Biztalk
        /// </summary>
        /// <see cref="CardDetails2PeriodDetails"/>
        /// <seealso cref="http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"/>
        public class BiztalkPeriodDetails
        {
            //byte
            public string CardCategoryField { get; set; }

            //string
            public string ProductTypeField { get; set; }

            //datetime
            public DateTime PeriodStartField { get; set; }

            //datetime
            public DateTime PeriodEndField { get; set; }
            
            //byte
            public string WaitingPeriodsField { get; set; }

            //byte
            public string ZoneListIDField { get; set; }

            //unit
            public int PricePaidField { get; set; }

            //string
            public string CurrencyField { get; set; }

            //bool
            public bool PeriodOutstandingDirectedAutoloadField { get; set; }

            //bool
            public bool PeriodOutstandingEnableThresholdAutoload { get; set; }

            //bool
            public bool HotlistedField { get; set; }

            //byte
            public string ContractSerialNumberField { get; set; }


        }

        /// <summary>
        /// Model for ZoneList from Biztalk
        /// </summary>
        /// <see cref="CardDetails2ZoneLists"/>
        /// <seealso cref="http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216"/>
        public class BiztalkZoneList
        {
            //byte
            public string ZoneListIdField { get; set; }

            //ushort
            public int ZoneField { get; set; }
        }

        //public class ValueCodeResponseMessage
        //{
        //    public string couponLink { get; set; }
        //    public Coupondetails couponDetails { get; set; }
        //}

        //public class Coupondetails
        //{
        //    public int coupons_companies_id { get; set; }
        //    public int coupons_admins_id { get; set; }
        //    public int coupons_clients_id { get; set; }
        //    public int coupons_templates_id { get; set; }
        //    public object coupons_ticket_reference { get; set; }
        //    public int coupons_amount { get; set; }
        //    public string coupons_reason { get; set; }
        //    public string coupons_type { get; set; }
        //    public string coupons_delivery_type { get; set; }
        //    public string coupons_status { get; set; }
        //    public string coupons_sent { get; set; }
        //    public object coupons_custom_image { get; set; }
        //    public object coupons_custom_text { get; set; }
        //    public string coupons_ean { get; set; }
        //    public Coupons_Last_Redemption_Date coupons_last_redemption_date { get; set; }
        //    public string coupons_provider_unique_code { get; set; }
        //    public string coupons_created_timestamp { get; set; }
        //    public int coupons_id { get; set; }
        //    public string coupons_image { get; set; }
        //    public Company company { get; set; }
        //    public Template template { get; set; }
        //}

        //public class Coupons_Last_Redemption_Date
        //{
        //    public string date { get; set; }
        //    public int timezone_type { get; set; }
        //    public string timezone { get; set; }
        //}

        //public class Company
        //{
        //    public int companies_id { get; set; }
        //    public string companies_name { get; set; }
        //    public string companies_active { get; set; }
        //    public string companies_sms_sender { get; set; }
        //    public string companies_api_key { get; set; }
        //    public string companies_email_sender { get; set; }
        //    public string companies_api_postback_url { get; set; }
        //    public string companies_contact_email { get; set; }
        //    public int companies_type { get; set; }
        //    public object companies_custom_domain { get; set; }
        //}

        //public class Template
        //{
        //    public int coupon_templates_id { get; set; }
        //    public int coupon_templates_companies_id { get; set; }
        //    public int coupon_templates_campaign_number { get; set; }
        //    public string coupon_templates_sms_sender { get; set; }
        //    public string coupon_templates_amount { get; set; }
        //    public string coupon_templates_valid_date { get; set; }
        //    public string coupon_templates_valid_at { get; set; }
        //    public string coupon_templates_name { get; set; }
        //    public string coupon_templates_description { get; set; }
        //    public string coupon_templates_coupon_email_text { get; set; }
        //    public string coupon_templates_coupon_sms_text { get; set; }
        //    public string coupon_templates_coupon_text { get; set; }
        //    public string coupon_templates_has_image { get; set; }
        //    public string coupon_templates_image_extension { get; set; }
        //    public string coupon_templates_has_mobile_image { get; set; }
        //    public string coupon_templates_mobile_image_extension { get; set; }
        //    public string coupon_templates_is_dynamic { get; set; }
        //    public string coupon_templates_type { get; set; }
        //    public string coupon_templates_active { get; set; }
        //    public string coupon_templates_email_sender { get; set; }
        //    public int coupon_templates_custom_image_is_allowed { get; set; }
        //    public int coupon_templates_custom_text_is_allowed { get; set; }
        //    public int coupon_templates_uses_qr_code { get; set; }
        //    public string coupon_templates_qr_text { get; set; }
        //}

    }
}
