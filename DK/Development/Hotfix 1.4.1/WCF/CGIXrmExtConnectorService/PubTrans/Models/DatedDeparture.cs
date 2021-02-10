using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("GetCallsforServiceJourneyMethodDatedDeparture")]
public class DatedDeparture
{
    #region Public Properties
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
    public string ProductName { get; set; }
    [DataMember]
    public string LineDesignation { get; set; }
    [DataMember]
    public string PrimaryDestinationName { get; set; }
    [DataMember]
    public string PrimaryDestinationShortName { get; set; }
    [DataMember]
    public string SecondaryDestinationName { get; set; }
    [DataMember]
    public string SecondaryDestinationShortName { get; set; }
    [DataMember]
    public string SecondaryDestinationType { get; set; }
    [DataMember]
    public string SymbolName { get; set; }
    [DataMember]
    public string PresentationType { get; set; }
    #endregion
}

