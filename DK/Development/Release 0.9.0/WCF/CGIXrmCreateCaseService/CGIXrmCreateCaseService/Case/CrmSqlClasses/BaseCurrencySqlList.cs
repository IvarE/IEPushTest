using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("currencies")]
    public class BaseCurrencySqlList
    {
        [XmlElement("currency")]
        public List<basecurrency> BaseCurrencies { get; set; }
    }
}