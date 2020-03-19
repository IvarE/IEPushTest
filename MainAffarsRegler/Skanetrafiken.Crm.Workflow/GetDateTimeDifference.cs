using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.Entities
{
    public class GetDateTimeDifference: CodeActivity
    {

        [Input("PlannedDate")]
        [RequiredArgument()]
        public InArgument<DateTime> PlannedDate { get; set; }

        [Input("ActualDate")]
        [RequiredArgument()]
        public InArgument<DateTime> ActualDate { get; set; }

        [Output("TimeDiff")]
        public OutArgument<string> TimeDiff { get; set; }

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
                localContext.Trace($"Exexute GetTravelInformationTimeString.");

                DateTime plannedDate = PlannedDate.Get(activityContext);
                DateTime actualDate = ActualDate.Get(activityContext);

                if (plannedDate == null || actualDate == null)
                {
                    throw new InvalidWorkflowException("Input dates can not be empty");
                }
                else
                {
                    string diff = ((int)actualDate.Subtract(plannedDate).TotalMinutes).ToString("+0;-#");
                    
                    TimeDiff.Set(activityContext, diff);

                    localContext.Trace($"GetTravelInformationTimeString finished.");
                }
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