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

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class CreateCreditOrderFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestCreateCreditOrder()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                //CreateCreditOrder.ExecuteCodeActivity(localContext, "SY3001019", "0", "", "");

                CallWorkflowCreateCreditOrder(localContext, "SY3001019", "0", "", "test", "");
            }
        }
        
        private void CallWorkflowCreateCreditOrder(Plugin.LocalPluginContext localContext, string orderNumber, string productNumber, string credit, string reason, string quantity)
        {

            OrganizationRequest request = new OrganizationRequest("ed_CreateCreditOrderActionWorkflow");
            request["OrderNumber"] = orderNumber;
            request["ProductNumber"] = productNumber;
            request["Credit"] = credit;
            request["Reason"] = reason;
            request["Quantity"] = quantity;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["CreditOrderResponse"];

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
