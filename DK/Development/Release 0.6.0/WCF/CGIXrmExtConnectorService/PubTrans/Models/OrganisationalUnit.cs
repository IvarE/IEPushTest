using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class OrganisationalUnit
    {

        //o.Id,
        [DataMember]
        [XmlElement("Id")]
        public string ID { get; set; }
        
        //o.Code,
        [DataMember]
        [XmlElement("Code")]
        public string Code { get; set; }

        //o.Name
        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }

    }
}