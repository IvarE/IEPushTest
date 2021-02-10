using System.Runtime.Serialization;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("GetDirectJourneysBetweenStopsResponseDeviationMessageVersion")]
public class DeviationMessageVersion
{
    #region Public Properties
    [DataMember]
    [XmlElement("Id")]
    public string Id { get; set; }
    
    [DataMember]
    [XmlElement("PublicNote")]
    public string PublicNote { get; set; }
    
    [DataMember]
    [XmlElement("InternalNote")]
    public string InternalNote { get; set; }

    [DataMember]
    [XmlElement("PriorityImportanceLevel")]
    public string PriorityImportanceLevel { get; set; }
    
    [DataMember]
    [XmlElement("PriorityInfluenceLevel")]
    public int PriorityInfluenceLevel { get; set; }
    
    [DataMember]
    [XmlElement("PriorityUrgencyLevel")]
    public string PriorityUrgencyLevel { get; set; }
    
    [DataMember]
    [XmlElement("TargetAudienceCustomer")]
    public string TargetAudienceCustomer { get; set; }
    
    [DataMember]
    [XmlElement("TargetAudiencePassenger")]
    public string TargetAudiencePassenger { get; set; }
    
    [DataMember]
    [XmlElement("TargetAudienceStaff")]
    public string TargetAudienceStaff { get; set; }
    #endregion
}