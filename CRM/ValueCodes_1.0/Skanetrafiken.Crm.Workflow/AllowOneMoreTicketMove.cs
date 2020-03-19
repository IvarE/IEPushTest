using System;
using System.Activities;
using System.IO;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System.Runtime.Serialization.Json;
using Endeavor.Crm;
using System.Net;
using Skanetrafiken.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class AllowOneMoreTicketMove : CodeActivity
    {

        [Input("ContactGuid")]
        [RequiredArgument()]
        public InArgument<string> Guid { get; set; }

        [Output("Results")]
        public OutArgument<string> Results { get; set; }

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
                localContext.Trace($"AllowOneMoreTicketMove started.");

                string guid = Guid.Get(activityContext);

                string results = ExecuteCodeActivity(localContext, guid);
                localContext.Trace($"AllowOneMoreTicketMove finished.");

                Results.Set(activityContext, results);
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException($"Process för att tillåta extra biljettbyte stötte på ett problem: {e.Message}", e);
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string guid)
        {
            try
            {
                Guid guidParam = System.Guid.Empty;
                if (!System.Guid.TryParse(guid, out guidParam))
                {
                    throw new Exception($"{guid} is not a valid format for Guid");
                }
                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, guidParam)
                        }
                    });
                if (contact == null)
                {
                    throw new Exception($"Could not find a contact with {guidParam.ToString()}");
                }

                string MklEndpoint = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_MklEndpoint);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{MklEndpoint}/admin/users/allowCarrierChange/{guid}");
                string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string InputJSON = "";

                    streamWriter.Write(InputJSON);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // Result is 
                    string result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace($"{MklEndpoint}/admin/users/{guid} returned StatusCode: {httpResponse.StatusCode} and results: {result}");
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {result}");
                    }
                }

                // Create NotifyMKL
                NotifyMKLEntity notification = new NotifyMKLEntity
                {
                    RegardingObjectId = contact.ToEntityReference(),
                    Subject = "En extra biljettflytt tillåten.",
                    ed_Method = Schema.Generated.ed_notifymkl_ed_method.NotApplicable
                };
                XrmHelper.Create(localContext, notification);

                return "Kund har nu ett extra byte.";
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;

                try
                {
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
#endif


                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MklErrorObject));
                    MklErrorObject mklErr = (MklErrorObject)serializer.ReadObject(response.GetResponseStream());

                    localContext.TracingService.Trace("Attempted GetTicketMoveDataFromMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, mklErr.localizedMessage);

                    throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {(mklErr.localizedMessage ?? mklErr.message) ?? mklErr.httpStatus.ToString()}", mklErr.innerException ?? null);
                }
                catch (Exception e)
                {
                    throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {e.Message}", e);
                }
            }
        }
    }
}
