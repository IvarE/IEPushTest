using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Endeavor.Crm;
using Endeavor.Crm.Schema;
using Endeavor.Crm.Extensions;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture(Category = "Plugin")]
    public class OpportunityFixture
    {
        private ServerConnection _serverConnection;

        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        private OrganizationServiceProxy _serviceProxy;

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

        [Test]
        public void GetOpportunities()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                FilterExpression filter = new FilterExpression(LogicalOperator.And);

                IList<OpportunityEntity> opportunities = OpportunityEntity.RetrieveMultiple(localContext, filter, new ColumnSet(true));

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

                stopwatch.Start();
                localContext.TracingService.Trace("Start Opportunities");
                foreach (OpportunityEntity opportunity in opportunities)
                {
                    opportunity.Trace(localContext.TracingService);
                }
                localContext.TracingService.Trace("Stop Opportunities, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        [Test]
        public void ResetProbability()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                FilterExpression filter = new FilterExpression(LogicalOperator.And);

                IList<OpportunityEntity> opportunities = OpportunityEntity.RetrieveMultiple(localContext, filter, new ColumnSet(OpportunityEntity.Fields.end_Probability, OpportunityEntity.Fields.CloseProbability));

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

                stopwatch.Start();
                localContext.TracingService.Trace("Start Opportunities");
                foreach (OpportunityEntity opportunity in opportunities)
                {
                    if (opportunity.end_Probability != null)
                    {
                        opportunity.Trace(localContext.TracingService);
                        opportunity.end_Probability = opportunity.end_Probability;
                        //opportunity.UpdateCloseProbability(localContext);
                        XrmHelper.Update(localContext.OrganizationService, opportunity);
                    }
                }
                localContext.TracingService.Trace("Stop Opportunities, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
