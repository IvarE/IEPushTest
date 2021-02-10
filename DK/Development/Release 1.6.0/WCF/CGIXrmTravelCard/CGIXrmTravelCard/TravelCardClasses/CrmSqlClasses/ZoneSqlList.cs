using System.Collections.Generic;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.CrmSqlClasses
{
    [XmlRoot("zonenames")]
    public class ZoneSqlList
    {
        #region Public Properties
        [XmlElement("zonename")]
        public List<Models.Zone> Zones { get; set; }
        #endregion
    }
}