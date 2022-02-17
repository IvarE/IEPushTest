using Endeavor.Crm;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Properties;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class IncidentController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("Wood attachment would a woodchuck chuck");

        private const string TenantID = "e1fcb9f3-e5f9-496f-a583-e495dfd57497"; //Tenent

        //private const string StorageAccountName = "webstpublicwebtest"; //TEST (OLD?)
        //private const string StorageAccountName = "webpublicwebtest"; //TEST
        //private const string ClientID = "635555fe-6cb2-4a48-9b54-8d5d6472b00f"; //TEST - App/Client ID
        //private const string ClientSecret = "czv7Q~iQuUCRwkryVp6FqHvmSAhbA_I2XidG_"; //TEST

        //private const string StorageAccountName = "webstpublicwebacc"; //ACC (OLD)
        private const string StorageAccountName = "webpublicwebacc"; //ACC
        private const string ClientID = "7450a67c-038e-4b43-b4a1-b5bbd2961912"; //ACC - App/Client ID
        private const string ClientSecret = "gf57Q~2TGjcsESRX~20ohxZ-Xg3JhX2C55XKc"; //ACC

        //private const string StorageAccountName = "webpublicwebprod"; //PROD
        //private const string ClientID = "73acce44-96d3-48a1-8c44-33185bc2f24f"; //PROD - App/Client ID
        //private const string ClientSecret = "mGJ7Q~4jEXcEgzoYd1IBc4tUYb4raX8XOXTJs"; //PROD


        private const string FileName = "2021/9/20/ef05fe16-0f9d-43d8-909e-6c64f6394aac.jpg";

        private static async Task<string> GetAccessToken(string clientId, string clientSectret, string tenantId)
        {
            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext("https://login.windows.net/" + $"{tenantId}");
            var credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientSectret);
            var result = await authContext.AcquireTokenAsync("https://storage.azure.com", credential);

            if (result == null)
            {
                _log.Warn($"-- GetAccessToken Failed! --");
                throw new Exception("Failed to authenticate via ADAL");
            }

            return result.AccessToken;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET IncidentController called.\n");
            return CrmPlusControl.PingConnection(threadId);
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentFromAzure(string encryptedUrl)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Get called.\n");
            _log.Warn($"Th={threadId} - GetAttachmentFromAzure called with Payload:\n {encryptedUrl}");
            _log.DebugFormat($"Th={threadId} - Get called with Payload:\n {encryptedUrl}");

            if (encryptedUrl == string.Empty)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            string clientId = ClientID;
            string clientSecret = ClientSecret;
            string tenantId = TenantID;
            string storageAccountName = StorageAccountName;

            if (string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(tenantId) ||
                    string.IsNullOrWhiteSpace(storageAccountName))
            {
                HttpResponseMessage keysError = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                keysError.Content = new StringContent($"GetAttachmentFromAzure: Could not retrieve the relevant keys.");
                _log.Warn($"Th={threadId} - Returning statuscode = {keysError.StatusCode}, Content = {keysError.Content.ReadAsStringAsync().Result}\n");
                return keysError;
            }

            HttpResponseMessage rm = new HttpResponseMessage();

            try
            {
                Guid? emailGuid = null;
                //Check if string contains semi-colons
                if (encryptedUrl.Contains(";") == true)
                {
                    //TODO: Split the string into sections based on the semi-colon
                    string[] links = encryptedUrl.Split(';');
                    emailGuid = (Guid?)Guid.Parse(links[links.Length - 1]);

                    if (emailGuid != null)
                    {
                        int count = 0;
                        int failedCount = 0;
                        //Handle Loop of all string values
                        foreach (var link in links)
                        {
                            //main Function
                            //var containerName = "crm-attachments"; //Skriv till oss
                            //var containerName = "rgolreceiptinformation"; //RGOL
                            string fileUrl = string.Empty;
                            string containerName = "";
                            if (link.Contains("rgolreceiptinformation"))
                            {
                                containerName = "rgolreceiptinformation";
                                fileUrl = link.Substring("rgolreceiptinformation/".Length);
                            }
                            else if (link.Contains("crm-attachments"))
                            {
                                containerName = "crm-attachments";
                                fileUrl = link.Substring("crm-attachments/".Length);
                            }
                            else
                            {
                                containerName = "crm-attachments";
                                fileUrl = link;
                            }
                            _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Container Name => {containerName}");

                            //TODO: Create email attachment using GUID passed in the API and attachmentBase64String
                            string attachmentBase64String = "";
                            if (!string.IsNullOrWhiteSpace(fileUrl))
                            {
                                attachmentBase64String = CrmPlusControl.HandleAttachemntFilesFromAzure(threadId, clientId, clientSecret, tenantId, storageAccountName, containerName, fileUrl, emailGuid);
                                if (attachmentBase64String.Contains("Exception") && attachmentBase64String.Contains("(Created)"))
                                {
                                    //Highlight that all attachments couldnt be created
                                    failedCount++;
                                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: {failedCount} Attachments failed to create");
                                }
                                else
                                {
                                    count++;
                                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: {count} Attachments Created");
                                }
                            }
                            else
                            {
                                _log.Warn($"Th={threadId} - GetAttachmentFromAzure: {failedCount + 1} Attachments failed to create - Missing URL");
                            }
                        }

                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: {count} Attachments Created");
                        //Return success
                        rm.StatusCode = HttpStatusCode.OK;
                        rm.Content = new StringContent($"Created Attachments");
                    }
                    else 
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Attachments failed to create - Missing Email Guid");
                    }
                    
                }
                else
                {
                    //Handle the return values
                    //var containerName = "crm-attachments"; //Skriv till oss
                    //var containerName = "rgolreceiptinformation"; //RGOL
                    string containerName = "";
                    if (encryptedUrl.Contains("rgolreceiptinformation"))
                    {
                        containerName = "rgolreceiptinformation";
                        encryptedUrl = encryptedUrl.Substring("rgolreceiptinformation/".Length);
                    }
                    else if (encryptedUrl.Contains("crm-attachments"))
                    {
                        containerName = "crm-attachments";
                        encryptedUrl = encryptedUrl.Substring("crm-attachments/".Length);
                    }
                    else
                    {
                        containerName = "crm-attachments";
                    }

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Container Name => {containerName}");
                    string attachmentBase64String = CrmPlusControl.HandleAttachemntFilesFromAzure(threadId, clientId, clientSecret, tenantId, storageAccountName, containerName, encryptedUrl, emailGuid);

                    if (!string.IsNullOrWhiteSpace(attachmentBase64String) && attachmentBase64String.Contains("Exception") == false)
                    {
                        rm.StatusCode = HttpStatusCode.OK;
                        rm.Content = new StringContent(attachmentBase64String);
                    }
                    else 
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: {attachmentBase64String}");
                    }
                }

                //Return Logg
                if (rm == null)
                {
                    rm.StatusCode = HttpStatusCode.InternalServerError;
                    rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Return Value was never set!");
                    return rm;
                }

                if (rm.StatusCode != HttpStatusCode.OK)
                {
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                }
                else
                {
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                    _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                }

                return rm;
            }
            catch (Exception ex)
            {
                rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.InnerException?.Message));
                _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }

        }
    }
}
