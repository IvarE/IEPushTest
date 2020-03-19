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

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class SynchroniseLeadsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Regression")]
        public void SetCampaignCodesToCampaignLeads()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                CampaignEntity newCampaign = null;
                LeadEntity newLeadOne = null, newLeadTwo = null, newLeadThree = null, newLeadFour = null;
                try
                {
                    #region Setup
                    var correctLeadSource = Generated.lead_leadsourcecode.Ovrig;
                    var correctLeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj;
                    var incorrectLeadSource = Generated.lead_leadsourcecode.OinloggatKop;

                    // Create new campaign and leads for testing
                    newCampaign = new CampaignEntity
                    {
                        Name = "TestCampaign",
                        ed_LeadSource = new OptionSetValue((int)correctLeadSource),
                        ed_LeadTopic = correctLeadTopic
                    };
                    newCampaign.Id = XrmHelper.Create(localContext.OrganizationService, newCampaign);
                    newCampaign.CampaignId = newCampaign.Id;

                    newLeadOne = CreateLead(localContext, newCampaign, correctLeadSource);
                    newLeadTwo = CreateLead(localContext, newCampaign, correctLeadSource);
                    newLeadThree = CreateLead(localContext, newCampaign, incorrectLeadSource);
                    newLeadFour = CreateLead(localContext, newCampaign, incorrectLeadSource);
                    #endregion

                    //SynchroniseLeads.ExecuteCodeActivity(localContext, newCampaign.ToEntityReference());
                    List<string> listCodes = CampaignEntity.generateRandomCampaignCodes(4);
                    List<string> leads = new List<string>
                    {
                        newLeadOne.Id.ToString(),
                        newLeadTwo.Id.ToString(),
                        newLeadThree.Id.ToString(),
                        newLeadFour.Id.ToString(),

                    };

                    // Update leads with campaign codes
                    GenerateCampaignCodesRecursive.ExecuteCodeActivity(localContext, leads.ToArray(), listCodes.ToArray());

                    UpdateLeadSourceOnUnderlyingLeadsRecursive.ExecuteCodeActivity(localContext, newCampaign.ToEntityReference());
                    UpdateTopicOnUnderlyingLeadsRecursive.ExecuteCodeActivity(localContext, newCampaign.ToEntityReference());

                    #region Test

                    CampaignEntity checkCampaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(true), new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.Id, ConditionOperator.Equal, newCampaign.Id)
                        }
                    });

                    // Get all leads for the campaign
                    IList<LeadEntity> list = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(true), new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, newCampaign.Id)
                        }
                    });

                    foreach (LeadEntity l in list)
                    {
                        NUnit.Framework.Assert.AreEqual(correctLeadTopic.ToString(), l.Subject);
                        NUnit.Framework.Assert.AreEqual(correctLeadSource, l.LeadSourceCode);
                        NUnit.Framework.Assert.IsNotEmpty(l.ed_CampaignCode);
                        NUnit.Framework.Assert.IsNotNull(l.ed_CampaignCode);
                    }
                    
                    #endregion

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    try { XrmHelper.Delete(localContext.OrganizationService, newCampaign.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, newLeadOne.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, newLeadTwo.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, newLeadThree.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, newLeadFour.ToEntityReference()); } catch { }
                }
            }

        }

        private LeadEntity CreateLead(Plugin.LocalPluginContext localContext, CampaignEntity newCampaign, Generated.lead_leadsourcecode leadSource)
        {
            LeadEntity newLeadOne = new LeadEntity
            {
                CampaignId = newCampaign.ToEntityReference(),
                LeadSourceCode = leadSource
            };
            newLeadOne.Id = XrmHelper.Create(localContext.OrganizationService, newLeadOne);

            return newLeadOne;
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
