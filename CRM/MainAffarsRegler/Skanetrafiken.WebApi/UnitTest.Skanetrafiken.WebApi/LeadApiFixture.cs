using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Samples;
using System.Net;
using System.IO;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm;
using System.Threading;
using Skanetrafiken.Crm.Controllers;
using System.Net.Http;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class LeadApiFixture : PluginFixtureBase
    {
        #region Configs
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
        #endregion

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, Explicit, Category("Debug")]
        public void ValidateEmailMittKonto()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                int threadId = Thread.CurrentThread.ManagedThreadId;

                Guid guid = Guid.Empty;
                string id = string.Empty;
                string mklId = string.Empty;

                HttpResponseMessage rmValidateLead = CrmPlusControl.ValidateEmail(threadId, guid, LeadEntity.EntityTypeCode, id, mklId);
                HttpResponseMessage rmValidateContact = CrmPlusControl.ValidateEmail(threadId, guid, ContactEntity.EntityTypeCode, id, mklId);

                HttpResponseMessage rmValidateKampanj = CrmPlusControl.ValidateEmailKampanj(threadId, guid);
                

            }
        }
    }
}
