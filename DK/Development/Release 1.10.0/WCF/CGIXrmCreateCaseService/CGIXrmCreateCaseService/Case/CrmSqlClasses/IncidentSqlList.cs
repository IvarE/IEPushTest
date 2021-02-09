using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("incidents")]
    public class IncidentSqlList
    {
        #region Public Properties
        [XmlElement("incident")]
        public List<Incident> Incidents { get; set; }
        #endregion
    }
}