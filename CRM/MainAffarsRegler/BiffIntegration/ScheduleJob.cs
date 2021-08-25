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

        private DateTime _lastExecutionTime = DateTime.MinValue;
        private long _lastExecutionMaxCardNumber = 0;
        private List<Tuple<DataRow, Exception>> _errorRows = null;
        private ColumnSet travelCardFields = new ColumnSet(
            TravelCardEntity.Fields.cgi_travelcardnumber,
            TravelCardEntity.Fields.cgi_value_card_type,                // Klartext
            TravelCardEntity.Fields.cgi_periodic_card_type,            // Klartext
            TravelCardEntity.Fields.statecode,
            // Period
            TravelCardEntity.Fields.ed_BlockedDate,
            TravelCardEntity.Fields.ed_BlockedDescription,
            // Reskassa
            TravelCardEntity.Fields.ed_BlockedDateValue,
            TravelCardEntity.Fields.ed_BlockedDescriptionValue,

            TravelCardEntity.Fields.cgi_ValidFrom,
            TravelCardEntity.Fields.cgi_ValidTo,
            TravelCardEntity.Fields.ed_LastTravelDate,
            TravelCardEntity.Fields.ed_ModifiedInBiff,
            
            TravelCardEntity.Fields.ed_BlockedStatus,
            TravelCardEntity.Fields.st_SaldoReskassa,
            TravelCardEntity.Fields.cgi_PeriodCardTypeId,
            TravelCardEntity.Fields.cgi_ValueCardTypeId
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
                               //",[Blocked] " +
                               ",case when [BlockedDate] <> '1899-12-30' then [BlockedDate] else null end as 'BlockedDate' " +
                               ",NULLIF([BlockedDescription],'') as 'BlockedDescription' " +
                               // Reskassa
                               //",[BlockedValue] " +
                               ",case when[BlockedDateValue] <> '1899-12-30' then[BlockedDateValue] else null end as 'BlockedDateValue' " +
                               ",NULLIF([BlockedDescriptionValue],'') as 'BlockedDescriptionValue' " +
                               // Hela kortet spärrat
                               //",[BlockedCard]  " +
                               //",case when[BlockedDateCard] <> '1899-12-30' then[BlockedDateCard] else null end as 'BlockedDateCard'  " +
                               //",NULLIF([BlockedDescriptionCard],'') as 'BlockedDescriptionCard'  " +

                               ",case when [ValidFromDate] <> '1899-12-30' then [ValidFromDate] else null end as 'ValidFromDate' " +
                               ",case when [ValidToDate] <> '1899-12-30' then [ValidToDate] else null end as 'ValidToDate' " +
                               ",case when [LatestTravelDate] <> '1899-12-30' then [LatestTravelDate] else null end as 'LatestTravelDate' " +
                               ",CONVERT(date, [ModifiedDate]) as 'ModifiedDate' " +
                               ",[ModifiedDate] as 'ModifiedOn' " +
                               ",[BlockedStatus] as 'BlockedStatus' " +
                               ",[Balance] as 'st_saldoreskassa' " +
                               ",[TicketCodeNumber] " +
                               ",[TicketCodeNumberValue] " +
                               " from dbo.TravelCardDiff order by [ModifiedDate] "
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
            int iExceptionCards = 0;
            int iUpdatedCards = 0;
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
                            //_log.Debug($"Processing field:{field}");

                            // Not used in update, separate SetStateRequest.
                            if ("statecode".Equals(field))
                                continue;

                            // Edit: Marcus Stenswed
                            // If Travelcard number, do not update since zeros (0) is not included and will update wrongly in CRM
                            if ("cgi_travelcardnumber".Equals(field))
                                continue;

                            switch (fromDiffTable.Contains(field) && fromDiffTable[field] != null ? fromDiffTable[field].GetType().ToString() : "null")
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

                                case "System.Decimal":

                                    //_log.Debug($"Decimal: fromDiffTable:{ ((decimal)fromDiffTable[field])}. existing:{((decimal)existingTravelCardInCRM[field])}");

                                    if (existingTravelCardInCRM.Contains(field) == false || ((decimal)fromDiffTable[field]) != ((decimal)existingTravelCardInCRM[field]))
                                    {
                                        updateEntity[field] = fromDiffTable[field];
                                        update = true;
                                    }
                                    break;

                                case "Microsoft.Xrm.Sdk.OptionSetValue":
                                    if (existingTravelCardInCRM.Contains(field) == false || ! ((OptionSetValue)fromDiffTable[field]).Equals((OptionSetValue)existingTravelCardInCRM[field]))
                                    {
                                        updateEntity[field] = fromDiffTable[field];
                                        update = true;
                                    }
                                    break;

                                    
                                default:
                                    throw new NotImplementedException($"Switch statement is missing types. Working with field:{field} of type:{fromDiffTable[field].GetType().ToString()}");
                            }
                        }


                        // Update only changed columns!
                        if (update)
                        {
                            // is record inactive now?
                            // Activate to be able to update
                            if (existingTravelCardInCRM.statecode == Generated.cgi_travelcardState.Inactive)
                            {
                                SetStateRequest req = new SetStateRequest()
                                {
                                    EntityMoniker = existingTravelCardInCRM.ToEntityReference(),
                                    State = new OptionSetValue((int)Generated.cgi_travelcardState.Active),
                                    Status = new OptionSetValue((int)Generated.cgi_travelcard_statuscode.Active)
                                };
                                localContext.OrganizationService.Execute(req);
                            }


                            // Do debug logging
                            //Trace(fromDiffTable, localContext.TracingService);
                            //Trace(existingTravelCardInCRM, localContext.TracingService);
                            //Trace(updateEntity, localContext.TracingService);


                            XrmHelper.Update(localContext, updateEntity);
                            iUpdatedCards++;

                            // Set back to inactive...
                            if (existingTravelCardInCRM.statecode == Generated.cgi_travelcardState.Inactive)
                            {
                                SetStateRequest req = new SetStateRequest()
                                {
                                    EntityMoniker = existingTravelCardInCRM.ToEntityReference(),
                                    State = new OptionSetValue((int)Generated.cgi_travelcardState.Inactive),
                                    Status = new OptionSetValue((int)Generated.cgi_travelcard_statuscode.Inactive)
                                };
                                localContext.OrganizationService.Execute(req);
                            }
                        }
                    }
                    else
                        iCardMissningInCRM ++;

                    // Update last exec time
                    if (iExceptionCards == 0)
                    {
                        if (_lastExecutionTime < row.Field<DateTime>("ModifiedOn"))
                            _lastExecutionTime = row.Field<DateTime>("ModifiedOn");
                    }
                }
                catch (Exception e)
                {
                    // Write to log file too
                    _log.Debug($"Exeption catched in ImportTravelCardDiff. Ex:{e.Message}");

                    iExceptionCards++;
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

            // If all are error, do not set lastRunTime
            if (iProcessedCards != iExceptionCards) { 
                // Update with time executed to....
                MarkLastRunTime("GetCardView", _lastExecutionTime, _lastExecutionMaxCardNumber);
            }
            // Info - JoAn -Always write a log
            if (_errorRows.Count >= 0)
            {
                string Message = $"Processed:{iProcessedCards}, Updated:{iUpdatedCards} travel cards. {iCardMissningInCRM} missing in CRM and {iExceptionCards} with Error(s).";

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


                //BiffIntegrationErrorLogEntity errorLog = new BiffIntegrationErrorLogEntity()
                //{
                //    ed_name = $"Biff Integration Log - {DateTime.Now.ToString("yyyy/MM/dd - HH:mm")}"
                //};

                //// Cut if string to long.
                //if (Message != null)
                //    if (Message.Length > BiffIntegrationErrorLogEntity.FieldLengths.ed_Statistics)
                //        errorLog.ed_Statistics = Message.Substring(0, BiffIntegrationErrorLogEntity.FieldLengths.ed_Statistics);
                //    else
                //        errorLog.ed_Statistics = Message;


                //errorLog.Id = XrmHelper.Create(localContext, errorLog);
                //errorLog.ed_biffintegrationerrorlogId = errorLog.Id;


                // Need to log rows?
                if(_errorRows != null)
                {
                    _log.Error($"Found {_errorRows.Count} error rows. Starting to log...");

                    //foreach (Tuple<DataRow, Exception> tuple in _errorRows)
                    //{
                    //    int? iNull = null;
                    //    BiffIntegrationErrorLogRowEntity errRow = new BiffIntegrationErrorLogRowEntity();

                    //    errRow.ed_BiffIntegrationErrorLog = errorLog.ToEntityReference();
                    //    errRow.ed_CardNumber = tuple.Item1.Field<long>("CardNumber").ToString();
                    //    errRow.ed_CardOfferingReskassa = tuple.Item1.IsNull("CardOfferingReskassa") == false ? tuple.Item1.Field<string>("CardOfferingReskassa").ToString() : null;
                    //    errRow.ed_CardOfferingPeriodkort = tuple.Item1.IsNull("CardOfferingPeriodkort") == false ? tuple.Item1.Field<string>("CardOfferingPeriodkort").ToString() : null;
                    //    errRow.ed_IsActive = tuple.Item1.IsNull("IsActive") == false ?  tuple.Item1.Field<int>("IsActive") : iNull;

                    //    // Period
                    //    //errRow.ed_Blocked = tuple.Item1.IsNull("Blocked") == false ?  tuple.Item1.Field<int?>("Blocked") : null;
                    //    errRow.ed_BlockedDate = tuple.Item1.IsNull("BlockedDate") == false ? tuple.Item1.Field<DateTime?>("BlockedDate") : null;
                    //    errRow.ed_BlockedDescription = tuple.Item1.IsNull("BlockedDescription") == false ? tuple.Item1.Field<string>("BlockedDescription") : null;
                    //    //
                    //    errRow.ed_ValidFromDate = tuple.Item1.IsNull("ValidFromDate") == false ? tuple.Item1.Field<DateTime?>("ValidFromDate") : null;
                    //    errRow.ed_ValidToDate = tuple.Item1.IsNull("ValidToDate") == false ? tuple.Item1.Field<DateTime?>("ValidToDate") : null;
                    //    errRow.ed_LatestTravelDate = tuple.Item1.IsNull("LatestTravelDate") == false ? tuple.Item1.Field<DateTime?>("LatestTravelDate") : null;
                    //    errRow.ed_ModifiedDate = tuple.Item1.Field<DateTime>("ModifiedDate");

                    //    // Add errormessage
                    //    errRow.ed_ErrorMessage = $"Error:{tuple.Item2.Message}, inner:{(tuple.Item2.InnerException != null ? tuple.Item2.InnerException.Message :string.Empty )}";
                    //    if (errRow.ed_ErrorMessage.Length > BiffIntegrationErrorLogRowEntity.FieldLengths.ed_ErrorMessage)
                    //        errRow.ed_ErrorMessage = errRow.ed_ErrorMessage.Substring(0, BiffIntegrationErrorLogRowEntity.FieldLengths.ed_ErrorMessage);

                    //    XrmHelper.Create(localContext, errRow);
                    //}

                    _log.Error($"Done writing to log");
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
            // Temporary loggning
            if(DateTime.Now < DateTime.Parse("2018-10-30"))
                _log.Debug($"MatchColumnValueToCrmEntity. Column:{column.ColumnName}");


            switch (column.ColumnName)
            {
                case "CardNumber":
                    fromDiffTable.cgi_travelcardnumber = row.Field<long>(column.ColumnName).ToString();
                    break;
                case "CardOfferingReskassa":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_value_card_type = row.Field<string>(column.ColumnName);
                    break;
                case "CardOfferingPeriodkort":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_periodic_card_type = row.Field<string>(column.ColumnName);
                    break;

                //case "OfferingName":
                //    fromDiffTable.ed_OfferingName = row.Field<string>(column.ColumnName);
                //    break;
                case "IsActive":
                    // Do nothing - Not to be used.
                    //if (row.Field<int>(column.ColumnName) == 0)
                    //    fromDiffTable[Generated.cgi_travelcard.Fields.statecode] = new OptionSetValue((int)Generated.cgi_travelcardState.Active);
                    //else if (row.Field<int>(column.ColumnName) == 1)
                    //    fromDiffTable[Generated.cgi_travelcard.Fields.statecode] = new OptionSetValue((int)Generated.cgi_travelcardState.Inactive);
                    break;
                case "BlockedDate":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDate = row.Field<DateTime>(column.ColumnName);
                    break;
                //case "BlockedDateCard":
                //    if (!row.IsNull(column.ColumnName))
                //        fromDiffTable.ed_BlockedDateCard = row.Field<DateTime>(column.ColumnName);
                //    break;
                case "BlockedDescription":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDescription = row.Field<string>(column.ColumnName);
                    break;
                case "BlockedDateValue":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDateValue = row.Field<DateTime>(column.ColumnName);
                    break;
                case "BlockedDescriptionValue":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedDescriptionValue = row.Field<string>(column.ColumnName);
                    break;
                case "BlockedStatus":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.ed_BlockedStatus = new OptionSetValue(row.Field<int>(column.ColumnName));
                    break;
                //case "BlockedDescriptionCard":
                //    if (!row.IsNull(column.ColumnName))
                //        fromDiffTable.ed_BlockedDescriptionCard = row.Field<string>(column.ColumnName);
                //    break;
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
                case "ModifiedOn":
                    // Do nothing used for sorting etc.
                    break;
                case "st_saldoreskassa":
                    fromDiffTable.st_SaldoReskassa = row.Field<decimal>(column.ColumnName);
                    break;
                case "TicketCodeNumber":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_PeriodCardTypeId = row.Field<int>(column.ColumnName);
                    break;
                case "TicketCodeNumberValue":
                    if (!row.IsNull(column.ColumnName))
                        fromDiffTable.cgi_ValueCardTypeId = row.Field<int>(column.ColumnName);
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



                    // Get latest exec info
                    ActiveCommand = "SELECT TOP (1) [LatestModifiedDate], [MaxCardNumber] FROM [dbo].[LoadDates] WHERE [Source] = 'GetCardView' order by [LatestModifiedDate] desc, [MaxCardNumber] desc";
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
                                _lastExecutionTime = reader.GetDateTime(0);
                                _lastExecutionMaxCardNumber = reader.GetInt64(1);
                                _log.Debug($"Got last exec time:{_lastExecutionTime} and cardnumber:{_lastExecutionMaxCardNumber}");
                                break;
                            }
                        }
                    }
                    sqlConn.Close();
                }

                // Run code for step1, GetTravelCardsFromBIFF1
                ActiveCommand = "Connecting to BIFF..";
                using (SqlConnection sqlBiffConn = CreateSqlConnectionBIFFDatabase())
                {
                    sqlBiffConn.Open();

                    // Get data in shunks...
                    int iMaxRecordsToProcess = 4000000;
                    int iRetreivedRecords = 0;
                    int iGetRecordsPerBatch  =  200000;
                    long iMaxRetreivedCardNumber = _lastExecutionMaxCardNumber;
                    DateTime lastRetreivedModifiedOn = DateTime.MinValue;
                    int iModifiedOnCount = 0;
                    while (true)
                    {
                        DataTable dtBIFFData = new DataTable();

                        // Records in GetCardView are grouped on ModifiedOn date. All records for a day are updated at the same time...
                        // To load the system (from empty) there are to many records and the processing will take to many hours to complete.
                        // This problem is solved by getting one unique date and per fetch and to build the store until iMaxRecordsToProcess is reached.
                        // Example:
                        // ModifiedOn              Count
                        // 1980 - 01 - 01 00:00    1
                        // 2016 - 06 - 27 16:19    7840718          To many to handle in one go! Split into separate sessions
                        // 2016 - 06 - 28 01:00    525
                        // 2016 - 06 - 29 01:00    1792
                        // 2016 - 06 - 30 01:00    1551
                        // 2016 - 07 - 01 01:00    1654
                        // 2016 - 07 - 02 01:00    1693
                        // 2016 - 07 - 03 01:00    991
                        // 2016 - 07 - 04 01:00    869
                        // *****************************************************************************************************************************  

                        // Get next unique modified on and the expected count.
                        {
                            StringBuilder query = new StringBuilder($"SELECT TOP(1) [ModifiedOn],  Count(*) ");
                            query.Append($"    FROM [{Properties.Settings.Default.BIFFDBName}].[dbo].[GetCardView] ");
                            query.Append($"   WHERE [ModifiedOn] >= '{_lastExecutionTime}' ");
                            query.Append($"     AND cast([CardNumber] as bigint) > {iMaxRetreivedCardNumber} ");
                            query.Append($"   group by [ModifiedOn] ");
                            query.Append($"   order by [ModifiedOn] asc");
                            ActiveCommand = query.ToString();
                            _log.Debug($"Running SQL command in DW database: {ActiveCommand}");
                            SqlCommand cmd = new SqlCommand(ActiveCommand, sqlBiffConn);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Data found?
                                if(reader.Read())
                                {
                                    lastRetreivedModifiedOn = reader.GetDateTime(0);
                                    iModifiedOnCount = reader.GetInt32(1);
                                    _log.Debug($"Got ModifiedOn:{lastRetreivedModifiedOn} and count:{iModifiedOnCount}");
                                }
                                else
                                {
                                    // No more data to get...
                                    _lastExecutionMaxCardNumber = 0;
                                    break;
                                }
                            }
                        }
                        
                        {
                            StringBuilder query = new StringBuilder($"SELECT TOP({iGetRecordsPerBatch}) [Activated],  [BlockDate], ");
                            query.Append("           [BlockDescription], [BlockState], ");
                            query.Append("           cast([CardNumber] as bigint) as CardNumber, ");
                            query.Append("           [CardSection], ");
                            query.Append("           [ExpireDay],  [IsActive], ");
                            query.Append("           [LatestTravelDate], [ModifiedOn], ");
                            query.Append("           [OfferingName], ");
                            query.Append("           [PeriodStartDay], [TicketCodeNumber], ");
                            query.Append("           [IsPassBlock], [IsPurseBlock], [IsCardBlock], ");
                            query.Append("           [Balance] ");
                            query.Append($"     FROM [{Properties.Settings.Default.BIFFDBName}].[dbo].[GetCardView] M");
                            query.Append($"    WHERE cast(M.[CardNumber] as bigint) > {iMaxRetreivedCardNumber} ");
                            query.Append($"     AND (M.[ModifiedOn] = '{lastRetreivedModifiedOn}' ");
                            // Make sure we add the card sections that are not included in this modified on. Must be handled in pairs
                            query.Append($"          OR (");
                            query.Append($"              EXISTS (SELECT 1 FROM [dbo].[GetCardView] AS O");
                            query.Append($"              WHERE M.CardNumber = O.CardNumber AND O.ModifiedOn = '{lastRetreivedModifiedOn}')))");
                            query.Append($"   order by M.[CardNumber] ");

                            ActiveCommand = query.ToString();
                            _log.Debug($"Running SQL command in DW database: {ActiveCommand}");
                            SqlCommand cmd = new SqlCommand(ActiveCommand, sqlBiffConn);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtBIFFData);
                                da.Dispose();
                            }
                        }

                        // Abort if no more rows...
                        if (dtBIFFData.Rows.Count == 0)
                        {
                            _lastExecutionMaxCardNumber = 0;
                            break;
                        }

                        iRetreivedRecords += dtBIFFData.Rows.Count;

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
                                //_log.Debug($"Done writing to destination");
                            }

                            sqlConn.Close();
                        }

                        // Get highest cardNo.
                        var maxItem = dtBIFFData.AsEnumerable().OrderByDescending(o => o.Field<long>("CardNumber")).FirstOrDefault();
                        iMaxRetreivedCardNumber = maxItem.Field<long>("CardNumber");


                        // Will this ModifiedOn Date exeed our limit?
                        if (iRetreivedRecords > iMaxRecordsToProcess)
                        {
                            _log.Debug($"iMaxRecordsToProcess limit hit");
                            // Yes, abort retreive and save values
                            _lastExecutionMaxCardNumber = iMaxRetreivedCardNumber;
                            break;
                        }
                        // Do we need to get this ModifiedOn in more than one batch?
                        else if (iModifiedOnCount < iGetRecordsPerBatch)
                        {
                            _log.Debug($"ModifiedOn date {lastRetreivedModifiedOn} complete");
                            // No, we got it all. Setup for next date.
                            _lastExecutionMaxCardNumber = 0;
                            iMaxRetreivedCardNumber = 0;                                        // new date, let's get all.
                            _lastExecutionTime = lastRetreivedModifiedOn.AddMinutes(1);         // Add a minute to make sure we get next date.
                        }
                        else
                        {
                            _log.Debug($"New iteration on ModifiedOn date {lastRetreivedModifiedOn}");
                        }


                    } // End while (true)

                    sqlBiffConn.Close();
                }


                using (SqlConnection sqlConn = CreateSqlConnectionDeltaDatabase())
                {
                    sqlConn.Open();

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
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        , CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }

                    //De periodkort som har förändrad reskassa -informationen läggs i TravelCardDiff, markerade med DiffValid = 1
                    ActiveCommand = "CheckTravelBalanceInBIFF";
                    _log.Debug($"Running SQL command: {ActiveCommand}");
                    using (var command = new SqlCommand(ActiveCommand, sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                        ,
                        CommandTimeout = 0
                    })
                    {
                        command.ExecuteNonQuery();
                    }


                    // De periodkort som har annan Active-informationen läggs i TravelCardDiff, markerade med DiffActive = 1
                    /* into - JOAN. Not to be used according to HåkanT 171120.
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
                    */
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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lastExecTime"></param>
        private void MarkLastRunTime(string type, DateTime lastExecTime, long MaxCardNumber)
        {
            // Should we update?
            if (lastExecTime == DateTime.MinValue)
                return;

            using (SqlConnection sqlConn = CreateSqlConnectionDeltaDatabase())
            {
                sqlConn.Open();

                string ActiveCommand = $"insert into loaddates ([Source], [LatestModifiedDate], [MaxCardNumber]) Values (@Source, @LatestModifiedDate, @MaxCardNumber)";
                _log.Debug($"Running SQL command: {ActiveCommand}");
                using (var command = new SqlCommand(ActiveCommand, sqlConn)
                {
                    CommandType = CommandType.Text,
                    CommandTimeout = 0
                })
                {
                    command.Parameters.AddWithValue("@Source", type);
                    command.Parameters.AddWithValue("@LatestModifiedDate", lastExecTime);
                    command.Parameters.AddWithValue("@MaxCardNumber", MaxCardNumber);
                    command.ExecuteNonQuery();
                }

                sqlConn.Close();

            }
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
            string password = "uSEme2!nstal1";
            var securePassword = new System.Security.SecureString();
            foreach (char c in password)
                securePassword.AppendChar(c);

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

        public static void Trace(Entity entity, ITracingService tracingService)
        {
            try
            {
                if (tracingService == null)
                {
                    return;
                }

                if (entity == null)
                {
                    tracingService.Trace("\nTrace()\nEntity is null");
                    return;
                }

                if (entity.Attributes == null)
                {
                    tracingService.Trace("\nTrace()\nEntity has no attributes");
                    return;
                }

                StringBuilder traceString = new StringBuilder();
                traceString.AppendLine();
                traceString.AppendLine("----------" + entity.LogicalName + "----------");
                foreach (KeyValuePair<string, object> attribute in entity.Attributes)
                {
                    try
                    {
                        traceString.Append("- " + attribute.Key + " = ");
                        if (attribute.Value == null)
                            traceString.Append("null;");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.EntityReference)
                            traceString.Append("(EntityReference) " + ((EntityReference)attribute.Value).LogicalName + " (" + ((EntityReference)attribute.Value).Id + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                            traceString.Append("(OptionSetValue) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.Money)
                            traceString.Append("(Money) " + "(" + ((Money)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                            traceString.Append("(Entity) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                        else if (attribute.Value is EntityCollection)
                            traceString.Append("(EntityCollection) " + ((EntityCollection)attribute.Value).EntityName + " (" + ((EntityCollection)attribute.Value).Entities.Count() + ")");
                        else
                            traceString.Append("(" + attribute.Value.GetType() + ") " + attribute.Value.ToString());
                        traceString.AppendLine(string.Empty);
                    }
                    catch (Exception ex)
                    {
                        traceString.AppendLine(ex.Message);
                    }
                }
                traceString.AppendLine("--------------------------------------");
                tracingService.Trace(traceString.ToString());
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Failed to trace entity: " + ex.Message);
            }
        }

    }
}
