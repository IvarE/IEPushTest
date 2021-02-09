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
    public class incident
    {
        //<attribute name='ticketnumber' />";
        private string _ticketnumber;
        [Xrm("ticketnumber")]
        public string Ticketnumber
        {
            get { return _ticketnumber; }
            set { _ticketnumber = value; }
        }

        //<attribute name='cgi_contactid' />";
        private Guid? _contactid;
        [Xrm("cgi_contactid", DecodePart = XrmDecode.Value)]
        public Guid? Contactid
        {
            get { return _contactid; }
            set { _contactid = value; }
        }

        //<attribute name='cgi_accountid' />";
        private Guid? _accountid;
        [Xrm("cgi_accountid", DecodePart = XrmDecode.Value)]
        public Guid? Accountid
        {
            get { return _accountid; }
            set { _accountid = value; }
        }

        [Xrm("description", DecodePart = XrmDecode.Value)]
        public string Description
        {
            get;
            set;
        }
        [Xrm("createdby", DecodePart = XrmDecode.Name)]
        public string CreatedByName
        {
            get;
            set;

        }
    }
}
