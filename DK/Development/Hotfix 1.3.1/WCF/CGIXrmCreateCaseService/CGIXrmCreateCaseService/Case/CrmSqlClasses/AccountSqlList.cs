using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("accounts")]
    public class AccountSqlList
    {
        [XmlElement("account")]
        public List<account> Accounts { get; set; }
    }
}