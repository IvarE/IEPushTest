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
    public class SkaKortEntity : Generated.ed_SKAkort
    {
        public static SkaKortEntity FetchSkaKort(Plugin.LocalPluginContext localContext, string cardNumber, EntityReference contactId = null)
        {

            var query = new QueryExpression()
            {
                EntityName = SkaKortEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(SkaKortEntity.Fields.ed_CardNumber),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, cardNumber)
                    }
                }
            };

            if (contactId != null)
                query.Criteria.AddCondition(new ConditionExpression(SkaKortEntity.Fields.ed_Contact, ConditionOperator.Equal, contactId));

            var skakort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, query);
            return skakort;
        }
    }
}
