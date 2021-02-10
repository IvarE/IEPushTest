using System;
using CGIXrmExtConnectorService.Shared;
using CGIXrmWin;

namespace CGIXrmExtConnectorService
{
    public class ExtConnectorServiceManager
    {
        #region Declaration -------------------------------------------------------------------------------------------
        
        private readonly XrmManager _xrmManager;
        private readonly XrmHelper _xrmHelper;

        #endregion

        #region Constructor -------------------------------------------------------------------------------------------

        public ExtConnectorServiceManager()
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(Guid.Empty);
        }
        public ExtConnectorServiceManager(Guid callerId)
        {
            _xrmHelper = new XrmHelper();
            _xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(callerId);
        }

         #endregion

        #region Internal Methods --------------------------------------------------------------------------------------

        internal Guid GetAgentUserId(string callguideAgentName)
        {
            var userId = _xrmHelper.GetId(callguideAgentName, "internalemailaddress", "systemuser", _xrmManager);
            return userId;

        }

        #endregion
    } 
}
