using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[Serializable]
[DataContract]
[XmlRoot("Zones")]
public class Zones
{
    #region Public Properties
    [XmlElement("Zone")]
    public Zone[] Zone { get; set; }
    #endregion
}