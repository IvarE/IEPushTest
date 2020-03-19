using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using NUnit.Framework;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.UnitTest
{
    public class BuyAndSendFixture : PluginFixtureBase
    {
        #region Full flow test

        [TestCase("Marcus@mail.com", "Marcus", "Ende", 61631, 512221, 99633, "Skickad", 2)]
        [TestCase("Magnus@mail.com", "Magnus", "Fran", 55512, 77583, 15228, "Skickad", 7)]
        [TestCase("vancarlnguyen@mail.com", "Van Carl", "Nguyen", 124161, 961865, 516250, "Skickad", 5 )]
        public void TestAPI_BuyAndSend_FullFlow_POST(string email = null, string first = null, string last = null, int cardNr = 0, 
            int orderId = 0, int orderLineId = 0, string status = null, int numberOfOrderLines = 0)
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
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();


                    DeleteAllTestData(localContext, email, orderId);

                    var customer = Create_CustomerInfo_For_KopOchSkickaKund(email, first, last);


                    CallApi($"Contacts", customer, "POST", typeof(CustomerInfo),
                        delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h)
                        {
                            #region Contact assertion
                            var query = new QueryExpression()
                            {
                                EntityName = ContactEntity.EntityLogicalName,
                                ColumnSet = new ColumnSet(true),
                                Criteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, email ?? cEmail)
                                    }
                                }
                            };
                            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

                            Assert.NotNull(contact);
                            Assert.AreEqual(contact.Id.ToString(), s);
                            Assert.AreEqual(HttpStatusCode.OK, h.StatusCode);
                            Assert.AreEqual(email ?? cEmail, customer.Email);
                            Assert.AreEqual((int)Generated.ed_informationsource.KopOchSkicka, customer.Source);
                            Assert.AreEqual(first ?? cFirstName, customer.FirstName);
                            Assert.AreEqual(last ?? cLastName, customer.LastName);
                            #endregion

                            CallApi($"SalesOrders", GenerateValidSalesOrderInfo_ForKopOchSkicka(s, cardNr, orderId, orderLineId, status, numberOfOrderLines),
                                "POST", typeof(SalesOrderInfo), delegate (Plugin.LocalPluginContext ll, string ss, HttpWebResponse hh)
                                {
                                    #region SalesOrders Assertion
                                    query = new QueryExpression()
                                    {
                                        EntityName = SkaKortEntity.EntityLogicalName,
                                        ColumnSet = new ColumnSet(),
                                        Criteria =
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(SkaKortEntity.Fields.ed_Contact, ConditionOperator.Equal, Guid.Parse(s))
                                            }
                                        }
                                    };

                                    var cards = XrmRetrieveHelper.RetrieveMultiple<SkaKortEntity>(localContext, query);


                                    query = new QueryExpression()
                                    {
                                        EntityName = SalesOrderEntity.EntityLogicalName,
                                        ColumnSet = new ColumnSet(),
                                        Criteria =
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, cOrderNumber_KopOchSkicka)
                                            }
                                        }
                                    };

                                    var salesOrder = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, query);
                                    if (salesOrder != null)
                                    {

                                        query = new QueryExpression()
                                        {
                                            EntityName = SalesOrderLineEntity.EntityLogicalName,
                                            ColumnSet = new ColumnSet(),
                                            Criteria =
                                            {
                                                Conditions =
                                                {
                                                    new ConditionExpression(SalesOrderLineEntity.Fields.ed_SalesOrderId, ConditionOperator.Equal, salesOrder.Id)
                                                }
                                            }
                                        };

                                        var salesOrderLines = XrmRetrieveHelper.RetrieveMultiple<SalesOrderLineEntity>(localContext, query);

                                        Assert.AreEqual(cards.Count, salesOrderLines.Count);
                                    }

                                    Assert.AreEqual(HttpStatusCode.OK, hh.StatusCode);
                                    #endregion
                                });

                        });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Test]
        public void TestAPI_BuyAndSend_FullFlow_PUT()
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
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();


                    var customer = Create_CustomerInfo_For_KopOchSkickaKund();


                    var query = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, cEmail)
                            }
                        }
                    };

                    var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

                    var salesObj = GenerateValidSalesOrderInfo_ForKopOchSkicka(contact.Id.ToString());
                    salesObj.SalesOrderLines[0].Status = "Skickad";
                    salesObj.SalesOrderLines[1].Status = "Annat";
                    CallApi($"SalesOrders/{cOrderNumber_KopOchSkicka}", salesObj,
                        "PUT", typeof(SalesOrderInfo), delegate (Plugin.LocalPluginContext ll, string responseBody, HttpWebResponse webResponse)
                        {
                            Assert.AreEqual(HttpStatusCode.OK, webResponse.StatusCode);
                        });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        #region Helpers

        #region Data

        //CustomerInfoAddressBlock
        private const string cCity = "Stockholm";
        private const string cCO = "";
        private const string cCountryISO = "SE";
        private const string cPostalCode = "11824";
        private const string cLine1 = "Tavastgatan";

        //CustomerInfo
        private const string cFirstName = "Van Carl";
        private const string cLastName = "Endeavor";
        private const string cEmail = "vancarl@test.com";

        //SkaKort
        private int cSkaKortNumber = 175620;


        int cOrderNumber_KopOchSkicka = 13131515;
        int cOrderLineNumber_KopOchSkicka = 883623;
        const int cType_KopOchSkicka = 17;

        private class Order_KopOchSkicka
        {
            public int OrderId { get; set; }
            public List<OrderLine_KopOchSkicka> OrderLines { get; set; }
            public int Type { get; set; }
            public DateTime Created { get; set; }
            public string ContactGuid { get; set; }
        }

        private class OrderLine_KopOchSkicka
        {
            public int OrderId { get; set; }
            public int OrderLineId { get; set; }
            public int Status { get; set; }
            public string CardNumber { get; set; }
        }

        #endregion

        private SalesOrderInfo GenerateValidSalesOrderInfo_ForKopOchSkicka(string contactId, int cardNumber = 0, 
            int orderNumberId = 0, int orderLineNumberId = 0, string status = null, int numberOfOrderLines = 0)
        {
            DateTime orderTime = DateTime.Now.ToUniversalTime();
            orderTime = orderTime.AddTicks(-(orderTime.Ticks % TimeSpan.TicksPerSecond));

            var salesOrderLines = new List<SalesOrderLineInfo>();
            if (numberOfOrderLines > 0)
            {
                for (int i = 0; i < numberOfOrderLines; i++)
                    salesOrderLines.Add(new SalesOrderLineInfo()
                    { CardNumber = (cardNumber + i).ToString(), OrderId = (orderNumberId + i), OrderLineId = orderLineNumberId + i, Status = status });
            }

            else
            {
                salesOrderLines = new List<SalesOrderLineInfo>()
                {
                    new SalesOrderLineInfo() { CardNumber = cSkaKortNumber.ToString(), OrderId = cOrderNumber_KopOchSkicka, OrderLineId = cOrderLineNumber_KopOchSkicka, Status = "Skapad" },
                    new SalesOrderLineInfo() { CardNumber = cSkaKortNumber+1.ToString(), OrderId = cOrderNumber_KopOchSkicka+1, OrderLineId = cOrderLineNumber_KopOchSkicka+1, Status = "Skickad" },
                    new SalesOrderLineInfo() { CardNumber = cSkaKortNumber+2.ToString(), OrderId = cOrderNumber_KopOchSkicka+2, OrderLineId = cOrderLineNumber_KopOchSkicka+2, Status = "Skickad" },
                };
            }

            var salesOrderInfo = new SalesOrderInfo()
            {
                OrderNo = orderNumberId != 0 ? orderNumberId.ToString() : cOrderNumber_KopOchSkicka.ToString(),
                InformationSource = cType_KopOchSkicka,
                OrderTime = orderTime,
                SalesOrderLines = salesOrderLines.ToArray(),
                ContactGuid = contactId
            };

            return salesOrderInfo;

        }
        private void DeleteContact(Plugin.LocalPluginContext localContext, Guid contactId)
        {
            //Change status to inactive
            ContactEntity uppContact = new ContactEntity()
            {
                Id = contactId,
                StateCode = Generated.ContactState.Inactive
            };

            XrmHelper.Update(localContext, uppContact);

            ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(false));
            XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
        }
        private CustomerInfo Create_CustomerInfo_For_KopOchSkickaKund(string email = null, string firstName = null, string lastName = null)
        {
            CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

            CustomerInfo contactInfo = new CustomerInfo()
            {
                Source = (int)Generated.ed_informationsource.KopOchSkicka,
                Email = email ?? cEmail,
                FirstName = firstName ?? cFirstName,
                LastName = lastName ?? cLastName,
                AddressBlock = addressBlockData,
            };

            return contactInfo;
        }

        private CustomerInfoAddressBlock Create_AddressBlock_For_CustomerInfo()
        {
            try
            {
                CustomerInfoAddressBlock addressBlockData = new CustomerInfoAddressBlock()
                {
                    City = cCity,
                    CO = cCO,
                    CountryISO = cCountryISO,
                    PostalCode = cPostalCode,
                    Line1 = cLine1
                };

                return addressBlockData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteAllTestData(Plugin.LocalPluginContext localContext, string email = null, int orderNr = 0)
        {

            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, email ?? cEmail)
                    }
                }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);
            if (contact == null)
                return;

            query = new QueryExpression()
            {
                EntityName = SkaKortEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SkaKortEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                    }
                }
            };

            var skaKort = XrmRetrieveHelper.RetrieveMultiple<SkaKortEntity>(localContext, query);
            foreach (var card in skaKort)
            {
                XrmHelper.Delete(localContext, card.ToEntityReference());
            }


            query = new QueryExpression()
            {
                EntityName = SalesOrderEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(SalesOrderEntity.Fields.ed_OrderNo, ConditionOperator.Equal, orderNr != 0 ? orderNr.ToString() : cOrderNumber_KopOchSkicka.ToString())
                    }
                }
            };

            var salesOrder = XrmRetrieveHelper.RetrieveFirst<SalesOrderEntity>(localContext, query);
            if (salesOrder != null)
            {

                query = new QueryExpression()
                {
                    EntityName = SalesOrderLineEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(SalesOrderLineEntity.Fields.ed_SalesOrderId, ConditionOperator.Equal, salesOrder.Id)
                        }
                    }
                };

                var salesOrderLines = XrmRetrieveHelper.RetrieveMultiple<SalesOrderLineEntity>(localContext, query);
                foreach (var salesOrderLine in salesOrderLines)
                {
                    XrmHelper.Delete(localContext, salesOrderLine.ToEntityReference());
                }

                XrmHelper.Delete(localContext, salesOrder.ToEntityReference());
            }

            DeleteContact(localContext, contact.Id);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">Api endpoint</param>
        /// <param name="body"></param>
        /// <param name="destination">Specifies the type of which the body shall be converted to.</param>
        /// <param name="assert">Pass assertion code here</param>
        /// <param name="xrmAction"></param>
        private void CallApi(string endpoint, object body, string httpRequestMethod, Type destination, Action<Plugin.LocalPluginContext, string, HttpWebResponse> assert = null, Action<Plugin.LocalPluginContext> clearDataAction = null)
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                //Set up http request
                string url = $"{WebApiTestHelper.WebApiRootEndpoint}{endpoint}"; //$"{WebApiTestHelper.WebApiRootEndpoint}TravelCard/BlockTravelCard";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = httpRequestMethod;

                try
                {
                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(Convert.ChangeType(body, destination));
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

        #endregion

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }


    }
}
