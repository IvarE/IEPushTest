using System;
using System.Collections.Generic;
using System.ServiceModel;
using CGIXrmExtConnectorService.PubTrans.Models;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService.PubTrans
{
    [ServiceContract]
    public interface IPubTransService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        [OperationContract]
        List<Zone> GetStradBusDetails();

        [OperationContract]
        List<Line> GetRegionBusDetails();

        [OperationContract]
        List<StopArea> GetTrainDetails();

        [OperationContract]
        GetDirectJourneysBetweenStopsResponse GetDirectJourneys(
            string fromStopArea, 
            string toStopArea, 
            DateTime tripDate, 
            string forLineGids, 
            TransportType 
            transportType);

        [OperationContract]
        CallsForServiceJourneyResponse GetCallsForServiceJourney(
            string serviceJourneyId, 
            DateTime operatingDate, 
            string atStopGid);

        [OperationContract]
        OrganisationalUnitResponse GetOrganisationalUnits();

        [OperationContract]
        ContractorResponse GetContractors();

        #endregion
    }
}
