using CGIXrmWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmHandler.PASS.CrmClasses
{
    [XrmEntity("account")]
    public class Account : XrmBaseEntity
    {
        private Guid _AccountId;
        [XrmPrimaryKey]
        [Xrm("accountid")]
        public Guid AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }

        private string _SocialSecurityNumber;
        [Xrm("cgi_socialsecuritynumber")]
        public string SocialSecurityNumber
        {
            get { return _SocialSecurityNumber; }
            set { _SocialSecurityNumber = value; }
        }

        private string _Name;
        [Xrm("name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Email;
        [Xrm("emailaddress1")]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string LogicalName { get { return "account"; } }

    }
}
