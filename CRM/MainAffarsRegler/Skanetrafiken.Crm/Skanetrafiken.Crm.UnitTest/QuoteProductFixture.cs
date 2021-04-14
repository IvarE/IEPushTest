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
    [TestFixture]
    public class QuoteProductFixture : PluginFixtureBase
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

                QuoteProductEntity quoteProduct = new QuoteProductEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    QuoteId = new EntityReference(QuoteEntity.EntityLogicalName, new Guid("06d8b0ef-af6a-eb11-9479-005056b6fa28")),
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
                    ProductId = new EntityReference(ProductEntity.EntityLogicalName, new Guid("a4fa5f5f-86a1-ea11-80f9-005056b61fff"))
                };

                Guid quoteProductId = XrmHelper.Create(localContext, quoteProduct);

                quoteProduct.Id = quoteProductId;
                quoteProduct.QuoteDetailId = quoteProductId;

                QuoteProductEntity.HandleQuoteProductEntityCreate(localContext, quoteProduct);
            }
        }

        [Test, Category("Debug")]
        public void PostQuoteProductUpdate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                

                QuoteProductEntity preImage = XrmRetrieveHelper.Retrieve<QuoteProductEntity>(localContext, QuoteProductEntity.EntityLogicalName,Guid.Parse("d7f330f0-109d-eb11-947e-005056b6fa28"),new ColumnSet(true));

                QuoteProductEntity target = new QuoteProductEntity()
                {
                    QuoteDetailId = new Guid("d7f330f0-109d-eb11-947e-005056b6fa28"),
                    Id = new Guid("d7f330f0-109d-eb11-947e-005056b6fa28"),
                    ed_FromDate = preImage.ed_FromDate.Value.AddDays(4),
                    ed_ToDate = preImage.ed_ToDate.Value.AddDays(5)
                };

                //XrmHelper.Update(localContext,);

                QuoteProductEntity.HandleQuoteProductEntityUpdate(localContext, target,preImage);
            }
        }
        [Test, Category("Debug")]
        public void CreateAndSendValueCodeByEmailOnRefundCreate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                Guid MarcusStenswedContact = new Guid("6A53CD21-20C6-E711-80EF-005056B64D75");
                IncidentEntity incident = new IncidentEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    CaseTypeCode = Generated.incident_casetypecode.Travelwarranty,
                    Title = "Test Send Value Code Refund",
                    cgi_Contactid = new EntityReference(ContactEntity.EntityLogicalName, MarcusStenswedContact),
                    CustomerId = new EntityReference(ContactEntity.EntityLogicalName, MarcusStenswedContact),
                    cgi_EmailAddress = "marcus.stenswed@endeavor.se",
                    Description = "Test för utskick av värdekod!"
                };

                Guid incidentId = XrmHelper.Create(localContext, incident);

                RefundEntity refund = new RefundEntity
                {
                    cgi_Contactid = incident.cgi_Contactid,
                    cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, incidentId),
                    // Refund type is value code
                    cgi_RefundTypeid = new EntityReference(Generated.cgi_refundtype.EntityLogicalName, new Guid("5EBAC4F4-6B9C-E811-8276-00155D010B00")),
                    cgi_ReimbursementFormid = new EntityReference(Generated.cgi_reimbursementform.EntityLogicalName, new Guid("3FA843A0-5EA4-E811-8276-00155D010B00")),
                    cgi_Amount = new Money(50),
                    cgi_email = incident.cgi_EmailAddress
                };
                refund.Id = XrmHelper.Create(localContext, refund);

                QueryExpression queryValueCode = new QueryExpression
                {
                    EntityName = ValueCodeEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                    {
                        new ConditionExpression(ValueCodeEntity.Fields.ed_Case, ConditionOperator.Equal, incidentId)
                    }
                    }
                };

                ValueCodeEntity valCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, queryValueCode);
                while (valCode == null)
                {
                    valCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, queryValueCode);
                }

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(EmailEntity.Fields.RegardingObjectId, ConditionOperator.Equal, valCode.Id);
                EmailEntity email = XrmRetrieveHelper.RetrieveFirst<EmailEntity>(localContext, new ColumnSet(true), filter);

                while (email == null)
                {
                    email = XrmRetrieveHelper.RetrieveFirst<EmailEntity>(localContext, new ColumnSet(true), filter);
                }
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
