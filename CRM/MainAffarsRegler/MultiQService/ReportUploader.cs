using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm.MultiQService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using log4net;

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

        private ILog _log = LogManager.GetLogger(typeof(ReportUploader));

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(OrdersService.CredentialFilePath, OrdersService.Entropy));

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

                string pdfMimeType = @"application/pdf";
                string pdfFilter = "*.pdf";

                QueryExpression queryOrders = new QueryExpression(OrderEntity.EntityLogicalName);
                queryOrders.NoLock = true;
                queryOrders.ColumnSet.AddColumns(OrderEntity.Fields.SalesOrderId, OrderEntity.Fields.ed_DeliveryReportName);
                queryOrders.Criteria.AddCondition(OrderEntity.Fields.ed_DeliveryReportStatus, ConditionOperator.Equal, (int)ed_deliveryreportstatus.Creatednotuploaded);
                queryOrders.Criteria.AddCondition(OrderEntity.Fields.ed_DeliveryReportName, ConditionOperator.NotNull);

                List<OrderEntity> lOrders = XrmRetrieveHelper.RetrieveMultiple<OrderEntity>(localContext, queryOrders);
                _log.Info($"Found " + lOrders.Count + " that have a Delivery Report Name and have a Status of 'Created and Not Uploaded'.");

                List<MultiQFile> pdfFiles = GetFileList(Properties.Settings.Default.MultiQStoreFiles, pdfFilter);
                _log.Info($"Found " + pdfFiles.Count + " PDF files in the location: " + Properties.Settings.Default.MultiQStoreFiles);

                foreach (OrderEntity order in lOrders)
                {
                    if (order.ed_DeliveryReportName != null)
                    {
                        List<MultiQFile> lFiles = pdfFiles.Where(x => x.fileName == order.ed_DeliveryReportName).ToList();

                        if (lFiles.Count == 1)
                        {
                            MultiQFile file = lFiles.FirstOrDefault();
                            string base64File = DownloadFile(file.filePath);

                            if (base64File != null)
                            {
                                Annotation note = new Annotation();
                                note.Subject = file.fileName;
                                note.NoteText = "Generated Report file from MultiQ for this Order.";
                                note.ObjectTypeCode = OrderEntity.EntityLogicalName;
                                note.ObjectId = new EntityReference(OrderEntity.EntityLogicalName, (Guid)order.SalesOrderId);
                                note.MimeType = pdfMimeType;
                                note.FileName = file.fileName;
                                note.DocumentBody = base64File;

                                Guid idNote = XrmHelper.Create(localContext, note);
                                _log.Info($"Note: " + idNote + " with Attachment was created on Related Order: " + order.SalesOrderId);

                                bool isMoved = MoveFile(file.filePath, Properties.Settings.Default.MultiQArchive + file.fileName);
                                if (isMoved)
                                    _log.Info($"The file " + file + " was moved to History.");
                                else
                                    _log.Info($"The file " + file + " was not moved to History. Please check the Exception for more details.");
                            }
                            else
                                _log.Error($"The Base64 for the file: " + file.fileName + " is null.");
                        }
                        else if (lFiles.Count == 0)
                            _log.Info($"There is no file named: " + order.ed_DeliveryReportName + " on the Shared Folder: " + Properties.Settings.Default.MultiQStoreFiles + ".");
                        else if (lFiles.Count > 1)
                            _log.Info($"There is more than one file named: " + order.ed_DeliveryReportName + " on the Shared Folder: " + Properties.Settings.Default.MultiQStoreFiles + ".");
                    }
                    else
                        _log.Error($"The File Name of the Order " + order.SalesOrderId + " was null.");
                }

                _log.Info($"ReportUploader Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ReportUploader.ExecuteJob():\n{e.Message}\n\n");
            }
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
                    mFile.fileName = Path.GetFileName(files[i]);
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
