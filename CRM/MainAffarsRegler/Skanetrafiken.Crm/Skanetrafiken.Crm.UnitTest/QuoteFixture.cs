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
using Microsoft.Xrm.Sdk.Metadata;


namespace Endeavor.Crm.UnitTest
{
    public class QuoteFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void UpdateQuoteDiscount()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());



                QuoteEntity preImage = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, QuoteEntity.EntityLogicalName, Guid.Parse("82b304bd-6d70-eb11-947d-005056b6fa28"), new ColumnSet(QuoteEntity.Fields.DiscountAmount,QuoteEntity.Fields.DiscountPercentage));

                QuoteEntity target = new QuoteEntity()
                {
                    QuoteId = new Guid("82b304bd-6d70-eb11-947d-005056b6fa28"),
                    Id = new Guid("82b304bd-6d70-eb11-947d-005056b6fa28"),
                    DiscountPercentage = 10
                };

                //XrmHelper.Update(localContext,);

                QuoteEntity.HandleQuoteEntityUpdate(localContext, target, preImage);
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
