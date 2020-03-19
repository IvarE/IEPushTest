using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


[DataContract]
public class StopArea
{
    #region Public Properties
    [DataMember]
    [XmlElement("StopAreaGid")]
    public string StopAreaGid{get;set;}
    
    
    [DataMember]
    [XmlElement("StopAreaName")]
    public string StopAreaName{get;set;}
    
    
    [DataMember]
    [XmlElement("StopAreaShortName")]
    public string StopAreaShortName{get;set;}
    
    
    [DataMember]
    [XmlElement("StopExistsFromDate")]
    public string StopExistsFromDate{get;set;}
    
    
    [DataMember]
    [XmlElement("StopExistsUptoDate")]
    public string StopExistsUptoDate{get;set;}
    
    
    [DataMember]
    [XmlArray("UptoStopAreas")]
    [XmlArrayItem("StopArea")]
    public List<StopArea> UptoStopAreas{get;set; }
    #endregion
}