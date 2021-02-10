using System.Collections.Generic;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.CrmSqlClasses
{
    
    [XmlRoot("transactions")]
    public class CardTransactionSqlList
    {
        #region Public Properties
        [XmlElement("transaction")]
        public List<Models.TravelCardTransaction> TravelCardTransactions { get; set; }
        #endregion
    }
   
}