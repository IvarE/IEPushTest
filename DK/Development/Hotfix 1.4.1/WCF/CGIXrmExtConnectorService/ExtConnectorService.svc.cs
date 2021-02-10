using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmExtConnectorService
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
    public partial class ExtConnectorService : IExtConnectorService
    {
        #region Public Methods
        public Guid GetAgentUserId(string callguideAgentName)
        {
            try
            {
                ExtConnectorServiceManager extConnectorServiceManager = new ExtConnectorServiceManager();
                return extConnectorServiceManager.GetAgentUserId(callguideAgentName);

            }
            catch (Exception Ex)
            {
                throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                {
                    ApplicationName = "CGIXrmExtConnectorService",
                    Message = Ex.Message,
                    Detail = Ex.InnerException != null ? Ex.InnerException.Message : string.Empty,
                    Source = "GetAgentUserId"
                }, "No Matching User Found");
            }
        }
        #endregion
    }
    
}
