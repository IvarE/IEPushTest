using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[XmlRoot("StopAreas")]
public class StopAreas
{
    #region Public Properties
    [DataMember]
    [XmlElement("StopArea")]
    public StopArea[] StopArea { get; set; }
    #endregion
}