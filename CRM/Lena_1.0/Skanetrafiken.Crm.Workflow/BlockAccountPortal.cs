using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using System.IO;

namespace Skanetrafiken.Crm.Entities
{
    public class BlockAccountPortal : CodeActivity
    {
        [Input("PortalID")]
        [RequiredArgument()]
        public InArgument<string> PortalID { get; set; }

        [Input("ParentID")]
        [RequiredArgument()]
        public InArgument<string> ParentID { get; set; }

        [Input("OrganizationNumber")]
        [RequiredArgument()]
        public InArgument<string> OrganizationNumber { get; set; }

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
            localContext.Trace($"BlockCustomerPortal started.");
            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                string portalId = PortalID.Get(activityContext);
                bool blocked = Blocked.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, portalId, blocked);
                localContext.Trace($"BlockAccountPortal finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                localContext.Trace($"BlockAccountPortal finished with exception: {ex.Message}");

                throw new InvalidPluginExecutionException($"BlockAccountPortal failed. Exception: {ex.Message}");

            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string portalId, bool blocked)
        {
            try
            {
                return SendMKLRequest(localContext, portalId, blocked);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string SendMKLRequest(Plugin.LocalPluginContext localContext, string portalId, bool blocked)
        {
            string errorMessage = "MKLRequest returned an exception. Certificate {0}, endpoint {1}, payload {2}. Message: {3}";
            string mklEndpoint = String.Empty;
            string clientCertName = String.Empty;
            string InputJSON = String.Empty;
            
            try
            {
                mklEndpoint = CgiSettingEntity.GetSettingString(localContext, "ed_companyintegrationendpoint");
                localContext.Trace($"Endpoint: {mklEndpoint}");
                
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}/api/block/accounts/{portalId}");
                clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                localContext.Trace($"Certificate: {clientCertName}");

                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    // Result is 
                    InputJSON = "{\"accountId\": " + portalId + ", \"blocked\": " + blocked.ToString().ToLower() + "}";
                    localContext.Trace($"Calling MKL with message: {InputJSON}");
                    streamWriter.Write(InputJSON);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("Test", result);

                    return result;

                    throw new Exception("Test passed");
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
                    localContext.TracingService.Trace("SendMKLRequest() returned an exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                }

#else
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("SendMKLRequest returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(we.Message, we);
                }

#endif
            }
        }
    }
}