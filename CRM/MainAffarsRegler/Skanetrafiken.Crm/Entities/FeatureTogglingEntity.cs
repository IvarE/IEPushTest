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
using System.Globalization;

namespace Skanetrafiken.Crm.Entities
{
    public class FeatureTogglingEntity : Generated.ed_FeatureToggling
    {
        public static FeatureTogglingEntity GetFeatureToggling(Plugin.LocalPluginContext localContext, string attribute)
        {
            QueryExpression queryFeatureToggling = new QueryExpression()
            {
                EntityName = FeatureTogglingEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(attribute),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(FeatureTogglingEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_FeatureTogglingState.Active)
                    }
                }
            };

            return XrmRetrieveHelper.RetrieveFirst<FeatureTogglingEntity>(localContext, queryFeatureToggling);
        }

        public static FeatureTogglingEntity GetFeatureToggling(Plugin.LocalPluginContext localContext, List<string> attributes)
        {
            ColumnSet columnSet = new ColumnSet();

            foreach (var attribute in attributes)
            {
                columnSet.AddColumn(attribute);
            }

            QueryExpression queryFeatureToggling = new QueryExpression()
            {
                EntityName = FeatureTogglingEntity.EntityLogicalName,
                ColumnSet = columnSet,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(FeatureTogglingEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_FeatureTogglingState.Active)
                    }
                }
            };

            return XrmRetrieveHelper.RetrieveFirst<FeatureTogglingEntity>(localContext, queryFeatureToggling);
        }

        public static FeatureTogglingEntity GetFeatureToggling(Plugin.LocalPluginContext localContext, bool allAttributes)
        {
            QueryExpression queryFeatureToggling = new QueryExpression()
            {
                EntityName = FeatureTogglingEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(allAttributes),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(FeatureTogglingEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_FeatureTogglingState.Active)
                    }
                }
            };

            return XrmRetrieveHelper.RetrieveFirst<FeatureTogglingEntity>(localContext, queryFeatureToggling);
        }
    }
}
