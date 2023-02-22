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
using Skanetrafiken.Crm;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class GetDirectJourneysFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestGetDirectJourneys()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                // CallGetDirectJourneys(localContext, "9021012007013000", "9021012093004000", "2016-02-09T10:33:02.447Z", "", "TRAIN");

                //string str = GetDirectJourneys.ExecuteCodeActivity(localContext, "9021012007013000", "9021012093004000", "2016-02-09T10:33:02.447Z", "", "TRAIN");

            }
        }

        private void CallGetDirectJourneys(Plugin.LocalPluginContext localContext, string fromStopAreaGid, string toStopAreaGid, string tripDateTime, string forLineGids, string transportType)
        {

            OrganizationRequest request = new OrganizationRequest("ed_GetDirectJourneysActionWorkflow");
            request["FromStopAreaGid"] = fromStopAreaGid;
            request["ToStopAreaGid"] = toStopAreaGid;
            request["TripDateTime"] = tripDateTime;
            request["ForLineGids"] = forLineGids;
            request["TransportType"] = transportType;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["DirectJourneysResponse"];
            
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
