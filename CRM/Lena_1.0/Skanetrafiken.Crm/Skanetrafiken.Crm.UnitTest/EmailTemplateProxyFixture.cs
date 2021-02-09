using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class EmailTemplateProxyFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test]
        public void TestEmailProxy()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                // unable to create this way...
                TemplateEntity template = new TemplateEntity()
                {
                    Title = "Test Email Proxy Title"
                };

                template.Id = XrmHelper.Create(localContext, template);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(EmailTemplateProxyEntity.Fields.ed_TemplateGuid, ConditionOperator.Equal, template.Id.ToString());
                EmailTemplateProxyEntity proxy = XrmRetrieveHelper.RetrieveFirst<EmailTemplateProxyEntity>(localContext, new ColumnSet(true), filter);

                Assert.NotNull(proxy);

                XrmHelper.Delete(localContext, template.ToEntityReference());
                proxy = XrmRetrieveHelper.RetrieveFirst<EmailTemplateProxyEntity>(localContext, new ColumnSet(true), filter);

                Assert.Null(proxy);
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
