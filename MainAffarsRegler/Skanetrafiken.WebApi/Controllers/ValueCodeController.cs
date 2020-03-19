using Newtonsoft.Json;
//using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.OCTest.Kassagirot;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.ValueCodes;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk;
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;

namespace Skanetrafiken.Crm.Controllers
{
    //public class ValueCodeEvent
    //{
    //    public int amount { get; set; }
    //    public DateTime created { get; set; }
    //    public int status { get; set; }
    //    public string tag { get; set; }
    //    public DateTime validFromDate { get; set; }
    //    public DateTime validToDate { get; set; }
    //    public string voucherCode { get; set; }
    //    public string voucherId { get; set; }
    //    public int voucherType { get; set; }
    //    public int remainingAmount { get; set; }
    //    public DateTime disabled { get; set; }
    //    public int eanCode { get; set; }
    //}

    /// <summary>
    /// Controller for handling value codes
    /// </summary>
    public class ValueCodeController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Method to be used for retrieving all active/not used value codes for a specific MklId
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET called with parameter: {id}");

            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }
            // TOKEN VERIFICATION
            try
            {
                HttpResponseMessage tokenResp = TokenValidation(id);
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }


            HttpResponseMessage resp = CrmPlusControl.GetValueCodesWithMklId(threadId, id);
            _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/ValueCode/GetMaxAmountValueCode/{q?}")]
        [HttpGet]
        public HttpResponseMessage GetMaxAmountValueCode(string q = "")
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - GET called with no parameter.");

            try
            {
                //Fetch system parameter.
                //Return it.

                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    var localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    var cgiParamSetting = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountForCompensationLoss);
                    return Request.CreateResponse(HttpStatusCode.OK, cgiParamSetting);
                }
            }
            catch (WebException ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "ErrorMsg: " + ex);
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }


        /// <summary>
        /// Used for receiving messages including value code information from a Push-queue (IronMQ).
        /// ReturnCodes:
        /// 200 - message is deleted / acknowledged and removed from the queue
        /// 202 - message is reserved until explicitly deleted or the timeout is exceeded</summary>
        /// 4XX or 5XX - the push request will be retried
        /// <param name="jsonMessage"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ValueCodeEvent jsonMessage)
        {
            // 200 - message is deleted / acknowledged and removed from the queue
            // 202 - message is reserved until explicitly deleted or the timeout is exceeded
            // 4XX or 5XX - the push request will be retried

            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Debug($"Th={threadId} - Post called with Payload:\n {jsonMessage}");

            HttpResponseMessage response;

            try
            {
                ValueCodeEvent valueCodeMsg = jsonMessage;// JsonConvert.DeserializeObject<ValueCodeEvent>(jsonMessage);
                _log.Debug($"Input log: Amount: '{jsonMessage?.amount}', Created: {jsonMessage?.created}, Tag: {jsonMessage?.tag}, ValidFromDate: {jsonMessage?.validFromDate}, " +
                    $"ValidToDate: {jsonMessage?.validToDate}, VoucherCode: {jsonMessage?.voucherCode}, VoucherId: {jsonMessage?.voucherId}, VoucherType: {jsonMessage?.voucherType}, " +
                    $"RemainingAmount: {jsonMessage?.remainingAmount}, Disabled: {jsonMessage?.disabled}, EanCode: {jsonMessage?.eanCode}, CouponId: {jsonMessage?.couponId}");

                if (string.IsNullOrWhiteSpace(jsonMessage.voucherCode))
                    return response = Request.CreateResponse(HttpStatusCode.BadRequest, "VoucherCode cannot be empty.");

                //bool isFoundAndUpdated = 
                valueCodeMsg.UpdateValueCodeInCRM(threadId);

                _log.DebugFormat($"ValueCode found/created in SeKund. Returning OK");
                return response = Request.CreateResponse(HttpStatusCode.OK, "OK");

                //if (isFoundAndUpdated == false)
                //{
                //    _log.DebugFormat($"ValueCode not found in SeKund. Nothing to update");

                //}

                //return response = Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (WebException ex)
            {
                // Temporary while in Test
                //return response = Request.CreateResponse(HttpStatusCode.OK, "OK");
                return response = Request.CreateResponse(HttpStatusCode.InternalServerError, "ErrorMsg: " + ex);
            }


        }

        /// <summary>
        /// Creates a value code and sends it to user.
        /// </summary>
        /// <param name="valueCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateValueCode([FromBody] ValueCodeCreationGiftCard valueCode)
        {
            //  *** Värdekod = Presentkort (endast) ***

            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(valueCode)}");

            Plugin.LocalPluginContext localContext = null;

            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    // Call method to do the heavy lifting
                    return HandleCreateValueCode(localContext, valueCode, threadId);
                }
            }
            catch (WebException ex)
            {
                _log.Error("WebException caught from ValueCodeController: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _log.Error("Exception caught from ValueCodeController: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ex.Message,
                                //ReturnMessageWebApiEntity.GetSettingString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError).ed_UnexpectedError,
                                HttpStatusCode.InternalServerError);
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }

        }

        ///// <summary>
        ///// Returns an error message to api and log
        ///// </summary>
        ///// <param name="threadId"></param>
        ///// <param name="errorMessage">Error message from CRM</param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //private static HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        //{
        //    _log.DebugFormat($"Th={threadId} - Returning statuscode = {code}, Content = {errorMessage}\n");

        //    return Request.CreateResponse(code, errorMessage);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueCode"></param>
        /// <param name="threadId"></param>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public HttpResponseMessage HandleCreateValueCode(Plugin.LocalPluginContext localContext, ValueCodeCreationGiftCard valueCode, int threadId)
        {
            _log.Debug($"--------------Running CreateValueCode--------------");

            #region Argument validation
            if (valueCode == null)
            {
                _log.Debug($"No body was sent into this api.");
                var resp = ReturnApiMessage(threadId, Resources.IncomingDataCannotBeNull, HttpStatusCode.BadRequest);
                return resp;
            }

            ////Amount cannot be less than 1
            if (valueCode.Amount < 0)
            {
                _log.Debug($"Amount cannot be less than 0.");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AmountBelowZero),
                    HttpStatusCode.BadRequest);
                return resp;
            }

            if (string.IsNullOrWhiteSpace(valueCode.Email) || string.IsNullOrWhiteSpace(valueCode.Mobile))
            {
                _log.Debug($"User has to enter both email and mobile");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_EmailAndPhoneNumber),
                    HttpStatusCode.BadRequest);
                return resp;
            }

            //This validation is also done in PreCreateContact, this solution is added here in order to handle status code from CRM which returns Internal Server Error.
            if (!CustomerUtility.CheckEmailFormat(valueCode.Email))
            {
                var resp = ReturnApiMessage(threadId, string.Format(Resources.InvalidFormatForEmail, valueCode.Email),
                HttpStatusCode.BadRequest);
                return resp;
            }

            if (!System.Text.RegularExpressions.Regex.Match(valueCode.Mobile, @"^([0-9]{6,16})$").Success)
            {
                var resp = ReturnApiMessage(threadId, string.Format(Resources.InvalidFormatForMobile, valueCode.Mobile),
                HttpStatusCode.BadRequest);
                return resp;
            }

            valueCode.Mobile = valueCode.Mobile;

            if (valueCode.deliveryMethod != 1 && valueCode.deliveryMethod != 2)
            {
                _log.Debug($"Delivery method does not exist.");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_DeliveryMethodDoesNotExist),
                    HttpStatusCode.NotFound);
                return resp;
            }

            //User has to pass information about the travel card
            if (valueCode.TravelCard == null)
            {
                _log.Debug($"TravelCard is empty.");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoInformationAboutTravelCard),
                    HttpStatusCode.BadRequest);
                return resp;
            }
            else if (string.IsNullOrWhiteSpace(valueCode.TravelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(valueCode.TravelCard.CVC))
            {
                _log.Debug($"TravelCardNumber or CVC is empty.");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC),
                    HttpStatusCode.BadRequest);
                return resp;
            }

            if (valueCode.TypeOfValueCode != 2) //If type of value code wasn't specified in the call, the value automatically becomes 0.
            {
                _log.Debug($"This flow stopped due to type of value code is not 2=Presentkort");
                var resp = ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                    HttpStatusCode.BadRequest);
                return resp;
            }

            #endregion


            _log.Debug($"Fetching travel card from CRM");
            //Travel card is used for validation
            TravelCardEntity travelCard = TravelCardEntity.GetCardAndContactFromCardNumber(localContext,
                           valueCode.TravelCard.TravelCardNumber,
                           new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber,
                                           TravelCardEntity.Fields.cgi_Contactid,
                                           TravelCardEntity.Fields.cgi_TravelCardCVC,
                                           TravelCardEntity.Fields.cgi_Blocked));

            if (travelCard == null)
                _log.Debug($"Travel card '{valueCode.TravelCard.TravelCardNumber}' could not be found in CRM.");
            else _log.Debug($"Travel card '{valueCode.TravelCard.TravelCardNumber}' was found in CRM.");


            //Contact is used for associating with value code and value code approval
            ContactEntity contact = null;

            #region User Is Signed In
            /*
             * Check whether the user is signed in or not. ContactId indicates whether the user is signed in or not.
             */

            if (valueCode.ContactId.HasValue && valueCode.ContactId.Value != Guid.Empty)
            {
                _log.Debug($"ContactId '{valueCode?.ContactId.Value}' was passed. Meaning user is logged in.");
                try
                {
                    _log.Debug($"Check if given contactId exist in CRM.");
                    contact = ContactEntity.GetContactById(localContext, new ColumnSet(ContactEntity.Fields.cgi_ContactNumber
                                                                                        , ContactEntity.Fields.FirstName
                                                                                        , ContactEntity.Fields.LastName
                                                                                        , ContactEntity.Fields.StateCode), valueCode.ContactId.Value);

                    if (contact == null)
                    {
                        _log.Debug($"Could not find given contact id {valueCode.ContactId.Value} in CRM.\n");
                        var resp = ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactDoesNotExist),
                            HttpStatusCode.NotFound);
                        return resp;
                    }
                    else
                    {
                        
                        #region Validate found contact with input parameters
                        _log.Debug($"Found contact '{valueCode?.ContactId.Value}'");

                        if(travelCard == null)
                        {
                            _log.Debug($"User signed in with id '{contact.Id}' but is not associated with travel card '{valueCode.TravelCard.TravelCardNumber}' because it doesn't exist in CRM. Create a new one later in flow.");
                        }

                        else if (travelCard?.cgi_Contactid == null)
                        {
                            _log.Debug($"Travel card '{travelCard?.cgi_travelcardnumber}' does not have an associated contact. Assign contact '{valueCode?.ContactId.Value}' to travel card '{travelCard?.cgi_travelcardnumber}'");
                            // 2DO, Why not do this prior and avoid the update?
                            var updateCard = new TravelCardEntity()
                            {
                                Id = travelCard.Id,
                                cgi_Contactid = contact.ToEntityReference()
                            };

                            XrmHelper.Update(localContext, updateCard);

                            /*
                             * Caution: This is to improve performance, however it can be dangerous.
                             * Instead of fetching latest update of post, assign the newly updated field to pre-fetched post.
                             */
                            travelCard.cgi_Contactid = updateCard.cgi_Contactid;
                        }
                        else if (!travelCard.cgi_Contactid.Equals(contact.ToEntityReference()))
                        {
                            _log.Debug($"Travel card '{travelCard.cgi_travelcardnumber}' is not associated with contact '{contact.Id}'");
                            return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactNotAssociatedWithTravelCard),
                                HttpStatusCode.BadRequest);
                        }

                        //Check if first/lastname matches contact
                        else if (!DoesNameInformationMatchContact(localContext, contact, valueCode.FirstName, valueCode.LastName))
                        {
                            _log.Debug($"Given contact '{contact.Id}' does not containt firstname '{valueCode.FirstName}' and lastname '{valueCode.LastName}'");
                            //Firstname or lastname does not match with travelcard - contact
                            return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactinfoIsNotEqualToTravelCard),
                                HttpStatusCode.BadRequest);
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    _log.Debug($"Error when validating Contact... Ex: {ex.Message}");
                }
            }

            #endregion

            #region User Not Signed In
            else
            {
                _log.Debug($"Entering check for user non-sign in.");

                /* 
                 * A travel card exists..
                 * Case 1: ..but has associated contact --> Tell user to request refund by signing in
                 * Case 2: ..but has no associated contact but contact information from user input exists in CRM --> Fetch contact and update cgi_contact in travel card
                 * Case 3: ..but has no associated contact but no contact in CRM matches users input --> Create a new contact and associate it with travel card
                 */

                //Case 1: If firstname and lastname is found, then it indicates that there's a travel card that's associated with a contact
                if (travelCard != null && travelCard.HasAliasedAttribute(ContactEntity.Fields.FirstName) && travelCard.HasAliasedAttribute(ContactEntity.Fields.LastName))
                {
                    _log.Debug($"Travel card has an associated contact.");

                    // tar bort kontroll efter kontakt med Per Ahrling, 190710
                    //_log.Debug($"Check if travel card information matches first/last name.");
                    ////Check if the values doesn't match with request body
                    //if (travelCard.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.FirstName) != valueCode.FirstName ||
                    //    travelCard.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.LastName) != valueCode.LastName)
                    //{
                    //    // Firstname/Lastname blir tomt när man försöker jämföra på detta sätt.
                    //    // Använd GetAliasedValue istället kanske?
                    //    _log.Debug($"First/last doesn't match.");
                    //    _log.Debug($"TravelCard firstname: {travelCard.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.FirstName)}. " +
                    //        $"TravelCard lastname: {travelCard.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.LastName)}. " +
                    //        $"ValueCode firstname: {valueCode.FirstName}. " +
                    //        $"ValueCode lastname: {valueCode.LastName}.");
                    //    //Firstname or lastname does not match with travelcard - contact
                    //    return ReturnApiMessage(threadId,
                    //        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactinfoIsNotEqualToTravelCard),
                    //        HttpStatusCode.BadRequest);
                    //}


                    _log.Debug($"First/last name matches, which means user has to login.");
                    //User has to login.
                    return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardAssociatedWithContact),
                        HttpStatusCode.BadRequest);
                }

                _log.Debug($"Creating a new contact with given information from api-request.");
                //Case 2 & 3: Since there is no contact associated with the travel card, create a relation. If there is no contact, create one.

                // Create a customer info object to be able to use predefined method for creating contact
                CustomerInfo customerInfo = new CustomerInfo()
                {
                    FirstName = valueCode.FirstName,
                    LastName = valueCode.LastName,
                    Email = valueCode.Email,
                    Mobile = valueCode.Mobile,
                    // TODO - Skicka med annan flagga för Värdekoder
                    Source = (int)Schema.Generated.ed_informationsource.AdmSkapaKund,
                };

                contact = ContactEntity.CreateUnvalidatedPrivateContact(localContext, customerInfo);

                //contact = ContactEntity.CreateContactWithTravelCard(localContext, travelCard, valueCode.FirstName, valueCode.LastName, valueCode.Email, valueCode.Mobile);
                _log.Debug($"Successfully exited CreateContactWithTravelCard");
            }
            #endregion


            if (contact == null)
            {
                _log.Debug($"No contact has been assigned.");
            }

            #region Validate travel card in BIFF
            //Check if travel card exists in BIFF
            //Check if travel card is already invalidated (From BIFF) 
            //Check if there's any value in travel card (if no then send error)
            //Check if there's an active period on the travel card (if yes then send error, cannot invalidate card)

            //Only block card if not one of these cards. This is used for mocking.
            if ((valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") ||
                (valueCode.TravelCard.TravelCardNumber == "654321" && valueCode.TravelCard.CVC == "321") ||
                (valueCode.TravelCard.TravelCardNumber == "45678" && valueCode.TravelCard.CVC == "456") ||
                (valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") ||
                (valueCode.TravelCard.TravelCardNumber == "987654" && valueCode.TravelCard.CVC == "987"))
            {
                _log.Debug($"Called mocked travel cards.");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            decimal outstandingChargesWaiting = 0;


            // Get Outstanding Charges
            #region GetOutstandingCharges
            _log.Debug($"Trying to retrieve OutstandingCharges.");
            try
            {
                string outstandingChargesResponse =
                ValueCodeHandler.CallGetOutstandingChargesFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);

                _log.Debug($"OutstandingCharges retrieve completed. {outstandingChargesResponse}");

                TravelCardEntity.OutstandingChargesEnvelope outstandingCharges = null;


                if (!string.IsNullOrEmpty(outstandingChargesResponse))
                {
                    try
                    {
                        _log.Debug("Starting to serialize outstanding charges");

                        XmlSerializer serializer = new XmlSerializer(typeof(TravelCardEntity.OutstandingChargesEnvelope));

                        _log.Debug("serializer step 1");

                        if (serializer == null)
                        {
                            _log.Debug("serializer step 2");
                            throw new Exception("ParseCardDetails: XmlSerializer is null.");
                        }

                        if (string.IsNullOrWhiteSpace(outstandingChargesResponse))
                        {
                            _log.Debug("serializer step 3");
                            throw new Exception("ParseCardDetails: soapResponse is null.");
                        }

                        _log.Debug("serializer step 4");

                        StringReader strReader = new StringReader(outstandingChargesResponse);

                        _log.Debug("serializer step 5");

                        if (strReader == null)
                        {
                            _log.Debug("serializer step 6");
                            throw new Exception("ParseCardDetails: StringReader is null.");
                        }

                        _log.Debug("serializer step 7");
                        outstandingCharges = (TravelCardEntity.OutstandingChargesEnvelope)serializer.Deserialize(strReader);

                    }
                    catch (Exception ex)
                    {
                        localContext.TracingService.Trace("Error from ParseCardDetails. Ex: " + ex.Message);
                        //throw new Exception($"(ParseCardDetails) error: {ex}");
                        _log.Debug($"InnerError while fetching Outstanding Charges...");
                        //throw new Exception("Någont oväntat fel skedde när vi försökte hämta outhämtade laddningar (ENDAST TILLFÄLLIGT MEDDELANDE)");
                    }

                    if (outstandingCharges != null && outstandingCharges.Body != null &&
                        outstandingCharges.Body.OutstandingChargesResponse != null)
                    {
                        if (outstandingCharges.Body.OutstandingChargesResponse.HasOutstandingCharge == true)
                        {

                            _log.Debug($"Outstanding Charges = {outstandingCharges.Body.OutstandingChargesResponse.Amount} for TravelCard: {valueCode.TravelCard.TravelCardNumber}");

                            decimal amountOutstandingCharges = outstandingCharges.Body.OutstandingChargesResponse.Amount;

                            if (amountOutstandingCharges > 0)
                            {
                                _log.Debug($"There are outstanding charges.");

                                outstandingChargesWaiting = amountOutstandingCharges;


                                //_log.Debug($"Returning BadRequest for TravelCard {valueCode.TravelCard.TravelCardNumber} having Outstanding Charges.");

                                //return ReturnApiMessage(threadId,
                                //    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_PendingCharge),
                                //    HttpStatusCode.BadRequest);
                            }
                            else
                            {
                                _log.Debug("Outstanding Charges Amount is 0 or less.");
                            }
                        }
                    }
                    else
                    {
                        _log.Debug("Outstanding Charges response (amount) is null");
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Debug($"OuterError while fetching Outstanding Charges...");
            }
            #endregion

            _log.Debug($"Calling ed_GetCardDetails workflow.");
            var card = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);
            _log.Debug($"GetCardDetails ran successfully.");

            _log.Debug($"Calling ed_ParseBiztalkResponse workflow.");
            var parsedCard = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, card);
            _log.Debug($"ParseCardDetails ran successfully.");
            _log.Debug($"Data from parse -\nCardNumberField: {parsedCard?.CardNumberField} \n" +
                $"CardIssuerField: {parsedCard?.CardIssuerField} \n" +
                $"CardHotlistedField: {parsedCard?.CardHotlistedField} \n" +
                $"CardTypePeriodField: {parsedCard?.CardTypePeriodField} \n" +
                $"CardTypeValueField: {parsedCard?.CardTypeValueField} \n" +
                $"CardValueProductTypeField: {parsedCard?.CardValueProductTypeField} \n" +
                $"CardKindField: {parsedCard?.CardKindField} \n" +
                $"CardCategoryField: {parsedCard?.CardCategoryField} \n" +
                $"BalanceField: {parsedCard?.BalanceField} \n" +
                $"CurrencyField: {parsedCard?.CurrencyField} \n" +
                $"OutstandingDirectedAutoloadField: {parsedCard?.OutstandingDirectedAutoloadField} \n" +
                $"OutstandingEnableThresholdAutoloadField: {parsedCard?.OutstandingEnableThresholdAutoloadField} \n" +
                $"PurseHotlistedField: {parsedCard?.PurseHotlistedField} \n" +
                $"PeriodCardCategoryField: {parsedCard?.PeriodCardCategoryField} \n" +
                $"ProductTypeField: {parsedCard?.ProductTypeField} \n" +
                $"PeriodStartField: {parsedCard?.PeriodStartField} \n" +
                $"PeriodEndField: {parsedCard?.PeriodEndField} \n" +
                $"WaitingPeriodsField: {parsedCard?.WaitingPeriodsField} \n" +
                $"ZoneListIDField: {parsedCard?.ZoneListIDField} \n" +
                $"ZonesListField: {parsedCard?.ZonesListField} \n" +
                $"PricePaidField: {parsedCard?.PricePaidField} \n" +
                $"ContractSerialNumberField: {parsedCard?.ContractSerialNumberField} \n" +
                $"PeriodCurrencyField: {parsedCard?.PeriodCurrencyField} \n" +
                $"PeriodHotlistedField: {parsedCard?.PeriodHotlistedField} \n" +
                $"PeriodOutstandingDirectedAutoloadField: {parsedCard?.PeriodOutstandingDirectedAutoloadField} \n" +
                $"PeriodOutstandingEnableThresholdAutoload: {parsedCard?.PeriodOutstandingEnableThresholdAutoload}");

            _log.Debug($"Validating parsed card from BIFF.");
            var respBiztalk = ValidateCardDetailsFromBiztalkForPresentkort(localContext, threadId, parsedCard, travelCard);
            if (respBiztalk.StatusCode == HttpStatusCode.NotFound)
            {
                _log.Debug($"Travel card was previously not found. Create a new one with blocked status = true.");
                var newTravelCard = new TravelCardEntity()
                {
                    cgi_travelcardnumber = valueCode.TravelCard.TravelCardNumber,
                    cgi_TravelCardCVC = valueCode.TravelCard.CVC,
                    cgi_Blocked = true,
                    ed_RequestedValueCodeForCard = false,
                    cgi_Contactid = contact.ToEntityReference()
                };

                newTravelCard.Id = XrmHelper.Create(localContext, newTravelCard);
                travelCard = newTravelCard;
                _log.Debug($"Newly created travel card - Id: {newTravelCard?.Id}, " +
                    $"cgi_travelcardnumber: {newTravelCard?.cgi_travelcardnumber}, " +
                    $"cgi_TravelCardCVC: {newTravelCard?.cgi_TravelCardCVC}, " +
                    $"cgi_Blocked: {newTravelCard?.cgi_Blocked}, " +
                    $"ed_RequestedValueCodeForCard: {newTravelCard?.ed_RequestedValueCodeForCard}");


            }
            else if (respBiztalk.StatusCode != HttpStatusCode.OK)
            {
                return respBiztalk;
            }

            #endregion

            #region Validation against travel card

            /*
             * Even if travel card exists in Biff, we need to verify that it also exists in CRM 
             */


            //#region Validation

            //If no travel card is found, create a new one.
            if (travelCard == null)
            {
                _log.Debug($"Specified travelcard '{valueCode?.TravelCard?.TravelCardNumber}' was not found in CRM. Create one in CRM.");

                // 2DO, Refactor to TravelCardEntity and fill more fields!

                var newTravelCard = new TravelCardEntity()
                {
                    cgi_travelcardnumber = valueCode.TravelCard.TravelCardNumber,
                    cgi_TravelCardCVC = valueCode.TravelCard.CVC,
                    cgi_Blocked = false,
                    ed_RequestedValueCodeForCard = false,
                    cgi_Contactid = contact.ToEntityReference()
                };

                newTravelCard.Id = XrmHelper.Create(localContext, newTravelCard);
                travelCard = newTravelCard;
                _log.Debug($"Newly created travel card - Id: {newTravelCard?.Id}, " +
                    $"cgi_travelcardnumber: {newTravelCard?.cgi_travelcardnumber}, " +
                    $"cgi_TravelCardCVC: {newTravelCard?.cgi_TravelCardCVC}, " +
                    $"cgi_Blocked: {newTravelCard?.cgi_Blocked}, " +
                    $"ed_RequestedValueCodeForCard: {newTravelCard?.ed_RequestedValueCodeForCard}" +
                    $"ed_Contactid: {newTravelCard?.cgi_Contactid.Id}");
            }

            ////else _log.Debug($"Found matching travel card '{travelCard?.cgi_travelcardnumber}' in CRM.");

            //#endregion


            /*---------------------------------------------------------------
             * Validation of user input is done, and travel card is valid.
             * Now, invalidate travel card and create value code
             * --------------------------------------------------------------
             */

            _log.Debug($"Fetching maximum value setting.");
            //Fetch setting for maximum value
            var maxAmount = CgiSettingEntity.GetSettingDecimal(localContext, CgiSettingEntity.Fields.ed_MaxAmountForGiftCard);
            if (maxAmount < 0M)
            {
                _log.Debug($"Max amount cannot go below 0. Edit your max limit in Setting entity.");
                return ReturnApiMessage(threadId,
                    "Max amount cannot be less than 0. Please contact CRM admin.",
                    HttpStatusCode.BadRequest);
            }

            _log.Debug($"Fetching valid for days setting.");
            //Fetch setting for number of valid days
            var validDays = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_ValidForDaysValueCode);
            if (validDays < 0)
                return ReturnApiMessage(threadId,
                       "Valid days cannot be less than 0. Please contact CRM admin.",
                       HttpStatusCode.BadRequest);


            //If exceeds max amount, create incident

            //Devop Task 745 - Round decimals
            if(parsedCard.BalanceField > 0)
                parsedCard.BalanceField = (decimal)Math.Round(parsedCard.BalanceField, 0, MidpointRounding.AwayFromZero);

            if (outstandingChargesWaiting > 0)
                parsedCard.BalanceField += outstandingChargesWaiting;

            //If exceeds max amount, create value code approval.
            if (parsedCard.BalanceField > maxAmount)
            {
                HttpResponseMessage blockResponse = null;
                if (respBiztalk.IsSuccessStatusCode)
                {
                    if (!parsedCard.CardHotlistedField)
                    {
                        blockResponse = BlockTravelCard(localContext, threadId, travelCard);
                        if (!blockResponse.IsSuccessStatusCode)
                            return blockResponse;
                    }
                }

                _log.Debug($"Travel card balance exceeded limit. Balance: {parsedCard?.BalanceField} - Limit: {maxAmount}");

                IncidentEntity incidentEnt = null;

                //Create case
                if (valueCode.deliveryMethod == 1)
                    incidentEnt = IncidentEntity.CreateCaseForTravelCardValueCodeExchange(localContext, parsedCard.CardNumberField, parsedCard.BalanceField, contact, valueCode.Email, null, valueCode.deliveryMethod, travelCard);
                else //Mobile - 2
                    incidentEnt = IncidentEntity.CreateCaseForTravelCardValueCodeExchange(localContext, parsedCard.CardNumberField, parsedCard.BalanceField, contact, null, valueCode.Mobile, valueCode.deliveryMethod, travelCard);

                if (incidentEnt == null)
                {
                    _log.Debug($"Incident was not created for some reason.");
                    return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                    HttpStatusCode.BadRequest);
                }

                _log.Debug($"Successfully created an incident. Exit function");
                return ReturnApiMessage(threadId,
                                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AboveMaxAmount),
                                        HttpStatusCode.OK);
            }
            else //Not exceeding maximum amount
            {
                HttpResponseMessage blockResponse = null;
                #region Block Travel Card
                if (respBiztalk.IsSuccessStatusCode)
                {
                    if (!parsedCard.CardHotlistedField)
                    {
                        blockResponse = BlockTravelCard(localContext, threadId, travelCard);
                        if (!blockResponse.IsSuccessStatusCode)
                            return blockResponse;
                    }
                }

                #endregion

                EntityReference valueCodeGeneric = null;

                valueCodeGeneric = CreateGiftCardValueCode(localContext, parsedCard, valueCode, travelCard, contact);

                if (valueCodeGeneric == null)
                {
                    _log.Debug($"Value code was not created, abort process.");
                    return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                    HttpStatusCode.BadRequest);
                }

                _log.Debug($"Fetch travel card '{travelCard?.cgi_travelcardnumber}' from CRM.");
                var crmTravelCard = FetchTravelCardFromCRM(localContext,
                        travelCard.cgi_travelcardnumber, new ColumnSet(TravelCardEntity.Fields.cgi_Blocked));

                //Update travel card to blocked
                if (crmTravelCard?.cgi_Blocked == true)
                {
                    var updateTravelCard = new TravelCardEntity()
                    {
                        Id = crmTravelCard.Id,
                        ed_RequestedValueCodeForCard = true
                    };
                    XrmHelper.Update(localContext, updateTravelCard);

                    _log.Debug($"Update travel card ed_RequestedValueCodeForCard: {updateTravelCard?.ed_RequestedValueCodeForCard}");
                }

                var sendValueCode = ValueCodeHandler.CallSendValueCodeAction(localContext, valueCodeGeneric);
                _log.Debug($"Value code sent.");

                //Successfully created value code
                return ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                    HttpStatusCode.OK);
            }

            #endregion
        }

        /// <summary>
        /// Creates a value code and sends it to user.
        /// </summary>
        /// <param name="valueCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateValueCodeLossCompensation([FromBody] ValueCodeLossCreation valueCode)
        {
            //  *** Endast Förlustgaranti ***
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Debug($"Th={threadId} - CreateValueCodeLossCompensation called with Payload: {CrmPlusControl.SerializeNoNull(valueCode)}");

            Plugin.LocalPluginContext localContext = null;

            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    #region Argument validation

                    if (valueCode == null)
                    {
                        var resp = ReturnApiMessage(threadId, Resources.IncomingDataCannotBeNull, HttpStatusCode.BadRequest);
                        return resp;
                    }

                    //User has to specify email or mobile, not both.
                    if (!(string.IsNullOrWhiteSpace(valueCode.Email) ^ string.IsNullOrWhiteSpace(valueCode.Mobile)))
                    {
                        var resp = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_EmailOrPhoneNumber),
                            HttpStatusCode.BadRequest);
                        return resp;
                    }

                    if (!string.IsNullOrWhiteSpace(valueCode.Email))
                    {
                        //This validation is also done in PreCreateContact, this solution is added here in order to handle status code from CRM which returns Internal Server Error.
                        if (!CustomerUtility.CheckEmailFormat(valueCode.Email))
                        {
                            var resp = ReturnApiMessage(threadId, string.Format(Resources.InvalidFormatForEmail, valueCode.Email),
                            HttpStatusCode.BadRequest);
                            return resp;
                        }
                        valueCode.deliveryMethod = 1;
                    }
                        
                    else
                    {
                        if (!System.Text.RegularExpressions.Regex.Match(valueCode.Mobile, @"^([0-9]{6,16})$").Success)
                        {
                            var resp = ReturnApiMessage(threadId, string.Format(Resources.InvalidFormatForMobile, valueCode.Mobile),
                            HttpStatusCode.BadRequest);
                            return resp;
                        }
                        valueCode.deliveryMethod = 2;

                    }

                    //User has to pass information about the travel card
                    if (valueCode.TravelCard == null)
                    {
                        _log.Debug($"TravelCard is empty.");

                        //TODO: Add error message for not passing travel card information
                        var resp = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoInformationAboutTravelCard),
                            HttpStatusCode.BadRequest);
                        return resp;
                    }
                    else if (string.IsNullOrWhiteSpace(valueCode.TravelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(valueCode.TravelCard.CVC))
                    {
                        _log.Debug($"TravelCardNumber or CVC is empty.");
                        var resp = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC),
                            HttpStatusCode.BadRequest);
                        return resp;
                    }

                    if (valueCode.TypeOfValueCode != 3) //If type of value code wasn't specified in the call, the value automatically becomes 0.
                    {
                        _log.Debug($"This flow stopped due to type of value code is not 3=Förlustgaranti");
                        var resp = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TypeofValueCodeError),
                            HttpStatusCode.BadRequest);
                        return resp;
                    }

                    if (valueCode.ContactId.HasValue && valueCode.ContactId.Value != Guid.Empty)
                    {
                        var con = FetchContact(localContext, valueCode.ContactId.Value);
                        if (con == null)
                        {
                            _log.Debug($"Given contact '{valueCode.ContactId.Value}' does not exist in CRM.");
                            return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactDoesNotExist),
                                HttpStatusCode.BadRequest);
                        }
                    }
                    else
                    {
                        _log.Debug($"ContactId was not sent into body.");
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                            HttpStatusCode.BadRequest);
                    }

                    #endregion

                    decimal outstandingChargesWaiting = 0;

                    // Get Amount from Biztalk
                    #region Handle biztalk process

                    BiztalkParseCardDetailsMessage cardDetailsParsed = null;
                    TravelCardEntity crmTravelCard = null;
                    HttpResponseMessage response = null;

                    //Only block card if not one of these cards. This is used for mocking.
                    if (!(valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") &&
                        !(valueCode.TravelCard.TravelCardNumber == "654321" && valueCode.TravelCard.CVC == "321") &&
                        !(valueCode.TravelCard.TravelCardNumber == "45678" && valueCode.TravelCard.CVC == "456") &&
                        !(valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") &&
                        !(valueCode.TravelCard.TravelCardNumber == "987654" && valueCode.TravelCard.CVC == "987"))
                    {
                        // Get Outstanding Charges
                        // Get Outstanding Charges
                        #region GetOutstandingCharges
                        _log.Debug($"Trying to retrieve OutstandingCharges.");
                        try
                        {
                            string outstandingChargesResponse =
                                ValueCodeHandler.CallGetOutstandingChargesFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);

                            _log.Debug($"OutstandingCharges retrieve completed.");

                            TravelCardEntity.OutstandingChargesEnvelope outstandingCharges = null;

                            if (!string.IsNullOrEmpty(outstandingChargesResponse))
                            {
                                try
                                {

                                    XmlSerializer serializer = new XmlSerializer(typeof(TravelCardEntity.OutstandingChargesEnvelope));


                                    if (serializer == null)
                                        throw new Exception("ParseCardDetails: XmlSerializer is null.");

                                    if (string.IsNullOrWhiteSpace(outstandingChargesResponse))
                                        throw new Exception("ParseCardDetails: soapResponse is null.");
                                    StringReader strReader = new StringReader(outstandingChargesResponse);

                                    if (strReader == null)
                                        throw new Exception("ParseCardDetails: StringReader is null.");

                                    outstandingCharges = (TravelCardEntity.OutstandingChargesEnvelope)serializer.Deserialize(strReader);

                                }
                                catch (Exception ex)
                                {
                                    localContext.TracingService.Trace("Error from ParseCardDetails. Ex: " + ex.Message);
                                    //throw new Exception($"(ParseCardDetails) error: {ex}");
                                    _log.Debug($"InnerError while fetching Outstanding Charges...");
                                }

                                if (outstandingCharges != null && outstandingCharges.Body != null &&
                                    outstandingCharges.Body.OutstandingChargesResponse != null && outstandingCharges.Body.OutstandingChargesResponse.Amount != null)
                                {
                                    if (outstandingCharges.Body.OutstandingChargesResponse.HasOutstandingCharge == true)
                                    {
                                        _log.Debug($"Outstanding Charges = {outstandingCharges.Body.OutstandingChargesResponse.Amount} for TravelCard: {valueCode.TravelCard.TravelCardNumber}");

                                        decimal amountOutstandingCharges = outstandingCharges.Body.OutstandingChargesResponse.Amount;

                                        if (amountOutstandingCharges > 0)
                                        {
                                            _log.Debug($"Returning BadRequest for TravelCard {valueCode.TravelCard.TravelCardNumber} having Outstanding Charges.");

                                            outstandingChargesWaiting = amountOutstandingCharges;

                                            _log.Debug($"Outhämtad laddningar har ett belopp på {outstandingChargesWaiting}. Returnerar tillfälligt detta meddelande. Bör egentligen fortsätta eftersom det är förlustgaranti.");

                                            return ReturnApiMessage(threadId,
                                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_PendingCharge),
                                                HttpStatusCode.BadRequest);
                                        }
                                        else
                                        {
                                            _log.Debug("Outstanding Charges Amount is 0 or less.");
                                        }
                                    }
                                }
                                else
                                {
                                    _log.Debug("Outstanding Charges response (amount) is null");
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            _log.Debug($"OuterError while fetching Outstanding Charges... {ex.Message}");
                        }
                        #endregion


                        #region Fetch card from BIFF
                        _log.Debug($"Fetching travel card '{valueCode.TravelCard.TravelCardNumber}' from BIFF.");
                        string cardDetailsResp = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);

                        if (string.IsNullOrWhiteSpace(cardDetailsResp))
                        {
                            _log.Debug($"Response from BIFF is empty.");
                            var resp = ReturnApiMessage(threadId,
                                 ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                 HttpStatusCode.BadRequest);
                            return resp;
                        }

                        cardDetailsParsed = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, cardDetailsResp);

                        _log.Debug($"Parsed travel card from BIFF: " +
                            $"CardNumberField: {cardDetailsParsed?.CardNumberField} " +
                            $"CardIssuerField: {cardDetailsParsed?.CardIssuerField} " +
                            $"CardHotlistedField: {cardDetailsParsed?.CardHotlistedField} " +
                            $"CardTypePeriodField: {cardDetailsParsed?.CardTypePeriodField} " +
                            $"CardTypeValueField: {cardDetailsParsed?.CardTypeValueField} " +
                            $"CardValueProductTypeField: {cardDetailsParsed?.CardValueProductTypeField} " +
                            $"CardKindField: {cardDetailsParsed?.CardKindField} " +
                            $"CardCategoryField: {cardDetailsParsed?.CardCategoryField} " +
                            $"BalanceField: {cardDetailsParsed?.BalanceField} " +
                            $"CurrencyField: {cardDetailsParsed?.CurrencyField} " +
                            $"OutstandingDirectedAutoloadField: {cardDetailsParsed?.OutstandingDirectedAutoloadField} " +
                            $"OutstandingEnableThresholdAutoloadField: {cardDetailsParsed?.OutstandingEnableThresholdAutoloadField} " +
                            $"PurseHotlistedField: {cardDetailsParsed?.PurseHotlistedField} " +
                            $"PeriodCardCategoryField: {cardDetailsParsed?.PeriodCardCategoryField} " +
                            $"ProductTypeField: {cardDetailsParsed?.ProductTypeField} " +
                            $"PeriodStartField: {cardDetailsParsed?.PeriodStartField} " +
                            $"PeriodEndField: {cardDetailsParsed?.PeriodEndField} " +
                            $"WaitingPeriodsField: {cardDetailsParsed?.WaitingPeriodsField} " +
                            $"ZoneListIDField: {cardDetailsParsed?.ZoneListIDField} " +
                            $"ZonesListField: {cardDetailsParsed?.ZonesListField} " +
                            $"PricePaidField: {cardDetailsParsed?.PricePaidField} " +
                            $"ContractSerialNumberField: {cardDetailsParsed?.ContractSerialNumberField} " +
                            $"PeriodCurrencyField: {cardDetailsParsed?.PeriodCurrencyField} " +
                            $"PeriodHotlistedField: {cardDetailsParsed?.PeriodHotlistedField} " +
                            $"PeriodOutstandingDirectedAutoloadField: {cardDetailsParsed?.PeriodOutstandingDirectedAutoloadField} " +
                            $"PeriodOutstandingEnableThresholdAutoload: {cardDetailsParsed?.PeriodOutstandingEnableThresholdAutoload} ");
                        #endregion

                        #region Validate card
                        _log.Debug($"Fetch travel card '{cardDetailsParsed.CardNumberField}'.");
                        crmTravelCard = FetchTravelCardFromCRM(localContext, cardDetailsParsed.CardNumberField,
                            new ColumnSet(TravelCardEntity.Fields.cgi_Blocked,
                            TravelCardEntity.Fields.ed_RequestedValueCodeForCard,
                            TravelCardEntity.Fields.cgi_travelcardnumber,
                            TravelCardEntity.Fields.cgi_TravelCardCVC,
                            TravelCardEntity.Fields.cgi_Contactid));

                        _log.Debug($"Crm Travel card: " +
                            $"cgi_Blocked: {crmTravelCard?.cgi_Blocked} " +
                            $"ed_RequestedValueCodeForCard: {crmTravelCard?.ed_RequestedValueCodeForCard} " +
                            $"cgi_travelcardnumber: {crmTravelCard?.cgi_travelcardnumber} " +
                            $"cgi_TravelCardCVC: {crmTravelCard?.cgi_TravelCardCVC} " +
                            $"cgi_Contactid: {crmTravelCard?.cgi_Contactid} ");

                        _log.Debug($"Fetching maximum amount limit.");
                        int maxAmountLimit = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountForCompensationLoss);

                        if (crmTravelCard?.cgi_Contactid == null)
                        {
                            _log.Debug($"Travel card is not associated with a contact. Create a new one with given information from request body.");
                            response = TravelCardEntity.ValidateCardDetailsFromBiztalkForCompensation(localContext, threadId, new HttpResponseMessage(),
                                ref cardDetailsParsed, ref crmTravelCard, maxAmountLimit,
                                valueCode.TravelCard.TravelCardNumber, valueCode.TravelCard.CVC, valueCode.ContactId);
                        }
                        else
                        {
                            _log.Debug($"Travel card '{crmTravelCard.cgi_travelcardnumber}' is associated with contact '{crmTravelCard.cgi_Contactid.Id}'");
                            response = TravelCardEntity.ValidateCardDetailsFromBiztalkForCompensation(localContext, threadId, new HttpResponseMessage(),
                                ref cardDetailsParsed, ref crmTravelCard, maxAmountLimit, contactId: valueCode.ContactId);
                        }

                        #endregion

                        #region Ok
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            _log.Debug($"Validation is ok.");
                        }
                        #endregion

                        #region Travel card field ed_RequestedValueCodeForTravelCard indicates a value code has been sent for this travel card.
                        else if (response.StatusCode == HttpStatusCode.Found)
                        {
                            var resp = HandleResendValueCode(localContext, threadId, crmTravelCard, valueCode);
                            if (resp.StatusCode == HttpStatusCode.OK)
                                return resp;
                        }
                        #endregion

                        #region Error
                        else
                        {
                            _log.Debug($"Badrequest from validation.");
                            return response;
                        }
                        #endregion

                    }
                    else
                    {
                        _log.Debug($"Mock travel card.");
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }

                    #endregion


                    // Create Value Code (voucherType (3) = Förlustgaranti)
                    #region Create Value Code

                    TravelCardEntity updateTravelCard = new TravelCardEntity();
                    EntityReference valueCodeCreated = null;

                    _log.Debug($"Re-fetch updated travel card '{cardDetailsParsed.CardNumberField}'");
                    crmTravelCard = FetchTravelCardFromCRM(localContext, valueCode.TravelCard.TravelCardNumber,
                       new ColumnSet(TravelCardEntity.Fields.cgi_Blocked, TravelCardEntity.Fields.cgi_Contactid,
                       TravelCardEntity.Fields.ed_RequestedValueCodeForCard));

                    updateTravelCard.Id = crmTravelCard.Id;
                    updateTravelCard.cgi_Blocked = true;
                    updateTravelCard.ed_RequestedValueCodeForCard = false;
                    updateTravelCard.cgi_TravelCardCVC = valueCode.TravelCard.CVC;

                    //Devop Task 745 - Round decimals
                    if (cardDetailsParsed.BalanceField > 0)
                        cardDetailsParsed.BalanceField = (decimal)Math.Round(cardDetailsParsed.BalanceField, 0, MidpointRounding.AwayFromZero);

                    if (cardDetailsParsed.BalanceField <= 0)
                    {
                        updateTravelCard.st_SaldoReskassa = 0M;

                        _log.Debug($"Note: Balance from BIFF has been changed to 0.");
                        cardDetailsParsed.BalanceField = 0M;
                    }
                    else
                    {
                        updateTravelCard.st_SaldoReskassa = cardDetailsParsed.BalanceField;
                    }

                    // make sure we have an active period
                    if (cardDetailsParsed.PeriodEndField >= DateTime.Now)
                    {
                        if (cardDetailsParsed.PricePaidField <= 0M)
                            updateTravelCard.ed_PeriodPricePaidAmount = 0M;
                        else updateTravelCard.ed_PeriodPricePaidAmount = cardDetailsParsed.PricePaidField;

                        updateTravelCard.ed_ZonesString = cardDetailsParsed.ProductTypeField; //New change: Use ProductType for zone field
                        updateTravelCard.cgi_ValidFrom = cardDetailsParsed.PeriodStartField;
                        updateTravelCard.cgi_ValidTo = cardDetailsParsed.PeriodEndField;
                    }

                    //Update travel card to blocked
                    if (crmTravelCard?.cgi_Blocked == true && crmTravelCard?.ed_RequestedValueCodeForCard != true)
                    {
                        updateTravelCard.ed_RequestedValueCodeForCard = true;
                    }

                    if (outstandingChargesWaiting > 0)
                    {
                        // Lägg till väntande laddning på reskassebeloppet
                        cardDetailsParsed.BalanceField += outstandingChargesWaiting;
                    }

                    _log.Debug($"Updating travel card. Id: {updateTravelCard?.Id}, cgi_Blocked: {updateTravelCard?.cgi_Blocked}, ed_RequestedValueCodeForCard: {updateTravelCard?.ed_RequestedValueCodeForCard}, " +
                        $"cgi_TravelCardCVC: {updateTravelCard?.cgi_TravelCardCVC}, st_SaldoReskassa: {updateTravelCard?.st_SaldoReskassa}, ed_PeriodPricePaidAmount: {updateTravelCard?.ed_PeriodPricePaidAmount}, " +
                        $"ed_ZonesString: {updateTravelCard?.ed_ZonesString}, cgi_ValidFrom: {updateTravelCard?.cgi_ValidFrom}, cgi_ValidTo: {updateTravelCard?.cgi_ValidTo}");

                    XrmHelper.Update(localContext, updateTravelCard);

                    decimal pricePaidCalculated = 0;

                    if (cardDetailsParsed.PeriodEndField > DateTime.Today)
                    {
                        // 2019-08-07 - Recalculate total PricePaid depending on waiting periods
                        pricePaidCalculated = GetTotalPricePaidBasedOnPeriod(cardDetailsParsed.PeriodStartField, cardDetailsParsed.PeriodEndField, cardDetailsParsed.PricePaidField, cardDetailsParsed.CardTypePeriodField);
                    }
                    //decimal waitingAmount = GetWaitingCharges(localContext, valueCode.TravelCard.TravelCardNumber);

                    //pricePaidCalculated += waitingAmount;


                    _log.Debug($"Calling ed_CreateValueCodeGeneric workflow.");
                    valueCodeCreated = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.Forlustgaranti, cardDetailsParsed.BalanceField, pricePaidCalculated, valueCode.Mobile, valueCode.Email, null, null,
                            crmTravelCard.cgi_Contactid, null, valueCode.deliveryMethod, crmTravelCard?.ToEntityReference());

                    #endregion

                    //// Send Value Code
                    #region Send Value Code

                    if (valueCodeCreated != null)
                    {
                        _log.Debug($"Calling ed_SendValueCode workflow.");
                        ValueCodeHandler.CallSendValueCodeAction(localContext, valueCodeCreated);
                        _log.Debug($"Successfully called ed_SendValueCode.");
                    }
                    else
                    {
                        _log.Debug($"A value code was not created, therefore no value code was not sent.");
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                            HttpStatusCode.OK);
                    }

                    #endregion

                    //Successfully created value code   
                    return ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                        HttpStatusCode.OK);
                }
            }
            catch (WebException ex)
            {
                _log.Error("WebException caught from ValueCodeController: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _log.Error("Exception caught from ValueCodeController: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.InternalServerError);
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        #region Helpers

        /// <summary>
        /// Returns an error message to api and log
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="errorMessage">Error message from CRM</param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {
            _log.DebugFormat($"Th={threadId} - Returning statuscode = {code}, Content = {errorMessage}\n");

            HttpResponseMessage response = new HttpResponseMessage(code);
            response.Content = new StringContent(errorMessage);
            return response;
        }

        //private decimal GetWaitingCharges(Plugin.LocalPluginContext localContext, string cardNumber)
        //{
        //    _log.Debug($"GetWaitingCharges: Step 1");

        //    decimal totalAmount = 0;

        //    QueryExpression settingQuery = new QueryExpression()
        //    {
        //        EntityName = CgiSettingEntity.EntityLogicalName,
        //        ColumnSet = new ColumnSet(
        //            CgiSettingEntity.Fields.ed_OutstandingChargesAPI,
        //            CgiSettingEntity.Fields.ed_OutstandingChargesAPIUserName,
        //            CgiSettingEntity.Fields.ed_OutstandingChargesAPIPassword
        //            ),
        //        Criteria =
        //        {
        //            Conditions =
        //            {
        //                new ConditionExpression(CgiSettingEntity.Fields.statecode, ConditionOperator.Equal, (int)Schema.Generated.cgi_settingState.Active)
        //            }
        //        }
        //    };

        //    CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, settingQuery);

        //    _log.Debug($"GetWaitingCharges: Step 2");

        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(setting.ed_OutstandingChargesAPIUserName + ":" + setting.ed_OutstandingChargesAPIPassword);
        //    string encodedAuth = System.Convert.ToBase64String(plainTextBytes);

        //    string url = setting.ed_OutstandingChargesAPI;
        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "POST";
        //    httpWebRequest.Headers.Add("Authorization", encodedAuth);

        //    _log.Debug($"GetWaitingCharges: Step 3");

        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        string InputJSON = JsonConvert.SerializeObject(cardNumber);
        //        //string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInfo);

        //        streamWriter.Write(InputJSON);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    _log.Debug($"GetWaitingCharges: Step 4");

        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //    }

        //    _log.Debug($"GetWaitingCharges: Step 5. Returning Waiting Amount: {totalAmount}");


        //    return totalAmount;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validFrom"></param>
        /// <param name="validTo"></param>
        /// <param name="periodPriceOriginal"></param>
        /// <param name="BiffCardPeriodTypeId"></param>
        /// <returns></returns>
        private decimal GetTotalPricePaidBasedOnPeriod(DateTime validFrom, DateTime validTo, decimal periodPriceOriginal, int BiffCardPeriodTypeId)
        {
            // TODO - Fetch from settings
            _log.Debug($"GetTotalPricePaidBasedOnPeriod: Started. ValidTo: {validTo} PeriodPriceOriginal: {periodPriceOriginal}. BiffCardPeriodTypeId: {BiffCardPeriodTypeId}");

            decimal totalAmount = 0;
            string cardsWithoutStandardPeriod = "3134";

            bool isCardsWithoutStandardPeriod = false;

            if (!string.IsNullOrEmpty(cardsWithoutStandardPeriod))
            {
                _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 1");

                var cardsWithoutStandardPeriodSplit = cardsWithoutStandardPeriod.Trim().Split(',');
                if (cardsWithoutStandardPeriodSplit.Contains(BiffCardPeriodTypeId.ToString())) // CardTypePeriod
                {
                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 2");

                    totalAmount += periodPriceOriginal;
                    isCardsWithoutStandardPeriod = true;

                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 3");
                }
            }

            _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 4");

            if (!isCardsWithoutStandardPeriod)
            {
                _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 5");

                if (validTo >= validFrom.AddDays(30))
                {
                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 6");

                    var totalDaysForAllPeriods = validTo - DateTime.Today;

                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 7. totalDaysForAllPeriods: {totalDaysForAllPeriods}");

                    var numberOfPeriods = (int)Math.Ceiling(totalDaysForAllPeriods.TotalDays / 30);

                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 8. numberOfPeriods: {numberOfPeriods}");

                    totalAmount += periodPriceOriginal * numberOfPeriods;

                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 9. totalAmount: {totalAmount}");
                }
                else
                {
                    totalAmount += periodPriceOriginal;

                    _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 10. totalAmount: {totalAmount}");
                }
            }

            _log.Debug($"GetTotalPricePaidBasedOnPeriod: Step 11. returning totalAmount: {totalAmount}");

            return totalAmount;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCard"></param>
        /// <returns></returns>
        private ValueCodeEntity GetValueCodeWithAssociatedTravelCard(Plugin.LocalPluginContext localContext, TravelCardEntity travelCard)
        {

            var query = new QueryExpression()
            {
                EntityName = ValueCodeEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ValueCodeEntity.Fields.ed_MobileNumber, ValueCodeEntity.Fields.ed_Email,
                ValueCodeEntity.Fields.ed_ValueCodeDeliveryTypeGlobal, ValueCodeEntity.Fields.ed_TypeOption,
                ValueCodeEntity.Fields.statecode),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_TravelCard, ConditionOperator.Equal, travelCard.Id)
                    }
                }
            };

            var foundValueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);
            return foundValueCode;
        }

        /// <summary>
        /// Fethces travel card from CRM
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        private static TravelCardEntity FetchTravelCardFromCRM(Plugin.LocalPluginContext localContext, string travelCardNumber, ColumnSet columnSet)
        {

            var queryTravelCard = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = columnSet,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, travelCardNumber)
                    }
                }
            };

            var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, queryTravelCard);

            return travelCard;
        }

        /// <summary>
        /// Fetches contact entity.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        private ContactEntity FetchContact(Plugin.LocalPluginContext localContext, Guid contactId)
        {
            return XrmRetrieveHelper.Retrieve<ContactEntity>(localContext,
                new EntityReference(ContactEntity.EntityLogicalName, contactId), new ColumnSet());
        }

        /// <summary>
        /// Updates value code with new delivery method and re-sends value code.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="crmTravelCard"></param>
        /// <param name="valueCode"></param>
        /// <returns></returns>
        private HttpResponseMessage HandleResendValueCode(Plugin.LocalPluginContext localContext, int threadId, TravelCardEntity crmTravelCard, ValueCodeLossCreation valueCode)
        {
            _log.Debug($"Th={threadId} - Entering HandleResendValueCode.");

            _log.Debug($"Check if there is an associated value code with given travel card.");
            //If there is an existing value code related to travel card, then resend the same value code.
            var existingValueCode = GetValueCodeWithAssociatedTravelCard(localContext, crmTravelCard);
            if (existingValueCode != null)
            {
                bool attrModified = false;

                var updateExistingValueCode = new ValueCodeEntity() { Id = existingValueCode.Id };

                _log.Debug($"Check if user entered a different email/mobile.");
                if (valueCode.deliveryMethod == 1)
                {
                    if (existingValueCode.ed_Email != valueCode.Email)
                    {
                        _log.Debug($"User entered a different email. New email: {valueCode.Email} - Old email: {existingValueCode.ed_Email}");
                        if (existingValueCode.ed_ValueCodeDeliveryTypeGlobal == Schema.Generated.ed_valuecodedeliverytypeglobal.SMS)
                            updateExistingValueCode.ed_ValueCodeDeliveryTypeGlobal = Schema.Generated.ed_valuecodedeliverytypeglobal.Email;
                        if (existingValueCode.ed_TypeOption == Schema.Generated.ed_valuecode_ed_typeoption.Mobile)
                            updateExistingValueCode.ed_TypeOption = Schema.Generated.ed_valuecode_ed_typeoption.Email;

                        updateExistingValueCode.ed_Email = valueCode.Email;
                        updateExistingValueCode.ed_MobileNumber = string.Empty;
                        attrModified = true;
                    }
                    else
                        _log.Debug($"User entered the same email '{valueCode.Email}' as before. No change in value code was made.");
                }
                else
                {
                    if (existingValueCode.ed_MobileNumber != valueCode.Mobile)
                    {
                        _log.Debug($"User entered a different mobile. New mobile: {valueCode.Mobile} - Old mobile: {existingValueCode.ed_MobileNumber}");

                        if (existingValueCode.ed_ValueCodeDeliveryTypeGlobal == Schema.Generated.ed_valuecodedeliverytypeglobal.Email)
                            updateExistingValueCode.ed_ValueCodeDeliveryTypeGlobal = Schema.Generated.ed_valuecodedeliverytypeglobal.SMS;
                        if (existingValueCode.ed_TypeOption == Schema.Generated.ed_valuecode_ed_typeoption.Email)
                            updateExistingValueCode.ed_TypeOption = Schema.Generated.ed_valuecode_ed_typeoption.Mobile;

                        updateExistingValueCode.ed_MobileNumber = valueCode.Mobile;
                        updateExistingValueCode.ed_Email = string.Empty;
                        attrModified = true;
                    }
                    else
                        _log.Debug($"User entered the same mobile '{valueCode.Mobile}' as before. No change in value code was made.");
                }

                if (attrModified)
                {
                    _log.Debug($"Update existing value code with new email/mobile.");
                    XrmHelper.Update(localContext, updateExistingValueCode);
                }

                _log.Debug($"Value code '{existingValueCode?.Id}' with associated travel card '{crmTravelCard?.cgi_travelcardnumber}' has been found.");
                _log.Debug($"Re-send value code.");
                ValueCodeHandler.CallSendValueCodeAction(localContext, existingValueCode.ToEntityReference());

                return ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                   HttpStatusCode.OK);
            }

            _log.Debug($"Could not find an associated value code with travel card '{crmTravelCard.cgi_travelcardnumber} in SeKund.'");
            _log.Debug($"Value code was probably manually deleted from SeKund.");
            _log.Debug($"No value code was re-sent.");
            _log.Debug($"Exiting HandleDeliveryChange.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Return true if first and last name matches Contact's information.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="contact"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private static bool DoesNameInformationMatchContact(Plugin.LocalPluginContext localContext, ContactEntity contact, string firstName, string lastName)
        {
            _log.Debug($"Entering DoesNameInformationMatchContact.");
            if (contact == null)
            {
                _log.Debug($"Contact is empty.");
                throw new Exception(ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError));
            }

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                _log.Debug($"FirstName: '{firstName}' - LastName: {lastName}");
                throw new Exception(ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError));
            }

            if (!contact.FirstName.Equals(firstName) || !contact.LastName.Equals(lastName))
            {
                _log.Debug($"Firstname or lastname does not match contact.");
                return false;
            }

            _log.Debug($"First and lastname matches contact.");
            return true;

        }

        private static HttpResponseMessage BlockTravelCard(Plugin.LocalPluginContext localContext, int threadId, TravelCardEntity travelCard)
        {
            //Only block card if not one of these cards. This is used for mocking.
            if (!(travelCard.cgi_travelcardnumber == "123456" && travelCard.cgi_TravelCardCVC == "123") &&
                !(travelCard.cgi_travelcardnumber == "654321" && travelCard.cgi_TravelCardCVC == "321") &&
                !(travelCard.cgi_travelcardnumber == "45678" && travelCard.cgi_TravelCardCVC == "456") &&
                !(travelCard.cgi_travelcardnumber == "123456" && travelCard.cgi_TravelCardCVC == "123") &&
                !(travelCard.cgi_travelcardnumber == "987654" && travelCard.cgi_TravelCardCVC == "987"))
            {

                _log.Debug($"Calling ed_BlockCardBiztalk workflow to block travel card '{travelCard?.cgi_travelcardnumber}'.");
                var blockCard = ValueCodeHandler.CallBlockCardBiztalkAction(localContext, travelCard.cgi_travelcardnumber, (int)BlockTravelCardReasonCode.Other);
                _log.Debug($"BlockCardBiztalk ran successfully.");

                _log.Debug($"Calling ed_ParseBlockCardResponseFromBiztalk workflow.");
                var parsedBlockCardResponse = ValueCodeHandler.CallParseBlockCardFromBiztalkAction(localContext, blockCard);
                _log.Debug($"ParseBlockCardFromBiztalk ran successfully.");

                _log.Debug($"Validating response from BIFF.");
                var blockResp = ValidateCardBlockFromBiztalk(localContext, threadId, parsedBlockCardResponse);
                if (blockResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.Debug($"Block response: {parsedBlockCardResponse}");
                    return blockResp;
                }

                _log.Debug($"Update block status on CRM travel card.");
                //Also block travel card in CRM
                var updateTravelCard = new TravelCardEntity()
                {
                    Id = travelCard.Id,
                    cgi_Blocked = true,
                    ed_RequestedValueCodeForCard = false
                };

                XrmHelper.Update(localContext, updateTravelCard);
                _log.Debug($"Updated travel card -\nId: {updateTravelCard?.Id}\ncgi_Blocked: {updateTravelCard?.cgi_Blocked}\n" +
                    $"ed_RequestedValueCodeForCard: {updateTravelCard?.ed_RequestedValueCodeForCard}");

                return blockResp;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        /// <summary>
        /// Validates biff and crm travel card.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="cardDetails"></param>
        /// <param name="crmTravelCard"></param>
        /// <param name="maximumAmount"></param>
        /// <param name="travelCardNr"></param>
        /// <param name="cvc"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public HttpResponseMessage ValidateCardDetailsFromBiztalkForCompensation(Plugin.LocalPluginContext localContext, int threadId,
            ref BiztalkParseCardDetailsMessage cardDetails, ref TravelCardEntity crmTravelCard, decimal maximumAmount,
            string travelCardNr = null, string cvc = null, Guid? contactId = null)
        {
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if (maximumAmount < cardDetails.BalanceField)
            {
                return ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AboveMaxAmount),
                   HttpStatusCode.BadRequest);
            }

            if (cardDetails.PeriodEndField.Date < DateTime.Now.Date && cardDetails.BalanceField <= 0)
            {
                return ReturnApiMessage(threadId,
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
                var updateContactTravelCard = new TravelCardEntity()
                {
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
                return ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNotBlockedYet),
                    HttpStatusCode.BadRequest);
            }
            else if (crmTravelCard.cgi_Blocked.Value && (crmTravelCard.ed_RequestedValueCodeForCard == null || !crmTravelCard.ed_RequestedValueCodeForCard.Value))
            {
                return ReturnApiMessage(threadId,
                    "",
                    HttpStatusCode.OK);
            }
            //This is for if user wants to resend value code.
            else if (crmTravelCard.cgi_Blocked.Value && crmTravelCard.ed_RequestedValueCodeForCard.Value)
            {
                return ReturnApiMessage(threadId, "", HttpStatusCode.Found);
            }
            else
            {
                localContext.TracingService.Trace($"User tried to call this api (CreateValueCodeLossCompensation) without calling BlockTravelCard first.");
                return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
            }
        }



        private static EntityReference CreateGiftCardValueCode(Plugin.LocalPluginContext localContext, BiztalkParseCardDetailsMessage parsedCard, ValueCodeCreationGiftCard valueCode, TravelCardEntity travelCard, ContactEntity contact)
        {
            _log.Debug($"Entering SendGiftCardValueCode");
            if (valueCode.deliveryMethod == 1)
            {
                _log.Debug($"Creating value code - Email.");
                _log.Debug($"Pass");
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, parsedCard.BalanceField, parsedCard.PricePaidField,
                    string.Empty, valueCode.Email, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                _log.Debug($"Exiting SendGiftCardValueCode");
                return res;
            }
            else //Mobile - 2
            {
                _log.Debug($"Creating value code - SMS.");
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, parsedCard.BalanceField, parsedCard.PricePaidField,
                valueCode.Mobile, string.Empty, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                _log.Debug($"Exiting SendGiftCardValueCode");
                return res;
            }
        }

        private HttpResponseMessage ValidateBlockResponse(Plugin.LocalPluginContext localContext, int threadId, string blockResponse)
        {
            _log.Debug($"Validating response from BIFF.");
            if (blockResponse == null || blockResponse == "")
            {
                _log.Debug($"Could not block travel card in Biff. Response return null.");
                ReturnApiMessage(threadId,
                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_CouldNotBlockCard),
                HttpStatusCode.BadRequest);
            }

            if (blockResponse == "-1")
            {
                _log.Debug($"Travel card in Biff is currently in process of blocking it.");
                ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNotBlockedYet),
                                    HttpStatusCode.BadRequest);
            }

            if (blockResponse == "-1000")
            {
                _log.Debug($"Possibly connection error with the database - See Open API documentation.");
                ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_CouldNotBlockCard),
                    HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        /// <summary>
        /// Validate travel card for presentkort.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        public HttpResponseMessage ValidateCardDetailsFromBiztalkForPresentkort(Plugin.LocalPluginContext localContext, int threadId, BiztalkParseCardDetailsMessage cardDetails, TravelCardEntity travelCard)
        {
            _log.Debug($"Entering ValidateCardDetailsFromBiztalkForPresentkort");



            HttpResponseMessage res = null;

            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                res = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.NotFound);
            }

            else if (cardDetails.CardHotlistedField == true)
            {
                /*
                 *In case travel card has been blocked from somewhere else outside this API,
                 * we'll ensure that the Blocked status is updated.
                 */
                #region Verify blocked status

                if (travelCard == null)
                    res = new HttpResponseMessage(HttpStatusCode.NotFound);

                else if (travelCard?.cgi_Blocked == null || travelCard?.cgi_Blocked == false)
                {
                    var updateTravelCard = new TravelCardEntity()
                    {
                        Id = travelCard.Id,
                        cgi_Blocked = true
                    };
                    XrmHelper.Update(localContext, updateTravelCard);

                    res = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardAlreadyBlocked),
                            HttpStatusCode.BadRequest);
                }
                #endregion
            }

            //If there is an incoming charge of purse or period.
            else if (cardDetails.OutstandingDirectedAutoloadField
                || cardDetails.PeriodOutstandingDirectedAutoloadField
                || (!string.IsNullOrEmpty(cardDetails.WaitingPeriodsField)
                    && int.Parse(cardDetails.WaitingPeriodsField) > 0))
            {
                res = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_PendingCharge),
                        HttpStatusCode.BadRequest);
            }

            //For presentkort, user shall not be able to block an active period.
            else if (cardDetails.PeriodEndField != DateTime.MinValue && cardDetails.PeriodEndField > DateTime.Now)
            {
                res = ReturnApiMessage(threadId,
                       ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardHasActivePeriod),
                       HttpStatusCode.BadRequest);
            }

            else if (cardDetails.BalanceField <= 0)
            {
                res = ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoAmountOnTravelCard),
                   HttpStatusCode.BadRequest);
            }

            _log.Debug($"Exiting ValidateCardDetailsFromBiztalkForPresentkort");

            return res != null ? res : new HttpResponseMessage(HttpStatusCode.OK);

        }

        public static HttpResponseMessage ValidateCardBlockFromBiztalk(Plugin.LocalPluginContext localContext, int threadId, string cardBlockResponse)
        {
            localContext.TracingService.Trace($"TravelCardEntity.ValidateCardBlockFromBiztalk: Entering method.");

            if (cardBlockResponse == null || cardBlockResponse == "")
            {
                return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_CouldNotBlockCard),
                            HttpStatusCode.BadRequest);
            }

            if (cardBlockResponse.Equals("-1"))
            {
                return ReturnApiMessage(threadId,
                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNotBlockedYet),
                    HttpStatusCode.BadRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        #endregion
    }
}