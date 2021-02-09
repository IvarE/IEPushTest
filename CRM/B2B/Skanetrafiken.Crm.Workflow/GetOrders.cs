using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class GetOrders : CodeActivity
    {

        [Input("EmailAddress")]
        [RequiredArgument()]
        public InArgument<string> EmailAddress { get; set; }   // {EmailAddress(Arguments)}

        [Input("CardNumber")]
        [RequiredArgument()]
        public InArgument<string> CardNumber { get; set; }

        [Input("OrderNumber")]
        [RequiredArgument()]
        public InArgument<string> OrderNumber { get; set; }

        [Input("StartDate")]
        [RequiredArgument()]
        public InArgument<string> StartDate { get; set; }

        [Input("EndDate")]
        [RequiredArgument()]
        public InArgument<string> EndDate { get; set; }

        [Output("GetOrdersResponse")]
        public OutArgument<string> GetOrdersResponse { get; set; }

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
            localContext.Trace($"GetOrders started.");

            //GET VALUE(S)
            string emailAddress = EmailAddress.Get(activityContext);
            string cardNumber = CardNumber.Get(activityContext);
            string orderNumber = OrderNumber.Get(activityContext);
            string startDate = StartDate.Get(activityContext);
            string endDate = EndDate.Get(activityContext);

            //TRY EXECUTE
            try
            {
                string soapResponse = ExecuteCodeActivity(localContext, emailAddress, cardNumber, orderNumber, startDate, endDate);
                GetOrdersResponse.Set(activityContext, soapResponse);
            }
            catch (Exception ex)
            {
                GetOrdersResponse.Set(activityContext, ex.Message);
            }

            localContext.Trace($"GetOrders finished.");

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string emailAddress, string cardNumber, string orderNumber, string startDate, string endDate)
        {
            // MAKE SOAP REQUEST
            string soapMessage = $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK008/GetOrders/20141031'>
                    <soapenv:Header/>
                    <soapenv:Body>
                    <ns:GetOrdersRequest>
                    <Email>{emailAddress}</Email>
                    <CardNumber>{cardNumber}</CardNumber>
                    <OrderNumber>{orderNumber}</OrderNumber>
                    <From>{startDate}</From>
                    <To>{endDate}</To>
                    </ns:GetOrdersRequest>
                    </soapenv:Body>
                    </soapenv:Envelope>";

            string soapResponse = "";
            string bizTalkOrderServiceUrl = "";

            //TRY GET SERVICE URL
            try
            {
                bizTalkOrderServiceUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkOrderService);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            //REMOVE SUFFIX OF URL
            string[] parts = bizTalkOrderServiceUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < parts.Length - 1; x++)
            {
                soapActionAddress += parts[x];
                soapActionAddress += "/";
            }
            
            //TRY SEND REQUEST
            try {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");
                    soapResponse = client.UploadString("" + bizTalkOrderServiceUrl + "", soapMessage);
                }
            }
            catch(Exception ex)
            {
                localContext.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }
            
            return soapResponse;

        }
    }
}