using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.Entities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm
{
    public class SendMarketingInfo : CodeActivity
    {
        [Input("MarketingListId")]
        [RequiredArgument()]
        public InArgument<string> MarketingListId { get; set; }

        [Output("OK")]
        public OutArgument<bool> OK { get; set; }

        [Output("Message")]
        public OutArgument<string> Message { get; set; }

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
            localContext.Trace($"SendMarketingInfo started.");

            // GET VALUE(S)
            string marketingListId = MarketingListId.Get(activityContext);

            if (string.IsNullOrEmpty(marketingListId))
            {
                OK.Set(activityContext, false);
                Message.Set(activityContext, "No Marketing List selected.");
                return;
            }

            // Execute-metod
            try
            {
                marketingListId = marketingListId.Replace("{", "").Replace("}", "");
                Guid gMarketingListId = Guid.Empty;
                
                if(Guid.TryParse(marketingListId, out gMarketingListId)) 
                {
                    MarketingListEntity.SendMarketingInfoResponse response = MarketingListEntity.SendMarketingInfo(localContext, gMarketingListId);
                    localContext.Trace($"SendMarketingInfo finished. Success: {response.OK} | Message: {response.Message}");

                    OK.Set(activityContext, response.OK);
                    Message.Set(activityContext, response.Message);
                }
            }
            catch (Exception ex)
            {
                OK.Set(activityContext, false);
                Message.Set(activityContext, "An error occurred when contacting ROM's API: " + ex.Message);
            }

            localContext.Trace($"SendMarketingInfo finished.");
        }
    }
}
