using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CGIXrmWin;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    
    public class EAIConnectorServiceManager
    {
        #region Global Variables
        private XrmManager xrmManager;
        private XrmHelper xrmHelper;
        #endregion

        #region Constructor
        public EAIConnectorServiceManager()
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public EAIConnectorServiceManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
         #endregion

        #region Internal Methods 
        internal Guid GetAgentUserId(string callguideAgentName)
        {
            Guid userId = xrmHelper.GetId(callguideAgentName, "internalemailaddress", "systemuser", xrmManager);
            return userId;

        }
        #endregion
    } 
}
