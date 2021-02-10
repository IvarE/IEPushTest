using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class Zone
    {
        #region Public Properties
        //<cgi_zoneid>101</cgi_zoneid>
        [DataMember]
        [XmlElement("cgi_zoneid")]
        public string ZoneId { get; set; }

        //<cgi_name>Mölle</cgi_name>
        [DataMember]
        [XmlElement("cgi_name")]
        public string ZoneName { get; set; }

        #endregion
    }
}