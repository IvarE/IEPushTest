using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CGIXrmWin;

namespace CGIXrmExtConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    
    public class ExtConnectorServiceManager
    {
        #region [Variable Declaration]
        private XrmManager xrmManager;
        private XrmHelper xrmHelper;

        #endregion

        #region [Constructor]
        public ExtConnectorServiceManager()
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }
        public ExtConnectorServiceManager(Guid callerId)
        {
            xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
         #endregion

        internal Guid GetAgentUserId(string callguideAgentName)
        {
            Guid userId = xrmHelper.GetId(callguideAgentName, "internalemailaddress", "systemuser", xrmManager);
            return userId;

        }
    } 
}
