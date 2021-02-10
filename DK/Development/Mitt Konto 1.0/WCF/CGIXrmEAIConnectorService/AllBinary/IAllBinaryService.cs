using System;
using System.ServiceModel;
using CGIXrmEAIConnectorService.AllBinary.Models;
using CGIXrmEAIConnectorService.Shared.Models;

namespace CGIXrmEAIConnectorService.AllBinary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface IAllBinaryService
    {
        #region Public Properties
        [OperationContract]
        GetCustomerDetailsResponse GetCustomerDetails(Guid customerId,AccountCategoryCode customerType,Guid callerId);
        [OperationContract]
        GetCustomerIdForTravelCardResponse GetCustomerIdForTravelCard(string[] travelCard, Guid callerId);
        #endregion
    }
}
