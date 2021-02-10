using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface IAllBinaryService
    {
        
        [OperationContract]
        GetCustomerDetailsResponse GetCustomerDetails(Guid customerId,AccountCategoryCode customerType,Guid callerId);
        [OperationContract]
        GetCustomerIdForTravelCardResponse GetCustomerIdForTravelCard(string[] travelCard, Guid callerId);
    }
}
