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
                if (order != null && order.StateCode != null && order.StatusCode == Generated.salesorder_statuscode.NoMoney)
                {

                    OrderEntity.UpdateSlotsInfo(localContext, order);
                }
                else if (order != null && order.StatusCode != null && order.StatusCode == Generated.salesorder_statuscode.Invoiced)
                {
                    OrderEntity.UpdateSlotsInfo(localContext, order,true);
                }

                if (order.IsAttributeModified(preImage, OrderEntity.Fields.DiscountPercentage) || order.IsAttributeModified(preImage, OrderEntity.Fields.DiscountAmount))
                {
                    //Note Change logic (maybe remove this call) after using orderProduct and quoteProduct discounts instead

                    //OrderEntity.UpdateSlotsCustomPrice(localContext, order, preImage);
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
