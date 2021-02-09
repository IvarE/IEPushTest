using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    [XmlRoot("GetDirectJourneysBetweenStopsResponseArrivalDeviation")]
    public class ArrivalDeviation
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        [XmlElement("Id")]
        public string Id { get; set; }


        [DataMember]
        [XmlElement("IsOnArrivalId")]
        public string IsOnArrivalId { get; set; }


        [DataMember]
        [XmlElement("HasDeviationMessageVersionId")]
        public string HasDeviationMessageVersionId { get; set; }


        [DataMember]
        [XmlElement("ConsequenceLongCode")]
        public string ConsequenceLongCode { get; set; }


        [DataMember]
        [XmlElement("AffectsPreviousDepartures")]
        public bool AffectsPreviousDepartures { get; set; }

        #endregion
    }
}