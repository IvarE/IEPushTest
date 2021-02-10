using Common.Logging;
using Microsoft.Xrm.Sdk.Query;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Linq;
using System.Collections;
using Skanetrafiken.Crm;
using Microsoft.Crm.Sdk.Messages;

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class UploadJob : DeltabatchJob
    {
        public const string JobName = "UploadJob";

        private string plusFileName;
        private string minusFileName;

        new private ILog _log = LogManager.GetLogger(typeof(UploadJob));

        new public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));
            
            ExecuteJob();

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        public void ExecuteJob()
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateLocalContext();

                IList<DeltabatchQueueEntity> currentQueues = FetchAllActiveQueuePosts(localContext);

                GenerateFiles(localContext, currentQueues);
                
                UploadFiles();

                DeleteQueuePosts(localContext, currentQueues);
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                throw e;
            }
        }

        public IList<DeltabatchQueueEntity> FetchAllActiveQueuePosts(Plugin.LocalPluginContext localContext)
        {
            return XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, deltabatchQueueColumnSet,
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                    }
                });
        }

        public void GenerateFiles(Plugin.LocalPluginContext localContext, IList<DeltabatchQueueEntity> currentQueues)
        {
            StringBuilder plusFileBuilder = new StringBuilder();
            StringBuilder minusFileBuilder = new StringBuilder();
            try
            {
                List<DeltabatchQueueEntity> list = new List<DeltabatchQueueEntity>();
                foreach (DeltabatchQueueEntity dbq in currentQueues)
                {
                    list.Add(dbq);
                }

                List<DeltabatchQueueEntity> sortedList = list.OrderByDescending(dbq => dbq.CreatedOn).ToList<DeltabatchQueueEntity>();
                List<string> processedSocSecNumbers = new List<string>();

                foreach (DeltabatchQueueEntity q in sortedList)
                {
                    // Do not add if SocSecNr already processed
                    if (processedSocSecNumbers.Contains(q.ed_ContactNumber) || !CustomerUtility.CheckPersonnummerFormat(q.ed_ContactNumber))
                        continue;

                    string fritext = q.ed_name.Split(":".ToCharArray())[0].Replace(" ","").Replace("-", "");
                    if (Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus.Equals(q.ed_DeltabatchOperation))
                    {
                        plusFileBuilder.AppendLine($"{q.ed_ContactNumber};{q.Id};{fritext}");
                    }
                    else if (Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus.Equals(q.ed_DeltabatchOperation))
                    {
                        minusFileBuilder.AppendLine($"{q.ed_ContactNumber};{q.Id};{fritext}");
                    }
                    else
                    {
                        throw new Exception($"Unrecognised DeltabatchOperation value {q.ed_DeltabatchOperation}");
                    }
                    processedSocSecNumbers.Add(q.ed_ContactNumber);
                }

                // Write Plus File
                plusFileName = $"{Properties.Settings.Default.DeltabatchSendFileLocation}\\{Properties.Settings.Default.PlusFileName}{DateTime.Now.ToShortDateString()}.txt";
                if (File.Exists(plusFileName))
                {
                    throw new Exception($"File {plusFileName} already exists.");
                    //string plusFileDatedPath = plusFileName + "_";
                    //System.IO.File.Move(plusFileName, plusFileDatedPath);
                }
                string plusString = plusFileBuilder.ToString();
                System.IO.StreamWriter plusFile = new System.IO.StreamWriter(plusFileName);
                plusFile.WriteLine(plusString);
                plusFile.Close();
                                
                // Write Minus File
                minusFileName = $"{Properties.Settings.Default.DeltabatchSendFileLocation}\\{Properties.Settings.Default.MinusFileName}{DateTime.Now.ToShortDateString()}.txt";
                if (File.Exists(minusFileName))
                {
                    throw new Exception($"File {minusFileName} already exists.");
                    //string plusFileDatedPath = plusFileName + "_";
                    //System.IO.File.Move(plusFileName, plusFileDatedPath);
                }
                string minusString = minusFileBuilder.ToString();
                System.IO.StreamWriter minusFile = new System.IO.StreamWriter(minusFileName);
                minusFile.WriteLine(minusString);
                minusFile.Close();
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in GenerateFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
        }

        public void DeleteQueuePosts(Plugin.LocalPluginContext localContext, IList<DeltabatchQueueEntity> currentQueues)
        {
            try
            {
                foreach (DeltabatchQueueEntity dbq in currentQueues)
                {
                    if (dbq.ed_DeltabatchQueueId != null && !Guid.Empty.Equals(dbq.ed_DeltabatchQueueId))
                    {
                        // TODO: teo - is deprecated as of 2016. Update when SDK is updated. to 6+
                        //DeltabatchQueueEntity updateEntity = new DeltabatchQueueEntity
                        //{
                        //    Id = (Guid)dbq.ed_DeltabatchQueueId,
                        //    statuscode = Generated.ed_deltabatchqueue_statuscode.Inactive
                        //};
                        //XrmHelper.Update(localContext.OrganizationService, updateEntity);
                        SetStateRequest req = new SetStateRequest
                        {
                            EntityMoniker = dbq.ToEntityReference(),
                            State = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_DeltabatchQueueState.Inactive),
                            Status = new Microsoft.Xrm.Sdk.OptionSetValue((int)Generated.ed_deltabatchqueue_statuscode.Inactive)
                        };
                        SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in DeleteQueuePosts():\n{e.Message}\n\n{e}");
                throw e;
            }
        }

        public void UploadFiles()
        {
            sftp = null;
            try
            {
                if (!File.Exists(plusFileName))
                {
                    throw new Exception($"Plus File not found at: {plusFileName}, please create and try again.");
                }
                if (!File.Exists(minusFileName))
                {
                    throw new Exception($"Minus File not found at: {minusFileName}, please create and try again.");
                }

                sftp = CreateSftpConnectionToCreditsafe();
                sftp.Connect();
                if (!sftp.Connected)
                    throw new Exception($"Unable to connect to sftp-server");
                sftp.Put(plusFileName, "/Infile");
                sftp.Put(minusFileName, "/Infile");
                //ArrayList fileList = sftp.GetFileList("/Infile");
                sftp.Close();

                File.Move(plusFileName, plusFileName.Insert(plusFileName.IndexOf(Properties.Settings.Default.PlusFileName), "History\\"));
                File.Move(minusFileName, minusFileName.Insert(minusFileName.IndexOf(Properties.Settings.Default.MinusFileName), "History\\"));
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in SendFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
            finally
            {
                //Close sftp
                try { sftp.Close(); }
                catch { }

                try { sftp = null; }
                catch { }

                try { GC.Collect(); }
                catch { }

            }
        }
    }
}
