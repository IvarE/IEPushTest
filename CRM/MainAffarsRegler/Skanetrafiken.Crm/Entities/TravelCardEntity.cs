using System;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using System.Net.Http;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.ValueCodes;
using System.Runtime.Serialization.Json;

namespace Skanetrafiken.Crm.Entities
{
    public class TravelCardEntity : Generated.cgi_travelcard
    {
        public static readonly string _NotDefined = "Ej Definierat";

        public enum BlockCardProductReasonCode
        {
            Lost = 1,
            FailureToPay = 2,
            Misuse = 3,
            HandlingError = 4,
            Other = 5
        }

        public static ValueCodeHandler.GetCardProperties HandlePlaceOrder(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            localContext.TracingService.Trace($"Running HandlePlaceOrder.");
            //TODO: Parse information from API to a GetCardProperties Object
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);

            CgiSettingEntity settingEntity = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI), settingFilter);

            if (settingEntity == null || string.IsNullOrWhiteSpace(settingEntity.ed_JojoCardDetailsAPI))
            {
                //Bad request -> exception
            }

            ValueCodeHandler.GetCardProperties getCardProperties = null;

            localContext.TracingService.Trace("\nJojoAPITest - Place Order:");
            WebRequest request = WebRequest.Create(string.Format("{0}placeOrder", settingEntity.ed_JojoCardDetailsAPI));

            request.Headers.Add("Card-Number", cardNumber);
            request.Headers.Add("20", "*/*");
            request.ContentLength = 0;
            request.Method = "POST";

            using (WebResponse placeOrderResponse = request.GetResponse())
            {
                var checkStatus = (HttpWebResponse)placeOrderResponse;
                if (checkStatus.StatusCode != HttpStatusCode.OK)
                {
                    //Bad Request
                }
                else
                {
                    using (Stream stream = placeOrderResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                        {

                            var jsonResponse = streamReader.ReadToEnd();

                            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ValueCodeHandler.GetCardProperties));
                            MemoryStream streamMemory = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
                            streamMemory.Position = 0;

                            getCardProperties = (ValueCodeHandler.GetCardProperties)jsonSerializer.ReadObject(streamMemory);
                            streamMemory.Close();

                            if (getCardProperties != null)
                            {
                                //OK
                            }
                            else
                            {

                            }
                        }
                    }
                }
            }

            localContext.TracingService.Trace($"Successfully exiting HandlePlaceOrder.");
            return getCardProperties;
        }

        public static string HandleCaptureOrder(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            localContext.TracingService.Trace($"Running HandleCaptureOrder.");
            //TODO: Parse information from API to a Status String (200 - Success / 400 - Bad Request, error message)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);

            CgiSettingEntity settingEntity = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI), settingFilter);

            localContext.TracingService.Trace("\nJojoAPITest - Capture Order:");
            string apiStatusResponse = "";

            WebRequest requestCaptureOrder = WebRequest.Create(string.Format("{0}captureOrder/", settingEntity.ed_JojoCardDetailsAPI));

            requestCaptureOrder.Headers.Add("Card-Number", cardNumber);
            requestCaptureOrder.Headers.Add("20", "*/*");
            requestCaptureOrder.ContentLength = 0;
            requestCaptureOrder.Method = "POST";

            var captureOrderResponse = requestCaptureOrder.GetResponse() as HttpWebResponse;

            if (captureOrderResponse.StatusCode != HttpStatusCode.OK)
            {
                //Send bad request
                apiStatusResponse = "400 - Fel";
            }
            else
            {
                //We are done
                apiStatusResponse = "200 - Success";
            }

            localContext.TracingService.Trace($"Successfully exiting HandleCaptureOrder.");
            
            return apiStatusResponse;
        }

        public static ValueCodeHandler.GetCardProperties HandleGetCard(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            localContext.TracingService.Trace($"Running HandleGetCard.");
            //TODO: Parse information from API to a GetCardProperties Object
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);

            CgiSettingEntity settingEntity = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI), settingFilter);

            if (settingEntity == null || string.IsNullOrWhiteSpace(settingEntity.ed_JojoCardDetailsAPI))
            {
                //Bad request -> exception
            }

            ValueCodeHandler.GetCardProperties getCardProperties = null;
            string message = "";
            localContext.TracingService.Trace("\nJojoAPITest - get Card:");

            var getCardRequest = WebRequest.Create(string.Format("{0}card/", settingEntity.ed_JojoCardDetailsAPI));

            getCardRequest.Headers.Add("Card-Number", cardNumber);
            getCardRequest.Headers.Add("20", "*/*");

            getCardRequest.Method = "GET";

            using (WebResponse getCardResponse = getCardRequest.GetResponse())
            {
                var checkStatus = (HttpWebResponse)getCardResponse;
                if (checkStatus.StatusCode != HttpStatusCode.OK)
                {
                    //Bad request -> exception
                }
                else
                {
                    using (Stream stream = getCardResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                        {

                            var jsonResponse = streamReader.ReadToEnd();

                            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ValueCodeHandler.GetCardProperties));
                            MemoryStream streamMemory = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
                            streamMemory.Position = 0;

                            getCardProperties = (ValueCodeHandler.GetCardProperties)jsonSerializer.ReadObject(streamMemory);
                            streamMemory.Close();
                            if (getCardProperties != null)
                            {
                                //Return this information
                                message = "Found information!";
                            }
                            else
                            {
                                //bad
                                message = "Did not find information!";
                            }
                        }
                    }
                }
            }

            localContext.TracingService.Trace($"Successfully exiting HandleGetCard, {message}");
            return getCardProperties;
        }

        public static string BlockCardBiztalk(Plugin.LocalPluginContext localContext, string travelCardNumber, int reasonCode)
        {
            localContext.TracingService.Trace($"Running InvalidateTravelCard.");
            var soapmessage = $"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK004/RequestCardBlock/20141216'>"
               + "<soapenv:Header/>" +
                    "<soapenv:Body>" +
                        "<ns:RequestCardBlock>" +
                            $"<CardSerialNumber>{travelCardNumber}</CardSerialNumber>" +
                            $"<ReasonCode>{(int)reasonCode}</ReasonCode>" +
                        "</ns:RequestCardBlock>" +
                    "</soapenv:Body>" +
                "</soapenv:Envelope>";


            var bizTalkUrl = string.Empty;
            string soapResponse = string.Empty;

            try
            {
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkBlockTravelCard);
                if (string.IsNullOrWhiteSpace(bizTalkUrl))
                    throw new Exception("No value in field. Check setting.");
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            string[] parts = bizTalkUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < parts.Length - 1; x++)
            {
                soapActionAddress += parts[x];
                soapActionAddress += "/";
            }

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");
                    localContext.TracingService.Trace($"Sending soap message.");
                    soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(resp, ex);
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }

            localContext.TracingService.Trace($"Successfully exiting InvalidateTravelCard.");
            return soapResponse;
        }

        public static string InvalidateTravelCard(Plugin.LocalPluginContext localContext, string travelCardNumber, BlockCardProductReasonCode reasonCode)
        {
            localContext.TracingService.Trace($"Running InvalidateTravelCard.");
            var soapmessage = $"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK004/RequestCardBlock/20141216'>"
               + "<soapenv:Header/>" +
                    "<soapenv:Body>" +
                        "<ns:RequestCardBlock>" +
                            $"<CardSerialNumber>{travelCardNumber}</CardSerialNumber>" +
                            $"<ReasonCode>{(int)reasonCode}</ReasonCode>" +
                        "</ns:RequestCardBlock>" +
                    "</soapenv:Body>" +
                "</soapenv:Envelope>";


            var bizTalkUrl = string.Empty;
            string soapResponse = string.Empty;

            try
            {
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkBlockTravelCard);
                if (string.IsNullOrWhiteSpace(bizTalkUrl))
                    throw new Exception("No value in field. Check setting.");
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            string[] parts = bizTalkUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < parts.Length - 1; x++)
            {
                soapActionAddress += parts[x];
                soapActionAddress += "/";
            }

            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");
                    localContext.TracingService.Trace($"Sending soap message.");
                    soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(resp, ex);
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }

            localContext.TracingService.Trace($"Successfully exiting InvalidateTravelCard.");
            return soapResponse;
        }

        public static string GetCardDetails(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            try
            {
                localContext.TracingService.Trace($"Setting up soap message.");
                // MAKE SOAP REQUEST
                string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2/20141216'>" +
                "<soapenv:Header/>" +
                    "<soapenv:Body>" +
                        "<int:GetCardDetails2>" +
                            "<int:CardSerialNumber>" + travelCardNumber + "</int:CardSerialNumber>" +
                        "</int:GetCardDetails2>" +
                    "</soapenv:Body>" +
                "</soapenv:Envelope>";

                localContext.TracingService.Trace($"SoapMessage Envelope to be sent: " + soapmessage);

                string soapResponse = "";
                string bizTalkUrl = "";

                //TRY GET SERVICE URL
                try
                {
                    localContext.TracingService.Trace($"Getting url to BizTalk...");
                    bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkGetCardDetailsService);
                    localContext.TracingService.Trace($"Successfully retrieve Biztalk url: " + bizTalkUrl);
                }
                catch (Exception ex)
                {
                    localContext.TracingService.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                    throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
                }

                // REMOVE SUFFIX OF URL
                string[] parts = bizTalkUrl.Split('/');
                string soapActionAddress = "";
                for (int x = 0; x < parts.Length - 1; x++)
                {
                    soapActionAddress += parts[x];
                    soapActionAddress += "/";
                }

                localContext.TracingService.Trace($"Soap action address for BizTalk: " + soapActionAddress);

                //TRY SEND REQUEST
                try
                {


                    using (var client = new System.Net.WebClient())
                    {
                        // the Content-Type needs to be set to XML
                        client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                        client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                        client.Encoding = Encoding.UTF8;
                        //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");

                        localContext.TracingService.Trace($"Send soap message to biztalk");
                        soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                    }
                }
                catch (WebException ex)
                {
                    localContext.TracingService.Trace("Error while making call to BizTalk. Ex: " + ex.Message);
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    throw new Exception(resp, ex);
                }
                catch (Exception ex)
                {
                    localContext.TracingService.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                    throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
                }

                localContext.TracingService.Trace("Returning CardDetails from soap response: " + soapResponse);
                return soapResponse;
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace("Error in GetCardDetails. Ex: " + ex.Message);
                throw new Exception($"An unexpected error occurred when contacting BizTalk service: {ex.Message}", ex);
            }

        }

        public static CardDetailsEnvelope.Envelope GetAndParseCardDetails(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            localContext.TracingService.Trace("Inside GetAndParseCardDetails...");
            return ParseCardDetails(localContext, GetCardDetails(localContext, travelCardNumber));
        }

        public static CardBlockEnvelope.Envelope InvalidateCardAndGetResponse(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            return ParseCardBlockResponse(InvalidateTravelCard(localContext, travelCardNumber, BlockCardProductReasonCode.Other));
        }


        public static HttpResponseMessage ValidateCardBlockFromBiztalk(Plugin.LocalPluginContext localContext, int threadId, string cardBlockResponse)
        {
            localContext.TracingService.Trace($"TravelCardEntity.ValidateCardBlockFromBiztalk: Entering method.");

            if (cardBlockResponse == null || cardBlockResponse == "")
            {
                return ReturnApiMessage(threadId,
                        "Kunde inte spärra kortet.",
                        HttpStatusCode.BadRequest);
            }

            if (cardBlockResponse.Equals("-1"))
            {
                return ReturnApiMessage(threadId,
                        "Resekortet är redan spärrat eller håller på att spärras.",
                        HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Obsolete]
        /// <summary>
        /// Check if blocking status.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="cardBlock"></param>

        public static void ValidateCardBlockResult(Plugin.LocalPluginContext localContext, CardBlockEnvelope.Envelope cardBlock)
        {
            localContext.TracingService.Trace($"Running ValidateCardBlockResult.");
            if (cardBlock.Body == null ||
                cardBlock.Body.RequestCardBlockResponse == null)
                throw new Exception("Unexpected error from BIFF.");

            var cardBlockResult = cardBlock.Body.RequestCardBlockResponse.RequestCardBlockResult;

            if (cardBlockResult == -1)
                throw new Exception($"Travel card is already blocked or is in progress to be blocked. ErrorCode: {cardBlockResult}");

            localContext.TracingService.Trace($"Successfully exiting ValidateCardBlockResult");
        }

        public static TravelCardEntity GetCardFromCardNumber(Plugin.LocalPluginContext localContext, string travelCardNumber, ColumnSet columns)
        {
            FilterExpression findCard = new FilterExpression(LogicalOperator.And);
            findCard.AddCondition(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, travelCardNumber);
            return XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, columns, findCard);
        }

        /// <summary>
        /// Return a travel card
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="columns"></param>
        /// <param name="CVC"></param>
        /// <returns></returns>
        /// <remarks>If LinkedEntities cannot find any linked ContactEntity then this method will return null.</remarks>
        public static TravelCardEntity GetCardAndContactFromCardNumber(Plugin.LocalPluginContext localContext, string travelCardNumber, ColumnSet columns)
        {
            localContext.TracingService.Trace($"Running GetCardAndContactFromCardNumber");
            QueryExpression travelCardQuery = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = columns,
                Criteria =
                {
                    Conditions = {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, travelCardNumber),
                        new ConditionExpression(TravelCardEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_travelcardState.Active)
                    }
                },

                LinkEntities =
                {
                    new LinkEntity()
                    {
                        JoinOperator = JoinOperator.LeftOuter,
                        LinkFromAttributeName = TravelCardEntity.Fields.cgi_Contactid,
                        LinkFromEntityName = TravelCardEntity.EntityLogicalName,
                        LinkToAttributeName = ContactEntity.Fields.ContactId,
                        LinkToEntityName = ContactEntity.EntityLogicalName,
                        Columns = new ColumnSet(ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName)
                    }
                }
            };

            TravelCardEntity travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardQuery);
            localContext.TracingService.Trace($"Exiting GetCardAndContactFromCardNumber");
            return travelCard;
        }



        /// <summary>
        /// Returns a TravelCardEntity
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="columnSet"></param>
        /// <param name="conditions"></param>
        public static TravelCardEntity GetTravelCard(Plugin.LocalPluginContext localContext, ColumnSet columnSet, params ConditionExpression[] conditions)
        {
            var filter = new FilterExpression();
            foreach (var condition in conditions)
                filter.AddCondition(condition);

            var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, TravelCardEntity.EntityLogicalName, columnSet, filter);

            return travelCard;
        }

        public static List<TravelCardEntity> GetTravelCards_ByAccount(Plugin.LocalPluginContext localContext, Guid parentId, ColumnSet column = null, FilterExpression filter = null)
        {
            var filterTravelCards = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = column != null ? column :
                new ColumnSet(
                    TravelCardEntity.Fields.cgi_travelcardnumber,
                    TravelCardEntity.Fields.cgi_TravelCardCVC,
                    TravelCardEntity.Fields.cgi_Accountid,
                    TravelCardEntity.Fields.cgi_Blocked
                    ),
                Criteria = filter != null ? filter : new FilterExpression 
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.Id, ConditionOperator.Equal, parentId)
                    }
                }
            };

            var travelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, filterTravelCards);
            return travelCards;
        }

        /** 
         * 
         * CardDetails2 from external service
         * 
         * **/

        public void ValidateTravelCard(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            ValidateTravelCard(localContext, TravelCardEntity.GetAndParseCardDetails(localContext, cardNumber));
        }

        public static void ValidateTravelCardFromBiztalk(Plugin.LocalPluginContext localContext, CardDetailsEnvelope.Envelope cardDetails)
        {
            if (cardDetails.Body == null ||
                    cardDetails.Body.GetCardDetails2Response == null ||
                    cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result == null ||
                    cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2 == null)
            {
                localContext.TracingService.Trace($"Oväntat svar från BizTalkGetCardDetails.");
                throw new Exception("Oväntat svar från BizTalkGetCardDetails.");
            }

            if (cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation == null &&
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails == null &&
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails == null)
            {
                localContext.TracingService.Trace($"Resekortet finns inte i systemet.");
                throw new Exception("Resekortet finns inte i systemet.");
            }

            var CardInformation = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation;
            var PurseDetails = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails;

            if (cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails != null)
            {
                var PeriodDetails = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails;

                //Räknar med tid eller bara datum?
                #region PeriodDetails
                if (PeriodDetails.PeriodEnd != null && PeriodDetails.PeriodEnd > DateTime.UtcNow)
                {
                    localContext.TracingService.Trace($"Resekortet har en aktiv period.");
                    throw new Exception("Resekortet har en aktiv period.");
                }
                #endregion
            }

            #region CardInformation
            if (CardInformation.CardHotlisted == true)
            {
                localContext.TracingService.Trace($"Kortet är spärrat.");
                throw new Exception("Kortet är spärrat.");
            }
            #endregion

        }

        private static HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {
            HttpResponseMessage response = new HttpResponseMessage(code);
            response.Content = new StringContent(errorMessage);
            return response;
        }

        public static HttpResponseMessage ValidateCardDetailsFromBiztalkForPresentkort(Plugin.LocalPluginContext localContext, int threadId, HttpResponseMessage response, BiztalkParseCardDetailsMessage cardDetails)
        {
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if (cardDetails.CardHotlistedField == true)
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked),
                        HttpStatusCode.BadRequest);
            }

            //If there is an incoming charge of purse or period
            if (cardDetails.OutstandingDirectedAutoloadField || cardDetails.PeriodOutstandingDirectedAutoloadField)
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_CouldNotBlockCard),
                        HttpStatusCode.BadRequest);
            }

            //For presentkort, user shall not be able to block an active period.
            if(cardDetails.PeriodEndField != DateTime.MinValue && cardDetails.PeriodEndField > DateTime.Now)
            {
                return response = ReturnApiMessage(threadId,
                       ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardHasActivePeriod),
                       HttpStatusCode.BadRequest);
            }


            return response = new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Blocks travel card dispite balance or existing periods.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="response"></param>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        public static HttpResponseMessage ValidateCardDetailsFromBiztalkForBlockTravelCard(Plugin.LocalPluginContext localContext, int threadId, HttpResponseMessage response, BiztalkParseCardDetailsMessage cardDetails)
        {
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if (cardDetails.CardHotlistedField == true)
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked),
                        HttpStatusCode.Continue);
            }

            return response = new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Validates biff and crm travel card.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="response"></param>
        /// <param name="cardDetails"></param>
        /// <param name="crmTravelCard"></param>
        /// <param name="maximumAmount"></param>
        /// <param name="travelCardNr"></param>
        /// <param name="cvc"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static HttpResponseMessage ValidateCardDetailsFromBiztalkForCompensation(Plugin.LocalPluginContext localContext, int threadId, HttpResponseMessage response,
            ref BiztalkParseCardDetailsMessage cardDetails, ref TravelCardEntity crmTravelCard, int maximumAmount,
            string travelCardNr = null, string cvc = null, Guid? contactId = null)
        {
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if(maximumAmount < cardDetails.BalanceField)
            {
                return response = ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AboveMaxAmount),
                   HttpStatusCode.BadRequest);
            }

            if(cardDetails.PeriodEndField.Date < DateTime.Now.Date && cardDetails.BalanceField <= 0)
            {
                return response = ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoAmountOnTravelCard),
                   HttpStatusCode.BadRequest);
            }

            //If travel card is null, we need to create a new one.
            if (crmTravelCard == null)
            {
                var newTravelCard = new TravelCardEntity()
                {
                    cgi_travelcardnumber = travelCardNr,
                    cgi_TravelCardCVC = cvc,
                    cgi_Blocked = true,
                    ed_RequestedValueCodeForCard = false
                };

                if (contactId.HasValue)
                {
                    newTravelCard.cgi_Contactid = new Microsoft.Xrm.Sdk.EntityReference(ContactEntity.EntityLogicalName, contactId.Value);
                }

                crmTravelCard = newTravelCard;
                crmTravelCard.Id = XrmHelper.Create(localContext, newTravelCard);
            }

            else if (crmTravelCard.cgi_Contactid == null)
            {
                var updateContactTravelCard = new TravelCardEntity() {
                    Id = crmTravelCard.Id,
                    cgi_Contactid = new Microsoft.Xrm.Sdk.EntityReference(ContactEntity.EntityLogicalName, contactId.Value)
                };
                XrmHelper.Update(localContext, updateContactTravelCard);
            }

            //This check is to ensure that no user cannot request value code without travel card being blocked in CRM.
            //Blocked travel card in CRM indicates that it also has been blocked in Biff.
            //Which means if anyone tries to make a call to this api-endpoint without reaching BlockTravelCard first, he/she won't be able to request a value code.
            if (crmTravelCard.cgi_Blocked == null || !crmTravelCard.cgi_Blocked.Value) //Treat null as false
            {
                localContext.TracingService.Trace($"User tried to call this api (CreateValueCodeLossCompensation) without calling BlockTravelCard first.");
                return response = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNotBlockedYet),
                    HttpStatusCode.BadRequest);
            }
            else if (crmTravelCard.cgi_Blocked.Value && (crmTravelCard.ed_RequestedValueCodeForCard == null || !crmTravelCard.ed_RequestedValueCodeForCard.Value))
            {
                return response = ReturnApiMessage(threadId,
                    "",
                    HttpStatusCode.OK);
            }
            //This is for if user wants to resend value code.
            else if (crmTravelCard.cgi_Blocked.Value && crmTravelCard.ed_RequestedValueCodeForCard.Value)
            {
                return response = ReturnApiMessage(threadId, "", HttpStatusCode.Found);
            }
            else
            {
                localContext.TracingService.Trace($"User tried to call this api (CreateValueCodeLossCompensation) without calling BlockTravelCard first.");
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Throws exception if card is not valid
        /// </summary>
        public static void ValidateTravelCard(Plugin.LocalPluginContext localContext, CardDetailsEnvelope.Envelope cardDetails)
        {
            localContext.TracingService.Trace($"Running ValidateTravelCard.");

            //try
            //{
            if (cardDetails.Body == null ||
                cardDetails.Body.GetCardDetails2Response == null ||
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result == null ||
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2 == null)
            {
                localContext.TracingService.Trace($"Oväntat svar från BizTalkGetCardDetails.");
                throw new Exception("Oväntat svar från BizTalkGetCardDetails.");
            }

            if (cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation == null &&
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails == null &&
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails == null)
            {
                localContext.TracingService.Trace($"Resekortet finns inte i systemet.");
                throw new Exception("Resekortet finns inte i systemet.");
            }

            var CardInformation = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.CardInformation;
            var PurseDetails = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails;

            if (cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails != null)
            {
                var PeriodDetails = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PeriodDetails;

                //Räknar med tid eller bara datum?
                #region PeriodDetails
                if (PeriodDetails.PeriodEnd != null && PeriodDetails.PeriodEnd > DateTime.UtcNow)
                {
                    localContext.TracingService.Trace($"Resekortet har en aktiv period.");
                    throw new Exception("Resekortet har en aktiv period.");
                }
                #endregion
            }

            #region CardInformation
            if (CardInformation.CardHotlisted == true)
            {
                localContext.TracingService.Trace($"Kortet är spärrat.");
                throw new Exception("Kortet är spärrat.");
            }
            #endregion

            #region PurseDetails
            if (PurseDetails.Hotlisted == true)
            {
                localContext.TracingService.Trace($"Reskassa är spärrad.");
                throw new Exception("Reskassa är spärrad.");
            }

            if (PurseDetails.Balance == 0)
            {
                localContext.TracingService.Trace($"Kortets reskassa saknar värde.");
                throw new Exception("Kortets reskassa saknar värde.");
            }

            #endregion


            //}
            //catch(Exception ex)
            //{
            //    localContext.TracingService.Trace($"Unexpected error on ValidateValueCodeApproval. Ex: { ex.Message }");
            //}
            localContext.TracingService.Trace($"Successfully exiting ValidateTravelCard");

        }

        public static decimal GetCardBalance(Plugin.LocalPluginContext localContext, CardDetailsEnvelope.Envelope cardDetails)
        {
            if (cardDetails.Body == null ||
                cardDetails.Body.GetCardDetails2Response == null ||
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result == null ||
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2 == null ||
                cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails == null)
                throw new Exception("Oväntat svar från BizTalkGetCardDetails");

            var PurseDetails = cardDetails.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails;

            if (PurseDetails.Balance <= 0)
                throw new Exception("Kortets reskassa saknar värde.");

            return PurseDetails.Balance;

        }

        public static CardDetailsEnvelope.Envelope ParseCardDetails(Plugin.LocalPluginContext localContext, string soapResponse)
        {

            if (localContext == null)
                throw new Exception("ParseCardDetails: localContext is null.");

            if (localContext.TracingService == null)
                throw new Exception("ParseCardDetails: TracingService is null.");
            //localContext.TracingService.Trace("Inside ParseCardDetails...");

            try
            {

                XmlSerializer serializer = new XmlSerializer(typeof(CardDetailsEnvelope.Envelope));


                if (serializer == null)
                    throw new Exception("ParseCardDetails: XmlSerializer is null.");

                if (string.IsNullOrWhiteSpace(soapResponse))
                    throw new Exception("ParseCardDetails: soapResponse is null.");
                StringReader strReader = new StringReader(soapResponse);

                if (strReader == null)
                    throw new Exception("ParseCardDetails: StringReader is null.");

                CardDetailsEnvelope.Envelope envelope = (CardDetailsEnvelope.Envelope)serializer.Deserialize(strReader);

                return envelope;


            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace("Error from ParseCardDetails. Ex: " + ex.Message);
                throw new Exception($"(ParseCardDetails) error: {ex}");
            }
        }

        public static CardBlockEnvelope.Envelope ParseCardBlockResponse(string soapResponse)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CardBlockEnvelope.Envelope));
                StringReader strReader = new StringReader(soapResponse);
                CardBlockEnvelope.Envelope envelope = (CardBlockEnvelope.Envelope)serializer.Deserialize(strReader);
                return envelope;
            }
            catch (Exception ex)
            {
                throw new Exception($"(ParseCardBlockResponse) error: {ex}");
            }
        }
        /// <summary>
        /// Fetches value code approval with associated travel card.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        public static ValueCodeApprovalEntity FetchApproval(Plugin.LocalPluginContext localContext, string travelCardNumber, ColumnSet columnSet)
        {
            var queryApproval = new QueryExpression()
            {
                EntityName = ValueCodeApprovalEntity.EntityLogicalName,
                ColumnSet = columnSet,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeApprovalEntity.Fields.ed_TravelCardNumber, ConditionOperator.Equal, travelCardNumber)
                    }
                }
            };

            var approval = XrmRetrieveHelper.RetrieveFirst<ValueCodeApprovalEntity>(localContext, queryApproval);
            return approval;
        }


        public class CardDetailsEnvelope
        {

            // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
            public partial class Envelope
            {

                private EnvelopeBody bodyField;

                /// <remarks/>
                public EnvelopeBody Body
                {
                    get
                    {
                        return this.bodyField;
                    }
                    set
                    {
                        this.bodyField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public partial class EnvelopeBody
            {

                private GetCardDetails2Response getCardDetails2ResponseField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216")]
                public GetCardDetails2Response GetCardDetails2Response
                {
                    get
                    {
                        return this.getCardDetails2ResponseField;
                    }
                    set
                    {
                        this.getCardDetails2ResponseField = value;
                    }
                }
            }



            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216", IsNullable = false)]
            public partial class GetCardDetails2Response
            {

                private GetCardDetails2ResponseGetCardDetails2Result getCardDetails2ResultField;

                /// <remarks/>
                public GetCardDetails2ResponseGetCardDetails2Result GetCardDetails2Result
                {
                    get
                    {
                        return this.getCardDetails2ResultField;
                    }
                    set
                    {
                        this.getCardDetails2ResultField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216")]
            public partial class GetCardDetails2ResponseGetCardDetails2Result
            {

                private CardDetails2 cardDetails2Field;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
                public CardDetails2 CardDetails2
                {
                    get
                    {
                        return this.cardDetails2Field;
                    }
                    set
                    {
                        this.cardDetails2Field = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216", IsNullable = false)]
            public partial class CardDetails2
            {

                private CardDetails2CardInformation cardInformationField;

                private CardDetails2PurseDetails purseDetailsField;

                private CardDetails2PeriodDetails periodDetailsField;

                private CardDetails2ZoneLists[] zoneListsField;

                /// <remarks/>
                public CardDetails2CardInformation CardInformation
                {
                    get
                    {
                        return this.cardInformationField;
                    }
                    set
                    {
                        this.cardInformationField = value;
                    }
                }

                /// <remarks/>
                public CardDetails2PurseDetails PurseDetails
                {
                    get
                    {
                        return this.purseDetailsField;
                    }
                    set
                    {
                        this.purseDetailsField = value;
                    }
                }

                /// <remarks/>
                public CardDetails2PeriodDetails PeriodDetails
                {
                    get
                    {
                        return this.periodDetailsField;
                    }
                    set
                    {
                        this.periodDetailsField = value;
                    }
                }

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute("ZoneLists")]
                public CardDetails2ZoneLists[] ZoneLists
                {
                    get
                    {
                        return this.zoneListsField;
                    }
                    set
                    {
                        this.zoneListsField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
            public partial class CardDetails2CardInformation
            {

                private uint cardNumberField;

                private string cardIssuerField;

                private byte cardKindField;

                private bool cardHotlistedField;

                private ushort cardTypePeriodField;

                private ushort cardTypeValueField;

                private string cardValueProductTypeField;

                /// <remarks/>
                public uint CardNumber
                {
                    get
                    {
                        return this.cardNumberField;
                    }
                    set
                    {
                        this.cardNumberField = value;
                    }
                }

                /// <remarks/>
                public string CardIssuer
                {
                    get
                    {
                        return this.cardIssuerField;
                    }
                    set
                    {
                        this.cardIssuerField = value;
                    }
                }

                /// <remarks/>
                public byte CardKind
                {
                    get
                    {
                        return this.cardKindField;
                    }
                    set
                    {
                        this.cardKindField = value;
                    }
                }

                /// <remarks/>
                public bool CardHotlisted
                {
                    get
                    {
                        return this.cardHotlistedField;
                    }
                    set
                    {
                        this.cardHotlistedField = value;
                    }
                }

                /// <remarks/>
                public ushort CardTypePeriod
                {
                    get
                    {
                        return this.cardTypePeriodField;
                    }
                    set
                    {
                        this.cardTypePeriodField = value;
                    }
                }

                /// <remarks/>
                public ushort CardTypeValue
                {
                    get
                    {
                        return this.cardTypeValueField;
                    }
                    set
                    {
                        this.cardTypeValueField = value;
                    }
                }

                /// <remarks/>
                public string CardValueProductType
                {
                    get
                    {
                        return this.cardValueProductTypeField;
                    }
                    set
                    {
                        this.cardValueProductTypeField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
            public partial class CardDetails2PurseDetails
            {

                private byte cardCategoryField;

                private decimal balanceField;

                private string currencyField;

                private bool outstandingDirectedAutoloadField;

                private bool outstandingEnableThresholdAutoloadField;

                private bool hotlistedField;

                /// <remarks/>
                public byte CardCategory
                {
                    get
                    {
                        return this.cardCategoryField;
                    }
                    set
                    {
                        this.cardCategoryField = value;
                    }
                }

                /// <remarks/>
                public decimal Balance
                {
                    get
                    {
                        return this.balanceField;
                    }
                    set
                    {
                        this.balanceField = value;
                    }
                }

                /// <remarks/>
                public string Currency
                {
                    get
                    {
                        return this.currencyField;
                    }
                    set
                    {
                        this.currencyField = value;
                    }
                }

                /// <remarks/>
                public bool OutstandingDirectedAutoload
                {
                    get
                    {
                        return this.outstandingDirectedAutoloadField;
                    }
                    set
                    {
                        this.outstandingDirectedAutoloadField = value;
                    }
                }

                /// <remarks/>
                public bool OutstandingEnableThresholdAutoload
                {
                    get
                    {
                        return this.outstandingEnableThresholdAutoloadField;
                    }
                    set
                    {
                        this.outstandingEnableThresholdAutoloadField = value;
                    }
                }

                /// <remarks/>
                public bool Hotlisted
                {
                    get
                    {
                        return this.hotlistedField;
                    }
                    set
                    {
                        this.hotlistedField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
            public partial class CardDetails2PeriodDetails
            {

                private byte cardCategoryField;

                private string productTypeField;

                private System.DateTime periodStartField;

                private System.DateTime periodEndField;

                private byte waitingPeriodsField;

                private byte zoneListIDField;

                private decimal pricePaidField;

                private string currencyField;

                private bool outstandingDirectedAutoloadField;

                private bool outstandingEnableThresholdAutoloadField;

                private bool hotlistedField;

                private byte contractSerialNumberField;

                /// <remarks/>
                public byte CardCategory
                {
                    get
                    {
                        return this.cardCategoryField;
                    }
                    set
                    {
                        this.cardCategoryField = value;
                    }
                }

                /// <remarks/>
                public string ProductType
                {
                    get
                    {
                        return this.productTypeField;
                    }
                    set
                    {
                        this.productTypeField = value;
                    }
                }

                /// <remarks/>
                public System.DateTime PeriodStart
                {
                    get
                    {
                        return this.periodStartField;
                    }
                    set
                    {
                        this.periodStartField = value;
                    }
                }

                /// <remarks/>
                public System.DateTime PeriodEnd
                {
                    get
                    {
                        return this.periodEndField;
                    }
                    set
                    {
                        this.periodEndField = value;
                    }
                }

                /// <remarks/>
                public byte WaitingPeriods
                {
                    get
                    {
                        return this.waitingPeriodsField;
                    }
                    set
                    {
                        this.waitingPeriodsField = value;
                    }
                }

                /// <remarks/>
                public byte ZoneListID
                {
                    get
                    {
                        return this.zoneListIDField;
                    }
                    set
                    {
                        this.zoneListIDField = value;
                    }
                }

                /// <remarks/>
                public decimal PricePaid
                {
                    get
                    {
                        return this.pricePaidField;
                    }
                    set
                    {
                        this.pricePaidField = value;
                    }
                }

                /// <remarks/>
                public string Currency
                {
                    get
                    {
                        return this.currencyField;
                    }
                    set
                    {
                        this.currencyField = value;
                    }
                }

                /// <remarks/>
                public bool OutstandingDirectedAutoload
                {
                    get
                    {
                        return this.outstandingDirectedAutoloadField;
                    }
                    set
                    {
                        this.outstandingDirectedAutoloadField = value;
                    }
                }

                /// <remarks/>
                public bool OutstandingEnableThresholdAutoload
                {
                    get
                    {
                        return this.outstandingEnableThresholdAutoloadField;
                    }
                    set
                    {
                        this.outstandingEnableThresholdAutoloadField = value;
                    }
                }

                /// <remarks/>
                public bool Hotlisted
                {
                    get
                    {
                        return this.hotlistedField;
                    }
                    set
                    {
                        this.hotlistedField = value;
                    }
                }

                /// <remarks/>
                public byte ContractSerialNumber
                {
                    get
                    {
                        return this.contractSerialNumberField;
                    }
                    set
                    {
                        this.contractSerialNumberField = value;
                    }
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216")]
            public partial class CardDetails2ZoneLists
            {

                private byte zoneListIDField;

                private ushort zoneField;

                /// <remarks/>
                public byte ZoneListID
                {
                    get
                    {
                        return this.zoneListIDField;
                    }
                    set
                    {
                        this.zoneListIDField = value;
                    }
                }

                /// <remarks/>
                public ushort Zone
                {
                    get
                    {
                        return this.zoneField;
                    }
                    set
                    {
                        this.zoneField = value;
                    }
                }
            }
        }

        public class CardBlockEnvelope
        {

            // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
            /// <remarks/>
            [System.SerializableAttribute()]
            [System.ComponentModel.DesignerCategoryAttribute("code")]
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
            public partial class Envelope
            {

                private EnvelopeBody bodyField;

                /// <remarks/>
                public EnvelopeBody Body
                {
                    get
                    {
                        return this.bodyField;
                    }
                    set
                    {
                        this.bodyField = value;
                    }
                }
            }

            /// <remarks/>
            [System.SerializableAttribute()]
            [System.ComponentModel.DesignerCategoryAttribute("code")]
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public partial class EnvelopeBody
            {

                private RequestCardBlockResponse requestCardBlockResponseField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/RequestCardBlock/20141216")]
                public RequestCardBlockResponse RequestCardBlockResponse
                {
                    get
                    {
                        return this.requestCardBlockResponseField;
                    }
                    set
                    {
                        this.requestCardBlockResponseField = value;
                    }
                }
            }

            /// <remarks/>
            [System.SerializableAttribute()]
            [System.ComponentModel.DesignerCategoryAttribute("code")]
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/RequestCardBlock/20141216")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK004/RequestCardBlock/20141216", IsNullable = false)]
            public partial class RequestCardBlockResponse
            {

                private sbyte requestCardBlockResultField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
                public sbyte RequestCardBlockResult
                {
                    get
                    {
                        return this.requestCardBlockResultField;
                    }
                    set
                    {
                        this.requestCardBlockResultField = value;
                    }
                }
            }
        }



        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false, ElementName = "Envelope")]
        public partial class OutstandingChargesEnvelope
        {

            private EnvelopeBody bodyField;

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private OutstandingChargesResponse outstandingChargesResponseField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310")]
            public OutstandingChargesResponse OutstandingChargesResponse
            {
                get
                {
                    return this.outstandingChargesResponseField;
                }
                set
                {
                    this.outstandingChargesResponseField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310", IsNullable = false)]
        public partial class OutstandingChargesResponse
        {

            private string messageField;

            private bool hasOutstandingChargeField;

            private bool hasExpiredChargeField;

            private byte amountField;

            private object errorMessageField;

            private byte statusCodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public string Message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public bool HasOutstandingCharge
            {
                get
                {
                    return this.hasOutstandingChargeField;
                }
                set
                {
                    this.hasOutstandingChargeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public bool HasExpiredCharge
            {
                get
                {
                    return this.hasExpiredChargeField;
                }
                set
                {
                    this.hasExpiredChargeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public byte Amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public object ErrorMessage
            {
                get
                {
                    return this.errorMessageField;
                }
                set
                {
                    this.errorMessageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public byte StatusCode
            {
                get
                {
                    return this.statusCodeField;
                }
                set
                {
                    this.statusCodeField = value;
                }
            }
        }


    }
}