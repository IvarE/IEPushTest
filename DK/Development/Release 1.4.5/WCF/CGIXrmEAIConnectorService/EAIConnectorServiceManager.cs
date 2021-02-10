using System;
using CGIXrmEAIConnectorService.Shared;
using CGIXrmWin;

namespace CGIXrmEAIConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    
    public class EAIConnectorServiceManager
    {
        #region Declarations
        private readonly XrmManager _xrmManager;
        private readonly XrmHelper _xrmHelper;
        #endregion

        #region Constructor
        public EAIConnectorServiceManager()
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }

        public EAIConnectorServiceManager(Guid callerId)
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }
         #endregion

        #region Internal Methods 
        internal Guid GetAgentUserId(string callguideAgentName)
        {
            Guid userId = _xrmHelper.GetId(callguideAgentName, "internalemailaddress", "systemuser", _xrmManager);
            return userId;

        }
        #endregion
    } 
}
