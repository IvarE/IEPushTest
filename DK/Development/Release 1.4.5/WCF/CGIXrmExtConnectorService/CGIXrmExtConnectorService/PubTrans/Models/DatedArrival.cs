using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    [XmlRoot("GetCallsforServiceJourneyMethodDatedArrival")]
    public class DatedArrival
    {
        #region Public Properties -----------------------------------------------------------------------------------------

        [DataMember]
        public string Id { get; set; }


        [DataMember]
        public string IsOnDatedServiceJourneyId { get; set; }


        [DataMember]
        public string JourneyPatternSequenceNumber { get; set; }


        [DataMember]
        public string IsTimetabledAtJourneyPatternPointGid { get; set; }


        [DataMember]
        public string IsTargetedAtJourneyPatternPointGid { get; set; }


        [DataMember]
        public string WasObservedAtJourneyPatternPointGid { get; set; }


        [DataMember]
        public string TimetabledEarliestDateTime { get; set; }


        [DataMember]
        public string TargetDateTime { get; set; }


        [DataMember]
        public string EstimatedDateTime { get; set; }


        [DataMember]
        public string ObservedDateTime { get; set; }


        [DataMember]
        public string State { get; set; }


        [DataMember]
        public string Type { get; set; }


        [DataMember]
        public string PresentationType { get; set; }

        #endregion
    }
}