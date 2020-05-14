using Endeavor.Crm;
using Endeavor.Crm.UpSalesMigration;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExcelApp = Microsoft.Office.Interop.Excel;

namespace Skanetrafiken.UpSalesMigration
{
    class Program
    {
        public class ImportExcelInfo 
        {
            public int? rowCount { get; set; }
            public int? colCount { get; set; }
            public ExcelApp.Range excelRange { get; set; }
            public List<ExcelColumn> lColumns { get; set; }

            public ImportExcelInfo(int? r, int? c, ExcelApp.Range range, List<ExcelColumn> lC)
            {
                rowCount = r;
                colCount = c;
                excelRange = range;
                lColumns = lC;
            }
        }

        public class ExcelColumn
        {
            public int index { get; set; }
            public string name { get; set; }

            public ExcelColumn(int i, string n)
            {
                index = i;
                name = n;
            }
        }

        private static Plugin.LocalPluginContext GetCrmConnection()
        {
            CrmServiceClient conn = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CrmConnection"].ConnectionString);

            if (conn.IsReady == false)
                throw new Exception("Failed to connect to Microsoft CRM. IsReady = false");

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        public static List<ExcelColumn> GetListExcelColumns(ExcelApp.Range excelRange, int colCount)
        {
            List<ExcelColumn> lColumns = new List<ExcelColumn>();

            for (int c = 1; c <= colCount; c++)
                if (excelRange.Cells[1, c] != null && excelRange.Cells[1, c].Value2 != null)
                    lColumns.Add(new ExcelColumn(c, excelRange.Cells[1, c].Value2.ToString()));

            return lColumns;
        }

        public static ExcelColumn GetSelectedExcelColumn(List<ExcelColumn> lColumns, int j)
        {
            List<ExcelColumn> lSelectedColumns = lColumns.Where(x => x.index == j).ToList();

            if (lSelectedColumns.Count == 0)
            {
                _log.Info("No Columns found with Index: " + j);
                return null;
            }
            else if (lSelectedColumns.Count > 1)
            {
                _log.Info("One Or More Columns found with Index: " + j);
                return null;
            }

            return lSelectedColumns.FirstOrDefault();
        }

        public static ImportExcelInfo HandleExcelInformation(string relativeExcelPath, string fileName)
        {
            try
            {
                ExcelApp.Application excelApp = new ExcelApp.Application();
                ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(relativeExcelPath + "\\" + fileName);

                int numberOfSheets = excelBook.Worksheets.Count;

                if (numberOfSheets > 1)
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"There is more than one WorkSheet in this file, by default it will check the first WorkSheet.");

                ExcelApp._Worksheet excelSheet = excelBook.Sheets[1];
                ExcelApp.Range excelRange = excelSheet.UsedRange;

                int rowCount = excelRange.Rows.Count;
                int colCount = excelRange.Columns.Count;

                List<ExcelColumn> lColumns = GetListExcelColumns(excelRange, colCount);

                return new ImportExcelInfo(rowCount, colCount, excelRange, lColumns);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Details: " + e.Message);
                return null;
            }
        }

        public static void ImportAccountRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {
            if(importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;
            List<Account> lAccounts = new List<Account>();

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                Account nAccount = new Account();

                string noteText = string.Empty;
                string street2 = string.Empty;
                string city = string.Empty;
                string country = string.Empty;
                string postalCode = string.Empty;

                for (int j = 1; j <= importExcelInfo.colCount; j++)
                {
                    if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null)
                    {
                        _log.Info("The value inside the Cell is null.");
                        continue;
                    }

                    ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

                    if(selectedColumn == null)
                    {
                        _log.Info("The Selected Column is null.");
                        continue;
                    }

                    string name = selectedColumn.name;

                    switch (name)
                    {
                        case "Företag: Namn":
                            nAccount.Name = excelRange.Cells[i, j].Value2.ToString();
                            break;
                        case "Företag: Telefon":
                            nAccount.Telephone2 = excelRange.Cells[i, j].Value2.ToString();
                            break;
                        case "Företag: Kontoansvarig":

                            //TODO Default User

                            string ownerName = excelRange.Cells[i, j].Value2.ToString();
                            FilterExpression filterUsers = new FilterExpression();
                            filterUsers.Conditions.Add(new ConditionExpression(SystemUser.Fields.FullName, ConditionOperator.Equal, ownerName));

                            List<SystemUser> lUsers = XrmRetrieveHelper.RetrieveMultiple<SystemUser>(localContext, new ColumnSet(SystemUser.Fields.SystemUserId), filterUsers).ToList();

                            if (lUsers.Count == 0)
                            {
                                _log.Info("No Users Found with Name: " + ownerName + ".");
                                break;
                            }
                            else if (lUsers.Count > 1)
                            {
                                _log.Info("More than one Users Found with Name: " + ownerName + ".");
                                break;
                            }

                            SystemUser user = lUsers.FirstOrDefault();
                            EntityReference erOwner = user.ToEntityReference();

                            nAccount.OwnerId = erOwner;

                            break;
                        case "Företag: Kampanjer":
                            //Not in the mappings
                            break;
                        case "Företag: Anteckningar":

                            noteText = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Postadress: Fullständig adress":

                            street2 = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Postadress: Stad":

                            city = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Postadress: Land":

                            country = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Postadress: Postnummer":

                            postalCode = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Besöksadress: Fullständig adress":

                            nAccount.Address1_Line1 = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Besöksadress: Stad":

                            nAccount.Address1_City = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Besöksadress: Land":

                            nAccount.Address1_Country = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Besöksadress: Län":

                            nAccount.Address1_County = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Besöksadress: Postnummer":

                            nAccount.Address1_PostalCode = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Faktureringsadress: Fullständig adress":

                            nAccount.Address2_Line1 = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Faktureringsadress: Stad":

                            nAccount.Address2_City = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Faktureringsadress: Postnummer":

                            nAccount.Address2_PostalCode = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: duns":
                            //ignore field
                            break;
                        case "Företag: Organisationsnummer":

                            nAccount.cgi_organizational_number = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Bransch (SNI)":

                            break;
                        case "Företag: Bolagsform":
                            break;
                        case "Företag: Kreditvärdighet (Bisnode)":
                            break;
                        case "Företag: Omsättning":
                            break;
                        case "Företag: Vinst":
                            break;
                        case "Företag: Status":
                            break;
                        case "Företag: Antal anställda":

                            nAccount.NumberOfEmployees = excelRange.Cells[i, j].Value2.ToString();

                            break;
                        case "Företag: Kundresa":
                            break;
                        default:

                            _log.Info("The Column " + name + " is not on the mappings initially set.");

                            break;
                    }
                }

                nAccount.StateCode = AccountState.Active;

                Guid gAccount = XrmHelper.Create(localContext, nAccount);

                if (noteText != string.Empty && gAccount != null)
                {
                    Annotation note = new Annotation();

                    note.ObjectId = new EntityReference(entityName, gAccount);
                    note.ObjectTypeCode = entityName;
                    note.Subject = "Note from UpSales Integration";
                    note.NoteText = noteText;

                    XrmHelper.Create(localContext, note);
                }

                if (street2 != string.Empty || city != string.Empty || country != string.Empty || postalCode != string.Empty)
                {
                    CustomerAddress nCustomerAddress = new CustomerAddress();
                    nCustomerAddress.AddressTypeCode = customeraddress_addresstypecode.ShipTo;
                    nCustomerAddress.Line2 = street2;
                    nCustomerAddress.City = city;
                    nCustomerAddress.Country = country;
                    nCustomerAddress.PostalCode = postalCode;

                    XrmHelper.Create(localContext, nCustomerAddress);
                }
            }
        }

        public static void ImportContactsRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        public static void ImportActivitiesRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        public static void ImportLeadsRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        public static void ImportOpportunitiesRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        public static void ImportWonOpportunitiesRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        public static void ImportHistoricalDataRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, string entityName)
        {

        }

        static ExcelApp.Application excelApp = new ExcelApp.Application();
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //Test Connection
            Plugin.LocalPluginContext localContext = GetCrmConnection();

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.ExcelRelativePath);
            string fileName = "Upsales företag 2020-04-30.xlsx";

            #region Import Accounts

            try
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportAccountRecords(localContext, importExcelInfo, "account");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Account Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Contacts

            try
            {
                fileName = "Contacts företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportContactsRecords(localContext, importExcelInfo, "contact");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Contact Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Contact Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Activities

            try
            {
                fileName = "Activities företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportActivitiesRecords(localContext, importExcelInfo, "activity");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Activities Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Activities Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Leads

            try
            {
                fileName = "Leads företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Leads Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportLeadsRecords(localContext, importExcelInfo, "lead");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Leads Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Leads Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Opportunities

            try
            {
                fileName = "Opportunities företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Opportunities Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportOpportunitiesRecords(localContext, importExcelInfo, "opportunity");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Opportunities Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Opportunities Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Won Opportunities

            try
            {
                fileName = "Won Opportunities företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Won Opportunities Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportWonOpportunitiesRecords(localContext, importExcelInfo, "opportunity");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Won Opportunities Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Won Opportunities Records. Details: " + e.Message);
                throw;
            }

            #endregion
            #region Import Historical Data

            try
            {
                fileName = "Historical Data företag 2020-04-30.xlsx";
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Historical Data Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportHistoricalDataRecords(localContext, importExcelInfo, "historicaldata");
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Historical Data Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Historical Data Records. Details: " + e.Message);
                throw;
            }

            #endregion

            //after reading, relaase the excel project
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            Console.ReadLine();
        }
    }
}
