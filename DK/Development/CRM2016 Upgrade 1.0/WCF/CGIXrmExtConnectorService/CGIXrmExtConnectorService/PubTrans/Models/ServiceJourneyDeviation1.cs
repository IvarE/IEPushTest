﻿using System.Runtime.Serialization;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("GetCallsforServiceJourneyMethodServiceJourneyDeviation")]
public class ServiceJourneyDeviation1
{
    [DataMember]
    [XmlElement("Id")]
    public string Id { get; set; }
    [DataMember]
    [XmlElement("IsOnDatedVehicleJourneyId")]
    public string IsOnDatedVehicleJourneyId { get; set; }
    [DataMember]
    [XmlElement("HasDeviationMessageVersionId")]
    public string HasDeviationMessageVersionId { get; set; }
    [DataMember]
    [XmlElement("ConsequenceLongCode")]
    public string ConsequenceLongCode { get; set; }
}