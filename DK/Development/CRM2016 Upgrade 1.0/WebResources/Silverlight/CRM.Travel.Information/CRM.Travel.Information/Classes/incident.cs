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

namespace CRM.Travel.Information.Classes
{
    public class incident : XrmBaseNotify
    {
        [Xrm("statecode", DecodePart=XrmDecode.Value)]
        public int Statecode { get; set; }

        [Xrm("statuscode", DecodePart = XrmDecode.Value)]
        public int Statuscode { get; set; }

        [Xrm("ownerid", DecodePart = XrmDecode.Value)]
        public Guid Ownerid { get; set; }

    }
}
