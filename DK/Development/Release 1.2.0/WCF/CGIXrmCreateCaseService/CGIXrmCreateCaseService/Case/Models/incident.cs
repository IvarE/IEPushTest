using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    public class incident
    {

        private string _ticketNumber;
        [XmlElement("ticketnumber")]
        public string TicketNumber
        {
            get { return _ticketNumber; }
            set { _ticketNumber = value; }
        }

        private string _title;
        [XmlElement("title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _travelCardNo;
        [XmlElement("cgi_travelcardno")]
        public string TravelCardNo
        {
            get { return _travelCardNo; }
            set { _travelCardNo = value; }
        }

        private string _accountId;
        [XmlElement("cgi_accountid")]
        public string AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        private string _accountIdName;
        [XmlElement("cgi_accountidname")]
        public string AccountIdName
        {
            get { return _accountIdName; }
            set { _accountIdName = value; }
        }

        private string _contactId;
        [XmlElement("cgi_contactid")]
        public string ContactId
        {
            get { return _contactId; }
            set { _contactId = value; }
        }

        private string _contactIdName;
        [XmlElement("cgi_contactidname")]
        public string ContactIdName
        {
            get { return _contactIdName; }
            set { _contactIdName = value; }
        }

        private string _mobileNumber;
        [XmlElement("cgi_telephonenumber")]
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; }
        }


        [XmlElement("incidentid")]
        public Guid IncidentId;

    }
}