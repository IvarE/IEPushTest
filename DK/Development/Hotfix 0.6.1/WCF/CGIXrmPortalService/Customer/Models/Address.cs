using System.Runtime.Serialization;
using System;

namespace CGICRMPortalService.Models
{

    [DataContract]
    public class Address
    {
        private Guid _CustomerAddressId;
        [DataMember]
        public Guid CustomerAddressId
        {
            get { return _CustomerAddressId; }
            set { _CustomerAddressId = value; }
        }
        private string _AddressId;
        [DataMember]
        public string AddressId
        {
            get { return _AddressId; }
            set { _AddressId = value; }
        }
        private string _CompanyName;
        [DataMember]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _Street;
        [DataMember]
        public string Street
        {
            get { return _Street; }
            set { _Street = value; }
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

        private string _emailNotificationAddress;
        [DataMember]
        public string EmailNotificationAddress
        {
            get { return _emailNotificationAddress; }
            set { _emailNotificationAddress = value; }
        }
        
        private AddressTypeCode _AddressType;
        [DataMember]
        public AddressTypeCode AddressType
        {
            get { return _AddressType; }
            set { _AddressType = value; }
        }
        //private Boolean _Deleted;
        //[DataMember]
        //public Boolean Deleted
        //{
        //    get { return _Deleted; }
        //    set { _Deleted = value; }
        //}
    }
}