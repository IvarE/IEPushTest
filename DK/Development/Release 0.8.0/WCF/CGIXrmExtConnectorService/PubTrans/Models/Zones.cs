
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


[Serializable]
[DataContract]
[XmlRoot("Zones")]
public class Zones
{

    [XmlElement("Zone")]
    public Zone[] Zone { get; set; }

    //[XmlArray("Lines"),XmlArrayItem("Line")]
    //public List<Line> LineInfos { get; set; }
}