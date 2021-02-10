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
        [XmlElement("OrganisationalUnit")]
        public OrganisationalUnit[] OrganisationalUnitList { get; set; }
    }
}