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
    public class LeadApiFixture : PluginFixtureBase
    {

        [Test]
        public void ValidateEmail_Test()
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

                    var contact2 = CreateTestData_Contact(localContext, "199302044293", "Test2", "Test3", "test@gmail.test");
                    var leadInfo = CreateTestData_LeadInfo(contact2.Id.ToString());
                    //Create contact from lead
                    CallApi($"{WebApiTestHelper.WebApiRootEndpoint_LocalTest}/Leads/{leadInfo.Guid}", leadInfo, "PUT", typeof(LeadInfo),
                        delegate (Plugin.LocalPluginContext ll, string ss, HttpWebResponse hh)
                        {

                        });


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        

        private ContactEntity CreateTestData_Contact(Plugin.LocalPluginContext localContext, string socialSec, string firstName, string lastName, string email)
        {
            var newContact = new ContactEntity()
            {
                cgi_socialsecuritynumber = socialSec,
                FirstName = firstName,
                LastName = lastName,
                EMailAddress1 = email,
                ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
            };

            newContact.Id = XrmHelper.Create(localContext, newContact);
            return newContact;
        }

        private LeadInfo CreateTestData_LeadInfo(string guid)
        {
            var leadInfo = new LeadInfo()
            {
                IsCampaignActiveSpecified = false,
                CampaignDiscountPercentSpecified = false,
                ServiceType = 0,
                Source = 1,
                SwedishSocialSecurityNumber = false,
                SwedishSocialSecurityNumberSpecified = true,
                CreditsafeOkSpecified = false,
                AvlidenSpecified = false,
                UtvandradSpecified = false,
                EmailInvalidSpecified = false,
                Guid = guid,
                MklId = "2167655",
                isLockedPortalSpecified = false,
                isAddressEnteredManuallySpecified = false,
                isAddressInformationCompleteSpecified = false
            };

            return leadInfo;
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
