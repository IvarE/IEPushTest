using System;
using System.Activities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using System.Threading;

namespace Skanetrafiken.Crm.Entities
{
    public class SynchroniseLeads : CodeActivity
    {
        private static Guid UpdateLeadSourceOnUnderlyingLeadsGuid = new Guid("42CB6253-FC69-407C-95AB-5B31AE20E940");
        private static Guid UpdateTopicOnUnderlyingLeadsGuid = new Guid("3923AB49-FE17-4726-BA8B-18B33DBFBCEB");
        private static Guid GenerateCampaignCodesGuid = new Guid("8C76A4D9-680F-4CE8-B3B2-7F096887F32B");

        [Input("CampaignId")]
        [RequiredArgument()]
        [ReferenceTarget(CampaignEntity.EntityLogicalName)]
        public InArgument<EntityReference> campaingReference { get; set; }

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
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            //localContext.TracingService.Trace("Plugin för att kolla vilka som ska skickas till startat.");
            try
            {
                EntityReference campaignId = campaingReference.Get(activityContext);
                ExecuteCodeActivity(localContext, campaignId);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Fel i AssignCampaignCodes:\n{0}", e.ToString()));
            }
        }


        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Start recursive job to generate CampaignCodes
            ExecuteWorkflowRequest generateCampaignCodesReq = new ExecuteWorkflowRequest
            {
                EntityId = campaignId.Id,
                WorkflowId = GenerateCampaignCodesGuid
            };
            ExecuteWorkflowResponse resp1 = (ExecuteWorkflowResponse)localContext.OrganizationService.Execute(generateCampaignCodesReq);

            // Start recursive job to synchronise LeadTopic with Campaign
            ExecuteWorkflowRequest updateLeadSourceReq = new ExecuteWorkflowRequest
            {
                EntityId = campaignId.Id,
                WorkflowId = UpdateLeadSourceOnUnderlyingLeadsGuid
            };
            ExecuteWorkflowResponse resp2 = (ExecuteWorkflowResponse)localContext.OrganizationService.Execute(updateLeadSourceReq);

            // Start recursive job to synchronise LeadSource with Campaign
            ExecuteWorkflowRequest updateTopicReq = new ExecuteWorkflowRequest
            {
                EntityId = campaignId.Id,
                WorkflowId = UpdateTopicOnUnderlyingLeadsGuid
            };
            ExecuteWorkflowResponse resp3 = (ExecuteWorkflowResponse)localContext.OrganizationService.Execute(updateTopicReq);
            

            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaignId.Id);

            int totalLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false), filter).Count;
            CampaignEntity updateEntity = new CampaignEntity
            {
                Id = campaignId.Id,
                ed_TotalLeads = totalLeads
            };
            XrmHelper.Update(localContext.OrganizationService, updateEntity);
        }

    }
}
