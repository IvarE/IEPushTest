using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CGIXrmEAIConnectorService.Shared.Models;
using Microsoft.Xrm.Sdk;
using SharedUnitTests;

namespace CGIXrmEAIConnectorServiceTest
{
    [TestClass]
    public class IEAIConnectorServiceManagerTest
    {
        readonly CGIXrmEAIConnectorService.AllBinary.EAIConnectorService _service;
        

        /// <summary>
        /// Constructor.
        /// </summary>
        public IEAIConnectorServiceManagerTest()
        {
            _service = new CGIXrmEAIConnectorService.AllBinary.EAIConnectorService();

        }


        /// <summary>
        /// Get Customer Entity details.
        /// </summary>
        [TestMethod]
        public void GetCustomerDetails()
        {
            //Test 1
            //Customer Type: Company
            //Authentication: Administrator
            //Status: Not found
            #region Test1

            Guid? customerGuid = new Guid();
            var type = AccountCategoryCode.Company;
            var login = SharedTest.ValidateLogin(@"D1\CrmAdmin");

            var response = _service.GetCustomerDetails(
                customerGuid.Value,
                type,
                login
                );
            Assert.IsNotNull(response);
            Assert.IsNull(response.Customer);
            Assert.IsTrue(response.Status == ProcessingStatus.SUCCESS);
            #endregion

            //Test 2
            //Customer Type: Private
            //Authentication: Administrator
            //Status: Found

            #region Test2

            customerGuid = SharedTest.GetFirstEntityId("contact", "contactid");
            Assert.IsNotNull(customerGuid);
            
            type = AccountCategoryCode.Private;
            login = SharedTest.ValidateLogin(@"D1\CrmAdmin");
            response = _service.GetCustomerDetails(
                customerGuid.Value,
                type,
                login
                );
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Customer);
            Assert.IsTrue(response.Status == ProcessingStatus.SUCCESS);

            #endregion

            //Test 3
            //Customer Type: Company
            //Authentication: Administrator
            //Status: Found
            #region Test3

            customerGuid = SharedTest.GetFirstEntityId("account", "accountid");
            Assert.IsNotNull(customerGuid);

            type = AccountCategoryCode.Company;
            login = SharedTest.ValidateLogin(@"D1\CrmAdmin");
            response = _service.GetCustomerDetails(
                customerGuid.Value,
                type,
                login
                );
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Customer);
            Assert.IsTrue(response.Status == ProcessingStatus.SUCCESS);

            #endregion
        }


        private string[] GetTravelCardNumber(DataCollection<Entity> entities)
        {
            return entities.Select(entity => entity["cgi_travelcardnumber"].ToString()).ToArray();
        }


        [TestMethod]
        public void GetTravelCardDetails()
        {
            //Test 1
            //Authentication: Administrator
            var travelCards = GetTravelCardNumber(SharedTest.GetMultipleEntityId("cgi_travelcard", new []{"cgi_travelcardnumber"}, 10));
            var login = SharedTest.ValidateLogin(@"D1\CrmAdmin");

            var response = _service.GetCustomerIdForTravelCard(
                travelCards,
                login
                );
            Assert.IsNotNull(response);
            Assert.AreNotEqual(0, response.Details.Count);
        }
    }
}
