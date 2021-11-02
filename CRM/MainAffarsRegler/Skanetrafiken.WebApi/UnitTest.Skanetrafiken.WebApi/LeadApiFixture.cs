using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Samples;
using System.Net;
using System.IO;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class LeadApiFixture : PluginFixtureBase
    {
        #region Configs
        private ServerConnection _serverConnection;

        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }
        #endregion

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, Explicit, Category("Debug")]
        public void ValidateEmailMittKonto()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var portalId = "583";
                var blocked = true;

                string errorMessage = "MKLRequest returned an exception. Certificate {0}, endpoint {1}, payload {2}. Message: {3}";
                string mklEndpoint = String.Empty;
                string clientCertName = String.Empty;
                string InputJSON = String.Empty;

                try
                {
                    mklEndpoint = CgiSettingEntity.GetSettingString(localContext, "ed_companyintegrationendpoint");
                    localContext.Trace($"Endpoint: {mklEndpoint}");

                    clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                    localContext.Trace($"Certificate: {clientCertName}");

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}/api/block/accounts/{portalId}");

                    httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        // Result is 
                        InputJSON = "{\"accountId\": " + portalId + ", \"blocked\": " + blocked.ToString().ToLower() + "}";
                        localContext.Trace($"Calling MKL with message: {InputJSON}");
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Test", result);
                    }

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;

                    if (response == null)
                    {
                        throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                    }
#if DEBUG
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("SendMKLRequest() returned an exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                    }

#else
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("SendMKLRequest returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(we.Message, we);
                }

#endif
                }

            }
        }

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">Api endpoint</param>
        /// <param name="body"></param>
        /// <param name="destination">Specifies the type of which the body shall be converted to.</param>
        /// <param name="assert">Pass assertion code here</param>
        /// <param name="xrmAction"></param>
        public void CallApi(string endpoint, object body, string httpRequestMethod, Type destination, Action<Plugin.LocalPluginContext, string, HttpWebResponse> assert = null, Action<Plugin.LocalPluginContext> clearDataAction = null)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //Set up http request
                string url = endpoint; //$"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = httpRequestMethod;

                try
                {
                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(Convert.ChangeType(body, destination));
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd().Replace("\"", "");
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);
                        assert(localContext, response, httpResponse);
                    }
                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd().Replace("\"", "");
                            assert(localContext, text, httpResponse);
                        }
                    }
                }
            }
        }


        #endregion
    }
}
