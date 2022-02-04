using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class SingaporeTicketEntity : Generated.st_singaporeticket
    {
        internal void HandlePostSingaporeTicketCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                Generated.st_singaporeticket uSingaporeTicket = new Generated.st_singaporeticket();
                uSingaporeTicket.Id = this.Id;

                string offerName = this.FormattedValues["st_singtickettype"];
                string contactNumber = this.ed_CRMNummer;
                uSingaporeTicket.st_name = contactNumber + "_" + offerName;

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
                        uSingaporeTicket.st_ContactID = erContact;
                    }
                }

                XrmHelper.Update(localContext, uSingaporeTicket);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostSingaporeTicketCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostSingaporeTicketUpdateAsync(Plugin.LocalPluginContext localContext, Generated.st_singaporeticket preImage)
        {
            try
            {
                Generated.st_singaporeticket uSingaporeTicket = new Generated.st_singaporeticket();
                uSingaporeTicket.Id = this.Id;

                string offerName = string.IsNullOrEmpty(this.FormattedValues["st_singtickettype"]) ? preImage.FormattedValues["st_singtickettype"] : this.FormattedValues["st_singtickettype"];
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNummer) ? preImage.ed_CRMNummer : this.ed_CRMNummer;
                uSingaporeTicket.st_name = contactNumber + "_" + offerName;

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
                        uSingaporeTicket.st_ContactID = erContact;
                    }
                }

                XrmHelper.Update(localContext, uSingaporeTicket);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostSingaporeTicketUpdateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
