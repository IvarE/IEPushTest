using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("settings")]
    public class SettingSqlList
    {
        #region Public Properties
        [XmlElement("setting")]
        public List<Setting> Settings { get; set; }
        #endregion
    }
}