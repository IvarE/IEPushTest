using Endeavor.Crm;
using Microsoft.Identity.Client;
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;

namespace Skanetrafiken.Crm.Controllers
{
    public class TravelCardController : WrapperController
    {
        private string _prefix = "TravelCard";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        public static string applicationId = "";
        public static string tenentId = "";
        public static string jojoCertificateName = "";
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
                //var certificate = Identity.GetCertToUse("crm-sekundfasaden-acc-sp");
                //var certificate = Identity.GetCertToUse("crm-sekundfasaden-prod-sp");
                var certificate = Identity.GetCertToUse(jojoCertificateName);

                //var authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/e1fcb9f3-e5f9-496f-a583-e495dfd57497"); //Tenent
                var authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/" + tenentId);

                // Dynamic
                return ConfidentialClientApplicationBuilder
                    .Create(applicationId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(authority))
                    .Build();

                //// PROD
                //return ConfidentialClientApplicationBuilder
                //    .Create("64c45900-b2dc-4818-8f29-a4b41cbcc21f")
                //    .WithCertificate(certificate)
                //    .WithAuthority(new Uri(authority))
                //    .Build();

                //// ACC
                //return ConfidentialClientApplicationBuilder
                //    .Create("9e84b58e-20aa-4ceb-aa89-abd98253afd2")
                //    .WithCertificate(certificate)
                //    .WithAuthority(new Uri(authority))
                //    .Build();
            });

        [HttpGet]
        public HttpResponseMessage GetCardWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetCardWithCardNumber", "Could not find a 'CardNumber' parameter in url", _logger);
                }

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);

                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

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
                            applicationId = settings.ed_JojoCardDetailsApplicationId;
                            tenentId = settings.ed_JojoCardDetailsTenentId;
                            jojoCertificateName = settings.ed_ClientCertNameReskassa;
                            msAuthScope = settings.ed_JojoCardDetailsScope;
                        }
                        else
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetCardWithCardNumber", "Could not find Settings Information from CRM", _logger);
                        }

                        //AuthenticationResult authenticationResponse = _taskFactory
                        //    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceprod/.default" }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default
                        //    .Unwrap()
                        //    .GetAwaiter()
                        //    .GetResult();

                        AuthenticationResult authenticationResponse = _taskFactory
                            .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync)
                            .Unwrap()
                            .GetAwaiter()
                            .GetResult();

                        if (authenticationResponse == null)
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetCardWithCardNumber", "Could not aquire token for client!", _logger);
                        }

                        //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/card/";
                        //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/card/";
                        string endPoint = settings.ed_JojoCardDetailsAPI + "card/";

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

                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = returnJson;
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("404"))
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.NotFound, "GetCardWithCardNumber", "Not found", _logger);
                    }

                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetCardWithCardNumber", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage PlaceOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "PlaceOrderWithCardNumber", "Could not find a 'CardNumber' parameter in url", _logger);
                }

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);

                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

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
                            applicationId = settings.ed_JojoCardDetailsApplicationId;
                            tenentId = settings.ed_JojoCardDetailsTenentId;
                            jojoCertificateName = settings.ed_ClientCertNameReskassa;
                            msAuthScope = settings.ed_JojoCardDetailsScope;
                        }
                        else
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "PlaceOrderWithCardNumber", "Could not find a Settings Information from CRM", _logger);
                        }

                        //AuthenticationResult authenticationResponse = _taskFactory
                        //    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardservice/.default" }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default
                        //    .Unwrap()
                        //    .GetAwaiter()
                        //    .GetResult();

                        AuthenticationResult authenticationResponse = _taskFactory
                            .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync)
                            .Unwrap()
                            .GetAwaiter()
                            .GetResult();

                        if (authenticationResponse == null)
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "PlaceOrderWithCardNumber", "Could not aquire token for client!", _logger);
                        }

                        //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/placeOrder/";
                        //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/placeOrder/";
                        string endPoint = settings.ed_JojoCardDetailsAPI + "placeOrder/";

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

                        responseStream.Close();
                        myWebResponse.Close();

                        var returnJson = new StringContent(json);

                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = returnJson;
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "PlaceOrderWithCardNumber", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage CancelOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CancelOrderWithCardNumber", "\"Could not find a 'CardNumber' parameter in url", _logger);
                }

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);

                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

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
                            applicationId = settings.ed_JojoCardDetailsApplicationId;
                            tenentId = settings.ed_JojoCardDetailsTenentId;
                            jojoCertificateName = settings.ed_ClientCertNameReskassa;
                            msAuthScope = settings.ed_JojoCardDetailsScope;
                        }
                        else
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CancelOrderWithCardNumber", "Could not find a Settings Information from CRM", _logger);
                        }

                        //AuthenticationResult authenticationResponse = _taskFactory
                        //    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardservice/.default" }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default
                        //    .Unwrap()
                        //    .GetAwaiter()
                        //    .GetResult();

                        AuthenticationResult authenticationResponse = _taskFactory
                            .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync)
                            .Unwrap()
                            .GetAwaiter()
                            .GetResult();

                        if (authenticationResponse == null)
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CancelOrderWithCardNumber", "Could not aquire token for client!", _logger);
                        }


                        //string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/cancelOrder/";
                        //string endPoint = "https://stjojocardserviceprod.azurewebsites.net/v1/cancelOrder/";
                        string endPoint = settings.ed_JojoCardDetailsAPI + "cancelOrder/";

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

                        responseStream.Close();
                        myWebResponse.Close();

                        var returnJson = new StringContent(json);

                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = returnJson;
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "CancelOrderWithCardNumber", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage CaptureOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CaptureOrderWithCardNumber", "Could not find a 'CardNumber' parameter in url", _logger);
                }

                Plugin.LocalPluginContext localContext = null;

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);

                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

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
                            applicationId = settings.ed_JojoCardDetailsApplicationId;
                            tenentId = settings.ed_JojoCardDetailsTenentId;
                            jojoCertificateName = settings.ed_ClientCertNameReskassa;
                            msAuthScope = settings.ed_JojoCardDetailsScope;
                        }
                        else
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CaptureOrderWithCardNumber", "Could not find a Settings Information from CRM", _logger);
                        }

                        //AuthenticationResult authenticationResponse = _taskFactory
                        //    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardservice/.default" }).ExecuteAsync) //https://skanetrafiken.se/apps/jojocardserviceacc/.default
                        //    .Unwrap()
                        //    .GetAwaiter()
                        //    .GetResult();

                        AuthenticationResult authenticationResponse = _taskFactory
                            .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync)
                            .Unwrap()
                            .GetAwaiter()
                            .GetResult();

                        if (authenticationResponse == null)
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "CaptureOrderWithCardNumber", "Could not aquire token for client!", _logger);
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

                        var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                        var json = myStreamReader.ReadToEnd();

                        responseStream.Close();
                        myWebResponse.Close();

                        var returnJson = new StringContent(json);

                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                        resp.Content = returnJson;
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "CaptureOrderWithCardNumber", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        //Testing New API Call
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Blocks and existing Travel Card
        /// </summary>
        /// <param name="travelCard"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage BlockTravelCard([FromBody] TravelCard travelCard)
        {
            //  *** Endast Förlustgaranti ***
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Plugin.LocalPluginContext localContext = null;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                var response = new HttpResponseMessage();

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);


                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                        if (string.IsNullOrWhiteSpace(travelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(travelCard.CVC))
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "BlockTravelCard", ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC), _logger);
                        }

                        if ((travelCard.TravelCardNumber == "123456" && travelCard.CVC == "123") ||
                            (travelCard.TravelCardNumber == "654321" && travelCard.CVC == "321") ||
                            (travelCard.TravelCardNumber == "45678" && travelCard.CVC == "456") ||
                            (travelCard.TravelCardNumber == "123456" && travelCard.CVC == "123") ||
                            (travelCard.TravelCardNumber == "987654" && travelCard.CVC == "987"))
                        {
                            return ReturnApiMessage(1, "Kortet spärrad - Mock.", HttpStatusCode.OK);
                        }

                        // Get Card Details from BizTalk
                        #region Get Card Details

                        string cardDetailsResponse =
                            ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.TravelCardNumber);
                        #endregion

                        if (string.IsNullOrWhiteSpace(cardDetailsResponse))
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "BlockTravelCard", ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError), _logger);
                        }

                        // Parse Card Details from response Biztalk
                        #region Parse Card Details from response

                        BiztalkParseCardDetailsMessage cardDetails =
                            ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, cardDetailsResponse);
                        #endregion

                        // Validate Card is valid to Block
                        #region Validate Card

                       
                        response = ValidateCardDetailsFromBiztalkForBlockTravelCard(localContext, threadId, response, cardDetails);

                        //If travel card already is blocked
                        if (response.StatusCode == HttpStatusCode.Continue)
                        {
                            response.StatusCode = HttpStatusCode.OK;
                            return response;
                        }

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            return response;
                        }

                        #endregion

                        // Block Travel Card
                        #region Block Card

                        //Set block reason to 5 = Other (See ST-304)
                        string blockCardDetails = ValueCodeHandler.CallBlockCardBiztalkAction(localContext, travelCard.TravelCardNumber, (int)TravelCardEntity.BlockCardProductReasonCode.Other);

                        // Parse Block Card details from response Biztalk
                        #region Parse Block Card details from response

                        string blockResponse = ValueCodeHandler.CallParseBlockCardFromBiztalkAction(localContext, blockCardDetails);

                        #endregion

                        // Validate Block Card response
                        #region Validate Block Card response

                        var blockStatus = ValidateBlockResponse(localContext, threadId, blockResponse);
                        if (!blockStatus.IsSuccessStatusCode)
                            return blockStatus;

                        #endregion


                        #endregion

                        #region Create travel card in CRM

                        var queryTravelCard = new QueryExpression()
                        {
                            EntityName = TravelCardEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_Blocked),
                            Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, cardDetails.CardNumberField)
                            }
                        }
                        };

                        var crmTravelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, queryTravelCard);

                        /*
                         * Create a new travel card if one is not found in CRM. Note that this travel card at this point
                         * will not be associated with a contact since no info regarding contact is given.
                         */
                        if (crmTravelCard == null)
                        {
                            var newTravelCard = new TravelCardEntity()
                            {
                                cgi_travelcardnumber = travelCard.TravelCardNumber,
                                cgi_TravelCardCVC = travelCard.CVC,
                                ed_RequestedValueCodeForCard = false,
                                cgi_Blocked = true
                            };

                            newTravelCard.Id = XrmHelper.Create(localContext, newTravelCard);
                        }
                        else
                        {
                            var updateTraverlCard = new TravelCardEntity()
                            {
                                Id = crmTravelCard.Id,
                                cgi_Blocked = true,
                                ed_RequestedValueCodeForCard = false
                            };
                            XrmHelper.Update(localContext, updateTraverlCard);
                        }

                        #endregion

                        // Return OK
                        #region Return OK Message

                        response = ReturnApiMessage(threadId,
                                    ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked),
                                    HttpStatusCode.OK);

                        return response;

                        #endregion
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

        /// <summary>
        /// Blocks travel card dispite balance or existing periods.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="response"></param>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        public HttpResponseMessage ValidateCardDetailsFromBiztalkForBlockTravelCard(Plugin.LocalPluginContext localContext, int threadId, HttpResponseMessage response, BiztalkParseCardDetailsMessage cardDetails)
        {
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if (cardDetails.CardHotlistedField == true)
            {
                /*
                 *In case travel card has been blocked from somewhere else outside this API,
                 * we'll ensure that the Blocked status is updated.
                 */
                #region Verify blocked status
                var queryTravelCard = new QueryExpression()
                {
                    EntityName = TravelCardEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_Blocked),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, cardDetails.CardNumberField)
                        }
                    }
                };

                var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, queryTravelCard);
                if (travelCard?.cgi_Blocked == null || travelCard?.cgi_Blocked == false)
                {
                    var updateTravelCard = new TravelCardEntity() { Id = travelCard.Id, cgi_Blocked = true };
                    XrmHelper.Update(localContext, updateTravelCard);
                }
                #endregion

                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked),
                        HttpStatusCode.Continue);
            }

            return response = new HttpResponseMessage(HttpStatusCode.OK);
        }


        #region Helpers
        private HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {
            return Request.CreateResponse(code, errorMessage);
        }

        /// <summary>
        /// Fethces travel card from CRM
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="columnSet"></param>
        /// <returns></returns>
        private TravelCardEntity FetchTravelCardFromCRM(Plugin.LocalPluginContext localContext, string travelCardNumber, ColumnSet columnSet)
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
        /// Validate block response message.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="threadId"></param>
        /// <param name="blockResponse"></param>
        /// <returns></returns>
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

        #endregion

    }
}