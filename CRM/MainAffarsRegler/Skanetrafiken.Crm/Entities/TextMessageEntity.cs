using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities.TextMessageSender;
using Skanetrafiken.Crm.TextMessageSender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class TextMessageEntity : Generated.ed_TextMessage
	{
        public enum Status
        {
            NotSent = 1,
            Sent = 899310003,
            Delivered = 2,
            Failed = 899310004
        };

        private static BosbecAPIHandler APIHandler;

        private void CheckIfReadyToSend()
        {
            if (this.StatusCode.Value == (int)TextMessageEntity.Status.Delivered)
            {
                throw new InvalidOperationException(Properties.Resources.TextMessageCannotBeSentIfDelivered);
            }

            if (this.To == null && this.ed_PhoneNumber == null)
            {
                throw new InvalidOperationException(Properties.Resources.TextMessageMustHaveReceiver);
            }

            if(this.Description == null && (this.ed_TextMessageTemplateId == null || this.ed_TextMessageTemplateId.Id == null))
            {
                throw new InvalidOperationException(Properties.Resources.TextMessageMustHaveContent);
            }

            if (this.ed_TextMessageTemplateId != null && this.ed_TextMessageTemplateId.Id != null && (this.To == null || this.To.Count() == 0))
            {
                throw new InvalidOperationException(Properties.Resources.TextMessageMustHaveToRecipient);
            }
            if (this.ed_SenderName == null)
            {
                throw new InvalidOperationException(Properties.Resources.TextMessageMustHaveSender);
            }

            ValidatePhoneNumber();
        }

        private void ValidatePhoneNumber() {
            // to be implemented
        }

        private static void RetrieveBosbecAPIHandler(Plugin.LocalPluginContext localContext)
        {
            if (TextMessageEntity.APIHandler == null)
            {
                string apiKey = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BosbecAPIKey);
                string bosbecUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_Bosbecurl);
                string workflowId = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_BosbecWorkflowId);

                APIHandler = new BosbecAPIHandler()
                {
                    apiKey = apiKey,
                    workflowId = workflowId,
                    baseUrlStr = bosbecUrl
                };
            }
        }

        private static double GetTotalTimeLimitMillis(Plugin.LocalPluginContext localContext)
        {
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_SMSTimeLimit));
            double timeLimitSeconds = settings.ed_SMSTimeLimit ?? 0;
            return timeLimitSeconds * 1000;
        }

        public void ResendTextMessage(Plugin.LocalPluginContext localContext, SentTextMessageEntity sentTextMessage, double TimeLimitMillis)
        {
            CheckIfReadyToSend();

            TextMessageToSend<Generated.ActivityParty> textMessageToSend = GetTextMessageToSend(sentTextMessage);

            IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messagesToSend = textMessageToSend.GetMessages(localContext);

            SendTextMessageToSend(localContext, messagesToSend, TimeLimitMillis);
        }

        public void SendTextMessage(Plugin.LocalPluginContext localContext)
        {
            CheckIfReadyToSend();

            TextMessageToSend<Generated.ActivityParty> textMessageToSend = GetTextMessageToSend();

            IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messagesToSend = textMessageToSend.GetMessages(localContext);

            double TimeLimitMillis = GetTotalTimeLimitMillis(localContext) / messagesToSend.Count();

            SendTextMessageToSend(localContext, messagesToSend, TimeLimitMillis);

            IList<SentTextMessageEntity> ProcessedNotDeliveredMessages = GetProcessedSentTextMessages(localContext, messagesToSend);

            UpdateSentMessagesStatus(localContext, ProcessedNotDeliveredMessages, TimeLimitMillis);

            UpdateTextMessageStatus(localContext, this);

        }

        public static void UpdateTextMessageStatus(Plugin.LocalPluginContext localContext, TextMessageEntity textMessage)
        {
            if(textMessage.StatusCode.Value != (int)TextMessageEntity.Status.Delivered && IsAllMessagesDeliveredAndSent(localContext, textMessage))
            {
                textMessage.StateCode = Generated.ed_TextMessageState.Completed;
                textMessage.StatusCode = new OptionSetValue((int)TextMessageEntity.Status.Delivered);
                XrmHelper.Update(localContext, textMessage);

                if(textMessage.RegardingObjectId != null && textMessage.RegardingObjectId.LogicalName == ValueCodeEntity.EntityLogicalName)
                {
                    ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, textMessage.RegardingObjectId, new ColumnSet(false));
                    valueCode.statuscode = Generated.ed_valuecode_statuscode.Skickad;
                    XrmHelper.Update(localContext, valueCode);
                }
            }
        }

        public static bool IsAllMessagesDeliveredAndSent(Plugin.LocalPluginContext localContext, TextMessageEntity textMessage)
        {
            IList<SentTextMessageEntity> AllMessages = GetSentTextMessages(localContext, textMessage);

            foreach (SentTextMessageEntity s in AllMessages)
            {
                if(s.statuscode.Value != (int)SentTextMessageEntity.Status.Delivered && s.statuscode.Value != (int)SentTextMessageEntity.Status.Sent)
                    return false;
            }

            return true;
        }

        public static IList<SentTextMessageEntity> GetSentTextMessages(Plugin.LocalPluginContext localContext, TextMessageEntity textMessage)
        {
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessage.Id);

            return XrmRetrieveHelper.RetrieveMultiple<SentTextMessageEntity>(localContext, new ColumnSet(true), filter);
        }

        private void SendTextMessageToSend(Plugin.LocalPluginContext localContext, IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messages, double TimeLimitMillis) {

            Stopwatch sw = new Stopwatch();

           
            RetrieveBosbecAPIHandler(localContext);
            foreach (TextMessageToSend<Generated.ActivityParty>.Message m in messages)
            {
                sw.Restart();
                string processId = SendRequestAndGetProcessId(sw, m.phoneNumber, m.sender, m.messageText, TimeLimitMillis);
                
                foreach(SentTextMessageEntity sentMessage in m.ProcessedNotDelivered)
                {

                    sentMessage.ed_TextMessageId = this.ToEntityReference();
                    if(RegardingObjectId != null)
                    {
                        sentMessage.ed_ValueCode = this.RegardingObjectId.LogicalName == ValueCodeEntity.EntityLogicalName ? this.RegardingObjectId : null;
                    }

                    if (processId != null)
                    {
                        sentMessage.statuscode = new OptionSetValue((int)SentTextMessageEntity.Status.Processed);
                        sentMessage.ed_ProcessId = processId;
                    }
                    else
                    {
                        sentMessage.statuscode = new OptionSetValue((int)SentTextMessageEntity.Status.NotProcessed);
                    }

                    XrmHelper.Update(localContext, sentMessage);
                }
            }
        }

        public void UpdateSentMessagesStatus(Plugin.LocalPluginContext localContext, IList<SentTextMessageEntity> ProcessedNotDeliveredMessages, double TimeLimitMillis)
        {
            RetrieveBosbecAPIHandler(localContext);
            Stopwatch sw = new Stopwatch();
            
            foreach(SentTextMessageEntity SentMessage in ProcessedNotDeliveredMessages)
            {
                sw.Restart();
                while (sw.ElapsedMilliseconds < TimeLimitMillis)
                {
                    int oldStatus = SentMessage.statuscode.Value;

                    if (oldStatus == (int)SentTextMessageEntity.Status.NotProcessed)
                    {
                        ResendTextMessage(localContext, SentMessage, TimeLimitMillis);
                    }
                    else
                    {
                        SentMessage.RetrieveUpdatedStatus(localContext);

                        if (SentMessage.statuscode.Value != oldStatus)
                        {
                            XrmHelper.Update(localContext, SentMessage);

                            if (SentMessage.statuscode.Value == (int)SentTextMessageEntity.Status.Delivered || SentMessage.statuscode.Value == (int)SentTextMessageEntity.Status.Failed)
                            {
                                break;
                            }
                        }
                    }
                }                
            }
        }

        
        private IList<SentTextMessageEntity> GetProcessedSentTextMessages(Plugin.LocalPluginContext localContext, IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messages)
        {
            IList<SentTextMessageEntity> ProcessedMessages = new List<SentTextMessageEntity>();

            this.StatusCode = new OptionSetValue((int)TextMessageEntity.Status.Sent);

            foreach (TextMessageToSend<Generated.ActivityParty>.Message m in messages)
            {
                foreach(SentTextMessageEntity sentTextMessage in m.ProcessedNotDelivered)
                {
                    if(sentTextMessage.statuscode.Value == (int)SentTextMessageEntity.Status.Processed)
                    {
                        ProcessedMessages.Add(sentTextMessage);
                    }
                    else if(StatusCode.Value != (int)TextMessageEntity.Status.Failed)
                    {
                        this.StatusCode = new OptionSetValue((int)TextMessageEntity.Status.Failed);
                    }
                }
                
                XrmHelper.Update(localContext, this);
            }

            return ProcessedMessages;
        }

        private static string SendRequestAndGetProcessId(Stopwatch sw, string phoneNumber, string sender, string messageText, double TimeLimitMillis)
        {
            BosbecAPIHandler.TextMessageResponseBase sendResponse;
            try
            {
                if (sw.ElapsedMilliseconds < TimeLimitMillis)
                {
                    sendResponse = APIHandler.SendTextMessage(phoneNumber, sender, messageText);
                }
                else
                {
                    return null;
                }
            }
            catch (WebException)
            {
                return SendRequestAndGetProcessId(sw, phoneNumber, sender, messageText, TimeLimitMillis);
            }

            return sendResponse.processId;
        }

        private TextMessageToSend<Generated.ActivityParty> GetTextMessageToSend()
        {
            if (this.To != null && this.ed_TextMessageTemplateId != null)
            {
                return new MultipleTextMessage(this.To, this.ed_SenderName, this.ed_TextMessageTemplateId);
            }
            else
            {
                return new SingleTextMessage(this.To, this.ed_SenderName, this.ed_PhoneNumber, this.Description);
            }
        }

        private TextMessageToSend<Generated.ActivityParty> GetTextMessageToSend(SentTextMessageEntity sentMessage)
        {
            return new SingleTextMessage(null, this.ed_SenderName, sentMessage.ed_PhoneNumber, sentMessage.ed_Description, sentMessage);
        }
    }
}
