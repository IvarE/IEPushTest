using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Setting
    {
        #region Public Properties
        private string _defaultCustomer;
        [XmlElement("defaultcustomeroncase")]
        public string DefaultCustomer
        {
            get { return _defaultCustomer; }
            set { _defaultCustomer = value; }
        }
        #endregion
    }
}