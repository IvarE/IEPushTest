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

namespace CRM.GetBIFFTransactions
{
    public class localizedlabel
    {

        //cgi_localizedlabelname
        private string _localizedlabelname;
        [Xrm("cgi_localizedlabelname")]
        public string Localizedlabelname
        {
            get { return _localizedlabelname; }
            set { _localizedlabelname = value; }
        }
        
        //cgi_localizedcontrolid
        private string _localizedcontrolid;
        [Xrm("cgi_localizedcontrolid")]
        public string Localizedcontrolid
        {
            get { return _localizedcontrolid; }
            set { _localizedcontrolid = value; }
        }

    }
}
