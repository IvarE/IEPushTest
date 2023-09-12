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

namespace Endeavor.Crm.IntegrationTests
{
    [TestFixture]
    public class LeadFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;
        private readonly int discountPercent = 20;

        private static readonly string FirstName = "Endeavor";
        private static readonly string LastName = "Ab";
        private static readonly string Subject = "Test Subject";
        private static readonly string EmailAdress1 = "test@endeavortest.com";


        private readonly string CampaignName = "TestCampaign";
        private readonly string TestProductNumber = "TestProductNumber";
        private readonly string TestProductName = "TestProductName";
        private readonly string processId = "2D95BBF8-E9FF-4C4B-A988-53E4CC1225A5";
        private readonly string stage1Create = "ceeb25ac-c704-4612-a296-61cef8986c70";


        [Test, Category("Regression")]
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
                    Source = (int)Generated.ed_informationsource.AdmSkapaKund,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    SocialSecurityNumber = contact.cgi_socialsecuritynumber,
                    SwedishSocialSecurityNumber = false,
                    SwedishSocialSecurityNumberSpecified = true,
                    Email = contact.EMailAddress1,
                    Mobile = contact.Telephone2
                };

                StatusBlock block = LeadEntity.CanLeadBeCreated(localContext, info);

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);


            }
        }

        [Test, Category("Debug")]
        public void ToLeadInfoTest()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    CampaignEntity campaign = CreateCampaign(localContext);
                    ProductEntity product = GetOrCreateTestProduct(localContext);
                    AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                    //Skapa ett lead
                    LeadEntity newLead = createNewLead(localContext, campaign);
                    
                    LeadInfo info = (LeadInfo)newLead.ToLeadInfo(localContext);

                    //Tear down
                    try { XrmHelper.Delete(localContext.OrganizationService, newLead.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, newLead.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, product.ToEntityReference()); } catch { }

                    Assert.AreEqual(campaign.CodeName, info.CampaignId);
                    Assert.AreEqual(discountPercent, info.CampaignDiscountPercent);
                    Assert.AreEqual(product.Name, info.CampaignProducts[0]);

                }
                catch (Exception e)
                {
                    throw e;
                }


            }

        }
        
        [Test, Category("Regression")]
        public void CreateCampaignLeads()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(false),
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(CampaignEntity.Fields.Name, ConditionOperator.Equal, "Jojo Företag")
                            }
                        });
                    ProductEntity product = GetOrCreateTestProduct(localContext);
                    AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                    int numberOfLeadsToCreate = 10;
                    //Skapa ett lead
                    for (int i = 0; i < numberOfLeadsToCreate; i++)
                    {
                        LeadEntity newLead = createNewLead(localContext, campaign);
                    }
                }
                catch (Exception e)
                {
                    throw e;
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

        private ProductEntity GetOrCreateTestProduct(Plugin.LocalPluginContext localContext)
        {
            ProductEntity product = XrmRetrieveHelper.RetrieveFirst<ProductEntity>(localContext, new ColumnSet(ProductEntity.Fields.Name, ProductEntity.Fields.ProductNumber,
                ProductEntity.Fields.QuantityDecimal),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(ProductEntity.Fields.ProductNumber, ConditionOperator.Equal, TestProductNumber)
                    }
                });

            if (product == null)
            {
                product = new ProductEntity
                {
                    ProductNumber = TestProductNumber,
                    Name = TestProductName,
                    QuantityDecimal = 2,
                    DefaultUoMScheduleId = XrmRetrieveHelper.RetrieveFirst<UnitGroupEntity>(localContext, new ColumnSet(false)).ToEntityReference(),
                    DefaultUoMId = XrmRetrieveHelper.RetrieveFirst<UnitEntity>(localContext, new ColumnSet(false)).ToEntityReference()
                };
                product.Id = XrmHelper.Create(localContext, product);
                product.ProductId = product.Id;
            }

            return product;
        }

        private void AssociateProductToCampaignIfNeeded(Plugin.LocalPluginContext localContext, CampaignEntity campaign, ProductEntity product)
        {
            CampaignItemEntity association = XrmRetrieveHelper.RetrieveFirst<CampaignItemEntity>(localContext, new ColumnSet(false),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(CampaignItemEntity.Fields.CampaignId, ConditionOperator.Equal, campaign.Id),
                        new ConditionExpression(CampaignItemEntity.Fields.CampaignItemId, ConditionOperator.Equal, product.Id)
                    }
                });

            if (association == null)
            {
                var request = new AddItemCampaignRequest
                {
                    CampaignId = campaign.Id,
                    EntityId = product.Id,
                    EntityName = ProductEntity.EntityLogicalName,
                };
                _serviceProxy.Execute(request);
            }
        }

        [Test, Category("Debug")]
        public void LeadSetStateAsync()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                
                string ProcessId = "2d95bbf8-e9ff-4c4b-a988-53e4cc1225a5";
                string CreateStage = "ceeb25ac-c704-4612-a296-61cef8986c70";
                string DR1 = "189dc031-75b4-4464-b8a5-1d1e181945d8";
                string DR2 = "74f9a153-4824-47d9-bcdd-9b42b2fe2556";
                string EndStage = "20bbf364-e634-406b-b322-5b922fecaead";

                CampaignEntity c = null;
                List<LeadEntity> leads = new List<LeadEntity>();
                int numberOfLeads = 50;

                try
                {
                    c = CampaignFixture.CreateCampaign(localContext);
                    CampaignEntity updateCamp = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(ProcessId),
                        StageId = new Guid(DR1)
                    };
                    XrmHelper.Update(localContext, updateCamp);

                    for (int i = 0; i < numberOfLeads ; i++)
                    {
                        leads.Add(createQualifiedLead(localContext, c));
                    }
                    
                    LeadEntity.HandlePostLeadSetStateAsync(localContext, new EntityReference(LeadEntity.EntityLogicalName, leads.First().Id), new OptionSetValue((int)Generated.LeadState.Qualified), new OptionSetValue((int)leads.First().StatusCode));

                    CampaignEntity checkCampaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, c.ToEntityReference(), new ColumnSet(true));

                    Assert.AreEqual(leads.Count, checkCampaign.ed_QualifiedLeadsDR1);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    foreach (LeadEntity l in leads)
                    {
                        try { XrmHelper.Delete(localContext, l.ToEntityReference()); } catch { }
                    }
                    try { XrmHelper.Delete(localContext, c.ToEntityReference()); } catch { }
                }
            }
        }

        private LeadEntity createQualifiedLead(Plugin.LocalPluginContext localContext, CampaignEntity c)
        {
            IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode), new FilterExpression { Conditions = { new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull)} });
            List<string> existingCodes = new List<string>();
            foreach (LeadEntity ld in leads)
                existingCodes.Add(ld.ed_CampaignCode);
            List<string> codeList = CampaignEntity.generateUniqueCampaignCodes(existingCodes, 1);

            LeadEntity l = new LeadEntity
            {
                CampaignId = c.ToEntityReference(),
                FirstName = FirstName,
                LastName = LastName,
                Subject = Subject,
                EMailAddress1 = EmailAdress1,
                ed_CampaignCode = codeList[0]
            };
            l.Id = XrmHelper.Create(localContext.OrganizationService, l);
            QualifyLeadRequest req = new QualifyLeadRequest
            {
                CreateAccount = false,
                CreateContact = false,
                CreateOpportunity = false,
                LeadId = l.ToEntityReference(),
                SourceCampaignId = c.ToEntityReference(),
                Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
            };
            QualifyLeadResponse resp = (QualifyLeadResponse)localContext.OrganizationService.Execute(req);
            return XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, l.ToEntityReference(), new ColumnSet(true));
        }

        // Functionality moved to Asynchronous workflow
        //[Test, Category("Regression")]
        //public void HandlePreLeadCreateTest()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            CampaignEntity campaign = CampaignFixture.createNewCampaign(localContext);

        //            LeadEntity lead = createNewLead(localContext, campaign);
        //            lead.ed_InformationSource = Generated.ed_informationsource.Kampanj;
        //            lead.HandlePreLeadCreate(localContext);

        //            Assert.AreEqual(lead.Subject, campaign.ed_LeadTopic.ToString());
        //            Assert.IsNotNull(lead.CampaignId);
        //        }
        //        catch (Exception e)
        //        {
        //            throw e;
        //        }
        //    }

        //}

        [Test, Category("Regression")]
        public void CampaignLeadDuplicateDetectionPlugin()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                CampaignEntity campaign = null;
                LeadEntity lead1 = null, lead2 = null, lead3 = null, lead4 = null;

                try
                {
                    campaign = CampaignFixture.createNewCampaign(localContext);

                    //lead1 = createNewLead(localContext, campaign);
                    lead1 = new LeadEntity
                    {
                        FirstName = "LeadFirstName",
                        LastName = "LeadLastName" + DateTime.Now.ToString(),
                        Subject = Subject,
                        EMailAddress1 = "testmail@test.com",
                        CampaignId = campaign.ToEntityReference(),
                        ed_InformationSource = Generated.ed_informationsource.Kampanj
                    };
                    lead1.Id = XrmHelper.Create(localContext.OrganizationService, lead1);
                    try
                    {
                        lead2 = new LeadEntity
                        {
                            FirstName = lead1.FirstName,
                            LastName = lead1.LastName,
                            Subject = lead1.Subject,
                            EMailAddress1 = lead1.EMailAddress1,
                            CampaignId = campaign.ToEntityReference(),
                            ed_InformationSource = Generated.ed_informationsource.Kampanj
                        };
                        lead2.Id = XrmHelper.Create(localContext.OrganizationService, lead2);
                        throw new Exception("Should have thrown an exception when creating a duplicate");
                    }
                    catch (Exception e)
                    {
                        if (e.Message != "There is already a lead with the same Name and Email in the assigned Campaign")
                            throw e;
                    }
                    lead3 = new LeadEntity
                    {
                        FirstName = lead1.FirstName,
                        LastName = lead1.LastName,
                        Subject = Subject,
                        EMailAddress1 = EmailAdress1 + "2nd",
                        CampaignId = campaign.ToEntityReference(),
                        ed_InformationSource = Generated.ed_informationsource.Kampanj,
                        ed_Personnummer = GenerateValidSocialSecurityNumber(DateTime.Now)
                    };
                    lead3.Id = XrmHelper.Create(localContext.OrganizationService, lead3);
                    try
                    {
                        lead4 = new LeadEntity
                        {
                            FirstName = lead3.FirstName + "2",
                            LastName = lead3.LastName + "2",
                            Subject = lead3.Subject,
                            EMailAddress1 = lead3.EMailAddress1,
                            CampaignId = campaign.ToEntityReference(),
                            ed_InformationSource = Generated.ed_informationsource.Kampanj,
                            ed_Personnummer = lead3.ed_Personnummer
                        };
                        lead4.Id = XrmHelper.Create(localContext.OrganizationService, lead4);
                        throw new Exception("Duplicate Creation should have thrown an exception");
                    }
                    catch (Exception e)
                    {
                        if (e.Message != "There is already a lead with the same Social Security Number in the assigned Campaign")
                            throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    try { XrmHelper.Delete(localContext.OrganizationService, lead1.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, lead2.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, lead3.ToEntityReference()); } catch { }
                    try { XrmHelper.Delete(localContext.OrganizationService, campaign.ToEntityReference()); } catch { }
                }
            }

        }


        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }

        public static LeadEntity createNewLead(Plugin.LocalPluginContext localContext, CampaignEntity campaign)
        {
            IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode), new FilterExpression { Conditions = { new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull) } });
            List<string> existingCodes = new List<string>();
            foreach (LeadEntity ld in leads)
                existingCodes.Add(ld.ed_CampaignCode);
            List<string> codeList = CampaignEntity.generateUniqueCampaignCodes(existingCodes, 1);

            LeadEntity newLead = new LeadEntity
            {
                FirstName = FirstName,
                LastName = LastName + DateTime.Now.ToString() + Random4Digits(),
                Subject = Subject,
                ed_CampaignCode = codeList[0],
                EMailAddress1 = EmailAdress1,
                CampaignId = campaign.ToEntityReference(),
                ed_InformationSource = Generated.ed_informationsource.Kampanj
            };
            newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);

            return XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, newLead.ToEntityReference(), new ColumnSet(true));
        }
        
        private ProductEntity getTestProduct(Plugin.LocalPluginContext localContext)
        {
            ProductEntity product = XrmRetrieveHelper.RetrieveFirst<ProductEntity>(localContext, new ColumnSet(ProductEntity.Fields.Name),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(ProductEntity.Fields.Id, ConditionOperator.Equal, "8AC3CECB-7D73-E711-81D2-00155D010B02")
                    }
                });

            return product;
        }

        /// <summary>
        /// Get unique string YYMMDD.HHSS
        /// </summary>
        /// <returns></returns>
        public static string GetUnitTestID()
        {
            DateTime today = DateTime.Now;

            return today.ToString("yyyyMMdd.hhmm");
        }

        public static string GenerateValidSocialSecurityNumber(DateTime dateTime)
        {
            string datum = dateTime.ToString("yyyyMMdd").Replace("-", "").Substring(0, 8);
            StringBuilder personnummer = new StringBuilder(datum);
            personnummer.Append(Random4Digits());
            string pnr9digits = personnummer.ToString().Substring(2, 9);
            personnummer[11] = CustomerUtility.calculateCheckDigit(pnr9digits)[0];
            return personnummer.ToString();
        }

        private static string Random4Digits()
        {
            Random rnd = new Random();
            int next = rnd.Next(0, 10000);
            return (next + 10000).ToString().Substring(1);
        }
    }
}
