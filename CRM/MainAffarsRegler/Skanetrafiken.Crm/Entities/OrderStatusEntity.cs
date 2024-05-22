using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderStatusEntity : Generated.ed_OrderStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="orderLineStatus"></param>
        /// <returns></returns>
        public static OrderStatusEntity FindOrCreateOrderStatus(Plugin.LocalPluginContext localContext, string orderLineStatus)
        {
            var query = new QueryExpression()
            {
                EntityName = OrderStatusEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(OrderStatusEntity.Fields.ed_name),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(OrderStatusEntity.Fields.ed_name, ConditionOperator.Equal, orderLineStatus),
                        new ConditionExpression(OrderStatusEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_OrderStatusState.Active)
                    }
                }
            };

            OrderStatusEntity ordStatus = XrmRetrieveHelper.RetrieveFirst<OrderStatusEntity>(localContext, query);

            if (ordStatus == null)
            {
                localContext.TracingService.Trace($"Could not find orderstatus '{orderLineStatus}'.");
                var newOrderStatus = new OrderStatusEntity()
                {
                    ed_name = orderLineStatus
                };

                newOrderStatus.Id = XrmHelper.Create(localContext, newOrderStatus);
                ordStatus = newOrderStatus;
            }

            return ordStatus;
        }
    }
}
