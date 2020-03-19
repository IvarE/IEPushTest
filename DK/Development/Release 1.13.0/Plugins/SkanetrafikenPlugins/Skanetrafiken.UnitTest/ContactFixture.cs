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

//using Generated = Skanetrafiken.Crm.Schema.Generated;
//using Skanetrafiken.Crm;
//using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Threading;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture(Category = "Plugin")]
    public class ContactFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Explicit]
        public void TestContactPostUpdate()
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

                // Testcode
                CRM2013.SkanetrafikenPlugins.contact_Post.UpdateQueueItems("4770579D-F4AF-E611-8113-00155D0A6B01", "Dummy full name", localContext.OrganizationService);

                bool bRunning = true;

                Thread myThread = new System.Threading.Thread(delegate () {
                    for (int i = 0; i < 1000; i++)
                    {
                        localContext.Trace($"Executing in Thread #1 {i}");
                        CRM2013.SkanetrafikenPlugins.contact_Post.UpdateQueueItems("F5E64D95-50EC-E411-80D8-005056903A38", "Dummy full name", localContext.OrganizationService);
                    }
                });
                stopwatch.Restart();
                myThread.Start();


                // Run for 10 sek
                while(bRunning)
                {
                    localContext.Trace("Executing in main Thread.");
                    CRM2013.SkanetrafikenPlugins.contact_Post.UpdateQueueItems("C2AC6004-3AEB-E411-80D8-005056903A38", "Dummy full name", localContext.OrganizationService);

                    if (stopwatch.ElapsedMilliseconds > 7000)
                    {
                        myThread.Abort();
                        Thread.Sleep(50); // Allow thread to complete
                        bRunning = false;
                    }

                }

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
