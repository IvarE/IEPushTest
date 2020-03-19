using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmExtConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface ICallGuideService
    {
        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        string GetChat(string interactionId, Guid callerId);
        
        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        string GetFBMessage(string interactionId, Guid callerId);

        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        Guid ExecuteNewCallRequest(CallGuideRequest callguideRequest, Guid callerId);

        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        Guid ExecuteCallTransferRequest(string sessionId, Guid callerId);

        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        Guid ExecuteChatRequest(CallGuideRequest callguideChatRequest, Guid callerId);

        [OperationContract]
        [FaultContract(typeof(ExtConnectorServiceFault))]
        Guid ExecuteFBRequest(CallGuideRequest callguideFBRequest, Guid callerId);
    }
}
