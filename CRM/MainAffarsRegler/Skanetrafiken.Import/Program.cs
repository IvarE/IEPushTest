using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Endeavor.Crm.Import;
using log4net;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Import.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Xml;

namespace Skanetrafiken.Import
{
    class Program
    {
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IOrganizationService ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                return (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "Error while connecting to CRM " + ex.Message);
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
                return null;
            }
        }

        public static bool MainMenuUpsales(Plugin.LocalPluginContext localContext, CrmContext crmContext, SaveChangesOptions optionsChanges, string relativeExcelPath)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Import Spann 1 Accounts");
            Console.WriteLine("2) Import Spann 5 Accounts");
            Console.WriteLine("3) Import Spann 2 Accounts");
            Console.WriteLine("4) Import Spann 3 Sub Accounts");
            Console.WriteLine("5) Import Contacts");
            Console.WriteLine("6) Import Activities");
            Console.WriteLine("7) Import Orders");
            Console.WriteLine("8) Import PDF Orders");
            Console.WriteLine("9) Import PDF Agreements");
            Console.WriteLine("10) Import PostNummer Postal Codes");
            Console.WriteLine("11) Import Contacts MKL");

            Console.WriteLine("-------Fixes-------");
            Console.WriteLine("20) Fix: Check for Duplicate Records");
            Console.WriteLine("21) Fix: Update SubAccounts Records");
            Console.WriteLine("22) Fix: Update Price List on Orders");
            Console.WriteLine("23) Fix: Update Total Amount on Orders");
            Console.WriteLine("24) Fix: Import Accounts May 2021");
            Console.WriteLine("25) Fix: Refresh Global OptionSets(ed_BusinessType/ed_companytrade)");
            Console.WriteLine("26) Fix: Clean Singapore Duplicated");

            Console.WriteLine("30) Update Birthday for Contacts");
            Console.WriteLine("31) Update Postal Codes Information Accounts");
            Console.WriteLine("32) Update Postal Codes Information Contacts");
            Console.WriteLine("34) Update Sent Value Codes");
            Console.WriteLine("35) Delete PostalCodes");
            Console.WriteLine("36) Fix Singapore Tickets Contacts");


            Console.WriteLine("0) Exit");
            Console.Write("\r\nSelect an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":

                    #region Import Accounts Spann 1

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload Bucket 1 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket1Accounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportAccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload Bucket 1 of the Account Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "2":

                    #region Import Accounts Spann 5

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload Bucket 5 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket5Accounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportAccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload Bucket 5 of the Account Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "3":

                    #region Update Upsales Id from Spann 2

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update Upsales Id from Spann 2 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket2Accounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            FixesHelper.UpdateUpsalesIdExistingAccounts(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Update Upsales Id from Spann 2 of the Account Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "4":

                    #region Import Accounts Spann 3 SubAccounts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload Bucket 3 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket3SubAccounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportSubAccountsRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Sub Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload Bucket 3 of the Account Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "5":

                    #region Import Contacts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Contacts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportContactsRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Contacts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Contact Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Contact Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "6":

                    #region Import Activities

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Activities);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportActivitiesRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Activities to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Activities Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Activities Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "7":

                    #region Import Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Orders Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Orders);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportOrdersRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Orders Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Order Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "8":

                    #region Import PDF Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload PDF Orders Leveransrapport--------------");

                        string relativePath = Properties.Settings.Default.RelativePath;
                        string path = Properties.Settings.Default.PDFilesReports;
                        string filter = "*.pdf";

                        string fullPath = relativePath + path;
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Full Path: " + fullPath);
                        ImportHelper.ImportPDFOrders(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                        Console.WriteLine("Batch Sent. Please check logs.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload PDF Orders Leveransrapport--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing PDF Orders Leveransrapport. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "9":

                    #region Import PDF Agreements

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload PDF Agreements--------------");

                        string relativePath = Properties.Settings.Default.RelativePath;
                        string path = Properties.Settings.Default.PDFilesAgreements;
                        string filter = "*.pdf";

                        string fullPath = relativePath + path;
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Full Path: " + fullPath);
                        ImportHelper.ImportPDFAgreements(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                        Console.WriteLine("Batch Sent. Please check logs.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload PDF Agreements--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Order Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "10":

                    #region Import Postal Codes

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Postal Codes Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.PostalCodes);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportPostalCodes(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of PostalCodes to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Postal Codes Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Postal Codes Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "11":

                    #region Import Contacts MKL

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload MKL Contacts--------------");
                        Console.WriteLine("--------------Starting to Upload MKL Contacts--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.MKLContacts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.ImportMKLContactsRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Contacts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload MKL Contacts--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Contact Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                //-----------------------METADATA and UPDATES-----------------------


                case "20":

                    #region Check for Account Duplicate Records

                    Console.WriteLine("What's the logical name of the entity?");
                    string entityName = Console.ReadLine();

                    Console.WriteLine("What's the string unique schema name of the field for that entity?");
                    string uniqueField = Console.ReadLine();

                    if (entityName == null || string.IsNullOrEmpty(entityName))
                    {
                        Console.WriteLine("The entity name is null or empty");
                        return true;
                    }

                    if (uniqueField == null || string.IsNullOrEmpty(uniqueField))
                    {
                        Console.WriteLine("The unique field name is null or empty");
                        return true;
                    }

                    QueryExpression queryDuplicates = new QueryExpression(entityName);
                    queryDuplicates.NoLock = true;
                    queryDuplicates.ColumnSet.AddColumns(uniqueField);
                    queryDuplicates.Criteria.AddCondition(uniqueField, ConditionOperator.NotNull);

                    List<Entity> lRecords = XrmRetrieveHelper.RetrieveMultiple<Entity>(localContext, queryDuplicates);

                    if (lRecords.Count == 0)
                    {
                        Console.WriteLine("No Accounts found.");
                        return true;
                    }

                    List<List<Entity>> lDuplicates = lRecords.GroupBy(i => i.GetAttributeValue<string>(uniqueField)).Where(x => x.ToList().Count > 1).Select(x => x.ToList()).ToList();

                    if (lDuplicates.Count == 0)
                    {
                        Console.WriteLine("No Duplicates found with entity name: " + entityName + " and unique field: " + uniqueField);
                        return true;
                    }

                    foreach (List<Entity> duplicate in lDuplicates)
                    {
                        Entity dEntity = duplicate.FirstOrDefault();
                        if (dEntity != null)
                            Console.WriteLine("The " + entityName + " is duplicated on field " + uniqueField + " with value: " + dEntity.GetAttributeValue<string>(uniqueField));
                    }

                    #endregion

                    return true;

                case "21":

                    #region Update SubAccounts Records

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update SubAccounts Records of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket3SubAccounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            FixesHelper.UpdateParentAccountExistingSubAccounts(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Update SubAccounts Records of the Account Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "22":

                    #region Update Price List on Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update Price List on Orders Entity--------------");

                        QueryExpression queryOrders = new QueryExpression(SalesOrder.EntityLogicalName);
                        queryOrders.NoLock = true;
                        queryOrders.ColumnSet.AddColumns(SalesOrder.Fields.SalesOrderId);
                        queryOrders.Criteria.AddCondition(SalesOrder.Fields.ed_UpsalesId, ConditionOperator.NotNull);
                        queryOrders.Criteria.AddCondition(SalesOrder.Fields.PriceLevelId, ConditionOperator.Null);

                        List<SalesOrder> lOrders = XrmRetrieveHelper.RetrieveMultiple<SalesOrder>(localContext, queryOrders);

                        if (lOrders.Count == 0)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Orders with Upsales Id and no Price List.");
                            return true;
                        }

                        Console.WriteLine("Found " + lOrders.Count + " Order that need a Price List.");

                        string defaultPriceList = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DefaultPriceList);
                        EntityReference erPriceList = ImportHelper.GetCRMPriceListByName(localContext, defaultPriceList);

                        if (erPriceList == null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Default Price List: " + defaultPriceList + " was not found.");
                            return true;
                        }

                        foreach (SalesOrder order in lOrders)
                        {
                            SalesOrder uOrder = new SalesOrder();
                            uOrder.Id = order.Id;
                            uOrder.PriceLevelId = erPriceList;

                            crmContext.Attach(uOrder);
                            crmContext.UpdateObject(uOrder);
                        }

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                        Console.WriteLine("Batch Sent. Please check logs.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Update Price List on Orders Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Updating Order Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "23":

                    #region Update Total Ammount on Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update Total Ammount on Orders of the Order Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Orders);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            FixesHelper.UpdateTotalAmmountonOrders(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Update Total Ammount on Orders of the Order Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Order Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "24":

                    #region Add New Options to Local OptionSet and Import Accounts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Add New Options to Local OptionSet and Import Accounts--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.BucketNewAccounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportHelper.Import2AccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Add New Options to Local OptionSet and Import Accounts--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "25":

                    #region Refresh OptionSets

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Refresh OptionSets--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.BucketNewAccounts);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            FixesHelper.RefreshGlobalOptionSets(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of OptionSets to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            ImportHelper.LogCrmContextMultipleResponses(localContext, responses);

                            Console.WriteLine("Batch Sent. Please check logs.");
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");


                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Refresh OptionSets--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "26":

                    #region Clean Singapore Duplicated

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Clean Duplicates Singapore--------------");


                        FixesHelper.CleanSingaporeDuplicates(localContext, crmContext);

                        Console.WriteLine("Sending Batch of Delete Requests to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Clean Duplicates Singapore--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Deleting Singapore Duplicates Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                //----------------------------------FIXES------------------------

                case "30":

                    #region Update Birthdays

                    try
                    {
                        QueryExpression queryContact = new QueryExpression(Contact.EntityLogicalName);
                        queryContact.NoLock = true;
                        queryContact.ColumnSet.AddColumns(Contact.Fields.cgi_socialsecuritynumber);
                        queryContact.Criteria.AddCondition(Contact.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull);
                        queryContact.Criteria.AddCondition(Contact.Fields.BirthDate, ConditionOperator.Null);

                        List<Contact> lContact = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContact);
                        Console.WriteLine("------------------------------------------");
                        Console.WriteLine(lContact.Count);
                        Console.WriteLine("------------------------------------------");

                        int i = 0;
                        foreach (Contact contact in lContact)
                        {
                            string socialNumber = contact.cgi_socialsecuritynumber;
                            if (socialNumber.Length == 12 && socialNumber != "000206177123")
                            {
                                int year = int.Parse(socialNumber.Substring(0, 4));
                                int month = int.Parse(socialNumber.Substring(4, 2));
                                int day = int.Parse(socialNumber.Substring(6, 2));

                                DateTime dtBirthday = new DateTime(year, month, day);

                                Contact uContact = new Contact();
                                uContact.Id = contact.Id;
                                uContact.BirthDate = dtBirthday;

                                try
                                {
                                    XrmHelper.Update(localContext, uContact);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(socialNumber);
                                    Console.WriteLine($"BirthDay Update Error: Details: " + e.Message);
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"BirthDay Update Error: Details: " + e.Message);
                                }

                            }

                            if (i % 1000 == 0)
                                Console.WriteLine(i);
                            i++;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Updating Birthdays Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "31":

                    #region Update Postal Codes Information Account

                    QueryExpression queryPostalCodes = new QueryExpression(ed_postnummer.EntityLogicalName);
                    queryPostalCodes.NoLock = true;
                    queryPostalCodes.ColumnSet.AddColumns(ed_postnummer.Fields.ed_Postnummer, ed_postnummer.Fields.ed_Kommun, ed_postnummer.Fields.ed_Kommunkod,
                        ed_postnummer.Fields.ed_Lan, ed_postnummer.Fields.ed_Lanskod, ed_postnummer.Fields.ed_name, ed_postnummer.Fields.ed_Postort, ed_postnummer.Fields.ed_postnummerId);

                    List<ed_postnummer> lPostalCodes = XrmRetrieveHelper.RetrieveMultiple<ed_postnummer>(localContext, queryPostalCodes);
                    Console.WriteLine("-----------------Postal Codes-------------------------");
                    Console.WriteLine(lPostalCodes.Count);
                    Console.WriteLine("-----------------Postal Codes-------------------------");


                    QueryExpression queryAccounts = new QueryExpression(Account.EntityLogicalName);
                    queryAccounts.NoLock = true;
                    queryAccounts.ColumnSet.AddColumns(Account.Fields.Address1_PostalCode);
                    queryAccounts.Criteria.AddCondition(Account.Fields.Address1_PostalCode, ConditionOperator.NotNull);

                    var filterAccounts = new FilterExpression();
                    queryAccounts.Criteria.AddFilter(filterAccounts);
                    filterAccounts.FilterOperator = LogicalOperator.Or;
                    filterAccounts.AddCondition(Account.Fields.Address1_Name, ConditionOperator.Null);
                    filterAccounts.AddCondition(Account.Fields.Address1_City, ConditionOperator.Null);
                    filterAccounts.AddCondition(Account.Fields.Address1_County, ConditionOperator.Null);
                    filterAccounts.AddCondition(Account.Fields.Address1_StateOrProvince, ConditionOperator.Null);
                    filterAccounts.AddCondition(Account.Fields.ed_Address1_CommunityNumber, ConditionOperator.Null);
                    filterAccounts.AddCondition(Account.Fields.ed_Address1_CountyNumber, ConditionOperator.Null);

                    List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, queryAccounts);
                    Console.WriteLine("-----------------Accounts-------------------------");
                    Console.WriteLine(lAccounts.Count);
                    Console.WriteLine("-----------------Accounts-------------------------");

                    Console.WriteLine("Continue?");
                    Console.ReadKey();
                    int j = 0;
                    foreach (Account account in lAccounts)
                    {
                        string postalCode = account.Address1_PostalCode;
                        postalCode = postalCode.Replace(" ", String.Empty);

                        List<ed_postnummer> auxPostalCode = lPostalCodes.Where(x => x.ed_Postnummer == postalCode).ToList();
                        if (auxPostalCode.Count == 1 || auxPostalCode.Count == 2)
                        {
                            ed_postnummer postNummer = auxPostalCode.FirstOrDefault();

                            Account uAccount = new Account();
                            uAccount.Id = account.Id;
                            uAccount.Address1_Name = postNummer.ed_name;
                            uAccount.Address1_City = postNummer.ed_Postort;
                            uAccount.ed_Address1_CountyNumber = int.Parse(postNummer.ed_Lanskod);
                            uAccount.Address1_County = postNummer.ed_Lan;
                            uAccount.ed_Address1_CommunityNumber = int.Parse(postNummer.ed_Kommunkod);
                            uAccount.Address1_StateOrProvince = postNummer.ed_Kommun;

                            try
                            {
                                XrmHelper.Update(localContext, uAccount);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Postal Code Account Update Error: Details: " + e.Message);
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Postal Code Account Update Error: Details: " + e.Message);
                            }

                        }
                        else if (auxPostalCode.Count == 0)
                            Console.WriteLine($"No Postal Codes found with Postal Code: " + postalCode);
                        else if (auxPostalCode.Count > 2)
                            Console.WriteLine($"More than one Postal Code found with Postal Code: " + postalCode);

                        if (j % 1000 == 0)
                            Console.WriteLine(j);
                        j++;
                    }

                    #endregion

                    return true;

                case "32":

                    #region Update Postal Codes Information Contact

                    QueryExpression query = new QueryExpression(ed_postnummer.EntityLogicalName);
                    query.NoLock = true;
                    query.ColumnSet.AddColumns(ed_postnummer.Fields.ed_Postnummer, ed_postnummer.Fields.ed_Kommun, ed_postnummer.Fields.ed_Kommunkod,
                        ed_postnummer.Fields.ed_Lan, ed_postnummer.Fields.ed_Lanskod, ed_postnummer.Fields.ed_name, ed_postnummer.Fields.ed_Postort, ed_postnummer.Fields.ed_postnummerId);

                    List<ed_postnummer> lPostalCode = XrmRetrieveHelper.RetrieveMultiple<ed_postnummer>(localContext, query);
                    Console.WriteLine("-----------------Postal Codes-------------------------");
                    Console.WriteLine(lPostalCode.Count);
                    Console.WriteLine("-----------------Postal Codes-------------------------");

                    QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
                    queryContacts.NoLock = true;
                    queryContacts.ColumnSet.AddColumns(Contact.Fields.Address1_PostalCode);
                    queryContacts.Criteria.AddCondition(Contact.Fields.Address1_PostalCode, ConditionOperator.NotNull);

                    var filterContacts = new FilterExpression();
                    queryContacts.Criteria.AddFilter(filterContacts);
                    filterContacts.FilterOperator = LogicalOperator.Or;
                    filterContacts.AddCondition(Contact.Fields.Address1_Name, ConditionOperator.Null);
                    filterContacts.AddCondition(Contact.Fields.Address1_City, ConditionOperator.Null);
                    filterContacts.AddCondition(Contact.Fields.Address1_County, ConditionOperator.Null);
                    filterContacts.AddCondition(Contact.Fields.ed_Address1_Community, ConditionOperator.Null);
                    filterContacts.AddCondition(Contact.Fields.ed_Address1_CommunityNumber, ConditionOperator.Null);
                    filterContacts.AddCondition(Contact.Fields.ed_Address1_CountyNumber, ConditionOperator.Null);

                    List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);
                    Console.WriteLine("-----------------Contacts-------------------------");
                    Console.WriteLine(lContacts.Count);
                    Console.WriteLine("-----------------Contacts-------------------------");

                    Console.WriteLine("Continue?");
                    Console.ReadKey();
                    int l = 0;
                    foreach (Contact contact in lContacts)
                    {
                        string postalCode = contact.Address1_PostalCode;
                        postalCode = postalCode.Replace(" ", String.Empty);

                        List<ed_postnummer> auxPostalCode = lPostalCode.Where(x => x.ed_Postnummer == postalCode).ToList();
                        if (auxPostalCode.Count == 1 || auxPostalCode.Count == 2)
                        {
                            ed_postnummer postNummer = auxPostalCode.FirstOrDefault();

                            Contact uContact = new Contact();
                            uContact.Id = contact.Id;
                            uContact.Address1_Name = postNummer.ed_name;
                            uContact.Address1_City = postNummer.ed_Postort;
                            uContact.ed_Address1_CountyNumber = int.Parse(postNummer.ed_Lanskod);
                            uContact.Address1_County = postNummer.ed_Lan;
                            uContact.ed_Address1_CommunityNumber = int.Parse(postNummer.ed_Kommunkod);
                            uContact.ed_Address1_Community = postNummer.ed_Kommun;

                            try
                            {
                                XrmHelper.Update(localContext, uContact);
                            }
                            catch (Exception e)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Postal Code Contact Update Error: Details: " + e.Message);
                            }

                        }
                        else if (auxPostalCode.Count == 0)
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Postal Codes found with Postal Code: " + postalCode);
                        else if (auxPostalCode.Count > 2)
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"More than one Postal Code found with Postal Code: " + postalCode);

                        if (l % 1000 == 0)
                            Console.WriteLine(l);
                        l++;
                    }

                    #endregion

                    return true;

                case "33":

                    #region Delete PostalCodes

                    try
                    {
                        QueryExpression queryDeletePostalCodes = new QueryExpression(ed_postnummer.EntityLogicalName);
                        queryDeletePostalCodes.NoLock = true;

                        List<ed_postnummer> lDeletePostalCodes = XrmRetrieveHelper.RetrieveMultiple<ed_postnummer>(localContext, queryDeletePostalCodes);
                        Console.WriteLine("Delete " + lDeletePostalCodes.Count + " Postal Codes...");
                        int n = 0;
                        foreach (ed_postnummer item in lDeletePostalCodes)
                        {
                            XrmHelper.Delete(localContext, item.ToEntityReference());

                            if (n % 1000 == 0)
                                Console.WriteLine(n);
                            n++;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Delete Postal Code Error: Details: " + e.Message);
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Delete Postal Code Error: Details: " + e.Message);
                        Console.ReadLine();
                    }


                    #endregion

                    return true;

                case "34":

                    #region Update Sent Value Codes

                    try
                    {
                        var sent_statuscode = 899310003;

                        // Instantiate QueryExpression query
                        var queryValueCodes = new QueryExpression(ed_ValueCode.EntityLogicalName);
                        queryValueCodes.NoLock = true;
                        queryValueCodes.ColumnSet.AddColumns(ed_ValueCode.Fields.ed_ValueCodeId);
                        queryValueCodes.Criteria.AddCondition(ed_ValueCode.Fields.statuscode, ConditionOperator.Equal, (int)ed_valuecode_statuscode.Skapad);

                        var ah = queryValueCodes.AddLink(ed_TextMessage.EntityLogicalName, ed_ValueCode.Fields.ed_ValueCodeId, ed_TextMessage.Fields.RegardingObjectId);
                        ah.EntityAlias = "ah";
                        ah.LinkCriteria.AddCondition(ed_TextMessage.Fields.StatusCode, ConditionOperator.Equal, sent_statuscode);

                        List<ed_ValueCode> lValueCodes = XrmRetrieveHelper.RetrieveMultiple<ed_ValueCode>(localContext, queryValueCodes);

                        Console.WriteLine("Found " + lValueCodes.Count + " Value Codes to process.");
                        Console.ReadLine();

                        foreach (var item in lValueCodes)
                        {
                            ed_ValueCode uValueCode = new ed_ValueCode();
                            uValueCode.Id = item.Id;
                            uValueCode.statuscode = ed_valuecode_statuscode.Skickad;

                            try
                            {
                                XrmHelper.Update(localContext, uValueCode);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Update Sent Value Codes: Details: " + e.Message);
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Update Sent Value Codes: Details: " + e.Message);
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Update Sent Value Codes: Details: " + e.Message);
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Update Sent Value Codes: Details: " + e.Message);
                        Console.ReadLine();
                    }

                    #endregion

                    return true;

                case "36":

                    #region Fix Singapore Tickets Contacts

                    try
                    {
                        try
                        {
                            crmContext.ClearChanges();
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload Singapore Tickets--------------");

                            List<string> fileNames = new List<string>();
                            fileNames.Add("first million.csv");
                            fileNames.Add("second million.csv");
                            fileNames.Add("third million.csv");
                            fileNames.Add("fourth million.csv");
                            fileNames.Add("fifth million.csv");
                            fileNames.Add("sixth million.csv");
                            relativeExcelPath = "C:\\Users\\Pedro\\Downloads\\";

                            ImportExcelInfo importExcelInfo = new ImportExcelInfo();

                            using (importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileNames, 1, 16)) 
                            {
                                bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                                if (isParsingOk)
                                {
                                }
                            }

                            var querySingaporeTickets = new QueryExpression(st_singaporeticket.EntityLogicalName);
                            querySingaporeTickets.NoLock = true;
                            querySingaporeTickets.ColumnSet = new ColumnSet(st_singaporeticket.Fields.ed_CRMNummer, st_singaporeticket.Fields.st_TicketID);

                            List<st_singaporeticket> lSingaporeTickets = XrmRetrieveHelper.RetrieveMultiple<st_singaporeticket>(localContext, querySingaporeTickets);



                            querySingaporeTickets = new QueryExpression(st_singaporeticket.EntityLogicalName);
                            querySingaporeTickets.NoLock = true;
                            querySingaporeTickets.ColumnSet = new ColumnSet(st_singaporeticket.Fields.ed_ActivationIntervalFrom, st_singaporeticket.Fields.ed_ActivationIntervalTo,
                                st_singaporeticket.Fields.ed_BearerCategory, st_singaporeticket.Fields.ed_BlockedDate, st_singaporeticket.Fields.ed_CRMNummer, st_singaporeticket.Fields.ed_HasGroupDiscount,
                                st_singaporeticket.Fields.ed_HasRefund, st_singaporeticket.Fields.ed_LastUpdated, st_singaporeticket.Fields.ed_OfferNameDetailed, st_singaporeticket.Fields.ed_SalesChannel,
                                st_singaporeticket.Fields.ed_TicketActivated, st_singaporeticket.Fields.ed_TravellersCount, st_singaporeticket.Fields.ed_TravelValidityIntervalFrom,
                                st_singaporeticket.Fields.ed_TravelValidityIntervalTo, st_singaporeticket.Fields.st_ContactID, st_singaporeticket.Fields.st_name, st_singaporeticket.Fields.st_PriceModel,
                                st_singaporeticket.Fields.st_PriceModelPrice, st_singaporeticket.Fields.st_SingTicketType, st_singaporeticket.Fields.st_TicketActivated, st_singaporeticket.Fields.st_TicketCreated,
                                st_singaporeticket.Fields.st_TicketID, st_singaporeticket.Fields.st_TicketPrice);

                            List<Entity> records = new List<Entity>();

                            RetrieveMultipleResponse response;
                            do
                            {
                                RetrieveMultipleRequest request = new RetrieveMultipleRequest();
                                request.Query = querySingaporeTickets;

                                response = (RetrieveMultipleResponse)localContext.OrganizationService.Execute(request);

                                if (response.EntityCollection.Entities.Count > 0)
                                    records.AddRange(response.EntityCollection.Entities);

                                if(records.Count >= 10000)
                                {
                                    List<OrganizationRequest> lRequests = HandleSingaporeTicket(localContext, relativeExcelPath, records);
                                }

                                if (response.EntityCollection.MoreRecords)
                                {
                                    querySingaporeTickets.PageInfo.PageNumber++;
                                    querySingaporeTickets.PageInfo.PagingCookie = response.EntityCollection.PagingCookie;
                                }
                            } while (response.EntityCollection.MoreRecords);

                            _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload Singapore Tickets--------------");
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Singapore Tickets Records. Details: " + e.Message);
                            throw;
                        }


                        // path to the csv file
                        string path = "C:\\Users\\PGOEND\\Downloads\\first million.csv";

                        string[] lines = System.IO.File.ReadAllLines(path);
                        List<string> lCrmNummer = new List<string>();
                        foreach (string line in lines)
                        {
                            string[] columns = line.Split(';');
                            foreach (string column in columns)
                            {
                                lCrmNummer.Add(column);
                            }
                        }

                        Console.WriteLine("Found " + lCrmNummer.Count + " Singapore Tickets to process.");

                        path = "C:\\Users\\PGOEND\\Downloads\\contacts.csv";

                        string[] linesC = System.IO.File.ReadAllLines(path);
                        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                        foreach (string line in linesC)
                        {
                            string[] columns = line.Split(';');
                            list.Add(new KeyValuePair<string, string>(columns[0], columns[1]));
                        }
                        Console.WriteLine("Found " + list.Count + " Contacts to process.");


                        //List<KeyValuePair<string, string>> lNewList = new List<KeyValuePair<string, string>>();
                        //foreach (var item in list)
                        //{
                        //    if (lCrmNummer.Contains(item.Key))
                        //        lNewList.Add(item);
                        //}

                        // Create an ExecuteTransactionRequest object.
                        var requestToCreateRecords = new ExecuteTransactionRequest()
                        {
                            // Create an empty organization request collection.
                            Requests = new OrganizationRequestCollection(),
                            ReturnResponses = false
                        };

                        for (int i = 6179; i < lCrmNummer.Count; i++)
                        {
                            string crmNummer = lCrmNummer[i];
                            if(requestToCreateRecords.Requests.Count != 0 && requestToCreateRecords.Requests.Count > 250)
                            {
                                Console.WriteLine("Process " + requestToCreateRecords.Requests.Count + "i: " + i);
                                // Execute all the requests in the request collection using a single web method call.
                                try
                                {
                                    localContext.OrganizationService.Execute(requestToCreateRecords);
                                    requestToCreateRecords.Requests = new OrganizationRequestCollection();
                                }
                                catch (FaultException<OrganizationServiceFault> ex)
                                {
                                    Console.WriteLine("Create request failed for the account{0} and the reason being: {1}",
                                        ((ExecuteTransactionFault)(ex.Detail)).FaultedRequestIndex + 1, ex.Detail.Message);
                                    throw;
                                }
                            }

                            var contact = list.FirstOrDefault(x => x.Key == crmNummer);

                            if (contact.Key == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Contact found with CRMNummer: {crmNummer}");
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    // Instantiate QueryExpression query
                                    var querySingapore = new QueryExpression(st_singaporeticket.EntityLogicalName);
                                    querySingapore.NoLock = true;
                                    querySingapore.ColumnSet.AddColumns(st_singaporeticket.Fields.ed_CRMNummer);
                                    querySingapore.Criteria.AddCondition(st_singaporeticket.Fields.ed_CRMNummer, ConditionOperator.Equal, crmNummer);

                                    var lSingaporeTickets = XrmRetrieveHelper.RetrieveMultiple<st_singaporeticket>(localContext, querySingapore);

                                    foreach (var singaporeTicket in lSingaporeTickets)
                                    {
                                        st_singaporeticket uSingaporeTicket = new st_singaporeticket();
                                        uSingaporeTicket.Id = singaporeTicket.Id;
                                        uSingaporeTicket.st_ContactID = new EntityReference(Contact.EntityLogicalName, new Guid(contact.Value));

                                        UpdateRequest updateRequest = new UpdateRequest { Target = uSingaporeTicket };
                                        requestToCreateRecords.Requests.Add(updateRequest);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Update Singapore Ticket Contact: Details: " + e.Message);
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Update Singapore Ticket Contact: Details: " + e.Message);
                                }
                            }
                        }

                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Fix Singapore Tickets Contacts: Details: " + e.Message);
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Fix Singapore Tickets Contacts: Details: " + e.Message);
                        Console.ReadLine();
                    }

                    #endregion

                    return true;

                case "0":
                    return false;
                default:
                    Console.WriteLine("The option " + option + " is not supported. Please choose again. 0) Exit");
                    return true;
            }
        }

        public static Entity CheckNecessaryFields(OptionMetadataCollection colOpSingaporeTicket, List<ExcelColumn> lColumns, List<ExcelLineData> line, Entity singaporeTicket)
        {
            try
            {
                Entity uSingaporeTicket = new Entity();

                if (line.Count != lColumns.Count)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Data count is not equal to the column count.");
                    return null;
                }

                for (int j = 0; j < lColumns.Count; j++)
                {
                    ExcelLineData selectedData = line[j];

                    if (selectedData == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                        continue;
                    }

                    ExcelColumn selectedColumn = ImportHelper.GetSelectedExcelColumn(lColumns, j);

                    if (selectedColumn == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null. Contact your administrator.");
                        continue;
                    }

                    string name = selectedColumn.name;
                    string value = selectedData.value;

                    if (value == null || string.IsNullOrEmpty(value))
                        continue;

                    switch (name)
                    {
                        case "LastUpdated":
                            #region Last Updated
                            DateTime dtLastUpdated = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtLastUpdated))
                            {
                                if (dtLastUpdated != DateTime.MinValue && (singaporeTicket["ed_lastupdated"] == null || singaporeTicket["ed_lastupdated"] != null && (DateTime)singaporeTicket["ed_lastupdated"] != dtLastUpdated))
                                    uSingaporeTicket["ed_lastupdated"] = dtLastUpdated;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "TicketId":
                        case "cgi_contactnumber":
                            break;
                        case "PriceModel":
                            #region Price Model
                            if(value != null && (singaporeTicket["st_pricemodel"] == null || singaporeTicket["st_pricemodel"] != null && (string)singaporeTicket["st_pricemodel"] != value))
                                uSingaporeTicket["st_pricemodel"] = value;
                            #endregion
                            break;
                        case "PriceModelPriceId":
                            #region Price Model Price
                            if (value != null && (singaporeTicket["st_pricemodelprice"] == null || singaporeTicket["st_pricemodelprice"] != null && (string)singaporeTicket["st_pricemodelprice"] != value))
                                uSingaporeTicket["st_pricemodelprice"] = value;
                            #endregion
                            break;
                        case "DataCreatedDate":
                            #region Data Created Date
                            DateTime dtTicketCreated = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtTicketCreated))
                            {
                                if (dtTicketCreated != DateTime.MinValue && (singaporeTicket["st_ticketcreated"] == null || singaporeTicket["st_ticketcreated"] != null && (DateTime)singaporeTicket["st_ticketcreated"] != dtTicketCreated))
                                    uSingaporeTicket["st_ticketcreated"] = dtTicketCreated;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "activatedDate":
                            #region Activated Date
                            DateTime dtTicketActivated = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtTicketActivated))
                            {
                                if (dtTicketActivated != DateTime.MinValue && (singaporeTicket["ed_ticketactivated"] == null || singaporeTicket["ed_ticketactivated"] != null && (DateTime)singaporeTicket["ed_ticketactivated"] != dtTicketActivated))
                                    uSingaporeTicket["ed_ticketactivated"] = dtTicketActivated;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "blockedDate":
                            #region Blocked Date
                            DateTime dtBlockedDate = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtBlockedDate))
                            {
                                if (dtBlockedDate != DateTime.MinValue && (singaporeTicket["ed_blockeddate "] == null || singaporeTicket["ed_blockeddate "] != null && (DateTime)singaporeTicket["ed_blockeddate "] != dtBlockedDate))
                                    uSingaporeTicket["ed_blockeddate"] = dtBlockedDate;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "OfferName":
                            #region Offer Name
                            int? optionSetST = ImportHelper.GetOptionSetValueByName(colOpSingaporeTicket, value);

                            if (optionSetST == null)
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM.");

                            if (optionSetST != null && (singaporeTicket["st_singtickettype"] == null || singaporeTicket["st_singtickettype"] != null && ((OptionSetValue)singaporeTicket["st_singtickettype"]).Value != optionSetST))
                                uSingaporeTicket["st_singtickettype"] = new OptionSetValue((int)optionSetST);
                            #endregion
                            break;
                        case "OfferNameDetailed":
                            #region Offer Name Detailed
                            if (value != null && (singaporeTicket["ed_offernamedetailed"] == null || singaporeTicket["ed_offernamedetailed"] != null && (string)singaporeTicket["ed_offernamedetailed"] != value))
                                uSingaporeTicket["ed_offernamedetailed"] = value;
                            #endregion
                            break;
                        case "TicketAmount":
                            #region Ticket Amount
                            decimal dTicketPrice = decimal.MinValue;

                            if (decimal.TryParse(value, out dTicketPrice))
                            {
                                if (dTicketPrice != decimal.MinValue && (singaporeTicket["st_ticketprice"] == null || singaporeTicket["st_ticketprice"] != null && ((Money)singaporeTicket["st_ticketprice"]).Value != dTicketPrice))
                                    uSingaporeTicket["st_ticketprice"] = new Money(dTicketPrice);
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a Money value.");
                            #endregion
                            break;
                        case "SalesChannelId":
                            #region Sales Channel
                            if (value != null && (singaporeTicket["ed_saleschannel"] == null || singaporeTicket["ed_saleschannel"] != null && (string)singaporeTicket["ed_saleschannel"] != value))
                                uSingaporeTicket["ed_saleschannel"] = value;
                            #endregion
                            break;
                        case "BearerCategory":
                            #region Bearer Category
                            if (value != null && (singaporeTicket["ed_bearercategory"] == null || singaporeTicket["ed_bearercategory"] != null && (string)singaporeTicket["ed_bearercategory"] != value))
                                uSingaporeTicket["ed_bearercategory"] = value;
                            #endregion
                            break;
                        case "HasGroupDiscount":
                            #region Has Group Discount

                            bool? hasGroupDiscount = null;

                            if (value == "1")
                                hasGroupDiscount = true;
                            else if (value == "0")
                                hasGroupDiscount = false;
                            else
                                break;

                            if (singaporeTicket["ed_hasgroupdiscount"] == null || singaporeTicket["ed_hasgroupdiscount"] != null && (bool)singaporeTicket["ed_hasgroupdiscount"] != hasGroupDiscount)
                                uSingaporeTicket["ed_hasgroupdiscount"] = hasGroupDiscount;

                            #endregion
                            break;
                        case "TravellerCount":
                            #region Traveller Count
                            int iTravellerCount = int.MinValue;

                            if (int.TryParse(value, out iTravellerCount))
                            {
                                if (iTravellerCount != int.MinValue && (singaporeTicket["ed_travellerscount"] == null || singaporeTicket["ed_travellerscount"] != null && (int)singaporeTicket["ed_travellerscount"] != iTravellerCount))
                                    uSingaporeTicket["ed_travellerscount"] = iTravellerCount;                                    
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a int value.");
                            #endregion
                            break;
                        case "HasRefund":
                            #region Has Refund
                            bool? hasRefund = null;

                            if (value == "1")
                                hasRefund = true;
                            else if (value == "0")
                                hasRefund = false;
                            else
                                break;
                                
                            if (singaporeTicket["ed_hasrefund"] == null || singaporeTicket["ed_hasrefund"] != null && (bool)singaporeTicket["ed_hasrefund"] != hasRefund)
                                uSingaporeTicket["ed_hasrefund"] = hasRefund;
                            #endregion
                            break;
                        case "activationinternal_from":
                            #region Activation Interval From
                            DateTime dtActivationFrom = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtActivationFrom))
                            {
                                if (dtActivationFrom != DateTime.MinValue && (singaporeTicket["ed_activationintervalfrom"] == null || singaporeTicket["ed_activationintervalfrom"] != null && (DateTime)singaporeTicket["ed_activationintervalfrom"] != dtActivationFrom))
                                    uSingaporeTicket["ed_activationintervalfrom"] = dtActivationFrom;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "activationinternal_to":
                            #region Activation Interval To
                            DateTime dtAtivationTo = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtAtivationTo))
                            {
                                if (dtAtivationTo != DateTime.MinValue && (singaporeTicket["ed_activationintervalto"] == null || singaporeTicket["ed_activationintervalto"] != null && (DateTime)singaporeTicket["ed_activationintervalto"] != dtAtivationTo))
                                    uSingaporeTicket["ed_activationintervalto"] = dtAtivationTo;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "travelvalidityinternal_from":
                            #region Travel Validity From
                            DateTime dtTravelValidityFrom = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtTravelValidityFrom))
                            {
                                if (dtTravelValidityFrom != DateTime.MinValue && (singaporeTicket["ed_travelvalidityintervalfrom"] == null || singaporeTicket["ed_travelvalidityintervalfrom"] != null && (DateTime)singaporeTicket["ed_travelvalidityintervalfrom"] != dtTravelValidityFrom))
                                    uSingaporeTicket["ed_travelvalidityintervalfrom"] = dtTravelValidityFrom;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        case "travelvalidityinternal_to":
                            #region Travel Validity To
                            DateTime dtTravelValidityTo = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dtTravelValidityTo))
                            {
                                if (dtTravelValidityTo != DateTime.MinValue && (singaporeTicket["ed_travelvalidityintervalto"] == null || singaporeTicket["ed_travelvalidityintervalto"] != null && (DateTime)singaporeTicket["ed_travelvalidityintervalto"] != dtTravelValidityTo))
                                    uSingaporeTicket["ed_travelvalidityintervalto"] = dtTravelValidityTo;
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");
                            #endregion
                            break;
                        default:
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                            break;
                    }
                }

                if (uSingaporeTicket.Attributes.Count == 0)
                    return null;

                return uSingaporeTicket;
            }
            catch (Exception e)
            {
                _log.ErrorFormat (CultureInfo.InvariantCulture, $"CheckNecessaryFields(): Details: {e.Message}");
                throw new Exception($"CheckNecessaryFields(): Details: {e.Message}");
            }
        }

        public static List<OrganizationRequest> HandleSingaporeTicket(Plugin.LocalPluginContext localContext, string relativeExcelPath, List<Entity> lSingaporeTickets)
        {
            var filePaths = Directory.GetFiles(@"C:\Users\Pedro\Downloads\", "*.csv");
            List<OrganizationRequest> lUpsertRequests = new List<OrganizationRequest>();
            OptionMetadataCollection colOpSingaporeTicket = ImportHelper.GetOptionSetMetadata(localContext, st_singaporeticket.EntityLogicalName, st_singaporeticket.Fields.st_SingTicketType);

            if (filePaths.Length == 0)
                return null;

            Console.Write("Creating Batch of Singapore Tickets... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int j = 0; j < lSingaporeTickets.Count; j++)
                {
                    progress.Report((double)j / (double)lSingaporeTickets.Count);
                    var singaporeTicket = lSingaporeTickets[j];
                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        ImportExcelInfo importExcelInfo = ImportHelper.HandleExcelInformationStreamReader(relativeExcelPath, fileName);
                        bool isParsingOk = ImportHelper.GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                                continue;
                            }

                            for (int i = 0; i < importExcelInfo.lData.Count; i++)
                            {
                                try
                                {
                                    List<ExcelLineData> line = importExcelInfo.lData[i];

                                    string ticketId = ImportHelper.GetValueFromLine(importExcelInfo, line, "TicketId");
                                    string crmNummer = ImportHelper.GetValueFromLine(importExcelInfo, line, "cgi_contactnumber");

                                    if((singaporeTicket.Contains("st_ticketid") && (string)singaporeTicket["st_ticketid"] != ticketId) || (singaporeTicket.Contains("ed_crmnummer") && (string)singaporeTicket["ed_crmnummer"] != crmNummer))
                                        continue;

                                    // Set alternate Key
                                    KeyAttributeCollection altKey = new KeyAttributeCollection();
                                    altKey.Add("st_ticketid", ticketId);
                                    altKey.Add("ed_crmnummer", crmNummer);

                                    Entity upsertSingaporeTicket = new Entity("st_singaporeticket", altKey);

                                    Entity eSingaporeTicket = CheckNecessaryFields(colOpSingaporeTicket, importExcelInfo.lColumns, line, singaporeTicket);
                                    if(eSingaporeTicket != null)
                                    {
                                        upsertSingaporeTicket.Attributes = eSingaporeTicket.Attributes;
                                        UpsertRequest request = new UpsertRequest()
                                        {
                                            Target = upsertSingaporeTicket
                                        };

                                        lUpsertRequests.Add(request);
                                    }
                                }
                                catch (Exception e)
                                {
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Singapore Tickets Exception. Details: " + e.Message);
                                }
                            }
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Excel Parsing is not ok. The number of data values is diferent from the number of columns.");
                    }
                }
            }
            Console.WriteLine("Done.");

            return lUpsertRequests;
        }

        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("SkanetrafikenImport");

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        static void Main()
        {

#if DEBUG
            _log.Debug("Main Started");

            string passwordArgument = null;

            if (!string.IsNullOrEmpty(passwordArgument))
            {
                _log.DebugFormat(CultureInfo.InvariantCulture, "Credentials parsed from command line.");
                string password = passwordArgument.Substring(passwordArgument.IndexOf(":") + 1);

                CrmConnection.SaveCredentials(CredentialFilePath, password, Entropy);
            }
#endif

            Plugin.LocalPluginContext localContext = GenerateLocalContext();

            if (localContext == null || localContext.OrganizationService == null)
            {
                Console.WriteLine("Connection to CRM was not possible.\n LocalContext/OrganizationService is null.");
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Connection to CRM was not possible.\n LocalContext/OrganizationService is null.\n\n");
                return;
            }

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.RelativePath);

            CrmContext crmContext = new CrmContext(localContext.OrganizationService);
            SaveChangesOptions optionsChanges = SaveChangesOptions.ContinueOnError;

            if (crmContext == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The CRM EarlyBound Context is null.");
                Console.WriteLine("The CRM EarlyBound Context is null.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Run Updales import selected...");
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenuUpsales(localContext, crmContext, optionsChanges, relativeExcelPath);
            }
        }
    }
}