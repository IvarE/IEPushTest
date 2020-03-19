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
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk.Messages;

namespace Skanetrafiken.Crm.Entities
{
    public class GenerateCampaignCodesRecursive : CodeActivity
    {
        private static Guid GenerateCampaignCodesRecursiveGuid = new Guid("017B49A9-9AB4-45BF-951F-4BE6A0A551FA");

        [Input("LeadIdsStr")]
        public InArgument<string> LeadIdsStr { get; set; }

        [Input("UniqueCampaignCodesStr")]
        public InArgument<string> UniqueCampaignCodesStr { get; set; }

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
                localContext.Trace($"GenerateCampaignCodesRecursive started.");

                if(LeadIdsStr.Get(activityContext) == null || LeadIdsStr.Get(activityContext) == "")
                    localContext.Trace($"LeadIdsStr is null or empty.");

                if (UniqueCampaignCodesStr.Get(activityContext) == null || UniqueCampaignCodesStr.Get(activityContext) == "")
                    localContext.Trace($"UniqueCampaignCodesStr is null or empty.");

                ExecuteCodeActivity(localContext, LeadIdsStr.Get(activityContext).Split(';'), UniqueCampaignCodesStr.Get(activityContext).Split(';'));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error caught in GenerateCampaignCodesRecursive:\n{0}", e.ToString()));
            }
        }

        private static List<LeadEntity> GetLeads(Plugin.LocalPluginContext localContext, string leadIdsString)
        {
            List<LeadEntity> leads = new List<LeadEntity>();
            List<string> leadsIds = new List<string>(leadIdsString.Split(';'));
            foreach(String idStr in leadsIds)
            {
                Guid id = new Guid(idStr);
                LeadEntity lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, id, new ColumnSet(false));
                leads.Add(lead);
            }

            return leads;
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

        private static void CallWorkflowGenerateCampaignCodesRecursive(Plugin.LocalPluginContext localContext, string[] leadIds, string[] UniqueCampaignCodes)
        {
            string leadsStr = ConcatListToString(leadIds);
            string UniqueCampaignCodesStr = ConcatListToString(UniqueCampaignCodes);

            var generateCampaignCodesReq = new OrganizationRequest("ed_GenerateCampaignCodesAction")
            {
                ["LeadIdsStr"] = leadsStr,
                ["UniqueCampaignCodesStr"] = UniqueCampaignCodesStr
            };

            localContext.OrganizationService.Execute(generateCampaignCodesReq);
        }

        private static string[] SubArray(string[] data, int index, int length)
        {
            string[] result = new string[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }


        public static void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string[] leadIds, string[] UniqueCampaignCodes)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int portionSize = 1000;
            int numberOfPortions = leadIds.Count() / portionSize;

            for (int i = 0; i < numberOfPortions; i++)
            {
                localContext.Trace($"Before UpdateLeadsBulk (first).");
                UpdateLeadsBulk(localContext, stopwatch, leadIds, UniqueCampaignCodes, i, 1000, 1000);
                localContext.Trace($"After UpdateLeadsBulk (first).");
            }

            int numberOfLeadsLeft = leadIds.Count() - numberOfPortions * portionSize;

            if (numberOfLeadsLeft > 0)
            {
                localContext.Trace($"Before UpdateLeadsBulk (second).");
                UpdateLeadsBulk(localContext, stopwatch, leadIds, UniqueCampaignCodes, numberOfPortions * portionSize, numberOfLeadsLeft, 1000);
                localContext.Trace($"After UpdateLeadsBulk (second).");
            }
            
        }


        private static void UpdateLeadsBulk(Plugin.LocalPluginContext localContext, Stopwatch stopwatch, string[] leadIds, string[] UniqueCampaignCodes, int portionIndex, int currentPortionSize, int standardPortionSize)
        {
            localContext.Trace($"Inside/start of UpdateLeadsBulk.");

            OrganizationRequestCollection requests = new OrganizationRequestCollection();

            bool timeLimitReached = false;
            int j;

            for (j = 0; j < currentPortionSize; j++)
            {               
                LeadEntity updateLead = new LeadEntity
                {
                    Id = new Guid(leadIds[portionIndex * standardPortionSize + j]),
                    ed_CampaignCode = UniqueCampaignCodes[portionIndex * standardPortionSize + j]
                };

                requests.Add(
                    new UpdateRequest()
                    {
                        Target = updateLead
                    }
                );

                timeLimitReached = stopwatch.ElapsedMilliseconds > 100000;

                if (timeLimitReached)
                    break;
            }

            PerformMultipleRequest(localContext, requests);

            if (timeLimitReached)
            {
                localContext.Trace($"Time limit reached. Start again.");

                int nextStartIndex = portionIndex * standardPortionSize + j;
                int nextLeadsLength = leadIds.Length - portionIndex * standardPortionSize - j;
                int nextCodesLength = UniqueCampaignCodes.Length - portionIndex * standardPortionSize - j;

                string[] leadsPortion = SubArray(leadIds, nextStartIndex, nextLeadsLength);
                string[] UniqueCampaignCodesPortion = SubArray(UniqueCampaignCodes, nextStartIndex, nextCodesLength);

                CallWorkflowGenerateCampaignCodesRecursive(localContext, leadsPortion, UniqueCampaignCodesPortion);
            }
            
        }

        private static void PerformMultipleRequest(Plugin.LocalPluginContext localContext, OrganizationRequestCollection requests)
        {
            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = false
                },
                Requests = requests
            };

            localContext.Trace($"Start execute multiple request.");

            localContext.OrganizationService.Execute(multipleRequest);

            localContext.Trace($"Execute multiple request finished.");
        }


    }
}