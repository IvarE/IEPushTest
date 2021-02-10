using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("OrganisationalUnits")]
    public class OrganisationalUnits
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [XmlElement("OrganisationalUnit")]
        public OrganisationalUnit[] OrganisationalUnitList { get; set; }
        
        #endregion
    }
}