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
    class QuoteProductFixture : PluginFixtureBase
    {
    
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void CreateAndSendValueCodeBySMSOnRefundCreate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                var testQuoteProduct = new QuoteProductEntity();
                testQuoteProduct.UoMId = new EntityReference(UnitEntity.EntityLogicalName, Guid.Parse("	3ffcc44f-a096-ea11-80f8-005056b61fff"));
                testQuoteProduct.ProductId = new EntityReference(ProductEntity.EntityLogicalName, Guid.Parse("1bffd424-a196-ea11-80f8-005056b61fff"));
                testQuoteProduct.ed_FromDate = DateTime.UtcNow;
                testQuoteProduct.ed_ToDate = DateTime.UtcNow.AddDays(1);

                QuoteProductEntity.HandleQuoteProductEntityCreate(localContext, testQuoteProduct);

            }
}

        //[Test, Category("Debug")]
        //public void CreateAndSendValueCodeByEmailOnRefundCreate()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                
        //    }
        //}

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
