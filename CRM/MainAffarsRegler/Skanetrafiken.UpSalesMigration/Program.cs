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
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Threading;

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

        public static string cleanMobileTelefon(string value)
        {
            return value.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("'", "").Replace(" ", "").Replace("–", "").Replace("/","").Replace(":","").Replace("Mobil", "");
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

        public static ImportExcelInfo HandleExcelInformationStreamReader(string relativeExcelPath, string fileName)
        {
            try
            {
                int i = 0;
                List<ExcelColumn> lColumns = new List<ExcelColumn>();
                List<List<ExcelLineData>> lData = new List<List<ExcelLineData>>();

                using (var reader = new StreamReader(relativeExcelPath + "\\" + fileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        List<string> values = line.Split('^').ToList();
                        if (i == 0)
                        {
                            for(int j = 0; j < values.Count; j++)
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

        public static EntityReference GetCrmContactByUpsalesId(Plugin.LocalPluginContext localContext, string upsalesId)
        {
            FilterExpression filterContacts = new FilterExpression();
            filterContacts.Conditions.Add(new ConditionExpression(Contact.Fields.ed_UpsalesId, ConditionOperator.Equal, upsalesId));

            List<Contact> lContacts = XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, new ColumnSet(Contact.Fields.ContactId), filterContacts).ToList();

            if (lContacts.Count == 1)
                return lContacts.FirstOrDefault().ToEntityReference();

            _log.InfoFormat(CultureInfo.InvariantCulture, $"No Contacts or More than One Contact found with Upsales Id: " + upsalesId + ".");
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
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the PhoneCall was updated.");
                            nPhoneCall.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The PhoneCall on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

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
                    case "Activity U-ID":
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
                    case "Activity: Anteckningar":
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
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the Email was updated.");
                            nEmail.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Email on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the Email was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> EmailTo = new List<ActivityParty>();
                        EmailTo.Add(toParty);

                        nEmail.To = EmailTo;

                        break;
                    case "Activity U-ID":
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
                    case "Activity: Anteckningar":
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
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " of the Appointment was updated.");
                            nAppointment.RegardingObjectId = erAccount;
                        }
                        else
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The RegardingObjectId with Upsales Id: " + value + " was not found. The Appointment on line " + (i + 1) + " will be ignored.");

                        break;
                    case "Contact U-ID":

                        EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                        if (erContact == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The To of the Email was not found.");
                            break;
                        }

                        ActivityParty toParty = new ActivityParty();
                        toParty.PartyId = erContact;

                        List<ActivityParty> AppointmentTo = new List<ActivityParty>();
                        AppointmentTo.Add(toParty);

                        nAppointment.RequiredAttendees = AppointmentTo;

                        break;
                    case "Activity U-ID":
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
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Owner " + value + " of the Appointment was not found. The default user was updated.");
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
                    case "Activity: Anteckningar":
                        nAppointment.Description = getSubString(value, maxDescription);
                        break;

                    default:
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                        break;
                }
            }

            if (nAppointment.RegardingObjectId == null)
                return null;

            return nAppointment;
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
                    OptionMetadataCollection colOpCompanyTrade = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_companytrade);
                    OptionMetadataCollection colOpBusinessType = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        Account nAccount = new Account();
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if(line.Count != importExcelInfo.lColumns.Count)
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
                                case "Branch ":

                                    int? optionSetCT = GetOptionSetValueByName(colOpCompanyTrade, value);

                                    if (optionSetCT == null)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        optionSetCT = InsertGlobalOptionSetOption(localContext, "ed_companytrade", value, 1053);
                                    }

                                    if (optionSetCT != null)
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
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " was not found. The Contact on line " + (i + 1) + " will be ignored.");

                                    break;
                                case "Contact U-ID":
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

                                _log.InfoFormat(CultureInfo.InvariantCulture, $"1Level: A Request has been created to relate the Account " + eParentAccount.Id + " to Contact " + nContact.FirstName + " " + nContact.LastName + " on the CGI Grid.");
                            }
                            catch (Exception e)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"CGI Grid Error Exception. Details: " + e.Message);
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
                                    {
                                        crmContext.Attach(eParentAccount);
                                        lAddedAccounts.Add(eParentAccount);
                                    }

                                    if (!crmContext.IsAttached(nContact))
                                        crmContext.Attach(nContact);

                                    if(accountAdded.Count == 0)
                                        crmContext.AddLink(eParentAccount, new Relationship(rel_contact_account), nContact);
                                    else
                                        crmContext.AddLink(accountAdded.FirstOrDefault(), new Relationship(rel_contact_account), nContact);

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
                        ExcelColumn activityTypeColumn = GetSelectedExcelColumnByName(importExcelInfo.lColumns, activityTypeNameColumn);

                        if (activityTypeColumn == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Column is null. Contact your administrator.");
                            continue;
                        }

                        int j = activityTypeColumn.index;
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        ExcelLineData selectedData = line[j];

                        if (selectedData == null)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Selected Data is null. Contact your administrator.");
                            continue;
                        }

                        string activityType = selectedData.value;

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

                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        for (int j = 0; j < importExcelInfo.lColumns.Count; j++)
                        {
                            ExcelLineData selectedData = line[j];

                            if(selectedData == null)
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
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " of the Order was updated.");
                                        nOrder.CustomerId = erAccount;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Customer with Upsales Id: " + value + " was not found. The Order on line " + (i + 1) + " will be ignored.");

                                    break;
                                case "Contact: U-ID":

                                    EntityReference erContact = GetCrmContactByUpsalesId(localContext, value);

                                    if (erContact != null)
                                    {
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"The Contact with Upsales Id: " + value + " of the Order was updated.");
                                        nOrder.ed_contact = erContact;
                                    }
                                    else
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Contact with Upsales Id: " + value + " was not found.");

                                    break;
                                case "Orderns U-ID":
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
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Couldn't parse " + value + " to a DateTime value.");

                                    break;
                                case "Värde":

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
                                case "Produkt (Colibri)":
                                    productColibri = value;
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }

                        if (nOrder.CustomerId == null)
                            continue;

                        nOrder.TransactionCurrencyId = erCurrency;

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
                            note.NoteText = noteText + "\n\nProduct Start Date: " + startProductDate + "\nProduct End Date: " + endProductDate + "\n\nProduct (Colibri): " + productColibri;

                            crmContext.AddRelatedObject(nOrder, new Relationship(rel_order_note), note);
                        }

                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Orders Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void ImportHistoricalDataRecords(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {

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

                    if(base64 == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Base64 for the file: " + file.fileName + " is null.");
                        continue;
                    }

                    string orderId = GetOrderId(fileName);

                    if(orderId == null)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Upsales Id for the Order is null. File: " + fileName + " will be ignored.");
                        continue;
                    }

                    QueryExpression queryOrders = new QueryExpression(SalesOrder.EntityLogicalName);
                    queryOrders.NoLock = true;
                    queryOrders.ColumnSet.AddColumns(SalesOrder.Fields.SalesOrderId);
                    queryOrders.Criteria.AddCondition(SalesOrder.Fields.ed_UpsalesId, ConditionOperator.Equal, orderId);

                    List<SalesOrder> lOrders = XrmRetrieveHelper.RetrieveMultiple<SalesOrder>(localContext, queryOrders);

                    if(lOrders.Count == 1)
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
                    else if(lOrders.Count == 0)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Orders found with  Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                    else if(lOrders.Count > 1)
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Orders found with  Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
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
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"No Orders found with  Upsales Id " + orderId + ". File: " + fileName + " will be ignored.");
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, string.Format("The file was downloaded: {0}", e.ToString()));
                }
            }
        }

        public static bool MainMenu(Plugin.LocalPluginContext localContext, CrmContext crmContext, SaveChangesOptions optionsChanges, string relativeExcelPath)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Import Spann 1 Accounts");
            Console.WriteLine("2) Import Spann 5 Accounts");
            Console.WriteLine("3) Import Spann 3 Accounts");
            Console.WriteLine("4) Import Contacts");
            Console.WriteLine("5) Import Activities");
            Console.WriteLine("6) Import Orders");
            Console.WriteLine("7) Import Historical Data");
            Console.WriteLine("8) Fix Duplicate OptionSets on Account/Branch");
            Console.WriteLine("9) Import PDF Orders");
            Console.WriteLine("10) Import PDF Agreements");
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
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportAccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportAccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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

                    #region Import Accounts Spann 3 SubAccounts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload Bucket 3 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket3SubAccounts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportSubAccountsRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Sub Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                case "4":

                    #region Import Contacts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Contact Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Contacts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportContactsRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Contacts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                case "5":

                    #region Import Activities

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Activities Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Activities);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportActivitiesRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Activities to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                case "6":

                    #region Import Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Orders Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Orders);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            ImportOrdersRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                case "7":

                    #region Import Historical Data

                    throw new NotImplementedException();

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload the Historical Data Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.HistoricalData);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);
                        ImportHistoricalDataRecords(localContext, crmContext, importExcelInfo);

                        Console.WriteLine("Sending Batch of Accounts to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        LogCrmContextMultipleResponses(localContext, responses);

                        Console.WriteLine("Batch Sent. Please check logs.");

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished to Upload the Historical Data Entity--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Importing Historical Data Records. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;
                case "8":

                    #region Fix DuplicatedOptionSets On Account/Branch

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting Logic to Eliminate Duplicate OptionSets--------------");

                        OptionMetadataCollection colOpCompanyTrade = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_companytrade);

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Count: " + colOpCompanyTrade.Count);

                        List<OptionMetadata> lOpDelete = new List<OptionMetadata>();

                        List<OptionMetadata> lFinal = colOpCompanyTrade.OrderBy(x => x.Label.UserLocalizedLabel.Label).ToList();

                        foreach (OptionMetadata item in lFinal)
                        {
                            Console.WriteLine(item.Value + " ---- " + item.Label.UserLocalizedLabel.Label);
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"" + item.Value + " ---- " + item.Label.UserLocalizedLabel.Label);
                        }

                        foreach (OptionMetadata item in lFinal)
                        {
                            int uniqueOption = int.MinValue;
                            string optionLabel = item.Label.UserLocalizedLabel.Label;

                            if(!string.IsNullOrEmpty(optionLabel))
                            {
                                if (lOpDelete.Exists(x => x.Label.UserLocalizedLabel.Label == optionLabel))
                                {
                                    //skip this one
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Option with name: " + optionLabel + " has already been handled.");
                                    continue;
                                }

                                List<OptionMetadata> lOptionsName = colOpCompanyTrade.Where(x => x.Label.UserLocalizedLabel.Label == optionLabel).OrderBy(x => x.Value).ToList();

                                if(lOptionsName.Count == 1)
                                {
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Only One Option with name: " + optionLabel);
                                    continue;
                                }

                                foreach (OptionMetadata itemDuplicate in lOptionsName)
                                {
                                    if (itemDuplicate == lOptionsName.FirstOrDefault())
                                    {
                                        //option with lesser value to keep
                                        uniqueOption = (int)itemDuplicate.Value;
                                    }
                                    else
                                    {
                                        //options to delete
                                        if(!lOpDelete.Exists(x => x.Value == itemDuplicate.Value))
                                            lOpDelete.Add(itemDuplicate);
                                    }
                                }

                                QueryExpression queryAccounts = new QueryExpression(Account.EntityLogicalName);
                                queryAccounts.NoLock = true;
                                queryAccounts.ColumnSet.AddColumns(Account.Fields.ed_companytrade, Account.Fields.Name);
                                queryAccounts.Criteria.AddCondition(Account.Fields.ed_UpsalesId, ConditionOperator.NotNull);

                                FilterExpression filter = new FilterExpression();
                                queryAccounts.Criteria.AddFilter(filter);

                                filter.FilterOperator = LogicalOperator.Or;

                                foreach (OptionMetadata metadata in lOptionsName)
                                {
                                    filter.AddCondition(Account.Fields.ed_companytrade, ConditionOperator.Equal, metadata.Value);
                                }

                                List<Account> lAccounts = XrmRetrieveHelper.RetrieveMultiple<Account>(localContext, queryAccounts);

                                _log.InfoFormat(CultureInfo.InvariantCulture, $"Account Count: " + lAccounts.Count);

                                if (uniqueOption != int.MinValue)
                                {
                                    foreach (Account account in lAccounts)
                                    {
                                        Account uAccount = new Account();
                                        uAccount.Id = account.Id;
                                        uAccount.ed_companytrade = new OptionSetValue(uniqueOption);

                                        XrmHelper.Update(localContext, uAccount);
                                        _log.InfoFormat(CultureInfo.InvariantCulture, $"A request has been sent to update the Account: " + account.Id + " with company trade: " + uniqueOption);
                                    }
                                }
                                else
                                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Unique Option is equal to the Min Value. Contact your administrator.");
                            }
                        }

                        foreach (OptionMetadata opDelete in lOpDelete)
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"Option to Delete: " + opDelete.Value + " and Name: " + opDelete.Label.UserLocalizedLabel.Label);

                            DeleteOptionValueRequest request = new DeleteOptionValueRequest
                            {
                                OptionSetName = "ed_companytrade",
                                Value = (int)opDelete.Value
                            };

                            try
                            {
                                localContext.OrganizationService.Execute(request);
                            }
                            catch (Exception)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to delete option from 'ed_companytrade': " + opDelete.Value);
                            }

                        }

                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Finished Logic to Eliminate Duplicate OptionSets--------------");
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error Fixing Duplicated OptionSets. Details: " + e.Message);
                        throw;
                    }

                    #endregion

                    return true;

                case "9":

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
                        ImportPDFOrders(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        //SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        //LogCrmContextMultipleResponses(localContext, responses);

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

                case "10":

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
                        ImportPDFAgreements(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        //SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        //LogCrmContextMultipleResponses(localContext, responses);

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
                case "0":
                    return false;
                default:
                    Console.WriteLine("The option " + option + " is not supported. Please choose again. 0) Exit");
                    return true;
            }
        }

        static void Main(string[] args)
        {
            string domainUser = string.Empty;
            string passWord = string.Empty;
            string urlOrganization = string.Empty;

            Console.WriteLine("Perform the Upsales Migration in? [tst]/[uat]/[prod]");
            string input = Console.ReadLine();

            if(input == "tst")
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"TEST Enviroment Selected.");
                Console.WriteLine("TEST Enviroment Selected.");
                domainUser = ConfigurationManager.AppSettings["domainUserTST"];
                passWord = ConfigurationManager.AppSettings["passWordTST"];
                urlOrganization = ConfigurationManager.AppSettings["urlOrganizationTST"];
            }
            else if(input == "uat")
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"UAT Enviroment Selected.");
                Console.WriteLine("UAT Enviroment Selected.");
                domainUser = ConfigurationManager.AppSettings["domainUserUAT"];
                passWord = ConfigurationManager.AppSettings["passWordUAT"];
                urlOrganization = ConfigurationManager.AppSettings["urlOrganizationUAT"];
            }
            else if (input == "prod")
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"PROD Enviroment Selected.");
                Console.WriteLine("PROD Enviroment Selected.");
                domainUser = ConfigurationManager.AppSettings["domainUserPROD"];
                passWord = ConfigurationManager.AppSettings["passWordPROD"];
                urlOrganization = ConfigurationManager.AppSettings["urlOrganizationPROD"];
            }
            else
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Enviroment Selected. No Changes were made.");
                Console.WriteLine("ERROR: No Enviroment Selected.");
                Console.ReadLine();
                return;
            }
            
            IOrganizationService _service = ConnectToMSCRM(domainUser, passWord, urlOrganization);
            if (_service == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The CRM Service is null.");
                Console.WriteLine("ERROR: The CRM Service is null.");
                Console.ReadLine();
                return;
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, $"The CRM Service is not null.");
            Console.WriteLine("The CRM Service is not null.");

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());
            CrmContext crmContext = new CrmContext(_service);
            SaveChangesOptions optionsChanges = SaveChangesOptions.ContinueOnError;

            if(localContext == null || crmContext == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The CRM localContext or the CRM EarlyBound Context is null.");
                Console.WriteLine("The CRM localContext or the CRM EarlyBound Context is null.");
                Console.ReadLine();
                return;
            }

            string relativeExcelPath = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.RelativePath);

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(localContext, crmContext, optionsChanges, relativeExcelPath);
            }
        }
    }
}
