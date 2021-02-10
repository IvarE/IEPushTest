
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


[Serializable]
[DataContract]
[XmlRoot("Lines")]
public class Lines
{

    [XmlElement("Line")]
    public Line[] Line { get; set; }

    //[XmlArray("Lines"),XmlArrayItem("Line")]
    //public List<Line> LineInfos { get; set; }
}