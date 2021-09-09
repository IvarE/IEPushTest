using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class SingaporeTicketEntity : Generated.st_singaporeticket
    {
        internal void HandlePreSingaporeTicketCreateSync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                string contactNumber = this.ed_CRMNummer;
                string offerName = this.FormattedValues["st_singtickettype"];

                this.st_name = contactNumber + "_" + offerName;

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    localContext.Trace(contactNumber);
                    QueryExpression queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId);
                    queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, contactNumber);

                    var lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);

                    localContext.Trace($"Found {lContacts.Count} Contacts with ed_MklId {contactNumber}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);

                        this.st_ContactID = erContact;
                    }
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePreSingaporeTicketCreateSync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePreSingaporeTicketUpdateSync(Plugin.LocalPluginContext localContext, SingaporeTicketEntity preImage)
        {
            try
            {
                string offerName = string.IsNullOrEmpty(this.FormattedValues["st_singtickettype"]) ? preImage.FormattedValues["st_singtickettype"] : this.FormattedValues["st_singtickettype"];
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNummer) ? preImage.ed_CRMNummer : this.ed_CRMNummer;

                this.st_name = contactNumber + "_" + offerName;

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    localContext.Trace(contactNumber);
                    QueryExpression queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId);
                    queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, contactNumber);

                    var lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);

                    localContext.Trace($"Found {lContacts.Count} Contacts with ed_MklId {contactNumber}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);

                        this.st_ContactID = erContact;
                    }
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePreSingaporeTicketUpdateSync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
