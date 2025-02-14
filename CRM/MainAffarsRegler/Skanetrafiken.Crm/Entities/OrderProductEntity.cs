﻿using Endeavor.Crm;
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
    public class OrderProductEntity : Generated.SalesOrderDetail
    {
        public static void HandleOrderProductEntityCreate (Plugin.LocalPluginContext localContext, OrderProductEntity orderProduct)
        {
            #region Slot removal
            //FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //    if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //    {
            //    if (!string.IsNullOrEmpty(orderProduct.ed_QuoteProductIDTXT))
            //    {
            //        //This means that the OrderProduct was created from the QuoteProduct Won
            //        Guid quoteProductId = new Guid(orderProduct.ed_QuoteProductIDTXT);

            //        if (quoteProductId != null && quoteProductId != Guid.Empty)
            //        {
            //            UpdateExistingSlots(localContext, quoteProductId, orderProduct.ToEntityReference(), orderProduct.SalesOrderId);
            //            //UpdateSlotsCustomPriceFromOrderProduct(localContext, orderProduct);
            //        }

            //        if (orderProduct.SalesOrderId != null)
            //        {
            //            OrderEntity order = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, orderProduct.SalesOrderId, new ColumnSet(OrderEntity.Fields.ed_discountpercentage, OrderEntity.Fields.DiscountAmount));



            //            UpdateSlotsCustomPriceFromOrderProduct(localContext, orderProduct);



            //            if (order.ed_discountpercentage != null && order.ed_discountpercentage > 0 || order.DiscountAmount != null && order.DiscountAmount.Value > 0)
            //            {
            //                UpdateSlotsCustomPrice(localContext, order);
            //            }
            //        }
            //    }
            //    else
            //    {

            //        if (orderProduct.UoMId != null && orderProduct.ProductId != null && orderProduct.ed_FromDate != null && orderProduct.ed_ToDate != null)
            //        {
            //            bool isSlotProduct = false;

            //            if (orderProduct.ProductId != null && orderProduct.ProductId.Id != Guid.Empty)
            //            {
            //                isSlotProduct = ProductEntity.IsSlotProduct(localContext, orderProduct.ProductId);
            //            }

            //            if (isSlotProduct)
            //            {
            //                UpdateOrGenerateSlots(localContext, orderProduct); 

            //            }
            //        }
            //    }
            //}
            #endregion

        }

        public static void UpdateSlotsCustomPrice(Plugin.LocalPluginContext localContext, OrderEntity order)
        {
            #region Slot removal
            //decimal? discountPercentage = null;
            //decimal? discountAmount = null;
            //if (order.ed_discountpercentage != null && order.ed_discountpercentage > 0)
            //{
            //    discountPercentage = order.ed_discountpercentage.Value;
            //}
            //else if (order.DiscountAmount != null && order.DiscountAmount.Value > 0)
            //{
            //    discountAmount = order.DiscountAmount.Value;
            //}

            //QueryExpression querySlots = new QueryExpression();
            //querySlots.EntityName = SlotsEntity.EntityLogicalName;
            //querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice, SlotsEntity.Fields.ed_DiscountAmount);

            //FilterExpression filterSlots = new FilterExpression();
            //filterSlots.FilterOperator = LogicalOperator.And;
            //filterSlots.AddCondition(SlotsEntity.Fields.ed_Order, ConditionOperator.Equal, order.Id);

            //querySlots.Criteria.AddFilter(filterSlots);

            //List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            //if (slots != null && slots.Count > 0)
            //{
            //    if (discountPercentage != null && discountPercentage.Value > 0)
            //    {
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

            //                //SlotsEntity slotToUpdate = new SlotsEntity();
            //                //slotToUpdate.Id = slot.Id;
            //                //slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - (slot.ed_StandardPrice.Value * (discountPercentage.Value / 100)));

            //                //XrmHelper.Update(localContext, slotToUpdate);
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

            //                //SlotsEntity slotToUpdate = new SlotsEntity();
            //                //slotToUpdate.Id = slot.Id;
            //                //slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
            //                //XrmHelper.Update(localContext, slotToUpdate);

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

        public static void HandleOrderProductEntityUpdate(Plugin.LocalPluginContext localContext, OrderProductEntity target,OrderProductEntity preImage)
        {
            #region Slot removal
            //localContext.Trace("Inside HandleOrderProductEntityUpdate");

            //localContext.Trace("PreImage Info");
            //preImage.Trace(localContext.TracingService);
            //localContext.Trace("_______________");

            //localContext.Trace("Target Info");
            //target.Trace(localContext.TracingService);
            //localContext.Trace("_______________");

            //if (!target.IsAttributeModified(preImage, OrderProductEntity.Fields.UoMId))
            //{
            //    localContext.Trace("UoMID not modified");
            //    target.UoMId = preImage.UoMId;
            //}
            //if (!target.IsAttributeModified(preImage, OrderProductEntity.Fields.ProductId))
            //{
            //    localContext.Trace("ProductId not modified");
            //    target.ProductId = preImage.ProductId;
            //}

            ////validate necessary things to generateSlots
            //if (target.UoMId != null && target.ProductId != null)
            //{
            //    localContext.Trace("UoMId and ProductId not NULL");
            //    FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //    if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //    {
            //        localContext.Trace("ed_bookingSystem enabled");
            //        bool isSlotProduct = false;
            //        //check if the Product on this QuoteProduct has non extended slots already created (method to see if this Product is a slot Product)
            //        if (target.ProductId != null && target.ProductId.Id != Guid.Empty)
            //        {
            //            isSlotProduct = ProductEntity.IsSlotProduct(localContext, target.ProductId);
            //        }
            //        if (isSlotProduct)
            //        {
            //            if (target.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_FromDate) || target.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_ToDate) || target.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_ManualDiscount))
            //            {
            //                localContext.Trace("ed_FromDate or ed_ToDate modified");
            //                UpdateOrGenerateSlots(localContext, target, preImage);
                            
                            
            //                UpdateSlotsCustomPriceFromOrderProduct(localContext, target);

            //                if (target.SalesOrderId != null)
            //                {
            //                    OrderEntity order = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, target.SalesOrderId, new ColumnSet(OrderEntity.Fields.ed_discountpercentage, OrderEntity.Fields.DiscountAmount));


            //                    if (order.ed_discountpercentage != null && order.ed_discountpercentage > 0 || order.DiscountAmount != null && order.DiscountAmount.Value > 0)
            //                    {
            //                        UpdateSlotsCustomPrice(localContext, order);
            //                    }

            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        public static void HandlePreValidationOrderProductEntityDelete(Plugin.LocalPluginContext localContext, EntityReference targetER)
        {
            #region Slot removal
            //if (targetER != null && targetER.Id != Guid.Empty)
            //{
            //    SlotsEntity.ReleaseSlots(localContext, true, null, targetER.Id);
            //}
            #endregion
        }
        public static void UpdateExistingSlots(Plugin.LocalPluginContext localContext, Guid guidQuoteProduct, EntityReference refOrderProduct, EntityReference refOrder)
        {
            QueryExpression query = new QueryExpression();
            query.EntityName = SlotsEntity.EntityLogicalName;
            query.ColumnSet = new ColumnSet(false);

            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition(SlotsEntity.Fields.ed_QuoteProductID, ConditionOperator.Equal, guidQuoteProduct);

            query.Criteria.AddFilter(filter);

            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, query);

            if(slots != null && slots.Count > 0)
            {
                foreach(SlotsEntity slot in slots)
                {
                    SlotsEntity slotToUpdate = new SlotsEntity();
                    slotToUpdate.Id = slot.Id;
                    slotToUpdate.ed_OrderProductID = refOrderProduct;
                    slotToUpdate.ed_Order = refOrder;
                    //slotToUpdate.ed_CustomPrice = slotToUpdate.ed_StandardPrice;
                    XrmHelper.Update(localContext,slotToUpdate);
                }
            }
        }

        public static void UpdateOrGenerateSlots(Plugin.LocalPluginContext localContext, OrderProductEntity orderProduct, OrderProductEntity preImage = null)
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

            if (orderProduct.SalesOrderId != null && orderProduct.SalesOrderId.Id != Guid.Empty)
            {
                OrderEntity order = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, orderProduct.SalesOrderId, new ColumnSet(OrderEntity.Fields.OpportunityId));

                if (order != null && order.OpportunityId != null && order.OpportunityId.Id != Guid.Empty)
                {
                    opportunityId = order.OpportunityId.Id;
                }
            }
            else
            {
                if (preImage != null && !orderProduct.IsAttributeModified(preImage, OrderProductEntity.Fields.SalesOrderId) && preImage.SalesOrderId != null
                    && preImage.SalesOrderId.Id != Guid.Empty)
                {
                    OrderEntity order = XrmRetrieveHelper.Retrieve<OrderEntity>(localContext, preImage.SalesOrderId, new ColumnSet(OrderEntity.Fields.OpportunityId));

                    if (order != null && order.OpportunityId != null && order.OpportunityId.Id != Guid.Empty)
                    {
                        opportunityId = order.OpportunityId.Id;
                    }

                    orderProduct.SalesOrderId = preImage.SalesOrderId;
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

                if (preImage.ed_ToDate != null)
                {
                    preToDate = preImage.ed_ToDate.Value;
                }

                if (orderProduct.ed_FromDate != null)
                {
                    postFromDate = orderProduct.ed_FromDate.Value;
                }
                else
                {
                    if (!orderProduct.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_FromDate) && preImage.ed_FromDate != null)
                    {
                        postFromDate = preImage.ed_FromDate.Value;
                    }
                }

                if (orderProduct.ed_ToDate != null)
                {
                    postToDate = orderProduct.ed_ToDate.Value;
                }
                else
                {
                    if (!orderProduct.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_ToDate) && preImage.ed_ToDate != null)
                    {
                        postToDate = preImage.ed_ToDate.Value;
                    }
                }
                if (postFromDate == null && postToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if (postFromDate != null && postToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }

                if (postFromDate != null && postToDate != null && postFromDate > postToDate)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }

                if (preFromDate == null && preToDate == null)
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, orderProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, orderProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots, opportunityId, null,orderProduct);
                }
                else if (postFromDate == null && postToDate == null)
                {
                    SlotsEntity.ReleaseSlots(localContext, true, null,orderProduct.Id);
                }
                else if (DateTime.Compare(postFromDate.Value, preToDate.Value) > 0 || DateTime.Compare(preFromDate.Value, postToDate.Value) > 0)
                {
                    SlotsEntity.ReleaseSlots(localContext, false, null, orderProduct.Id, preFromDate, preToDate, 1);
                    availableSlots = SlotsEntity.AvailableSlots(localContext, orderProduct.ProductId, postFromDate.Value, postToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, orderProduct.ProductId.Id, 1, postFromDate.Value, postToDate.Value, availableSlots, opportunityId, null,orderProduct);
                }
                else
                {
                    var compareFrom = DateTime.Compare(preFromDate.Value, postFromDate.Value);
                    if (compareFrom > 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, orderProduct.ProductId, postFromDate.Value, preFromDate.Value.AddDays(-1));
                        SlotsEntity.GenerateSlotsInternal(localContext, orderProduct.ProductId.Id, 1, postFromDate.Value, preFromDate.Value.AddDays(-1), availableSlots, opportunityId, null,orderProduct);
                    }
                    else if (compareFrom < 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, false, null, orderProduct.Id, preFromDate.Value, postFromDate.Value.AddDays(-1), 1);
                    }

                    var compareTo = DateTime.Compare(preToDate.Value, postToDate.Value);
                    if (compareTo < 0)
                    {
                        availableSlots = SlotsEntity.AvailableSlots(localContext, orderProduct.ProductId, preToDate.Value.AddDays(1), postToDate.Value);
                        SlotsEntity.GenerateSlotsInternal(localContext, orderProduct.ProductId.Id, 1, preToDate.Value.AddDays(1), postToDate.Value, availableSlots, opportunityId, null, orderProduct);
                    }
                    else if (compareTo > 0)
                    {
                        SlotsEntity.ReleaseSlots(localContext, false, null, orderProduct.Id, postToDate.Value.AddDays(1), preToDate.Value, 1);
                    }
                }
            }
            else
            {
                if (orderProduct.ed_FromDate == null && orderProduct.ed_ToDate == null)
                {
                    return;
                }
                else if (orderProduct.ed_FromDate == null && orderProduct.ed_ToDate != null)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be empty if ToDate is set");
                }
                else if (orderProduct.ed_FromDate != null && orderProduct.ed_ToDate == null)
                {
                    throw new InvalidPluginExecutionException("ToDate cannot be empty if FromDate is set");
                }
                else if (DateTime.Compare(orderProduct.ed_FromDate.Value, orderProduct.ed_ToDate.Value) > 0)
                {
                    throw new InvalidPluginExecutionException("FromDate cannot be after ToDate");
                }
                else
                {
                    availableSlots = SlotsEntity.AvailableSlots(localContext, orderProduct.ProductId, orderProduct.ed_FromDate.Value, orderProduct.ed_ToDate.Value);
                    SlotsEntity.GenerateSlotsInternal(localContext, orderProduct.ProductId.Id, 1, orderProduct.ed_FromDate.Value, orderProduct.ed_ToDate.Value, availableSlots, opportunityId, null,orderProduct);
                }


            }
        }

        public void HandlePreOrderProductUpdate(Plugin.LocalPluginContext localContext, OrderProductEntity preImage)
        {
            #region slot removal
            //FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            //if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            //{
            //    updatePriceOrderProduct(localContext, preImage);
            //}
            #endregion
        }

        public void updatePriceOrderProduct(Plugin.LocalPluginContext localContext, OrderProductEntity preImage)
        {
            if (this.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_totalslots) || this.IsAttributeModified(preImage, OrderProductEntity.Fields.PricePerUnit) || this.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_ManualDiscount))
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

                UnitEntity orderProductUnit = XrmRetrieveHelper.RetrieveFirst<UnitEntity>(localContext, queryUnit);


                if (this.UoMId != null)
                {
                    unit = this.UoMId.Name;
                }
                else
                {
                    unit = preImage.UoMId.Name;
                }

                localContext.Trace($"inside updatePriceOrderProduct");

                if (!this.IsAttributeModified(preImage, OrderProductEntity.Fields.PricePerUnit))
                {
                    price = preImage.PricePerUnit.Value;
                }
                else
                {
                    price = this.PricePerUnit.Value;
                }

                if (!this.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_ManualDiscount))
                {
                    localContext.Trace($"inside !this.IsAttributeModified(preImage, QuoteProductEntity.Fields.ed_ManualDiscount)");
                    if (preImage.ed_ManualDiscount == null || preImage.ManualDiscountAmount == null)
                    {
                        manualDiscount = 0;
                    }
                    else
                    {
                        manualDiscount = preImage.ed_ManualDiscount.Value - preImage.ManualDiscountAmount.Value;
                    }
                    localContext.Trace($"Manual Discount amount: {manualDiscount}");

                }
                else
                {
                    localContext.Trace($"inside manual discount not changed");
                    if (this.ed_ManualDiscount == null || this.ManualDiscountAmount == null)
                    {
                        manualDiscount = 0;
                    }
                    else
                    {
                        manualDiscount = this.ed_ManualDiscount.Value - this.ManualDiscountAmount.Value;
                    }
                    localContext.Trace($"Manual Discount amount: {manualDiscount}");
                }

                if (!this.IsAttributeModified(preImage, OrderProductEntity.Fields.Quantity))
                {
                    localContext.Trace($"Inside quantity changed");
                    quantity = (int)preImage.Quantity.Value;
                    localContext.Trace($"quantity amount: {quantity}");
                }
                else
                {
                    localContext.Trace($"Inside quantity not changed");
                    quantity = (int)this.Quantity.Value;
                    localContext.Trace($"quantity amount: {quantity}");
                }
                if (!this.IsAttributeModified(preImage, OrderProductEntity.Fields.ed_totalslots))
                {
                    localContext.Trace($"Total slots changed");

                    localContext.Trace($"preImage: {preImage.ed_totalslots}");
                    localContext.Trace($"this: {this.ed_totalslots}");
                    totalSlots = preImage.ed_totalslots.Value;
                    localContext.Trace($"totalSlots: {totalSlots}");
                }
                else
                {
                    localContext.Trace($"Total slots not changed");
                    localContext.Trace($"preImage: {preImage.ed_totalslots}");
                    localContext.Trace($"this: {this.ed_totalslots}");
                    totalSlots = this.ed_totalslots.Value;
                    localContext.Trace($"totalSlots: {totalSlots}");
                }
                localContext.Trace($"Almost Done");

                localContext.Trace($"Manual Discount amount: {manualDiscount}");

                if (unit == orderProductUnit.Name)
                {
                    this.Tax = new Money(0 - manualDiscount);
                }
                else
                {
                    this.Tax = new Money((totalSlots - quantity) * price - manualDiscount);
                }

                
            }
        }

        public static void UpdateSlotsCustomPriceFromOrderProduct(Plugin.LocalPluginContext localContext, OrderProductEntity orderProduct)
        {
            decimal? discountAmount = null;

            if (orderProduct.ed_ManualDiscount != null && orderProduct.ed_ManualDiscount.Value > 0)
            {
                discountAmount = orderProduct.ed_ManualDiscount.Value;
            }

            QueryExpression querySlots = new QueryExpression();
            querySlots.EntityName = SlotsEntity.EntityLogicalName;
            querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice, SlotsEntity.Fields.ed_DiscountAmount);

            FilterExpression filterSlots = new FilterExpression();
            filterSlots.FilterOperator = LogicalOperator.And;
            filterSlots.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, orderProduct.Id);

            querySlots.Criteria.AddFilter(filterSlots);

            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            if (slots != null && slots.Count > 0)
            {
                if (discountAmount != null && discountAmount.Value > 0)
                {
                    decimal discountPerSlot = discountAmount.Value / slots.Count;

                    foreach (SlotsEntity slot in slots)
                    {
                        if (slot.ed_StandardPrice != null && slot.ed_StandardPrice.Value > 0)
                        {
                            SlotsEntity slotToUpdate = new SlotsEntity();
                            slotToUpdate.Id = slot.Id;
                            slotToUpdate.ed_DiscountAmount = discountPerSlot;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value - discountPerSlot);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this orderproduct
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

                            
                            slotToUpdate.ed_DiscountAmount = 0;
                            slotToUpdate.ed_CustomPrice = new Money(slot.ed_StandardPrice.Value);
                            XrmHelper.Update(localContext, slotToUpdate);

                            //get all slots conected to this orderproduct
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
