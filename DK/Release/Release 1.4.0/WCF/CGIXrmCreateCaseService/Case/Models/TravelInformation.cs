using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class TravelInformation
    {
        #region Public Properties
        [DataMember]
        public string Line { get; set; } //Saved as case cgi_travelinformationline, matches RGOL-DB LineName

        [DataMember]
        public string Company { get; set; } //Saved as case cgi_travelinformationcompany, matches RGOL-DB LineOperatorName

        [DataMember]
        public DateTime? StartPlanned { get; set; } //Saved as case cgi_travelinformationstartplanned, matches RGOL-DB DepartureDate

        [DataMember]
        public DateTime? ArrivalPlanned { get; set; } //Saved as case cgi_travelinformationarrivalplanned, matches RGOL-DB ArrivalDate

        [DataMember]
        public string Title { get; set; } //Saved as case cgi_travelinformation, matches ???

        [DataMember]
        public string ArrivalActual { get; set; } // Saved as case cgi_travelinformationarrivalactual, matches ???

        [DataMember]
        public string City { get; set; } //Saved as case cgi_travelinformationcity, matches ???

        [DataMember]
        public string DeviationMessage { get; set; } //Saved as case cgi_travelinformationdeviationmessage, matches ???

        [DataMember]
        public string DirectionText { get; set; } // Saved as case cgi_travelinformationdirectiontext, matches ???

        [DataMember]
        public string DisplayText { get; set; } //Saved as case cgi_travelinformationdisplaytext, matches ???

        [DataMember]
        public string Start { get; set; } //Saved as case cgi_travelinformationstart, matches DepartureName?

        [DataMember]
        public string StartActual { get; set; } //Saved as case cgi_travelinformationstartactual, matches ??

        [DataMember]
        public string Stop { get; set; } //Saved as case cgi_travelinformationstop, matches ArrivalName?

        [DataMember]
        public string Tour { get; set; } //Saved as case cgi_travelinformationtour, matches??

        [DataMember]
        public string Transport { get; set; } //Saved as case cgi_travelinformationtransport, matches ??
        #endregion
    }
}