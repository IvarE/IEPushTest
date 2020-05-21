using Endeavor.Crm;
using Endeavor.Crm.UpSalesMigration;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Description;
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

        public static void ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                _service = (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "Error while connecting to CRM " + ex.Message);
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
            }
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

        public static EntityReference GetCrmUserOrTeamByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterUsers = new FilterExpression();
            filterUsers.Conditions.Add(new ConditionExpression(SystemUser.Fields.FullName, ConditionOperator.Equal, name));

            List<SystemUser> lUsers = XrmRetrieveHelper.RetrieveMultiple<SystemUser>(localContext, new ColumnSet(SystemUser.Fields.SystemUserId), filterUsers).ToList();

            if(lUsers.Count == 1)
                return lUsers.FirstOrDefault().ToEntityReference();

            FilterExpression filterTeams = new FilterExpression();
            filterTeams.Conditions.Add(new ConditionExpression(Team.Fields.Name, ConditionOperator.Equal, name));

            List<Team> lTeams = XrmRetrieveHelper.RetrieveMultiple<Team>(localContext, new ColumnSet(), filterTeams).ToList();

            if (lTeams.Count == 1)
                return lTeams.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Users/Teams or More than One found with Name: " + name + ".");
            return null;
        }

        public static EntityReference GetCrmAccountByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.Name, ConditionOperator.Equal, name));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Accounts or More than One found with Name: " + name + ".");
            return null;
        }

        public static EntityReference GetCrmContactByFullName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterContacts = new FilterExpression();
            filterContacts.Conditions.Add(new ConditionExpression(Contact.Fields.FullName, ConditionOperator.Equal, name));

            List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, new ColumnSet(Contact.Fields.ContactId), filterContacts).ToList();

            if (lContacts.Count == 1)
                return lContacts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Contacts or More than One found with Name: " + name + ".");
            return null;
        }

        public static void LogExecuteMultipleResponses(ExecuteMultipleRequest requestWithResults, ExecuteMultipleResponse responseWithResults)
        {
            foreach (ExecuteMultipleResponseItem responseItem in responseWithResults.Responses)
            {
                // A valid response.
                if (responseItem.Response != null)
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Success Response: " + requestWithResults.Requests[responseItem.RequestIndex] + " : Response: " + responseItem.Response);

                // An error has occurred.
                else if (responseItem.Fault != null)
                {
                    string errorMessage = $"Failed Response: " + requestWithResults.Requests[responseItem.RequestIndex] + " : Response: " + responseItem.Fault;

                    if (requestWithResults.Requests[responseItem.RequestIndex].RequestName == "Create")
                    {
                        Entity entity = (Entity)requestWithResults.Requests[responseItem.RequestIndex].Parameters.Values.FirstOrDefault();

                        errorMessage += " - Details: "; //TODO SWITCH FOR EACH ENTITY

                        _log.ErrorFormat(CultureInfo.InvariantCulture, errorMessage);
                    }
                }
            }
        }

        public static void HandleMultipleRequests(Plugin.LocalPluginContext localContext, List<CreateRequest> lCreateRequests)
        {
            int maxLimitCalls = int.Parse(ConfigurationManager.AppSettings["maxLimitCalls"]);

            ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            if (lCreateRequests.Count > maxLimitCalls)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"The total Requests is greater than the max allowed.");

                List<List<CreateRequest>> lAuxCreate = lCreateRequests.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / maxLimitCalls)
                                                .Select(x => x.Select(v => v.Value).ToList()).ToList();

                foreach (List<CreateRequest> listCreate in lAuxCreate)
                {
                    requestWithResults.Requests = new OrganizationRequestCollection();
                    requestWithResults.Requests.AddRange(listCreate);

                    ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                    LogExecuteMultipleResponses(requestWithResults, responseWithResults);
                }
            }
            else
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"The total Requests is less than the max allowed.");

                requestWithResults.Requests = new OrganizationRequestCollection();
                requestWithResults.Requests.AddRange(lCreateRequests);

                ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                LogExecuteMultipleResponses(requestWithResults, responseWithResults);
            }
        }

        public static void ImportAccountRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo)
        {
            if(importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;

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
                    if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The cell in position (" + i + "," + j + ") is null or empty.");
                        continue;
                    }

                    ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

                    if(selectedColumn == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null.");
                        continue;
                    }

                    string name = selectedColumn.name;
                    string value = excelRange.Cells[i, j].Value2.ToString();

                    switch (name)
                    {
                        case "CreateOn":

                            DateTime dateTime = DateTime.MinValue;

                            if(DateTime.TryParse(value, out dateTime))
                            {
                                if(dateTime != DateTime.MinValue)
                                    nAccount.OverriddenCreatedOn = dateTime;
                            }

                            break;
                        case "Företag: Namn": nAccount.Name = value;
                            break;
                        case "Företag: Telefon": nAccount.Telephone2 = value;
                            break;
                        case "Företag: Hemsida": nAccount.WebSiteURL = value;
                            break;
                        case "Företag: Kontoansvarig":

                            EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                            if(erOwner == null)
                            {
                                string defaultUserName = "Admin"; //TODO Put on App Config
                                erOwner = GetCrmUserOrTeamByName(localContext, defaultUserName);
                            }

                            if (erOwner != null)
                                nAccount.OwnerId = erOwner;
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Owner of the Account was not found.");

                            break;
                        case "Företag: Moderbolag":

                            EntityReference erParentAccount = GetCrmAccountByName(localContext, value);

                            if (erParentAccount != null)
                                nAccount.ParentAccountId = erParentAccount;
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Parent Account of the Account was not found or it was already null.");

                            break;
                        case "Företag: Anteckningar": noteText = value;
                            break;
                        case "Företag: Postadress: Fullständig adress": street2 = value;
                            break;
                        case "Företag: Postadress: Stad": city = value;
                            break;
                        case "Företag: Postadress: Land": country = value;
                            break;
                        case "Företag: Postadress: Postnummer": postalCode = value;
                            break;
                        case "Företag: Besöksadress: Fullständig adress": nAccount.Address1_Line1 = value;
                            break;
                        case "Företag: Besöksadress: Stad": nAccount.Address1_City = value;
                            break;
                        case "Företag: Besöksadress: Land": nAccount.Address1_Country = value;
                            break;
                        case "Företag: Besöksadress: Län": nAccount.Address1_County = value;
                            break;
                        case "Företag: Besöksadress: Postnummer": nAccount.Address1_PostalCode = value;
                            break;
                        case "Företag: Faktureringsadress: Fullständig adress": nAccount.Address2_Line1 = value;
                            break;
                        case "Företag: Faktureringsadress: Stad": nAccount.Address2_City = value;
                            break;
                        case "Företag: Faktureringsadress: Land": nAccount.Address2_Country = value;
                            break;
                        case "Företag: Faktureringsadress: Län": nAccount.Address2_County = value;
                            break;
                        case "Företag: Faktureringsadress: Postnummer": nAccount.Address2_PostalCode = value;
                            break;
                        case "Företag: Organisationsnummer": nAccount.cgi_organizational_number = value;
                            break;
                        case "Företag: Antal anställda":

                            int iValue;

                            if(int.TryParse(value, out iValue))
                                nAccount.NumberOfEmployees = iValue;
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"It was not possible to convert " + value + " to an integer.");

                            break;
                        default: _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                            break;
                    }
                }

                nAccount.StateCode = AccountState.Active;

                //TODO field line of text, random string characters use that as a unique field to map the records, also on UECCIntegration - dont do

                //crm.AddObject
                //crm.AddRelatedObject
                Guid gAccount = XrmHelper.Create(localContext, nAccount);

                if (noteText != string.Empty && gAccount != null)
                {
                    Annotation note = new Annotation();

                    note.ObjectId = new EntityReference(Account.EntityLogicalName, gAccount);
                    note.ObjectTypeCode = Account.EntityLogicalName;
                    note.Subject = "Note from UpSales Integration";
                    note.NoteText = noteText;

                    XrmHelper.Create(localContext, note);
                }

                if ((street2 != string.Empty || city != string.Empty || country != string.Empty || postalCode != string.Empty) && gAccount != null)
                {
                    CustomerAddress nCustomerAddress = new CustomerAddress();
                    nCustomerAddress.AddressTypeCode = customeraddress_addresstypecode.ShipTo;
                    nCustomerAddress.Line2 = street2;
                    nCustomerAddress.City = city;
                    nCustomerAddress.Country = country;
                    nCustomerAddress.PostalCode = postalCode;
                    nCustomerAddress.ParentId = new EntityReference(Account.EntityLogicalName, gAccount);

                    XrmHelper.Create(localContext, nCustomerAddress);
                }
            }
        }

        public static void ImportContactsRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                Contact nContact = new Contact();

                string noteText = string.Empty;

                for (int j = 1; j <= importExcelInfo.colCount; j++)
                {
                    if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The cell in position (" + i + "," + j + ") is null or empty.");
                        continue;
                    }

                    ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

                    if (selectedColumn == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null.");
                        continue;
                    }

                    string name = selectedColumn.name;
                    string value = excelRange.Cells[i, j].Value2.ToString();

                    switch (name)
                    {
                        case "CreateOn":

                            DateTime dateTime = DateTime.MinValue;

                            if (DateTime.TryParse(value, out dateTime))
                            {
                                if (dateTime != DateTime.MinValue)
                                    nContact.OverriddenCreatedOn = dateTime;
                            }

                            break;
                        case "Företag":

                            EntityReference erParent = GetCrmAccountByName(localContext, value);

                            if (erParent == null)
                                erParent = GetCrmContactByFullName(localContext, value);

                            if (erParent != null)
                                nContact.ParentCustomerId = erParent;
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Parent Customer of the Contact was not found or it was already null.");

                            break;
                        case "Förnamn": nContact.FirstName = value;
                            break;
                        case "Efternamn": nContact.LastName = value;
                            break;
                        case "Anteckningar": noteText = value;
                            break;
                        case "Titel": //nContact.ed_.AccountRoleCode = TODO ask about the Option Set Values mappings
                            break;
                        case "Telefon": nContact.Telephone1 = value;
                            break;
                        case "Mobiltelefon": nContact.Telephone2 = value; //Asigned from the Web API mapping file?
                            break;
                        case "Email": nContact.EMailAddress1 = value;
                            break;
                        default:
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                            break;
                    }
                }

                nContact.StateCode = ContactState.Active;
                //nContact.AccountRoleCode = contact_accountrolecode.AnsvarigforInfotainment;

                Guid gContact = XrmHelper.Create(localContext, nContact);

                if (noteText != string.Empty && gContact != null)
                {
                    Annotation note = new Annotation();

                    note.ObjectId = new EntityReference(Contact.EntityLogicalName, gContact);
                    note.ObjectTypeCode = Contact.EntityLogicalName;
                    note.Subject = "Note from UpSales Integration";
                    note.NoteText = noteText;

                    XrmHelper.Create(localContext, note);
                }
            }
        }

        public static void ImportActivitiesRecords(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            //Check Excel File First
            //Will we have a file with all activities or 3 files with email, appointments and phonecall
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

        public static IOrganizationService _service = null;
        static ExcelApp.Application excelApp = new ExcelApp.Application();
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //Test Connection
            ConnectToMSCRM("D1\\CRMAdmin", "uSEme2!nstal1", "https://sekundtst.skanetrafiken.se/DKCRM/XRMServices/2011/Organization.svc");

            if (_service == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The CRM Service is null.");
                Console.WriteLine("The CRM Service is null.");
                Console.ReadLine();
                return;
            }

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());
            //CrmContext crmContext = new CrmContext(_service);

            //crmContext.AddObject(account);
            //crmContext.AddRelatedObject(relatedAccount, new Relationship("gpe_account_gpe_morada_conta"), address);

            //https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/org-service/create-early-bound-entity-classes-code-generation-tool 

            //TODO ADD CRM CONTEXT TO THE HOLE PROGRAM AND RETURN LIST OF CREATEREQUESTS



            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.ExcelRelativePath);
            string fileName = "Upsales företag 2020-04-30.xlsx";

            #region Import Accounts

            try
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportAccountRecords(localContext, importExcelInfo);
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Account Records. Details: " + e.Message);
                throw;
            }

            #endregion

            fileName = "Contacts företag 2020-04-30.xlsx";

            #region Import Contacts

            try
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportContactsRecords(localContext, importExcelInfo);
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Contact Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Contact Records. Details: " + e.Message);
                throw;
            }

            #endregion

            fileName = "Activities företag 2020-04-30.xlsx";

            #region Import Activities

            try
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");
                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
                ImportActivitiesRecords(localContext, importExcelInfo);
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Activities Entity--------------");
            }
            catch (Exception e)
            {
                _log.Error("Error Importing Activities Records. Details: " + e.Message);
                throw;
            }

            #endregion

            fileName = "Leads företag 2020-04-30.xlsx";

            #region Import Leads

            try
            {
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

            fileName = "Opportunities företag 2020-04-30.xlsx";

            #region Import Opportunities

            try
            {
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

            fileName = "Won Opportunities företag 2020-04-30.xlsx";

            #region Import Won Opportunities

            try
            {
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

            fileName = "Historical Data företag 2020-04-30.xlsx";

            #region Import Historical Data

            try
            {
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
