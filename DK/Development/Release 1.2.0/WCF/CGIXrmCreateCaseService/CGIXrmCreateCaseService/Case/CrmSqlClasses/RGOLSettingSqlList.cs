using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("rgolsettings")]
    public class RGOLSettingSqlList
    {
        [XmlElement("rgolsetting")]
        public List<RGOLSetting> RGOLSettings { get; set; }
    }
}