using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class IncidentController : WrapperController
    {
        private string _prefix = "Incident";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("Wood attachment would a woodchuck chuck");

        private const string TenantID = "e1fcb9f3-e5f9-496f-a583-e495dfd57497"; //Tenent (ed_jojocarddetailstenentid)

        //private const string StorageAccountName = "webstpublicwebtest"; //TEST (OLD?)
        private const string StorageAccountName = "webpublicwebtest"; //TEST
        private const string ClientID = "635555fe-6cb2-4a48-9b54-8d5d6472b00f"; //TEST - App/Client ID
        private const string ClientSecret = "czv7Q~iQuUCRwkryVp6FqHvmSAhbA_I2XidG_"; //TEST

        //private const string StorageAccountName = "webstpublicwebacc"; //ACC (OLD)
        //private const string StorageAccountName = "webpublicwebacc"; //ACC
        //private const string ClientID = "7450a67c-038e-4b43-b4a1-b5bbd2961912"; //ACC - App/Client ID
        //private const string ClientSecret = "gf57Q~2TGjcsESRX~20ohxZ-Xg3JhX2C55XKc"; //ACC

        //private const string StorageAccountName = "webpublicwebprod"; //PROD
        //private const string ClientID = "73acce44-96d3-48a1-8c44-33185bc2f24f"; //PROD - App/Client ID
        //private const string ClientSecret = "mGJ7Q~4jEXcEgzoYd1IBc4tUYb4raX8XOXTJs"; //PROD


        private const string FileName = "2021/9/20/ef05fe16-0f9d-43d8-909e-6c64f6394aac.jpg";

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            return CrmPlusControl.PingConnection(threadId, _prefix);
        }

        //public static string ToInsecureString(SecureString input)
        //{
        //    string returnValue = string.Empty;
        //    IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
        //    try
        //    {
        //        returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
        //    }
        //    return returnValue;
        //}

        //public static SecureString ToSecureString(string input)
        //{
        //    SecureString secure = new SecureString();
        //    foreach (char c in input)
        //    {
        //        secure.AppendChar(c);
        //    }
        //    secure.MakeReadOnly();
        //    return secure;
        //}

        //internal static string LoadCredentials(string filePath, byte[] entropy)
        //{
        //    if (!File.Exists(filePath))
        //    {
        //        throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "The credentials file \"{0}\" cannot be found. Use the /Password: parameter to create the credential file.", filePath));
        //    }
        //    XDocument doc = XDocument.Load(filePath);

        //    return ToInsecureString(DecryptString(doc.Root.Element("Password").Value, entropy));
        //}

        //public static SecureString DecryptString(string encryptedData, byte[] entropy)
        //{
        //    try
        //    {
        //        byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
        //            Convert.FromBase64String(encryptedData),
        //            entropy,
        //            System.Security.Cryptography.DataProtectionScope.CurrentUser);
        //        return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
        //    }
        //    catch (Exception)
        //    {
        //        return new SecureString();
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage GetAttachmentFromAzure(string encryptedUrl)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (encryptedUrl == string.Empty)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetAttachmentFromAzure", Resources.IncomingDataCannotBeNull, _logger);
                }

                Plugin.LocalPluginContext localContext = null;
                HttpResponseMessage rm = new HttpResponseMessage();

                try
                {
                    CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                    // Cast the proxy client to the IOrganizationService interface.
                    using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                    {
                        localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                        if (localContext.OrganizationService == null)
                            throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                        FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.ed_AttachmentClientID, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.ed_AttachmentClientSecret, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.ed_AttachmentStorageAccountName, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.st_AttachmentAudience, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.ed_ClientCertNameReskassa, ConditionOperator.NotNull);
                        CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                            CgiSettingEntity.Fields.ed_AttachmentClientID,
                            CgiSettingEntity.Fields.ed_AttachmentClientSecret,
                            CgiSettingEntity.Fields.ed_AttachmentStorageAccountName,
                            CgiSettingEntity.Fields.st_AttachmentAudience,
                            CgiSettingEntity.Fields.ed_JojoCardDetailsTenentId,
                            CgiSettingEntity.Fields.ed_ClientCertNameReskassa), settingFilter);

                        if (settings == null)
                        {
                            //Throw an exception
                            throw new Exception(string.Format("GetAttachmentFromAzure: Failed to aquire settings from CRM."));
                        }
                        else
                        {
                            string clientId = settings.ed_AttachmentClientID;
                            string clientSecret = settings.ed_AttachmentClientSecret;
                            string tenantId = settings.ed_JojoCardDetailsTenentId;
                            string storageAccountName = settings.ed_AttachmentStorageAccountName;
                            string audience = settings.st_AttachmentAudience;

                            if (string.IsNullOrWhiteSpace(clientId) ||
                                    string.IsNullOrWhiteSpace(clientSecret) ||
                                    string.IsNullOrWhiteSpace(tenantId) ||
                                    string.IsNullOrWhiteSpace(storageAccountName) ||
                                    string.IsNullOrWhiteSpace(audience))
                            {
                                throw new Exception(string.Format("GetAttachmentFromAzure: Could not retrieve the relevant keys from CgiSettings."));
                            }

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
                                    int createFailedCount = 0;
                                    //Handle Loop of all string values
                                    for (int i = 0; i < links.Length - 1; i++)
                                    {
                                        //main Function
                                        //var containerName = "crm-attachments"; //Skriv till oss
                                        //var containerName = "rgolreceiptinformation"; //RGOL
                                        string fileUrl = string.Empty;
                                        string containerName = "";
                                        if (links[i].Contains("rgolreceiptinformation"))
                                        {
                                            containerName = "rgolreceiptinformation";
                                            fileUrl = links[i].Substring("rgolreceiptinformation/".Length);
                                        }
                                        else if (links[i].Contains("crm-attachments"))
                                        {
                                            containerName = "crm-attachments";
                                            fileUrl = links[i].Substring("crm-attachments/".Length);
                                        }
                                        else
                                        {
                                            containerName = "crm-attachments";
                                            fileUrl = links[i];
                                        }

                                        //TODO: Create email attachment using GUID passed in the API and attachmentBase64String
                                        string attachmentBase64String = "";
                                        if (!string.IsNullOrWhiteSpace(fileUrl))
                                        {
                                            attachmentBase64String = CrmPlusControl.HandleAttachemntFilesFromAzure(threadId, clientId, clientSecret, audience, tenantId, storageAccountName, containerName, fileUrl, emailGuid, _prefix);
                                            if (attachmentBase64String.Contains("(FailedCreate)"))
                                            {
                                                //Highlight that all attachments couldnt be created
                                                failedCount++;
                                            }
                                            else if (attachmentBase64String.Contains("(SuccessCreate)"))
                                            {
                                                //Highlight that all attachments couldnt be created
                                                createFailedCount++;
                                            }
                                            else
                                            {
                                                count++;
                                            }
                                        }
                                    }

                                    //Return success
                                    rm.StatusCode = HttpStatusCode.OK;
                                    rm.Content = new StringContent($"Created Attachments. Failed count:{failedCount}");
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

                                string attachmentBase64String = CrmPlusControl.HandleAttachemntFilesFromAzure(threadId, clientId, clientSecret, audience, tenantId, storageAccountName, containerName, encryptedUrl, emailGuid, _prefix);

                                if (!string.IsNullOrWhiteSpace(attachmentBase64String) && attachmentBase64String.Contains("Exception") == false)
                                {
                                    rm.StatusCode = HttpStatusCode.OK;
                                    rm.Content = new StringContent(attachmentBase64String);
                                }
                                else
                                {
                                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetAttachmentFromAzure", attachmentBase64String, _logger);
                                }
                            }

                            //Return Logg
                            if (rm == null)
                            {
                                return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetAttachmentFromAzure", "Return Value was never set!", _logger);
                            }

                            return rm;
                        }
                    }
                }
                catch (Exception ex)
                {
                    rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.InnerException?.Message));

                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return rm;
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }
    }
}
