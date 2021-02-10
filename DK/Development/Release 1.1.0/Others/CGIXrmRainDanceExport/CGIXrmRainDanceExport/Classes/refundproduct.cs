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
    public class refundproduct
    {
        //<attribute name='cgi_refundproductname' />
        private string _refundproductname;
        [Xrm("cgi_refundproductname")]
        public string Refundproductname
        {
            get { return _refundproductname; }
            set { _refundproductname = value; }
        }

        //<attribute name='cgi_account' />
        private string _account;
        [Xrm("cgi_account")]
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }


    }
}
