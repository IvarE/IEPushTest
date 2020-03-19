using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    [XmlRoot("ContractorDetail")]
    public class ContactorDetail
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        [XmlElement("id")]
        public string ContractorId { get; set; }


        [DataMember]
        [XmlElement("Gid")]
        public string ContractorGid { get; set; }


        [DataMember]
        [XmlElement("ContractorName")]
        public string ContractorName { get; set; }

        #endregion
    }
}
