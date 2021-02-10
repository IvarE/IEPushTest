using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[DataContract]
public class Zone
{
    #region Public Properties
    [DataMember]
    [XmlElement("ZoneId")]
    public string ZoneId { get; set; }
   
    [DataMember]
    [XmlElement("ZoneGid")]
    public string ZoneGid { get; set; }
    
    [DataMember]
    [XmlElement("ZoneNumber")]
    public string ZoneNumber { get; set; }
    
    
    [DataMember]
    [XmlElement("ZoneName")]
    public string ZoneName { get; set; }
    
    [DataMember]
    [XmlElement("ZoneShortName")]
    public string ZoneShortName { get; set; }
    
    [DataMember]
    [XmlElement("ZoneType")]
    public string ZoneType { get; set; }
    
    [DataMember]
    [XmlElement("ZoneTransportAuthorityCode")]
    public string ZoneTransportAuthorityCode { get; set; }
   
    [DataMember]
    [XmlElement("ZoneExistsFromDate")]
    public string ZoneExistsFromDate { get; set; }
    
    [DataMember]
    [XmlArray("Lines")]
    [XmlArrayItem("Line")]
    public List<Line> Lines { get; set; }
    #endregion
}