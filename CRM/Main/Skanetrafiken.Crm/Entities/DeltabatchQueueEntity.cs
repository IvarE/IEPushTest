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
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class DeltabatchQueueEntity : Generated.ed_DeltabatchQueue
    {
        public static IList<DeltabatchQueueEntity> RetrieveDeltabatchQueuesWithSameSocialSecurityNumber(Plugin.LocalPluginContext localContext, string socialSecurityNumber, Generated.ed_DeltabatchQueueState state, Generated.ed_deltabatchqueue_ed_deltabatchoperation operation)
        {
            IList<DeltabatchQueueEntity> deltabatchQueueMatches = new List<DeltabatchQueueEntity>();

            if (socialSecurityNumber != "")
            {
                deltabatchQueueMatches = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, new ColumnSet(
                    DeltabatchQueueEntity.Fields.Id,
                    DeltabatchQueueEntity.Fields.ed_Contact),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, socialSecurityNumber),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, ConditionOperator.Equal, (int)operation),
                            new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)state)
                        }
                    });
            }

            return deltabatchQueueMatches;
        }
    }


}