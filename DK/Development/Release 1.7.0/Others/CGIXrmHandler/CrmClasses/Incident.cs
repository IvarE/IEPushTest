using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.InteropServices;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("incident")]
    [ComVisibleAttribute(false)]
    public class Incident : XrmBaseEntity
    {
        #region Public Properties
        #region General

        [XrmPrimaryKey]
        [Xrm("incidentid")]
        public Guid IncidentId { get; set; }

        [Xrm("cgi_accountid")]
        public EntityReference Account { get; set; }

        [Xrm("cgi_contactid")]
        public EntityReference Contact { get; set; }

        [Xrm("customerid")]
        public EntityReference DefaultCustomer { get; set; }

        [Xrm("title")]
        public string Title { get; set; }

        [Xrm("casetypecode")]
        public OptionSetValue CaseType { get; set; }

        [Xrm("casetypecode", DecodePart = XrmDecode.Formatted)]
        public string CaseTypeText { get; set; }

        [Xrm("casetypecode", DecodePart = XrmDecode.Value)]
        public string CaseTypeValue { get; set; }


        [Xrm("caseorigincode")]
        public OptionSetValue CaseOrigin { get; set; }

        [Xrm("caseorigincode", DecodePart = XrmDecode.Formatted)]
        public string CaseOriginText { get; set; }

        [Xrm("caseorigincode", DecodePart = XrmDecode.Value)]
        public string CaseOriginValue { get; set; }

        #endregion

        #region CallGuide

        [Xrm("cgi_telephonenumber")]
        public string TelephoneNumber { get; set; }

        [Xrm("cgi_facebookpostid")]
        public EntityReference SourceFBActivity { get; set; }

        [Xrm("cgi_chatid")]
        public EntityReference SourceChatActivity { get; set; }

        [Xrm("cgi_callguideinfo")]
        public EntityReference CallGuideInfo { get; set; }

        [Xrm("cgi_casdet_row1_cat1id")]
        public EntityReference Category1 { get; set; }

        [Xrm("cgi_casdet_row1_cat2id")]
        public EntityReference Category2 { get; set; }

        [Xrm("cgi_casdet_row1_cat3id")]
        public EntityReference Category3 { get; set; }

        #endregion

        #region PASS

        [Xrm("cgi_sfn")]
        public string sFN { get; set; }

        [Xrm("cgi_sln")]
        public string sLN { get; set; }

        [Xrm("cgi_sa")]
        public string sA { get; set; }

        [Xrm("cgi_spa")]
        public string sPA { get; set; }

        [Xrm("cgi_sph")]
        public string sPH { get; set; }

        [Xrm("cgi_spw")]
        public string sPW { get; set; }

        [Xrm("cgi_spm")]
        public string sPM { get; set; }

        [Xrm("cgi_spf")]
        public string sPF { get; set; }

        [Xrm("cgi_sem")]
        public string sEM { get; set; }

        private string _sSSN;
        [Xrm("cgi_sssn")]
        public string sSSN
        {
            get { return _sSSN; }
            set
            {
                _sSSN = value;
                if (_sSSN.Length == 10)
                {
                    string _ssNoa = _sSSN.Substring(0, 6);
                    string _ssNob = _sSSN.Substring(6, 4);
                    _sSSN = string.Format("19{0}-{1}", _ssNoa, _ssNob);
                }
            }
        }

        [Xrm("cgi_spr")]
        public string sPR { get; set; }

        [Xrm("cgi_sop1")]
        public string sOP1 { get; set; }

        [Xrm("cgi_sop2")]
        public string sOP2 { get; set; }

        [Xrm("cgi_sttj")]
        public string sTTJ { get; set; }

        [Xrm("cgi_ibid")]
        public int? iBID { get; set; }

        [Xrm("cgi_idc")]
        public int? iDC { get; set; }

        [Xrm("cgi_circulationnameinpass1")]
        public string circulationNameInPass1 { get; set; }

        [Xrm("cgi_circulationnameinpass2")]
        public string circulationNameInPass2 { get; set; }

        [Xrm("cgi_operatorpass1")]
        public string operatorPass1 { get; set; }

        [Xrm("cgi_operatorpass2")]
        public string operatorPass2 { get; set; }
        #endregion
        #endregion
    }
}