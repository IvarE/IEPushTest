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

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class GenerateCampaignCodesFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void MeasureTimeForCampaignCodeGeneration()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                CampaignEntity campaign = null;
                List<LeadEntity> leads = new List<LeadEntity>();
                int leadsToTest = 50;

                try
                {
                    campaign = new CampaignEntity
                    {
                        Name = "StressTestCampaign",
                        TypeCode = Generated.campaign_typecode.NyinflyttadKampanj
                    };
                    campaign.Id = XrmHelper.Create(localContext.OrganizationService, campaign);
                    campaign.CampaignId = campaign.Id;

                    for (int i = 0; i < leadsToTest; i++)
                    {
                        leads.Add(CreateLead(localContext, campaign, Generated.lead_leadsourcecode.SPAR));
                    }

                    sw.Restart();
                    GenerateCampaignCodesRecursive.ExecuteCodeActivity(localContext, campaign.ToEntityReference());
                    localContext.Trace("Generating codes for " + leadsToTest + " leads took " + sw.ElapsedMilliseconds / 1000 + " seconds.");
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (leads != null && leads.Count > 0)
                    {
                        foreach (LeadEntity l in leads)
                        {
                            try { XrmHelper.Delete(localContext.OrganizationService, l.ToEntityReference()); } catch { }
                        }
                    }
                    if (campaign != null)
                        try { XrmHelper.Delete(localContext.OrganizationService, campaign.ToEntityReference()); } catch { }
                }

            }

        }

        private LeadEntity CreateLead(Plugin.LocalPluginContext localContext, CampaignEntity newCampaign, Generated.lead_leadsourcecode leadSource)
        {
            LeadEntity newLead = new LeadEntity
            {
                CampaignId = newCampaign.ToEntityReference(),
                LeadSourceCode = leadSource
            };
            newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);

            return newLead;
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
