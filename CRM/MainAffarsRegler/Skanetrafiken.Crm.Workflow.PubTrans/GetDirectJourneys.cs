using System;
using System.Data;
using System.Activities;
using System.ServiceModel;
using System.Web.Script.Serialization;

using Endeavor.Crm;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.StopMonitoringService;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Skanetrafiken.Crm
{
    public class GetDirectJourneys : CodeActivity
    {
        [Input("FromStopAreaGid")]
        [RequiredArgument()]
        public InArgument<string> FromStopAreaGid { get; set; }

        [Input("ToStopAreaGid")]
        [RequiredArgument()]
        public InArgument<string> ToStopAreaGid { get; set; }

        [Input("TripDateTime")]
        [RequiredArgument()]
        public InArgument<string> TripDateTime { get; set; }

        [Input("ForLineGids")]
        [RequiredArgument()]
        public InArgument<string> ForLineGids { get; set; }

        [Input("TransportType")]
        [RequiredArgument()]
        public InArgument<string> TransportType { get; set; }

        [Output("DirectJourneysResponse")]
        public OutArgument<string> DirectJourneysResponse { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {
            // GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"GetDirectJourneys started.");

            // GET VALUE(S)
            string fromStopArea = FromStopAreaGid.Get(activityContext);
            string toStopArea = ToStopAreaGid.Get(activityContext);
            string tripDateTime = TripDateTime.Get(activityContext);
            string forLineGids = ForLineGids.Get(activityContext);
            string transportType = TransportType.Get(activityContext);
            string productCode = "";

            // Execute-metod
            try
            {
                string responsetext = ExecuteCodeActivityPubTrans(localContext, fromStopArea, toStopArea, tripDateTime, forLineGids, transportType, productCode);
                DirectJourneysResponse.Set(activityContext, responsetext);
            }
            catch (Exception ex)
            {
                DirectJourneysResponse.Set(activityContext, "An error occurred when contacting TravelInformation webservice: " + ex.Message);
            }

            localContext.Trace($"GetDirectJourneys finished.");

        }

        private static BasicHttpBinding GetPubTransBasicHttpBinding(string name)
        {
            var binding = new BasicHttpBinding
            {
                Name = name,
                OpenTimeout = TimeSpan.FromSeconds(20),
                MaxReceivedMessageSize = 20971520,
                Security = new BasicHttpSecurity()
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic
                    }
                }
            };
            return binding;
        }

        internal static StopMonitoringServiceClient GetStopMonitoringServiceClient(string _serviceEndpointUrl, string _userName, string _passWord)
        {
            var binding = GetPubTransBasicHttpBinding("StopMonitoringService");

            var endpoint = new EndpointAddress(_serviceEndpointUrl);
            return new StopMonitoringServiceClient(binding, endpoint)
            {
                ClientCredentials =
                {
                    UserName =
                    {
                        UserName = _userName,
                        Password = _passWord
                    }
                }
            };
        }

        public static string ExecuteCodeActivityPubTrans(Plugin.LocalPluginContext localContext, string fromStopAreaGid, string toStopAreaGid, string tripDateTime, string forLineGids, string transportType, string productCode)
        {
            string responseJourneys = "";

            // ONLY ASSIGNED IF TRAIN. REQUIRED FOR REQUEST
            string aot = productCode;
            string district = "PRODUCT";
            if (transportType == "TRAIN")
            {
                aot = "AOT";
                district = "DISTRICT";
            }

            byte timeDuration = 120;
            int departureMaxCount = 30;
            DateTime departureDate = DateTime.Parse(tripDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

            byte[] entropy = System.Text.Encoding.Unicode.GetBytes("PubTransService");
            string _serviceEndPointUrl = string.Empty;
            string _userName = string.Empty;
            string _passWordEncrypt = string.Empty;

            try
            {
                _serviceEndPointUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.cgi_PubTransService);
                _userName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_PubTransUserName);
                _passWordEncrypt = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_PubTransPassWord);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retrieving PubTrans URL/Credentials: {ex.Message}");
                throw new Exception($"An error occurred when retrieving PubTrans URL/Credentials: {ex.Message}", ex);
            }

            string _passWord = Decrypt("FBmSRydIbEtfgoJTr1R7Jbwrb+SXc4h2N8UxmJztGzM=", "PubTransPassWord");

            throw new Exception("123: " + _passWord);

            try
            {
                using (var client = GetDirectJourneys.GetStopMonitoringServiceClient(_serviceEndPointUrl, _userName, _passWord))
                {
                    DataSet dsJourneys = client.GetDirectJourneysBetweenStops(fromStopAreaGid, toStopAreaGid, departureDate, timeDuration, departureMaxCount, null, aot, district);

                    var serializer = new JavaScriptSerializer();
                    responseJourneys = serializer.Serialize(dsJourneys);

                    //responseJourneys = "Count: " + dsJourneys.Tables.Count + " ---DataSetName: " + dsJourneys.DataSetName;
                }
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when sending the request to PubTrans: {ex.Message}");
                throw new Exception($"An error occurred when sending the request to PubTrans: {ex.Message}", ex);
            }

            return responseJourneys;
        }

        public static string Decrypt(string cipherText, string encryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static SecureString DecryptString(string encryptedData, byte[] entropy)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch (Exception e)
            {
                //return new SecureString();
                return ToSecureString(e.Message);
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
