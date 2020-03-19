using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;

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
            AccountEntity.Fields.ed_AllowCreate
            );
        
        internal static AccountEntity FindAccount(Plugin.LocalPluginContext localContext, string accountNumber, ColumnSet columns)
        {
            IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, columns,
                                new FilterExpression()
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, accountNumber)
                                    }
                                });

            if (accounts != null && accounts.Count > 1)
            {
                throw new Exception($"Multiple accounts with AccountNumber {accountNumber}");
            }

            if (accounts != null && accounts.Count == 0)
            {
                throw new Exception($"Found no accounts with AccountNumber {accountNumber}.");
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

            foreach(AccountEntity account in accounts)
            {
                account.ed_IsLockedPortal = blocked;
                XrmHelper.Update(localContext, account);
            }

            return accounts.Count;

        }
    }
}