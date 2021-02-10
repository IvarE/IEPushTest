using System.Collections.Generic;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.CrmSqlClasses
{
    [XmlRoot("cards")]
    public class CardSqlList
    {
        #region Public Properties
        [XmlElement("card")]
        public List<Models.Card> Cards { get; set; }
        #endregion
    }
}