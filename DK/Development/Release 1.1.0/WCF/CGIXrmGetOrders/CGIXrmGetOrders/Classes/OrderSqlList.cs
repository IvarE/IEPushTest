using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmGetOrders
{
    [XmlRoot("creditrows")]
    public class OrderSqlList
    {
        [XmlElement("creditrow")]
        public List<CreditOrderRow> CreditOrderRows { get; set; }
    }
}