using System;
using CGIXrmWin;

namespace CGIXrmRainDanceExport.Classes
{
    public class contact
    {
        #region Public Properties
        //<attribute name='contactid' />";
        [Xrm("contactid")]
        public Guid Contactid { get; set; }

        //<attribute name='lastname' />";
        [Xrm("lastname")]
        public string Lastname { get; set; }

        //<attribute name='firstname' />";
        [Xrm("firstname")]
        public string Firstname { get; set; }

        //<attribute name='address1_line1' />";
        [Xrm("address1_line2")]
        public string Address1_line1 { get; set; }

        //<attribute name='address1_city' />";
        [Xrm("address1_city")]
        public string Address1_city { get; set; }

        //<attribute name='address1_postalcode' />";
        [Xrm("address1_postalcode")]
        public string Address1_postalcode { get; set; }
        #endregion
    }
}
