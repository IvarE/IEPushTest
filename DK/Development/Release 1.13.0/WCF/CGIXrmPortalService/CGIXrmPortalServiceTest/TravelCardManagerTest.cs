using CGICRMPortalService.Customer;
using CGICRMPortalService.Customer.Models;
using CGICRMPortalService.Shared.Models;
using CGICRMPortalService.TravelCard;
using CGICRMPortalService.TravelCard.CrmClasses;
using CGICRMPortalService.TravelCard.Models;
using CGIXrmPortalServiceTest.Models;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SharedUnitTests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmPortalServiceTest
{
    [TestClass]
    public class TravelCardManagerTest
    {
        #region Declarations ------------------------------------------------------------------------------------------
        private readonly XrmManager _manager;
        private readonly TravelCardManager _travelCardManager = new TravelCardManager(Guid.Empty);
        private readonly CustomerManager _customerManager = new CustomerManager(Guid.Empty);
        private readonly string unitTestTravelCardNumber = "1111111111";
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
        public void RegisterTravelCard_CreateOrUpdateUnitTest()
        {
            //SharedTest.DeleteTravelCardEntity(new Guid("7451DD7B-EF0D-E611-80E9-0050569010AD"));
            #region UnitTest Create/Update travel card for private customer
            //Register card
            TravelCard updateTravelCard = CreateCardTest(AccountCategoryCode.Private);
            Assert.IsNotNull(updateTravelCard);
            //Try to update a registered card
            Assert.IsFalse(TestUpdateTravelCard(updateTravelCard, AccountCategoryCode.Private));
            //Update an unregistered card
            Assert.IsTrue(UpdateExistingCardTest(updateTravelCard, AccountCategoryCode.Private));
            #endregion

            #region UnitTest Create/Update travel card for organization
            //Register card
            updateTravelCard = CreateCardTest(AccountCategoryCode.Company);
            Assert.IsNotNull(updateTravelCard);
            //Try to update a registered card
            Assert.IsFalse(TestUpdateTravelCard(updateTravelCard, AccountCategoryCode.Company));
            //Update an unregistered card
            Assert.IsTrue(UpdateExistingCardTest(updateTravelCard, AccountCategoryCode.Company));
            #endregion

            //Create travel card for private customer with company travel card
            TravelCardResponseTest travelCardResponseTest = SharedTest.TestNewTravelCard(AccountCategoryCode.Private, AccountCategoryCode.Company, true);
            Assert.AreEqual(travelCardResponseTest.Status, ProcessingStatus.FAILED);

            //Create travel card for organization with travel card for private customer
            travelCardResponseTest = SharedTest.TestNewTravelCard(AccountCategoryCode.Company, AccountCategoryCode.Private, true);
            Assert.AreEqual(travelCardResponseTest.Status, ProcessingStatus.FAILED);

            //Create travel card without private customer id
            TravelCard travelCard = SharedTest.CreateUnitTestTravelCard(Guid.Empty, AccountCategoryCode.Private);
            RegisterTravelCardResponse travelCardResponse = _travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            Assert.AreEqual(travelCardResponse.Status, ProcessingStatus.FAILED);

            //Create travel card without organization id
            travelCard = SharedTest.CreateUnitTestTravelCard(Guid.Empty, AccountCategoryCode.Company);
            travelCardResponse = _travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            Assert.AreEqual(travelCardResponse.Status, ProcessingStatus.FAILED);

            //UnitTest empty travel card
            travelCard = new TravelCard();
            travelCardResponse = _travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            Assert.AreEqual(travelCardResponse.Status, ProcessingStatus.FAILED);
            //UnitTest null travel card
            AssertFunc.Throws<NullReferenceException>(() => _travelCardManager.RegisterTravelCard_CreateOrUpdate(null));
        }
        [TestMethod]
        public void UpdateTravelCardTest()
        {
            SharedTest.ClearCustomers(AccountCategoryCode.Private);
            SharedTest.ClearCustomers(AccountCategoryCode.Company);
            CustomerManagerTest customerManagerTest = new CustomerManagerTest();
            //Test updating private customer
            Assert.IsTrue(UpdateCardTest(AccountCategoryCode.Private, true));
            //Test updating organization
            Assert.IsTrue(UpdateCardTest(AccountCategoryCode.Company, true));
            //Test updating non existing travel card for private customer
            Assert.IsFalse(UpdateCardTest(AccountCategoryCode.Private, false));
            //Test updating non existing travel card for organization
            Assert.IsFalse(UpdateCardTest(AccountCategoryCode.Company, false));
            //Test updating empty card
            UpdateTravelCardResponse updateTravelCardResponse = _travelCardManager.UpdateTravelCard(new TravelCard());
            Assert.AreEqual(updateTravelCardResponse.Status, ProcessingStatus.FAILED);
            //Test updating null card
            updateTravelCardResponse = _travelCardManager.UpdateTravelCard(null);
            Assert.AreEqual(updateTravelCardResponse.Status, ProcessingStatus.FAILED);
        }
        [TestMethod]
        public void UnregisterTravelCard_RemoveRelationshipsTest()
        {
            SharedTest.ClearCustomers(AccountCategoryCode.Private);
            SharedTest.ClearCustomers(AccountCategoryCode.Company);
            //Test unregister card on private customer
            Assert.IsTrue(UnregisterCardTest(AccountCategoryCode.Private, AccountCategoryCode.Private, true, true));
            //Test unregister card on organization
            Assert.IsTrue(UnregisterCardTest(AccountCategoryCode.Company, AccountCategoryCode.Company, true, true));
            //Test unregister organization card on private customer
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Private, AccountCategoryCode.Company, true, true));
            //Test unregister private customer card on organization
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Company, AccountCategoryCode.Private, true, true));

            //Test non existing account id & non existing travelCard number for a private customer
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Private, AccountCategoryCode.Private, false, true));
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Private, AccountCategoryCode.Private, false, false));
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Private, AccountCategoryCode.Private, true, false));

            //Test non existing account id &non existing travelCard number for an organization
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Company, AccountCategoryCode.Company, false, true));
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Company, AccountCategoryCode.Company, false, false));
            Assert.IsFalse(UnregisterCardTest(AccountCategoryCode.Company, AccountCategoryCode.Company, true, false));
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private bool UnregisterCardTest(AccountCategoryCode accountCategoryCode, AccountCategoryCode unregisterAccountCategoryCode, bool accountIdExists, bool travelCardNumberExists)
        {
            bool resultValue = false;
            CustomerManagerTest customerManagerTest = new CustomerManagerTest();
            TravelCardResponseTest registerTravelCardResponse = SharedTest.TestNewTravelCard(accountCategoryCode, accountCategoryCode, false);
            GetCardsForCustomerResponse travelCards1 = _travelCardManager.GetCardsForCustomer(registerTravelCardResponse.AccountId, accountCategoryCode);
            UnRegisterTravelCardResponse unRegisterTravelCardResponse;
            if (accountIdExists && travelCardNumberExists)
            {
                unRegisterTravelCardResponse = _travelCardManager.UnregisterTravelCard_RemoveRelationships(registerTravelCardResponse.AccountId, unregisterAccountCategoryCode, unitTestTravelCardNumber);
            }
            else if (!accountIdExists && travelCardNumberExists)
            {
                unRegisterTravelCardResponse = _travelCardManager.UnregisterTravelCard_RemoveRelationships(Guid.Empty, unregisterAccountCategoryCode, unitTestTravelCardNumber);
            }
            else if (accountIdExists && !travelCardNumberExists)
            {
                unRegisterTravelCardResponse = _travelCardManager.UnregisterTravelCard_RemoveRelationships(registerTravelCardResponse.AccountId, unregisterAccountCategoryCode, string.Empty);
            }
            else
            {
                unRegisterTravelCardResponse = _travelCardManager.UnregisterTravelCard_RemoveRelationships(Guid.Empty, unregisterAccountCategoryCode, string.Empty);
            }
            if (unRegisterTravelCardResponse.Status == ProcessingStatus.SUCCESS)
            {
                TravelCard travelCards2 = GetInformationFromCard(SharedTest.GetUnitTestTravelCard()).FirstOrDefault();
                resultValue = checkCard(travelCards1.TravelCards[0], travelCards2, true);
            }
            SharedTest.DeleteTravelCardEntity(SharedTest.GetUnitTestTravelCard());
            SharedTest.DeleteCustomerEntity(registerTravelCardResponse.AccountId, accountCategoryCode);
            return resultValue;
        }

        private TravelCard GetTravelCardFromCrmTravelCard(CrmTravelCard crmTravelCard)
        {
            TravelCard travelcard = new TravelCard
            {
                AccountId = crmTravelCard.Account != null ? crmTravelCard.Account.Id : (crmTravelCard.Contact != null ? crmTravelCard.Contact.Id : Guid.Empty),
                CardName = crmTravelCard.CardName,
                CardNumber = crmTravelCard.CardNumber,
                //CardType = crmTravelCard.CardTypeName,
                CustomerType = crmTravelCard.Account != null ? AccountCategoryCode.Company : AccountCategoryCode.Private,
                PeriodCardTypeTitle = crmTravelCard.PeriodCardTypeTitle,
                ValueCardTypeTitle = crmTravelCard.ValueCardTypeTitle,
                PeriodValidFrom = crmTravelCard.PeriodValidFrom,
                PeriodValidTo = crmTravelCard.PeriodValidTo,
                Blocked = crmTravelCard.Blocked,
                AutoloadConnectionDate = crmTravelCard.AutoloadConnectionDate,
                AutoloadDisconnectionDate = crmTravelCard.AutoloadDisconnectionDate,
                AutoloadStatus = crmTravelCard.AutoloadStatus,
                CardCategory = crmTravelCard.CardCategory,
                CreditCardMask = crmTravelCard.CreditCardMask,
                Currency = crmTravelCard.Currency,
                FailedAttemptsToChargeMoney = crmTravelCard.FailedAttemptsToChargeMoney,
                LatestAutoloadAmount = crmTravelCard.LatestAutoloadAmount != null ? crmTravelCard.LatestAutoloadAmount.Value : 0,
                LatestChargeDate = crmTravelCard.LatestChargeDate,
                LatestFailedAttempt = crmTravelCard.LatestFailedAttempt,
                PeriodCardTypeId = crmTravelCard.PeriodCardTypeId,
                ValueCardTypeId = crmTravelCard.ValueCardTypeId,
                VerifyId = crmTravelCard.VerifyId
            };
            return travelcard;
        }

        private List<TravelCard> GetInformationFromCard(Guid cardGuid)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute("cgi_travelcard")
            {
                ColumnSet = new ColumnSet(true)
            };

            queryByAttribute.AddAttributeValue("cgi_travelcardid", cardGuid);
            ObservableCollection<CrmTravelCard> crmTravelCards = _manager.Get<CrmTravelCard>(queryByAttribute);
            var lstTravelCard = (from crmTravelCard in crmTravelCards
                                 select GetTravelCardFromCrmTravelCard(crmTravelCard)).ToList();
            return lstTravelCard;
        }

        private bool TestUpdateTravelCard(TravelCard travelCard, AccountCategoryCode accountCategoryCode)
        {
            bool result = false;
            CustomerManagerTest customerManagerTest = new CustomerManagerTest();
            RegisterTravelCardResponse travelCardResponse = _travelCardManager.RegisterTravelCard_CreateOrUpdate(travelCard);
            if (travelCard.AccountId != null && travelCardResponse.Status == ProcessingStatus.FAILED)
            {
                return false;
            }
            else
            {
                GetCardsForCustomerResponse getCardsResponse = _travelCardManager.GetCardsForCustomer(travelCard.AccountId, accountCategoryCode);
                result = checkCard(getCardsResponse.TravelCards[0], travelCard, false);
            }
            if (travelCardResponse.Status == ProcessingStatus.SUCCESS)
            {
                //Delete travel card
                SharedTest.DeleteTravelCardEntity(SharedTest.GetUnitTestTravelCard());

                //Delete customer
                SharedTest.DeleteCustomerEntity(travelCard.AccountId, accountCategoryCode);
                return result;
            }
            else
            {
                return false;
            }
        }

        private TravelCard UpdateUnitTestTravelCard(Guid accountId, AccountCategoryCode accountCategoryCode)
        {
            TravelCard travelCard = new TravelCard();
            travelCard.AccountId = accountId;
            travelCard.AutoloadConnectionDate = DateTime.Today.AddDays(1).ToUniversalTime();
            travelCard.AutoloadStatus = 0;
            travelCard.Blocked = true;
            travelCard.CardCategory = 2;
            travelCard.CardName = "TravelCardUnitTest2";
            travelCard.CreditCardMask = "1234";
            travelCard.Currency = "2";
            travelCard.CustomerType = accountCategoryCode;
            travelCard.FailedAttemptsToChargeMoney = 2;
            travelCard.LatestAutoloadAmount = 2;
            travelCard.LatestChargeDate = DateTime.Today.AddDays(1).ToUniversalTime();
            travelCard.LatestFailedAttempt = DateTime.Today.AddDays(1).ToUniversalTime();
            travelCard.PeriodCardTypeId = 2;
            travelCard.PeriodCardTypeTitle = "TravelCardUnitTest2";
            travelCard.PeriodValidFrom = DateTime.Today.ToUniversalTime();
            travelCard.PeriodValidTo = DateTime.Today.AddDays(1).ToUniversalTime();
            travelCard.ValueCardTypeId = 2;
            travelCard.ValueCardTypeTitle = "TravelCardUnitTest2";
            travelCard.VerifyId = "2";
            travelCard.CardNumber = unitTestTravelCardNumber;
            return travelCard;
        }

        private TravelCard CreateCardTest(AccountCategoryCode accountCategoryCode)
        {
            //Create travel card for private customer
            TravelCardResponseTest travelCardResponse = SharedTest.TestNewTravelCard(accountCategoryCode, accountCategoryCode, false);
            if(travelCardResponse.Status != ProcessingStatus.SUCCESS)
            {
                return null;
            }
            TravelCard updateTravelCard = UpdateUnitTestTravelCard(travelCardResponse.AccountId, accountCategoryCode);
            return updateTravelCard;
        }

        private bool UpdateCardTest(AccountCategoryCode accountCategoryCode, bool existingCardNumber)
        {
            TravelCard updateTravelCard;
            Customer customer;
            bool cardResult = false;
            CreateCustomerResponse response = new CreateCustomerResponse();
            if (existingCardNumber)
            {
                updateTravelCard = CreateCardTest(accountCategoryCode);
            }
            else
            {
                //Create new private customer
                customer = SharedTest.getUnitTestCustomer(accountCategoryCode);
                response = _customerManager.CreateCustomer(customer);
                updateTravelCard = UpdateUnitTestTravelCard(response.AccountId, accountCategoryCode);
            }
            UpdateTravelCardResponse updateTravelCardResponse = _travelCardManager.UpdateTravelCard(updateTravelCard);
            GetCardsForCustomerResponse travelCardResponse = _travelCardManager.GetCardsForCustomer(updateTravelCard.AccountId, accountCategoryCode);
            if (travelCardResponse.TravelCards.Any())
            {
                cardResult = checkCard(travelCardResponse.TravelCards[0], updateTravelCard, false);
            }

            if (existingCardNumber)
            {
                SharedTest.DeleteCustomerEntity(updateTravelCard.AccountId, accountCategoryCode);
                SharedTest.DeleteTravelCardEntity(SharedTest.GetUnitTestTravelCard());
            }
            else
            {
                SharedTest.DeleteCustomerEntity(response.AccountId, accountCategoryCode);
            }

            if (updateTravelCardResponse.Status != ProcessingStatus.SUCCESS)
            {
                return false;
            }
            if(updateTravelCard.AccountId == null)
            {
                return false;
            }

            return cardResult;
        }

        private bool UpdateExistingCardTest(TravelCard updateTravelCard, AccountCategoryCode accountCategoryCode)
        {
            //Delete any existing customer with test address
            var unitTestEntities = SharedTest.GetCustomerEntities(accountCategoryCode, SharedTest.UnitTestCreateCustomerEmailAddress);
            if (unitTestEntities != null && unitTestEntities.Count > 0)
            {
                foreach (Entity unitTestEntity in unitTestEntities)
                {
                    SharedTest.DeleteCustomerEntity(unitTestEntity.Id, accountCategoryCode);
                }
            }
            //Create new private customer
            Customer customer = SharedTest.getUnitTestCustomer(accountCategoryCode);
            CreateCustomerResponse response = _customerManager.CreateCustomer(customer);
            updateTravelCard.AccountId = response.AccountId;
            return TestUpdateTravelCard(updateTravelCard, accountCategoryCode);
        }

        private bool checkCard(TravelCard travelCardResponse, TravelCard travelCard, bool relationshipRemoved)
        {
            if(!relationshipRemoved)
            {
                if (travelCardResponse.AccountId != travelCard.AccountId) return false;
            }
            if (travelCardResponse.AutoloadConnectionDate.ToUniversalTime() != travelCard.AutoloadConnectionDate.ToUniversalTime()) return false;
            if (travelCardResponse.AutoloadStatus != travelCard.AutoloadStatus) return false;
            if (travelCardResponse.Blocked != travelCard.Blocked) return false;
            //if (travelCardResponse.CardName != travelCard.CardName) return false;
            if (travelCardResponse.CreditCardMask != travelCard.CreditCardMask) return false;
            if (travelCardResponse.Currency != travelCard.Currency) return false;
            if(!relationshipRemoved)
            {
                if (travelCardResponse.CustomerType != travelCard.CustomerType) return false;
            }
            if (travelCardResponse.FailedAttemptsToChargeMoney != travelCard.FailedAttemptsToChargeMoney) return false;
            if (travelCardResponse.LatestAutoloadAmount != travelCard.LatestAutoloadAmount) return false;
            if (travelCardResponse.LatestChargeDate.ToUniversalTime() != travelCard.LatestChargeDate.ToUniversalTime()) return false;
            if (travelCardResponse.LatestFailedAttempt.ToUniversalTime() != travelCard.LatestFailedAttempt.ToUniversalTime()) return false;
            if (travelCardResponse.PeriodCardTypeId != travelCard.PeriodCardTypeId) return false;
            if (travelCardResponse.PeriodCardTypeTitle != travelCard.PeriodCardTypeTitle) return false;
            if (travelCardResponse.PeriodValidFrom.ToUniversalTime() != travelCard.PeriodValidFrom.ToUniversalTime()) return false;
            if (travelCardResponse.PeriodValidTo.ToUniversalTime() != travelCard.PeriodValidTo.ToUniversalTime()) return false;
            if (travelCardResponse.ValueCardTypeId != travelCard.ValueCardTypeId) return false;
            if (travelCardResponse.ValueCardTypeTitle != travelCard.ValueCardTypeTitle) return false;
            if (travelCardResponse.VerifyId != travelCard.VerifyId) return false;
            if (travelCardResponse.CardNumber != travelCard.CardNumber) return false;

            return true;
        }

        #endregion
    }
}