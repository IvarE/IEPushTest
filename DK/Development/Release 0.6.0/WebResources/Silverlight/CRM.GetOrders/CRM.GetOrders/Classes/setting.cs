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

namespace CRM.GetOrders
{
    public class setting
    {
        [Xrm("cgi_ehandelorderservice")]
        public string ServiceAddress { get; set; }
    }
}
