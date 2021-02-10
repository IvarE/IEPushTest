using System;
using CGIXrmWin;

namespace CGIXrmRainDanceExport.Classes
{
    public class incident
    {
        #region Public Properties
        //<attribute name='ticketnumber' />";
        [Xrm("ticketnumber")]
        public string Ticketnumber { get; set; }

        //<attribute name='cgi_contactid' />";
        [Xrm("cgi_contactid", DecodePart = XrmDecode.Value)]
        public Guid? Contactid { get; set; }

        //<attribute name='cgi_accountid' />";
        [Xrm("cgi_accountid", DecodePart = XrmDecode.Value)]
        public Guid? Accountid { get; set; }

        [Xrm("description", DecodePart = XrmDecode.Value)]
        public string Description
        {
            get;
            set;
        }
        [Xrm("createdby", DecodePart = XrmDecode.Name)]
        public string CreatedByName
        {
            get;
            set;
        }
        #endregion
    }
}
