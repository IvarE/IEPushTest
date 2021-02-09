using System;
using System.Activities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;
using System.Threading;

using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class UpdateTopicOnUnderlyingLeadsRecursive : CodeActivity
    {
        private static Guid UpdateTopicOnUnderlyingLeadsGuid = new Guid("3923AB49-FE17-4726-BA8B-18B33DBFBCEB");

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
                throw new Exception(string.Format("Error caught in UpdateTopicOnUnderlyingLeadsRecursive:\n{0}", e.ToString()));
            }
        }
        
        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaignId, new ColumnSet(CampaignEntity.Fields.ed_LeadTopic));
            FilterExpression leadFilter = new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal,campaignId.Id),
                        new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                }
            };
            if (campaign.ed_LeadTopic != null)
                leadFilter.AddCondition(new ConditionExpression(LeadEntity.Fields.Subject, ConditionOperator.NotEqual, campaign.ed_LeadTopic.ToString()));
            else
                leadFilter.AddCondition(new ConditionExpression(LeadEntity.Fields.Subject, ConditionOperator.NotNull));

            IList<LeadEntity> campaignConnectedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false), leadFilter);
            
            foreach (LeadEntity lead in campaignConnectedLeads)
            {
                // recursive call when executionTime exceeds 100 seconds (timeout is 120)
                if (stopwatch.ElapsedMilliseconds < 100000)
                {
                    LeadEntity updateLead = new LeadEntity
                    {
                        Id = lead.Id,
                        Subject = campaign.ed_LeadTopic != null ? campaign.ed_LeadTopic.ToString() : null
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateLead);
                }
                else
                {
                    ExecuteWorkflowRequest req = new ExecuteWorkflowRequest
                    {
                        EntityId = campaignId.Id,
                        WorkflowId = UpdateTopicOnUnderlyingLeadsGuid
                    };

                    localContext.OrganizationService.Execute(req);
                    break;
                }
            }
        }
    }
}
