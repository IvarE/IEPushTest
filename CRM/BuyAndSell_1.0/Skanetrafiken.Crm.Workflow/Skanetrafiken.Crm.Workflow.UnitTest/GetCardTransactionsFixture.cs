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
    public class GetCardTrasactionsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestGetCardTransactions()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                //GetCardTransactions.ExecuteCodeActivity(localContext, "123321123", "1", "", "");
            
                CallWorkFlowGetCardTransactions(localContext, "123321123", "1", "", "");
            }
        }

        private void CallWorkFlowGetCardTransactions(Plugin.LocalPluginContext localContext, string cardNumber, string maxTransactions, string dateFrom, string dateTo)
        {

            OrganizationRequest request = new OrganizationRequest("ed_GetCardTransactionsActionWorkflow");
            request["TravelCardNumber"] = cardNumber;
            request["MaxTransactions"] = maxTransactions;
            request["DateFrom"] = dateFrom;
            request["DateTo"] = dateTo;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["CardTransactionsResponse"];
            
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
