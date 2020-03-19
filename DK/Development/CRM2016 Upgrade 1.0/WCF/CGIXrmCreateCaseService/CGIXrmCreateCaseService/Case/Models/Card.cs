using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Card
    {
        #region Public Properties
        //travelcard.cgi_travelcardid
        [XmlElement("cgi_travelcardid")]
        public string TravelCardId { get; set; }

        #endregion
    }
}