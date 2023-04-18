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

namespace Skanetrafiken.Crm.Entities
{
    public class SendEmailFromRibbon : CodeActivity
    {
        [Input("EmailId")]
        [RequiredArgument()]
        [ReferenceTarget("email")]
        public InArgument<EntityReference> EmailId { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {//GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"SendEmail started.");
            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                EntityReference email = EmailId.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, email);
                //localContext.Trace($"BlockAccountPortal finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                //localContext.Trace($"BlockAccountPortal finished with exception: {ex.Message}");

                throw new InvalidPluginExecutionException($"SendEmail failed. Exception: {ex.Message}");

            }
        }
        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference email)
        {
            try
            {
                //TEST 22-02-23 CK ------

                EmailEntity tstEmail = XrmRetrieveHelper.Retrieve<EmailEntity>(localContext, email.Id, new ColumnSet(true));

                //TEST 22-02-23 CK ------

                SendEmailRequest emailRequest = new SendEmailRequest();
                emailRequest.EmailId = email.Id;
                emailRequest.IssueSend = true;
                //emailRequest.TrackingToken = "";
                emailRequest.TrackingToken = !string.IsNullOrWhiteSpace(tstEmail.TrackingToken) ? tstEmail.TrackingToken : "";

                SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(emailRequest);

                return sendEmailresp.ResponseName.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
