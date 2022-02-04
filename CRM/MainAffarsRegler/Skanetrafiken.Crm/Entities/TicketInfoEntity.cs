using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class TicketInfoEntity : Generated.ed_ticketinfo
    {
        internal void HandlePostTicketPurchasesCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                Generated.ed_ticketinfo uTicketInfo = new Generated.ed_ticketinfo();
                uTicketInfo.Id = this.Id;

                string offerName = this.FormattedValues["ed_offername"];
                string contactNumber = this.ed_CRMNumber;
                uTicketInfo.ed_name = contactNumber + "_" + offerName;

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    localContext.Trace(contactNumber);
                    QueryExpression queryContacts = new QueryExpression(Generated.Contact.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet = new ColumnSet(Generated.Contact.Fields.ContactId);
                    queryContacts.Criteria.AddCondition(Generated.Contact.Fields.cgi_ContactNumber, ConditionOperator.Equal, contactNumber);
                    var lContacts = XrmRetrieveHelper.RetrieveMultiple<Generated.Contact>(localContext, queryContacts);
                    localContext.Trace($"Found {lContacts.Count} Contacts with cgi_ContactNumber {contactNumber}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);
                        uTicketInfo.ed_Contact = erContact;
                    }
                }

                XrmHelper.Update(localContext, uTicketInfo);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostTicketPurchasesUpdateAsync(Plugin.LocalPluginContext localContext, Generated.ed_ticketinfo preImage)
        {
            try
            {
                Generated.ed_ticketinfo uTicketInfo = new Generated.ed_ticketinfo();
                uTicketInfo.Id = this.Id;

                string offerName = string.IsNullOrEmpty(this.FormattedValues["ed_offername"]) ? preImage.FormattedValues["ed_offername"] : this.FormattedValues["ed_offername"];
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNumber) ? preImage.ed_CRMNumber : this.ed_CRMNumber;
                uTicketInfo.ed_name = contactNumber + "_" + offerName;

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    localContext.Trace(contactNumber);
                    QueryExpression queryContacts = new QueryExpression(Generated.Contact.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet = new ColumnSet(Generated.Contact.Fields.ContactId);
                    queryContacts.Criteria.AddCondition(Generated.Contact.Fields.cgi_ContactNumber, ConditionOperator.Equal, contactNumber);
                    var lContacts = XrmRetrieveHelper.RetrieveMultiple<Generated.Contact>(localContext, queryContacts);
                    localContext.Trace($"Found {lContacts.Count} Contacts with cgi_ContactNumber {contactNumber}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);
                        uTicketInfo.ed_Contact = erContact;
                    }
                }

                XrmHelper.Update(localContext, uTicketInfo);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketPurchasesUpdateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
