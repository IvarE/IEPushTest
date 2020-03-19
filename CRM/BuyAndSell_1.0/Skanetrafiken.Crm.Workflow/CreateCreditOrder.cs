using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class CreateCreditOrder : CodeActivity
    {

        [Input("OrderNumber")]
        [RequiredArgument()]

        public InArgument<string> OrderNumber { get; set; }

        [Input("ProductNumber")]
        [RequiredArgument()]

        public InArgument<string> ProductNumber { get; set; }

        [Input("Credit")]
        [RequiredArgument()]

        public InArgument<string> Credit { get; set; }

        [Input("Reason")]
        [RequiredArgument()]

        public InArgument<string> Reason { get; set; }

        [Input("Quantity")]
        [RequiredArgument()]

        public InArgument<string> Quantity { get; set; }

        [Output("CreditOrderResponse")]
        public OutArgument<string> CreditOrderResponse { get; set; }

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
            // generate context
            Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
            localContext.Trace($"CreateCreditOrder started.");

            // get-anrop
            string orderNumber = OrderNumber.Get(activityContext);
            string credit = Credit.Get(activityContext);
            string productNumber = ProductNumber.Get(activityContext);
            string quantity = Quantity.Get(activityContext);

            //Execute-metod
            try
            {
                string soapResponse = ExecuteCodeActivity(localContext, orderNumber, credit, productNumber, quantity);
                CreditOrderResponse.Set(activityContext, soapResponse);
            }
            catch (Exception ex)
            {
                CreditOrderResponse.Set(activityContext, "An error occurred when contacting Orders webservice: " + ex.Message);
            }
            
            localContext.Trace($"CreateCreditOrder finished.");
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string orderNumber, string credit, string productNumber, string quantity)
        {
            // MAKE SOAP REQUEST
            string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns='http://www.skanetrafiken.com/DK/INTSTDK008/CreditOrder/20141031'>" +
                "<soapenv:Header/>" +
                "<soapenv:Body>" +
                "    <ns:CreditOrderRequest>" +
                "        <orderNumber>" + orderNumber + "</orderNumber>" +
                "        <sum>" + credit + "</sum>" +
                "        <ProductNumber>" + productNumber + "</ProductNumber>" +
                "        <Quantity>" + quantity + "</Quantity>" +
                "    </ns:CreditOrderRequest>" +
                "</soapenv:Body>" +
                "</soapenv:Envelope>";
            
            string soapResponse = "";
            string bizTalkUrl = "";

            try
            {
                bizTalkUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BizTalkCreditOrderService);
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when retreiving BizTalk URL: {ex.Message}");
                //throw ex;
                throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
            }

            string[] stringparts = bizTalkUrl.Split('/');
            string soapActionAddress = "";
            for (int x = 0; x < stringparts.Length - 1; x++)
            {
                soapActionAddress += stringparts[x];
                soapActionAddress += "/";
            }

            //todo: call web service, set result to GetOrdersResponse
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // the Content-Type needs to be set to XML
                    client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                    client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                    client.Encoding = Encoding.UTF8;
                    soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                }
            }
            catch (Exception ex)
            {
                localContext.Trace($"An error occurred when contacting BizTalk service: {ex.Message}");
                //throw ex;
                throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
            }
            
            return soapResponse;
        }
    }
}
