using Common.Logging;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.MultiQService
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

                if(localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }

                string pdfMimeType = @"application/pdf";
                string ipAddress = "192.168.2.110";
                string pathToFolder = "/lib1";
                int port = 21;
                string userName = "user1";
                string passWord = "pass1";

                QueryExpression queryOrders = new QueryExpression(OrderEntity.EntityLogicalName);
                queryOrders.NoLock = true;
                queryOrders.ColumnSet.AddColumns(OrderEntity.Fields.SalesOrderId, OrderEntity.Fields.ed_DeliveryReportName);
                queryOrders.Criteria.AddCondition(OrderEntity.Fields.ed_DeliveryReportStatus, ConditionOperator.Equal, (int)ed_deliveryreportstatus.Creatednotuploaded);

                List<OrderEntity> lOrders = XrmRetrieveHelper.RetrieveMultiple<OrderEntity>(localContext, queryOrders);

                List<string> dirListFiles = GetFileList(ipAddress, pathToFolder, port, userName, passWord);

                foreach (OrderEntity order in lOrders)
                {
                    if(order.ed_DeliveryReportName != null)
                    {
                        List<string> lFiles = dirListFiles.Where(x => x == order.ed_DeliveryReportName).ToList();

                        if(lFiles.Count == 1)
                        {
                            string file = lFiles.FirstOrDefault();
                            string base64File = DownloadFile(ipAddress, pathToFolder, port, file, userName, passWord);

                            Annotation note = new Annotation();
                            note.Subject = file;
                            note.NoteText = "Generated Report file from MultiQ for this Order.";
                            note.ObjectTypeCode = OrderEntity.EntityLogicalName;
                            note.ObjectId = new EntityReference(OrderEntity.EntityLogicalName, (Guid)order.SalesOrderId);
                            note.MimeType = pdfMimeType;
                            note.FileName = file;
                            note.DocumentBody = base64File;

                            Guid idNote = XrmHelper.Create(localContext, note);
                            _log.Info($"Note: " + idNote + " with Attachment was created on Related Order: " + order.SalesOrderId);

                            //MOVE THE FILE TO AN HISTORY FOLDER
                        }
                        else if(lFiles.Count == 0)
                        {
                            _log.Info($"There is no file named: " + order.ed_DeliveryReportName + " on the FTP Server.");
                        }
                        else if(lFiles.Count > 1)
                        {
                            _log.Info($"There is more than one file named: " + order.ed_DeliveryReportName + " on the FTP Server.");
                        }
                    }
                }

                _log.Info($"ReportUploader Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in ReportUploader.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public List<string> GetFileList(string ipAddress, string path, int port, string userName, string passWord)
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

        private string DownloadFile(string ipAddress, string path, int port, string fileName, string userName, string passWord)
        {
            try
            {
                Uri serverUri = new Uri("ftp://" + ipAddress +  path + ":" + port +  "/" + fileName);
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
    }
}
