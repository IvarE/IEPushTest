using System;
using System.Text;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.Entities
{
    public class GetFormattedDateString: CodeActivity
    {

        [Input("InputDate")]
        [RequiredArgument()]
        public InArgument<DateTime> InputDate { get; set; }

        [Input("InputFormat")]
        [RequiredArgument()]
        public InArgument<string> InputFormat { get; set; }

        [Output("TimeString")]
        public OutArgument<string> TimeString { get; set; }

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

                DateTime inputDate = InputDate.Get(activityContext);
                DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(inputDate, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

                string inputFormat = InputFormat.Get(activityContext);

                if (inputDate == null)
                {
                    throw new InvalidWorkflowException("Input date can not be empty");
                }
                else
                {
                    TimeString.Set(activityContext, localDate.ToString(inputFormat));

                    localContext.Trace($"GetFormattedDateString finished.");
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