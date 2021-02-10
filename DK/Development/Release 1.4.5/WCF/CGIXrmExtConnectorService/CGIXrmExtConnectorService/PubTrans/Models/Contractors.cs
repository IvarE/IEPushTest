using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Contractors")]
    public class Contractors
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [XmlElement("Contractor")]
        public Contractor[] ContractorList { get; set; }
        
        #endregion
    }
}