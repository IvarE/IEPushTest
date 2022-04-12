using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Linq;
using Skanetrafiken.Crm.Schema.Generated;
using System.Collections.Generic;
using Skanetrafiken.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class SingaporeTicketEntity : st_singaporeticket
    {
        public static List<Contact> GetActiveContactQuery(Plugin.LocalPluginContext localContext, string mklId)
        {
            QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
            queryContacts.NoLock = true;
            queryContacts.ColumnSet = new ColumnSet(Contact.Fields.ContactId);
            queryContacts.Criteria.AddCondition(Contact.Fields.ed_MklId, ConditionOperator.Equal, mklId);
            queryContacts.Criteria.AddCondition(Contact.Fields.StateCode, ConditionOperator.Equal, (int)ContactState.Active);
            return XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);
        }

        internal void HandlePostSingaporeTicketCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                localContext.Trace($"Start Trace");
                bool needsUpdate = false;
                st_singaporeticket uSingaporeTicket = new st_singaporeticket();

                string mklId = ed_MklId;
                localContext.Trace($"MKL id: {ed_MklId}");
                string campaignIdValue = ed_Source_Campaign;
                localContext.Trace($"Source Campaign: {ed_Source_Campaign}");

                string offerName = string.Empty;
                if (this.st_SingTicketType != null)
                {
                    offerName = $"{this.st_SingTicketType}";
                    localContext.Trace($"OfferName (this):" + offerName);
                }

                string nName = ed_CRMNummer + "_" + FormattedValues["st_singtickettype"];

                localContext.Trace($"CRM Nummer: {ed_CRMNummer}");
                if (st_name != nName) {

                    localContext.Trace($"st_name: {nName}");

                    uSingaporeTicket.st_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);
                        uSingaporeTicket.st_ContactID = erContact;
                        needsUpdate = true;
                    }
                }
                localContext.Trace($"CampaignIdValue with Id {campaignIdValue}");
                
                //Get SourceCampaign
                if (!string.IsNullOrEmpty(campaignIdValue) && Guid.TryParse(campaignIdValue, out var campaignGuid))
                {
                    localContext.Trace($"CampaignIdValue converted to Guid {campaignGuid}.");

                    Campaign campaign = XrmRetrieveHelper.Retrieve<Campaign>(localContext, campaignGuid, new ColumnSet(false));

                    localContext.Trace($"Campain id is {campaign?.Id}.");

                    if (campaign != null)
                    {
                        localContext.Trace($"Found Campaign with Id {campaignGuid}");
                        uSingaporeTicket.ed_SourceCampaignId = campaign.ToEntityReference();
                        needsUpdate = true;
                    }
                }

                if (needsUpdate)
                {
                    uSingaporeTicket.Id = Id;
                    XrmHelper.Update(localContext, uSingaporeTicket);
                }

            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostSingaporeTicketCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostSingaporeTicketUpdateAsync(Plugin.LocalPluginContext localContext, st_singaporeticket preImage)
        {
            try
            {
                localContext.Trace($"Start Trace");
                bool needsUpdate = false;
                st_singaporeticket uSingaporeTicket = new st_singaporeticket();

                string mklId = string.IsNullOrEmpty(ed_MklId) ? preImage.ed_MklId : ed_MklId;
                localContext.Trace($"MKL id: {ed_MklId}");
                string campaignIdValue = string.IsNullOrEmpty(ed_Source_Campaign) ? preImage.ed_Source_Campaign : ed_Source_Campaign;
                localContext.Trace($"Source Campaign: {ed_Source_Campaign}");
                string contactNumber = string.IsNullOrEmpty(ed_CRMNummer) ? preImage.ed_CRMNummer : ed_CRMNummer;
                
                //string offerName = string.IsNullOrEmpty(FormattedValues["st_singtickettype"]) ? preImage.FormattedValues["st_singtickettype"] : FormattedValues["st_singtickettype"];

                string offerName = string.Empty;
                if (this.st_SingTicketType != null)
                {
                    offerName = $"{this.st_SingTicketType}";
                    localContext.Trace($"OfferName (this):" + offerName);
                }
                else if (preImage.st_SingTicketType != null)
                {
                    offerName = $"{preImage.st_SingTicketType}";
                    localContext.Trace($"OfferName (preImage):" + offerName);
                }

                string nName = contactNumber + "_" + offerName;
                if (preImage.st_name != nName)
                {
                    uSingaporeTicket.st_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        if (eContact.Id != preImage.st_ContactID?.Id)
                        {
                            uSingaporeTicket.st_ContactID = eContact.ToEntityReference();
                            needsUpdate = true;
                        }
                    }
                }

                localContext.Trace($"CampaignIdValue with Id {campaignIdValue}");

                //Get SourceCampaign
                if (!string.IsNullOrEmpty(campaignIdValue) && Guid.TryParse(campaignIdValue, out var campaignGuid))
                {
                    localContext.Trace($"CampaignIdValue converted to Guid {campaignGuid}.");

                    Campaign campaign = XrmRetrieveHelper.Retrieve<Campaign>(localContext, campaignGuid, new ColumnSet(false));

                    localContext.Trace($"Campain id is {campaign?.Id}.");

                    if (campaign != null)
                    {
                        localContext.Trace($"Found Campaign with Id {campaignGuid}");
                        uSingaporeTicket.ed_SourceCampaignId = campaign.ToEntityReference();
                        needsUpdate = true;
                    }
                }

                if (needsUpdate)
                {
                    uSingaporeTicket.Id = Id;
                    XrmHelper.Update(localContext, uSingaporeTicket);
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostSingaporeTicketUpdateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
