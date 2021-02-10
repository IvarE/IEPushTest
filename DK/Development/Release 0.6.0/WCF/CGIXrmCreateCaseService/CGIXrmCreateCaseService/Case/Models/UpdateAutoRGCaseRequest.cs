using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class UpdateAutoRGCaseRequest
    {

        [DataMember]
        public string CaseID { get; set; }//used to get the parent incident

        //if true the refund transaction will be closed as Approved - Transaction Pending and the transaction will be executed by workflow
        //if false the the transaction will be closed as Declined and no further actions will be taken
        [DataMember]
        public bool Approved { get; set; }

        [DataMember]
        public int RefundType { get; set; } //used to get the RgolSetting which holds the ReImBursementFormId used when creating the refund

        [DataMember]
        public decimal Value { get; set; } //used as amount of the refund

        [DataMember]
        public string Currency { get; set; } //not used


        [DataMember]
        public string InternalMessage { get; set; }//saved as cgi_comments on the refund

        [DataMember]
        public string CustomerMessage { get; set; }//saved as cgi_CustomerMessage on the refund

        [DataMember]
        public int DelayType { get; set; }//saved as [REPLACEME] on the [REPLACEME] TODO type, från johan *DelayType = "30-59 min"; (eller snarare en enum (int))

        [DataMember]
        public string TravelInformationLine { get; set; } //Saved as case cgi_travelinformationline, matches RGOL-DB LineName

        [DataMember]
        public string TravelInformationCompany { get; set; } //Saved as case cgi_travelinformationcompany, matches RGOL-DB LineOperatorName

        [DataMember]
        public DateTime? TravelInformationStartPlanned { get; set; } //Saved as case cgi_travelinformationstartplanned, matches RGOL-DB DepartureDate

        [DataMember]
        public DateTime? TravelInformationArrivalPlanned { get; set; } //Saved as case cgi_travelinformationarrivalplanned, matches RGOL-DB ArrivalDate

        [DataMember]
        public string TravelInformationTitle { get; set; } //Saved as case cgi_travelinformation, matches ???

        [DataMember]
        public string TravelInformationArrivalActual { get; set; } // Saved as case cgi_travelinformationarrivalactual, matches ???

        [DataMember]
        public string TravelInformationCity { get; set; } //Saved as case cgi_travelinformationcity, matches ???

        [DataMember]
        public string TravelInformationDeviationMessage { get; set; } //Saved as case cgi_travelinformationdeviationmessage, matches ???

        [DataMember]
        public string TravelInformationDirectionText { get; set; } // Saved as case cgi_travelinformationdirectiontext, matches ???

        [DataMember]
        public string TravelInformationDisplayText { get; set; } //Saved as case cgi_travelinformationdisplaytext, matches ???

        [DataMember]
        public string TravelInformationStart { get; set; } //Saved as case cgi_travelinformationstart, matches DepartureName?

        [DataMember]
        public string TravelInformationStartActual { get; set; } //Saved as case cgi_travelinformationstartactual, matches ??

        [DataMember]
        public string TravelInformationStop { get; set; } //Saved as case cgi_travelinformationstop, matches ArrivalName?

        [DataMember]
        public string TravelInformationTour { get; set; } //Saved as case cgi_travelinformationtour, matches??

        [DataMember]
        public string TravelInformationTransport { get; set; } //Saved as case cgi_travelinformationtransport, matches ??

    }
}


