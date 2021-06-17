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
        internal void HandlePostTicketInfoCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                //This can't be done on the Pre Event because SSIS is running to import these records
                //It can't be syncronos
                TicketInfoEntity eTicketInfo = new TicketInfoEntity();
                eTicketInfo.Id = this.Id;

                string name = this.ed_name;
                string contactNumber = this.ed_CRMNumber;
                string offerName = this.FormattedValues["ed_offername"];

                eTicketInfo.ed_name = contactNumber + "_" + offerName;

                if (!string.IsNullOrEmpty(contactNumber))
                {
                    localContext.Trace(contactNumber);
                    QueryExpression queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId);
                    queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, contactNumber);

                    var lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);

                    localContext.Trace($"Found {lContacts.Count} Contacts with cgi_ContactNumber {contactNumber}");
                    if (lContacts.Count == 1)
                    {
                        var eContact = lContacts.FirstOrDefault();
                        var erContact = new EntityReference(eContact.LogicalName, eContact.Id);

                        eTicketInfo.ed_Contact = erContact;
                    }
                }

                XrmHelper.Update(localContext, eTicketInfo);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostTicketInfoCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }
    }
}
