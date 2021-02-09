using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.TextMessageSender;
using System;
using System.Collections.Generic;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities.TextMessageSender
{
    public class SingleTextMessage : TextMessageToSend<Generated.ActivityParty>
    {
        private readonly string phonenumber, messageText;

        private readonly SentTextMessageEntity existingSentTextMessage;

        public SingleTextMessage(IEnumerable<Generated.ActivityParty> To, string sender, string mobileNumber, string messageText) : base(sender, To)
        {
            this.phonenumber = mobileNumber;
            this.messageText = messageText;
        }

        public SingleTextMessage(IEnumerable<Generated.ActivityParty> To, string sender, string mobileNumber, string messageText, SentTextMessageEntity existingSentTextMessage) : this(To, sender, mobileNumber, messageText)
        {
            this.existingSentTextMessage = existingSentTextMessage;
        }

        public override IEnumerable<Message> GetMessages(Plugin.LocalPluginContext localContext)
        {
            if (messagesToSend.Count() == 0)
            {
                Message message = new Message()
                {
                    messageText = this.messageText,
                    sender = this.sender
                };

                if(this.phonenumber != null)
                {
                    message.phoneNumber = this.phonenumber;
                }
                else if (this.To != null && this.To.Count() > 0)
                {
                    string concatPhoneNumbers = ConcatAttributesOfActivityParties(localContext, To, "mobilephone", "partyid", ";");
                    message.phoneNumber = concatPhoneNumbers;
                }
                else
                {
                    throw new Exception("Text message has no receiver.");
                }
                

                if(existingSentTextMessage == null)
                {
                    message.ProcessedNotDelivered = GetMessagesToSend(localContext);
                } 
                else
                {
                    IList<SentTextMessageEntity> list = new List<SentTextMessageEntity>();
                    list.Add(existingSentTextMessage);

                    message.ProcessedNotDelivered = list;
                }
                

                messagesToSend.Add(message);
            }
            return messagesToSend;
        }

        private IList<SentTextMessageEntity> GetMessagesToSend(Plugin.LocalPluginContext localContext)
        {
            IList<SentTextMessageEntity> textMessagesToSend = new List<SentTextMessageEntity>();
            if(this.phonenumber != null)
            {
                string[] phoneNumbers = this.phonenumber.Split(';');
                foreach (String number in phoneNumbers)
                {
                    SentTextMessageEntity sentMessage = new SentTextMessageEntity()
                    {
                        ed_PhoneNumber = number,
                        ed_Description = messageText
                    };

                    sentMessage.Id = XrmHelper.Create(localContext, sentMessage);
                    textMessagesToSend.Add(sentMessage);
                }
            }
            else if (this.To != null && this.To.Count() > 0)
            {
                foreach (Generated.ActivityParty a in To)
                {
                    
                    SentTextMessageEntity sentMessage = new SentTextMessageEntity()
                    {
                        ed_Description = messageText
                    };

                    if(a.PartyId.LogicalName == ContactEntity.EntityLogicalName)
                    {
                        // TODO : 
                        // Thelephone2 contains the mobilePhone...
                        ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, a.PartyId, new ColumnSet(ContactEntity.Fields.MobilePhone));

                        sentMessage.ed_PhoneNumber = contact.MobilePhone;
                    }
                    else if (a.PartyId.LogicalName == LeadEntity.EntityLogicalName)
                    {
                        LeadEntity lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, a.PartyId, new ColumnSet(LeadEntity.Fields.MobilePhone));
                        sentMessage.ed_PhoneNumber = lead.MobilePhone;
                    }
                    else
                    {
                        throw new Exception("Recipent must be a Lead or a Contact");
                    }

                    sentMessage.Id = XrmHelper.Create(localContext, sentMessage);
                    textMessagesToSend.Add(sentMessage);
                }
            }
           

            return textMessagesToSend;
        }
        
    }
}
