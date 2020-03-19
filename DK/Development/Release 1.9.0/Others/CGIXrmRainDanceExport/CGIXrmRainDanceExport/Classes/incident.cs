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

        [Xrm("cgi_rgol_address1_line1")]
        public string RgolAddressLine1
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_address1_line2")]
        public string RgolAddressLine2
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_address1_postalcode")]
        public string RgolAddress1Postalcode
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_address1_city")]
        public string RgolAddress1City
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_address1_country")]
        public string RgolAddress1Country
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_fullname")]
        public string RgolFullname
        {
            get;
            set;
        }
        [Xrm("cgi_soc_sec_number")]
        public string SocialSecurityNumber
        {
            get;
            set;
        }
        [Xrm("cgi_rgol_socialsecuritynumber")]
        public string RgolSocialSecurityNumber
        {
            get;
            set;
        }
        [Xrm("caseorigincode", DecodePart = XrmDecode.Value)]
        public int Caseorigincode //= 285050007 = RGOL
        {
            get;
            set;
        }
        #endregion
    }
}
