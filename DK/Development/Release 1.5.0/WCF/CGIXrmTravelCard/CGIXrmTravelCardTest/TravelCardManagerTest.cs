using System.Configuration;
using System.Linq;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

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
        public void GetGetOutstandingCharges()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var outstandingCharges = _travelCard.GetGetOutstandingCharges(firstCardNumber);
            
            Assert.IsNotNull(outstandingCharges);
            Assert.AreEqual(outstandingCharges.ErrorMessage, "");
        }

        [TestMethod]
        public void RechargeCard()
        {
            var firstCardNumber = GetFirstCardNumber();
            Assert.IsNotNull(firstCardNumber);
            var rechargeCard = _travelCard.RechargeCard(firstCardNumber);
            Assert.IsNotNull(rechargeCard);
            Assert.AreEqual(rechargeCard.ErrorMessage, "");
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
            //var travelcardtransactionid = GetFirstCardTransactionId();
            //Assert.IsNotNull(travelcardtransactionid);
            //var travelCardTransactions = _travelCard.GetTravelCardTransactions(travelcardtransactionid);
            //Assert.IsNotNull(travelCardTransactions);
            //Assert.IsNotNull(travelCardTransactions.TravelCardTransactions);
            //Assert.IsTrue(travelCardTransactions.TravelCardTransactions.Any());
            //Assert.AreEqual(travelCardTransactions.ErrorMessage, "");
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

        private string GetFirstCardTransactionId()
        {
            //Leta fram ett giltigt transaktionsid
            var query = new QueryExpression("cgi_travelcardtransaction");
            query.ColumnSet.AddColumns("cgi_travelcardtransactionid");

            var results = _manager.Service.RetrieveMultiple(query);
            var firstCardEntity = results.Entities.FirstOrDefault();
            if (firstCardEntity == null)
            {
                return null;
            }
            return firstCardEntity.Attributes["cgi_travelcardtransactionid"].ToString();
        }

        #endregion
    }
}
