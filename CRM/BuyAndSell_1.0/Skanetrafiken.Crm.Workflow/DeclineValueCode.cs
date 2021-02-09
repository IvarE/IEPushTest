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
    public class DeclineValueCode : CodeActivity
    {
        [Input("ValueCodeApprovalId")]
        [RequiredArgument()]
        [ReferenceTarget("ed_valuecodeapproval")]
        public InArgument<EntityReference> ValueCodeApprovalId { get; set; }

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

                if (valueCodeApprovalId == null)
                    throw new ArgumentNullException("Input value code approval id is null.");

                ValueCodeApprovalEntity.ChangeState(localContext, valueCodeApprovalId, Schema.Generated.ed_valuecodeapproval_statuscode.Declined);
                localContext.TracingService.Trace($"Exiting ApproveValueCode");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"CreateValueCodeGeneric failed. Exception: {ex.Message}");

            }
        }
    }
}
