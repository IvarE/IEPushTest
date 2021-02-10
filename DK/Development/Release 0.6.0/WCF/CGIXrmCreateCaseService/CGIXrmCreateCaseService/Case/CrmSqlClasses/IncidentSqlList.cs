using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("incidents")]
    public class IncidentSqlList
    {
        [XmlElement("incident")]
        public List<incident> Incidents { get; set; }
    }
}