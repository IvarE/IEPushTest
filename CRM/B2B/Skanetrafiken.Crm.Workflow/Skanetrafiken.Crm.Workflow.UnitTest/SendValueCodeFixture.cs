using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Skanetrafiken.Crm.TextMessageSender;
using Skanetrafiken.Crm.ValueCodes;
using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    public class SendValueCodeFixture : PluginFixtureBase
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


        //[Test, Category("Debug")]
        //public void SendValueCodeAsSMS()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        Guid valueCodeId = ValueCodeHandler.CreateValueCode(localContext, 161, 50);


        //        string result = SendValueCode.ExecuteCodeActivity(localContext, valueCode.ToEntityReference());
        //    }
        //}

        [Test]
        public void TestApproveValueCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                ValueCodeApprovalEntity approval = XrmRetrieveHelper.Retrieve<ValueCodeApprovalEntity>(localContext, new Guid("7967B3B3-C44B-E911-80F0-005056B61FFF"), new ColumnSet(true));

                

                //CreateValueCodeGeneric.ExecuteCodeActivity(localContext, "2", (DateTime)approval.ed_ValidTo, approval.ToEntityReference(), approval.ed_Contact,
                //    null, 161, (double)approval.ed_Amount, approval.ed_Mobile, approval.ed_EmailAddress, null, "", "", "", "", "", "", "", "", "", "");

                //ValueCodeApprovalEntity.ChangeState(localContext, approval.ToEntityReference(), ed_valuecodeapproval_statuscode.Approved);

                //localContext.TracingService.Trace($"Creating incident case.");
                //var incidentCase = IncidentEntity.CreateCaseAfterOnlineRefund(localContext, "", "0735198846", "marcus.stenswed@endeavor.se");

                //localContext.TracingService.Trace($"Creating refund.");
                //var refund = RefundEntity.CreateRefundAfterOnlineRefund(localContext, incidentCase, 1);

                ////Update value code
                //var valueCodeWithCaseAndRefund = new ValueCodeEntity { Id = Guid.Parse(valueCodeId), ed_CaseNumber = incidentCase.TicketNumber, ed_Refund = refund.ToEntityReference() };
                //XrmHelper.Update(localContext, valueCodeWithCaseAndRefund);

                //var incidentCaseClose = new IncidentEntity { Id = incidentCase.Id, IncidentStageCode = incident_incidentstagecode.ResolvedApproved };
                //XrmHelper.Update(localContext, incidentCaseClose);
            }
        }

        [Test]
        public void TestSendValueCodeWithLead()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        LeadEntity lead = new LeadEntity()
        //        {
        //            FirstName = "Marcus",
        //            LastName = "Stenswed",
        //            MobilePhone = "+46735198846"
        //        };

        //        lead.Id = XrmHelper.Create(localContext, lead);

        //        /*
        //        ValueCodeEntity SMSValueCode = new ValueCodeEntity()
        //        {
        //            ed_ValueCodeTemplate = new EntityReference(ValueCodeTemplateEntity.EntityLogicalName, new Guid("F42FEB89-CC90-E811-8276-00155D010B00")),
        //            ed_Lead = lead.ToEntityReference(),
        //            ed_TypeOption = Generated.ed_valuecode_ed_typeoption.Mobile
        //        };

        //        SMSValueCode.Id = XrmHelper.Create(localContext, SMSValueCode);
        //        */


                //Guid SMSValueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 50, "+46735198846", templateNumber: 3, lead: lead);

                //SendValueCode.ExecuteCodeActivity(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId));

                //ValueCodeEntity SMSValueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, SMSValueCodeId, new ColumnSet(true));

                //Assert.NotNull(SMSValueCode.ed_Link);
            }
        }

        [Test]
        public void TestResendValueCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                Guid SMSValueCodeId = new Guid("E2221E48-BAC4-E911-80F4-005056B665EC");

                ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId), new ColumnSet(true));
                //ValueCodeEntity valueCode = (ValueCodeEntity)localContext.OrganizationService.Retrieve(ValueCodeEntity.EntityLogicalName, SMSValueCodeId, new ColumnSet(true));
                valueCode.SendValueCode(localContext);

                //SendValueCode.ExecuteCodeActivity(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId));
            }
        }

        //[Test]
        //public void TestSendValueCodeWithContact()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid("6A53CD21-20C6-E711-80EF-005056B64D75"), new ColumnSet(true));

        //        /*
        //        ValueCodeEntity SMSValueCode = new ValueCodeEntity()
        //        {
        //            ed_ValueCodeTemplate = new EntityReference(ValueCodeTemplateEntity.EntityLogicalName, new Guid("F42FEB89-CC90-E811-8276-00155D010B00")),
        //            ed_Lead = lead.ToEntityReference(),
        //            ed_TypeOption = Generated.ed_valuecode_ed_typeoption.Mobile
        //        };

        //        SMSValueCode.Id = XrmHelper.Create(localContext, SMSValueCode);
        //        */


                //Guid SMSValueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 50, "+46735198846", templateNumber: 3, contact: contact);

        //        //SendValueCode.ExecuteCodeActivity(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId));
        //    }
        //}

        ////[Test]
        ////public void TestSendValueCodeWithContactActivity()
        ////{
        ////    // Connect to the Organization service. 
        ////    // The using statement assures that the service proxy will be properly disposed.
        ////    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        ////    {
        ////        // This statement is required to enable early-bound type support.
        ////        _serviceProxy.EnableProxyTypes();

        ////        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        ////        ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid("6A53CD21-20C6-E711-80EF-005056B64D75"), new ColumnSet(true));

        //        //Guid SMSValueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 50, "+46735198846", templateNumber: 3, contact: contact);

        //        //OrganizationRequest request = new OrganizationRequest("ed_SendvalueCode");
        //        //request["Target"] = new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId);

        //        //OrganizationResponse response = localContext.OrganizationService.Execute(request);
        //    }
        //}

        //internal ServerConnection ServerConnection
        //{
        //    get
        //    {
        //        if (_serverConnection == null)
        //        {
        //            _serverConnection = new ServerConnection();
        //        }
        //        return _serverConnection;
        //    }
        //}

        //internal ServerConnection.Configuration Config
        //{
        //    get
        //    {
        //        return TestSetup.Config;
        //    }
        //}
    }
}
