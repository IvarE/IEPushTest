

using System.Runtime.Serialization;
using System.Xml.Serialization;
[DataContract]
[XmlRoot("GetDirectJourneysBetweenStopsResponseArrivalDeviation")]
public class ArrivalDeviation
{
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
}