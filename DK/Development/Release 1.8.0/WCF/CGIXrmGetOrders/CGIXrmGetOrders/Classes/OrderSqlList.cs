using System.Collections.Generic;
using System.Xml.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [XmlRoot("creditrows")]
    public class OrderSqlList
    {
        #region Public Properties
        [XmlElement("creditrow")]
        public List<CreditOrderRow> CreditOrderRows { get; set; }
        #endregion
    }
}