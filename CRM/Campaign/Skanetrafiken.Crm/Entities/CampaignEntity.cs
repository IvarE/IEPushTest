using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Endeavor.Crm;
using System.Diagnostics;

namespace Skanetrafiken.Crm.Entities
{
    public class CampaignEntity : Generated.Campaign
    {
        private static readonly int campaignCodeLength = 6;
        private static readonly char[] campaignCodeChars = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
        private static Guid UpdateLeadSourceOnUnderlyingLeadsGuid = new Guid("42CB6253-FC69-407C-95AB-5B31AE20E940");
        private static Guid AnonymiseUnderlyingLeadsGuid = new Guid("66C2FE2B-A0D0-464A-AADA-AD4EBBFF1EE5");

        private readonly string processId = "2D95BBF8-E9FF-4C4B-A988-53E4CC1225A5";
        private readonly string stage1Create = "ceeb25ac-c704-4612-a296-61cef8986c70";
        private readonly string stage2DR1 = "189dc031-75b4-4464-b8a5-1d1e181945d8";
        private readonly string stage3DR2 = "74f9a153-4824-47d9-bcdd-9b42b2fe2556";
        private readonly string stage4End = "20bbf364-e634-406b-b322-5b922fecaead";

        public static List<string> generateUniqueCampaignCodes(List<string> listOfExistingCodes, int numberOfCodesToGenerate)
        {
            List<string> listOfNewGeneratedCodes = generateRandomCampaignCodes(numberOfCodesToGenerate);
            return makeCampaignCodesUnique(listOfExistingCodes, listOfNewGeneratedCodes);
        }

        public static List<string> makeCampaignCodesUnique(List<string> listOfExistingCodes, List<string> listOfNewGeneratedCodes)
        {
            List<string> duplicatesBetweenTwoLists = listOfNewGeneratedCodes.Intersect(listOfExistingCodes).ToList();
            List<string> duplicatesWithinList = listOfNewGeneratedCodes.GroupBy(x => x)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key).ToList();

            if (duplicatesBetweenTwoLists.Count == 0 && duplicatesWithinList.Count == 0)
            {
                return listOfNewGeneratedCodes;
            }
            else
            {
                if (duplicatesBetweenTwoLists.Count != 0)
                {
                    foreach (string duplicateCode in duplicatesBetweenTwoLists)
                    {
                        listOfNewGeneratedCodes.Remove(duplicateCode);
                    }
                    List<string> newCodes = generateRandomCampaignCodes(duplicatesBetweenTwoLists.Count);
                    listOfNewGeneratedCodes.AddRange(newCodes);
                    makeCampaignCodesUnique(listOfExistingCodes, listOfNewGeneratedCodes);
                }
                else if (duplicatesWithinList.Count != 0)
                {
                    foreach (string duplicateCode in duplicatesWithinList)
                    {
                        listOfNewGeneratedCodes.Remove(duplicateCode);
                    }
                    List<string> newCodes = generateRandomCampaignCodes(duplicatesWithinList.Count);
                    listOfNewGeneratedCodes.AddRange(newCodes);
                    makeCampaignCodesUnique(listOfExistingCodes, listOfNewGeneratedCodes);
                }
            }
            return listOfNewGeneratedCodes;
        }

        public static void HandlePreAssociateWithProduct(Plugin.LocalPluginContext localContext, EntityReference targetId, EntityReferenceCollection relatedEntityIds)
        {
            if (relatedEntityIds != null && relatedEntityIds.Count > 0)
            {
                QueryExpression query = new QueryExpression()
                {
                    EntityName = ProcessStageEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(
                        ProcessStageEntity.Fields.StageCategory
                        ),
                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = ProcessStageEntity.EntityLogicalName,
                            LinkToEntityName = CampaignEntity.EntityLogicalName,
                            LinkFromAttributeName = ProcessStageEntity.Fields.ProcessStageId,
                            LinkToAttributeName = CampaignEntity.Fields.StageId,
                            EntityAlias = CampaignEntity.EntityLogicalName,
                            JoinOperator = JoinOperator.Inner,
                            LinkCriteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(CampaignEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.CampaignState.Active),
                                    new ConditionExpression(CampaignEntity.Fields.CampaignId, ConditionOperator.Equal, targetId.Id)
                                }
                            }
                        }
                    }
                };

                ProcessStageEntity processStage = XrmRetrieveHelper.RetrieveFirst<ProcessStageEntity>(localContext, query);

                if (processStage != null && processStage.StageCategory != Generated.processstage_category.Propose)
                {
                    throw new Exception("Not allowed to add Products to a Campaign that is already Launched");
                }
            }
        }

        public void HandlePreCampaignUpdate(Plugin.LocalPluginContext localContext, CampaignEntity preImage)
        {
            CampaignEntity combined = new CampaignEntity
            {
                Id = Id,
                CampaignId = CampaignId
            };
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

            // ActualCampaignEnd should only be allowed to be set after The Campaign is over
            if (Contains(CampaignEntity.Fields.ed_ActualCampaignEnd) && ed_ActualCampaignEnd.HasValue && ed_ActualCampaignEnd != preImage.ed_ActualCampaignEnd &&
                combined.StageId != null && combined.ProcessId != null)
            {
                ProcessStageEntity stage = XrmRetrieveHelper.RetrieveFirst<ProcessStageEntity>(localContext, new ColumnSet(ProcessStageEntity.Fields.StageCategory),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ProcessStageEntity.Fields.ProcessStageId, ConditionOperator.Equal, combined.StageId)
                        }
                    });

                if (stage != null && stage.StageCategory != Generated.processstage_category.Close)
                {
                    throw new Exception("It is not allowed to set ActualCampaignEnd until the Campaign is in a Closed process Stage");
                }

                //TODO How to know which stage was earlier?
                if (combined.ProcessId == new Guid(processId))
                {
                    if (combined.ed_ValidToPhase2 == null || !combined.ed_ValidToPhase2.HasValue)
                    {
                        throw new Exception("It is not allowed to set ActualCampaignEnd until Valid To - Phase 2 is set.");
                    }
                    if (ed_ActualCampaignEnd.Value.CompareTo(combined.ed_ValidToPhase2) < 0)
                    {
                        throw new Exception("It is not allowed to set ActualCampaignEnd until response time is complete");
                    }
                }
            }

            // Verify that the Campaign acctually is over when trying to enter End Stage
            if (Contains(CampaignEntity.Fields.StageId) && StageId != null && StageId != preImage.StageId)
            {
                IList<ProcessStageEntity> stages = XrmRetrieveHelper.RetrieveMultiple<ProcessStageEntity>(localContext, new ColumnSet(ProcessStageEntity.Fields.StageCategory),
                    new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(ProcessStageEntity.Fields.ProcessStageId, ConditionOperator.Equal, StageId),
                            new ConditionExpression(ProcessStageEntity.Fields.ProcessStageId, ConditionOperator.Equal, preImage.StageId)
                        }
                    });
                ProcessStageEntity targetStage = null, preImageStage = null;
                foreach (ProcessStageEntity s in stages)
                {
                    if (s.Id == StageId)
                        targetStage = s;
                    if (s.Id == preImage.StageId)
                        preImageStage = s;
                }
                //throw new InvalidPluginExecutionException("targetStage == " + targetStage + ". preImageStage == " + preImageStage);
                if (targetStage != null || preImageStage != null)
                {
                    if (targetStage.StageCategory == Generated.processstage_category.Close)
                    {
                        // The first (or only) SendOut-Phase is category Qualify, the re-sendout is category Develop.
                        // DateTime.Now.CompareTo(null) == 1
                        if ((preImageStage.StageCategory == Generated.processstage_category.Qualify && DateTime.Now.CompareTo(combined.ed_ValidToPhase1) < 0) ||
                            (preImageStage.StageCategory == Generated.processstage_category.Develop && DateTime.Now.CompareTo(combined.ed_ValidToPhase2) < 0))
                        {
                            throw new Exception("Campaign is not allowed to end before response time is complete");
                        }
                    }
                }
            }
        }


        public static List<string> generateRandomCampaignCodes(int numberOfCodes)
        {
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            StringBuilder result = new StringBuilder(campaignCodeLength);
            crypto.GetNonZeroBytes(data);
            data = new byte[campaignCodeLength];

            for (int i = 0; i < numberOfCodes; i++)
            {
                crypto.GetNonZeroBytes(data);
                foreach (byte b in data)
                {
                    result.Append(campaignCodeChars[b % (campaignCodeChars.Length - 1)]);
                }
                if (i != numberOfCodes)
                    result.Append(",");
            }

            string[] stringArray = result.ToString().Split(',');
            List<String> returnList = new List<string>(stringArray);
            returnList.Remove(""); //Last element always empty

            return returnList;
        }

        public void HandlePostCampaignUpdateAsync(Plugin.LocalPluginContext localContext, CampaignEntity preImage)
        {
            // If change was applied to ed_ActualCampaignEnd we start the recursive process of anonymizing Leads or ed_IsLeadToAnonymize
            if (this.ed_ActualCampaignEnd != null)
            {
                ExecuteWorkflowRequest req = new ExecuteWorkflowRequest
                {
                    EntityId = this.Id,
                    WorkflowId = AnonymiseUnderlyingLeadsGuid
                };
                localContext.OrganizationService.Execute(req);
            }

            // If change was applied to ed_LeadSource we start the recursive process of updating the Leadsource for all underlying Leads
            if (this.Contains(CampaignEntity.Fields.ed_LeadSource))
            {
                ExecuteWorkflowRequest req = new ExecuteWorkflowRequest
                {
                    EntityId = this.Id,
                    WorkflowId = UpdateLeadSourceOnUnderlyingLeadsGuid
                };
                localContext.OrganizationService.Execute(req);
            }
        }

        public bool IsAcceptingResponse(Plugin.LocalPluginContext localContext)
        {
            if (!(Contains(CampaignEntity.Fields.StateCode) && ed_ValidFromPhase1 != null && ed_ValidToPhase1 != null))
                return false;

            if (!Generated.CampaignState.Active.Equals(StateCode))
                return false;

            if (ed_TryOutFromPhase2 != null && ed_TryOutToPhase2 != null)
            {
                if ((DateTime.Now.CompareTo(ed_ValidFromPhase1) < 0 || DateTime.Now.CompareTo(ed_ValidToPhase1) > 0) && (DateTime.Now.CompareTo(ed_ValidFromPhase2) < 0 || DateTime.Now.CompareTo(ed_ValidToPhase2) > 0))
                    return false;
            }
            else
            {
                if (DateTime.Now.CompareTo(ed_ValidFromPhase1) < 0 || DateTime.Now.CompareTo(ed_ValidToPhase1) > 0)
                    return false;
            }

            return true;
        }
    }

    
}