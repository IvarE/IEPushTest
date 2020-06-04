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
        public static string rel_order_note = "SalesOrder_Annotation";

        public static string cgi_account_contact = "cgi_account_contact";

        public static IOrganizationService _service = null;
        static ExcelApp.Application excelApp = new ExcelApp.Application();
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            return value.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("'", "").Replace(" ", "").Replace("–", "");
        }

        public static string getSubString(string value, int max)
        {
            return value.Length > max ? value.Substring(0, max - 1) : value;
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

        public static EntityReference GetCrmCurrencyByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterCurrencies = new FilterExpression();
            filterCurrencies.Conditions.Add(new ConditionExpression(TransactionCurrency.Fields.CurrencyName, ConditionOperator.Equal, name));

            List<TransactionCurrency> lCurrencies = XrmRetrieveHelper.RetrieveMultiple<TransactionCurrency>(localContext, new ColumnSet(TransactionCurrency.Fields.TransactionCurrencyId), filterCurrencies).ToList();

            if (lCurrencies.Count == 1)
                return lCurrencies.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Currencies or More than One Currency found with Currency Name: " + name + ".");
            return null;
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

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Users/Teams or More than One User/Team found with Name: " + name + ".");
            return null;
        }

        public static EntityReference GetCrmAccountByOrganizationNumber(Plugin.LocalPluginContext localContext, string orgNumber)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.cgi_organizational_number, ConditionOperator.Equal, orgNumber));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Accounts or More than One Account found with field 'cgi_organizational_number' = " + orgNumber + ".");
            return null;
        }

        public static EntityReference GetCrmAccountByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Accounts or More than One Account found with Upsales Id: " + upsalesId + ".");
            return null;
        }

        public static EntityReference GetCrmAccountByName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterAccounts = new FilterExpression();
            filterAccounts.Conditions.Add(new ConditionExpression(Account.Fields.Name, ConditionOperator.Equal, name));

            List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, new ColumnSet(Account.Fields.AccountId), filterAccounts).ToList();

            if (lAccounts.Count == 1)
                return lAccounts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Accounts or More than One Account found with Name: " + name + ".");
            return null;
        }

        public static EntityReference GetCrmContactByFullName(Plugin.LocalPluginContext localContext, string name)
        {
            FilterExpression filterContacts = new FilterExpression();
            filterContacts.Conditions.Add(new ConditionExpression(Contact.Fields.FullName, ConditionOperator.Equal, name));

            List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, new ColumnSet(Contact.Fields.ContactId), filterContacts).ToList();

            if (lContacts.Count == 1)
                return lContacts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Contacts or More than One Contact found with Name: " + name + ".");
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
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Appointment with Id: " + salesOrder.Id + " was updated.");

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

                                    int newStatus = (int)salesorder_statuscode.Complete;

                                    Entity orderClose = new Entity("orderclose");
                                    orderClose["salesorderid"] = new EntityReference(SalesOrder.EntityLogicalName, id);
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
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"ERROR - Appointment with Name: " + salesOrder.Name + " was not created. Details: " + response.Error.Message);

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

        public static PhoneCall GetPhoneCall(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
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
                    case "U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the PhoneCall was updated.");
                            nPhoneCall.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The PhoneCall on line " + i + " will be ignored.");

                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":

                        string cleanValue = getCleanToValue(value);
                        EntityReference erContact = GetCrmContactByFullName(localContext, cleanValue);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the PhoneCall was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nPhoneCall.To = phoneCallTo;

                        break;
                    case "Beskrivning":
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
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

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
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner " + value + " of the PhoneCall was not found. The default user was updated.");
                            nPhoneCall.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nPhoneCall.OwnerId = erOwner;
                        else
                        {
                            nPhoneCall.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nPhoneCall.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nPhoneCall.RegardingObjectId == null)
                return null;

            if(erDefaultUser != null)
            {
                ActivityParty fromParty = new ActivityParty();
                fromParty.PartyId = erDefaultUser;

                List<ActivityParty> phoneCallFrom = new List<ActivityParty>();
                phoneCallFrom.Add(fromParty);

                nPhoneCall.From = phoneCallFrom;
            }

            nPhoneCall.StateCode = PhoneCallState.Completed;

            return nPhoneCall;
        }

        public static Email GetEmail(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
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
                    case "U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the Email was updated.");
                            nEmail.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Email on line " + i + " will be ignored.");

                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":

                        EntityReference erContact = GetCrmContactByFullName(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the Email was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nEmail.To = phoneCallTo;

                        break;
                    case "Beskrivning":
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
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

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

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner " + value + " of the Email was not found. The default user was updated.");
                            nEmail.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nEmail.OwnerId = erOwner;
                        else
                        {
                            nEmail.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nEmail.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nEmail.RegardingObjectId == null)
                return null;

            if (erDefaultUser != null)
            {
                ActivityParty fromParty = new ActivityParty();
                fromParty.PartyId = erDefaultUser;

                List<ActivityParty> emailFrom = new List<ActivityParty>();
                emailFrom.Add(fromParty);

                nEmail.From = emailFrom;
            }

            nEmail.StateCode = EmailState.Completed;

            return nEmail;
        }

        public static Appointment GetAppointment(Plugin.LocalPluginContext localContext, ExcelApp.Range excelRange, ImportExcelInfo importExcelInfo, int i, int maxSubject, int maxDescription, EntityReference erDefaultUser)
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
                    case "U-ID":

                        EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                        if (erAccount != null)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the Appointment was updated.");
                            nAppointment.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Appointment on line " + i + " will be ignored.");

                        break;
                    case "Företag":
                        //Already Handleded
                        break;
                    case "Kontaktperson":

                        EntityReference erContact = GetCrmContactByFullName(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the Appointment was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> phoneCallTo = new List<ActivityParty>();
                        phoneCallTo.Add(toParty);

                        nAppointment.RequiredAttendees = phoneCallTo;

                        break;
                    case "Beskrivning":
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
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

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
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                        break;
                    case "Användare":

                        if (value == "Alexander Zaragoza")
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner " + value + " of the PhoneCall was not found. The default user was updated.");
                            nAppointment.OwnerId = erDefaultUser;
                            break;
                        }

                        EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                        if (erOwner != null)
                            nAppointment.OwnerId = erOwner;
                        else
                        {
                            nAppointment.OwnerId = erDefaultUser;
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner was updated with the default user.");
                        }

                        break;
                    case "Aktivitetstyp":
                        //Already Handleded
                        break;
                    case "Anteckningar":
                        nAppointment.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nAppointment.RegardingObjectId == null)
                return null;

            nAppointment.StateCode = AppointmentState.Completed;

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
                                //nAccount.edp_OrgNo = value;
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
                                _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                break;
                        }
                    }

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

        public static void ImportSubAccountsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
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
                    Account nSubAccount = new Account();

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
                                nSubAccount.ed_UpsalesId = value;
                                break;
                            case "Företagsnamn":
                                nSubAccount.Name = value;
                                break;
                            case "Organisationsnummer":

                                EntityReference erParentAccount = GetCrmAccountByOrganizationNumber(localContext, value);

                                if (erParentAccount != null)
                                    nSubAccount.ParentAccountId = erParentAccount;
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Parent Account with Organization Number: " + value + " was not found.");

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

        public static void ImportContactsRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            List<Account> lAddedAccounts = new List<Account>();
            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                try
                {
                    Account eParentAccount = null;
                    Account ePParentAccount = null;

                    Contact nContact = new Contact();

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

                                EntityReference erParentAccount = GetCrmAccountByUpsalesId(localContext, value);

                                if (erParentAccount != null)
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Parent Customer with Upsales Id: " + value + " of the Contact was updated.");
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
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " was not found. The Contact on line " + i + " will be ignored.");

                                break;
                            case " Företag":
                                //Already Handleded
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

                            #endregion

                            default:
                                _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                break;
                        }
                    }

                    if (nContact.ParentCustomerId == null)
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

                    if(eParentAccount != null)
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

                            _log.InfoFormat(CultureInfo.InvariantCulture, $"1Level: A Request has been created to relate the Account " + eParentAccount.Id + " to Contact " + nContact.FirstName + " " + nContact.LastName + " on the CGI Grid.");
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"CGI Grid Error Exception. Details: " + e.Message);
                        }
                    }

                    if(ePParentAccount != null)
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

                            _log.InfoFormat(CultureInfo.InvariantCulture, $"2Level: A Request has been created to relate the Account " + ePParentAccount.Id + " to Contact " + nContact.FirstName + " " + nContact.LastName + " on the CGI Grid.");
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"CGI Grid Error Exception. Details: " + e.Message);
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
                                    crmContext.Attach(eParentAccount);

                                if (!crmContext.IsAttached(nContact))
                                    crmContext.Attach(nContact);

                                crmContext.AddLink(eParentAccount, new Relationship(rel_contact_account), nContact);

                                _log.InfoFormat(CultureInfo.InvariantCulture, $"A Request has been created for the Account " + eParentAccount.Id + " to be updated with Contact " + nContact.FirstName + " " + nContact.LastName + " as it's Primary Contact.");
                            }
                            else
                                _log.InfoFormat(CultureInfo.InvariantCulture, $"The Account " + account.Id + " already has a Primary Contact.");
                        }
                        catch (Exception e)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"Primary Contact Error Exception. Details: " + e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Contacts Exception. Details: " + e.Message);
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

            string defaultUserName = "Kristina Paulsson";
            EntityReference erDefaultUser = GetCrmUserOrTeamByName(localContext, defaultUserName);

            string activityTypeNameColumn = "Aktivitetstyp";
            int maxSubject = 200;
            int maxDescription = 2000;
            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 400; i < importExcelInfo.rowCount; i++)
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

                            PhoneCall nPhoneCall = GetPhoneCall(localContext, excelRange, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

                            if(nPhoneCall != null)
                                crmContext.AddObject(nPhoneCall);

                            break;
                        case "E-post":

                            Email nEmail = GetEmail(localContext, excelRange, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

                            if (nEmail != null)
                                crmContext.AddObject(nEmail);

                            break;
                        case "Övrigt":

                            Appointment nAppointment = GetAppointment(localContext, excelRange, importExcelInfo, i, maxSubject, maxDescription, erDefaultUser);

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

        public static void ImportOrdersRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.excelRange == null || importExcelInfo.lColumns == null || importExcelInfo.rowCount == null || importExcelInfo.colCount == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            string defaultUserName = "Kristina Paulsson";
            EntityReference erDefaultUser = GetCrmUserOrTeamByName(localContext, defaultUserName);

            string currencyName = "Svensk krona";
            EntityReference erCurrency = GetCrmCurrencyByName(localContext, currencyName);

            ExcelApp.Range excelRange = importExcelInfo.excelRange;

            for (int i = 2; i <= importExcelInfo.rowCount; i++)
            {
                if (i == 50)
                    return;

                try
                {
                    SalesOrder nOrder = new SalesOrder();

                    string orderName = "{0} - {1}";
                    string orderId = string.Empty;
                    string orderNameValue = string.Empty;
                    string noteText = string.Empty;
                    string noteTextProduct = string.Empty;
                    string startProductDate = string.Empty;
                    string endProductDate = string.Empty;

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
                            case "Företag: Upsales-ID":

                                EntityReference erAccount = GetCrmAccountByUpsalesId(localContext, value);

                                if (erAccount != null)
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " of the Order was updated.");
                                    nOrder.CustomerId = erAccount;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " was not found. The Order on line " + i + " will be ignored.");

                                break;
                            case "Order: Företag":
                                //Already Handled
                                break;
                            case "Order: Kontaktperson":

                                EntityReference erContact = GetCrmContactByFullName(localContext, value);

                                if (erContact != null)
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Contact with Fullname : " + value + " of the Order was updated.");
                                    nOrder.ed_contact = erContact;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Contact with Fullname : " + value + " was not found.");

                                break;
                            case "Order: Orderns ID":
                                orderId = value;
                                break;
                            case "Order: Beskrivning":
                                orderNameValue = value;
                                break;
                            case "Order: Datum":

                                DateTime submitDate = DateTime.MinValue;

                                if (DateTime.TryParse(value, out submitDate))
                                {
                                    if (submitDate != DateTime.MinValue)
                                        nOrder.SubmitDate = submitDate;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                break;
                            case "Order: Värde":

                                decimal dtotalLineAmount = decimal.MinValue;

                                if (decimal.TryParse(value, out dtotalLineAmount))
                                {
                                    if (dtotalLineAmount != decimal.MinValue)
                                        nOrder.TotalLineItemAmount = new Money(dtotalLineAmount);
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a decimal value.");

                                break;
                            case "Order: Användare":

                                if (value == "Alexander Zaragoza")
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner " + value + " of the PhoneCall was not found. The default user was updated.");
                                    nOrder.OwnerId = erDefaultUser;
                                    break;
                                }

                                EntityReference erOwner = GetCrmUserOrTeamByName(localContext, value);

                                if (erOwner != null)
                                    nOrder.OwnerId = erOwner;
                                else
                                {
                                    nOrder.OwnerId = erDefaultUser;
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner was updated with the default user.");
                                }

                                break;
                            case "Order: Anteckningar":
                                noteText = value;
                                break;
                            case "Order: Produkt":
                                noteTextProduct = value;
                                break;
                            case "Order: Fas":
                                //Already Handled
                                break;
                            case "Order: Skapad":

                                DateTime createdOn = DateTime.MinValue;

                                if (DateTime.TryParse(value, out createdOn))
                                {
                                    if (createdOn != DateTime.MinValue)
                                        nOrder.OverriddenCreatedOn = createdOn;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                break;
                            case "Kampanj start":

                                DateTime campaignStart = DateTime.MinValue;

                                if (DateTime.TryParse(value, out campaignStart))
                                {
                                    if (campaignStart != DateTime.MinValue)
                                        nOrder.ed_campaigndatestart = campaignStart;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                break;
                            case "Kampanj slut":

                                DateTime campaignEnd = DateTime.MinValue;

                                if (DateTime.TryParse(value, out campaignEnd))
                                {
                                    if (campaignEnd != DateTime.MinValue)
                                        nOrder.ed_campaigndateend = campaignEnd;
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                break;
                            case "Startdatum":
                                startProductDate = value;
                                break;
                            case "Slutdatum":
                                endProductDate = value;
                                break;

                            default:
                                _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                break;
                        }
                    }

                    if (nOrder.CustomerId == null)
                        continue;
                    
                    nOrder.TransactionCurrencyId = erCurrency;

                    orderName = string.Format(orderName, orderId, orderNameValue);
                    nOrder.Name = orderName.Replace("{0}", "").Replace("{1}", "");

                    crmContext.AddObject(nOrder);

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
                        note.NoteText = noteText + "\n\nProduct Start Date: " + startProductDate + "\nProduct End Date: " + endProductDate;

                        crmContext.AddRelatedObject(nOrder, new Relationship(rel_order_note), note);
                    }
                    
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Orders Exception. Details: " + e.Message);
                }
            }
        }

        public static void ImportHistoricalDataRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

        }

        static void Main(string[] args)
        {
            //Test Connection
            ConnectToMSCRM("D1\\CRMAdmin", "uSEme2!nstal1", "https://sekunduat.skanetrafiken.se/DKCRM/XRMServices/2011/Organization.svc");

            if (_service == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The CRM Service is null.");
                Console.WriteLine("The CRM Service is null.");
                return;
            }

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());
            CrmContext crmContext = new CrmContext(_service);
            SaveChangesOptions optionsChanges = SaveChangesOptions.ContinueOnError;

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.ExcelRelativePath);
            string fileName = "Spann 1 - unika 2020-05-27.xlsx";

            //#region Import Accounts Spann 1

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Accounts", fileName);
            //    ImportAccountRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(localContext, responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Spann 5 - nya datakällor 2020-05-25.xlsx";

            //#region Import Accounts Spann 5

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Accounts", fileName);
            //    ImportAccountRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(localContext, responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Spann 3 - subaccounts_2020-05-28.xlsx";

            //#region Import Accounts Spann 3 SubAccounts

            //try
            //{
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Account Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Accounts", fileName);
            //    ImportSubAccountsRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(localContext, responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Account Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Account Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Upsales data clean 2020-05-25_kontakter.xlsx";

            //#region Import Contacts

            //try
            //{
            //    crmContext.ClearChanges();
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Contacts", fileName);
            //    ImportContactsRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(localContext, responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Contact Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Contact Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //fileName = "Upsales data clean 2020-05-26_aktiviteter.xlsx";

            //#region Import Activities

            //try
            //{
            //    crmContext.ClearChanges();
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Activities", fileName);
            //    ImportActivitiesRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(localContext, responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Activities Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Activities Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            fileName = "Upsales data clean 2020-06-01_order.xlsx";

            #region Import Orders

            try
            {
                crmContext.ClearChanges();
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Orders Entity--------------");

                ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\Orders", fileName);
                ImportOrdersRecords(localContext, crmContext, importExcelInfo);
                SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                LogCrmContextMultipleResponses(localContext, responses);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Orders Entity--------------");
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Order Records. Details: " + e.Message);
                throw;
            }

            #endregion

            //fileName = "Historical Data företag 2020-04-30.xlsx";

            //#region Import Historical Data

            //try
            //{
            //    crmContext.ClearChanges();
            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Historical Data Entity--------------");

            //    ImportExcelInfo importExcelInfo = HandleExcelInformation(relativeExcelPath + "\\HistoricalData", fileName);
            //    ImportHistoricalDataRecords(localContext, crmContext, importExcelInfo);
            //    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
            //    LogCrmContextMultipleResponses(responses);

            //    _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Historical Data Entity--------------");
            //}
            //catch (Exception e)
            //{
            //    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Historical Data Records. Details: " + e.Message);
            //    throw;
            //}

            //#endregion

            //QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
            //queryContacts.NoLock = true;
            //queryContacts.Criteria.AddCondition(Contact.Fields.ed_UpsalesId, ConditionOperator.NotNull);

            //List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);

            //foreach (Contact contact in lContacts)
            //{
            //    Contact nContact = new Contact();
            //    nContact.Id = contact.Id;
            //    nContact.StateCode = ContactState.Inactive;
            //    nContact.StatusCode = contact_statuscode.Inactive;

            //    if(!crmContext.IsAttached(nContact))
            //        crmContext.Attach(nContact);

            //    crmContext.UpdateObject(nContact);
            //}

            //crmContext.SaveChanges(SaveChangesOptions.ContinueOnError);

            //crmContext.ClearChanges();
            //foreach (Contact contact in lContacts)
            //{
            //    if (!crmContext.IsAttached(contact))
            //        crmContext.Attach(contact);

            //    crmContext.DeleteObject(contact);
            //}

            //crmContext.SaveChanges(SaveChangesOptions.ContinueOnError);

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }
    }
}
