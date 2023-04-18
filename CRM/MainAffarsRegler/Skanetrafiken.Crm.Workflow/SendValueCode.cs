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
    public class SendValueCode : CodeActivity
    {
        [Input("ValueCodeId")]
        [RequiredArgument()]
        [ReferenceTarget("ed_valuecode")]
        public InArgument<EntityReference> ValueCodeId { get; set; }

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
                localContext.TracingService.Trace($"SendValueCode started.");


                if (ValueCodeId == null)
                {
                    throw new InvalidPluginExecutionException("Input value code reference is null");
                }

                //GET VALUE(S)
                EntityReference valueCode = ValueCodeId.Get(activityContext);

                string response = ExecuteCodeActivity(localContext, valueCode);
                localContext.Trace($"SendValueCode finished. Responsemessage: {response}. ValueCode: {valueCode.Id}");

                Result.Set(activityContext, response);
            }
            catch (Exception ex)
            {

                //throw new InvalidPluginExecutionException("Error at: " + ex);

                EntityReference valueCodeRef = ValueCodeId.Get(activityContext);
                ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeRef.Id, 
                    new ColumnSet(
                        ValueCodeEntity.Fields.ed_Refund
                    ));

                if (valueCode.ed_Refund != null)
                {
                    HandleErrorValueCode(localContext, valueCode.ed_Refund, ex.Message);
                }

                Result.Set(activityContext, $"Kunde inte skicka ut värdekod. Vänligen kontakta kundtjänst. StripError (SendValueCode): { ex.Message}");

                
            }
        }

        public void HandleErrorValueCode(Plugin.LocalPluginContext localContext, EntityReference refundRef, string errorMessage)
        {
            RefundEntity refund = XrmRetrieveHelper.Retrieve<RefundEntity>(localContext, refundRef.Id, new ColumnSet(
                RefundEntity.Fields.cgi_Caseid
                ));

            IncidentEntity incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refund.cgi_Caseid.Id, new ColumnSet(
                false
                ));

            incident.ReOpenCase(localContext, Schema.Generated.IncidentState.Active);

            refund.SetTransactionFailed(localContext, errorMessage);

            throw new InvalidPluginExecutionException("Kunde inte skicka ut värdekod. Vänligen kontakta kundtjänst: " + errorMessage);

        }

        public static string ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference valueCodeRef)
        {
            ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeRef.Id, new ColumnSet(true));

            if (valueCode == null)
                throw new InvalidPluginExecutionException("Could not get ValueCode");

            valueCode.SendValueCode(localContext);

            return "Value code was sent.";
        }
    }
}
