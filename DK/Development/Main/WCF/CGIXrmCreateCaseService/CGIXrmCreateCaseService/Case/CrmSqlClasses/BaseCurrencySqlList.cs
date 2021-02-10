using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("currencies")]
    public class BaseCurrencySqlList
    {
        #region Public Properties
        [XmlElement("currency")]
        public List<BaseCurrency> BaseCurrencies { get; set; }
        #endregion
    }
}