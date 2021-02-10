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
    public class BlockCardBiztalk : CodeActivity
    {
        [Input("CardNumber")]
        [RequiredArgument()]
        public InArgument<string> CardNumber { get; set; }

        [Input("ReasonCode")]
        [RequiredArgument()]
        public InArgument<int> ReasonCode { get; set; }

        [Output("BiztalkResponse")]
        public OutArgument<string> BiztalkResponse { get; set; }

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

                localContext.Trace($"CreateValueCodeGeneric started.");


                //GET VALUE(S)
                localContext.Trace($"CreateValueCodeGeneric. Get CardNumber");
                string cardNumber = CardNumber.Get(activityContext);
                int reasonCode = ReasonCode.Get(activityContext);

                string biztalkResponse = ExecuteCodeActivity(localContext, cardNumber, reasonCode);

                BiztalkResponse.Set(activityContext, biztalkResponse);
            }
            catch (Exception ex)
            {
                BiztalkResponse.Set(activityContext, $"Kunde inte spärra kort för att få värdekod. Vänligen försök igen eller kontakta kundtjänst. StripError (BlockCardBiztalk): { ex.Message}");
            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string cardNumber, int reasonCode)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");

            return TravelCardEntity.BlockCardBiztalk(localContext, cardNumber, reasonCode);
        }
    }
}
