using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;

namespace Skanetrafiken.Crm
{
    public class ApproveValueCodeApproval : CodeActivity
    {
        [Input("ValueCodeApprovalId")]
        [ReferenceTarget("ed_valuecodeapproval")]
        public InArgument<EntityReference> ValueCodeApprovalId { get; set; }

        [Output("Amount")]
        public OutArgument<decimal> Amount { get; set; }

        [Output("Mobile")]
        public OutArgument<string> Mobile { get; set; }

        [Output("Email")]
        public OutArgument<string> Email { get; set; }

        [Output("VoucherType")]
        public OutArgument<int> VoucherType { get; set; }

        [Output("VoucherDeliveryType")]
        public OutArgument<int> VoucherDeliveryType { get; set; }

        [Output("ContactId")]
        [ReferenceTarget("contact")]
        public OutArgument<EntityReference> ContactId { get; set; }

        [Output("ResultApproveValueCodeApproval")]
        public OutArgument<string> ResultApproveValueCodeApproval { get; set; }

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
            //TRY EXECUTE
            try
            {
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.TracingService.Trace($"SendValueCode started.");


                //GET VALUE(S)
                EntityReference approval = ValueCodeApprovalId.Get(activityContext);

                ValueCodeApprovalEntity valueCodeApproval = ExecuteCodeActivity(localContext, approval);
                localContext.Trace($"ApproveValueCodeApproval finished. ValueCodeApproval: {valueCodeApproval.Id}");

                Amount.Set(activityContext, valueCodeApproval.ed_Amount);
                Mobile.Set(activityContext, valueCodeApproval.ed_Mobile);
                Email.Set(activityContext, valueCodeApproval.ed_EmailAddress);
                ContactId.Set(activityContext, valueCodeApproval.ed_Contact);
                VoucherType.Set(activityContext, (int)valueCodeApproval.ed_ValueCodeTypeGlobal.Value);
                VoucherDeliveryType.Set(activityContext, (int)valueCodeApproval.ed_ValueCodeDeliveryTypeGlobal.Value);
            }
            catch (Exception ex)
            {
                ResultApproveValueCodeApproval.Set(activityContext, $"Kunde inte godkänna värdekod. Vänligen kontakta kundtjänst. StripError (ApproveValueCodeApproval): { ex.Message}");
            }
        }
        public static ValueCodeApprovalEntity ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference approval)
        {
            ValueCodeApprovalEntity valueCodeApproval = XrmRetrieveHelper.Retrieve<ValueCodeApprovalEntity>(localContext, approval, new ColumnSet(
                ValueCodeApprovalEntity.Fields.ed_Amount,
                ValueCodeApprovalEntity.Fields.ed_Mobile,
                ValueCodeApprovalEntity.Fields.ed_EmailAddress,
                ValueCodeApprovalEntity.Fields.ed_Contact,
                ValueCodeApprovalEntity.Fields.ed_ValueCodeTypeGlobal,
                ValueCodeApprovalEntity.Fields.ed_ValueCodeDeliveryTypeGlobal));

            valueCodeApproval.statuscode = Schema.Generated.ed_valuecodeapproval_statuscode.Approved;
            XrmHelper.Update(localContext, valueCodeApproval);

            return valueCodeApproval;

        }
    }
}
