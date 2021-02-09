using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService
{
    [Serializable]
    [DataContract]
    [XmlRoot("OrganisationalUnits")]
    public class OrganisationalUnits
    {
        #region Public Properties
        [XmlElement("OrganisationalUnit")]
        public OrganisationalUnit[] OrganisationalUnitList { get; set; }
        #endregion
    }
}