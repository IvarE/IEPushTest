using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Card
    {
        #region Public Properties
        //travelcard.cgi_travelcardid
        private string _travelCardId;
        [XmlElement("cgi_travelcardid")]
        public string TravelCardId
        {
            get { return _travelCardId; }
            set { _travelCardId = value; }
        }
        #endregion
    }
}