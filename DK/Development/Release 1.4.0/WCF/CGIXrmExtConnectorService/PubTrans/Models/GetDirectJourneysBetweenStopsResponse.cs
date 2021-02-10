using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.PubTrans.Models
{
    [DataContract]
    public class GetDirectJourneysBetweenStopsResponse
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public ObservableCollection<DirectJourneyBetweenStops> DirectJourneysBetweenStops { get; set; }

        [DataMember]
        public ObservableCollection<DeviationMessageVersion> DeviationMessageVersions { get; set; }

        [DataMember]
        public ObservableCollection<DeviationMessageVariant> DeviationMessageVariants { get; set; }

        [DataMember]
        public ObservableCollection<ServiceJourneyDeviation> ServiceJourneyDeviations { get; set; }

        [DataMember]
        public ObservableCollection<ArrivalDeviation> ArrivalDeviations { get; set; }

        [DataMember]
        public ObservableCollection<DepartureDeviation> DepartureDeviations { get; set; }

        #endregion
    }
}