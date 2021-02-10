using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    public class RecalculateCampaignStatisticsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

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

        [Test, Category("Debug")]
        public void RecalculateStatistics()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                CampaignEntity campaign1 = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, new Guid("8EB5C249-75E2-E811-827A-00155D010B00"), new ColumnSet(true));

                int? totalLeadsBefore = campaign1.ed_TotalLeads;
                int? qualifiedLeadsBefore = campaign1.ed_QualifiedLeads;
                int? qualifiedDR1Before = campaign1.ed_QualifiedLeadsDR1;
                int? qualifiedDR2Before = campaign1.ed_QualifiedLeadsDR2;

                campaign1.RecalculateStatisticsOnCampaign(localContext);

                CampaignEntity campaign2 = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaign1.Id, new ColumnSet(true));

                int? totalLeadsAfter = campaign2.ed_TotalLeads;
                int? qualifiedLeadsAfter = campaign2.ed_QualifiedLeads;
                int? qualifiedDR1After = campaign2.ed_QualifiedLeadsDR1;
                int? qualifiedDR2After = campaign2.ed_QualifiedLeadsDR2;

                
                
            }
        }
    }
}
