using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Zones")]
    public class Zones
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [XmlElement("Zone")]
        public Zone[] Zone { get; set; }
        
        #endregion
    }
}