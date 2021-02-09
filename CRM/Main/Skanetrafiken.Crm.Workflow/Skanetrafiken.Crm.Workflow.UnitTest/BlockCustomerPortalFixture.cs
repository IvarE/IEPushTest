using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endeavor.Crm.UnitTest;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Endeavor.Crm;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Skanetrafiken.Crm;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class BlockCustomerPortalFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;
        
        [Test, Explicit, Category("Debug")]
        public void MKLGetTest()
        {
            var mklEndpoint = "https://stmkorgapitest.azurewebsites.net/";

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var user = System.Security.Principal.WindowsIdentity.GetCurrent().User;
            var userName = user.Translate(typeof(System.Security.Principal.NTAccount));


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}");
            //string clientCertName = "SE162321000255-F16638";
            string clientCertName = "SE162321000255-F16677";
            request.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
            request.ContentType = "application/json";
            request.Method = "GET";
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        [Test, Explicit, Category("Debug")]
        public void BlockCustomerMKL()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                string response = BlockCustomerPortal.ExecuteCodeActivity(localContext, "199012120417", false);

            }
        }

        [Test, Explicit, Category("Debug")]
        public void BlockAccountMKL()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                BlockAccountPortal.ExecuteCodeActivity(localContext, "965", true);

            }
        }

        [Test, Explicit, Category("Debug")]
        public void BlockCompanyRoleMKL()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                BlockCompanyRolePortal.ExecuteCodeActivity(localContext, "199012120417", "992", true);

            }
        }

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
    }
}
