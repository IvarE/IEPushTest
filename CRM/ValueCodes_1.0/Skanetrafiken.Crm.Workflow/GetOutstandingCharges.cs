using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class GetOutstandingCharges : CodeActivity
    {

        [Input("TravelCardNumber")]
        [RequiredArgument()]

        public InArgument<string> TravelCardNumber { get; set; }

        [Output("GetOutstandingChargesResponse")]
        public OutArgument<string> GetOutstandingChargesResponse { get; set; }

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
            localContext.Trace($"GetOutstandingCharges started.");

            //GET VALUES
            string travelCardNumber = TravelCardNumber.Get(activityContext);

            //TRY EXECUTE
            try
            {
                string soapResponse = ExecuteCodeActivity(localContext, travelCardNumber);
                GetOutstandingChargesResponse.Set(activityContext, soapResponse);
            }
            catch (Exception ex)
            {
                GetOutstandingChargesResponse.Set(activityContext, "An error occurred when contacting TravelCard webservice: " + ex.Message);
            }

            localContext.Trace($"GetOutstandingCharges finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            //MAKE SOAP REQUEST
            string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK008.Card/GetOutstandingCharges/20150310'>" +
            "   <soapenv:Header/>" +
            "       <soapenv:Body>" +
            "           <ns:OutstandingChargesRequest>" +
            "               <CardNumber>" + travelCardNumber + "</CardNumber>" +
            "           </ns:OutstandingChargesRequest>" +
            "   </soapenv:Body>" +
            "</soapenv:Envelope>";

            string soapResponse = "";
            string bizTalkUrl = "";

            //TRY GET SERVICE URL
            try
            {
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkGetOutstandingChargesService);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            //REMOVE SUFFIX OF URL
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
