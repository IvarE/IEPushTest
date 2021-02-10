using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("settings")]
    public class SettingSqlList
    {
        [XmlElement("setting")]
        public List<setting> Settings { get; set; }
    }
}