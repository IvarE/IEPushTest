using System.ServiceModel;
using CGIXrmGetOrders.Classes;

namespace CGIXrmGetOrders
{
    [ServiceContract]
    public interface IGetOrdersService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderNumber"></param>
        /// <param name="cardNumber"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [OperationContract]
        GetOrderResponse GetOrders(
            string customerId, 
            string orderNumber, 
            string cardNumber, 
            string from, 
            string to, 
            string email);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        GetCreditOrderResponse CreditOrder(CreditOrderRequest request);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="countactid"></param>
        /// <returns></returns>
        [OperationContract]
        SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid);
        
        #endregion
    }
}
