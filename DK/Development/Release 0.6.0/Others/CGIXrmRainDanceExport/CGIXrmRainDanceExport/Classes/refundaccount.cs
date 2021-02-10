using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CGIXrmWin;
using Microsoft.Crm;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Configuration;
using System.ServiceModel;

namespace CGIXrmRainDanceExport
{
    public class refundaccount
    {
        //"<attribute name='cgi_refundaccountname'
        private string _refundaccountname;
        [Xrm("cgi_refundaccountname")]
        public string Refundaccountname
        {
            get { return _refundaccountname; }
            set { _refundaccountname = value; }
        }

        //"<attribute name='cgi_account'
        private string _account;
        [Xrm("cgi_account")]
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        //"<attribute name='cgi_refundaccountid'
        private Guid _refundaccountid;
        [Xrm("cgi_refundaccountid")]
        public Guid Refundaccountid
        {
            get { return _refundaccountid; }
            set { _refundaccountid = value; }
        }

    }
}
