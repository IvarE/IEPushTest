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
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.Entities
{
    public class MergeOldAccountWithPortal : CodeActivity
    {
        [Input("oldAccountNumber")]
        [RequiredArgument()]
        public InArgument<string> OldAccountNumber { get; set; }

        [Input("newAccountNumber_KST")]
        [RequiredArgument()]
        public InArgument<string> NewAccountNumber_KST { get; set; }

        [Output("MergeResponse")]
        public OutArgument<string> MergeResponse { get; set; }

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

                localContext.Trace($"MergeOldAccountWithPortal started.");

                var oldAccNumber = OldAccountNumber.Get(activityContext);
                var newAccNumber_KST = NewAccountNumber_KST.Get(activityContext);

                ExecuteCodeActivity(localContext, oldAccNumber, newAccNumber_KST);
                MergeResponse.Set(activityContext, "");
            }
            catch (Exception ex)
            {
                MergeResponse.Set(activityContext, $"Kunde inte migrera konton. { ex.Message}.");
            }
        }

        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string oldAccNumber, string newAccNumber_KST)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            if (string.IsNullOrWhiteSpace(oldAccNumber) || string.IsNullOrWhiteSpace(newAccNumber_KST))
                throw new InvalidPluginExecutionException($"{nameof(oldAccNumber)} or {nameof(newAccNumber_KST)} cannot be empty.");

            //AccountEntity.HandleMergeOldAccountWithKST(localContext, oldAccNumber, newAccNumber_KST);
            
        }
    }
}
