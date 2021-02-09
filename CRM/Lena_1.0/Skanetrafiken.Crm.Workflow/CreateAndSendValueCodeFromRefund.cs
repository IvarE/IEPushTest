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
using Skanetrafiken.Crm.ValueCodes;

namespace Skanetrafiken.Crm.Entities
{
    public class CreateAndSendValueCodeFromRefund : CodeActivity
    {
        [Input("RefundId")]
        [RequiredArgument()]
        [ReferenceTarget("cgi_refund")]
        public InArgument<EntityReference> RefundId { get; set; }

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

            localContext.Trace($"CreateAndSendValueCodeFromRefund started.");

            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                EntityReference refund = RefundId.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, refund);
                localContext.Trace($"CreateAndSendValueCodeFromRefund finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                //localContext.Trace($"BlockAccountPortal finished with exception: {ex.Message}");

                throw new InvalidPluginExecutionException($"CreateAndSendValueCodeFromRefund failed. Exception: {ex.Message}");

            }
        }
        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference refundRef)
        {
            try
            {
                RefundEntity refund = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refundRef.Id, new ColumnSet(true));

                if (refund == null)
                    throw new InvalidPluginExecutionException("Could not get Refund");

                //refund.CreateAndSendValueCodeFromRefund(localContext);

                //Guid valueCodeId = refund.CreateValueCode(localContext, refund.cgi_Caseid.Id);
                //refund.SendValueCode(localContext, valueCodeId);
                
                return "ValueCode Sent";
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("(CreateAndSendValueCodeFromRefund) Error: " + ex);
            }
        }
    }
}
