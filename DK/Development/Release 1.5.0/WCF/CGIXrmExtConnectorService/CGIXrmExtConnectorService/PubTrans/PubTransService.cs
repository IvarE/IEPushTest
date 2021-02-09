using System;
using System.Collections.Generic;
using CGIXrmExtConnectorService.PubTrans.Models;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService.PubTrans
{
    public class ExtConnectorService : IPubTransService
    {
        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Zone> GetStradBusDetails()
        {
        
            var pubtransManager = new PubTransManager();
            return pubtransManager.GetStradBusDetails();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Line> GetRegionBusDetails()
        {

            var pubtransManager = new PubTransManager();
            return pubtransManager.GetRegionBusDetails();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<StopArea> GetTrainDetails()
        {

            var pubtransManager = new PubTransManager();
            return pubtransManager.GetTrainDetails();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromStopArea"></param>
        /// <param name="toStopArea"></param>
        /// <param name="tripDateTime"></param>
        /// <param name="forLineGids"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public GetDirectJourneysBetweenStopsResponse GetDirectJourneys(
            string fromStopArea, 
            string toStopArea, 
            DateTime tripDateTime, 
            string forLineGids, 
            TransportType transportType)
        {
            var pubtransManager = new PubTransManager();
            
            return pubtransManager.GetDirectJourneysbetweenStops(
                fromStopArea, 
                toStopArea, 
                tripDateTime, 
                forLineGids,
                transportType);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceJourneyId"></param>
        /// <param name="operatingDate"></param>
        /// <param name="atStopGid"></param>
        /// <returns></returns>
        public CallsForServiceJourneyResponse GetCallsForServiceJourney(
            string serviceJourneyId, 
            DateTime operatingDate, 
            string atStopGid)
        {
            var pubtransManager = new PubTransManager();
            return pubtransManager.GetCallsForServiceJourney(
                serviceJourneyId, 
                operatingDate, 
                atStopGid);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrganisationalUnitResponse GetOrganisationalUnits()
        {

            var pubtransManager = new PubTransManager();
            return pubtransManager.GetOrganisationalUnits();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContractorResponse GetContractors()
        {

            var pubtransManager = new PubTransManager();
            return pubtransManager.GetContractors();

        }

        #endregion
    }
}
