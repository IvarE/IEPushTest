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
    public class AnonymiseLeadsFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Run Always")]
        public void AnonymiseCampaignLeads()
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
                    var correctLeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj;
                    var correctLeadSource = Generated.lead_leadsourcecode.Ovrig;
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

                    AnonymiseUnderlyingLeadsRecursive.ExecuteCodeActivity(localContext, newCampaign.ToEntityReference());

                    #region Test

                    CampaignEntity checkCampaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(true), new FilterExpression
                    {
                        Conditions =
                    {
                        new ConditionExpression(CampaignEntity.Fields.Id, ConditionOperator.Equal, newCampaign.Id)
                    }
                    });

                    IList<LeadEntity> list = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(true), new FilterExpression
                    {
                        Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Equal, newCampaign.Id)
                    }
                    });


                    string anonName = "Anonymous", anonMail = "anonymous@fakemail.com";
                    foreach (LeadEntity l in list)
                    {
                        NUnit.Framework.Assert.AreEqual(anonName, l.LastName);
                        NUnit.Framework.Assert.AreEqual(anonName, l.Address1_Line1);
                        NUnit.Framework.Assert.AreEqual(anonName, l.Address1_Line2);

                        NUnit.Framework.Assert.AreEqual(anonMail, l.EMailAddress1);

                        NUnit.Framework.Assert.AreEqual(null, l.FirstName);
                        NUnit.Framework.Assert.AreEqual(null, l.ed_Personnummer);
                        NUnit.Framework.Assert.AreEqual(null, l.Telephone1);
                        NUnit.Framework.Assert.AreEqual(null, l.MobilePhone);
                        NUnit.Framework.Assert.AreEqual(null, l.ed_CampaignCode);
                    }

                    NUnit.Framework.Assert.AreEqual(4, checkCampaign.ed_AnonymisedLeads);

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

        //[Test, Category("Run Always")]
        //public void CreateNextCampaign()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //        stopwatch.Start();

        //        DateTime lastJan = new DateTime(2012, 01, 31);

        //        int _exampleYear1 = 2013, _exampleYear2 = 2014;
        //        int _exampleMonth1 = 5, _exampleMonth2 = 12;
        //        string _campaignName1 = "EndeavorTestCampaign1", _campaignName2 = "EndeavorTestCampaign2";
        //        DateTime _tryOutFrom1 = new DateTime(_exampleYear1, _exampleMonth1, 1);
        //        DateTime _tryOutFrom2 = new DateTime(_exampleYear2, _exampleMonth2, 1);
        //        DateTime _tryOutTo1 = new DateTime(_exampleYear1, _exampleMonth1, DateTime.DaysInMonth(_exampleYear1, _exampleMonth1));
        //        DateTime _tryOutTo2 = new DateTime(_exampleYear2, _exampleMonth2, DateTime.DaysInMonth(_exampleYear2, _exampleMonth2));
        //        DateTime _lastResponseDate1 = new DateTime(_exampleYear1, _exampleMonth1, DateTime.DaysInMonth(_exampleYear1, _exampleMonth1));
        //        DateTime _lastResponseDate2 = new DateTime(_exampleYear2, _exampleMonth2, DateTime.DaysInMonth(_exampleYear2, _exampleMonth2));
        //        CampaignEntity existingCampaign1 = null, existingCampaign2 = null;
        //        CampaignEntity createdCampaign1 = null, createdCampaign2 = null;
        //        EntityReference newCampaign1 = null, newCampaign2 = null;
        //        //CurrencyEntity aCurrency = XrmRetrieveHelper.RetrieveFirst<CurrencyEntity>(localContext, new ColumnSet(false));

        //        // Test Part 1
        //        try
        //        {
        //            existingCampaign1 = new CampaignEntity
        //            {
        //                ed_Year = _exampleYear1,
        //                ed_Month = _exampleMonth1,
        //                Name = _campaignName1,
        //                TypeCode = Generated.campaign_typecode.NyinflyttadKampanj,
        //                ed_LastResponseDate = _lastResponseDate1,
        //                ed_TryOutFrom = _tryOutFrom1,
        //                ed_TryOutTo = _tryOutTo1,
        //                ed_OrderMethod = Generated.ed_campaign_ed_ordermethod.Singapore
        //            };
        //            existingCampaign1.Id = XrmHelper.Create(localContext.OrganizationService, existingCampaign1);
        //            existingCampaign1.CampaignId = existingCampaign1.Id;

        //            GenerateNextCampaign codeActivity = new GenerateNextCampaign();
        //            newCampaign1 = codeActivity.ExecuteCodeActivity(localContext, existingCampaign1.ToEntityReference());

        //            createdCampaign1 = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(true),
        //                new FilterExpression
        //                {
        //                    Conditions =
        //                    {
        //                        new ConditionExpression(CampaignEntity.Fields.CampaignId, ConditionOperator.Equal, newCampaign1.Id)
        //                    }
        //                });

        //            DateTime nextMonth = new DateTime(_exampleYear1, _exampleMonth1, 1).AddMonths(1);

        //            NUnit.Framework.Assert.NotNull(createdCampaign1);
        //            NUnit.Framework.Assert.AreEqual(nextMonth.Year, createdCampaign1.ed_Year);
        //            NUnit.Framework.Assert.AreEqual(nextMonth.Month, createdCampaign1.ed_Month);
        //            NUnit.Framework.Assert.AreEqual(Generated.campaign_typecode.NyinflyttadKampanj, createdCampaign1.TypeCode);

        //            // TODO: teo - Verify desired behaviour
        //            NUnit.Framework.Assert.AreEqual(existingCampaign1.ed_OrderMethod, createdCampaign1.ed_OrderMethod);
        //            NUnit.Framework.Assert.AreEqual(existingCampaign1.ed_LastResponseDate.Value.AddMonths(1), existingCampaign1.ed_LastResponseDate.Value);

        //        }
        //        catch (Exception e)
        //        {
        //            localContext.Trace($"CreateNextCampaign threw an unexpected error: {e.Message}");
        //            throw e;
        //        }
        //        finally
        //        {
        //            if (existingCampaign1?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign1.ToEntityReference()); } catch { }
        //            }
        //            if (createdCampaign1?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign1.ToEntityReference()); } catch { }
        //            }
        //            if (newCampaign1?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign1.ToEntityReference()); } catch { }
        //            }
        //        }


        //        // Test Part 2
        //        try
        //        {
        //            existingCampaign2 = new CampaignEntity
        //            {
        //                ed_Year = _exampleYear2,
        //                ed_Month = _exampleMonth2,
        //                Name = _campaignName2,
        //                TypeCode = Generated.campaign_typecode.NyinflyttadKampanj,
        //                ed_LastResponseDate = _lastResponseDate2,
        //                ed_TryOutFrom =_tryOutFrom2,
        //                ed_TryOutTo = _tryOutTo2,
        //                ed_OrderMethod = Generated.ed_campaign_ed_ordermethod.Singapore
        //            };
        //            existingCampaign2.Id = XrmHelper.Create(localContext.OrganizationService, existingCampaign2);
        //            existingCampaign2.CampaignId = existingCampaign2.Id;

        //            GenerateNextCampaign codeActivity = new GenerateNextCampaign();
        //            newCampaign2 = codeActivity.ExecuteCodeActivity(localContext, existingCampaign2.ToEntityReference());

        //            createdCampaign2 = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(true),
        //                new FilterExpression
        //                {
        //                    Conditions =
        //                    {
        //                        new ConditionExpression(CampaignEntity.Fields.CampaignId, ConditionOperator.Equal, newCampaign2.Id)
        //                    }
        //                });

        //            DateTime nextMonth = new DateTime(_exampleYear1, _exampleMonth1, 1).AddMonths(1);

        //            NUnit.Framework.Assert.NotNull(createdCampaign2);
        //            NUnit.Framework.Assert.AreEqual(nextMonth.Year, createdCampaign2.ed_Year);
        //            NUnit.Framework.Assert.AreEqual(nextMonth.Month, createdCampaign2.ed_Month);
        //            NUnit.Framework.Assert.AreEqual(Generated.campaign_typecode.NyinflyttadKampanj, createdCampaign2.TypeCode);

        //            // TODO: teo - Verify desired behaviour
        //            NUnit.Framework.Assert.AreEqual(existingCampaign2.ed_OrderMethod, createdCampaign2.ed_OrderMethod);

        //        }
        //        catch (Exception e)
        //        {
        //            localContext.Trace($"CreateNextCampaign threw an unexpected error: {e.Message}");
        //            throw e;
        //        }
        //        finally
        //        {
        //            if (existingCampaign2?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign2.ToEntityReference()); } catch { }
        //            }
        //            if (createdCampaign2?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign2.ToEntityReference()); } catch { }
        //            }
        //            if (newCampaign2?.Id != null)
        //            {
        //                try { XrmHelper.Delete(localContext.OrganizationService, existingCampaign2.ToEntityReference()); } catch { }
        //            }
        //        }

        //        localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
        //    }
        //}

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
