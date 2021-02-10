using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.Entities
{
    public class PrintRefundReports: CodeActivity
    {

        [Input("StartDate")]
        [RequiredArgument()]
        public InArgument<DateTime> StartDate { get; set; }

        [Input("EndDate")]
        [RequiredArgument()]
        public InArgument<DateTime> EndDate { get; set; }
        
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
                Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
                localContext.Trace($"Execute PrintRefundReports.");

                DateTime startDate = StartDate.Get(activityContext);
                DateTime endDate = EndDate.Get(activityContext);

                //RefundEntity.PrintRefunds(localContext, startDate, endDate);

                localContext.Trace($"PrintRefundReports finished.");

            }
            catch (InvalidWorkflowException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new InvalidWorkflowException(e.Message);
            }
        }
    }
}