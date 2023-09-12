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
    [TestFixture]
    public class RefundFixture : PluginFixtureBase
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

                DateTime lastValid = DateTime.Now.AddYears(2).ToLocalTime();

                RefundEntity refund = new RefundEntity
                {
                    cgi_Contactid = incident.cgi_Contactid,
                    cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, incidentId),
                    // Refund type is value code
                    cgi_RefundTypeid = new EntityReference(Generated.cgi_refundtype.EntityLogicalName, new Guid("5EBAC4F4-6B9C-E811-8276-00155D010B00")),
                    cgi_ReimbursementFormid = new EntityReference(Generated.cgi_reimbursementform.EntityLogicalName, new Guid("98F4CC92-5EA4-E811-8276-00155D010B00")),
                    cgi_Amount = new Money(50),
                    cgi_MobileNumber = "+46735198846",
                    cgi_last_valid = lastValid
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
                while(valCode == null)
                {
                    valCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, queryValueCode);
                }

                Assert.Less((lastValid.ToUniversalTime() - valCode.ed_LastRedemptionDate.Value.ToUniversalTime()).Seconds, 30);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(TextMessageEntity.Fields.RegardingObjectId, ConditionOperator.Equal, valCode.Id);
                TextMessageEntity textMessage = XrmRetrieveHelper.RetrieveFirst<TextMessageEntity>(localContext, new ColumnSet(true), filter);

                while(textMessage == null)
                {
                    textMessage = XrmRetrieveHelper.RetrieveFirst<TextMessageEntity>(localContext, new ColumnSet(true), filter);
                }

                Assert.AreNotEqual(textMessage.StatusCode.Value, (int)TextMessageEntity.Status.Failed);
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
