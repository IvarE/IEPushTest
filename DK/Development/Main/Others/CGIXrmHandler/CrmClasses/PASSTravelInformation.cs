using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("cgi_passtravelinformation")]
    public class PASSTravelInformation : XrmBaseEntity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("cgi_passtravelinformationid")]
        public Guid PASSTravelInformationId { get; set; }

        [Xrm("cgi_passtravelinformation")]
        public string PASSTravelInformationName { get; set; }

        [Xrm("cgi_incidentid")]
        public EntityReference Case { get; set; }

        [Xrm("cgi_stcn")]
        public string sTCN { get; set; }

        [Xrm("cgi_stcid")]
        public string sTCID { get; set; }

        [Xrm("cgi_itlid")]
        public int? iTLID { get; set; }

        [Xrm("cgi_stln")]
        public string sTLN { get; set; }

        [Xrm("cgi_strn")]
        public string sTRN { get; set; }

        [Xrm("cgi_itjid")]
        public int? iTJID { get; set; }

        [Xrm("cgi_itfid")]
        public int? iTFID { get; set; }

        [Xrm("cgi_stfn")]
        public string sTFN { get; set; }

        [Xrm("cgi_stfd")]
        public string sTFD { get; set; }

        [Xrm("cgi_stft")]
        public string sTFT { get; set; }

        [Xrm("cgi_ittid")]
        public int? iTTID { get; set; }

        [Xrm("cgi_sttn")]
        public string sTTN { get; set; }

        [Xrm("cgi_sttd")]
        public string sTTD { get; set; }

        [Xrm("cgi_sttt")]
        public string sTTT { get; set; }

        #endregion
    }
}
