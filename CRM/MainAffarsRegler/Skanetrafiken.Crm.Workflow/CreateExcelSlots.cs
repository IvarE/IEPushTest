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

namespace Skanetrafiken.Crm
{
    public class CreateExcelSlots : CodeActivity
    {
        [Input("FromDate")]
        [RequiredArgument()]
        public InArgument<string> FromDate { get; set; }

        [Input("ToDate")]
        [RequiredArgument()]
        public InArgument<string> ToDate { get; set; }

        [Output("ExcelBase64")]
        public OutArgument<string> ExcelBase64 { get; set; }

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
            localContext.Trace($"CreateExcelBase64 started.");

            //TRY EXECUTE
            try
            {
                //GET VALUE(S)
                localContext.Trace($"CreateExcelSlots. Get FromDate and ToDate");

                string fromDate = FromDate.Get(activityContext);
                string toDate = ToDate.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, fromDate, toDate);
                ExcelBase64.Set(activityContext, response);
            }
            catch (Exception e)
            {
                throw new InvalidWorkflowException($"Exception: {e.Message}");
            }

            localContext.Trace($"CreateExcelBase64 finished.");
        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, string fromDate, string toDate)
        {
            localContext.Trace($"(ExecuteCodeActivity) started.");
            return SlotsEntity.HandleCreateExcelBase64(localContext, fromDate, toDate);
        }
    }
}
