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
    public class UpdateLeadSourceOnUnderlyingLeadsRecursive : CodeActivity
    {
        private static Guid UpdateLeadSourceOnUnderlyingLeadsGuid = new Guid("42CB6253-FC69-407C-95AB-5B31AE20E940");

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
                throw new Exception(string.Format("Error caught in UpdateLeadSourceOnUnderlyingLeadsRecursive:\n{0}", e.ToString()));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="campaignId"></param>
        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaignId, new ColumnSet(CampaignEntity.Fields.ed_LeadSource));
            FilterExpression leadFilter = new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal,campaignId.Id),
                        new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                }
            };
            if (campaign.ed_LeadSource != null)
                leadFilter.AddCondition(new ConditionExpression(LeadEntity.Fields.LeadSourceCode, ConditionOperator.NotEqual, campaign.ed_LeadSource.Value));
            else
                leadFilter.AddCondition(new ConditionExpression(LeadEntity.Fields.LeadSourceCode, ConditionOperator.NotNull));

            IList<LeadEntity> campaignConnectedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false), leadFilter);
            
            foreach (LeadEntity lead in campaignConnectedLeads)
            {
                // recursive call when executionTime exceeds 100 seconds (timeout is 120)
                if (stopwatch.ElapsedMilliseconds < 100000)
                {
                    LeadEntity updateLead = new LeadEntity
                    {
                        Id = lead.Id,
                        LeadSourceCode = campaign.ed_LeadSource != null ? (Generated.lead_leadsourcecode?)campaign.ed_LeadSource.Value : null
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateLead);
                }
                else
                {
                    ExecuteWorkflowRequest req = new ExecuteWorkflowRequest
                    {
                        EntityId = campaignId.Id,
                        WorkflowId = UpdateLeadSourceOnUnderlyingLeadsGuid
                    };

                    localContext.OrganizationService.Execute(req);
                    break;
                }
            }
        }
    }
}
