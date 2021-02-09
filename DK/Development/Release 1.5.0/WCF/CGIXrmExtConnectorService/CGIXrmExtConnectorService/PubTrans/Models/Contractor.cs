using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class Contractor
    {
        #region Public Properties -------------------------------------------------------------------------------------

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
        
        #endregion
    }
}