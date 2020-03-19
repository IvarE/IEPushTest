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

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class CampaignFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        private static readonly string campaignName = "Leadfixture test name";
        private static readonly string CampaignCodeName = "CMP-01041-CCCC"; 
        private static readonly int discountPercent = 20;

        private static readonly string processId = "2D95BBF8-E9FF-4C4B-A988-53E4CC1225A5";
        private static readonly string stage1Create = "ceeb25ac-c704-4612-a296-61cef8986c70";
        private static readonly string stage2DR1 = "189dc031-75b4-4464-b8a5-1d1e181945d8";
        private static readonly string stage3DR2 = "74f9a153-4824-47d9-bcdd-9b42b2fe2556";
        private static readonly string stage4End = "20bbf364-e634-406b-b322-5b922fecaead";

        [Test, Category("Run Always")]
        public void GenerateCampaignCodesFullFlow()
        {
            int numberOfCodesToGenerate = 100000;
            List<string> listOfExistingCodes = CampaignEntity.generateRandomCampaignCodes(numberOfCodesToGenerate);
            List<string> listOfNewGeneratedCodes = CampaignEntity.generateUniqueCampaignCodes(listOfExistingCodes, numberOfCodesToGenerate);

            Assert.AreEqual(listOfNewGeneratedCodes.Count, numberOfCodesToGenerate);
            Assert.AreEqual(listOfNewGeneratedCodes.Count, listOfNewGeneratedCodes.Distinct().Count());
            Assert.Zero(listOfNewGeneratedCodes.Intersect(listOfExistingCodes).ToList().Count);
        }

        [Test, Explicit]
        public void GenerateCampaignCodes()
        {
            // QR: 1 million codes within 1 second
            int numberOfCodesToGenerate = 1000000;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<string> listWithCampaignCodes = CampaignEntity.generateRandomCampaignCodes(numberOfCodesToGenerate);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Assert.LessOrEqual(elapsedMs, 1000);
        }


        [Test, Explicit]
        public void MakeCampaignCodesUnique()
        {
            int numberOfCodesToGenerate = 100000;
            List<String> listOfExistingCodes = CampaignEntity.generateRandomCampaignCodes(numberOfCodesToGenerate);
            List<String> listOfNewGeneratedCodes = CampaignEntity.generateRandomCampaignCodes(numberOfCodesToGenerate);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<String> uniqueCampaignCodes = CampaignEntity.makeCampaignCodesUnique(listOfExistingCodes, listOfNewGeneratedCodes);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            List<string> duplicatesBetweenTwoLists = uniqueCampaignCodes.Intersect(listOfExistingCodes).ToList();
            List<string> duplicatesWithinList = uniqueCampaignCodes.GroupBy(x => x)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key).ToList();

            Assert.NotZero(uniqueCampaignCodes.Count);
            Assert.AreEqual(uniqueCampaignCodes.Count, uniqueCampaignCodes.Distinct().Count());
        }


        [Test, Category("Run Always")]
        public void TestActualCampaignEndRules()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                CampaignEntity campaign = null;

                try
                {
                    campaign = CreateCampaign(localContext);

                    CampaignEntity updateEnt = new CampaignEntity
                    {
                        Id = campaign.Id,
                        ed_ValidFromPhase2 = DateTime.Now.AddHours(1),
                        ed_ValidToPhase2 = DateTime.Now.AddDays(24),
                        StageId = new Guid(stage2DR1),
                        ProcessId = new Guid(processId)
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEnt);
                    campaign.CombineAttributes(updateEnt);

                    updateEnt = new CampaignEntity
                    {
                        Id = campaign.Id,
                        StageId = new Guid(stage3DR2),
                        ProcessId = new Guid(processId)
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEnt);
                    campaign.CombineAttributes(updateEnt);

                    try
                    {
                        updateEnt = new CampaignEntity
                        {
                            Id = campaign.Id,
                            ed_ActualCampaignEnd = DateTime.Now.AddDays(32)
                        };
                        XrmHelper.Update(localContext.OrganizationService, updateEnt);
                        throw new Exception("Setting ActualCampaignEnd before last stage is reached should throw an error");
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Equals("It is not allowed to set ActualCampaignEnd until the Campaign is in a Closed process Stage"))
                            throw e;
                    }
                    updateEnt = new CampaignEntity
                    {
                        Id = campaign.Id,
                        ed_ValidFromPhase1 = DateTime.Now.AddDays(-20),
                        ed_ValidToPhase1 = DateTime.Now.AddDays(-10),
                        ed_ValidFromPhase2 = DateTime.Now.AddHours(-8),
                        ed_ValidToPhase2 = DateTime.Now.AddDays(-1),
                        StageId = new Guid(stage4End),
                        ProcessId = new Guid(processId)
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEnt);
                    campaign.CombineAttributes(updateEnt);
                    try
                    {
                        updateEnt = new CampaignEntity
                        {
                            Id = campaign.Id,
                            ed_ActualCampaignEnd = DateTime.Now.AddDays(-5)
                        };
                        //updateEnt.HandlePreCampaignUpdate(localContext, campaign);
                        XrmHelper.Update(localContext.OrganizationService, updateEnt);
                        throw new Exception("Setting ActualCampaignEnd to an earlier date than ed_ValidToPhase2 should throw an error");
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Equals("It is not allowed to set ActualCampaignEnd until response time is complete"))
                            throw e;
                    }
                    updateEnt = new CampaignEntity
                    {
                        Id = campaign.Id,
                        ed_ActualCampaignEnd = DateTime.Now.AddDays(32)
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateEnt);
                    campaign.CombineAttributes(updateEnt);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    try { XrmHelper.Delete(localContext.OrganizationService, campaign.ToEntityReference()); } catch { }
                }

            }
        }

        /// <summary>
        /// Test is only intented for updating leads with campaign codes and nothing else.
        /// Creates one new campaign and two new leads and assign the leads with static codes.
        /// </summary>
        [Test, Category("Run Always")]
        public void UpdateLeadsWithCampaignCodes()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                // Create new campaign and leads for testing
                CampaignEntity newCampaign = new CampaignEntity
                {
                    Name = "TestVSCampaign",
                    Id = Guid.NewGuid()
                };
                XrmHelper.Create(localContext.OrganizationService, newCampaign);


                string codeOne = "abc123";
                string codeTwo = "def456";
                List<String> listOfCodes = new List<string>() { codeOne, codeTwo };
                LeadEntity newLeadOne = new LeadEntity
                {
                    Subject = "TestVSLeadOne",
                    Id = Guid.NewGuid(),
                    CampaignId = newCampaign.ToEntityReference()
                };
                XrmHelper.Create(localContext.OrganizationService, newLeadOne);

                LeadEntity newLeadTwo = new LeadEntity
                {
                    Subject = "TestVSLeadOne",
                    Id = Guid.NewGuid(),
                    CampaignId = newCampaign.ToEntityReference()
                };
                XrmHelper.Create(localContext.OrganizationService, newLeadTwo);

                //Get the leads connected to the campaign
                IList<LeadEntity> connectedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(false),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, newCampaign.Id),
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Null)
                    }
                });

                for (int i = 0; i < connectedLeads.Count; i++)
                {
                    LeadEntity updateLead = new LeadEntity
                    {
                        Id = connectedLeads[i].Id,
                        ed_CampaignCode = listOfCodes[i]
                    };
                    XrmHelper.Update(localContext.OrganizationService, updateLead);
                }
                LeadEntity lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, new ColumnSet(true),
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, codeOne)
                        }
                    });

                Assert.AreEqual(lead.ed_CampaignCode, codeOne);

                lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, new ColumnSet(true),
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, codeTwo)
                        }
                    });

                Assert.AreEqual(lead.ed_CampaignCode, codeTwo);

                //Tear down
                XrmHelper.Delete(localContext.OrganizationService, newCampaign.ToEntityReference());
                XrmHelper.Delete(localContext.OrganizationService, newLeadOne.ToEntityReference());
                XrmHelper.Delete(localContext.OrganizationService, newLeadTwo.ToEntityReference());

            }
        }



        [Test, Explicit]
        public void ExecuteCanLeadBeCreated()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext,
                    new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber,
                                ContactEntity.Fields.FirstName,
                                ContactEntity.Fields.LastName
                                ),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_HasSwedishSocialSecurityNumber, ConditionOperator.Equal, false),
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull)
                        }
                    });

                CustomerInfo info = new CustomerInfo
                {
                    Source = (int)CustomerUtility.Source.AdmSkapaKund,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    SocialSecurityNumber = contact.cgi_socialsecuritynumber,
                    SwedishSocialSecurityNumber = false,
                    SwedishSocialSecurityNumberSpecified = true
                };

                StatusBlock block = LeadEntity.CanLeadBeCreated(localContext, info);

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);


            }
        }

        public static void createNewLead(Plugin.LocalPluginContext localContext, CampaignEntity campaign)
        {
            LeadEntity newLead = new LeadEntity
            {
                FirstName = "x",
                LastName = "y",
                ed_CampaignCode = "000000",
                CampaignId = campaign.ToEntityReference(),
                ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
            };
            newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);
            
        }

        public static CampaignEntity createNewCampaign(Plugin.LocalPluginContext localContext)
        {
            CampaignEntity newCampaign = new CampaignEntity
            {
                Name = campaignName,
                ed_DiscountPercent = discountPercent,
                ed_LeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj
            };
            newCampaign.Id = XrmHelper.Create(localContext.OrganizationService, newCampaign);

            return XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, newCampaign.ToEntityReference(), new ColumnSet(true));
        }

        public static CampaignEntity CreateCampaign(Plugin.LocalPluginContext localContext)
        {
            CampaignEntity campaign = new CampaignEntity
            {
                Name = campaignName + DateTime.Now.ToString(),
                CodeName = CampaignCodeName,
                ed_DiscountPercent = discountPercent,
                ed_LeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj,
                ed_LeadSource = new OptionSetValue((int)Generated.lead_leadsourcecode.MittKonto),
                ed_ValidFromPhase1 = DateTime.Now.AddMinutes(-10),
                ed_ValidToPhase1 = DateTime.Now.AddDays(2),
                StageId = new Guid(stage1Create),
                ProcessId = new Guid(processId)
            };
            campaign.Id = XrmHelper.Create(localContext.OrganizationService, campaign);
            campaign.CampaignId = campaign.Id;

            return campaign;
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
