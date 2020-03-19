using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
    public class UpdateTextMessageStatus : CodeActivity
    {
        [Input("TextMessageId")]
        [RequiredArgument()]
        [ReferenceTarget(TextMessageEntity.EntityLogicalName)]
        public InArgument<EntityReference> TextMessageReference { get; set; }

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
            try
            {
                ExecuteCodeActivity(localContext, TextMessageReference.Get(activityContext));
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error caught in UpdateSentMessageStatus:\n{0}", e.ToString()));
            }
        }

        public void ExecuteCodeActivity(Plugin.LocalPluginContext localContext, EntityReference TextMessageId)
        {

            TextMessageEntity textMessage = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, TextMessageId, new ColumnSet(true));

            if(textMessage.StatusCode.Value == (int)TextMessageEntity.Status.Delivered)
            {
                return;
            }

            IList<SentTextMessageEntity> SentTextMessages = TextMessageEntity.GetSentTextMessages(localContext, textMessage);

            double TimeLimitMillis = GetTotalTimeLimitMillis(localContext) / SentTextMessages.Count;

            textMessage.UpdateSentMessagesStatus(localContext, SentTextMessages, TimeLimitMillis);

            TextMessageEntity.UpdateTextMessageStatus(localContext, textMessage);

        }

        private static double GetTotalTimeLimitMillis(Plugin.LocalPluginContext localContext)
        {
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_SMSTimeLimit));
            double timeLimitSeconds = settings.ed_SMSTimeLimit ?? 0;
            return timeLimitSeconds * 1000;
        }
    }
}
