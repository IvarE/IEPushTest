using CGIXrm;
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

namespace CRM.GetOrders
{
    public class Language : XrmBaseEntity
    {
        [XrmPrimaryKey]
        [Xrm("cgi_localizedlabelid")]
        public Guid LocalizedLabelId { get; set; }

        [Xrm("cgi_localizedcontrolid")]
        public string Tag { get; set; }

        [Xrm("cgi_localizedlabelname")]
        public string Name { get; set; }
    }
}
