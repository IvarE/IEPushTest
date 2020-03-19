using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endeavor.Crm.UnitTest;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Endeavor.Crm;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Crm.Sdk.Messages;
using System.Xml;
using Assert = NUnit.Framework.Assert;
using System.Linq;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class MergeOldAccountWithPortalFixture : PluginFixtureBase
    {

        #region Data

        private const string c_email = "test@gmail.com";
        private const string c_mobile = "0654687113";

        private const string c_account_Id_Old = "bb30cea6-5cf5-43f5-8ae7-c6989c7596b5";
        private const string c_account_Id_New_Parent = "4675be92-a15e-4f9d-9627-b499ec45f092";
        private const string c_account_Id_New_Child = "bc2f1bcf-9703-4d0d-ad8d-c9e7e54b394e";
        private const string c_AccountName = "Endeavor Company AB";
        private const string c_AccountName_KST = "Endeavor Company AB - KST";
        private const string c_oldAccountOrganizationNumber_FaultyFormat = "341513-1232";
        private const string c_newAccountOrgnr_CorrectFormat = "3415131232";
        private const string c_oldAccountNumber = "VC-1627234-Old";
        private const string c_newAccountNumber_Parent = "VC-652163-New";
        private const string c_newAccountNumber_KST = "161956153";
        private const string c_RSID = "";
        private const string c_responsibility = "";
        private const string c_Organization_Sub_Number = "";

        private const string c_Address_Id = "2325d48a-bbd3-4df5-ae2f-a838fe368bb6";
        private const string c_Address_Line2 = "Sjöparksvägen 2";
        private const string c_postalCode = "12345";
        private const string c_city = "Stockholm";
        private const string c_address_name = "Endeavor";

        private const string c_travelCard_Name = "Endeavor_TravelCard";

        [Test, Category("Debug")]
        public void TestAcc()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //Create an account
                var account = new AccountEntity()
                {
                    Name = "ADDRESS TESTING",
                    cgi_organizational_number = c_oldAccountOrganizationNumber_FaultyFormat,
                    cgi_rsid = "",
                    cgi_responsibility = "",
                    cgi_organization_sub_number = ""
                };

                account.Id = XrmHelper.Create(localContext, account);


                var query = new QueryExpression()
                {
                    EntityName = AccountEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(AccountEntity.Fields.Id, ConditionOperator.Equal, account.Id)
                        }
                    }
                };

                var acc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, query);

                TestData_CustomerAddress(localContext, account.Id, Generated.customeraddress_addresstypecode.Visiting,
    "Torpa", "Torpagatan 1a", "51512", "Jönköping");

                var addressColumnSet = new ColumnSet(
                        CustomerAddressEntity.Fields.AddressTypeCode,
                        CustomerAddressEntity.Fields.Name,
                        CustomerAddressEntity.Fields.Line2,
                        CustomerAddressEntity.Fields.PostalCode,
                        CustomerAddressEntity.Fields.City,
                        CustomerAddressEntity.Fields.ParentId);

                var filterAddress = new FilterExpression
                {
                    Conditions = {
                            new ConditionExpression(CustomerAddressEntity.Fields.AddressTypeCode, ConditionOperator.NotNull),
                    }
                };
                filterAddress.AddCondition(new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, acc.Id));
                var oldAcc_address = TestDataHelper.GetCustomerAddresses_ByParentId(localContext, acc.Id, addressColumnSet, filterAddress);

                var addr = oldAcc_address.First();

                XrmHelper.Delete(localContext, new EntityReference(CustomerAddressEntity.EntityLogicalName, oldAcc_address[0].Id));


            }
        }


        #endregion

        [Test, Category("Debug")]
        public void FullFlow_MergeOldAccountWithPortal()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                #region Delete data

                TestDataHelper.DeleteAccount_ByName(localContext, c_AccountName, c_AccountName_KST);
                TestDataHelper.DeleteRelatedTravelCards_ByAccountName(localContext, c_AccountName);
                TestDataHelper.DeleteTravelCard_ByName(localContext, c_travelCard_Name);

                #endregion

                #region Create test data

                //Old account
                var acc = TestData_CreateAccount_Old(localContext, "rsid_OLD", "Van Carl");
                TestData_CustomerAddress(localContext, acc.Id, Generated.customeraddress_addresstypecode.BillTo);
                TestData_CustomerAddress(localContext, acc.Id, Generated.customeraddress_addresstypecode.Visiting,
                    "Torpa", "Torpagatan 1a", "51512", "Jönköping");
                TestData_CustomerAddress(localContext, acc.Id, Generated.customeraddress_addresstypecode.Visiting,
                    "Hermosdal", "Jyskgatan 45", "47743", "Sockholm");
                TestData_CustomerAddress(localContext, acc.Id, Generated.customeraddress_addresstypecode.ShipTo,
                    "Ulricehamn", "Lantgatan 66", "77127", "Ulricehamn");
                TestData_TravelCard(localContext, c_travelCard_Name, acc.Id, "1208111539");

                //Portal acc
                var portalAcc_Parent = TestData_CreateAccount_Parent(localContext, c_newAccountNumber_Parent);
                var portalAcc_Child = TestData_CreateAccount_KST(localContext, portalAcc_Parent.Id, c_newAccountNumber_KST, "fusg23", "", "89234ykba");
                //TestData_CustomerAddress(localContext, portalAcc_Child.Id, Generated.customeraddress_addresstypecode.BillTo);
                //TestData_CustomerAddress(localContext, portalAcc_Child.Id, Generated.customeraddress_addresstypecode.Visiting, "KST_Name", "Adress2", "414131", "Jönköping");

                #endregion

                #region Step 1 - User enters accountnumber

                var userInput_OldAccount = c_oldAccountNumber;
                var userInput_PortalAccount_KST = c_newAccountNumber_KST;

                #endregion

                #region Step 2 - Validate given account number in CRM


                var filterAccount = new QueryExpression()
                {
                    EntityName = AccountEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(AccountEntity.Fields.cgi_rsid,
                    AccountEntity.Fields.cgi_organizational_number,
                    AccountEntity.Fields.cgi_responsibility,
                    AccountEntity.Fields.cgi_organization_sub_number,
                    AccountEntity.Fields.ParentAccountId,
                    AccountEntity.Fields.AccountNumber),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, userInput_OldAccount)
                        }
                    }
                };

                //Old Account
                var oldAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
                if (oldAcc == null)
                    throw new NullReferenceException($"Could not find account with account number '{c_oldAccountNumber}'.");
                #region Step 3 - Format organization number & validate

                //Format both accounts since portal account may also be incorrect
                oldAcc.cgi_organizational_number = FormatOrganizationNumber(oldAcc.cgi_organizational_number);


                //New Portal Account - KST
                filterAccount.Criteria = null;
                filterAccount.Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, userInput_PortalAccount_KST),
                        new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.NotNull),
                    }
                };
                var childAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
                if (childAcc == null)
                    throw new NullReferenceException($"Could not find KST account with accountnumber '{userInput_PortalAccount_KST}'");

                if (!string.IsNullOrWhiteSpace(childAcc.cgi_organizational_number))
                    throw new InvalidPluginExecutionException($"KST account '{childAcc.Id}' should not contain");


                //NewPortal Account - Parent
                filterAccount.Criteria = null;
                filterAccount.Criteria = new FilterExpression
                {
                    Conditions = {
                        new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, oldAcc.cgi_organizational_number),
                        new ConditionExpression(AccountEntity.Fields.ed_TypeOfAccount, ConditionOperator.Equal, (int)Generated.ed_account_ed_typeofaccount.Companycustomerportal),
                        new ConditionExpression(AccountEntity.Fields.Id, ConditionOperator.Equal, childAcc.ParentAccountId.Id)
                    }
                };

                var newAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
                if (newAcc == null)
                    throw new NullReferenceException($"Could not find account with account org nr '{oldAcc.cgi_organizational_number}'.");

                ////Check if org-nr matches
                if (!oldAcc.cgi_organizational_number.Equals(newAcc.cgi_organizational_number))
                    throw new InvalidPluginExecutionException($"Organization number '{oldAcc.cgi_organizational_number}' from '{oldAcc.Name}' does not match organization number with account '{newAcc.Name}'");

                #endregion

                #endregion



                #region Step 5 - Validate address & assign

                var addressColumnSet = new ColumnSet(
                        CustomerAddressEntity.Fields.AddressTypeCode,
                        CustomerAddressEntity.Fields.Name,
                        CustomerAddressEntity.Fields.Line2,
                        CustomerAddressEntity.Fields.PostalCode,
                        CustomerAddressEntity.Fields.City,
                        CustomerAddressEntity.Fields.ParentId);

                var filterAddress = new FilterExpression
                {
                    Conditions = {
                            new ConditionExpression(CustomerAddressEntity.Fields.AddressTypeCode, ConditionOperator.NotNull),
                    }
                };
                filterAddress.AddCondition(new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, oldAcc.Id));
                var oldAcc_address = TestDataHelper.GetCustomerAddresses_ByParentId(localContext, oldAcc.Id, addressColumnSet, filterAddress);

                filterAddress = new FilterExpression
                {
                    Conditions = {
                            new ConditionExpression(CustomerAddressEntity.Fields.AddressTypeCode, ConditionOperator.NotNull),
                    }
                };
                filterAddress.AddCondition(new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, childAcc.Id));
                var newAcc_KST_address = TestDataHelper.GetCustomerAddresses_ByParentId(localContext, childAcc.Id, addressColumnSet, filterAddress);

                //bool match = true;
                bool hasBilling = false, hasVisiting = false, hasShipping = false, hasInitiallyNoAddresses = false;
                hasInitiallyNoAddresses = newAcc_KST_address.Count <= 1 ? true : false;
                bool hasUpdated = false;

                //bool hasBillingAddress = false;
                foreach (var old_addr in oldAcc_address)
                {
                    if (string.IsNullOrWhiteSpace(old_addr.Name) &&
                        string.IsNullOrWhiteSpace(old_addr.PostalCode) &&
                        string.IsNullOrWhiteSpace(old_addr.City) &&
                        string.IsNullOrWhiteSpace(old_addr.Line2))
                        continue;


                    if (hasInitiallyNoAddresses)
                    {
                        var createNewAddress = true;

                        CustomerAddressEntity addr_ = new CustomerAddressEntity();
                        if (!hasUpdated)
                        {
                            addr_ = newAcc_KST_address.First();
                            if (addr_.AddressTypeCode == Generated.customeraddress_addresstypecode.BillTo)
                                createNewAddress = false;
                        }

                        if (createNewAddress && hasUpdated)
                        {
                            var copyAddr = new CustomerAddressEntity()
                            {
                                AddressTypeCode = old_addr.AddressTypeCode,
                                Name = old_addr.Name,
                                PostalCode = old_addr.PostalCode,
                                City = old_addr.City,
                                Line2 = old_addr.Line2,
                                ParentId = new EntityReference(AccountEntity.EntityLogicalName, childAcc.Id),
                            };

                            copyAddr.Id = XrmHelper.Create(localContext, copyAddr);
                            addr_ = copyAddr;

                        }
                        else
                        {
                            if (addr_.AddressTypeCode == null)
                                addr_.AddressTypeCode = old_addr.AddressTypeCode;

                            if (string.IsNullOrWhiteSpace(addr_.Name))
                                addr_.Name = old_addr.Name;


                            if (string.IsNullOrWhiteSpace(addr_.Line2))
                                addr_.Line2 = old_addr.Line2;

                            if (string.IsNullOrWhiteSpace(addr_.PostalCode))
                                addr_.PostalCode = old_addr.PostalCode;

                            if (string.IsNullOrWhiteSpace(addr_.City))
                                addr_.City = old_addr.PostalCode;

                            if (addr_.ParentId == null)
                                addr_.ParentId = new EntityReference(AccountEntity.EntityLogicalName, childAcc.Id);
                            XrmHelper.Update(localContext, addr_);

                            hasUpdated = true;
                        }

                        //var copy_address = TestData_CustomerAddress(localContext, childAcc.Id, old_addr.AddressTypeCode.Value, old_addr.Name,
                        //    old_addr.Line2, old_addr.PostalCode, old_addr.City);
                        Console.WriteLine($"Creating an address with type '{addr_.AddressTypeCode}'");

                        var updatedCustomerAddress_KST = XrmRetrieveHelper.Retrieve<CustomerAddressEntity>(localContext,
                            new EntityReference(CustomerAddressEntity.EntityLogicalName, addr_.Id),
                            new ColumnSet(CustomerAddressEntity.Fields.AddressTypeCode,
                            CustomerAddressEntity.Fields.Name,
                            CustomerAddressEntity.Fields.Line2,
                            CustomerAddressEntity.Fields.PostalCode,
                            CustomerAddressEntity.Fields.City));

                        Assert.AreEqual(old_addr.AddressTypeCode, updatedCustomerAddress_KST.AddressTypeCode);
                        Assert.AreEqual(childAcc.Id, updatedCustomerAddress_KST.ParentId.Id);

                    }
                    else
                    {
                        break;
                    }
                }

                if (!hasInitiallyNoAddresses)
                {
                    foreach (var new_addr in newAcc_KST_address)
                    {
                        if (new_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.BillTo)
                        {
                            if (string.IsNullOrWhiteSpace(new_addr.Name) &&
                                string.IsNullOrWhiteSpace(new_addr.PostalCode) &&
                                string.IsNullOrWhiteSpace(new_addr.City) &&
                                string.IsNullOrWhiteSpace(new_addr.Line2))
                                continue;

                            else
                            {
                                hasBilling = true;
                                continue;
                            }
                        }

                        else if (new_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.Visiting)
                        {
                            hasVisiting = true;
                            continue;
                        }

                        else if (new_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.ShipTo)
                        {
                            hasShipping = true;
                            continue;
                        }
                    }

                    foreach (var old_addr in oldAcc_address)
                    {
                        if ((!hasBilling && old_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.BillTo) ||
                                (!hasVisiting && old_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.Visiting) ||
                                (!hasShipping && old_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.ShipTo) ||
                                old_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.Other ||
                                old_addr.AddressTypeCode == Generated.customeraddress_addresstypecode.Primary)
                        {

                            var copy_address = TestData_CustomerAddress(localContext, childAcc.Id, old_addr.AddressTypeCode.Value, old_addr.Name,
                                old_addr.Line2, old_addr.PostalCode, old_addr.City);
                            Console.WriteLine($"Creating an address with type '{copy_address.AddressTypeCode}'");

                            /*
                             * Fetch CustomerAddressEntity based on the recently created one and validate its data with currently active element in
                             * outer foreach loop.
                             */
                            var updatedCustomerAddress_KST = XrmRetrieveHelper.Retrieve<CustomerAddressEntity>(localContext,
                                new EntityReference(CustomerAddressEntity.EntityLogicalName, copy_address.Id),
                                new ColumnSet(CustomerAddressEntity.Fields.AddressTypeCode,
                                CustomerAddressEntity.Fields.Name,
                                CustomerAddressEntity.Fields.Line2,
                                CustomerAddressEntity.Fields.PostalCode,
                                CustomerAddressEntity.Fields.City));

                            Assert.AreEqual(old_addr.AddressTypeCode, updatedCustomerAddress_KST.AddressTypeCode);
                            Assert.AreEqual(childAcc.Id, updatedCustomerAddress_KST.ParentId.Id);

                        }
                    }
                }
                


                #endregion

                #region Step 5 - Update accounts

                #region Step 4 - Move values & assign new values

                var updatePortalAccount_KST = new AccountEntity()
                {
                    Id = childAcc.Id,
                    ed_AccountRefNumber = oldAcc.AccountNumber,
                    ed_MigratedOrganisation = true
                };

                if (string.IsNullOrWhiteSpace(childAcc.cgi_rsid))
                    updatePortalAccount_KST.cgi_rsid = oldAcc.cgi_rsid;
                if (string.IsNullOrWhiteSpace(childAcc.cgi_responsibility))
                    updatePortalAccount_KST.cgi_responsibility = oldAcc.cgi_responsibility;
                if (string.IsNullOrWhiteSpace(childAcc.cgi_organization_sub_number))
                    updatePortalAccount_KST.cgi_organization_sub_number = oldAcc.cgi_organization_sub_number;


                var updateOldAccount = new AccountEntity()
                {
                    Id = oldAcc.Id,
                    ed_AccountRefNumber = childAcc.AccountNumber,
                    ed_MigratedOrganisation = true,
                    cgi_organizational_number = oldAcc.cgi_organizational_number //New format
                };

                #endregion

                XrmHelper.Update(localContext, updateOldAccount);
                //XrmHelper.Update(localContext, updatePortalAccount_Parent);
                XrmHelper.Update(localContext, updatePortalAccount_KST);

                var accountColumnSet = new ColumnSet(
                    AccountEntity.Fields.cgi_rsid,
                    AccountEntity.Fields.cgi_responsibility,
                    AccountEntity.Fields.cgi_organizational_number,
                    AccountEntity.Fields.cgi_organization_sub_number,
                    AccountEntity.Fields.ed_AccountRefNumber,
                    AccountEntity.Fields.ed_MigratedOrganisation);

                var oldUpdatedAcc = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext,
                    new EntityReference(AccountEntity.EntityLogicalName, oldAcc.Id), accountColumnSet);
                var newUpdatedPortalAcc_Parent = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext,
                    new EntityReference(AccountEntity.EntityLogicalName, newAcc.Id), accountColumnSet);
                var newUpdatedPortalAcc_KST = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext,
                    new EntityReference(AccountEntity.EntityLogicalName, childAcc.Id), accountColumnSet);

                //Assert Old Account
                Assert.AreEqual(updateOldAccount.ed_AccountRefNumber, oldUpdatedAcc.ed_AccountRefNumber);
                Assert.AreEqual(updateOldAccount.ed_MigratedOrganisation, oldUpdatedAcc.ed_MigratedOrganisation);
                Assert.AreEqual(updateOldAccount.cgi_organizational_number, oldUpdatedAcc.cgi_organizational_number);

                //Assert Portal Accunt  Parent
                Assert.AreEqual(newAcc.cgi_organizational_number, oldUpdatedAcc.cgi_organizational_number);

                //Assert Portal Account KST
                if (string.IsNullOrWhiteSpace(childAcc.cgi_rsid))
                    Assert.AreEqual(updatePortalAccount_KST.cgi_rsid, newUpdatedPortalAcc_KST.cgi_rsid);
                else Assert.AreEqual(childAcc.cgi_rsid, newUpdatedPortalAcc_KST.cgi_rsid);

                if (string.IsNullOrWhiteSpace(childAcc.cgi_responsibility))
                    Assert.AreEqual(updatePortalAccount_KST.cgi_responsibility, newUpdatedPortalAcc_KST.cgi_responsibility);
                else Assert.AreEqual(childAcc.cgi_responsibility, newUpdatedPortalAcc_KST.cgi_responsibility);

                if (string.IsNullOrWhiteSpace(childAcc.cgi_organization_sub_number))
                    Assert.AreEqual(updatePortalAccount_KST.cgi_organization_sub_number, newUpdatedPortalAcc_KST.cgi_organization_sub_number);
                else Assert.AreEqual(childAcc.cgi_organization_sub_number, newUpdatedPortalAcc_KST.cgi_organization_sub_number);

                Assert.AreEqual(updatePortalAccount_KST.ed_AccountRefNumber, newUpdatedPortalAcc_KST.ed_AccountRefNumber);
                Assert.AreEqual(updatePortalAccount_KST.ed_MigratedOrganisation, newUpdatedPortalAcc_KST.ed_MigratedOrganisation);

                #endregion

                #region Step 6 - Check if old acc has related travel cards & Inactivate

                var travelCards = TestDataHelper.GetTravelCards_ByAccount(localContext, oldAcc.Id);
                bool deactivate_Account = false;
                foreach (var travelCard in travelCards)
                {
                    var card = Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.cgi_travelcardnumber);
                    var parsedCard = Skanetrafiken.Crm.ValueCodes.ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, card);

                    //If any card isn't hotlisted then don't deactivate account.
                    if (!parsedCard.CardHotlistedField)
                        deactivate_Account = true;
                }

                if (deactivate_Account)
                {

                    if (oldAcc.StateCode == Generated.AccountState.Inactive)
                        return;

                    var setStateRequest = new SetStateRequest()
                    {
                        EntityMoniker = new EntityReference(AccountEntity.EntityLogicalName, oldAcc.Id),
                        State = new OptionSetValue((int)Generated.AccountState.Inactive),
                        Status = new OptionSetValue(2)
                    };

                    try
                    {
                        var resp = (SetStateResponse)localContext.OrganizationService.Execute(setStateRequest);
                    }
                    catch (Exception e)
                    {
                        localContext.Trace($"Unexpected exception caught when attempting to inactivate Account with Id: {oldAcc.Id}, exception message:\n{e.Message}");
                        throw e;
                    }

                    var oldAcc_checkState = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext,
                        new EntityReference(AccountEntity.EntityLogicalName, oldAcc.Id),
                        new ColumnSet(
                            AccountEntity.Fields.StateCode,
                            AccountEntity.Fields.StatusCode));

                    Assert.AreEqual(Generated.AccountState.Inactive, oldAcc_checkState.StateCode);
                    Assert.AreEqual(2, oldAcc_checkState.StatusCode.Value);
                }

                else
                {

                    var oldAcc_checkState = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext,
                        new EntityReference(AccountEntity.EntityLogicalName, oldAcc.Id),
                        new ColumnSet(
                            AccountEntity.Fields.StateCode,
                            AccountEntity.Fields.StatusCode));
                    Assert.AreEqual(Generated.AccountState.Active, oldAcc_checkState.StateCode);
                    Assert.AreEqual(1, oldAcc_checkState.StatusCode.Value);
                }

                #endregion
            }
        }


        [Test]
        public void GetAddresses()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var addressColumnSet = new ColumnSet(
                        CustomerAddressEntity.Fields.AddressTypeCode,
                        CustomerAddressEntity.Fields.Name,
                        CustomerAddressEntity.Fields.Line2,
                        CustomerAddressEntity.Fields.PostalCode,
                        CustomerAddressEntity.Fields.City,
                        CustomerAddressEntity.Fields.ParentId);

                var oldAcc_address = TestDataHelper.GetCustomerAddresses_ByParentId(localContext, Guid.Parse("18DDFF4A-0AC4-E911-80F0-005056B61FFF"),
                    addressColumnSet);

            }
        }

        [Test]
        public void Test_CreatePortalAccount_WithKST()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                TestDataHelper.DeleteAccount_ByName(localContext, c_AccountName, c_AccountName_KST);

                var oldAcc = TestData_CreateAccount_Old(localContext);
                var parentAcc = TestData_CreateAccount_Parent(localContext, c_oldAccountNumber);
                var childAcc = TestData_CreateAccount_KST(localContext, parentAcc.Id, c_newAccountNumber_Parent);

                TestDataHelper.DeleteAccount_ByName(localContext, c_AccountName, c_AccountName_KST);
            }
        }


        /// <summary>
        /// Validate/format organization number.
        /// </summary>
        [Test, Category("Debug")]
        public void Test_OrganizationNumber_Formatting()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                NUnit.Framework.Assert.Throws<Exception>(() => FormatOrganizationNumber(""));

                NUnit.Framework.Assert.Throws<Exception>(() => FormatOrganizationNumber("312"));
                NUnit.Framework.Assert.Throws<Exception>(() => FormatOrganizationNumber("31216125323215"));
                NUnit.Framework.Assert.Throws<Exception>(() => FormatOrganizationNumber("12562-2352"));

                NUnit.Framework.Assert.AreEqual("1234567890", FormatOrganizationNumber("123456-7890"));
                NUnit.Framework.Assert.AreEqual("1234567890", FormatOrganizationNumber("123s45e6-78g9q0"));
                NUnit.Framework.Assert.AreEqual("1234567890", FormatOrganizationNumber("1234567890"));

            }
        }

        /// <summary>
        /// Cannot remove customeraddressentity
        /// </summary>
        [Test]
        public void Test_RemoveAddressFromAccount()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                TestDataHelper.DeleteAccount_ByName(localContext, c_AccountName_KST);

                var account = TestData_CreateAccount_Old(localContext);
                TestData_CustomerAddress(localContext, account.Id, Generated.customeraddress_addresstypecode.BillTo);

                TestDataHelper.DeleteCustomerAddress_ByParentId(localContext, account.Id);
            }
        }

        public string Format_ToLowerAndReplaceWhitespace(string str)
        {
            return str.ToLower().Replace(" ", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        private AccountEntity TestData_CreateAccount_Old(Plugin.LocalPluginContext localContext, string rsid = null, string resp = null, string org_sub = null, bool isInactive = false)
        {
            try
            {
                //Create an account
                var account = new AccountEntity()
                {
                    Name = c_AccountName,
                    cgi_organizational_number = c_oldAccountOrganizationNumber_FaultyFormat,
                    cgi_rsid = rsid != null ? rsid : "",
                    cgi_responsibility = resp != null ? resp : "",
                    cgi_organization_sub_number = org_sub != null ? org_sub : "",
                    AccountNumber = c_oldAccountNumber
                };

                account.Id = XrmHelper.Create(localContext, account);

                if (isInactive)
                {
                    var setStateRequest = new SetStateRequest()
                    {
                        EntityMoniker = new EntityReference(AccountEntity.EntityLogicalName, account.Id),
                        State = new OptionSetValue((int)Generated.AccountState.Inactive),
                        Status = new OptionSetValue(2)
                    };

                    var response = (SetStateResponse)localContext.OrganizationService.Execute(setStateRequest);
                }

                return account;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private AccountEntity TestData_CreateAccount_Parent(Plugin.LocalPluginContext localContext, string accountNumber)
        {
            var parentAcc = new AccountEntity()
            {
                Name = c_AccountName,
                cgi_organizational_number = c_newAccountOrgnr_CorrectFormat,
                ed_TypeOfAccount = Generated.ed_account_ed_typeofaccount.Companycustomerportal,
                AccountNumber = accountNumber
            };

            parentAcc.Id = XrmHelper.Create(localContext, parentAcc);

            return parentAcc;
        }

        private AccountEntity TestData_CreateAccount_KST(Plugin.LocalPluginContext localContext, Guid parentAccountId, string accountNumber, string rsid = null, string resp = null, string org_sub = null)
        {
            var childAcc = new AccountEntity()
            {
                Name = c_AccountName_KST,
                ParentAccountId = new EntityReference(AccountEntity.EntityLogicalName, parentAccountId),
                ed_TypeOfAccount = Generated.ed_account_ed_typeofaccount.Companycustomerportal,
                AccountNumber = accountNumber,
                cgi_rsid = rsid != null ? rsid : "",
                cgi_responsibility = resp != null ? resp : "",
                cgi_organization_sub_number = org_sub != null ? org_sub : ""
            };

            childAcc.Id = XrmHelper.Create(localContext, childAcc);

            return childAcc;
        }


        private TravelCardEntity TestData_TravelCard(Plugin.LocalPluginContext localContext, string travelCardName, Guid accountId, string travelCardNumber)
        {
            //Create travel card and assign it to old account
            var travelCard = new TravelCardEntity()
            {
                cgi_TravelCardName = travelCardName,
                cgi_travelcardnumber = travelCardNumber,
                cgi_Accountid = new EntityReference(AccountEntity.EntityLogicalName, accountId)
            };

            XrmHelper.Create(localContext, travelCard);

            return travelCard;
        }

        private CustomerAddressEntity TestData_CustomerAddress(Plugin.LocalPluginContext localContext, Guid parentId,
            Generated.customeraddress_addresstypecode type, string name = null, string line2 = null, string postalCode = null, string city = null)
        {
            var address = new CustomerAddressEntity()
            {
                AddressTypeCode = type,
                Name = name == null ? c_address_name : name,
                Line2 = line2 == null ? c_Address_Line2 : line2,
                PostalCode = postalCode == null ? c_postalCode : postalCode,
                City = city == null ? c_city : city,
                ParentId = new EntityReference(AccountEntity.EntityLogicalName, parentId)
            };

            address.Id = XrmHelper.Create(localContext, address);

            return address;
        }


        private string FormatOrganizationNumber(string orgNr)
        {
            if (string.IsNullOrWhiteSpace(orgNr))
                throw new Exception("Argument does not contain value.");
            if (orgNr.Length < 10)
                throw new Exception("Argument cannot contain less than 10 characters.");


            if (!int.TryParse(orgNr, out int correctOrgNr))
            {
                var stringArr = orgNr.ToCharArray();
                foreach (var s in stringArr)
                {
                    if (!char.IsDigit(s))
                        orgNr = orgNr.Replace($"{s}", "");
                }

                if (orgNr.Length != 10)
                    throw new Exception("Organization number has to be exactly 10 numbers.");

                return orgNr;
            }
            else return correctOrgNr.ToString();
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }
    }
}
