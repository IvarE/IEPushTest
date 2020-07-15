using Endeavor.Crm;
using Microsoft.Identity.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.Entities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm
{
    public class FrontOffice : CodeActivity
    {
        public static string tenentId = String.Empty;
        public static string msAuthScope = String.Empty;
        public static string applicationId = String.Empty;
        public static string certificateName = String.Empty;

        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        private static IConfidentialClientApplication _msalApplication => _msalApplicationFactory.Value;

        private static Lazy<IConfidentialClientApplication> _msalApplicationFactory =
            new Lazy<IConfidentialClientApplication>(() =>
            {
                var certificate = Identity.GetCertToUse(certificateName);

                var authority = string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/" + tenentId);

                // Dynamic
                return ConfidentialClientApplicationBuilder
                    .Create(applicationId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(authority))
                    .Build();
            });

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
            //GENERATE CONTEXT
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"FrontOffice started.");

            //TRY EXECUTE
            try
            {
                string response = ExecuteCodeActivity(localContext);
                localContext.Trace($"FrontOffice finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                localContext.Trace($"FrontOffice finished with exception: {ex.Message}");
                throw new InvalidPluginExecutionException($"Failed to FrontOffice. Exception: {ex.Message}");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext)
        {
            string errorMessage = "FrontOffice returned an exception. Certificate {0}, endpoint {1}. Message: {2}";
            string ticketId = String.Empty;
            string travelEndpoint = String.Empty;
            string clientCertName = String.Empty;
            string certificateName = String.Empty;

            localContext.Trace($"FrontOffice: Calling -> Get information from settings");

            try
            {
                //Get information from settings
                FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
                settingFilter.AddCondition(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI, ConditionOperator.NotNull);
                CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_JojoCardDetailsAPI), settingFilter);

                if (settings != null)
                {
                    applicationId = settings.ed_JojoCardDetailsApplicationId;
                    tenentId = settings.ed_JojoCardDetailsTenentId;
                    clientCertName = settings.ed_ClientCertNameReskassa;
                    msAuthScope = settings.ed_JojoCardDetailsScope;
                }
                else
                {
                    localContext.Trace("FrontOffice returned an exception\nContent: {0}\n", "Could not find a Settings Information from CRM (FrontOffice)");
                    throw new Exception(String.Format(errorMessage, clientCertName, travelEndpoint, "Could not find a Settings Information from CRM (FrontOffice)"));
                }

                localContext.Trace($"FrontOffice: Calling -> _msalApplication.AcquireTokenForClient");
                AuthenticationResult authenticationResponse = _taskFactory
                    .StartNew(_msalApplication.AcquireTokenForClient(new[] { msAuthScope }).ExecuteAsync)
                    .Unwrap()
                    .GetAwaiter()
                    .GetResult();

                if (authenticationResponse == null)
                {
                    localContext.Trace($"FrontOffice: Error -> Could not aquire token for client!");
                    throw new Exception(String.Format(errorMessage, clientCertName, travelEndpoint, "Error: Could not aquire token for client!"));
                }

                localContext.Trace($"FrontOffice: Checking AccessToken -> {authenticationResponse?.AccessToken}");

                ticketId = "12345";
                travelEndpoint = "https://sttravelconfiguratorweb.azurewebsites.net"; //CgiSettingEntity.GetSettingString(localContext, "ed_companyintegrationendpoint");
                localContext.Trace($"Endpoint: {travelEndpoint}");

                localContext.Trace($"FrontOffice: Endpoint to use for Travel Cards -> {travelEndpoint}");

                localContext.Trace($"FrontOffice: Building Travel Card GET Call...");

                string uri = $"{travelEndpoint}/ticketdetails/{ticketId}";
                var myUri = new Uri(uri);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + authenticationResponse.AccessToken);
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Headers.Add("20", "*/*");
                myHttpWebRequest.Method = "GET";

                localContext.Trace($"FrontOffice: Calling Travel Card GET Call...");
                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();
                if (responseStream == null) return null;

                var myStreamReader = new StreamReader(responseStream, Encoding.Default);
                var json = myStreamReader.ReadToEnd();

                responseStream.Close();
                myWebResponse.Close();

                localContext.Trace($"GetCardWithCardNumber: Jojo Card GET returned -> {json}");
                return json;
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;

                if (response == null)
                {
                    throw new Exception(String.Format(errorMessage, clientCertName, travelEndpoint, we.Message), we);
                }

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.Trace("FrontOffice returned an exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(String.Format(errorMessage, clientCertName, travelEndpoint, we.Message), we);
                }
            }
        }
    }
}
