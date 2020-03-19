
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


[DataContract]
public class Line
{   
    
    [DataMember]
    [XmlElement("LineGid")]
    public string LineGid{get;set;}
    
   
    [DataMember]
    [XmlElement("LineName")]
    public string LineName{get;set;}
    
    
    [DataMember]
    [XmlElement("LineNumber")]
    public string LineNumber{get;set;}
    
    
    [DataMember]
    [XmlElement("LineDesignation")]
    public string LineDesignation{get;set;}
   
    
    [DataMember]
    [XmlElement("LineDisplayOrder")]
    public string LineDisplayOrder{get;set;}
    
    
    [DataMember]
    [XmlElement("LineTransportAuthorityCode")]
    public string LineTransportAuthorityCode{get;set;}
    
    
    [DataMember]
    [XmlElement("LineOperatorCode")]
    public string LineOperatorCode{get;set;}
    
    
    [DataMember]
    [XmlElement("LineExistsFromDate")]
    public string LineExistsFromDate{get;set;}
    
    
    [DataMember]
    [XmlElement("LineExistsUpToDate")]
    public string LineExistsUpToDate{get;set;}
   
    
    [DataMember]
    [XmlArray("StopAreas")]
    [XmlArrayItem("StopArea")]
    public List<StopArea> StopAreas{get;set;}
   
}