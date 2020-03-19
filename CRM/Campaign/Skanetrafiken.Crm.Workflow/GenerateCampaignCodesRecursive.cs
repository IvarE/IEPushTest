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
    public class GenerateCampaignCodesRecursive : CodeActivity
    {
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
                throw new Exception(string.Format("Error caught in AnonymiseUnderlyingLeads:\n{0}", e.ToString()));
            }
        }

        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            
            IList<LeadEntity> leadsNoCampaignCodes = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false),
            new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaignId.Id),
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Null),
                        new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                }
            });
            
            IList<LeadEntity> allLeadsWithCampaignCodes = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode),
            new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull)
                }
            });

            List<string> listOfExistingCodes = new List<string>();
            foreach (LeadEntity campaignCode in allLeadsWithCampaignCodes)
            {
                listOfExistingCodes.Add(campaignCode.ed_CampaignCode);
            }

            List<string> uniqueCampaignCodes = CampaignEntity.generateUniqueCampaignCodes(listOfExistingCodes, leadsNoCampaignCodes.Count);

            for (int i = 0; i < leadsNoCampaignCodes.Count; i++)
            {
                if (stopwatch.ElapsedMilliseconds < 100000)
                {
                    LeadEntity updateLead = new LeadEntity
                    {
                        Id = leadsNoCampaignCodes[i].Id,
                        ed_CampaignCode = uniqueCampaignCodes[i]
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateLead);
                }
                else
                {
                    ExecuteWorkflowRequest generateCampaignCodesReq = new ExecuteWorkflowRequest
                    {
                        EntityId = campaignId.Id,
                        WorkflowId = GenerateCampaignCodesGuid
                    };
                    localContext.OrganizationService.Execute(generateCampaignCodesReq);
                }
            }
        }
    }
}