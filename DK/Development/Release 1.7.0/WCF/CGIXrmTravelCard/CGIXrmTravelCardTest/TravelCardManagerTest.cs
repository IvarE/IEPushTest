using System.Configuration;
using System.Linq;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Xml;
using System.Xml.Linq;
using SharedUnitTests;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.TravelCard.Models;
using CGIXrmTravelCard.Test.Models;
using CGIXrmPortalServiceTest.Models;

namespace CGIXrmTravelCard.Test
{
    [TestClass]
    public class TravelCardManagerTest
    {
        #region Declarations ------------------------------------------------------------------------------------------
        private readonly XrmManager _manager;
        private readonly TravelCard _travelCard = new TravelCard();

        #endregion

        #region Test Methods ------------------------------------------------------------------------------------------

        public TravelCardManagerTest()
        {
            //Logga in mot CRM
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _manager = new XrmManager(serverAdress, domain, username, password);
        }

        [TestMethod]
        public void GetCardDetailsTest()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var cardDetails = _travelCard.GetCardDetails(firstCardNumber);

            Assert.IsNotNull(cardDetails);
            Assert.AreNotEqual(cardDetails.CardDetails.Length, 0);
            Assert.AreEqual(cardDetails.ErrorMessage, "");
        }

        [TestMethod]
        public void GetCardFromCRM()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var cardFromCrm = _travelCard.GetCardFromCRM(firstCardNumber);
            Assert.IsNotNull(cardFromCrm);
            Assert.IsNotNull(cardFromCrm.Card);
            Assert.AreEqual(cardFromCrm.ErrorMessage, "");
        }

        [TestMethod]
        public void GetCardFromCRMExtended()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var cardFromCrmExtended = _travelCard.GetCardFromCRMExtended(firstCardNumber);
            Assert.IsNotNull(cardFromCrmExtended);
            Assert.IsNotNull(cardFromCrmExtended.Card);
            Assert.AreEqual(cardFromCrmExtended.ErrorMessage, "");
        }

        [TestMethod]
        public void GetOutstandingCharges()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var outstandingCharges = _travelCard.GetGetOutstandingCharges(firstCardNumber);
            Assert.IsNotNull(outstandingCharges);
            Assert.AreEqual(outstandingCharges.ErrorMessage, "");
            XmlStatus xmlStatus = GetStatusFromXML(outstandingCharges.OutstandingCharges);
            //Assert.AreEqual(xmlStatus.StatusCodeMessage, "200");
            Assert.IsNotNull(xmlStatus.Amount);
        }

        [TestMethod]
        public void RechargeCard()
        {
            //Test new travel card (Private customer)
            #region Test new travel card
            TravelCardResponseTest response = SharedTest.TestNewTravelCard(AccountCategoryCode.Private, AccountCategoryCode.Private, false);
            Assert.IsTrue(rechargeTravelCardTest(response, AccountCategoryCode.Private));
            #endregion
            //Test new travel card (Organization)
            #region Test new travel card (organization)
            response = SharedTest.TestNewTravelCard(AccountCategoryCode.Company, AccountCategoryCode.Company, false);
            Assert.IsTrue(rechargeTravelCardTest(response, AccountCategoryCode.Company));
            #endregion
            //Test null
            #region Test Null travel card number
            string firstCardNumber = "";
            var rechargeCard = _travelCard.RechargeCard(firstCardNumber);
            Assert.IsNotNull(rechargeCard);
            XmlStatus xmlStatus = GetStatusFromXML(rechargeCard.RechargeCard);
            Assert.IsTrue(xmlStatus.ErrorMessage.Contains("You must provide an Cardnumber"));
            Assert.AreEqual(xmlStatus.StatusMessage, "false");
            #endregion

        }

        [TestMethod]
        public void GetZoneNames()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var zoneNames = _travelCard.GetZoneNames();
            Assert.IsNotNull(zoneNames);
            Assert.IsNotNull(zoneNames.Zones);
            Assert.IsTrue(zoneNames.Zones.Any());
            Assert.AreEqual(zoneNames.ErrorMessage, "");
        }

        [TestMethod]
        public void GetTravelCardTransactions()
        {
            //Test to retrieve transactions
            //Status: Found
            #region Test1
            var travelcardtransactionid = GetFirstCardTransactionId();
            Assert.IsNotNull(travelcardtransactionid);
            var travelCardTransactions = _travelCard.GetTravelCardTransactions(travelcardtransactionid);
            Assert.IsNotNull(travelCardTransactions);
            Assert.IsNotNull(travelCardTransactions.TravelCardTransactions);
            Assert.IsTrue(travelCardTransactions.TravelCardTransactions.Any());
            Assert.AreEqual(travelCardTransactions.ErrorMessage, "");
            #endregion

            //Test to retrieve transactions
            //Status: Not Found
            //Input: Empty Guid
            #region Test2
            travelcardtransactionid = Guid.Empty.ToString();
            Assert.IsNotNull(travelcardtransactionid);
            travelCardTransactions = _travelCard.GetTravelCardTransactions(travelcardtransactionid);
            Assert.IsNotNull(travelCardTransactions);
            Assert.IsNull(travelCardTransactions.TravelCardTransactions);
            #endregion
        }

        [TestMethod]
        public void GetCardTransactions()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var travelCardTransactions = _travelCard.GetCardTransactions(firstCardNumber, "1", "", "");
            Assert.IsNotNull(travelCardTransactions);
            Assert.IsNotNull(travelCardTransactions.Transactions);
            Assert.AreEqual(travelCardTransactions.ErrorMessage, "");
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// Method for returning the first travel card number
        /// </summary>
        /// <returns></returns>
        private string GetFirstCardNumber()
        {
            //Leta fram ett giltigt kort
            var query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns("cgi_travelcardnumber");

            var results = _manager.Service.RetrieveMultiple(query);
            var firstCardEntity = results.Entities.FirstOrDefault();
            if (firstCardEntity == null)
            {
                return null;
            }
            return firstCardEntity.Attributes["cgi_travelcardnumber"].ToString();
        }

        private string GetTravelCardNumber(Guid travelCardId)
        {
            var query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns("cgi_travelcardnumber");
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = LogicalOperator.And;
            filterExpression.Conditions.Add(new ConditionExpression("cgi_travelcardid", ConditionOperator.Equal, travelCardId));
            query.Criteria.Filters.Add(filterExpression);
            query.TopCount = 1;

            var results = _manager.Service.RetrieveMultiple(query);
            var firstCardEntity = results.Entities.FirstOrDefault();
            if (firstCardEntity == null)
            {
                return null;
            }
            return firstCardEntity.Attributes["cgi_travelcardnumber"].ToString();
        }

        private string GetFirstCardTransactionId()
        {
            //Leta fram ett giltigt transaktionsid
            var query = new QueryExpression("cgi_travelcardtransaction");
            query.ColumnSet.AddColumns("cgi_caseid");
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = LogicalOperator.And;
            filterExpression.Conditions.Add(new ConditionExpression("cgi_caseid", ConditionOperator.NotNull));
            query.Criteria.Filters.Add(filterExpression);
            query.TopCount = 1;

            var results = _manager.Service.RetrieveMultiple(query);
            var firstCardEntity = results.Entities.FirstOrDefault();
            if (firstCardEntity == null)
            {
                return null;
            }
            EntityReference caseRef = (EntityReference)firstCardEntity.Attributes["cgi_caseid"];
            return caseRef.Id.ToString();
        }

        private XmlStatus GetStatusFromXML(string xmlResponse)
        {
            var xml = XDocument.Parse(xmlResponse);


            // Query the data and write out a subset of contacts
            var errorQuery = from c in xml.Root.Descendants("ErrorMessage") select c;
            var statusQuery = from c in xml.Root.Descendants("Success") select c;
            var statusCodeQuery = from c in xml.Root.Descendants("StatusCode") select c;
            var amountQuery = from c in xml.Root.Descendants("Amount") select c;

            XmlStatus xmlStatus = new XmlStatus();

            foreach (string m in errorQuery)
            {
                xmlStatus.ErrorMessage = m;
            }
            foreach (string s in statusQuery)
            {
                xmlStatus.StatusMessage = s;
            }
            foreach (string sc in statusCodeQuery)
            {
                xmlStatus.StatusCodeMessage = sc;
            }
            foreach (int? a in amountQuery)
            {
                xmlStatus.Amount = a;
            }

            return xmlStatus;
        }
        
        private bool rechargeTravelCardTest(TravelCardResponseTest response, AccountCategoryCode accountCategoryCode)
        {
            var firstCardNumber = GetTravelCardNumber(SharedTest.GetUnitTestTravelCard());
            if (firstCardNumber == null) return false;
            var rechargeCard = _travelCard.RechargeCard(firstCardNumber);

            XmlStatus xmlStatus = GetStatusFromXML(rechargeCard.RechargeCard);

            SharedTest.DeleteCustomerEntity(response.AccountId, accountCategoryCode);
            SharedTest.DeleteTravelCardEntity(SharedTest.GetUnitTestTravelCard());

            if (rechargeCard == null) return false;
            if (!xmlStatus.ErrorMessage.Contains("Found no registered card in e-commerce with cardnumber")) return false;
            if (!(xmlStatus.StatusMessage.Equals("false"))) return false;
            return true;
        }

        #endregion
    }
}
