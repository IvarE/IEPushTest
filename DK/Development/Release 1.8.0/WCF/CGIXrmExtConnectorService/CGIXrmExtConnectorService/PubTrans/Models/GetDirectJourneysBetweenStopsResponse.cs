using CGIXrmExtConnectorService.SvcDirectJourneyBetweenStops;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

[DataContract]
public class GetDirectJourneysBetweenStopsResponse
{
    ObservableCollection<DirectJourneyBetweenStops> _DirectJourneysBetweenStops;
    [DataMember]
    public ObservableCollection<DirectJourneyBetweenStops> DirectJourneysBetweenStops
    {
        get { return _DirectJourneysBetweenStops; }
        set { _DirectJourneysBetweenStops = value; }
    }
    ObservableCollection<DeviationMessageVersion> _DeviationMessageVersions;
     [DataMember]
    public ObservableCollection<DeviationMessageVersion> DeviationMessageVersions
    {
        get { return _DeviationMessageVersions; }
        set { _DeviationMessageVersions = value; }
    }
     ObservableCollection<DeviationMessageVariant> _DeviationMessageVariants;
     [DataMember]
     public ObservableCollection<DeviationMessageVariant> DeviationMessageVariants
     {
         get { return _DeviationMessageVariants; }
         set { _DeviationMessageVariants = value; }
     }
     private ObservableCollection<ServiceJourneyDeviation> _ServiceJourneyDeviations;
     [DataMember]
     public ObservableCollection<ServiceJourneyDeviation> ServiceJourneyDeviations
     {
         get { return _ServiceJourneyDeviations; }
         set { _ServiceJourneyDeviations = value; }
     }
    ObservableCollection<ArrivalDeviation> _ArrivalDeviations;
    [DataMember]
    public ObservableCollection<ArrivalDeviation> ArrivalDeviations
    {
        get { return _ArrivalDeviations; }
        set { _ArrivalDeviations = value; }
    }
    ObservableCollection<DepartureDeviation> _DepartureDeviations;
    [DataMember]
    public ObservableCollection<DepartureDeviation> DepartureDeviations
    {
        get { return _DepartureDeviations; }
        set { _DepartureDeviations = value; }
    }
   
    
    
}