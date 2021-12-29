using Endeavor.Crm;
using log4net;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Import.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Import
{
    public class ImportHelper
    {
        public static string rel_account_note = "Account_Annotation";
        public static string rel_account_address = "Account_CustomerAddress";
        public static string rel_contact_note = "Contact_Annotation";
        public static string rel_contact_account = "account_primary_contact";
        public static string rel_order_note = "SalesOrder_Annotation";
        public static string rel_order_orderdetail = "order_details";

        public static string cgi_account_contact = "cgi_account_contact";

        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void ImportMKLContactsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            string uniqueLabel = "Contact Number";
            List<ExcelLineData> lColumnData = GetColumnData(importExcelInfo, uniqueLabel);

            QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
            queryContacts.NoLock = true;
            queryContacts.ColumnSet = new ColumnSet(Contact.Fields.cgi_ContactNumber);

            FilterExpression queryCriteria = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryCriteria);

            queryCriteria.FilterOperator = LogicalOperator.Or;

            foreach (ExcelLineData item in lColumnData)
                queryCriteria.AddCondition(Contact.Fields.cgi_ContactNumber, ConditionOperator.Equal, item.value);

            List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);

            Console.Write("Creating Batch of Contacts... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);

                        string uniqueValue = lColumnData[i].value;
                        Contact uContact = lContacts.Where(x => x.cgi_ContactNumber == uniqueValue).FirstOrDefault();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "Analysdata 1":
                                    uContact.ed_analysdata1 = value;
                                    break;
                                case "Analysdata 2":
                                    uContact.ed_analysdata2 = value;
                                    break;
                                case "Analysdata 3":
                                    uContact.ed_analysdata3 = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        crmContext.Attach(uContact);
                        crmContext.UpdateObject(uContact);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Import Contacts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportPostalCodes(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            Console.Write("Creating Batch of PostalCodes... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);

                        ed_postnummer nPostalCode = new ed_postnummer();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "Postnummer":
                                    nPostalCode.ed_Postnummer = value;
                                    break;
                                case "Postort":
                                    nPostalCode.ed_Postort = value;
                                    break;
                                case "Länskod":
                                    nPostalCode.ed_Lanskod = value;
                                    break;
                                case "Län":
                                    nPostalCode.ed_Lan = value;
                                    break;
                                case "Kommunkod":
                                    nPostalCode.ed_Kommunkod = value;
                                    break;
                                case "Kommun":
                                    nPostalCode.ed_Kommun = value;
                                    break;
                                case "AR-kod":
                                    nPostalCode.ed_ARkod = value;
                                    break;
                                case "Name":
                                    nPostalCode.ed_name = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        crmContext.AddObject(nPostalCode);

                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Import PostalCodes Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void Import2AccountRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            OptionMetadataCollection colOpBusinessType = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

            Console.Write("Creating Batch of Accounts... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        Account nAccount = new Account();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "Företagsnamn":
                                    nAccount.Name = value;
                                    break;
                                case "Orgnr":

                                    QueryExpression queryAccount = new QueryExpression(Account.EntityLogicalName);
                                    queryAccount.NoLock = true;
                                    queryAccount.ColumnSet = new ColumnSet(Account.Fields.AccountId);
                                    queryAccount.Criteria.AddCondition(Account.Fields.cgi_organizational_number, ConditionOperator.Equal, value);
                                    queryAccount.Criteria.AddCondition(Account.Fields.StateCode, ConditionOperator.Equal, (int)AccountState.Active);

                                    List<Account> l = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, queryAccount);

                                    if (l.Count == 0)
                                        nAccount.cgi_organizational_number = value;
                                    else if (l.Count > 1)
                                        _log.Error("ERROROROROROORORORO------------------");
                                    else if (l.Count == 1)
                                        nAccount.Id = l.FirstOrDefault().Id;

                                    break;
                                case "Bolagsform":
                                    nAccount.ed_BusinessTypeId = value;
                                    nAccount.edp_LegalClassification = value;
                                    break;
                                case "Bolagsform text":

                                    int? optionSetBT = GetOptionSetValueByName(colOpBusinessType, value);

                                    if (optionSetBT == null)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        optionSetBT = InsertGlobalOptionSetOption(localContext, "ed_businesstype", value, 1053);
                                    }

                                    if (optionSetBT != null)
                                        nAccount.ed_BusinessType = new OptionSetValue((int)optionSetBT);

                                    break;
                                case "Branschtext":
                                    nAccount.ed_CompanyTradeText = value;
                                    break;
                                case "Branschkod":
                                    nAccount.ed_IndustryCodeId = value;
                                    break;
                                case "Faxnr":
                                    nAccount.Fax = value;
                                    break;
                                case "Telefon":
                                    nAccount.Telephone1 = cleanMobileTelefon(value);
                                    break;
                                case "Utd.adress":
                                    nAccount.Address1_Line2 = value;
                                    break;
                                case "Postnr":
                                    nAccount.Address1_PostalCode = value;
                                    break;
                                case "Postort":
                                    nAccount.Address1_City = value;
                                    break;
                                case "Ant. anst. AB":

                                    int numberOfEmplyees = 0;
                                    if (int.TryParse(value, out numberOfEmplyees))
                                        nAccount.NumberOfEmployees = numberOfEmplyees;

                                    break;
                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nAccount.Id != null && nAccount.Id != Guid.Empty)
                        {
                            crmContext.Attach(nAccount);
                            crmContext.UpdateObject(nAccount);
                        }
                        else
                            _log.Error("UPDATE: EROROROORORORO");

                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import 2Accounts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportContactsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            List<Account> lAddedAccounts = new List<Account>();
            List<Account> lPrimaryContactAccounts = new List<Account>();

            Console.Write("Creating Batch of Contacts... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        Account eParentAccount = null;
                        Account ePParentAccount = null;

                        Contact nContact = new Contact();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "Account U-ID":

                                    EntityReference erParentAccount = GetCrmAccountByUpsalesId(localContext, value);

                                    if (erParentAccount != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Parent Customer with Upsales Id: " + value + " of the Contact was updated.");
                                        nContact.ParentCustomerId = erParentAccount;

                                        eParentAccount = new Account();
                                        eParentAccount = XrmRetrieveHelper.Retrieve<Account>(localContext, erParentAccount, new ColumnSet(Account.Fields.ParentAccountId, Account.Fields.Name));

                                        if (eParentAccount != null && eParentAccount.ParentAccountId != null)
                                        {
                                            ePParentAccount = new Account();
                                            ePParentAccount.Id = eParentAccount.ParentAccountId.Id;
                                        }
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Customer with Upsales Id: " + value + " was not found. The Contact on line " + (i + 1) + " will be ignored.");

                                    break;
                                case "Contact U-ID":

                                    EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                                    if (erContact != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Contact with Upsales Id: " + value + " was already imported.");
                                        continue;
                                    }

                                    nContact.ed_UpsalesId = value;
                                    break;
                                case "Account":
                                    //Already Handleded
                                    break;
                                case "Namn":
                                    //Already Handleded
                                    break;
                                case "Förnamn":
                                    nContact.FirstName = value;
                                    break;
                                case "Efternamn":
                                    nContact.LastName = value;
                                    break;
                                case "Titel":
                                    nContact.ed_title = value;
                                    break;
                                case "Telefon":
                                    nContact.Telephone1 = cleanMobileTelefon(value);
                                    break;
                                case "Mobiltelefon":
                                    nContact.Telephone2 = cleanMobileTelefon(value);
                                    break;
                                case "Email":
                                    nContact.EMailAddress1 = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nContact.ParentCustomerId == null || nContact.ed_UpsalesId == null)
                            continue;

                        nContact.ed_InfotainmentContact = true;
                        nContact.StateCode = ContactState.Active;
                        nContact.ed_InformationSource = ed_informationsource.AdmAndraKund;
                        nContact.AccountRoleCode = contact_accountrolecode.Ansvarigforinfotainment;

                        //Prevent Plugin Errors
                        if (nContact.FirstName == null)
                            nContact.FirstName = ".";

                        if (nContact.LastName == null)
                            nContact.LastName = ".";

                        crmContext.AddObject(nContact);

                        if (eParentAccount != null)
                        {
                            try
                            {
                                List<Account> accountAdded = lAddedAccounts.Where(x => x.Id == eParentAccount.Id).ToList();

                                if (!crmContext.IsAttached(eParentAccount) && accountAdded.Count == 0)
                                {
                                    crmContext.Attach(eParentAccount);
                                    lAddedAccounts.Add(eParentAccount);
                                }

                                if (!crmContext.IsAttached(nContact))
                                    crmContext.Attach(nContact);

                                if (accountAdded.Count == 0)
                                    crmContext.AddLink(eParentAccount, new Relationship(cgi_account_contact), nContact);
                                else
                                    crmContext.AddLink(accountAdded.FirstOrDefault(), new Relationship(cgi_account_contact), nContact);

                                _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": 1Level: A Request has been created to relate the Account " + eParentAccount.Id + " to Contact " + nContact.FirstName + " " + nContact.LastName + " on the CGI Grid.");
                            }
                            catch (Exception e)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": CGI Grid Error Exception. Details: " + e.Message);
                            }
                        }

                        if (ePParentAccount != null)
                        {
                            try
                            {
                                List<Account> accountAdded = lAddedAccounts.Where(x => x.Id == ePParentAccount.Id).ToList();

                                if (!crmContext.IsAttached(ePParentAccount) && accountAdded.Count == 0)
                                {
                                    crmContext.Attach(ePParentAccount);
                                    lAddedAccounts.Add(ePParentAccount);
                                }

                                if (!crmContext.IsAttached(nContact))
                                    crmContext.Attach(nContact);

                                if (accountAdded.Count == 0)
                                    crmContext.AddLink(ePParentAccount, new Relationship(cgi_account_contact), nContact);
                                else
                                    crmContext.AddLink(accountAdded.FirstOrDefault(), new Relationship(cgi_account_contact), nContact);

                                _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": 2Level: A Request has been created to relate the Account " + ePParentAccount.Id + " to Contact " + nContact.FirstName + " " + nContact.LastName + " on the CGI Grid.");
                            }
                            catch (Exception e)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": CGI Grid Error Exception. Details: " + e.Message);
                            }
                        }

                        if (nContact.ParentCustomerId != null && nContact.ParentCustomerId.LogicalName == Account.EntityLogicalName)
                        {
                            try
                            {
                                EntityReference erAccount = new EntityReference(Account.EntityLogicalName, nContact.ParentCustomerId.Id);
                                Account account = XrmRetrieveHelper.Retrieve<Account>(localContext, erAccount, new ColumnSet(Account.Fields.PrimaryContactId));

                                if (account != null && account.PrimaryContactId == null)
                                {
                                    List<Account> accountAdded = lAddedAccounts.Where(x => x.Id == eParentAccount.Id).ToList();

                                    if (!crmContext.IsAttached(eParentAccount) && accountAdded.Count == 0)
                                    {
                                        crmContext.Attach(eParentAccount);
                                        lAddedAccounts.Add(eParentAccount);
                                    }

                                    if (!crmContext.IsAttached(nContact))
                                        crmContext.Attach(nContact);

                                    if (accountAdded.Count == 0)
                                        crmContext.AddLink(eParentAccount, new Relationship(rel_contact_account), nContact);
                                    else
                                        crmContext.AddLink(accountAdded.FirstOrDefault(), new Relationship(rel_contact_account), nContact);

                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": A Request has been created for the Account " + eParentAccount.Id + " to be updated with Contact " + nContact.FirstName + " " + nContact.LastName + " as it's Primary Contact.");
                                }
                                else
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Account " + account.Id + " already has a Primary Contact.");
                            }
                            catch (Exception e)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Primary Contact Error Exception. Details: " + e.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Import Contacts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportActivitiesRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            string defaultUserName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DefaultOwner);
            EntityReference erDefaultUser = GetCrmUserOrTeamByName(localContext, defaultUserName);

            string activityTypeNameColumn = "Aktivitetstyp";
            int maxSubject = 200;
            int maxDescription = 2000;

            Console.Write("Creating Batch of Activities... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        string activityType = GetValueFromLine(importExcelInfo, line, activityTypeNameColumn);

                        if (activityType == null || string.IsNullOrEmpty(activityType))
                            continue;

                        switch (activityType)
                        {
                            case "Telefonsamtal":

                                PhoneCall nPhoneCall = GetPhoneCall(localContext, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

                                if (nPhoneCall != null)
                                    crmContext.AddObject(nPhoneCall);

                                break;
                            case "E-post":

                                Email nEmail = GetEmail(localContext, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

                                if (nEmail != null)
                                    crmContext.AddObject(nEmail);

                                break;
                            case "Övrigt":

                                Appointment nAppointment = GetAppointment(localContext, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

                                if (nAppointment != null)
                                    crmContext.AddObject(nAppointment);

                                break;
                            default:
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Activity Type " + activityType + " is not implemented yet.");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Activities Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportOrdersRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            string defaultUserName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DefaultOwner);
            EntityReference erDefaultUser = GetCrmUserOrTeamByName(localContext, defaultUserName);

            string currencyName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DefaultCurrency);
            EntityReference erCurrency = GetCrmCurrencyByName(localContext, currencyName);

            string defaultPriceList = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.DefaultPriceList);
            EntityReference erPriceList = GetCRMPriceListByName(localContext, defaultPriceList);

            Console.Write("Creating Batch of Orders... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        SalesOrder nOrder = new SalesOrder();

                        string noteText = string.Empty;
                        string noteTextProduct = string.Empty;
                        string startProductDate = string.Empty;
                        string endProductDate = string.Empty;
                        string productColibri = string.Empty;
                        string totalLineAmmount = string.Empty;

                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "Account U-ID":

                                    EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                                    if (erAccount != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Customer with Upsales Id: " + value + " of the Order was updated.");
                                        nOrder.CustomerId = erAccount;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " was not found. The Order on line " + (i + 1) + " will be ignored.");

                                    break;
                                case "Contact: U-ID":

                                    EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                                    if (erContact != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Contact with Upsales Id: " + value + " of the Order was updated.");
                                        nOrder.ed_contact = erContact;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Contact with Upsales Id: " + value + " was not found.");

                                    break;
                                case "Orderns U-ID":

                                    EntityReference erOrder = GetCrmOrderByUpsalesId(localContext, value);

                                    if (erOrder != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Order with Upsales Id: " + value + " was already imported.");
                                        continue;
                                    }

                                    nOrder.ed_UpsalesId = value;
                                    break;
                                case "Företag":
                                    //Already Handled
                                    break;
                                case "Kontaktperson":
                                    //Already Handled
                                    break;
                                case "Beskrivning":
                                    nOrder.Name = value;
                                    break;
                                case "Datum":

                                    DateTime submitDate = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out submitDate))
                                    {
                                        if (submitDate != DateTime.MinValue)
                                            nOrder.SubmitDate = submitDate;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "Värde":
                                    totalLineAmmount = value;
                                    break;
                                case "Order: Användare":

                                    if (value == "Alexander Zaragoza")
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner " + value + " of the PhoneCall was not found. The default user was updated.");
                                        nOrder.OwnerId = erDefaultUser;
                                        break;
                                    }

                                    EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                                    if (erOwner != null)
                                        nOrder.OwnerId = erOwner;
                                    else
                                    {
                                        nOrder.OwnerId = erDefaultUser;
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner was updated with the default user.");
                                    }

                                    break;
                                case "Anteckningar":
                                    noteText = value;
                                    break;
                                case "Produkt":
                                    noteTextProduct = value;
                                    break;
                                case "Order: Fas":
                                    //Already Handled
                                    break;
                                case "Skapad":

                                    DateTime createdOn = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out createdOn))
                                    {
                                        if (createdOn != DateTime.MinValue)
                                            nOrder.OverriddenCreatedOn = createdOn;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "Kampanj start":

                                    DateTime campaignStart = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out campaignStart))
                                    {
                                        if (campaignStart != DateTime.MinValue)
                                            nOrder.ed_campaigndatestart = campaignStart;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "Kampanj slut":

                                    DateTime campaignEnd = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out campaignEnd))
                                    {
                                        if (campaignEnd != DateTime.MinValue)
                                            nOrder.ed_campaigndateend = campaignEnd;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "Startdatum":
                                    startProductDate = value;
                                    break;
                                case "Slutdatum":
                                    endProductDate = value;
                                    break;
                                case "Produkt (Colibri)":
                                    productColibri = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nOrder.CustomerId == null || nOrder.ed_UpsalesId == null)
                            continue;

                        nOrder.TransactionCurrencyId = erCurrency;
                        nOrder.PriceLevelId = erPriceList;

                        crmContext.AddObject(nOrder);

                        if (totalLineAmmount != string.Empty)
                        {
                            decimal dtotalLineAmount = decimal.MinValue;

                            if (decimal.TryParse(totalLineAmmount, out dtotalLineAmount))
                            {
                                if (dtotalLineAmount != decimal.MinValue)
                                {
                                    SalesOrderDetail nOrderProduct = nOrderProduct = new SalesOrderDetail();
                                    nOrderProduct.IsProductOverridden = true;
                                    nOrderProduct.IsPriceOverridden = true;
                                    nOrderProduct.ProductDescription = "Upsales Order Value";
                                    nOrderProduct.Quantity = 1M;
                                    nOrderProduct.PricePerUnit = new Money(dtotalLineAmount);
                                    nOrderProduct.ManualDiscountAmount = new Money(0M);
                                    nOrderProduct.Tax = new Money(0M);

                                    crmContext.AddRelatedObject(nOrder, new Relationship(rel_order_orderdetail), nOrderProduct);
                                }
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + totalLineAmmount + " to a decimal value.");
                        }

                        if (noteTextProduct != string.Empty)
                        {
                            Annotation noteProduct = new Annotation();

                            noteProduct.Subject = "Note from UpSales Integration";
                            noteProduct.NoteText = noteTextProduct;

                            crmContext.AddRelatedObject(nOrder, new Relationship(rel_order_note), noteProduct);
                        }

                        if (noteText != string.Empty)
                        {
                            Annotation note = new Annotation();

                            note.Subject = "Note from UpSales Integration";
                            note.NoteText = noteText + "\n\nProduct Start Date: " + startProductDate + "\nProduct End Date: " + endProductDate + "\n\nProduct (Colibri): " + productColibri;

                            crmContext.AddRelatedObject(nOrder, new Relationship(rel_order_note), note);
                        }

                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Import Orders Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportPDFOrders(Plugin.LocalPluginContext localContext, CrmContext crmContext, string path, string filter)
        {
            List<UpsalesFile> lFiles = GetFileList(path, filter);

            foreach (UpsalesFile file in lFiles)
            {
                try
                {
                    string pdfMimeType = @"application/pdf";
                    string fileName = file.fileName;
                    byte[] fileBytes = File.ReadAllBytes(file.filePath);
                    string base64 = Convert.ToBase64String(fileBytes);

                    if (base64 == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Base64 for the file: " + file.fileName + " is null.");
                        continue;
                    }

                    string orderId = GetOrderId(fileName);

                    if (orderId == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Upsales Id for the Order is null. File: " + fileName + " will be ignored.");
                        continue;
                    }

                    QueryExpression queryOrders = new QueryExpression(SalesOrder.EntityLogicalName);
                    queryOrders.NoLock = true;
                    queryOrders.ColumnSet.AddColumns(SalesOrder.Fields.SalesOrderId);
                    queryOrders.Criteria.AddCondition(SalesOrder.Fields.ed_UpsalesId, ConditionOperator.Equal, orderId);

                    List<SalesOrder> lOrders = XrmRetrieveHelper.RetrieveMultiple<SalesOrder>(localContext, queryOrders);

                    if (lOrders.Count == 1)
                    {
                        SalesOrder salesOrder = lOrders.FirstOrDefault();

                        Annotation note = new Annotation();
                        note.Subject = file.fileName;
                        note.NoteText = "Leveransrapport file for this Order.";
                        note.ObjectTypeCode = SalesOrder.EntityLogicalName;
                        note.ObjectId = new EntityReference(SalesOrder.EntityLogicalName, (Guid)salesOrder.SalesOrderId);
                        note.MimeType = pdfMimeType;
                        note.FileName = file.fileName;
                        note.DocumentBody = base64;

                        crmContext.AddObject(note);
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"A request has been sent to CRM to create an Attachment on Related Order: " + salesOrder.SalesOrderId);
                    }
                    else if (lOrders.Count == 0)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Orders found with  Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                    else if (lOrders.Count > 1)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"More than one Order found with Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, string.Format("The file was downloaded: {0}", e.ToString()));
                }
            }
        }

        public static void ImportPDFAgreements(Plugin.LocalPluginContext localContext, CrmContext crmContext, string path, string filter)
        {
            List<UpsalesFile> lFiles = GetFileList(path, filter);

            foreach (UpsalesFile file in lFiles)
            {
                try
                {
                    string pdfMimeType = @"application/pdf";
                    string fileName = file.fileName;
                    byte[] fileBytes = File.ReadAllBytes(file.filePath);
                    string base64 = Convert.ToBase64String(fileBytes);

                    if (base64 == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Base64 for the file: " + file.fileName + " is null.");
                        continue;
                    }

                    string orderId = GetOrderId(fileName);

                    if (orderId == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Upsales Id for the Order is null. File: " + fileName + " will be ignored.");
                        continue;
                    }

                    QueryExpression queryOrders = new QueryExpression(SalesOrder.EntityLogicalName);
                    queryOrders.NoLock = true;
                    queryOrders.ColumnSet.AddColumns(SalesOrder.Fields.SalesOrderId);
                    queryOrders.Criteria.AddCondition(SalesOrder.Fields.ed_UpsalesId, ConditionOperator.Equal, orderId);

                    List<SalesOrder> lOrders = XrmRetrieveHelper.RetrieveMultiple<SalesOrder>(localContext, queryOrders);

                    if (lOrders.Count == 1)
                    {
                        SalesOrder salesOrder = lOrders.FirstOrDefault();

                        Annotation note = new Annotation();
                        note.Subject = file.fileName;
                        note.NoteText = "Agreement file for this Order.";
                        note.ObjectTypeCode = SalesOrder.EntityLogicalName;
                        note.ObjectId = new EntityReference(SalesOrder.EntityLogicalName, (Guid)salesOrder.SalesOrderId);
                        note.MimeType = pdfMimeType;
                        note.FileName = file.fileName;
                        note.DocumentBody = base64;

                        crmContext.AddObject(note);
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"A request has been sent to CRM to create an Attachment on Related Order: " + salesOrder.SalesOrderId);
                    }
                    else if (lOrders.Count == 0)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Orders found with  Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                    else if (lOrders.Count > 1)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"More than one Order found with Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, string.Format("The file was downloaded: {0}", e.ToString()));
                }
            }
        }

        public static void ImportAccountRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            Console.Write("Creating Batch of Accounts... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    OptionMetadataCollection colOpBusinessType = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        Account nAccount = new Account();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "U-ID":

                                    EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                                    if (erAccount != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Account with Upsales Id: " + value + " was already imported.");
                                        continue;
                                    }

                                    nAccount.ed_UpsalesId = value;
                                    break;
                                case "Företagsnamn":
                                    nAccount.Name = value;
                                    break;
                                case "Organisationsnummer":
                                    nAccount.cgi_organizational_number = value;
                                    break;
                                case "Besöksadress - Gatuadress":
                                    nAccount.Address1_Line2 = value;
                                    break;
                                case "Besöksadress - Postnummer":
                                    nAccount.Address1_PostalCode = value;
                                    break;
                                case "Besöksadress - Ort":
                                    nAccount.Address1_City = value;
                                    break;
                                case "Besöksadress - Land":
                                    nAccount.Address1_Country = value;
                                    break;
                                case "CS datum för senast adressändring":
                                    nAccount.ed_CSDateforlastaddressChange = value;
                                    break;
                                case "E-post":
                                case "epost":
                                    nAccount.EMailAddress1 = value;
                                    break;
                                case "Mobil":
                                case "Telefon":
                                    nAccount.Telephone1 = cleanMobileTelefon(value);
                                    break;
                                case "Webbplats":
                                    nAccount.WebSiteURL = value;
                                    break;
                                case "Branchid":
                                    nAccount.ed_IndustryCodeId = value;
                                    break;
                                case "Bolagsformid":
                                    nAccount.ed_BusinessTypeId = value;
                                    break;
                                case "Bolagsform":

                                    int? optionSetBT = GetOptionSetValueByName(colOpBusinessType, value);

                                    if (optionSetBT == null)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        optionSetBT = InsertGlobalOptionSetOption(localContext, "ed_businesstype", value, 1053);
                                    }

                                    if (optionSetBT != null)
                                        nAccount.ed_BusinessType = new OptionSetValue((int)optionSetBT);

                                    break;
                                case "Net Margin":
                                    nAccount.edp_NetMargin = value + "%";
                                    break;
                                case "Nettoresultat":

                                    decimal dNetProfit = decimal.MinValue;

                                    if (decimal.TryParse(value, out dNetProfit))
                                    {
                                        if (dNetProfit != decimal.MinValue)
                                            nAccount.edp_NetProfit = new Money(dNetProfit);
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a decimal value.");

                                    break;
                                case "Nettoomsättning":

                                    decimal dRevenue = decimal.MinValue;

                                    if (decimal.TryParse(value, out dRevenue))
                                    {
                                        if (dRevenue != decimal.MinValue)
                                            nAccount.Revenue = new Money(dRevenue);
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a decimal value.");

                                    break;
                                case "Totalt antal anställda 1":

                                    int iNEmployees = int.MinValue;

                                    if (int.TryParse(value, out iNEmployees))
                                    {
                                        if (iNEmployees != decimal.MinValue)
                                            nAccount.NumberOfEmployees = iNEmployees;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to an integer value.");

                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nAccount.ed_UpsalesId == null)
                            continue;

                        nAccount.ed_InfotainmentCustomer = true;
                        nAccount.StateCode = AccountState.Active;

                        crmContext.AddObject(nAccount);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Accounts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportSubAccountsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            Console.Write("Creating Batch of Sub Accounts... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        Account nSubAccount = new Account();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                                case "U-ID":

                                    EntityReference erSubAccount = GetCrmAccountByUpsalesId(localContext, value);

                                    if (erSubAccount != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Sub Account with Upsales Id: " + value + " was already imported.");
                                        continue;
                                    }

                                    nSubAccount.ed_UpsalesId = value;
                                    break;
                                case "Företagsnamn":
                                    nSubAccount.Name = value;
                                    break;
                                case "Organisationsnummer":

                                    EntityReference erParentAccount = GetCrmAccountByOrganizationNumber(localContext, value);

                                    if (erParentAccount != null)
                                        nSubAccount.ParentAccountId = erParentAccount;

                                    break;
                                case "Besöksadress - Gatuadress":
                                    nSubAccount.Address1_Line2 = value;
                                    break;
                                case "Besöksadress - Postnummer":
                                    nSubAccount.Address1_PostalCode = value;
                                    break;
                                case "Besöksadress - Ort":
                                    nSubAccount.Address1_City = value;
                                    break;
                                case "Besöksadress - Land":
                                    nSubAccount.Address1_Country = value;
                                    break;
                                case "CS datum för senast adressändring":
                                    nSubAccount.ed_CSDateforlastaddressChange = value;
                                    break;
                                case "E-post":
                                case "epost":
                                    nSubAccount.EMailAddress1 = value;
                                    break;
                                case "Mobil":
                                case "Telefon":
                                    nSubAccount.Telephone1 = cleanMobileTelefon(value);
                                    break;
                                case "Webbplats":
                                    nSubAccount.WebSiteURL = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nSubAccount.ed_UpsalesId == null)
                            continue;

                        nSubAccount.ed_InfotainmentCustomer = true;
                        nSubAccount.StateCode = AccountState.Active;

                        crmContext.AddObject(nSubAccount);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Sub Accounts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static List<OrganizationRequest> ImportSingaporeTicket(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return new List<OrganizationRequest>();
            }

            OptionMetadataCollection colOpSingaporeTicket = GetOptionSetMetadata(localContext, st_singaporeticket.EntityLogicalName, st_singaporeticket.Fields.st_SingTicketType);
            List<OrganizationRequest> lUpsertRequests = new List<OrganizationRequest>();

            Console.Write("Creating Batch of Singapore Tickets... ");
            using (ProgressBar progress = new ProgressBar())
            {
                for (int i = 0; i < importExcelInfo.lData.Count; i++)
                {
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        string ticketId = GetValueFromLine(importExcelInfo, line, "TicketId");
                        string crmNummer = GetValueFromLine(importExcelInfo, line, "cgi_contactnumber");

                        // Set alternate Key
                        KeyAttributeCollection altKey = new KeyAttributeCollection();
                        altKey.Add("st_ticketid", ticketId);
                        altKey.Add("ed_crmnummer", crmNummer);

                        Entity upsertSingaporeTicket = new Entity("st_singaporeticket", altKey);

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if (selectedData == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                                continue;
                            }

                            ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

                            if (selectedColumn == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null. Contact your administrator.");
                                continue;
                            }
                            st_singaporeticket x = new st_singaporeticket();
                            string name = selectedColumn.name;
                            string value = selectedData.value;

                            if (value == null || string.IsNullOrEmpty(value))
                                continue;

                            switch (name)
                            {
                                case "LastUpdated":

                                    DateTime lastUpdated = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out lastUpdated))
                                    {
                                        if (lastUpdated != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_lastupdated"] = lastUpdated;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");
                                    
                                    break;
                                case "TicketId":
                                case "cgi_contactnumber":
                                    break;
                                case "PriceModel":
                                    upsertSingaporeTicket["st_pricemodel"] = value;
                                    break;
                                case "PriceModelPriceId":
                                    upsertSingaporeTicket["st_pricemodelprice"] = value;
                                    break;
                                case "DataCreatedDate":

                                    DateTime ticketCreated = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out ticketCreated))
                                    {
                                        if (ticketCreated != DateTime.MinValue)
                                            upsertSingaporeTicket["st_ticketcreated"] = ticketCreated;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "activatedDate":

                                    DateTime ticketActivated = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out ticketActivated))
                                    {
                                        if (ticketActivated != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_ticketactivated"] = ticketActivated;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "blockedDate":

                                    DateTime blockedDate = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out blockedDate))
                                    {
                                        if (blockedDate != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_blockeddate"] = blockedDate;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "OfferName":

                                    int? optionSetST = GetOptionSetValueByName(colOpSingaporeTicket, value);

                                    if (optionSetST == null)
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM.");

                                    if (optionSetST != null)
                                        upsertSingaporeTicket["st_singtickettype"] = new OptionSetValue((int)optionSetST);

                                    break;
                                case "OfferNameDetailed":
                                    upsertSingaporeTicket["ed_offernamedetailed"] = value;
                                    break;
                                case "TicketAmount":

                                    decimal ticketPrice = decimal.MinValue;

                                    if (decimal.TryParse(value, out ticketPrice))
                                    {
                                        if (ticketPrice != decimal.MinValue)
                                            upsertSingaporeTicket["st_ticketprice"] = new Money(ticketPrice);
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a Money value.");

                                    break;
                                case "SalesChannelId":
                                    upsertSingaporeTicket["ed_saleschannel"] = value;
                                    break;
                                case "BearerCategory":
                                    upsertSingaporeTicket["ed_bearercategory"] = value;
                                    break;
                                case "HasGroupDiscount":

                                    bool? hasGroupDiscount = null;

                                    if (value == "1")
                                        hasGroupDiscount = true;
                                    else if (value == "0")
                                        hasGroupDiscount = false;

                                    if(hasGroupDiscount != null)
                                        upsertSingaporeTicket["ed_hasgroupdiscount"] = hasGroupDiscount;

                                    break;
                                case "TravellerCount":

                                    int travellerCount = int.MinValue;

                                    if (int.TryParse(value, out travellerCount))
                                    {
                                        if (travellerCount != int.MinValue)
                                            upsertSingaporeTicket["ed_travellerscount"] = travellerCount;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a int value.");

                                    break;
                                case "HasRefund":

                                    bool? hasRefund = null;

                                    if (value == "1")
                                        hasRefund = true;
                                    else if (value == "0")
                                        hasRefund = false;

                                    if (hasRefund != null)
                                        upsertSingaporeTicket["ed_hasrefund"] = hasRefund;

                                    break;
                                case "activationinternal_from":

                                    DateTime activationFrom = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out activationFrom))
                                    {
                                        if (activationFrom != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_activationintervalfrom"] = activationFrom;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "activationinternal_to":

                                    DateTime activationTo = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out activationTo))
                                    {
                                        if (activationTo != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_activationintervalto"] = activationTo;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "travelvalidityinternal_from":

                                    DateTime travelValidityFrom = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out travelValidityFrom))
                                    {
                                        if (travelValidityFrom != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_travelvalidityintervalfrom"] = travelValidityFrom;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "travelvalidityinternal_to":

                                    DateTime travelValidityTo = DateTime.MinValue;

                                    if (DateTime.TryParse(value, out travelValidityTo))
                                    {
                                        if (travelValidityTo != DateTime.MinValue)
                                            upsertSingaporeTicket["ed_travelvalidityintervalto"] = travelValidityTo;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                                    break;

                            default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        UpsertRequest request = new UpsertRequest()
                        {
                            Target = upsertSingaporeTicket
                        };

                        lUpsertRequests.Add(request);
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Singapore Tickets Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
            return lUpsertRequests;
        }

        #region Small Helper Methods

        public static string cleanMobileTelefon(string value)
        {
            return value.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("'", "").Replace(" ", "").Replace("–", "").Replace("/", "").Replace(":", "").Replace("Mobil", "");
        }

        public static string getSubString(string value, int max)
        {
            string cleanValue = System.Text.RegularExpressions.Regex.Unescape(value);
            return cleanValue.Length > max ? cleanValue.Substring(0, max - 1) : cleanValue;
        }

        public static string getCleanToValue(string value)
        {
            if (value.Contains("|"))
            {
                List<string> lStrings = value.Split('|').ToList();
                if (lStrings.Count > 0)
                    return lStrings.FirstOrDefault();
            }

            return value;
        }

        public static bool GetParsingStatus(ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null)
                return true;

            int nColumns = importExcelInfo.lColumns.Count;

            List<List<ExcelLineData>> lAux = importExcelInfo.lData.Where(x => x.Count != nColumns).ToList();

            if (lAux.Count == 0)
                return true;
            else
                return false;
        }

        public static string GetOrderId(string fileName)
        {
            //Leveransrapport_NowaKommunikationAB_1282
            List<string> nameParts = fileName.Split('_').ToList();
            string orderIdExtension = nameParts.LastOrDefault();

            List<string> orderIdParts = orderIdExtension.Split('.').ToList();
            string orderId = orderIdParts.FirstOrDefault();

            int iOrderId = int.MinValue;

            if (int.TryParse(orderId, out iOrderId))
                return orderId;
            else
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to Convert " + orderId + " to an integer.");
                return null;
            }
        }

        public static string GetValueFromLine(ImportExcelInfo importExcelInfo, List<ExcelLineData> line, string labelCsv)
        {
            ExcelColumn column = GetSelectedExcelColumnByName(importExcelInfo.lColumns, labelCsv);

            if (column == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null. Contact your administrator.");
                return null;
            }

            ExcelLineData selectedData = line[column.index];

            if (selectedData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                return null;
            }

            return selectedData.value;
        }

        public static List<ExcelLineData> GetColumnData(ImportExcelInfo importExcelInfo, string labelCsv)
        {
            ExcelColumn column = GetSelectedExcelColumnByName(importExcelInfo.lColumns, labelCsv);

            if (column == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null. Contact your administrator.");
                return null;
            }

            List<ExcelLineData> excelLineData = new List<ExcelLineData>();

            foreach (List<ExcelLineData> lineData in importExcelInfo.lData)
                foreach (ExcelLineData item in lineData)
                    if (item.index == column.index)
                        excelLineData.Add(item);

            return excelLineData;
        }

        public static List<UpsalesFile> GetFileList(string path, string filter)
        {
            try
            {
                string[] files = Directory.GetFiles(path, filter);

                List<UpsalesFile> lFiles = new List<UpsalesFile>();

                for (int i = 0; i < files.Length; i++)
                {
                    UpsalesFile uFile = new UpsalesFile();
                    uFile.fileName = Path.GetFileName(files[i]);
                    uFile.filePath = files[i];

                    lFiles.Add(uFile);
                }

                return lFiles;
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, string.Format("The List of files was not retrieved: {0}", e.ToString()));
                return null;
            }
        }

        public static ExcelColumn GetSelectedExcelColumn(List<ExcelColumn> lColumns, int j)
        {
            List<ExcelColumn> lSelectedColumns = lColumns.Where(x => x.index == j).ToList();

            if (lSelectedColumns.Count == 0)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Columns found with Index: " + j);
                return null;
            }
            else if (lSelectedColumns.Count > 1)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Columns found with Index: " + j);
                return null;
            }

            return lSelectedColumns.FirstOrDefault();
        }

        public static ExcelColumn GetSelectedExcelColumnByName(List<ExcelColumn> lColumns, string name)
        {
            List<ExcelColumn> lSelectedColumns = lColumns.Where(x => x.name == name).ToList();

            if (lSelectedColumns.Count == 0)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Columns found with Name: " + name);
                return null;
            }
            else if (lSelectedColumns.Count > 1)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Columns found with Name: " + name);
                return null;
            }

            return lSelectedColumns.FirstOrDefault();
        }

        public static ImportExcelInfo HandleExcelInformationStreamReader(string relativeExcelPath, string fileName)
        {
            try
            {
                int i = 0;
                List<ExcelColumn> lColumns = new List<ExcelColumn>();
                List<List<ExcelLineData>> lData = new List<List<ExcelLineData>>();

                using (var reader = new StreamReader(relativeExcelPath + "\\" + fileName, Encoding.GetEncoding("iso-8859-1")))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        List<string> values = line.Split(';').ToList();
                        if (i == 0)
                        {
                            for (int j = 0; j < values.Count; j++)
                            {
                                ExcelColumn column = new ExcelColumn(j, values[j]);
                                lColumns.Add(column);
                            }
                        }
                        else
                        {
                            List<ExcelLineData> lLine = new List<ExcelLineData>();
                            for (int k = 0; k < values.Count; k++)
                            {
                                ExcelLineData dataLine = new ExcelLineData(k, values[k]);
                                lLine.Add(dataLine);
                            }

                            lData.Add(lLine);
                        }
                        i++;
                    }
                }

                return new ImportExcelInfo(lColumns, lData);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Details: " + e.Message);
                return null;
            }
        }

        public static ImportExcelInfo HandleExcelInformationStreamReader(string relativeExcelPath, List<string> fileNames)
        {
            try
            {
                ImportExcelInfo importExcelInfo = new ImportExcelInfo();

                foreach (string fileName in fileNames)
                {
                    int i = 0;
                    List<ExcelColumn> lColumns = new List<ExcelColumn>();
                    List<List<ExcelLineData>> lData = new List<List<ExcelLineData>>();

                    using (var reader = new StreamReader(relativeExcelPath + "\\" + fileName, Encoding.GetEncoding("iso-8859-1")))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            List<string> values = line.Split(';').ToList();
                            if (i == 0)
                            {
                                for (int j = 0; j < values.Count; j++)
                                {
                                    ExcelColumn column = new ExcelColumn(j, values[j]);
                                    lColumns.Add(column);
                                }
                            }
                            else
                            {
                                List<ExcelLineData> lLine = new List<ExcelLineData>();
                                for (int k = 0; k < values.Count; k++)
                                {
                                    ExcelLineData dataLine = new ExcelLineData(k, values[k]);
                                    lLine.Add(dataLine);
                                }

                                lData.Add(lLine);
                            }
                            i++;
                        }
                    }

                    if(importExcelInfo.lColumns.Count != 0 && importExcelInfo.lColumns.Count != lColumns.Count)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Details: Fatal error reading CSV Columns.");
                        return null;
                    }
                        
                    importExcelInfo.lColumns = lColumns;
                    importExcelInfo.lData.AddRange(lData);
                }

                return importExcelInfo;
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Details: " + e.Message);
                return null;
            }
        }

        public static EntityReference GetCRMPriceListByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterPriceList = new FilterExpression();
            filterPriceList.Conditions.Add(new ConditionExpression(PriceLevel.Fields.Name, ConditionOperator.Equal, name));

            List<PriceLevel> lPriceLevels = XrmRetrieveHelper.RetrieveMultiple<PriceLevel>(localContext, new ColumnSet(PriceLevel.Fields.PriceLevelId), filterPriceList).ToList();

            if (lPriceLevels.Count == 1)
                return lPriceLevels.FirstOrDefault().ToEntityReference();
            else if (lPriceLevels.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Price Lists found with Name: " + name + ".");
            else if (lPriceLevels.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Price List found with Name: " + name + ".");

            return null;
        }

        public static EntityReference GetCrmCurrencyByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterCurrencies = new FilterExpression();
            filterCurrencies.Conditions.Add(new ConditionExpression(TransactionCurrency.Fields.CurrencyName, ConditionOperator.Equal, name));

            List<TransactionCurrency> lCurrencies = XrmRetrieveHelper.RetrieveMultiple<TransactionCurrency>(localContext, new ColumnSet(TransactionCurrency.Fields.TransactionCurrencyId), filterCurrencies).ToList();

            if (lCurrencies.Count == 1)
                return lCurrencies.FirstOrDefault().ToEntityReference();
            else if (lCurrencies.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Currencies found with Currency Name: " + name + ".");
            else if (lCurrencies.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Currency found with Currency Name: " + name + ".");

            return null;
        }

        public static EntityReference GetCrmUserOrTeamByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterUsers = new FilterExpression();
            filterUsers.Conditions.Add(new ConditionExpression(SystemUser.Fields.FullName, ConditionOperator.Equal, name));

            List<SystemUser> lUsers = XrmRetrieveHelper.RetrieveMultiple<SystemUser>(localContext, new ColumnSet(SystemUser.Fields.SystemUserId), filterUsers).ToList();

            if (lUsers.Count == 1)
                return lUsers.FirstOrDefault().ToEntityReference();

            FilterExpression filterTeams = new FilterExpression();
            filterTeams.Conditions.Add(new ConditionExpression(Team.Fields.Name, ConditionOperator.Equal, name));

            List<Team> lTeams = XrmRetrieveHelper.RetrieveMultiple<Team>(localContext, new ColumnSet(), filterTeams).ToList();

            if (lTeams.Count == 1)
                return lTeams.FirstOrDefault().ToEntityReference();
            else if (lTeams.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Users/Teams found with Name: " + name + ".");
            else if (lTeams.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One User/Team found with Name: " + name + ".");

            return null;
        }

        public static EntityReference GetCrmAccountByOrganizationNumber(Plugin.LocalPluginContext localContext, string orgNumber)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.cgi_organizational_number, ConditionOperator.Equal, orgNumber));
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.StateCode, ConditionOperator.Equal, (int)AccountState.Active));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();
            else if (lAccounts.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Active Accounts found with field 'cgi_organizational_number' = " + orgNumber + ".");
            else if (lAccounts.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Active Account found with field 'cgi_organizational_number' = " + orgNumber + ".");

            return null;
        }

        public static EntityReference GetCrmAccountByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();
            else if (lAccounts.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Accounts found with Upsales Id: " + upsalesId + ".");
            else if (lAccounts.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Account found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static EntityReference GetCrmContactByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterContacts = new FilterExpression();
            filterContacts.Conditions.Add(new ConditionExpression(Contact.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, new ColumnSet(Contact.Fields.ContactId), filterContacts).ToList();

            if (lContacts.Count == 1)
                return lContacts.FirstOrDefault().ToEntityReference();
            else if (lContacts.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Contacts found with Upsales Id: " + upsalesId + ".");
            else if (lContacts.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Contact found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static EntityReference GetCrmPhoneCallByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterPhoneCall = new FilterExpression();
            filterPhoneCall.Conditions.Add(new ConditionExpression(PhoneCall.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<PhoneCall> lPhoneCall = XrmRetrieveHelper.RetrieveMultiple<PhoneCall>(localContext, new ColumnSet(PhoneCall.Fields.ActivityId), filterPhoneCall).ToList();

            if (lPhoneCall.Count == 1)
                return lPhoneCall.FirstOrDefault().ToEntityReference();
            else if (lPhoneCall.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No PhoneCalls found with Upsales Id: " + upsalesId + ".");
            else if (lPhoneCall.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One PhoneCall found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static EntityReference GetCrmEmailByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterEmail = new FilterExpression();
            filterEmail.Conditions.Add(new ConditionExpression(Email.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Email> lEmail = XrmRetrieveHelper.RetrieveMultiple<Email>(localContext, new ColumnSet(Email.Fields.ActivityId), filterEmail).ToList();

            if (lEmail.Count == 1)
                return lEmail.FirstOrDefault().ToEntityReference();
            else if (lEmail.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Emails found with Upsales Id: " + upsalesId + ".");
            else if (lEmail.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Email found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static EntityReference GetCrmAppointmentByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterAppointment = new FilterExpression();
            filterAppointment.Conditions.Add(new ConditionExpression(Appointment.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Appointment> lAppointments = XrmRetrieveHelper.RetrieveMultiple<Appointment>(localContext, new ColumnSet(Appointment.Fields.ActivityId), filterAppointment).ToList();

            if (lAppointments.Count == 1)
                return lAppointments.FirstOrDefault().ToEntityReference();
            else if (lAppointments.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Appointments found with Upsales Id: " + upsalesId + ".");
            else if (lAppointments.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Appointment found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static EntityReference GetCrmOrderByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterOrder = new FilterExpression();
            filterOrder.Conditions.Add(new ConditionExpression(SalesOrder.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<SalesOrder> lSalesOrder = XrmRetrieveHelper.RetrieveMultiple<SalesOrder>(localContext, new ColumnSet(SalesOrder.Fields.SalesOrderId), filterOrder).ToList();

            if (lSalesOrder.Count == 1)
                return lSalesOrder.FirstOrDefault().ToEntityReference();
            else if (lSalesOrder.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Orders found with Upsales Id: " + upsalesId + ".");
            else if (lSalesOrder.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Order found with Upsales Id: " + upsalesId + ".");

            return null;
        }

        public static void FulFillOrder(Plugin.LocalPluginContext localContext, Guid orderId)
        {
            int newStatus = (int)salesorder_statuscode.Complete;

            Entity orderClose = new Entity("orderclose");
            orderClose["salesorderid"] = new EntityReference(SalesOrder.EntityLogicalName, orderId);
            FulfillSalesOrderRequest request = new FulfillSalesOrderRequest
            {
                OrderClose = orderClose,
                Status = new OptionSetValue(newStatus)
            };

            try
            {
                localContext.OrganizationService.Execute(request);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error setting the Sales Order to Completed. Details: " + e.Message);
            }
        }

        public static void CreateSalesOrderDetail(Plugin.LocalPluginContext localContext, CrmContext crmContext, decimal dtotalLineAmount, EntityReference erOrder)
        {
            SalesOrderDetail nOrderProduct = null;

            try
            {
                nOrderProduct = new SalesOrderDetail();
                nOrderProduct.IsProductOverridden = true;
                nOrderProduct.IsPriceOverridden = true;
                nOrderProduct.ProductDescription = "Upsales Order Value";
                nOrderProduct.Quantity = 1M;
                nOrderProduct.PricePerUnit = new Money(dtotalLineAmount);
                nOrderProduct.ManualDiscountAmount = new Money(0M);
                nOrderProduct.Tax = new Money(0M);
                nOrderProduct.SalesOrderId = erOrder;

                crmContext.AddObject(nOrderProduct);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error adding Order Product to Batch. Details: " + e.Message);
            }
        }

        public static OptionMetadataCollection GetOptionSetMetadata(Plugin.LocalPluginContext localContext, string entityName, string attributeName)
        {
            RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };

            RetrieveAttributeResponse attributeResponse = (RetrieveAttributeResponse)localContext.OrganizationService.Execute(attributeRequest);
            PicklistAttributeMetadata attributeMetadata = (PicklistAttributeMetadata)attributeResponse.AttributeMetadata;

            return attributeMetadata?.OptionSet?.Options;
        }

        public static int? GetOptionSetValueByName(OptionMetadataCollection colOptions, string name)
        {
            OptionMetadata option = colOptions.FirstOrDefault(x => x.Label.UserLocalizedLabel.Label == name);

            if (option == null)
                return null;

            return option.Value;
        }

        public static string GetOptionSetValueByValue(OptionMetadataCollection colOptions, int value)
        {
            OptionMetadata option = colOptions.FirstOrDefault(x => x.Value == value);

            if (option == null)
                return null;

            return option.Label.UserLocalizedLabel.Label;
        }

        public static int? InsertGlobalOptionSetOption(Plugin.LocalPluginContext localContext, string optionSetName, string label, int languageCode)
        {
            try
            {
                InsertOptionValueRequest insertOptionValueRequest =
                new InsertOptionValueRequest
                {
                    OptionSetName = optionSetName,
                    Label = new Label(label, languageCode)
                };

                InsertOptionValueResponse insertedOption = ((InsertOptionValueResponse)localContext.OrganizationService.Execute(insertOptionValueRequest));

                //Publish the OptionSet
                PublishXmlRequest pxReq2 = new PublishXmlRequest { ParameterXml = String.Format("<importexportxml><optionsets><optionset>{0}</optionset></optionsets></importexportxml>", optionSetName) };
                localContext.OrganizationService.Execute(pxReq2);

                return insertedOption.NewOptionValue;
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error getting Global OptionSet " + optionSetName + ". Details: " + e.Message);
                return null;
            }
        }

        public static void LogCrmContextMultipleResponses(Plugin.LocalPluginContext localContext, SaveChangesResultCollection lResponses)
        {
            foreach (SaveChangesResult response in lResponses)
            {
                try
                {
                    // A valid response.
                    if (response.Error == null)
                    {
                        if (response.Response.ResponseName == "Update")
                        {
                            Entity entity = (Entity)response.Request["Target"];

                            if (entity == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"A record has been updated, but no Request was retrieved.");
                                continue;
                            }

                            switch (entity.LogicalName)
                            {
                                case Account.EntityLogicalName:

                                    Account account = (Account)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Account with Id: " + account.Id + " was updated.");

                                    break;
                                case Contact.EntityLogicalName:

                                    Contact contact = (Contact)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Contact with Id: " + contact.Id + " was updated.");

                                    break;
                                case PhoneCall.EntityLogicalName:

                                    PhoneCall phoneCall = (PhoneCall)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"PhoneCall with Id: " + phoneCall.Id + " was updated.");

                                    break;
                                case Email.EntityLogicalName:

                                    Email email = (Email)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Email with Id: " + email.Id + " was updated.");

                                    break;
                                case Appointment.EntityLogicalName:

                                    Appointment appointment = (Appointment)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Appointment with Id: " + appointment.Id + " was updated.");

                                    break;
                                case SalesOrder.EntityLogicalName:

                                    SalesOrder salesOrder = (SalesOrder)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Sales Order with Id: " + salesOrder.Id + " was updated.");

                                    break;

                                case Annotation.EntityLogicalName:

                                    Annotation annotation = (Annotation)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Annotation with Id: " + annotation.Id + " was updated.");

                                    break;
                                default:
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"No logs implemented for " + entity.LogicalName + ". Please contact you administrator.");
                                    break;
                            }
                        }
                        else if (response.Response.ResponseName == "Create")
                        {
                            Entity entity = (Entity)response.Request["Target"];
                            Guid id = (Guid)response.Response["id"];

                            if (id == null || entity == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"A record has been created, but no id or Request was retrieved.");
                                continue;
                            }

                            switch (entity.LogicalName)
                            {
                                case Account.EntityLogicalName:

                                    Account account = (Account)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Account with Name: " + account.Name + " was created with id: " + id + ".");

                                    break;
                                case Contact.EntityLogicalName:

                                    Contact contact = (Contact)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Contact with Name: " + contact.FirstName + " " + contact.LastName + " was created with id: " + id + ".");

                                    break;
                                case PhoneCall.EntityLogicalName:

                                    PhoneCall phoneCall = (PhoneCall)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"PhoneCall with Subject: " + phoneCall.Subject + " was created with id: " + id + ".");

                                    SetStateRequest requestPhoneCall = new SetStateRequest
                                    {
                                        EntityMoniker = new EntityReference(PhoneCall.EntityLogicalName, id),
                                        State = new OptionSetValue((int)PhoneCallState.Completed),
                                        Status = new OptionSetValue((int)phonecall_statuscode.Made)
                                    };

                                    try
                                    {
                                        localContext.OrganizationService.Execute(requestPhoneCall);
                                    }
                                    catch (Exception e)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error setting the Phone Call to Completed. Details: " + e.Message);
                                    }

                                    break;
                                case Email.EntityLogicalName:

                                    Email email = (Email)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Email with Subject: " + email.Subject + " was created with id: " + id + ".");

                                    SetStateRequest requestEmail = new SetStateRequest
                                    {
                                        EntityMoniker = new EntityReference(Email.EntityLogicalName, id),
                                        State = new OptionSetValue((int)EmailState.Completed),
                                        Status = new OptionSetValue((int)email_statuscode.Completed)
                                    };

                                    try
                                    {
                                        localContext.OrganizationService.Execute(requestEmail);
                                    }
                                    catch (Exception e)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error setting the Email to Completed. Details: " + e.Message);
                                    }

                                    break;
                                case Appointment.EntityLogicalName:

                                    Appointment appointment = (Appointment)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Appointment with Subject: " + appointment.Subject + " was created with id: " + id + ".");

                                    SetStateRequest requestAppointment = new SetStateRequest
                                    {
                                        EntityMoniker = new EntityReference(Appointment.EntityLogicalName, id),
                                        State = new OptionSetValue((int)AppointmentState.Completed),
                                        Status = new OptionSetValue((int)appointment_statuscode.Completed)
                                    };

                                    try
                                    {
                                        localContext.OrganizationService.Execute(requestAppointment);
                                    }
                                    catch (Exception e)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error setting the Appointment to Completed. Details: " + e.Message);
                                    }

                                    break;
                                case SalesOrder.EntityLogicalName:

                                    SalesOrder salesOrder = (SalesOrder)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Sales Order with Name: " + salesOrder.Name + " was created with id: " + id + ".");

                                    FulFillOrder(localContext, id);

                                    break;
                                case SalesOrderDetail.EntityLogicalName:

                                    SalesOrderDetail salesOrderDetail = (SalesOrderDetail)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Sales Order Detail with value: " + salesOrderDetail.PricePerUnit?.Value + " was created with id: " + id + ".");

                                    EntityReference erOrder = salesOrderDetail.SalesOrderId;

                                    if (erOrder != null)
                                    {
                                        FulFillOrder(localContext, erOrder.Id);
                                    }

                                    break;
                                case Annotation.EntityLogicalName:

                                    Annotation annotation = (Annotation)entity;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Annotation with Name: " + annotation.Subject + " was created with id: " + id + ".");

                                    break;
                                default:
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"No logs implemented for " + entity.LogicalName + ". Please contact you administrator.");
                                    break;
                            }
                        }
                        else if (response.Response.ResponseName == "Associate")
                        {
                            EntityReference entityRef = (EntityReference)response.Request["Target"];
                            EntityReferenceCollection lEntityReference = (EntityReferenceCollection)response.Request["RelatedEntities"];
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The " + entityRef.LogicalName + " with id: " + entityRef.Id + " was associated with " + lEntityReference.Count + " record(s)");
                        }
                    }
                    //An error has occurred.
                    else
                    {
                        if (response.Request.RequestName == "Associate")
                        {
                            EntityReference entityRef = (EntityReference)response.Request["Target"];
                            EntityReferenceCollection lEntityReference = (EntityReferenceCollection)response.Request["RelatedEntities"];
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Failed to Associate the " + entityRef.LogicalName + " with id: " + entityRef.Id + " with " + lEntityReference.Count + " record(s). Details: " + response.Error.Message);
                        }
                        else
                        {
                            Entity entity = (Entity)response.Request["Target"];

                            if (entity == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"A record has failed to be created, but no Request was retrieved.");
                                continue;
                            }

                            switch (entity.LogicalName)
                            {
                                case Account.EntityLogicalName:

                                    Account account = (Account)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Account with Name: " + account.Name + " was not created. Details: " + response.Error.Message);

                                    break;
                                case Contact.EntityLogicalName:

                                    Contact contact = (Contact)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Contact with Name: " + contact.FirstName + " " + contact.LastName + " was not created. Details: " + response.Error.Message);

                                    break;
                                case PhoneCall.EntityLogicalName:

                                    PhoneCall phoneCall = (PhoneCall)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - PhoneCall with Subject: " + phoneCall.Subject + " was not created. Details: " + response.Error.Message);

                                    break;
                                case Email.EntityLogicalName:

                                    Email email = (Email)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Email with Subject: " + email.Subject + " was not created. Details: " + response.Error.Message);

                                    break;
                                case Appointment.EntityLogicalName:

                                    Appointment appointment = (Appointment)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Appointment with Subject: " + appointment.Subject + " was not created. Details: " + response.Error.Message);

                                    break;
                                case SalesOrder.EntityLogicalName:

                                    SalesOrder salesOrder = (SalesOrder)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Sales Order with Name: " + salesOrder.Name + " was not created. Details: " + response.Error.Message);

                                    break;
                                case SalesOrderDetail.EntityLogicalName:

                                    SalesOrderDetail salesOrderDetail = (SalesOrderDetail)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Sales Order Detail with value: " + salesOrderDetail.PricePerUnit?.Value + " was not created. Details: " + response.Error.Message);

                                    break;
                                case Annotation.EntityLogicalName:

                                    Annotation annotation = (Annotation)entity;
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Annotation with Name: " + annotation.Subject + " was not created. Details: " + response.Error.Message);

                                    break;
                                default:
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"No logs implemented for " + entity.LogicalName + ". Please contact you administrator.");
                                    break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error logging results. Details: " + e.Message);
                }
            }
        }

        public static PhoneCall GetPhoneCall(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
        {
            PhoneCall nPhoneCall = new PhoneCall();
            List<ExcelLineData> line = importExcelInfo.lData[i];

            for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
            {
                ExcelLineData selectedData = line[j];

                if (selectedData == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                    continue;
                }

                ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                    case "Account U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The RegardingObjectId with Upsales Id: " + value + " of the PhoneCall was updated.");
                            nPhoneCall.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The PhoneCall on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The To of the PhoneCall was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nPhoneCall.To = phoneCallTo;

                        break;
                    case "Activity U-ID":

                        EntityReference erPhoneCall = GetCrmPhoneCallByUpsalesId(localContext, value);

                        if (erPhoneCall != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Phone Call with Upsales Id: " + value + " was already imported.");
                            continue;
                        }

                        nPhoneCall.ed_UpsalesId = value;
                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":
                        //Already Handleded
                        break;
                    case "Activity: Beskrivning":
                        nPhoneCall.Subject = getSubString(value, maxSubject);
                        break;
                    case "Datum":

                        DateTime scheduleStart = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleStart))
                        {
                            if (scheduleStart != DateTime.MinValue)
                                nPhoneCall.ScheduledStart = scheduleStart;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nPhoneCall.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":
                        //Not Possible to Update Modified On Unsuported
                        break;
                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nPhoneCall.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner " + value + " of the PhoneCall was not found. The default user was updated.");
                            nPhoneCall.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nPhoneCall.OwnerId = erOwner;
                        else
                        {
                            nPhoneCall.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Activity: Anteckningar":
                        nPhoneCall.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nPhoneCall.RegardingObjectId == null || nPhoneCall.ed_UpsalesId == null)
                return null;

            if (erDefaultUser != null)
            {
                ActivityParty fromParty = new ActivityParty();
                fromParty.PartyId = erDefaultUser;

                List<ActivityParty> phoneCallFrom = new List<ActivityParty>();
                phoneCallFrom.Add(fromParty);

                nPhoneCall.From = phoneCallFrom;
            }

            return nPhoneCall;
        }

        public static Email GetEmail(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
        {
            Email nEmail = new Email();
            List<ExcelLineData> line = importExcelInfo.lData[i];

            for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
            {
                ExcelLineData selectedData = line[j];

                if (selectedData == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                    continue;
                }

                ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                    case "Account U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The RegardingObjectId with Upsales Id: " + value + " of the Email was updated.");
                            nEmail.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Email on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The To of the Email was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> EmailTo = new List<ActivityParty>();
                        EmailTo.Add(toParty);

                        nEmail.To = EmailTo;

                        break;
                    case "Activity U-ID":

                        EntityReference erEmail = GetCrmEmailByUpsalesId(localContext, value);

                        if (erEmail != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Email with Upsales Id: " + value + " was already imported.");
                            continue;
                        }

                        nEmail.ed_UpsalesId = value;
                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":
                        //Already Handleded
                        break;

                    case "Activity: Beskrivning":
                        nEmail.Subject = getSubString(value, maxSubject);
                        break;
                    case "Datum":

                        DateTime scheduleStart = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleStart))
                        {
                            if (scheduleStart != DateTime.MinValue)
                                nEmail.ScheduledStart = scheduleStart;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nEmail.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":
                        //Not Possible to Update Modified On Unsuported
                        break;
                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nEmail.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner " + value + " of the Email was not found. The default user was updated.");
                            nEmail.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nEmail.OwnerId = erOwner;
                        else
                        {
                            nEmail.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Activity: Anteckningar":
                        nEmail.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nEmail.RegardingObjectId == null || nEmail.ed_UpsalesId == null)
                return null;

            if (erDefaultUser != null)
            {
                ActivityParty fromParty = new ActivityParty();
                fromParty.PartyId = erDefaultUser;

                List<ActivityParty> emailFrom = new List<ActivityParty>();
                emailFrom.Add(fromParty);

                nEmail.From = emailFrom;
            }

            return nEmail;
        }

        public static Appointment GetAppointment(Plugin.LocalPluginContext localContext, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
        {
            Appointment nAppointment = new Appointment();
            List<ExcelLineData> line = importExcelInfo.lData[i];

            for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
            {
                ExcelLineData selectedData = line[j];

                if (selectedData == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                    continue;
                }

                ExcelColumn selectedColumn = GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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
                    case "Account U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The RegardingObjectId with Upsales Id: " + value + " of the Appointment was updated.");
                            nAppointment.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Appointment on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The To of the Email was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> AppointmentTo = new List<ActivityParty>();
                        AppointmentTo.Add(toParty);

                        nAppointment.RequiredAttendees = AppointmentTo;

                        break;
                    case "Activity U-ID":

                        EntityReference erAppointment = GetCrmAppointmentByUpsalesId(localContext, value);

                        if (erAppointment != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Appointment with Upsales Id: " + value + " was already imported.");
                            continue;
                        }

                        nAppointment.ed_UpsalesId = value;
                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":
                        //Already Handleded
                        break;

                    case "Activity: Beskrivning":
                        nAppointment.Subject = getSubString(value, maxSubject);
                        break;
                    case "Datum":

                        DateTime scheduleStart = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleStart))
                        {
                            if (scheduleStart != DateTime.MinValue)
                                nAppointment.ScheduledStart = scheduleStart;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nAppointment.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":
                        //Not Possible to Update Modified On Unsuported
                        break;
                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nAppointment.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner " + value + " of the Appointment was not found. The default user was updated.");
                            nAppointment.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nAppointment.OwnerId = erOwner;
                        else
                        {
                            nAppointment.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Activity: Anteckningar":
                        nAppointment.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nAppointment.RegardingObjectId == null || nAppointment.ed_UpsalesId == null)
                return null;

            return nAppointment;
        }


        #endregion
    }
}
