using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    public class account
    {

        private Guid _accountId;
        [XmlElement("accountid")]
        public Guid AccountId 
        {
            get { return _accountId; }
            set { _accountId = value; } 
        }

        private string _accountNumber;
        [XmlElement("accountnumber")]
        public string AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }
        
        private string _accountName;
        [XmlElement("name")]
        public string AccountName 
        {
            get { return _accountName; }
            set { _accountName = value; } 
        }

        private string _emailAddress;
        [XmlElement("emailaddress")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //property used for how many email hits.
        private int? _emailCount;
        public int? EmailCount
        {
            get { return _emailCount; }
            set { _emailCount = value; }
        }

    }
}