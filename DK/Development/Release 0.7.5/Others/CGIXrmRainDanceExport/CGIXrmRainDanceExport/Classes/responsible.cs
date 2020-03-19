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
    public class responsible
    {
        //<attribute name='cgi_responsible' />
        private string _responsible;
        [Xrm("cgi_responsible")]
        public string Responsible
        {
            get { return _responsible; }
            set { _responsible = value; }
        }
    }
}
