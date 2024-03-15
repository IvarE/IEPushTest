//using Skanetrafiken.Crm.Models;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.Properties;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;
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
        private string _prefix = "ValueCode";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        #region New Authentication Method

        public static string _applicationId = "6097adee-83bc-4f8c-80a0-5a803f15c740"; //VoucherService 2.0
        public static string _tenentId = "fa132364-5be1-493f-8d36-42dd02f1ba11"; //VoucherService 2.0

        public static string globalCertificateName = "";
        public static string msAuthScope = "";

        private static readonly TaskFactory _taskFactory = new
        TaskFactory(CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.Default);

        private static IConfidentialClientApplication _msalApplication => _msalApplicationFactory.Value;

        private static Lazy<IConfidentialClientApplication> _msalApplicationFactory =
            new Lazy<IConfidentialClientApplication>(() =>
            {
                var certificate = Identity.GetCertToUse(globalCertificateName);

                var authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/" + _tenentId);

                //Dynamic
                return ConfidentialClientApplicationBuilder
                    .Create(_applicationId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(authority))
                    .Build();

            });

        #endregion

        [Route("api/ValueCode/GetAccessToken/{process}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccessToken(string process)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrWhiteSpace(process))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetAccessToken", "Could not find value in 'process' parameter. Please provide a valid 'process'.", _logger);
                }

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                        var clientId = string.Empty;
                        var clientSecret = string.Empty;
                        var tenentId = string.Empty;
                        var audience = string.Empty;

                        CgiSettingEntity settings = null;
                        FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);

                        if (process == "attachment")
                        {

                            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_AttachmentClientID, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_AttachmentClientSecret, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.st_AttachmentAudience, ConditionOperator.NotNull);

                            settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                            CgiSettingEntity.Fields.ed_AttachmentClientID,
                            CgiSettingEntity.Fields.ed_AttachmentClientSecret,
                            CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                            CgiSettingEntity.Fields.st_AttachmentAudience), settingFilter);

                            clientId = settings.ed_AttachmentClientID;
                            clientSecret = settings.ed_AttachmentClientSecret;
                            tenentId = settings.ed_JojoCardDetailsTenentId;
                            audience = settings.st_AttachmentAudience;
                        }
                        else if (process == "voucher")
                        {

                            settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationClientId, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationClientSecret, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationTenantId, ConditionOperator.NotNull);
                            settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationAudience, ConditionOperator.NotNull);

                            settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                            CgiSettingEntity.Fields.st_CrmAppRegistrationClientId,
                            CgiSettingEntity.Fields.st_CrmAppRegistrationClientSecret,
                            CgiSettingEntity.Fields.st_CrmAppRegistrationTenantId,
                            CgiSettingEntity.Fields.st_CrmAppRegistrationAudience), settingFilter);

                            clientId = settings.st_CrmAppRegistrationClientId;
                            clientSecret = settings.st_CrmAppRegistrationClientSecret;
                            tenentId = settings.st_CrmAppRegistrationTenantId;
                            audience = settings.st_CrmAppRegistrationAudience;
                        }
                        else
                        {
                            //Throw an exception
                            throw new Exception(string.Format("GetAccessToken: Provided 'Process' argument does not match any available processes. Please provide a valid 'Process'."));
                        }

                        if (settings == null || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tenentId) || string.IsNullOrWhiteSpace(audience))
                        {
                            //Throw an exception
                            throw new Exception(string.Format("GetAccessToken: Failed to aquire settings from CRM."));
                        }
                        else
                        {
                            //var tokenResp = await CrmPlusControl.GetAccessToken(clientId, clientSecret, tenentId, "api://9ec4d21c-a934-4692-a322-44698afe33a5/");
                            var tokenResp = await CrmPlusControl.GetAccessToken(clientId, clientSecret, tenentId, audience);

                            //Create a return OK with token in string
                            return ReturnApiMessage(threadId, tokenResp, HttpStatusCode.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    return rm;
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        /// <summary>
        /// Method to be used for retrieving all active/not used value codes for a specific MklId
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrWhiteSpace(id))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                }

                // TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(id);
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetWithId", string.Format(Resources.UnexpectedException, ex.Message), _logger);
                }

                HttpResponseMessage resp = CrmPlusControl.GetValueCodesWithMklId(threadId, id, _prefix);

                return resp;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("api/ValueCode/GetMaxAmountValueCode/{q?}")]
        [HttpGet]
        public HttpResponseMessage GetMaxAmountValueCode(string q = "")
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                try
                {
                    //Fetch system parameter.
                    //Return it.

                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
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
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "ErrorMsg: " + ex);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
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
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                HttpResponseMessage response;
                try
                {
                    ValueCodeEvent valueCodeMsg = jsonMessage;// JsonConvert.DeserializeObject<ValueCodeEvent>(jsonMessage);

                    if (string.IsNullOrWhiteSpace(jsonMessage.voucherCode))
                    {
                        string errorMessage = "VoucherCode cannot be empty.";
                        _logger.LogError($"Post: Failed - {errorMessage}");

                        return response = Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    }

                    //Execute Core Method
                    valueCodeMsg.UpdateValueCodeInCRM(threadId);

                    return response = Request.CreateResponse(HttpStatusCode.OK, "OK");

                }
                catch (WebException ex)
                {
                    // Temporary while in Test
                    //return response = Request.CreateResponse(HttpStatusCode.OK, "OK");
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "ErrorMsg: " + ex);
                }
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
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
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
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                    HttpStatusCode.InternalServerError);
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

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
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                FeatureTogglingEntity feature = XrmRetrieveHelper.RetrieveFirst<FeatureTogglingEntity>(localContext, new ColumnSet(FeatureTogglingEntity.Fields.ed_BlockTravelCardAPI));

                #region Argument validation
                if (valueCode == null)
                {
                    var resp = ReturnApiMessage(threadId, Resources.IncomingDataCannotBeNull, HttpStatusCode.BadRequest);
                    return resp;
                }

                ////Amount cannot be less than 1
                if (valueCode.Amount < 0)
                {
                    var resp = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AmountBelowZero),
                        HttpStatusCode.BadRequest);
                    return resp;
                }

                if (string.IsNullOrWhiteSpace(valueCode.Email) || string.IsNullOrWhiteSpace(valueCode.Mobile))
                {
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

                bool checkMobile = PhoneNumberUtility.CheckRegexPhoneNumber(localContext, valueCode.Mobile);
                #region old code 8042
                //if (!System.Text.RegularExpressions.Regex.Match(valueCode.Mobile, @"^((([+]46)(7)|07)[0-9]{8,8})$").Success)
                #endregion
                if (!checkMobile)
                {
                    var resp = ReturnApiMessage(threadId, string.Format(Resources.InvalidFormatForMobile, valueCode.Mobile),
                    HttpStatusCode.BadRequest);
                    return resp;
                }

                valueCode.Mobile = valueCode.Mobile;

                if (valueCode.deliveryMethod != 1 && valueCode.deliveryMethod != 2)
                {
                    var resp = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_DeliveryMethodDoesNotExist),
                        HttpStatusCode.NotFound);
                    return resp;
                }

                //User has to pass information about the travel card
                if (valueCode.TravelCard == null)
                {
                    var resp = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoInformationAboutTravelCard),
                        HttpStatusCode.BadRequest);
                    return resp;
                }
                else if (string.IsNullOrWhiteSpace(valueCode.TravelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(valueCode.TravelCard.CVC))
                {
                    var resp = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC),
                        HttpStatusCode.BadRequest);
                    return resp;
                }

                if (valueCode.TypeOfValueCode != 2) //If type of value code wasn't specified in the call, the value automatically becomes 0. //CHECK THIS!!!
                {
                    var resp = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    return resp;
                }

                #endregion


                //Travel card is used for validation
                TravelCardEntity travelCard = TravelCardEntity.GetCardAndContactFromCardNumber(localContext,
                               valueCode.TravelCard.TravelCardNumber,
                               new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber,
                                               TravelCardEntity.Fields.cgi_Contactid,
                                               TravelCardEntity.Fields.cgi_TravelCardCVC,
                                               TravelCardEntity.Fields.cgi_Blocked));


                //Contact is used for associating with value code and value code approval
                ContactEntity contact = null;

                #region User Is Signed In
                /*
                 * Check whether the user is signed in or not. ContactId indicates whether the user is signed in or not.
                 */

                if (valueCode.ContactId.HasValue && valueCode.ContactId.Value != Guid.Empty)
                {
                    try
                    {
                        contact = ContactEntity.GetContactById(localContext, new ColumnSet(ContactEntity.Fields.cgi_ContactNumber
                                                                                            , ContactEntity.Fields.FirstName
                                                                                            , ContactEntity.Fields.LastName
                                                                                            , ContactEntity.Fields.StateCode), valueCode.ContactId.Value);

                        if (contact == null)
                        {
                            var resp = ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactDoesNotExist),
                                HttpStatusCode.NotFound);

                            return resp;
                        }
                        else
                        {

                            #region Validate found contact with input parameters

                            if (travelCard == null)
                            {
                                _logger.LogInformation("Travelcard is null");
                            }
                            else if (travelCard?.cgi_Contactid == null)
                            {
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
                                return ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactNotAssociatedWithTravelCard),
                                    HttpStatusCode.BadRequest);
                            }
                            else if (!DoesNameInformationMatchContact(localContext, contact, valueCode.FirstName, valueCode.LastName)) //Check if first/lastname matches contact
                            {
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
                        _exceptionCustomProperties["source"] = _prefix;
                        _logger.LogException(ex, _exceptionCustomProperties);
                    }
                }

                #endregion

                #region User Not Signed In
                else
                {

                    /* 
                     * A travel card exists..
                     * Case 1: ..but has associated contact --> Tell user to request refund by signing in
                     * Case 2: ..but has no associated contact but contact information from user input exists in CRM --> Fetch contact and update cgi_contact in travel card
                     * Case 3: ..but has no associated contact but no contact in CRM matches users input --> Create a new contact and associate it with travel card
                     */

                    //Case 1: If firstname and lastname is found, then it indicates that there's a travel card that's associated with a contact
                    if (travelCard != null && travelCard.HasAliasedAttribute(ContactEntity.Fields.FirstName) && travelCard.HasAliasedAttribute(ContactEntity.Fields.LastName))
                    {
                        //User has to login.
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardAssociatedWithContact),
                            HttpStatusCode.BadRequest);
                    }

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
                }
                #endregion

                //Only block card if not one of these cards. This is used for mocking.
                if ((valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") ||
                    (valueCode.TravelCard.TravelCardNumber == "654321" && valueCode.TravelCard.CVC == "321") ||
                    (valueCode.TravelCard.TravelCardNumber == "45678" && valueCode.TravelCard.CVC == "456") ||
                    (valueCode.TravelCard.TravelCardNumber == "123456" && valueCode.TravelCard.CVC == "123") ||
                    (valueCode.TravelCard.TravelCardNumber == "987654" && valueCode.TravelCard.CVC == "987"))
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }

                //Check if there is a TravelCard - If there is no travel Card, create a new travel card
                if (travelCard == null)
                {

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

                }

                /*---------------------------------------------------------------
                 * Validation of user input is done, and travel card is valid.
                 * Now, invalidate travel card and create value code
                 * --------------------------------------------------------------
                 */

                //Call GET API "Card" [Get Card]

                ValueCodeHandler.GetCardProperties getCardProperties = null;
                var jsonAnswer = GetCardWithCardNumber(localContext, valueCode.TravelCard.TravelCardNumber);
                //Convert response to ValueCodeHandler.GetCardProperties
                if (!string.IsNullOrEmpty(jsonAnswer) && jsonAnswer != "ERROR")
                {

                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    getCardProperties = (ValueCodeHandler.GetCardProperties)serializer.Deserialize(jsonAnswer, typeof(ValueCodeHandler.GetCardProperties));
                }

                if (getCardProperties == null || getCardProperties.Amount == null || string.IsNullOrWhiteSpace(getCardProperties.CardNumber))
                {
                    return ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                    HttpStatusCode.BadRequest);
                }


                //Fetch setting for maximum value
                var maxAmount = CgiSettingEntity.GetSettingDecimal(localContext, CgiSettingEntity.Fields.ed_MaxAmountForGiftCard);
                if (maxAmount < 0M)
                {
                    return ReturnApiMessage(threadId,
                        "Max amount cannot be less than 0. Please contact CRM admin.",
                        HttpStatusCode.BadRequest);
                }

                //Fetch setting for number of valid days
                var validDays = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_ValidForDaysValueCode);
                if (validDays < 0)
                {
                    return ReturnApiMessage(threadId,
                               "Valid days cannot be less than 0. Please contact CRM admin.",
                               HttpStatusCode.BadRequest);
                }

                //If exceeds max amount, create incident
                //Devop Task 745 - Round decimals
                if (getCardProperties.Amount > 0)
                {
                    getCardProperties.Amount = (decimal)Math.Round(getCardProperties.Amount, 0, MidpointRounding.AwayFromZero);
                }

                //If exceeds max amount, create value code approval.
                if (getCardProperties.Amount > maxAmount)
                {
                    //Block Travel Card (OBS! Using Capture Order and Place Order)

                    //Call "Place Order API" to actually block the travel card
                    var placeOrderResponse = PlaceOrderWithCardNumber(localContext, getCardProperties.CardNumber);
                    if (string.IsNullOrWhiteSpace(placeOrderResponse) || placeOrderResponse != "200 - Success")
                    {
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }

                    //OBS! We never create a value code here, instead we create an incident bellow.
                    //[Create a new Value Code before blocking the card through the API / Validate that Value Code has been created]

                    //Call "Card Order API" to start the value code creation and travel card block process (requesting a block of the card)

                    var captureOrderResponse = CaptureOrderWithCardNumber(localContext, getCardProperties.CardNumber);
                    if (string.IsNullOrWhiteSpace(captureOrderResponse) || captureOrderResponse != "200 - Success")
                    {
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }

                    //Create Incident

                    IncidentEntity incidentEnt = null;

                    //Create case (OBS! Replace "valueCode.TravelCard.TravelCardNumber" with fetched cardNumber from "Card GET" - API)
                    if (valueCode.deliveryMethod == 1)
                        incidentEnt = IncidentEntity.CreateCaseForTravelCardValueCodeExchange(localContext, getCardProperties.CardNumber/*valueCode.TravelCard.TravelCardNumber*/, getCardProperties.Amount, contact, valueCode.Email, null, valueCode.deliveryMethod, travelCard);
                    else //Mobile - 2
                        incidentEnt = IncidentEntity.CreateCaseForTravelCardValueCodeExchange(localContext, getCardProperties.CardNumber/*valueCode.TravelCard.TravelCardNumber*/, getCardProperties.Amount, contact, null, valueCode.Mobile, valueCode.deliveryMethod, travelCard);

                    if (incidentEnt == null)
                    {
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }

                    return ReturnApiMessage(threadId,
                                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_AboveMaxAmount),
                                            HttpStatusCode.OK);

                }
                else //Not exceeding maximum amount
                {
                    //Block Travel Card (OBS! Using Capture Order and Place Order)
                    //Call "Place Order API" to actually block the travel card
                    var placeOrderResponse = PlaceOrderWithCardNumber(localContext, getCardProperties.CardNumber);
                    if (string.IsNullOrWhiteSpace(placeOrderResponse) || placeOrderResponse != "200 - Success")
                    {
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }


                    EntityReference valueCodeGeneric = null;

                    try
                    {
                        //Create a new Value Code before blocking the card through the API
                        valueCodeGeneric = CreateGiftCardValueCode(localContext, getCardProperties.Amount, valueCode, travelCard, contact); //new version for new API

                        //Validate that Value Code has been created
                        if (valueCodeGeneric == null)
                        {
                            throw new Exception(string.Format("Could not create Gift Card Value Code."));
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO: CallCancelOrder API
                        var cancelOrderResponse = CancelOrderWithCardNumber(localContext, getCardProperties.CardNumber);

                        _exceptionCustomProperties["source"] = _prefix;
                        _logger.LogException(e, _exceptionCustomProperties);

                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }

                    //Call "Card Order API" to start the value code creation and travel card block process (requesting a block of the card)
                    var captureOrderResponse = CaptureOrderWithCardNumber(localContext, getCardProperties.CardNumber);
                    if (string.IsNullOrWhiteSpace(captureOrderResponse) || captureOrderResponse != "200 - Success")
                    {
                        return ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                        HttpStatusCode.BadRequest);
                    }

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

                    }

                    // Notify customer (send mail?) that the travel card has been blocked
                    var sendValueCode = ValueCodeHandler.CallSendValueCodeAction(localContext, valueCodeGeneric);

                    //Successfully created value code
                    return ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                        HttpStatusCode.OK);

                }
            }
        }

        /// <summary>
        /// Creates a value code and sends it to user.
        /// </summary>
        /// <param name="valueCode"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage CreateValueCodeLossCompensation([FromBody] ValueCodeLossCreation valueCode)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                //  *** Endast Förlustgaranti ***
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
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

                            //TODO: Add error message for not passing travel card information
                            var resp = ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_NoInformationAboutTravelCard),
                                HttpStatusCode.BadRequest);
                            return resp;
                        }
                        else if (string.IsNullOrWhiteSpace(valueCode.TravelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(valueCode.TravelCard.CVC))
                        {
                            var resp = ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC),
                                HttpStatusCode.BadRequest);
                            return resp;
                        }

                        if (valueCode.TypeOfValueCode != 3) //If type of value code wasn't specified in the call, the value automatically becomes 0.
                        {
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
                                var resp = ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_ContactDoesNotExist),
                                    HttpStatusCode.BadRequest);
                                return resp;
                            }
                        }
                        else
                        {
                            var resp = ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.BadRequest);
                            return resp;
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
                            try
                            {
                                string outstandingChargesResponse =
                                    ValueCodeHandler.CallGetOutstandingChargesFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);

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
                                        _exceptionCustomProperties["source"] = _prefix;
                                        _logger.LogException(ex, _exceptionCustomProperties);

                                        localContext.TracingService.Trace("Error from ParseCardDetails. Ex: " + ex.Message);
                                        //throw new Exception($"(ParseCardDetails) error: {ex}");
                                    }

                                    if (outstandingCharges != null && outstandingCharges.Body != null &&
                                        outstandingCharges.Body.OutstandingChargesResponse != null && outstandingCharges.Body.OutstandingChargesResponse.Amount != null)
                                    {
                                        if (outstandingCharges.Body.OutstandingChargesResponse.HasOutstandingCharge == true)
                                        {
                                            decimal amountOutstandingCharges = outstandingCharges.Body.OutstandingChargesResponse.Amount;

                                            if (amountOutstandingCharges > 0)
                                            {

                                                outstandingChargesWaiting = amountOutstandingCharges;


                                                return ReturnApiMessage(threadId,
                                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_PendingCharge),
                                                    HttpStatusCode.BadRequest);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _exceptionCustomProperties["source"] = _prefix;
                                _logger.LogException(ex, _exceptionCustomProperties);
                            }
                            #endregion


                            #region Fetch card from BIFF
                            string cardDetailsResp = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, valueCode.TravelCard.TravelCardNumber);

                            if (string.IsNullOrWhiteSpace(cardDetailsResp))
                            {
                                var resp = ReturnApiMessage(threadId,
                                     ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                     HttpStatusCode.BadRequest);
                                return resp;
                            }

                            cardDetailsParsed = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, cardDetailsResp);

                            #endregion

                            #region Validate card
                            crmTravelCard = FetchTravelCardFromCRM(localContext, cardDetailsParsed.CardNumberField,
                                new ColumnSet(TravelCardEntity.Fields.cgi_Blocked,
                                TravelCardEntity.Fields.ed_RequestedValueCodeForCard,
                                TravelCardEntity.Fields.cgi_travelcardnumber,
                                TravelCardEntity.Fields.cgi_TravelCardCVC,
                                TravelCardEntity.Fields.cgi_Contactid));

                            int maxAmountLimit = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountForCompensationLoss);

                            if (crmTravelCard?.cgi_Contactid == null)
                            {
                                response = TravelCardEntity.ValidateCardDetailsFromBiztalkForCompensation(localContext, threadId, new HttpResponseMessage(),
                                    ref cardDetailsParsed, ref crmTravelCard, maxAmountLimit,
                                    valueCode.TravelCard.TravelCardNumber, valueCode.TravelCard.CVC, valueCode.ContactId);
                            }
                            else
                            {
                                response = TravelCardEntity.ValidateCardDetailsFromBiztalkForCompensation(localContext, threadId, new HttpResponseMessage(),
                                    ref cardDetailsParsed, ref crmTravelCard, maxAmountLimit, contactId: valueCode.ContactId);
                            }

                            #endregion


                            #region Travel card field ed_RequestedValueCodeForTravelCard indicates a value code has been sent for this travel card.
                            if (response.StatusCode == HttpStatusCode.Found)
                            {
                                var resp = HandleResendValueCode(localContext, threadId, crmTravelCard, valueCode);
                                if (resp.StatusCode == HttpStatusCode.OK)
                                    return resp;
                            }
                            #endregion

                            #region Error
                            else
                            {
                                return response;
                            }
                            #endregion

                        }
                        else
                        {
                            return new HttpResponseMessage(HttpStatusCode.OK);
                        }

                        #endregion

                        // Create Value Code (voucherType (3) = Förlustgaranti)
                        #region Create Value Code

                        TravelCardEntity updateTravelCard = new TravelCardEntity();
                        EntityReference valueCodeCreated = null;

                        crmTravelCard = FetchTravelCardFromCRM(localContext, valueCode.TravelCard.TravelCardNumber,
                           new ColumnSet(TravelCardEntity.Fields.cgi_Blocked, TravelCardEntity.Fields.cgi_Contactid,
                           TravelCardEntity.Fields.ed_RequestedValueCodeForCard));

                        updateTravelCard.Id = crmTravelCard.Id;
                        updateTravelCard.cgi_Blocked = true;
                        updateTravelCard.ed_RequestedValueCodeForCard = false;
                        updateTravelCard.cgi_TravelCardCVC = valueCode.TravelCard.CVC;

                        //Devop Task 745 - Round decimals
                        if (cardDetailsParsed.BalanceField > 0)
                        {
                            cardDetailsParsed.BalanceField = (decimal)Math.Round(cardDetailsParsed.BalanceField, 0, MidpointRounding.AwayFromZero);
                        }

                        if (cardDetailsParsed.BalanceField <= 0)
                        {
                            updateTravelCard.st_SaldoReskassa = 0M;
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

                        XrmHelper.Update(localContext, updateTravelCard);

                        decimal pricePaidCalculated = 0;

                        if (cardDetailsParsed.PeriodEndField > DateTime.Today)
                        {
                            // 2019-08-07 - Recalculate total PricePaid depending on waiting periods
                            pricePaidCalculated = GetTotalPricePaidBasedOnPeriod(cardDetailsParsed.PeriodStartField, cardDetailsParsed.PeriodEndField, cardDetailsParsed.PricePaidField, cardDetailsParsed.CardTypePeriodField);
                        }
                        //decimal waitingAmount = GetWaitingCharges(localContext, valueCode.TravelCard.TravelCardNumber);

                        //pricePaidCalculated += waitingAmount;

                        valueCodeCreated = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.Forlustgaranti, cardDetailsParsed.BalanceField, pricePaidCalculated, valueCode.Mobile, valueCode.Email, null, null,
                                crmTravelCard.cgi_Contactid, null, valueCode.deliveryMethod, crmTravelCard?.ToEntityReference());

                        #endregion

                        //// Send Value Code
                        #region Send Value Code

                        if (valueCodeCreated != null)
                        {
                            ValueCodeHandler.CallSendValueCodeAction(localContext, valueCodeCreated);
                        }
                        else
                        {
                            var resp = ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.OK);
                            return resp;
                        }

                        #endregion

                        //Successfully created value code
                        return ReturnApiMessage(threadId, ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                            HttpStatusCode.OK);
                    }
                }
                catch (WebException ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                    HttpStatusCode.InternalServerError);
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                    HttpStatusCode.InternalServerError);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
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
            HttpResponseMessage response = new HttpResponseMessage(code);
            response.Content = new StringContent(errorMessage);
            return response;
        }

        //private decimal GetWaitingCharges(Plugin.LocalPluginContext localContext, string cardNumber)
        //{

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


        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(setting.ed_OutstandingChargesAPIUserName + ":" + setting.ed_OutstandingChargesAPIPassword);
        //    string encodedAuth = System.Convert.ToBase64String(plainTextBytes);

        //    string url = setting.ed_OutstandingChargesAPI;
        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //    httpWebRequest.ContentType = "application/json";
        //    httpWebRequest.Method = "POST";
        //    httpWebRequest.Headers.Add("Authorization", encodedAuth);


        //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    {
        //        string InputJSON = JsonConvert.SerializeObject(cardNumber);
        //        //string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInfo);

        //        streamWriter.Write(InputJSON);
        //        streamWriter.Flush();
        //        streamWriter.Close();
        //    }

        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();
        //    }



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
            decimal totalAmount = 0;
            string cardsWithoutStandardPeriod = "3134";

            bool isCardsWithoutStandardPeriod = false;

            if (!string.IsNullOrEmpty(cardsWithoutStandardPeriod))
            {

                var cardsWithoutStandardPeriodSplit = cardsWithoutStandardPeriod.Trim().Split(',');
                if (cardsWithoutStandardPeriodSplit.Contains(BiffCardPeriodTypeId.ToString())) // CardTypePeriod
                {
                    totalAmount += periodPriceOriginal;
                    isCardsWithoutStandardPeriod = true;
                }
            }

            if (!isCardsWithoutStandardPeriod)
            {
                if (validTo >= validFrom.AddDays(30))
                {
                    var totalDaysForAllPeriods = validTo - DateTime.Today;

                    var numberOfPeriods = (int)Math.Ceiling(totalDaysForAllPeriods.TotalDays / 30);

                    totalAmount += periodPriceOriginal * numberOfPeriods;
                }
                else
                {
                    totalAmount += periodPriceOriginal;
                }
            }

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
            //If there is an existing value code related to travel card, then resend the same value code.
            var existingValueCode = GetValueCodeWithAssociatedTravelCard(localContext, crmTravelCard);
            if (existingValueCode != null)
            {
                bool attrModified = false;

                var updateExistingValueCode = new ValueCodeEntity() { Id = existingValueCode.Id };

                if (valueCode.deliveryMethod == 1)
                {
                    if (existingValueCode.ed_Email != valueCode.Email)
                    {
                        if (existingValueCode.ed_ValueCodeDeliveryTypeGlobal == Schema.Generated.ed_valuecodedeliverytypeglobal.SMS)
                            updateExistingValueCode.ed_ValueCodeDeliveryTypeGlobal = Schema.Generated.ed_valuecodedeliverytypeglobal.Email;
                        if (existingValueCode.ed_TypeOption == Schema.Generated.ed_valuecode_ed_typeoption.Mobile)
                            updateExistingValueCode.ed_TypeOption = Schema.Generated.ed_valuecode_ed_typeoption.Email;

                        updateExistingValueCode.ed_Email = valueCode.Email;
                        updateExistingValueCode.ed_MobileNumber = string.Empty;
                        attrModified = true;
                    }
                }
                else
                {
                    if (existingValueCode.ed_MobileNumber != valueCode.Mobile)
                    {
                        if (existingValueCode.ed_ValueCodeDeliveryTypeGlobal == Schema.Generated.ed_valuecodedeliverytypeglobal.Email)
                            updateExistingValueCode.ed_ValueCodeDeliveryTypeGlobal = Schema.Generated.ed_valuecodedeliverytypeglobal.SMS;
                        if (existingValueCode.ed_TypeOption == Schema.Generated.ed_valuecode_ed_typeoption.Email)
                            updateExistingValueCode.ed_TypeOption = Schema.Generated.ed_valuecode_ed_typeoption.Mobile;

                        updateExistingValueCode.ed_MobileNumber = valueCode.Mobile;
                        updateExistingValueCode.ed_Email = string.Empty;
                        attrModified = true;
                    }
                }

                if (attrModified)
                {
                    XrmHelper.Update(localContext, updateExistingValueCode);
                }

                ValueCodeHandler.CallSendValueCodeAction(localContext, existingValueCode.ToEntityReference());

                return ReturnApiMessage(threadId,
                   ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_OK),
                   HttpStatusCode.OK);
            }

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
            if (contact == null)
            {
                throw new Exception(ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError));
            }

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new Exception(ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError));
            }

            if (!contact.FirstName.Equals(firstName) || !contact.LastName.Equals(lastName))
            {
                return false;
            }

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
                var blockCard = ValueCodeHandler.CallBlockCardBiztalkAction(localContext, travelCard.cgi_travelcardnumber, (int)BlockTravelCardReasonCode.Other);

                var parsedBlockCardResponse = ValueCodeHandler.CallParseBlockCardFromBiztalkAction(localContext, blockCard);

                var blockResp = ValidateCardBlockFromBiztalk(localContext, threadId, parsedBlockCardResponse);
                if (blockResp.StatusCode != HttpStatusCode.OK)
                {
                    return blockResp;
                }

                //Also block travel card in CRM
                var updateTravelCard = new TravelCardEntity()
                {
                    Id = travelCard.Id,
                    cgi_Blocked = true,
                    ed_RequestedValueCodeForCard = false
                };

                XrmHelper.Update(localContext, updateTravelCard);

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
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                try
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
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return ReturnApiMessage(threadId, ex.Message, HttpStatusCode.InternalServerError);
                }
            }
        }


        //TEST JOJOCARD
        public string GetCardWithCardNumber(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string answer = "";

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    answer = "ERROR";
                    return answer;
                }

                try
                {
                    //Get information from settings
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsScope, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertNameReskassa, ConditionOperator.NotNull);
                    CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                        CgiSettingEntity.Fields.ed_JojoCardDetailsAPI,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsScope,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                        CgiSettingEntity.Fields.ed_ClientCertNameReskassa), settingFilter);

                    if (settings != null)
                    {
                        _applicationId = settings.ed_JojoCardDetailsApplicationId;
                        _tenentId = settings.ed_JojoCardDetailsTenentId;
                        globalCertificateName = settings.ed_ClientCertNameReskassa;
                        msAuthScope = settings.ed_JojoCardDetailsScope;
                    }
                    else
                    {
                        HttpResponseMessage badReqSetting = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        badReqSetting.Content = new StringContent("Could not find a Settings Information from CRM (GetCardWithCardNumber)");

                        answer = "ERROR - " + new StringContent("Could not find a Settings Information from CRM (GetCardWithCardNumber)");
                        return answer;
                    }

                    AuthenticationResult authenticationResponse = _taskFactory
                        .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default - https://skanetrafiken.se/apps/jojocardservice/.default
                        .Unwrap()
                        .GetAwaiter()
                        .GetResult();

                    if (authenticationResponse == null)
                    {
                        //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //badReq.Content = new StringContent("Error: Could not aquire token for client!");
                        //return badReq;
                        answer = "ERROR";
                        return answer;
                    }

                    string endPoint = settings.ed_JojoCardDetailsAPI + "card/";
                    //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/card/";
                    //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/card/";

                    var myUri = new Uri(endPoint);
                    var myWebRequest = WebRequest.Create(myUri);
                    var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                    myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                    myHttpWebRequest.Headers.Add("20", "*/*");
                    myHttpWebRequest.Method = "GET";

                    var myWebResponse = myWebRequest.GetResponse();
                    var responseStream = myWebResponse.GetResponseStream();
                    if (responseStream == null) return null;

                    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    var json = myStreamReader.ReadToEnd();

                    responseStream.Close();
                    myWebResponse.Close();

                    var returnJson = new StringContent(json);

                    //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    //resp.Content = returnJson;
                    //return resp;

                    return json;
                }
                catch (Exception ex)
                {
                    //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    //rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    //return rm;
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    throw new InvalidPluginExecutionException("Error Getting Card (GetCardWithCardNumber): " + ex.Message);
                }
            }
        }


        public string PlaceOrderWithCardNumber(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                string answer = "";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                    //return badReq;
                    answer = "ERROR";
                    return answer;
                }

                try
                {
                    //Get information from settings
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsScope, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertNameReskassa, ConditionOperator.NotNull);
                    CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                        CgiSettingEntity.Fields.ed_JojoCardDetailsAPI,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsScope,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                        CgiSettingEntity.Fields.ed_ClientCertNameReskassa), settingFilter);

                    if (settings != null)
                    {
                        _applicationId = settings.ed_JojoCardDetailsApplicationId;
                        _tenentId = settings.ed_JojoCardDetailsTenentId;
                        globalCertificateName = settings.ed_ClientCertNameReskassa;
                        msAuthScope = settings.ed_JojoCardDetailsScope;
                    }
                    else
                    {
                        HttpResponseMessage badReqSetting = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        badReqSetting.Content = new StringContent("Could not find a Settings Information from CRM (PlaceOrderWithCardNumber)");

                        answer = "ERROR - " + new StringContent("Could not find a Settings Information from CRM (PlaceOrderWithCardNumber)");
                        return answer;
                    }

                    AuthenticationResult authenticationResponse = _taskFactory
                        .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default - https://skanetrafiken.se/apps/jojocardservice/.default
                        .Unwrap()
                        .GetAwaiter()
                        .GetResult();

                    if (authenticationResponse == null)
                    {
                        //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //badReq.Content = new StringContent("Error: Could not aquire token for client!");
                        //return badReq;
                        answer = "ERROR";
                        return answer;
                    }


                    string endPoint = settings.ed_JojoCardDetailsAPI + "placeOrder/";
                    //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/placeOrder/";
                    //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/placeOrder/";

                    var myUri = new Uri(endPoint);
                    var myWebRequest = WebRequest.Create(myUri);
                    var myHttpWebRequest = (HttpWebRequest)myWebRequest;

                    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                    myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                    myHttpWebRequest.Headers.Add("20", "*/*");

                    myHttpWebRequest.ContentLength = 0;
                    myHttpWebRequest.Method = "POST";

                    var myWebResponse = myWebRequest.GetResponse();
                    var responseStream = myWebResponse.GetResponseStream();
                    if (responseStream == null) return null;

                    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    var json = myStreamReader.ReadToEnd();

                    var checkStatus = (HttpWebResponse)myWebResponse;
                    if (checkStatus.StatusCode != HttpStatusCode.OK)
                    {
                        //Send bad request
                        answer = "400 - Fel";
                    }
                    else
                    {
                        //We are done
                        answer = "200 - Success";
                    }

                    responseStream.Close();
                    myWebResponse.Close();

                    var returnJson = new StringContent(json);

                    //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    //resp.Content = returnJson;
                    //return resp;

                    return answer;
                }
                catch (Exception ex)
                {
                    //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    //rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    //return rm;
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    throw new InvalidPluginExecutionException("Error Getting Card (PlaceOrderWithCardNumber): " + ex.Message);
                }
            }
        }


        public string CancelOrderWithCardNumber(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string answer = "";

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                    //return badReq;
                    answer = "ERROR";
                    return answer;
                }

                try
                {
                    //Get information from settings
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsScope, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertNameReskassa, ConditionOperator.NotNull);
                    CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                        CgiSettingEntity.Fields.ed_JojoCardDetailsAPI,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsScope,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                        CgiSettingEntity.Fields.ed_ClientCertNameReskassa), settingFilter);

                    if (settings != null)
                    {
                        _applicationId = settings.ed_JojoCardDetailsApplicationId;
                        _tenentId = settings.ed_JojoCardDetailsTenentId;
                        globalCertificateName = settings.ed_ClientCertNameReskassa;
                        msAuthScope = settings.ed_JojoCardDetailsScope;
                    }
                    else
                    {
                        HttpResponseMessage badReqSetting = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        badReqSetting.Content = new StringContent("Could not find a Settings Information from CRM (CancelOrderWithCardNumber)");

                        answer = "ERROR - " + new StringContent("Could not find a Settings Information from CRM (CancelOrderWithCardNumber)");
                        return answer;
                    }

                    AuthenticationResult authenticationResponse = _taskFactory
                        .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default - https://skanetrafiken.se/apps/jojocardservice/.default
                        .Unwrap()
                        .GetAwaiter()
                        .GetResult();

                    if (authenticationResponse == null)
                    {
                        //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //badReq.Content = new StringContent("Error: Could not aquire token for client!");
                        //return badReq;
                        answer = "ERROR";
                        return answer;
                    }


                    string endPoint = settings.ed_JojoCardDetailsAPI + "cancelOrder/";
                    //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/cancelOrder/";
                    //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/cancelOrder/";

                    var myUri = new Uri(endPoint);
                    var myWebRequest = WebRequest.Create(myUri);
                    var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                    myHttpWebRequest.Headers.Add("Card-Number", cardNumber);

                    myHttpWebRequest.ContentLength = 0;
                    myHttpWebRequest.Headers.Add("20", "*/*");
                    myHttpWebRequest.Method = "POST";

                    var myWebResponse = myWebRequest.GetResponse();
                    //Check Status and return ok or not ok

                    var responseStream = myWebResponse.GetResponseStream();
                    if (responseStream == null) return null;

                    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    var json = myStreamReader.ReadToEnd();

                    var checkStatus = (HttpWebResponse)myWebResponse;
                    if (checkStatus.StatusCode != HttpStatusCode.OK)
                    {
                        //Send bad request
                        answer = "Could not Cancel Order!";
                    }
                    else
                    {
                        //We are done
                        answer = "Order was canceled!";
                    }

                    responseStream.Close();
                    myWebResponse.Close();

                    var returnJson = new StringContent(json);

                    //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    //resp.Content = returnJson;

                    return answer;
                }
                catch (Exception ex)
                {
                    //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    //rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    //return rm;
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    throw new InvalidPluginExecutionException("Error Getting Card (CancelOrderWithCardNumber): " + ex.Message);
                }
            }
        }


        public string CaptureOrderWithCardNumber(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string answer = "";

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                    //return badReq;
                    answer = "ERROR";
                    return answer;
                }

                try
                {
                    //Get information from settings
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsScope, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                    settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertNameReskassa, ConditionOperator.NotNull);
                    CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                        CgiSettingEntity.Fields.ed_JojoCardDetailsAPI,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsScope,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsApplicationId,
                        CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                        CgiSettingEntity.Fields.ed_ClientCertNameReskassa), settingFilter);

                    if (settings != null)
                    {
                        _applicationId = settings.ed_JojoCardDetailsApplicationId;
                        _tenentId = settings.ed_JojoCardDetailsTenentId;
                        globalCertificateName = settings.ed_ClientCertNameReskassa;
                        msAuthScope = settings.ed_JojoCardDetailsScope;
                    }
                    else
                    {
                        HttpResponseMessage badReqSetting = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        badReqSetting.Content = new StringContent("Could not find a Settings Information from CRM (CaptureOrderWithCardNumber)");

                        answer = "ERROR - " + new StringContent("Could not find a Settings Information from CRM (CaptureOrderWithCardNumber)");
                        return answer;
                    }

                    AuthenticationResult authenticationResponse = _taskFactory
                        .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default - https://skanetrafiken.se/apps/jojocardservice/.default
                        .Unwrap()
                        .GetAwaiter()
                        .GetResult();

                    if (authenticationResponse == null)
                    {
                        //HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //badReq.Content = new StringContent("Error: Could not aquire token for client!");
                        //return badReq;
                        answer = "ERROR";
                        return answer;
                    }


                    //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/captureOrder/";
                    //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/captureOrder/";
                    string endPoint = settings.ed_JojoCardDetailsAPI + "captureOrder/";

                    var myUri = new Uri(endPoint);
                    var myWebRequest = WebRequest.Create(myUri);
                    var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                    myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                    myHttpWebRequest.Headers.Add("20", "*");
                    myHttpWebRequest.ContentLength = 0;
                    myHttpWebRequest.Method = "POST";

                    var myWebResponse = myWebRequest.GetResponse();
                    //Check Status and return ok or not ok

                    var responseStream = myWebResponse.GetResponseStream();
                    if (responseStream == null) return null;

                    var checkStatus = (HttpWebResponse)myWebResponse;
                    if (checkStatus.StatusCode != HttpStatusCode.OK)
                    {
                        //Send bad request
                        answer = "400 - Fel";
                    }
                    else
                    {
                        //We are done
                        answer = "200 - Success";
                    }

                    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                    var json = myStreamReader.ReadToEnd();

                    responseStream.Close();
                    myWebResponse.Close();

                    var returnJson = new StringContent(json);

                    //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                    //resp.Content = returnJson;
                    //return resp;

                    return answer;
                }
                catch (Exception ex)
                {
                    //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    //rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    //return rm;
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    throw new InvalidPluginExecutionException("Error Getting Card (CaptureOrderWithCardNumber): " + ex.Message);
                }
            }
        }

        private static EntityReference CreateGiftCardValueCode(Plugin.LocalPluginContext localContext, BiztalkParseCardDetailsMessage parsedCard, ValueCodeCreationGiftCard valueCode, TravelCardEntity travelCard, ContactEntity contact)
        {
            if (valueCode.deliveryMethod == 1)
            {
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, parsedCard.BalanceField, parsedCard.PricePaidField,
                    string.Empty, valueCode.Email, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                return res;
            }
            else //Mobile - 2
            {
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, parsedCard.BalanceField, parsedCard.PricePaidField,
                valueCode.Mobile, string.Empty, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                return res;
            }
        }

        //New Value Code Creation version for new API (not BIFF)
        private static EntityReference CreateGiftCardValueCode(Plugin.LocalPluginContext localContext, decimal balance, ValueCodeCreationGiftCard valueCode, TravelCardEntity travelCard, ContactEntity contact)
        {
            if (valueCode.deliveryMethod == 1)
            {
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, balance, 0,
                    string.Empty, valueCode.Email, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                return res;
            }
            else //Mobile - 2
            {
                var res = ValueCodeHandler.CallCreateValueCodeAction(localContext, (int)Schema.Generated.ed_valuecodetypeglobal.InlostReskassa, balance, 0,
                valueCode.Mobile, string.Empty, null, null, contact.ToEntityReference(), null, valueCode.deliveryMethod, travelCard?.ToEntityReference());

                return res;
            }
        }

        private HttpResponseMessage ValidateBlockResponse(Plugin.LocalPluginContext localContext, int threadId, string blockResponse)
        {
            if (blockResponse == null || blockResponse == "")
            {
                ReturnApiMessage(threadId,
                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_CouldNotBlockCard),
                HttpStatusCode.BadRequest);
            }

            if (blockResponse == "-1")
            {
                ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNotBlockedYet),
                                    HttpStatusCode.BadRequest);
            }

            if (blockResponse == "-1000")
            {
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