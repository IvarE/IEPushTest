using System;
using Endeavor.Crm.UnitTest;
using Microsoft.Crm.Sdk.Samples;
using NUnit.Framework;
using Endeavor.Crm;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Skanetrafiken.Crm;
using Microsoft.Crm.Sdk.Messages;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk.Messages;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class GenerateCampaignCodesFixture : PluginFixtureBase
    {
        private readonly string processId = "2D95BBF8-E9FF-4C4B-A988-53E4CC1225A5";
        private readonly string CampaignName = "TestCampaign";
        private readonly int discountPercent = 20;
        private readonly string stage1Create = "ceeb25ac-c704-4612-a296-61cef8986c70";

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
                        leads.Add(CreateLead(localContext, campaign, Generated.lead_leadsourcecode.Spar));
                    }

                    sw.Restart();
                    //GenerateCampaignCodesWrapper.ExecuteCodeActivity(localContext, campaign.ToEntityReference());
                    localContext.TracingService.Trace("Generating codes for " + leadsToTest + " leads took " + sw.ElapsedMilliseconds / 1000 + " seconds.");
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

        [Test]
        public void TestWorkflowGenerateCampaignCodesRecursive()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                Guid GenerateCampaignCodesRecursiveGuid = new Guid("017B49A9-9AB4-45BF-951F-4BE6A0A551FA");

                CampaignEntity campaign = CreateCampaign(localContext);
                int noOfLeads = 5;

                List<LeadEntity> leads = new List<LeadEntity>();

                for (int i = 0; i < noOfLeads; i++)
                {
                    CustomerInfo campaignCustomer = GetTestCustomer(i);
                    LeadEntity campaignLead = new LeadEntity(localContext, campaignCustomer);
                    campaignLead.CampaignId = campaign.ToEntityReference();
                    
                    campaignLead.Id = XrmHelper.Create(localContext, campaignLead);

                    leads.Add(campaignLead);
                }

                List<string> existingCodes = GetExisitingCampaignCodes(localContext, campaign.ToEntityReference());

                List<string> UniqueCampaignCodes = CampaignEntity.generateUniqueCampaignCodes(existingCodes, leads.Count);

                string leadsStr = ConcatListToString(leads.Select(l => l.Id.ToString()));
                string UniqueCampaignCodesStr = ConcatListToString(UniqueCampaignCodes);

                var generateCampaignCodesReq = new OrganizationRequest("ed_GenerateCampaignCodesAction")
                {
                    ["LeadIdsStr"] = leadsStr,
                    ["UniqueCampaignCodesStr"] = UniqueCampaignCodesStr
                };

                localContext.OrganizationService.Execute(generateCampaignCodesReq);

                System.Threading.Thread.Sleep(500);


                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id);

                IList<LeadEntity> updatedLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(true), filter);

                foreach(LeadEntity l in updatedLeads)
                {
                    Assert.NotNull(l.ed_CampaignCode);
                }
            }
        }

        private static string ConcatListToString(IEnumerable<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
            {
                sb.Append(s);
                sb.Append(";");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        private static List<string> GetExisitingCampaignCodes(Plugin.LocalPluginContext localContext, EntityReference campaignId)
        {
            IList<LeadEntity> allLeadsWithCampaignCodes = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode),
            new FilterExpression
            {
                Conditions =
                {
                        new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull)
                }
            });

            List<string> listOfExistingCodes = new List<string>();
            foreach (LeadEntity campaignCode in allLeadsWithCampaignCodes)
            {
                listOfExistingCodes.Add(campaignCode.ed_CampaignCode);
            }

            return listOfExistingCodes;
        }


        [Test]
        public void TestWorkflowGenerateCampaignCodesWrapper()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                
                CampaignEntity campaign = CreateCampaign(localContext);
                int noOfLeads = 10;

                OrganizationRequestCollection createRequests;
                ExecuteMultipleRequest createMultipleRequest;

                for (int i=0; i < noOfLeads/1000; i++)
                {
                    createRequests = new OrganizationRequestCollection();

                    for (int j = 0; j < 1000; j++)
                    {
                        CustomerInfo campaignCustomer = GetTestCustomer(i*1000 + j);
                        LeadEntity campaignLead = new LeadEntity(localContext, campaignCustomer);
                        campaignLead.CampaignId = campaign.ToEntityReference();

                        createRequests.Add(new CreateRequest()
                        {
                            Target = campaignLead
                        });
                    }

                    createMultipleRequest = new ExecuteMultipleRequest()
                    {
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = false
                        },
                        // Create an empty organization request collection.
                        Requests = createRequests
                    };

                    localContext.OrganizationService.Execute(createMultipleRequest);
                }

                createRequests = new OrganizationRequestCollection();

                for (int j = (noOfLeads / 1000)*1000; j < noOfLeads; j++)
                {
                    CustomerInfo campaignCustomer = GetTestCustomer(j);
                    LeadEntity campaignLead = new LeadEntity(localContext, campaignCustomer);
                    campaignLead.CampaignId = campaign.ToEntityReference();

                    createRequests.Add(new CreateRequest()
                    {
                        Target = campaignLead
                    });
                }

                createMultipleRequest = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = false
                    },
                    // Create an empty organization request collection.
                    Requests = createRequests
                };

                localContext.OrganizationService.Execute(createMultipleRequest);

                /*
                CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, new Guid("45C235B9-A49B-E811-8276-00155D010B00"), new ColumnSet(true));
                */

                ExecuteWorkflowRequest workflowRequest = new ExecuteWorkflowRequest()
                {
                    WorkflowId = new Guid("8C76A4D9-680F-4CE8-B3B2-7F096887F32B"),
                    EntityId = campaign.Id
                };

                
                // Execute the workflow.
                ExecuteWorkflowResponse response =
                    (ExecuteWorkflowResponse)_serviceProxy.Execute(workflowRequest);

                System.Threading.Thread.Sleep(10000);

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id);

                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(true), filter);

                foreach(LeadEntity lead in leads)
                {
                    Assert.NotNull(lead.ed_CampaignCode);
                }
            }
        }        

        private CampaignEntity CreateCampaign(Plugin.LocalPluginContext localContext)
        {
            string nowString = DateTime.Now.ToString();
            CampaignEntity campaign = new CampaignEntity
            {
                Name = CampaignName + nowString,
                CodeName = nowString.Substring(0, nowString.Length > 32 ? 31 : nowString.Length - 1),
                ed_DiscountPercent = discountPercent,
                ed_LeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj,
                ed_LeadSource = new OptionSetValue((int)Generated.lead_leadsourcecode.MittKonto),
                ed_ValidFromPhase1 = DateTime.Now.AddDays(-2),
                ed_ValidToPhase1 = DateTime.Now.AddDays(2),
                ProcessId = new Guid(processId),
                StageId = new Guid(stage1Create)
            };
            campaign.Id = XrmHelper.Create(localContext.OrganizationService, campaign);
            campaign.CampaignId = campaign.Id;

            return campaign;
        }

        private CustomerInfo GetTestCustomer(int i)
        {
            string testInstanceName = DateTime.Now.Millisecond.ToString();

            DateTime start = new DateTime(1900, 1, 1);
            DateTime date = start.AddDays(i / 1000);

            string lastFour = ConvertToLastFourString(i % 1000);
            // Build a random, valid, unique personnummer
            string personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(date, lastFour);

            return new CustomerInfo()
            {
                Source = (int)Generated.ed_informationsource.Kampanj,
                FirstName = "CampaignTestFirstName",
                LastName = "CampaignTestLastName:" + personnummer,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "Kampanjvägen " + testInstanceName,
                    PostalCode = "12345",
                    City = "By" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = personnummer,
                Email = string.Format("test{0}@test.test", testInstanceName),
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true

            };
        }

        private string ConvertToLastFourString(int i)
        {
            string s = i.ToString();
            s = s + "0";
            while (s.Count() < 4)
            {
                s = s.Insert(0, "0");
            }

            return s;
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
