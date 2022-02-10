using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Linq;
using Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class TicketInfoEntity : ed_ticketinfo
    {
        internal void HandlePostTicketPurchasesCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                bool needsUpdate = false;
                ed_ticketinfo uTicketInfo = new ed_ticketinfo();

                string mklId = ed_MklId;
                string nName = ed_CRMNumber + "_" + FormattedValues["ed_offername"];
                if (ed_name != nName)
                {
                    uTicketInfo.ed_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = SingaporeTicketEntity.GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);
                        uTicketInfo.ed_Contact = erContact;
                        needsUpdate = true;
                    }
                }

                if (needsUpdate)
                {
                    uTicketInfo.Id = Id;
                    XrmHelper.Update(localContext, uTicketInfo);
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostTicketPurchasesUpdateAsync(Plugin.LocalPluginContext localContext, ed_ticketinfo preImage)
        {
            try
            {
                bool needsUpdate = false;
                ed_ticketinfo uTicketInfo = new ed_ticketinfo();

                string mklId = string.IsNullOrEmpty(ed_MklId) ? preImage.ed_MklId : ed_MklId;
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNumber) ? preImage.ed_CRMNumber : this.ed_CRMNumber;
                string offerName = string.IsNullOrEmpty(this.FormattedValues["ed_offername"]) ? preImage.FormattedValues["ed_offername"] : this.FormattedValues["ed_offername"];

                string nName = contactNumber + "_" + offerName;
                if (preImage.ed_name != nName)
                {
                    uTicketInfo.ed_name = nName;
                    needsUpdate = true;
                }

                if (!string.IsNullOrEmpty(mklId))
                {
                    var lContacts = SingaporeTicketEntity.GetActiveContactQuery(localContext, mklId);
                    localContext.Trace($"Found {lContacts.Count} Active Contacts with ed_MklId {mklId}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        if(eContact.Id != preImage.ed_Contact?.Id)
                        {
                            uTicketInfo.ed_Contact = eContact.ToEntityReference();
                            needsUpdate = true;
                        }
                    }
                }

                if (needsUpdate)
                {
                    uTicketInfo.Id = Id;
                    XrmHelper.Update(localContext, uTicketInfo);
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesUpdateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
