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
        public static void HandleQuoteEntityUpdate(Plugin.LocalPluginContext localContext, QuoteEntity quote, QuoteEntity preImage)
        {
            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                if (quote != null && quote.StateCode != null && quote.StateCode == Generated.QuoteState.Closed)
                {
                    QuoteEntity.UpdateSlotsInfo(localContext, quote);
                }

                if (quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountPercentage) || quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountAmount))
                {
                    //Note Change logic (maybe remove this call) after using orderProduct and quoteProduct discounts instead
                    //Quote discount % - remove if Skåne wants. 
                    QuoteEntity.UpdateSlotsCustomPrice(localContext, quote, preImage);
                }
            }
            
        }

        public static void UpdateSlotsCustomPrice(Plugin.LocalPluginContext localContext, QuoteEntity quote, QuoteEntity preImage)
        {
            decimal? discountPercentage = null;
            decimal? discountAmount = null;
            if(quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountPercentage) && quote.DiscountPercentage != null && quote.DiscountPercentage > 0)
            {
                discountPercentage = quote.DiscountPercentage.Value;
            }
            else if(quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountAmount) && quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
            {
                discountAmount = quote.DiscountAmount.Value;
            }

            QueryExpression querySlots = new QueryExpression();
            querySlots.EntityName = SlotsEntity.EntityLogicalName;
            querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice);

            FilterExpression filterSlots = new FilterExpression();
            filterSlots.FilterOperator = LogicalOperator.And;
            filterSlots.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Equal, quote.Id);

            querySlots.Criteria.AddFilter(filterSlots);

            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            if(slots != null && slots.Count > 0)
            {
                if (discountPercentage != null && discountPercentage.Value > 0)
                {
                    //update all customPrice slots with this percentage
                    foreach (SlotsEntity slot in slots)
                    {
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)));

                            XrmHelper.Update(localContext, slotToUpdate);
                        }
                    }
                }
                else if (discountAmount != null && discountAmount.Value > 0)
                {
                    decimal discountPerSlot = discountAmount.Value / slots.Count;

                    foreach (SlotsEntity slot in slots)
                    {
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this quote
                            //divide the discount amount by the number of slots
                            // subtract the result to the customPrice
                            //update all slots
                        }
                    }
                }
                else
                {
                    //update customprice of all slots with their defaultPriceValue
                    foreach (SlotsEntity slot in slots)
                    {
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this quote
                            //divide the discount amount by the number of slots
                            // subtract the result to the customPrice
                            //update all slots
                        }
                    }
                }
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
                    filterSlots.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, quoteP.Id);

                    querySlots.Criteria.AddFilter(filterSlots);

                    List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

                    if(slots != null && slots.Count > 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, true,quoteP.Id, null);
                    }
                }
                
            }

        }
        
    }
}
