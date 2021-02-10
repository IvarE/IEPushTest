using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Net;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.ValueCodes;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class CancelOrder : CodeActivity
    {
        [Input("CardNumber")]
        [RequiredArgument()]
        public InArgument<string> CardNumber { get; set; }

        [Output("CancelOrderResponse")]
        public OutArgument<string> CancelOrderResponse { get; set; }

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
            //TRY EXECUTE
            try
            {
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.Trace($"CanceleOrder started.");


                //GET VALUE(S)
                localContext.Trace($"CanceleOrder. Get CardNumber");
                string cardNumber = CardNumber.Get(activityContext);

                string cancelOrderResponse = ExecuteCodeActivity(localContext, cardNumber);

                CancelOrderResponse.Set(activityContext, cancelOrderResponse);
            }
            catch (Exception ex)
            {
                CancelOrderResponse.Set(activityContext, $"Kunde inte spärra kort för att få värdekod. Vänligen försök igen eller kontakta kundtjänst. StripError (CancelOrder): { ex.Message }");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string cardNumber)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            return TravelCardEntity.HandleCancelOrder(localContext, cardNumber);
        }
    }
}
