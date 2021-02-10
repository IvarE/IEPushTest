using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Endeavor.Crm.UnitTest;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Endeavor.Crm;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Crm.Sdk.Messages;
using Skanetrafiken.Crm;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class GetCardDetailsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestGetCardDetails()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                //GetCardDetails.ExecuteCodeActivity(localContext, "123321123");
            
                CallWorkFlowGetCardDetails(localContext, "123321123");
            }
        }

        [Test, Category("Debug")]
        public void TestGetCardDetails_CustomAction()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string biztalkResponse = CallWorkFlowGetCardDetails(localContext, "123321123");
                bool cardHotlistedField = CallWorkFlowParseCardDetails(localContext, biztalkResponse);
                string biztalkBlockResponse = CallWorkBlockCardDetails(localContext, "123321123", 5);
                string biztalkBlockCardResponse = CallWorkParseBlockCardDetails(localContext, biztalkBlockResponse);
            }
        }

        private string CallWorkFlowGetCardDetails(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {

            OrganizationRequest request = new OrganizationRequest("ed_GetCardDetails");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["CardDetailsResponse"];
            return soapresponse;
        }

        private bool CallWorkFlowParseCardDetails(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBiztalkResponse");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            bool cardHotlistedField = (bool)response["CardHotlistedField"];
            return cardHotlistedField;
        }

        private string CallWorkBlockCardDetails(Plugin.LocalPluginContext localContext, string cardNumber, int reasonCode)
        {
            OrganizationRequest request = new OrganizationRequest("ed_BlockCardBiztalk");
            request["CardNumber"] = cardNumber;
            request["ReasonCode"] = reasonCode;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string cardBlockResponse = (string)response["CardBlockResponse"];
            return cardBlockResponse;
        }

        private string CallWorkParseBlockCardDetails(Plugin.LocalPluginContext localContext, string biztalkResponse)
        {
            OrganizationRequest request = new OrganizationRequest("ed_ParseBlockCardResponseFromBiztalk");
            request["BiztalkResponse"] = biztalkResponse;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string parsedCardBlockResponse = (string)response["RequestCardBlockResult"];
            return parsedCardBlockResponse;
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
