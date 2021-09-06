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

            CalculateRollupFieldRequest request = new CalculateRollupFieldRequest
            {

                Target = new EntityReference("quote", quote.QuoteId.Value), // Entity Reference of record that needs updating

                FieldName = "ed_totalextendedamount" // Rollup Field Name

            };

            CalculateRollupFieldResponse response = (CalculateRollupFieldResponse)localContext.OrganizationService.Execute(request);

            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                if (quote != null && quote.StateCode != null && quote.StateCode == Generated.QuoteState.Closed)
                {
                    QuoteEntity.UpdateSlotsInfo(localContext, quote);
                }

                //if (quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountPercentage) || quote.IsAttributeModified(preImage, QuoteEntity.Fields.DiscountAmount))
                //{
                    //Note Change logic (maybe remove this call) after using orderProduct and quoteProduct discounts instead
                    //Quote discount % - remove if Skåne wants. 
                    QuoteEntity.UpdateSlotsCustomPrice(localContext, quote, preImage);
                //}


            }
            
        }

        public static void UpdateSlotsCustomPrice(Plugin.LocalPluginContext localContext, QuoteEntity quote, QuoteEntity preImage)
        {
            decimal? discountPercentage = null;
            decimal? discountAmount = null;

            if (preImage.ed_DiscountPercentage != null && preImage.ed_DiscountPercentage > 0)
            {
                discountPercentage = preImage.ed_DiscountPercentage.Value;
            }
            else if (preImage.DiscountAmount != null && preImage.DiscountAmount.Value > 0)
            {
                discountAmount = preImage.DiscountAmount.Value;
            }

            if (quote.ed_DiscountPercentage != null && quote.ed_DiscountPercentage > 0)
            {
                discountPercentage = quote.ed_DiscountPercentage.Value;
            }
            else if(quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
            {
                discountAmount = quote.DiscountAmount.Value;
            }

            QueryExpression querySlots = new QueryExpression();
            querySlots.EntityName = SlotsEntity.EntityLogicalName;
            querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice, SlotsEntity.Fields.ed_DiscountAmount);

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
                            localContext.Trace($"looping trough slots discountPercentage: {discountPercentage}");

                            decimal previousDiscountAmount = 0;

                            if (slot.ed_DiscountAmount != null && slot.ed_DiscountAmount.Value != 0)
                            {
                                localContext.Trace($"Slot standard price: {slot.ed_StandardPrice.Value} Slot custom price: {slot.ed_CustomPrice.Value}");
                                previousDiscountAmount = (slot.ed_DiscountAmount.Value * (discountPercentage.Value / 100));


                            }

                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;

                            if (previousDiscountAmount == 0)
                            {
                                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)));
                            }
                            else
                            {
                                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)) - previousDiscountAmount);
                            }
                            
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

                            if (slot.ed_DiscountAmount != null && slot.ed_DiscountAmount.Value != 0)
                            {
                                slotToUpdate.ed_CustomPrice = new Money((slot.ed_StandardPrice.Value - discountPerSlot) - slot.ed_DiscountAmount.Value);
                            }
                            else
                            {
                                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
                            }
                                
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

                           if (slot.ed_DiscountAmount != null && slot.ed_DiscountAmount.Value != 0)
                           {
                                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - slot.ed_DiscountAmount.Value);
                           }
                           else
                           {
                                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value);
                           }
                            
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
