using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("rgolsettings")]
    public class RgolSettingSqlList
    {
        #region Public properties 
        [XmlElement("rgolsetting")]
        public List<RgolSetting> RgolSettings { get; set; }
        #endregion
    }
}