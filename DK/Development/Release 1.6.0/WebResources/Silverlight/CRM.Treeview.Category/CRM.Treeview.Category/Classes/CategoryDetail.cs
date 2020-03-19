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

namespace CRM.Treeview.Category.Classes
{
    public class CategoryDetail : XrmBaseEntity
    {

        [Xrm("cgi_categorydetailid")]
        public Guid CategoryDetailId { get; set; }

        [Xrm("cgi_categorydetailname")]
        public string Name { get; set; }

        [Xrm("cgi_parentid", DecodePart = XrmDecode.Value)]
        public Nullable<Guid> Parent { get; set; }

        [Xrm("cgi_parentid", DecodePart = XrmDecode.Name)]
        public string ParentName { get; set; }

        [Xrm("cgi_sortorder")]
        public int? Sortorder { get; set; }

        [Xrm("cgi_color", DecodePart = XrmDecode.Value)]
        public int Color { get; set; }

    }
}
