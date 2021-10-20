using Endeavor.Crm;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Import.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Import
{
    public class FixesHelper
    {
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                        string orgNumberValue = ImportHelper.GetValueFromLine(importExcelInfo, line, orgNumber);

                        if (orgNumberValue == null || string.IsNullOrEmpty(orgNumberValue))
                            continue;

                        EntityReference erAccount = ImportHelper.GetCrmAccountByOrganizationNumber(localContext, orgNumberValue);

                        if (erAccount != null)
                        {
                            string upsalesId = "U-ID";
                            string upsalesIdValue = ImportHelper.GetValueFromLine(importExcelInfo, line, upsalesId);

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
                        string orgNumberValue = ImportHelper.GetValueFromLine(importExcelInfo, line, orgNumber);

                        if (orgNumberValue == null || string.IsNullOrEmpty(orgNumberValue))
                            continue;

                        EntityReference erParentAccount = ImportHelper.GetCrmAccountByOrganizationNumber(localContext, orgNumberValue);

                        if (erParentAccount != null)
                        {
                            string upsalesId = "U-ID";
                            string upsalesIdValue = ImportHelper.GetValueFromLine(importExcelInfo, line, upsalesId);

                            if (upsalesIdValue == null || string.IsNullOrEmpty(upsalesIdValue))
                                continue;

                            EntityReference erSubAccount = ImportHelper.GetCrmAccountByUpsalesId(localContext, upsalesIdValue);

                            if (erSubAccount != null)
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
                        string orderUIDValue = ImportHelper.GetValueFromLine(importExcelInfo, line, orderUID);

                        if (orderUIDValue == null || string.IsNullOrEmpty(orderUIDValue))
                            continue;

                        EntityReference erOrder = ImportHelper.GetCrmOrderByUpsalesId(localContext, orderUIDValue);

                        if (erOrder != null)
                        {
                            string totalLineAmmount = "Värde";
                            string totalLineAmmountValue = ImportHelper.GetValueFromLine(importExcelInfo, line, totalLineAmmount);

                            if (totalLineAmmountValue == null || string.IsNullOrEmpty(totalLineAmmountValue))
                                continue;

                            decimal dtotalLineAmount = decimal.MinValue;

                            if (decimal.TryParse(totalLineAmmountValue, out dtotalLineAmount))
                            {
                                if (dtotalLineAmount != decimal.MinValue)
                                    ImportHelper.CreateSalesOrderDetail(localContext, crmContext, dtotalLineAmount, erOrder);
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

        public static void RefreshGlobalOptionSets(Plugin.LocalPluginContext localContext, CrmContext crmContext, ImportExcelInfo importExcelInfo)
        {
            if (importExcelInfo == null || importExcelInfo.lColumns == null || importExcelInfo.lData == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to read Excel Information. Please contact your Administrator.");
                return;
            }

            List<string> lAddBusinessType = new List<string>();
            OptionMetadataCollection colOpBusinessType = ImportHelper.GetOptionSetMetadata(localContext, Account.EntityLogicalName, Account.Fields.ed_BusinessType);

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

                            ExcelColumn selectedColumn = ImportHelper.GetSelectedExcelColumn(importExcelInfo.lColumns, j);

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

                                    int? optionSetBT = ImportHelper.GetOptionSetValueByName(colOpBusinessType, value);
                                    bool existsB = lAddBusinessType.Any(x => x == value);
                                    if (optionSetBT == null && !existsB)
                                    {
                                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"The OptionSet " + value + " was not found on CRM. By default it will be created into Skund.");
                                        lAddBusinessType.Add(value);
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
                ImportHelper.InsertGlobalOptionSetOption(localContext, "ed_businesstype", option, 1053);

            Console.WriteLine("Done.");
        }

        public static void CleanSingaporeDuplicates(Plugin.LocalPluginContext localContext, CrmContext crmContext)
        {
            try
            {
                string queryString = "SELECT [TicketId], [cgi_contactnumber] FROM [STDW].[dbo].[GetTicketInfoView]";
                string connectionString = "Server=V-DWSQL;Database=STDW;Integrated Security=True;";

                Dictionary<string, string> lTickets = new Dictionary<string, string>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            Guid contactId = new Guid(reader["cgi_contactnumber"].ToString());
                            string ticketId = reader["TicketId"].ToString();

                            lTickets.Add(ticketId, contactId.ToString());
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
                    }
                    finally
                    {
                        // Always call Close when done reading.
                        reader.Close();
                    }
                }

                QueryExpression query = new QueryExpression(st_singaporeticket.EntityLogicalName);
                query.NoLock = true;
                query.ColumnSet.AddColumns(st_singaporeticket.Fields.st_singaporeticketId, st_singaporeticket.Fields.st_TicketID, st_singaporeticket.Fields.ed_CRMNummer);

                List<st_singaporeticket> lCRMTickets = XrmRetrieveHelper.RetrieveMultiple<st_singaporeticket>(localContext, query);
                Console.WriteLine("Tickets Dynamics: " + lCRMTickets.Count);
                Console.WriteLine("Tickets View: " + lTickets.Count);


            }
            catch (Exception e)
            {
                _log.Error("ERROR: " + e.Message);
                Console.WriteLine("ERROR: " + e.Message);
            }
        }
    }
}
