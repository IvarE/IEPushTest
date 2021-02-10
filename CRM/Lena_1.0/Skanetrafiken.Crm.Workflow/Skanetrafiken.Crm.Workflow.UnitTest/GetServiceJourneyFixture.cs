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
    public class GetServiceJourneyFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestGetServiceJourney()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                // CRM REQUEST
                OrganizationRequest request = new OrganizationRequest("ed_GetServiceJourneyActionWorkflow");

                OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

                string responsetext = (string)response["GetServiceJourneyResponse"];
                
                // DIRECT CALL
                string responsetext2 = GetServiceJourney.ExecuteCodeActivity(localContext, "", "", ""); // TODO ADD VALUES
                
                // ASSERT SOMEHTING?
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
