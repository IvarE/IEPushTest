using Endeavor.Crm;
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
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Xml;

namespace Skanetrafiken.Import
{
    class Program
    {
        public static string rel_account_note = "Account_Annotation";
        public static string rel_account_address = "Account_CustomerAddress";
        public static string rel_contact_note = "Contact_Annotation";
        public static string rel_contact_account = "account_primary_contact";
        public static string rel_order_note = "SalesOrder_Annotation";
        public static string rel_order_orderdetail = "order_details";

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
            else if(lCurrencies.Count == 0)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No Currencies found with Currency Name: " + name + ".");
            else if(lCurrencies.Count > 1)
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More than One Currency found with Currency Name: " + name + ".");

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

                                    if(erOrder != null)
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

        public static void UpdateUpsalesIdExistingAccounts(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
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
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        string orgNumber = "Organisationsnummer";
                        string orgNumberValue = GetValueFromLine(importExcelInfo, line, orgNumber);

                        if (orgNumberValue == null || string.IsNullOrEmpty(orgNumberValue))
                            continue;

                        EntityReference erAccount = GetCrmAccountByOrganizationNumber(localContext, orgNumberValue);

                        if (erAccount != null)
                        {
                            string upsalesId = "U-ID";
                            string upsalesIdValue = GetValueFromLine(importExcelInfo, line, upsalesId);

                            if (upsalesIdValue == null || string.IsNullOrEmpty(upsalesIdValue))
                                continue;

                            Account uAccount = new Account();
                            uAccount.Id = erAccount.Id;
                            uAccount.ed_UpsalesId = upsalesIdValue;
                            uAccount.ed_InfotainmentCustomer = true;

                            crmContext.Attach(uAccount);
                            crmContext.UpdateObject(uAccount);
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import Sub Accounts Exception. Details: " + e.Message);
                    }
                }
            }
            Console.WriteLine("Done.");
        }

        public static void UpdateParentAccountExistingSubAccounts(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
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
                    try
                    {
                        progress.Report((double)i / (double)importExcelInfo.lData.Count);
                        List<ExcelLineData> line = importExcelInfo.lData[i];

                        if (line.Count != importExcelInfo.lColumns.Count)
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The line " + (i + 1) + " was not imported, because the data count is not equal to the column count.");
                            continue;
                        }

                        string orgNumber = "Organisationsnummer";
                        string orgNumberValue = GetValueFromLine(importExcelInfo, line, orgNumber);

                        if (orgNumberValue == null || string.IsNullOrEmpty(orgNumberValue))
                            continue;

                        EntityReference erParentAccount = GetCrmAccountByOrganizationNumber(localContext, orgNumberValue);

                        if (erParentAccount != null)
                        {
                            string upsalesId = "U-ID";
                            string upsalesIdValue = GetValueFromLine(importExcelInfo, line, upsalesId);

                            if (upsalesIdValue == null || string.IsNullOrEmpty(upsalesIdValue))
                                continue;

                            EntityReference erSubAccount = GetCrmAccountByUpsalesId(localContext, upsalesIdValue);

                            if(erSubAccount != null)
                            {
                                Account uAccount = new Account();
                                uAccount.Id = erSubAccount.Id;
                                uAccount.ParentAccountId = erParentAccount;

                                crmContext.Attach(uAccount);
                                crmContext.UpdateObject(uAccount);
                            }
                        }
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

                                    if(erContact != null)
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

                                    if(accountAdded.Count == 0)
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

                        if(totalLineAmmount != string.Empty)
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

        public static void UpdateTotalAmmountonOrders(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            Console.Write("Creating Batch of Orders... ");
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

                        string orderUID = "Orderns U-ID";
                        string orderUIDValue = GetValueFromLine(importExcelInfo, line, orderUID);

                        if (orderUIDValue == null || string.IsNullOrEmpty(orderUIDValue))
                            continue;

                        EntityReference erOrder = GetCrmOrderByUpsalesId(localContext, orderUIDValue);

                        if (erOrder != null)
                        {
                            string totalLineAmmount = "Värde";
                            string totalLineAmmountValue = GetValueFromLine(importExcelInfo, line, totalLineAmmount);

                            if (totalLineAmmountValue == null || string.IsNullOrEmpty(totalLineAmmountValue))
                                continue;

                            decimal dtotalLineAmount = decimal.MinValue;

                            if (decimal.TryParse(totalLineAmmountValue, out dtotalLineAmount))
                            {
                                if (dtotalLineAmount != decimal.MinValue)
                                    CreateSalesOrderDetail(localContext, crmContext, dtotalLineAmount, erOrder);
                            }
                            else
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Line " + (i + 1) + ": Couldn't parse " + totalLineAmmountValue + " to a decimal value.");
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
                                    else if(l.Count == 1)
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

        public static void RefreshGlobalOptionSets(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            List<string> lAddCompanyTrade = new List<string>();
            List<string> lAddBusinessType = new List<string>();
            OptionMetadataCollection colOpCompanyTrade = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_companytrade);
            OptionMetadataCollection colOpBusinessType = GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

            Console.Write("Creating Batch of OptionSets Values... ");
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
                                case "Bolagsform text":

                                    int? optionSetBT = GetOptionSetValueByName(colOpBusinessType, value);
                                    bool existsB = lAddBusinessType.Any(x => x == value);
                                    if (optionSetBT == null && !existsB)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        lAddBusinessType.Add(value);
                                    }

                                    break;
                                case "Branschtext":

                                    int? optionSetCT = GetOptionSetValueByName(colOpCompanyTrade, value);
                                    bool existsC = lAddCompanyTrade.Any(x => x == value);

                                    if (optionSetCT == null && !existsC)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        lAddCompanyTrade.Add(value);
                                    }
                                    break;

                                default:
                                    _log.InfoFormat(CultureInfo.InvariantCulture, $"The Column " + name + " is not on the mappings initially set.");
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Import 2Accounts Exception. Details: " + e.Message);
                    }
                }
            }

            foreach (string option in lAddBusinessType)
                InsertGlobalOptionSetOption(localContext, "ed_businesstype", option, 1053);

            foreach (string option in lAddCompanyTrade)
            InsertGlobalOptionSetOption(localContext, "ed_companytrade", option, 1053);

            Console.WriteLine("Done.");
        }


        public static void CleanSingaporeDuplicates(Plugin.LocalPluginContext localContext, CrmContext crmContext)
        {
            try
            {
                string queryString = "select st_ContactID, st_TicketID, Count(*) from st_singaporeticketBase group by st_ContactID, st_TicketID having count(*) > 1";
                string connectionString = "Server=AG-SQL4-CRM;Database=DKCRM_MSCRM;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        List<string> lTickets = new List<string>();

                        QueryExpression query = new QueryExpression(st_singaporeticket.EntityLogicalName);
                        query.ColumnSet.AddColumns(st_singaporeticket.Fields.st_singaporeticketId, st_singaporeticket.Fields.st_TicketID);

                        FilterExpression queryCriteria = new FilterExpression();
                        query.Criteria.AddFilter(queryCriteria);
                        queryCriteria.FilterOperator = LogicalOperator.Or;

                        int i = 0;
                        while (reader.Read())
                        {
                            Guid contactId = new Guid(reader["st_ContactID"].ToString());
                            string ticketId = reader["st_TicketID"].ToString();

                            queryCriteria.AddCondition(st_singaporeticket.Fields.st_TicketID, ConditionOperator.Equal, ticketId);

                            lTickets.Add(ticketId);
                            //QueryExpression querySingapore = new QueryExpression(st_singaporeticket.EntityLogicalName);
                            //querySingapore.NoLock = true;
                            //querySingapore.TopCount = 5000;
                            //querySingapore.ColumnSet = new ColumnSet(st_singaporeticket.Fields.st_ContactID, st_singaporeticket.Fields.st_TicketID);
                            //querySingapore.Criteria.AddCondition(st_singaporeticket.Fields.st_ContactID, ConditionOperator.Equal, contactId);
                            //querySingapore.Criteria.AddCondition(st_singaporeticket.Fields.st_TicketID, ConditionOperator.Equal, ticketId);

                            //List<st_singaporeticket> lSingapore = XrmRetrieveHelper.RetrieveMultiple<st_singaporeticket>(localContext, querySingapore);

                            //for (int j = 1; j < lSingapore.Count; j++)
                            //{
                            //    st_singaporeticket item = lSingapore[j];
                            //    st_singaporeticket dSingapore = new st_singaporeticket();
                            //    dSingapore.Id = item.Id;
                            //    crmContext.Attach(dSingapore);
                            //    crmContext.DeleteObject(dSingapore);
                            //}

                            i++;
                            
                        }

                        List<st_singaporeticket> lDynamics = XrmRetrieveHelper.RetrieveMultiple<st_singaporeticket>(localContext, query);
                        Console.WriteLine("sfasfasf");
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        reader.Close();
                    }
                }

            }
            catch (Exception e)
            {
                _log.Error("ERROR: " + e.Message);
                Console.WriteLine("ERROR: " + e.Message);
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

            Console.WriteLine("-------Fixes-------");
            Console.WriteLine("10) Fix: Duplicate OptionSets on Account/Branch");
            Console.WriteLine("11) Fix: Check for Duplicate Records");
            Console.WriteLine("12) Fix: Update SubAccounts Records");
            Console.WriteLine("13) Fix: Update Price List on Orders");
            Console.WriteLine("14) Fix: Update Total Amount on Orders");

            Console.WriteLine("15) Fix: Import Accounts May 2021");
            Console.WriteLine("16) Fix: Refresh Global OptionSets(ed_BusinessType/ed_companytrade)");
            Console.WriteLine("17) Fix: Clean Singapore Duplicated");


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

                    #region Update Upsales Id from Spann 2

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update Upsales Id from Spann 2 of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket2Accounts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            UpdateUpsalesIdExistingAccounts(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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

                case "5":

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
                case "6":

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
                case "7":

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
                        ImportPDFOrders(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        LogCrmContextMultipleResponses(localContext, responses);

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
                        ImportPDFAgreements(localContext, crmContext, fullPath, filter);

                        Console.WriteLine("Sending Batch of Orders to Sekund...");

                        SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                        LogCrmContextMultipleResponses(localContext, responses);

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

                //-----------------------FIXES-----------------------

                case "10":

                    #region Fix Duplicated OptionSets On Account/Branch

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


                case "11":

                    #region Check for Account Duplicate Records

                    Console.WriteLine("What's the logical name of the entity?");
                    string entityName = Console.ReadLine();

                    Console.WriteLine("What's the string unique schema name of the field for that entity?");
                    string uniqueField = Console.ReadLine();

                    if(entityName == null || string.IsNullOrEmpty(entityName))
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

                    if(lRecords.Count == 0)
                    {
                        Console.WriteLine("No Accounts found.");
                        return true;
                    }

                    List<List<Entity>> lDuplicates = lRecords.GroupBy(i => i.GetAttributeValue<string>(uniqueField)).Where(x => x.ToList().Count > 1).Select(x => x.ToList()).ToList();

                    if(lDuplicates.Count == 0)
                    {
                        Console.WriteLine("No Duplicates found with entity name: " + entityName + " and unique field: " + uniqueField);
                        return true;
                    }

                    foreach (List<Entity> duplicate in lDuplicates)
                    {
                        Entity dEntity = duplicate.FirstOrDefault();
                        if(dEntity != null)
                            Console.WriteLine("The " + entityName + " is duplicated on field " + uniqueField + " with value: " + dEntity.GetAttributeValue<string>(uniqueField));
                    }

                    #endregion

                    return true;

                case "12":

                    #region Update SubAccounts Records

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update SubAccounts Records of the Account Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Bucket3SubAccounts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            UpdateParentAccountExistingSubAccounts(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Accounts to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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
                case "13":

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
                        EntityReference erPriceList = GetCRMPriceListByName(localContext, defaultPriceList);

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
                        LogCrmContextMultipleResponses(localContext, responses);

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
                case "14":

                    #region Update Total Ammount on Orders

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Update Total Ammount on Orders of the Order Entity--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Orders);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            UpdateTotalAmmountonOrders(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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

                case "15":

                    #region Add New Options to Local OptionSet and Import Accounts

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Add New Options to Local OptionSet and Import Accounts--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.BucketNewAccounts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            Import2AccountRecords(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of Orders to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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

                case "16":

                    #region Refresh OptionSets

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Refresh OptionSets--------------");

                        string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.BucketNewAccounts);
                        ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                        bool isParsingOk = GetParsingStatus(importExcelInfo);

                        if (isParsingOk)
                        {
                            RefreshGlobalOptionSets(localContext, crmContext, importExcelInfo);

                            Console.WriteLine("Sending Batch of OptionSets to Sekund...");

                            SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                            LogCrmContextMultipleResponses(localContext, responses);

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

                case "17":

                    #region Clean Singapore Duplicated

                    try
                    {
                        crmContext.ClearChanges();
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Clean Duplicates Singapore--------------");


                        CleanSingaporeDuplicates(localContext, crmContext);

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

                case "0":
                    return false;
                default:
                    Console.WriteLine("The option " + option + " is not supported. Please choose again. 0) Exit");
                    return true;
            }
        }
        
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

        public static void ImportContactsMKLId(Plugin.LocalPluginContext localContext, CrmContext crmContext, SaveChangesOptions optionsChanges, string relativeExcelPath)
        {
            try
            {
                crmContext.ClearChanges();
                _log.InfoFormat(CultureInfo.InvariantCulture, $"--------------Starting to Upload MKL Contacts--------------");
                Console.WriteLine("--------------Starting to Upload MKL Contacts--------------");

                string fileName = Environment.ExpandEnvironmentVariables(Properties.Settings.Default.MKLContacts);
                ImportExcelInfo importExcelInfo = HandleExcelInformationStreamReader(relativeExcelPath, fileName);

                bool isParsingOk = GetParsingStatus(importExcelInfo);

                if (isParsingOk)
                {
                    ImportMKLContactsRecords(localContext, crmContext, importExcelInfo);

                    Console.WriteLine("Sending Batch of Contacts to Sekund...");

                    SaveChangesResultCollection responses = crmContext.SaveChanges(optionsChanges);
                    LogCrmContextMultipleResponses(localContext, responses);

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

            string passwordArgument = "uSEme2!nstal1";

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
            string runUpdateContacts = ConfigurationManager.AppSettings["runUpdateContacts"];

            if(runUpdateContacts == "true")
            {
                Console.WriteLine("Run Update Contacts selected... from file Import analysdata.csv.");
                ImportContactsMKLId(localContext, crmContext, optionsChanges, relativeExcelPath);
            }
            else
            {
                Console.WriteLine("Run Updales import selected...");
                bool showMenu = true;
                while (showMenu)
                {
                    showMenu = MainMenuUpsales(localContext, crmContext, optionsChanges, relativeExcelPath);
                }
            }
        }
    }
}
