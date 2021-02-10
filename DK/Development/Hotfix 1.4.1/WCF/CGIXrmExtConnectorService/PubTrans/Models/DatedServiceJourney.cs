using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[DataContract]
[XmlRoot("GetCallsforServiceJourneyMethodDatedServiceJourney")]
public class DatedServiceJourney
{
    #region Public Properties
    [DataMember]
    [XmlElement("Id")]
    public string Id { get; set; }
    [DataMember]
    [XmlElement("IsDatedVehicleJourneyId")]
    public string IsDatedVehicleJourneyId { get; set; }
    [DataMember]
    public string OperatingDayDate { get; set; }
    [DataMember]
    public string Gid { get; set; }
    [DataMember]
    public string IsWorkedOnDirectionOfLineGid { get; set; }
    [DataMember]
    public string LineDesignation { get; set; }
    [DataMember]
    public string TransportModeCode { get; set; }
    [DataMember]
    public string TransportAuthorityCode { get; set; }
    [DataMember]
    public string TransportAuthorityName { get; set; }
    [DataMember]
    public string ContractorCode { get; set; }
    [DataMember]
    public string ContractorName { get; set; }
    [DataMember]
    public Boolean ExpectedToBeMonitored { get; set; }
    [DataMember]
    public string IsAssignedToVehicleGid { get; set; }
    [DataMember]
    public string State { get; set; }
    [DataMember]
    public string PredictionState { get; set; }
    [DataMember]
    public string OriginName { get; set; }
    [DataMember]
    public string OriginShortName { get; set; }
    [DataMember]
    public string ProductType { get; set; }
    #endregion
}

