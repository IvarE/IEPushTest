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
    public class AnonymiseUnderlyingLeadsRecursive : CodeActivity
    {
        private static Guid AnonymiseUnderlyingLeadsGuid = new Guid("66C2FE2B-A0D0-464A-AADA-AD4EBBFF1EE5");

        private static string anon = "Anonymous";
        private static string anonEmail = "anonymous@fakemail.com";

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

            IList<LeadEntity> campaignConnectedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(
                localContext,
                new QueryExpression()
                {
                    EntityName = LeadEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaignId.Id),
                                new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                            }
                    }
                });

            bool done = true;
            // If the execution takes more than 100 seconds, go recurcsive
            foreach (LeadEntity lead in campaignConnectedLeads)
            {
                if (stopwatch.ElapsedMilliseconds < 100000)
                {
                    LeadEntity anonEnt = CreateAnonymizeEntity(lead);
                    XrmHelper.Update(localContext.OrganizationService, anonEnt);

                    SetStateRequest req = new SetStateRequest()
                    {
                        EntityMoniker = anonEnt.ToEntityReference(),
                        State = new OptionSetValue((int)Generated.LeadState.Disqualified),
                        Status = new OptionSetValue((int)Generated.lead_statuscode.Canceled)
                    };
                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                }
                else
                {
                    done = false;
                    ExecuteWorkflowRequest req = new ExecuteWorkflowRequest
                    {
                        EntityId = campaignId.Id,
                        WorkflowId = AnonymiseUnderlyingLeadsGuid
                    };
                    localContext.OrganizationService.Execute(req);
                    break;
                }
            }
            if (done)
            {
                string query =
                    "<fetch returntotalrecordcount='true'>"+
                        "<entity name='lead' >"+
                            "<filter type='and' >"+
                                "<condition attribute='campaignid' operator='eq' value='" + campaignId.Id.ToString() + "' />"+
                                "<condition attribute='lastname' operator='eq' value='" + anon + "' />" +
                                "<condition attribute='address1_line1' operator='eq' value='" + anon + "' />" +
                                "<condition attribute='address1_line2' operator='eq' value='" + anon + "' />" +
                            "</filter>"+
                        "</entity>"+
                    "</fetch>";
                EntityCollection anonymisedLeads = localContext.OrganizationService.RetrieveMultiple(new FetchExpression(query));
                CampaignEntity updateEntity = new CampaignEntity
                {
                    Id = campaignId.Id,
                    ed_AnonymisedLeads = anonymisedLeads.TotalRecordCount
                };
                XrmHelper.Update(localContext.OrganizationService, updateEntity);
            }
        }

        private static LeadEntity CreateAnonymizeEntity(LeadEntity l)
        {
            return new LeadEntity
            {
                Id = l.Id,
                FirstName = null,
                LastName = anon,
                ed_Personnummer = null,
                Address1_Line1 = anon,
                Address1_Line2 = anon,
                Telephone1 = null,
                MobilePhone = null,
                EMailAddress1 = anonEmail,
                ed_CampaignCode = null
            };
        }
    }
}