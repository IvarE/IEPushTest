using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class CreateAutoRGCaseRequest
    {
        [DataMember]
        public string CustomersCategory { get; set; } //saved as Case customer category

        [DataMember]
        public string CustomersSubcategory { get; set; }//saved as Case customer subcategory

        [DataMember]
        public string Title { get; set; } //saved as case title

        [DataMember]
        public string Description { get; set; } //saved as case description

        [DataMember]
        public string Customer { get; set; } //if given this will be used to fetch customer information

        [DataMember]
        public int CustomerType { get; set; } // 0 = private, 1 = organization used when looking up account och contact customer

        [DataMember]
        public string CardNumber { get; set; } // saved as case travelcardnumber

        [DataMember]
        public string WayOfTravel { get; set; }//saved as case incident way of transport

        [DataMember]
        public string Line { get; set; }//saved as case line

        [DataMember]
        public string FirstName { get; set; }//used to create a private customer if no customer is found by customerid or email.

        [DataMember]
        public string LastName { get; set; }//used to create a private customer if no customer is found by customerid or email.

        //Useda to search for customer if customerid is not given
        //also used to create a private customer if no customer is found by customerid or email.
        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string MobileNo { get; set; }//sparas på case cgi_customer_telephonenumber_mobile

        [DataMember]
        public string RGOLIssueID { get; set; }//saved as case cgi_rgolissueid

        [DataMember]
        public DateTime? DepartureDateTime { get; set; }//saved as case cgi_departuredatetime

        [DataMember]
        public string City { get; set; } // not implemented logic for this paramameter in code

        [DataMember]
        public string ExperiencedDelay { get; set; } // not implemented logic for this paramameter in code. TODO datatype? crm field??





    }
}
