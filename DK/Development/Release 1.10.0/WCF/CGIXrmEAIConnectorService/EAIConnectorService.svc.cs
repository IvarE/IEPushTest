using System;
using System.ServiceModel;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EAIConnectorService : IEAIConnectorService
    {
        #region Public Methods
        public Guid GetAgentUserId(string callguideAgentName)
        {
            try
            {
                EAIConnectorServiceManager eaiConnectorServiceManager = new EAIConnectorServiceManager();
                return eaiConnectorServiceManager.GetAgentUserId(callguideAgentName);

            }
            catch (Exception ex)
            {
                throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                {
                    ApplicationName = "CGIXrmEAIConnectorService",
                    Message = ex.Message,
                    Detail = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                    Source = "GetAgentUserId"
                }, "No Matching User Found");
            }
        }
        #endregion
    }
}
