﻿using System;
using System.Runtime.Serialization;
using CGIXrmEAIConnectorService.Shared.Models;

namespace CGIXrmEAIConnectorService.AllBinary.Models
{
    [DataContract]
    public class Customer
    {
        #region Public Properties

        [DataMember(IsRequired = true)]
        public string CompanyName { get; set; }

        public Guid PrimaryContact { get; set; }

        [DataMember(IsRequired = true)]
        public string AccountFirstName { get; set; }

        [DataMember(IsRequired = true)]
        public string AccountLastName { get; set; }

        [DataMember]
        public Guid AccountId { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember(IsRequired = true)]
        internal string MobilePhone { get; set; }

        [DataMember]
        internal string Phone { get; set; }

        [DataMember(IsRequired = true)]
        public string Email { get; set; }

        [DataMember]
        public bool AllowAutoLoad { get; set; }

        [DataMember]
        public int MaxCardsAutoLoad { get; set; }

        [DataMember]
        public Address[] Addresses { get; set; }

        [DataMember(IsRequired = true)]
        public AccountCategoryCode CustomerType { get; set; }

        [DataMember]
        public bool OrganizationCreditApproved { get; set; }

        [DataMember]
        public string OrganizationNumber { get; set; }

        [DataMember]
        public string OrganizationSubNumber { get; set; }

        [DataMember]
        public string Responsibility { get; set; }

        [DataMember]
        public string Rsid { get; set; }

        [DataMember]
        public string Counterpart { get; set; }

        [DataMember]
        public string SocialSecurityNumber { get; set; }

        [DataMember]
        public bool InActive { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        #endregion
    }
}