using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace CGIXrmEAIConnectorService
{
    [DataContract]
    public class Customer
    {
        private string _CompanyName;
        [DataMember(IsRequired = true)]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        private Guid _PrimaryContact;
        public Guid PrimaryContact
        {
            get { return _PrimaryContact; }
            set { _PrimaryContact = value; }
        }

        private string _AccountFirstName;
        [DataMember(IsRequired = true)]
        public string AccountFirstName
        {
            get { return _AccountFirstName; }
            set { _AccountFirstName = value; }
        }
        private string _AccountLastName;
        [DataMember(IsRequired = true)]
        public string AccountLastName
        {
            get { return _AccountLastName; }
            set { _AccountLastName = value; }
        }

        private Guid _AccountId;
        [DataMember]
        public Guid AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }

        private string _AccountNumber;
        [DataMember]
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
        }
        
        private string _MobilePhone;
        [DataMember(IsRequired = true)]
        internal string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; }
        }

        private string _Phone;
        [DataMember]
        internal string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _Email;
        [DataMember(IsRequired = true)]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        //private bool _AllowBulkEmail;
        //[DataMember]
        //public bool AllowBulkEmail
        //{
        //    get { return _AllowBulkEmail; }
        //    set { _AllowBulkEmail = value; }
        //}
        //private bool _AllowEmail;
        //[DataMember]
        //public bool AllowEmail
        //{
        //    get { return _AllowEmail; }
        //    set { _AllowEmail = value; }
        //}
        //private bool _AllowPhone;
        //[DataMember]
        //public bool AllowPhone
        //{
        //    get { return _AllowPhone; }
        //    set { _AllowPhone = value; }
        //}
        //private bool _AllowMail;
        //[DataMember]
        //public bool AllowMail
        //{
        //    get { return _AllowMail; }
        //    set { _AllowMail = value; }
        //}
        
        private bool _AllowAutoLoad;
        [DataMember]
        public bool AllowAutoLoad
        {
            get { return _AllowAutoLoad; }
            set { _AllowAutoLoad = value; }
        }
       
        private int _MaxCardsAutoLoad;
         [DataMember]
        public int MaxCardsAutoLoad
        {
            get { return _MaxCardsAutoLoad; }
            set { _MaxCardsAutoLoad = value; }
        }
         private Address[] _Addresses;
         [DataMember]
         public Address[] Addresses
         {
             get { return _Addresses; }
             set { _Addresses = value; }
         }

         private AccountCategoryCode _CustomerType;
          [DataMember(IsRequired=true)]
        public AccountCategoryCode CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
       
          private bool _OrganizationCreditApproved;
         [DataMember]
        public bool OrganizationCreditApproved
        {
            get { return _OrganizationCreditApproved; }
            set { _OrganizationCreditApproved = value; }
        }
       
         private string _OrganizationNumber;
         [DataMember]
        public string OrganizationNumber
        {
            get { return _OrganizationNumber; }
            set { _OrganizationNumber = value; }
        }

         private string _OrganizationSubNumber;
        [DataMember]
         public string OrganizationSubNumber
         {
             get { return _OrganizationSubNumber; }
             set { _OrganizationSubNumber = value; }
         }

        private string _Responsibility;
        [DataMember]
        public string Responsibility
        {
            get { return _Responsibility; }
            set { _Responsibility = value; }
        }

        private string _Rsid;
        [DataMember]
        public string Rsid
        {
            get { return _Rsid; }
            set { _Rsid = value; }
        }
        
        private string _Counterpart;
        [DataMember]
        public string Counterpart
        {
            get { return _Counterpart; }
            set { _Counterpart = value; }
        }

        private string _SocialSecurityNumber;
        [DataMember]
        public string SocialSecurityNumber
        {
            get { return _SocialSecurityNumber; }
            set { _SocialSecurityNumber = value; }
        }
 
        private bool _InActive;
         [DataMember]
        public bool InActive
        {
            get { return _InActive; }
            set { _InActive = value; }
        }
       
         private bool _Deleted;
               [DataMember]
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

    }
}