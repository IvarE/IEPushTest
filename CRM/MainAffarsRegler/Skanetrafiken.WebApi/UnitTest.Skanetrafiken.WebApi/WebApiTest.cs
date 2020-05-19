using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Skanetrafiken.Crm.Controllers;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System.Threading;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm.Extensions;

namespace Endeavor.Crm.UnitTest
{
    [TestClass]
    public class WebApiTest : PluginFixtureBase
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

        [Test, Category("Regression")]
        public void ValidateCustomerInfoTest()
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

                #region Test Code

                Skanetrafiken.Crm.StatusBlock validateStatus;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, ValidCustomerInfo_SkapaKonto1);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate ValidCustomerInfo_SkapaKonto1.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }

                //Testing Persnummer
                CustomerInfo testInfo = ValidCustomerInfo_SkapaKonto1;
                testInfo.SocialSecurityNumber = invalidPersNr1;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr1"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr2;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr2"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr3;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr3"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr4;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr4"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr5;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr5"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr6;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr6"));
                }
                testInfo.SocialSecurityNumber = invalidPersNr7;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidPersNr7"));
                }
                testInfo.SocialSecurityNumber = validPersNr1;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate validPersNr1.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }

                //Testing Emails
                testInfo.Email = invalidEmail1;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidEmail1."));
                }
                testInfo.Email = invalidEmail2;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidEmail2."));
                }
                testInfo.Email = invalidEmail3;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidEmail3."));
                }
                testInfo.Email = invalidEmail4;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo validated invalidEmail4."));
                }
                testInfo.Email = validemail1;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate email1.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }
                testInfo.Email = validemail2;
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, testInfo);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate email2.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }


                // Testing valid Data
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, ValidCustomerInfo_SkapaKonto1_MinimalInfo);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate ValidCustomerInfo_SkapaKonto1_MinimalInfo.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, ValidCustomerInfo_OinloggatKop_MinimalInfoNotLoggedInPurchase);
                if (!validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo did not validate ValidCustomerInfo_SkapaKonto1_MinimalInfoNotLoggedInPurchase.\nErrorMessage =\n\n{0}", validateStatus.ErrorMessage));
                }

                // Testing invalid Data
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, null);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo() validated null."));
                }
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, InvalidCustomerInfo_SkapaKonto_PersnumberMissing);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo() validated InvalidCustomerInfo_SkapaKonto_PersnumberMissing."));
                }
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, InvalidCustomerInfo_SkapaKonto_InvalidMailAddress);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo() validated InvalidCustomerInfo_SkapaKonto_InvalidMailAddress."));
                }
                validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, InvalidCustomerInfo_AdmSkapaAndraKund);
                if (validateStatus.TransactionOk)
                {
                    throw new Exception(string.Format("ValidateCustomerInfo() validated InvalidCustomerInfo_AdmSkapaAndraKund."));
                }

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("ValidateCustomerInfoTest time = {0}", stopwatch.Elapsed);
            }
        }

        [Test, Category("Regression")]
        public void CreateTwoMittKontoSameEmailSameMobile()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    Guid ContactId1 = Guid.Empty;
                    Guid ContactId2 = Guid.Empty;
                    string mobileContact1 = "";
                    string mobileContact2 = "";
                    string emailContact1 = "";
                    string emailContact2 = "";

                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;

                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    CustomerInfo customer2 = customer;

                    mobileContact1 = customer.Mobile;
                    mobileContact2 = customer.Mobile;
                    emailContact1 = customer.Email;
                    emailContact2 = customer.Email;

                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = customer.AddressBlock.CO,
                            Line1 = "UpdatedLine1",
                            PostalCode = customer.AddressBlock.PostalCode,
                            City = "UpdatedCity",
                            CountryISO = customer.AddressBlock.CountryISO
                        },
                        SocialSecurityNumber = customer.SocialSecurityNumber,
                        SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber,
                        SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Telephone = customer.Telephone,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    // Contact 1
                    #region SkapaKontoLead (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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

                    #region Hämta LatestLinkGuid Lead (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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

                    #region ValideraEmailSkapaKund (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }

                            if (!Guid.TryParse(info.Guid, out ContactId1))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion
                    
                    customerId = Guid.Empty;

                    #region SkapaKontoLead (second contact)
                    {
                        customer.Email = emailContact1;
                        customer.SocialSecurityNumber = customer2.SocialSecurityNumber;
                        customer.Mobile = mobileContact1;
                        //customer.Telephone = customer2.Telephone;

                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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

                        try
                        {
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                WrapperController.FormatCustomerInfo(ref customer);

                                // Result is 
                                var result = streamReader.ReadToEnd();
                                localContext.TracingService.Trace("CreateCustomerLead results={0}", result);

                                // Validate result, must have DataValid
                                CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                                NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                                NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                                NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                                NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                                NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                                NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                                NUnit.Framework.Assert.NotNull(info.AddressBlock);
                                NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                                NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                                NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                                NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                                NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                                if (!Guid.TryParse(info.Guid, out customerId))
                                {
                                    throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                                }

                                //// Get lead to validate subject
                                //LeadEntity newlyCreatedLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, customerId
                                //        , new ColumnSet(LeadEntity.Fields.Subject));

                                //// Make sure we have a subject
                                //NUnit.Framework.Assert.IsNotNull(newlyCreatedLead.Subject);

                                throw new Exception("Should not get here because of duplicate Emails");
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
                                localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);

                                if (!result.Contains("# Kunde inte skapa Kund. En kund med samma e-postadress och mobilnummer existerar redan."))
                                    throw new Exception(result, we);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Lead (second contact) - not used stops before
                    {
                        //localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        //_partTimer.Restart();
                        //localContext.TracingService.Trace("\nHämta LatestLinkGuid Lead:");

                        //string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLatestLinkGuid/{customer.Email}/";
                        //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //httpWebRequest.ContentType = "application/json";
                        //httpWebRequest.Method = "GET";

                        //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        //{
                        //    var result = streamReader.ReadToEnd();
                        //    localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                        //    linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        //}
                    }
                    #endregion

                    #region ValideraEmailSkapaKund (second contact) - not used stops before
                    //{
                    //    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    //    _partTimer.Restart();
                    //    localContext.TracingService.Trace("\nValidera epost:");
                    //    CustomerInfo verifyEmailInfo = new CustomerInfo
                    //    {
                    //        Guid = customerId.ToString(),
                    //        Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                    //        MklId = "jättemycketMKlId"
                    //    };
                    //    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{linkGuid}";
                    //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                    //    httpWebRequest.ContentType = "application/json";
                    //    httpWebRequest.Method = "PUT";

                    //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    //    {
                    //        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, verifyEmailInfo);

                    //        streamWriter.Write(InputJSON);
                    //        streamWriter.Flush();
                    //        streamWriter.Close();
                    //    }

                    //    try
                    //    {
                    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //        {
                    //            var result = streamReader.ReadToEnd();
                    //            localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
                    //            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                    //            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                    //            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                    //            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                    //            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                    //            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                    //            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                    //            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                    //            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                    //            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                    //            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                    //            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                    //            if (!Guid.TryParse(info.Guid, out customerId))
                    //            {
                    //                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                    //            }
                    //        }

                    //        throw new Exception("Should not end up here.. Duplicate emails");
                    //    }
                    //    catch (WebException we)
                    //    {
                    //        if (we.Message == "Unable to connect to the remote server")
                    //            throw we;

                    //        HttpWebResponse response = (HttpWebResponse)we.Response;
                    //        if (response == null)
                    //        {
                    //            throw we;
                    //        }

                    //        using (var streamReader = new StreamReader(response.GetResponseStream()))
                    //        {
                    //            var result = streamReader.ReadToEnd();
                    //            localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);

                    //            if(!result.Contains("# Kunde inte skapa Kund. En kund med samma e-postadress existerar redan"))
                    //                throw new Exception(result, we);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw ex;
                    //    }
                    //}
                    #endregion

                    #region HämtaKunduppgifter (second contact) - not used stops before
                    //{
                    //    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    //    _partTimer.Restart();
                    //    localContext.TracingService.Trace("\nHämta kunduppgifter:");
                    //    string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                    //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                    //    httpWebRequest.ContentType = "application/json";
                    //    httpWebRequest.Method = "GET";

                    //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //    {
                    //        var result = streamReader.ReadToEnd();
                    //        localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                    //        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //        NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                    //        NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                    //        NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                    //        NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                    //        NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                    //        NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                    //        NUnit.Framework.Assert.NotNull(info.AddressBlock);
                    //        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                    //        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                    //        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                    //        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                    //        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    //        NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                    //    }
                    //}
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
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
        public void CreateTwoMittKontoDifferentEmailSameSSN()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;

                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    CustomerInfo customer2 = customer;
                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = customer.AddressBlock.CO,
                            Line1 = "UpdatedLine1",
                            PostalCode = customer.AddressBlock.PostalCode,
                            City = "UpdatedCity",
                            CountryISO = customer.AddressBlock.CountryISO
                        },
                        SocialSecurityNumber = customer.SocialSecurityNumber,
                        SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber,
                        SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Telephone = customer.Telephone,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    // Contact 1
                    #region SkapaKontoLead (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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

                    #region Hämta LatestLinkGuid Lead (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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

                    #region ValideraEmailSkapaKund (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter (first contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion

                    // Contact 2 = Same Social Security Number and different Email

                    customerId = Guid.Empty;

                    #region SkapaKontoLead (second contact)
                    {
                        customer.Email += "e";
                        customer.SocialSecurityNumber = customer2.SocialSecurityNumber;

                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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

                    #region Hämta LatestLinkGuid Lead (second contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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

                    #region ValideraEmailSkapaKund (second contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter (second contact)
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }


        [Test, Category("Regression"), Category("Pure WebApi")]
        public void FullFlowAffarsregler()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();


                try
                {


                    var query = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(false),
                        Criteria =
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, "199102013936")
                                    }
                                }
                    };
                    var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

                    if (contact == null)
                    {
                        contact = new ContactEntity()
                        {
                            cgi_socialsecuritynumber = "199102013936",
                            FirstName = "Marcus",
                            LastName = "Stenswed",
                            EMailAddress1 = "marcus.stenswed@endeavor.se",
                            ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                            ed_HasSwedishSocialSecurityNumber = true
                        };

                        Guid contactId = XrmHelper.Create(localContext, contact);
                        contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(false));
                    }

                    contact.cgi_socialsecuritynumber = "";
                    contact.ed_InformationSource = Generated.ed_informationsource.AdmAndraKund;

                    XrmHelper.Update(localContext.OrganizationService, contact);

                    //XrmHelper.Delete(localContext.OrganizationService, contact.ToEntityReference());

                    string testInstanceName = WebApiTestHelper.GetUnitTestID();


                    AccountInfo accountInfoInitial = new AccountInfo();
                    CustomerInfo customerInfoInitial = new CustomerInfo();
                    string customerInfoInitialId = "";

                    // 2020-02-03 - Test for FTG
                    #region Create Business Account
                    {
                        AccountInfo accountInfo = GenerateValidAccount();
                        accountInfoInitial = accountInfo;

                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nAccount POST:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}accounts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.GenericSerializer(accountInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Account POST results={0}", result);
                        }
                    }
                    #endregion

                    // 2020-02-03 - Test for FTG
                    #region Create Business Contact
                    {
                        CustomerInfo customerInfo = GenerateValidBusinessContact(accountInfoInitial);
                        customerInfoInitial = customerInfo;

                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nContact POST:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            customerInfoInitialId = result;
                            localContext.TracingService.Trace("Contact POST results={0}", result);
                        }
                    }
                    #endregion


                    Guid customerId = Guid.Empty;
                    string linkGuid = null;


                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);

                    customer.Email = customerInfoInitial.CompanyRole[0].Email;
                    customer.Telephone = customerInfoInitial.CompanyRole[0].Telephone;
                    customer.FirstName = customerInfoInitial.FirstName;
                    customer.LastName = customerInfoInitial.LastName;
                    customer.SocialSecurityNumber = customerInfoInitial.SocialSecurityNumber;

                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public CustomerInfo GenerateValidBusinessContact(AccountInfo accountInfo)
        {
            CustomerInfoCompanyRole companyRole = new CustomerInfoCompanyRole()
            {
                CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                CompanyRoleSpecified = true,
                deleteCompanyRole = false,
                Email = GenerateValidEmail(),
                isLockedPortalSpecified = false,
                PortalId = accountInfo.PortalId,
                Telephone = "0701122334"
            };

            CustomerInfoCompanyRole[] companyRoleArr = new CustomerInfoCompanyRole[1];
            companyRoleArr[0] = companyRole;

            CustomerInfo customerInfo = new CustomerInfo()
            {
                AvlidenSpecified = false,
                CompanyRole = companyRoleArr,
                CreditsafeOkSpecified = false,
                EmailInvalidSpecified = false,
                FirstName = "Marcus",
                Guid = null,
                isAddressEnteredManuallySpecified = false,
                isAddressInformationCompleteSpecified = false,
                isLockedPortalSpecified = false,
                LastName = "Andersson",
                //ServiceType = null,
                SocialSecurityNumber = "199102013936",
                Source = (int)Generated.ed_informationsource.ForetagsPortal,
                SwedishSocialSecurityNumberSpecified = true,
                SwedishSocialSecurityNumber = true,
                UtvandradSpecified = false
            };

            return customerInfo;
        }

        /// <summary>
        /// Used for testing FTG Portal. Generating a valid Account to use for Account/POST
        /// </summary>
        /// <returns></returns>
        public AccountInfo GenerateValidAccount()
        {
            List<AddressInfo> addresses = new List<AddressInfo>();

            AddressInfo addressInfo = new AddressInfo()
            {
                City = "Stockholm",
                CountryISO = "SE",
                Name = "Faktureringsadress",
                PostalCode = "11249",
                Street = "Företagsgatan 1",
                TypeCode = (int)Generated.customeraddress_addresstypecode.BillTo
            };

            addresses.Add(addressInfo);


            AccountInfo info = new AccountInfo()
            {
                Addresses = addresses,
                AllowCreate = true,
                BillingEmailAddress = "invoice@foretaget.se",
                BillingMethod = (int)Generated.ed_account_ed_billingmethod.Email,
                CostSite = "",
                EMail = GenerateValidEmail(),
                Guid = null,
                InformationSource = (int)Generated.ed_informationsource.ForetagsPortal,
                IsLockedPortal = false,
                OrganizationName = "Företaget AB",
                OrganizationNumber = RandomOrgNumber(),
                PaymentMethod = (int)Generated.ed_account_ed_paymentmethod.Invoice,
                PortalId = RandomPortalId(),
                ReferencePortal = "SYSTEMTEST",
                Suborgname = "Undernamn Företaget AB"
            };

            return info;
        }


        /// <summary>
        /// Generate Random PortalId between 10000 and 99999
        /// </summary>
        /// <returns></returns>
        public string RandomPortalId()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString();
        }

        /// <summary>
        /// Generate Random Organization Number between 1000000000 and 1999999999
        /// </summary>
        /// <returns></returns>
        public string RandomOrgNumber()
        {
            Random random = new Random();
            return random.Next(1000000000, 1999999999).ToString();
        }

        public string GenerateValidEmail()
        {
            string email = "";

            Random random = new Random();
            // Generate 10 random email addresses.
            for (int i = 0; i < 10; ++i)
            {
                email = string.Format("marcus.{0:0000}@foretaget.se", random.Next(10000));
            }

            return email;
        }


        //[Test]
        //public void CanLeadBeCreatedTest()
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

        //        #region Test Code

        //        LeadEntity lead1 = LeadEntity.CreateLead(localContext, CanLeadBeCreated_Lead1);
        //        StatusBlock status = LeadEntity.CanLeadBeCreated(localContext, CanLeadBeCreated_Lead1);
        //        if (status.TransactionOk)
        //        {
        //            throw new Exception(string.Format("After Creating a lead, CanLeadBeCreated returned OK when inputted with the same data"));
        //        }
        //        localContext.OrganizationService.Delete(LeadEntity.EntityLogicalName, lead1.Id);

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        //[Test]
        //public void CreateLeadTest()
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

        //        #region Test Code

        //        LeadEntity lead1 = LeadEntity.CreateLead(localContext, CreateLead_Lead1);

        //        LeadEntity createdLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, lead1.Id, new ColumnSet(LeadEntity.Fields.FirstName,
        //                                                                                                              LeadEntity.Fields.LastName,
        //                                                                                                              LeadEntity.Fields.ed_Address1_Co,
        //                                                                                                              LeadEntity.Fields.Address1_Line1,
        //                                                                                                              LeadEntity.Fields.Address1_PostalCode,
        //                                                                                                              LeadEntity.Fields.Address1_City,
        //                                                                                                              LeadEntity.Fields.ed_Address1_Country,
        //                                                                                                              LeadEntity.Fields.Telephone1,
        //                                                                                                              LeadEntity.Fields.MobilePhone,
        //                                                                                                              LeadEntity.Fields.ed_Personnummer,
        //                                                                                                              LeadEntity.Fields.EMailAddress1));
        //        if (createdLead == null)
        //        {
        //            throw new Exception(string.Format("Lead == null when retrieving Lead after creation"));
        //        }

        //        if (createdLead.FirstName != CreateLead_Lead1.FirstName ||
        //            createdLead.LastName != CreateLead_Lead1.LastName ||
        //            createdLead.ed_Address1_Co != CreateLead_Lead1.AddressBlock.CO ||
        //            createdLead.Address1_Line1 != CreateLead_Lead1.AddressBlock.Line1 ||
        //            createdLead.Address1_PostalCode != CreateLead_Lead1.AddressBlock.PostalCode ||
        //            createdLead.Address1_City != CreateLead_Lead1.AddressBlock.City ||
        //            createdLead.ed_Address1_Country != CountryUtility.GetEntityRefForCountryCode(localContext, CreateLead_Lead1.AddressBlock.CountryISO) ||
        //            createdLead.Telephone1 != CreateLead_Lead1.Telephone ||
        //            createdLead.MobilePhone != CreateLead_Lead1.Mobile ||
        //            createdLead.ed_Personnummer != CreateLead_Lead1.SocialSecurityNumber ||
        //            createdLead.EMailAddress1 != CreateLead_Lead1.Email)
        //        {
        //            localContext.OrganizationService.Delete(LeadEntity.EntityLogicalName, createdLead.Id);
        //            throw new Exception(string.Format("When creating Lead from CreateLead_Lead1, the resulting entity in CRM did not match indata"));
        //        }

        //        localContext.OrganizationService.Delete(LeadEntity.EntityLogicalName, createdLead.Id);

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        //[Test]
        //public void ContactFromLeadTest()
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

        //        #region Test Code

        //        LeadEntity lead1 = LeadEntity.CreateLead(localContext, ContactFromLead_input1);

        //        ContactEntity contact1 = ContactEntity.CreateContactFromLead(localContext, lead1);
        //        ContactEntity createdContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contact1.Id, ContactEntity.ContactInfoBlock);

        //        if (createdContact.FirstName != ContactFromLead_input1.FirstName ||
        //            createdContact.LastName != ContactFromLead_input1.LastName ||
        //            createdContact.ed_Address1_Co != ContactFromLead_input1.AddressBlock.CO ||
        //            createdContact.Address1_Line1 != ContactFromLead_input1.AddressBlock.Line1 ||
        //            createdContact.Address1_PostalCode != ContactFromLead_input1.AddressBlock.PostalCode ||
        //            createdContact.Address1_City != ContactFromLead_input1.AddressBlock.City ||
        //            createdContact.ed_Address1_Country != CountryUtility.GetEntityRefForCountryCode(localContext, ContactFromLead_input1.AddressBlock.CountryISO) ||
        //            createdContact.Telephone1 != ContactFromLead_input1.Telephone ||
        //            createdContact.Telephone2 != ContactFromLead_input1.Mobile ||
        //            createdContact.cgi_socialsecuritynumber != ContactFromLead_input1.SocialSecurityNumber ||
        //            createdContact.EMailAddress1 != ContactFromLead_input1.Email
        //            )
        //        {
        //            throw new Exception("Information is not correct when examining Contact created From Lead");
        //        }

        //        localContext.OrganizationService.Delete(ContactEntity.EntityLogicalName, createdContact.Id);

        //        LeadEntity leadStillThere = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, new ColumnSet(false),
        //            new FilterExpression()
        //            {
        //                Conditions =
        //                {
        //                    new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, lead1.Id)
        //                }
        //            });
        //        if (leadStillThere != null)
        //        {
        //            localContext.OrganizationService.Delete(LeadEntity.EntityLogicalName, leadStillThere.Id);
        //            throw new Exception("Lead still existing after Contact was created from it.");
        //        }

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        //[Test]
        //public void GetContactFromEmailTest()
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

        //        #region Test Code

        //        ContactEntity contact = new ContactEntity(localContext, ValidCustomerInfo_SkapaKonto1);
        //        contact.Id = localContext.OrganizationService.Create(contact);

        //        ContactEntity retrievedContact = ContactEntity.GetValidatedContactFromEmail(localContext, ValidCustomerInfo_SkapaKonto1.Email);
        //        if (!retrievedContact.Id.Equals(contact.Id))
        //        {
        //            localContext.OrganizationService.Delete(ContactEntity.EntityLogicalName, contact.Id);
        //            localContext.OrganizationService.Delete(ContactEntity.EntityLogicalName, retrievedContact.Id);
        //            throw new Exception(string.Format("Guids does not match after creating and retrieving based on email"));
        //        }
        //        localContext.OrganizationService.Delete(ContactEntity.EntityLogicalName, contact.Id);

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        //[Test, Explicit]
        //public void SendEmailTest()
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

        //        #region Test Code

        //        LeadEntity lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, new Guid("D886595E-299C-E611-810F-00155D0A6B01"), LeadEntity.LeadInfoBlock);

        //        CrmPlusControl.SendValidationEmail(localContext, lead);

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        //[Test, Explicit]
        //public void WebApiCanLeadBeCreatedTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
        //        #region Test Setup

        //        CustomerInfo customer = new CustomerInfo();
        //        customer.FirstName = string.Format("Unit");
        //        customer.LastName = string.Format("Test ({0})", DateTime.Now.ToShortTimeString());
        //        customer.Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto;
        //        customer.Email = "test@rr.eu";

        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "CreateCustomerLead"));
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        #endregion

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string InputJSON = SerializeCustomerInfo(localContext, customer);

        //            streamWriter.Write(InputJSON);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            localContext.TracingService.Trace("Done, returned: {0}", result);
        //        }
        //    }
        //}


        //[Test, Explicit]
        //public void WebApiChangeEmailAddressTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
        //        #region Test Setup

        //        CustomerInfo customer = new CustomerInfo();
        //        customer.FirstName = string.Format("Unit");
        //        customer.LastName = string.Format("Test ({0})", DateTime.Now.ToShortTimeString());
        //        customer.Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto;
        //        customer.Email = "test@r.eu";

        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "ChangeEmailAddress"));
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        #endregion

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string InputJSON = SerializeCustomerInfo(localContext, customer);
        //            streamWriter.Write(InputJSON);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            localContext.TracingService.Trace("Done, returned: {0}", result);
        //        }

        //    }
        //}


        //[Test, Explicit]
        //public void WebApiUpdateContactTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        #region Test Setup

        //        CustomerInfo customer = new CustomerInfo();
        //        customer.FirstName = string.Format("Unit");
        //        customer.LastName = string.Format("Test ({0})", DateTime.Now.ToShortTimeString());
        //        customer.Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto;
        //        customer.Email = "test@r.eu";

        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "UpdateContact"));
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "POST";
        //        #endregion

        //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //        {
        //            string InputJSON = SerializeCustomerInfo(localContext, customer);

        //            streamWriter.Write(InputJSON);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            localContext.TracingService.Trace("Done, returned: {0}", result);
        //        }

        //    }
        //}



        //[Test, Explicit]
        //public void WebApiGetCustomerTest()
        //{

        //    #region Test Setup
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        string url = string.Format(@"{0}{1}?contactGuidOrEmail={2}", WebApiTestHelper.WebApiRootEndpoint, "GetContact", "95FCAD0E-50FD-E411-80D8-005056903A38");
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "GET";
        //        #endregion

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            localContext.TracingService.Trace("Done, returned: {0}", result);
        //        }

        //    }
        //}

        [Test, Category("Debug")]
        public void TestConnectionAgainCRM()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    ContactEntity testContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(true),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                 new ConditionExpression(ContactEntity.Fields.ed_InformationSource, ConditionOperator.Null),
                                 new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.NotNull),
                                 new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Skanetrafiken.Crm.Schema.Generated.ContactState.Active),
                                 new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull)
                            }
                        });
                }
                catch (Exception ex)
                {

                }
            }
        }

        [Test, Explicit]
        public void WebApiUtilTest()
        {

            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                try
                {
                    //CustomerInfo InitialCustomerInfo = new CustomerInfo
                    //{
                    //    FirstName = "Ullf",
                    //    LastName = "Hesusson",
                    //    Email = "ullf.hesusson@mail.com",
                    //    Telephone = "030212345",
                    //    Source = (int)CustomerUtility.Source.OinloggatKundArende
                    //};

                    //CustomerInfo creatingAccountInfo = new CustomerInfo
                    //{
                    //    FirstName = InitialCustomerInfo.FirstName,
                    //    LastName = InitialCustomerInfo.LastName,
                    //    Email = InitialCustomerInfo.Email,
                    //    AddressBlock = new CustomerInfoAddressBlock
                    //    {
                    //        Line1 = "Fluffgatan 1",
                    //        PostalCode = "89898",
                    //        City = "Falun",
                    //        CountryISO = "SE"
                    //    },
                    //    Mobile = "0703189911",
                    //    SocialSecurityNumber = "199108070054",
                    //    SwedishSocialSecurityNumber = true,
                    //    SwedishSocialSecurityNumberSpecified = true,
                    //    Source = (int)CustomerUtility.Source.SkapaMittKonto
                    //};

                    //{
                    //    string url1 = $"{WebApiTestHelper.WebApiRootEndpoint}contacts";
                    //    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url1);
                    //    this.CreateTokenForTest(localContext, httpWebRequest1);
                    //    httpWebRequest1.ContentType = "application/json";
                    //    httpWebRequest1.Method = "POST";

                    //    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    //    {
                    //        string InputJSON = SerializeCustomerInfo(localContext, InitialCustomerInfo);
                    //        streamWriter.Write(InputJSON);
                    //        streamWriter.Flush();
                    //        streamWriter.Close();
                    //    }

                    //    var httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();
                    //    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    //    {
                    //        var result = streamReader.ReadToEnd();
                    //        localContext.TracingService.Trace("Created 'incident customer', returned httpCode: {0} Content: {1}", httpResponse1.StatusCode, result);
                    //        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //    }
                    //}
                    //{
                    //    string url2 = $"{WebApiTestHelper.WebApiRootEndpoint}leads";
                    //    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url2);
                    //    this.CreateTokenForTest(localContext, httpWebRequest2);
                    //    httpWebRequest2.ContentType = "application/json";
                    //    httpWebRequest2.Method = "POST";

                    //    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    //    {
                    //        string InputJSON = SerializeCustomerInfo(localContext, creatingAccountInfo);
                    //        streamWriter.Write(InputJSON);
                    //        streamWriter.Flush();
                    //        streamWriter.Close();
                    //    }

                    //    var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    //    using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                    //    {
                    //        var result = streamReader.ReadToEnd();
                    //        localContext.TracingService.Trace("created lead for new account, returned httpCode: {0} Content: {1}", httpResponse2.StatusCode, result);
                    //        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //        creatingAccountInfo.Guid = info.Guid;
                    //    }
                    //}
                    //string linkGuid;
                    //{
                    //    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLatestLinkGuid/{creatingAccountInfo.Email}/";
                    //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //    this.CreateTokenForTest(localContext, httpWebRequest);
                    //    httpWebRequest.ContentType = "application/json";
                    //    httpWebRequest.Method = "GET";

                    //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //    {
                    //        var result = streamReader.ReadToEnd();
                    //        localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                    //        linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                    //    }
                    //}

                    //CustomerInfo verifyEmailInfo = new CustomerInfo
                    //{
                    //    Guid = creatingAccountInfo.Guid,
                    //    Source = (int)CustomerUtility.Source.ValideraEpost
                    //};
                    //Guid customerId = Guid.Empty;
                    //{
                    //    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{linkGuid}";
                    //    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //    this.CreateTokenForTest(localContext, httpWebRequest, Guid.Parse(creatingAccountInfo.Guid));
                    //    httpWebRequest.ContentType = "application/json";
                    //    httpWebRequest.Method = "PUT";

                    //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    //    {
                    //        string InputJSON = SerializeCustomerInfo(localContext, verifyEmailInfo);

                    //        streamWriter.Write(InputJSON);
                    //        streamWriter.Flush();
                    //        streamWriter.Close();
                    //    }

                    //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    //    {
                    //        var result = streamReader.ReadToEnd();
                    //        localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
                    //        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.Email, info.Email);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.FirstName, info.FirstName);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.LastName, info.LastName);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.Telephone, info.Telephone);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.Mobile, info.Mobile);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                    //        NUnit.Framework.Assert.NotNull(info.AddressBlock);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.AddressBlock.CO, info.AddressBlock.CO);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.AddressBlock.City, info.AddressBlock.City);
                    //        NUnit.Framework.Assert.AreEqual(creatingAccountInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                    //        if (!Guid.TryParse(info.Guid, out customerId))
                    //        {
                    //            throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                    //        }
                    //    }
                    //}

                    #endregion

                    {

                        //HttpResponseMessage resp = CrmPlusControl.ValidateEmail(new Guid("7a621f88-9a51-e711-80e3-0050569071be"), 4, "5b6f577d-14ef-447d-99e1-f4c8485b808a", "1234");


                        //string url = string.Format(@"{0}{1}/12/12", WebApiTestHelper.WebApiRootEndpoint, "leads");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/4f840d14-0926-e811-80ef-005056b61fff";
                        //string url = string.Format(@"{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "contacts");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = "{'CampaignCode':'or97fj','CampaignId':'CMP-01047-K0P3','IsCampaignActive':true,'IsCampaignActiveSpecified':true,'CampaignDiscountPercentSpecified':false,'CampaignProducts':[],'Source':7,'FirstName':'Jonas','LastName':'Nydahl','AddressBlock':{'CO':'C/o Poste Restante','Line1':'Östergatan 20 A','PostalCode':'203 10','City':'MALMÖ'},'Mobile':'0702101010','SwedishSocialSecurityNumber':false,'SwedishSocialSecurityNumberSpecified':true,'Email':'tina.nylen@cgi.com','CreditsafeOkSpecified':false,'AvlidenSpecified':false,'UtvandradSpecified':false,'EmailInvalidSpecified':false,'Guid':'4f840d14-0926-e811-80ef-005056b61fff','isAddressEnteredManuallySpecified':false,'isAddressInformationComplete':false,'isAddressInformationCompleteSpecified':true}";
                            //InputJSON = "{\"guid\":\"2668b9f0-b55b-e711-80e3-0050569071be\",\"email\":\"xxx\",\"source\":1}";

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, result);
                        }
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("UtilTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void ValidateEmailWithConflicts()
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
                    string testInstanceName = CustomerUtility.GetUnitTestID();
                    CustomerInfo fullInfo = ValidCustomerInfo_FullTest(testInstanceName);
                    fullInfo.AddressBlock.CO = null;

                    #region Validate Email with Conflicts Test
                    #region Create Conflict Contact
                    localContext.TracingService.Trace("\nSkapa konfliktKontakt:");
                    CustomerInfo contact2 = CustomerUtility.CopyInfo(fullInfo);
                    contact2.AddressBlock.CO = null;
                    contact2.AddressBlock.PostalCode = null;
                    contact2.AddressBlock.CountryISO = null;
                    contact2.Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende;

                    var httpWebRequestConflict = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestConflict);
                    httpWebRequestConflict.ContentType = "application/json";
                    httpWebRequestConflict.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestConflict.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, contact2);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseConflict = (HttpWebResponse)httpWebRequestConflict.GetResponse();

                    using (var streamReader = new StreamReader(httpResponseConflict.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref fullInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ValidateConflictEmail New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    }
                    #endregion

                    string leadGuid = null;
                    #region Create Lead
                    localContext.TracingService.Trace("\nSkapa Lead:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, fullInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref fullInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ValidateConflictEmail New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(fullInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(fullInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(fullInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(fullInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(fullInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                        NUnit.Framework.Assert.NotNull(info.AddressBlock);
                        NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                        leadGuid = info.Guid;
                    }
                    #endregion

                    string linkGuid = null;
                    #region Hämta LatestLinkGuid Lead
                    {
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLatestLinkGuid/{fullInfo.Email}/";
                        var httpWebRequestLink = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestLink);
                        httpWebRequestLink.ContentType = "application/json";
                        httpWebRequestLink.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequestLink.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Lead done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region Validate Conflict Lead
                    {
                        localContext.TracingService.Trace("\nValidera konflikt:");
                        CustomerInfo verifyEmailInfo = new CustomerInfo
                        {
                            Guid = leadGuid,
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "jättemycketMKlId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{linkGuid}";
                        var httpWebRequestValidate = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestValidate, new Guid(leadGuid));
                        httpWebRequestValidate.ContentType = "application/json";
                        httpWebRequestValidate.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequestValidate.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, verifyEmailInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequestValidate.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(fullInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(fullInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(fullInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(fullInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(fullInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(fullInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(fullInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion
                    #endregion
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Debug")]
        public void WebApiLoadTest()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                #endregion

                System.Diagnostics.Stopwatch roundWatch = new System.Diagnostics.Stopwatch();
                roundWatch.Restart();

                int threadNumber;
                List<Thread> threads;


                //threadNumber = 1;
                //threads = new List<Thread>();

                //for (int i = 0; i < threadNumber; i++)
                //{
                //    LoadTestGet ltg = new LoadTestGet(localContext, $"#{i}");
                //    Thread thread1 = new Thread(new ThreadStart(ltg.Test));

                //    thread1.Start();
                //    threads.Add(thread1);

                //    LoadTestPost ltp = new LoadTestPost(localContext, $"#{i}");
                //    Thread thread2 = new Thread(new ThreadStart(ltp.Test));

                //    thread2.Start();
                //    threads.Add(thread2);
                //}
                ////Thread.Sleep(10);
                //foreach (Thread t in threads)
                //{
                //    t.Join();
                //}
                //localContext.Trace($"Done with {threadNumber} parallel Threads in {roundWatch.ElapsedMilliseconds} ms\n\n");
                //roundWatch.Restart();

                //threadNumber = 2;
                //threads = new List<Thread>();

                //for (int i = 0; i < threadNumber; i++)
                //{
                //    LoadTestGet ltg = new LoadTestGet(localContext, $"#{i}");
                //    Thread thread1 = new Thread(new ThreadStart(ltg.Test));

                //    thread1.Start();
                //    threads.Add(thread1);

                //    LoadTestPost ltp = new LoadTestPost(localContext, $"#{i}");
                //    Thread thread2 = new Thread(new ThreadStart(ltp.Test));

                //    thread2.Start();
                //    threads.Add(thread2);
                //}
                ////Thread.Sleep(10);
                //foreach (Thread t in threads)
                //{
                //    t.Join();
                //}
                //localContext.Trace($"Done with {threadNumber} parallel Threads in {roundWatch.ElapsedMilliseconds} ms\n\n");
                //roundWatch.Restart();

                //threadNumber = 10;
                //threads = new List<Thread>();

                //for (int i = 0; i < threadNumber; i++)
                //{
                //    LoadTestGet ltg = new LoadTestGet(localContext, $"#{i}");
                //    Thread thread1 = new Thread(new ThreadStart(ltg.Test));

                //    thread1.Start();
                //    threads.Add(thread1);

                //    LoadTestPost ltp = new LoadTestPost(localContext, $"#{i}");
                //    Thread thread2 = new Thread(new ThreadStart(ltp.Test));

                //    thread2.Start();
                //    threads.Add(thread2);
                //}
                ////Thread.Sleep(10);
                //foreach (Thread t in threads)
                //{
                //    t.Join();
                //}
                //localContext.Trace($"Done with {threadNumber} parallel Threads in {roundWatch.ElapsedMilliseconds} ms\n\n");
                //roundWatch.Restart();

                threadNumber = 20;
                threads = new List<Thread>();

                for (int i = 0; i < threadNumber; i++)
                {
                    LoadTestGet ltg = new LoadTestGet(localContext, $"#{i}");
                    Thread thread1 = new Thread(new ThreadStart(ltg.Test));

                    thread1.Start();
                    threads.Add(thread1);

                    LoadTestPost ltp = new LoadTestPost(localContext, $"#{i}");
                    Thread thread2 = new Thread(new ThreadStart(ltp.Test));

                    thread2.Start();
                    threads.Add(thread2);
                }
                //Thread.Sleep(10);
                foreach (Thread t in threads)
                {
                    t.Join();
                }
                localContext.Trace($"Done with {threadNumber} parallel Threads in {roundWatch.ElapsedMilliseconds} ms\n\n");
                roundWatch.Restart();

            }
        }

        private class LoadTestPost : PluginFixtureBase
        {
            private Plugin.LocalPluginContext localContext;
            private string threadName;

            public LoadTestPost()
            {
                throw new Exception("Can't create LoadTest without Context");
            }

            public LoadTestPost(Plugin.LocalPluginContext lc, string name)
            {
                localContext = lc;
                threadName = name;
            }

            public void Test()
            {
                try
                {
                    System.Diagnostics.Stopwatch totalWatch = new System.Diagnostics.Stopwatch();
                    //System.Diagnostics.Stopwatch roundWatch = new System.Diagnostics.Stopwatch();
                    totalWatch.Restart();
                    int rounds = 100;
                    CustomerInfo leadInfo = CreateLead_Lead1;
                    leadInfo.Source = (int)Generated.ed_informationsource.SkapaMittKonto;
                    leadInfo.Telephone = leadInfo.Telephone.IndexOf('-') == -1 ? leadInfo.Telephone : leadInfo.Telephone.Remove(leadInfo.Telephone.IndexOf('-'), 1);
                    leadInfo.Mobile = leadInfo.Mobile.IndexOf('-') == -1 ? leadInfo.Mobile : leadInfo.Mobile.Remove(leadInfo.Mobile.IndexOf('-'), 1);
                    leadInfo.SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
                    leadInfo.SwedishSocialSecurityNumber = true;
                    leadInfo.SwedishSocialSecurityNumberSpecified = true;

                    for (int i = 0; i < rounds; i++)
                    {
                        //roundWatch.Restart();

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, leadInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //var result = streamReader.ReadToEnd();
                            //localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, result);
                        }
                        //if (i < 10)
                        //    localContext.Trace($"{threadName} - Contact GET completed in {roundWatch.ElapsedMilliseconds} millisecs");
                        //if (i == 10)
                        //{
                        //    localContext.Trace($"{threadName} - .");
                        //    localContext.Trace($"{threadName} - .");
                        //    localContext.Trace($"{threadName} - .");
                        //}
                        //if ((rounds - i) < 10)
                        //    localContext.Trace($"{threadName} - Contact GET completed in {roundWatch.ElapsedMilliseconds} millisecs");
                    }

                    localContext.Trace($"{threadName} - Total millisecs for {rounds} GETs = {totalWatch.ElapsedMilliseconds}");
                    localContext.Trace($"{threadName} - Average milliseconds for each GET = {totalWatch.ElapsedMilliseconds / rounds}");

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("LoadTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        private class LoadTestGet : PluginFixtureBase
        {
            private Plugin.LocalPluginContext localContext;
            private string threadName;

            public LoadTestGet()
            {
                throw new Exception("Can't create LoadTest without Context");
            }

            public LoadTestGet(Plugin.LocalPluginContext lc, string name)
            {
                localContext = lc;
                threadName = name;
            }

            public void Test()
            {
                try
                {
                    System.Diagnostics.Stopwatch totalWatch = new System.Diagnostics.Stopwatch();
                    //System.Diagnostics.Stopwatch roundWatch = new System.Diagnostics.Stopwatch();
                    totalWatch.Restart();
                    int rounds = 500;
                    Guid guid = new Guid("361FD7EE-5BE9-E611-8122-00155D010B02"); // UTV
                    //Guid guid = new Guid("9844fd55-0c1b-e711-80e2-005056906ae2"); // ACC

                    for (int i = 0; i < rounds; i++)
                    {
                        //roundWatch.Restart();

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{guid}"; // GET Contact
                        //string url = $"{WebApiTestHelper.WebApiRootEndpoint}ping"; // Ping
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, guid); // GET Contact
                        //CreateTokenForTest(localContext, httpWebRequest); // ping
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";


                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            //var result = streamReader.ReadToEnd();
                            //localContext.TracingService.Trace("Done, returned httpCode: {0} Content: {1}", httpResponse.StatusCode, result);
                        }
                        //if (i < 10)
                        //    localContext.Trace($"{threadName} - Contact GET completed in {roundWatch.ElapsedMilliseconds} millisecs");
                        //if (i == 10)
                        //{
                        //    localContext.Trace($"{threadName} - .");
                        //    localContext.Trace($"{threadName} - .");
                        //    localContext.Trace($"{threadName} - .");
                        //}
                        //if ((rounds - i) < 10)
                        //    localContext.Trace($"{threadName} - Contact GET completed in {roundWatch.ElapsedMilliseconds} millisecs");
                    }

                    localContext.Trace($"{threadName} - Total millisecs for {rounds} GETs = {totalWatch.ElapsedMilliseconds}");
                    localContext.Trace($"{threadName} - Average milliseconds for each GET = {totalWatch.ElapsedMilliseconds / rounds}");

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("LoadTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        //private static void AssignTokenForTest(HttpWebRequest httpWebRequest, string manualToken)
        //{
        //    httpWebRequest.Headers["X-CRMPlusToken"] = manualToken;
        //}


        //[Test, Explicit]
        //public void WebApiFlow_1_CreateLeadTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region CreateCustomerLead
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "leads"));
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                string InputJSON = SerializeCustomerInfo(localContext, ValidCustomerInfo_SkapaKonto1);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("CreateCustomerLead results= {0}", result);

        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        [Test, Category("Debug")]
        public void CreateContactRGOLPost()
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
                    CustomerInfo incorrectInfo = new CustomerInfo
                    {
                        Source = 5,
                        FirstName = "Marcus",
                        LastName = "Jönsson",
                        AddressBlock = new CustomerInfoAddressBlock
                        {
                            CO = "",
                            Line1 = "Folkbokföringsadress Saknas",
                            PostalCode = "",
                            City = "",
                            CountryISO = "SE"
                        },
                        Mobile = "0768194219",
                        SocialSecurityNumber = "198510184099",
                        Email = "Marcusjonsson85@hotmail.com",
                        CreditsafeOkSpecified = false,
                        AvlidenSpecified = false,
                        UtvandradSpecified = false,
                        EmailInvalidSpecified = false,
                        isLockedPortalSpecified = false,
                        isAddressEnteredManuallySpecified = false,
                        isAddressInformationCompleteSpecified = false,
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true
                    };

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, incorrectInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();


                    }
                }
                catch (WebException we)
                {
                    if (we.Response == null)
                    {
                        throw we;
                    }

                    HttpWebResponse response = (HttpWebResponse)we.Response;

                    string responseContent;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = streamReader.ReadToEnd();
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }

        [Test]
        public void TokenTest()
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
                    // Fel Guid i URL
                    try
                    {
                        CustomerInfo correctInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "LastName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                            Email = "korrekt@email.com",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{Guid.NewGuid().ToString()}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, Guid.Parse(correctInfo.Guid));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";
                        //httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                        //{
                        //    return true;
                        //};

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, correctInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        if (we.Response == null)
                        {
                            throw we;
                        }

                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }


                    // No token.
                    try
                    {
                        CustomerInfo correctInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "LastName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                            Email = "korrekt@email.com",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{correctInfo.Guid}");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, correctInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
                    }

                    // Fel guid i token.
                    try
                    {
                        CustomerInfo correctInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "LastName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                            Email = "korrekt@email.com",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{correctInfo.Guid}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, Guid.Parse(Guid.NewGuid().ToString()));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, correctInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }

                    // Fel Guid i payload
                    try
                    {
                        CustomerInfo correctInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "LastName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                            Email = "korrekt@email.com",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{correctInfo.Guid}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, Guid.Parse(correctInfo.Guid));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        correctInfo.Guid = Guid.NewGuid().ToString();

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, correctInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }

                    // Rätt token och URL. Skall gå igenom
                    string correctInfoEmail = "korrekt@email.com";
                    Guid correctInfoGuid = Guid.NewGuid();
                    try
                    {
                        CustomerInfo correctInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "LastName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Mobile = "08123456789",
                            Guid = correctInfoGuid.ToString(),
                            Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                            Email = correctInfoEmail,
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{correctInfo.Guid}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, Guid.Parse(correctInfo.Guid));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, correctInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.OK, httpResponse1.StatusCode);

                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        string responseContent;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            responseContent = streamReader.ReadToEnd();
                        }

                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                        NUnit.Framework.Assert.AreEqual(string.Format("# Kunde inte hitta en Kund med {0} och {1}", correctInfoEmail, correctInfoGuid), responseContent);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void IncorrectInformationTest()
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
                    try
                    {
                        CustomerInfo badInfo = new CustomerInfo
                        {
                            FirstName = "Name1",
                            LastName = "other name",
                            AddressBlock = null,
                            Source = (int)Generated.ed_informationsource.SkapaMittKonto,
                            Email = "korrekt@email.com",
                            SocialSecurityNumber = "20160101",
                            SwedishSocialSecurityNumber = false,
                            SwedishSocialSecurityNumberSpecified = false
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, badInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }

                    try
                    {
                        CustomerInfo badinfo2 = new CustomerInfo
                        {
                            FirstName = "realName",
                            LastName = "MoreRealName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "SE"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.SkapaMittKonto,
                            Email = "bad.email",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, badinfo2);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }

                    try
                    {
                        CustomerInfo badinfo3 = new CustomerInfo
                        {
                            FirstName = "realName",
                            LastName = "MoreRealName",
                            AddressBlock = new CustomerInfoAddressBlock
                            {
                                Line1 = "gatan 1",
                                PostalCode = "12345",
                                City = "stad",
                                CountryISO = "Sverige"
                            },
                            Guid = Guid.NewGuid().ToString(),
                            Source = (int)Generated.ed_informationsource.SkapaMittKonto,
                            Email = "real@email.com",
                            SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now),
                            SwedishSocialSecurityNumber = true,
                            SwedishSocialSecurityNumberSpecified = true
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, badinfo3);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                        throw new Exception("Request should have resulted in a WebException");
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        NUnit.Framework.Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void AllowDuplicateBirthDates()
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
                    CustomerInfo goodInfo1 = new CustomerInfo
                    {
                        FirstName = "realName",
                        LastName = "MoreRealName",
                        AddressBlock = new CustomerInfoAddressBlock
                        {
                            Line1 = "gatan 1",
                            PostalCode = "12345",
                            City = "stad",
                            CountryISO = "SE"
                        },

                        Source = (int)Generated.ed_informationsource.SkapaMittKonto,
                        Email = "real@email.com",
                        SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now).Substring(0, 8),
                        SwedishSocialSecurityNumber = false,
                        SwedishSocialSecurityNumberSpecified = true
                    };

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, goodInfo1);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                    NUnit.Framework.Assert.AreEqual(HttpStatusCode.Created, httpResponse1.StatusCode);


                    httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, goodInfo1);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse2 = (HttpWebResponse)httpWebRequest.GetResponse();
                    NUnit.Framework.Assert.AreEqual(HttpStatusCode.Created, httpResponse2.StatusCode);

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.Trace($"AllowDuplicateBirthDates results={result}");
                        throw new Exception($"AllowDuplicateBirthDates returned a response with httpCode: {response.StatusCode}, and content: {result}");
                    }
                }
            }
        }

        [Test, Category("Debug")]
        public void UpdateContactInputJSON()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                DateTime.Now.CompareTo(null);

                try
                {
                    try
                    {
                        string inputJSONString = "{\"Source\":10,\"FirstName\":\"Hello\",\"LastName\":\"World\",\"AddressBlock\":{\"Line1\":\"Address 13\",\"PostalCode\":\"12345\",\"City\":\"Malmö\",\"CountryISO\":\"SE\"},\"Mobile\":\"987654321\",\"SocialSecurityNumber\":\"19710906\",\"SwedishSocialSecurityNumber\":false,\"SwedishSocialSecurityNumberSpecified\":true,\"Email\":\"maboi1@mailinator.com\",\"CreditsafeOkSpecified\":false,\"AvlidenSpecified\":false,\"UtvandradSpecified\":false,\"EmailInvalidSpecified\":false,\"Guid\":\"343eb58a-d2cd-e711-80ef-005056b64d75\",\"isAddressEnteredManuallySpecified\":false,\"isAddressInformationCompleteSpecified\":false}";

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //AssignTokenForTest(httpWebRequest, token);

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = inputJSONString;
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Uppdatera mittkonto results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContactInputJSON returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [Test, Category("Debug")]
        public void LeadCreationInputJSON()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                DateTime.Now.CompareTo(null);

                try
                {
                    try
                    {
                        string inputJSONString = "{\"addressBlock\":{\"city\":\"Staden\",\"countryIso\":\"DK\",\"line1\":\"Gatan 1\",\"postalCode\":\"JUL88:1231\"},\"email\":\"Olle@Bjoern.636190346511718736.com\",\"firstName\":\"Olle\",\"lastName\":\"Björn\",\"socialSecurityNumber\":\"19421106\",\"source\":0,\"statusFlag\":\"CreditSafeOk\",\"swedishSocialSecurityNumber\":false}";
                        //string token = "eyJhbGciOiJSUzUxMiIsIng1dSI6IkJnSUFBQUNrQUFCU1UwRXhBQWdBQUFFQUFRQ3huWUdEYXFqcEx4OEs0OTlKRXhEaWlXbG05aUo3N0t4Y21TS3JpK1oxbzJTQ1o3YnhTMGRnZDVwaklFcUhhQlZDelFGZU5FZlI0VmtBNTcxbFR5NFo3bzhHQlU0OEF4Y245V2J4TWdFa0hzb2xESmNLS3BKa2FKVlZwTE9qVmpWZVRkbkJCcmRvRU1OQ2swZWlwNXoyZTc2aTE0aEh3Q2pmZmkzWjFqUVNCQTBuSVgvUWdrK2FFVXJHNTBBZ1FDSUxPOHg1UmV0THBoblBMSmZubnA0cmlGSytvcGZYeGF1ZmhUenVhRmFFNVQ3NVI4cE1ScjNWYTRoclRnRFFOblZmV21pcGlYVEVwTUtTeFRFMUFDWnlxSkxVSjlzVjVRZk1lQVRuSkZOYVRqbjZMQ2d6UnJtY3JJenZ1K2xuOWFlcHZuM011cE9pbHJ1RDg0cExHc0dpIn0.eyJpc3MiOiJTdE1rbCIsImV4cCI6MTQ4MTcxNTkzNCwiVXNlck5hbWUiOiJCaWdCZW5ndCJ9.kN1Y0JHegyAfm7lqob0-tKDp_pRwgKYXN2C4T1SkUsHa4hfTxzSVST3s2S34wYMVCA-XzvtFrc7gkUNz6Enf39oliYAjEs3g2HmbDjHOCD-gefMGMI3agH_1KBzg_6DbZ6B920apHCGinHeDEo05-YLACkjtPcqOW-YRVS5X5DI9ehsILao96DORFohl-9KojO9y_qUiXgFxujLf_C0L8yw0GwMi_389sseKxtd5E9je6TV_Mtes347_pnPh6Y_Dp0X4X5IInZ_5gomM9iTB0z-wswYMQYzxskXgi9xog1uJkrUokze1aPvS8Ek57VedAjiOf1bTmijlkv82GLr2tQ";

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //AssignTokenForTest(httpWebRequest, token);

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = inputJSONString;
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateCustomerLead results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("LeadCreationInputJSON returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test]
        public void LeadUpdate()
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
                    CustomerInfo customer = ValidCustomerInfo_FullTest(WebApiTestHelper.GetUnitTestID());
                    try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //AssignTokenForTest(httpWebRequest, token);

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateLead results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("LeadUpdate, call 1, returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }

                    customer.Mobile = "1234567890";

                    try
                    {
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //AssignTokenForTest(httpWebRequest, token);

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("CreateLead results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("LeadUpdate, call 2, returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //[Test, Explicit]
        //public void PersonnummerHopp()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            CustomerInfo customer = ValidCustomerInfo_FullTest(CustomerUtility.GetUnitTestID());
        //            ContactEntity firstContact = new ContactEntity(localContext, customer);
        //            firstContact.Id = localContext.OrganizationService.Create(firstContact);

        //            ContactEntity secondContact = new ContactEntity
        //            {
        //                FirstName = "firstName",
        //                LastName = "lastName",
        //                Telephone2 = "123456789",
        //                EMailAddress1 = $"{customer.Email}x",
        //                Address1_Line2 = "Line2",
        //                Address1_PostalCode = "12345",
        //                Address1_City = "gStad",
        //                cgi_socialsecuritynumber = "199108070054",
        //                ed_Address1_Country = CountryEntity.GetEntityRefForCountryCode(localContext, "SE"),
        //                ed_HasSwedishSocialSecurityNumber = true,
        //                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto
        //            };
        //            secondContact.Id = localContext.OrganizationService.Create(secondContact);

        //            ContactEntity alterSecond = new ContactEntity
        //            {
        //                ContactId = secondContact.ContactId,
        //                cgi_socialsecuritynumber = null,
        //                ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.AdmAndraKund
        //            };
        //            localContext.OrganizationService.Update(alterSecond);

        //            secondContact.cgi_socialsecuritynumber = firstContact.cgi_socialsecuritynumber;
        //            secondContact.ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto;
        //            customer = secondContact.ToCustomerInfo(localContext);

        //            try
        //            {
        //                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{secondContact.Id.ToString()}");
        //                this.CreateTokenForTest(localContext, httpWebRequest, secondContact.Id);
        //                httpWebRequest.ContentType = "application/json";
        //                httpWebRequest.Method = "PUT";

        //                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //                {
        //                    string InputJSON = SerializeCustomerInfo(localContext, customer);
        //                    streamWriter.Write(InputJSON);
        //                    streamWriter.Flush();
        //                    streamWriter.Close();
        //                }
        //                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //                {
        //                    // Result is
        //                    var result = streamReader.ReadToEnd();
        //                    localContext.TracingService.Trace("CreateLead results={0}", result);

        //                    // Validate result, must have DataValid
        //                    CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
        //                    Guid customerId;
        //                    if (!Guid.TryParse(info.Guid, out customerId))
        //                    {
        //                        throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
        //                    }
        //                    NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
        //                    NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
        //                }
        //            }
        //            catch (WebException we)
        //            {
        //                HttpWebResponse response = (HttpWebResponse)we.Response;
        //                using (var streamReader = new StreamReader(response.GetResponseStream()))
        //                {
        //                    var result = streamReader.ReadToEnd();
        //                    localContext.TracingService.Trace("LeadUpdate, call 1, returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                    throw new Exception(result, we);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        [Test, Explicit]
        public void ContactUpdateMklIdDiagnosis()
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

                string socSecNr = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now.AddHours(-new Random().Next(876000)));

                ContactEntity existingContact = new ContactEntity
                {
                    ed_MklId = "anMklId",
                    ed_InformationSource = Generated.ed_informationsource.Kampanj,
                    FirstName = "MKlIdDiagnosis",
                    LastName = "SoDiagnose",
                    EMailAddress1 = $"mkl.diagnose{socSecNr}@mail.test"
                };
                existingContact.Id = XrmHelper.Create(localContext, existingContact);
                existingContact.ContactId = existingContact.Id;

                ContactEntity existingConflict = new ContactEntity
                {
                    FirstName = existingContact.FirstName + "conflict",
                    LastName = existingContact.LastName + "conflict",
                    cgi_socialsecuritynumber = socSecNr,
                    ed_HasSwedishSocialSecurityNumber = true,
                    ed_InformationSource = Generated.ed_informationsource.Kampanj,
                    EMailAddress2 = $"mkl.diagnose{socSecNr}@mail.test"
                };
                existingConflict.Id = XrmHelper.Create(localContext, existingConflict);
                existingConflict.ContactId = existingConflict.Id;

                ContactEntity updateExistingContact = new ContactEntity
                {
                    ContactId = existingContact.ContactId,
                    Id = existingContact.Id,
                    ed_HasSwedishSocialSecurityNumber = true,
                    cgi_socialsecuritynumber = existingConflict.cgi_socialsecuritynumber,
                    ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto,
                    Telephone2 = "1235467890",
                    Address1_Line2 = "mklLine1",
                    Address1_PostalCode = "12345",
                    Address1_City = "MKLTown",
                    ed_Address1_Country = CountryEntity.GetEntityRefForCountryCode(localContext, "SE")
                };


                try
                {
                    #region ÄndraKunduppgifter
                    {
                        ContactEntity combinedExisting = new ContactEntity();
                        combinedExisting.CombineAttributes(existingContact);
                        combinedExisting.CombineAttributes(updateExistingContact);
                        combinedExisting.Id = (Guid)combinedExisting.ContactId;

                        CustomerInfo updateInfo = combinedExisting.ToCustomerInfo(localContext);

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{updateExistingContact.ContactId.ToString()}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, updateExistingContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, updateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref updateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContact results= {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(updateInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(updateInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(updateInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(updateInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(updateInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(updateInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(updateInfo.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(updateInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(updateInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(updateInfo.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(updateInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(updateInfo.Guid, info.Guid);
                        }
                    }
                    #endregion

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [Test, Category("Regression")]
        public void UpdateOldContactWithCustomerInfoData()
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
                    // Get old contact
                    ContactEntity testContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(true),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                 new ConditionExpression(ContactEntity.Fields.ed_InformationSource, ConditionOperator.Null),
                                 new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.NotNull),
                                 new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Skanetrafiken.Crm.Schema.Generated.ContactState.Active),
                                 new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull)
                            }
                        });

                    if (testContact != null)
                    {
                        string testInstanceName = WebApiTestHelper.GetUnitTestID();
                        CustomerInfo rgolInfo = GetDynamicRgolInfo(testInstanceName);

                        // TEST!
                        rgolInfo.Email = testContact.EMailAddress2;
                        rgolInfo.SocialSecurityNumber = testContact.cgi_socialsecuritynumber;

                        #region RGOL_Update_Customer_with_RGOL_Data
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                        // Test is valid if we can update the customer

                        #endregion
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
                        localContext.TracingService.Trace("RgolTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
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
        public void PassTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string testInstanceName = CustomerUtility.GetUnitTestID();

                try
                {
                    // create a valid passContact
                    CustomerInfo passInfo = GetDynamicPassInfo(testInstanceName);

                    //string json = TestDataHelper.ReadTestFile(@"C:\TFS\Skåne\Main\Skanetrafiken.WebApi\UnitTest.Skanetrafiken.WebApi\TestMessages\Pass_duplicateContactCreated.json");
                    //passInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(json);
                    //passInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(json);

                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1);
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, passInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest1.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref passInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(passInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(passInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(passInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(passInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(passInfo.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }
                    // create a minimal passContact
                    CustomerInfo minimalPassInfo = new CustomerInfo
                    {
                        FirstName = "PassFirstNameTest",
                        LastName = $"PasslastName{testInstanceName}Test",
                        Email = $"pass.TestMail{testInstanceName}@mail.pass"
                    };

                    var httpWebRequestMin = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestMin);
                    httpWebRequestMin.ContentType = "application/json";
                    httpWebRequestMin.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestMin.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, passInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseMin = (HttpWebResponse)httpWebRequestMin.GetResponse();

                    using (var streamReader = new StreamReader(httpResponseMin.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref passInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest Minimal Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(passInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(passInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(passInfo.LastName, info.LastName);
                    }

                    // attempt to usa an an invalid source 
                    try
                    {
                        CustomerInfo invalidPassSourceInfo = new CustomerInfo
                        {
                            FirstName = "PassFirstNameTest",
                            LastName = $"PasslastName{testInstanceName}Test",
                            Email = $"pass.TestMail{testInstanceName}@mail.pass",
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto
                        };

                        var httpWebRequestInvSource = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestInvSource);
                        httpWebRequestInvSource.ContentType = "application/json";
                        httpWebRequestInvSource.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequestInvSource.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, invalidPassSourceInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponseInvSource = (HttpWebResponse)httpWebRequestInvSource.GetResponse();

                        throw new Exception("Attempt to create an invalid PASS-Contact did not return a 'Bad Request'-code");
                    }
                    catch (WebException we1)
                    {
                        HttpWebResponse response = (HttpWebResponse)we1.Response;
                        if (response == null)
                            throw we1;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            if (response.StatusCode != HttpStatusCode.BadRequest)
                                throw new Exception("Attempt to create an invalid PASS-Contact (source) did not return a 'Bad Request'-code");
                        }
                    }
                    // attempt to create an invalid one (Last Name Missing)
                    try
                    {
                        CustomerInfo invalidPassInfo = new CustomerInfo
                        {
                            FirstName = "PassFirstNameTest",
                            Email = $"pass.TestMail{testInstanceName}@mail.pass",
                            Source = (int)Generated.ed_informationsource.PASS
                        };

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, invalidPassInfo);
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                        throw new Exception("Attempt to create an invalid PASS-Contact did not return a 'Bad Request'-code");
                    }
                    catch (WebException we1)
                    {
                        HttpWebResponse response = (HttpWebResponse)we1.Response;
                        if (response == null)
                            throw we1;

                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            if (response.StatusCode != HttpStatusCode.BadRequest)
                                throw new Exception("Attempt to create an invalid PASS-Contact did not return a 'Bad Request'-code");
                        }
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
                        localContext.TracingService.Trace("PassTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception) { throw; }
            }
        }

        [Test, Explicit]
        public void ChangeEmailTest()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string testInstanceName = CustomerUtility.GetUnitTestID();

                try
                {
                    ContactEntity retrievedContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, "19910201")
                            }
                        });

                    retrievedContact.ContactId = retrievedContact.Id;

                    #region RetrieveContact - Check if isEmailChangeInProgress = false
                    {
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{retrievedContact.ContactId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, retrievedContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("RetrieveContact done, returned: {0}", result);
                        }
                    }
                    #endregion

                    string newEmail = "";
                    #region ChangeEmail
                    {
                        CustomerInfo customerInfoChangeEmail = new CustomerInfo()
                        {
                            NewEmail = retrievedContact.EMailAddress1 + DateTime.Now.Date.DayOfYear + DateTime.Now.Date.Second,
                            Guid = retrievedContact.Id.ToString(),
                            Source = (int)Generated.ed_informationsource.BytEpost,
                            FirstName = retrievedContact.FirstName,
                            LastName = retrievedContact.LastName,
                            SocialSecurityNumber = "19910201",
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                City = "Stockholm",
                                CountryISO = "SE",
                                Line1 = "Alströmergatan 2",
                                PostalCode = "11249"
                            },
                            Email = retrievedContact.EMailAddress1,
                        };

                        newEmail = customerInfoChangeEmail.NewEmail;

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{retrievedContact.ContactId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, retrievedContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(customerInfoChangeEmail, Formatting.None);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailTest done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        }
                    }
                    #endregion

                    #region RetrieveContact - Check if isEmailChangeInProgress = true
                    {
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{retrievedContact.ContactId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, retrievedContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("RetrieveContactTest done, returned: {0}", result);
                        }
                    }
                    #endregion

                    string linkGuid;
                    #region GetLatestLinkGuid Contact
                    {
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{newEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValidateEmail
                    {
                        CustomerInfo constumerValidateEmail = new CustomerInfo()
                        {
                            Guid = retrievedContact.Id.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "143562786",
                        };

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, retrievedContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = JsonConvert.SerializeObject(constumerValidateEmail, Formatting.None);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ValidateEmailTest done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        }
                    }
                    #endregion

                    #region RetrieveContact - Check if isEmailChangeInProgress = true
                    {
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{retrievedContact.ContactId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, retrievedContact.ContactId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("RetrieveContactTest done, returned: {0}", result);
                            ContactInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInfo>(result);
                            if (info.isEmailChangeInProgress == true)
                            {
                                throw new Exception("isEmailChangeInProgress should be false since it has been validated in the ValidateEmail.");
                            }
                        }
                    }
                    #endregion
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("ChangeEmailTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception) { throw; }
            }
        }


        [Test, Category("Regression")]
        public void MergeAfterEmailChangeTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                string testInstanceName = CustomerUtility.GetUnitTestID();

                try
                {
                    CustomerInfo unvalidatedEmail1WithPhoneNumber = GenerateChangeEmailMergeTestInfo(testInstanceName);
                    unvalidatedEmail1WithPhoneNumber.SocialSecurityNumber = null;
                    unvalidatedEmail1WithPhoneNumber.Email += "1";
                    string guid1 = null;
                    // Create unvalidated Contact with Email1 - have TelephoneNUmbers
                    var httpWebRequestChangeEmail = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestChangeEmail);
                    httpWebRequestChangeEmail.ContentType = "application/json";
                    httpWebRequestChangeEmail.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestChangeEmail.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, unvalidatedEmail1WithPhoneNumber);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponseChangeEmail = (HttpWebResponse)httpWebRequestChangeEmail.GetResponse();
                    using (var streamReader = new StreamReader(httpResponseChangeEmail.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref unvalidatedEmail1WithPhoneNumber);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("unvalidatedEmail1WithAddress results={0}", result);

                        // Validera att resultatet av den nya kunden är samma guid som angeNamn-kontakten
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

                        // Save Guid To check that is inactivated
                        guid1 = info.Guid;
                    }

                    // Create Validated Contact with Email2 - No phoneNumbers
                    CustomerInfo validatedEmail2WithoutPhoneNumber = GenerateChangeEmailMergeTestInfo(testInstanceName);
                    validatedEmail2WithoutPhoneNumber.FirstName = unvalidatedEmail1WithPhoneNumber.FirstName;
                    validatedEmail2WithoutPhoneNumber.LastName = unvalidatedEmail1WithPhoneNumber.LastName;
                    validatedEmail2WithoutPhoneNumber.Telephone = null;
                    validatedEmail2WithoutPhoneNumber.Mobile = null;

                    string guid2 = null;
                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("\nSkapaKontoLead:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}leads/", WebApiTestHelper.WebApiRootEndpoint));
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            validatedEmail2WithoutPhoneNumber.Source = (int)Generated.ed_informationsource.SkapaMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validatedEmail2WithoutPhoneNumber);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        try
                        {
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                WrapperController.FormatCustomerInfo(ref validatedEmail2WithoutPhoneNumber);

                                // Result is 
                                var result = streamReader.ReadToEnd();
                                localContext.TracingService.Trace("CreateCustomerLead results={0}", result);

                                // Validate result, must have DataValid
                                CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Email, info.Email);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.FirstName, info.FirstName);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.LastName, info.LastName);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Telephone, info.Telephone);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Mobile, info.Mobile);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.SocialSecurityNumber, info.SocialSecurityNumber);
                                NUnit.Framework.Assert.NotNull(info.AddressBlock);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CO, info.AddressBlock.CO);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.Line1, info.AddressBlock.Line1);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.City, info.AddressBlock.City);
                                NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                                guid2 = info.Guid;
                            }

                            throw new Exception("Should go wrong because phone is missing");
                        }
                        catch (WebException we)
                        {
                            HttpWebResponse response = (HttpWebResponse)we.Response;
                            if (response == null)
                                throw we;

                            using (var streamReader = new StreamReader(response.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                                localContext.TracingService.Trace("CreateCustomerLead returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                                if (result != "Fält som saknas:<BR><BR>Mobil\r\n<BR>")
                                    throw new Exception(result, we);
                            }
                        }
                        catch (Exception) { throw; }
                    }
                    #endregion

                    string linkGuid = null;
                    #region Hämta LatestLinkGuid Lead
                    {
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Lead:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/GetLatestLinkGuid/{validatedEmail2WithoutPhoneNumber.Email}/";
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
                        localContext.TracingService.Trace("\nValideraEmailSkapaKund:");
                        CustomerInfo verifyEmailInfo = new CustomerInfo
                        {
                            Guid = guid2,
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "jättemycketMKlId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, new Guid(guid2));
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

                            WrapperController.FormatCustomerInfo(ref validatedEmail2WithoutPhoneNumber);

                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            validatedEmail2WithoutPhoneNumber.Guid = info.Guid;
                        }
                    }
                    #endregion

                    // Change to Email1 - Return value should have PhoneNumbers and Guid2

                    #region ÄndraEpost
                    {
                        localContext.TracingService.Trace("\nÄndra epost:");
                        validatedEmail2WithoutPhoneNumber.NewEmail = unvalidatedEmail1WithPhoneNumber.Email;
                        validatedEmail2WithoutPhoneNumber.Source = (int)Generated.ed_informationsource.BytEpost;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{validatedEmail2WithoutPhoneNumber.Guid}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, new Guid(validatedEmail2WithoutPhoneNumber.Guid));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validatedEmail2WithoutPhoneNumber);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref validatedEmail2WithoutPhoneNumber);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

                            // Validate result, must have NoConflictingEntity
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.NewEmail, info.NewEmail);
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Contact
                    {
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Contact:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{validatedEmail2WithoutPhoneNumber.NewEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValideraÄndradEpost
                    {
                        localContext.TracingService.Trace("\nValidera ändrad epost:");
                        CustomerInfo validateChangedEmail = new CustomerInfo
                        {
                            Guid = validatedEmail2WithoutPhoneNumber.Guid,
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "ÄnnumerMKLId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, new Guid(validatedEmail2WithoutPhoneNumber.Guid));
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validateChangedEmail);

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

                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.NewEmail, info.Email);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(unvalidatedEmail1WithPhoneNumber.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(unvalidatedEmail1WithPhoneNumber.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(validatedEmail2WithoutPhoneNumber.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            validatedEmail2WithoutPhoneNumber.Guid = info.Guid;
                        }
                    }
                    #endregion



                    // Get contact1 and fail

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("MergeAfterEmailChangeTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception) { throw; }
            }
        }

        #region Not used

        //        [Test, Category("Run Always"), Category("Pure WebApi")]
        //        public void AngeNamnTest()
        //        {
        //            // Connect to the Organization service. 
        //            // The using statement assures that the service proxy will be properly disposed.
        //            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //            {
        //                // This statement is required to enable early-bound type support.
        //                _serviceProxy.EnableProxyTypes();

        //                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //                string testInstanceName = "2" + CustomerUtility.GetUnitTestID().Substring(1);

        //                try
        //                {
        //#if !PRODUKTION
        //                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName);

        //                    /** Skapa ange namn kund
        //                        Validerad epost (epost1)
        //                        Namn: Ange Namn
        //                        Inget personnummer
        //                        Inget MKLID
        //                    **/

        //                    ContactEntity angeNamnContact = new ContactEntity
        //                    {
        //                        FirstName = "Ange",
        //                        LastName = "Namn",
        //                        ed_MklId = "AngeNamnId1",
        //                        EMailAddress1 = customer.Email,
        //                        ed_InformationSource = Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatLaddaKort
        //                    };
        //                    CustomerInfo angeNamnContactInfo = angeNamnContact.ToCustomerInfo(localContext);

        //                    var httpWebRequestAngeNamn = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/AngeNamnDebugPost");
        //                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestAngeNamn);
        //                    httpWebRequestAngeNamn.ContentType = "application/json";
        //                    httpWebRequestAngeNamn.Method = "POST";

        //                    using (var streamWriter = new StreamWriter(httpWebRequestAngeNamn.GetRequestStream()))
        //                    {
        //                        angeNamnContactInfo.Source = (int)Generated.ed_informationsource.OinloggatLaddaKort;
        //                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, angeNamnContactInfo);

        //                        streamWriter.Write(InputJSON);
        //                        streamWriter.Flush();
        //                        streamWriter.Close();
        //                    }

        //                    var httpResponseAngeNamn = (HttpWebResponse)httpWebRequestAngeNamn.GetResponse();
        //                    using (var streamReader = new StreamReader(httpResponseAngeNamn.GetResponseStream()))
        //                    {
        //                        WrapperController.FormatCustomerInfo(ref angeNamnContactInfo);

        //                        // Result is 
        //                        var result = streamReader.ReadToEnd();
        //                        localContext.TracingService.Trace("OinloggatKöp Existerande Kund results={0}", result);

        //                        // Validera att resultatet av den nya kunden är samma guid som angeNamn-kontakten
        //                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);

        //                        angeNamnContact.Id = new Guid(info.Guid);
        //                        angeNamnContact.ContactId = new Guid(info.Guid);
        //                        angeNamnContact.ed_MklId = info.MklId;
        //                    }

        //                    /**
        //                        "Genomför" ett oinloggat köp
        //                    **/
        //                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
        //                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
        //                    httpWebRequest.ContentType = "application/json";
        //                    httpWebRequest.Method = "POST";

        //                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //                    {
        //                        customer.Source = (int)Generated.ed_informationsource.OinloggatKop;
        //                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

        //                        streamWriter.Write(InputJSON);
        //                        streamWriter.Flush();
        //                        streamWriter.Close();
        //                    }

        //                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //                    {
        //                        WrapperController.FormatCustomerInfo(ref customer);

        //                        // Result is 
        //                        var result = streamReader.ReadToEnd();
        //                        localContext.TracingService.Trace("OinloggatKöp Existerande Kund results={0}", result);

        //                        // Validera att resultatet av den nya kunden är samma guid som angeNamn-kontakten
        //                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
        //                        NUnit.Framework.Assert.AreEqual(angeNamnContact.EMailAddress1, info.Email);
        //                        NUnit.Framework.Assert.AreEqual(angeNamnContact.ed_MklId, info.MklId);
        //                        NUnit.Framework.Assert.AreEqual(angeNamnContact.ContactId.ToString(), info.Guid);
        //                        NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
        //                        NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
        //                        NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);

        //                        NUnit.Framework.Assert.NotNull(info.AddressBlock);
        //                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
        //                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
        //                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
        //                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

        //                    }
        //#endif

        //                }
        //                catch (WebException we)
        //                {
        //                    HttpWebResponse response = (HttpWebResponse)we.Response;
        //                    if (response == null)
        //                        throw we;

        //                    using (var streamReader = new StreamReader(response.GetResponseStream()))
        //                    {
        //                        var result = streamReader.ReadToEnd();
        //                        localContext.TracingService.Trace("PassTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                        throw new Exception(result, we);
        //                    }
        //                }
        //                catch (Exception) { throw; }
        //            }
        //        }
        //        */
        #endregion

        [Test, Category("Regression")]
        public void RgolTest()
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
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();


                    #region RGOL DEBUG!
                    //CustomerInfo rgolInfoDebug = new CustomerInfo
                    //{
                    //    FirstName = "Testfirstname",
                    //    LastName = "Testlastname:20170713.0829",
                    //    SocialSecurityNumber = "201707130827",
                    //    SwedishSocialSecurityNumber = true,
                    //    SwedishSocialSecurityNumberSpecified = true,
                    //    Telephone = "031201707130829",
                    //    Mobile = "0735201707130829",
                    //    Email = "test20170713.0829@test.test",
                    //    Source = (int)CustomerUtility.Source.RGOL,
                    //    AddressBlock = new CustomerInfoAddressBlock
                    //    {
                    //        Line1 = "Testvägen 20170713.0829",
                    //        PostalCode = "12345",
                    //        City = "By20170713.0829",
                    //        CountryISO = "SE"
                    //    },

                    //};
                    //var httpWebRequestDebug = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    //this.CreateTokenForTest(localContext, httpWebRequestDebug);
                    //httpWebRequestDebug.ContentType = "application/json";
                    //httpWebRequestDebug.Method = "POST";

                    //using (var streamWriter = new StreamWriter(httpWebRequestDebug.GetRequestStream()))
                    //{
                    //    string InputJSON = SerializeCustomerInfo(localContext, rgolInfoDebug);
                    //    streamWriter.Write(InputJSON);
                    //    streamWriter.Flush();
                    //    streamWriter.Close();
                    //}
                    //HttpWebResponse httpResponseDebug = (HttpWebResponse)httpWebRequestDebug.GetResponse();

                    //using (var streamReader = new StreamReader(httpResponseDebug.GetResponseStream()))
                    //{
                    //    WrapperController.FormatCustomerInfo(ref rgolInfoDebug);

                    //    var result = streamReader.ReadToEnd();
                    //    localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                    //    CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.Email, info.Email);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.FirstName, info.FirstName);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.LastName, info.LastName);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.Telephone, info.Telephone);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.SocialSecurityNumber, info.SocialSecurityNumber);

                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.AddressBlock.CO, info.AddressBlock.CO);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.AddressBlock.Line1, info.AddressBlock.Line1);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.AddressBlock.City, info.AddressBlock.City);
                    //    NUnit.Framework.Assert.AreEqual(rgolInfoDebug.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    //}
                    #endregion

                    #region RGOL Test Both email and PersNr
                    CustomerInfo rgolInfo = GetDynamicRgolInfo(testInstanceName);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }

                    rgolInfo.Telephone = "987654321";
                    rgolInfo.FirstName = "NewRgolName";
                    rgolInfo.AddressBlock.Line1 = "NewStreet";

                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest2);
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest existing Contact results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }
                    #endregion


                    #region RGOL Test only email
                    CustomerInfo rgolInfoMailOnly = GetDynamicRgolInfo(testInstanceName);
                    rgolInfoMailOnly.SocialSecurityNumber = null;
                    var httpWebRequestMailOnly = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestMailOnly);
                    httpWebRequestMailOnly.ContentType = "application/json";
                    httpWebRequestMailOnly.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestMailOnly.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfoMailOnly);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseMailOnly1 = (HttpWebResponse)httpWebRequestMailOnly.GetResponse();

                    using (var streamReader = new StreamReader(httpResponseMailOnly1.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfoMailOnly);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }

                    rgolInfoMailOnly.Telephone = "987654321";
                    rgolInfoMailOnly.FirstName = "NewRgolName";
                    rgolInfoMailOnly.AddressBlock.Line1 = "NewStreet";

                    var httpWebRequestMailOnly2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestMailOnly2);
                    httpWebRequestMailOnly2.ContentType = "application/json";
                    httpWebRequestMailOnly2.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestMailOnly2.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfoMailOnly);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseMailOnly2 = (HttpWebResponse)httpWebRequestMailOnly2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponseMailOnly2.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfoMailOnly);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest existing Contact results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfoMailOnly.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }
                    #endregion


                    #region RGOL Test only SocSec
                    CustomerInfo rgolInfoSocSecOnly = GetDynamicRgolInfo(testInstanceName);
                    rgolInfoSocSecOnly.Email = null;
                    var httpWebRequestSocSecOnly = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestSocSecOnly);
                    httpWebRequestSocSecOnly.ContentType = "application/json";
                    httpWebRequestSocSecOnly.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestSocSecOnly.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfoSocSecOnly);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseSocSecOnly1 = (HttpWebResponse)httpWebRequestSocSecOnly.GetResponse();

                    using (var streamReader = new StreamReader(httpResponseSocSecOnly1.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfoSocSecOnly);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }

                    rgolInfoSocSecOnly.Telephone = "987654321";
                    rgolInfoSocSecOnly.FirstName = "NewRgolName";
                    rgolInfoSocSecOnly.AddressBlock.Line1 = "NewStreet";

                    var httpWebRequestSocSecOnly2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequestSocSecOnly2);
                    httpWebRequestSocSecOnly2.ContentType = "application/json";
                    httpWebRequestSocSecOnly2.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequestSocSecOnly2.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, rgolInfoSocSecOnly);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponseSocSecOnly2 = (HttpWebResponse)httpWebRequestSocSecOnly2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponseSocSecOnly2.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref rgolInfoSocSecOnly);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest existing Contact results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(rgolInfoSocSecOnly.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }
                    #endregion


                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("RgolTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Debug")]
        public void RgolCreateUpdateContactInputJSON()
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
                    try
                    {
                        // Make sure contact is prepared
                        ContactEntity upCont = new ContactEntity()
                        {
                            Id = new Guid("6F57DE27-CCF9-E511-80DF-005056903A38"),
                            ed_PrivateCustomerContact = null,
                        };
                        localContext.OrganizationService.Update(upCont);


                        string inputJSONString = "{'Source':5,'FirstName':'Kawa','LastName':'Fkri','AddressBlock':{ 'CO':'','Line1':'Veterinärgatan 5','PostalCode':'21235','City':'Malmö','CountryISO':'SE'},'Mobile':'0704412285','SocialSecurityNumber':'196005011819','SwedishSocialSecurityNumber':true,'SwedishSocialSecurityNumberSpecified':true,'Email':'kawa@ownit.nu','CreditsafeOkSpecified':false,'AvlidenSpecified':false,'UtvandradSpecified':false,'EmailInvalidSpecified':false,'isLockedPortalSpecified':false,'isAddressEnteredManuallySpecified':false,'isAddressInformationCompleteSpecified':false}";

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        //AssignTokenForTest(httpWebRequest, token);

                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = inputJSONString;
                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Uppdatera mittkonto results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            Guid customerId;
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    catch (WebException we)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContactInputJSON returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string GetSKAKortJsonString(Guid? contactGuid, bool withoutCardNumber, bool withoutCardName, bool withoutContactId, out string cardNumberOut, out string cardNameOut)
        {
            string json = "{ ";

            string cardNumber = "";
            string cardName = "";
            string contactId = "";

            #region Random Card Number
            if (withoutCardNumber != true)
            {
                Random rnd = new Random();
                int randomCardNumber = rnd.Next(100000, 999999);
                cardNumber = randomCardNumber.ToString();
                json += "'cardNumber': '" + cardNumber + "', ";

                cardNumberOut = cardNumber;
            }
            else
            {
                cardNumberOut = "";
            }
            #endregion

            #region Card Name
            if (withoutCardName != true)
            {
                cardName = "Skånetrafikenskortet_" + DateTime.Now + "_" + DateTime.Now.Hour + " " + DateTime.Now.Minute;
                json += "'cardName': '" + cardNumber + "', ";

                cardNameOut = cardName;
            }
            else
            {
                cardNameOut = "";
            }
            #endregion

            #region Contact
            if (withoutContactId != true)
            {
                contactId = contactGuid.ToString();
                json += "'contactId': '" + contactId + "' ";
            }
            #endregion

            //json = "{ 'cardNumber': '" + cardNumber + "', 'cardName': '" + cardName + "', 'contactId': '" + contactId + "' }";

            return json;
        }

        [Test, Category("Debug")]
        public void FullFlowSKAkort()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();


                QueryExpression contactQuery = new QueryExpression()
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(false),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                        }
                    }
                };

                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

                string cardNumber = "";
                string cardName = "";
                string skaKortJsonMessage = GetSKAKortJsonString(contact.Id, false, false, false, out cardNumber, out cardName);
                
                // 2020-04-07 Register Skå Card against Contact
                #region Register SKÅ Card
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SkaKort");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(skaKortJsonMessage);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("SkaKort POST results={0}", result);
                    }

                    QueryExpression skaKortQuery = new QueryExpression()
                    {
                        EntityName = SkaKortEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SkaKortEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_SKAkortState.Active),
                                new ConditionExpression(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, cardNumber),
                                new ConditionExpression(SkaKortEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.Id)
                            }
                        }
                    };

                    SkaKortEntity skaKort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, skaKortQuery);

                    if(skaKort == null)
                    {
                        throw new Exception("SKA kort should have been created. Found no card...");
                    }
                }
                #endregion

                // 2020-04-07 Unregister Skå Card against Contact
                #region "Unregister" SKÅ Card
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SkaKort");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "PUT";
                    
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(skaKortJsonMessage);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("SkaKort PUT results={0}", result);
                    }

                    QueryExpression skaKortQuery = new QueryExpression()
                    {
                        EntityName = SkaKortEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SkaKortEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_SKAkortState.Active),
                                new ConditionExpression(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, cardNumber)
                            }
                        }
                    };

                    SkaKortEntity skaKort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, skaKortQuery);

                    if (skaKort == null || skaKort.ed_Contact != null)
                    {
                        throw new Exception("Found no SKA kort or Contact still exists on card..");
                    }
                }
                #endregion

                // 2020-04-07 Unregister Skå Card against Contact
                #region Delete SKÅ Card
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}SkaKort");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "DELETE";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(skaKortJsonMessage);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("SkaKort DELETE results={0}", result);
                    }

                    QueryExpression skaKortQuery = new QueryExpression()
                    {
                        EntityName = SkaKortEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(SkaKortEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_SKAkortState.Inactive),
                                new ConditionExpression(SkaKortEntity.Fields.ed_CardNumber, ConditionOperator.Equal, cardNumber)
                            }
                        }
                    };

                    SkaKortEntity skaKort = XrmRetrieveHelper.RetrieveFirst<SkaKortEntity>(localContext, skaKortQuery);

                    if (skaKort == null)
                    {
                        throw new Exception("Found no inactive SKA kort..");
                    }
                }
                #endregion
                
            }
        }


        [Test]
        public void ValidateEmailAddressWithStrangeCharacters()
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
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    string[] strangeEmails = {
                        "pia.elisabeth.j@gmail.com",
                        "utbemifri1@skola.Malmö.se",
                        "jeanette-åkesson@hotmail.com",
                        "vd-nivåmilkfish07@hotmail.com",
                        "saba76@löive.com",
                        "ingrid-sjölin@hotmail.com",
                        "sonja@allbäckpaint.com",
                        "joonathan-93@hotmail.com"
                    };

                    #region ValidateStrangeCharacters
                    foreach (string strangeEmail in strangeEmails)
                    {
                        CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                        // Add chars to validate.
                        customer.Email = strangeEmail;

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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }

                    #endregion

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("lead create returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        [Test, Explicit, Category("DebugStarCall")]
        public void LeadDebugStarCall()
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
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();

                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}leads/71382605-cfe6-e611-80f8-00505690700f";
                    var httpWebRequest1 = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest1, new Guid("71382605-cfe6-e611-80f8-00505690700f"));
                    httpWebRequest1.ContentType = "application/json";
                    httpWebRequest1.Method = "PUT";

                    using (var streamWriter = new StreamWriter(httpWebRequest1.GetRequestStream()))
                    {
                        string InputJSON = "{'CampaignCode':'avzkxn','CampaignId':'CMP-01018-V0K8','IsCampaignActive':true,'IsCampaignActiveSpecified':true,'CampaignDiscountPercentSpecified':false,'CampaignProducts':[],'Source':7,'FirstName':'Olle','LastName':'Björn','AddressBlock':{'Line1':'Gatan 1','PostalCode':'12345','City':'Staden','CountryISO':'SE'},'Mobile':'0123456789','SocialSecurityNumber':'19140122','SwedishSocialSecurityNumber':false,'SwedishSocialSecurityNumberSpecified':true,'Email':'ak@mailinator.com','CreditsafeOkSpecified':false,'AvlidenSpecified':false,'UtvandradSpecified':false,'EmailInvalidSpecified':false,'Guid':'71382605-cfe6-e611-80f8-00505690700f','isAddressEnteredManually':true,'isAddressEnteredManuallySpecified':true,'isAddressInformationComplete':true,'isAddressInformationCompleteSpecified':true}";

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest1.GetResponse();

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
        public void PassTest2()
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
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    CustomerInfo passInfo = GetDynamicPassInfo(testInstanceName);

                    #region PASS Test
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, passInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref passInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest New Info results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(passInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(passInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(passInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(passInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(passInfo.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }

                    passInfo.Telephone = "987654321";
                    passInfo.FirstName = "NewPassName";
                    passInfo.AddressBlock.Line1 = "NewStreet";

                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest2);
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, passInfo);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    HttpWebResponse httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse2.GetResponseStream()))
                    {
                        WrapperController.FormatCustomerInfo(ref passInfo);

                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest existing Contact results= {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(passInfo.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(passInfo.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(passInfo.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(passInfo.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(passInfo.SocialSecurityNumber, info.SocialSecurityNumber);

                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(passInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                    }
                    #endregion
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                        throw we;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("PassTest returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
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
        public void GetCustomerInfoBasedOnDiscountCode()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();




            }
        }

        [Test, Category("Regression"), Category("Pure WebApi")]
        public void PingTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();


                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}ping/");
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // Result is 
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("CreateCustomerLead results={0}", result);
                }
            }
        }

        [Test, Category("Regression"), Category("Pure WebApi")]
        public void PingConnectionTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();


                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}pingconnection/");
                WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // Result is 
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("CreateCustomerLead results={0}", result);
                }
            }
        }

        [Test, Explicit]
        public void FullFlowTest_ForEmailFlagCheck()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    // 
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;
                    //string personnummerToUse = "201612158558";
                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = customer.AddressBlock.CO,
                            Line1 = "UpdatedLine1",
                            PostalCode = customer.AddressBlock.PostalCode,
                            City = "UpdatedCity",
                            CountryISO = customer.AddressBlock.CountryISO
                        },
                        SocialSecurityNumber = customer.SocialSecurityNumber,
                        SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber,
                        SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Telephone = customer.Telephone,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.UppdateraMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContact results= {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Guid, info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra epost:");
                        customerUpdateInfo.NewEmail = updatedEmailAdress;
                        customerUpdateInfo.Source = (int)Generated.ed_informationsource.BytEpost;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

                            // Validate result, must have NoConflictingEntity
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.NewEmail);
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("ÄndraEpost reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact2 done, returned: {0}", result);
                            ContactInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInfo>(result);
                            //CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            //NUnit.Framework.Assert.AreEqual(info.isEmailChangeInProgress, true);
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Contact
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Contact:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{customerUpdateInfo.NewEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValideraÄndradEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nValidera ändrad epost:");
                        CustomerInfo validateChangedEmail = new CustomerInfo
                        {
                            Guid = customerId.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "ÄnnumerMKLId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validateChangedEmail);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp Existerande Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp Existerande Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            customerUpdateInfo.Email = customerUpdateInfo.NewEmail;
                            customerUpdateInfo.NewEmail = null;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"19{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatKöp ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatLaddaKort ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatLaddaKort ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"18{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatLaddaKort;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatLaddaKort ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region Multiple NotifyMKL
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nNotifyMKL:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo1 = new NotificationInfo
                            {
                                Guid = customerId.ToString(),
                                ContactType = (int)Generated.ed_notifymkl_ed_method.Mail,
                                NotificationType = (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword,
                                //SendTo = "telefonnummer eller mailadress"
                            };
                            NotificationInfo notificationInfo2 = new NotificationInfo
                            {
                                Guid = customerId.ToString(),
                                ContactType = (int)Generated.ed_notifymkl_ed_method.Mail,
                                NotificationType = (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword,
                                //SendTo = "telefonnummer eller mailadress"
                            };
                            NotificationInfo[] infos = new NotificationInfo[] { notificationInfo1, notificationInfo2 };
                            string InputJSON = WebApiTestHelper.SerializeNotificationInfos(localContext, infos);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
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
        public void CreateMultipleCompanyRolesSameContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                AccountInfo accountInfoInitial = new AccountInfo();
                AccountInfo accountInfoSecond = new AccountInfo();
                CustomerInfo customerInfoInitial = new CustomerInfo();
                string customerInfoInitialId = "";
                string customerInfoSecondId = "";

                // 2020-02-03 - Test for FTG
                #region Create Business Account
                {
                    AccountInfo accountInfo = GenerateValidAccount();
                    accountInfoInitial = accountInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nAccount POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}accounts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(accountInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Account POST results={0}", result);
                    }
                }
                #endregion

                // 2020-02-03 - Test for FTG
                #region Create Business Contact
                {
                    CustomerInfo customerInfo = GenerateValidBusinessContact(accountInfoInitial);
                    customerInfoInitial = customerInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nContact POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        customerInfoInitialId = result.Substring(1, result.Length - 2);
                        localContext.TracingService.Trace("Contact POST results={0}", result);
                    }
                }
                #endregion

                ContactEntity newlyCreatedFTGContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(customerInfoInitialId),
                    new ColumnSet(
                        ContactEntity.Fields.FirstName,
                        ContactEntity.Fields.LastName,
                        ContactEntity.Fields.Telephone2,
                        ContactEntity.Fields.EMailAddress1,
                        ContactEntity.Fields.EMailAddress2,
                        ContactEntity.Fields.cgi_socialsecuritynumber,
                        ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                        ContactEntity.Fields.ed_BusinessContact
                        ));

                if (newlyCreatedFTGContact == null)
                    throw new Exception("Contact not created...");
                if (newlyCreatedFTGContact.ed_BusinessContact == null || newlyCreatedFTGContact.ed_BusinessContact.Value == false)
                    throw new Exception("Contact not marked as Business Contact");
                if (newlyCreatedFTGContact.cgi_socialsecuritynumber != null)
                    throw new Exception("Social Security Number should be null or empty");
                if (newlyCreatedFTGContact.ed_SocialSecurityNumberBlock == null)
                    throw new Exception("Social Security Number Block should not be null");

                QueryExpression query = new QueryExpression()
                {
                    EntityName = CompanyRoleEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, newlyCreatedFTGContact.Id)
                        }
                    }
                };

                List<CompanyRoleEntity> companyRoleLst = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, query);

                if (companyRoleLst == null || companyRoleLst.Count < 1)
                    throw new Exception("CompanyRole should have been created");

                foreach (var role in companyRoleLst)
                {
                    if (String.IsNullOrEmpty(role.ed_FirstName))
                        throw new Exception("Role firstname is null");
                    if (String.IsNullOrEmpty(role.ed_LastName))
                        throw new Exception("Role lastname is null");
                    if (String.IsNullOrEmpty(role.ed_EmailAddress))
                        throw new Exception("Role email is null");
                    if (String.IsNullOrEmpty(role.ed_Telephone))
                        throw new Exception("Role telephone is null");
                    if (role.ed_Account == null)
                        throw new Exception("Role account is null");
                }

                // 2020-02-03 - Test for FTG
                #region Create Business Contact (second)
                {
                    CustomerInfo customerInfo = customerInfoInitial;
                    //customerInfoInitial = customerInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nContact POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    try
                    {
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            customerInfoInitialId = result.Substring(1, result.Length - 2);
                            localContext.TracingService.Trace("Contact POST results={0}", result);
                        }

                        throw new Exception("Should not come here. Conlicting Roles.");
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
                            localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);

                            if(!result.Contains("Contact already have the role"))
                                throw new Exception("Incorrect error message. The Contact already has a role connected to same company with same role.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                #endregion

                // 2020-02-03 - Test for FTG
                #region Create Business Account (second)
                {
                    AccountInfo accountInfo = GenerateValidAccount();
                    accountInfoSecond = accountInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nAccount POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}accounts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(accountInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Account POST results={0}", result);
                    }
                }
                #endregion

                // 2020-02-03 - Test for FTG
                #region Create Business Contact (third)
                {
                    CustomerInfo customerInfo = customerInfoInitial;
                    customerInfo.CompanyRole[0].PortalId = accountInfoSecond.PortalId;
                    //customerInfoInitial = customerInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nContact POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    try
                    {
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            customerInfoSecondId = result.Substring(1, result.Length - 2);
                            localContext.TracingService.Trace("Contact POST results={0}", result);
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
                            localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                            
                            throw new Exception(result, we);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    ContactEntity newlyCreatedFTGContactSecond = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(customerInfoSecondId),
                    new ColumnSet(
                        ContactEntity.Fields.FirstName,
                        ContactEntity.Fields.LastName,
                        ContactEntity.Fields.Telephone2,
                        ContactEntity.Fields.EMailAddress1,
                        ContactEntity.Fields.EMailAddress2,
                        ContactEntity.Fields.cgi_socialsecuritynumber,
                        ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                        ContactEntity.Fields.ed_BusinessContact
                        ));

                    if (newlyCreatedFTGContactSecond == null)
                        throw new Exception("Contact not created...");
                    if (newlyCreatedFTGContactSecond.ed_BusinessContact == null || newlyCreatedFTGContactSecond.ed_BusinessContact.Value == false)
                        throw new Exception("Contact not marked as Business Contact");
                    if (newlyCreatedFTGContactSecond.cgi_socialsecuritynumber != null)
                        throw new Exception("Social Security Number should be null or empty");
                    if (newlyCreatedFTGContactSecond.ed_SocialSecurityNumberBlock == null)
                        throw new Exception("Social Security Number Block should not be null");

                    QueryExpression querySecond = new QueryExpression()
                    {
                        EntityName = CompanyRoleEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(true),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, newlyCreatedFTGContact.Id)
                            }
                        }
                    };

                    List<CompanyRoleEntity> companyRoleLstSecond = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, querySecond);

                    if (companyRoleLstSecond == null || companyRoleLstSecond.Count < 2)
                        throw new Exception("CompanyRole2 should have been created and there should be one existing");

                    foreach (var role in companyRoleLstSecond)
                    {
                        if (String.IsNullOrEmpty(role.ed_FirstName))
                            throw new Exception("Role firstname is null");
                        if (String.IsNullOrEmpty(role.ed_LastName))
                            throw new Exception("Role lastname is null");
                        if (String.IsNullOrEmpty(role.ed_EmailAddress))
                            throw new Exception("Role email is null");
                        if (String.IsNullOrEmpty(role.ed_Telephone))
                            throw new Exception("Role telephone is null");
                        if (role.ed_Account == null)
                            throw new Exception("Role account is null");
                    }
                }
                #endregion

            }
        }

        [Test, Category("Regression")]
        public void CreateSingleCompanyContact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                AccountInfo accountInfoInitial = new AccountInfo();
                CustomerInfo customerInfoInitial = new CustomerInfo();
                string customerInfoInitialId = "";

                // 2020-02-03 - Test for FTG
                #region Create Business Account
                {
                    AccountInfo accountInfo = GenerateValidAccount();
                    accountInfoInitial = accountInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nAccount POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}accounts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(accountInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Account POST results={0}", result);
                    }
                }
                #endregion

                // 2020-02-03 - Test for FTG
                #region Create Business Contact
                {
                    CustomerInfo customerInfo = GenerateValidBusinessContact(accountInfoInitial);
                    customerInfoInitial = customerInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nContact POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        customerInfoInitialId = result.Substring(1, result.Length - 2);
                        localContext.TracingService.Trace("Contact POST results={0}", result);
                    }
                }
                #endregion

                ContactEntity newlyCreatedFTGContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(customerInfoInitialId),
                    new ColumnSet(
                        ContactEntity.Fields.FirstName,
                        ContactEntity.Fields.LastName,
                        ContactEntity.Fields.Telephone2,
                        ContactEntity.Fields.EMailAddress1,
                        ContactEntity.Fields.EMailAddress2,
                        ContactEntity.Fields.cgi_socialsecuritynumber,
                        ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                        ContactEntity.Fields.ed_BusinessContact
                        ));

                if (newlyCreatedFTGContact == null)
                    throw new Exception("Contact not created...");
                if (newlyCreatedFTGContact.ed_BusinessContact == null || newlyCreatedFTGContact.ed_BusinessContact.Value == false)
                    throw new Exception("Contact not marked as Business Contact");
                if (newlyCreatedFTGContact.cgi_socialsecuritynumber != null)
                    throw new Exception("Social Security Number should be null or empty");
                if (newlyCreatedFTGContact.ed_SocialSecurityNumberBlock == null)
                    throw new Exception("Social Security Number Block should not be null");


                QueryExpression query = new QueryExpression()
                {
                    EntityName = CompanyRoleEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, newlyCreatedFTGContact.Id)
                        }
                    }
                };

                List<CompanyRoleEntity> companyRoleLst = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, query);

                if (companyRoleLst == null || companyRoleLst.Count < 1)
                    throw new Exception("CompanyRole should have been created");

                foreach (var role in companyRoleLst)
                {
                    if (String.IsNullOrEmpty(role.ed_FirstName))
                        throw new Exception("Role firstname is null");
                    if (String.IsNullOrEmpty(role.ed_LastName))
                        throw new Exception("Role lastname is null");
                    if (String.IsNullOrEmpty(role.ed_EmailAddress))
                        throw new Exception("Role email is null");
                    if (String.IsNullOrEmpty(role.ed_Telephone))
                        throw new Exception("Role telephone is null");
                    if (role.ed_Account == null)
                        throw new Exception("Role account is null");
                }
            }
        }

        [Test, Category("Regression")]
        public void CreateSingleCompanyContactAndCreateSinglePrivateContactMatchingSSNAndEMail()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                AccountInfo accountInfoInitial = new AccountInfo();
                CustomerInfo customerInfoInitial = new CustomerInfo();
                string customerInfoInitialId = "";

                // 2020-02-19 - Marcus Stenswed
                // This test currently creates two different Contacts. They should match.
                //throw new Exception("ERROR!!!");

                // 2020-02-03 - Test for FTG
                #region Create Business Account
                {
                    AccountInfo accountInfo = GenerateValidAccount();
                    accountInfoInitial = accountInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nAccount POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}accounts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(accountInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Account POST results={0}", result);
                    }
                }
                #endregion

                // 2020-02-03 - Test for FTG
                #region Create Business Contact
                {
                    CustomerInfo customerInfo = GenerateValidBusinessContact(accountInfoInitial);
                    customerInfoInitial = customerInfo;

                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nContact POST:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);

                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
                        customerInfoInitialId = result.Substring(1, result.Length - 2);
                        localContext.TracingService.Trace("Contact POST results={0}", result);
                    }
                }
                #endregion

                // Check data from SeKund
                #region Verifications
                ContactEntity newlyCreatedFTGContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(customerInfoInitialId),
                    new ColumnSet(
                        ContactEntity.Fields.FirstName,
                        ContactEntity.Fields.LastName,
                        ContactEntity.Fields.Telephone2,
                        ContactEntity.Fields.EMailAddress1,
                        ContactEntity.Fields.EMailAddress2,
                        ContactEntity.Fields.cgi_socialsecuritynumber,
                        ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                        ContactEntity.Fields.ed_BusinessContact
                        ));

                if (newlyCreatedFTGContact == null)
                    throw new Exception("Contact not created...");
                if (newlyCreatedFTGContact.ed_BusinessContact == null || newlyCreatedFTGContact.ed_BusinessContact.Value == false)
                    throw new Exception("Contact not marked as Business Contact");
                if (newlyCreatedFTGContact.cgi_socialsecuritynumber != null)
                    throw new Exception("Social Security Number should be null or empty");
                if (newlyCreatedFTGContact.ed_SocialSecurityNumberBlock == null)
                    throw new Exception("Social Security Number Block should not be null");


                QueryExpression query = new QueryExpression()
                {
                    EntityName = CompanyRoleEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, newlyCreatedFTGContact.Id)
                        }
                    }
                };

                List<CompanyRoleEntity> companyRoleLst = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, query);

                if (companyRoleLst == null || companyRoleLst.Count < 1)
                    throw new Exception("CompanyRole should have been created");

                foreach (var role in companyRoleLst)
                {
                    if (String.IsNullOrEmpty(role.ed_FirstName))
                        throw new Exception("Role firstname is null");
                    if (String.IsNullOrEmpty(role.ed_LastName))
                        throw new Exception("Role lastname is null");
                    if (String.IsNullOrEmpty(role.ed_EmailAddress))
                        throw new Exception("Role email is null");
                    if (String.IsNullOrEmpty(role.ed_Telephone))
                        throw new Exception("Role telephone is null");
                    if (role.ed_Account == null)
                        throw new Exception("Role account is null");
                }
                #endregion

                #region Create Private Contact

                string contactBusineesMobile = customerInfoInitial.CompanyRole[0].Telephone;
                string contactPrivateMobile = "";

                string testInstanceName = WebApiTestHelper.GetUnitTestID();
                Guid customerId = Guid.Empty;
                string linkGuid = null;
                //string personnummerToUse = "201612158558";
                CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                CustomerInfo customerUpdateInfo = new CustomerInfo()
                {
                    FirstName = customerInfoInitial.FirstName,
                    LastName = customerInfoInitial.LastName,
                    AddressBlock = new CustomerInfoAddressBlock()
                    {
                        CO = customer.AddressBlock.CO,
                        Line1 = "UpdatedLine1",
                        PostalCode = customer.AddressBlock.PostalCode,
                        City = "UpdatedCity",
                        CountryISO = customer.AddressBlock.CountryISO
                    },
                    SocialSecurityNumber = customerInfoInitial.SocialSecurityNumber,
                    SwedishSocialSecurityNumber = customerInfoInitial.SwedishSocialSecurityNumber,
                    SwedishSocialSecurityNumberSpecified = customerInfoInitial.SwedishSocialSecurityNumberSpecified,
                    Email = customerInfoInitial.Email,
                    Mobile = customerInfoInitial.Mobile,
                    Telephone = customer.Telephone,
                    Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                };

                contactPrivateMobile = customer.Mobile;
                customer.Email = customerInfoInitial.CompanyRole[0].Email;
                customer.FirstName = customerInfoInitial.FirstName;
                customer.LastName = customerInfoInitial.LastName;

                string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                #region SkapaKontoLead
                {
                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
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
                        NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                        NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                        NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                        NUnit.Framework.Assert.NotNull(info.AddressBlock);
                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                        NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
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
                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
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

                    try
                    {


                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
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
                            localContext.TracingService.Trace("CreateSingleCompanyContactAnd... returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                            throw new Exception(result, we);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                #endregion

                #region HämtaKunduppgifter
                {
                    localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                    _partTimer.Restart();
                    localContext.TracingService.Trace("\nHämta kunduppgifter:");
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                        CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                        NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                        NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                        NUnit.Framework.Assert.AreNotEqual(customer.Telephone, info.Telephone);
                        NUnit.Framework.Assert.AreNotEqual(customer.Mobile, info.Mobile);
                        NUnit.Framework.Assert.AreNotEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                    }
                }
                #endregion


                #endregion

            }
        }

        [Test, Category("Regression"), Category("Pure WebApi")]
        public void FullFlowTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    // 
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;
                    //string personnummerToUse = "201612158558";
                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = customer.AddressBlock.CO,
                            Line1 = "UpdatedLine1",
                            PostalCode = customer.AddressBlock.PostalCode,
                            City = "UpdatedCity",
                            CountryISO = customer.AddressBlock.CountryISO
                        },
                        //SocialSecurityNumber = customer.SocialSecurityNumber,
                        //SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber,
                        //SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Telephone = customer.Telephone,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.UppdateraMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContact results= {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Guid, info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra epost:");
                        customerUpdateInfo.NewEmail = updatedEmailAdress;
                        customerUpdateInfo.Source = (int)Generated.ed_informationsource.BytEpost;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

                            // Validate result, must have NoConflictingEntity
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.NewEmail);
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("ÄndraEpost reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Contact
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Contact:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{customerUpdateInfo.NewEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValideraÄndradEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nValidera ändrad epost:");
                        CustomerInfo validateChangedEmail = new CustomerInfo
                        {
                            Guid = customerId.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "ÄnnumerMKLId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validateChangedEmail);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp Existerande Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp Existerande Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            customerUpdateInfo.Email = customerUpdateInfo.NewEmail;
                            customerUpdateInfo.NewEmail = null;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"19{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatKöp ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatLaddaKort ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatLaddaKort ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"18{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatLaddaKort;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatLaddaKort ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region Multiple NotifyMKL
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nNotifyMKL:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}notifications");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";


                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            NotificationInfo notificationInfo1 = new NotificationInfo
                            {
                                Guid = customerId.ToString(),
                                ContactType = (int)Generated.ed_notifymkl_ed_method.Mail,
                                NotificationType = (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword,
                                //SendTo = "telefonnummer eller mailadress",
                                TimeStamp = DateTime.Now
                            };
                            NotificationInfo notificationInfo2 = new NotificationInfo
                            {
                                Guid = customerId.ToString(),
                                ContactType = (int)Generated.ed_notifymkl_ed_method.Mail,
                                NotificationType = (int)Generated.ed_notifymkl_ed_notificationtype.ChangePassword,
                                //SendTo = "telefonnummer eller mailadress",
                                TimeStamp = DateTime.Now
                            };
                            NotificationInfo[] infos = new NotificationInfo[] { notificationInfo1, notificationInfo2 };
                            string InputJSON = WebApiTestHelper.SerializeNotificationInfos(localContext, infos);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ResetPasswordEmailSent results={0}", result);
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Regression"), Category("Pure WebApi")]
        public void FullFlowNoNameTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    // 
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;
                    //string personnummerToUse = "201612158558";
                    CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    customer.FirstName = null;
                    customer.LastName = null;
                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        FirstName = null,
                        LastName = null,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = customer.AddressBlock.CO,
                            Line1 = "UpdatedLine1",
                            PostalCode = customer.AddressBlock.PostalCode,
                            City = "UpdatedCity",
                            CountryISO = customer.AddressBlock.CountryISO
                        },
                        SocialSecurityNumber = customer.SocialSecurityNumber,
                        SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber,
                        SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Telephone = customer.Telephone,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.UppdateraMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContact results= {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CO, info.AddressBlock.CO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.City, info.AddressBlock.City);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Guid, info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra epost:");
                        customerUpdateInfo.NewEmail = updatedEmailAdress;
                        customerUpdateInfo.Source = (int)Generated.ed_informationsource.BytEpost;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

                            // Validate result, must have NoConflictingEntity
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.NewEmail);
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("ÄndraEpost reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Contact
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Contact:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{customerUpdateInfo.NewEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValideraÄndradEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nValidera ändrad epost:");
                        CustomerInfo validateChangedEmail = new CustomerInfo
                        {
                            Guid = customerId.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "ÄnnumerMKLId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validateChangedEmail);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp Existerande Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp Existerande Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            customerUpdateInfo.Email = customerUpdateInfo.NewEmail;
                            customerUpdateInfo.NewEmail = null;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"19{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatKöp ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatLaddaKort ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatLaddaKort ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"18{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatLaddaKort;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatLaddaKort ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [Test, Category("Regression"), Category("Pure WebApi")]
        public void FullFlowJustEmailAndMobileTest()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch _totalTimer = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch _partTimer = new System.Diagnostics.Stopwatch();
                _totalTimer.Restart();
                _partTimer.Restart();

                try
                {
                    // 
                    string testInstanceName = WebApiTestHelper.GetUnitTestID();
                    Guid customerId = Guid.Empty;
                    string linkGuid = null;
                    //string personnummerToUse = "201612158558";
                    //CustomerInfo customer = ValidCustomerInfo_FullTest(testInstanceName/*, personnummerToUse*/);
                    CustomerInfo customer = new CustomerInfo()
                    {
                        Email = GenerateValidEmail(),
                        Mobile = "0735" + testInstanceName.Replace(".", ""),
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
                        SwedishSocialSecurityNumberSpecified = false
                    };
                    customer.FirstName = null;
                    customer.LastName = null;
                    CustomerInfo customerUpdateInfo = new CustomerInfo()
                    {
                        SwedishSocialSecurityNumberSpecified = customer.SwedishSocialSecurityNumberSpecified,
                        Email = customer.Email,
                        Mobile = customer.Mobile,
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto
                    };
                    string updatedEmailAdress = string.Format("{0}{1}", customer.Email, "Update");

                    #region SkapaKontoLead
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            //NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
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
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            //NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("Validera nytt konto reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region HämtaKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("GetContact done, returned: {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
                            //NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
                            //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            //NUnit.Framework.Assert.AreEqual(customerId.ToString(), info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraKunduppgifter
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra kunduppgifter:");
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.UppdateraMittKonto;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("UpdateContact results= {0}", result);
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.FirstName, info.FirstName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.LastName, info.LastName);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Telephone, info.Telephone);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Mobile, info.Mobile);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.SocialSecurityNumber, info.SocialSecurityNumber);
                            //NUnit.Framework.Assert.NotNull(info.AddressBlock);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CO, info.AddressBlock.CO);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.Line1, info.AddressBlock.Line1);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.City, info.AddressBlock.City);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.AddressBlock.CountryISO, info.AddressBlock.CountryISO);
                            //NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Guid, info.Guid);
                        }
                    }
                    #endregion

                    #region ÄndraEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nÄndra epost:");
                        customerUpdateInfo.NewEmail = updatedEmailAdress;
                        customerUpdateInfo.Source = (int)Generated.ed_informationsource.BytEpost;
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/{customerId.ToString()}");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Guid = customerId.ToString();
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerUpdateInfo);

                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

                            // Validate result, must have NoConflictingEntity
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.NewEmail);
                            if (!Guid.TryParse(info.Guid, out customerId))
                            {
                                throw new Exception("ÄndraEpost reurnerade inte ett giltigt Guid.");
                            }
                        }
                    }
                    #endregion

                    #region Hämta LatestLinkGuid Contact
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nHämta LatestLinkGuid Contact:");

                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/GetLatestLinkGuid/{customerUpdateInfo.NewEmail}/";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "GET";

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("Hämta LatestLinkGuid Contact done, returned: {0}", result);
                            linkGuid = (JsonConvert.DeserializeObject<CrmPlusControl.GuidsPlaceholder>(result)).LinkId;
                        }
                    }
                    #endregion

                    #region ValideraÄndradEpost
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nValidera ändrad epost:");
                        CustomerInfo validateChangedEmail = new CustomerInfo
                        {
                            Guid = customerId.ToString(),
                            Source = (int)Generated.ed_informationsource.LoggaInMittKonto,
                            MklId = "ÄnnumerMKLId"
                        };
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}contacts/{linkGuid}";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest, customerId);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "PUT";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, validateChangedEmail);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.NewEmail, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp Existerande Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp Existerande Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customerUpdateInfo.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            customerUpdateInfo.Email = customerUpdateInfo.NewEmail;
                            customerUpdateInfo.NewEmail = null;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerUpdateInfo);

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
                            NUnit.Framework.Assert.AreEqual(customerUpdateInfo.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatKöp ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatKöp ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"19{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatKop;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatKöp ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
                    #endregion

                    #region OinloggatLaddaKort ny Kund
                    {
                        localContext.TracingService.Trace("TotalTime elapsed: {0}. Since last reset: {1}", _totalTimer.ElapsedMilliseconds, _partTimer.ElapsedMilliseconds);
                        _partTimer.Restart();
                        localContext.TracingService.Trace("\nOinloggatLaddaKort ny Kund:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            customer = ValidCustomerInfo_FullTest($"18{testInstanceName.Substring(2)}");
                            customer.Source = (int)Generated.ed_informationsource.OinloggatLaddaKort;
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            // Result is 
                            var result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace("OinloggatLaddaKort ny Kund results={0}", result);

                            // Validate result, must have DataValid
                            CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
                            NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
                        }
                    }
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
                        localContext.TracingService.Trace("FullFlow returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(result, we);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //[Test, Explicit]
        //public void WebApiFlow_3_UpdateContactTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region UpdateCustomer
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}/", WebApiTestHelper.WebApiRootEndpoint, "contacts"));
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "PUT";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                string InputJSON = SerializeCustomerInfo(localContext, ValidCustomerInfo_UpdateContactJanJacob);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("UpdateContact results= {0}", result);

        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        //[Test, Explicit]
        //public void WebApiFlow_3_ChangeEmailTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region CreateCustomerLead
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts/21e926e4-31a7-e611-8112-00155d0a6b01");
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "PUT";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {

        //                string InputJSON = SerializeCustomerInfo(localContext, ValidCustomerInfo_ChangeEmailJanJacob);
        //                InputJSON = "{\"Source\":2,\"SwedishSocialSecurityNumber\":false,\"SwedishSocialSecurityNumberSpecified\":true,\"Email\":\"Olle@Bjoern.77799.com\",\"CreditsafeOkSpecified\":false,\"AvlidenSpecified\":false,\"UtvandradSpecified\":false,\"EmailInvalidSpecified\":false,\"NewEmail\":\"MyUpdatedName\",\"Guid\":\"21e926e4-31a7-e611-8112-00155d0a6b01\",\"isAddressEnteredManuallySpecified\":false,\"isAddressInformationCompleteSpecified\":false,\"allowPurchaseSpecified\":false,\"allowAutoloadSpecified\":false}";
        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("ChangeEmailAddress results= {0}", result);

        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        //[Test, Explicit]
        //public void WebApiFlow_2_ValidateEmailTest()
        //{

        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region ValidateEmail
        //            //CustomerInfo validateEmailInfo = new CustomerInfo
        //            //{
        //            //    Guid = "109F946C-6697-E611-810F-00155D0A6B01",
        //            //    NewEmail = ValidCustomerInfo_SkapaKonto1.Email,
        //            //};
        //            CustomerInfo validateEmailInfo = new CustomerInfo
        //            {
        //                Guid = "1236A375-7797-E611-810F-00155D0A6B01",
        //                NewEmail = ValidCustomerInfo_ChangeEMail1.NewEmail,
        //            };

        //            //string url = string.Format(@"{0}{1}/{2}", WebApiTestHelper.WebApiRootEndpoint, "leads", "ValidateEmail");
        //            string url = string.Format(@"{0}{1}/{2}", WebApiTestHelper.WebApiRootEndpoint, "contacts", "ValidateEmail");
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "PUT";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                string InputJSON = SerializeCustomerInfo(localContext, validateEmailInfo);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("ValidateEmail done, returned: {0}", result);
        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        //[Test, Explicit]
        //public void WebApiFlow_3_GetContactTest()
        //{
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
        //        try
        //        {
        //            #region GetContact
        //            //string url = string.Format(@"{0}{1}?contactGuidOrEmail={2}", WebApiTestHelper.WebApiRootEndpoint, "GetContact", ValidCustomerInfo_SkapaKonto1.Email);
        //            string url = string.Format(@"{0}contacts/{1}/", WebApiTestHelper.WebApiRootEndpoint, "email-100003@example.com");
        //            //string url = string.Format(@"{0}{1}?contactGuidOrEmail={2}", WebApiTestHelper.WebApiRootEndpoint, "GetContact", "73ABE25F-58FD-E411-80D8-005056903A38");
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "GET";

        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact done, returned: {0}", result);
        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        //[Test, Explicit]
        //public void WebApiFlow_4_NonLoginCustomerIncidentTest()
        //{

        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region NonLoginCustomerIncident
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "contacts"));
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                DataContractJsonSerializer requestSerializer = new DataContractJsonSerializer(typeof(CustomerInfo));

        //                string InputJSON;
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    requestSerializer.WriteObject(ms, ValidCustomerInfo_NonLoginCustomerIncident1);
        //                    var sr = new StreamReader(ms);
        //                    ms.Position = 0;
        //                    InputJSON = sr.ReadToEnd().Replace("Field\":", "\":").Replace("FieldSpecified\":", "\":");
        //                }
        //                localContext.TracingService.Trace("JSON to Post to NonLoginCustomerIncident: {0}", InputJSON);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("NonLoginCustomerIncident results= {0}", result);

        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        //[Test, Explicit]
        //public void WebApiFlow_4_NonLoginRGOLTest()
        //{

        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        try
        //        {
        //            #region NonLoginCustomerIncident
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", WebApiTestHelper.WebApiRootEndpoint, "contacts"));
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";

        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                DataContractJsonSerializer requestSerializer = new DataContractJsonSerializer(typeof(CustomerInfo));

        //                string InputJSON;
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    requestSerializer.WriteObject(ms, ValidCustomerInfo_NonLoginRGOL1);
        //                    var sr = new StreamReader(ms);
        //                    ms.Position = 0;
        //                    InputJSON = sr.ReadToEnd().Replace("Field\":", "\":").Replace("FieldSpecified\":", "\":");
        //                }
        //                localContext.TracingService.Trace("JSON to Post to NonLoginRGOL: {0}", InputJSON);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("NonLoginRGOL results= {0}", result);

        //            }
        //            #endregion

        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("GetContact returned an exeption httpCode: {0} Content: {1}", response.StatusCode, result);
        //                throw new Exception(result, we);
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        ////}

        //[Test, Explicit]
        //public void WebApiValidateEmailTest()
        //{

        //    #region Test Setup
        //    // Connect to the Organization service. 
        //    // The using statement assures that the service proxy will be properly disposed.
        //    using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
        //    {
        //        // This statement is required to enable early-bound type support.
        //        _serviceProxy.EnableProxyTypes();

        //        Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

        //        string url = string.Format(@"{0}{1}?customerId={2}&entityTypeCode={3}&eMailAddress={4}", WebApiTestHelper.WebApiRootEndpoint, "ValidateEmail", "95FCAD0E-50FD-E411-80D8-005056903A38", ContactEntity.EntityTypeCode, "testaddress@example.com");
        //        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //        httpWebRequest.ContentType = "application/json";
        //        httpWebRequest.Method = "GET";
        //        #endregion

        //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //        {
        //            var result = streamReader.ReadToEnd();
        //            localContext.TracingService.Trace("Done, returned: {0}", result);
        //        }

        //    }
        //}

        [Test, Explicit]
        public void Debug_CreateCustomerLead()
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

                #region Test Code

                //CustomerInfo data = new CustomerInfo()
                //{
                //    Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
                //    FirstName = "Olle",
                //    LastName = "Björn",
                //    AddressBlock = new CustomerInfoAddressBlock()
                //    {
                //        CO = null,
                //        Line1 = "Gatan 1",
                //        PostalCode = "12345",
                //        City = "Staden",
                //        CountryISO = "SE"
                //    },
                //    //Telephone = "123456789",
                //    //Mobile = "1234567890",
                //    SocialSecurityNumber = "19421106",
                //    Email = "Olle@Bjoern.636190346511718736.com",
                //    SwedishSocialSecurityNumber = false,
                //    SwedishSocialSecurityNumberSpecified = true
                //};

                //HttpResponseMessage resp1 = CrmPlusControl.CreateCustomerLead(data);
                //_log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
            }
        }

        //[Test, Explicit]
        //public void Debug_CreateNewLeadContactWithMailAndEmptySSNOExists()
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

        //        #region Test Code
        //        try
        //        {
        //            localContext.TracingService.Trace("\nSkapaKontoLead:");
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}leads/", WebApiTestHelper.WebApiRootEndpoint));
        //            WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
        //            httpWebRequest.ContentType = "application/json";
        //            httpWebRequest.Method = "POST";

        //            CustomerInfo customer = new CustomerInfo()
        //            {
        //                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
        //                FirstName = "firstName",
        //                LastName = "lastName",
        //                AddressBlock = new CustomerInfoAddressBlock()
        //                {
        //                    CO = null,
        //                    Line1 = "Testvägen ",
        //                    PostalCode = "12345",
        //                    City = "By",
        //                    CountryISO = "SE"
        //                },
        //                //Telephone = "031" + testInstanceName.Replace(".", ""),
        //                //Mobile = "0735" + testInstanceName.Replace(".", ""),
        //                SocialSecurityNumber = "19880101",
        //                Email = string.Format("test20170120.0356@test.testx"),
        //                SwedishSocialSecurityNumber = false,
        //                SwedishSocialSecurityNumberSpecified = true,
        //            };


        //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //            {
        //                customer.Source = (int)CustomerUtility.Source.SkapaMittKonto;
        //                string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

        //                streamWriter.Write(InputJSON);
        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }

        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            {
        //                WrapperController.FormatCustomerInfo(ref customer);

        //                // Result is 
        //                var result = streamReader.ReadToEnd();
        //                localContext.TracingService.Trace("CreateCustomerLead results={0}", result);

        //                // Validate result, must have DataValid
        //                //CustomerInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(result);
        //                //NUnit.Framework.Assert.AreEqual(customer.Email, info.Email);
        //                //NUnit.Framework.Assert.AreEqual(customer.FirstName, info.FirstName);
        //                //NUnit.Framework.Assert.AreEqual(customer.LastName, info.LastName);
        //                //NUnit.Framework.Assert.AreEqual(customer.Telephone, info.Telephone);
        //                //NUnit.Framework.Assert.AreEqual(customer.Mobile, info.Mobile);
        //                //NUnit.Framework.Assert.AreEqual(customer.SocialSecurityNumber, info.SocialSecurityNumber);
        //                //NUnit.Framework.Assert.NotNull(info.AddressBlock);
        //                //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CO, info.AddressBlock.CO);
        //                //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.Line1, info.AddressBlock.Line1);
        //                //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.PostalCode, info.AddressBlock.PostalCode);
        //                //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.City, info.AddressBlock.City);
        //                //NUnit.Framework.Assert.AreEqual(customer.AddressBlock.CountryISO, info.AddressBlock.CountryISO);

        //                //if (!Guid.TryParse(info.Guid, out customerId))
        //                //{
        //                //    throw new Exception("Skapa mitt konto reurnerade inte ett giltigt Guid.");
        //                //}
        //            }
        //        }
        //        catch (WebException we)
        //        {
        //            HttpWebResponse response = (HttpWebResponse)we.Response;
        //            string responseContent;

        //            using (var streamReader = new StreamReader(response.GetResponseStream()))
        //            {
        //                responseContent = streamReader.ReadToEnd();
        //            }

        //            // Should never end up here....
        //            throw new Exception(responseContent, we);
        //        }


        //        //CustomerInfo data = new CustomerInfo()
        //        //{
        //        //    Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
        //        //    FirstName = "firstName",
        //        //    LastName = "lastName",
        //        //    AddressBlock = new CustomerInfoAddressBlock()
        //        //    {
        //        //        CO = null,
        //        //        Line1 = "Gatan 1",
        //        //        PostalCode = "12345",
        //        //        City = "Staden",
        //        //        CountryISO = "SE"
        //        //    },
        //        //    //Telephone = "123456789",
        //        //    //Mobile = "1234567890",
        //        //    SocialSecurityNumber = "19421106",
        //        //    Email = "test20170120.0356@test.testx",
        //        //    SwedishSocialSecurityNumber = false,
        //        //    SwedishSocialSecurityNumberSpecified = true
        //        //};

        //        //HttpResponseMessage resp1 = CrmPlusControl.CreateCustomerLead(data);
        //        ////_log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);

        //        #endregion
        //        stopwatch.Stop();
        //        localContext.TracingService.Trace("CanLeadBeCreatedTest time = {0}", stopwatch.Elapsed);
        //    }
        //}

        [Test, Explicit]
        public void CreateAndUpdateValidateLead()
        {

        }

        [Test, Explicit]
        public void CreateContactDuplicateChecks()
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

                #region Test Code
                //try
                //{
                //    //localContext.TracingService.Trace("\nSkapaUppdateraContact:");
                //    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                //    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                //    httpWebRequest.ContentType = "application/json";
                //    httpWebRequest.Method = "POST";

                //    CustomerInfo customer = new CustomerInfo()
                //    {
                //        Source = (int)CustomerUtility.Source.SkapaMittKonto,
                //        FirstName = "Firstnametest",
                //        LastName = "Lastnametest", //Lübeck
                //        AddressBlock = new CustomerInfoAddressBlock()
                //        {
                //            CO = null,
                //            Line1 = "Äsphultsvägen 62",
                //            PostalCode = "11339",
                //            City = "Stockholm",
                //            CountryISO = "SE"
                //        },
                //        //Telephone = "031" + testInstanceName.Replace(".", ""),
                //        //Mobile = "0735" + testInstanceName.Replace(".", ""),
                //        SocialSecurityNumber = "199102013936",
                //        Email = string.Format("validatedemail@emailaddress.com"),
                //        SwedishSocialSecurityNumber = true,
                //        SwedishSocialSecurityNumberSpecified = true,
                //    };


                //    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //    {
                //        customer.Source = (int)CustomerUtility.Source.SkapaMittKonto;
                //        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customer);

                //        streamWriter.Write(InputJSON);
                //        streamWriter.Flush();
                //        streamWriter.Close();
                //    }

                //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        WrapperController.FormatCustomerInfo(ref customer);

                //        // Result is 
                //        var result = streamReader.ReadToEnd();
                //        localContext.TracingService.Trace("CreateCustomerLead results={0}", result);
                //    }
                //}
                //catch (WebException we)
                //{
                //    HttpWebResponse response = (HttpWebResponse)we.Response;
                //    string responseContent;

                //    using (var streamReader = new StreamReader(response.GetResponseStream()))
                //    {
                //        responseContent = streamReader.ReadToEnd();
                //    }

                //    // Should never end up here....
                //    throw new Exception(responseContent, we);
                //}

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("Debug_CreateUpdateContactDuplicateChecks time = {0}", stopwatch.Elapsed);
            }
        }

        [Test]
        public void Debug_CreateUpdateContactDuplicateChecks()
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

                #region Test Code
                try
                {
                    //localContext.TracingService.Trace("\nSkapaUppdateraContact:");
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    CustomerInfo customer = new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,
                        FirstName = "Johanna",
                        LastName = "Karlén-Fräsig", //Lübeck
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            CO = null,
                            Line1 = "Äsphultsvägen 62",
                            PostalCode = "11339",
                            City = "Stockholm",
                            CountryISO = "SE"
                        },
                        //Telephone = "031" + testInstanceName.Replace(".", ""),
                        //Mobile = "0735" + testInstanceName.Replace(".", ""),
                        //SocialSecurityNumber = "197210183989",
                        Email = string.Format("jeanettelubeck72@yahoo.se"),
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                    };


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        customer.Source = (int)Generated.ed_informationsource.OinloggatKundArende;
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
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    string responseContent;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = streamReader.ReadToEnd();
                    }

                    // Should never end up here....
                    throw new Exception(responseContent, we);
                }

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("Debug_CreateUpdateContactDuplicateChecks time = {0}", stopwatch.Elapsed);
            }
        }

        [Test, Explicit]
        public void Debug_CreateUpdateLeadDuplicateChecks()
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

                #region Test Code
                try
                {
                    CustomerInfo goodInfo1 = new CustomerInfo
                    {
                        FirstName = "Anders",
                        LastName = "Brolèn-Anderssön",
                        AddressBlock = new CustomerInfoAddressBlock
                        {
                            Line1 = "Äsphultsvägen 11",
                            PostalCode = "11339",
                            City = "Stockholm",
                            CountryISO = "SE"
                        },

                        Source = (int)Generated.ed_informationsource.OinloggatLaddaKort,
                        Email = "anders.brolen1337@gmail.com",
                        SocialSecurityNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now).Substring(0, 8),
                        SwedishSocialSecurityNumber = false,
                        SwedishSocialSecurityNumberSpecified = true
                    };

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}leads");
                    WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";


                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, goodInfo1);
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();
                    NUnit.Framework.Assert.AreEqual(HttpStatusCode.Created, httpResponse1.StatusCode);
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    string responseContent;

                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = streamReader.ReadToEnd();
                    }

                    // Should never end up here....
                    throw new Exception(responseContent, we);
                }

                #endregion
                stopwatch.Stop();
                localContext.TracingService.Trace("Debug_CreateUpdateLeadDuplicateChecks time = {0}", stopwatch.Elapsed);
            }
        }


        private CustomerInfo GetDynamicRgolInfo(string testInstanceName)
        {
            // Build a valid, unique personnummer
            StringBuilder personnummer = new StringBuilder(testInstanceName.Substring(0, 8));
            string socSecNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.RGOL,
                FirstName = "TestRgolFirstName",
                LastName = "TestRgolName:" + socSecNumber,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "RgolLine1 " + testInstanceName,
                    PostalCode = "12345",
                    City = "RgolVillage" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                //Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = socSecNumber,
                Email = $"RGOL{testInstanceName}@r.gol",
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
            };
        }

        private CustomerInfo GetDynamicPassInfo(string testInstanceName)
        {
            // Build a valid, unique personnummer
            StringBuilder personnummer = new StringBuilder(testInstanceName.Substring(0, 8));
            string socSecNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.PASS,
                FirstName = "TestPassFirstName",
                LastName = "TestPassName:" + socSecNumber,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = null,
                    Line1 = "PassLine1 " + testInstanceName,
                    PostalCode = "12345",
                    City = "PassVillage" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                //Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = socSecNumber,
                Email = $"PASS{testInstanceName}@p.ass",
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
            };
        }

        private CustomerInfo GenerateChangeEmailMergeTestInfo(string testInstanceName)
        {
            string socSecNumber = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);

            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,
                FirstName = "TestMailChangeFirstName",
                LastName = "TestMailChange:" + socSecNumber,
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = "MailChangeCO",
                    Line1 = "MailChangeLine1 " + testInstanceName,
                    PostalCode = "12345",
                    City = "MailChangeVillage" + testInstanceName,
                    CountryISO = "SE"
                },
                Telephone = "031" + testInstanceName.Replace(".", ""),
                Mobile = "0735" + testInstanceName.Replace(".", ""),
                SocialSecurityNumber = socSecNumber,
                Email = $"mailchange{testInstanceName}@mail.change",
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
            };
        }


        #region ValidateCustomerInfoTest Inputs
        #region valid inputs
        string validPersNr1 = "196106160747";

        string validemail1 = "test@test.test";
        string validemail2 = "test234test@testing.t";

        /// <summary>
        /// Create a CustomerInfo object using SkapaMittKonto
        /// </summary>
        /// <param name="testInstanceName"></param>
        /// <param name="personnummer"></param>
        /// <returns></returns>
        public static CustomerInfo ValidCustomerInfo_FullTest(string testInstanceName, string personnummer = null)
        {
            //if (personnummer == null)
            //    // Build a random, valid, unique personnummer
            //    personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
            personnummer = "";
            return new CustomerInfo()
            {
                Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
                FirstName = "TestFirstName",
                LastName = "TestLastName:" + testInstanceName,
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
                Email = string.Format("test{0}@test.test", testInstanceName),
                //SwedishSocialSecurityNumber = true,
                //SwedishSocialSecurityNumberSpecified = true
            };

        }


        private static CustomerInfo ValidCustomerInfo_SkapaKonto1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "123456789",
            Mobile = "1234567890",
            SocialSecurityNumber = "196106160747",
            Email = "test@test.test",
            SwedishSocialSecurityNumber = true,
            SwedishSocialSecurityNumberSpecified = true
        };
        private static CustomerInfo ValidCustomerInfo_UpdateContact1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto,
            FirstName = "TestFirstName2",
            LastName = "TestLastName2",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 504",
                PostalCode = "12345",
                City = "byttby",
                CountryISO = "SE"
            },
            Telephone = "987654321",
            Mobile = "1234567890",
            SocialSecurityNumber = "196106160747",
            Email = "changed@email.test",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_ChangeEMail1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.BytEpost,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "123456789",
            Mobile = "1234567890",
            SocialSecurityNumber = "196106160747",
            Email = "test@test.test",
            NewEmail = "changed@email.test",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_ChangeEmailJanJacob = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.BytEpost,
            FirstName = "Jan Jacob",
            LastName = "test-99906",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Fluffgatan 3",
                PostalCode = "12345",
                City = "Limhamn",
                CountryISO = "SE"
            },
            Telephone = null,
            Mobile = null,
            SocialSecurityNumber = "199502102206",
            Email = "radikalnyemail@cool.swag",
            NewEmail = "radikalnyemail2@cool.swag",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_UpdateContactJanJacob = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto,
            FirstName = "Jan Jacob",
            LastName = "test-99906",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Fluffgatan 3",
                PostalCode = "12345",
                City = "Limhamn",
                CountryISO = "SE"
            },
            Telephone = null,
            Mobile = "0730618110",
            SocialSecurityNumber = "199502102206",
            Email = "radikalnyemail2@cool.swag",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_SkapaKonto1_MinimalInfo = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            SocialSecurityNumber = "196106160747",
            Email = "test@test.test",
            Mobile = "0701233214",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_OinloggatKop_MinimalInfoNotLoggedInPurchase = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Email = "test@test.test",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_NonLoginCustomerIncident1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            Email = "test@test.test",
        };
        private static CustomerInfo TestData = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "Aayman",
            LastName = "test-395573",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Email = "email-389923@example.com",
            SocialSecurityNumber = "192711044962",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_NonLoginRGOL1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.RGOL,
            FirstName = "RGOLTestFirstName",
            LastName = "RGOLTestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Mobile = "0730618110",
            Email = "email-389923@example.com",
            SocialSecurityNumber = "192711044962",
            SwedishSocialSecurityNumber = true
        };
        private static CustomerInfo ValidCustomerInfo_Admskapakund1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.AdmSkapaKund,
            FirstName = "RGOLTestFirstName",
            LastName = "RGOLTestLastName"
        };

        #endregion

        #region invalid inputs
        string invalidPersNr1 = "";
        string invalidPersNr2 = "000101-0000";
        string invalidPersNr3 = "123456789";
        string invalidPersNr4 = "124356789-1234";
        string invalidPersNr5 = "12345678-12345";
        string invalidPersNr6 = "610616-0747";
        string invalidPersNr7 = "6106160747";

        string invalidEmail1 = "test@test.";
        string invalidEmail2 = "test.test.test";
        string invalidEmail3 = "test.test@test";
        string invalidEmail4 = "@test.test";

        private static Skanetrafiken.Crm.CustomerInfo InvalidCustomerInfo_SkapaKonto_PersnumberMissing = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "1234-56789",
            Mobile = "1234-567890",
            Email = "test@test.test"
        };
        private static CustomerInfo InvalidCustomerInfo_SkapaKonto_InvalidMailAddress = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "1234-56789",
            Mobile = "1234-567890",
            SocialSecurityNumber = "000101-0000",
            Email = "test"
        };
        private static CustomerInfo InvalidCustomerInfo_AdmSkapaAndraKund = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.AdmSkapaKund,
            LastName = "lastname"
        };
        #endregion
        #endregion

        #region CanLeadBeCreatedTest_Inputs
        private static CustomerInfo CanLeadBeCreated_Lead1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkapaMittKonto,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "1234-56789",
            Mobile = "1234-567890",
            SocialSecurityNumber = "000101-0000",
            Email = "test@test.test"
        };
        #endregion

        #region CreateLeadTest_Inputs
        private static CustomerInfo CreateLead_Lead1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "1234-56789",
            Mobile = "1234-567890",
            SocialSecurityNumber = "000101-0000",
            Email = "test@test.test"
        };
        #endregion

        #region ContactFromLeadTest_Inputs
        private static CustomerInfo ContactFromLead_input1 = new CustomerInfo()
        {
            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            AddressBlock = new CustomerInfoAddressBlock()
            {
                CO = null,
                Line1 = "Testvägen 404",
                PostalCode = "12345",
                City = "Testby",
                CountryISO = "SE"
            },
            Telephone = "1234-56789",
            Mobile = "1234-567890",
            SocialSecurityNumber = "000101-0000",
            Email = "test@test.test"
        };
        #endregion


        /// <summary>
        /// Create a CustomerInfo object for tests
        /// </summary>
        /// <param name="testInstanceName"></param>
        /// <param name="personnummer"></param>
        /// <returns></returns>
        public static CustomerInfo Generate_ValidCustomerInfoObject(string testInstanceName, string personnummer = null, bool generatePN = true, bool isNewRecord = false, Generated.ed_informationsource? source = null)
        {
            if (source == null)
                source = Generated.ed_informationsource.SkapaMittKonto;

            if (personnummer == null && generatePN == true)
            {
                // Build a random, valid, unique personnummer
                personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
            }
            else if (personnummer == null && generatePN == false)
            {
                personnummer = String.Empty;
            }

            switch (source)
            {
                case Generated.ed_informationsource.SkapaMittKonto:
                    return new CustomerInfo()
                    {
                        Source = (int)source,

                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated
                        //SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        //SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        //SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)

                        FirstName = "TestFirstName", //O
                        //LastName = "TestLastName:" + testInstanceName, //O

                        //Befintlig För och Efternamn
                        //FirstName = "Testfirstname", //O
                        //LastName = "Testlastname:18190710.1158", //O

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };

                case Generated.ed_informationsource.UppdateraMittKonto:
                    return new CustomerInfo()
                    {
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                        //Email = string.Format("test{0}@test.test", testInstanceName), //M
                        Email = "201904055892@unit.test", //M

                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M
                        //Telephone = customer.Telephone, //O

                        //FirstName = string.Format($"UppdateraMittKontoFörnamn {testInstanceName}"), //O
                        //LastName = customer.LastName, //O

                        //SocialSecurityNumber = customer.SocialSecurityNumber, //O
                        //SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber, //O
                        //SwedishSocialSecurityNumberSpecified = false, //O

                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    CO = customer.AddressBlock.CO,
                        //    Line1 = "UpdatedLine1",
                        //    PostalCode = customer.AddressBlock.PostalCode,
                        //    City = "UpdatedCity",
                        //    CountryISO = customer.AddressBlock.CountryISO
                        //}, // O

                    };

                case Generated.ed_informationsource.OinloggatKop:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {

                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,

                            //Existerande
                            FirstName = "Testmailchangefirstname",
                            LastName = "Testmailchange:201909196311",
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                //co = "mailchangeco",
                                Line1 = "Mailchangeline1 19190919.0342",
                                PostalCode = "12345",
                                City = "Mailchangevillage19190919.0342",
                                CountryISO = "se"
                            },
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {

                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,

                            //Ny
                            FirstName = "Test" + personnummer,
                            LastName = "TestMailChange:" + personnummer,
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                //co = "mailchangeco",
                                Line1 = "mailchangeline1 " + testInstanceName,
                                PostalCode = "12345",
                                City = "mailchangevillage" + testInstanceName,
                                CountryISO = "se"
                            },
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }


                case Generated.ed_informationsource.OinloggatLaddaKort:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)source,

                            //Existerande

                            FirstName = "Testmailchangefirstname",
                            LastName = "Testmailchange:201909196311",
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                //co = "mailchangeco",
                                Line1 = "Mailchangeline1 19190919.0342",
                                PostalCode = "12345",
                                City = "Mailchangevillage19190919.0342",
                                CountryISO = "se"
                            },
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)source,

                            //Ny
                            FirstName = "Test" + personnummer,
                            LastName = "TestMailChange:" + personnummer,
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                //co = "mailchangeco",
                                Line1 = "mailchangeline1 " + testInstanceName,
                                PostalCode = "12345",
                                City = "mailchangevillage" + testInstanceName,
                                CountryISO = "se"
                            },
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }



                case Generated.ed_informationsource.RGOL:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "RGOLTestFirst" + testInstanceName,
                        LastName = "RGOLTestLast" + testInstanceName,
                        AddressBlock = new CustomerInfoAddressBlock()
                        {
                            Line1 = testInstanceName + " 404",
                            PostalCode = "12345",
                            City = "Testby",
                            CountryISO = "SE"
                        },
                        Mobile = "0735157766",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        SocialSecurityNumber = personnummer,
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.PASS:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "PASSTestFirstName",
                        LastName = "PASSTestLastName",
                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    Line1 = "Testvägen 404",
                        //    PostalCode = "12345",
                        //    City = "Testby",
                        //    CountryISO = "SE"
                        //},
                        //Mobile = "0730618110",
                        //Email = "email-389923@example.com",
                        //SocialSecurityNumber = "192711044962",
                        //SwedishSocialSecurityNumber = true,
                        //SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.Kampanj:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "KampTestFirstName",
                        LastName = "KampTestLastName",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    Line1 = "Testvägen 404",
                        //    PostalCode = "12345",
                        //    City = "Testby",
                        //    CountryISO = "SE"
                        //},
                        //Mobile = "0730618110",
                        //SocialSecurityNumber = "192711044962",
                        //SwedishSocialSecurityNumber = true,
                        //SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.OinloggatKundArende:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,

                            //Existerande
                            FirstName = "Testmailchangefirstname",
                            LastName = "Testmailchange:201909196311",
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,

                            //Ny
                            FirstName = "TestMailFirstName" + personnummer,
                            LastName = "TestMailChange:" + personnummer,
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }


                case Generated.ed_informationsource.ForetagsPortal:

                    Random random = new Random();
                    int talaphoneRand = random.Next(1000);
                    CustomerInfoCompanyRole customerInfoCoRoleFTG = new CustomerInfoCompanyRole()
                    {
                        //PortalId = "415125",
                        PortalId = "ST003295",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "073556" + talaphoneRand,
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                        CompanyRoleSpecified = true
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.ForetagsPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SeniorPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkolPortal,
                        //Source = (int)source,
                        FirstName = "TestFirstName",
                        LastName = "TestLastNameFTG:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                                                             //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleFTG },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };

                case Generated.ed_informationsource.SkolPortal:
                    Random random2 = new Random();
                    int talaphoneRand2 = random2.Next(1000);
                    CustomerInfoCompanyRole customerInfoCoRoleSkol = new CustomerInfoCompanyRole()
                    {
                        PortalId = "415125",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "073556" + talaphoneRand2,
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                        CompanyRoleSpecified = true
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkolPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SeniorPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkolPortal,
                        //Source = (int)source,
                        FirstName = "TestFirstName",
                        LastName = "TestLastNameSkol:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleSkol },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };
                case Generated.ed_informationsource.SeniorPortal:
                    Random random3 = new Random();
                    int talaphoneRand3 = random3.Next(1000);
                    CustomerInfoCompanyRole customerInfoCoRoleSenior = new CustomerInfoCompanyRole()
                    {
                        PortalId = "415125",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "073556" + talaphoneRand3,
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                        CompanyRoleSpecified = true
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SeniorPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SeniorPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkolPortal,
                        //Source = (int)source,
                        FirstName = "TestFirstName",
                        LastName = "TestLastNameSenior:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleSenior },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };
                default:
                    throw new NotImplementedException($"Source of type {source} not implemented");
            }
        }

        /// <summary>
        /// Create a CustomerInfo object for tests
        /// </summary>
        /// <param name="testInstanceName"></param>
        /// <param name="personnummer"></param>
        /// <returns></returns>
        public static CustomerInfo Generate_InvalidCustomerInfoObject(string testInstanceName, string personnummer = null, bool generatePN = true, bool isNewRecord = false, Generated.ed_informationsource? source = null)
        {
            if (source == null)
                source = Generated.ed_informationsource.SkapaMittKonto;

            if (personnummer == null && generatePN == true)
            {
                // Build a random, valid, unique personnummer
                personnummer = CustomerUtility.GenerateValidSocialSecurityNumber(DateTime.Now);
            }
            else if (personnummer == null && generatePN == false)
            {
                personnummer = String.Empty;
            }

            switch (source)
            {
                case Generated.ed_informationsource.SkapaMittKonto:
                    return new CustomerInfo()
                    {
                        Source = (int)source,

                        //Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated
                        //SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        //SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        //SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)

                        FirstName = "TestFirstName", //O
                        //LastName = "TestLastName:" + testInstanceName, //O

                        //Befintlig För och Efternamn
                        //FirstName = "Testfirstname", //O
                        //LastName = "Testlastname:18190710.1158", //O

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };

                case Generated.ed_informationsource.UppdateraMittKonto:
                    return new CustomerInfo()
                    {
                        Source = (int)Generated.ed_informationsource.UppdateraMittKonto,
                        //Email = string.Format("test{0}@test.test", testInstanceName), //M
                        //Email = "201904055892@unit.test", //M

                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M
                        //Telephone = customer.Telephone, //O

                        //FirstName = string.Format($"UppdateraMittKontoFörnamn {testInstanceName}"), //O
                        //LastName = customer.LastName, //O

                        //SocialSecurityNumber = customer.SocialSecurityNumber, //O
                        //SwedishSocialSecurityNumber = customer.SwedishSocialSecurityNumber, //O
                        //SwedishSocialSecurityNumberSpecified = false, //O

                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    CO = customer.AddressBlock.CO,
                        //    Line1 = "UpdatedLine1",
                        //    PostalCode = customer.AddressBlock.PostalCode,
                        //    City = "UpdatedCity",
                        //    CountryISO = customer.AddressBlock.CountryISO
                        //}, // O

                    };

                case Generated.ed_informationsource.OinloggatKop:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {

                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,

                            //Existerande
                            FirstName = "Testmailchangefirstname",
                            LastName = "Testmailchange:201909196311",
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "Mailchangeline1 19190919.0342",
                            //    PostalCode = "12345",
                            //    City = "Mailchangevillage19190919.0342",
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,

                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {

                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKop,

                            //Ny
                            FirstName = "Test" + personnummer,
                            LastName = "TestMailChange:" + personnummer,
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }


                case Generated.ed_informationsource.OinloggatLaddaKort:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)source,

                            //Existerande

                            FirstName = "Testmailchangefirstname",
                            //LastName = "Testmailchange:201909196311",
                            AddressBlock = new CustomerInfoAddressBlock()
                            {
                                //co = "mailchangeco",
                                Line1 = "Mailchangeline1 19190919.0342",
                                PostalCode = "12345",
                                City = "Mailchangevillage19190919.0342",
                                CountryISO = "se"
                            },
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,

                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)source,
                            //Ny

                            FirstName = "Test" + personnummer,
                            //LastName = "TestMailChange:" + personnummer,
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }


                case Generated.ed_informationsource.RGOL:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "RGOLTestFirst" + testInstanceName,
                        //LastName = "RGOLTestLast" + testInstanceName,
                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    Line1 = testInstanceName + " 404",
                        //    PostalCode = "12345",
                        //    City = "Testby",
                        //    CountryISO = "SE"
                        //},
                        Mobile = "0735157766",
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        SocialSecurityNumber = personnummer,
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.PASS:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "RGOLTestFirstName",
                        //LastName = "RGOLTestLastName",
                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    Line1 = "Testvägen 404",
                        //    PostalCode = "12345",
                        //    City = "Testby",
                        //    CountryISO = "SE"
                        //},
                        //Mobile = "0730618110",
                        //Email = "email-389923@example.com",
                        //SocialSecurityNumber = "192711044962",
                        //SwedishSocialSecurityNumber = true,
                        //SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.Kampanj:
                    return new CustomerInfo()
                    {
                        Source = (int)source, //Ändra
                        FirstName = "RGOLTestFirstName",
                        //LastName = "RGOLTestLastName",
                        //AddressBlock = new CustomerInfoAddressBlock()
                        //{
                        //    Line1 = "Testvägen 404",
                        //    PostalCode = "12345",
                        //    City = "Testby",
                        //    CountryISO = "SE"
                        //},
                        //Mobile = "0730618110",
                        //Email = "email-389923@example.com",
                        //SocialSecurityNumber = "192711044962",
                        //SwedishSocialSecurityNumber = true,
                        //SwedishSocialSecurityNumberSpecified = true,
                    };

                case Generated.ed_informationsource.OinloggatKundArende:

                    if (isNewRecord == false)
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,

                            //Existerande

                            FirstName = "Testmailchangefirstname",
                            //LastName = "Testmailchange:201909196311",
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            //Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange19190919.0342@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }
                    else
                    {
                        return new CustomerInfo()
                        {
                            Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.OinloggatKundArende,

                            //Ny

                            FirstName = "TestMailFirstName" + personnummer,
                            //LastName = "TestMailChange:" + personnummer,
                            //AddressBlock = new CustomerInfoAddressBlock()
                            //{
                            //    //co = "mailchangeco",
                            //    Line1 = "mailchangeline1 " + testInstanceName,
                            //    PostalCode = "12345",
                            //    City = "mailchangevillage" + testInstanceName,
                            //    CountryISO = "se"
                            //},
                            //Telephone = "031" + testInstanceName.Replace(".", ""),
                            Mobile = "0735" + testInstanceName.Replace(".", ""),
                            //SocialSecurityNumber = personnummer,
                            Email = $"mailchange{testInstanceName}@mail.change",
                            //SwedishSocialSecurityNumber = true,
                            //SwedishSocialSecurityNumberSpecified = true,
                        };
                    }

                case Generated.ed_informationsource.ForetagsPortal:

                    CustomerInfoCompanyRole customerInfoCoRoleFTG = new CustomerInfoCompanyRole()
                    {
                        PortalId = "415125",
                        //Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "0735" + testInstanceName.Replace(".", ""),
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                        CompanyRoleSpecified = true
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.ForetagsPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SeniorPortal,
                        //Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.SkolPortal,
                        //Source = (int)source,
                        //FirstName = "TestMailChangeFirstName",
                        LastName = "TestMailChange:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleFTG },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };
                case Generated.ed_informationsource.SkolPortal:
                    CompanyRoleEntity coRoleSkol = new CompanyRoleEntity()
                    {
                        ed_Role = Generated.ed_companyrole_ed_role.Administrator,
                        ed_FirstName = "TestMailChangeFirstName",
                        ed_LastName = "TestMailChange:" + personnummer,
                        ed_SocialSecurityNumber = personnummer,
                        ed_EmailAddress = string.Format("test{0}@test.test", testInstanceName),
                        ed_Telephone = "0735" + testInstanceName.Replace(".", ""),
                    };

                    CustomerInfoCompanyRole customerInfoCoRoleSkol = new CustomerInfoCompanyRole()
                    {
                        PortalId = personnummer,
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "0735" + testInstanceName.Replace(".", ""),
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = 0
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.ForetagsPortal,
                        FirstName = "TestMailChangeFirstName",
                        LastName = "TestMailChange:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleSkol },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };
                case Generated.ed_informationsource.SeniorPortal:
                    CompanyRoleEntity coRoleSenior = new CompanyRoleEntity()
                    {
                        ed_Role = Generated.ed_companyrole_ed_role.Administrator,
                        ed_FirstName = "TestMailChangeFirstName",
                        ed_LastName = "TestMailChange:" + personnummer,
                        ed_SocialSecurityNumber = personnummer,
                        ed_EmailAddress = string.Format("test{0}@test.test", testInstanceName),
                        ed_Telephone = "0735" + testInstanceName.Replace(".", ""),
                    };

                    CustomerInfoCompanyRole customerInfoCoRoleSenior = new CustomerInfoCompanyRole()
                    {
                        PortalId = personnummer,
                        Email = string.Format("test{0}@test.test", testInstanceName),
                        Telephone = "0735" + testInstanceName.Replace(".", ""),
                        isLockedPortal = false,
                        isLockedPortalSpecified = false,
                        CompanyRole = 0
                    };

                    return new CustomerInfo()
                    {
                        Source = (int)Skanetrafiken.Crm.Schema.Generated.ed_informationsource.ForetagsPortal,
                        FirstName = "TestMailChangeFirstName",
                        LastName = "TestMailChange:" + personnummer,
                        Mobile = "0735" + testInstanceName.Replace(".", ""), //M + F
                        Email = string.Format("test{0}@test.test", testInstanceName), //M + F
                        SocialSecurityNumber = personnummer, //O + Formated (Personnummer som redan finns test)
                        //SwedishSocialSecurityNumberBlock //Detta ska finnas med
                        SwedishSocialSecurityNumber = true, //Personnummer som redan finns (hittar inte)
                        SwedishSocialSecurityNumberSpecified = true, //Personnummer som redan finns (hittar inte)
                        CompanyRole = new CustomerInfoCompanyRole[] { customerInfoCoRoleSenior },

                        //Telephone = "031" + testInstanceName.Replace(".", ""), //O + Formated

                        //AddressBlock = new CustomerInfoAddressBlock() //O
                        //{
                        //    CO = null,
                        //    Line1 = "Testvägen " + testInstanceName,
                        //    PostalCode = "12345", //O
                        //    City = "By" + testInstanceName,
                        //    CountryISO = "SE"
                        //},
                    };
                default:
                    throw new NotImplementedException($"Source of type {source} not implemented");
            }
        }

        public static AccountInfo Generate_ValidAccountInfoObject(string testInstanceName, string organizationNumber = null, Generated.ed_informationsource? source = null)
        {
            if (source == null)
                source = Generated.ed_informationsource.ForetagsPortal;

            if (organizationNumber == null)
                organizationNumber = CustomerUtility.GenerateValidOrganizationalNumber(DateTime.Now);

            switch (source)
            {
                case Generated.ed_informationsource.ForetagsPortal:
                    return new AccountInfo
                    {
                        Addresses = new List<AddressInfo>
                        {
                            new AddressInfo
                            {
                                City = "Göteborg" + testInstanceName,
                                CountryISO = "SE",
                                Name = "Hvitfeldtsgatan 15" + testInstanceName,
                                PostalCode = "41660",
                                Street = "Hvitfeldtsgatan 15" + testInstanceName,
                                TypeCode = (int)Generated.customeraddress_addresstypecode.BillTo
                            }
                        },
                        AllowCreate = true,
                        BillingEmailAddress = string.Format("billing-{0}@test.test", testInstanceName),
                        BillingMethod = (int)Generated.ed_account_ed_billingmethod.Email,
                        CostSite = "555",
                        EMail = string.Format("email-{0}@test.test", testInstanceName),
                        Guid = null,
                        InformationSource = (int)Generated.ed_informationsource.ForetagsPortal,
                        IsLockedPortal = false,
                        OrganizationName = "Endeavor AB" + testInstanceName,
                        OrganizationNumber = "4125672343",
                        PaymentMethod = (int)Generated.ed_account_ed_paymentmethod.Invoice,
                        PortalId = "444",
                        ReferencePortal = "Ref Marcus Stenswed" + testInstanceName,
                        Suborgname = "Marknadsavdelningen" + testInstanceName
                    };

                case Generated.ed_informationsource.SkolPortal:
                    return new AccountInfo
                    {
                        Addresses = new List<AddressInfo>
                        {
                            new AddressInfo
                            {
                                City = "Göteborg" + testInstanceName,
                                CountryISO = "SE",
                                Name = "Hvitfeldtsgatan 15" + testInstanceName,
                                PostalCode = "41660",
                                Street = "Hvitfeldtsgatan 15" + testInstanceName,
                                TypeCode = (int)Generated.customeraddress_addresstypecode.BillTo
                            }
                        },
                        AllowCreate = true,
                        BillingEmailAddress = string.Format("billing-{0}@test.test", testInstanceName),
                        BillingMethod = (int)Generated.ed_account_ed_billingmethod.Email,
                        CostSite = "555",
                        EMail = string.Format("email-{0}@test.test", testInstanceName),
                        Guid = null,
                        InformationSource = (int)Generated.ed_informationsource.ForetagsPortal,
                        IsLockedPortal = false,
                        OrganizationName = "Endeavor AB" + testInstanceName,
                        OrganizationNumber = "4125672343",
                        PaymentMethod = (int)Generated.ed_account_ed_paymentmethod.Invoice,
                        PortalId = "444",
                        ReferencePortal = "Ref Marcus Stenswed" + testInstanceName,
                        Suborgname = "Marknadsavdelningen" + testInstanceName
                    };

                case Generated.ed_informationsource.SeniorPortal:
                    return new AccountInfo
                    {
                        Addresses = new List<AddressInfo>
                        {
                            new AddressInfo
                            {
                                City = "Göteborg" + testInstanceName,
                                CountryISO = "SE",
                                Name = "Hvitfeldtsgatan 15" + testInstanceName,
                                PostalCode = "41660",
                                Street = "Hvitfeldtsgatan 15" + testInstanceName,
                                TypeCode = (int)Generated.customeraddress_addresstypecode.BillTo
                            }
                        },
                        AllowCreate = true,
                        BillingEmailAddress = string.Format("billing-{0}@test.test", testInstanceName),
                        BillingMethod = (int)Generated.ed_account_ed_billingmethod.Email,
                        CostSite = "555",
                        EMail = string.Format("email-{0}@test.test", testInstanceName),
                        Guid = null,
                        InformationSource = (int)Generated.ed_informationsource.ForetagsPortal,
                        IsLockedPortal = false,
                        OrganizationName = "Endeavor AB" + testInstanceName,
                        OrganizationNumber = "4125672343",
                        PaymentMethod = (int)Generated.ed_account_ed_paymentmethod.Invoice,
                        PortalId = "444",
                        ReferencePortal = "Ref Marcus Stenswed" + testInstanceName,
                        Suborgname = "Marknadsavdelningen" + testInstanceName
                    };

                default:
                    throw new NotImplementedException($"Source of type {source} not implemented");
            }

        }
    }
}
