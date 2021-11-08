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

        //private const string StorageAccountName = "webstpublicwebtest"; //TEST
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

            //Create Credential File
            _log.Debug("Main Started");

            string passwordArgument = null;

            string passwordClientID = $"Password: {ClientID}";
            string passwordClientSecret = $"Password: {ClientSecret}";
            string passwordTenent = $"Password: {TenantID}";
            string passwordStorageName = $"Password: {StorageAccountName}";

            string passwordCreditsafeArgument = "ijg6fmUq"; //"3L6932Vt";

            //string[] args = System.Environment.GetCommandLineArgs();
            //if (args != null)
            //{
            //    var passwordArgs = args.Where(s => s.Contains("Password:"));
            //    if (passwordArgs.Count() > 0)
            //    {
            //        passwordArgument = passwordArgs.First();
            //    }
            //    var passwordCreditsafeArgs = args.Where(s => s.Contains("PasswordCreditsafe:"));
            //    if (passwordCreditsafeArgs.Count() > 0)
            //    {
            //        passwordCreditsafeArgument = passwordCreditsafeArgs.First();
            //    }
            //}

            //if (!string.IsNullOrEmpty(passwordClientID))
            //{
            //    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
            //    string password = passwordClientID.Substring(passwordClientID.IndexOf(":") + 1);

            //    CrmConnection.SaveCredentials(Properties.Settings.Default.ClientIdCredentialFilePath, password, IncidentController.Entropy);
            //}

            //if (!string.IsNullOrEmpty(passwordClientSecret))
            //{
            //    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
            //    string password = passwordClientSecret.Substring(passwordClientSecret.IndexOf(":") + 1);

            //    CrmConnection.SaveCredentials(Properties.Settings.Default.ClientSecretCredentialFilePath, password, IncidentController.Entropy);
            //}

            //if (!string.IsNullOrEmpty(passwordTenent))
            //{
            //    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
            //    string password = passwordTenent.Substring(passwordTenent.IndexOf(":") + 1);

            //    CrmConnection.SaveCredentials(Properties.Settings.Default.TenentCredentialFilePath, password, IncidentController.Entropy);
            //}

            //if (!string.IsNullOrEmpty(passwordStorageName))
            //{
            //    _log.DebugFormat(CultureInfo.InvariantCulture, Properties.Resources.CredentialsCommandLine);
            //    string password = passwordStorageName.Substring(passwordStorageName.IndexOf(":") + 1);

            //    CrmConnection.SaveCredentials(Properties.Settings.Default.StorageNameCredentialFilePath, password, IncidentController.Entropy);
            //}

            HttpResponseMessage rm = new HttpResponseMessage();
            //Decrypt the string

            try
            {
                //string clientId = CrmConnection.LoadCredentials(Properties.Settings.Default.ClientIdCredentialFilePath, IncidentController.Entropy);
                //string clientSecret = CrmConnection.LoadCredentials(Properties.Settings.Default.ClientSecretCredentialFilePath, IncidentController.Entropy);
                //string tenantId = CrmConnection.LoadCredentials(Properties.Settings.Default.TenentCredentialFilePath, IncidentController.Entropy);
                //string storageAccountName = CrmConnection.LoadCredentials(Properties.Settings.Default.StorageNameCredentialFilePath, IncidentController.Entropy);

                string clientId = ClientID;
                string clientSecret = ClientSecret;
                string tenantId = TenantID;
                string storageAccountName = StorageAccountName;

                byte[] imageByteArray = { };

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

                //Get settings from CRM
                //CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_StorageAccountName));
                _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Check that we have found the relevant keys.");
                if (string.IsNullOrWhiteSpace(clientId) ||
                    string.IsNullOrWhiteSpace(clientSecret) ||
                    string.IsNullOrWhiteSpace(tenantId) ||
                    string.IsNullOrWhiteSpace(storageAccountName) ||
                    string.IsNullOrWhiteSpace(containerName))
                {
                    rm.StatusCode = HttpStatusCode.InternalServerError;
                    rm.Content = new StringContent($"Th={threadId} - GetAttachmentFromAzure: Could not retrieve the relevant keys.");
                    return rm;
                }
                else 
                {
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Starting TASK...");
                    Task.Run(async () =>
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Getting Access Token...");
                        var token = await GetAccessToken(clientId, clientSecret, tenantId);
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

                        string cloudBlobClientURI = "https://" + $"{storageAccountName}" + ".blob.core.windows.net";
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

                        imageByteArray = ms.ToArray();
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: Stream Parsed to Byte content!");


                    }).Wait();
                    _log.Warn($"Th={threadId} - GetAttachmentFromAzure: TASK finished!");

                    if (imageByteArray?.Length > 0)
                    {
                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: imageByteArray greater than 0!");
                        string imageBase64String = Convert.ToBase64String(imageByteArray);

                        _log.Warn($"Th={threadId} - GetAttachmentFromAzure: imageByteArray gconverted to Base64 String!");
                        rm.StatusCode = HttpStatusCode.OK;
                        rm.Content = new StringContent(imageBase64String);
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
