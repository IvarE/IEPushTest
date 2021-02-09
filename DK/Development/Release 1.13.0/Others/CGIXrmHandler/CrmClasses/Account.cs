using System;
using CGIXrmWin;

namespace CGIXrmHandler.CrmClasses
{
    [XrmEntity("account")]
    public class Account : XrmBaseEntity
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("accountid")]
        public Guid AccountId { get; set; }

        [Xrm("cgi_socialsecuritynumber")]
        public string SocialSecurityNumber { get; set; }

        [Xrm("name")]
        public string Name { get; set; }

        [Xrm("emailaddress1")]
        public string Email { get; set; }

        public string LogicalName { get { return "account"; } }
        #endregion
    }
}
