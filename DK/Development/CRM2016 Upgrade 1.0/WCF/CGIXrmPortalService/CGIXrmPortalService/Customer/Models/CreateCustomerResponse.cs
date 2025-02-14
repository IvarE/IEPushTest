﻿using System;
using System.Runtime.Serialization;
using CGICRMPortalService.Shared.Models;

namespace CGICRMPortalService.Customer.Models
{
    [DataContract]
    public class CreateCustomerResponse:Response
    {
        #region Public Properties
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
        #endregion
    }
}