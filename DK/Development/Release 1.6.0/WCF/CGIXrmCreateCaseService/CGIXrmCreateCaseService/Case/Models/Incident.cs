using System;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Incident
    {
        #region Declarations
        [XmlElement("incidentid")]
        public Guid IncidentId;
        #endregion

        #region Public Properties

        [XmlElement("ticketnumber")]
        public string TicketNumber { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("cgi_travelcardno")]
        public string TravelCardNo { get; set; }

        [XmlElement("cgi_accountid")]
        public string AccountId { get; set; }

        [XmlElement("cgi_accountidname")]
        public string AccountIdName { get; set; }

        [XmlElement("cgi_contactid")]
        public string ContactId { get; set; }

        [XmlElement("cgi_contactidname")]
        public string ContactIdName { get; set; }

        [XmlElement("cgi_telephonenumber")]
        public string MobileNumber { get; set; }
        
        [XmlElement("cgi_rgol_delivery_email")]
        public string DeliveryEmailAddress { get; set; }
        #endregion
    }
}