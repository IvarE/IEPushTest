using System;
using System.Runtime.InteropServices;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("cgi_categorydetail")]
    [ComVisible(false)]
    public class CategoryDetail:XrmBaseEntity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("cgi_categorydetailid")]
        public Guid CategoryDetailId { get; set; }

        [Xrm("cgi_categorydetailname")]
        public string CategoryDetailName { get; set; }

        [Xrm("cgi_parentid")]
        public EntityReference ParentLevel1 { get; set; }

        [Xrm("cgi_parentid2")]
        public EntityReference ParentLevel2 { get; set; }

        [Xrm("cgi_level")]
        public string Level { get; set; }

        [Xrm("cgi_callguidecategory")]
        public string CallGuideCategory { get; set; }

        #endregion
    }
}