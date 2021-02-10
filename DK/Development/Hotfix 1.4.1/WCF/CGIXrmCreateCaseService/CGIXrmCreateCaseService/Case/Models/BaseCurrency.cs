using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class BaseCurrency
    {
        #region Public Properties
        //org.basecurrencyid
        private string _baseCurrencyId;
        [XmlElement("basecurrencyid")]
        public string BaseCurrencyId
        {
            get { return _baseCurrencyId; }
            set { _baseCurrencyId = value; }
        }

	    //org.basecurrencyidname
        private string _baseCurrencyIdName;
        [XmlElement("basecurrencyidname")]
        public string BaseCurrencyIdName
        {
            get { return _baseCurrencyIdName; }
            set { _baseCurrencyIdName = value; }
        }
    #endregion

    }
}