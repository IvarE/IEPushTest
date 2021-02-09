using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class OrganisationalUnit
    {
        #region Public Properties
        [DataMember]
        [XmlElement("Id")]
        public string ID { get; set; }

        [DataMember]
        [XmlElement("Code")]
        public string Code { get; set; }

        [DataMember]
        [XmlElement("Name")]
        public string Name { get; set; }
        #endregion
    }
}