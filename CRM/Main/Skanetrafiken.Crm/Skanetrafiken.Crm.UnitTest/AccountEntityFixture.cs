using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Endeavor.Crm;
using Endeavor.Crm.Entities;
using Endeavor.Crm.Extensions;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture(Category = "Plugin")]
    public class AccountEntityFixture
    {
        private ServerConnection _serverConnection;

        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        private OrganizationServiceProxy _serviceProxy;

        [Test, Explicit]
        public void SimulateSingaporeIntegration()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                // 08490015 - A952 - E311 - A0C4 - 005056970008 - Marcus Testkonto 2

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(AccountEntity.Fields.Name, ConditionOperator.Equal, "Marcus Testkonto 2");
                AccountEntity acc = XrmRetrieveHelper.RetrieveFirst<AccountEntity>(localContext, new ColumnSet(true), filter);

                if (acc != null)
                {
                    localContext.TracingService.Trace("Update all attributes to simulate SSIS functionality");

                    acc.Trace(localContext.TracingService);

                    localContext.OrganizationService.Update(acc);
                }

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }


        [Test, Explicit]
        public void DeactivateDuplicates()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();


                var fetchXml = "<fetch aggregate='true' >"+
                                "<entity name = 'account' >" +
                                "<attribute name = 'accountid' aggregate = 'countcolumn' alias = 'count' />"+
                                "<attribute name = 'accountnumber' groupby = 'true' alias = 'accountnumber' />"+
                                "<order alias='count' descending='true' />" +
                                "<filter type = 'and' >" +
                                //"<condition attribute = 'accountnumber' operator= 'like' value = 'DE%' />" +
                                "<condition attribute = 'statecode' operator= 'eq' value = '0' />" +
                                "</filter >" +
                                "</entity >" +
                                "</fetch >";


                // Run the query with the FetchXML.
                // Convert the FetchXML into a query expression.
                var conversionRequest = new FetchXmlToQueryExpressionRequest
                {
                    FetchXml = fetchXml
                };
                var conversionResponse =
                    (FetchXmlToQueryExpressionResponse)localContext.OrganizationService.Execute(conversionRequest);
                // Use the newly converted query expression to make a retrieve multiple
                // request to Microsoft Dynamics CRM.
                //QueryExpression queryExpression = conversionResponse.Query;
                //IList<AccountEntity> results = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, queryExpression);
                var fetchExpression = new FetchExpression(fetchXml);
                //IList<Entity> results = XrmHelper.RetrieveMultiple(localContext.OrganizationService, conversionResponse.Query);
                EntityCollection results = localContext.OrganizationService.RetrieveMultiple(fetchExpression);

                foreach (Entity result in results.Entities)
                {
                    if(result.Contains("count") && result.Contains("accountnumber"))
                    {
                        int count = (int)((AliasedValue)result["count"]).Value;
                        string accountnumber = ((AliasedValue)result["accountnumber"]).Value.ToString();

                        if (count > 1)
                        {
                            localContext.TracingService.Trace("Found {0} records for account {1}", count, accountnumber);

                            // Get all accounts for this accountNo
                            FilterExpression filter = new FilterExpression(LogicalOperator.And);
                            filter.AddCondition(AccountEntity.Fields.AccountNumber, ConditionOperator.Equal, accountnumber);
                            filter.AddCondition(AccountEntity.Fields.StateCode, ConditionOperator.Equal, 0);
                            IList<AccountEntity> accounts = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(
                                AccountEntity.Fields.CreatedOn
                                ,AccountEntity.Fields.Telephone1
                                , AccountEntity.Fields.Fax
                                , AccountEntity.Fields.EMailAddress1
                                , AccountEntity.Fields.Address1_Line1
                                , AccountEntity.Fields.Address1_Line2
                                , AccountEntity.Fields.Address1_PostalCode
                                , AccountEntity.Fields.Address1_City
                                , AccountEntity.Fields.Address1_StateOrProvince
                                , AccountEntity.Fields.edp_Address1_CountryId
                                , AccountEntity.Fields.new_District
                                , AccountEntity.Fields.PaymentTermsCode
                                , AccountEntity.Fields.Address1_FreightTermsCode
                                , AccountEntity.Fields.new_SalesYTD
                                , AccountEntity.Fields.new_BudgetYTM
                                , AccountEntity.Fields.Address2_Line1
                                , AccountEntity.Fields.Address2_Line2
                                , AccountEntity.Fields.Address2_PostalCode
                                , AccountEntity.Fields.Address2_City
                                , AccountEntity.Fields.Address2_StateOrProvince
                                , AccountEntity.Fields.edp_Address2_CountryId
                                ), filter);

                            // Sort all records.
                            IEnumerable<AccountEntity> sorted = accounts.OrderBy(f => ((DateTime)f.CreatedOn));
                            IList<AccountEntity> sortedAccounts = sorted.ToList();

                            MergeRequest merge = new MergeRequest();
                            //merge.SubordinateId = _account2Id;
                            //merge.Target = target;
                            merge.PerformParentingChecks = false;


                            for (int i = 0; i < sortedAccounts.Count; i++)
                            {
                                if (i == 0)
                                {
                                    merge.Target = sortedAccounts[i].ToEntityReference();
                                    continue;
                                }

                                // Repair only error created this year
                                if (sortedAccounts[i].CreatedOn < new DateTime(2016, 01, 01))
                                {
                                    localContext.TracingService.Trace("Account {0} has old duplicate. Created {1} Not merged", accountnumber, sortedAccounts[i].CreatedOn.ToString());
                                    continue;
                                }

                                merge.SubordinateId = sortedAccounts[i].Id;
                                AccountEntity mergeAccount = new AccountEntity();

                                // Take all attributes with data from newer account
                                mergeAccount.CombineAttributes(sortedAccounts[i]);
                                if(mergeAccount.edp_Address1_CountryId != null)
                                {
                                    if (mergeAccount.edp_Address1_CountryId.Name != null)
                                        mergeAccount.edp_Address1_CountryId.Name = null;
                                }
                                if (mergeAccount.edp_Address2_CountryId != null)
                                {
                                    if (mergeAccount.edp_Address2_CountryId.Name != null)
                                        mergeAccount.edp_Address2_CountryId.Name = null;
                                }

                                merge.UpdateContent = mergeAccount;

                                MergeResponse mergeResp = (MergeResponse)localContext.OrganizationService.Execute(merge);
                            }

                            string text = "";

                        }
                        else
                            localContext.TracingService.Trace("No duplicate :-) {0}", accountnumber);

                    }
                }

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }


        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
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
