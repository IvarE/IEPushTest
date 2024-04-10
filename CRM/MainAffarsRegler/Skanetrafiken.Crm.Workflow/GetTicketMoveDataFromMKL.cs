using System;
using System.Activities;
using System.IO;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using Skanetrafiken.Crm;
using System.Runtime.Serialization.Json;

namespace Skanetrafiken.Crm.Entities
{
    public class GetTicketMoveDataFromMKL : CodeActivity
    {

        [Input("ContactGuid")]
        [RequiredArgument()]
        public InArgument<string> Guid { get; set; }

        [Output("GetTicketMoveDataFromMKLResponse")]
        public OutArgument<string> GetTicketMoveDataFromMKLResponse { get; set; }

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
            try
            {
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
                localContext.Trace($"GetTicketMoveDataFromMKL started.");

                string guid = Guid.Get(activityContext);

                string result = ExecuteCodeActivity(localContext, guid);

                GetTicketMoveDataFromMKLResponse.Set(activityContext, result);
                localContext.Trace($"GetTicketMoveDataFromMKL finished.");
            }
            catch (Exception ex)
            {
                GetTicketMoveDataFromMKLResponse.Set(activityContext, "Ett fel uppstod vid kommunikation med MKL: " + ex.Message);
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string guid)
        {
            try
            {
                string mklEndpoint = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_MklEndpoint); // Viggo  TODO här behöver vi tillfälligt hårdkoda till gamla adressen.
                //string mklEndpoint = "https://stmkltest.azurewebsites.net";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}/admin/users/{guid}");
                string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                //string clientCertName = "SE162321000255-F16675";
                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // Result is 
                    string result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace($"GET with {mklEndpoint}/admin/users/{guid} returned StatusCode: {httpResponse.StatusCode} and results: {result}");
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception($"Kommunikation med externt system returnerade: {httpResponse.StatusCode.ToString()}, {result}");
                    }
                    return result;
                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;

                if (response == null)
                {
                    throw we;
                }
#if DEBUG
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("ExecuteCodeActivity returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(result, we);
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
