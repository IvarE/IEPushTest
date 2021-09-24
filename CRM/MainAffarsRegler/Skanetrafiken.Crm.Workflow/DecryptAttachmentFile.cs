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
    public class DecryptAttachmentFile : CodeActivity
    {
        [Input("EncryptedString")]
        [RequiredArgument()]
        public InArgument<string> EncryptedString { get; set; }

        [Output("DecryptedResponse")]
        public OutArgument<string> DecryptedResponse { get; set; }

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

                localContext.Trace($"DecryptAttachmentFile started.");
                IWorkflowContext wfContext = activityContext.GetExtension<IWorkflowContext>();
                var executingUser = (Guid?)wfContext.UserId;

                //GET VALUE(S)
                localContext.Trace($"DecryptAttachmentFile. Get ValueCode ID");
                string encryptedString = EncryptedString.Get(activityContext);

                string cancelValueCodeResponse = ExecuteCodeActivity(localContext, encryptedString, executingUser);

                DecryptedResponse.Set(activityContext, cancelValueCodeResponse);
            }
            catch (Exception ex)
            {
                DecryptedResponse.Set(activityContext, $"Kunde inte Hämta filen. Vänligen försök igen eller kontakta kundtjänst. (DecryptAttachmentFile) Error : { ex.Message }");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string encryptString, Guid? userGuid)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            return IncidentEntity.HandleDecryptAttachment(localContext, encryptString, userGuid);
        }
    }
}
