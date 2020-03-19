using CGIXrmExtConnectorService;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CGIXrmExtConnectorServiceTest
{
    [TestClass]
    public class PubTransManagerTest
    {
        PubTransManager _pubTransManager;
        private readonly XrmManager _manager;

        public PubTransManagerTest()
        {
            _pubTransManager = new PubTransManager();
            //Logga in mot CRM
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _manager = new XrmManager(serverAdress, domain, username, password);
        }
        [TestMethod]
        public void GetStradBusDetailsTest()
        {
            List<Zone> zoneList = _pubTransManager.GetStradBusDetails();
            //Assert.IsNotNull(zoneList);
        }

        [TestMethod]
        public void GetRegionBusDetailsTest()
        {
            List<Line> lines = _pubTransManager.GetRegionBusDetails();
            //Assert.IsNotNull(lines);
        }

        [TestMethod]
        public void GetTrainDetailsTest()
        {
            List<StopArea> stopAreas = _pubTransManager.GetTrainDetails();
            //Assert.IsNotNull(stopAreas);
        }

        //Method does not work
        //[TestMethod]
        //public void GetDirectJourneysbetweenStops0Test()
        //{
        //    string fromStopAreaGid = "9021012080000000";
        //    string toStopAreaGid = "9021012080051000";
        //    DateTime journeyDateTime = new DateTime(2014, 05, 13);
        //    string forLineGids = "9011012000800000";
        //    TransportType transportType = TransportType.Citybus;

        //    GetDirectJourneysBetweenStopsResponse journeyResponse = _pubTransManager.GetDirectJourneysbetweenStops0(fromStopAreaGid, toStopAreaGid, journeyDateTime, forLineGids, transportType);
        //    Assert.IsNotNull(journeyResponse);
        //    Assert.IsNotNull(journeyResponse.ArrivalDeviations);
        //    Assert.IsNotNull(journeyResponse.DepartureDeviations);
        //    Assert.IsNotNull(journeyResponse.DeviationMessageVariants);
        //    Assert.IsNotNull(journeyResponse.DeviationMessageVersions);
        //    Assert.IsNotNull(journeyResponse.DirectJourneysBetweenStops);
        //    Assert.IsNotNull(journeyResponse.ServiceJourneyDeviations);
        //}

        [TestMethod]
        public void GetDirectJourneysbetweenStopsTest()
        {
            string fromStopAreaGid = "9021012080000000";
            string toStopAreaGid = "9021012080051000";
            DateTime journeyDateTime = new DateTime(2014, 05, 13);
            string forLineGids = "9011012000800000";
            TransportType transportType = TransportType.CITYBUS;

            GetDirectJourneysBetweenStopsResponse journeyResponse = _pubTransManager.GetDirectJourneysbetweenStops(fromStopAreaGid, toStopAreaGid, journeyDateTime, forLineGids, transportType);
            Assert.IsNotNull(journeyResponse);
            Assert.IsNotNull(journeyResponse.ArrivalDeviations);
            Assert.IsNotNull(journeyResponse.DepartureDeviations);
            Assert.IsNotNull(journeyResponse.DeviationMessageVariants);
            Assert.IsNotNull(journeyResponse.DeviationMessageVersions);
            Assert.IsNotNull(journeyResponse.DirectJourneysBetweenStops);
            Assert.IsNotNull(journeyResponse.ServiceJourneyDeviations);
        }

        [TestMethod]
        public void GetCallsForServiceJourneyTest()
        {
            string serviceJourneyId = "9015012017400100";
            DateTime operatingDate = new DateTime(2014, 10, 21);
            string atStopGid = "9025012008012005";
            bool includeArrivalTable = true;
            bool includeDepartureTable = true;
            bool includeDeviationTable = true;

            CallsForServiceJourneyResponse journeyResponse =_pubTransManager.GetCallsForServiceJourney(serviceJourneyId, operatingDate, atStopGid, includeArrivalTable, includeDepartureTable, includeDeviationTable);
            Assert.IsNotNull(journeyResponse);
            Assert.IsNotNull(journeyResponse.ArrivalDeviations);
            Assert.IsNotNull(journeyResponse.DatedArrivals);
            Assert.IsNotNull(journeyResponse.DatedDepartures);
            Assert.IsNotNull(journeyResponse.DatedServiceJourneys);
            Assert.IsNotNull(journeyResponse.DepartureDeviations);
            Assert.IsNotNull(journeyResponse.DeviationMessageVariants);
            Assert.IsNotNull(journeyResponse.DeviationMessageVersions);
            Assert.IsNotNull(journeyResponse.ServiceJourneyDeviations);
        }

        [TestMethod]
        public void GetOrganisationalUnitsTest()
        {
            OrganisationalUnitResponse organisationalUnitResponse = _pubTransManager.GetOrganisationalUnits();
            Assert.IsNotNull(organisationalUnitResponse);
            Assert.IsNotNull(organisationalUnitResponse.OrganisationalUnitList);
        }

        [TestMethod]
        public void GetContractorsTest()
        {
            ContractorResponse contractorResponse = _pubTransManager.GetContractors();
            Assert.IsNotNull(contractorResponse);
            Assert.IsNotNull(contractorResponse.ContractorList);
        }
    }
}
