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
using Endeavor.Crm;

using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class ProductEntity : Generated.Product
    {
        public void HandlePreProductUpdate(Plugin.LocalPluginContext localContext, ProductEntity preImage)
        {
            ProductEntity combined = new ProductEntity
            {
                Id = this.Id
            };
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

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
                                new ConditionExpression(CampaignEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.CampaignState.Active)
                            }
                        },
                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = CampaignEntity.EntityLogicalName,
                                LinkToEntityName = CampaignItemEntity.EntityLogicalName,
                                LinkFromAttributeName = CampaignEntity.Fields.CampaignId,
                                LinkToAttributeName = CampaignItemEntity.Fields.CampaignId,
                                EntityAlias = CampaignEntity.EntityLogicalName,
                                JoinOperator = JoinOperator.Inner,
                                LinkCriteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(CampaignItemEntity.Fields.CampaignItemId, ConditionOperator.Equal, combined.Id)
                                    }
                                }
                            }
                        }
                    }
                }
            };

            ProcessStageEntity processStage = XrmRetrieveHelper.RetrieveFirst<ProcessStageEntity>(localContext, query);

            if (processStage != null && processStage.StageCategory != Generated.processstage_category.Propose)
            {
                if (Contains(ProductEntity.Fields.StandardCost) && StandardCost != preImage.StandardCost ||
                    Contains(ProductEntity.Fields.PriceLevelId) && PriceLevelId != preImage.PriceLevelId ||
                    Contains(ProductEntity.Fields.ProductTypeCode) && ProductTypeCode != preImage.ProductTypeCode)
                {
                    throw new Exception("Not allowed to alter a product that is on offer in an active Campaign");
                }
            }
        }
    }   
}