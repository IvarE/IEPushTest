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
using System.Threading;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class CampaignResponseFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void CalculateAmountOnCampaignOnCampaignResponseCreate()
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

                CampaignEntity campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, new Guid("8EB5C249-75E2-E811-827A-00155D010B00"), new ColumnSet(true));

                int? totalLeadsBefore = campaign.ed_TotalLeads;
                int? totalQualifiedLeads = campaign.ed_QualifiedLeads;
                int? totalQualifiedLeadsDR1 = campaign.ed_QualifiedLeadsDR1;
                int? totalQualifiedLeadsDR2 = campaign.ed_QualifiedLeadsDR2;

                List<LeadEntity> createdLeads = new List<LeadEntity>();
                for (int i = 0; i < 20; i++)
                {
                    LeadEntity lead = new LeadEntity();
                    lead.FirstName = "Johanna";
                    lead.LastName = "Test" + i;
                    lead.LeadSourceCode = Generated.lead_leadsourcecode.Ovrig;
                    lead.CampaignId = campaign.ToEntityReference();
                    lead.ed_CampaignCode = "ABC123AB" + i;
                    Guid leadId = XrmHelper.Create(localContext, lead);
                    createdLeads.Add(XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, leadId, new ColumnSet(true)));
                    Thread.Sleep(5000);
                }

                int counter = 0;
                DateTime receivedOn = new DateTime();
                foreach (var lead in createdLeads)
                {
                    if (counter < 8)
                        receivedOn = new DateTime(2018, 11, 13);
                    else
                        receivedOn = new DateTime(2018, 12, 15);


                    CampaignResponseEntity response = new CampaignResponseEntity
                    {
                        Subject = $"{lead.FullName} har svarat på {campaign.Name}",
                        RegardingObjectId = campaign.ToEntityReference(),
                        Customer = new List<ActivityPartyEntity>
                        {
                            new ActivityPartyEntity() {
                                PartyId = lead.ToEntityReference()
                            }
                        },
                        PromotionCodeName = lead.ed_CampaignCode,
                        ReceivedOn = receivedOn
                    };
                    //response.HandlePostCampaignResponseCreateAsync(localContext);
                    XrmHelper.Create(localContext, response);
                    Thread.Sleep(5000);

                    counter++;
                }

                Thread.Sleep(20000);

                CampaignEntity updatedCampaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaign.Id, new ColumnSet(true));

                int? totalLeadsAfter = updatedCampaign.ed_TotalLeads;
                int? totalQualifiedLeadsAfter = updatedCampaign.ed_QualifiedLeads;
                int? totalQualifiedLeadsDR1After = updatedCampaign.ed_QualifiedLeadsDR1;
                int? totalQualifiedLeadsDR2After = updatedCampaign.ed_QualifiedLeadsDR2;

                

                
            }
        }

        [Test, Category("Debug")]
        public void UpdateCreateCampaignWithCustomer()
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

                ContactEntity contact1 = new ContactEntity
                {
                    EMailAddress1 = "test1@epost.se",
                    FirstName = "TestPeter",
                    LastName = "Göransson",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                };

                Guid contact1Guid = XrmHelper.Create(localContext, contact1);

                // ActivityParty
                var activityParty1 = new ActivityPartyEntity
                {
                    PartyId = new EntityReference(ContactEntity.EntityLogicalName, contact1Guid)
                };

                QueryExpression queryCampaign = new QueryExpression
                {
                    EntityName = CampaignEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria = new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                        {
                            new ConditionExpression(CampaignEntity.Fields.StateCode, ConditionOperator.Equal, 0)
                        }
                    }
                };

                CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, queryCampaign);

                CampaignResponseEntity campaignResponse1 = new CampaignResponseEntity
                {
                    Customer = new ActivityPartyEntity[] { activityParty1 },
                    RegardingObjectId = campaign.ToEntityReference(),
                };

                campaignResponse1.HandlePreCampaignResponseCreate(localContext);

                if (campaignResponse1.ed_Customer == null)
                    throw new Exception("ed_Customer not set");

                Guid campaignResponse1Guid = XrmHelper.Create(localContext, campaignResponse1);
                campaignResponse1 = XrmRetrieveHelper.Retrieve<CampaignResponseEntity>(localContext, campaignResponse1Guid, new ColumnSet(true));

                foreach (var customer in campaignResponse1.Customer)
                {
                    if (customer.PartyId.Id != contact1Guid)
                    {
                        throw new Exception("Customer and ed_Customer is not the same");
                    }
                }

                ContactEntity contact2 = new ContactEntity
                {
                    EMailAddress1 = "test2@epost.se",
                    FirstName = "TestPeter",
                    LastName = "Göransson",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                };

                Guid contact2Guid = XrmHelper.Create(localContext, contact2);

                // ActivityParty
                var activityParty2 = new ActivityPartyEntity
                {
                    PartyId = new EntityReference(ContactEntity.EntityLogicalName, contact2Guid)
                };

                campaignResponse1.Customer = new ActivityPartyEntity[] { activityParty2 };
                campaignResponse1.HandlePreCampaignResponseUpdate(localContext);

                if (campaignResponse1.ed_Customer == null)
                    throw new Exception("ed_Customer not set");

                XrmHelper.Update(localContext, campaignResponse1);

                campaignResponse1 = XrmRetrieveHelper.Retrieve<CampaignResponseEntity>(localContext, campaignResponse1Guid, new ColumnSet(true));
                foreach (var customer in campaignResponse1.Customer)
                {
                    if (customer.PartyId.Id != contact2Guid)
                    {
                        throw new Exception("Customer and ed_Customer is not the same");
                    }
                }


                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);

                //{
                //    ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1Guid, new ColumnSet(false));

                //    SetStateRequest req = new SetStateRequest
                //    {
                //        EntityMoniker = contact.ToEntityReference(),
                //        State = new OptionSetValue((int)Generated.ContactState.Inactive),
                //        Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                //    };
                //    localContext.OrganizationService.Execute(req);
                //}

                //{
                //    ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact2Guid, new ColumnSet(false));

                //    SetStateRequest req = new SetStateRequest
                //    {
                //        EntityMoniker = contact.ToEntityReference(),
                //        State = new OptionSetValue((int)Generated.ContactState.Inactive),
                //        Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                //    };
                //    localContext.OrganizationService.Execute(req);
                //}

                XrmHelper.Delete(localContext.OrganizationService, campaignResponse1.ToEntityReference());
                //XrmHelper.Delete(localContext.OrganizationService, new EntityReference(ContactEntity.EntityLogicalName, contact1Guid));
                //XrmHelper.Delete(localContext.OrganizationService, new EntityReference(ContactEntity.EntityLogicalName, contact2Guid));
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
