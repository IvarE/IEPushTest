using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IO;

namespace Skanetrafiken.Crm.Controllers
{
    public class IncidentController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string TenantID = "e1fcb9f3-e5f9-496f-a583-e495dfd57497"; //Tenent
        //private const string StorageAccountName = "CRM-WEBCASE-SP-ACC"; //DisplayName
        private const string StorageAccountName = "webstpublicwebtest"; //DisplayName
        private const string ClientID = "635555fe-6cb2-4a48-9b54-8d5d6472b00f"; //AppId?
        private const string ClientSecret = "czv7Q~iQuUCRwkryVp6FqHvmSAhbA_I2XidG_"; //Password
        private const string FileName = "";

        private static async Task<string> GetAccessToken()
        {
            var authContext = new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext("https://login.windows.net/" + $"{TenantID}");
            var credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(ClientID, ClientSecret);
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

            HttpResponseMessage rm = null;
            //Decrypt the string

            try
            {
                byte[] testContent = { };
                var containerName = "crm-attachments"; //might not be correct?
                _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Container Name => {containerName}");

                _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Starting TASK...");
                Task.Run(async () =>
                {
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Getting Access Token...");
                    var token = await GetAccessToken();
                    if (token != string.Empty)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Token Retrieved => {token}");
                    }
                    else
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not retrieve a Token!");
                    }

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Creating Token Credential...");
                    TokenCredential tokenCredential = new TokenCredential(token);
                    if (tokenCredential != null)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Token Credential Created!");
                    }
                    else
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not create Token Credential!");
                    }

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Creating Storage Credential...");
                    StorageCredentials storageCredentials = new StorageCredentials(tokenCredential);
                    if (storageCredentials != null)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Storage Credential Created!");
                    }
                    else
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not create Storage Credential!");
                    }

                    string cloudBlobClientURI = "https://" + $"{StorageAccountName}" + ".blob.core.windows.net";
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Creating CloudBlobClient with URI: \n {cloudBlobClientURI}");
                    CloudBlobClient client = new CloudBlobClient(new Uri(cloudBlobClientURI), storageCredentials);
                    if (client != null)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: CloudBlobClient Created!");
                    }
                    else
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not create CloudBlobClient with URI: {cloudBlobClientURI}");
                    }

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Creating CloudBlobContainer with ContainerName: {containerName}");
                    CloudBlobContainer container = client.GetContainerReference(containerName);
                    if (container != null)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: CloudBlobContainer Created!");
                    }
                    else
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not create CloudBlobContainer with ContainerName: {containerName}");
                    }

                    //var blob = container.GetBlockBlobReference(FileName);
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Getting File as a blob using 'container.GetBlockBlobReference()'...");
                    var blob = container.GetBlockBlobReference(encryptedUrl);
                    if (blob != null)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Blob Retrieved!");
                    }
                    else 
                    {
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not find a Blob using GetBlockBlobReference()!");
                    }

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Creating MemoryStream...");
                    MemoryStream ms = new MemoryStream();
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: MemoryStream Created!");

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Downloading Blob to created Stream...");
                    blob.DownloadToStream(ms); //*****
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Blob downloaded to Stream!");

                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Parsing Stream to Byte content...");
                    //byte[] content = ms.ToArray();
                    testContent = ms.ToArray();
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Stream Parsed to Byte content!");

                }).Wait();
                _log.Warn($"Th={threadId} - GetAttachmentFromAzure: TASK finished!");

                //var test = IncidentEntity.HandleDecryptAttachment(localContext, encryptedUrl, null);
                if (testContent?.Length > 0)
                {
                    rm.StatusCode = HttpStatusCode.OK;
                    rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Created stream and parsed stream to byte content!");
                    //rm.Content = File();
                }
                else 
                {
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Parsed Stream did not have any value!");
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
                ex.ToString();
                rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }

        }
    }
}
