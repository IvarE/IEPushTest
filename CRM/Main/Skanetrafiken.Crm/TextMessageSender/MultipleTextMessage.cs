using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.TextMessageSender
{
    public class MultipleTextMessage : TextMessageToSend<Generated.ActivityParty>
    {
        private readonly EntityReference templateReference;

        public MultipleTextMessage(IEnumerable<Generated.ActivityParty> To, string sender, EntityReference templateReference) : base(sender, To)
        {
            this.templateReference = templateReference;
        }

        public override IEnumerable<Message> GetMessages(Plugin.LocalPluginContext localContext)
        {
            if (messagesToSend.Count() == 0)
            {
                TextMessageTemplateEntity template = XrmRetrieveHelper.Retrieve<TextMessageTemplateEntity>(localContext, this.templateReference, new ColumnSet(TextMessageTemplateEntity.Fields.ed_Description));
                string templateText = template.ed_Description;
                foreach (Generated.ActivityParty a in this.To)
                {
                    Entity entity = XrmRetrieveHelper.Retrieve<Entity>(localContext, a.PartyId, new ColumnSet(true));
                    if (entity.Attributes.Contains("mobilephone"))
                    {
                        // TODO: handle if prefix is different from used entity?
                        // TODO: Add functionality of datetime
                        string messageText = ReplaceWithEntityData(localContext, templateText, entity, entity.LogicalName, null);

                        SentTextMessageEntity MessageToSend = new SentTextMessageEntity()
                        {
                            ed_PhoneNumber = (string)entity.Attributes["mobilephone"],
                            ed_Description = messageText
                        };

                        MessageToSend.Id = XrmHelper.Create(localContext, MessageToSend);

                        IList<SentTextMessageEntity> MessagesToBeProcessed = new List<SentTextMessageEntity>();
                        MessagesToBeProcessed.Add(MessageToSend);
                        
                        Message message = new Message()
                        {
                            phoneNumber = (string)entity.Attributes["mobilephone"],
                            messageText = messageText,
                            sender = this.sender,
                            ProcessedNotDelivered = MessagesToBeProcessed
                        };

                        messagesToSend.Add(message);

                    }
                    else
                    {
                        throw new InvalidOperationException(Properties.Resources.RecipientsMustHavePhoneNumber);
                    }
                }
            }

            return messagesToSend;
        }

        private static string ReplaceWithEntityData(Plugin.LocalPluginContext localContext, string text, Entity entity, string prefix, Guid? systemUserId)
        {
            foreach (KeyValuePair<string, object> attribute in entity.Attributes)
            {
                if (text.Contains("{!" + prefix + ":" + attribute.Key + ";}"))
                {

                    string value = "";
                    switch (attribute.Value.GetType().ToString())
                    {
                        case "Microsoft.Xrm.Sdk.Money":
                            value = ((Money)attribute.Value).Value.ToString();
                            break;
                        case "Microsoft.Xrm.Sdk.OptionSetValue":
                            value = ((OptionSetValue)attribute.Value).Value.ToString();
                            break;
                        case "Microsoft.Xrm.Sdk.EntityReference":
                            value = ((EntityReference)attribute.Value).Name.ToString();
                            break;
                        case "System.DateTime":
                            // TODO: Add functionality of datetime
                            //value = GetFormattedDate(localContext, (DateTime)attribute.Value, systemUserId);
                            break;
                        case "Microsoft.Xrm.Sdk.EntityCollection":
                            EntityCollection entityCollection = (EntityCollection)attribute.Value;
                            if (entityCollection.Entities.Count > 0)
                            {
                                EntityReference entityReference = (EntityReference)entityCollection.Entities[0]["partyid"];
                                value = entityReference.Name;
                            }
                            break;
                        default:
                            value = attribute.Value.ToString();
                            break;
                    }

                    //text = text + " " + attribute.Value.GetType().ToString();

                    text = text.Replace("{!" + prefix + ":" + attribute.Key + ";}", value);
                }
            }
            text = Regex.Replace(text, @"\{!" + prefix + @":[^}]+\}", "");

            return text;
        }

        private static string GetFormattedDate(Plugin.LocalPluginContext localContext, DateTime date, Guid systemUserId)
        {
            string formattedDate = date.ToString();

            // Get user settings
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(UserSettingsEntity.Fields.SystemUserId, ConditionOperator.Equal, systemUserId);
            ColumnSet columnSet = new ColumnSet(UserSettingsEntity.Fields.DateFormatString, UserSettingsEntity.Fields.DateSeparator, UserSettingsEntity.Fields.TimeFormatString, UserSettingsEntity.Fields.TimeZoneCode);
            Entity userSettings = XrmHelper.RetrieveFirst(localContext, UserSettingsEntity.EntityLogicalName, columnSet, filter);

            // Set user timezone
            if (userSettings != null && userSettings.Contains(UserSettingsEntity.Fields.TimeZoneCode))
            {
                LocalTimeFromUtcTimeRequest request = new LocalTimeFromUtcTimeRequest
                {
                    TimeZoneCode = (Int32)userSettings[UserSettingsEntity.Fields.TimeZoneCode],
                    UtcTime = date.ToUniversalTime()
                };
                LocalTimeFromUtcTimeResponse response = (LocalTimeFromUtcTimeResponse)localContext.OrganizationService.Execute(request);
                date = response.LocalTime;
            }

            // String format string
            if (userSettings != null && userSettings.Contains("dateformatstring") && userSettings.Contains("dateseparator") && userSettings.Contains("timeformatstring"))
            {
                string dateFormat = userSettings["dateformatstring"].ToString().Replace("/", userSettings["dateseparator"].ToString()) + " " + userSettings["timeformatstring"].ToString();
                formattedDate = date.ToString(dateFormat);
            }

            return formattedDate;
        }

    }
}
