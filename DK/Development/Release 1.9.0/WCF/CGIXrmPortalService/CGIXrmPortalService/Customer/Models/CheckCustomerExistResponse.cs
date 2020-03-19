using System;
using System.Runtime.Serialization;
using CGICRMPortalService.Shared.Models;

namespace CGICRMPortalService.Customer.Models
{
    [DataContract]
    public class CheckCustomerExistResponse : Response
    {
        #region Public Properties
        [DataMember]
        public AccountCategoryCode? CustomerType { get; set; }

        [DataMember]
        public Guid AccountId { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public bool CustomerExists { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
        #endregion
    }
}