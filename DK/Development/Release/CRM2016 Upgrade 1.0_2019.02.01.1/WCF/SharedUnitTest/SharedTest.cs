using System;
using System.Configuration;
using System.Linq;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using CGICRMPortalService.TravelCard.Models;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService.Customer;
using CGICRMPortalService.TravelCard;
using CGIXrmPortalServiceTest.Models;

namespace SharedUnitTests
{
    public static class SharedTest
    {
        private static readonly XrmManager _manager = GetXrmManagerFromAppSettings(Guid.Empty);
        private static readonly CustomerManager _customerManager = new CustomerManager(Guid.Empty);
        private static readonly TravelCardManager _travelCardManager = new TravelCardManager(Guid.Empty);
        public static string UnitTestCreateCustomerEmailAddress { get { return "UnitTestEmail@cgi.com"; } }
        public static string UnitTestUpdateCustomerEmailAddress { get { return "UnitTestEmail2@cgi.com"; } }
        private static readonly string unitTestTravelCardNumber = "1111111111";
        /// <summary>
        /// Validate the security code given against the user in CRM
        /// </summary>
        /// <param name="username">CRM User Name</param>
        /// <returns>CRM User Guid</returns>
        public static Guid ValidateLogin(string username)
        {
            //Get user details
            string fetchXmlQuery = string.Format(@"<fetch mapping='logical'>
              <entity name='systemuser'>
                <attribute name='fullname' />
                <attribute name='businessunitid' />
                <attribute name='title' />
                <attribute name='address1_telephone1' />
                <attribute name='systemuserid' />
                <attribute name='domainname' />
                <attribute name='cgi_attestid' />
                <order attribute='fullname' descending='false' />
                <filter type='and'>
                  <condition attribute='domainname' operator='eq' value='{0}' />
                </filter>
              </entity>
            </fetch>", username);

            var users = _manager.Service.RetrieveMultiple(new FetchExpression(fetchXmlQuery));


            if (users != null && users.Entities != null && users.Entities.Count > 0)
            {
                var userEntity = users.Entities[0];

                return userEntity.Id;
            }

            return Guid.Empty;
        }


        /// <summary>
        /// Creates an instance towards CRM based on given parameters.
        /// </summary>
        /// <param name="callerId">CRM User Id</param>
        /// <returns></returns>
        public static XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {
                var crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var username = ConfigurationManager.AppSettings["Username"];
                var password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                var xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                if (callerId != Guid.Empty)
                    ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                return xrmManager;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        public static Guid? GetFirstEntityId(string entityName, string entityIdName)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet.AddColumns(entityIdName);
            query.TopCount = 1;
            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities[0].Id;
            }
            return null;
        }

        public static DataCollection<Entity> GetMultipleEntityId(string entityName, string[] columnNames, int count = 0)
        {
            var query = new QueryExpression(entityName);
            foreach (string column in columnNames)
            {
                query.ColumnSet.AddColumns(column);
            }
            if (count > 0)
            {
                query.TopCount = count;
            }
            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities;
            }
            return null;
        }

        public static TravelCardResponseTest TestNewTravelCard(AccountCategoryCode customerAccountCategoryCode, AccountCategoryCode travelCardAccountCategoryCode, bool deleteAfterTest)
        {
            ClearCustomers(customerAccountCategoryCode);
            //Create new private customer
            Customer customer = getUnitTestCustomer(customerAccountCategoryCode);
            CreateCustomerResponse response = _customerManager.CreateCustomer(customer);
            //Create travel card for private person
            TravelCard travelCard = CreateUnitTestTravelCard(response.AccountId, travelCardAccountCategoryCode);
            RegisterTravelCardResponse travelCardResponse = _travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            TravelCardResponseTest travelCardResponseTest = new TravelCardResponseTest();
            travelCardResponseTest.AccountId = response.AccountId;
            travelCardResponseTest.Status = travelCardResponse.Status;
            //Delete customer & travel card
            if (deleteAfterTest)
            {
                if (travelCardResponse.Status == ProcessingStatus.SUCCESS)
                {
                    DeleteTravelCardEntity(GetUnitTestTravelCard());
                }
                DeleteCustomerEntity(response.AccountId, customerAccountCategoryCode);
            }
            else
            {
                travelCardResponseTest.AccountId = travelCard.AccountId;
            }
            return travelCardResponseTest;
        }

        public static TravelCard CreateUnitTestTravelCard(Guid accountId, AccountCategoryCode accountCategoryCode)
        {
            TravelCard travelCard = new TravelCard();
            travelCard.AccountId = accountId;
            travelCard.AutoloadConnectionDate = DateTime.Today.ToUniversalTime();
            travelCard.AutoloadStatus = 1;
            travelCard.Blocked = false;
            travelCard.CardCategory = 1;
            travelCard.CardName = "TravelCardUnitTest";
            travelCard.CreditCardMask = "123";
            travelCard.Currency = "1";
            travelCard.CustomerType = accountCategoryCode;
            travelCard.FailedAttemptsToChargeMoney = 1;
            travelCard.LatestAutoloadAmount = 1;
            travelCard.LatestChargeDate = DateTime.Today.ToUniversalTime();
            travelCard.LatestFailedAttempt = DateTime.Today.ToUniversalTime();
            travelCard.PeriodCardTypeId = 1;
            travelCard.PeriodCardTypeTitle = "TravelCardUnitTest";
            travelCard.PeriodValidFrom = DateTime.Today.AddDays(-1).ToUniversalTime();
            travelCard.PeriodValidTo = DateTime.Today.ToUniversalTime();
            travelCard.ValueCardTypeId = 1;
            travelCard.ValueCardTypeTitle = "TravelCardUnitTest";
            travelCard.VerifyId = "1";
            travelCard.CardNumber = unitTestTravelCardNumber;
            return travelCard;
        }

        public static bool DeleteTravelCardEntity(Guid id)
        {
            Entity entityId;
            entityId = new Entity("cgi_travelcard");
            _manager.Service.Delete(entityId.LogicalName, id);
            return true;
        }

        public static void ClearCustomers(AccountCategoryCode accountCategoryCode)
        {
            //Delete any existing customer with test address
            var unitTestEntities = GetCustomerEntities(accountCategoryCode, UnitTestCreateCustomerEmailAddress);
            if (unitTestEntities != null && unitTestEntities.Count > 0)
            {
                foreach (Entity unitTestEntity in unitTestEntities)
                {
                    DeleteCustomerEntity(unitTestEntity.Id, accountCategoryCode);
                }
            }
        }

        public static Guid GetTravelCardIdFromAccountId(Guid accountId)
        {
            QueryExpression query;

            query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns("cgi_travelcardid");
            query.Criteria = new FilterExpression();
            FilterExpression filter = query.Criteria.AddFilter(LogicalOperator.And);
            filter.Conditions.Add(new ConditionExpression("cgi_accountid", ConditionOperator.Equal, accountId));

            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return new Guid(results.Entities[0].Attributes["cgi_travelcardid"].ToString());
            }
            return Guid.Empty;
        }

        public static Guid GetUnitTestTravelCard()
        {
            QueryExpression query;
            query = new QueryExpression("cgi_travelcard");
            query.ColumnSet.AddColumns("cgi_travelcardid");
            query.Criteria = new FilterExpression();
            FilterExpression filter = query.Criteria.AddFilter(LogicalOperator.And);
            filter.Conditions.Add(new ConditionExpression("cgi_travelcardnumber", ConditionOperator.Equal, unitTestTravelCardNumber));

            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return new Guid(results.Entities[0].Attributes["cgi_travelcardid"].ToString());
            }
            return Guid.Empty;
        }

        public static DataCollection<Entity> GetCustomerEntities(AccountCategoryCode type, string emailAddress)
        {
            QueryExpression query;

            //Return first private person
            if (type == AccountCategoryCode.Private)
            {
                query = new QueryExpression("contact");
                query.ColumnSet.AddColumns("contactid");
                query.ColumnSet.AddColumns("emailaddress1");
            }
            //Return first organization
            else if (type == AccountCategoryCode.Company)
            {
                query = new QueryExpression("account");
                query.ColumnSet.AddColumns("accountid");
                query.ColumnSet.AddColumns("emailaddress1");
            }
            else
            {
                return null;
            }

            //Add filter criteria
            query.Criteria = new FilterExpression();
            FilterExpression filter = query.Criteria.AddFilter(LogicalOperator.And);
            filter.Conditions.Add(new ConditionExpression("emailaddress1", ConditionOperator.Equal, emailAddress));

            if (type == AccountCategoryCode.Private)
            {
                filter.Conditions.Add(new ConditionExpression("contactid", ConditionOperator.NotNull));
            }
            else if (type == AccountCategoryCode.Company)
            {
                filter.Conditions.Add(new ConditionExpression("accountid", ConditionOperator.NotNull));
            }

            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities;
            }
            return null;
        }

        public static bool DeleteCustomerEntity(Guid id, AccountCategoryCode type)
        {
            Entity entityId;
            if (type == AccountCategoryCode.Private)
            {
                entityId = new Entity("contact");
            }
            else if (type == AccountCategoryCode.Company)
            {
                entityId = new Entity("account");
            }
            else
            {
                return false;
            }
            _manager.Service.Delete(entityId.LogicalName, id);
            return true;
        }

        public static Entity GetCustomerEntity(AccountCategoryCode type, Guid customerId)
        {
            QueryExpression query;

            //Return first private person
            if (type == AccountCategoryCode.Private)
            {
                query = new QueryExpression("contact");
                query.ColumnSet.AddColumns("contactid");
                query.ColumnSet.AddColumns("emailaddress1");
            }
            //Return first organization
            else if (type == AccountCategoryCode.Company)
            {
                query = new QueryExpression("account");
                query.ColumnSet.AddColumns("accountid");
                query.ColumnSet.AddColumns("emailaddress1");
            }
            else
            {
                return null;
            }
            query.ColumnSet.AddColumn("cgi_myaccount_lastlogin");

            //Add filter criteria
            query.Criteria = new FilterExpression();
            FilterExpression filter = query.Criteria.AddFilter(LogicalOperator.And);
            filter.Conditions.Add(new ConditionExpression("emailaddress1", ConditionOperator.NotNull));
            if (customerId == Guid.Empty)
            {
                if (type == AccountCategoryCode.Private)
                {
                    filter.Conditions.Add(new ConditionExpression("contactid", ConditionOperator.NotNull));
                }
                else if (type == AccountCategoryCode.Company)
                {
                    filter.Conditions.Add(new ConditionExpression("accountid", ConditionOperator.NotNull));
                }
            }
            else
            {
                if (type == AccountCategoryCode.Private)
                {
                    filter.Conditions.Add(new ConditionExpression("contactid", ConditionOperator.Equal, customerId));
                }
                else if (type == AccountCategoryCode.Company)
                {
                    filter.Conditions.Add(new ConditionExpression("accountid", ConditionOperator.Equal, customerId));
                }
            }

            query.TopCount = 1;
            var results = _manager.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities[0];
            }
            return null;
        }

        public static Customer getUnitTestCustomer(AccountCategoryCode customerType)
        {
            //Create a private customer
            return new Customer
            {
                CustomerType = customerType,
                AccountFirstName = "UnitTestFirstName",
                AccountLastName = "UnitTestLastName",
                //Phone = "0700000000", Add later
                //MobilePhone = "0700000001", Add later
                Email = UnitTestCreateCustomerEmailAddress,
                AllowAutoLoad = true,
                MaxCardsAutoLoad = 1,
                SocialSecurtiyNumber = "197602228393",
                OrganizationCreditApproved = true,
                Deleted = false,
                InActive = false,
                HasEpiserverAccount = false,

                Addresses = new[]
                {
                    new Address()
                    {
                        CompanyName = "UnitTestCompanyName",
                        CareOff = "UnitTestCareOff",
                        Street = "UnitTestStreet",
                        PostalCode = "UnitTestPostalCode",
                        City = "UnitTestCity",
                        County = "UnitTestCounty",
                        Country = "UnitTestCountry",
                        ContactPerson = "UnitTestPerson",
                        ContactPhoneNumber = "0700000002",
                        SMSNotificationNumber = "0700000003",
                        AddressType = AddressTypeCode.Invoice
                    }
                }
            };
        }
    }
}
