using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("phonecall")]
    public class PhoneCallActivity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("activityid")]
        public Guid PhoneCallId { get; set; }

        [Xrm("subject")]
        public string Subject { get; set; }

        [Xrm("from")]
        public EntityCollection CallFrom { get; set; }

        [Xrm("to")]
        public EntityCollection CallTo { get; set; }

        [Xrm("phonenumber")]
        public string PhoneNumber { get; set; }

        [Xrm("directioncode")]
        public bool Direction { get; set; }

        [Xrm("regardingobjectid")]
        public EntityReference Regarding { get; set; }

        [Xrm("actualdurationminutes")]
        public string Duration { get; set; }

        [Xrm("description")]
        public string Desciption { get; set; }

        [Xrm("cgi_callguideinfoid")]
        public EntityReference CallGuideInfo { get; set; }

        #endregion
    }
}