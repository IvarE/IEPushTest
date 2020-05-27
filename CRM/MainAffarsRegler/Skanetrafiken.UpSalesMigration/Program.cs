using Endeavor.Crm;
using Endeavor.Crm.UpSalesMigration;
using log4net;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.UpSalesMigration.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Description;

using ExcelApp = Microsoft.Office.Interop.Excel;

namespace Skanetrafiken.UpSalesMigration
{
    class Program
    {
        public static string rel_account_note = "Account_Annotation";
        public static string rel_account_address = "Account_CustomerAddress";
        public static string rel_contact_note = "Contact_Annotation";
        public static string rel_contact_account = "account_primary_contact";

        public static IOrganizationService _service = null;
        static ExcelApp.Application excelApp = new ExcelApp.Application();
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Account> lAccountsUpdate = new List<Account>();

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

        public static string cleanMobileTelefon(string value)
        {
            return value.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("'", "").Replace(" ", "");
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
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Columns found with Index: " + j);
                return null;
            }
            else if (lSelectedColumns.Count > 1)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"One Or More Columns found with Index: " + j);
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
                _log.InfoFormat(CultureInfo.InvariantCulture, $"One Or More Columns found with Name: " + name);
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

        public static void PublishGlobalOptionSet(Plugin.LocalPluginContext localContext, string optionSetName)
        {
            try
            {
                //Publish the OptionSet
                PublishXmlRequest pxReq2 = new PublishXmlRequest { ParameterXml = String.Format("<importexportxml><optionsets><optionset>{0}</optionset></optionsets></importexportxml>", optionSetName) };
                localContext.OrganizationService.Execute(pxReq2);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error publishing Global OptionSet " + optionSetName + ". Details: " + e.Message);
            }
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

                return insertedOption.NewOptionValue;
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error getting Global OptionSet " + optionSetName + ". Details: " + e.Message);
                return null;
            }
        }

        public static int? InsertLocalOptionSetOption(Plugin.LocalPluginContext localContext, string entityName, string optionSetName, string label, int languageCode)
        {
            // Create a request.
            InsertOptionValueRequest insertOptionValueRequest =
                new InsertOptionValueRequest
                {
                    AttributeLogicalName = optionSetName,
                    EntityLogicalName = entityName,
                    Label = new Label(label, languageCode)
                };
            // Execute the request.
            InsertOptionValueResponse response = ((InsertOptionValueResponse)localContext.OrganizationService.Execute(insertOptionValueRequest));


            //TODO TEST FOR ERROR RESPONSE
            if (response != null)
                return response.NewOptionValue;
            else
                return null;
        }

        public static void UpdateAccountsWithPrimaryContactId(Plugin.LocalPluginContext localContext, CrmContext crmContext)
        {
            if (lAccountsUpdate.Count == 0)
                return;

            List<Account> lFinalList = lAccountsUpdate.GroupBy(elem => elem.AccountId).Select(group => group.First()).ToList();

            foreach (Account account in lFinalList)
            {
                string upsalesId = account.Description;

                if (upsalesId == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Account object does not have Contact Upsales Id.");
                    continue;
                }

                QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
                queryContacts.NoLock = true;
                queryContacts.ColumnSet.AddColumns(Contact.Fields.ContactId, Contact.Fields.FirstName, Contact.Fields.LastName);
                queryContacts.Criteria.AddCondition(Contact.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId);

                List<Contact> lContact = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts).ToList();

                if (lContact.Count == 1)
                {
                    Guid contactId = (Guid)lContact.FirstOrDefault().ContactId;
                    string firstName = lContact.FirstOrDefault().FirstName;
                    string lastName = lContact.FirstOrDefault().LastName;

                    Account uAccount = new Account();
                    uAccount.Id = account.Id;
                    uAccount.PrimaryContactId = new EntityReference(Contact.EntityLogicalName, contactId);

                    if (!crmContext.IsAttached(uAccount))
                        crmContext.Attach(uAccount);

                    crmContext.UpdateObject(uAccount);

                    _log.InfoFormat(CultureInfo.InvariantCulture, $"A Request has been created for the Account " + account.Id + " to be updated with Contact " + firstName + " " + lastName + " as it's Primary Contact.");
                }
                else
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"There were found zero or more than one Contacts with Upsales Id: " + upsalesId);
            }
        }

        public static void LogCrmContextMultipleResponses(SaveChangesResultCollection lResponses)
        {
            //TODO
            try
            {
                foreach (SaveChangesResult response in lResponses)
                {
                    // A valid response.
                    if (response.Error == null)
                    {
                        Entity entity = (Entity)response.Request["Target"];

                        if (response.Response.ResponseName == "Update")
                        {
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
                                case "activity":
                                    break;
                                case Lead.EntityLogicalName:
                                    break;
                                case "opportunity":
                                    break;
                                default:
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"No logs implemented for " + entity.LogicalName + ". Please contact you administrator.");
                                    break;
                            }
                        }
                        else if(response.Response.ResponseName == "Create")
                        {
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
                                case "activity":
                                    break;
                                case Lead.EntityLogicalName:
                                    break;
                                case "opportunity":
                                    break;
                                default:
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"No logs implemented for " + entity.LogicalName + ". Please contact you administrator.");
                                    break;
                            }
                        }
                    }
                    //An error has occurred.
                    else
                    {
                        Entity entity = (Entity)response.Request["Target"];

                        if(entity == null)
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
                            case "activity":
                                break;
                            case Lead.EntityLogicalName:
                                break;
                            case "opportunity":
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

        public static PhoneCall GetPhoneCall(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i)
        {
            PhoneCall nPhoneCall = new PhoneCall();

            for (int j = 1; j <= importExcelInfo.colCount; j++)
            {
                if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                    continue;

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
                    case "Företag":

                        EntityReference erAccount = GetCrmAccountByName(localContext, value);

                        if (erAccount == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Regarding Object of the PhoneCall was not found. There is not Account with Name: " + value);
                            break;
                        }

                        nPhoneCall.RegardingObjectId = erAccount;

                        break;
                    case "Kontaktperson":

                        EntityReference erContact = GetCrmContactByFullName(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the PhoneCall was not found. There is not Contact with FullName: " + value);
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nPhoneCall.To = phoneCallTo;

                        break;
                    case "Beskrivning":
                        nPhoneCall.Subject = value;
                        break;
                    case "Datum":
                        //No Information
                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nPhoneCall.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":

                        //Not Possible Unsuported
                        //DateTime dateTime = DateTime.MinValue;

                        //if (DateTime.TryParse(value, out dateTime))
                        //{
                        //    if (dateTime != DateTime.MinValue)
                        //        nPhoneCall.ModifiedOn = dateTime;
                        //}
                        //else
                        //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;

                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nPhoneCall.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner == null)
                        {
                            string defaultUserName = "Admin"; //TODO Put on App Config
                            erOwner = GetCrmUserOrTeamByName(localContext, defaultUserName);
                        }

                        if (erOwner != null)
                            nPhoneCall.OwnerId = erOwner;
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Owner of the PhoneCall was not found.");

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nPhoneCall.Description = value;
                        break;

                    default:
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            return nPhoneCall;
        }

        public static Email GetEmail(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i)
        {
            Email nEmail = new Email();

            for (int j = 1; j <= importExcelInfo.colCount; j++)
            {
                if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                    continue;

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
                    case "Företag":

                        EntityReference erAccount = GetCrmAccountByName(localContext, value);

                        if (erAccount == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Regarding Object of the Email was not found. There is not Account with Name: " + value);
                            break;
                        }

                        nEmail.RegardingObjectId = erAccount;

                        break;
                    case "Kontaktperson":

                        EntityReference erContact = GetCrmContactByFullName(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the PhoneCall was not found. There is not Contact with FullName: " + value);
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nEmail.To = phoneCallTo;

                        break;
                    case "Beskrivning":
                        nEmail.Subject = value;
                        break;
                    case "Datum":
                        //No Information
                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nEmail.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":

                        //Not Possible Unsuported
                        //DateTime dateTime = DateTime.MinValue;

                        //if (DateTime.TryParse(value, out dateTime))
                        //{
                        //    if (dateTime != DateTime.MinValue)
                        //        nEmail.ModifiedOn = dateTime;
                        //}
                        //else
                        //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;

                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nEmail.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner == null)
                        {
                            string defaultUserName = "Admin"; //TODO Put on App Config
                            erOwner = GetCrmUserOrTeamByName(localContext, defaultUserName);
                        }

                        if (erOwner != null)
                            nEmail.OwnerId = erOwner;
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Owner of the Email was not found.");

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nEmail.Description = value;
                        break;

                    default:
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            return nEmail;
        }

        public static Appointment GetAppointment(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i)
        {
            Appointment nAppointment = new Appointment();

            for (int j = 1; j <= importExcelInfo.colCount; j++)
            {
                if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                    continue;

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
                    case "Företag":

                        EntityReference erAccount = GetCrmAccountByName(localContext, value);

                        if (erAccount == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Regarding Object of the Appointment was not found. There is not Account with Name: " + value);
                            break;
                        }

                        nAppointment.RegardingObjectId = erAccount;

                        break;
                    case "Kontaktperson":

                        EntityReference erContact = GetCrmContactByFullName(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the Appointment was not found. There is not Contact with FullName: " + value);
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nAppointment.RequiredAttendees = phoneCallTo;

                        break;
                    case "Beskrivning":
                        nAppointment.Subject = value;
                        break;
                    case "Datum":
                        //No Information
                        break;
                    case "Faktiskt avslutsdatum":

                        DateTime scheduleEnd = DateTime.MinValue;

                        if (DateTime.TryParse(value, out scheduleEnd))
                        {
                            if (scheduleEnd != DateTime.MinValue)
                                nAppointment.ScheduledEnd = scheduleEnd;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Modifierad":

                        //Not Possible Unsuported
                        //DateTime dateTime = DateTime.MinValue;

                        //if (DateTime.TryParse(value, out dateTime))
                        //{
                        //    if (dateTime != DateTime.MinValue)
                        //        nAppointment.ModifiedOn = dateTime;
                        //}
                        //else
                        //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;

                    case "Skapad":

                        DateTime createdOn = DateTime.MinValue;

                        if (DateTime.TryParse(value, out createdOn))
                        {
                            if (createdOn != DateTime.MinValue)
                                nAppointment.OverriddenCreatedOn = createdOn;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner == null)
                        {
                            string defaultUserName = "Admin"; //TODO Put on App Config
                            erOwner = GetCrmUserOrTeamByName(localContext, defaultUserName);
                        }

                        if (erOwner != null)
                            nAppointment.OwnerId = erOwner;
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Owner of the Appointment was not found.");

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nAppointment.Description = value;
                        break;

                    default:
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            return nAppointment;
        }

        public static void ImportAccountRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if(importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;
            OptionMetadataCollection colOpCompanyTrade = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_companytrade);
            OptionMetadataCollection colOpBusinessType = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                try
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
                            continue;

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
                            case "U-ID":
                                nAccount.ed_UpsalesId = value;
                                break;
                            case "Företagsnamn":
                                nAccount.Name = value;
                                break;
                            case "Organisationsnummer":
                                nAccount.edp_OrgNo = value;
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
                            case "Branch ":

                                int? optionSetCT = GetOptionSetValueByName(colOpCompanyTrade, value);

                                if (optionSetCT == null)
                                {
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                    optionSetCT = InsertGlobalOptionSetOption(localContext, "ed_companytrade", value, 1053);
                                }

                                if(optionSetCT != null)
                                    nAccount.ed_companytrade = new OptionSetValue((int)optionSetCT);

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

                                if(optionSetBT != null)
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


                            //-----------------------------------------------------OLD MAPPINGS---------------------------------------------------------

                            #region OldMappings
                            case "CreateOn - test":

                                DateTime dateTime = DateTime.MinValue;

                                if (DateTime.TryParse(value, out dateTime))
                                {
                                    if (dateTime != DateTime.MinValue)
                                        nAccount.OverriddenCreatedOn = dateTime;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                break;
                            case "Owner - ToBeDefined - test":

                                EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                                if (erOwner == null)
                                {
                                    string defaultUserName = "Admin"; //TODO Put on App Config
                                    erOwner = GetCrmUserOrTeamByName(localContext, defaultUserName);
                                }

                                if (erOwner != null)
                                    nAccount.OwnerId = erOwner;
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Owner of the Account was not found.");

                                break;
                            case "Företag: Moderbolag - test":

                                EntityReference erParentAccount = GetCrmAccountByName(localContext, value);

                                if (erParentAccount != null)
                                    nAccount.ParentAccountId = erParentAccount;
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Parent Account of the Account was not found or it was already null.");

                                break;
                            case "Företag: Anteckningar - test":
                                noteText = value;
                                break;
                            case "Företag: Postadress: Fullständig adress - test":
                                street2 = value;
                                break;
                            case "Företag: Postadress: Stad - test":
                                city = value;
                                break;
                            case "Företag: Postadress: Land - test":
                                country = value;
                                break;
                            case "Företag: Postadress: Postnummer - test":
                                postalCode = value;
                                break;
                            case "Företag: Besöksadress: Län - test":
                                nAccount.Address1_County = value;
                                break;
                            case "Företag: Faktureringsadress: Fullständig adress - test":
                                nAccount.Address2_Line1 = value;
                                break;
                            case "Företag: Faktureringsadress: Stad - test":
                                nAccount.Address2_City = value;
                                break;
                            case "Företag: Faktureringsadress: Land - test":
                                nAccount.Address2_Country = value;
                                break;
                            case "Företag: Faktureringsadress: Län - test":
                                nAccount.Address2_County = value;
                                break;
                            case "Företag: Faktureringsadress: Postnummer - test":
                                nAccount.Address2_PostalCode = value;
                                break;
                            #endregion

                            default:
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                break;
                        }
                    }

                    nAccount.ed_InfotainmentCustomer = true;
                    nAccount.StateCode = AccountState.Active;

                    crmContext.AddObject(nAccount);

                    if (noteText != string.Empty)
                    {
                        Annotation note = new Annotation();

                        note.Subject = "Note from UpSales Integration";
                        note.NoteText = noteText;

                        crmContext.AddRelatedObject(nAccount, new Relationship(rel_account_note), note);
                    }

                    if (street2 != string.Empty || city != string.Empty || country != string.Empty || postalCode != string.Empty)
                    {
                        CustomerAddress nCustomerAddress = new CustomerAddress();
                        nCustomerAddress.AddressTypeCode = customeraddress_addresstypecode.ShipTo;
                        nCustomerAddress.Line2 = street2;
                        nCustomerAddress.City = city;
                        nCustomerAddress.Country = country;
                        nCustomerAddress.PostalCode = postalCode;

                        crmContext.AddRelatedObject(nAccount, new Relationship(rel_account_address), nCustomerAddress);
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to import row " + i + ". Details: " + e.Message); 
                }
            }
        }

        public static void ImportContactsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                try
                {
                    Contact nContact = new Contact();

                    string noteText = string.Empty;

                    for (int j = 1; j <= importExcelInfo.colCount; j++)
                    {
                        if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                            continue;

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
                            case " Upsales-ID":
                                nContact.ed_UpsalesId = value;
                                break;
                            case " Företag":

                                EntityReference erParent = GetCrmAccountByName(localContext, value);

                                if (erParent != null)
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Parent Customer " + value + " of the Contact was updated.");
                                    nContact.ParentCustomerId = erParent;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Parent Customer " + value + " of the Contact was not found.");

                                break;
                            case "Förnamn":
                                nContact.FirstName = value;
                                break;
                            case " Efternamn":
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

                            //-----------------------------------------------------OLD MAPPINGS---------------------------------------------------------

                            #region OldMappings
                            case "CreateOn - test":

                                DateTime dateTime = DateTime.MinValue;

                                if (DateTime.TryParse(value, out dateTime))
                                {
                                    if (dateTime != DateTime.MinValue)
                                        nContact.OverriddenCreatedOn = dateTime;
                                }

                                break;
                            case "Anteckningar - test":
                                noteText = value;
                                break;

                            #endregion

                            default:
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                break;
                        }
                    }

                    nContact.ed_InfotainmentContact = true;
                    nContact.StateCode = ContactState.Active;
                    nContact.ed_InformationSource = ed_informationsource.AdmAndraKund;
                    nContact.AccountRoleCode = contact_accountrolecode.Ansvarigforinfotainment;

                    //Prevent Plugin Errors
                    if (nContact.FirstName == null)
                        nContact.FirstName = " ";

                    if (nContact.LastName == null)
                        nContact.LastName = " ";

                    crmContext.AddObject(nContact);

                    if(nContact.ParentCustomerId != null && nContact.ParentCustomerId.LogicalName == Account.EntityLogicalName)
                    {
                        EntityReference erAccount = new EntityReference(Account.EntityLogicalName, nContact.ParentCustomerId.Id);
                        Account account = XrmRetrieveHelper.Retrieve<Account>(localContext, erAccount, new ColumnSet(Account.Fields.PrimaryContactId));

                        if (account != null && account.PrimaryContactId == null)
                        {
                            Account uAccount = new Account();
                            uAccount.Id = account.Id;
                            uAccount.Description = nContact.ed_UpsalesId;

                            lAccountsUpdate.Add(uAccount);

                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Account Object Added to the List.");
                        }
                        else
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Account " + nContact.ParentCustomerId.Id + " already has a Primary Contact.");
                    }
                    
                    if (noteText != string.Empty)
                    {
                        Annotation note = new Annotation();

                        note.Subject = "Note from UpSales Integration";
                        note.NoteText = noteText;

                        crmContext.AddRelatedObject(nContact, new Relationship(rel_contact_note), note);
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to import row " + i + ". Details: " + e.Message);
                }
            }
        }

        public static void ImportActivitiesRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            string activityTypeNameColumn = "Aktivitetstyp";
            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 2; i < importExcelInfo.rowCount; i++)
            {
                try
                {
                    ExcelColumn activityTypeColumn = GetSelectedExcelColumnByName(importExcelInfo.lColumns, activityTypeNameColumn);

                    if(activityTypeColumn == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null.");
                        continue;
                    }

                    int j = activityTypeColumn.index;

                    if (excelRange.Cells[i, j] == null || excelRange.Cells[i, j].Value2 == null || string.IsNullOrEmpty(excelRange.Cells[i, j].Value2.ToString()))
                        continue;

                    string activityType = excelRange.Cells[i, j].Value2.ToString();

                    switch (activityType)
                    {
                        case "Telefonsamtal":

                            PhoneCall nPhoneCall = GetPhoneCall(localContext, excelRange, importExcelInfo, i);
                            crmContext.AddObject(nPhoneCall);

                            break;
                        case "E-post":

                            Email nEmail = GetEmail(localContext, excelRange, importExcelInfo, i);
                            crmContext.AddObject(nEmail);

                            break;
                        case "Övrigt":

                            Appointment nAppointment = GetAppointment(localContext, excelRange, importExcelInfo, i);
                            crmContext.AddObject(nAppointment);

                            break;
                        default:
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Activity Type " + activityType + " is not implemented yet.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to import row " + i + ". Details: " + e.Message);
                }
            }
        }

        public static void ImportLeadsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

        }

        public static void ImportOpportunitiesRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

        }

        public static void ImportWonOpportunitiesRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

        }

        public static void ImportHistoricalDataRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

        }

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
            CrmContext crmContext = new CrmContext(_service);
            SaveChangesOptions optionsChanges = SaveChangesOptions.ContinueOnError;

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.ExcelRelativePath);
            string fileName = "Spann 1 - unika 2020-05-27.xlsx";

            #region Import Accounts

            try
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");

                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Accounts", fileName);
                ImportAccountRecords(localContext, crmContext, importExcelInfo);
                SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                LogCrmContextMultipleResponses(responses);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Publishing Accounts OptionSets--------------");

                PublishGlobalOptionSet(localContext, "ed_companytrade");
                PublishGlobalOptionSet(localContext, "ed_businesstype");

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished Publishing Accounts OptionSets--------------");
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
                throw;
            }

            #endregion

            fileName = "Upsales data clean 2020-05-25_kontakter.xlsx";

            #region Import Contacts

            try
            {
                crmContext.ClearChanges();
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");

                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Contacts", fileName);
                ImportContactsRecords(localContext, crmContext, importExcelInfo);
                SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                LogCrmContextMultipleResponses(responses);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Contact Entity--------------");

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Update Accounts Primary Contact--------------------");

                crmContext.ClearChanges();
                UpdateAccountsWithPrimaryContactId(localContext, crmContext);
                SaveChangesResultCollection responsesAccounts = crmContext.SaveChanges(optionsChanges);
                LogCrmContextMultipleResponses(responsesAccounts);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished Updating Accounts Primary Contact--------------------");
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Contact Records. Details: " + e.Message);
                throw;
            }

            #endregion

            fileName = "Activities företag 2020-04-30.xlsx";

            //#region Import Activities

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Activities", fileName);
            //    ImportActivitiesRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Activities Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.Error("Error Importing Activities Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Leads företag 2020-04-30.xlsx";

            //#region Import Leads

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Leads Entity--------------");
            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
            //    ImportLeadsRecords(localContext, crmContext, importExcelInfo);
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Leads Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.Error("Error Importing Leads Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Opportunities företag 2020-04-30.xlsx";

            //#region Import Opportunities

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Opportunities Entity--------------");
            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
            //    ImportOpportunitiesRecords(localContext, crmContext, importExcelInfo);
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Opportunities Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.Error("Error Importing Opportunities Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Won Opportunities företag 2020-04-30.xlsx";

            //#region Import Won Opportunities

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Won Opportunities Entity--------------");
            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
            //    ImportWonOpportunitiesRecords(localContext, crmContext, importExcelInfo);
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Won Opportunities Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.Error("Error Importing Won Opportunities Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Historical Data företag 2020-04-30.xlsx";

            //#region Import Historical Data

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Historical Data Entity--------------");
            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath, fileName);
            //    ImportHistoricalDataRecords(localContext, crmContext, importExcelInfo);
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Historical Data Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.Error("Error Importing Historical Data Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }
}
