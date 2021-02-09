using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("GetCallsforServiceJourneyMethodDepartureDeviation")]
public class DepartureDeviation1
{
    #region Public Properties
    [DataMember]
    [XmlElement("Id")]
    public string Id { get; set; }

    [DataMember]
    [XmlElement("PublicNote")]
    public string PublicNote{get; set;}

    [DataMember]
    [XmlElement("IsOnDepartureId")]
    public string IsOnDepartureId { get; set; }

    [DataMember]
    [XmlElement("HasDeviationMessageVersionId")]
    public string HasDeviationMessageVersionId { get; set; }

    [DataMember]
    [XmlElement("ConsequenceLongCode")]
    public string ConsequenceLongCode { get; set; }

    [DataMember]
    [XmlElement("AffectsLaterArrivalsYesNo")]
    public bool AffectsLaterArrivalsYesNo { get; set; }
    #endregion
}