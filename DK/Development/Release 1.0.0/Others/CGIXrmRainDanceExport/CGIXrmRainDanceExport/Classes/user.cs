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
    public class user
    {

        //<attribute name='cgi_rsid' />";
        private string _rsId;
        [Xrm("cgi_rsid")]
        public string RsId
        {
            get { return _rsId; }
            set { _rsId = value; }
        }

    }
}
