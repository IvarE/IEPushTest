using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    public class CreateMultipleValueCodesFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test]
        public void TestCreationOfValueCodes()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                IList<ValueCodeEntity> valueCodesBefore = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, new ColumnSet(false));

                int amount = 100;
                int count = 5;
                CreateMultipleValueCodes.ExecuteCodeActivity(localContext, count, amount);

                IList<ValueCodeEntity> valueCodesAfter = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, new ColumnSet(true));

                Assert.AreEqual(valueCodesBefore.Count + count, valueCodesAfter.Count);

                IEnumerable<ValueCodeEntity> newValueCodes = valueCodesAfter.Where(v => !valueCodesBefore.Select(b => b.Id).Contains(v.Id));

                foreach(ValueCodeEntity valueCode in newValueCodes)
                {
                    Assert.AreEqual(valueCode.ed_Amount, amount);
                    XrmHelper.Delete(localContext, valueCode.ToEntityReference());
                }
            }
        }

        [Test]
        public void TestCallActionCreateMultipleValueCodes()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                IList<ValueCodeEntity> valueCodesBefore = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, new ColumnSet(false));

                double amount = 100;
                int count = 5;

                OrganizationRequest request = new OrganizationRequest("ed_CreateMultipleValueCodes")
                {
                    ["Amount"] = amount,
                    ["Count"] = count
                };

                localContext.OrganizationService.Execute(request);

                System.Threading.Thread.Sleep(500);

                IList<ValueCodeEntity> valueCodesAfter = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, new ColumnSet(true));

                Assert.AreEqual(valueCodesBefore.Count + count, valueCodesAfter.Count);

                IEnumerable<ValueCodeEntity> newValueCodes = valueCodesAfter.Where(v => !valueCodesBefore.Select(b => b.Id).Contains(v.Id));

                foreach (ValueCodeEntity valueCode in newValueCodes)
                {
                    Assert.AreEqual(valueCode.ed_Amount, amount);
                    XrmHelper.Delete(localContext, valueCode.ToEntityReference());
                }
            }
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
