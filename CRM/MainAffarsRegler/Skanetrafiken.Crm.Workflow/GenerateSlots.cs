using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Runtime.Serialization;

namespace Skanetrafiken.Crm.Entities
{
    public class GenerateSlots : CodeActivity
    {
        [Input("ProductID")]
        public InArgument<string> ProductId { get; set; }
        [Input("QuantityPerDay")]
        public InArgument<int> QuantityPerDay { get; set; }

        [Input("StartDate")]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("EndDate")]
        public InArgument<DateTime> EndDate { get; set; }

        [Output("OK")]
        public OutArgument<string> OK { get; set; }

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
            //TRY EXECUTE
            try
            {
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.TracingService.Trace($"GenerateSlots started.");

                //GET VALUE(S)
                string product = ProductId.Get(activityContext);
                int quantityPerDay = QuantityPerDay.Get(activityContext);
                DateTime startDate = StartDate.Get(activityContext);
                DateTime endDate = EndDate.Get(activityContext);

                if(string.IsNullOrEmpty(product))
                {
                    OK.Set(activityContext, false);
                    Message.Set(activityContext, "No Product selected.");

                    return;
                }

                Guid productId = Guid.Parse(product);

                SlotsEntity.GenerateSlotsResponse response = ExecuteCodeActivity(localContext, productId, quantityPerDay,startDate,endDate);
                localContext.Trace($"GenerateSlots finished. Success: {response.OK} | Message: {response.Message}");

                OK.Set(activityContext, response.OK);
                Message.Set(activityContext, response.Message);

            }
            catch (Exception ex)
            {
                OK.Set(activityContext, false);
                Message.Set(activityContext, $"Kunde inte skapa värdkod. Vänligen kontakta kundtjänst. StripError (GenerateSlots): { ex.Message}");
            }
        }
        public static SlotsEntity.GenerateSlotsResponse ExecuteCodeActivity(Plugin.LocalPluginContext localContext, Guid productId, int quantityPerDay, DateTime startDate, DateTime endDate)
        {
            SlotsEntity.GenerateSlotsResponse response = SlotsEntity.GenerateSlots(localContext, productId, quantityPerDay, startDate, endDate);

            return response;
        }
    }

    
}
