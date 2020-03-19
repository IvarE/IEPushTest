using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Skanetrafiken.Crm;
using System;
using System.Net;
using System.IO;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using Endeavor.Crm.Extensions;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Controllers;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Text;
using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.ValueCodes;
using System.Globalization;
using System.Linq;
using System.Web;
using static Skanetrafiken.Crm.ValueCodes.ValueCodeHandler;
using System.Runtime.Serialization.Json;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Net.Http;

namespace Endeavor.Crm.UnitTest
{

    [TestFixture]
    public class ValueCodeFixture : PluginFixtureBase
    {


        #region Configs
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
        #endregion

        #region General - Test values

        private const string vcFirstName = "Van Carl";
        private const string vcLastName = "Nguyen";
        private const string vcEmail = "1604298@mailinator.com";
        private const string vcMobile = "0700158181";
        private const string vcContactId = "564f2331-94f5-4c46-8501-4f4b236677c8";

        private const string cFirstName = "Test";
        private const string cLastName = "Test";

        private const string cEmail = "o1046308@nwytg.com";
        private const string cMobile = "0735198846";

        private const string cTravelCardNumber = "72369659165";
        private const string cTravelCardName = "UnitTestCard";
        private const string cTravelCardCVC = "516";


        #endregion

        [SetUp]
        public void SetUp()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        }

        [TearDown]
        public void TearDown()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">Api endpoint</param>
        /// <param name="body"></param>
        /// <param name="destination">Specifies the type of which the body shall be converted to.</param>
        /// <param name="assert">Pass assertion code here</param>
        /// <param name="xrmAction"></param>
        public void CallApi(string endpoint, object body, Type destination, Action<Plugin.LocalPluginContext, string, HttpWebResponse> assert = null, Action<Plugin.LocalPluginContext> clearDataAction = null)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //Set up http request
                string url = endpoint; //$"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";


                try
                {
                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(Convert.ChangeType(body,destination));
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd().Replace("\"", "");
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);
                        assert(localContext, response, httpResponse);
                    }
                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        {
                            string text = new StreamReader(data).ReadToEnd().Replace("\"", "");
                            assert(localContext, text, httpResponse);
                        }
                    }
                }


            }
        }


        [Test]
        public void TestResendValueCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                Guid SMSValueCodeId = new Guid("E2221E48-BAC4-E911-80F4-005056B665EC");

                ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId), new ColumnSet(true));
                //ValueCodeEntity valueCode = (ValueCodeEntity)localContext.OrganizationService.Retrieve(ValueCodeEntity.EntityLogicalName, SMSValueCodeId, new ColumnSet(true));
                valueCode.SendValueCode(localContext);

                //SendValueCode.ExecuteCodeActivity(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, SMSValueCodeId));
            }
        }


        //[Test, Category("Debug")]
        //public void RetrieveMessageFromPushQueue()
        //{
        //    #region Test Setup
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
        //        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //        stopwatch.Start();


        //        #endregion

        //        ValueCodeTemplateEntity template = XrmRetrieveHelper.RetrieveFirst<ValueCodeTemplateEntity>(localContext, new ColumnSet(true));
        //        Guid valueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 100, template: template, phoneNumber: "+46735198846");

        //        ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //        Assert.AreEqual(Generated.ed_ValueCodeState.Active, valueCode.statecode);

        //        string dateString = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK");

        //        //string sampleMessage = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>"
        //        //    + "<Coupon xmlns = 'http://v1_0.model.loopback.kuponginlosen.se'>"
        //        //    + "<Issuer>KUPONGINLOSEN</Issuer>"
        //        //    + "<StoreId>12345</StoreId>"
        //        //    + "<CampaignNumber>52845</CampaignNumber>"
        //        //    + "<EanCode>9952845101002</EanCode>"
        //        //    + "<UniqueCode>"+ valueCode.ed_name +"</UniqueCode>"
        //        //    + "<RedeemedDT>" + dateString + "</RedeemedDT>"
        //        //    + "</Coupon>";

        //        string sampleMessage = @"{
        //             'event': 'coupon_redeemed',
        //             'coupon':  {
        //                        'coupons_companies_id': 22,
        //              'coupons_admins_id': 1,
        //              'coupons_clients_id': 2,
        //              'coupons_templates_id': 161,
        //              'coupons_ticket_reference': null,
        //              'coupons_amount': '50.00',
        //              'coupons_reason': '',
        //              'coupons_type': 'mobile',
        //              'coupons_delivery_type': 'api',
        //              'coupons_status': '0',
        //              'coupons_sent': '1',
        //              'coupons_custom_image': null,
        //              'coupons_custom_text': null,
        //              'coupons_ean': '0000000000000',
        //              'coupons_last_redemption_date': {
        //                            'date': '2018-12-31 12:51:32.256479',
        //               'timezone_type': 3,
        //               'timezone': 'UTC'

        //                    },
        //              'coupons_provider_unique_code': '624165',
        //              'coupons_created_timestamp': '2018-11-12 12:51:32',
        //              'coupons_id': 1,
        //              'coupons_image': 'https:\/\/uploads.kupongsupport.se\/coupons\/V7WxEm9xJg_mobile.jpg',
        //              'coupons_redeemed': 1,
        //               'coupons_redeemed_store_id': '12345',
        //                'coupons_redeemed_timestamp': '2018-10-03 12:13:00'

        //                }
        //                    }";


        //        // OrderResult
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}ValueCode");
        //        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string InputJSON = SerializeXmlMessage(localContext, sampleMessage);

        //            streamWriter.Write(InputJSON);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //        }

        //        valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));
        //        Assert.AreEqual(Generated.ed_ValueCodeState.Inactive, valueCode.statecode);
        //        Assert.Less((DateTime.Parse(dateString).ToUniversalTime() - valueCode.ed_RedemptionDate.Value.ToUniversalTime()).Seconds, 1);
        //    }
        //}

        [Test, Category("Debug")]
        public void GetValueCodesFromContact()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                #endregion

                string url = $"{WebApiTestHelper.WebApiRootEndpoint}valuecode/25";
                var httpWebRequestLink = (HttpWebRequest)WebRequest.Create(url);
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestLink);
                httpWebRequestLink.ContentType = "application/json";
                httpWebRequestLink.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequestLink.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                }

            }
        }

        [Test]
        public void Test_TravelCardEntity_GetCardAndContactFromCardNumber()
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //var contact = TestDataHelper.CreateContact(localContext, cLastName, cFirstName, null, cMobile);

                    //var travelCard = TestDataHelper.CreateTravelCard(localContext, cTravelCardNumber, cTravelCardCVC, contact);

                    //var travel = TravelCardEntity.GetCardAndContactFromCardNumber(localContext, cTravelCardNumber, new ColumnSet());

                    //11252568351125256835
                    //0067937354
                    //0017272442 active & contact
                    //1125419171 inactive & contact
                    //73772719315 Mitt kort (koretet spärrad)
                    //1135700019 Mitt senare kort

                    var biffTravel = TravelCardEntity.GetAndParseCardDetails(localContext, "1135709999");

                    //var travelCard = TravelCardEntity.GetCardAndContactFromCardNumber(localContext, "4684713218", new ColumnSet(), "534");

                    var nonHotlisted = new List<TravelCardEntity>();

                    var query = new QueryExpression()
                    {
                        EntityName = TravelCardEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.CreatedOn, ConditionOperator.Above, new DateTime(2018,06,01)),
                                new ConditionExpression(TravelCardEntity.Fields.CreatedOn, ConditionOperator.Under, new DateTime(2018,10,01))
                            }
                        }
                    };

                    var cards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, query);

                    //foreach (var card in cards)
                    //{
                    //    try
                    //    {
                    //        var biffTravel = TravelCardEntity.GetAndParseCardDetails(localContext, card.cgi_travelcardnumber);
                    //        TravelCardEntity.ValidateValueCodeApproval(localContext, biffTravel);
                    //    }
                    //    catch (Exception)
                    //    {

                    //    }

                    //}


                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        #region Unit tests for ValueCodeController.CreateValueCode

        public ValueCodeCreationGiftCard CreateValueCodeTestData_ValidTravelCardAndContact(ContactEntity contact, TravelCardEntity travelCard, int amount, int typeOfValueCode)
        {
            var valCode = new ValueCodeCreationGiftCard()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Amount = amount,
                Email = contact.EMailAddress1,
                Mobile = contact.MobilePhone,
                deliveryMethod = 2,
                TravelCard = new TravelCard() { TravelCardNumber = travelCard.cgi_travelcardnumber, CVC = travelCard.cgi_TravelCardCVC },
                ContactId = contact.Id,
                TypeOfValueCode = typeOfValueCode
            };

            return valCode;
        }

        public ValueCodeCreationGiftCard CreateTestData_ValueCodeCreation_200()
        {
            var valCode = new ValueCodeCreationGiftCard()
            {
                FirstName = cFirstName,
                LastName = cLastName,
                Amount = 20,
                Email = cEmail,
                Mobile = cMobile,
                deliveryMethod = 2,
                TravelCard = new TravelCard() { TravelCardNumber = "3674026699", CVC = "111" },
                ContactId = new Guid("5329DFB4-6537-E811-80EF-005056B61FFF")
            };

            return valCode;
        }

        public ValueCodeCreationGiftCard CreateTestData_ValueCodeCreation_900()
        {
            var valCode = new ValueCodeCreationGiftCard()
            {
                FirstName = cFirstName,
                LastName = cLastName,
                Amount = 900,
                Email = cEmail,
                Mobile = cMobile,
                deliveryMethod = 1,
                TravelCard = new TravelCard() { TravelCardNumber = "1066846141 ", CVC = "3BB0" },
                TypeOfValueCode = 2
            };

            return valCode;
        }

        [Test]
        public void CreateValueCode_AboveMaxAmount()
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    DeleteAllTestData(localContext);

                    //Set up http request
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCode";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(CreateTestData_ValueCodeCreation_900());
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ValueCodeController vcc = new ValueCodeController();
                    vcc.HandleCreateValueCode(localContext, CreateTestData_ValueCodeCreation_900(), 1);

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                    }

                }
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw new Exception(resp, ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"*Error: {ex.Message}", ex);
            }
        }

        [Test, Category("Regression")]
        public void Test_TravelCardWithNoBalance()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                // Setup the test
                BiztalkParseCardDetailsMessage cardDetailsMessage = new BiztalkParseCardDetailsMessage() {
                    CardNumberField = "1315085267",
                    CardHotlistedField = false,

                    BalanceField = 0,
                    PeriodStartField = DateTime.Now.AddMonths(-2),
                    PeriodEndField = DateTime.Now.AddMonths(-1),
                    PricePaidField = 550,
                };

                HttpResponseMessage res = new HttpResponseMessage();
                ValueCodeController vcc = new ValueCodeController();
                res = vcc.ValidateCardDetailsFromBiztalkForPresentkort(localContext, 0, cardDetailsMessage);

                // Should return BadRequest, no balance on card...
                Assert.AreNotEqual(res.StatusCode, HttpStatusCode.OK);

            }



        }


        [Test]
        public void CreateValueCode_BelowMaxAmount()
        {
            try
            {


                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //ApiHelper.CreateTokenForVoucherService(localContext, null);

                    //string travelCardNumber = "3690714203";
                    //ColumnSet columns = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber);
                    //QueryExpression travelCardQuery = new QueryExpression()
                    //{
                    //    EntityName = TravelCardEntity.EntityLogicalName,
                    //    ColumnSet = columns,
                    //    Criteria =
                    //    {
                    //        Conditions = {
                    //            new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, travelCardNumber)
                    //        }
                    //    },

                    //    LinkEntities =
                    //    {
                    //        new LinkEntity()
                    //        {
                    //            JoinOperator = JoinOperator.LeftOuter,
                    //            LinkFromAttributeName = TravelCardEntity.Fields.cgi_Contactid,
                    //            LinkFromEntityName = TravelCardEntity.EntityLogicalName,
                    //            LinkToAttributeName = ContactEntity.Fields.ContactId,
                    //            LinkToEntityName = ContactEntity.EntityLogicalName,
                    //            Columns = new ColumnSet(ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName)
                    //        }
                    //    }
                    //};
                    //TravelCardEntity travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardQuery);

                    //string tcFirstName = travelCard.GetAttributeValue<string>(ContactEntity.Fields.FirstName);
                    //string tcLastName = travelCard.GetAttributeValue<string>(ContactEntity.Fields.LastName);
                    //string tc1FirstName = travelCard.GetAliasedValue<string>(ContactEntity.Fields.FirstName);
                    //string tc1LastName = travelCard.GetAliasedValue<string>(ContactEntity.Fields.LastName);


                    //DeleteAllTestData(localContext);

                    //Set up http request
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCode";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(CreateTestData_ValueCodeCreation_200());
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                    }

                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public class VoucherServicePost
        {
            public int amount { get; set; }
            public string contactAddress { get; set; }
            public int contactType { get; set; }
            public string tag { get; set; }
            public int travellerId { get; set; }
            public DateTime validFromDate { get; set; }
            public DateTime validToDate { get; set; }
            public int voucherType { get; set; }
        }

        [Test, Category("Debug")]
        public void TestCreateValueCode()
        {
            try
            {

                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //string apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeAPI);
                    string apiUrl = string.Empty;
                   
                    var tovcEnum = (Generated.ed_valuecodetype)Enum.Parse(typeof(Generated.ed_valuecodetype), "3");

                    //Fetch different api depending on TypeOfValueCode
                    if (tovcEnum == Generated.ed_valuecodetype.Utansaldo)
                        apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeCoupons);
                    else
                        apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeVoucher);

                    ValueCodeLossCreation forlustGaranti = new ValueCodeLossCreation()
                    {
                        ContactId = new Guid("3ea54f88-dec2-e711-80ef-005056b62b18"),
                        deliveryMethod = 1,
                        Email = "marcus.stenswed@endeavor.se",
                        FirstName = "Marcus",
                        LastName = "Stenswed",
                        Mobile = "0735198846",
                        TypeOfValueCode = 3,
                        TravelCard = new TravelCard()
                        {
                            CVC = "1BEE",
                            TravelCardNumber = "1078776029"
                        }
                    };

                    //Set up http request
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
                    //string url = $"http://localhost:37909/api/ValueCode/CreateValueCodeLossCompensation";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                        var InputJSON = WebApiTestHelper.GenericSerializer(forlustGaranti);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        //Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                    }

                    ContactEntity contactMarcus = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext,
                        new Guid("3ea54f88-dec2-e711-80ef-005056b62b18"),
                        new ColumnSet(
                        ContactEntity.Fields.MobilePhone, ContactEntity.Fields.EMailAddress1));

                    TravelCardEntity travelCard = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext, 
                        new Guid("fb0047d7-86e6-e911-80f5-005056b665ec"), 
                        new ColumnSet());

                    ValueCodeHandler.CreateMobileValueCodeGeneric(localContext, 2, DateTime.Now.AddDays(20), 20, 2055, "0735198846",
                        3, contactMarcus, null, null, null, null, travelCard);

                    //CallCreateValueCodeAction(localContext, 3, 20, 130, "0735198846", "marcus.stenswed@endeavor.se", null, null,
                    //    new EntityReference(ContactEntity.EntityLogicalName, new Guid("2DBAD8C2-17CE-E811-80F1-005056B665EC")), null, 2, 
                    //    new EntityReference(TravelCardEntity.EntityLogicalName, new Guid("FB0047D7-86E6-E911-80F5-005056B665EC")));

                    //CallCreateValueCodeAction(localContext, 3, 20, 2055, "0735198846", "marcus.stenswed@endeavor.se", null, null,
                    //    new EntityReference(ContactEntity.EntityLogicalName, new Guid("2DBAD8C2-17CE-E811-80F1-005056B665EC")), null, 2,
                    //    new EntityReference(TravelCardEntity.EntityLogicalName, new Guid("fb0047d7-86e6-e911-80f5-005056b665ec")));


                    //CallSendValueCodeAction(localContext, new EntityReference(ValueCodeEntity.EntityLogicalName, new Guid("306C1994-F7E8-E911-80F5-005056B665EC")));

                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response == null)
                    throw we;

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    throw new Exception(result, we);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Test, Category("Debug")]
        public void TestConnectionVoucherService()
        {
            try
            {
                //string resp = "{\"amount\":20.0,\"created\":\"2019-04-02T08:13:19.6546044Z\",\"status\":1,\"validFromDate\":\"2019-04-02T08:13:19.6546044Z\",\"validToDate\":\"2022-04-02T00:00:00Z\",\"voucherCode\":\"560357593475\",\"voucherId\":\"a4d70bbc-1990-4e3c-afe1-f88871ae3e86\",\"voucherType\":2}";
                ////ValueCodeVoucherServiceResponseMessage info1 = Newtonsoft.Json.JsonConvert.DeserializeObject<ValueCodeVoucherServiceResponseMessage>(resp);

                //try
                //{
                //    ValueCodeVoucherServiceResponseMessage deserializedVoucher = new ValueCodeVoucherServiceResponseMessage();
                //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(resp));

                //    DataContractJsonSerializerSettings settings = GetSettings();
                //    DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedVoucher.GetType(), settings);
                //    deserializedVoucher = ser.ReadObject(ms) as ValueCodeVoucherServiceResponseMessage;
                //    ms.Close();


                //}
                //catch(Exception ex)
                //{

                //}



                //VoucherServicePost voucher = new VoucherServicePost
                //{
                //    amount = 10,
                //    contactAddress = "0735198846",
                //    contactType = 1,
                //    tag = "Test Endeavor",
                //    travellerId = 0,
                //    validFromDate = DateTime.Now,
                //    validToDate = DateTime.Now.AddYears(1),
                //    voucherType = 1
                //};

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //string apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeAPI);
                    string apiUrl = string.Empty;
                    //string apiToken = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeToken);
                    string InputJSON = string.Empty;

                    //Convert string value to enum
                    var tovcEnum = (Generated.ed_valuecodetype)Enum.Parse(typeof(Generated.ed_valuecodetype), "2");

                    //Fetch different api depending on TypeOfValueCode
                    if (tovcEnum == Generated.ed_valuecodetype.Utansaldo)
                        apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeCoupons);
                    else
                        apiUrl = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CreateValueCodeVoucher);

                    Uri url = new Uri(apiUrl);
                    HttpWebRequest httpWebRequest = CreateRequest(url);
                    httpWebRequest.Method = "POST";
                    ApiHelper.CreateTokenForVoucherService(localContext, httpWebRequest);

                    // TODO - Which template
                    //template = GetValueCodeTemplate(localContext, template, templateNumber);
                    ValueCodeTemplateEntity template = new ValueCodeTemplateEntity();
                    template.ed_TemplateId = 1;

                    // Call to Voucher Service
                    InputJSON = CreateInputJSONVoucherServiceGeneric(localContext, tovcEnum, 365, (int)template.ed_TemplateId, "mobile", (float)20, "api", "0735198846", "marcus.stenswed@endeavor.se",
                        "", "", "", "", "", "", "", "", "", "", "");


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //var response = GetAPIResponse<ValueCodeVoucherServiceResponseMessage>(httpWebRequest);

                    //Guid valueCodeGuid = CreateValueCodeFromVoucherServiceResponseGeneric(localContext, response, template, contact, lead, type, email, phoneNumber, refund, valueCodeApproval);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();

                        ValueCodeVoucherServiceResponseMessage deserializedVoucher = new ValueCodeVoucherServiceResponseMessage();
                        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedVoucher.GetType());
                        deserializedVoucher = ser.ReadObject(ms) as ValueCodeVoucherServiceResponseMessage;
                        ms.Close();

                        // Validate result, must have DataValid
                        //ValueCodeVoucherServiceResponseMessage responseService = Newtonsoft.Json.JsonConvert.DeserializeObject<ValueCodeVoucherServiceResponseMessage>(response);

                        //Guid valueCodeGuid = CreateValueCodeFromVoucherServiceResponseGeneric(localContext, responseService, template, contact, lead, type, email, phoneNumber, refund, valueCodeApproval);

                        //localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        //Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static Guid CreateValueCodeFromVoucherServiceResponseGeneric(Plugin.LocalPluginContext localContext, ValueCodeVoucherServiceResponseMessage response, ValueCodeTemplateEntity template, ContactEntity contact, LeadEntity lead, Generated.ed_valuecode_ed_typeoption type, string email, string phoneNumber, RefundEntity refund, ValueCodeApprovalEntity valueCodeApproval)
        {

            localContext.TracingService.Trace($"(CreateValueCodeFromResponseGeneric) started.");

            //// No Campaign connected to Value Code from Voucher Service
            //QueryExpression queryCampaign = new QueryExpression
            //{
            //    EntityName = CampaignEntity.EntityLogicalName,
            //    ColumnSet = new ColumnSet(true),
            //    Criteria = new FilterExpression(LogicalOperator.And)
            //    {
            //        Conditions =
            //            {
            //                new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, response.couponDetails.template.coupon_templates_campaign_number)
            //            }
            //    }
            //};

            //CampaignEntity campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, queryCampaign);
            //localContext.TracingService.Trace($"Fetched campaign: {campaign?.Id}");


            DateTime lastDate = Convert.ToDateTime(response.validToDate);

            string rgolId = "";
            string caseNumber = "";
            if (refund?.cgi_Caseid != null)
            {
                IncidentEntity incident = XrmRetrieveHelper.Retrieve<IncidentEntity>(localContext, refund.cgi_Caseid, new ColumnSet(
                    IncidentEntity.Fields.TicketNumber, IncidentEntity.Fields.cgi_RGOLIssueId));

                rgolId = incident.cgi_RGOLIssueId;
                caseNumber = incident.TicketNumber;
            }

            localContext.TracingService.Trace($"Setting up ValueCodeEntity.");

            ValueCodeEntity valueCode = new ValueCodeEntity
            {
                //ed_Link = response.couponLink,
                //ed_AdminsId = response.couponDetails.coupons_admins_id,
                ed_Amount = new Money(response.amount),
                //ed_Campaign = (campaign != null) ? campaign.ToEntityReference() : null,
                //ed_ClientsId = response.couponDetails.coupons_clients_id,
                ed_CodeId = response.voucherCode,
                //ed_CompaniesId = response.couponDetails.coupons_companies_id,
                //ed_CreatedTimestamp = (response.couponDetails.coupons_created_timestamp != "") ? Convert.ToDateTime(response.couponDetails.coupons_created_timestamp) : nullDate,
                ed_CustomImage = "",
                ed_CustomText = "",
                //ed_DeliveryType = response.couponDetails.coupons_delivery_type,
                //ed_Ean = response.couponDetails.coupons_ean,
                //ed_Image = response.couponDetails.coupons_image,
                ed_LastRedemptionDate = Convert.ToDateTime(response.validToDate),
                //ed_Reason = response.couponDetails.coupons_reason,
                //ed_Sent = response.couponDetails.coupons_sent,
                //ed_Status = response.status.ToString(),
                //ed_TemplatesId = response.couponDetails.coupons_templates_id,
                ed_TicketReference = "",
                //ed_Type = response.couponDetails.coupons_type,
                ed_TypeOption = type,
                ed_name = response.voucherId.ToString(),
                ed_ValueCodeTemplate = template.ToEntityReference(),
                ed_Contact = (contact != null) ? contact.ToEntityReference() : null,
                ed_Lead = (lead != null) ? lead.ToEntityReference() : null,
                ed_Case = (refund?.cgi_Caseid != null) ? refund.cgi_Caseid : null,
                ed_Refund = (refund != null) ? refund.ToEntityReference() : null,
                ed_RgolId = (rgolId != null) ? rgolId : null,
                ed_CaseNumber = (caseNumber != null) ? caseNumber : null,
                ed_MobileNumber = phoneNumber,
                ed_Email = email,
                ed_ValueCodeApprovalId = valueCodeApproval?.ToEntityReference()

            };



            Guid valueCodeId = XrmHelper.Create(localContext.OrganizationService, valueCode);
            localContext.TracingService.Trace($"Created ValueCodeEntity.");

            return valueCodeId;

        }

        private static HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json";
            return httpWebRequest;
        }

        private static string CreateInputJSONVoucherServiceGeneric(Plugin.LocalPluginContext localContext, Generated.ed_valuecodetype valueCodeType, int validDays, int clearOnTemplate, string type, float amount, string deliverType, string clientsPhoneNumber, string clientsEmailAddress, string apiToken, string label1, string value1, string label2, string value2, string label3, string value3, string label4, string value4, string label5, string value5)
        {
            string customText = "";

            if (label1 != "" && value1 != "")
                customText += $"{label1}: {value1}\n";
            if (label2 != "" && value2 != "")
                customText += $"{label2}: {value2}\n";
            if (label3 != "" && value3 != "")
                customText += $"{label3}: {value3}\n";
            if (label4 != "" && value4 != "")
                customText += $"{label4}: {value4}\n";
            if (label5 != "" && value5 != "")
                customText += $"{label5}: {value5}";

            DataContractJsonSerializer js = null;
            MemoryStream msObj = new MemoryStream();

            localContext.TracingService.Trace($"Value Code Type: {valueCodeType}");

            if (valueCodeType == Generated.ed_valuecodetype.Utansaldo)
            {
                localContext.TracingService.Trace($"Setting up ValueCodeCouponRequest.------\nclearon:{clearOnTemplate}\tapi_token:{apiToken}\ttype:{type}\namount:{amount}\tdelivery_type:{deliverType}\t" +
                    $"email:{clientsEmailAddress}:\tmobile:{clientsPhoneNumber}\nvaliddays:{validDays}\tcustom_Text:{customText}\n-------");

                ValueCodeCouponVoucherServiceRequest valueCode = new ValueCodeCouponVoucherServiceRequest();

                // Amount
                valueCode.amount = Convert.ToInt32(amount);

                if (clientsEmailAddress != null && clientsEmailAddress != "")
                {
                    valueCode.contactAddress = clientsEmailAddress;
                    // contactType 2 = Email
                    valueCode.contactType = 2;
                }
                else if (clientsPhoneNumber != null && clientsPhoneNumber != "")
                {
                    valueCode.contactAddress = clientsPhoneNumber;
                    // contactType 1 = SMS
                    valueCode.contactType = 1;
                }

                // TODO - Set MKL-id from Contact (not necessary)
                //valueCode.travellerId = "";

                // Valid From
                valueCode.validFromDate = DateTime.Now;

                // Valid To
                valueCode.validToDate = DateTime.Now.AddDays(validDays);

                // 1 = förseningsersätting, 2 = presentkort (med saldo), 3 = förlustgaranti
                valueCode.voucherType = 1;


                js = new DataContractJsonSerializer(typeof(ValueCodeCouponRequest));
                js.WriteObject(msObj, valueCode);
            }

            else
            {
                localContext.TracingService.Trace($"Setting up ValueCodeVoucherRequest.");

                ValueCodeCouponVoucherServiceRequest valueCode = new ValueCodeCouponVoucherServiceRequest();

                // Amount
                valueCode.amount = Convert.ToInt32(amount);

                if (clientsEmailAddress != null && clientsEmailAddress != "")
                {
                    valueCode.contactAddress = clientsEmailAddress;
                    // contactType 2 = Email
                    valueCode.contactType = 2;
                }
                else if (clientsPhoneNumber != null && clientsPhoneNumber != "")
                {
                    valueCode.contactAddress = clientsPhoneNumber;
                    // contactType 1 = SMS
                    valueCode.contactType = 1;
                }

                // TODO - Set MKL-id from Contact (not necessary)
                //valueCode.travellerId = "";

                // Valid From
                valueCode.validFromDate = DateTime.Now;

                // Valid To
                valueCode.validToDate = DateTime.Now.AddDays(validDays);

                // 1 = förseningsersätting, 2 = presentkort (med saldo), 3 = förlustgaranti
                valueCode.voucherType = 2;

                js = new DataContractJsonSerializer(typeof(ValueCodeVoucherRequest));
                js.WriteObject(msObj, valueCode);
            }


            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();

            return json;

        }

        private static T GetAPIResponse<T>(HttpWebRequest httpWebRequest) where T : class, new()
        {
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw;
            }

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string responseJson = streamReader.ReadToEnd();

                T responseObj = new T();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseJson)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(responseObj.GetType());
                    responseObj = ser.ReadObject(ms) as T;
                    ms.Close();
                }
                return responseObj;
            }
        }

        [Test, Category("Debug")]
        public void FullFlowValueCode()
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    ContactEntity contact = null;

                    try
                    {
                        int amountToUse = 50;

                        #region Create Contact to use (contact)
                        contact = new ContactEntity
                        {
                            cgi_socialsecuritynumber = "19910201",
                            FirstName = "Marcus",
                            LastName = "Stenswed" + DateTime.Now.Hour + DateTime.Now.Minute,
                            EMailAddress1 = "marcus.stenswed@endeavor.se",
                            MobilePhone = "0735198846",
                            Telephone2 = "0735198846",
                            Telephone1 = "0735198846",
                            ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                        };

                        Guid contactId = XrmHelper.Create(localContext, contact);
                        contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(true));

                        //contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, Guid.Parse("e83ded6e-d623-e911-80f0-005056b61fff"), new ColumnSet(true));
                        #endregion

                        #region Retrieve TravelCards with Amount (cards)
                        var query = new QueryExpression()
                        {
                            EntityName = TravelCardEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber),
                            Criteria =
                            {
                                Conditions =
                                {
                                    //new ConditionExpression(TravelCardEntity.Fields.statecode, ConditionOperator.Equal, 0),
                                    //new ConditionExpression(TravelCardEntity.Fields.cgi_LatestAutoloadAmount, ConditionOperator.GreaterThan, (decimal)0),
                                    //new ConditionExpression(TravelCardEntity.Fields.cgi_value_card_type, ConditionOperator.Like, "%reskassa%"),
                                    new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, "3322916282") //https://sekundtst.skanetrafiken.se/DKCRM/main.aspx?etc=10034&extraqs=%3f_gridType%3d10034%26etc%3d10034%26id%3d%257b10943161-CCED-E411-80D8-005056903A38%257d%26rskey%3d%257b5A20DBD2-4670-4A2B-8A9C-A781C16B859B%257d&histKey=751115726&newWindow=true&pagetype=entityrecord&rskey=%7b5A20DBD2-4670-4A2B-8A9C-A781C16B859B%7d#61200275
                                }
                            }
                        };

                        List<TravelCardEntity> cards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, query);
                        #endregion

                        #region Find a Travel Card to use (validTravelCard)
                        TravelCardEntity validTravelCard = null;
                        foreach (var travelCard in cards)
                        {
                            //string getTravelCardResponse = RetrieveCardInformation(localContext, travelCard.cgi_travelcardnumber);
                            var card = TravelCardEntity.GetAndParseCardDetails(localContext, travelCard.cgi_travelcardnumber);

                            try
                            {
                                TravelCardEntity.ValidateTravelCard(localContext, card);

                                if (travelCard.cgi_travelcardnumber == "1220235062"
                                    || travelCard.cgi_travelcardnumber == "1223837414"
                                    || travelCard.cgi_travelcardnumber == "1185567510")
                                    continue;

                                validTravelCard = travelCard;
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Unvalid TravelCard ({travelCard.cgi_travelcardnumber}) found. Ex: {ex.Message}");
                            }
                        }
                        #endregion

                        #region Set Created Contact on valid Travel Card (validTravelCard)
                        validTravelCard.cgi_Contactid = contact.ToEntityReference();
                        XrmHelper.Update(localContext, validTravelCard);
                        #endregion

                        #region Call WebAPI to block Travel Card and send Value Code (Below maximum amount)

                        try
                        {
                            //Set up http request
                            string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCode";
                            //string url = $"http://localhost:37909/api/ValueCode/CreateValueCode";
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                            WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            //Send request
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                var InputJSON = WebApiTestHelper.GenericSerializer(CreateValueCodeTestData_ValidTravelCardAndContact(contact, validTravelCard, amountToUse, 2));
                                streamWriter.Write(InputJSON);
                                streamWriter.Flush();
                                streamWriter.Close();
                            }

                            //Get response
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                //Read response
                                var response = streamReader.ReadToEnd();
                                localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                                Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                            }
                        }
                        catch (WebException we)
                        {
                            HttpWebResponse response = (HttpWebResponse)we.Response;
                            if (response == null)
                                throw we;

                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                                throw new Exception(result);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                        }
                        #endregion


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error. Ex: {ex.Message}");
                    }
                    finally
                    {
                        //XrmHelper.Delete(localContext.OrganizationService, contact.ToEntityReference());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Test]
        public void CreateValueCode_Presentkort_AboveLimit()
        {


            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                    #region Create Contact to use (contact)
                    var contact = new ContactEntity
                    {
                        cgi_socialsecuritynumber = "19910201",
                        FirstName = "Marcus",
                        LastName = "Stenswed" + DateTime.Now.Hour + DateTime.Now.Minute,
                        EMailAddress1 = "marcus.stenswed@endeavor.se",
                        MobilePhone = "0735198846",
                        Telephone2 = "0735198846",
                        Telephone1 = "0735198846",
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };

                    Guid contactId = XrmHelper.Create(localContext, contact);
                    contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(true));
                    #endregion

                    //var maxAmount = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountWebsiteValueCode);
                    int maxAmount = 500;

                    #region Retrieve TravelCards with Amount (cards)

                    var travelCardQuery = new QueryExpression()
                    {
                        EntityName = TravelCardEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, "123456")
                            }
                        }
                    };

                    var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardQuery);

                    var card = TravelCardEntity.GetAndParseCardDetails(localContext, travelCard.cgi_travelcardnumber);
                    TravelCardEntity.ValidateTravelCard(localContext, card);

                    //var card = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, travelCard.cgi_travelcardnumber);
                    //var parsedCard = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, card);

                    //var respBiztalk = TravelCardEntity.ValidateCardDetailsFromBiztalk(localContext, 3, new System.Net.Http.HttpResponseMessage(), parsedCard);

                    #endregion


                    #region Set Created Contact on valid Travel Card (validTravelCard)
                    travelCard.cgi_Contactid = contact.ToEntityReference();
                    XrmHelper.Update(localContext, travelCard);
                    #endregion


                    #region Call WebAPI to block Travel Card and send Value Code (Above maximum amount)

                    try
                    {
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCode";
                        //string url = $"http://localhost:37909/api/ValueCode/CreateValueCode";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            var requestAmount = maxAmount + (int)card.Body.GetCardDetails2Response.GetCardDetails2Result.CardDetails2.PurseDetails.Balance;
                            var InputJSON = WebApiTestHelper.GenericSerializer(CreateValueCodeTestData_ValidTravelCardAndContact(contact, travelCard, requestAmount, 2));
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                        }

                        /*
                         * Verify that Approval has been created
                         */
                        var approvalQuery = new QueryExpression()
                        {
                            EntityName = ValueCodeApprovalEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ValueCodeApprovalEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                                }
                            }
                        };

                        var approvalEntity = XrmRetrieveHelper.RetrieveFirst<ValueCodeApprovalEntity>(localContext, approvalQuery);
                        Assert.IsNotNull(approvalEntity, "Could not find approval.");

                        //Clear data
                        //TestDataHelper.DeleteContactById(localContext, contact.Id);
                        //TestDataHelper.DeleteValueCodeApprovalsByTravelCardNumber(localContext, travelCard.cgi_travelcardnumber);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error. Ex: {ex.Message}");
            }
        }

        [Test, Category("Debug")]
        public void CreateValueCode_ForlustGaranti_BelowLimit()
        {
            //For deleteing contact
            Guid contactId_test = Guid.Empty;



            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    #region Create Contact to use (contact)
                    var contact = new ContactEntity
                    {
                        cgi_socialsecuritynumber = "19910201",
                        FirstName = "Marcus",
                        LastName = "Stenswed" + DateTime.Now.Hour + DateTime.Now.Minute,
                        EMailAddress1 = "marcus.stenswed@endeavor.se",
                        MobilePhone = "0735198846",
                        Telephone2 = "0735198846",
                        Telephone1 = "0735198846",
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };

                    Guid contactId = XrmHelper.Create(localContext, contact);
                    contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(true));
                    contactId_test = contactId;
                    #endregion

                    //var maxAmount = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountWebsiteValueCode);
                    int maxAmount = 500;


                    #region Retrieve TravelCards with Amount (cards)

                    var travelCardQuery = new QueryExpression()
                    {
                        EntityName = TravelCardEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, "1315159187")
                            }
                        }
                    };

                    var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardQuery);

                    if (travelCard == null)
                    {
                        var newTravelCard = new TravelCardEntity() { cgi_travelcardnumber = "1315159187", cgi_TravelCardCVC = "123", cgi_LatestAutoloadAmount = new Money(maxAmount - 1) };
                        travelCard = newTravelCard;
                        travelCard.Id = XrmHelper.Create(localContext, newTravelCard);
                    }

                    var card = TravelCardEntity.GetAndParseCardDetails(localContext, travelCard.cgi_travelcardnumber);
                    TravelCardEntity.ValidateTravelCard(localContext, card);

                    #endregion

                    #region Set Created Contact on valid Travel Card (validTravelCard)
                    travelCard.cgi_Contactid = contact.ToEntityReference();
                    XrmHelper.Update(localContext, travelCard);
                    #endregion

                    #region Call WebAPI to block Travel Card and send Value Code (Below maximum amount)

                    try
                    {
                        Console.WriteLine("Setting up http request.");
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                        //string url = $"http://localhost:37909/api/TravelCard/BlockTravelCard";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            var InputJSON = WebApiTestHelper.GenericSerializer(new TravelCard() { TravelCardNumber = travelCard.cgi_travelcardnumber, CVC = travelCard.cgi_TravelCardCVC });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                    }

                    /*********************************/

                    try
                    {
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
                        //string url = $"http://localhost:37909/api/ValueCode/CreateValueCodeLossCompensation";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            ValueCodeLossCreation vclc = new ValueCodeLossCreation()
                            {
                                ContactId = contactId,
                                deliveryMethod = 2,
                                Email = "marcus.stenswed@endeavor.se",
                                FirstName = "Marcus",
                                LastName = "Stenswed",
                                Mobile = "+46735198846",
                                TravelCard = new TravelCard()
                                {
                                    CVC = "123",
                                    TravelCardNumber = "1315159187"
                                },
                                TypeOfValueCode = (int)Generated.ed_valuecodetypeglobal.Forlustgaranti
                            };

                            var InputJSON = WebApiTestHelper.GenericSerializer(vclc);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                        }

                        /*
                         * Verify that a value code has been created 
                         */


                        var query = new QueryExpression()
                        {
                            EntityName = ValueCodeEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ValueCodeEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                                }
                            }
                        };

                        var valueCode = XrmRetrieveHelper.RetrieveFirst<ValueCodeEntity>(localContext, query);
                        Assert.IsNotNull(valueCode, "Could not find value code.");

                        //Clear data
                        //TestDataHelper.DeleteContactById(localContext, contact.Id);
                        //TestDataHelper.DeleteValueCodeApprovalsByTravelCardNumber(localContext, travelCard.cgi_travelcardnumber);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                    }

                    #endregion

                }
            }
            catch (WebException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        [Test]
        public void CreateValueCode_ForlustGaranti_AboveLimit()
        {

            Guid contactId_test = Guid.Empty;
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    #region Create Contact to use (contact)
                    var contact = new ContactEntity
                    {
                        cgi_socialsecuritynumber = "19910201",
                        FirstName = "Marcus",
                        LastName = "Stenswed" + DateTime.Now.Hour + DateTime.Now.Minute,
                        EMailAddress1 = "marcus.stenswed@endeavor.se",
                        MobilePhone = "0700158181",
                        Telephone2 = "0735198846",
                        Telephone1 = "0735198846",
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };

                    Guid contactId = XrmHelper.Create(localContext, contact);
                    contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(true));
                    contactId_test = contactId;
                    #endregion

                    //var maxAmount = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_MaxAmountWebsiteValueCode);
                    int maxAmount = 500;

                    #region Retrieve TravelCards with Amount (cards)

                    var travelCardQuery = new QueryExpression()
                    {
                        EntityName = TravelCardEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.Equal, "654321")
                            }
                        }
                    };

                    var travelCard = XrmRetrieveHelper.RetrieveFirst<TravelCardEntity>(localContext, travelCardQuery);

                    if(travelCard == null)
                    {
                        var newTravelCard = new TravelCardEntity() { cgi_travelcardnumber = "654321", cgi_TravelCardCVC = "321", cgi_LatestAutoloadAmount = new Money(maxAmount + 1) };
                        travelCard = newTravelCard;
                        travelCard.Id = XrmHelper.Create(localContext, newTravelCard);
                    }

                    //var card = TravelCardEntity.GetAndParseCardDetails(localContext, travelCard.cgi_travelcardnumber);
                    //TravelCardEntity.ValidateTravelCard(localContext, card);

                    #endregion

                    #region Set Created Contact on valid Travel Card (validTravelCard)
                    travelCard.cgi_Contactid = contact.ToEntityReference();
                    XrmHelper.Update(localContext, travelCard);
                    #endregion

                    #region Call WebAPI to block Travel Card and send Value Code (Below maximum amount)

                    try
                    {
                        Console.WriteLine("Setting up http request.");
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                        //string url = $"http://localhost:37909/api/TravelCard/BlockTravelCard";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            var InputJSON = WebApiTestHelper.GenericSerializer(new TravelCard() { TravelCardNumber = travelCard.cgi_travelcardnumber, CVC = travelCard.cgi_TravelCardCVC });
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                    }

                    /*********************************/

                    try
                    {
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
                        //string url = $"http://localhost:37909/api/ValueCode/CreateValueCodeLossCompensation";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {

                            var InputJSON = WebApiTestHelper.GenericSerializer(CreateValueCodeTestData_ValidTravelCardAndContact(contact, travelCard, 50, 3));
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        //Get response
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //Read response
                            var response = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                        }

                        /*
                         * Verify that a value code has been created 
                         */


                        var query = new QueryExpression()
                        {
                            EntityName = ValueCodeApprovalEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ValueCodeApprovalEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                                }
                            }
                        };

                        var approval = XrmRetrieveHelper.RetrieveFirst<ValueCodeApprovalEntity>(localContext, query);
                        Assert.IsNotNull(approval, "Could not find approval.");

                        //Clear data
                        //TestDataHelper.DeleteContactById(localContext, contact.Id);
                        //TestDataHelper.DeleteValueCodeApprovalsByTravelCardNumber(localContext, travelCard.cgi_travelcardnumber);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response == null)
                            throw we;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            throw new Exception(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [Test]
        public void CreateValueCode_BelowMaxAmount_WithContactId()
        {
            try
            {


                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    //DeleteAllTestData(localContext);

                    //Set up http request
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCode";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    ValueCodeCreationGiftCard valueCodePresentkort = new ValueCodeCreationGiftCard()
                    {
                        Amount = 0,
                        ContactId = null,
                        deliveryMethod = 2,
                        Email = "marcus.stenswed@endeavor.se",
                        FirstName = "MarcusTest",
                        LastName = "StenswedTest",
                        Mobile = "0735198846",
                        TravelCard = new TravelCard()
                        {
                            CVC = "5256",
                            TravelCardNumber = "1078584733"
                        },
                        TypeOfValueCode = 2
                    };

                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        // TODO : Marcus Generate
                        var InputJSON = WebApiTestHelper.GenericSerializer(valueCodePresentkort);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                    }

                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Create value code without associated travel card
        /// </summary>
        [Test]
        public void TestCreateValueCodeGeneric()
        {

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext,
                    new EntityReference(ContactEntity.EntityLogicalName, Guid.Parse("7FDA72A0-8689-E811-80EF-005056B61FFF")), new ColumnSet(true));

                //ValueCodeHandler.CreateValueCodeGeneric(localContext, 2, DateTime.Now.AddDays(2), 20, Generated.ed_valuecode_ed_typeoption.Mobile, null,
                    //3, contact, "0700158181", null, null, null, null);

                var travelCard = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext,
                    new EntityReference(TravelCardEntity.EntityLogicalName, Guid.Parse("4a6827e3-aa7b-e911-80f0-005056b61fff")),
                    new ColumnSet());

                ValueCodeHandler.CreateValueCodeGeneric(localContext, 2, DateTime.Now.AddDays(2), 10, 550, Generated.ed_valuecode_ed_typeoption.Mobile, null,
                    3, contact, "0700158181", null, null, null, null, travelCard/*, 0, "", new DateTime(), new DateTime()*/);

            }
        }

        [Test, Category("Debug")]
        public void BizTalkTest()
        {
            try
            {
                try
                {
                    string travelCardNumber = "13151591875";
                    // MAKE SOAP REQUEST
                    string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2/20141216'>" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<int:GetCardDetails2>" +
                                "<int:CardSerialNumber>" + travelCardNumber + "</int:CardSerialNumber>" +
                            "</int:GetCardDetails2>" +
                        "</soapenv:Body>" +
                    "</soapenv:Envelope>";

                    string soapResponse = "";
                    string bizTalkUrl = "";

                    //TRY GET SERVICE URL
                    try
                    {
                        bizTalkUrl = "http://v-dkbiz-tst.int.skanetrafiken.com/INTSTDK004/CardDetails2.svc";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
                    }

                    // REMOVE SUFFIX OF URL
                    string[] parts = bizTalkUrl.Split('/');
                    string soapActionAddress = "";
                    for (int x = 0; x < parts.Length - 1; x++)
                    {
                        soapActionAddress += parts[x];
                        soapActionAddress += "/";
                    }

                    //TRY SEND REQUEST
                    try
                    {


                        using (var client = new System.Net.WebClient())
                        {
                            // the Content-Type needs to be set to XML
                            client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                            client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                            client.Encoding = Encoding.UTF8;
                            //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");

                            soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                        }
                    }
                    catch (WebException ex)
                    {
                        var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new Exception(resp, ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
                    }

                    //return soapResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An unexpected error occurred when contacting BizTalk service: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Test]
        public void GetWaitingLoad()
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                    
                    //_log.Debug($"GetWaitingCharges: Step 2");

                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("starrepublic:stars-are-nice");
                    string encodedAuth = System.Convert.ToBase64String(plainTextBytes);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    
                    string url = "http://10.16.228.100/api/v3/outstandingcharges";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.Headers.Add("Authorization", encodedAuth);

                    //_log.Debug($"GetWaitingCharges: Step 3");

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = JsonConvert.SerializeObject("12345");
                        //string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //_log.Debug($"GetWaitingCharges: Step 4");

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }

                    url += "/12345";

                    //string url = $"{WebApiTestHelper.WebApiRootEndpoint}ping"; // Ping
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";
                    httpWebRequest.Headers.Add("Authorization", encodedAuth);


                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        //localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, result);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [Test]
        public void GetValueCodeAmountLimit()
        {
            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();

                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/GetMaxAmountValueCode/";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    //httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, result);
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }

        public string RetrieveCardInformation(Plugin.LocalPluginContext localContext, string travelCardNumber)
        {
            try
            {
                try
                {
                    //string travelCardNumber = "4684713218";
                    // MAKE SOAP REQUEST
                    string soapmessage = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:int='http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2/20141216'>" +
                    "<soapenv:Header/>" +
                        "<soapenv:Body>" +
                            "<int:GetCardDetails2>" +
                                "<int:CardSerialNumber>" + travelCardNumber + "</int:CardSerialNumber>" +
                            "</int:GetCardDetails2>" +
                        "</soapenv:Body>" +
                    "</soapenv:Envelope>";

                    string soapResponse = "";
                    string bizTalkUrl = "";

                    //TRY GET SERVICE URL
                    try
                    {
                        bizTalkUrl = "http://stip.skanetrafiken.se/INTSTDK004/CardDetails2.svc";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred when retreiving BizTalk URL: {ex.Message}", ex);
                    }

                    // REMOVE SUFFIX OF URL
                    string[] parts = bizTalkUrl.Split('/');
                    string soapActionAddress = "";
                    for (int x = 0; x < parts.Length - 1; x++)
                    {
                        soapActionAddress += parts[x];
                        soapActionAddress += "/";
                    }

                    //TRY SEND REQUEST
                    try
                    {


                        using (var client = new System.Net.WebClient())
                        {
                            // the Content-Type needs to be set to XML
                            client.Headers.Add("Content-Type", "text/xml;charset=utf-8");
                            client.Headers.Add("SOAPAction", "\"" + soapActionAddress + "");
                            client.Encoding = Encoding.UTF8;
                            //client.Credentials = new System.Net.NetworkCredential("crmadmin", "__", "d1");

                            soapResponse = client.UploadString("" + bizTalkUrl + "", soapmessage);
                        }
                    }
                    catch (WebException ex)
                    {
                        var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        throw new Exception(resp, ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred when contacting BizTalk service: {ex.Message}", ex);
                    }

                    return soapResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An unexpected error occurred when contacting BizTalk service: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string SerializeXmlMessage(Plugin.LocalPluginContext localContext, string xmlMsg)
        {
            return JsonConvert.SerializeObject(xmlMsg, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private void DeleteAllTestData(Plugin.LocalPluginContext localContext)
        {
            TestDataHelper.DeleteTravelCardByTravelCardNumber(localContext, cTravelCardNumber);
            //TestDataHelper.DeleteValueCodeApprovalsByContact(localContext, TestDataHelper.GetContactByFullName(localContext, cFirstName, cLastName));
            //TestDataHelper.DeleteValueCodesByContact(localContext, TestDataHelper.GetContactByFullName(localContext, cFirstName, cLastName));
            //TestDataHelper.DeleteContactByName(localContext, cLastName);
        }

        /// <summary>
        /// Fetch valid travel cards from Biff with balance above 0 and assign value to TravelCardEntity.
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        private TravelCardEntity FetchValidTravelCardWithBalanceFromBiztalk(Plugin.LocalPluginContext localContext, bool aboveMaxAmount, int maxAmount = 0)
        {

            var validCardQuery = new QueryExpression()
            {
                EntityName = TravelCardEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(
                    TravelCardEntity.Fields.cgi_Blocked,
                    TravelCardEntity.Fields.cgi_travelcardnumber,
                    TravelCardEntity.Fields.st_Balance,
                    TravelCardEntity.Fields.cgi_TravelCardCVC,
                    TravelCardEntity.Fields.cgi_LatestAutoloadAmount),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.NotNull),
                        new ConditionExpression(TravelCardEntity.Fields.statuscode, ConditionOperator.Equal, (int)Generated.cgi_travelcard_statuscode.Active),
                        new ConditionExpression(TravelCardEntity.Fields.cgi_Blocked, ConditionOperator.Equal, false),
                        new ConditionExpression(TravelCardEntity.Fields.cgi_value_card_type, ConditionOperator.Equal, "Jojo Reskassa"),
                    }
                }
            };

            var crmTravelCards = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, validCardQuery);

            //This will make multiple requests to Biztalk
            foreach (var card in crmTravelCards)
            {
                var cardDetailsResp = ValueCodeHandler.CallGetCardDetailsFromBiztalkAction(localContext, card.cgi_travelcardnumber);
                var parsedCardDetail = ValueCodeHandler.CallParseCardDetailsFromBiztalkAction(localContext, cardDetailsResp);
                var res = TravelCardEntity.ValidateCardDetailsFromBiztalkForPresentkort(localContext, 0, new System.Net.Http.HttpResponseMessage(), parsedCardDetail);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    if(aboveMaxAmount && parsedCardDetail.BalanceField > maxAmount)
                    {
                        card.cgi_LatestAutoloadAmount = new Money(parsedCardDetail.BalanceField + maxAmount);
                        return card;
                    }
                    if (!aboveMaxAmount && parsedCardDetail.BalanceField > 0)
                    {
                        //Since balance in CRM doesn't match with biff, use value from biff.
                        card.cgi_LatestAutoloadAmount = new Money(parsedCardDetail.BalanceField);
                        return card;
                    }
                }
            }
            return null;
        }

        [Test, Explicit, Category("Debug")]
        public void Debug_CreateValueCodeLossCompensation_ManuallBody()
        {
            //Set up http request
            string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
            //string url = $"http://localhost:37909/api/ValueCode/CreateValueCodeLossCompensation";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //Send request
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var InputJSON = "{ \"TravelCard\":{ \"TravelCardNumber\":\"1075075901\",\"CVC\":\"24F8\"},\"Mobile\":\"0768890551\",\"deliveryMethod\":2,\"ContactId\":\"7fc57a1e-27eb-e411-80d6-0050569071be\",\"FirstName\":\"Per\",\"LastName\":\"Ahrling\",\"TypeOfValueCode\":3}";
                streamWriter.Write(InputJSON);
                streamWriter.Flush();
                streamWriter.Close();
            }

            //Get response
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //Read response
                var response = streamReader.ReadToEnd();
                //localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            }
        }


        #region All tests towards BlockTravelCard

        private TravelCard CreateTestData_TravelCard(string number = null, string cvc = null)
        {
            return new TravelCard { TravelCardNumber = number, CVC = cvc };
        }


        [Test]
        public void Test_BlockTravelCard_MissingTravelCardNumber()
        {
            CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/TravelCard/BlockTravelCard", 
                CreateTestData_TravelCard(), typeof(TravelCard), 
                delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                    Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC), s);
                    Assert.AreEqual(HttpStatusCode.BadRequest, h.StatusCode);
                });
        }
        [Test]
        public void Test_BlockTravelCard_MissingCVC()
        {
            CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/TravelCard/BlockTravelCard",
                CreateTestData_TravelCard("9999999999"), typeof(TravelCard),
                delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                    Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC), s);
                    Assert.AreEqual(HttpStatusCode.BadRequest, h.StatusCode);
                });
        }
        [Test]
        public void Test_BlockTravelCard_NonExistingTravelCard()
        {
            CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/TravelCard/BlockTravelCard",
                CreateTestData_TravelCard("999999", "9999"), typeof(TravelCard),
                delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                    Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_TravelCardNumberAndCVC), s);
                    Assert.AreEqual(HttpStatusCode.BadRequest, h.StatusCode);
                });
        }
        [Test]
        public void Test_BlockTravelCard_HotlistedTravelCard()
        {
            CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/TravelCard/BlockTravelCard",
                CreateTestData_TravelCard("1317785283", "9999"), typeof(TravelCard),
                delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                    Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_TravelCardBlocked), s);
                    Assert.AreEqual(HttpStatusCode.OK, h.StatusCode);
                });
        }

        //[Test]
        //public void Test_BlockTravelCard_ActivePeriodWith_0_Balance()
        //{

        //}
        //[Test]
        //public void Test_BlockTravelCard_ActivePeriodWithMoreThan_0_Balance()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //    }
        //}
        //[Test]
        //public void Test_BlockTravelCard_NonActivePeriodWith_0_Balance()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //    }
        //}
        //[Test]
        //public void Test_BlockTravelCard_NonActivePeriodWithMoreThan_0_Balance()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //    }
        //}

        ////Wait with this one because travel cards are currently going towards production.
        //[Test]
        //public void Test_BlockTravelCard_BlockTravelCard()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //    }
        //}

        #endregion

        #region All tests towards CreateValueCodeLossCompensation

        /// <summary>
        /// Creates test data för ValueCodeLossCreation
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="travelCardNumber"></param>
        /// <param name="CVC"></param>
        /// <param name="deliveryMethod"></param>
        /// <param name="mobile"></param>
        /// <param name="email"></param>
        /// <param name="contactId"></param>
        /// <param name="typeOfValueCode"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private ValueCodeLossCreation CreateTestData_ValueCode(string travelCardNumber = null, string CVC = null, int? deliveryMethod = null, string mobile = null, string email = null, 
            ContactEntity contact = null, int? typeOfValueCode = null, string firstName = null, string lastName = null)
        {
            var travelCard = new TravelCard() { TravelCardNumber = travelCardNumber, CVC = CVC };

            return new ValueCodeLossCreation() { ContactId = contact?.Id, TravelCard = travelCard, deliveryMethod = deliveryMethod.HasValue == true ? deliveryMethod.Value : 0,
                Email = email, Mobile = mobile, FirstName = firstName, LastName = lastName, TypeOfValueCode = typeOfValueCode.HasValue == true ? typeOfValueCode.Value : 0 };
        }

        private ContactEntity CreateTestData_CreateContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var contact = new ContactEntity
                {
                    Id = Guid.Parse(vcContactId),
                    FirstName = vcFirstName,
                    LastName = vcLastName + DateTime.Now.Hour + DateTime.Now.Minute,
                    EMailAddress1 = vcEmail,
                    MobilePhone = vcMobile,
                    Telephone2 = vcMobile,
                    Telephone1 = vcMobile,
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                };

                contact.Id = XrmHelper.Create(localContext, contact);
                return contact;
            }
        }
        
        [Test]
        public void Test_CreateValueCodeLossCompensation_NoValueCodeInBody()
        {
                CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/ValueCode/CreateValueCodeLossCompensation",
                CreateTestData_ValueCode(), typeof(ValueCodeLossCreation),
                delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                    Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_EmailOrPhoneNumber), s);
                    Assert.AreEqual(HttpStatusCode.BadRequest, h.StatusCode);
                });
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_NoEmailAndMobile()
        {
            CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/ValueCode/CreateValueCodeLossCompensation",
            CreateTestData_ValueCode(), typeof(ValueCodeLossCreation),
            delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h) {
                Assert.AreEqual(ReturnMessageWebApiEntity.GetValueString(l, ReturnMessageWebApiEntity.Fields.ed_EmailOrPhoneNumber), s);
                Assert.AreEqual(HttpStatusCode.BadRequest, h.StatusCode);
            });
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_EmailAndMobile()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

            }
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_EmailDeliveryMethodValue()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

            }
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_MobileDeliveryMethodValue()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

            }
        }

        [Test]
        public void Test_CreateValueCodeLossCompensation_NoTravelCardInBody()
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
                    //Set up http request
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    TestDataHelper.DeleteContactById(localContext, Guid.Parse(vcContactId));

                    //Set up request body
                    var contact = new ContactEntity
                    {
                        Id = Guid.Parse(vcContactId),
                        cgi_socialsecuritynumber = "900101",
                        FirstName = vcFirstName,
                        LastName = vcLastName + DateTime.Now.Hour + DateTime.Now.Minute,
                        EMailAddress1 = vcEmail,
                        MobilePhone = vcMobile,
                        Telephone2 = vcMobile,
                        Telephone1 = vcMobile,
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };

                    XrmHelper.Create(localContext, contact);


                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(CreateValueCodeTestData_ValidTravelCardAndContact(contact, null, 10, 3));
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    //Get response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        //Read response
                        var response = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                        Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);

                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        throw new Exception(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                }

            }
        }

        [Test]
        public void Test_CreateValueCodeLossCompensation_NoTravelCardNumber()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());



                var travelCard = XrmRetrieveHelper.Retrieve<TravelCardEntity>(localContext,
                    new EntityReference(TravelCardEntity.EntityLogicalName, Guid.Parse("4FF22FE2-AB6C-E911-80F0-005056B61FFF")),
                    new ColumnSet(true));
            }
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_NoCVC()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

            }
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_TypeOfValueCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                throw new NotImplementedException("To be finished!");
            }
        }


        [Test]
        public void Test_CreateValueCodeLossCompensation_ExistingContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                throw new NotImplementedException("To be finished!");
            }
        }


        [Test]
        public void Test_CreateValueCodeLossCompensation_NonExistingContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string testInstance = CustomerUtility.GetUnitTestID();

                TravelCard travelCard = new TravelCard() {
                    TravelCardNumber = "1126000483",
                    CVC = "f1cc",
                };

                ValueCodeCreationGiftCard valueCode = new ValueCodeCreationGiftCard()
                {
                    Amount = 100,
                    deliveryMethod = 1,
                    Email = CustomerUtility.GetUnitTestEmailAddress(testInstance),
                    FirstName = testInstance,
                    LastName = testInstance,
                    Mobile = CustomerUtility.GetUnitTestMobilePhone(testInstance),
                    TypeOfValueCode = 2,
                    TravelCard = travelCard
                };

                int iThread = 0;
                HttpResponseMessage res = new HttpResponseMessage();
                ValueCodeController vcc = new ValueCodeController();
                res = vcc.HandleCreateValueCode(localContext, valueCode, iThread);
                //var res = ValueCodeController.HandleCreateValueCode(localContext, valueCode, iThread);

                Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);

            }
        }


        [Test]
        public void Test_CreateValueCodeLossCompensation_NonExistingTravelCard()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                throw new NotImplementedException("To be finished!");
            }
        }
        [Test]
        public void Test_CreateValueCodeLossCompensation_ExistingApproval()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                throw new NotImplementedException("To be finished!");
            }
        }

        [Test]
        public void Test_CreateValueCodeLossCompensation_TravelCardBlockedInCRM()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                throw new NotImplementedException("To be finished!");
            }
        }

        /// <summary>
        /// Verifies that correct error message is thrown when:
        /// * User is not loggin
        /// * Using a travelcard that is already connected to a contact in CRM
        /// This should result in a error like:
        ///     "Jojo-kortet kan inte spärras då det är registrerat på Mitt konto. Logga in för att på Mitt konto för att spärra kortet." 
        /// </summary>
        [Test, Category("Regression")]
        public void Test_CreateValueCodeLossCompensation_TravelCardConnectedToUserInCRM()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                var query = new QueryExpression()
                {
                    EntityName = TravelCardEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(TravelCardEntity.Fields.cgi_travelcardnumber
                                            , TravelCardEntity.Fields.cgi_TravelCardCVC
                                            , TravelCardEntity.Fields.cgi_Contactid),
                    Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(TravelCardEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.cgi_travelcardState.Active),
                                new ConditionExpression(TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.NotNull),
                                new ConditionExpression(TravelCardEntity.Fields.cgi_travelcardnumber, ConditionOperator.NotNull),
                                new ConditionExpression(TravelCardEntity.Fields.cgi_TravelCardCVC, ConditionOperator.NotNull),
                            }
                        }
                };

                // Get a card from CRM to test on
                IList<TravelCardEntity> travelCardFromCRMs = XrmRetrieveHelper.RetrieveMultiple<TravelCardEntity>(localContext, query);
                TravelCardEntity travelCardFromCRM = new TravelCardEntity();

                foreach (var item in travelCardFromCRMs)
                {
                    // Is CVC less than 4 chars?
                    if (item.cgi_TravelCardCVC.Length != 4)
                    {
                        // Remove CVC code
                        item.cgi_TravelCardCVC = null;
                        localContext.OrganizationService.Update(item);
                    }
                    else { 
                        travelCardFromCRM = item;
                        break;
                    }
                }

                if (travelCardFromCRM == null)
                    throw new Exception("Failed to get a valid travelcard from crm for test");

                ContactEntity connectedContact = ContactEntity.GetContactById(localContext, new ColumnSet(true), travelCardFromCRM.cgi_Contactid.Id);

                string testInstance = CustomerUtility.GetUnitTestID();
                TravelCard travelCard = new TravelCard()
                {
                    TravelCardNumber = travelCardFromCRM.cgi_travelcardnumber,
                    CVC = travelCardFromCRM.cgi_TravelCardCVC,
                };


                ValueCodeCreationGiftCard valueCode = new ValueCodeCreationGiftCard()
                {
                    Amount = 100,
                    deliveryMethod = 1,
                    Email = CustomerUtility.GetUnitTestEmailAddress(testInstance),
                    FirstName = connectedContact.FirstName,
                    LastName = connectedContact.LastName,
                    Mobile = CustomerUtility.GetUnitTestMobilePhone(testInstance),
                    TypeOfValueCode = 2,
                    TravelCard = travelCard,
                    ContactId = null    // User not signed in
                };

                localContext.Trace($"Testing {travelCard.TravelCardNumber}-{travelCard.CVC} connected to {valueCode.FirstName} {valueCode.LastName}");

                int iThread = 0;

                HttpResponseMessage res = new HttpResponseMessage();
                ValueCodeController vcc = new ValueCodeController();
                res = vcc.HandleCreateValueCode(localContext, valueCode, iThread);
                //res = ValueCodeController.HandleCreateValueCode(localContext, valueCode, iThread);

                Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);

                // TODO, verify that message returned is correct

            }
        }


        #endregion

        #region All tests towards Gift Card

        [Test]
        public void Test_CreateValueCodeGiftCard_NoValueCodeInBody()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //try
                //{
                //    //Set up http request
                //    string url = $"{WebApiTestHelper.WebApiRootEndpoint}ValueCode/CreateValueCodeLossCompensation";
                //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                //    httpWebRequest.ContentType = "application/json";
                //    httpWebRequest.Method = "POST";


                //    //Set up request body


                //    //Send request
                //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //    {
                //        var InputJSON = WebApiTestHelper.GenericSerializer(CreateValueCodeTestData_ValidTravelCardAndContact(contact, validTravelCard, amountToUse, 2));
                //        streamWriter.Write(InputJSON);
                //        streamWriter.Flush();
                //        streamWriter.Close();
                //    }

                //    //Get response
                //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        //Read response
                //        var response = streamReader.ReadToEnd();
                //        localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, response);

                //        Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                //    }
                //}
                //catch (WebException we)
                //{
                //    HttpWebResponse response = (HttpWebResponse)we.Response;
                //    if (response == null)
                //        throw we;

                //    using (var streamReader = new StreamReader(response.GetResponseStream()))
                //    {
                //        var result = streamReader.ReadToEnd();
                //        throw new Exception(result);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"Exception caught when creating value code. Ex: {ex.Message}");
                //}

            }
        }


        #endregion

    }
    #endregion
}