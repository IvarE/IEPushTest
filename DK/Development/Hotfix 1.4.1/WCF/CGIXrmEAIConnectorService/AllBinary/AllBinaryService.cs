
using System;
using System.Collections.Generic;
using System.ServiceModel;
namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
    public partial class EAIConnectorService : IAllBinaryService
    {
        #region Public Methods
        public GetCustomerDetailsResponse GetCustomerDetails(Guid customerId,AccountCategoryCode customerType, Guid callerId)
        {
            AllBinaryManager allBinaryMgr = new AllBinaryManager(callerId);
            return allBinaryMgr.GetCustomerDetails(customerId,customerType);

        }

        public GetCustomerIdForTravelCardResponse GetCustomerIdForTravelCard(string[] travelCard, Guid callerId)
        {
            AllBinaryManager allBinaryMgr = new AllBinaryManager(callerId);
            return allBinaryMgr.GetCustomerIdForTravelCard(travelCard);
        }
        #endregion
    }
}
