using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Setting
    {
        #region Public Properties

        [XmlElement("defaultcustomeroncase")]
        public string DefaultCustomer { get; set; }

        #endregion
    }
}