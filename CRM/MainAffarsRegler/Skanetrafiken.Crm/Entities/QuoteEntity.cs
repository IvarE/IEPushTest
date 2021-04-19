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
using System.Net;
using System.Net.Http;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using System.Globalization;
using System.IdentityModel;

namespace Skanetrafiken.Crm.Entities
{
    public class QuoteEntity : Generated.Quote
    {
        public static void HandleQuoteEntityUpdate(Plugin.LocalPluginContext localContext, QuoteEntity quote)
        {
            if(quote != null && quote.StateCode != null && quote.StateCode == Generated.QuoteState.Closed)
            {
                QuoteEntity.UpdateSlotsInfo(localContext, quote);
            }
            
        }

        public static void UpdateSlotsInfo(Plugin.LocalPluginContext localContext, QuoteEntity quote)
        {
            QueryExpression queryQuoteProducts = new QueryExpression();
            queryQuoteProducts.EntityName = QuoteProductEntity.EntityLogicalName;
            queryQuoteProducts.ColumnSet = new ColumnSet(false);

            FilterExpression filterQuoteProducts = new FilterExpression();
            filterQuoteProducts.FilterOperator = LogicalOperator.And;
            filterQuoteProducts.AddCondition(QuoteProductEntity.Fields.QuoteId, ConditionOperator.Equal, quote.Id);

            queryQuoteProducts.Criteria.AddFilter(filterQuoteProducts);

            List<QuoteProductEntity> quoteProducts = XrmRetrieveHelper.RetrieveMultiple<QuoteProductEntity>(localContext, queryQuoteProducts);

            if(quoteProducts != null && quoteProducts.Count > 0)
            {
                foreach(QuoteProductEntity quoteP in quoteProducts)
                {
                    QueryExpression querySlots = new QueryExpression();
                    querySlots.EntityName = SlotsEntity.EntityLogicalName;
                    querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_Extended);

                    FilterExpression filterSlots = new FilterExpression();
                    filterSlots.FilterOperator = LogicalOperator.And;
                    filterSlots.AddCondition(QuoteProductEntity.Fields.QuoteDetailId, ConditionOperator.Equal, quoteP.Id);

                    querySlots.Criteria.AddFilter(filterSlots);

                    List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

                    if(slots != null && slots.Count > 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, quoteP.Id, true);
                    }
                }
                
            }

        }
    }
}
