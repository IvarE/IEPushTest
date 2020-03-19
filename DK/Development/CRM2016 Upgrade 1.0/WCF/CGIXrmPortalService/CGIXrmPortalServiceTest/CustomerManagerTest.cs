using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using CGICRMPortalService.Shared.Models;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Query;
using SharedUnitTests;
using CGICRMPortalService;
using CGICRMPortalService.Customer;
using CGICRMPortalService.Customer.Models;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using System.Collections.Generic;

namespace CGIXrmPortalServiceTest
{
    [TestClass]
    public class CustomerManagerTest
    {
        #region Declarations ------------------------------------------------------------------------------------------
        private readonly XrmManager _manager;
        private readonly CustomerManager _customerManager = new CustomerManager(Guid.Empty);
        public string UnitTestCreateCustomerEmailAddress { get { return "UnitTestEmail@cgi.com"; } }
        public string UnitTestUpdateCustomerEmailAddress { get { return "UnitTestEmail2@cgi.com"; } }
        #endregion

        #region Test Methods ------------------------------------------------------------------------------------------

        public CustomerManagerTest()
        {
            //Logga in mot CRM
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _manager = new XrmManager(serverAdress, domain, username, password);
        }

        [TestMethod]
        public void GetAccountNumber()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            //UnitTest get account number for private customer
            #region UnitTest get account number for private customer
            Guid? customerId = SharedTest.GetFirstEntityId("contact", "contactid");

            Assert.IsNotNull(customerId);
            Assert.IsNotNull(customerId.Value);
            var accountNumber = _customerManager.GetAccountNumber(customerId.Value, AccountCategoryCode.Private);
            Assert.IsNotNull(accountNumber);
            Assert.IsTrue(accountNumber.Length > 0);
            #endregion

            //UnitTest get account number for organization
            #region UnitTest get account number for organization
            customerId = SharedTest.GetFirstEntityId("account", "accountid");

            Assert.IsNotNull(customerId);
            Assert.IsNotNull(customerId.Value);
            accountNumber = _customerManager.GetAccountNumber(customerId.Value, AccountCategoryCode.Company);
            Assert.IsNotNull(accountNumber);
            Assert.IsTrue(accountNumber.Length > 0);
            #endregion

            //UnitTest get account number for empty guid (private customer)
            #region UnitTest get account number for empty guid (private customer)
            AssertFunc.Throws<NullReferenceException>(() => _customerManager.GetAccountNumber(Guid.Empty, AccountCategoryCode.Private));
            #endregion

            //UnitTest get account number for empty guid (organization)
            #region UnitTest get account number for empty guid (organization)
            AssertFunc.Throws<NullReferenceException>(() => _customerManager.GetAccountNumber(Guid.Empty, AccountCategoryCode.Company)); 
            #endregion
        }

        [TestMethod]
        public void CheckCustomerExist()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            Entity customerEntity;
            CheckCustomerExistResponse customerResponse;

            //UnitTest check private customer
            #region UnitTest check private customer
            customerEntity = SharedTest.GetCustomerEntity(AccountCategoryCode.Private, Guid.Empty);
            customerResponse = _customerManager.CheckCustomerExist(customerEntity.Attributes["emailaddress1"].ToString());
            Assert.IsNotNull(customerResponse);
            Assert.AreEqual(customerResponse.Status, ProcessingStatus.SUCCESS);
            Assert.IsNotNull(customerResponse.AccountId);
            Assert.IsNotNull(customerResponse.AccountNumber);
            Assert.AreNotEqual(customerResponse.AccountNumber, "");
            Assert.AreEqual(customerResponse.CustomerExists, true);
            Assert.AreEqual(customerResponse.CustomerType, AccountCategoryCode.Private);
            #endregion

            //UnitTest check organization
            #region UnitTest check organization
            customerEntity = SharedTest.GetCustomerEntity(AccountCategoryCode.Company, Guid.Empty);
            customerResponse = _customerManager.CheckCustomerExist(customerEntity.Attributes["emailaddress1"].ToString());
            Assert.IsNotNull(customerResponse);
            Assert.AreEqual(customerResponse.Status, ProcessingStatus.SUCCESS);
            Assert.IsNotNull(customerResponse.AccountId);
            Assert.IsNotNull(customerResponse.AccountNumber);
            Assert.AreNotEqual(customerResponse.AccountNumber, "");
            Assert.AreEqual(customerResponse.CustomerExists, true);
            Assert.AreEqual(customerResponse.CustomerType, AccountCategoryCode.Company);
            #endregion

            //UnitTest check customer that does not exists
            #region Unit check customer that does not exists
            customerResponse = _customerManager.CheckCustomerExist("novalidemailaddress");
            Assert.IsNotNull(customerResponse);
            Assert.AreEqual(customerResponse.Status, ProcessingStatus.SUCCESS);
            Assert.AreEqual(customerResponse.AccountId, Guid.Empty);
            Assert.IsNull(customerResponse.AccountNumber);
            Assert.AreEqual(customerResponse.CustomerExists, false);
            Assert.AreEqual(customerResponse.Deleted, true);
            #endregion

            //UnitTest check null customer
            #region UnitTest check null customer
            customerResponse = _customerManager.CheckCustomerExist(null);
            Assert.IsNotNull(customerResponse);
            Assert.AreEqual(customerResponse.Status, ProcessingStatus.FAILED);
            Assert.AreEqual(customerResponse.AccountId, Guid.Empty);
            Assert.IsNull(customerResponse.AccountNumber);
            Assert.AreEqual(customerResponse.CustomerExists, false);
            Assert.AreEqual(customerResponse.Deleted, false);
            #endregion
        }

        [TestMethod]
        public void CreateCustomer()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            #region UnitTest private customer
            //Create a private customer
            Customer privateCustomer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Private);

            //Create and check response
            CreateCustomerResponse response = _customerManager.CreateCustomer(privateCustomer);
            Assert.IsNotNull(response.AccountId);
            Assert.IsNotNull(response.AccountNumber);
            Assert.AreNotEqual(response.AccountNumber, "");
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);

            //Check if output values conforms 
            GetCustomerResponse customerResponse = _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Private);

            Assert.IsTrue(checkCustomer(customerResponse.Customer, privateCustomer, AccountCategoryCode.Private));

            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, privateCustomer.CustomerType);
            #endregion

            #region UnitTest private customer minimum
            //Create a private customer
            privateCustomer = getUnitTestCustomerMinimum(AccountCategoryCode.Private);

            //Create and check response
            response = _customerManager.CreateCustomer(privateCustomer);
            Assert.IsNotNull(response.AccountId);
            Assert.IsNotNull(response.AccountNumber);
            Assert.AreNotEqual(response.AccountNumber, "");
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);

            //Check if output values conforms 
            customerResponse = _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Private);

            Assert.IsTrue(checkCustomerMinimum(customerResponse.Customer, privateCustomer, AccountCategoryCode.Private));

            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, privateCustomer.CustomerType);
            #endregion

            #region UnitTest organization customer
            //Create organization customer
            Customer organizationCustomer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Company);

            //Create and check response
            response = _customerManager.CreateCustomer(organizationCustomer);
            Assert.IsNotNull(response.AccountId);
            Assert.IsNotNull(response.AccountNumber);
            Assert.AreNotEqual(response.AccountNumber, "");
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);

            //Check if output values conforms 
            customerResponse = _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Company);

            Assert.IsTrue(checkCustomer(customerResponse.Customer, organizationCustomer, AccountCategoryCode.Company));

            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, organizationCustomer.CustomerType);
            #endregion

            #region UnitTest organization customer minimum
            //Create organization customer
            organizationCustomer = getUnitTestCustomerMinimum(AccountCategoryCode.Company);

            //Create and check response
            response = _customerManager.CreateCustomer(organizationCustomer);
            Assert.IsNotNull(response.AccountId);
            Assert.IsNotNull(response.AccountNumber);
            Assert.AreNotEqual(response.AccountNumber, "");
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);

            //Check if output values conforms 
            customerResponse = _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Company);

            Assert.IsTrue(checkCustomerMinimum(customerResponse.Customer, organizationCustomer, AccountCategoryCode.Company));

            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, organizationCustomer.CustomerType);
            #endregion

            #region UnitTest empty customer
            //Create new customer
            Customer customer = new Customer();

            //Create and check response
            response = _customerManager.CreateCustomer(customer);
            Assert.AreEqual(response.AccountId, Guid.Empty);
            Assert.IsNull(response.AccountNumber);
            Assert.AreEqual(response.Status, ProcessingStatus.FAILED);
            #endregion

            #region UnitTest null customer
            //Create and check response
            response = _customerManager.CreateCustomer(null);
            Assert.AreEqual(response.AccountId, Guid.Empty);
            Assert.IsNull(response.AccountNumber);
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);
            #endregion
        }

        [TestMethod]
        public void UpdateMyAccountLastLogin()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            DateTime myAccountLastLogin = new DateTime(2016, 04, 08).ToUniversalTime();

            //UnitTest private customer
            #region UnitTest private customer
            Guid customerId = SharedTest.GetCustomerEntity(AccountCategoryCode.Private, Guid.Empty).Id;
            UpdateCustomerResponse response = _customerManager.UpdateMyAccountLastLogin(customerId, false, myAccountLastLogin);
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);
            Entity customerEntity = SharedTest.GetCustomerEntity(AccountCategoryCode.Private, customerId);
            Assert.IsNotNull(customerEntity);
            Assert.AreEqual(customerEntity.Attributes["cgi_myaccount_lastlogin"], myAccountLastLogin);
            #endregion

            //UnitTest organization
            #region UnitTest organization
            customerId = SharedTest.GetCustomerEntity(AccountCategoryCode.Company, Guid.Empty).Id;
            response = _customerManager.UpdateMyAccountLastLogin(customerId, true, myAccountLastLogin);
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);
            customerEntity = SharedTest.GetCustomerEntity(AccountCategoryCode.Company, customerId);
            Assert.IsNotNull(customerEntity);
            Assert.AreEqual(customerEntity.Attributes["cgi_myaccount_lastlogin"], myAccountLastLogin);
            #endregion

            //UnitTest customer that does not exists
            #region UnitTest customer that does not exists
            customerId = SharedTest.GetCustomerEntity(AccountCategoryCode.Company, Guid.Empty).Id;
            response = _customerManager.UpdateMyAccountLastLogin(customerId, false, myAccountLastLogin);
            Assert.AreEqual(response.Status, ProcessingStatus.FAILED);
            #endregion

            //UnitTest Empty guid
            #region UnitTest Empty guid
            response = _customerManager.UpdateMyAccountLastLogin(Guid.Empty, false, myAccountLastLogin);
            Assert.AreEqual(response.Status, ProcessingStatus.FAILED);
            #endregion
        }

        [TestMethod]
        public void UpdateCustomer()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            //UnitTest update private customer
            #region UnitTest update private customer
            //Create a private customer
            Customer customer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Private);
            //Create updateCustomer (contains information on what values should be updated)
            Customer updateCustomer = getUnitTestCustomerUpdate(AccountCategoryCode.Private, false);
            //Create the private customer in CRM
            CreateCustomerResponse response = _customerManager.CreateCustomer(customer);
            //Get information of the new private customer in CRM
            Customer responseCustomer =
                _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Private).Customer;
            //Set accountId and address id to match the updated record
            updateCustomer.AccountId = responseCustomer.AccountId;
            updateCustomer.Addresses[0].AddressId = responseCustomer.Addresses[0].AddressId;
            //Update the customer
            UpdateCustomerResponse updateCustomerResponse = _customerManager.UpdateCustomer(responseCustomer.AccountId, updateCustomer);
            //Check if the function succeded
            Assert.AreEqual(updateCustomerResponse.Status, ProcessingStatus.SUCCESS);
            //Get the new updated record and see if all values has been updated in CRM
            responseCustomer =
                _customerManager.GetCustomer(updateCustomer.AccountId, AccountCategoryCode.Private).Customer;
            Assert.IsTrue(checkCustomer(responseCustomer, updateCustomer, AccountCategoryCode.Private));
            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, responseCustomer.CustomerType);
            #endregion

            //UnitTest delete private customer
            #region UnitTest delete private customer
            //Create a private customer
            customer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Private);
            //Create the private customer in CRM
            response = _customerManager.CreateCustomer(customer);
            customer.Deleted = true;
            updateCustomerResponse = _customerManager.UpdateCustomer(response.AccountId, customer);
            Assert.IsNotNull(updateCustomerResponse);
            Assert.AreEqual(updateCustomerResponse.Status, ProcessingStatus.SUCCESS);
            //Get the new updated record and see if all values has been updated in CRM
            responseCustomer =
                _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Private).Customer;
            Assert.IsNotNull(responseCustomer);
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);
            Assert.IsTrue(checkCustomer(responseCustomer, customer, AccountCategoryCode.Private));
            #endregion

            //UnitTest update organization
            #region UnitTest update organization
            //Create an organization
            customer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Company);
            //Create updateCustomer (contains information on what values should be updated)
            updateCustomer = getUnitTestCustomerUpdate(AccountCategoryCode.Company, false);
            //Create the organization in CRM
            response = _customerManager.CreateCustomer(customer);
            //Get information of the new organization in CRM
            responseCustomer =
                _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Company).Customer;
            //Set accountId and address id to match the updated record
            updateCustomer.AccountId = responseCustomer.AccountId;
            updateCustomer.Addresses[0].AddressId = responseCustomer.Addresses[0].AddressId;
            //Update the organization
            updateCustomerResponse = _customerManager.UpdateCustomer(responseCustomer.AccountId, updateCustomer);
            //Check if the function succeded
            Assert.AreEqual(updateCustomerResponse.Status, ProcessingStatus.SUCCESS);
            //Get the new updated record and see if all values has been updated in CRM
            responseCustomer =
                _customerManager.GetCustomer(updateCustomer.AccountId, AccountCategoryCode.Company).Customer;
            Assert.IsTrue(checkCustomer(responseCustomer, updateCustomer, AccountCategoryCode.Company));
            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, responseCustomer.CustomerType);
            #endregion

            //UnitTest delete organization
            #region UnitTest delete organization
            //Create an organization
            customer = SharedTest.getUnitTestCustomer(AccountCategoryCode.Company);
            //Create the organization in CRM
            response = _customerManager.CreateCustomer(customer);
            customer.Deleted = true;
            updateCustomerResponse = _customerManager.UpdateCustomer(response.AccountId, customer);
            Assert.IsNotNull(updateCustomerResponse);
            Assert.AreEqual(updateCustomerResponse.Status, ProcessingStatus.SUCCESS);
            //Get the new updated record and see if all values has been updated in CRM
            responseCustomer =
                _customerManager.GetCustomer(response.AccountId, AccountCategoryCode.Company).Customer;
            Assert.IsNotNull(responseCustomer);
            Assert.AreEqual(response.Status, ProcessingStatus.SUCCESS);
            Assert.IsTrue(checkCustomer(responseCustomer, customer, AccountCategoryCode.Company));
            #endregion

            //UnitTest null customer
            #region UnitTest null customer
            updateCustomerResponse = _customerManager.UpdateCustomer(Guid.Empty, null);
            Assert.AreEqual(updateCustomerResponse.Status, ProcessingStatus.SUCCESS);
            #endregion

            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
        }

        [TestMethod]
        public void GetCustomer()
        {
            ClearCustomers(AccountCategoryCode.Private);
            ClearCustomers(AccountCategoryCode.Company);
            //UnitTest get private customer
            #region UnitTest get private customer
            Assert.IsTrue(GetCustomerUnitTest(true, AccountCategoryCode.Private));
            Assert.IsTrue(GetCustomerUnitTest(false, AccountCategoryCode.Private));
            #endregion

            //UnitTest get organization
            #region UnitTest get organization
            Assert.IsTrue(GetCustomerUnitTest(true, AccountCategoryCode.Company));
            Assert.IsTrue(GetCustomerUnitTest(false, AccountCategoryCode.Company));
            #endregion

            //UnitTest get empty private
            #region UnitTest get empty private customer
            AssertFunc.Throws<NullReferenceException>(() => _customerManager.GetCustomer(Guid.Empty, AccountCategoryCode.Private));
            #endregion

            //UnitTest get empty organization
            #region UnitTest get empty organization
            AssertFunc.Throws<NullReferenceException>(() =>_customerManager.GetCustomer(Guid.Empty, AccountCategoryCode.Company));
            #endregion
        }

        #endregion


        #region Private Methods

        private void ClearCustomers(AccountCategoryCode accountCategoryCode)
        {
            var unitTestEntities = SharedTest.GetCustomerEntities(accountCategoryCode, UnitTestCreateCustomerEmailAddress);
            if (unitTestEntities != null && unitTestEntities.Count > 0)
            {
                foreach (Entity unitTestEntity in unitTestEntities)
                {
                    SharedTest.DeleteCustomerEntity(unitTestEntity.Id, accountCategoryCode);
                }
            }
        }

        private Customer getUnitTestCustomerUpdate(AccountCategoryCode customerType, bool delete)
        {
            //Create a private customer
            return new Customer
            {
                CustomerType = customerType,
                AccountFirstName = "UnitTestFirstName2",
                AccountLastName = "UnitTestLastName2",
                //Phone = "0720000000", Add later
                //MobilePhone = "0720000001", Add later
                Email = UnitTestUpdateCustomerEmailAddress,
                AllowAutoLoad = false,
                MaxCardsAutoLoad = 0,
                SocialSecurtiyNumber = "197602228392",
                OrganizationCreditApproved = false,
                Deleted = delete,
                InActive = false,
                HasEpiserverAccount = true,

                Addresses = new[]
                {
                    new Address()
                    {
                        CompanyName = "UnitTestCompanyName2",
                        CareOff = "UnitTestCareOff2",
                        Street = "UnitTestStreet2",
                        PostalCode = "UnitTestPostalCode2",
                        City = "UnitTestCity2",
                        County = "UnitTestCounty2",
                        Country = "UnitTestCountry2",
                        ContactPerson = "UnitTestPerson2",
                        ContactPhoneNumber = "0720000002",
                        SMSNotificationNumber = "0720000003",
                        AddressType = AddressTypeCode.Delivery
                    }
                }
            };
        }

        private Customer getUnitTestCustomerMinimum(AccountCategoryCode customerType)
        {
            //Create a private customer
            return new Customer
            {
                CustomerType = customerType,
                Email = UnitTestCreateCustomerEmailAddress
            };
        }

        private bool checkCustomerMinimum(Customer customerResponse, Customer customer, AccountCategoryCode type)
        {
            //Check contact values
            if (customerResponse.CustomerType != customer.CustomerType) return false;
            if (type == AccountCategoryCode.Private)
            {
                if (customerResponse.AccountFirstName != "") return false;
                if ((customerResponse.AccountLastName != "Ange namn") && (customerResponse.AccountLastName != "")) return false;
            }
            else if (type == AccountCategoryCode.Company)
            {
                if (customerResponse.Addresses.Any())
                {
                    if (customerResponse.RSID != customer.Addresses[0].CareOff)
                    {
                        return false;
                    }
                }
            }
            //if (customerResponse.Phone != "") return false; Add later
            //if (customerResponse.MobilePhone != "") return false; Add later
            if (customerResponse.Email != customer.Email) return false;
            if (customerResponse.AllowAutoLoad != customer.AllowAutoLoad) return false;
            if (customerResponse.MaxCardsAutoLoad != customer.MaxCardsAutoLoad) return false;
            if (customerResponse.SocialSecurtiyNumber != "") return false;
            if (type == AccountCategoryCode.Private)
            {
                if (customerResponse.OrganizationCreditApproved)
                    return false; //Private customer should not be accepted
            }
            else
            {
                if (customerResponse.OrganizationCreditApproved != customer.OrganizationCreditApproved)
                    return false;
            }
            if (customerResponse.Deleted != customer.Deleted) return false;
            if (customerResponse.InActive != customer.InActive) return false;
            if (customerResponse.HasEpiserverAccount == true || customer.HasEpiserverAccount != null)
            {
                return false;
            }
            if (customerResponse.CustomerType != customer.CustomerType) return false;

            //Check address values
            if (customerResponse.Addresses != null && customerResponse.Addresses.Any())
            {
                if (customerResponse.Addresses[0].CompanyName != null)
                    return false;
                if (customerResponse.Addresses[0].CareOff != null) return false;
                if (customerResponse.Addresses[0].Street != null) return false;
                if (customerResponse.Addresses[0].PostalCode != null) return false;
                if (customerResponse.Addresses[0].City != null) return false;
                if (customerResponse.Addresses[0].County != null) return false;
                if (customerResponse.Addresses[0].Country != null) return false;
                if (customerResponse.Addresses[0].ContactPerson != null) return false;
                if (customerResponse.Addresses[0].ContactPhoneNumber != null) return false;
                if (customerResponse.Addresses[0].SMSNotificationNumber != null) return false;
            }
            return true;
        }


        private bool checkCustomer(Customer customerResponse, Customer customer, AccountCategoryCode type)
        {
            //Check contact values
            if (customerResponse.CustomerType != customer.CustomerType) return false;
            if (type == AccountCategoryCode.Private)
            {
                if (customerResponse.AccountFirstName != customer.AccountFirstName) return false;
                if (customerResponse.AccountLastName != customer.AccountLastName) return false;
            }
            else if(type == AccountCategoryCode.Company)
            {
                if (customerResponse.Addresses.Any())
                {
                    if (customerResponse.RSID != customer.Addresses[0].CareOff)
                    {
                        return false;
                    }
                }
            }
            //if (customerResponse.Phone != customer.Phone) return false; Add later
            //if (customerResponse.MobilePhone != customer.MobilePhone) return false; Add later
            if (customerResponse.Email != customer.Email) return false;
            if (customerResponse.AllowAutoLoad != customer.AllowAutoLoad) return false;
            if (customerResponse.MaxCardsAutoLoad != customer.MaxCardsAutoLoad) return false;
            if (customerResponse.SocialSecurtiyNumber != customer.SocialSecurtiyNumber) return false;
            if (type == AccountCategoryCode.Private)
            {
                if (customerResponse.OrganizationCreditApproved)
                    return false; //Private customer should not be accepted
            }
            else
            {
                if (customerResponse.OrganizationCreditApproved != customer.OrganizationCreditApproved)
                    return false;
            }
            if (customerResponse.Deleted != customer.Deleted) return false;
            if (customerResponse.InActive != customer.InActive) return false;
            if (customerResponse.HasEpiserverAccount != null || customer.HasEpiserverAccount != null)
            {
                if (customerResponse.HasEpiserverAccount == null)
                    return false;
                if (customer.HasEpiserverAccount == null)
                    return false;
                //Star and episerver has bool on the other side. 
                //Cant be null for serialization. They dont use this field. Maybe another class or subclass in future,
                //or just a bool on customer that even if not retrieved serializes to false. ( proberbly )
                //But that might effect create and update if they dont send it in in all calls.
                //A bool that has required attribute would proberbly be the best thing.
                
                //if (customerResponse.HasEpiserverAccount.Value != customer.HasEpiserverAccount.Value)
                //    return false;
            }
            if (customerResponse.CustomerType != customer.CustomerType) return false;

            //Check address values
            if (customerResponse.Addresses != null && customerResponse.Addresses.Any())
            {
                if (customerResponse.Addresses[0].CompanyName != customer.Addresses[0].CompanyName)
                    return false;
                if (customerResponse.Addresses[0].CareOff != customer.Addresses[0].CareOff) return false;
                if (customerResponse.Addresses[0].Street != customer.Addresses[0].Street) return false;
                if (customerResponse.Addresses[0].PostalCode != customer.Addresses[0].PostalCode) return false;
                if (customerResponse.Addresses[0].City != customer.Addresses[0].City) return false;
                if (customerResponse.Addresses[0].County != customer.Addresses[0].County) return false;
                if (customerResponse.Addresses[0].Country != customer.Addresses[0].Country) return false;
                if (customerResponse.Addresses[0].ContactPerson != customer.Addresses[0].ContactPerson) return false;
                if (customerResponse.Addresses[0].ContactPhoneNumber != customer.Addresses[0].ContactPhoneNumber) return false;
                if (customerResponse.Addresses[0].SMSNotificationNumber != customer.Addresses[0].SMSNotificationNumber) return false;
            }
            return true;
        }

        private bool GetCustomerUnitTest(bool fullCustomer, AccountCategoryCode accountCategoryCode)
        {
            //Create a private customer

            Customer customer;
            if (fullCustomer)
            {
                customer = SharedTest.getUnitTestCustomer(accountCategoryCode);
            }
            else
            {
                customer = getUnitTestCustomerMinimum(accountCategoryCode);
            }
            //Create the private customer in CRM
            CreateCustomerResponse response = _customerManager.CreateCustomer(customer);
            //Check if customer was created successfully
            if (response.AccountId == Guid.Empty)
            {
                return false;
            }
            //Get customer response
            var customerResponse = _customerManager.GetCustomer(response.AccountId, accountCategoryCode);
            //Check if we got a match
            if (customerResponse == null)
            {
                return false;
            }
            //Check status
            if (customerResponse.Status != ProcessingStatus.SUCCESS)
            {
                return false;
            }
            //Check if all values are returned
            if (fullCustomer)
            {
                if (!checkCustomer(customerResponse.Customer, customer, accountCategoryCode))
                {
                    return false;
                }
            }
            else
            {
                if (!checkCustomerMinimum(customerResponse.Customer, customer, customerResponse.Customer.CustomerType))
                {
                    return false;
                }
            }

            //Delete customer
            SharedTest.DeleteCustomerEntity(response.AccountId, customerResponse.Customer.CustomerType);
            return true;
        }

        #endregion
    }
}
