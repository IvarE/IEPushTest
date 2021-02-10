using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmHandler.PASS.CrmClasses
{
    [XrmEntity("cgi_passtravelinformation")]
    public class PASSTravelInformation : XrmBaseEntity 
    {
        
        private Guid _PASSTravelInformationId;
        [XrmPrimaryKey]
        [Xrm("cgi_passtravelinformationid")]
        public Guid PASSTravelInformationId
        {
            get { return _PASSTravelInformationId; }
            set { _PASSTravelInformationId = value; }
        }

        private string _PASSTravelInformationName;
        [Xrm("cgi_passtravelinformation")]
        public string PASSTravelInformationName
        {
            get { return _PASSTravelInformationName; }
            set { _PASSTravelInformationName = value; }
        }

        private EntityReference _Case;
        [Xrm("cgi_incidentid")]
        public EntityReference Case
        {
            get { return _Case; }
            set { _Case = value; }
        }

        private string _sTCN;
        [Xrm("cgi_stcn")]
        public string sTCN
        {
            get { return _sTCN; }
            set { _sTCN = value; }
        }

        private string _sTCID;
        [Xrm("cgi_stcid")]
        public string sTCID
        {
            get { return _sTCID; }
            set { _sTCID = value; }
        }

        private int? _iTLID;
        [Xrm("cgi_itlid")]
        public int? iTLID
        {
            get { return _iTLID; }
            set { _iTLID = value; }
        }

        private string _sTLN;
        [Xrm("cgi_stln")]
        public string sTLN
        {
            get { return _sTLN; }
            set { _sTLN = value; }
        }

        private string _sTRN;
        [Xrm("cgi_strn")]
        public string sTRN
        {
            get { return _sTRN; }
            set { _sTRN = value; }
        }

        private int? _iTJID;
        [Xrm("cgi_itjid")]
        public int? iTJID
        {
            get { return _iTJID; }
            set { _iTJID = value; }
        }

        private int? _iTFID;
        [Xrm("cgi_itfid")]
        public int? iTFID
        {
            get { return _iTFID; }
            set { _iTFID = value; }
        }

        private string _sTFN;
        [Xrm("cgi_stfn")]
        public string sTFN
        {
            get { return _sTFN; }
            set { _sTFN = value; }
        }

        private string _sTFD;
        [Xrm("cgi_stfd")]
        public string sTFD
        {
            get { return _sTFD; }
            set { _sTFD = value; }
        }

        private string _sTFT;
        [Xrm("cgi_stft")]
        public string sTFT
        {
            get { return _sTFT; }
            set { _sTFT = value; }
        }

        private int? _iTTID;
        [Xrm("cgi_ittid")]
        public int? iTTID
        {
            get { return _iTTID; }
            set { _iTTID = value; }
        }

        private string _sTTN;
        [Xrm("cgi_sttn")]
        public string sTTN
        {
            get { return _sTTN; }
            set { _sTTN = value; }
        }

        private string _sTTD;
        [Xrm("cgi_sttd")]
        public string sTTD
        {
            get { return _sTTD; }
            set { _sTTD = value; }
        }

        private string _sTTT;
        [Xrm("cgi_sttt")]
        public string sTTT
        {
            get { return _sTTT; }
            set { _sTTT = value; }
        }
    }
}
