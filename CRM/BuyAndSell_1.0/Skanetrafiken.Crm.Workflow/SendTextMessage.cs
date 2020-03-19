using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Skanetrafiken.Crm.Entities;
using System.Activities;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm
{
    public class SendTextMessage : CodeActivity
	{

		[Input("TextMessageId")]
        [RequiredArgument()]
        [ReferenceTarget(TextMessageEntity.EntityLogicalName)]
        public InArgument<EntityReference> TextMessageId { get; set; }
        [Output("SendTextMessageResult")]
        public OutArgument<string> SendTextMessageResult { get; set; }

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
			Plugin.LocalPluginContext localContext = GetLocalContext(activityContext);
			
			EntityReference textMessageId = TextMessageId.Get(activityContext);

            ExecuteWorkflow(localContext, textMessageId);

            SendTextMessageResult.Set(activityContext, $"Completed execution of Send Text Message");

        }

        public void ExecuteWorkflow(Plugin.LocalPluginContext localContext, EntityReference textMessageId)
        {

            ColumnSet columnSet = new ColumnSet(
                TextMessageEntity.Fields.ed_PhoneNumber,
                TextMessageEntity.Fields.ed_SenderName,
                TextMessageEntity.Fields.ed_TextMessageTemplateId,
                TextMessageEntity.Fields.Description,
                TextMessageEntity.Fields.To,
                TextMessageEntity.Fields.StateCode,
                TextMessageEntity.Fields.StatusCode
                );

            TextMessageEntity textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, columnSet);
            
            textMessageEntity.SendTextMessage(localContext);
        }

	}
}
