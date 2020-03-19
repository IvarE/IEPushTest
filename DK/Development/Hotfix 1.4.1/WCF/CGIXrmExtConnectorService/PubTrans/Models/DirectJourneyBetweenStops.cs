using System;
using System.Xml.Serialization;

[XmlRoot("GetDirectJourneysBetweenStopsResponseDirectJourneysBetweenStops")]
public class DirectJourneyBetweenStops
{
    #region Public Properties
    [XmlElement("DatedVehicleJourneyId")]
    public string DatedVehicleJourneyId { get; set; }

    [XmlElement("ServiceJourneyGid")]
    public string ServiceJourneyGid { get; set; }

    [XmlElement("OperatingDayDate")]
    public DateTime OperatingDayDate { get; set; }

    [XmlElement("ContractorGid")]
    public string ContractorGid { get; set; }

    [XmlElement("LineDesignation")]
    public string LineDesignation { get; set; }

    [XmlElement("JourneyNumber")]
    public string JourneyNumber { get; set; }

    [XmlElement("DirectionOfLineDescription")]
    public string DirectionOfLineDescription { get; set; }

    [XmlElement("OriginName")]
    public string OriginName { get; set; }

    [XmlElement("OriginShortName")]
    public string OriginShortName { get; set; }

    [XmlElement("OriginPlaceGid")]
    public string OriginPlaceGid { get; set; }

    [XmlElement("PrimaryDestinationName")]
    public string PrimaryDestinationName { get; set; }

    [XmlElement("PrimaryShortDestinationName")]
    public string PrimaryShortDestinationName { get; set; }

    [XmlElement("PrimaryDestinationPlaceGid")]
    public string PrimaryDestinationPlaceGid { get; set; }

    [XmlElement("SecondaryDestinationName")]
    public string SecondaryDestinationName { get; set; }

    [XmlElement("SecondaryDestinationShortName")]
    public string SecondaryDestinationShortName { get; set; }

    [XmlElement("SecondaryDestinationPlaceGid")]
    public string SecondaryDestinationPlaceGid { get; set; }

    [XmlElement("ExpectedToBeMonitored")]
    public bool ExpectedToBeMonitored { get; set; }

    [XmlElement("DepartureId")]
    public string DepartureId { get; set; }

    [XmlElement("DepartureStopPostringGid")]
    public string DepartureStopPostringGid { get; set; }

    [XmlElement("DepartureType")]
    public string DepartureType { get; set; }

    [XmlElement("DepartureSequenceNumber")]
    public string DepartureSequenceNumber { get; set; }

    [XmlElement("PlannedDepartureDateTime")]
    public DateTime PlannedDepartureDateTime { get; set; }

    [XmlElement("ObservedDepartureDateTime")]
    public DateTime ObservedDepartureDateTime { get; set; }

    [XmlElement("ArrivalId")]
    public string ArrivalId { get; set; }

    [XmlElement("ArrivesToStopPostringGid")]
    public string ArrivesToStopPostringGid { get; set; }

    [XmlElement("ArrivalType")]
    public string ArrivalType { get; set; }

    [XmlElement("ArrivalSequenceNumber")]
    public string ArrivalSequenceNumber { get; set; }

    [XmlElement("PlannedArrivalDateTime")]
    public DateTime PlannedArrivalDateTime { get; set; }

    [XmlElement("ObservedArrivalDateTime")]
    public DateTime ObservedArrivalDateTime { get; set; }

    [XmlElement("TargetDepartureStopPostringGid")]
    public string TargetDepartureStopPostringGid{get;set;}

    [XmlElement("TargetDepartureDateTime")]
    public DateTime TargetDepartureDateTime { get; set; }

    [XmlElement("ArrivesToStopPostringGid1")]
    public string ArrivesToStopPostringGid1 { get; set; }

    public bool HasArrivalDeviation { get; set; }

    public bool HasDepartureDeviation { get; set; }

    public bool HasServiceJourneyDeviation { get; set; }

    public string ArrivalDeviationMessage { get; set; }

    public string DepartureDeviationMessage { get; set; }

    public string ServiceJourneyDeviationMessage { get; set; }
    #endregion
}
