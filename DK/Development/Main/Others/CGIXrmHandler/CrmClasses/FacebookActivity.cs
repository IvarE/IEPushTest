using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("cgi_callguidefacebook")]
    public class FacebookActivity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("activityid")]
        public Guid ChatId { get; set; }

        [Xrm("subject")]
        public string Subject { get; set; }

        [Xrm("from")]
        public EntityCollection CallFrom { get; set; }

        [Xrm("to")]
        public EntityCollection CallTo { get; set; }


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