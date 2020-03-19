using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.Entities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm.Entities
{
    public class GetCardBalance : CodeActivity
    {
        [Input("TravelCardNumber")]
        [RequiredArgument()]
        public InArgument<string> TravelCardNumber { get; set; }

        [Output("Balance")]
        public OutArgument<decimal> Balance { get; set; }

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

            localContext.TracingService.Trace($"GetCardBalance started.");

            //TRY EXECUTE
            try
            {
                var travelCardNumber = TravelCardNumber.Get(activityContext);
                var balance = ExecuteCodeActivity(localContext, travelCardNumber);

                Balance.Set(activityContext, balance);

                localContext.TracingService.Trace($"GetCardBalance exited.");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"GetCardBalance failed. Exception: {ex.Message}");
            }
        }

        public static decimal ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(travelCardNumber))
                    throw new InvalidPluginExecutionException($"Argument {travelCardNumber} is null.");
                localContext.Trace($"Travel card number: {travelCardNumber}");

                localContext.Trace($"Fetching card from BIFF");
                var cardDetail = TravelCardEntity.GetAndParseCardDetails(localContext, travelCardNumber);

                localContext.Trace($"Validate card balance");
                var balance = TravelCardEntity.GetCardBalance(localContext, cardDetail);
                localContext.Trace($"Balance to return from BIFF: {balance}");

                return balance;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"(GetCardBalance) Error: {ex}");
            }
        }
    }
}
