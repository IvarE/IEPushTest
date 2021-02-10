using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Skanetrafiken.Crm.Controllers;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System.Net.Http;
using System.Threading;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class CustomerUtilityFixture : PluginFixtureBase
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

        [Test, Category("Run Always")]
        public void EmailFormatTest()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                #endregion

                List<string> validEmails = new List<string>
                {
                    "a.b@c.d",
                    "éxámple@téstá.com",
                    "ävenåäöfårvaramed@här.nu"
                };

                List<string> invalidEmails = new List<string>
                {
                    "a.b@c",
                    "´xámpLe@téstá.com",
                    "inget mellanslag@is.allowed",
                    "need.alfakrull",
                    "two..is@not.ok"
                };
                foreach (string valid in validEmails)
                {
                    if (!CustomerUtility.CheckEmailFormat(valid))
                    {
                        throw new Exception($"{valid} should be accepted as a valid Email");
                    }
                }
                foreach (string invalid in invalidEmails)
                {
                    if (CustomerUtility.CheckEmailFormat(invalid))
                    {
                        throw new Exception($"{invalid} should not be accepted as a valid Email");
                    }
                }
            }
        }

        [Test, Category("Run Always")]
        public void PersonnummerFormatTest()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                #endregion

                if (!CustomerUtility.CheckPersonnummerFormat("199401204483"))
                {
                    throw new Exception("Personnummerkoll fel");
                }


            }
        }
    }
}
