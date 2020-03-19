using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService
{
    [Serializable]
    [DataContract]
    [XmlRoot("Contractors")]
    public class Contractors
    {
        [XmlElement("Contractor")]
        public Contractor[] ContractorList { get; set; }
    }
}