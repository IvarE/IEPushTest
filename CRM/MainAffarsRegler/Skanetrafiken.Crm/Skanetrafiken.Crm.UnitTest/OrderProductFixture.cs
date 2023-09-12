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

namespace Endeavor.Crm.IntegrationTests
{
    public class OrderProductFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void CreateQuoteProduct()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                OrderProductEntity orderProduct = new OrderProductEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    SalesOrderId = new EntityReference(SalesOrderEntity.EntityLogicalName, new Guid("")),
                    SkipPriceCalculation = new OptionSetValue(1),
                    ManualDiscountAmount = new Money(0),
                    WillCall = false,
                    ed_FromDate = DateTime.UtcNow.Date,
                    ed_ToDate = DateTime.UtcNow.Date.AddDays(3),
                    UoMId = new EntityReference(UnitEntity.EntityLogicalName, Guid.Parse("3ffcc44f-a096-ea11-80f8-005056b61fff")),
                    IsPriceOverridden = false,
                    BaseAmount = new Money(5860),
                    IsProductOverridden = false,
                    PricePerUnit = new Money(5860),
                    Quantity = 1,
                    TransactionCurrencyId = new EntityReference(CurrencyEntity.EntityLogicalName, new Guid("7db294f9-53a1-e411-80d4-005056903a38")),
                    ExtendedAmount = new Money(5860),
                    ProductTypeCode = new OptionSetValue(1),
                    PricingErrorCode = new OptionSetValue(0),
                    ProductId = new EntityReference(ProductEntity.EntityLogicalName, new Guid(""))
                };

                Guid orderProductId = XrmHelper.Create(localContext, orderProduct);

                orderProduct.Id = orderProductId;
                orderProduct.SalesOrderDetailId = orderProductId;

                OrderProductEntity.HandleOrderProductEntityCreate(localContext, orderProduct);
            }
        }

        [Test, Category("Debug")]
        public void PostOrderProductUpdate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());
                
                OrderProductEntity preImage = XrmRetrieveHelper.Retrieve<OrderProductEntity>(localContext, OrderProductEntity.EntityLogicalName, Guid.Parse("1a164f39-2ba7-eb11-9480-005056b6fa28"), new ColumnSet(true));

                OrderProductEntity target = new OrderProductEntity()
                {
                    QuoteDetailId = new EntityReference( OrderProductEntity.EntityLogicalName,new Guid("1a164f39-2ba7-eb11-9480-005056b6fa28")),
                    Id = new Guid("1a164f39-2ba7-eb11-9480-005056b6fa28"),
                    //ed_FromDate = preImage.ed_FromDate.Value.AddDays(-1),
                    ed_ToDate = preImage.ed_ToDate.Value.AddDays(1)
                };

                //XrmHelper.Update(localContext,);

                OrderProductEntity.HandleOrderProductEntityUpdate(localContext, target, preImage);
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
