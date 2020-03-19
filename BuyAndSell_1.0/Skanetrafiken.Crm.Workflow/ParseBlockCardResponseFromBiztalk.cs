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
using static Skanetrafiken.Crm.Entities.TravelCardEntity;

namespace Skanetrafiken.Crm
{
    public class ParseBlockCardResponseFromBiztalk : CodeActivity
    {
        [Input("BiztalkResponse")]
        [RequiredArgument()]
        public InArgument<string> BiztalkResponse { get; set; }

        [Output("RequestCardBlockResult")]
        public OutArgument<string> RequestCardBlockResult { get; set; }


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

                localContext.Trace($"ParseBlockCardResponseFromBiztalk started.");


                //GET VALUE(S)
                localContext.Trace($"ParseBlockCardResponseFromBiztalk. Get Contact");
                string biztalkResponse = BiztalkResponse.Get(activityContext);

                CardBlockEnvelope.Envelope envelope = ExecuteCodeActivity(localContext, biztalkResponse);

                if (envelope != null &&
                    envelope.Body != null &&
                    envelope.Body.RequestCardBlockResponse != null &&
                    envelope.Body.RequestCardBlockResponse.RequestCardBlockResult != null)
                {
                    RequestCardBlockResult.Set(activityContext, envelope.Body.RequestCardBlockResponse.RequestCardBlockResult.ToString());
                }
            }
            catch (Exception ex)
            {
                // TODO - returnera text från fält.
                RequestCardBlockResult.Set(activityContext, $"Kunde inte spärra kort för att få värdekod. Vänligen försök igen eller kontakta kundtjänst. StripError (ParseBlockCardResponseFromBiztalk): { ex.Message}");
            }
        }

        public static CardBlockEnvelope.Envelope ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            CardBlockEnvelope.Envelope envelope = null;

            envelope = TravelCardEntity.ParseCardBlockResponse(biztalkResponse);

            return envelope;
        }
    }
}
