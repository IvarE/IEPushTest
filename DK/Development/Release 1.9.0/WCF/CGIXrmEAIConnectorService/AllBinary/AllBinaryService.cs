using System;
using CGIXrmEAIConnectorService.AllBinary.Models;
using CGIXrmEAIConnectorService.Shared.Models;

namespace CGIXrmEAIConnectorService.AllBinary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
    public class EAIConnectorService : IAllBinaryService
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
