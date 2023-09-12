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
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using Skanetrafiken.Crm.ValueCodes;

namespace Endeavor.Crm.IntegrationTests
{
    [TestFixture]
    public class ValueCodeTemplateFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test]
        public void TestSetTemplateId()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                ValueCodeTemplateEntity template_01 = new ValueCodeTemplateEntity()
                {
                    ed_subject = "template_01",
                    ed_CaseTypeCode = new OptionSetValue((int)ValueCodeTemplateEntity.casetypecode.InjuryIncident)
                };

                template_01.Id = XrmHelper.Create(localContext, template_01);

                template_01 = XrmRetrieveHelper.Retrieve<ValueCodeTemplateEntity>(localContext, template_01.Id, new ColumnSet(true));
                Assert.AreEqual((int)ValueCodeTemplateEntity.casetypecode.InjuryIncident % 10000, template_01.ed_TemplateId.Value);

                template_01.ed_CaseTypeCode = new OptionSetValue((int)ValueCodeTemplateEntity.casetypecode.Praise);
                XrmHelper.Update(localContext, template_01);
                template_01 = XrmRetrieveHelper.Retrieve<ValueCodeTemplateEntity>(localContext, template_01.Id, new ColumnSet(true));
                Assert.AreEqual((int)ValueCodeTemplateEntity.casetypecode.Praise % 10000, template_01.ed_TemplateId.Value);

                Assert.Catch(new TestDelegate(CreateTemplate));

                template_01.statecode = Generated.ed_valuecodetemplateState.Inactive;
                template_01.statuscode = new OptionSetValue(2);
                XrmHelper.Update(localContext, template_01);

                Assert.DoesNotThrow(new TestDelegate(CreateTemplate));

                XrmHelper.Delete(localContext, template_01.ToEntityReference());

            }
        }

        private void CreateTemplate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                ValueCodeTemplateEntity template_02 = new ValueCodeTemplateEntity()
                {
                    ed_subject = "template_02",
                    ed_CaseTypeCode = new OptionSetValue((int)ValueCodeTemplateEntity.casetypecode.Praise)
                };
                template_02.Id = XrmHelper.Create(localContext, template_02);

                XrmHelper.Delete(localContext, template_02.ToEntityReference());
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