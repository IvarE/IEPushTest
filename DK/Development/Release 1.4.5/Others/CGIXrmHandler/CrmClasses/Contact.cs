using System;
using CGIXrmWin;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("contact")]
    public class Contact
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("contactid")]
        public Guid ContactId { get; set; }

        [Xrm("cgi_socialsecuritynumber")]
        public string SocialSecurityNumber { get; set; }

        [Xrm("emailaddress1")]
        public string Email { get; set; }

        public string LogicalName { get { return "contact"; } }
        #endregion
    }
}
