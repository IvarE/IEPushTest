using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("accounts")]
    public class AccountSqlList
    {
        #region Public Properties
        [XmlElement("account")]
        public List<Account> Accounts { get; set; }
        #endregion
    }
}