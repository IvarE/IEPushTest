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
    public class CampaignResponseEntity : Generated.CampaignResponse
    {
        public void HandlePreCampaignResponseCreate(Plugin.LocalPluginContext localContext)
        {
            if (this.Customer != null)
            {
                this.CopyCustomerToEdCustomer(localContext);
            }
        }

        public void HandlePreCampaignResponseUpdate(Plugin.LocalPluginContext localContext)
        {
            if (this.Customer != null)
            {
                this.CopyCustomerToEdCustomer(localContext);
            }
        }

        internal void CopyCustomerToEdCustomer(Plugin.LocalPluginContext localContext)
        {
            EntityCollection apeCollection = this.GetAttributeValue<EntityCollection>(CampaignResponseEntity.Fields.Customer);

            if (apeCollection != null)
            {
                for (int i = 0; i < apeCollection.Entities.Count; i++)
                {
                    Generated.ActivityParty ap = apeCollection[i].ToEntity<Generated.ActivityParty>();

                    if (ap != null && ap.PartyId != null)
                    {
                        if (ap.PartyId.LogicalName == AccountEntity.EntityLogicalName)
                        {
                            this.ed_Customer = ap.PartyId;
                            

                        }
                        else if (ap.PartyId.LogicalName == ContactEntity.EntityLogicalName)
                        {
                            this.ed_Customer = ap.PartyId;
                        }
                    }
                }
            }
        }

        public void HandlePostCampaignResponseCreateAsync(Plugin.LocalPluginContext localContext)
        {
            // Calculate total number of campaign responses on campaign
            this.ReCalculateCampaignResponsesOnCampaign(localContext);
        }

        internal void ReCalculateCampaignResponsesOnCampaign(Plugin.LocalPluginContext localContext)
        {
            if (this.RegardingObjectId != null && this.RegardingObjectId.Id != null)
            {
                CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, this.RegardingObjectId.Id, new ColumnSet(
                    CampaignEntity.Fields.ed_TotalLeads,
                    CampaignEntity.Fields.ed_QualifiedLeads,
                    CampaignEntity.Fields.ed_QualifiedLeadsDR1,
                    CampaignEntity.Fields.ed_QualifiedLeadsDR2,
                    CampaignEntity.Fields.ed_ValidFromPhase1,
                    CampaignEntity.Fields.ed_ValidToPhase1,
                    CampaignEntity.Fields.ed_ValidFromPhase2,
                    CampaignEntity.Fields.ed_ValidToPhase2));
                
                // If Phase 1
                if (IsValidPhaseOne(campaign, DateTime.Now) == true)
                {
                    campaign.ed_QualifiedLeadsDR1 += 1;
                    campaign.ed_QualifiedLeads += 1;
                    XrmHelper.Update(localContext, campaign);
                }

                // If Phase 2
                if (IsValidPhaseTwo(campaign, DateTime.Now) == true)
                {
                    campaign.ed_QualifiedLeadsDR2 += 1;
                    campaign.ed_QualifiedLeads += 1;
                    XrmHelper.Update(localContext, campaign);
                }

            }
        }

        internal bool IsValidPhaseOne(CampaignEntity campaign, DateTime receivedOn)
        {
            if (receivedOn >= campaign.ed_ValidFromPhase1.Value.ToLocalTime() && receivedOn <= campaign.ed_ValidToPhase1.Value.ToLocalTime().AddDays(1))
                return true;
            else
                return false;
        }

        internal bool IsValidPhaseTwo(CampaignEntity campaign, DateTime receivedOn)
        {
            if (receivedOn >= campaign.ed_ValidFromPhase2.Value.ToLocalTime() && receivedOn <= campaign.ed_ValidToPhase2.Value.ToLocalTime().AddDays(1))
                return true;
            else
                return false;
        }
    }
}