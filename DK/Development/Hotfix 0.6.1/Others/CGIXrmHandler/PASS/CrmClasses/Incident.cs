using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmHandler.PASS.CrmClasses
{
    [XrmEntity("incident")]
    public class Incident : XrmBaseEntity
    {
        private Guid _IncidentId;
        [XrmPrimaryKey]
        [Xrm("incidentid")]
        public Guid IncidentId
        {
            get { return _IncidentId; }
            set { _IncidentId = value; }
        }

        private EntityReference _Account;
        [Xrm("cgi_accountid")]
        public EntityReference Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        private EntityReference _Contact;
        [Xrm("cgi_contactid")]
        public EntityReference Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        EntityReference _Customer;
        [Xrm("customerid")]
        public EntityReference Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }

        OptionSetValue _CaseOrigin;
        [Xrm("caseorigincode")]
        public OptionSetValue CaseOrigin
        {
            get { return _CaseOrigin; }
            set { _CaseOrigin = value; }
        }

        private string _Title;
        [Xrm("title")]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

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

 
    }
}
