using System;
using System.Configuration;
using CGIXrmCreateCaseService.Case.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Xrm.Sdk;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService;

namespace CGIXrmCreateCaseService.Test
{
    [TestClass]
    public class CreateCaseUnitTest
    {
        #region Declarations ------------------------------------------------------------------------------------------

        private readonly CreateCase _createCase;
        private readonly XrmManager _manager;
        private string _CRMPlusAddress;
        #endregion

        #region Test Methods ------------------------------------------------------------------------------------------

        public CreateCaseUnitTest()
        {
            _createCase = new CreateCase();
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];
            _CRMPlusAddress = ConfigurationManager.AppSettings["CRMPlusAddress"];

            _manager = new XrmManager(serverAdress, domain, username, password);

        }

        /// <summary>
        /// Test Method creating an RGOL Case.
        /// </summary>
        [TestMethod]
        public void CreateSkrivTillOssUnitTest()
        {
            //var autoRgCase = GetTravelCardWithCorrespondingContact(CustomerType.Private);

            var manualCase = new CreateCaseRequest
            {
                CustomersCategory = "Skador och vandalisering",
                CustomersSubcategory = null,
                Title = "test title",
                Description = "fågelkvitter. Fanns inte innan.",
                Customer = null,
                CustomerType = Case.Models.CustomerType.Private

            };

            _createCase.RequestCreateCase(manualCase);

        }

        /// <summary>
        /// Test Method Get or create contact.
        /// </summary>
        [TestMethod]
        public void GetOrCreateContactFromFasad()
        {
            var manualRGCase = new AutoRgCaseRequest
            {
                Address_City = "",
                Address_Country = "",
                Address_Line1 = null,
                Address_Line2 = "line2",
                Address_PostalCode = "12345",
                CustomerAddress1City = "Broby",
                CustomerAddress1Country = "SE",
                CustomerAddress1Line1 = null,
                CustomerAddress1Line2 = "gataline2",
                CustomerAddress1Postalcode = "12345",
                CustomerFirstName = "fname",
                CustomerLastName = "lnamne",
                CustomerSocialSecurityNumber = "181001018751",
                CustomerTelephonenumber = "0708513515",
                CustomerType = 0,
                CustomersCategory = "RGOL",
                CustomersSubcategory = "RGOL",
                DeliveryEmailAddress = "hans.ekelof@cgi.com",
                DepartureDateTime = DateTime.Now,
                Description = "En jättelång text",
                EmailAddress = "hans.ekelof@cgi.com",
                ExperiencedDelay = "20-39 min",
                FirstName = "Given_Nameakt_Bet311",
                LastName = "Efternamnakt_Bet311",
                MobileNo = "0708513515",
                RGOLIssueID = "RG2017-01-10#3",
                SocialSecurityNumber = "181001018751",
                TaxiClaimedAmount = 0,
                TicketType1 = "Pappersbiljett",
                Title = "2017-01-27: Öresundståg Malmö C - Lund C[1008]",
            };

            Entities.SettingsEntity settings = new Entities.SettingsEntity
            {
                ed_CRMPlusService = _CRMPlusAddress
            };

#if DEBUG
            manualRGCase.CustomerAddress1Country = "IC";
#endif

            Case.CreateCaseManager caseMgr = new Case.CreateCaseManager();
            Guid contactId = caseMgr.GetOrCreateContactRGOLCase(manualRGCase, settings);

            Assert.IsNotNull(contactId);
        }


        /// <summary>
        /// Test Method creating an RGOL Case.
        /// </summary>
        [TestMethod]
        public void CreateMANUALResegarantiCaseUnitTest()
        {
            //var autoRgCase = GetTravelCardWithCorrespondingContact(CustomerType.Private);

            var manualRGCase = new AutoRgCaseRequest
            {
                Address_City = "",
                Address_Country = "",
                Address_Line1 = null,
                Address_Line2 = "line2",
                Address_PostalCode = "12345",
                CustomerAddress1City = "Broby",
                CustomerAddress1Country = "SE",
                CustomerAddress1Line1 = null,
                CustomerAddress1Line2 = "gataline2",
                CustomerAddress1Postalcode = "12345",
                CustomerFirstName = "fname",
                CustomerLastName = "lnamne",
                CustomerSocialSecurityNumber = "181001018751",
                CustomerTelephonenumber = "0708513515",
                CustomerType = 0,
                CustomersCategory = "RGOL",
                CustomersSubcategory = "RGOL",
                DeliveryEmailAddress = "hans.ekelof@cgi.com",
                DepartureDateTime = DateTime.Now,
                Description = "En jättelång text",
                EmailAddress = "hans.ekelof@cgi.com",
                ExperiencedDelay = "20-39 min",
                FirstName = "Given_Nameakt_Bet311",
                LastName = "Efternamnakt_Bet311",
                MobileNo = "0708513515",
                RGOLIssueID = "RG2017-01-10#3",
                SocialSecurityNumber = "181001018751",
                TaxiClaimedAmount = 0,
                TicketType1 = "Pappersbiljett",
                Title = "2017-01-27: Öresundståg Malmö C - Lund C[1008]",
            };

            var result = _createCase.RequestCreateAutoRGCase(manualRGCase);

            Assert.IsNotNull(result);

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                throw new Exception($"Error catched:{result.ErrorMessage}");

            //Succeeded in retrieving a Case Number
            Assert.AreNotEqual(result.CaseNumber, string.Empty);
            //Created case has an ID in CRM
            Assert.AreNotEqual(result.CaseId, Guid.Empty);

        }

        /// <summary>
        /// Test Method creating an RGOL Case.
        /// </summary>
        [TestMethod]
        public void CreateMANUALResegarantiCaseUnitTestNoSwedishSocialSecurityNo()
        {
            //var autoRgCase = GetTravelCardWithCorrespondingContact(CustomerType.Private);

            string sRGOLIssueID = "RG2017-04-" + Random4Digits();

            var manualRGCase = new AutoRgCaseRequest
            {
                Address_City = "",
                Address_Country = "",
                Address_Line1 = null,
                Address_Line2 = "line2",
                Address_PostalCode = "12345",
                CustomerAddress1City = "Broby",
                CustomerAddress1Country = "SE",
                CustomerAddress1Line1 = null,
                CustomerAddress1Line2 = "gataline2",
                CustomerAddress1Postalcode = "12345",
                CustomerFirstName = "fname",
                CustomerLastName = "lnamne",
                CustomerSocialSecurityNumber = "18100101",
                CustomerTelephonenumber = "0708513515",
                CustomerType = 0,
                CustomersCategory = "RGOL",
                CustomersSubcategory = "RGOL",
                DeliveryEmailAddress = "hans.ekelof@cgi.com",
                DepartureDateTime = DateTime.Now,
                Description = "En jättelång text",
                EmailAddress = "hans.ekelof@cgi.com",
                ExperiencedDelay = "20-39 min",
                FirstName = "Given_Nameakt_Bet311",
                LastName = "Efternamnakt_Bet311",
                MobileNo = "0708513515",
                RGOLIssueID = sRGOLIssueID,
                SocialSecurityNumber = "181001018751",
                TaxiClaimedAmount = 0,
                TicketType1 = "Pappersbiljett",
                Title = "2017-01-27: Öresundståg Malmö C - Lund C[1008]",
            };

            var result = _createCase.RequestCreateAutoRGCase(manualRGCase);

            Assert.IsNotNull(result);
            //Succeeded in retrieving a Case Number
            Assert.AreNotEqual(result.CaseNumber, string.Empty);
            //Created case has an ID in CRM
            Assert.AreNotEqual(result.CaseId, Guid.Empty);

        }

        /// <summary>
        /// Test Method creating an RGOL Case.
        /// </summary>
        [TestMethod]
        public void CreateAutoResegarantiCaseUnitTest()
        {
            var autoRgCase = GetTravelCardWithCorrespondingContact(CustomerType.Private);
            var result = _createCase.RequestCreateAutoRGCase(autoRgCase);

            Assert.IsNotNull(result);
            //Succeeded in retrieving a Case Number
            Assert.AreNotEqual(result.CaseNumber, string.Empty);
            //Created case has an ID in CRM
            Assert.AreNotEqual(result.CaseId, Guid.Empty);

            autoRgCase = GetTravelCardWithCorrespondingContact(CustomerType.Company);
            result = _createCase.RequestCreateAutoRGCase(autoRgCase);

            Assert.IsNotNull(result);
            //Succeeded in retrieving a Case Number
            Assert.AreNotEqual(result.CaseNumber, string.Empty);
            //Created case has an ID in CRM
            Assert.AreNotEqual(result.CaseId, Guid.Empty);
        }


        /// <summary>
        /// Test Method updating an existing RGOL Case.
        /// </summary>
        [TestMethod]
        public void UpdateAutoResegarantiCaseBiffUnitTest()
        {
            var autoRgCaseBiff = GetTravelcardTransaction();

            var result = _createCase.RequestUpdateAutoRGCaseBiffTransactions(autoRgCaseBiff);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, string.Empty);
        }

        [TestMethod]
        public void UpdateCaseWithRefundDecision()
        {
            CreateSkrivTillOssUnitTest();

        }
        

#endregion

#region Private Methods ---------------------------------------------------------------------------------------

        private void DeleteCase(string title)
        {

        }

        /// <summary>
        /// Method acquiring a Travel Card Entity with corresponding Contact from CRM.
        /// </summary>
        /// <returns>RGOL Case Model</returns>
        private AutoRgCaseRequest GetTravelCardWithCorrespondingContact(CustomerType customerType)
        {
            var query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns( "cgi_travelcardid",
                                        "cgi_travelcardnumber",
                                        "createdon",
                                        "cgi_contactid");
            query.LinkEntities.Add(new LinkEntity("cgi_travelcard", "contact",
                                                                    "cgi_contactid",
                                                                    "contactid", JoinOperator.Inner));
            query.LinkEntities[0].Columns.AddColumns("firstname",
                                                    "lastname",
                                                    "emailaddress1",
                                                    "cgi_contactnumber",
                                                    "address1_postalcode",
                                                    "address1_line1",
                                                    "address1_city",
                                                    "cgi_socialsecuritynumber",
                                                    "address1_name",
                                                    "address1_line2",
                                                    "address2_county",
                                                    "telephone2");
            query.LinkEntities[0].EntityAlias = "contact";
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("contact", "emailaddress1", ConditionOperator.NotNull);
            query.Criteria.AddCondition("contact", "cgi_socialsecuritynumber", ConditionOperator.NotNull);
            query.Criteria.AddCondition("contact", "emailaddress1", ConditionOperator.NotNull);
            query.Criteria.AddCondition("contact", "telephone2", ConditionOperator.NotNull);            // Must have mobile.

            var travelCards = _manager.Service.RetrieveMultiple(query);
            var travelCard = (from n in travelCards.Entities
                              select n).FirstOrDefault();

            if (travelCard == null) return null;

            var caseReq = new AutoRgCaseRequest
            {
                CustomerId = ((EntityReference)travelCard.Attributes["cgi_contactid"]).Id.ToString(),
                CustomerFirstName = GetAttributeValue(travelCard, "contact.firstname"),
                CustomerLastName = GetAttributeValue(travelCard, "contact.lastname"),
                CardNumber = travelCard.Attributes["cgi_travelcardnumber"].ToString(),
                CustomerAddress1Line2 = GetAttributeValue(travelCard, "contact.address1_line2"),
                CustomerAddress1Postalcode = GetAttributeValue(travelCard, "contact.address1_postalcode"),
                CustomerAddress1City = GetAttributeValue(travelCard, "contact.address1_city"),
                CustomerSocialSecurityNumber = GetAttributeValue(travelCard, "contact.cgi_socialsecuritynumber"),
                CustomerType = (int)customerType,
                MobileNo = GetAttributeValue(travelCard, "contact.telephone2"),
                EmailAddress = GetAttributeValue(travelCard, "contact.emailaddress1"),
                RGOLIssueID = "R-" + DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString()
            };

            return caseReq;
        }


        private UpdateAutoRgCaseBiffTransactionsRequest GetTravelcardTransaction()
        {
            var query = new QueryExpression("cgi_travelcardtransaction");
            query.ColumnSet.AddColumns(
                "cgi_travelcardtransactionid",
                "cgi_travelcardtransaction",
                "createdon",
                "cgi_travelcard",
                "cgi_travelcardid",
                "cgi_time",
                "cgi_route",
                "cgi_rectype",
                "cgi_origzonename",
                "cgi_origzone",
                "cgi_deviceid",
                "cgi_date",
                "cgi_caseid",
                "cgi_cardsect",
                "cgi_amount",
                "cgi_txntype");
            query.LinkEntities.Add(new LinkEntity("cgi_travelcardtransaction",
                "incident",
                "cgi_caseid",
                "incidentid", JoinOperator.Inner));
            query.LinkEntities[0].Columns.AddColumns("incidentid", "cgi_travelcardid");
            query.LinkEntities[0].EntityAlias = "incident";

            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("incident", "ticketnumber", ConditionOperator.NotNull);
            query.Criteria.AddCondition("cgi_travelcardtransaction", "cgi_amount", ConditionOperator.NotNull);
            query.Criteria.AddCondition("cgi_travelcardtransaction", "cgi_travelcardtransaction", ConditionOperator.NotNull);


            var travelCardTransactions = _manager.Service.RetrieveMultiple(query);
            var travelCardTransaction = (from transactions in travelCardTransactions.Entities
                                         select transactions).FirstOrDefault();

            if (travelCardTransaction == null) return null;

            var biffTransaction = new BiffTransaction
            {
                Amount = GetEntityAttributeValue(travelCardTransaction, "cgi_amount"),
                Cardsect = GetEntityAttributeValue(travelCardTransaction, "cgi_cardsect"),
                Date = GetEntityAttributeValue(travelCardTransaction, "cgi_date"),
                Deviceid = GetEntityAttributeValue(travelCardTransaction, "cgi_deviceid"),
                Origzone = GetEntityAttributeValue(travelCardTransaction, "cgi_origzone"),
                Origzonename = GetEntityAttributeValue(travelCardTransaction, "cgi_origzonename"),
                Rectype = GetEntityAttributeValue(travelCardTransaction, "cgi_rectype"),
                Route = GetEntityAttributeValue(travelCardTransaction, "cgi_route"),
                Time = GetEntityAttributeValue(travelCardTransaction, "cgi_time"),
                Travelcard = GetEntityAttributeValue(travelCardTransaction, "cgi_travelcard"),
                Txntype = GetEntityAttributeValue(travelCardTransaction, "cgi_txntype"),
                Travelcardtransaction = GetEntityAttributeValue(travelCardTransaction, "cgi_travelcardtransaction")
            };

            var cardTransaction = new UpdateAutoRgCaseBiffTransactionsRequest
            {
                CaseID = ((EntityReference)travelCardTransaction.Attributes["cgi_caseid"]).Id,
                BiffTransactions = new[] {biffTransaction}
            };




            return cardTransaction;

        }


        /// <summary>
        /// Method retrieving values from Entity attributes based on given parameters.
        /// </summary>
        /// <param name="entity">Entity Data</param>
        /// <param name="attribute">Attribute Name</param>
        /// <returns>Attribute Value</returns>
        private static string GetAttributeValue(Entity entity, string attribute)
        {
            if (string.IsNullOrEmpty(attribute)) return string.Empty;

            var value = entity[attribute] as AliasedValue;

            return value?.Value.ToString() ?? entity[attribute].ToString();
        }


        /// <summary>
        /// Method acquiring an Xrm entity's attribute value by key name.
        /// </summary>
        /// <param name="entity">Xrm Entity</param>
        /// <param name="columnName">Attribute Key Name</param>
        /// <returns></returns>
        private string GetEntityAttributeValue(Entity entity, string columnName)
        {
            foreach (var column in entity.Attributes.Where(column => column.Key == columnName))
            {
                return column.Value.ToString();
            }

            return string.Empty;
        }

        private static string Random4Digits()
        {
            Random rnd = new Random();
            int next = rnd.Next(0, 10000);
            return (next + 10000).ToString().Substring(1);
        }

        private enum CustomerType
        {
            Private = 0,
            Company = 1
        }

#endregion
    }
}
