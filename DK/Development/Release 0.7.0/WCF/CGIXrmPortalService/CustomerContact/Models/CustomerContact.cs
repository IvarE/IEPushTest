using System.Runtime.Serialization;
using System;

namespace CGICRMPortalService
{
    [DataContract]
    [Serializable]
    public class CustomerContact
    {
        private Guid _ContactId;
        [DataMember]
        public Guid ContactId
        {
            get { return _ContactId; }
            set { _ContactId = value; }
        }
        private string _CompanyName;
        [DataMember]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        private string _FirstName = string.Empty;
        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName = string.Empty;
        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _Street1;
        [DataMember]
        public string Street1
        {
            get { return _Street1; }
            set { _Street1 = value; }
        }
        private string _PostalCode;
        [DataMember]
        public string PostalCode
        {
            get { return _PostalCode; }
            set { _PostalCode = value; }
        }
        private string _City;
        [DataMember]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        private string _County;
        [DataMember]
        public string County
        {
            get { return _County; }
            set { _County = value; }
        }
        private string _Country;
        [DataMember]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }
        private string _CareOff;
        [DataMember]
        public string CareOff
        {
            get { return _CareOff; }
            set { _CareOff = value; }
        }

        private string _ContactPerson;
        [DataMember]
        public string ContactPerson
        {
            get { return _ContactPerson; }
            set { _ContactPerson = value; }
        }

        private string _ContactPhoneNumber;
        [DataMember]
        public string ContactPhoneNumber
        {
            get { return _ContactPhoneNumber; }
            set { _ContactPhoneNumber = value; }
        }

        private string _SMSNotificationNumber;
        [DataMember]
        public string SMSNotificationNumber
        {
            get { return _SMSNotificationNumber; }
            set { _SMSNotificationNumber = value; }
        }

        private string _Email;
        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private bool _IsPrimaryAddress;
        [DataMember]
        public bool IsPrimaryAddress
        {
            get { return _IsPrimaryAddress; }
            set { _IsPrimaryAddress = value; }
        }
        private AddressTypeCode _AddressType;
        [DataMember]
        public AddressTypeCode AddressType
        {
            get { return _AddressType; }
            set { _AddressType = value; }
        }
    }
}