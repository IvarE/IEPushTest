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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExcelApp = Microsoft.Office.Interop.Excel;

namespace Skanetrafiken.UpSalesMigration
{
    class Program
    {
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

        public class FilesInfo
        {
            public string fileName { get; set; }
            public string entityName { get; set; }

            public FilesInfo(string fName, string eName)
            {
                fileName = fName;
                entityName = eName;
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

        public static void ImportAccountRecords(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, List<ExcelColumn> lColumns, string entityName, int rowCount, int colCount)
        {
            List<Account> lAccounts = new List<Account>();

            for (int i = 2; i <= rowCount; i++)
            {
                Account nAccount = new Account();

                string noteText = string.Empty;
                string street2 = string.Empty;
                string city = string.Empty;
                string country = string.Empty;
                string postalCode = string.Empty;

                for (int j = 1; j <= colCount; j++)
                {
                    List<ExcelColumn> lSelectedColumns = lColumns.Where(x => x.index == j).ToList();

                    if (lSelectedColumns.Count == 0)
                    {
                        _log.Info("No Columns found with Index: " + j);
                        continue;
                    }
                    else if (lSelectedColumns.Count > 1)
                    {
                        _log.Info("One Or More Columns found with Index: " + j);
                        continue;
                    }

                    ExcelColumn selectedColumn = lSelectedColumns.FirstOrDefault();
                    string name = selectedColumn.name;

                    if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null)
                    {
                        _log.Info("The value inside the Cell is null.");
                        continue;
                    }

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

        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Plugin.LocalPluginContext localContext = GetCrmConnection();

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.ExcelRelativePath);

            List<FilesInfo> lFileInfo = new List<FilesInfo>();
            lFileInfo.Add(new FilesInfo("Upsales företag 2020-04-30.xlsx", "account"));

            foreach (FilesInfo fileInfo in lFileInfo)
            {
                ExcelApp.Application excelApp = new ExcelApp.Application();
                ExcelApp.Workbook excelBook = excelApp.Workbooks.Open(relativeExcelPath + "\\" + fileInfo.fileName);

                int numberOfSheets = excelBook.Worksheets.Count;

                if (numberOfSheets > 1)
                {
                    _log.Info("There is more than one WorkSheet in this file, by default it will check the first WorkSheet.");
                }

                ExcelApp._Worksheet excelSheet = excelBook.Sheets[1];
                ExcelApp.Range excelRange = excelSheet.UsedRange;

                int rowCount = excelRange.Rows.Count;
                int colCount = excelRange.Columns.Count;

                string entityName = fileInfo.entityName;
                List<ExcelColumn> lColumns = GetListExcelColumns(excelRange, colCount);

                switch (entityName)
                {
                    case "account":

                        try
                        {
                            ImportAccountRecords(localContext, excelRange, lColumns, entityName, rowCount, colCount);
                        }
                        catch (Exception e)
                        {
                            _log.Error("Error Importing Account Records. Details: " + e.Message);
                            throw;
                        }

                        break;
                    case "contact":
                        break;
                    default:
                        break;
                }

                //after reading, relaase the excel project
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                Console.ReadLine();

            }
        }
    }
}
