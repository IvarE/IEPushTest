
using System;
using System.Collections.Generic;
using System.ServiceModel;
namespace CGIXrmExtConnectorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExtConnectorService" in code, svc and config file together.
    public partial class ExtConnectorService : IPubTransService
    {
        #region Public Methods
        public List<Zone> GetStradBusDetails()
        {
        
            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetStradBusDetails();

        }

        public List<Line> GetRegionBusDetails()
        {

            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetRegionBusDetails();

        }

        public List<StopArea> GetTrainDetails()
        {

            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetTrainDetails();

        }

        public GetDirectJourneysBetweenStopsResponse GetDirectJourneys(string fromStopArea, string toStopArea, DateTime tripDateTime, string forLineGids, TransportType transportType)
        {
            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetDirectJourneysbetweenStops(fromStopArea, toStopArea, tripDateTime, forLineGids, transportType);

        }

        public CallsForServiceJourneyResponse GetCallsForServiceJourney(string serviceJourneyId, DateTime operatingDate, string atStopGid)
        {
            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetCallsForServiceJourney(serviceJourneyId, operatingDate, atStopGid);

        }

        public OrganisationalUnitResponse GetOrganisationalUnits()
        {

            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetOrganisationalUnits();

        }

        public ContractorResponse GetContractors()
        {

            PubTransManager pubtransManager = new PubTransManager();
            return pubtransManager.GetContractors();

        }
        #endregion
    }
}
