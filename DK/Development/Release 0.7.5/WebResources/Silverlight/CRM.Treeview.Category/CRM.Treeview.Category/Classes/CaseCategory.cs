using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using CGIXrm;
using CGIXrm.CrmSdk;
using CRM.Treeview.Category.ViewModel;

namespace CRM.Treeview.Category.Classes
{
    [XrmEntity("cgi_casecategory")]
    public class CaseCategory : XrmBaseEntity
    {

        [XrmPrimaryKey]
        [Xrm("cgi_casecategoryid")]
        public Guid CaseCategoryId { get; set; }

        [Xrm("cgi_casecategoryname")]
        public string Name { get; set; }

        [Xrm("cgi_category1id")]
        public EntityReference Category1Id { get; set; }

        [Xrm("cgi_category2id")]
        public EntityReference Category2Id { get; set; }

        [Xrm("cgi_category3id")]
        public EntityReference Category3Id { get; set; }

        [Xrm("cgi_caseid")]
        public EntityReference CaseId { get; set; }

    }
}
