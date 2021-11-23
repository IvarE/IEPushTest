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
using Renci.SshNet;
using Renci.SshNet.Common;
//using Tamir.SharpSsh;

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class UploadJob : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Upload Schedule";
        public const string TriggerDescription = "Upload Schedule Trigger";
        public const string JobDescription = "Upload Schedule Job";
        public const string TriggerName = "UploadTrigger";
        public const string JobName = "UploadJob";

        //protected Sftp sftp;
        protected SftpClient sftpClient;
        private string plusFileName;
        private string minusFileName;

        private ILog _log = LogManager.GetLogger(typeof(UploadJob));

        public void Execute(IJobExecutionContext context)
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
            Plugin.LocalPluginContext localContext = null;
            try
            {
                localContext = DeltabatchJobHelper.GenerateLocalContext();

                _log.Info($"Fetching max {Properties.Settings.Default.DeltabatchQueueCount} queues");
                IList<DeltabatchQueueEntity> currentQueues = FetchABatchOfActiveQueuePosts(localContext);

                _log.Info($"Generating Files for {currentQueues.Count} queue posts");
                GenerateFiles(localContext, currentQueues);

                _log.Info($"Upload Files");
                UploadFiles();

                _log.Info($"Deleting used queues");
                DeleteQueuePosts(localContext, currentQueues);
                
                _log.Info($"UploadJob Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ExecuteJob():\n{e.Message}\n\n{e}");
                //if (localContext != null)
                DeltabatchJobHelper.SendErrorMailToDev(localContext, e);
            }
        }

        //public IList<DeltabatchQueueEntity> FetchAllActiveQueuePosts(Plugin.LocalPluginContext localContext)
        //{
        //    return XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, DeltabatchJobHelper.deltabatchQueueColumnSet,
        //        new FilterExpression
        //        {
        //            Conditions =
        //            {
        //                new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
        //            }
        //        });
        //}

        public IList<DeltabatchQueueEntity> FetchABatchOfActiveQueuePosts(Plugin.LocalPluginContext localContext)
        {
            QueryExpression batchQuery = new QueryExpression
            {
                EntityName = DeltabatchQueueEntity.EntityLogicalName,
                ColumnSet = DeltabatchJobHelper.deltabatchQueueColumnSet,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(DeltabatchQueueEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_DeltabatchQueueState.Active)
                    }
                }
                //TopCount = Properties.Settings.Default.DeltabatchQueueCount
            };

            // Assign the pageinfo properties to the query expression.
            batchQuery.PageInfo = new PagingInfo();
            batchQuery.PageInfo.Count = 5000;
            batchQuery.PageInfo.PageNumber = 1;
            
            // The current paging cookie. When retrieving the first page, 
            // pagingCookie should be null.
            batchQuery.PageInfo.PagingCookie = null;

            return XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, batchQuery).Take(Properties.Settings.Default.DeltabatchQueueCount).ToList();
        }

        public void GenerateFiles(Plugin.LocalPluginContext localContext, IList<DeltabatchQueueEntity> currentQueues)
        {
            StringBuilder plusFileBuilder = new StringBuilder();
            StringBuilder minusFileBuilder = new StringBuilder();
            try
            {
                _log.Info($"Send File Path is: {Properties.Settings.Default.DeltabatchSendFileLocation}");

                List<DeltabatchQueueEntity> list = new List<DeltabatchQueueEntity>();
                foreach (DeltabatchQueueEntity dbq in currentQueues)
                {
                    list.Add(dbq);
                }

                if (list.Count == 0)
                {
                    _log.Info($"No DeltabatchQueue Records were found.");
                    return;
                }

                list.RemoveAll(r => null == r.ed_ContactNumber);


                List<DeltabatchQueueEntity> distinctList = list.GroupBy(x => x.ed_ContactNumber).Select(f => f.First()).ToList();
                List<DeltabatchQueueEntity> sortedList = distinctList.OrderByDescending(dbq => dbq.CreatedOn).ToList<DeltabatchQueueEntity>();
                List<string> processedSocSecNumbers = new List<string>();
                
                foreach (DeltabatchQueueEntity q in sortedList)
                {
                    // Do not add if SocSecNr already processed
                    if (processedSocSecNumbers.Contains(q.ed_ContactNumber) || !CustomerUtility.CheckPersonnummerFormat(q.ed_ContactNumber))
                        continue;

                    string fritext = q.ed_name.Split(":".ToCharArray())[0].Replace(" ","").Replace("-", "");
                    if (Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus.Equals(q.ed_DeltabatchOperation))
                    {
                        plusFileBuilder.AppendLine($"{q.ed_ContactNumber};{q.ed_ContactGuid};{fritext}");
                    }
                    else if (Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus.Equals(q.ed_DeltabatchOperation))
                    {
                        minusFileBuilder.AppendLine($"{q.ed_ContactNumber};{q.ed_ContactGuid};{fritext}");
                    }
                    else
                    {
                        throw new Exception($"Unrecognised DeltabatchOperation value {q.ed_DeltabatchOperation}");
                    }
                    processedSocSecNumbers.Add(q.ed_ContactNumber);
                }

                // Write Plus File
                plusFileName = $"{Properties.Settings.Default.DeltabatchSendFileLocation}\\{Properties.Settings.Default.PlusFileName}{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':','-')}.txt";
                if (File.Exists(plusFileName))
                {
                    throw new Exception($"File {plusFileName} already exists.");
                    //string plusFileDatedPath = plusFileName + "_";
                    //System.IO.File.Move(plusFileName, plusFileDatedPath);
                }
                string plusString = plusFileBuilder.ToString();
                if (string.IsNullOrWhiteSpace(plusString))
                    plusString = GenerateAPlusFileString(localContext);
                using (System.IO.StreamWriter plusFile = new System.IO.StreamWriter(plusFileName))
                {
                    plusFile.WriteLine(plusString);
                    plusFile.Close();
                }
                
                // Write Minus File
                minusFileName = $"{Properties.Settings.Default.DeltabatchSendFileLocation}\\{Properties.Settings.Default.MinusFileName}{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':', '-')}.txt";
                if (File.Exists(minusFileName))
                {
                    throw new Exception($"File {minusFileName} already exists.");
                    //string plusFileDatedPath = plusFileName + "_";
                    //System.IO.File.Move(plusFileName, plusFileDatedPath);
                }
                string minusString = minusFileBuilder.ToString();
                using (System.IO.StreamWriter minusFile = new System.IO.StreamWriter(minusFileName))
                {
                    minusFile.WriteLine(minusString);
                    minusFile.Close();
                }
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in GenerateFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
        }

        private string GenerateAPlusFileString(Plugin.LocalPluginContext localContext)
        {
            ContactEntity c = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull),
                        new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                        new ConditionExpression(ContactEntity.Fields.ed_HasSwedishSocialSecurityNumber, ConditionOperator.Equal, true),
                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull)
                    }
                });
            return $"{c.cgi_socialsecuritynumber};{c.Id.ToString()};ToAvoidEmpyFile";
        }

        public void DeleteQueuePosts(Plugin.LocalPluginContext localContext, IList<DeltabatchQueueEntity> currentQueues)
        {
            _log.Info($"Entered DeleteQueuePosts");
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
            _log.Info($"Entered UploadFiles");


            //sftp = null;
            sftpClient = null;
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

                sftpClient = DeltabatchJobHelper.CreateSftpConnectionToCreditsafe(_log);
                sftpClient.Connect();
                if (!sftpClient.IsConnected)
                    throw new Exception($"Unable to connect to sftp-server");
                _log.Info("Connected to SFTP");

                sftpClient.ChangeDirectory("/INFILE/");
                

                //sftpClient.Put(plusFileName, "/INFILE");
                //sftpClient.Put(minusFileName, "/INFILE");

                //var fileTest = new FileStream(plusFileName, FileMode.Open);
                //sftpClient.UploadFile(fileTest, plusFileName, true, null);

                

                using (var fileStream = new FileStream(plusFileName,FileMode.Open))
                {
                    sftpClient.UploadFile(fileStream, Path.GetFileName(plusFileName)); 
                }

                

                using (var fileStream = new FileStream(minusFileName, FileMode.Open))
                {
                    sftpClient.UploadFile(fileStream, Path.GetFileName(minusFileName)); 
                }



                //using (var uplfileStream = System.IO.File.OpenRead(plusFileName))
                //{
                //    sftpClient.UploadFile(uplfileStream, @"\INFILE\" + plusFileName); // + plusFileName

                //    //sftpClient.UploadFile(uplfileStream, plusFileName, true);
                //}

                //using (var uplfileStream = System.IO.File.OpenRead(minusFileName))
                //{
                //    sftpClient.UploadFile(uplfileStream, "/INFILE/" + minusFileName, true);
                //}

                //ArrayList fileList = sftp.GetFileList("/INFILE");
                sftpClient.Disconnect();

                File.Move(plusFileName, plusFileName.Insert(plusFileName.IndexOf(Properties.Settings.Default.PlusFileName), "History\\"));
                File.Move(minusFileName, minusFileName.Insert(minusFileName.IndexOf(Properties.Settings.Default.MinusFileName), "History\\"));
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in UploadFiles():\n{e.Message}\n\n{e}");
                throw e;
            }
            finally
            {
                //Close sftp
                try { sftpClient.Disconnect(); }
                catch { }

                try { sftpClient = null; }
                catch { }

                try { GC.Collect(); }
                catch { }

            }
        }
    }
}
