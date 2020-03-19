using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class CallsForServiceJourneyResponse
    {
        #region Public Properties

        [DataMember]
        public ObservableCollection<DatedServiceJourney> DatedServiceJourneys { get; set; }

        [DataMember]
        public ObservableCollection<DatedArrival> DatedArrivals { get; set; }


        [DataMember]
        public ObservableCollection<DatedDeparture> DatedDepartures { get; set; }

        [DataMember]
        public ObservableCollection<DeviationMessageVersion1> DeviationMessageVersions { get; set; }

        [DataMember]
        public ObservableCollection<DeviationMessageVariant1> DeviationMessageVariants { get; set; }

        [DataMember]
        public ObservableCollection<ServiceJourneyDeviation1> ServiceJourneyDeviations { get; set; }

        [DataMember]
        public ObservableCollection<ArrivalDeviation1> ArrivalDeviations { get; set; }

        [DataMember]
        public ObservableCollection<DepartureDeviation1> DepartureDeviations { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}