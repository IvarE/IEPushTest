using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using System.Runtime.Serialization;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm
{
    public class GenerateSlots : CodeActivity
    {

        [Input("ProductID")]
        [RequiredArgument()]
        public InArgument<string> ProductId { get; set; }

        [Input("QuantityPerDay")]
        [RequiredArgument()]
        public InArgument<int> QuantityPerDay { get; set; }

        [Input("StartDate")]
        [RequiredArgument()]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("EndDate")]
        [RequiredArgument()]
        public InArgument<DateTime> EndDate { get; set; }

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
            //TRY EXECUTE
            try
            {
                //throw new InvalidWorkflowException("test");
                //GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);

                localContext.Trace($"GenerateSlots started.");

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

                product.Replace("{", "");
                product.Replace("}", "");


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
