using System.Collections.Generic;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("cards")]
    public class CardSqlList
    {
        #region Public Properties
        [XmlElement("Card")]
        public List<Card> Cards { get; set; }
        #endregion
    }
}