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

        private string _address1_line1;
        [XmlElement("address1_line1")]
        public string Address1_line1
        {
            get { return _address1_line1; }
            set { _address1_line1 = value; }
        }

        private string _address1_line2;
        [XmlElement("address1_line2")]
        public string Address1_line2
        {
            get { return _address1_line2; }
            set { _address1_line2 = value; }
        }

        private string _address1_postalcode;
        [XmlElement("address1_postalcode")]
        public string Address1_postalcode
        {
            get { return _address1_postalcode; }
            set { _address1_postalcode = value; }
        }

        private string _address1_city;
        [XmlElement("address1_city")]
        public string Address1_city
        {
            get { return _address1_city; }
            set { _address1_city = value; }
        }

        private string _address1_country;
        [XmlElement("address1_country")]
        public string Address1_country
        {
            get { return _address1_country; }
            set { _address1_country = value; }
        }

        private string _firstname;
        [XmlElement("firstname")]
        public string FirstName        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        
        private string _lastname;
        [XmlElement("lastname")]
        public string LastName
        {
            get { return _lastname; }
            set { _lastname= value; }
        }
        
        private string _socialsecuritynumber;
        [XmlElement("socialsecuritynumber")]
        public string SocialSecurityNumber
        {
            get { return _socialsecuritynumber; }
            set { _socialsecuritynumber = value; }
        }

        private string _telephone2;
        [XmlElement("telephone2")]
        public string Telephone2
        {
            get { return _telephone2; }
            set { _telephone2 = value; }
        }

        private string _telephone3;
        [XmlElement("telephone3")]
        public string Telephone3
        {
            get { return _telephone3; }
            set { _telephone3 = value; }
        }
        //property used for how many records was found in the search.
        public int? RecordCount { get; set; }
    #endregion
    }
}