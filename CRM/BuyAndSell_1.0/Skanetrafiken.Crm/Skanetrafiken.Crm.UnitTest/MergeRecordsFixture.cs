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
    public class MergeRecordsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test]
        public void MergeContacts()
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

                // Create first contact
                ContactEntity fromContact = ContactFixture.CreateUnitTestContact(localContext);

                // Create second contact
                ContactEntity toContact = ContactFixture.CreateUnitTestContact(localContext);

                localContext.TracingService.Trace("Performing merge on {0} to {1}", fromContact.FirstName+" " + fromContact.LastName, toContact.FirstName +" "+toContact.LastName);

                // Build entity
                MergeRecordsEntity merger = new MergeRecordsEntity();
                merger.ed_MergeFromContact = fromContact.Id.ToString();
                merger.ed_MergeToContact = toContact.Id.ToString();

                // Perform merge
                merger.PerformeMerge(localContext);

                merger.Trace(localContext.TracingService);

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
