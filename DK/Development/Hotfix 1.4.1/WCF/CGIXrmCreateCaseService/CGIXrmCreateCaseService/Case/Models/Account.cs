using System;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    public class Account
    {
        #region Public Properties
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

        //property used for how many records was found in the search.
        public int? RecordCount { get; set; }
    #endregion
    }
}