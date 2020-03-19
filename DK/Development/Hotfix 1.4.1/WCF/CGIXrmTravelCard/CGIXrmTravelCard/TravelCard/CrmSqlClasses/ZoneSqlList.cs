using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmTravelCard
{
    [XmlRoot("zonenames")]
    public class ZoneSqlList
    {
        #region Public Properties
        [XmlElement("zonename")]
        public List<Zone> Zones { get; set; }
        #endregion
    }
}