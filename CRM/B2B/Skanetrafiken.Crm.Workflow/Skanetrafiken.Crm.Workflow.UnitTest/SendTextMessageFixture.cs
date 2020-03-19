using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.TextMessageSender;
using System;
using System.Collections.Generic;
using System.Linq;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using ReportResponse = Skanetrafiken.Crm.TextMessageSender.BosbecAPIHandler.DeliveryReportResponse;
using SendReponse = Skanetrafiken.Crm.TextMessageSender.BosbecAPIHandler.TextMessageResponseBase;

namespace Endeavor.Crm.UnitTest
{
    public class SendTextMessageFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        private string cWorkflowId = "71492023-144d-4917-b8c9-0d8e7a9f0905";
        private string cApiKey = "228efe78-ebad-45ef-844c-5a3f94449750";
        private string cHostStr = "https://rest.mobileresponse.se";

        private readonly string cMockHostStr = "https://51e78337-7789-4dc2-8c16-dffcbc6e4d82.mock.pstmn.io";

        private BosbecAPIHandler GetAPIHandler()
        {
            BosbecAPIHandler APIHandler = new BosbecAPIHandler()
            {
                apiKey = cApiKey,
                workflowId = cWorkflowId,
                baseUrlStr = cHostStr
            };

            return APIHandler;
        }

        [Test]
        public void TestAPICallOneMessage()
        {

            BosbecAPIHandler APIHandler = GetAPIHandler();

            SendReponse postResponse = APIHandler.SendTextMessage("+46735198846", "Marcus", "Test Text 1 Test Text 2 Test Text 3 Test Text 4 Test Text 5 Test Text 6 Test Text 7 Test Text 8 Test Text 9 Test Text 10 Test Text 11 Test Text 12 Test Text 13 Test Text 14 Test Text 15 Test Text 16 Test Text 17 Test Text 18 Test Text 19 Test Text 20 Test Text 21 Test Text 22 Test Text 23 Test Text 24 Test Text 25 Test Text 26 Test Text 27 Test Text 28 Test Text 29 Test Text 30 Test Text 31 Test Text 32 Test Text 33 Test Text 34 Test Text 35 Test Text 36 Test Text 37 Test Text 38 Test Text 39 Test Text 40 Test Text 1 Test Text 2 Test Text 3 Test Text 4 Test Text 5 Test Text 6 Test Text 7 Test Text 8 Test Text 9 Test Text 10 Test Text 11 Test Text 12 Test Text 13 Test Text 14 Test Text 15 Test Text 16 Test Text 17 Test Text 18 Test Text 19 Test Text 20 Test Text 21 Test Text 22 Test Text 23 Test Text 24 Test Text 25 Test Text 26 Test Text 27 Test Text 28 Test Text 29 Test Text 30 Test Text 31 Test Text 32 Test Text 33 Test Text 34 Test Text 35 Test Text 36 Test Text 37 Test Text 38 Test Text 39 Test Text 40");

            while (APIHandler.NumberOfSentMessagesByProcess(postResponse.processId) != 1)
            {
                System.Threading.Thread.Sleep(100);
            }

            ReportResponse deliveryReport = APIHandler.GetDeliveryReport<ReportResponse>(postResponse.processId);

            // verify response Ids are the same
            Assert.AreEqual(deliveryReport.processId, postResponse.processId);
        }

        [Test]
        public void TestAPICallMultipleNumbers()
        {
            BosbecAPIHandler APIHandler = GetAPIHandler();

            SendReponse postResponse = APIHandler.SendTextMessage("+46700000001;+46700000004;+46700000005", "TestSender", "TestContent");

            while (APIHandler.NumberOfSentMessagesByProcess(postResponse.processId) != 3)
            {
                System.Threading.Thread.Sleep(100);
            }

            ReportResponse deliveryReport = APIHandler.GetDeliveryReport<ReportResponse>(postResponse.processId);

            Assert.AreEqual(deliveryReport.processId, postResponse.processId);

            Assert.AreEqual(3, deliveryReport.messages.Count());

        }

        [Test, Category("Debug")]
        public void TestSendMessageActivity()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                TextMessageEntity textMessage = new TextMessageEntity()
                {
                    Description = "Bäste kund, du är utvald!\nKom hit innan 19 och du får en liten present! //Personalen på Ica Nära Stabbetorget",
                    ed_PhoneNumber = "0735198846",
                    ed_SenderName = "hej1!#¤%&/"
                };

                Guid textMessageId = XrmHelper.Create(localContext, textMessage);
                EntityReference textMessageRef = new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId);
                OrganizationRequest request = new OrganizationRequest("ed_SendTextMessage");
                request["Target"] = textMessageRef;

                OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

                textMessage = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual(Generated.ed_TextMessageState.Completed, textMessage.StateCode);
                //XrmHelper.Delete(localContext, textMessageRef);
            }
        }

        [Test, Category("Debug")]
        public void TestSendMessageActivityFail()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                // Phone number does not exist
                TextMessageEntity textMessage = new TextMessageEntity()
                {
                    ed_SenderName = "sender",
                    Description = "description",
                    ed_PhoneNumber = "+46700000002"
                };

                Guid textMessageId = XrmHelper.Create(localContext, textMessage);
                EntityReference textMessageRef = new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId);
                OrganizationRequest request = new OrganizationRequest("ed_SendTextMessage_test");
                request["Target"] = textMessageRef;
                bool SendSmsFailFlag = false;
                try
                {
                    OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);
                }
                catch (Exception)
                {
                    SendSmsFailFlag = true;
                }

                Assert.IsTrue(SendSmsFailFlag);
                //XrmHelper.Delete(localContext, textMessageRef);
            }
        }

        [Test, Category("Debug")]
        public void TestSendMessageActivityMultipleRecipients()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                TextMessageEntity textMessage = new TextMessageEntity()
                {
                    ed_SenderName = "sender",
                    Description = "description",
                    ed_PhoneNumber = "+46700000001;+46700000004"
                };

                Guid textMessageId = XrmHelper.Create(localContext, textMessage);
                EntityReference textMessageRef = new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId);
                OrganizationRequest request = new OrganizationRequest("ed_SendTextMessage");
                request["Target"] = textMessageRef;
                OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

                //XrmHelper.Delete(localContext, textMessageRef);
            }
        }

        
        [Test, Category("Debug")]
        public void TestSendMessage()
        {
            
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                
                ContactEntity contact1 = new ContactEntity()
                {
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    FirstName = "Contact1FirstName",
                    LastName = "Contact1LastName",
                    MobilePhone = "0735198846"
                };
                // Create contact & Activity Party
                Guid contact1Id = XrmHelper.Create(localContext, contact1);
                contact1 = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1Id, new ColumnSet());
                ActivityPartyEntity contact1ActivityParty = new ActivityPartyEntity()
                {
                    PartyId = contact1.ToEntityReference()
                };

                TextMessageEntity textMessage = new TextMessageEntity()
                {
                    Description = "description",
                    To = new List<Generated.ActivityParty>() {
                        //contact1ActivityParty, contact2ActivityParty
                        contact1ActivityParty
                    },
                    ed_SenderName = "testsender"
                };

                Guid textMessageId = XrmHelper.Create(localContext, textMessage);
                
                EntityReference textMessageRef = new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId);

                SendTextMessage workflow = new SendTextMessage();
                workflow.ExecuteWorkflow(localContext, textMessageRef);

                textMessage = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual((int)TextMessageEntity.Status.Delivered, textMessage.StatusCode.Value);
            }
        }


        [Test]
        public void TestUpdateStatusOnUnprocessedMessages()
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

                SendTextMessage sendTextMessage = new SendTextMessage();
                sendTextMessage.ExecuteWorkflow(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));

                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual((int)TextMessageEntity.Status.Failed, textMessageEntity.StatusCode.Value);

                settings.ed_SMSTimeLimit = timeoutBefore;
                XrmHelper.Update(localContext, settings);

                System.Threading.Thread.Sleep(500);

                UpdateTextMessageStatus updateTextMessage = new UpdateTextMessageStatus();
                updateTextMessage.ExecuteCodeActivity(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));

                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual((int)TextMessageEntity.Status.Delivered, textMessageEntity.StatusCode.Value);

                //XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                //XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));

            }
        }

        [Test]
        public void TestUpdateStatusWorkflowOnUnprocessedMessages()
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

                OrganizationRequest request = new OrganizationRequest("ed_SendTextMessage");
                request["Target"] = textMessageEntity.ToEntityReference();

                OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual((int)TextMessageEntity.Status.Failed, textMessageEntity.StatusCode.Value);

                settings.ed_SMSTimeLimit = timeoutBefore;
                XrmHelper.Update(localContext, settings);

                System.Threading.Thread.Sleep(500);

                ExecuteWorkflowRequest workflowRequest = new ExecuteWorkflowRequest()
                {
                    WorkflowId = new Guid("e84941df-df7d-4b33-a950-860b6d2015bc"),
                    EntityId = textMessageId
                };

                localContext.OrganizationService.Execute(workflowRequest);

                System.Threading.Thread.Sleep(10000);

                textMessageEntity = XrmRetrieveHelper.Retrieve<TextMessageEntity>(localContext, textMessageId, new ColumnSet(true));
                Assert.AreEqual((int)TextMessageEntity.Status.Delivered, textMessageEntity.StatusCode.Value);

                //XrmHelper.Delete(localContext, new EntityReference(TextMessageEntity.EntityLogicalName, textMessageId));
                //XrmHelper.Delete(localContext, new EntityReference(SentTextMessageEntity.EntityLogicalName, sentTextMessages.First().Id));

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
