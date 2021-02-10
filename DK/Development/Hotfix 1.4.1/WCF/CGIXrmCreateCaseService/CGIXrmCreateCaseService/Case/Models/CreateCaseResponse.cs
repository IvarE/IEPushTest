using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class CreateCaseResponse
    {
        #region Public Properties
        private string _ticketNumber;
        [DataMember]
        [XmlElement("ticketnumber")]
        public string TicketNumber 
        {
            get { return _ticketNumber; }
            set { _ticketNumber = value; }
        }

        private string _title;
        [DataMember]
        [XmlElement("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _exeptionString;
        [DataMember]
        public string ExeptionString
        {
            get { return _exeptionString; }
            set { _exeptionString = value; }
        }
        #endregion
    }
}