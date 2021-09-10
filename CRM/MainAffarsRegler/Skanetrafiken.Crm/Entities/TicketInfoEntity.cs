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
    public class TicketInfoEntity : Generated.ed_ticketinfo
    {
        internal void HandlePreTicketInfoCreateSync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                string contactNumber = this.ed_CRMNumber;
                string offerName = this.FormattedValues["ed_offername"];

                this.ed_name = contactNumber + "_" + offerName;

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

                        this.ed_Contact = erContact;
                    }
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePreTicketInfoCreateSync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePreTicketInfoUpdateSync(Plugin.LocalPluginContext localContext, Generated.ed_ticketinfo preImage)
        {
            try
            {
                string offerName = string.IsNullOrEmpty(this.FormattedValues["ed_offername"]) ? preImage.FormattedValues["ed_offername"] : this.FormattedValues["ed_offername"];
                string contactNumber = string.IsNullOrEmpty(this.ed_CRMNumber) ? preImage.ed_CRMNumber : this.ed_CRMNumber;

                this.ed_name = contactNumber + "_" + offerName;

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

                        this.ed_Contact = erContact;
                    }
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePreTicketInfoCreateSync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
