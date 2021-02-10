using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmEAIConnectorService.Shared.CrmClasses
{
    [XrmEntity("cgi_setting")]
    public class Settings
    {
        #region Public Properties

        [Xrm("cgi_validfrom",DecodePart=XrmDecode.Formatted)]
        public string ValidFrom { get; set; }

        [Xrm("cgi_validto", DecodePart = XrmDecode.Formatted)]
        public string ValidTo { get; set; }

        [Xrm("cgi_defaultcustomeroncase")]
        public EntityReference DefaultCustomerOnCase { get; set; }

        #endregion
    }
}
