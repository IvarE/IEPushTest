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

namespace Skanetrafiken.Crm
{
    public class CancelValueCode : CodeActivity
    {
        [Input("ValueCodeId")]
        [RequiredArgument()]
        public InArgument<string> ValueCodeId { get; set; }

        [Output("CancelValueCodeResponse")]
        public OutArgument<string> CancelValueCodeResponse { get; set; }

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

                localContext.Trace($"CanceleValueCode started.");


                //GET VALUE(S)
                localContext.Trace($"CanceleValueCode. Get ValueCode ID");
                string valueCodeGuid = ValueCodeId.Get(activityContext);

                string cancelValueCodeResponse = ExecuteCodeActivity(localContext, valueCodeGuid);

                CancelValueCodeResponse.Set(activityContext, cancelValueCodeResponse);
            }
            catch (Exception ex)
            {
                CancelValueCodeResponse.Set(activityContext, $"Kunde inte Makulera VärdeCod. Vänligen försök igen eller kontakta kundtjänst. (CanceleValueCode) Error : { ex.Message }");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string valueCodeGuid)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            return ValueCodeEntity.HandleCancelValueCode(localContext, valueCodeGuid);
        }
    }
}
