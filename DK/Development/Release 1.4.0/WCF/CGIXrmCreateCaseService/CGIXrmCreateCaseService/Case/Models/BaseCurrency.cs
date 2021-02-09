using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class BaseCurrency
    {
        #region Public Properties
        //org.basecurrencyid
        [XmlElement("basecurrencyid")]
        public string BaseCurrencyId { get; set; }

        //org.basecurrencyidname
        [XmlElement("basecurrencyidname")]
        public string BaseCurrencyIdName { get; set; }

        #endregion
    }
}