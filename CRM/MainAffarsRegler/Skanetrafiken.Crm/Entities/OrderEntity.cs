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
    public class OrderEntity :  Generated.SalesOrder
    {
        public static void HandleOrderEntityUpdate(Plugin.LocalPluginContext localContext, OrderEntity order, OrderEntity preImage)
        {
            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_bookingsystem);
            if (feature != null && feature.ed_bookingsystem != null && feature.ed_bookingsystem == true)
            {
                if (order != null && order.StateCode != null && order.StateCode == Generated.SalesOrderState.Canceled)
                {

                    OrderEntity.UpdateSlotsInfo(localContext, order);
                }
                else if (order != null && order.StateCode != null && order.StateCode == Generated.SalesOrderState.Fulfilled)
                {
                    OrderEntity.UpdateSlotsInfo(localContext, order,true);
                }

                if (order.IsAttributeModified(preImage, OrderEntity.Fields.DiscountPercentage) || order.IsAttributeModified(preImage, OrderEntity.Fields.DiscountAmount))
                {
                    //Note Change logic (maybe remove this call) after using orderProduct and quoteProduct discounts instead

                    OrderEntity.UpdateSlotsCustomPrice(localContext, order, preImage);
                }
            }

        }

        public static void UpdateSlotsCustomPrice(Plugin.LocalPluginContext localContext, OrderEntity order, OrderEntity preImage)
        {
            decimal? discountPercentage = null;
            decimal? discountAmount = null;
            if (order.ed_discountpercentage != null && order.ed_discountpercentage > 0)
            {
                discountPercentage = order.ed_discountpercentage.Value;
            }
            else if (order.DiscountAmount != null && order.DiscountAmount.Value > 0)
            {
                discountAmount = order.DiscountAmount.Value;
            }

            QueryExpression querySlots = new QueryExpression();
            querySlots.EntityName = SlotsEntity.EntityLogicalName;
            querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_StandardPrice, SlotsEntity.Fields.ed_CustomPrice);

            FilterExpression filterSlots = new FilterExpression();
            filterSlots.FilterOperator = LogicalOperator.And;
            filterSlots.AddCondition(SlotsEntity.Fields.ed_Order, ConditionOperator.Equal, order.Id);

            querySlots.Criteria.AddFilter(filterSlots);

            List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

            if (slots != null && slots.Count > 0)
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

                        }
                    }
                }
            }
        }

        public static void UpdateSlotsInfo(Plugin.LocalPluginContext localContext, OrderEntity order, bool won = false)
        {
            QueryExpression queryOrderProducts = new QueryExpression();
            queryOrderProducts.EntityName = OrderProductEntity.EntityLogicalName;
            queryOrderProducts.ColumnSet = new ColumnSet(false);

            FilterExpression filterOrderProducts = new FilterExpression();
            filterOrderProducts.FilterOperator = LogicalOperator.And;
            filterOrderProducts.AddCondition(OrderProductEntity.Fields.SalesOrderId, ConditionOperator.Equal, order.Id);

            queryOrderProducts.Criteria.AddFilter(filterOrderProducts);

            List<OrderProductEntity> orderProducts = XrmRetrieveHelper.RetrieveMultiple<OrderProductEntity>(localContext, queryOrderProducts);

            if (orderProducts != null && orderProducts.Count > 0)
            {
                foreach (OrderProductEntity orderP in orderProducts)
                {
                    QueryExpression querySlots = new QueryExpression();
                    querySlots.EntityName = SlotsEntity.EntityLogicalName;
                    querySlots.ColumnSet = new ColumnSet(SlotsEntity.Fields.ed_Extended);

                    FilterExpression filterSlots = new FilterExpression();
                    filterSlots.FilterOperator = LogicalOperator.And;
                    filterSlots.AddCondition(SlotsEntity.Fields.ed_OrderProductID, ConditionOperator.Equal, orderP.Id);

                    querySlots.Criteria.AddFilter(filterSlots);

                    List<SlotsEntity> slots = XrmRetrieveHelper.RetrieveMultiple<SlotsEntity>(localContext, querySlots);

                    if (slots != null && slots.Count > 0)
                    {
                        if(!won)
                        {
                            SlotsEntity.ReleaseSlots(localContext, true, null, orderP.Id);
                        }
                        else
                        {
                            foreach(SlotsEntity slot in slots)
                            {
                                if(slot != null && slot.Id != Guid.Empty)
                                {
                                    SlotsEntity slotToUpdate = new SlotsEntity();
                                    slotToUpdate.Id = slot.Id;
                                    slotToUpdate.ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Booked;

                                    XrmHelper.Update(localContext, slotToUpdate);
                                }
                            }

                        }
                        
                    }
                }

            }

        }


    }
}
