using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class Contractor
    {
        //c.Id,
        [DataMember]
        [XmlElement("Id")]
        public string ID { get; set; }
        
		//c.Gid,
        [DataMember]
        [XmlElement("Gid")]
        public string Gid { get; set; }

        //c.IsOrganisationId
        [DataMember]
        [XmlElement("IsOrganisationId")]
        public string IsOrganisationId { get; set; }

    }
}