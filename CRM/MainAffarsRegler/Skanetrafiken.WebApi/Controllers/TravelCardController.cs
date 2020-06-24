using Newtonsoft.Json;
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
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using System.Globalization;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using System.Threading.Tasks;
using System.Text;

namespace Skanetrafiken.Crm.Controllers
{
    public class TravelCardController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly TaskFactory _taskFactory = new
        TaskFactory(CancellationToken.None,
                    TaskCreationOptions.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.Default);

        private static IConfidentialClientApplication _msalApplication => _msalApplicationFactory.Value;

        private static Lazy<IConfidentialClientApplication> _msalApplicationFactory =
            new Lazy<IConfidentialClientApplication>(() =>
            {
                var certificate = Identity.GetCertToUse("crm-sekundfasaden-acc-sp");
                _log.DebugFormat($"<----- Initializing: Cert - {certificate?.Subject} ----->");

                //var TentantId = "e1fcb9f3-e5f9-496f-a583-e495dfd57497";
                var authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/e1fcb9f3-e5f9-496f-a583-e495dfd57497");

                _log.DebugFormat($"<----- Initializing: Authority - {authority} ----->");


                _log.DebugFormat($"<----- Initializing: ConfidentialClientApplication ----->");

                return ConfidentialClientApplicationBuilder
                    .Create("9e84b58e-20aa-4ceb-aa89-abd98253afd2")
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(authority))
                    .Build();
            });

        [HttpGet]
        public HttpResponseMessage GetCardWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetCardWithCardNumber called with parameter: {cardNumber}");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }

            try
            {
                _log.DebugFormat($"GetCardWithCardNumber: Calling -> _msalApplication.AcquireTokenForClient");
                AuthenticationResult authenticationResponse = _taskFactory
                    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceacc/.default" }).ExecuteAsync)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                if (authenticationResponse == null)
                {
                    _log.DebugFormat($"GetCardWithCardNumber: Error -> Could not aquire token for client!");
                    HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    badReq.Content = new StringContent("Error: Could not aquire token for client!");
                    _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                    return badReq;
                }

                _log.DebugFormat($"GetCardWithCardNumber: Checking AccessToken -> {authenticationResponse?.AccessToken}");

                string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/card/";
                _log.DebugFormat($"GetCardWithCardNumber: Endpoint to use for Jojo Card -> {endPoint}");

                _log.DebugFormat($"GetCardWithCardNumber: Building Jojo Card GetCard GET Call...");
                var myUri = new Uri(endPoint);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                myHttpWebRequest.Headers.Add("20", "*/*");
                myHttpWebRequest.Method = "GET";

                _log.DebugFormat($"GetCardWithCardNumber: Calling Jojo Card GetCard GET...");
                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();

                responseStream.Close();
                myWebResponse.Close();

                var returnJson = new StringContent(json);
                _log.DebugFormat($"GetCardWithCardNumber: Jojo Card GET returned -> {returnJson}");

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = returnJson;
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content}\n");
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
        }

        [HttpGet]
        public HttpResponseMessage PlaceOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PlaceOrderWithCardNumber called with parameter: {cardNumber}");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }

            try
            {
                _log.DebugFormat($"PlaceOrderWithCardNumber: Calling -> _msalApplication.AcquireTokenForClient");
                AuthenticationResult authenticationResponse = _taskFactory
                    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceacc/.default" }).ExecuteAsync)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                if (authenticationResponse == null)
                {
                    _log.DebugFormat($"PlaceOrderWithCardNumber: Error -> Could not aquire token for client!");
                    HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    badReq.Content = new StringContent("Error: Could not aquire token for client!");
                    _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                    return badReq;
                }

                _log.DebugFormat($"PlaceOrderWithCardNumber: Checking AccessToken -> {authenticationResponse?.AccessToken}");

                string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/placeOrder/";
                _log.DebugFormat($"PlaceOrderWithCardNumber: Endpoint to use for Jojo Card -> {endPoint}");

                _log.DebugFormat($"PlaceOrderWithCardNumber: Building Jojo Card PlaceOrder POST Call...");
                var myUri = new Uri(endPoint);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;

                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                myHttpWebRequest.Headers.Add("20", "*/*");

                myHttpWebRequest.ContentLength = 0;
                myHttpWebRequest.Method = "POST";

                _log.DebugFormat($"PlaceOrderWithCardNumber: Calling Jojo Card PlaceOrder POST...");
                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();

                responseStream.Close();
                myWebResponse.Close();

                var returnJson = new StringContent(json);
                _log.DebugFormat($"PlaceOrderWithCardNumber: Jojo Card PlaceOrder POST returned -> {returnJson}");

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = returnJson;
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content}\n");
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
        }

        [HttpGet]
        public HttpResponseMessage CancelOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - CancelOrderWithCardNumber called with parameter: {cardNumber}");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }

            try
            {
                _log.DebugFormat($"CancelOrderWithCardNumber: Calling -> _msalApplication.AcquireTokenForClient");
                AuthenticationResult authenticationResponse = _taskFactory
                    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceacc/.default" }).ExecuteAsync)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                if (authenticationResponse == null)
                {
                    _log.DebugFormat($"CancelOrderWithCardNumber: Error -> Could not aquire token for client!");
                    HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    badReq.Content = new StringContent("Error: Could not aquire token for client!");
                    _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                    return badReq;
                }

                _log.DebugFormat($"CancelOrderWithCardNumber: Checking AccessToken -> {authenticationResponse?.AccessToken}");

                string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/cancelOrder/";
                _log.DebugFormat($"CancelOrderWithCardNumber: Endpoint to use for Jojo Card -> {endPoint}");

                _log.DebugFormat($"CancelOrderWithCardNumber: Building Jojo Card CancelOrder POST Call...");
                var myUri = new Uri(endPoint);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                myHttpWebRequest.Headers.Add("Card-Number", cardNumber);

                myHttpWebRequest.ContentLength = 0;
                myHttpWebRequest.Headers.Add("20", "*/*");
                myHttpWebRequest.Method = "POST";

                _log.DebugFormat($"GetCardWithCardNumber: Calling Jojo Card CancelOrder POST...");
                var myWebResponse = myWebRequest.GetResponse();
                //Check Status and return ok or not ok

                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();

                responseStream.Close();
                myWebResponse.Close();

                var returnJson = new StringContent(json);
                _log.DebugFormat($"GetCardWithCardNumber: Jojo Card CancelOrder POST returned -> {returnJson}");

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = returnJson;
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content}\n");
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
        }

        [HttpGet]
        public HttpResponseMessage CaptureOrderWithCardNumber(string cardNumber)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - CaptureOrderWithCardNumber called with parameter: {cardNumber}");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
                _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }

            try
            {
                _log.DebugFormat($"CaptureOrderWithCardNumber: Calling -> _msalApplication.AcquireTokenForClient");
                AuthenticationResult authenticationResponse = _taskFactory
                    .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceacc/.default" }).ExecuteAsync)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                if (authenticationResponse == null)
                {
                    _log.DebugFormat($"CaptureOrderWithCardNumber: Error -> Could not aquire token for client!");
                    HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    badReq.Content = new StringContent("Error: Could not aquire token for client!");
                    _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                    return badReq;
                }

                _log.DebugFormat($"CaptureOrderWithCardNumber: Checking AccessToken -> {authenticationResponse?.AccessToken}");

                string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/captureOrder/";
                _log.DebugFormat($"CaptureOrderWithCardNumber: Endpoint to use for Jojo Card -> {endPoint}");

                _log.DebugFormat($"CaptureOrderWithCardNumber: Building Jojo Card CaptureOrder POST Call...");
                var myUri = new Uri(endPoint);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
                myHttpWebRequest.Headers.Add("20", "*");
                myHttpWebRequest.ContentLength = 0;
                myHttpWebRequest.Method = "POST";

                _log.DebugFormat($"CaptureOrderWithCardNumber: Calling Jojo Card CaptureOrder POST...");
                var myWebResponse = myWebRequest.GetResponse();
                //Check Status and return ok or not ok

                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();

                responseStream.Close();
                myWebResponse.Close();

                var returnJson = new StringContent(json);
                _log.DebugFormat($"CaptureOrderWithCardNumber: Jojo Card CaptureOrder POST returned -> {returnJson}");

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = returnJson;
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content}\n");
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
        }

        //Testing New API Call
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);

            //var cardNumber = "9999184770";
            //int threadId = Thread.CurrentThread.ManagedThreadId;
            //_log.Info($"Th={threadId} - GetCardWithCardNumber called with parameter: {cardNumber}");

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //if (string.IsNullOrWhiteSpace(cardNumber))
            //{
            //    HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //    badReq.Content = new StringContent("Could not find a 'CardNumber' parameter in url");
            //    _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
            //    return badReq;
            //}

            //try
            //{
            //    _log.DebugFormat($"GetCardWithCardNumber: Calling -> _msalApplication.AcquireTokenForClient");
            //    AuthenticationResult authenticationResponse = _taskFactory
            //        .StartNew(_msalApplication.AcquireTokenForClient(new[] { "https://skanetrafiken.se/apps/jojocardserviceacc/.default" }).ExecuteAsync)
            //        .Unwrap()
            //        .GetAwaiter()
            //        .GetResult();

            //    if (authenticationResponse == null)
            //    {
            //        _log.DebugFormat($"GetCardWithCardNumber: Error -> Could not aquire token for client!");
            //        HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
            //        badReq.Content = new StringContent("Error: Could not aquire token for client!");
            //        _log.Info($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
            //        return badReq;
            //    }

            //    _log.DebugFormat($"GetCardWithCardNumber: Checking AccessToken -> {authenticationResponse?.AccessToken}");

            //    string endPoint = "https://stjojocardserviceacc.azurewebsites.net/v1/card/";
            //    _log.DebugFormat($"GetCardWithCardNumber: Endpoint to use for Jojo Card -> {endPoint}");

            //    _log.DebugFormat($"GetCardWithCardNumber: Building Jojo Card GET Call...");
            //    var myUri = new Uri(endPoint);
            //    var myWebRequest = WebRequest.Create(myUri);
            //    var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            //    myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
            //    myHttpWebRequest.Headers.Add("Card-Number", cardNumber);
            //    myHttpWebRequest.Headers.Add("20", "*/*");
            //    myHttpWebRequest.Method = "GET";

            //    _log.DebugFormat($"GetCardWithCardNumber: Calling Jojo Card GET...");
            //    var myWebResponse = myWebRequest.GetResponse();
            //    var responseStream = myWebResponse.GetResponseStream();
            //    if (responseStream == null) return null;

            //    var myStreamReader = new StreamReader(responseStream, Encoding.Default);
            //    var json = myStreamReader.ReadToEnd();

            //    responseStream.Close();
            //    myWebResponse.Close();

            //    var returnJson = new StringContent(json);
            //    _log.DebugFormat($"GetCardWithCardNumber: Jojo Card GET returned -> {returnJson}");

            //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            //    resp.Content = returnJson;
            //    _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content}\n");
            //    return resp;
            //}
            //catch (Exception ex)
            //{
            //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
            //    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            //    return rm;
            //}
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
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(travelCard)}");

            Plugin.LocalPluginContext localContext = null;

            var response = new HttpResponseMessage();

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

                    _log.Debug($"Running BlockTravelCard");
                    _log.Debug($"Input parameter:\nTravelCardNumber '{travelCard?.TravelCardNumber}'\nCVC: '{travelCard?.CVC}'.");

                    if (string.IsNullOrWhiteSpace(travelCard.TravelCardNumber) || string.IsNullOrWhiteSpace(travelCard.CVC))
                    {
                        _log.Debug($"TravelCardNumber or CVC is empty.");
                        var resp = ReturnApiMessage(threadId,
                            ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC),
                            HttpStatusCode.BadRequest);
                        return resp;
                    }

                    if ((travelCard.TravelCardNumber == "123456" && travelCard.CVC == "123") ||
                        (travelCard.TravelCardNumber == "654321" && travelCard.CVC == "321") ||
                        (travelCard.TravelCardNumber == "45678" && travelCard.CVC == "456") ||
                        (travelCard.TravelCardNumber == "123456" && travelCard.CVC == "123") ||
                        (travelCard.TravelCardNumber == "987654" && travelCard.CVC == "987"))
                    {
                        _log.Debug($"User passed mocked travel cards.");
                        return ReturnApiMessage(1, "Kortet spärrad - Mock.", HttpStatusCode.OK);
                    }

                    // Get Card Details from BizTalk
                    #region Get Card Details

                    _log.Debug($"Calling ed_GetCardDetails workflow.");
                    string cardDetailsResponse =
                        ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.TravelCardNumber);
                    _log.Debug($"GetCardDetails call ran successfully.");
                    #endregion

                    if (string.IsNullOrWhiteSpace(cardDetailsResponse))
                    {
                        _log.Debug($"Response from GetCardDetails is empty.");
                        return ReturnApiMessage(threadId,
                             ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                             HttpStatusCode.BadRequest);
                    }

                    // Parse Card Details from response Biztalk
                    #region Parse Card Details from response

                    _log.Debug($"Running ed_ParseBiztalkResponse workflow.");
                    BiztalkParseCardDetailsMessage cardDetails =
                        ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, cardDetailsResponse);
                    _log.Debug($"ParseCardDetails ran successfully.");
                    _log.Debug($"Response parameters for validation -\nTravelCardNumber: {cardDetails.CardNumberField}\nHotlisted: {cardDetails.CardHotlistedField}");
                    #endregion


                    // Validate Card is valid to Block
                    #region Validate Card

                    _log.Debug($"(TravelCardEntity.ValidateCardDetailsFromBiztalkForBlockTravelCard) Validating parsed travel card from BIFF.");
                    response = ValidateCardDetailsFromBiztalkForBlockTravelCard(localContext, threadId, response, cardDetails);

                    _log.Debug($"Result from validation - CardNumber: {cardDetails?.CardNumberField} StatusCode: {response?.StatusCode}\n" +
                        $"CardHotlisted: {cardDetails?.CardHotlistedField} StatusCode: {response?.StatusCode}");


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

                    _log.Debug($"Calling ed_BlockCardBiztalk workflow to block travel card '{travelCard?.TravelCardNumber}'.");
                    //Set block reason to 5 = Other (See ST-304)
                    string blockCardDetails = ValueCodeHandler.CallBlockCardBiztalkAction(localContext, travelCard.TravelCardNumber, (int)TravelCardEntity.BlockCardProductReasonCode.Other);

                    _log.Debug($"BlockCardBiztalk ran successfully.");

                    // Parse Block Card details from response Biztalk
                    #region Parse Block Card details from response

                    _log.Debug($"Calling ed_ParseBlockCardResponseFromBiztalk workflow.");
                    string blockResponse = ValueCodeHandler.CallParseBlockCardFromBiztalkAction(localContext, blockCardDetails);

                    _log.Debug($"ParseBlockCardFromBiztalk ran successfully.");
                    #endregion

                    // Validate Block Card response
                    #region Validate Block Card response

                    var blockStatus = ValidateBlockResponse(localContext, threadId, blockResponse);
                    if (!blockStatus.IsSuccessStatusCode)
                        return blockStatus;

                    _log.Debug($"Validation from response is OK.");
                    #endregion


                    #endregion


                    #region Create travel card in CRM

                    _log.Debug($"Query given travel card number in CRM to see if one exists.");
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
                        _log.Debug($"Could not find given travel card '{cardDetails?.CardNumberField}' in CRM. Create a new travel card.");
                        var newTravelCard = new TravelCardEntity()
                        {
                            cgi_travelcardnumber = travelCard.TravelCardNumber,
                            cgi_TravelCardCVC = travelCard.CVC,
                            ed_RequestedValueCodeForCard = false,
                            cgi_Blocked = true
                        };

                        newTravelCard.Id = XrmHelper.Create(localContext, newTravelCard);

                        _log.Debug($"Newly created travel card:\nId: {newTravelCard?.Id}\n" +
                            $"cgi_travelcardnumber: {newTravelCard?.cgi_travelcardnumber}\n" +
                            $"cgi_TravelCardCVC: {newTravelCard?.cgi_TravelCardCVC}\n" +
                            $"ed_RequestedValueCodeForCard: {newTravelCard?.ed_RequestedValueCodeForCard}\n" +
                            $"cgi_Blocked: {newTravelCard?.cgi_Blocked}");
                    }
                    else
                    {
                        _log.Debug($"Found given travel card '{cardDetails?.CardNumberField}' in CRM.");
                        var updateTraverlCard = new TravelCardEntity()
                        {
                            Id = crmTravelCard.Id,
                            cgi_Blocked = true,
                            ed_RequestedValueCodeForCard = false
                        };
                        XrmHelper.Update(localContext, updateTraverlCard);

                        _log.Debug($"Updated travel card:\nId: {updateTraverlCard?.Id}\n" +
                            $"ed_RequestedValueCodeForCard: {updateTraverlCard?.ed_RequestedValueCodeForCard}\n" +
                            $"cgi_Blocked: {updateTraverlCard?.cgi_Blocked}");
                    }


                    #endregion

                    // Return OK
                    #region Return OK Message

                    response = ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked),
                                HttpStatusCode.OK);

                    _log.Debug($"Successfully exiting BlockTravelCard api with StatusCode: {response?.StatusCode}");

                    return response;

                    #endregion
                }
            }
            catch (WebException ex)
            {
                _log.Error("WebException caught from BlockTravelCard: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _log.Error("Exception caught from BlockTravelCard: " + ex.Message);

                return ReturnApiMessage(threadId,
                                ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_UnexpectedError),
                                HttpStatusCode.InternalServerError);
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
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
            _log.Debug($"Entering ValidateCardDetailsFromBiztalkForBlockTravelCard");
            if (string.IsNullOrWhiteSpace(cardDetails.CardNumberField))
            {
                _log.Debug($"Travel card number could not be found in BIFF.");
                return response = ReturnApiMessage(threadId,
                        ReturnMessageWebApiEntity.GetValueString(localContext, ReturnMessageWebApiEntity.Fields.ed_TravelCardDoesNotExist),
                        HttpStatusCode.BadRequest);
            }

            if (cardDetails.CardHotlistedField == true)
            {
                _log.Debug($"Travel card '{cardDetails.CardNumberField}' is hotlisted in BIFF.");
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

            _log.Debug($"Exiting ValidateCardDetailsFromBiztalkForBlockTravelCard");
            return response = new HttpResponseMessage(HttpStatusCode.OK);
        }


        #region Helpers
        private HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {
            _log.DebugFormat($"Th={threadId} - Returning statuscode = {code}, Content = {errorMessage}\n");
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

        #endregion

    }
}