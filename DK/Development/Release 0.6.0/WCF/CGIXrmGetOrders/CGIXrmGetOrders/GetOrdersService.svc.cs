using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmGetOrders
{

    public class GetOrdersService : IGetOrdersService
    {

        public GetOrderResponse GetOrders(string customerId, string orderNumber, string from, string to, string email)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.GetOrders(customerId, orderNumber, from, to, email);
        }

        public GetCreditOrderResponse CreditOrder(CreditOrderRequest request)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.CreditOrder(request);
        }

        public SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.GetSavedCreditOrderRows(accountid, countactid);
        }

    }
}
