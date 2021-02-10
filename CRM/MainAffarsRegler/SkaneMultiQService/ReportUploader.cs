using Endeavor.Crm.MultiQService.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Crm.Sdk.Messages;

namespace Endeavor.Crm.MultiQService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class ReportUploader : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Report Uploader Schedule";
        public const string TriggerDescription = "ReportUploader Schedule Trigger";
        public const string JobDescription = "ReportUploader Schedule Job";
        public const string TriggerName = "ReportUploaderTrigger";
        public const string JobName = "ReportUploader";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(OrderService.CredentialFilePath, OrderService.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
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

        public void ExecuteJob()
        {
            Plugin.LocalPluginContext localContext = null;

            try
            {
                localContext = GenerateLocalContext();

                if (localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }

                string multiQLocation = Properties.Settings.Default.MultiQStoreFiles;
                string multiQLocationArchive = Properties.Settings.Default.MultiQArchive;
                string pdfMimeType = @"application/pdf";
                string pdfFilter = "*.pdf";

                List<MultiQFile> pdfFiles = GetFileList(multiQLocation, pdfFilter);

                if (pdfFiles == null || pdfFiles.Count == 0)
                {
                    _log.Error($"There is no PDF Files in this location: " + multiQLocation);
                    return;
                }

                _log.Info($"Found " + pdfFiles.Count + " PDF files in the location: " + multiQLocation);

                List<OrderEntity> lOrders = GetAllRelevantOrders(localContext, pdfFiles);
                _log.Info($"Found " + lOrders.Count + " that have a matching file on the MultiQ Import Location.");

                foreach (OrderEntity order in lOrders)
                {
                    Guid orderId = (Guid)order.SalesOrderId;
                    string orderNumber = order.OrderNumber;

                    if(orderNumber == null)
                    {
                        _log.Error($"The order number for the order: " + orderId + " is null.");
                        continue;
                    }

                    List<MultiQFile> lFiles = pdfFiles.Where(x => x.fileName.Contains(orderNumber)).ToList();

                    if (lFiles.Count == 0)
                    {
                        _log.Info($"There is no file that contains the Order Number: " + orderNumber + " on the Shared Folder: " + multiQLocation + ".");
                        continue;
                    }
                    else if (lFiles.Count > 1)
                    {
                        _log.Info($"There is more than one file that contains the Order Number: " + orderNumber + " on the Shared Folder: " + multiQLocation + ".");
                        continue;
                    }
                    else if (lFiles.Count == 1)
                    {
                        MultiQFile file = lFiles.FirstOrDefault();

                        if (file.filePath == null)
                        {
                            _log.Error($"The file path for the MultiQ File is null.");
                            continue;
                        }

                        string base64File = DownloadFile(file.filePath);

                        if (base64File == null)
                        {
                            _log.Error($"The Base64 for the file: " + file.fileName + " is null.");
                            continue;
                        }

                        bool hasFile = CheckIfOrderAlreadyHasFile(localContext, orderId, file.fileName);

                        if (hasFile)
                        {
                            _log.Error($"The OrderId: " + orderId + " for the file: " + file.fileName + " already is updated on CRM.");
                            continue;
                        }

                        Annotation note = new Annotation();
                        note.Subject = file.fileName;
                        note.NoteText = "Generated Report file from MultiQ for this Order.";
                        note.ObjectTypeCode = OrderEntity.EntityLogicalName;
                        note.ObjectId = new EntityReference(OrderEntity.EntityLogicalName, (Guid)order.SalesOrderId);
                        note.MimeType = pdfMimeType;
                        note.FileName = file.fileNameExtension;
                        note.DocumentBody = base64File;

                        Guid idNote = XrmHelper.Create(localContext, note);
                        _log.Info($"Note: " + idNote + " with Attachment was created on Related Order: " + order.SalesOrderId);

                        bool isMoved = MoveFile(file.filePath, multiQLocationArchive + file.fileNameExtension);
                        if (isMoved)
                            _log.Info($"The file " + file + " was moved to History.");
                        else
                            _log.Info($"The file " + file + " was not moved to History. Please check the Exception for more details.");
                    }
                }

                _log.Info($"ReportUploader Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ReportUploader.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public static bool CheckIfOrderAlreadyHasFile(Plugin.LocalPluginContext localContext, Guid orderId, string fileName)
        {
            bool hasFile = false;

            QueryExpression queryAnnotation = new QueryExpression(Annotation.EntityLogicalName);
            queryAnnotation.NoLock = true;
            queryAnnotation.Criteria.AddCondition(Annotation.Fields.Subject, ConditionOperator.Equal, fileName);
            queryAnnotation.Criteria.AddCondition(Annotation.Fields.ObjectId, ConditionOperator.Equal, orderId);
            queryAnnotation.Criteria.AddCondition(Annotation.Fields.ObjectTypeCode, ConditionOperator.Equal, OrderEntity.EntityLogicalName);

            List<Annotation> lAnnotations = XrmRetrieveHelper.RetrieveMultiple<Annotation>(localContext, queryAnnotation);

            if (lAnnotations.Count == 1)
                return !hasFile;
            else if (lAnnotations.Count == 0)
                _log.Info("There was no Annotation on Order: " + orderId + " with Subject: " + fileName);
            else if (lAnnotations.Count > 1)
                _log.Info("There was more than one Annotation on Order: " + orderId + " with Subject: " + fileName);

            return hasFile;
        }

        public List<OrderEntity> GetAllRelevantOrders(Plugin.LocalPluginContext localContext, List<MultiQFile> pdfFiles)
        {
            QueryExpression queryAllRelevantOrders = new QueryExpression(OrderEntity.EntityLogicalName);
            queryAllRelevantOrders.ColumnSet = new ColumnSet(OrderEntity.Fields.SalesOrderId, OrderEntity.Fields.OrderNumber);

            FilterExpression filter = new FilterExpression();
            queryAllRelevantOrders.Criteria.AddFilter(filter);

            filter.FilterOperator = LogicalOperator.Or;
            foreach (MultiQFile file in pdfFiles)
            {
                if (file.fileName == null)
                {
                    _log.Error($"The file name for the MultiQ File is null.");
                    continue;
                }
                
                string[] words = file.fileName.Split('_');
                string orderId = words.LastOrDefault();

                if (orderId == null)
                {
                    _log.Error($"The OrderId for the file: " + file.fileName + " is null.");
                    continue;
                }
                filter.AddCondition(OrderEntity.Fields.OrderNumber, ConditionOperator.Equal, orderId);
            }

            return XrmRetrieveHelper.RetrieveMultiple<OrderEntity>(localContext, queryAllRelevantOrders);
        }

        public List<MultiQFile> GetFileList(string path, string filter)
        {
            try
            {
                string[] files = Directory.GetFiles(path, filter);

                List<MultiQFile> lFiles = new List<MultiQFile>();

                for (int i = 0; i < files.Length; i++)
                {
                    MultiQFile mFile = new MultiQFile();
                    mFile.fileNameExtension = Path.GetFileName(files[i]);
                    mFile.fileName = Path.GetFileNameWithoutExtension(files[i]);
                    mFile.filePath = files[i];

                    lFiles.Add(mFile);
                }

                return lFiles;
            }
            catch (Exception e)
            {
                _log.Error(string.Format("The List of files was not retrieved: {0}", e.ToString()));
                return null;
            }
        }

        public string DownloadFile(string path)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(path);
                string base64 = Convert.ToBase64String(fileBytes);

                return base64;
            }
            catch (Exception e)
            {
                _log.Error(string.Format("The file was not downloaded: {0}", e.ToString()));
                return null;
            }
        }

        public bool MoveFile(string sourcePath, string targetPath)
        {
            try
            {
                if (!File.Exists(sourcePath))
                {
                    _log.Error($"The Source File does not exist. Contact your administrator.");
                    return false;
                }

                if (File.Exists(targetPath))
                {
                    _log.Error($"The Target File already exists. Contact your administrator.");
                    return false;
                }

                // Move the file.
                File.Move(sourcePath, targetPath);
                _log.Info(string.Format("{0} was moved to {1}.", sourcePath, targetPath));

                // See if the original exists now.
                if (File.Exists(sourcePath))
                    _log.Info($"The original file still exists, which is unexpected.");
                else
                    _log.Info($"The original file no longer exists, which is expected.");

                return true;
            }
            catch (Exception e)
            {
                _log.Error(string.Format("The file was not moved: {0}", e.ToString()));
                return false;
            }
        }

        public List<string> GetFileListFTP(string ipAddress, string path, int port, string userName, string passWord)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ipAddress + path + ":" + port));
                reqFTP.Credentials = new NetworkCredential(userName, passWord);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();
                        return responseString.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Error getting files names from FTP Server:\n{ex.Message}");
                return new List<string>();
            }
        }

        public string DownloadFileFTP(string ipAddress, string path, int port, string fileName, string userName, string passWord)
        {
            try
            {
                Uri serverUri = new Uri("ftp://" + ipAddress + path + ":" + port + "/" + fileName);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    _log.Error($"The URI Scheme is not FTP");
                    return null;
                }

                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(serverUri);
                reqFTP.Credentials = new NetworkCredential(userName, passWord);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.Proxy = null;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;

                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        responseStream.CopyTo(memoryStream);
                        byte[] bytes = memoryStream.ToArray();

                        return Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (WebException wEx)
            {
                _log.Error($"WebException Error DownloadFile() from FTP:\n{wEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"Exception Error DownloadFile() from FTP:\n{ex.Message}");
                return null;
            }
        }

        public bool MoveFileFTP(string ipAddress, string path, int port, string fileName, string userName, string passWord, string moveToPath)
        {
            try
            {
                Uri serverFile = new Uri("ftp://" + ipAddress + path + ":" + port + "/" + fileName);
                FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(serverFile);
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.Credentials = new NetworkCredential(userName, passWord);
                reqFTP.RenameTo = moveToPath + fileName;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"Exception Error MoveFile() from FTP:\n{ex.Message}");
                return false;
            }
        }
    }
}
