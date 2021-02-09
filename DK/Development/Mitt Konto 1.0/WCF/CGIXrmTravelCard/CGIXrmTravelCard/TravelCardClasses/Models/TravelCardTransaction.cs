using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class TravelCardTransaction
    {
        #region Public Properties

        [DataMember]
        [XmlElement("cgi_travelcardtransactionid")]
        public Guid? Transactionid { get; set; }

        [DataMember]
        [XmlElement("cgi_date")]
        public string Date { get; set; }

        [DataMember]
        [XmlElement("cgi_time")]
        public string Time { get; set; }

        [DataMember]
        [XmlElement("cgi_deviceid")]
        public string DeviceID { get; set; }

        [DataMember]
        [XmlElement("cgi_cardsect")]
        public string CardSect { get; set; }

        [DataMember]
        [XmlElement("cgi_rectype")]
        public string RecType { get; set; }

        [DataMember]
        [XmlElement("cgi_txntype")]
        public string TxnType { get; set; }

        [DataMember]
        [XmlElement("cgi_route")]
        public string Route { get; set; }

        [DataMember]
        [XmlElement("cgi_origzone")]
        public string OrigZone { get; set; }

        [DataMember]
        [XmlElement("cgi_origzonename")]
        public string OrigZoneName { get; set; }

        [DataMember]
        [XmlElement("cgi_amount")]
        public string Amount { get; set; }

        [DataMember]
        [XmlElement("cgi_currency")]
        public string Currency { get; set; }

        [DataMember]
        [XmlElement("cgi_caseid")]
        public Guid? CaseId { get; set; }

        [DataMember]
        [XmlElement("cgi_travelcardid")]
        public Guid? CardId { get; set; }

        [DataMember]
        [XmlElement("cgi_travelcard")]
        public string TravelCard { get; set; }

        [DataMember]
        [XmlElement("cgi_travelcardtransaction")]
        public string Name { get; set; }

        #endregion
    }
}