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
        #region General
        Guid _IncidentId;
        [XrmPrimaryKey]
        [Xrm("incidentid")]
        public Guid IncidentId
        {
            get { return _IncidentId; }
            set { _IncidentId = value; }
        }

        /*
        private bool? _cgi_ContactCustomer;
        [Xrm("cgi_contactcustomer")]
        public bool? ContactCustomer
        {
            get { return _cgi_ContactCustomer; }
            set { _cgi_ContactCustomer = value; }
        }*/

        EntityReference _Account;
        [Xrm("cgi_accountid")]
        public EntityReference Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        EntityReference _Contact;
        [Xrm("cgi_contactid")]
        public EntityReference Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        EntityReference _DefaultCustomer;
        [Xrm("customerid")]
        public EntityReference DefaultCustomer
        {
            get { return _DefaultCustomer; }
            set { _DefaultCustomer = value; }
        }

        string _Title;
        [Xrm("title")]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /* Added by HF in changeset 3716, however this breaks the functionality as 
         * DefaultCustomer exist that serializes to same crm field.
        EntityReference _Customer;
        [Xrm("customerid")]
        public EntityReference Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }
         */

        OptionSetValue _CaseType;
        [Xrm("casetypecode")]
        public OptionSetValue CaseType
        {
            get { return _CaseType; }
            set { _CaseType = value; }
        }

        string _CaseTypeText;
        [Xrm("casetypecode", DecodePart = XrmDecode.Formatted)]
        public string CaseTypeText
        {
            get { return _CaseTypeText; }
            set { _CaseTypeText = value; }
        }

        string _CaseTypeValue;
        [Xrm("casetypecode", DecodePart = XrmDecode.Value)]
        public string CaseTypeValue
        {
            get { return _CaseTypeValue; }
            set { _CaseTypeValue = value; }
        }


        OptionSetValue _CaseOrigin;
        [Xrm("caseorigincode")]
        public OptionSetValue CaseOrigin
        {
            get { return _CaseOrigin; }
            set { _CaseOrigin = value; }
        }

        string _CaseOriginText;
        [Xrm("caseorigincode", DecodePart = XrmDecode.Formatted)]
        public string CaseOriginText
        {
            get { return _CaseOriginText; }
            set { _CaseOriginText = value; }
        }

        string _CaseOriginValue;
        [Xrm("caseorigincode", DecodePart = XrmDecode.Value)]
        public string CaseOriginValue
        {
            get { return _CaseOriginValue; }
            set { _CaseOriginValue = value; }
        }
        #endregion

        #region CallGuide
        string _TelephoneNumber;
        [Xrm("cgi_telephonenumber")]
        public string TelephoneNumber
        {
            get { return _TelephoneNumber; }
            set { _TelephoneNumber = value; }
        }

        EntityReference _SourceFBActivity;
        [Xrm("cgi_facebookpostid")]
        public EntityReference SourceFBActivity
        {
            get { return _SourceFBActivity; }
            set { _SourceFBActivity = value; }
        }

        EntityReference _SourceChatActivity;
        [Xrm("cgi_chatid")]
        public EntityReference SourceChatActivity
        {
            get { return _SourceChatActivity; }
            set { _SourceChatActivity = value; }
        }
        EntityReference _CallGuideInfo;
        [Xrm("cgi_callguideinfo")]
        public EntityReference CallGuideInfo
        {
            get { return _CallGuideInfo; }
            set { _CallGuideInfo = value; }
        }

        EntityReference _Category1;
        [Xrm("cgi_casdet_row1_cat1id")]
        public EntityReference Category1
        {
            get { return _Category1; }
            set { _Category1 = value; }
        }
        EntityReference _Category2;
        [Xrm("cgi_casdet_row1_cat2id")]
        public EntityReference Category2
        {
            get { return _Category2; }
            set { _Category2 = value; }
        }
        EntityReference _Category3;
        [Xrm("cgi_casdet_row1_cat3id")]
        public EntityReference Category3
        {
            get { return _Category3; }
            set { _Category3 = value; }
        }
        #endregion

        #region PASS
        private string _sFN;
        [Xrm("cgi_sfn")]
        public string sFN
        {
            get { return _sFN; }
            set { _sFN = value; }
        }

        private string _sLN;
        [Xrm("cgi_sln")]
        public string sLN
        {
            get { return _sLN; }
            set { _sLN = value; }
        }

        private string _sA;
        [Xrm("cgi_sa")]
        public string sA
        {
            get { return _sA; }
            set { _sA = value; }
        }

        private string _sPA;
        [Xrm("cgi_spa")]
        public string sPA
        {
            get { return _sPA; }
            set { _sPA = value; }
        }

        private string _sPH;
        [Xrm("cgi_sph")]
        public string sPH
        {
            get { return _sPH; }
            set { _sPH = value; }
        }

        private string _sPW;
        [Xrm("cgi_spw")]
        public string sPW
        {
            get { return _sPW; }
            set { _sPW = value; }
        }

        private string _sPM;
        [Xrm("cgi_spm")]
        public string sPM
        {
            get { return _sPM; }
            set { _sPM = value; }
        }

        private string _sPF;
        [Xrm("cgi_spf")]
        public string sPF
        {
            get { return _sPF; }
            set { _sPF = value; }
        }

        private string _sEM;
        [Xrm("cgi_sem")]
        public string sEM
        {
            get { return _sEM; }
            set { _sEM = value; }
        }

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

        private string _sPR;
        [Xrm("cgi_spr")]
        public string sPR
        {
            get { return _sPR; }
            set { _sPR = value; }
        }

        private string _sOP1;
        [Xrm("cgi_sop1")]
        public string sOP1
        {
            get { return _sOP1; }
            set { _sOP1 = value; }
        }

        private string _sOP2;
        [Xrm("cgi_sop2")]
        public string sOP2
        {
            get { return _sOP2; }
            set { _sOP2 = value; }
        }

        private string _sTTJ;
        [Xrm("cgi_sttj")]
        public string sTTJ
        {
            get { return _sTTJ; }
            set { _sTTJ = value; }
        }

        private int? _iBID;
        [Xrm("cgi_ibid")]
        public int? iBID
        {
            get { return _iBID; }
            set { _iBID = value; }
        }

        private int? _iDC;
        [Xrm("cgi_idc")]
        public int? iDC
        {
            get { return _iDC; }
            set { _iDC = value; }
        }

        [Xrm("cgi_circulationnameinpass1")]
        public string circulationNameInPass1 { get; set; }

        [Xrm("cgi_circulationnameinpass2")]
        public string circulationNameInPass2 { get; set; }

        [Xrm("cgi_operatorpass1")]
        public string operatorPass1 { get; set; }

        [Xrm("cgi_operatorpass2")]
        public string operatorPass2 { get; set; }
        #endregion

    }
}