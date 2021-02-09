using System;
using System.ServiceModel;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService
{
    [ServiceContract]
    public interface IExtConnectorService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        Guid GetAgentUserId(string callguideAgentName);

        #endregion
    } 
}
