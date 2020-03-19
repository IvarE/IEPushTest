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

namespace Skanetrafiken.Crm.Entities
{
    public class CancelValueCode : CodeActivity
    {
        [Input("ValueCodeId")]
        [RequiredArgument()]
        public InArgument<string> ValueCodeId { get; set; }

        [Output("Result")]
        public OutArgument<string> Result { get; set; }

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

            //TRY EXECUTE
            try
            {
                localContext.TracingService.Trace($"CancelValueCode started.");

                localContext.TracingService.Trace($"Checking Values started...");

                localContext.TracingService.Trace($"ValueCodeId is not Null.");

                String valueCode = ValueCodeId.Get(activityContext);

                if (!String.IsNullOrWhiteSpace(valueCode))
                {
                    localContext.TracingService.Trace("The Refrence ID string is: ");
                    //localContext.TracingService.Trace(valueCode);
                    //TODO
                    String response = "";
                    if (valueCode.Contains(";") == true)
                    {
                        //Parse value code string
                        string[] valueCodeArray = valueCode.Split(';');

                        foreach (var valueCodeId in valueCodeArray)
                        {
                            if (!String.IsNullOrWhiteSpace(valueCodeId))
                            {
                                response = ExecuteCodeActivity(localContext, valueCodeId);
                            }
                        }
                    }
                    else
                    {
                        response = ExecuteCodeActivity(localContext, valueCode);
                    }
                }
                else
                {
                    localContext.TracingService.Trace($"ValueCodeId is null.");
                    throw new Exception("Did not find any Value Code to Cancel.");
                }

                Result.Set(activityContext, "Finished Execution.");

            }
            catch (Exception ex)
            {
                localContext.Trace($"Error when Caneling ValueCode: " + ex.Message);
                //throw new InvalidPluginExecutionException("Error at: " + ex);

            }
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string valueCodeRef)
        {
            ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, new Guid(valueCodeRef), new ColumnSet(true));

            if (valueCode == null)
                throw new InvalidPluginExecutionException("Could not get ValueCode");
            
            valueCode.CancelValueCode(localContext);

            return "Value code was canceled.";
        }
    }
}
