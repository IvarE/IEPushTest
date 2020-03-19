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

namespace Skanetrafiken.Crm
{
    public class RecalculateStatisticsOfCampaign : CodeActivity
    {
        [Input("CampaignId")]
        [RequiredArgument()]
        [ReferenceTarget("campaign")]
        public InArgument<EntityReference> CampaignId { get; set; }

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

            localContext.Trace($"SendValueCode started.");

            //TRY EXECUTE
            try
            {
                if (CampaignId == null)
                {
                    throw new InvalidPluginExecutionException("Input value code reference is null");
                }

                //GET VALUE(S)
                EntityReference campaign = CampaignId.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, campaign);
                localContext.Trace($"RecalculateStatisticsOfCampaign finished. Responsemessage: {response}");

            }
            catch(Exception ex)
            {
                throw new InvalidPluginExecutionException($"RecalculateStatisticsOfCampaign failed. Exception: {ex.Message}");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignRef)
        {
            CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaignRef.Id, new ColumnSet(true));

            if (campaign == null)
                throw new InvalidPluginExecutionException("Could not get Campaign");

            campaign.RecalculateStatisticsOnCampaign(localContext);


            return "Created Successfully";
        }
    }
}
