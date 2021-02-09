using CGIXrmGetOrders.Classes;

namespace CGIXrmGetOrders
{
    public class GetOrdersService : IGetOrdersService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// Method retrieving orders based on given parameters.
        /// </summary>
        /// <param name="customerId">Customer Entity Guid</param>
        /// <param name="orderNumber">Order Number</param>
        /// <param name="cardNumber">Jojo Card Number</param>
        /// <param name="from">Sender</param>
        /// <param name="to">Receiver</param>
        /// <param name="email">E-mail Address</param>
        /// <returns></returns>
        public GetOrderResponse GetOrders(string customerId, string orderNumber, string cardNumber, string from, string to, string email)
        {
            var getOrdersServiceManager = new GetOrdersServiceManager();

            return getOrdersServiceManager.GetOrders(customerId, orderNumber, cardNumber, from, to, email);
        }


        /// <summary>
        /// Method that is creating a credit order.
        /// </summary>
        /// <param name="request">Credit Order containing credit rows</param>
        /// <returns></returns>
        public GetCreditOrderResponse CreditOrder(CreditOrderRequest request)
        {
            var getOrdersServiceManager = new GetOrdersServiceManager();

            return getOrdersServiceManager.CreditOrder(request);
        }


        /// <summary>
        /// Method retrieving credit orders based on given parameters.
        /// </summary>
        /// <param name="accountid">Account Entity Guid</param>
        /// <param name="countactid">Contact Entity Guid</param>
        /// <returns></returns>
        public SavedCreditOrderRowsResponse GetSavedCreditOrderRows(string accountid, string countactid)
        {
            var getOrdersServiceManager = new GetOrdersServiceManager();

            return getOrdersServiceManager.GetSavedCreditOrderRows(accountid, countactid);
        }
        
        #endregion
    }
}
