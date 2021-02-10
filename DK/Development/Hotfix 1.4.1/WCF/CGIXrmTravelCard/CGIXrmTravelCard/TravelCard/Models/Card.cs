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
    public class Card
    {
        #region Public Properties
        //travelcard.cgi_travelcardid
	    private Guid _travelCardID;
        [DataMember]
        [XmlElement("cgi_travelcardid")]
        public Guid TravelCardID
        {
            get { return _travelCardID; }
            set { _travelCardID = value; }
        }

        //travelcard.cgi_travelcardname
	    private string _travelCardName;
        [DataMember]
        [XmlElement("cgi_travelcardname")]
        public string TravelCardName
        {
            get { return _travelCardName; }
            set { _travelCardName = value; }
        }

        //travelcard.cgi_accountid,
        private Guid? _accountId;
        [DataMember]
        [XmlElement("cgi_accountid")]
        public Guid? AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

		//travelcard.cgi_accountnumber,
		private string _accountNumber;
		[DataMember]
		[XmlElement("accountnumber")]
		public string AccountNumber {
		  get { return _accountNumber; }
		  set { _accountNumber = value; }
		}
        
        //travelcard.cgi_accountidname,
        private string _accountIdName;
        [DataMember]
        [XmlElement("cgi_accountidname")]
        public string AccountIdName
        {
            get { return _accountIdName; }
            set { _accountIdName = value; }
        }
        
        //travelcard.cgi_contactid,
        private Guid? _contactId;
        [DataMember]
        [XmlElement("cgi_contactid")]
        public Guid? ContactId
        {
            get { return _contactId; }
            set { _contactId = value; }
        } 

        //travelcard.cgi_contactidname,
        private string _contactIdName;
        [DataMember]
        [XmlElement("cgi_contactidname")]
        public string ContactIdName
        {
            get { return _contactIdName; }
            set { _contactIdName = value; }
        }

        //travelcard.cgi_blocked,
        private int? _blocked;
        [DataMember]
        [XmlElement("cgi_blocked")]
        public int? Blocked
        {
            get { return _blocked; }
            set { _blocked = value; }
        }
	    
        //travelcard.cgi_cardtypeid,
        private Guid? _cardTypeId;
        [DataMember]
        [XmlElement("cgi_cardtypeid")]
        public Guid? CardTypeId
        {
            get { return _cardTypeId; }
            set { _cardTypeId = value; }
        }

        //travelcard.cgi_cardtypeidname,
        private string _cardTypeIdName;
        [DataMember]
        [XmlElement("cgi_cardtypeidname")]
        public string CardTypeIdName
        {
            get { return _cardTypeIdName; }
            set { _cardTypeIdName = value; }
        }

        //travelcard.cgi_numberofzones,
        private string _numberOfZones;
        [DataMember]
        [XmlElement("cgi_numberofzones")]
        public string NumberOfZones
        {
            get { return _numberOfZones; }
            set { _numberOfZones = value; }
        }

        //travelcard.cgi_periodic_card_type,
        private string _periodeicCardType;
        [DataMember]
        [XmlElement("cgi_periodic_card_type")]
        public string PeriodeicCardType
        {
            get { return _periodeicCardType; }
            set { _periodeicCardType = value; }
        }
        
        //travelcard.cgi_validfrom,
        private string _validFrom;
        [DataMember]
        [XmlElement("cgi_validfrom")]
        public string ValidFrom
        {
            get { return _validFrom; }
            set { _validFrom = value; }
        }

        //travelcard.cgi_validto,
        private string _validTo;
        [DataMember]
        [XmlElement("cgi_validto")]
        public string ValidTo
        {
            get { return _validTo; }
            set { _validTo = value; }
        }

        //travelcard.cgi_value_card_type,
        private string _value_card_type;
        [DataMember]
        [XmlElement("cgi_value_card_type")]
        public string Value_card_type
        {
            get { return _value_card_type; }
            set { _value_card_type = value; }
        }

        //travelcard.statecode
        private int _stateCode;
        [DataMember]
        [XmlElement("statecode")]
        public int StateCode
        {
            get { return _stateCode; }
            set { _stateCode = value; }
        }
        
        //travelcard.cgi_autoloadstatus,
        private int _autoloadstatus;
        [DataMember]
        [XmlElement("cgi_autoloadstatus")]
        public int Autoloadstatus
        {
            get { return _autoloadstatus; }
            set { _autoloadstatus = value; }
        }
        

        //travelcard.cgi_autoloadstatusname,
        private string _autoloadstatusname;
        [DataMember]
        [XmlElement("cgi_autoloadstatusname")]
        public string Autoloadstatusname
        {
            get { return _autoloadstatusname; }
            set { _autoloadstatusname = value; }
        }

        
        //travelcard.cgi_autoloadconnectiondate,
        private string _autoloadconnectiondate;
        [DataMember]
        [XmlElement("cgi_autoloadconnectiondate")]
        public string Autoloadconnectiondate
        {
            get { return _autoloadconnectiondate; }
            set { _autoloadconnectiondate = value; }
        }

        
        //travelcard.cgi_autoloaddisconnectiondate
        private string _autoloaddisconnectiondate;
        [DataMember]
        [XmlElement("cgi_autoloaddisconnectiondate")]
        public string Autoloaddisconnectiondate
        {
            get { return _autoloaddisconnectiondate; }
		    set { _autoloaddisconnectiondate = value; }
        }

		//travelcard.cgi_creditcardmask
		private string _creditcardmask;
		[DataMember]
		[XmlElement("cgi_creditcardmask")]
		public string Creditcardmask {
		  get { return _creditcardmask; }
		  set { _creditcardmask = value; }
		}

		//travelcard.cgi_failedattemptstochargemoney
		private string _failedattemptstochargemoney;
		[DataMember]
		[XmlElement("cgi_failedattemptstochargemoney")]
		public string Failedattemptstochargemoney {
		  get { return _failedattemptstochargemoney; }
		  set { _failedattemptstochargemoney = value; }
		}

		//travelcard.cgi_latestfailedattempt
		private string _latestfailedattempt;
		[DataMember]
		[XmlElement("cgi_latestfailedattempt")]
		public string Latestfailedattempt {
		  get { return _latestfailedattempt; }
		  set { _latestfailedattempt = value; }
		}

        //<cgi_contactnumber>5</cgi_contactnumber>
        private string _contactnumber;
        [DataMember]
        [XmlElement("cgi_contactnumber")]
        public string Contactnumber
        {
            get { return _contactnumber; }
            set { _contactnumber = value; }
        }
        #endregion
    }
}