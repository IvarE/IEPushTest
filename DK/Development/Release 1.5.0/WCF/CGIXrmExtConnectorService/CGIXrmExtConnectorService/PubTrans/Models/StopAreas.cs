using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [XmlRoot("StopAreas")]
    public class StopAreas
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        [XmlElement("StopArea")]
        public StopArea[] StopArea { get; set; }
        
        #endregion
    }
}