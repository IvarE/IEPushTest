using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class GetCardDetails : CodeActivity
    {

        [Input("TravelCardNumber")]
        [RequiredArgument()]

        public InArgument<string> TravelCardNumber { get; set; }

        [Output("CardDetailsResponse")]
        public OutArgument<string> CardDetailsResponse { get; set; }

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
                // GENERATE CONTEXT
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
                localContext.Trace($"GetCardDetails started.");

                // GET VALUE(S)
                string travelCardNumber = TravelCardNumber.Get(activityContext);


                string soapResponse = TravelCardEntity.GetCardDetails(localContext, travelCardNumber);
                CardDetailsResponse.Set(activityContext, soapResponse);

                localContext.Trace($"GetCardDetails finished.");

            }
            catch (Exception ex)
            {
                // TODO - returnera text från fält.
                CardDetailsResponse.Set(activityContext, $"Kunde inte hämta kortuppgifter. Vänligen försök igen eller kontakta kundtjänst. StripError (GetCardDetails): {ex.Message}");
            }


        }

    }
}