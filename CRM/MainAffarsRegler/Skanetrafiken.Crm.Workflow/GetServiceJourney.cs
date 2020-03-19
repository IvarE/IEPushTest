using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class GetServiceJourney : CodeActivity
    {

        [Input("ForServiceJourney")]
        [RequiredArgument()]
        public InArgument<string> ForServiceJourney { get; set; }

        [Input("AtOperatingDate")]
        [RequiredArgument()]
        public InArgument<string> AtOperatingDate { get; set; }

        [Input("AtStopGid")]
        [RequiredArgument()]
        public InArgument<string> AtStopGid { get; set; }

        [Output("GetServiceJourneyResponse")]
        public OutArgument<string> GetServiceJourneyResponse { get; set; }

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
            localContext.Trace($"ServiceJourney started.");

            // GET VALUE(S)
            string forServiceJourney = ForServiceJourney.Get(activityContext);
            string atOperatingDate = AtOperatingDate.Get(activityContext);
            string atStopGid = AtStopGid.Get(activityContext);
            
            // Execute-metod
            try
            {
                string responsetext = ExecuteCodeActivity(localContext, forServiceJourney, atOperatingDate, atStopGid);
                GetServiceJourneyResponse.Set(activityContext, responsetext);
            }
            catch (Exception ex)
            {
                GetServiceJourneyResponse.Set(activityContext, "An error occurred when contacting TravelCard webservice: " + ex.Message);
            }

            localContext.Trace($"GetCardDetails finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string forServiceJourney, string atOperatingDate, string atStopGid)
        {

            // MAKE SOAP REQUEST
            string soapmessage = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanestrafiken.com/DK/INTSTDK001/GetCallsForServiceJourney/20141020'>
                    <soapenv:Header/>
                    <soapenv:Body>
                        <ns:GetCallsForServiceJourney>
                            <forServiceJourneyIdOrGid>{forServiceJourney}<forServiceJourneyIdOrGid>
                            <atOperatingDate>{atOperatingDate}<atOperatingDate>
                            <atStopGid>{atStopGid}<atStopGid>
                            <includeArrivalsTable>true<includeArrivalsTable>
                            <includeDeparturesTable>true<includeDeparturesTable>
                            <includeDeviationTables>true<includeDeviationTables>
                        </ns:GetCallsForServiceJourney>
                    </soapenv:Body>
                </soapenv:Envelope>";

            string soapResponse = "";
            string bizTalkUrl = "";

            //TRY GET SERVICE URL
            try
            {
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkGetServiceJourneyService);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            // REMOVE SUFFIX OF URL
            string[] parts = bizTalkUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < parts.Length - 1; x++)
            {
                soapActionAddress += parts[x];
                soapActionAddress += "/";
            }

            //TRY SEND REQUEST
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");
                    soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                }
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }

            return soapResponse;

        }
    }
}