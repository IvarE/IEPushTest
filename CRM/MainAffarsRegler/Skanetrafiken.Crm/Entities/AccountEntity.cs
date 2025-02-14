﻿using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using System.Net;
using System.Net.Http;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using System.Globalization;
using System.IdentityModel;
using System.Linq;

namespace Skanetrafiken.Crm.Entities
{
    public class AccountEntity : Generated.Account
    {

        internal static ColumnSet AccountInfoBlock = new ColumnSet(
            AccountEntity.Fields.AccountNumber,
            AccountEntity.Fields.Name,
            AccountEntity.Fields.cgi_organizational_number,
            AccountEntity.Fields.ed_PaymentMethod,
            AccountEntity.Fields.ed_IsLockedPortal,
            AccountEntity.Fields.EMailAddress1,
            AccountEntity.Fields.ed_CostSite,
            AccountEntity.Fields.ed_BillingEmailAddress,
            AccountEntity.Fields.ed_BillingMethod,
            AccountEntity.Fields.StateCode,
            AccountEntity.Fields.ed_AllowCreate,
            AccountEntity.Fields.ed_AccountDescription,
            AccountEntity.Fields.ed_SubOrgNamn
            );

        internal static AccountEntity FindAccount(Plugin.LocalPluginContext localContext, string accountNumber, ColumnSet columns)
        {
            IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, columns,
                new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, accountNumber),
                        new ConditionExpression(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active)
                    }
                });

            if (accounts != null && accounts.Count > 1)
            {
                throw new BadRequestException($"Multiple accounts with AccountNumber {accountNumber}.");
                //throw new Exception($"Multiple accounts with AccountNumber {accountNumber}.");
            }

            if (accounts != null && accounts.Count == 0)
            {
                throw new BadRequestException($"Found no accounts with AccountNumber {accountNumber}.");
                //throw new Exception($"Found no accounts with AccountNumber {accountNumber}.");
            }

            return accounts[0];
        }

        internal static int BlockUnderlyingCostSites(Plugin.LocalPluginContext localContext, string orgNr, bool blocked)
        {
            IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(false),
                new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, orgNr),
                        new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.NotNull)
                    }
                });

            foreach (AccountEntity account in accounts)
            {
                account.ed_IsLockedPortal = blocked;
                XrmHelper.Update(localContext, account);
            }

            return accounts.Count;

        }

        /// <summary>
        /// Merge old account to portal KST
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAccNumber"></param>
        /// <param name="newAccNumber_KST"></param>
        internal static void HandleMergeOldAccountWithKST(Plugin.LocalPluginContext localContext, string oldAccNumber, string newAccNumber_KST)
        {
            var accounts = FetchAccounts(localContext, oldAccNumber, newAccNumber_KST);
            ValidateAndAssignAddresses(localContext, accounts.Item1.Id, accounts.Item2.Id);
            UpdateAccounts(localContext, accounts.Item1, accounts.Item2);
        }

        /// <summary>
        /// Fetches old account and portal KST account & formats organization number
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAccNumber"></param>
        /// <param name="newAccNumber_KST"></param>
        /// <returns></returns>
        private static Tuple<AccountEntity, AccountEntity> FetchAccounts(Plugin.LocalPluginContext localContext, string oldAccNumber, string newAccNumber_KST)
        {
            var filterAccount = new QueryExpression()
            {
                EntityName = AccountEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(AccountEntity.Fields.cgi_rsid,
                     AccountEntity.Fields.cgi_organizational_number,
                     AccountEntity.Fields.cgi_responsibility,
                     AccountEntity.Fields.cgi_organization_sub_number,
                     AccountEntity.Fields.StateCode,
                     AccountEntity.Fields.StatusCode,
                     AccountEntity.Fields.ParentAccountId,
                     AccountEntity.Fields.AccountNumber),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, oldAccNumber)
                    }
                }
            };

            //Old Account
            var oldAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
            if (oldAcc == null)
                throw new NullReferenceException($"Angiven kontonr '{oldAccNumber}' är inte kopplat till ett konto.");

            oldAcc.cgi_organizational_number = FormatOrganizationNumber(oldAcc.cgi_organizational_number);


            //New Portal Account - KST
            filterAccount.Criteria = null;
            filterAccount.Criteria = new FilterExpression
            {
                Conditions =
                    {
                        new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, newAccNumber_KST),
                        new ConditionExpression(AccountEntity.Fields.ParentAccountId, ConditionOperator.NotNull),
                    }
            };
            var childAcc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, filterAccount);
            if (childAcc == null)
                throw new NullReferenceException($"Could not find KST account with accountnumber '{newAccNumber_KST}' or account does not have a parent account.");

            if (!string.IsNullOrWhiteSpace(childAcc.cgi_organizational_number))
                throw new InvalidPluginExecutionException($"KST account '{childAcc.Id}' should not contain organization number.");


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
                throw new NullReferenceException($"Could not find parent account with org nr '{oldAcc.cgi_organizational_number}'.");


            ////Check if org-nr matches
            if (!oldAcc.cgi_organizational_number.Equals(newAcc.cgi_organizational_number))
                throw new InvalidPluginExecutionException($"Organization number '{oldAcc.cgi_organizational_number}' from '{oldAcc.Id}' does not match organization number with account '{newAcc.Id}'");




            return new Tuple<AccountEntity, AccountEntity>(oldAcc, childAcc);
        }

        /// <summary>
        /// Migrates addresses from old account to portal KST account
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAccId"></param>
        /// <param name="newAccId_KST"></param>
        private static void ValidateAndAssignAddresses(Plugin.LocalPluginContext localContext, Guid oldAccId, Guid newAccId_KST)
        {
            localContext.TracingService.Trace($"Entering ValidateAndAssignAddresses");
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
            filterAddress.AddCondition(new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, oldAccId));
            var oldAcc_address = CustomerAddressEntity.GetCustomerAddresses_ByParentId(localContext, oldAccId, addressColumnSet, filterAddress);

            filterAddress = new FilterExpression
            {
                Conditions = {
                            new ConditionExpression(CustomerAddressEntity.Fields.AddressTypeCode, ConditionOperator.NotNull),
                }
            };
            filterAddress.AddCondition(new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, newAccId_KST));
            var newAcc_KST_address = CustomerAddressEntity.GetCustomerAddresses_ByParentId(localContext, newAccId_KST, addressColumnSet, filterAddress);

            bool hasBilling = false, hasVisiting = false, hasShipping = false, hasInitiallyNoAddresses = false;
            hasInitiallyNoAddresses = newAcc_KST_address.Count <= 1 ? true : false; //When an account is created, it will be associated with an empty address with BillTo as AddressTypeCode
            bool hasUpdated = false;

            localContext.TracingService.Trace($"KST hasInitiallyNoAddresses = {hasInitiallyNoAddresses}");
            if (hasInitiallyNoAddresses)
            {
                foreach (var old_addr in oldAcc_address)
                {

                    if (string.IsNullOrWhiteSpace(old_addr.Name) &&
                        string.IsNullOrWhiteSpace(old_addr.PostalCode) &&
                        string.IsNullOrWhiteSpace(old_addr.City) &&
                        string.IsNullOrWhiteSpace(old_addr.Line2))
                        continue;

                    var createNewAddress = true;

                    CustomerAddressEntity addr_ = new CustomerAddressEntity();
                    if (!hasUpdated)
                    {
                        addr_ = newAcc_KST_address[0];
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
                            ParentId = new EntityReference(AccountEntity.EntityLogicalName, newAccId_KST),
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
                            addr_.ParentId = new EntityReference(AccountEntity.EntityLogicalName, newAccId_KST);
                        XrmHelper.Update(localContext, addr_);

                        hasUpdated = true;
                    }

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

                        var address = new CustomerAddressEntity()
                        {
                            AddressTypeCode = old_addr.AddressTypeCode,
                            Name = old_addr.Name,
                            Line2 = old_addr.Line2,
                            PostalCode = old_addr.PostalCode,
                            City = old_addr.City,
                            ParentId = new EntityReference(AccountEntity.EntityLogicalName, newAccId_KST)
                        };

                        XrmHelper.Create(localContext, address);
                    }
                }
            }
        }

        /// <summary>
        /// Updates both old account and KST
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAcc"></param>
        /// <param name="newAcc_KST"></param>
        private static void UpdateAccounts(Plugin.LocalPluginContext localContext, AccountEntity oldAcc, AccountEntity newAcc_KST)
        {

            var updatePortalAccount_KST = new AccountEntity()
            {
                Id = newAcc_KST.Id,
                ed_AccountRefNumber = oldAcc.AccountNumber,
                ed_MigratedOrganisation = true
            };

            if (string.IsNullOrWhiteSpace(newAcc_KST.cgi_rsid))
                updatePortalAccount_KST.cgi_rsid = oldAcc.cgi_rsid;
            if (string.IsNullOrWhiteSpace(newAcc_KST.cgi_responsibility))
                updatePortalAccount_KST.cgi_responsibility = oldAcc.cgi_responsibility;
            if (string.IsNullOrWhiteSpace(newAcc_KST.cgi_organization_sub_number))
                updatePortalAccount_KST.cgi_organization_sub_number = oldAcc.cgi_organization_sub_number;

            var updateOldAccount = new AccountEntity()
            {
                Id = oldAcc.Id,
                ed_AccountRefNumber = newAcc_KST.AccountNumber,
                ed_MigratedOrganisation = true,
                cgi_organizational_number = oldAcc.cgi_organizational_number //New format
            };

            XrmHelper.Update(localContext, updatePortalAccount_KST);
            XrmHelper.Update(localContext, updateOldAccount);

            if (!DoesAccountHaveActiveTravelCard(localContext, oldAcc))
                DeactivateAccount(localContext, oldAcc);
        }

        internal void HandlePreAccountCreate(Plugin.LocalPluginContext localContext)
        {
            string orgNumber = this.cgi_organizational_number;

            if (this.PrimaryContactId != null)
            {
                this.setContactNamePlaceholder(localContext, null);
            }

            localContext.Trace($"Entered HandlePreAccountCreate() OrgNumber value: {orgNumber}");
            if (!string.IsNullOrWhiteSpace(orgNumber))
            {
                bool isUnique = CheckOrgNumberisUnique(localContext, orgNumber);

                if (!isUnique)
                    throw new InvalidPluginExecutionException("Detta organisationsnummer finns redan.");

                EntityReference erParentAccount = this.ParentAccountId;
                bool isAllowed = CheckParentAccountOrganizationNumber(localContext, erParentAccount);

                if (!isAllowed)
                    throw new InvalidPluginExecutionException("Det går inte att ange organisationsnummer på ett underliggande konto");

                this.cgi_organizational_number = FormatOrgNumber(localContext, orgNumber);
            }

            string postalCode = this.Address1_PostalCode;
            string city = this.Address1_City;

            localContext.Trace($"Entered HandlePreAccountCreate() Postal Code: {postalCode} City: {city}");
            if (!string.IsNullOrEmpty(postalCode))
            {
                QueryExpression queryPostalCodes = new QueryExpression(PostalCodesEntity.EntityLogicalName);
                queryPostalCodes.NoLock = true;
                queryPostalCodes.ColumnSet.AddColumns(PostalCodesEntity.Fields.ed_Kommun, PostalCodesEntity.Fields.ed_Kommunkod, PostalCodesEntity.Fields.ed_Lan,
                    PostalCodesEntity.Fields.ed_Lanskod, PostalCodesEntity.Fields.ed_name, PostalCodesEntity.Fields.ed_Postort);
                queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postnummer, ConditionOperator.Equal, postalCode);
                if (!string.IsNullOrEmpty(city))
                    queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postort, ConditionOperator.Equal, city);

                List<PostalCodesEntity> lPostalCodes = XrmRetrieveHelper.RetrieveMultiple<PostalCodesEntity>(localContext, queryPostalCodes);

                if (lPostalCodes.Count > 1)
                    localContext.Trace($"HandlePreAccountCreate: Found multiple Postal Codes with ZIP Code: {postalCode} and City: {city}");
                else if (lPostalCodes.Count == 0)
                    localContext.Trace($"HandlePreAccountCreate: No Postal Codes with ZIP Code: {postalCode} and City: {city}");
                else
                {
                    PostalCodesEntity ePostalCode = lPostalCodes.FirstOrDefault();

                    this.Address1_StateOrProvince = ePostalCode.ed_Kommun;
                    this.ed_Address1_CommunityNumber = int.Parse(ePostalCode.ed_Kommunkod);
                    this.Address1_County = ePostalCode.ed_Lan;
                    this.ed_Address1_CountyNumber = int.Parse(ePostalCode.ed_Lanskod);
                    this.Address1_Name = ePostalCode.ed_name;
                    this.Address1_City = ePostalCode.ed_Postort;
                }
            }
        }

        internal void setContactNamePlaceholder(Plugin.LocalPluginContext localContext, AccountEntity preImage)
        {
            ContactEntity thisContact = null;
            AccountEntity updatedAccount = new AccountEntity();

            if (this.PrimaryContactId != null)
            {
                if (this.PrimaryContactId != null)
                {

                    thisContact = XrmRetrieveHelper.Retrieve<ContactEntity>(
                    localContext,
                    this.PrimaryContactId.Id,
                    new ColumnSet(ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName)
                    );
                }
                else
                {
                    return;
                }
            }
            else if(this.PrimaryContactId == null && preImage != null)
            {
                if (preImage.PrimaryContactId != null)
                {
                  thisContact = XrmRetrieveHelper.Retrieve<ContactEntity>(
                  localContext,
                  preImage.PrimaryContactId.Id,
                  new ColumnSet(ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName)
                  );
                }
                else
                {
                    return;
                }
            }

            this.ed_FirstNamePlaceholder = thisContact.FirstName;
            this.ed_LastNamePlaceholder = thisContact.LastName;

            if(preImage != null)
            {
                updatedAccount.Id = this.Id;

                updatedAccount.ed_FirstNamePlaceholder = thisContact.FirstName;
                updatedAccount.ed_LastNamePlaceholder = thisContact.LastName;

                XrmHelper.Update(localContext, updatedAccount);
            }
            


        }

        internal void HandlePreAccountUpdate(Plugin.LocalPluginContext localContext, AccountEntity preImage)
        {
            string orgNumber = this.cgi_organizational_number;

            if (this.PrimaryContactId != null || preImage.PrimaryContactId != null)
            {
                this.setContactNamePlaceholder(localContext, preImage);
            }

            localContext.Trace($"Entered HandlePreAccountUpdate() OrgNumber value: {orgNumber}");
            if (!string.IsNullOrWhiteSpace(orgNumber))
            {
                bool isUnique = CheckOrgNumberisUnique(localContext, orgNumber);

                if (!isUnique)
                    throw new InvalidPluginExecutionException("Detta organisationsnummer finns redan.");

                EntityReference erParentAccount = preImage.ParentAccountId;
                bool isAllowed = CheckParentAccountOrganizationNumber(localContext, erParentAccount);

                if (!isAllowed)
                    throw new InvalidPluginExecutionException("Det går inte att ange organisationsnummer på ett underliggande konto");

                this.cgi_organizational_number = FormatOrgNumber(localContext, orgNumber);
            }

            string postalCode = this.Address1_PostalCode != null ? this.Address1_PostalCode : preImage.Address1_PostalCode;
            string city = this.Address1_City != null ? this.Address1_City : preImage.Address1_City;

            localContext.Trace($"Entered HandlePreAccountUpdate() Postal Code: {postalCode} and City: {city}");
            if (!string.IsNullOrEmpty(postalCode))
            {
                QueryExpression queryPostalCodes = new QueryExpression(PostalCodesEntity.EntityLogicalName);
                queryPostalCodes.NoLock = true;
                queryPostalCodes.ColumnSet.AddColumns(PostalCodesEntity.Fields.ed_Kommun, PostalCodesEntity.Fields.ed_Kommunkod, PostalCodesEntity.Fields.ed_Lan,
                    PostalCodesEntity.Fields.ed_Lanskod, PostalCodesEntity.Fields.ed_name, PostalCodesEntity.Fields.ed_Postort);
                queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postnummer, ConditionOperator.Equal, postalCode);
                if (!string.IsNullOrEmpty(city))
                    queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postort, ConditionOperator.Equal, city);

                List<PostalCodesEntity> lPostalCodes = XrmRetrieveHelper.RetrieveMultiple<PostalCodesEntity>(localContext, queryPostalCodes);

                if (lPostalCodes.Count > 1)
                    localContext.Trace($"HandlePreAccountUpdate: Found multiple Postal Codes with ZIP Code: {postalCode} and City: {city}");
                else if (lPostalCodes.Count == 0)
                    localContext.Trace($"HandlePreAccountUpdate: No Postal Codes with ZIP Code: {postalCode} and City: {city}");
                else
                {
                    PostalCodesEntity ePostalCode = lPostalCodes.FirstOrDefault();

                    this.Address1_StateOrProvince = ePostalCode.ed_Kommun;
                    this.ed_Address1_CommunityNumber = int.Parse(ePostalCode.ed_Kommunkod);
                    this.Address1_County = ePostalCode.ed_Lan;
                    this.ed_Address1_CountyNumber = int.Parse(ePostalCode.ed_Lanskod);
                    this.Address1_Name = ePostalCode.ed_name;
                    this.Address1_City = ePostalCode.ed_Postort;
                }
            }
        }

        #region Helpers
        private static bool CheckParentAccountOrganizationNumber(Plugin.LocalPluginContext localContext, EntityReference parentAccount)
        {
            localContext.Trace($"Entered CheckParentAccountOrganizationNumber.");
            bool isAllowed = true;
            if (parentAccount != null)
            {
                localContext.Trace($"Parent Account Id: {parentAccount.Id}");
                AccountEntity eAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, parentAccount.Id, new ColumnSet(AccountEntity.Fields.cgi_organizational_number));

                if (eAccount != null && eAccount.cgi_organizational_number != null)
                    return false;
            }
            else
            {
                localContext.Trace($" Parent Account Id is null");
            }
            localContext.Trace($"Leaving CheckParentAccountOrganizationNumber.");
            return isAllowed;
        }

        private static bool CheckOrgNumberisUnique(Plugin.LocalPluginContext localContext, string orgNumber)
        {
            localContext.Trace($"Entered CheckOrgNumberisUnique.");
            bool isUnique = true;

            QueryExpression queryAccounts = new QueryExpression(AccountEntity.EntityLogicalName);
            queryAccounts.NoLock = true;
            queryAccounts.ColumnSet = new ColumnSet(AccountEntity.Fields.cgi_organizational_number);
            queryAccounts.Criteria.AddCondition(AccountEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.AccountState.Active);
            queryAccounts.Criteria.AddCondition(AccountEntity.Fields.cgi_organizational_number, ConditionOperator.Equal, orgNumber);

            var lAccounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, queryAccounts);

            if (lAccounts.Count > 0)
                isUnique = false;

            localContext.Trace($"Leaving CheckOrgNumberisUnique.");
            return isUnique;
        }

        private static string FormatOrgNumber(Plugin.LocalPluginContext localContext, string orgNumber)
        {
            localContext.Trace($"Entered FormatOrgNumber. OrgNumber value: {orgNumber}");
            if (orgNumber.Length < 10)
                throw new InvalidPluginExecutionException("Organisationsnummer innehåller mindre än 10 tecken.");
            foreach (char c in orgNumber.ToCharArray())
            {
                if (!char.IsDigit(c))
                    orgNumber = orgNumber.Replace(string.Format("{0}", (object)c), "");
            }
            if (orgNumber.Length != 10)
                throw new InvalidPluginExecutionException("Organisationsnummer måste vara exakt 10 siffror.");
            localContext.Trace($"Leaving FormatOrgNumber. OrgNumber value: {orgNumber}");
            return orgNumber;
        }

        public static AccountEntity GetAccountByPortalId(Plugin.LocalPluginContext localContext, ColumnSet columnSet, string portalId)
        {
            AccountEntity account = null;

            if (!String.IsNullOrWhiteSpace(portalId))
            {
                //Check if account exists in CRM.
                var accountQuery = new QueryExpression()
                {
                    EntityName = AccountEntity.EntityLogicalName,
                    ColumnSet = columnSet,
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, portalId)
                        }
                    }
                };

                account = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, accountQuery);

                // Make sure Account is active
                if (account.StateCode != Generated.AccountState.Active)
                    return null;
            }

            return account;
        }

        /// <summary>
        /// Checkes whether given account has an active travel card (non hotlisted in BIFF)
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAcc"></param>
        /// <returns></returns>
        private static bool DoesAccountHaveActiveTravelCard(Plugin.LocalPluginContext localContext, AccountEntity oldAcc)
        {
            var travelCards = TravelCardEntity.GetTravelCards_ByAccount(localContext, oldAcc.Id);
            foreach (var travelCard in travelCards)
            {
                var card = ValueCodes.ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.cgi_travelcardnumber);
                if (string.IsNullOrWhiteSpace(card))
                    throw new InvalidPluginExecutionException("Exception caught when trying to get card details from BIFF.");

                var parsedCard = ValueCodes.ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, card);
                if (parsedCard == null)
                    throw new InvalidPluginExecutionException("Exception caught when trying to parse data from BIFF.");

                //If any card isn't hotlisted then don't deactivate account.
                if (!parsedCard.CardHotlistedField)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Deactivates given account.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="oldAcc"></param>
        private static void DeactivateAccount(Plugin.LocalPluginContext localContext, AccountEntity oldAcc)
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
        }

        /// <summary>
        /// Formats organization number to correct format.
        /// </summary>
        /// <param name="orgNr"></param>
        /// <returns></returns>
        private static string FormatOrganizationNumber(string orgNr)
        {
            if (string.IsNullOrWhiteSpace(orgNr))
                throw new InvalidPluginExecutionException("Argument does not contain value.");
            if (orgNr.Length < 10)
                throw new InvalidPluginExecutionException("Argument cannot contain less than 10 characters.");
            int result;
            if (int.TryParse(orgNr, out result))
                return result.ToString();
            foreach (char c in orgNr.ToCharArray())
            {
                if (!char.IsDigit(c))
                    orgNr = orgNr.Replace(string.Format("{0}", (object)c), "");
            }
            if (orgNr.Length != 10)
                throw new InvalidPluginExecutionException("Organization number has to be exactly 10 numbers.");
            return orgNr;
        }

        private static bool DoesAddressMatch(CustomerAddressEntity old, CustomerAddressEntity new_addr)
        {
            string replaceWhitespace1 = Format_ToLowerAndReplaceWhitespace(old.Name);
            string replaceWhitespace2 = Format_ToLowerAndReplaceWhitespace(old.Line2);
            string replaceWhitespace3 = Format_ToLowerAndReplaceWhitespace(old.PostalCode);
            string replaceWhitespace4 = Format_ToLowerAndReplaceWhitespace(old.City);
            string replaceWhitespace5 = Format_ToLowerAndReplaceWhitespace(new_addr.Name);
            string replaceWhitespace6 = Format_ToLowerAndReplaceWhitespace(new_addr.Line2);
            string replaceWhitespace7 = Format_ToLowerAndReplaceWhitespace(new_addr.PostalCode);
            string replaceWhitespace8 = Format_ToLowerAndReplaceWhitespace(new_addr.City);
            return replaceWhitespace1 == replaceWhitespace5 && replaceWhitespace2 == replaceWhitespace6 && (replaceWhitespace3 == replaceWhitespace7 && replaceWhitespace4 == replaceWhitespace8);
        }

        private static string Format_ToLowerAndReplaceWhitespace(string str)
        {
            return str.ToLower().Replace(" ", "");
        }
        #endregion
    }
}