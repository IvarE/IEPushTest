using System;
using System.ServiceModel;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IEAIConnectorService
    {
        #region Public Methods
        [OperationContract]
        Guid GetAgentUserId(string crmuserName);

        // TODO: Add your service operations here
        #endregion
    }
}
