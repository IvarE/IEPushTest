using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmTravelCard
{
    [DataContract]
    public class TravelCardTransaction
    {
        #region Public Properties

        private Guid? _transactionid;
        [DataMember]
        [XmlElement("cgi_travelcardtransactionid")]
        public Guid? Transactionid
        {
            get { return _transactionid; }
            set { _transactionid = value; }
        }

        private string _date;
        [DataMember]
        [XmlElement("cgi_date")]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _time;
        [DataMember]
        [XmlElement("cgi_time")]
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        private string _deviceID;
        [DataMember]
        [XmlElement("cgi_deviceid")]
        public string DeviceID
        {
            get { return _deviceID; }
            set { _deviceID = value; }
        }

        private string _cardSect;
        [DataMember]
        [XmlElement("cgi_cardsect")]
        public string CardSect
        {
            get { return _cardSect; }
            set { _cardSect = value; }
        }

        private string _recType;
        [DataMember]
        [XmlElement("cgi_rectype")]
        public string RecType
        {
            get { return _recType; }
            set { _recType = value; }
        }

        private string _txnType;
        [DataMember]
        [XmlElement("cgi_txntype")]
        public string TxnType
        {
            get { return _txnType; }
            set { _txnType = value; }
        }

        private string _route;
        [DataMember]
        [XmlElement("cgi_route")]
        public string Route
        {
            get { return _route; }
            set { _route = value; }
        }

        private string _origZone;
        [DataMember]
        [XmlElement("cgi_origzone")]
        public string OrigZone
        {
            get { return _origZone; }
            set { _origZone = value; }
        }

        private string _origZoneName;
        [DataMember]
        [XmlElement("cgi_origzonename")]
        public string OrigZoneName
        {
            get { return _origZoneName; }
            set { _origZoneName = value; }
        }

        private string _amount;
        [DataMember]
        [XmlElement("cgi_amount")]
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private string _currency;
        [DataMember]
        [XmlElement("cgi_currency")]
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private Guid? _caseId;
        [DataMember]
        [XmlElement("cgi_caseid")]
        public Guid? CaseId
        {
            get { return _caseId; }
            set { _caseId = value; }
        }

        private Guid? _cardId;
        [DataMember]
        [XmlElement("cgi_travelcardid")]
        public Guid? CardId
        {
            get { return _cardId; }
            set { _cardId = value; }
        }

        private string _cardnumber;
        [DataMember]
        [XmlElement("cgi_travelcard")]
        public string TravelCard
        {
            get { return _cardnumber; }
            set { _cardnumber = value; }
        }

        private string _name;
        [DataMember]
        [XmlElement("cgi_travelcardtransaction")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

 #endregion
    }
}