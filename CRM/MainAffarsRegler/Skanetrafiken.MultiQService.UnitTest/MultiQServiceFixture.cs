
using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using System.Collections.Generic;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xrm.Sdk;
using System.Linq;
using Endeavor.Crm.UnitTest;
using Endeavor.Crm;

namespace Skanetrafiken.MultiQService.UnitTest
{
    [TestClass]
    public class MultiQServiceFixture : PluginFixtureBase
    {
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

        [TestMethod]
        public void MultiQServiceUploadFiles()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                Skanetrafiken.MultiQService.ReportUploader reportUploader = new Skanetrafiken.MultiQService.ReportUploader();
                reportUploader.ExecuteJob();

                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }

        [Test, Category("Debug")]
        public void TestMethod1()
        {
        }
    }
}
