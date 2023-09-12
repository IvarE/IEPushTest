using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Entities.TextMessageSender;
using Skanetrafiken.Crm.TextMessageSender;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.IntegrationTests
{
	[TestFixture]
	class TextMessageFixture : PluginFixtureBase
	{
		private ServerConnection _serverConnection;

        
		[Test]
		public void TestMultipleTextMessageWithTemplate()
		{
			using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
			{
				// This statement is required to enable early-bound type support.
				_serviceProxy.EnableProxyTypes();

				Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());
				
				ContactEntity contact1 = new ContactEntity()
				{
					ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
					FirstName = "Contact1FirstName",
					LastName = "Contact1LastName",
					MobilePhone = "+46700000000"
				};
				// Create contact & Activity Party
				Guid contact1Id = XrmHelper.Create(localContext, contact1);
				contact1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1Id, new ColumnSet(true));
				ActivityPartyEntity contact1ActivityParty = new ActivityPartyEntity()
				{
					PartyId = contact1.ToEntityReference() 
				};

				ContactEntity contact2 = new ContactEntity()
				{
					ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
					FirstName = "Contact2FirstName",
					LastName = "Contact2LastName",
					MobilePhone = "+46700000001"
                };
				// Create contact & Activity Party
				Guid contact2Id = XrmHelper.Create(localContext, contact2);
				contact2 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact2Id, new ColumnSet(true));
				ActivityPartyEntity contact2ActivityParty = new ActivityPartyEntity()
				{
					PartyId = contact2.ToEntityReference()
				};

				IEnumerable<Generated.ActivityParty> receivers = new List<Generated.ActivityParty>() {
					contact1ActivityParty, contact2ActivityParty
				};

				// Create template
				TextMessageTemplateEntity template = new TextMessageTemplateEntity()
				{
					ed_name = "Test template",
					ed_Description = "Hello {!contact:firstname;} {!contact:lastname;}"
				};
				Guid templateId = XrmHelper.Create(localContext, template);

                TextMessageToSend<Generated.ActivityParty> multipleTextMessage = new MultipleTextMessage(receivers, "sender", new EntityReference(TextMessageTemplateEntity.EntityLogicalName,templateId));

                IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messages = multipleTextMessage.GetMessages(localContext);
                Assert.AreEqual(2, messages.Count());
				foreach (TextMessageToSend<Generated.ActivityParty>.Message message in messages)
				{
                    Assert.AreEqual(1, message.ProcessedNotDelivered.Count());

                    if (message.phoneNumber.Equals("+46700000000"))
					{
						Assert.AreEqual("Hello Contact1firstname Contact1lastname", message.messageText);
                        Assert.AreEqual("Hello Contact1firstname Contact1lastname", message.ProcessedNotDelivered.First().ed_Description);
                        
                        Assert.AreEqual(contact1.MobilePhone, message.ProcessedNotDelivered.First().ed_PhoneNumber);
                        
					}
					else
					{
						Assert.AreEqual("Hello Contact2firstname Contact2lastname", message.messageText);
                        Assert.AreEqual("Hello Contact2firstname Contact2lastname", message.ProcessedNotDelivered.First().ed_Description);
                        Assert.AreEqual(contact2.MobilePhone, message.ProcessedNotDelivered.First().ed_PhoneNumber);
                    }
				}
                
				XrmHelper.Delete(localContext, new EntityReference(TextMessageTemplateEntity.EntityLogicalName, templateId));
            }
		}

        [Test]
        public void TestSendTextMessageEntityWithTemplate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                ContactEntity contact1 = new ContactEntity()
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    FirstName = "Contact1FirstName",
                    LastName = "Contact1LastName",
                    MobilePhone = "+46735198846"
                };
                // Create contact & Activity Party
                Guid contact1Id = XrmHelper.Create(localContext, contact1);
                contact1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1Id, new ColumnSet());
                ActivityPartyEntity contact1ActivityParty = new ActivityPartyEntity()
                {
                    PartyId = contact1.ToEntityReference()
                };

                IEnumerable<Generated.ActivityParty> receivers = new List<Generated.ActivityParty>() {
                    contact1ActivityParty
                };

                // Create template
                TextMessageTemplateEntity template = new TextMessageTemplateEntity()
                {
                    ed_name = "Test template",
                    ed_Description = "Hello {!contact:firstname;} {!contact:lastname;}"
                };
                Guid templateId = XrmHelper.Create(localContext, template);

                TextMessageEntity textMessageEntity = new TextMessageEntity()
                {
                    To = receivers,
                    ed_TextMessageTemplateId = new EntityReference(TextMessageTemplateEntity.EntityLogicalName,templateId),
                    ed_SenderName = "TestSender"
                };

                Guid textMessageId = XrmHelper.Create(localContext, textMessageEntity);
                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                textMessageEntity.SendTextMessage(localContext);

                System.Threading.Thread.Sleep(10000);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessageId);
                IList<SentTextMessageEntity> sentTextMessages = XrmRetrieveHelper.RetrieveMultiple<SentTextMessageEntity>(localContext, new ColumnSet(true), filter);
                Assert.AreEqual(1, sentTextMessages.Count);
                Assert.NotNull(sentTextMessages.First().ed_MessageId);

                contact1.StateCode = Generated.ContactState.Inactive;
                XrmHelper.Update(localContext, contact1);
                XrmHelper.Delete(localContext, new EntityReference(TextMessageTemplateEntity.EntityLogicalName, templateId));
                XrmHelper.Delete(localContext, new EntityReference(ContactEntity.EntityLogicalName, contact1Id));

            }
        }

        [Test]
		public void TestSingleTextMessageWithContacts()
		{
			using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
			{
				// This statement is required to enable early-bound type support.
				_serviceProxy.EnableProxyTypes();

				Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());
                
                string senderName = "TestSender";
                string description = "Test content";

                ContactEntity contact1 = new ContactEntity()
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    FirstName = "Contact1FirstName",
                    LastName = "Contact1LastName",
                    MobilePhone = "+46735198846"
                };
                // Create contact & Activity Party
                Guid contact1Id = XrmHelper.Create(localContext, contact1);
                contact1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1Id, new ColumnSet());
                ActivityPartyEntity contact1ActivityParty = new ActivityPartyEntity()
                {
                    PartyId = contact1.ToEntityReference()
                };

                ContactEntity contact2 = new ContactEntity()
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    FirstName = "Contact2FirstName",
                    LastName = "Contact2LastName",
                    MobilePhone = "+46735198846"
                };
                // Create contact & Activity Party
                Guid contact2Id = XrmHelper.Create(localContext, contact2);
                contact2 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact2Id, new ColumnSet());
                ActivityPartyEntity contact2ActivityParty = new ActivityPartyEntity()
                {
                    PartyId = contact2.ToEntityReference()
                };

                IEnumerable<Generated.ActivityParty> receivers = new List<Generated.ActivityParty>() {
                    contact1ActivityParty, contact2ActivityParty
                };

                TextMessageToSend<Generated.ActivityParty> textMessageToSend = new SingleTextMessage(receivers, senderName, null, description);

                IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messages = textMessageToSend.GetMessages(localContext);
                Assert.AreEqual(1, messages.Count());
				Assert.AreEqual("TestSender", messages.First().sender);
				Assert.AreEqual("+46735198846;+46735198846", messages.First().phoneNumber);
				Assert.AreEqual("Test content", messages.First().messageText);


                IEnumerable<SentTextMessageEntity> sentMessages = messages.First().ProcessedNotDelivered;
                Assert.AreEqual(2, sentMessages.Count());
                foreach (SentTextMessageEntity sentMessage in sentMessages)
                {
                    Assert.IsNotNull(sentMessage.ed_PhoneNumber);
                    Assert.AreEqual("Test content", sentMessage.ed_Description);
                }
            }
		}

        [Test]
        public void TestSingleTextMessageWithPhoneNumber()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                string senderName = "TestSender";
                string phoneNumber = "+46700000001;+46700000002;+46700000003";
                string description = "Test content";

                TextMessageToSend<Generated.ActivityParty> textMessageToSend = new SingleTextMessage(null, senderName, phoneNumber, description);

                //IEnumerable<IDictionary<string, string>> messageData = textMessageToSend.GetMessageSendData(localContext);
                IEnumerable<TextMessageToSend<Generated.ActivityParty>.Message> messages = textMessageToSend.GetMessages(localContext);
                Assert.AreEqual(1, messages.Count());
                Assert.AreEqual("TestSender", messages.First().sender);
                Assert.AreEqual("+46700000001;+46700000002;+46700000003", messages.First().phoneNumber);
                Assert.AreEqual("Test content", messages.First().messageText);

                IEnumerable<SentTextMessageEntity> sentMessages = messages.First().ProcessedNotDelivered;
                Assert.AreEqual(3, sentMessages.Count());
                foreach (SentTextMessageEntity sentMessage in sentMessages)
                {
                    Assert.NotNull(sentMessage.ed_PhoneNumber);
                    Assert.AreEqual("Test content", sentMessage.ed_Description);
                }
            }
        }

        [Test]
        public void TestSendTextMessageEntity()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                TextMessageEntity textMessageEntity = new TextMessageEntity()
                {
                    ed_SenderName = "TestSender",
                    ed_PhoneNumber = "0735198846",
                    Description = "Test Content"
                };
                Guid textMessageId = XrmHelper.Create(localContext, textMessageEntity);
                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                textMessageEntity.SendTextMessage(localContext);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessageId);
                IList<SentTextMessageEntity> sentTextMessages = XrmRetrieveHelper.RetrieveMultiple<SentTextMessageEntity>(localContext,new ColumnSet(true),filter);
                Assert.AreEqual(1, sentTextMessages.Count);
                Assert.NotNull(sentTextMessages.First().ed_MessageId);

                XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));
            }
        }

        [Test]
        public void TestSendTextMessageEntityWithLead()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                LeadEntity lead = new LeadEntity()
                {
                    MobilePhone = "+46735198846"
                };

                lead.Id = XrmHelper.Create(localContext, lead);

                TextMessageEntity textMessageEntity = new TextMessageEntity()
                {
                    ed_SenderName = "TestSender",
                    Description = "Description",
                    To = new List<ActivityPartyEntity>()
                    {
                        new ActivityPartyEntity()
                        {
                            PartyId = lead.ToEntityReference()
                        }
                    }
                };
                Guid textMessageId = XrmHelper.Create(localContext, textMessageEntity);
                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                textMessageEntity.SendTextMessage(localContext);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessageId);
                IList<SentTextMessageEntity> sentTextMessages = XrmRetrieveHelper.RetrieveMultiple<SentTextMessageEntity>(localContext, new ColumnSet(true), filter);
                Assert.AreEqual(1, sentTextMessages.Count);
                Assert.NotNull(sentTextMessages.First().ed_MessageId);

                XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));
            }
        }
        
        [Test]
        public void TestResendUnprocessedSentTextMessageStatus()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                double timeoutBefore = GetTimeLimit(localContext);

                CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(false));
                settings.ed_SMSTimeLimit = 0;
                XrmHelper.Update(localContext, settings);

                TextMessageEntity textMessageEntity = new TextMessageEntity()
                {
                    ed_SenderName = "TestSender",
                    ed_PhoneNumber = "+46735198846",
                    Description = "Test Content"
                };
                Guid textMessageId = XrmHelper.Create(localContext, textMessageEntity);
                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                textMessageEntity.SendTextMessage(localContext);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessageId);
                IEnumerable<SentTextMessageEntity> sentTextMessages = TextMessageEntity.GetSentTextMessages(localContext, textMessageEntity);
                Assert.AreEqual(1, sentTextMessages.Count());
                Assert.Null(sentTextMessages.First().ed_ProcessId);
                Assert.AreEqual((int)SentTextMessageEntity.Status.NotProcessed, sentTextMessages.First().statuscode.Value);

                settings.ed_SMSTimeLimit = timeoutBefore;
                XrmHelper.Update(localContext, settings);

                textMessageEntity.UpdateSentMessagesStatus(localContext, new List<SentTextMessageEntity>(sentTextMessages), timeoutBefore);

                sentTextMessages = TextMessageEntity.GetSentTextMessages(localContext, textMessageEntity);
                Assert.NotNull(sentTextMessages.First().ed_ProcessId);
                Assert.AreNotEqual((int)SentTextMessageEntity.Status.NotProcessed, sentTextMessages.First().statuscode.Value);

                XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));

            }
        }

        [Test]
        public void TestUpdateProcessedSentTextMessageStatus()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                double timeOutBefore = GetTimeLimit(localContext);

                CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(false));
                settings.ed_SMSTimeLimit = 0.1;
                XrmHelper.Update(localContext, settings);

                TextMessageEntity textMessageEntity = new TextMessageEntity()
                {
                    ed_SenderName = "TestSender",
                    ed_PhoneNumber = "+46735198846",
                    Description = "Test Content"
                };
                Guid textMessageId = XrmHelper.Create(localContext, textMessageEntity);
                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                textMessageEntity.SendTextMessage(localContext);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(SentTextMessageEntity.Fields.ed_TextMessageId, ConditionOperator.Equal, textMessageId);
                IList<SentTextMessageEntity> sentTextMessages = XrmRetrieveHelper.RetrieveMultiple<SentTextMessageEntity>(localContext, new ColumnSet(true), filter);
                Assert.AreEqual(1, sentTextMessages.Count);
                Assert.AreNotEqual((int)SentTextMessageEntity.Status.NotProcessed, sentTextMessages.First().statuscode.Value);

                settings.ed_SMSTimeLimit = timeOutBefore;
                XrmHelper.Update(localContext, settings);

                System.Threading.Thread.Sleep(10000);
                sentTextMessages.First().RetrieveUpdatedStatus(localContext);

                Assert.NotNull(sentTextMessages.First().ed_MessageId);
                Assert.AreNotEqual((int)SentTextMessageEntity.Status.Processed, sentTextMessages.First().statuscode.Value);

                XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));

            }
        }

        private static double GetTimeLimit(Plugin.LocalPluginContext localContext)
        {
            CgiSettingEntity settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(CgiSettingEntity.Fields.ed_SMSTimeLimit));
            return settings.ed_SMSTimeLimit ?? 0;
        }

        internal ServerConnection ServerConnection
		{
			get
			{
				if (_serverConnection == null)
				{
					_serverConnection = new ServerConnection();
				}
				return _serverConnection;
			}
		}

		internal ServerConnection.Configuration Config
		{
			get
			{
				return TestSetup.Config;
			}
		}
	}
}
