using System;
using System.Runtime.InteropServices;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("cgi_casecategory")]
    [ComVisible(false)]
    public class CaseCatergory:XrmBaseEntity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("cgi_casecategoryid")]
        public Guid CaseCategoryid { get; set; }

        [Xrm("cgi_casecategoryname")]
        public string CaseCategoryName { get; set; }

        [Xrm("cgi_caseid")]
        public EntityReference CaseId { get; set; }

        [Xrm("cgi_caseid",DecodePart=XrmDecode.Name)]
        public string CaseIdName { get; set; }

        [Xrm("cgi_caseid", DecodePart = XrmDecode.Value)]
        public string CaseIdValue { get; set; }

        [Xrm("cgi_category1id")]
        public EntityReference CaseCategory1 { get; set; }

        [Xrm("cgi_category1id", DecodePart = XrmDecode.Name)]
        public string CaseCategory1Name { get; set; }

        [Xrm("cgi_category1id", DecodePart = XrmDecode.Value)]
        public string CaseCategory1Value { get; set; }

        [Xrm("cgi_category2id")]
        public EntityReference CaseCategory2 { get; set; }

        [Xrm("cgi_category2id", DecodePart = XrmDecode.Name)]
        public string CaseCategory2Name { get; set; }

        [Xrm("cgi_category2id", DecodePart = XrmDecode.Value)]
        public string CaseCategory2Value { get; set; }

        [Xrm("cgi_category3id")]
        public EntityReference CaseCategory3 { get; set; }

        [Xrm("cgi_category3id", DecodePart = XrmDecode.Name)]
        public string CaseCategory3Name { get; set; }

        [Xrm("cgi_category3id", DecodePart = XrmDecode.Value)]
        public string CaseCategory3Value { get; set; }

        #endregion
    }
}