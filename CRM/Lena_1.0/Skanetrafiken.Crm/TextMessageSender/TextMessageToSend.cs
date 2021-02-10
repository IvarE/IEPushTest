using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;
using Skanetrafiken.Crm.Properties;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.Crm.TextMessageSender
{
    public abstract class TextMessageToSend<T> where T : Entity
    {
        protected readonly string sender;
        protected readonly IEnumerable<T> To;
        protected ICollection<IDictionary<string, string>> messageToSendData;
        protected ICollection<IDictionary<string, object>> expectedTextMessages;
        protected ICollection<string> strings;
        protected ICollection<Message> messagesToSend;

        protected TextMessageToSend(string sender, IEnumerable<T> To)
        {
            if (sender.Length > 11) {
                throw new InvalidOperationException(Resources.SenderNameTooLong);
            }
            this.sender = sender;
            this.To = To;
            messageToSendData = new List<IDictionary<string, string>>();
            expectedTextMessages = new List<IDictionary<string, object>>();
            messagesToSend = new List<Message>();
        }

        abstract public IEnumerable<Message> GetMessages(Plugin.LocalPluginContext localContext);

        protected string ConcatAttributesOfActivityParties(Plugin.LocalPluginContext localContext, IEnumerable<T> activityParties, string attr, string identifier, string separator)
        {

            StringBuilder sb = new StringBuilder();

            foreach (T a in activityParties)
            {
                if (a.Attributes.Contains(identifier) && a.Attributes.Contains(identifier) && a.Attributes[identifier] != null)
                {
                    Entity entity;
                    if (a.Attributes[identifier].GetType() == typeof(Guid))
                    {
                        entity = XrmRetrieveHelper.Retrieve<Entity>(localContext, (Guid)a.Attributes[identifier], new ColumnSet(true));
                    }
                    else if (a.Attributes.Contains(identifier) && a.Attributes[identifier].GetType() == typeof(EntityReference))
                    {
                        entity = XrmRetrieveHelper.Retrieve<Entity>(localContext, (EntityReference)a.Attributes[identifier], new ColumnSet(true));
                    }
                    else
                    {
                        throw new InvalidOperationException(String.Format(Properties.Resources.PartiesHaveNotRequiredAttribute, identifier));
                    }

                    if (entity.Attributes.Contains(attr) && entity.Attributes[attr] != null)
                    {
                        sb.Append(entity.Attributes[attr].ToString()).Append(separator);
                    }
                    else
                    {
                        throw new InvalidOperationException(String.Format(Properties.Resources.PartiesHaveNotRequiredAttribute, attr));
                    }
                }
            }


            sb.Remove(sb.Length - separator.Length, separator.Length);

            return sb.ToString();

        }

        public class Message
        {
            public string sender, phoneNumber, messageText;

            public IList<SentTextMessageEntity> ProcessedNotDelivered;

        }
    }
}
