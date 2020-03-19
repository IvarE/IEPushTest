using System.Collections.ObjectModel;
using System.Runtime.Serialization;

[DataContract]
public class CallsForServiceJourneyResponse
{
    #region Public Properties
    ObservableCollection<DatedServiceJourney> _DatedServiceJourneys;
    [DataMember]
    public ObservableCollection<DatedServiceJourney> DatedServiceJourneys
    {
        get { return _DatedServiceJourneys; }
        set { _DatedServiceJourneys = value; }
    }

    ObservableCollection<DatedArrival> _DatedArrivals;
    [DataMember]
    public ObservableCollection<DatedArrival> DatedArrivals
    {
        get { return _DatedArrivals; }
        set { _DatedArrivals = value; }
    }


    ObservableCollection<DatedDeparture> _DatedDepartures;
     [DataMember]
    public ObservableCollection<DatedDeparture> DatedDepartures
    {
        get { return _DatedDepartures; }
        set { _DatedDepartures = value; }
    }

    ObservableCollection<DeviationMessageVersion1> _DeviationMessageVersions;
    [DataMember]
    public ObservableCollection<DeviationMessageVersion1> DeviationMessageVersions
    {
        get { return _DeviationMessageVersions; }
        set { _DeviationMessageVersions = value; }
    }

    ObservableCollection<DeviationMessageVariant1> _DeviationMessageVariants;
    [DataMember]
    public ObservableCollection<DeviationMessageVariant1> DeviationMessageVariants
    {
        get { return _DeviationMessageVariants; }
        set { _DeviationMessageVariants = value; }
    }

    private ObservableCollection<ServiceJourneyDeviation1> _ServiceJourneyDeviations;
    [DataMember]
    public ObservableCollection<ServiceJourneyDeviation1> ServiceJourneyDeviations
    {
        get { return _ServiceJourneyDeviations; }
        set { _ServiceJourneyDeviations = value; }
    }

    ObservableCollection<ArrivalDeviation1> _ArrivalDeviations;
    [DataMember]
    public ObservableCollection<ArrivalDeviation1> ArrivalDeviations
    {
        get { return _ArrivalDeviations; }
        set { _ArrivalDeviations = value; }
    }

    ObservableCollection<DepartureDeviation1> _DepartureDeviations;
    [DataMember]
    public ObservableCollection<DepartureDeviation1> DepartureDeviations
    {
        get { return _DepartureDeviations; }
        set { _DepartureDeviations = value; }
    }

    private string _errorMessage;
    [DataMember]
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }
    #endregion
}