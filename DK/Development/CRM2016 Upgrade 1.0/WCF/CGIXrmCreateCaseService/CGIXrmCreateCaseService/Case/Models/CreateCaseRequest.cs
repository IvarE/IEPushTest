using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using CGIXrmCreateCaseService.Case.Models;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class CreateCaseRequest
    {
        #region Public Properties
        [DataMember]
        public string CustomersCategory { get; set; }

        [DataMember]
        public string CustomersSubcategory { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public Nullable<Guid> Customer { get; set; }

        [DataMember]
        public CustomerType CustomerType { get; set; }

        [DataMember]
        public string InvoiceNumber { get; set; } 

        [DataMember]
        public string ControlFeeNumber { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public string WayOfTravel { get; set; }

        [DataMember]
        public string Line { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string DriverId { get; set; }

        [DataMember]
        public string Train { get; set; }

        [DataMember]
        public string County { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public bool ContactCustomer { get; set; }

        [DataMember]
        public string MobilePhoneNumber { get; set; }

        [DataMember]
        public List<Document> DocumentList { get; set; }

        [DataMember]
        public List<FileLink> FileLinks { get; set; }

        [DataMember]
        public DateTime? ActionDate { get; set; }
        #endregion

    }
}