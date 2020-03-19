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

namespace Skanetrafiken.Crm.Entities
{
    public class CreateMultipleValueCodes : CodeActivity
    {
        [Input("Count")]
        [RequiredArgument()]
        public InArgument<int> Count { get; set; }
        [Input("Amount")]
        [RequiredArgument()]
        public InArgument<int> Amount { get; set; }

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

            localContext.Trace($"CreateMultipleValueCodes started.");

            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                int count = Count.Get(activityContext);
                float amount = Amount.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, count, amount);
                localContext.Trace($"CreateMultipleValueCodes finished. Responsemessage: {response}");
            }
            catch (Exception ex)
            {
                //localContext.Trace($"BlockAccountPortal finished with exception: {ex.Message}");

                throw new InvalidPluginExecutionException($"CreateMultipleValueCodes failed. Exception: {ex.Message}");

            }
        }
        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, int count, float amount)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    ValueCodeTemplateEntity template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(true));
                    //ValueCodeHandler.CreateValueCode(localContext, amount, Schema.Generated.ed_valuecode_ed_typeoption.Mobile, template);
                }

                return "Value Codes created";
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("(CreateMultipleValueCodes) Error: " + ex);
            }
        }
    }
}
