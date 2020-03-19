using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class Card
    {
        #region Public Properties
        //travelcard.cgi_travelcardid
        [DataMember]
        [XmlElement("cgi_travelcardid")]
        public Guid TravelCardID { get; set; }

        //travelcard.cgi_travelcardname
        [DataMember]
        [XmlElement("cgi_travelcardname")]
        public string TravelCardName { get; set; }

        //travelcard.cgi_accountid,
        [DataMember]
        [XmlElement("cgi_accountid")]
        public Guid? AccountId { get; set; }

        //travelcard.cgi_accountnumber,
        [DataMember]
		[XmlElement("accountnumber")]
		public string AccountNumber { get; set; }

        //travelcard.cgi_accountidname,
        [DataMember]
        [XmlElement("cgi_accountidname")]
        public string AccountIdName { get; set; }

        //travelcard.cgi_contactid,
        [DataMember]
        [XmlElement("cgi_contactid")]
        public Guid? ContactId { get; set; }

        //travelcard.cgi_contactidname,
        [DataMember]
        [XmlElement("cgi_contactidname")]
        public string ContactIdName { get; set; }

        //travelcard.cgi_blocked,
        [DataMember]
        [XmlElement("cgi_blocked")]
        public int? Blocked { get; set; }

        //travelcard.cgi_cardtypeid,
        [DataMember]
        [XmlElement("cgi_cardtypeid")]
        public Guid? CardTypeId { get; set; }

        //travelcard.cgi_cardtypeidname,
        [DataMember]
        [XmlElement("cgi_cardtypeidname")]
        public string CardTypeIdName { get; set; }

        //travelcard.cgi_numberofzones,
        [DataMember]
        [XmlElement("cgi_numberofzones")]
        public string NumberOfZones { get; set; }

        //travelcard.cgi_periodic_card_type,
        [DataMember]
        [XmlElement("cgi_periodic_card_type")]
        public string PeriodeicCardType { get; set; }

        //travelcard.cgi_validfrom,
        [DataMember]
        [XmlElement("cgi_validfrom")]
        public string ValidFrom { get; set; }

        //travelcard.cgi_validto,
        [DataMember]
        [XmlElement("cgi_validto")]
        public string ValidTo { get; set; }

        //travelcard.cgi_value_card_type,
        [DataMember]
        [XmlElement("cgi_value_card_type")]
        public string Value_card_type { get; set; }

        //travelcard.statecode
        [DataMember]
        [XmlElement("statecode")]
        public int StateCode { get; set; }

        //travelcard.cgi_autoloadstatus,
        [DataMember]
        [XmlElement("cgi_autoloadstatus")]
        public int Autoloadstatus { get; set; }


        //travelcard.cgi_autoloadstatusname,
        [DataMember]
        [XmlElement("cgi_autoloadstatusname")]
        public string Autoloadstatusname { get; set; }

        //travelcard.cgi_autoloadconnectiondate,
        [DataMember]
        [XmlElement("cgi_autoloadconnectiondate")]
        public string Autoloadconnectiondate { get; set; }


        //travelcard.cgi_autoloaddisconnectiondate
        [DataMember]
        [XmlElement("cgi_autoloaddisconnectiondate")]
        public string Autoloaddisconnectiondate { get; set; }

        //travelcard.cgi_creditcardmask
        [DataMember]
		[XmlElement("cgi_creditcardmask")]
		public string Creditcardmask { get; set; }

        //travelcard.cgi_failedattemptstochargemoney
        [DataMember]
		[XmlElement("cgi_failedattemptstochargemoney")]
		public string Failedattemptstochargemoney { get; set; }

        //travelcard.cgi_latestfailedattempt
        [DataMember]
		[XmlElement("cgi_latestfailedattempt")]
		public string Latestfailedattempt { get; set; }

        //<cgi_contactnumber>5</cgi_contactnumber>
        [DataMember]
        [XmlElement("cgi_contactnumber")]
        public string Contactnumber { get; set; }

        #endregion
    }
}