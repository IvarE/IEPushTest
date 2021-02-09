using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmTravelCard
{
    [XmlRoot("transactions")]
    public class CardTransactionSqlList
    {
        [XmlElement("transaction")]
        public List<TravelCardTransaction> TravelCardTransactions { get; set; }
    }
}