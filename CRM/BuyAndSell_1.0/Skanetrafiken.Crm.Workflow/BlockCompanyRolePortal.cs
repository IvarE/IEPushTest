using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Skanetrafiken.Crm.Entities
{
    public class BlockCompanyRolePortal : CodeActivity
    {
        [Input("SSN")]
        [RequiredArgument()]
        public InArgument<string> SSN { get; set; }

        [Input("PortalId")]
        [RequiredArgument()]
        public InArgument<string> PortalId { get; set; }

        [Input("Blocked")]
        [RequiredArgument()]
        public InArgument<bool> Blocked { get; set; }

        private Plugin.LocalPluginContext GetLocalContext(CodeActivityContext activityContext)
        {
            IWorkflowContext workflowContext = activityContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = activityContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(workflowContext.InitiatingUserId);
            ITracingService tracingService = activityContext.GetExtension<ITracingService>();

            return new Plugin.LocalPluginContext(null, organizationService, null, tracingService);
        }

        protected override void Execute(CodeActivityContext activityContext)
        {//GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"BlockCompanyRolePortal started.");
            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                string ssn = SSN.Get(activityContext);
                string portalId = PortalId.Get(activityContext);
                bool blocked = Blocked.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, ssn, portalId, blocked);
                localContext.Trace($"BlockCompanyRolePortal finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                localContext.Trace($"BlockCustomerPortal finished with exception: {ex.Message}");

                throw new InvalidPluginExecutionException($"Failed to block customer. Exception: {ex.Message}");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string ssn, string portalId, bool blocked)
        {
            string errorMessage = "MKLRequest returned an exception. Certificate {0}, endpoint {1}, payload {2}. Message: {3}";
            string mklEndpoint = String.Empty;
            string clientCertName = String.Empty;
            string InputJSON = String.Empty;
            
            try
            {
                mklEndpoint = CgiSettingEntity.GetSettingString(localContext, "ed_companyintegrationendpoint");
                localContext.Trace($"Endpoint: {mklEndpoint}");

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}/api/block/accounts/{portalId}/users/{ssn}");
                clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                localContext.Trace($"Certificate: {clientCertName}");

                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    // Result is 
                    InputJSON = "{\"accountId\": " + portalId + ", \"socialSecurityNumber\": \"" + ssn + "\",  \"blocked\": " + blocked.ToString().ToLower() + "}";
                    localContext.Trace($"Calling MKL with message: {InputJSON}");
                    streamWriter.Write(InputJSON);
                    streamWriter.Flush();
                    streamWriter.Close();
                    
                }

                HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace(result);

                    return result;
                }

            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;

                if (response == null)
                {
                    throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                }
#if DEBUG
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("MKLRequest returned an exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                }
                
#else
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MklErrorObject));

                    MklErrorObject mklErr = (MklErrorObject)serializer.ReadObject(response.GetResponseStream());

                    localContext.TracingService.Trace("Attempted GetTicketMoveDataFromMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, mklErr.localizedMessage);

                    throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {(mklErr.localizedMessage ?? mklErr.message) ?? mklErr.httpStatus.ToString()}", mklErr.innerException ?? null);
                }
                catch (Exception e)
                {
                    if (e.Message?.StartsWith("Fel vid kommunikation med externt system") != true)
                        throw e;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        throw new InvalidPluginExecutionException($"Fel vid hantering av fel från externt system: {e.Message}, result: {result}", e);
                    }
                }
#endif
            }
        }
    }
}