using System.Runtime.Serialization;
using System;

namespace CGICRMPortalService.Models
{

    [DataContract]
    public class Address
    {
        [DataMember]
        public Guid CustomerAddressId { get; set; }
        
        [DataMember]
        public string AddressId { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string County { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string CareOff { get; set; }

        [DataMember]
        public string ContactPerson { get; set; }

        [DataMember]
        public string ContactPhoneNumber { get; set; }

        [DataMember]
        public string SMSNotificationNumber { get; set; }

        [DataMember]
        public string EmailNotificationAddress { get; set; }

        [DataMember]
        public AddressTypeCode AddressType { get; set; }
    }
}