using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Microsoft.Xrm.Sdk;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class GetOutstandingChargesFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void TestGetOutstandingCharges()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                //GetOutstandingCharges.ExecuteCodeActivity(localContext, "123321123");
            
                CallWorkFlowGetOutstandingCharges(localContext, "123321123");
            }
        }

        private void CallWorkFlowGetOutstandingCharges(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {

            OrganizationRequest request = new OrganizationRequest("ed_GetOutstandingChargesActionWorkflow");
            request["TravelCardNumber"] = travelCardNumber;

            OrganizationResponse response = (OrganizationResponse)localContext.OrganizationService.Execute(request);

            string soapresponse = (string)response["GetOutstandingChargesResponse"];

            // TODO Assert something with response?

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
