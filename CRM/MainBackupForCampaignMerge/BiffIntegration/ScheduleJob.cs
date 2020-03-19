using Common.Logging;
using Quartz;
using System;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Endeavor.Crm.BiffIntegration
{
    /// <summary>
    /// Class used to check if the Schedule defined in CRM has changed.
    /// </summary>
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class ScheduleJob : IJob
    {
        public const string JobName = "ScheduleJob1";
        public const string JobDescription = "BiffIntegration";
        public const string TriggerName = "ScheduleTrigger1";
        public const string TriggerDescription = "Schedule Trigger";
        public const string GroupName = "Schedule";
        public const string DataMapModifiedAfter = "ModifiedAfter";

        private List<Tuple<DataRow, Exception>> _errorRows = null;
        private ColumnSet travelCardFields = new ColumnSet(
            TravelCardEntity.Fields.cgi_travelcardnumber,
            TravelCardEntity.Fields.cgi_ValueCardTypeId,
            TravelCardEntity.Fields.cgi_PeriodCardTypeId,
            TravelCardEntity.Fields.statecode,
            // Period
            TravelCardEntity.Fields.cgi_Blocked,
            TravelCardEntity.Fields.ed_BlockedDate,
            TravelCardEntity.Fields.ed_BlockedDescription,
            // Reskassa
            TravelCardEntity.Fields.ed_BlockedValue,
            TravelCardEntity.Fields.ed_BlockedDateValue,
            TravelCardEntity.Fields.ed_BlockedDescriptionValue,
            // Kort
            TravelCardEntity.Fields.ed_BlockedCard,
            TravelCardEntity.Fields.ed_BlockedDateCard,
            TravelCardEntity.Fields.ed_BlockedDescriptionCard,
            TravelCardEntity.Fields.cgi_ValidFrom,
            TravelCardEntity.Fields.cgi_ValidTo,
            TravelCardEntity.Fields.ed_LastTravelDate,
            TravelCardEntity.Fields.ed_ModifiedInBiff
            );

        //private const string _connectionString = @"Server=ED-SQL1\CRM1;DataBase=CRMTravelCard;Integrated Security=SSPI";
        //private const string _connStr = @"Persist Security Info=False;User ID=edCRMTravelCardUser;Password=uSEme2!nstal1;Trusted_Connection=True; database=CRMTravelCard;server=ED-SQL1\CRM1";
        //private const string _connStr2 = "User ID=edCRMTravelCardUser;Password=uSEme2!nstal1;Initial Catalog=CRMTravelCard;Server=ED-SQL1\\CRM1";

        private ILog _log = LogManager.GetLogger(typeof(ScheduleJob));

        private TimeSpan? AdjustTimezone;

        // Default constructor
        public ScheduleJob()
        {
            AdjustTimezone = null;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));

            ExecuteJob();

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecuteJob()
        {
            try
            {
                // TODO: (teo) - Run the integration flow with parameters from cgi_settings
                Plugin.LocalPluginContext localContext = GenerateLocalContext();

                CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_BiffIntegrationRuntime));

                // Execute depending on settings
                if (setting == null)
                {
                   _log.Error("No instance of cgi_Setting found");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(setting.ed_BiffIntegrationRuntime))
                {
                    _log.Error($"No value found for valiable {CgiSettingEntity.Fields.ed_BiffIntegrationRuntime} in settins entity");
                    return;
                }

                // Inside working hours?
                if (TimeToExecute(setting.ed_BiffIntegrationRuntime))
                {
                    // Prepare all database related things. Copy from DW database etc
                    ExecuteSQLCommands(localContext);

                    // Do the merge in CRM!!!
                    // **********************
                    ImportTravelCardDiff(localContext, setting.ed_BiffIntegrationRuntime);
                }
            }
            catch (Exception e)
            {
                _log.Error($"BiffIntegrations ScheduleJob caught an unexpected error:\n{e.Message}");
            }
        }

        /// <summary>
        /// Get setting from CRM and check if now is within run time for execution
        /// </summary>
        /// <param name="runtime"></param>
        /// <returns>
        /// true: It's ok to execute service
        /// false: It's outside of running hours.
        /// </returns>
        private bool TimeToExecute(string runtime)
        {
            string[] runtimes = runtime.Split('-');
            if (runtimes.Length != 2)
            {
                throw new Exception($"Invalid format for runtime parameter '{runtime}'. Needs to be at form xx-xx");
            }
            int startHour, endHour;
            if (!int.TryParse(runtimes[0], out startHour))
            {
                throw new Exception($"Could not parse '{runtimes[0]}' to an integer.");
            }
            if (!int.TryParse(runtimes[1], out endHour))
            {
                throw new Exception($"Could not parse '{runtimes[1]}' to an integer.");
            }
            if (startHour < 0 || startHour > 24)
            {
                throw new Exception($"Invalid value for start hour {startHour}. Must be 0-24");
            }
            if (endHour < 0 || endHour > 24)
            {
                throw new Exception($"Invalid value for end hour {endHour}. Must be 0-24");
            }
            if (startHour == endHour)
            {
                throw new Exception($"Runtime cannot start and end the same hour.");
            }

#if DEV
            return true;
#endif

#if TEST
           _log.Debug($"Running between:{startHour} and {endHour}. Now is:{DateTime.Now}");
#endif

            DateTime now = DateTime.Now;
            if (endHour < startHour)
            {
                return now.Hour >= startHour || now.Hour < endHour;
            }
            else
            {
                return now.Hour >= startHour && now.Hour < endHour;
            }
        }

        /// <summary>
        /// Main execution function
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="timeFrameToRunService"></param>
        public void ImportTravelCardDiff(Plugin.LocalPluginContext localContext, string timeFrameToRunService)
        {
            string query = "SELECT [CardNumber] " +
                               ",[CardOfferingReskassa] " + 
                               ",[CardOfferingPeriodkort] " +
                               ",[IsActive] " +          // Statecode
                               // Period
                               ",[Blocked] " +
                               ",case when [BlockedDate] <> '1899-12-30' then [BlockedDate] else null end as 'BlockedDate' " +
                               ",NULLIF([BlockedDescription],'') as 'BlockedDescription' " +
                               // Reskassa
                               ",[BlockedValue] " +
                               ",case when[BlockedDateValue] <> '1899-12-30' then[BlockedDateValue] else null end as 'BlockedDateValue' " +
                               ",NULLIF([BlockedDescriptionValue],'') as 'BlockedDescriptionValue' " +
                               // Hela kortet spärrat
                               ",[BlockedCard]  " +
                               ",case when[BlockedDateCard] <> '1899-12-30' then[BlockedDateCard] else null end as 'BlockedDateCard'  " +
                               ",NULLIF([BlockedDescriptionCard],'') as 'BlockedDescriptionCard'  " +

                               ",case when [ValidFromDate] <> '1899-12-30' then [ValidFromDate] else null end as 'ValidFromDate' " +
                               ",case when [ValidToDate] <> '1899-12-30' then [ValidToDate] else null end as 'ValidToDate' " +
                               ",case when [LatestTravelDate] <> '1899-12-30' then [LatestTravelDate] else null end as 'LatestTravelDate' " +
                               ",CONVERT(date, [ModifiedDate]) as 'ModifiedDate' " +
                               " from dbo.TravelCardDiff "
                               // + " where cardnumber = '1200672086'"
                               ;

            DataTable dataTable = new DataTable();
            using (SqlConnection conn = CreateSqlConnectionDeltaDatabase())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                                                    
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                    conn.Close();
                    da.Dispose();
                }
            }

            _log.Debug($"Found {dataTable.Rows.Count} delta records to process. Starting...");

            _errorRows = new List<Tuple<DataRow, Exception>>();
            int iProcessedCards = 0;
            int iUpdatedCards = 0;
            int iActivatedCards = 0;
            int iDeactivatedCards = 0;
            int iCardMissningInCRM = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                iProcessedCards++;
                try
                {
                    TravelCardEntity fromDiffTable = new TravelCardEntity();

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        MatchColumnValueToCrmEntity(localContext, ref fromDiffTable, row, column);
                    }

                    // Try to get with exact match
                    TravelCardEntity existingTravelCardInCRM = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardFields,
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, fromDiffTable.cgi_travelcardnumber)
                            }
                        });

                    // No match? To to search with endsWith. Sometimes there are leading zeroes...
                    if (existingTravelCardInCRM == null)
                    {
                        existingTravelCardInCRM = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardFields,
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.EndsWith, fromDiffTable.cgi_travelcardnumber)
                            }
                        });
                    }

                    if (existingTravelCardInCRM != null)
                    {
                        TravelCardEntity updateEntity = new TravelCardEntity
                        {
                            Id = existingTravelCardInCRM.Id
                        };

                        bool update = false;
                        foreach (string field in travelCardFields.Columns)
                        {
                            // Not used in update, separate SetStateRequest.
                            if ("statecode".Equals(field))
                                continue;

                            //if (!fromDiffTable.Contains(field))
                            //    continue;

                            switch (fromDiffTable.Contains(field) ? fromDiffTable[field].GetType().ToString() : "null")
                            {
                                case "null":
                                    if (existingTravelCardInCRM.Contains(field) == true )
                                    {
                                        updateEntity[field] = null;
                                        update = true;
                                    }
                                    break;

                                case "System.Boolean":
                                    if(existingTravelCardInCRM.Contains(field) == false || fromDiffTable[field].ToString() != existingTravelCardInCRM[field].ToString())
                                    {
                                        updateEntity[field] = fromDiffTable[field];
                                        update = true;
                                    }
                                    break;

                                case "System.DateTime":
                                    if (existingTravelCardInCRM.Contains(field) == false || (DateTime)fromDiffTable[field] != AdjustDateTimeForTimeZone(localContext, (DateTime)existingTravelCardInCRM[field]))
                                    {
                                        updateEntity[field] = fromDiffTable[field];
                                        update = true;
                                    }
                                    break;

                                case "System.String":
                                case "System.Int32":
                                    if (existingTravelCardInCRM.Contains(field) == false || fromDiffTable[field].ToString() != existingTravelCardInCRM[field].ToString())
                                    {
                                        updateEntity[field] = fromDiffTable[field];
                                        update = true;
                                    }
                                    break;

                                default:
                                    throw new NotImplementedException("Switch statement is missing types.");
                            }
                        }

                        // Changed from inactive -> active?
                        if (fromDiffTable.statecode == Generated.cgi_travelcardState.Active && existingTravelCardInCRM.statecode == Generated.cgi_travelcardState.Inactive)
                        {
                            SetStateRequest req = new SetStateRequest()
                            {
                                EntityMoniker = existingTravelCardInCRM.ToEntityReference(),
                                State = new OptionSetValue((int)Generated.cgi_travelcardState.Active),
                                Status = new OptionSetValue((int)Generated.cgi_travelcard_statuscode.Active)
                            };
                            localContext.OrganizationService.Execute(req);
                            iActivatedCards++;
                        }

                        // Update only changed columns!
                        if (update)
                        {
                            XrmHelper.Update(localContext, updateEntity);
                            iUpdatedCards++;
                        }

                        // Changed from active -> inactive?
                        if (fromDiffTable.statecode == Generated.cgi_travelcardState.Inactive && existingTravelCardInCRM.statecode == Generated.cgi_travelcardState.Active)
                        {
                            SetStateRequest req = new SetStateRequest()
                            {
                                EntityMoniker = existingTravelCardInCRM.ToEntityReference(),
                                State = new OptionSetValue((int)Generated.cgi_travelcardState.Inactive),
                                Status = new OptionSetValue((int)Generated.cgi_travelcard_statuscode.Inactive)
                            };
                            localContext.OrganizationService.Execute(req);
                            iDeactivatedCards++;
                        }
                    }
                    else
                        iCardMissningInCRM ++;
                }
                catch (Exception e)
                {
                    _errorRows.Add(new Tuple<DataRow, Exception>(row, e));
                }


                // Still in timeframe for execution?
                if(!TimeToExecute(timeFrameToRunService))
                {
                    // No! Abort
                    _log.Debug("Aborting execution. Time outside of execution span.");
                    break;
                }

            }

            // Info - JoAn -Always write a log
            if (_errorRows.Count >= 0)
            {
                string Message = $"Processed:{iProcessedCards}, Updated:{iUpdatedCards}, Activated:{iActivatedCards}, Deactivated:{iDeactivatedCards} travel cards. {iCardMissningInCRM} missing in CRM.";

                WriteToBiffIntegrationLog(localContext, Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="iProcessedCards"></param>
        /// <param name="iUpdatedCards"></param>
        /// <param name="iActivatedCards"></param>
        /// <param name="iDeactivatedCards"></param>
        /// <param name="iCardMissningInCRM"></param>
        private void WriteToBiffIntegrationLog(Plugin.LocalPluginContext localContext, string Message)
        {
            try
            {

                _log.Error($"Starting log, Message:{Message} ");


                BiffIntegrationErrorLogEntity errorLog = new BiffIntegrationErrorLogEntity()
                {
                    ed_name = $"Biff Integration Log - {DateTime.Now.ToString("yyyy/MM/dd - hh:mm")}"
                };

                // Cut if string to long.
                if (Message != null)
                    if (Message.Length > BiffIntegrationErrorLogEntity.FieldLengths.ed_Statistics)
                        errorLog.ed_Statistics = Message.Substring(0, BiffIntegrationErrorLogEntity.FieldLengths.ed_Statistics);
                    else
                        errorLog.ed_Statistics = Message;


                errorLog.Id = XrmHelper.Create(localContext, errorLog);
                errorLog.ed_biffintegrationerrorlogId = errorLog.Id;




                // Need to log rows?
                if(_errorRows != null)
                {
                    _log.Error($"Found {_errorRows.Count} error rows. Starting to log...");


                    foreach (Tuple<DataRow, Exception> tuple in _errorRows)
                    {

                        BiffIntegrationErrorLogRowEntity errRow = new BiffIntegrationErrorLogRowEntity
                        {
                            ed_BiffIntegrationErrorLog = errorLog.ToEntityReference(),
                            ed_CardNumber = tuple.Item1.Field<int>("CardNumber").ToString(),
                            ed_CardOfferingReskassa = tuple.Item1.Field<int>("CardOfferingReskassa").ToString(),
                            ed_CardOfferingPeriodkort = tuple.Item1.Field<int>("CardOfferingPeriodkort").ToString(),
                            ed_IsActive = tuple.Item1.Field<int>("IsActive"),
                            ed_Blocked = tuple.Item1.Field<int>("Blocked"),
                            ed_BlockedDate = tuple.Item1.IsNull("BlockedDate") == false ? tuple.Item1.Field<DateTime?>("BlockedDate") : null,
                            ed_BlockedDescription = tuple.Item1.Field<string>("BlockedDescription"),
                            ed_ValidFromDate = tuple.Item1.IsNull("ValidFromDate") == false ? tuple.Item1.Field<DateTime?>("ValidFromDate") : null,
                            ed_ValidToDate = tuple.Item1.IsNull("ValidToDate") == false ? tuple.Item1.Field<DateTime?>("ValidToDate") : null,
                            ed_LatestTravelDate = tuple.Item1.IsNull("LatestTravelDate") == false ? tuple.Item1.Field<DateTime?>("LatestTravelDate") : null,
                            ed_ModifiedDate = tuple.Item1.Field<DateTime>("ModifiedDate")
                        };
                        XrmHelper.Create(localContext, errRow);
                    }

                }



            }
            catch (Exception e)
            {
                _log.Error($"Error caught when creating Error Log:\n{e.Message}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDiffTable"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void MatchColumnValueToCrmEntity(Plugin.LocalPluginContext localContext, ref TravelCardEntity fromDiffTable, DataRow row, DataColumn column)
        {
            switch (column.ColumnName)
            {
                case "CardNumber":
                    fromDiffTable.cgi_travelcardnumber = row.Field<long>(column.ColumnName).ToString();
                    break;
                case "CardOfferingReskassa":
                    fromDiffTable.cgi_ValueCardTypeId = row.Field<int>(column.ColumnName);
                    break;
                case "CardOfferingPeriodkort":
                    fromDiffTable.cgi_PeriodCardTypeId = row.Field<int>(column.ColumnName);
                    break;
                case "IsActive":
                    if (row.Field<int>(column.ColumnName) == 0)
                        fromDiffTable[Generated.cgi_travelcard.Fields.statecode] = new OptionSetValue((int)Generated.cgi_travelcardState.Active);
                    else if (row.Field<int>(column.ColumnName) == 1)
                        fromDiffTable[Generated.cgi_travelcard.Fields.statecode] = new OptionSetValue((int)Generated.cgi_travelcardState.Inactive);
                    break;
                case "Blocked":
                    if (row.Field<int>(column.ColumnName) == 0)
                        fromDiffTable.cgi_Blocked = false;
                    else if (row.Field<int>(column.ColumnName) == 1)
                        fromDiffTable.cgi_Blocked = true;
                    break;
                case "BlockedValue":
                    if (row.Field<int>(column.ColumnName) == 0)
                        fromDiffTable.ed_BlockedValue = false;
                    else if (row.Field<int>(column.ColumnName) == 1)
                        fromDiffTable.ed_BlockedValue = true;
                    break;
                case "BlockedCard":
                    if (row.Field<int>(column.ColumnName) == 0)
                        fromDiffTable.ed_BlockedCard = false;
                    else if (row.Field<int>(column.ColumnName) == 1)
                        fromDiffTable.ed_BlockedCard = true;
                    break;
                case "BlockedDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDate = row.Field<DateTime>(column.ColumnName);
                    break;
                case "BlockedDateValue":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDateValue = row.Field<DateTime>(column.ColumnName);
                    break;
                case "BlockedDateCard":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDateCard = row.Field<DateTime>(column.ColumnName);
                    break;
                case "BlockedDescription":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDescription = row.Field<string>(column.ColumnName);
                    break;
                case "BlockedDescriptionValue":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDescriptionValue = row.Field<string>(column.ColumnName);
                    break;
                case "BlockedDescriptionCard":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDescriptionCard = row.Field<string>(column.ColumnName);
                    break;
                case "ValidFromDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_ValidFrom = row.Field<DateTime>(column.ColumnName);
                    break;
                case "ValidToDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_ValidTo = row.Field<DateTime>(column.ColumnName);
                    break;
                case "LatestTravelDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_LastTravelDate = row.Field<DateTime>(column.ColumnName);
                    break;
                case "ModifiedDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_ModifiedInBiff = row.Field<DateTime>(column.ColumnName);
                    break;

                default:
                    _log.Error($"Unrecognized columnName: {column.ColumnName}");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecuteSQLCommands(Plugin.LocalPluginContext localContext)
        {
            string ActiveCommand = null;
            try
            {
                DateTime lastExecutionTime = DateTime.MinValue;
                DateTime maxModifiedOn = DateTime.MinValue;

                // Get latest execution time from delta DB
                using (SqlConnection sqlConn = CreateSqlConnectionDeltaDatabase())
                {
                    sqlConn.Open();

                    // Remove all old entries from the Delta table.
                    ActiveCommand = "delete TravelCardDiff";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // Remove all old entries from BIFF table.
                    ActiveCommand = "TRUNCATE TABLE [BIFFtravelcards]";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }



                    // Remove all old entries from the Delta table.
                    ActiveCommand = "SELECT MAX([LatestModifiedDate]) FROM [dbo].[LoadDates] WHERE [Source] = 'GetCardView'";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.Text,
                        CommandTimeout = 0
                    })
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lastExecutionTime = reader.GetDateTime(0);
                                _log.Debug($"Got last exec time:{lastExecutionTime}");
                                break;
                            }
                        }
                    }
                    sqlConn.Close();
                }


                using (SqlConnection sqlBiffConn = CreateSqlConnectionBIFFDatabase())
                {
                    sqlBiffConn.Open();

                    // Get data in shunks...
                    int iGetRecordsPerBatch = 100000;
                    while (true)
                    {
                        DataTable dtBIFFData = new DataTable();

                        StringBuilder query = new StringBuilder($"SELECT TOP({iGetRecordsPerBatch}) [Activated],  [BlockDate], ");
                        query.Append("           [BlockDescription], [BlockState], ");
                        query.Append("           [CardNumber], [CardSection], ");
                        query.Append("           [ExpireDay],  [IsActive], ");
                        query.Append("           [LatestTravelDate], [ModifiedOn], ");
                        query.Append("           [PeriodStartDay], [TicketCodeNumber] ");
                        query.Append($"    FROM [{Properties.Settings.Default.BIFFDBName}].[dbo].[GetCardView] ");
                        query.Append($"   WHERE [ModifiedOn] > '{lastExecutionTime}'");
                        query.Append($"   order by [ModifiedOn] ");

                        ActiveCommand = query.ToString();
                        _log.Debug($"Running SQL command in DW database: {ActiveCommand}");
                        SqlCommand cmd = new SqlCommand(query.ToString(), sqlBiffConn);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtBIFFData);
                            sqlBiffConn.Close();
                            da.Dispose();
                        }

                        // Abort if no more rows...
                        if (dtBIFFData.Rows.Count == 0)
                            break;

                        // Write to destination
                        using (SqlConnection sqlConn = CreateSqlConnectionDeltaDatabase())
                        {
                            sqlConn.Open();

                            // Spara ner BIFF rådata.
                            ActiveCommand = "insert BIFFtravelcards (SQLBulk)";
                            _log.Debug($"Running SQL command: {ActiveCommand}");
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn))
                            {
                                bulkCopy.DestinationTableName = "BIFFtravelcards";
                                bulkCopy.WriteToServer(dtBIFFData);
                            }

                            sqlConn.Close();
                        }

                        // Get new lastexecutionTime...
                        var maxItem = dtBIFFData.AsEnumerable().OrderByDescending(o => o.Field<DateTime>("ModifiedOn")).FirstOrDefault();
                        lastExecutionTime = maxItem.Field<DateTime>("ModifiedOn");

                    }
                }


                using (SqlConnection sqlConn = CreateSqlConnectionDeltaDatabase())
                {
                    sqlConn.Open();

                    // läser in förändrad rådata från Analysplattformens GetCardView till tabellen BIFFtravelcards
                    // Används inte. Se SQL-kommandon ovan.
                    ActiveCommand = "GetTravelCardsFromBIFF1";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // läser in förändrad data till AllBIFFtravelcards
                    ActiveCommand = "GetTravelCardsFromBIFF2";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("GetTravelCardsFromBIFF2", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // Slår ihop periodkort och reskassa till en rad i AllUniqueBIFFtravelcards
                    ActiveCommand = "GetTravelCardsFromBIFF3";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("GetTravelCardsFromBIFF3", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // Lägger till Offering i AllUniqueBIFFtravelcards
                    ActiveCommand = "GetTravelCardsFromBIFF4";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("GetTravelCardsFromBIFF4", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // läser in data från CRMs FilteredCgi_Travelcard till CRMtravelcards baserat på de rader som är ändrade i CRM och de rader som ändrade i Analysplattformen
                    ActiveCommand = "GetTravelCardsFromCRM";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("GetTravelCardsFromCRM", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    /* De periodkort som finns i CRM men saknas i BIFF läggs i TravelCardError
                     * info (JoAn) - No support for delete.
                    using (var command = new SqlCommand("CheckMissingInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        command.ExecuteNonQuery();
                    }
                    */

                    // De periodkort som har annan Blocked-informationen läggs i TravelCardDiff, markerade med DiffBlocked = 1
                    ActiveCommand = "CheckBlockedInBIFF";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("CheckBlockedInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // De periodkort som har annan Offering-informationen läggs i TravelCardDiff, markerade med DiffOffering = 1
                    ActiveCommand = "CheckOfferingInBIFF";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("CheckOfferingInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    //De periodkort som har annan Valid-informationen läggs i TravelCardDiff, markerade med DiffValid = 1
                    ActiveCommand = "CheckValidDatesInBIFF";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("CheckValidDatesInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    // De periodkort som har annan Active-informationen läggs i TravelCardDiff, markerade med DiffActive = 1
                    ActiveCommand = "CheckActiveInBIFF";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand("CheckActiveInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    /* info - JOAN - Not to be used. Uses the statusCode and this is not needed
                    using (var command = new SqlCommand("CheckActivedInBIFF", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        command.ExecuteNonQuery();
                    }
                    */

                    sqlConn.Close();
                }
            }
            catch (Exception e)
            {
                string errorMessage = $"Error from SQL during processing of step {ActiveCommand}. Message:{e.Message}";

                WriteToBiffIntegrationLog(localContext, errorMessage);
                throw;
            }
            _log.Debug($"Done executing SQL commands");

        }

        /// <summary>
        /// Builds a connection to the database.
        /// </summary>
        /// <returns></returns>
        private SqlConnection CreateSqlConnectionDeltaDatabase()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Properties.Settings.Default.CRMTravelCardDBServerName;
            builder.ConnectTimeout = 300;
            builder.InitialCatalog = Properties.Settings.Default.CRMTravelCardDBName;

            if (Properties.Settings.Default.CRMTravelCardDBUseIntegratedSecurity == true)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                string passw = CrmConnection.LoadCredentials(BiffIntegration.CredentialSQLFilePath, BiffIntegration.Entropy);
                builder.UserID = Properties.Settings.Default.CRMTravelCardDBUserName;
                builder.Password = passw;
                builder.IntegratedSecurity = false;
            }

            return new SqlConnection(builder.ToString());
        }

        private SqlConnection CreateSqlConnectionBIFFDatabase()
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = Properties.Settings.Default.BIFFDBServerName;
            builder.ConnectTimeout = 300;
            builder.InitialCatalog = Properties.Settings.Default.BIFFDBName;

            if (Properties.Settings.Default.CRMTravelCardDBUseIntegratedSecurity == true)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                throw new NotImplementedException("Support for password auth to BIFF database");
                //string passw = CrmConnection.LoadCredentials(BiffIntegration.CredentialSQLFilePath, BiffIntegration.Entropy);
                //builder.UserID = Properties.Settings.Default.CRMTravelCardDBUserName;
                //builder.Password = passw;
                //builder.IntegratedSecurity = false;
            }
            //_log.Debug($"Got connectionstring to DW:{builder.ToString()}");

            return new SqlConnection(builder.ToString());
        }


        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(BiffIntegration.CredentialFilePath, BiffIntegration.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        /// <summary>
        /// Converts retreived dateTime from CRM and caches the value to increase performance
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="dtToAdjust"></param>
        /// <returns></returns>
        private DateTime AdjustDateTimeForTimeZone(Plugin.LocalPluginContext localContext, DateTime dtToAdjust)
        {
            // Quick fix to get dateTime without timezone info...
            return (DateTime)(dtToAdjust.AddHours(4).Date);

            //if (AdjustTimezone == null)
            //{
            //    int? TimeZoneCode = 110;            // Default to Stockholm

            //    // Get from executing user
            //    if (localContext.executeAsUserId != Guid.Empty)
            //        TimeZoneCode = GetTimeZoneCodeForUser(localContext, new EntityReference(SystemUserEntity.EntityLogicalName, localContext.executeAsUserId));

            //    // Convert
            //    var request = new LocalTimeFromUtcTimeRequest {
            //                        TimeZoneCode = (int)TimeZoneCode,  
            //                        UtcTime = dtToAdjust
            //                     };
            //    var response = (LocalTimeFromUtcTimeResponse)localContext.OrganizationService.Execute(request);

            //    // Save in global to avoid many requests..
            //    AdjustTimezone = (response.LocalTime - dtToAdjust);

            //    return response.LocalTime;
            //}
            //else
            //{
            //    return dtToAdjust.Add((TimeSpan)AdjustTimezone);
            //}
        }

        public static int? GetTimeZoneCodeForUser(Plugin.LocalPluginContext localContext, EntityReference systemUser)
        {
            UserSettingsEntity userSettings = XrmRetrieveHelper.Retrieve<UserSettingsEntity>(
                localContext,
                systemUser.Id,
                new ColumnSet(UserSettingsEntity.Fields.TimeZoneCode)
            );
            return userSettings.TimeZoneCode;
        }


    }
}
