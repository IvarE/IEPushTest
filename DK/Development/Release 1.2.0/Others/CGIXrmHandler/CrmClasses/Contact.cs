using CGIXrmWin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmHandler.PASS.CrmClasses
{
    [XrmEntity("contact")]
    public class Contact
    {
        private Guid _ContactId;
        [XrmPrimaryKey]
        [Xrm("contactid")]
        public Guid ContactId
        {
            get { return _ContactId; }
            set { _ContactId = value; }
        }

        private string _SocialSecurityNumber;
        [Xrm("cgi_socialsecuritynumber")]
        public string SocialSecurityNumber
        {
            get { return _SocialSecurityNumber; }
            set { _SocialSecurityNumber = value; }
        }

        private string _Email;
        [Xrm("emailaddress1")]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string LogicalName { get { return "contact"; } }
    }
}
