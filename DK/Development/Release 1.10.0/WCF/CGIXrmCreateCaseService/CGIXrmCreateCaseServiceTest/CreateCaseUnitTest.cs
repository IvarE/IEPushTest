using System;
using System.Configuration;
using CGIXrmCreateCaseService.Case.Models;
using CGIXrmWin;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace CGIXrmCreateCaseService.Test
{
    [TestClass]
    public class CreateCaseUnitTest
    {
        #region Declarations ------------------------------------------------------------------------------------------

        private readonly CreateCase _createCase;
        private readonly XrmManager _manager;

        #endregion

        #region Test Methods ------------------------------------------------------------------------------------------

        public CreateCaseUnitTest()
        {
            _createCase = new CreateCase();
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _manager = new XrmManager(serverAdress, domain, username, password);

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

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// Method acquiring a Travel Card Entity with corresponding Contact from CRM.
        /// </summary>
        /// <returns>RGOL Case Model</returns>
        private AutoRgCaseRequest GetTravelCardWithCorrespondingContact(CustomerType customerType)
        {
            var query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns(
                "cgi_travelcardid",
                "cgi_travelcardnumber",
                "createdon",
                "cgi_contactid");
            query.LinkEntities.Add(new LinkEntity("cgi_travelcard",
                "contact",
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
                "address2_county");
            query.LinkEntities[0].EntityAlias = "contact";
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("contact", "emailaddress1", ConditionOperator.NotNull);
            query.Criteria.AddCondition("contact", "cgi_socialsecuritynumber", ConditionOperator.NotNull);

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
                EmailAddress = GetAttributeValue(travelCard, "contact.emailaddress1")
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


        private enum CustomerType
        {
            Private = 0,
            Company = 1
        }

        #endregion
    }
}
