using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class AutoRgCaseRequest
    {
        #region Public Properties
        [DataMember]
        public string CustomersCategory { get; set; } //saved as Case customer category

        [DataMember]
        public string CustomersSubcategory { get; set; }//saved as Case customer subcategory

        [DataMember]
        public string Title { get; set; } //saved as case title

        [DataMember]
        public string Description { get; set; } //saved as case ed_description (scaled)

        [DataMember]
        public string RGOLExtendedDescription { get; set; } //saved as case description

        [DataMember]
        public string QueueId { get; set; } // default queue to add case to

        [DataMember]
        public string CustomerId { get; set; } //if given this will be used to fetch customer information

        [DataMember]
        public int CustomerType { get; set; } // 0 = private, 1 = organization used when looking up Account och contact customer

        [DataMember]
        public string CardNumber { get; set; } // saved as case travelcardnumber

        [DataMember]
        public string SKACardNumber { get; set; } // saved as case SKA Card number

        [DataMember]
        public string WayOfTravel { get; set; }//saved as case Incident way of transport

        [DataMember]
        public string Line { get; set; }//saved as case line

        [DataMember]
        public string FirstName { get; set; }//used to create a private customer if no customer is found by customerid or email.

        [DataMember]
        public string LastName { get; set; }//used to create a private customer if no customer is found by customerid or email.

        //Used to search for customer if customerid is not given
        //also used to create a private customer if no customer is found by customerid or email.
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string DeliveryEmailAddress { get; set; }

        [DataMember]
        public string MobileNo { get; set; }//sparas på case cgi_customer_telephonenumber_mobile

        [DataMember]
        public string RGOLIssueID { get; set; }//saved as case cgi_rgolissueid

        [DataMember]
        public DateTime? DepartureDateTime { get; set; }//saved as case cgi_departuredatetime

        [DataMember]
        public string ExperiencedDelay { get; set; } // not implemented logic for this paramameter in code. TODO datatype? crm field??

        [DataMember]
        public string SocialSecurityNumber { get; set; }

        [DataMember]
        public string Address_Line1 { get; set; }

        [DataMember]
        public string Address_Line2 { get; set; }

        [DataMember]
        public string Address_PostalCode { get; set; }

        [DataMember]
        public string Address_City { get; set; }

        [DataMember]
        public string Address_Country { get; set; }

        [DataMember]
        public string TicketType1 { get; set; }

        [DataMember]
        public string TicketNumber1 { get; set; }

        [DataMember]
        public string TicketType2 { get; set; }

        [DataMember]
        public string TicketNumber2 { get; set; }
        
        [DataMember]
        public string MileageFrom { get; set; }

        [DataMember]
        public string MileageTo { get; set; }

        [DataMember]
        public string MileageKilometers { get; set; }

        [DataMember]
        public string MileageLicencePlateNumber { get; set; }

        [DataMember]
        public string TaxiFrom { get; set; }

        [DataMember]
        public string TaxiTo { get; set; }

        [DataMember]
        public decimal TaxiClaimedAmount { get; set; }

        [DataMember]
        public FileLink[] FileLinks { get; set; }

        [DataMember]
        public string Iban { get; set; }

        [DataMember]
        public string Bic { get; set; }

        [DataMember]
        public string CustomerSocialSecurityNumber { get; set; }

        [DataMember]
        public string CustomerAddress1Line1 { get; set; }

        [DataMember]
        public string CustomerAddress1Line2 { get; set; }

        [DataMember]
        public string CustomerAddress1Postalcode { get; set; }

        [DataMember]
        public string CustomerAddress1City { get; set; }

        [DataMember]
        public string CustomerAddress1Country { get; set; }


        [DataMember]
        public string CustomerTelephonenumber { get; set; }

        [DataMember]
        public string CustomerFirstName { get; set; }

        [DataMember]
        public string CustomerLastName { get; set; }

        






        #endregion


        #region Internal Properties

        internal bool IsCustomerIdValid
        {
            get
            {
                Guid customerId;
                return Guid.TryParse(CustomerId, out customerId);
            }
        }

        internal bool IsOrganisation
        {
            get
            {
                return CustomerType == 1;
            }
        }

        internal bool IsLoggedIn
        {
            get
            {
                return IsCustomerIdValid;
            }
        }

        #endregion
    }
}
