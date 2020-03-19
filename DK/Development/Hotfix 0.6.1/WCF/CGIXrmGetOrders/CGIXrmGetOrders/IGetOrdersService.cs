using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmGetOrders
{

    [ServiceContract]
    public interface IGetOrdersService
    {

        [OperationContract]
        GetOrderResponse GetOrders(string customerId, string orderNumber, string from, string to, string email);

        [OperationContract]
        GetCreditOrderResponse CreditOrder(CreditOrderRequest request);

        [OperationContract]
        SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid);

    }

}
