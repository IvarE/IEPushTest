using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


using Microsoft.Crm.Sdk.Messages;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Skanetrafiken.Crm.Entities;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Diagnostics;
using Microsoft.Crm.Sdk.Samples;
using Skanetrafiken.Crm;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class CalculatePriceFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Explicit, Category("Debug")]
        public void CreateClientelingActionAndEmailForContacts()
        {
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());


             OrderProductEntity orderProduct = new OrderProductEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    SalesOrderId = new EntityReference(SalesOrderEntity.EntityLogicalName, new Guid("387fb383-dd01-ed11-9489-005056b6fa28")),
                    SkipPriceCalculation = new OptionSetValue(1),
                    ManualDiscountAmount = new Money(0),
                    WillCall = false,
                    UoMId = new EntityReference(UnitEntity.EntityLogicalName, new Guid("873f0c59-a096-ea11-80f8-005056b61fff")),
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
