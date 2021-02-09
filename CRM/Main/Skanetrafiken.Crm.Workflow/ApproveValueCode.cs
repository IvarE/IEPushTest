using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.Entities
{
    /// <summary>
    /// Summary: Creates Incident and refund posts in CRM.
    /// </summary>
    public class ApproveValueCode : CodeActivity
    {

        [Input("ValueCodeId")]
        [RequiredArgument()]
        //[ReferenceTarget("ed_valuecode")]
        public InArgument<string> ValueCodeId { get; set; }

        [Input("ValueCodeApprovalId")]
        [ReferenceTarget("ed_valuecodeapproval")]
        public InArgument<EntityReference> ValueCodeApprovalId { get; set; }

        [Input("Mobile")]
        public InArgument<string> Mobile { get; set; }

        [Input("Email")]
        public InArgument<string> Email { get; set; }

        [Input("TravelCardNumber")]
        public InArgument<string> TravelCardNumber { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();


            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

            localContext.Trace($"CreateValueCodeGeneric started.");

            //TRY EXECUTE
            try
            {
                var valueCodeApprovalId = ValueCodeApprovalId.Get(activityContext);
                var valueCodeId = ValueCodeId.Get(activityContext);
                var travelCardNumber = TravelCardNumber.Get(activityContext);
                var email = Email.Get(activityContext);
                var mobile = Mobile.Get(activityContext);

                if (valueCodeApprovalId == null)
                    throw new InvalidPluginExecutionException("ApproveValueCode. Input value code approval id is empty.");

                if (valueCodeId == null)
                    throw new InvalidPluginExecutionException("ValueCode. Input value code is empty.");

                if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(mobile))
                    throw new InvalidPluginExecutionException("Both mobile and email cannot be empty");

                ValueCodeApprovalEntity.ChangeState(localContext, valueCodeApprovalId, Schema.Generated.ed_valuecodeapproval_statuscode.Approved, valueCodeId);

                var deliveryMethod = string.IsNullOrWhiteSpace(mobile) ? 1 : 2; //If mobile is empty then return 1 because 1 represents email delivery method (see ValueCodeCreation model)

                //Fetch Contact and Amount from Value Code for Incident and Refund.

                //var valueCodeQuery = new QueryExpression()
                //{
                //    EntityName = ValueCodeEntity.EntityLogicalName,
                //    ColumnSet = new ColumnSet(ValueCodeEntity.Fields.ed_Amount, ValueCodeEntity.Fields.ed_Contact),
                //    Criteria =
                //    {
                //        Conditions =
                //        {
                //            new ConditionExpression(ValueCodeEntity.Fields.Id, ConditionOperator.Equal, Guid.Parse(valueCodeId))
                //        }
                //    }
                //};
                //var valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, valueCodeQuery);

                var valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, Guid.Parse(valueCodeId), new ColumnSet(ValueCodeEntity.Fields.ed_Amount, ValueCodeEntity.Fields.ed_Contact));

                if (valueCode == null)
                    throw new InvalidPluginExecutionException($"Could not find valuecode with GUID: {valueCodeId}");

                localContext.TracingService.Trace($"Creating incident case.");
                var incidentCase = IncidentEntity.CreateCaseAfterOnlineRefund(localContext, travelCardNumber, mobile, email, valueCode.ed_Contact);

                localContext.TracingService.Trace($"Creating refund.");
                var refund = RefundEntity.CreateRefundAfterOnlineRefund(localContext, incidentCase, deliveryMethod, valueCode.ed_Amount);

                //Update value code
                var valueCodeWithCaseAndRefund = new ValueCodeEntity { Id = Guid.Parse(valueCodeId), ed_CaseNumber = incidentCase.TicketNumber, ed_Refund = refund.ToEntityReference()};
                XrmHelper.Update(localContext, valueCodeWithCaseAndRefund);

                var incidentCaseClose = new IncidentEntity { Id = incidentCase.Id, IncidentStageCode = Schema.Generated.incident_incidentstagecode.ResolvedApproved };
                XrmHelper.Update(localContext, incidentCaseClose);

                localContext.TracingService.Trace($"incidentcase id: {incidentCase.Id} \nrefund id: {refund.Id}");

                localContext.TracingService.Trace($"Exiting ApproveValueCode");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"CreateValueCodeGeneric failed. Exception: {ex.Message}");

            }
        }
    }
}
