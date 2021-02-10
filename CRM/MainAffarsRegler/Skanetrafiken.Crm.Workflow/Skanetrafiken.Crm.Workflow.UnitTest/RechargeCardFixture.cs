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
using System.Threading;
using Microsoft.Crm.Sdk.Messages;
using System.Xml;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class RechargeCardFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestRechargeCard()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                //string response = RechargeCard.ExecuteCodeActivity(localContext, "123321123");

                /*
                int Start, End;
                String StatusCode = "";
                if (response.Contains("<StatusCode>") && response.Contains("</StatusCode>"))
                {
                    Start = response.IndexOf("<StatusCode>", 0) + "<StatusCode>".Length;
                    End = response.IndexOf("</StatusCode>", Start);
                    StatusCode = response.Substring(Start, End - Start);
                }

                NUnit.Framework.Assert.AreEqual(StatusCode, "200", String.Format("Error in BizTalk server, StatusCode {0}", StatusCode));
                */

                CallWorkflowRechargeCard(localContext, "123321123");
                
            }
        }

        private void CallWorkflowRechargeCard(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {

            OrganizationRequest request = new OrganizationRequest("ed_RechargeCardActionWorkflow");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["RechargeCardResponse"];

            // TODO Assert something with response?

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
