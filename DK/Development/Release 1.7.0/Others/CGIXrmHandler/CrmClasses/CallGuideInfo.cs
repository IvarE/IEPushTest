using System;
using CGIXrmWin;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("cgi_callguideinfo")]
    public class CallGuideInfo
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("cgi_callguideinfoid")]
        public Guid CallGuideInfoId { get; set; }

        [Xrm("cgi_callguideinfoname")]
        public string CallGuideInfoName { get; set; }

        [Xrm("cgi_callguidesessionid")]
        public string CallGuideSessionID { get; set; }

        [Xrm("cgi_queuetime")]
        public string QueueTime { get; set; }

        [Xrm("cgi_aphonenumber")]
        public string APhoneNumber { get; set; }


        [Xrm("cgi_bphonenumber")]
        public string BPhoneNumber { get; set; }


        [Xrm("cgi_errandtasktype")]
        public string ErrandTaskType { get; set; }

        [Xrm("cgi_screenpopchoice")]
        public string ScreenPopChoice { get; set; }

        [Xrm("cgi_contactsourcetype")]
        public string ContactSourceType { get; set; }

        [Xrm("cgi_cid")]
        public string CId { get; set; }

        [Xrm("cgi_agentname")]
        public string AgentName { get; set; }

        [Xrm("cgi_chatcustomeralias")]
        public string ChatCustomerAlias { get; set; }

        #endregion
    }
}