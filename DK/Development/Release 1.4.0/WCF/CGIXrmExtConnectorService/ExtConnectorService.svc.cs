using System;
using System.ServiceModel;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService
{
    public class ExtConnectorService : IExtConnectorService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        public Guid GetAgentUserId(string callguideAgentName)
        {
            try
            {
                var extConnectorServiceManager = new ExtConnectorServiceManager();

                return extConnectorServiceManager.GetAgentUserId(callguideAgentName);
            }
            catch (Exception ex)
            {
                throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                {
                    ApplicationName = "CGIXrmExtConnectorService",
                    Message = ex.Message,
                    Detail = ex.InnerException != null ? ex.InnerException.Message : string.Empty,
                    Source = "GetAgentUserId"
                }, "No Matching User Found");
            }
        }

        #endregion
    }
    
}
