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

        public GetOrderResponse GetOrders(string customerId, string orderNumber, string cardNumber, string from, string to, string email)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.GetOrders(customerId, orderNumber, cardNumber, from, to, email);
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
        /*
        public GetAccountResponse GetAccount(string accountId)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.GetAccount(accountId);
        }

        public GetContactResponse GetContact(string contactId)
        {
            GetOrdersServiceManager getOrdersServiceManager = new GetOrdersServiceManager();
            return getOrdersServiceManager.GetContact(contactId);

        }
        */
    }
}
