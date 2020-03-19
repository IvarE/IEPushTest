using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skanetrafiken.Crm.Entities
{
    public class GenerateCampaignCodesWrapper : CodeActivity
    {
        private static Guid GenerateCampaignCodesRecursiveGuid = new Guid("017B49A9-9AB4-45BF-951F-4BE6A0A551FA");

        [Input("CampaignId")]
        [RequiredArgument()]
        [ReferenceTarget(CampaignEntity.EntityLogicalName)]
        public InArgument<EntityReference> CampaignReference { get; set; }

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
            try
            {
                EntityReference campaignId = CampaignReference.Get(activityContext);
                ExecuteCodeActivity(localContext, campaignId);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error caught in GenerateCampaignCodesWrapper:\n{0}", e.ToString()));
            }
        }
        private static int GetPortionSize(int numberOfLeadsWithoutCode)
        {
            if(numberOfLeadsWithoutCode < 1000 * 1000)
            {
                return 1000;
            }
            else
            {
                return (int) Math.Ceiling(numberOfLeadsWithoutCode / 1000.0);
            }
        }

        private static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            
            List<LeadEntity> leadsWithoutCode = GetLeadsWithoutCode(localContext, campaignId);

            if(leadsWithoutCode.Count > 0)
            {
                int portionSize = GetPortionSize(leadsWithoutCode.Count());

                List<string> existingCodes = GetExisitingCampaignCodes(localContext, campaignId);

                List<string> UniqueCampaignCodes = CampaignEntity.generateUniqueCampaignCodes(existingCodes, leadsWithoutCode.Count);

                OrganizationRequestCollection requestCollection = new OrganizationRequestCollection();

                if (leadsWithoutCode.Count < portionSize)
                {
                    requestCollection.Add(CreateOrganizationRequest(localContext, leadsWithoutCode, UniqueCampaignCodes));
                }
                else
                {
                    int numberOfPortions = leadsWithoutCode.Count / portionSize;

                    for (int i = 0; i < numberOfPortions; i++)
                    {
                        List<LeadEntity> leadsPortion = leadsWithoutCode.GetRange(i * portionSize, portionSize);
                        List<string> UniqueCampaignCodesPortion = UniqueCampaignCodes.GetRange(i * portionSize, portionSize);
                        requestCollection.Add(CreateOrganizationRequest(localContext, leadsPortion, UniqueCampaignCodesPortion));
                    }

                    int numberOfLeadsLeft = leadsWithoutCode.Count() - numberOfPortions * portionSize;

                    if (numberOfLeadsLeft > 0)
                    {
                        List<LeadEntity> leadsPortion = leadsWithoutCode.GetRange(numberOfPortions * portionSize, leadsWithoutCode.Count - numberOfPortions * portionSize);
                        List<string> UniqueCampaignCodesPortion = UniqueCampaignCodes.GetRange(numberOfPortions * portionSize, UniqueCampaignCodes.Count - numberOfPortions * portionSize);
                        requestCollection.Add(CreateOrganizationRequest(localContext, leadsPortion, UniqueCampaignCodesPortion));
                    }
                }

                DoMultipleRequest(localContext, requestCollection);
            }           
            
        }

        private static void DoMultipleRequest(Plugin.LocalPluginContext localContext, OrganizationRequestCollection requests)
        {
            var multipleRequests = new ExecuteMultipleRequest()
            { 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = false
                },
                Requests = requests
            };

            localContext.OrganizationService.Execute(multipleRequests);
        }

        private static string ConcatListToString(IEnumerable<string> list)
        {
            if (list.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in list)
                {
                    sb.Append(s);
                    sb.Append(";");
                }

                sb.Remove(sb.Length - 1, 1);

                return sb.ToString();
            }
            else
                return null;
            
        }

        private static OrganizationRequest CreateOrganizationRequest(Plugin.LocalPluginContext localContext, List<LeadEntity> leads, List<string> UniqueCampaignCodes)
        {
            string leadsStr = ConcatListToString(leads.Select(l => l.Id.ToString()));
            string UniqueCampaignCodesStr = ConcatListToString(UniqueCampaignCodes);

            return new OrganizationRequest("ed_GenerateCampaignCodesAction")
            {
                ["LeadIdsStr"] = leadsStr,
                ["UniqueCampaignCodesStr"] = UniqueCampaignCodesStr
            };
        }

        private static List<LeadEntity> GetLeadsWithoutCode(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false),
            new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaignId.Id),
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Null),
                        new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Schema.Generated.LeadState.Open)
                }
            });

            return new List<LeadEntity>(leads);
        }

        private static List<string> GetExisitingCampaignCodes(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
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

            return listOfExistingCodes;
        }
    }
}