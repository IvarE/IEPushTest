using CGIXrmHandler.CrmClasses;
using CGIXrmWin;

namespace CGIXrmHandler
{
    public class IncidentExtended : Incident
    {
        #region Public Properties
        // nulable didnt work in CgiXrmWin attribute decorated classes
        // always serialize to false, even with defaultvalue attribute 
        // to fix CgiXrmWin needs to be handeling bool? diffrently
        // therefor subclassed for this specific instance
        [Xrm("cgi_contactcustomer")]
        public bool ContactCustomer { get; set; }

        #endregion
    }
}
