using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class QuoteProductEntity : Generated.QuoteDetail
    {
        public void HandlePreQuoteProductUpdate(Plugin.LocalPluginContext localContext, QuoteProductEntity preImage)
        {
            #region slot removal
            //FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //{
            //    updatePriceQuoteProduct(localContext, preImage);
            //}
            #endregion
        }
        public static void HandleQuoteProductEntityCreate(Plugin.LocalPluginContext localContext, QuoteProductEntity quoteProduct)
        {
            #region slot removal
            //localContext.Trace("Inside HandleQuoteProductEntityCreate");
            ////validate necessary things to generateSlots
            //if(quoteProduct.Id != null)
            //{
            //    QuoteProductEntity updateQuoteProduct = new QuoteProductEntity();
            //    updateQuoteProduct.Id = quoteProduct.Id;

            //    var idString = quoteProduct.Id.ToString();

            //    idString = idString.Replace("{", "");
            //    idString = idString.Replace("}", "");

            //    updateQuoteProduct.ed_QuoteProductIDTXT = idString;

            //    XrmHelper.Update(localContext, updateQuoteProduct);

            //}
            //if (quoteProduct.UoMId != null && quoteProduct.ProductId != null && quoteProduct.ed_FromDate != null && quoteProduct.ed_ToDate != null)
            //{
            //    FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //    if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //    {
            //        bool isSlotProduct = false;

            //        if(quoteProduct.ProductId != null && quoteProduct.ProductId.Id != Guid.Empty)
            //        {
            //            isSlotProduct = ProductEntity.IsSlotProduct(localContext, quoteProduct.ProductId);
            //        }

            //        if(isSlotProduct)
            //        {
            //            UpdateOrGenerateSlots(localContext, quoteProduct);

            //            if(quoteProduct.QuoteId != null)
            //            {
            //                QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, quoteProduct.QuoteId, new ColumnSet(QuoteEntity.Fields.ed_DiscountPercentage, QuoteEntity.Fields.DiscountAmount));




            //                    UpdateSlotsCustomPriceFromQuoteProduct(localContext, quoteProduct);



            //                if (quote.ed_DiscountPercentage != null && quote.ed_DiscountPercentage > 0 || quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
            //                {
            //                    UpdateSlotsCustomPrice(localContext, quote);
            //                }

            //            }


            //        }
            //    }

            //}


            //if (quoteProduct.QuoteId != null)
            //{
            //    QuoteEntity thisQuote = XrmRetrieveHelper.Retrieve<QuoteEntity>(
            //        localContext,
            //        quoteProduct.QuoteId.Id,
            //        new ColumnSet(QuoteEntity.Fields.OpportunityId, QuoteEntity.Fields.QuoteId, QuoteEntity.Fields.CreatedOn, QuoteEntity.Fields.TotalAmount)
            //    );


            //    IList<QuoteEntity> quotes = XrmRetrieveHelper.RetrieveMultiple<QuoteEntity>(localContext,
            //        new ColumnSet(QuoteEntity.Fields.CreatedOn),
            //        new FilterExpression()
            //        {
            //            Conditions =
            //            {
            //                new ConditionExpression(QuoteEntity.Fields.OpportunityId, ConditionOperator.Equal,
            //                    thisQuote.OpportunityId.Id),
            //                new ConditionExpression(QuoteEntity.Fields.QuoteId, ConditionOperator.NotEqual,
            //                    thisQuote.QuoteId)

            //            }
            //        });

            //    localContext.Trace($"Number of Quotes found {quotes.Count}");

            //    bool isLatestQuote = true;

            //    DateTime? quoteCreatedOn = thisQuote.CreatedOn;



            //        foreach (QuoteEntity listQuote in quotes)
            //    {
            //        //localContext.Trace($"listQuote created ón: {listQuote.CreatedOn}. Current Quote Created on: {quoteCreatedOn}");

            //        int result = DateTime.Compare((DateTime)listQuote.CreatedOn, (DateTime)quoteCreatedOn);
            //        localContext.Trace($"Result: {result}");

            //        if (result > 0)
            //        {
            //            isLatestQuote = false;
            //        }

            //        localContext.Trace($"listQuote: {(DateTime)listQuote.CreatedOn}");
            //        localContext.Trace($"thisQuote: {(DateTime)quoteCreatedOn}");
            //        localContext.Trace($"isLatestQuote: {isLatestQuote}");
            //    }

            //    if (isLatestQuote)
            //    {

            //        Money quoteTotalAmount = thisQuote.TotalAmount;

            //        quoteTotalAmount = new Money(thisQuote.TotalAmount.Value + quoteProduct.ExtendedAmount.Value);

            //        OpportunityEntity thisOpportunity = XrmRetrieveHelper.Retrieve<OpportunityEntity>(
            //            localContext,
            //            thisQuote.OpportunityId.Id,
            //            new ColumnSet(OpportunityEntity.Fields.EstimatedValue)
            //        );

            //        //IList<QuoteProductEntity> quoteProducts = XrmRetrieveHelper.RetrieveMultiple<QuoteProductEntity>(localContext,
            //        //    new ColumnSet(QuoteProductEntity.Fields.ExtendedAmount),
            //        //    new FilterExpression()
            //        //    {
            //        //        Conditions =
            //        //        {
            //        //            new ConditionExpression(QuoteProductEntity.Fields.QuoteId, ConditionOperator.Equal,
            //        //                thisQuote.QuoteId),
            //        //            new ConditionExpression(QuoteEntity.Fields.QuoteId, ConditionOperator.NotEqual,
            //        //                thisQuote.QuoteId)

            //        //        }
            //        //    });

            //        localContext.Trace($"quoteTotalAmount: {quoteTotalAmount}");
            //        localContext.Trace($"quoteTotalAmount.value: {quoteTotalAmount.Value}");
            //        if (thisOpportunity != null)
            //        {
            //            OpportunityEntity opportunityToUpdate = new OpportunityEntity
            //            {
            //                Id = thisOpportunity.Id,
            //                IsRevenueSystemCalculated = false,
            //                EstimatedValue = quoteTotalAmount
            //            };


            //            //localContext.Trace($"opportunityToUpdate: {thisOpportunity.Id}");
            //            //localContext.Trace($"opportunityToUpdate.EstimatedValue: {opportunityToUpdate.EstimatedValue.Value}");

            //            //localContext.Trace($"preImageQuote Opp id.id: {quote.OpportunityId.Id}");
            //            XrmHelper.Update(localContext, opportunityToUpdate);
            //        }
            //    }
            //}
            #endregion
        }

        public static void UpdateSlotsCustomPrice(Plugin.LocalPluginContext localContext, QuoteEntity quote)
        {
            #region slot removal
            //localContext.Trace("Inside UpdateSlotsCustomPrice");
            //decimal? discountPercentage = null;
            //decimal? discountAmount = null;
            //if (quote.ed_DiscountPercentage != null && quote.ed_DiscountPercentage > 0)//(quote.DiscountPercentage != null && quote.DiscountPercentage > 0)
            //{
            //    discountPercentage = quote.ed_DiscountPercentage.Value;
            //}
            //else if (quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
            //{
            //    discountAmount = quote.DiscountAmount.Value;
            //}
            //localContext.Trace($"DiscountPercentage = {discountPercentage} ");
            //localContext.Trace($"discountAmount = {discountAmount} ");

            //QueryExpression querySlots = new QueryExpression();
            //querySlots.EntityName = SlotsEntity.EntityLogicalName;
            //querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice, SlotsEntity.Fields.ed_DiscountAmount);

            //FilterExpression filterSlots = new FilterExpression();
            //filterSlots.FilterOperator = LogicalOperator.And;
            //filterSlots.AddCondition(SlotsEntity.Fields.ed_Quote, ConditionOperator.Equal, quote.Id);

            //querySlots.Criteria.AddFilter(filterSlots);

            //List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            //if (slots != null && slots.Count > 0)
            //{
            //    if (discountPercentage != null && discountPercentage.Value > 0)
            //    {
            //        localContext.Trace("discountPercentage not null");
            //        //update all customPrice slots with this percentage
            //        foreach (SlotsEntity slot in slots)
            //        {
            //            if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
            //            {
            //                decimal previousDiscountAmount = 0; 

            //                localContext.Trace($"ed_StandardPrice = {slot.ed_StandardPrice.Value} ");
            //                SlotsEntity slotToUpdate = new SlotsEntity();
            //                slotToUpdate.Id = slot.Id;

            //                if (slot.ed_DiscountAmount != null && slot.ed_DiscountAmount.Value != 0)
            //                {
            //                    localContext.Trace($"Slot standard price: {slot.ed_StandardPrice.Value} Slot custom price: {slot.ed_CustomPrice.Value}");
            //                    previousDiscountAmount = (slot.ed_DiscountAmount.Value * (discountPercentage.Value / 100));


            //                }

            //                if (previousDiscountAmount == 0)
            //                {
            //                    slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)));
            //                }
            //                else
            //                {
            //                    slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)) - previousDiscountAmount);
            //                }

            //                localContext.Trace($"new ed_CustomPrice = {slotToUpdate.ed_CustomPrice} ");
            //                XrmHelper.Update(localContext, slotToUpdate);
                            
            //            }
            //        }
            //    }
            //    else if (discountAmount != null && discountAmount.Value > 0)
            //    {
            //        decimal discountPerSlot = discountAmount.Value / slots.Count;

            //        foreach (SlotsEntity slot in slots)
            //        {
            //            if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
            //            {
            //                SlotsEntity slotToUpdate = new SlotsEntity();
            //                slotToUpdate.Id = slot.Id;

            //                if (slot.ed_DiscountAmount != null && slot.ed_DiscountAmount.Value != 0)
            //                {
            //                    slotToUpdate.ed_CustomPrice = new Money((slot.ed_StandardPrice.Value - discountPerSlot) - slot.ed_DiscountAmount.Value);
            //                }
            //                else
            //                {
            //                    slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
            //                }
                                
            //                XrmHelper.Update(localContext, slotToUpdate);

            //                //get all slots conected to this quote
            //                //divide the discount amount by the number of slots
            //                // subtract the result to the customPrice
            //                //update all slots
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //update customprice of all slots with their defaultPriceValue
            //        foreach (SlotsEntity slot in slots)
            //        {
            //            if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
            //            {
            //                SlotsEntity slotToUpdate = new SlotsEntity();
            //                slotToUpdate.Id = slot.Id;
            //                slotToUpdate.ed_DiscountAmount = 0;
            //                slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value);
            //                XrmHelper.Update(localContext, slotToUpdate);

            //                //get all slots conected to this quote
            //                //divide the discount amount by the number of slots
            //                // subtract the result to the customPrice
            //                //update all slots
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        public static void HandleQuoteProductEntityUpdate(Plugin.LocalPluginContext localContext, QuoteProductEntity target, QuoteProductEntity preImage)
        {
            #region slot removal
            //localContext.Trace("Inside HandleQuoteProductEntityUpdate");

            //localContext.Trace("PreImage Info");
            //preImage.Trace(localContext.TracingService);
            //localContext.Trace("_______________");

            //localContext.Trace("Target Info");
            //target.Trace(localContext.TracingService);
            //localContext.Trace("_______________");

            //bool removeSlotsConnectedPreProduct = false;
            //if (!target.IsAttributeModified(preImage, QuoteProductEntity.Fields.UoMId))
            //{
            //    localContext.Trace("UoMID not modified");
            //    target.UoMId = preImage.UoMId;
            //}
            //if (!target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ProductId))
            //{
            //    localContext.Trace("ProductId not modified");
            //    target.ProductId = preImage.ProductId;
            //}
            //else
            //{
            //    if(preImage.ProductId != null && preImage.ProductId.Id != Guid.Empty)
            //    {
            //        removeSlotsConnectedPreProduct = true;
            //    }
            //}
            //FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //{
            //    if (removeSlotsConnectedPreProduct)
            //    {
            //        //relese all slots connect to the QuoteProduct

            //        //Generate Slots for new product

            //        // Testa ta bort. 
            //    }
            //    else
            //    {
            //        //validate necessary things to generateSlots
            //        if (target.UoMId != null && target.ProductId != null)
            //        {
            //            localContext.Trace("UoMId and ProductId not NULL");

            //            localContext.Trace("ed_bookingSystem enabled");
            //            bool isSlotProduct = false;
            //            //check if the Product on this QuoteProduct has non extended slots already created (method to see if this Product is a slot Product)
            //            if (target.ProductId != null && target.ProductId.Id != Guid.Empty)
            //            {
            //                isSlotProduct = ProductEntity.IsSlotProduct(localContext, target.ProductId);
            //            }
            //            if (isSlotProduct)
            //            {
            //                if (target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_FromDate) || target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate) || target.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ManualDiscount))
            //                {
            //                    localContext.Trace("ed_FromDate or ed_ToDate modified");
            //                    UpdateOrGenerateSlots(localContext, target, preImage);

                                
            //                    UpdateSlotsCustomPriceFromQuoteProduct(localContext, target);

            //                    if (target.QuoteId != null)
            //                    {
            //                        QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, target.QuoteId, new ColumnSet(QuoteEntity.Fields.ed_DiscountPercentage, QuoteEntity.Fields.DiscountAmount));


            //                        if (quote.ed_DiscountPercentage != null && quote.ed_DiscountPercentage > 0 || quote.DiscountAmount != null && quote.DiscountAmount.Value > 0)
            //                        {
            //                            UpdateSlotsCustomPrice(localContext, quote);
            //                        }

            //                    }

            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        public static void HandlePreValidationQuoteProductEntityDelete(Plugin.LocalPluginContext localContext, EntityReference targetER)
        {
            #region slot removal
            //if (targetER != null && targetER.Id != Guid.Empty)
            //{
            //    SlotsEntity.ReleaseSlots(localContext, true, targetER.Id);
            //}
            #endregion
        }
        public void updatePriceQuoteProduct(Plugin.LocalPluginContext localContext, QuoteProductEntity preImage)
        {
            if (this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_totalslots) || this.IsAttributeModified(preImage, QuoteProductEntity.Fields.PricePerUnit) || this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ManualDiscount))
            {
                decimal price = 0;
                decimal manualDiscount = 0; 
                int quantity = 0;
                int totalSlots = 0;
                string unit;

                

                QueryExpression queryUnit = new QueryExpression();
                queryUnit.EntityName = UnitEntity.EntityLogicalName;
                queryUnit.ColumnSet = new ColumnSet(UnitEntity.Fields.Name);
                queryUnit.Criteria.AddCondition(UnitEntity.Fields.Name, ConditionOperator.Like, "1 vecka");

                UnitEntity quoteProductUnit = XrmRetrieveHelper.RetrieveFirst<UnitEntity>(localContext, queryUnit);


                if(this.UoMId != null)
                {
                    unit = this.UoMId.Name;
                }
                else
                {
                    unit = preImage.UoMId.Name;
                }
                localContext.Trace("Inside updatePriceQuoteProduct");

                localContext.Trace($"unit = {unit} quoteProductUnit.UoMId {quoteProductUnit.Name}");

                
               
                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.PricePerUnit))
                {
                    price = preImage.PricePerUnit.Value;
                }
                else
                {
                    price = this.PricePerUnit.Value;
                }



                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ManualDiscount))
                {
                    if (preImage.ed_ManualDiscount == null || preImage.ManualDiscountAmount == null)
                    {
                        manualDiscount = 0;
                    }
                    else
                    {
                        manualDiscount = preImage.ed_ManualDiscount.Value - preImage.ManualDiscountAmount.Value;
                    }

                }
                else
                {
                    if (this.ed_ManualDiscount == null || this.ManualDiscountAmount == null)
                    {
                        manualDiscount = 0;
                    }
                    else
                    {
                        manualDiscount = this.ed_ManualDiscount.Value - this.ManualDiscountAmount.Value;
                    }

                }

                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.Quantity))
                {
                    quantity = (int)preImage.Quantity.Value;
                }
                else
                {
                    quantity = (int)this.Quantity.Value;
                }
                if (!this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_totalslots))
                {
                    totalSlots = preImage.ed_totalslots.Value;
                }
                else
                {
                    totalSlots = this.ed_totalslots.Value;
                }


                if (unit == quoteProductUnit.Name)
                {
                    this.Tax = new Money(0 - manualDiscount);
                }
                else
                {
                    this.Tax = new Money((totalSlots - quantity) * price - manualDiscount);
                }

                
                
            }
        }

        /*
         function UpdateOrGenerateSlots
         -> quoteProduct target of the execution (Create/Update)
         -> preImage (preImage Update)
         */
        public static void UpdateOrGenerateSlots(Plugin.LocalPluginContext localContext, QuoteProductEntity quoteProduct, QuoteProductEntity preImage = null)
        {
            localContext.Trace("Inside UpdateOrGenerateSlots");
            //bool removeAllSlots = false;
            //bool fromDateModified = false;
            //bool toDateModified = false;
            List<SlotsEntity> availableSlots = null;
            //DateTime? startDate = quoteProduct.ed_FromDate;
            //DateTime? endDate = quoteProduct.ed_ToDate;

            //DateTime? startRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            //DateTime? endRemoveIntervalFrom = null; //used if after Update the preImage.ed_FromDate is before the quoteProduct.ed_FromDate
            //DateTime? startRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            //DateTime? endRemoveIntervalTo = null; //used if after Update the preImage.ed_ToDate is after the quoteProduct.ed_ToDate
            //DateTime? startCreateIntervalFrom = null;
            //DateTime? endCreateIntervalFrom = null;
            //DateTime? startCreateIntervalTo = null;
            //DateTime? endCreateIntervalTo = null;

            Guid? opportunityId = null;

            if (quoteProduct.QuoteId != null && quoteProduct.QuoteId.Id != Guid.Empty)
            {
                QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, quoteProduct.QuoteId, new ColumnSet(QuoteEntity.Fields.OpportunityId));

                if (quote != null && quote.OpportunityId != null && quote.OpportunityId.Id != Guid.Empty)
                {
                    opportunityId = quote.OpportunityId.Id;
                }
            }
            else
            {
                if(preImage != null && !quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.QuoteId) && preImage.QuoteId != null
                    && preImage.QuoteId.Id != Guid.Empty)
                {
                    QuoteEntity quote = XrmRetrieveHelper.Retrieve<QuoteEntity>(localContext, preImage.QuoteId, new ColumnSet(QuoteEntity.Fields.OpportunityId));

                    if (quote != null && quote.OpportunityId != null && quote.OpportunityId.Id != Guid.Empty)
                    {
                        opportunityId = quote.OpportunityId.Id;
                    }

                    quoteProduct.QuoteId = preImage.QuoteId;
                }
            }

            if (preImage != null)
            {

                localContext.Trace("PreImage not null.");
                DateTime? preFromDate = null;
                DateTime? preToDate = null;
                DateTime? postFromDate = null;
                DateTime? postToDate = null;

                if (preImage.ed_FromDate != null)
                {
                    preFromDate = preImage.ed_FromDate.Value;
                }

                if(preImage.ed_ToDate != null)
                {
                    preToDate = preImage.ed_ToDate.Value;
                }

                if(quoteProduct.ed_FromDate != null)
                {
                    postFromDate = quoteProduct.ed_FromDate.Value;
                }
                else
                {
                    if(!quoteProduct.IsAttributeModified(preImage,QuoteProductEntity.Fields.ed_FromDate) && preImage.ed_FromDate != null)
                    {
                        postFromDate = preImage.ed_FromDate.Value;
                    }
                }

                if (quoteProduct.ed_ToDate != null)
                {
                    postToDate = quoteProduct.ed_ToDate.Value;
                }
                else
                {
                    if (!quoteProduct.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ToDate) && preImage.ed_ToDate != null)
                    {
                        postToDate = preImage.ed_ToDate.Value;
                    }
                }
                if (postFromDate == null && postToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if(postFromDate != null && postToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }

                if (postFromDate != null && postToDate != null && postFromDate > postToDate)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }
                
                if (preFromDate == null && preToDate == null)
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots, opportunityId, quoteProduct);
                }
                else if(postFromDate == null && postToDate == null)
                {
                    SlotsEntity.ReleaseSlots(localContext, true, quoteProduct.Id);
                }
                else if(DateTime.Compare(postFromDate.Value,preToDate.Value) > 0 || DateTime.Compare(preFromDate.Value, postToDate.Value) > 0)
                {
                    SlotsEntity.ReleaseSlots(localContext, false, quoteProduct.Id, null, preFromDate, preToDate, 1);
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots,opportunityId,quoteProduct);
                }
                else
                {
                    var compareFrom = DateTime.Compare(preFromDate.Value, postFromDate.Value);
                    if (compareFrom > 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, postFromDate.Value, preFromDate.Value.AddDays(-1));
                        SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, postFromDate.Value, preFromDate.Value.AddDays(-1), availableSlots, opportunityId, quoteProduct);
                    }
                    else if (compareFrom < 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, false, quoteProduct.Id, null, preFromDate.Value, postFromDate.Value.AddDays(-1), 1);
                    }

                    var compareTo = DateTime.Compare(preToDate.Value, postToDate.Value);
                    if(compareTo < 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, preToDate.Value.AddDays(1), postToDate.Value);
                        SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, preToDate.Value.AddDays(1), postToDate.Value, availableSlots, opportunityId, quoteProduct);
                    }
                    else if(compareTo > 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, false, quoteProduct.Id, null, postToDate.Value.AddDays(1), preToDate.Value, 1);
                    }
                }
            }
            else
            {
                if (quoteProduct.ed_FromDate == null && quoteProduct.ed_ToDate == null)
                {
                    return;
                }
                else if (quoteProduct.ed_FromDate == null && quoteProduct.ed_ToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if (quoteProduct.ed_FromDate != null && quoteProduct.ed_ToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }
                else if(DateTime.Compare(quoteProduct.ed_FromDate.Value,quoteProduct.ed_ToDate.Value) > 0)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }
                else
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, quoteProduct.ProductId, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, quoteProduct.ProductId.Id, 1, quoteProduct.ed_FromDate.Value, quoteProduct.ed_ToDate.Value, availableSlots, opportunityId, quoteProduct);
                }
                
                
            }
        }

        public static void UpdateSlotsCustomPriceFromQuoteProduct(Plugin.LocalPluginContext localContext, QuoteProductEntity quoteProduct) 
        { 
            decimal? discountAmount = null;
            localContext.Trace("Inside UpdateSlotsCustomPriceFromQuoteProduct");

            quoteProduct.Trace(localContext.TracingService);


            if (quoteProduct.ed_ManualDiscount != null && quoteProduct.ed_ManualDiscount.Value > 0)
            {
                localContext.Trace("Inside manual discount target");
                discountAmount = quoteProduct.ed_ManualDiscount.Value;
                localContext.Trace($"Discount Amount: {discountAmount}");
            }
            

            QueryExpression querySlots = new QueryExpression();
            querySlots.EntityName = SlotsEntity.EntityLogicalName;
            querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice, SlotsEntity.Fields.ed_DiscountAmount);

            FilterExpression filterSlots = new FilterExpression();
            filterSlots.FilterOperator = LogicalOperator.And;
            filterSlots.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, quoteProduct.Id);

            querySlots.Criteria.AddFilter(filterSlots);

            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            localContext.Trace("Slots fetched");

            if (slots != null && slots.Count > 0)
            {
                if (discountAmount != null && discountAmount.Value > 0)
                {
                    localContext.Trace("discountAmount is not null");
                    decimal discountPerSlot = discountAmount.Value / slots.Count;

                    foreach (SlotsEntity slot in slots)
                    {
                        localContext.Trace("loop trough slots");
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_DiscountAmount = discountPerSlot;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this quoteproduct
                            //divide the discount amount by the number of slots
                            // subtract the result to the customPrice
                            //update all slots
                        }

                    
                    }

                }
                else // else if här på manualdiscount? 
                {
                    //update customprice of all slots with their defaultPriceValue
                    foreach (SlotsEntity slot in slots)
                    {
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_DiscountAmount = 0;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this quoteproduct
                            //divide the discount amount by the number of slots
                            // subtract the result to the customPrice
                            //update all slots
                        }
                    }
                }
            }
        }

    }
}