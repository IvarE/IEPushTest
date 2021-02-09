using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class EAIConnectorService : IEAIConnectorService
    {
        public Guid GetAgentUserId(string callguideAgentName)
        {
            try
            {
                EAIConnectorServiceManager eaiConnectorServiceManager = new EAIConnectorServiceManager();
                return eaiConnectorServiceManager.GetAgentUserId(callguideAgentName);

            }
            catch (Exception Ex)
            {
                throw new FaultException<ExtConnectorServiceFault>(new ExtConnectorServiceFault
                {
                    ApplicationName = "CGIXrmEAIConnectorService",
                    Message = Ex.Message,
                    Detail = Ex.InnerException != null ? Ex.InnerException.Message : string.Empty,
                    Source = "GetAgentUserId"
                }, "No Matching User Found");
            }
        }
    }
}
