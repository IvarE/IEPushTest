using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using System.Net;
using System.IO;
using System.Linq;
using Skanetrafiken.Crm.Controllers;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm.Extensions;
using System.Collections.Generic;
using System.Diagnostics;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class WebApiTestCampaign : PluginFixtureBase
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

        private readonly string CampaignCode = "888888";
        private readonly string CampaignCodeName = "CMP-" + DateTime.Now.ToString("mm:ss") + "-M1N7";
        private readonly string D1CampaignCodeName = "Campaign In D1";
        private readonly string FirstName = "Endeavor";
        private readonly string LastName = "Ab";
        private readonly string ed_Personnummer = "199304129415";
        private readonly string MobilePhone = "46123456789";
        private readonly string EMailAddress1 = "endeavorab@endeavor.test";
        private readonly string Address1_Line1 = "Care Of-sträng";
        private readonly string Address1_Line2 = "HVITFELDTSGATAN 15";
        private readonly string Address1_PostalCode = "41120";
        private readonly string Address1_City = "Gothenburg";
        private readonly string ed_Address1_Country = "Sverige";
        private readonly string CampaignName = "TestCampaign";
        private readonly string TestProductNumber = "TestProductNumber";
        private readonly string TestProductName = "TestProductName";
        private readonly int discountPercent = 20;
        private static Random random = new Random();

        private readonly string processId = "2D95BBF8-E9FF-4C4B-A988-53E4CC1225A5";
        private readonly string stage1Create = "ceeb25ac-c704-4612-a296-61cef8986c70";
        private readonly string stage2DR1 = "189dc031-75b4-4464-b8a5-1d1e181945d8";
        private readonly string stage3DR2 = "74f9a153-4824-47d9-bcdd-9b42b2fe2556";
        private readonly string stage4End = "20bbf364-e634-406b-b322-5b922fecaead";

        private readonly string shouldShowThisNumber = "11111111";
        private readonly string shouldNotShowThisNumber = "00000000";
        private readonly string shouldNotShowThis = "Should not show this";


        //[Test, Category("Regression")]
        //public void QualifyLeadToExistingContact()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        _serviceProxy.EnableProxyTypes();
        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        string personummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
        //        LeadEntity newLead = CreateNewLead(localContext, personummer);
        //        CustomerInfo customerInfo = newLead.ToCustomerInfo(localContext);

        //        #region Test validera lead till existerande kontakt

        //        #region HTTP Put
        //        try
        //        {
        //            string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{customerInfo.Guid}";
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //            WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, newLead.Id);
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "PUT";


        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                string InputJSON = JsonConvert.SerializeObject(customerInfo, Formatting.None);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }

        //            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.ToString());
        //        }
        //        #endregion

        //        ContactEntity getContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false), new FilterExpression
        //        {
        //            Conditions =
        //                {
        //                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, customerInfo.Email)
        //                }
        //        });

        //        Assert.IsNotNull(getContact);

        //        #endregion

        //        XrmHelper.Delete(localContext.OrganizationService, newLead.ToEntityReference());

        //    }
        //}

        //[Test, Category("Regression")]
        //public void QualifyLeadToNewContact()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        _serviceProxy.EnableProxyTypes();
        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        string personummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
        //        LeadEntity newLead = CreateNewLead(localContext, personummer);

        //        #region Test validera Lead till Contact
        //        {
        //            var customerInfo = new CustomerInfo
        //            {
        //                Guid = newLead.Id.ToString(),
        //                FirstName = FirstName,
        //                LastName = LastName,
        //                Email = EMailAddress1,
        //                Source = (int)CustomerUtility.Source.Kampanj
        //            };

        //            try
        //            {
        //                string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{customerInfo.Guid}";
        //                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, newLead.Id);
        //                httpWebRequest.ContentType = "application/json";
        //                httpWebRequest.Method = "PUT";


        //                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //                {
        //                    string InputJSON = JsonConvert.SerializeObject(customerInfo, Formatting.None);

        //                    streamWriter.Write(InputJSON);
        //                    streamWriter.Flush();
        //                    streamWriter.Close();
        //                }

        //                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

        //            }
        //            catch (Exception e)
        //            {
        //                throw new Exception(e.ToString());
        //            }

        //            ContactEntity getContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false), new FilterExpression
        //            {
        //                Conditions =
        //                {
        //                    new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, customerInfo.Email)
        //                }
        //            });

        //            Assert.IsNotNull(getContact);

        //            DeleteContact(localContext, getContact);
        //        }
        //        #endregion

        //        XrmHelper.Delete(localContext.OrganizationService, newLead.ToEntityReference());

        //    }
        //}

        [Test, Category("Regression")]
        public void KeepCampaignLeadsSeperate()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    #region setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CustomerInfo campaignCustomer = ValidCampaignCustomerInfo(testInstanceName);

                    LeadEntity campaignLead = new LeadEntity(localContext, campaignCustomer);
                    CampaignEntity campaign = CreateCampaign(localContext);
                    campaignLead.CampaignId = campaign.ToEntityReference();

                    campaignLead.Id = XrmHelper.Create(localContext, campaignLead);
                    #endregion

                    #region Skapa MittKonto
                    CustomerInfo customer = WebApiTest.ValidCustomerInfo_FullTest(testInstanceName);
                    customer.FirstName = campaignLead.FirstName;
                    customer.LastName = campaignLead.LastName;
                    customer.Email = campaignCustomer.Email;
                    customer.SocialSecurityNumber = campaignCustomer.SocialSecurityNumber;

                    Guid customerId;
                    string linkGuid;
                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("\nSkapaKontoLead:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}leads/", WebApiTestHelper.WebApiRootEndpoint));
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer.Source = (int)Generated.ed_informationsource.SkapaMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customer);

                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateCustomerLead results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Assert.AreEqual(customer.Email, info.Email);
                            Assert.AreEqual(customer.FirstName, info.FirstName);
                            Assert.AreEqual(customer.LastName, info.LastName);
                            Assert.AreEqual(customer.Telephone, info.Telephone);
                            Assert.AreEqual(customer.Mobile, info.Mobile);
                            Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            Assert.NotNull(info.AddressBlock);
                            Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }

                            //// Get lead to validate subject
                            //LeadEntity newlyCreatedLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, customerId
                            //        , new ColumnSet(LeadEntity.Fields.Subject));

                            //// Make sure we have a subject
                            //NUnit.Framework.Assert.IsNotNull(newlyCreatedLead.Subject);
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Lead
                    {
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Lead:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLatestLinkGuid/{customer.Email}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion


                    #region ValideraEmailSkapaKund
                    {
                        localContext.TracingService.Trace("\nValidera epost:");
                        CustomerInfo verifyEmailInfo = new CustomerInfo
                        {
                            Guid = customerId.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "jättemycketMKlId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, verifyEmailInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Assert.AreEqual(customer.Email, info.Email);
                            Assert.AreEqual(customer.FirstName, info.FirstName);
                            Assert.AreEqual(customer.LastName, info.LastName);
                            Assert.AreEqual(customer.Telephone, info.Telephone);
                            Assert.AreEqual(customer.Mobile, info.Mobile);
                            Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            Assert.NotNull(info.AddressBlock);
                            Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion
                    #endregion

                    #region Oinloggat Köp
                    {
                        CustomerInfo nonLoginPurchaseInfo = ValidNonLoginPurchaseCustomerInfo(testInstanceName);
                        nonLoginPurchaseInfo.Email = campaignCustomer.Email;
                        nonLoginPurchaseInfo.SocialSecurityNumber = campaignCustomer.SocialSecurityNumber;

                        localContext.TracingService.Trace("\nOinloggatKöp Existerande Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            nonLoginPurchaseInfo.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, nonLoginPurchaseInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatKöp Existerande Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Assert.AreEqual(nonLoginPurchaseInfo.Email, info.Email);
                        }
                    }
                    #endregion


                    //#region OinloggatKundärende

                    //#endregion


                    //#region RGOL

                    //#endregion


                    //#region PASS

                    //#endregion


                    //#region OinloggatLaddaKort

                    //#endregion

                    LeadEntity controlLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, campaignLead.ToEntityReference(), new ColumnSet(LeadEntity.Fields.StateCode));
                    Assert.AreEqual(Generated.LeadState.Open, controlLead.StateCode);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("KeepCampaignLeadsSeperate returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        [Test, Category("Regression")]
        public void TestCampaignTotalNumberOfLeads()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                CampaignEntity campaign = CreateCampaign(localContext);
                int noOfLeads = 5005;

                IList<Guid> leads = new List<Guid>();
                Random random = new Random();

                for (int i = 5000; i < noOfLeads; i++)
                {
                    Debug.WriteLine("Generate lead " + i);
                    //GenerateRandomLead(localContext, campaign);
                    //leads.Add(campaignLeadId);

                    localContext.TracingService.Trace("\nSkapaKontoLead:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}leads/", WebApiTestHelper.WebApiRootEndpoint));
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                }

                campaign = XrmRetrieveHelper.Retrieve<CampaignEntity>(localContext, campaign.Id, new ColumnSet(true));
                Assert.AreEqual(noOfLeads, campaign.ed_TotalLeads);

                foreach(Guid g in leads)
                {
                    XrmHelper.Delete(localContext, new EntityReference(LeadEntity.EntityLogicalName, g));
                }
                    
            }
        }

        private CustomerInfo GetTestCustomer(int i)
        {
            string testInstanceName = DateTime.Now.Millisecond.ToString();

            DateTime start = new DateTime(1900, 1, 1);
            DateTime date = start.AddDays(i/1000);

            string lastFour = ConvertToLastFourString(i % 1000);
            // Build a random, valid, unique personnummer
            string personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(date, lastFour);
            Debug.WriteLine(personnummer);

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
            while(s.Count() < 4)
            {
                s = s.Insert(0, "0");
            }

            return s;
        }

        private CustomerInfo GetRandomCustomer()
        {
            string testInstanceName = DateTime.Now.Millisecond.ToString();

            DateTime start = new DateTime(1900, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime date = start.AddDays(random.Next(range));

            CustomerInfo customer = ValidCampaignCustomerInfo(testInstanceName, date);
            customer.Source = (int)Generated.ed_informationsource.Kampanj;

            return customer;
        }

        [Test, Category("Regression")]
        public void QualifyLead()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                IList<LeadEntity> allLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull)
                        }
                    });
                IEnumerable<string> codes = allLeads.Select(l => l.ed_CampaignCode);
                string code = CampaignEntity.generateUniqueCampaignCodes(codes.ToList(), 1).First();
                CampaignEntity campaign = CreateCampaign(localContext);
                ProductEntity product = GetOrCreateTestProduct(localContext);
                AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                LeadEntity newLead = CreateNewLeadWithCampaignId(localContext, campaign, code);

                try
                {
                    var leadInformation = newLead.ToLeadInfo(localContext);
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{leadInformation.Guid}";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, newLead.Id);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInformation);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("QualifyLead done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("QualifyLead returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }


                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(CampaignResponseEntity.Fields.RegardingObjectId, ConditionOperator.Equal, campaign.Id);

                CampaignResponseEntity campaignResponse = XrmRetrieveHelper.RetrieveMultiple<CampaignResponseEntity>(localContext, new ColumnSet(true), filter)[0];
                Assert.AreEqual(newLead.FirstName + " " + newLead.LastName + " har svarat på " + campaign.Name, campaignResponse.Subject);
                Assert.AreEqual(newLead.ed_CampaignCode, campaignResponse.PromotionCodeName);

            }
        }

        [Test, Category("Debug")]
        public void GetSpecificLeadInformation()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string code = "at1xyv";
                //string code = new Random().Next(0, 1000000).ToString("D6");
                //CampaignEntity campaign = CreateCampaign(localContext);
                //ProductEntity product = GetOrCreateTestProduct(localContext);
                //AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                //LeadEntity newLead = CreateNewLeadWithCampaignId(localContext, campaign, code);

                #region Test hämta kundinfo från lead med kampanjkod
                try
                {
                    var leadInformation = new LeadInfo();
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLeadInfo/{code}/";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    //Internal Request Error: Probably two or more leads with same campaign code
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInfo done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }

                    Assert.AreEqual(FirstName, leadInformation.FirstName);
                    Assert.AreEqual(LastName, leadInformation.LastName);
                    Assert.AreEqual(ed_Personnummer, leadInformation.SocialSecurityNumber);
                    Assert.AreEqual(MobilePhone, leadInformation.Mobile);
                    Assert.AreEqual(EMailAddress1, leadInformation.Email);
                    Assert.AreEqual(Address1_PostalCode, leadInformation.AddressBlock.PostalCode);
                    Assert.AreEqual(Address1_City, leadInformation.AddressBlock.City);
                    //Assert.AreEqual(campaign.CodeName, leadInformation.CampaignId);
                    Assert.NotZero(leadInformation.CampaignProducts.Count());
                    Assert.AreEqual(discountPercent, leadInformation.CampaignDiscountPercent);

                    //DeleteLead(localContext, newLead);
                    //DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInformation returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                #endregion
            }

        }

        [Test, Category("Regression")]
        public void GetLeadInformation()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string code = new Random().Next(0, 1000000).ToString("D6");
                CampaignEntity campaign = CreateCampaign(localContext);
                ProductEntity product = GetOrCreateTestProduct(localContext);
                AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                LeadEntity newLead = CreateNewLeadWithCampaignId(localContext, campaign, code);

                #region Test hämta kundinfo från lead med kampanjkod
                try
                {
                    var leadInformation = new LeadInfo();
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLeadInfo/{code}/";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    //Internal Request Error: Probably two or more leads with same campaign code
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInfo done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }

                    Assert.AreEqual(FirstName, leadInformation.FirstName);
                    Assert.AreEqual(LastName, leadInformation.LastName);
                    Assert.AreEqual(ed_Personnummer, leadInformation.SocialSecurityNumber);
                    Assert.AreEqual(MobilePhone, leadInformation.Mobile);
                    Assert.AreEqual(EMailAddress1, leadInformation.Email);
                    Assert.AreEqual(Address1_PostalCode, leadInformation.AddressBlock.PostalCode);
                    Assert.AreEqual(Address1_City, leadInformation.AddressBlock.City);
                    Assert.AreEqual(campaign.CodeName, leadInformation.CampaignId);
                    Assert.NotZero(leadInformation.CampaignProducts.Count());
                    Assert.AreEqual(discountPercent, leadInformation.CampaignDiscountPercent);

                    DeleteLead(localContext, newLead);
                    DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInformation returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                #endregion
            }

        }

        [Test, Category("Debug")]
        public void QualifySingleLead()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                #region Test hämta kundinfo från lead med kampanjkod
                try
                {
                    LeadInfo leadInformation = null;
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLeadInfo/G9BIAV/";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    //Internal Request Error: Probably two or more leads with same campaign code
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInfo done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }

                    leadInformation.Source = (int)Generated.ed_informationsource.Kampanj;
                    leadInformation.Email = "test@kampanj.se";

                    url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{leadInformation.Guid}";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, new Guid(leadInformation.Guid));
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInformation);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("QualifyLead done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }

                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetLeadInformation returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                #endregion
            }

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


        [Test, Explicit]
        public void DEBUG_PutCampaignLead()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    //LeadEntity campaignLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    //    new FilterExpression
                    //    {
                    //        Conditions =
                    //        {
                    //            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, "abcdef")
                    //        }
                    //    });
                    LeadInfo info = null;

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLeadInfo/abcdef/";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                        info = JsonConvert.DeserializeObject<LeadInfo>(result);
                    }
                    info.Source = 7;
                    info.Email = "test@test.test";
                    info.Mobile = "123456789";

                    url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{info.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, new Guid(info.Guid));
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(info, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref info);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);

                    }
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutQualifyLeadToCstomer_MatchOnNothing returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void TestWorkflowTimelimit()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                ExecuteWorkflowRequest request = new ExecuteWorkflowRequest()
                {
                    WorkflowId = new Guid("12E6f980-8B47-4790-BAA4-f5f478ADF30D"),
                    EntityId = new Guid("0EBC8373-DDEC-E411-80D8-005056903A38")
                };

                // Execute the workflow.
                ExecuteWorkflowResponse response =
                    (ExecuteWorkflowResponse)_serviceProxy.Execute(request);
            }
        }

#if DEV || TEST

        [Test, Category("Regression")]
        public void PutCampaignLead_MatchOnNothing()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på ingenting
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    LeadInfo matchOnNothingLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    matchOnNothingLead.CampaignId = campaign.CodeName;

                    WrapperController.FormatLeadInfo(ref matchOnNothingLead);
                    LeadEntity campaignLead = new LeadEntity
                    {
                        FirstName = matchOnNothingLead.FirstName,
                        LastName = matchOnNothingLead.LastName,
                        ed_Personnummer = matchOnNothingLead.SocialSecurityNumber,
                        ed_HasSwedishSocialSecurityNumber = matchOnNothingLead.SwedishSocialSecurityNumberSpecified ? matchOnNothingLead.SwedishSocialSecurityNumber : new bool?(),
                        EMailAddress2 = matchOnNothingLead.Email,
                        Telephone1 = matchOnNothingLead.Telephone,
                        MobilePhone = matchOnNothingLead.Mobile,
                        Address1_Line1 = matchOnNothingLead.AddressBlock.CO,
                        Address1_Line2 = matchOnNothingLead.AddressBlock.Line1,
                        Address1_PostalCode = matchOnNothingLead.AddressBlock.PostalCode,
                        Address1_City = matchOnNothingLead.AddressBlock.City,
                        ed_Address1_Country = CountryUtility.GetEntityRefForCountryCode(localContext, matchOnNothingLead.AddressBlock.CountryISO),
                        CampaignId = campaign.ToEntityReference(),
                        ed_InformationSource = Generated.ed_informationsource.Kampanj
                    };
                    campaignLead.Id = XrmHelper.Create(localContext, campaignLead);
                    campaignLead.LeadId = campaignLead.Id;
                    matchOnNothingLead.Guid = campaignLead.Id.ToString();
                    #endregion


                    #region Test

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{matchOnNothingLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, campaignLead.Id);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchOnNothingLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchOnNothingLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);


                        Assert.AreEqual(matchOnNothingLead.FirstName, info.FirstName);
                        Assert.AreEqual(matchOnNothingLead.LastName, info.LastName);
                        Assert.AreEqual(matchOnNothingLead.Email, info.Email);
                        Assert.AreEqual(matchOnNothingLead.SocialSecurityNumber, info.SocialSecurityNumber);
                        Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumberSpecified, info.SwedishSocialSecurityNumberSpecified);
                        Assert.AreEqual(matchOnNothingLead.Telephone, info.Telephone);
                        Assert.AreEqual(matchOnNothingLead.Mobile, info.Mobile);

                        Assert.AreEqual((int)campaignLead.ed_InformationSource, info.Source);

                    }

                    ContactEntity checkContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, matchOnNothingLead.SocialSecurityNumber)
                            }
                        });
                    Assert.AreEqual(matchOnNothingLead.FirstName, checkContact.FirstName);
                    Assert.AreEqual(matchOnNothingLead.LastName, checkContact.LastName);
                    Assert.AreEqual(matchOnNothingLead.Email, checkContact.EMailAddress2);
                    Assert.AreEqual(matchOnNothingLead.Telephone, checkContact.Telephone1);
                    Assert.AreEqual(matchOnNothingLead.Mobile, checkContact.Telephone2);
                    Assert.AreEqual(matchOnNothingLead.AddressBlock.CO, checkContact.Address1_Line1);
                    Assert.AreEqual(matchOnNothingLead.AddressBlock.Line1, checkContact.Address1_Line2);
                    Assert.AreEqual(matchOnNothingLead.AddressBlock.PostalCode, checkContact.Address1_PostalCode);
                    Assert.AreEqual(matchOnNothingLead.AddressBlock.City, checkContact.Address1_City);
                    Assert.AreEqual(CountryUtility.GetEntityRefForCountryCode(localContext, matchOnNothingLead.AddressBlock.CountryISO), checkContact.ed_Address1_Country);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchOnNothingLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);

                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(campaign.Id, qualLead.CampaignId.Id);
                    #endregion

                    DeleteLead(localContext, qualLead);
                    DeleteContact(localContext, checkContact);
                    DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutQualifyLeadToCstomer_MatchOnNothing returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Regression")]
        public void PutCampaignLead_MatchOnSSN()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    // Match på personnummer: Behåll den existerande kontaktens uppgifter
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    LeadInfo ssnConflictLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    ssnConflictLead.CampaignId = campaign.CodeName;

                    WrapperController.FormatLeadInfo(ref ssnConflictLead);
                    ContactEntity ssnContact = new ContactEntity
                    {
                        FirstName = ssnConflictLead.FirstName,
                        LastName = ssnConflictLead.LastName,
                        cgi_socialsecuritynumber = ssnConflictLead.SocialSecurityNumber,
                        ed_HasSwedishSocialSecurityNumber = ssnConflictLead.SwedishSocialSecurityNumberSpecified ? ssnConflictLead.SwedishSocialSecurityNumber : new bool?(),
                        Telephone2 = ssnConflictLead.Mobile,
                        Address1_Line1 = ssnConflictLead.AddressBlock.CO,
                        Address1_Line2 = ssnConflictLead.AddressBlock.Line1 + "contact",
                        Address1_PostalCode = ssnConflictLead.AddressBlock.PostalCode,
                        Address1_City = ssnConflictLead.AddressBlock.City,
                        ed_Address1_Country = CountryUtility.GetEntityRefForCountryCode(localContext, ssnConflictLead.AddressBlock.CountryISO),
                        ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.AdmSkapaKund,
                    };
                    ssnContact.Id = XrmHelper.Create(localContext.OrganizationService, ssnContact);
                    ssnContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, ssnContact.Id, new ColumnSet(true));

                    ssnConflictLead.Mobile = null;

                    LeadEntity testLead = new LeadEntity(localContext, ssnConflictLead);
                    testLead.EMailAddress1 = null;
                    testLead.Telephone1 = null;
                    testLead.CampaignId = campaign.ToEntityReference();
                    testLead.Id = XrmHelper.Create(localContext, testLead);
                    testLead.LeadId = testLead.Id;

                    ssnConflictLead.Guid = testLead.Id.ToString();
                    #endregion


                    #region Test

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{ssnConflictLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, testLead.Id);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(ssnConflictLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref ssnConflictLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        WrapperController.FormatCustomerInfo(ref info);

                        Assert.AreEqual(ssnContact.FirstName, info.FirstName);
                        Assert.AreEqual(ssnContact.LastName, info.LastName);
                        Assert.AreEqual(ssnContact.EMailAddress2, info.Email);
                        Assert.AreEqual(ssnContact.cgi_socialsecuritynumber, info.SocialSecurityNumber);
                        Assert.AreEqual(ssnContact.ed_HasSwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(ssnConflictLead.Telephone, info.Telephone);
                        Assert.AreEqual(ssnContact.Telephone2, info.Mobile);
                    }


                    ContactEntity checkContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, ssnContact.Id)
                            }
                        });

                    Assert.AreEqual(ssnContact.FirstName, checkContact.FirstName);
                    Assert.AreEqual(ssnContact.LastName, checkContact.LastName);
                    Assert.AreEqual(ssnConflictLead.AddressBlock.CO, checkContact.Address1_Line1);
                    Assert.AreEqual(ssnContact.Address1_Line2, checkContact.Address1_Line2);
                    Assert.AreEqual(ssnConflictLead.AddressBlock.PostalCode, checkContact.Address1_PostalCode);
                    Assert.AreEqual(ssnConflictLead.AddressBlock.City, checkContact.Address1_City);
                    Assert.AreEqual(CountryUtility.GetEntityRefForCountryCode(localContext, ssnConflictLead.AddressBlock.CountryISO), checkContact.ed_Address1_Country);
                    Assert.AreEqual(ssnConflictLead.Telephone, checkContact.Telephone1);
                    Assert.AreEqual(ssnContact.Telephone2, checkContact.Telephone2);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, ssnConflictLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);

                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(campaign.Id, qualLead.CampaignId.Id);
                    #endregion

                    DeleteLead(localContext, qualLead);
                    DeactivateAndDeleteContact(localContext, _serviceProxy, checkContact);
                    DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutQualifyLeadToCustomer_MatchOnSSN returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Regression")]
        public void PutCampaignLead_MatchOnName_UnvalidatedEmail()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på Namn och Email2: Ersätt den existerande kontaktens uppgifter med leadets
                    #region Setup

                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    LeadInfo nameEmail2ConflictLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    nameEmail2ConflictLead.Mobile = null;
                    nameEmail2ConflictLead.CampaignId = campaign.CodeName;

                    ContactEntity matchNameEmail2Contact = new ContactEntity
                    {
                        FirstName = nameEmail2ConflictLead.FirstName,
                        LastName = nameEmail2ConflictLead.LastName,
                        EMailAddress2 = nameEmail2ConflictLead.Email,
                        Telephone2 = shouldShowThisNumber,
                        Address1_Line1 = shouldNotShowThis,
                        Address1_Line2 = shouldNotShowThis,
                        Address1_PostalCode = shouldNotShowThisNumber,
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };
                    matchNameEmail2Contact.Id = XrmHelper.Create(localContext.OrganizationService, matchNameEmail2Contact);
                    matchNameEmail2Contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, matchNameEmail2Contact.Id, new ColumnSet(true));

                    LeadEntity testLead = new LeadEntity(localContext, nameEmail2ConflictLead);
                    testLead.EMailAddress1 = null;
                    testLead.Telephone1 = null;
                    testLead.MobilePhone = null;
                    testLead.CampaignId = campaign.ToEntityReference();
                    testLead.Id = XrmHelper.Create(localContext, testLead);
                    testLead.LeadId = testLead.Id;

                    nameEmail2ConflictLead.Guid = testLead.Id.ToString();
                    #endregion

                    //Change to HTTP call
                    //CrmPlusControlCampaign.Stuff(localContext, matchNameEmail2Lead);
                    #region Test

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{nameEmail2ConflictLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, testLead.Id);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(nameEmail2ConflictLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref nameEmail2ConflictLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        Assert.AreEqual(nameEmail2ConflictLead.FirstName, info.FirstName);
                        Assert.AreEqual(nameEmail2ConflictLead.LastName, info.LastName);
                        Assert.AreEqual(nameEmail2ConflictLead.Email, info.Email);
                        Assert.AreEqual(nameEmail2ConflictLead.SocialSecurityNumber, info.SocialSecurityNumber);
                        Assert.AreEqual(nameEmail2ConflictLead.SwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(nameEmail2ConflictLead.SwedishSocialSecurityNumberSpecified, info.SwedishSocialSecurityNumberSpecified);
                        Assert.AreEqual((int)matchNameEmail2Contact.ed_InformationSource, info.Source);
                        Assert.AreEqual(nameEmail2ConflictLead.Telephone, info.Telephone);

                        Assert.AreEqual(matchNameEmail2Contact.Telephone2, info.Mobile);
                    }

                    ContactEntity checkContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, nameEmail2ConflictLead.FirstName),
                            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, nameEmail2ConflictLead.LastName),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, nameEmail2ConflictLead.Email),
                            }
                        });

                    Assert.AreEqual(nameEmail2ConflictLead.AddressBlock.CO, checkContact.Address1_Line1);
                    Assert.AreEqual(nameEmail2ConflictLead.AddressBlock.Line1, checkContact.Address1_Line2);
                    Assert.AreEqual(nameEmail2ConflictLead.AddressBlock.PostalCode, checkContact.Address1_PostalCode);
                    Assert.AreEqual(nameEmail2ConflictLead.AddressBlock.City, checkContact.Address1_City);
                    Assert.AreEqual(CountryUtility.GetEntityRefForCountryCode(localContext, nameEmail2ConflictLead.AddressBlock.CountryISO), checkContact.ed_Address1_Country);

                    Assert.AreEqual(nameEmail2ConflictLead.Telephone, checkContact.Telephone1);
                    Assert.AreEqual(matchNameEmail2Contact.Telephone2, checkContact.Telephone2);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, nameEmail2ConflictLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);

                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(campaign.Id, qualLead.CampaignId.Id);
                    #endregion

                    //DeleteLead(localContext, qualLead);
                    //DeleteContact(localContext, checkContact);
                    //DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutQualifyLeadToCustomer_MatchOnName_UnvalidatedEmail returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        [Test, Category("Regression")]
        public void PutCampaignLead_MatchOnName_ValidatedEmail()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på namn och mail1: Skapa validerad kund
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    LeadInfo matchNameEmailLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    matchNameEmailLead.CampaignId = campaign.CodeName;

                    WrapperController.FormatLeadInfo(ref matchNameEmailLead);

                    ContactEntity validatedContact = new ContactEntity(localContext, matchNameEmailLead);
                    validatedContact.Id = XrmHelper.Create(localContext, validatedContact);
                    validatedContact.ContactId = validatedContact.Id;

                    matchNameEmailLead.SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now.AddYears(-3));

                    LeadEntity campaignLead = new LeadEntity(localContext, matchNameEmailLead);
                    campaignLead.EMailAddress1 = null;
                    campaignLead.Telephone1 = null;
                    campaignLead.MobilePhone = null;
                    campaignLead.CampaignId = campaign.ToEntityReference();
                    campaignLead.Id = XrmHelper.Create(localContext, campaignLead);
                    campaignLead.LeadId = campaignLead.Id;
                    matchNameEmailLead.Guid = campaignLead.Id.ToString();
                    #endregion

                    #region Test

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{matchNameEmailLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, campaignLead.Id);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchNameEmailLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchNameEmailLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        //Match on name and email is validated - Keep all contact info and update if contact has empty fields
                        Assert.AreEqual(matchNameEmailLead.FirstName, info.FirstName);
                        Assert.AreEqual(matchNameEmailLead.LastName, info.LastName);
                        Assert.AreEqual(matchNameEmailLead.Email, info.Email);
                        Assert.AreEqual(matchNameEmailLead.SwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(matchNameEmailLead.SwedishSocialSecurityNumberSpecified, info.SwedishSocialSecurityNumberSpecified);
                        Assert.AreEqual(matchNameEmailLead.Source, info.Source);
                        Assert.AreEqual(matchNameEmailLead.Telephone, info.Telephone);
                        Assert.AreEqual(matchNameEmailLead.Mobile, info.Mobile);

                        Assert.AreEqual(validatedContact.cgi_socialsecuritynumber, info.SocialSecurityNumber);
                    }


                    ContactEntity checkContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, matchNameEmailLead.FirstName),
                            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, matchNameEmailLead.LastName),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, matchNameEmailLead.Email)
                            }
                        });
                    Assert.AreEqual(matchNameEmailLead.FirstName, checkContact.FirstName);
                    Assert.AreEqual(matchNameEmailLead.LastName, checkContact.LastName);
                    Assert.AreEqual(validatedContact.cgi_socialsecuritynumber, checkContact.cgi_socialsecuritynumber);
                    Assert.AreEqual(matchNameEmailLead.AddressBlock.CO, checkContact.Address1_Line1);
                    Assert.AreEqual(matchNameEmailLead.AddressBlock.Line1, checkContact.Address1_Line2);
                    Assert.AreEqual(matchNameEmailLead.AddressBlock.PostalCode, checkContact.Address1_PostalCode);
                    Assert.AreEqual(matchNameEmailLead.AddressBlock.City, checkContact.Address1_City);
                    Assert.AreEqual(CountryUtility.GetEntityRefForCountryCode(localContext, matchNameEmailLead.AddressBlock.CountryISO), checkContact.ed_Address1_Country);
                    Assert.AreEqual(matchNameEmailLead.Email, checkContact.EMailAddress1);
                    Assert.AreEqual(matchNameEmailLead.Telephone, checkContact.Telephone1);
                    Assert.AreEqual(matchNameEmailLead.Mobile, checkContact.Telephone2);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchNameEmailLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);

                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(campaign.Id, qualLead.CampaignId.Id);

                    #endregion

                    //DeleteLead(localContext, qualLead);
                    //DeleteContact(localContext, checkContact);
                    //DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutQualifyLeadToCustomer_MatchOnName_ValidatedEmail returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Regression")]
        public void PostCampaignLead_NoMatch()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på ingenting
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    LeadInfo matchOnNothingLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);
                    matchOnNothingLead.CampaignId = campaign.CodeName;
                    #endregion

                    #region Test

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchOnNothingLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    Guid contactGuid = Guid.Empty;
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchOnNothingLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        Assert.AreEqual(matchOnNothingLead.FirstName, info.FirstName);
                        Assert.AreEqual(matchOnNothingLead.LastName, info.LastName);
                        Assert.AreEqual(matchOnNothingLead.Email, info.Email);
                        Assert.AreEqual(matchOnNothingLead.SocialSecurityNumber, info.SocialSecurityNumber);
                        Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumberSpecified, info.SwedishSocialSecurityNumberSpecified);
                        Assert.AreEqual(matchOnNothingLead.Source, info.Source);
                        Assert.AreEqual(matchOnNothingLead.Telephone, info.Telephone);
                        Assert.AreEqual(matchOnNothingLead.Mobile, info.Mobile);

                        contactGuid = new Guid(info.Guid);
                    }


                    QueryExpression contactQuery = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, contactGuid)
                            }
                        }
                    };
                    ContactEntity qualContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

                    Assert.AreEqual(matchOnNothingLead.FirstName, qualContact.FirstName);
                    Assert.AreEqual(matchOnNothingLead.LastName, qualContact.LastName);
                    Assert.AreEqual(matchOnNothingLead.Email, qualContact.EMailAddress2);
                    Assert.AreEqual(matchOnNothingLead.SocialSecurityNumber, qualContact.cgi_socialsecuritynumber);
                    Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumber, qualContact.ed_HasSwedishSocialSecurityNumber);
                    Assert.AreEqual(matchOnNothingLead.Source, (int)qualContact.ed_InformationSource);
                    Assert.AreEqual(matchOnNothingLead.Telephone, qualContact.Telephone1);
                    Assert.AreEqual(matchOnNothingLead.Mobile, qualContact.Telephone2);


                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchOnNothingLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);


                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(matchOnNothingLead.FirstName, qualLead.FirstName);
                    Assert.AreEqual(matchOnNothingLead.LastName, qualLead.LastName);
                    Assert.AreEqual(matchOnNothingLead.Email, qualLead.EMailAddress1);
                    Assert.AreEqual(matchOnNothingLead.SocialSecurityNumber, qualLead.ed_Personnummer);
                    Assert.AreEqual(matchOnNothingLead.SwedishSocialSecurityNumber, qualLead.ed_HasSwedishSocialSecurityNumber);
                    Assert.AreEqual(matchOnNothingLead.Source, (int)qualLead.ed_InformationSource);
                    Assert.AreEqual(matchOnNothingLead.Telephone, qualLead.Telephone1);
                    Assert.AreEqual(matchOnNothingLead.Mobile, qualLead.MobilePhone);

                    Assert.AreEqual(campaign.CodeName, qualLead.GetAliasedValueOrDefault<string>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.CodeName));

                    #endregion

                    DeleteLead(localContext, qualLead);
                    DeleteContact(localContext, qualContact);
                    DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PostCampaignLead_NoMatch returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        [Test, Category("Regression")]
        public void PostCampaignLead_MatchOnSSN()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på SocSecNr
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CustomerInfo customerInfo = ValidCampaignCustomerInfo(testInstanceName);

                    ContactEntity existingContact = new ContactEntity(localContext, customerInfo);
                    existingContact.EMailAddress2 = existingContact.EMailAddress1 + "contact";
                    existingContact.EMailAddress1 = null;
                    existingContact.Telephone1 = null;
                    existingContact.ed_InformationSource = Generated.ed_informationsource.OinloggatKop;
                    existingContact.Id = XrmHelper.Create(localContext, existingContact);
                    existingContact.ContactId = existingContact.Id;
                    existingContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, existingContact.Id, new ColumnSet(true));

                    LeadInfo matchOnSocSecLead = WebApiTestHelper.ToLeadInfo(customerInfo);
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);
                    matchOnSocSecLead.CampaignId = campaign.CodeName;
                    #endregion

                    #region Test
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchOnSocSecLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    Guid contactGuid = Guid.Empty;
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchOnSocSecLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        Assert.AreEqual(existingContact.FirstName, info.FirstName);
                        Assert.AreEqual(existingContact.LastName, info.LastName);
                        Assert.AreEqual(existingContact.EMailAddress2, info.Email);
                        Assert.AreEqual(existingContact.cgi_socialsecuritynumber, info.SocialSecurityNumber);
                        Assert.AreEqual(existingContact.ed_HasSwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual((int)existingContact.ed_InformationSource, info.Source);
                        Assert.AreEqual(matchOnSocSecLead.Telephone, info.Telephone);
                        Assert.AreEqual(existingContact.Telephone2, info.Mobile);

                        contactGuid = new Guid(info.Guid);
                    }


                    QueryExpression contactQuery = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, contactGuid)
                            }
                        }
                    };
                    ContactEntity qualContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

                    Assert.AreEqual(existingContact.FirstName, qualContact.FirstName);
                    Assert.AreEqual(existingContact.LastName, qualContact.LastName);
                    Assert.AreEqual(existingContact.EMailAddress2, qualContact.EMailAddress2);
                    Assert.AreEqual(existingContact.cgi_socialsecuritynumber, qualContact.cgi_socialsecuritynumber);
                    Assert.AreEqual(existingContact.ed_InformationSource, qualContact.ed_InformationSource);
                    Assert.AreEqual(matchOnSocSecLead.Telephone, qualContact.Telephone1);
                    Assert.AreEqual(existingContact.Telephone2, qualContact.Telephone2);


                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                         {
                             Conditions =
                             {
                                 new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchOnSocSecLead.SocialSecurityNumber)
                             }
                         },
                        LinkEntities =
                         {
                             new LinkEntity()
                             {
                                 LinkFromEntityName = LeadEntity.EntityLogicalName,
                                 LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                 LinkToEntityName = CampaignEntity.EntityLogicalName,
                                 LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                 EntityAlias = CampaignEntity.EntityLogicalName,
                                 JoinOperator = JoinOperator.Inner,
                                 Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                             }
                         }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);


                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(matchOnSocSecLead.FirstName, qualLead.FirstName);
                    Assert.AreEqual(matchOnSocSecLead.LastName, qualLead.LastName);
                    Assert.AreEqual(matchOnSocSecLead.Email, qualLead.EMailAddress1);
                    Assert.AreEqual(matchOnSocSecLead.SocialSecurityNumber, qualLead.ed_Personnummer);
                    Assert.AreEqual(matchOnSocSecLead.SwedishSocialSecurityNumber, qualLead.ed_HasSwedishSocialSecurityNumber);
                    Assert.AreEqual(matchOnSocSecLead.Source, (int)qualLead.ed_InformationSource);
                    Assert.AreEqual(matchOnSocSecLead.Telephone, qualLead.Telephone1);
                    Assert.AreEqual(matchOnSocSecLead.Mobile, qualLead.MobilePhone);

                    Assert.AreEqual(campaign.CodeName, qualLead.GetAliasedValueOrDefault<string>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.CodeName));

                    DeleteLead(localContext, qualLead);
                    DeleteContact(localContext, qualContact);
                    DeleteCampaign(localContext, campaign);

                    #endregion
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PostCampaignLead_NoMatch returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        [Test, Category("Regression")]
        public void PostCampaignLead_LeadDuplicate_MatchOnEmailName()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    #region Setup
                    CampaignEntity campaign = CreateCampaign(localContext);
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    //Create an existing lead
                    LeadEntity lead = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
                    lead.CampaignId = campaign.ToEntityReference();
                    lead.Id = XrmHelper.Create(localContext.OrganizationService, lead);

                    //Try to create a lead with same info and attempt to do POST method
                    LeadInfo testLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    testLead.LastName = lead.LastName;
                    testLead.CampaignId = campaign.CodeName;
                    testLead.Source = (int)Generated.ed_informationsource.Kampanj;
                    testLead.Guid = Guid.NewGuid().ToString();
                    #endregion

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(testLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //API should return bad request. Throw an exception otherwise.
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    throw new Exception("Should have thrown error");
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PostCampaignLead_LeadDuplicate_MatchOnEmailName returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                        Assert.AreEqual("Koden som du har angivit är felaktig, förbrukad eller har gått ut.", result);
                    }

                }
            }
        }

        //[Test, Category("Regression")]
        //public void PutCampaignLead_CompleteWithCampaignCodeCall()
        //{
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        _serviceProxy.EnableProxyTypes();
        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
        //        try
        //        {
        //            #region Setup
        //            CampaignEntity campaign = CreateCampaign(localContext);
        //            string testInstanceName = WebApiTestHelper.GetUnitTestID();

        //            //Create an existing lead
        //            LeadEntity lead = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
        //            string email = lead.EMailAddress1, persNr = lead.ed_Personnummer, telephone = lead.Telephone1, mobile = lead.MobilePhone;
        //            lead.EMailAddress1 = null;
        //            lead.ed_Personnummer = null;
        //            lead.Telephone1 = null;
        //            lead.MobilePhone = null;
        //            lead.CampaignId = campaign.ToEntityReference();
        //            lead.ed_CampaignCode = new Random().Next(0, 1000000).ToString("D6");
        //            lead.Id = XrmHelper.Create(localContext.OrganizationService, lead);

        //            //Try to complete a lead with same info and attempt to do PUT method
        //            LeadInfo testLead = new LeadInfo
        //            {
        //                CampaignCode = lead.ed_CampaignCode,
        //                Email = email,
        //                SocialSecurityNumber = persNr,
        //                SwedishSocialSecurityNumberSpecified = true,
        //                SwedishSocialSecurityNumber = true,
        //                Telephone = telephone,
        //                Mobile = mobile,
        //                Source = (int)Generated.ed_informationsource.Kampanj
        //            };
        //            #endregion

        //            string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{testLead.CampaignCode}";
        //            var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
        //            WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
        //            httpWebRequest1.ContentType = "application/json";
        //            httpWebRequest1.Method = "PUT";

        //            using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
        //            {
        //                string InputJSON = JsonConvert.SerializeObject(testLead, Formatting.None);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }

        //            HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

        //            testLead.CombineLeadInfos(localContext, lead.ToLeadInfo(localContext));


        //            LeadEntity controlLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
        //                new FilterExpression
        //                {
        //                    Conditions =
        //                    {
        //                        new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, testLead.SocialSecurityNumber)
        //                    }
        //                });
        //            Assert.NotNull(controlLead);
        //            Assert.AreEqual(Generated.LeadState.Qualified, controlLead.StateCode);
        //            Assert.AreEqual(testLead.Email, controlLead.EMailAddress1);
        //            Assert.AreEqual(testLead.Telephone, controlLead.Telephone1);
        //            Assert.IsNull(controlLead.ed_CampaignCode);
        //            Assert.AreEqual(testLead.FirstName, controlLead.FirstName);
        //            Assert.AreEqual(testLead.LastName, controlLead.LastName);
        //            Assert.AreEqual(testLead.AddressBlock.Line1, controlLead.Address1_Line2);

        //            LeadsController.FormatLeadInfo(ref testLead);


        //            ContactEntity controlContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
        //                new FilterExpression
        //                {
        //                    Conditions =
        //                    {
        //                        new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, testLead.SocialSecurityNumber)
        //                    }
        //                });

        //            Assert.NotNull(controlContact);
        //            Assert.AreEqual(Generated.ContactState.Active, controlContact.StateCode);
        //            Assert.AreEqual(testLead.Email, controlContact.EMailAddress2);
        //            Assert.AreEqual(testLead.Telephone, controlContact.Telephone1);
        //            Assert.AreEqual(testLead.FirstName, controlContact.FirstName);
        //            Assert.AreEqual(testLead.LastName, controlContact.LastName);
        //            Assert.AreEqual(testLead.AddressBlock.Line1, controlContact.Address1_Line2);

        //        }
        //        catch (WebException we)
        //        {
        //            if (we.Message == "Unable to connect to the remote server")
        //                throw we;

        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            if (response == null)
        //            {
        //                throw we;
        //            }

        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("PutCampaignLead_LeadDuplicate_MatchOnEmailName returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
        //                throw new Exception(result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        [Test, Category("Regression")]
        public void PutCampaignLead_LeadDuplicate_MatchOnEmailName()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    #region Setup
                    CampaignEntity campaign = CreateCampaign(localContext);
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    // Create an existing, Qualified lead
                    LeadEntity existingLead = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
                    existingLead.CampaignId = campaign.ToEntityReference();
                    existingLead.ed_CampaignCode = new Random().Next(0, 1000000).ToString("D6");
                    existingLead.Id = XrmHelper.Create(localContext.OrganizationService, existingLead);

                    QualifyLeadRequest req = new QualifyLeadRequest
                    {
                        CreateAccount = false,
                        CreateContact = false,
                        CreateOpportunity = false,
                        LeadId = existingLead.ToEntityReference(),
                        SourceCampaignId = campaign.ToEntityReference(),
                        Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
                    };
                    localContext.OrganizationService.Execute(req);

                    // 
                    LeadEntity lead = new LeadEntity(localContext, ValidCampaignCustomerInfo(DateTime.Now.AddMonths(-3).ToString("yyyyMMdd.hhmm")));
                    string persNr = existingLead.ed_Personnummer, telephone = existingLead.Telephone1, mobile = existingLead.MobilePhone;
                    lead.FirstName = existingLead.FirstName;
                    lead.LastName = existingLead.LastName;
                    lead.EMailAddress1 = null;
                    lead.ed_Personnummer = null;
                    lead.Telephone1 = null;
                    lead.MobilePhone = null;
                    lead.CampaignId = campaign.ToEntityReference();
                    lead.ed_CampaignCode = new Random().Next(0, 1000000).ToString("D6");
                    lead.Id = XrmHelper.Create(localContext.OrganizationService, lead);
                    #endregion

                    //Try to complete a lead with same info and attempt to do PUT method
                    LeadInfo testLead = new LeadInfo
                    {
                        Guid = lead.Id.ToString(),
                        CampaignCode = lead.ed_CampaignCode,
                        Email = existingLead.EMailAddress1,
                        SocialSecurityNumber = persNr,
                        SwedishSocialSecurityNumberSpecified = true,
                        SwedishSocialSecurityNumber = true,
                        Telephone = telephone,
                        Mobile = mobile,
                        Source = (int)Generated.ed_informationsource.Kampanj
                    };

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{testLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, new Guid(testLead.Guid));
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(testLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    throw new Exception("Attempt to qualify lead with same Name and Email should have returned an exception");
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutCampaignLead_LeadDuplicate_MatchOnEmailName returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                        Assert.AreEqual("Du har redan tagit del av denna kampanj.", result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [Test, Category("Regression")]
        public void PostCampaignLead_LeadDuplicate_MatchOnSSN()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    #region Setup
                    CampaignEntity campaign = CreateCampaign(localContext);
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    string ssn = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

                    //Create an existing lead
                    LeadEntity lead = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
                    lead.CampaignId = campaign.ToEntityReference();
                    lead.ed_Personnummer = ssn;
                    lead.Id = XrmHelper.Create(localContext.OrganizationService, lead);

                    //Try to create a lead with same info and attempt to do PUT method
                    LeadInfo testLead = WebApiTestHelper.ToLeadInfo(ValidCampaignCustomerInfo(testInstanceName));
                    testLead.CampaignId = campaign.CodeName;
                    testLead.Source = (int)Generated.ed_informationsource.Kampanj;
                    testLead.Guid = Guid.NewGuid().ToString();
                    testLead.SocialSecurityNumber = ssn;
                    #endregion

                    #region Test
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, new Guid(testLead.Guid));
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(testLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //API should return bad request. Throw an exception otherwise.
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    throw new Exception("Code did not find a duplicate which it should've.");

                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutCampaignLead_LeadDuplicate_MatchOnSSN returned a correct exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                        Assert.AreEqual("Koden som du har angivit är felaktig, förbrukad eller har gått ut.", result);
                    }
                    #endregion
                }

            }

        }

        [Test, Category("Regression")]
        public void PutCampaignLead_LeadDuplicate_MatchOnSSN()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    #region Setup
                    CampaignEntity campaign = CreateCampaign(localContext);
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    string ssn = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

                    //Create an existing lead
                    LeadEntity lead = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
                    lead.CampaignId = campaign.ToEntityReference();
                    lead.ed_Personnummer = ssn;
                    lead.Id = XrmHelper.Create(localContext, lead);

                    //Try to create a lead with same info and attempt to do PUT method
                    LeadEntity lead2 = new LeadEntity(localContext, ValidCampaignCustomerInfo(testInstanceName));
                    lead2.CampaignId = campaign.ToEntityReference();
                    lead2.ed_Personnummer = null;
                    lead2.EMailAddress1 = null;
                    lead2.Id = XrmHelper.Create(localContext, lead2);

                    LeadInfo testLead = new LeadInfo
                    {
                        CampaignId = campaign.CodeName,
                        Source = (int)Generated.ed_informationsource.Kampanj,
                        Guid = lead2.Id.ToString(),
                        SocialSecurityNumber = ssn,
                        Email = "test@test.test2"
                    };
                    #endregion

                    #region Test
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{testLead.Guid}";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, new Guid(testLead.Guid));
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(testLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //API should return bad request. Throw an exception otherwise.
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

                    throw new Exception("Code did not find a duplicate which it should've.");

                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PutCampaignLead_LeadDuplicate_MatchOnSSN returned a correct exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                        Assert.AreEqual("Du har redan tagit del av denna kampanj.", result);
                    }
                    #endregion
                }

            }

        }

        [Test, Category("Regression")]
        public void PostCampaignLead_MatchOnName_UnvalidatedEmail()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    //Match på Ovaliderad Email
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CustomerInfo customerInfo = ValidCampaignCustomerInfo(testInstanceName);

                    ContactEntity existingContact = new ContactEntity(localContext, customerInfo);
                    existingContact.EMailAddress2 = existingContact.EMailAddress1;
                    existingContact.EMailAddress1 = null;
                    existingContact.cgi_socialsecuritynumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now.AddMonths(-15));
                    existingContact.Telephone1 = null;
                    existingContact.ed_InformationSource = Generated.ed_informationsource.Kampanj;
                    existingContact.Id = XrmHelper.Create(localContext, existingContact);
                    existingContact.ContactId = existingContact.Id;
                    existingContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, existingContact.Id, new ColumnSet(true));

                    LeadInfo matchOnUnvalidatedMailLead = WebApiTestHelper.ToLeadInfo(customerInfo);
                    matchOnUnvalidatedMailLead.Mobile = null;
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    matchOnUnvalidatedMailLead.CampaignId = campaign.CodeName;
                    #endregion

                    #region Test
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchOnUnvalidatedMailLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchOnUnvalidatedMailLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        Assert.AreEqual(matchOnUnvalidatedMailLead.FirstName, info.FirstName);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.LastName, info.LastName);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.Email, info.Email);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.SocialSecurityNumber, info.SocialSecurityNumber);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.SwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.Source, info.Source);
                        Assert.AreEqual(matchOnUnvalidatedMailLead.Telephone, info.Telephone);
                        Assert.AreEqual(existingContact.Telephone2, info.Mobile);

                        Assert.AreEqual(existingContact.Id, new Guid(info.Guid));
                    }


                    QueryExpression contactQuery = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, existingContact.Id)
                            }
                        }
                    };
                    ContactEntity qualContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

                    Assert.AreEqual(matchOnUnvalidatedMailLead.FirstName, qualContact.FirstName);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.LastName, qualContact.LastName);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Email, qualContact.EMailAddress2);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.SocialSecurityNumber, qualContact.cgi_socialsecuritynumber);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Source, (int)qualContact.ed_InformationSource);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Telephone, qualContact.Telephone1);
                    Assert.AreEqual(existingContact.Telephone2, qualContact.Telephone2);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchOnUnvalidatedMailLead.SocialSecurityNumber)
                            }
                        },
                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = LeadEntity.EntityLogicalName,
                                LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                LinkToEntityName = CampaignEntity.EntityLogicalName,
                                LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                EntityAlias = CampaignEntity.EntityLogicalName,
                                JoinOperator = JoinOperator.Inner,
                                Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                            }
                        }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);


                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.FirstName, qualLead.FirstName);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.LastName, qualLead.LastName);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Email, qualLead.EMailAddress1);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.SocialSecurityNumber, qualLead.ed_Personnummer);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.SwedishSocialSecurityNumber, qualLead.ed_HasSwedishSocialSecurityNumber);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Source, (int)qualLead.ed_InformationSource);
                    Assert.AreEqual(matchOnUnvalidatedMailLead.Telephone, qualLead.Telephone1);
                    Assert.IsNull(qualLead.MobilePhone);

                    Assert.AreEqual(campaign.CodeName, qualLead.GetAliasedValueOrDefault<string>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.CodeName));

                    #endregion


                    //Tear down
                    DeleteLead(localContext, qualLead);
                    DeleteContact(localContext, qualContact);
                    DeleteCampaign(localContext, campaign);

                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PostCampaignLead_NoMatch returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }
        
        [Test, Category("Regression")]
        public void PostCampaignLead_MatchOnName_ValidatedEmail()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                try
                {
                    //Match på Validerad Email
                    #region Setup
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CustomerInfo customerInfo = ValidCampaignCustomerInfo(testInstanceName);

                    ContactEntity existingContact = new ContactEntity(localContext, customerInfo);
                    existingContact.cgi_socialsecuritynumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now.AddMonths(-15));
                    existingContact.Telephone1 = null;
                    existingContact.ed_InformationSource = Generated.ed_informationsource.RGOL;
                    existingContact.Id = XrmHelper.Create(localContext, existingContact);
                    existingContact.ContactId = existingContact.Id;
                    existingContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, existingContact.Id, new ColumnSet(true));

                    LeadInfo matchOnValidatedMailLead = WebApiTestHelper.ToLeadInfo(customerInfo);
                    matchOnValidatedMailLead.Mobile = null;
                    CampaignEntity c = CreateCampaign(localContext);
                    CampaignEntity uc = new CampaignEntity
                    {
                        Id = c.Id,
                        ProcessId = new Guid(processId),
                        StageId = new Guid(stage2DR1)
                    };
                    XrmHelper.Update(localContext, uc);
                    CampaignEntity campaign = new CampaignEntity()
                    {
                        Id = c.Id,
                        CampaignId = c.CampaignId
                    };
                    campaign.CombineAttributes(c);
                    campaign.CombineAttributes(uc);

                    matchOnValidatedMailLead.CampaignId = campaign.CodeName;
                    #endregion

                    #region Test
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject(matchOnValidatedMailLead, Formatting.None);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatLeadInfo(ref matchOnValidatedMailLead);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        Assert.AreEqual(existingContact.FirstName, info.FirstName);
                        Assert.AreEqual(existingContact.LastName, info.LastName);
                        Assert.AreEqual(existingContact.EMailAddress1, info.Email);
                        Assert.AreEqual(existingContact.cgi_socialsecuritynumber, info.SocialSecurityNumber);
                        Assert.AreEqual(existingContact.ed_HasSwedishSocialSecurityNumber, info.SwedishSocialSecurityNumber);
                        Assert.AreEqual((int)existingContact.ed_InformationSource, info.Source);
                        Assert.AreEqual(matchOnValidatedMailLead.Telephone, info.Telephone);
                        Assert.AreEqual(existingContact.Telephone2, info.Mobile);

                        Assert.AreEqual(existingContact.Id, new Guid(info.Guid));
                    }

                    QueryExpression contactQuery = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, existingContact.Id)
                            }
                        }
                    };
                    ContactEntity qualContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

                    Assert.AreEqual(existingContact.FirstName, qualContact.FirstName);
                    Assert.AreEqual(existingContact.LastName, qualContact.LastName);
                    Assert.AreEqual(existingContact.EMailAddress1, qualContact.EMailAddress1);
                    Assert.AreEqual(existingContact.cgi_socialsecuritynumber, qualContact.cgi_socialsecuritynumber);
                    Assert.AreEqual(existingContact.ed_InformationSource, qualContact.ed_InformationSource);
                    Assert.AreEqual(matchOnValidatedMailLead.Telephone, qualContact.Telephone1);
                    Assert.AreEqual(existingContact.Telephone2, qualContact.Telephone2);

                    ColumnSet infoBlockWithState = LeadEntity.LeadInfoBlock;
                    infoBlockWithState.AddColumn(LeadEntity.Fields.StateCode);

                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = LeadEntity.EntityLogicalName,
                        ColumnSet = infoBlockWithState,
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, matchOnValidatedMailLead.SocialSecurityNumber)
                            }
                        },
                        LinkEntities =
                        {
                            new LinkEntity()
                            {
                                LinkFromEntityName = LeadEntity.EntityLogicalName,
                                LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                LinkToEntityName = CampaignEntity.EntityLogicalName,
                                LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                EntityAlias = CampaignEntity.EntityLogicalName,
                                JoinOperator = JoinOperator.Inner,
                                Columns = new ColumnSet(CampaignEntity.Fields.CodeName)
                            }
                        }
                    };
                    LeadEntity qualLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);


                    Assert.AreEqual(Generated.LeadState.Qualified, qualLead.StateCode);
                    Assert.AreEqual(matchOnValidatedMailLead.FirstName, qualLead.FirstName);
                    Assert.AreEqual(matchOnValidatedMailLead.LastName, qualLead.LastName);
                    Assert.AreEqual(matchOnValidatedMailLead.Email, qualLead.EMailAddress1);
                    Assert.AreEqual(matchOnValidatedMailLead.SocialSecurityNumber, qualLead.ed_Personnummer);
                    Assert.AreEqual(matchOnValidatedMailLead.SwedishSocialSecurityNumber, qualLead.ed_HasSwedishSocialSecurityNumber);
                    Assert.AreEqual(matchOnValidatedMailLead.Source, (int)qualLead.ed_InformationSource);
                    Assert.AreEqual(matchOnValidatedMailLead.Telephone, qualLead.Telephone1);
                    Assert.IsNull(qualLead.MobilePhone);

                    Assert.AreEqual(campaign.CodeName, qualLead.GetAliasedValueOrDefault<string>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.CodeName));

                    #endregion

                    ////Tear down
                    //DeleteLead(localContext, qualLead);
                    //DeleteContact(localContext, qualContact);
                    //DeleteCampaign(localContext, campaign);
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PostCampaignLead_NoMatch returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }

        private string GetMillisecondsTimestamp()
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return ((long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds).ToString();
        }
        

        [Test, Category("Regression")]
        public void QualifyLead_CopyAdressToContact()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "QualifyLead_FirstName");


                string ContactFirstName = "QualifyLead_FirstName" + GetMillisecondsTimestamp();

                ContactEntity contact = new ContactEntity()
                {   FirstName = ContactFirstName,
                    LastName = "QualifyLead_LastName",
                    EMailAddress1 = "qualify@endeavor.se",
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                    Address1_Line1 = "Contact_Line1",
                    Address1_Line2 = "Contact_Line2",
                    Address1_Country = "Contact_Country",
                    Address1_PostalCode = "Contact_PostalCode",
                    Address1_City = "ContactCity"
                };

                contact.Id = XrmHelper.Create(localContext, contact);
                IList<LeadEntity> allLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.ed_CampaignCode),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.NotNull)
                        }
                    });
                IEnumerable<string> codes = allLeads.Select(l => l.ed_CampaignCode);
                string code = CampaignEntity.generateUniqueCampaignCodes(codes.ToList(), 1).First();
                CampaignEntity campaign = CreateCampaign(localContext);
                ProductEntity product = GetOrCreateTestProduct(localContext);
                AssociateProductToCampaignIfNeeded(localContext, campaign, product);

                LeadEntity newLead = new LeadEntity
                {
                    ed_InformationSource = Generated.ed_informationsource.Kampanj,
                    FirstName = ContactFirstName,
                    LastName = "QualifyLead_LastName",
                    EMailAddress1 = "qualify@endeavor.se",
                    MobilePhone = MobilePhone,
                    Address1_Line1 = Address1_Line1,
                    Address1_Line2 = Address1_Line2,
                    Address1_PostalCode = Address1_PostalCode,
                    Address1_City = Address1_City,
                    Address1_Country = ed_Address1_Country,
                    ed_Address1_Country = null,
                    ed_CampaignCode = code,
                    CampaignId = campaign.ToEntityReference()
                };
                newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);
                newLead.LeadId = newLead.Id;

                

                try
                {
                    var leadInformation = newLead.ToLeadInfo(localContext);
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{leadInformation.Guid}";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, newLead.Id);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInformation);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("QualifyLead done, returned: {0}", result);
                        leadInformation = (JsonConvert.DeserializeObject<LeadInfo>(result));
                    }
                }
                catch (WebException we)
                {
                    if (we.Message == "Unable to connect to the remote server")
                        throw we;

                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        throw we;
                    }

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("QualifyLead returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                
                contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact.Id, new ColumnSet(true));
                Assert.AreEqual(Address1_Line1.ToLower(), contact.Address1_Line1.ToLower());
                Assert.AreEqual(Address1_Line2.ToLower(), contact.Address1_Line2.ToLower());
                Assert.AreEqual(Address1_PostalCode, contact.Address1_PostalCode);
                Assert.AreEqual(Address1_City, contact.Address1_City);

                Assert.IsNull(contact.ed_Address1_Country);
                

            }
        }

        [Test, Category("Debug")]
        public void DateTimeCompare()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                
                try
                {
                    IList<LeadEntity> getLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(
                      localContext,
                      new QueryExpression()
                      {
                          EntityName = LeadEntity.EntityLogicalName,
                          ColumnSet = LeadEntity.LeadInfoBlock,
                          Criteria =
                          {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.ed_CampaignCode, ConditionOperator.Equal, "9102013936")
                                }
                          },
                          LinkEntities =
                          {
                                new LinkEntity()
                                {
                                    LinkFromEntityName = LeadEntity.EntityLogicalName,
                                    LinkToEntityName = CampaignEntity.EntityLogicalName,
                                    LinkFromAttributeName = LeadEntity.Fields.CampaignId,
                                    LinkToAttributeName = CampaignEntity.Fields.CampaignId,
                                    EntityAlias = CampaignEntity.EntityLogicalName,
                                    JoinOperator = JoinOperator.Inner,
                                    Columns = new ColumnSet(
                                        CampaignEntity.Fields.StateCode,
                                        CampaignEntity.Fields.ed_ValidFromPhase1,
                                        CampaignEntity.Fields.ed_ValidToPhase1,
                                        CampaignEntity.Fields.ed_ValidFromPhase2,
                                        CampaignEntity.Fields.ed_ValidToPhase2)
                                }
                          }
                      });

                    LeadEntity lead = getLeads.First();

                    #region CampaignChecks
                    bool isCampaignActive = false;
                    DateTime validFromPhase1 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidFromPhase1).ToLocalTime();
                    DateTime validToPhase1 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidToPhase1).ToLocalTime();
                    DateTime validFromPhase2 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidFromPhase2).ToLocalTime();
                    DateTime validToPhase2 = lead.GetAliasedValueOrDefault<DateTime>(CampaignEntity.EntityLogicalName, CampaignEntity.Fields.ed_ValidToPhase2).ToLocalTime();
                    if (validFromPhase1 != null && validToPhase1 != null)
                    {

                        DateTime universalDateNow = new DateTime(2018, 11, 20, 23, 01, 02);

                        if (universalDateNow.CompareTo(validFromPhase1) > 0 && universalDateNow.CompareTo(validToPhase1.AddDays(1)) < 0)
                        {
                            isCampaignActive = true;
                        }
                        else if (validFromPhase2 != null && validToPhase2 != null)
                        {
                            if (universalDateNow.CompareTo(validFromPhase2) > 0 && universalDateNow.CompareTo(validToPhase2.AddDays(1)) < 0)
                            {
                                isCampaignActive = true;
                            }
                        }
                    }
                    else
                    {
                        //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        //rm.Content = new StringContent(string.Format(Resources.CampaignIncomplete, CampaignEntity.Fields.ed_ValidFromPhase1 + " och " + CampaignEntity.Fields.ed_ValidToPhase1));
                        //return rm;
                    }

                    if (!isCampaignActive)
                    {
                        //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //rm.Content = new StringContent(Resources.CampaignInvalid);
                        //return rm;
                    }
                    #endregion

                    #region LeadChecks
                    if (Generated.LeadState.Qualified.Equals(lead.StateCode))
                    {
                        //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //rm.Content = new StringContent(Resources.CampaignCodeUsed);
                        //return rm;
                    }
                    else if (Generated.LeadState.Disqualified.Equals(lead.StateCode))
                    {
                        //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //rm.Content = new StringContent(Resources.CampaignCodeTooOld);
                        //return rm;
                    }
                    #endregion

                    //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    LeadInfo info = (LeadInfo)lead.ToLeadInfo(localContext);

                    //response.Content = new StringContent(JsonConvert.SerializeObject(info));
                    //return response;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


#endif

        private static void DeleteLead(Plugin.LocalPluginContext localContext, LeadEntity lead)
        {
            try
            {
                XrmHelper.Delete(localContext.OrganizationService, lead.ToEntityReference());
            }
            catch (Exception) { }
        }

        private static void DeactivateAndDeleteContact(Plugin.LocalPluginContext localContext, IOrganizationService _serviceProxy, ContactEntity contact)
        {
            SetStateRequest deactivateContact = new SetStateRequest()
            {
                EntityMoniker = contact.ToEntityReference(),

                // Sets the user to disabled.
                State = new OptionSetValue(1),

                // Required by request but always valued at -1 in this context.
                Status = new OptionSetValue(-1)
            };
            _serviceProxy.Execute(deactivateContact);
            XrmHelper.Delete(localContext.OrganizationService, contact.ToEntityReference());
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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

        private static string SerializeCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customer)
        {
            return JsonConvert.SerializeObject(customer, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private CustomerInfo ValidCampaignCustomerInfo(string testInstanceName)
        {
            return ValidCampaignCustomerInfo(testInstanceName, DateTime.Now);
        }

        private CustomerInfo ValidCampaignCustomerInfo(string testInstanceName, DateTime date)
        {
            // Build a random, valid, unique personnummer
            string personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(date);

            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.Kampanj,
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
                SwedishSocialSecurityNumberSpecified = true,
            };
        }

        private CustomerInfo ValidNonLoginPurchaseCustomerInfo(string testInstanceName)
        {
            // Build a random, valid, unique personnummer
            string personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,
                FirstName = "NonLoginPurchaseFirstName",
                LastName = "NonLoginPurchaseLastName:" + testInstanceName,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "ologgvägen " + testInstanceName,
                    PostalCode = "12345",
                    City = "By" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = personnummer,
                Email = string.Format("testkop{0}@test.test", testInstanceName),
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
            };
        }

        private LeadEntity CreateNewLeadWithCampaignId(Plugin.LocalPluginContext localContext, CampaignEntity campaign, string code)
        {
            LeadEntity newLead = new LeadEntity
            {
                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.Kampanj,
                FirstName = FirstName,
                LastName = LastName,
                ed_Personnummer = ed_Personnummer,
                ed_HasSwedishSocialSecurityNumber = true,
                EMailAddress1 = EMailAddress1,
                MobilePhone = MobilePhone,
                Address1_Line1 = Address1_Line1,
                Address1_Line2 = Address1_Line2,
                Address1_PostalCode = Address1_PostalCode,
                Address1_City = Address1_City,
                Address1_Country = ed_Address1_Country,
                ed_Address1_Country = null,
                ed_CampaignCode = code,
                CampaignId = campaign.ToEntityReference()
            };
            newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);
            newLead.LeadId = newLead.Id;

            return newLead;
        }

        private LeadEntity CreateNewLead(Plugin.LocalPluginContext localContext, string personnummer)
        {
            LeadEntity newLead = new LeadEntity
            {
                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.Kampanj,
                FirstName = FirstName,
                LastName = LastName,
                ed_Personnummer = personnummer,
                ed_HasSwedishSocialSecurityNumber = true,
                EMailAddress1 = EMailAddress1,
                MobilePhone = MobilePhone,
                Address1_Line1 = Address1_Line1,
                Address1_Line2 = Address1_Line2,
                Address1_PostalCode = Address1_PostalCode,
                Address1_City = Address1_City,
                Address1_Country = ed_Address1_Country,
                ed_Address1_Country = null,
                ed_CampaignCode = CampaignCode
            };
            newLead.Id = XrmHelper.Create(localContext.OrganizationService, newLead);
            newLead.LeadId = newLead.Id;

            return newLead;
        }

        //public CampaignEntity GetOrCreateD1Campaign(Plugin.LocalPluginContext localContext)
        //{
        //    CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(true),
        //        new FilterExpression
        //        {
        //            Conditions =
        //            {
        //                new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, D1CampaignCodeName)
        //            }
        //        });

        //    if (campaign.StatusCode != Generated.campaign_statuscode.Launched ||
        //        campaign.StageId)
        //}

        private CampaignEntity CreateCampaign(Plugin.LocalPluginContext localContext)
        {
            DateTime lateDate = DateTime.Now;
            lateDate.AddHours((23 - lateDate.Hour));

            string nowString = lateDate.ToString();
            CampaignEntity campaign = new CampaignEntity
            {
                Name = CampaignName + nowString,
                CodeName = nowString.Substring(0, nowString.Length > 32 ? 31 : nowString.Length - 1),
                ed_DiscountPercent = discountPercent,
                ed_LeadTopic = Generated.ed_campaign_ed_leadtopic.NyinflyttadKampanj,
                ed_LeadSource = new OptionSetValue((int)Generated.lead_leadsourcecode.MittKonto),
                ed_ValidFromPhase1 = lateDate.AddDays(-2),
                ed_ValidToPhase1 = lateDate.AddDays(2),
                ProcessId = new Guid(processId),
                StageId = new Guid(stage1Create)
            };
            campaign.Id = XrmHelper.Create(localContext.OrganizationService, campaign);
            campaign.CampaignId = campaign.Id;

            return campaign;
        }

        private ContactEntity CreateNewUnvalidatedContact(Plugin.LocalPluginContext localContext, string personnummer)
        {
            ContactEntity newContact = new ContactEntity
            {
                Id = Guid.NewGuid(),
                FirstName = FirstName,
                LastName = LastName,
                EMailAddress2 = EMailAddress1,
                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.Kampanj,
                ed_HasSwedishSocialSecurityNumber = true,
                cgi_socialsecuritynumber = personnummer

            };
            newContact.Id = XrmHelper.Create(localContext.OrganizationService, newContact);
            newContact.ContactId = newContact.Id;

            return newContact;
        }

        private void DeleteContact(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {
            SetStateRequest setContactToInactive = new SetStateRequest
            {
                EntityMoniker = contact.ToEntityReference(),
                State = new OptionSetValue((int)Skanetrafiken.Crm.Schema.Generated.ContactState.Inactive),
                Status = new OptionSetValue((int)Skanetrafiken.Crm.Schema.Generated.contact_statuscode.Inactive)
            };
            SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(setContactToInactive);

            XrmHelper.Delete(localContext.OrganizationService, contact.ToEntityReference());
        }

        private void DeleteCampaign(Plugin.LocalPluginContext localContext, CampaignEntity campaign)
        {
            XrmHelper.Delete(localContext.OrganizationService, campaign.ToEntityReference());
        }
    }
}
