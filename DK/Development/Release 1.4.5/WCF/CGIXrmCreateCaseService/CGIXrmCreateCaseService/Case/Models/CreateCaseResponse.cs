using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class CreateCaseResponse
    {
        #region Public Properties

        [DataMember]
        [XmlElement("ticketnumber")]
        public string TicketNumber { get; set; }

        [DataMember]
        [XmlElement("title")]
        public string Title { get; set; }

        [DataMember]
        public string ExeptionString { get; set; }

        #endregion
    }
}