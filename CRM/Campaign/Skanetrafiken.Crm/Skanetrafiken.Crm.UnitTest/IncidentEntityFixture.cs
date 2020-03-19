using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class IncidentEntityFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Explicit]
        public void TestCreateIncident()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                IncidentEntity incident = new IncidentEntity();
                incident.CaseOriginCode = new OptionSetValue(3);
                incident.cgi_sendtoqueue = new OptionSetValue(285050000);
                incident.Title = "";
                incident.cgi_Contactid = new EntityReference(ContactEntity.EntityLogicalName, new Guid("d3f19a93-28df-e611-80f8-00505690700f"));
                incident.CustomerId = new EntityReference(ContactEntity.EntityLogicalName, new Guid("d3f19a93-28df-e611-80f8-00505690700f"));
                incident.cgi_EmailAddress = "HT.st_test001@mailinator.com";
                incident.cgi_ContactCustomer = true;
                incident.cgi_emailcount = "1";
                incident.cgi_ActionDate = DateTime.Now;
                incident.Description = "Cert rollover!";

                localContext.OrganizationService.Create(incident);

                 localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
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
