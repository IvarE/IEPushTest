using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Skanetrafiken.Crm;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
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

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class SalesOrderFixture : PluginFixtureBase
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
        public void parseBiffResult()
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("ns", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("ns0", "http://www.skanetrafiken.com/DK/INTSTDK004/CardDetails2/20141216");
            namespaceManager.AddNamespace("ns1", "http://www.skanetrafiken.com/DK/INTSTDK004/GetCardDetails2Response/20141216");

            XDocument doc = XDocument.Load(@"C:\Stuff\Skånetrafiken\Main\Skanetrafiken.WebApi\Resources\BIFF-call result1.xml");

            XElement elem = doc.XPathSelectElement("./ns:Envelope/ns:Body/ns1:GetCardDetails2Response/ns1:GetCardDetails2Result/ns0:CardDetails2", namespaceManager);
            
            XmlSerializer serializer = new XmlSerializer(typeof(CardDetails2));
            byte[] utf8Array = Encoding.UTF8.GetBytes(new XDocument(elem).ToString());
            MemoryStream memStream = new MemoryStream(utf8Array);
            CardDetails2 cardDetails = (CardDetails2)serializer.Deserialize(memStream);
            
        }

        [Test, Category("Debug")]
        public void DebugSalesorderInfoTest()
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

                try
                {
                    SalesOrderInfo debugInfo = new SalesOrderInfo
                    {
                        OrderNo = "11-11-28",
                        OrderTime = new DateTime(2017, 08, 08, 13, 11, 00),
                        Customer = new CustomerInfo
                        {
                            FirstName = "SalesOrderTestFirstName",
                            LastName = "Salesordertestlastname:20170808.0111",
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                CO = null,
                                Line1 = "Testvägen 20170808.0111",
                                PostalCode = "12345",
                                City = "By20170808.0111",
                                CountryISO = "SE"
                            },
                            Telephone = "031201708080111",
                            Mobile = "0735201708080111",
                            SocialSecurityNumber = "201708089147",
                            Email = $"test20170808.0111@test.test",
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        },
                        Productinfos = new Productinfo[] {
                            new Productinfo
                            {
                                Reference = "3d0876c3",
                                NameOnCard = "CardName8999",
                                ProductCode = 11944,
                                Serial = "0017251994"
                            },
                            new Productinfo
                            {
                                Reference = "4f9eb86e",
                                NameOnCard = "CardName8c19",
                                ProductCode = 55883,
                                Serial = "0017254650"
                            },
                            new Productinfo
                            {
                                Reference = "14d238c1",
                                NameOnCard = "CardName91f5",
                                ProductCode = 23312,
                                Serial = "0017254794"
                            }
                        }
                    };

                    // OrderResult
                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SalesOrders/{debugInfo.OrderNo}");
                    this.CreateTokenForTest(localContext, httpWebRequest2);
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        string InputJSON = SerializeSalesOrderInfo(localContext, debugInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                    {
                        // Result is
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ValidateSalesorderInfoTest First Create results={0}", result);

                        // Validate result, must have DataValid
                        SalesOrderInfo info = JsonConvert.DeserializeObject<SalesOrderInfo>(result);

                        NUnit.Framework.Assert.AreEqual(debugInfo.OrderNo, info.OrderNo);
                        NUnit.Framework.Assert.AreEqual(debugInfo.OrderTime, info.OrderTime);

                        //NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Source, info.Customer.Source);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.FirstName, info.Customer.FirstName);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.LastName, info.Customer.LastName);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.Telephone, info.Customer.Telephone);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.Mobile, info.Customer.Mobile);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.SocialSecurityNumber, info.Customer.SocialSecurityNumber);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.Email, info.Customer.Email);
                        NUnit.Framework.Assert.NotNull(info.Customer.AddressBlock);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.AddressBlock.CO, info.Customer.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.AddressBlock.Line1, info.Customer.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.AddressBlock.PostalCode, info.Customer.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.AddressBlock.City, info.Customer.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(debugInfo.Customer.AddressBlock.CountryISO, info.Customer.AddressBlock.CountryISO);

                        NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos.Length, info.Productinfos.Length);
                        for (int i = 0; i < debugInfo.Productinfos.Length; i++)
                        {
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].Reference, info.Productinfos[i].Reference);
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].ProductCode, info.Productinfos[i].ProductCode);
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].Name, info.Productinfos[i].Name);
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].Qty, info.Productinfos[i].Qty);
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].NameOnCard, info.Productinfos[i].NameOnCard);
                            NUnit.Framework.Assert.AreEqual(debugInfo.Productinfos[i].Serial, info.Productinfos[i].Serial);
                        }
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
            }
        }


        [Test, Category("Run Always")]
        public void ValidateSalesorderInfoTest()
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

                int numberOfProducts = 3;

                SalesOrderInfo standardInfo = GenerateStandarsSalesOrderInfo(localContext, numberOfProducts);

                Productinfo[] fullInfos = new Productinfo[numberOfProducts];
                for (int i = 0; i < numberOfProducts; i++)
                {
                    fullInfos[i] = new Productinfo
                    {
                        Reference = standardInfo.Productinfos[i].Reference,
                        ProductCode = standardInfo.Productinfos[i].ProductCode,
                        Name = standardInfo.Productinfos[i].Name,
                        Qty = standardInfo.Productinfos[i].Qty,
                        QtySpecified = standardInfo.Productinfos[i].QtySpecified,
                        NameOnCard = standardInfo.Productinfos[i].NameOnCard,
                        Serial = standardInfo.Productinfos[i].Serial
                    };
                    standardInfo.Productinfos[i].Serial = null;
                }
                try
                {
                    // CreateOrder
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SalesOrders/");
                    this.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = SerializeSalesOrderInfo(localContext, standardInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ValidateSalesorderInfoTest First Create results={0}", result);

                        // Validate result, must have DataValid
                        SalesOrderInfo info = JsonConvert.DeserializeObject<SalesOrderInfo>(result);

                        NUnit.Framework.Assert.AreEqual(standardInfo.OrderNo, info.OrderNo);
                        NUnit.Framework.Assert.AreEqual(standardInfo.OrderTime, info.OrderTime);

                        //NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Source, info.Customer.Source);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.FirstName, info.Customer.FirstName);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.LastName, info.Customer.LastName);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, info.Customer.Telephone);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Mobile, info.Customer.Mobile);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.SocialSecurityNumber, info.Customer.SocialSecurityNumber);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Email, info.Customer.Email);
                        NUnit.Framework.Assert.NotNull(info.Customer.AddressBlock);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CO, info.Customer.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.Line1, info.Customer.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.PostalCode, info.Customer.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.City, info.Customer.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CountryISO, info.Customer.AddressBlock.CountryISO);

                        NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos.Length, info.Productinfos.Length);
                        NUnit.Framework.Assert.AreEqual(numberOfProducts, info.Productinfos.Length);
                        for (int i = 0; i < numberOfProducts; i++)
                        {
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Reference, info.Productinfos[i].Reference);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].ProductCode, info.Productinfos[i].ProductCode);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Name, info.Productinfos[i].Name);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Qty, info.Productinfos[i].Qty);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].NameOnCard, info.Productinfos[i].NameOnCard);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Serial, info.Productinfos[i].Serial);
                        }
#if DEV
                        // verify that the entities were created correctly
                        QueryExpression query = new QueryExpression
                        {
                            EntityName = SalesOrderEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(SalesOrderEntity.Fields.ed_ContactId, SalesOrderEntity.Fields.ed_Name, SalesOrderEntity.Fields.ed_OrderNo, SalesOrderEntity.Fields.ed_OrderPlacedOn),
                            Criteria =
                        {
                           Conditions =
                           {
                               new ConditionExpression(SalesOrderEntity.Fields.ed_SalesOrderId, ConditionOperator.Equal, info.Guid)
                           }
                        },
                            LinkEntities =
                        {
                            new LinkEntity
                            {
                                LinkFromEntityName = SalesOrderEntity.EntityLogicalName,
                                LinkFromAttributeName = SalesOrderEntity.Fields.ed_ContactId,
                                LinkToEntityName = ContactEntity.EntityLogicalName,
                                LinkToAttributeName = ContactEntity.Fields.ContactId,
                                Columns = ContactEntity.ContactInfoBlock
                            },
                            new LinkEntity
                            {
                                LinkFromEntityName = SalesOrderEntity.EntityLogicalName,
                                LinkFromAttributeName = SalesOrderEntity.Fields.ed_SalesOrderId,
                                LinkToEntityName = TravelCardEntity.EntityLogicalName,
                                LinkToAttributeName = TravelCardEntity.Fields.ed_SalesOrderId,
                                Columns = new ColumnSet(
                                    TravelCardEntity.Fields.cgi_travelcardnumber,
                                    TravelCardEntity.Fields.cgi_TravelCardName,
                                    TravelCardEntity.Fields.cgi_ValueCardTypeId,
                                    TravelCardEntity.Fields.cgi_Blocked,
                                    TravelCardEntity.Fields.cgi_Contactid,
                                    TravelCardEntity.Fields.ed_ReferenceNo
                                    )
                            }
                        }
                        };
                        IList<SalesOrderEntity> salesOrdersIList = XrmRetrieveHelper.RetrieveMultiple<SalesOrderEntity>(localContext, query);
                        List<SalesOrderEntity> salesOrders = new List<SalesOrderEntity>();
                        foreach (SalesOrderEntity s in salesOrdersIList)
                            salesOrders.Add(s);

                        NUnit.Framework.Assert.AreEqual(numberOfProducts, salesOrders.Count);
                        for (int i = 0; i < numberOfProducts; i++)
                        {
                            CustomerInfo c = standardInfo.Customer;
                            WrapperController.FormatCustomerInfo(ref c);
                            standardInfo.Customer = c;

                            SalesOrderEntity salesOrder = salesOrders.Find(s => standardInfo.Productinfos[i].Reference.Equals(s.GetAliasedValue<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.ed_ReferenceNo)));
                            NUnit.Framework.Assert.NotNull(salesOrder);
                            // SalesOrder
                            NUnit.Framework.Assert.AreEqual(standardInfo.OrderNo, salesOrder.ed_OrderNo);
                            NUnit.Framework.Assert.AreEqual(standardInfo.OrderTime, salesOrder.ed_OrderPlacedOn);

                            // Contact
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Source, (int)salesOrder.GetAliasedValue<Generated.ed_informationsource>(ContactEntity.EntityLogicalName, ContactEntity.Fields.ed_InformationSource));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.FirstName, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.FirstName));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.LastName, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.LastName));

                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CO, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_Line1));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.Line1, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_Line2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.PostalCode, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_PostalCode));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.City, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_City));

                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone1));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Mobile, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.SocialSecurityNumber, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.cgi_socialsecuritynumber));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Email, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.EMailAddress2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone1));

                            // Product / TravelCard
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Reference, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.ed_ReferenceNo));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].ProductCode, salesOrder.GetAliasedValueOrDefault<int?>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_ValueCardTypeId));
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Name, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.?));
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Qty, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.?));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].NameOnCard, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_TravelCardName));
                            NUnit.Framework.Assert.AreEqual(TravelCardEntity._NotDefined, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_travelcardnumber));
                        }
#endif
                    }
                    // OrderResult
                    standardInfo.Productinfos = fullInfos;
                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SalesOrders/{standardInfo.OrderNo}");
                    this.CreateTokenForTest(localContext, httpWebRequest2);
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        string InputJSON = SerializeSalesOrderInfo(localContext, standardInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                    {
                        // Result is
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ValidateSalesorderInfoTest First Create results={0}", result);

                        // Validate result, must have DataValid
                        SalesOrderInfo info = JsonConvert.DeserializeObject<SalesOrderInfo>(result);

                        NUnit.Framework.Assert.AreEqual(standardInfo.OrderNo, info.OrderNo);
                        NUnit.Framework.Assert.AreEqual(standardInfo.OrderTime, info.OrderTime);

                        //NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Source, info.Customer.Source);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.FirstName, info.Customer.FirstName);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.LastName, info.Customer.LastName);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, info.Customer.Telephone);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Mobile, info.Customer.Mobile);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.SocialSecurityNumber, info.Customer.SocialSecurityNumber);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Email, info.Customer.Email);
                        NUnit.Framework.Assert.NotNull(info.Customer.AddressBlock);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CO, info.Customer.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.Line1, info.Customer.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.PostalCode, info.Customer.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.City, info.Customer.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CountryISO, info.Customer.AddressBlock.CountryISO);

                        NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos.Length, info.Productinfos.Length);
                        NUnit.Framework.Assert.AreEqual(numberOfProducts, info.Productinfos.Length);
                        for (int i = 0; i < numberOfProducts; i++)
                        {
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Reference, info.Productinfos[i].Reference);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].ProductCode, info.Productinfos[i].ProductCode);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Name, info.Productinfos[i].Name);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Qty, info.Productinfos[i].Qty);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].NameOnCard, info.Productinfos[i].NameOnCard);
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Serial, info.Productinfos[i].Serial);
                        }
#if DEV
                        // verify that the entities were updated correctly
                        QueryExpression query = new QueryExpression
                        {
                            EntityName = SalesOrderEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(SalesOrderEntity.Fields.ed_ContactId, SalesOrderEntity.Fields.ed_Name, SalesOrderEntity.Fields.ed_OrderNo, SalesOrderEntity.Fields.ed_OrderPlacedOn),
                            Criteria =
                        {
                           Conditions =
                           {
                               new ConditionExpression(SalesOrderEntity.Fields.ed_SalesOrderId, ConditionOperator.Equal, info.Guid)
                           }
                        },
                            LinkEntities =
                        {
                            new LinkEntity
                            {
                                LinkFromEntityName = SalesOrderEntity.EntityLogicalName,
                                LinkFromAttributeName = SalesOrderEntity.Fields.ed_ContactId,
                                LinkToEntityName = ContactEntity.EntityLogicalName,
                                LinkToAttributeName = ContactEntity.Fields.ContactId,
                                Columns = ContactEntity.ContactInfoBlock
                            },
                            new LinkEntity
                            {
                                LinkFromEntityName = SalesOrderEntity.EntityLogicalName,
                                LinkFromAttributeName = SalesOrderEntity.Fields.ed_SalesOrderId,
                                LinkToEntityName = TravelCardEntity.EntityLogicalName,
                                LinkToAttributeName = TravelCardEntity.Fields.ed_SalesOrderId,
                                Columns = new ColumnSet(
                                    TravelCardEntity.Fields.cgi_travelcardnumber,
                                    TravelCardEntity.Fields.cgi_TravelCardName,
                                    TravelCardEntity.Fields.cgi_ValueCardTypeId,
                                    TravelCardEntity.Fields.cgi_Blocked,
                                    TravelCardEntity.Fields.cgi_Contactid,
                                    TravelCardEntity.Fields.ed_ReferenceNo
                                    )
                            }
                        }
                        };
                        IList<SalesOrderEntity> salesOrdersIList = XrmRetrieveHelper.RetrieveMultiple<SalesOrderEntity>(localContext, query);
                        List<SalesOrderEntity> salesOrders = new List<SalesOrderEntity>();
                        foreach (SalesOrderEntity s in salesOrdersIList)
                            salesOrders.Add(s);

                        NUnit.Framework.Assert.AreEqual(numberOfProducts, salesOrders.Count);
                        for (int i = 0; i < numberOfProducts; i++)
                        {
                            SalesOrderEntity salesOrder = salesOrders.Find(s => standardInfo.Productinfos[i].Reference.Equals(s.GetAliasedValue<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.ed_ReferenceNo)));
                            NUnit.Framework.Assert.NotNull(salesOrder);
                            // SalesOrder
                            NUnit.Framework.Assert.AreEqual(standardInfo.OrderNo, salesOrder.ed_OrderNo);
                            NUnit.Framework.Assert.AreEqual(standardInfo.OrderTime, salesOrder.ed_OrderPlacedOn);

                            // Contact
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Source, (int)salesOrder.GetAliasedValueOrDefault<Generated.ed_informationsource>(ContactEntity.EntityLogicalName, ContactEntity.Fields.ed_InformationSource));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.FirstName, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.FirstName));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.LastName, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.LastName));

                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.CO, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_Line1));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.Line1, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_Line2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.PostalCode, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_PostalCode));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.AddressBlock.City, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Address1_City));

                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone1));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Mobile, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.SocialSecurityNumber, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.cgi_socialsecuritynumber));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Email, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.EMailAddress2));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Customer.Telephone, salesOrder.GetAliasedValueOrDefault<string>(ContactEntity.EntityLogicalName, ContactEntity.Fields.Telephone1));

                            // Product / TravelCard
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Reference, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.ed_ReferenceNo));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].ProductCode, salesOrder.GetAliasedValueOrDefault<int?>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_ValueCardTypeId));
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Name, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.?));
                            //NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Qty, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.?));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].NameOnCard, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_TravelCardName));
                            NUnit.Framework.Assert.AreEqual(standardInfo.Productinfos[i].Serial, salesOrder.GetAliasedValueOrDefault<string>(TravelCardEntity.EntityLogicalName, TravelCardEntity.Fields.cgi_travelcardnumber));
                        }
#endif
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
                        localContext.TracingService.Trace("CreateNotifyMKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }


                stopwatch.Stop();
                localContext.TracingService.Trace("ValidateCustomerInfoTest time = {0}", stopwatch.Elapsed);
            }
        }

        private string SerializeSalesOrderInfo(Plugin.LocalPluginContext localContext, SalesOrderInfo standardInfo)
        {
            return JsonConvert.SerializeObject(standardInfo, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private SalesOrderInfo GenerateStandarsSalesOrderInfo(Plugin.LocalPluginContext localContext, int numberOfProducts = 1)
        {
            string testInstanceName = CustomerUtility.GetUnitTestID();

            DateTime orderTime = DateTime.Now.ToUniversalTime();
            orderTime = orderTime.AddTicks(-(orderTime.Ticks % TimeSpan.TicksPerSecond));
            string orderNo = orderTime.ToString().Substring(orderTime.ToString().Length - 8).Replace(':', '-');

            string personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(orderTime);
            CustomerInfo customer = GenerateValidCustomerInfo(localContext, testInstanceName, personnummer);

            Productinfo[] products = new Productinfo[numberOfProducts];
            for (int i = 0; i < numberOfProducts; i++)
                products[i] = GenerateValidRandomProductInfo(localContext);

            SalesOrderInfo info = new SalesOrderInfo
            {
                OrderNo = orderNo,
                BusinessUnit = "TestBU",
                OrderTime = orderTime,
                Customer = customer,
                Productinfos = products
            };
            return info;
        }

        private Productinfo GenerateValidRandomProductInfo(Plugin.LocalPluginContext localContext)
        {
            string[] numbers = Guid.NewGuid().ToString().Split('-');

            return new Productinfo
            {
                Reference = numbers[0],
                ProductCode = Int32.Parse(numbers[1], System.Globalization.NumberStyles.HexNumber),
                Name = $"Name{numbers[2]}",
                Qty = new Random().Next(7),
                QtySpecified = true,
                NameOnCard = $"CardName{numbers[3]}",
                Serial = numbers[4]
            };
        }

        private CustomerInfo GenerateValidCustomerInfo(Plugin.LocalPluginContext localContext, string testInstanceName, string personnummer)
        {
            return new CustomerInfo
            {
                FirstName = "SalesOrderTestFirstName",
                LastName = "SalesOrderTestLastName:" + testInstanceName,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "Testvägen " + testInstanceName,
                    PostalCode = "12345",
                    City = "By" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = personnummer,
                Email = $"test{testInstanceName}@test.test",
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true
            };
        }

        private CustomerInfo GenerateValidCustomerInfo(Plugin.LocalPluginContext localContext, string testInstanceName)
        {
            return GenerateValidCustomerInfo(localContext, testInstanceName, CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now));
        }
    }
}
