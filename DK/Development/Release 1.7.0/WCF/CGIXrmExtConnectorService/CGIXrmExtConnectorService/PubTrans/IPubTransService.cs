using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CGIXrmExtConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExtConnectorService" in both code and config file together.
    [ServiceContract]
    public interface IPubTransService
    {
        [OperationContract]
        List<Zone> GetStradBusDetails();
        [OperationContract]
        List<Line> GetRegionBusDetails();
        [OperationContract]
        List<StopArea> GetTrainDetails();
        [OperationContract]
        GetDirectJourneysBetweenStopsResponse GetDirectJourneys(string fromStopArea, string toStopArea, DateTime tripDate, string forLineGids, TransportType transportType);
        [OperationContract]
        CallsForServiceJourneyResponse GetCallsForServiceJourney(string serviceJourneyId, DateTime operatingDate, string atStopGid);
        [OperationContract]
        OrganisationalUnitResponse GetOrganisationalUnits();
        [OperationContract]
        ContractorResponse GetContractors();
    }
}
