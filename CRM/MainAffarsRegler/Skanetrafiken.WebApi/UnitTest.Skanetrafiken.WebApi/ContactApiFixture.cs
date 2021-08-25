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
using System.Runtime.Serialization.Json;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Microsoft.Crm.Sdk.Messages;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture]
    public class ContactApiFixture : PluginFixtureBase
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

        //CustomerInfo Object
        private const string ckFirstName = "Chris";
        private const string ckLastName = "Kangaji";
        private const string ckEmail = "yyy@yyy.com";
        private const string ckMobile = "0700158181";
        private const string ck46Mobile = "+46700158181";
        private const string ckTelephone = "0857970740";

        private const string xFirstName = "TestCK";
        private const string xLastName = "TestCK";

        private const string xEmail = "o1046308@nwytg.com";
        private const string xEmailNull = "";
        private const string xMobile = "0735198846";
        private const string x46Mobile = "+46735198846";
        private const string xMobileNull = "";

        private const string xSocialSecurityNr = "900601-1655";
        private const string xSocialSecurityNrNull = "";
        private const string xSweSocialSecurityNr = "900601-1655";
        private const string xSweSocialSecurityNrNull = "";

        private int xPrivateContactSource = (int)Generated.ed_informationsource.OinloggatLaddaKort;
        private int xBusinessContactSource = (int)Generated.ed_informationsource.ForetagsPortal;
        private int xSchoolContactSource = (int)Generated.ed_informationsource.SkolPortal;
        private int xSeniorContactSource = (int)Generated.ed_informationsource.SeniorPortal;
        private int xKopOchSkickaSource = (int)Generated.ed_informationsource.KopOchSkicka;

        private CustomerInfoCompanyRole[] xCompanyRole = null;

        //CustomerInfoCompanyRole
        private const string yPortalId = "";
        private const string yCompanyRole = "";
        private const string yEmail = "";
        private const string yTelephone = "";
        private const string yDeleteCompanyRole = "";

        private bool yIsLockedPortal = false;
        private bool yCompanyRoleSpecified = false;
        private bool yIsLockedPortalSpecified = false;


        //CustomerInfoAddressBlock
        private const string xCity = "Stockholm";
        private const string xCO = "";
        private const string xCountryISO = "SE";
        private const string xPostalCode = "11824";
        private const string xLine1 = "Tavastgatan";

        #endregion

        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, Explicit, Category("Debug")]
        public void BlockAccountMKL()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
                var portalId = "583";
                var blocked = true;

                string errorMessage = "MKLRequest returned an exception. Certificate {0}, endpoint {1}, payload {2}. Message: {3}";
                string mklEndpoint = String.Empty;
                string clientCertName = String.Empty;
                string InputJSON = String.Empty;

                try
                {
                    mklEndpoint = CgiSettingEntity.GetSettingString(localContext, "ed_companyintegrationendpoint");
                    localContext.Trace($"Endpoint: {mklEndpoint}");

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{mklEndpoint}/api/block/accounts/{portalId}");
                    clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                    localContext.Trace($"Certificate: {clientCertName}");

                    httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                        // Result is 
                        InputJSON = "{\"accountId\": " + portalId + ", \"blocked\": " + blocked.ToString().ToLower() + "}";
                        localContext.Trace($"Calling MKL with message: {InputJSON}");
                        streamWriter.Write(InputJSON);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    HttpWebResponse httpResponse1 = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse1.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("Test", result);


                    }

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;

                    if (response == null)
                    {
                        throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                    }
#if DEBUG
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        localContext.TracingService.Trace("SendMKLRequest() returned an exception\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                        throw new Exception(String.Format(errorMessage, clientCertName, mklEndpoint, InputJSON, we.Message), we);
                    }

#else
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    localContext.TracingService.Trace("SendMKLRequest returned an exeption\nHttpCode: {0}\nContent: {1}\n", response.StatusCode, result);
                    throw new Exception(we.Message, we);
                }

#endif
                }

            }
        }

        #region API

        //Create Request
        private static HttpWebRequest CreateRequest(Uri uri)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json";
            return httpWebRequest;
        }
        //Create Response
        private static T GetAPIResponse<T>(HttpWebRequest httpWebRequest) where T : class, new()
        {
            HttpWebResponse httpResponse;

            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException ex)
            {
                throw ex;
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

        private string SerializeXmlMessage(Plugin.LocalPluginContext localContext, string xmlMsg)
        {
            return JsonConvert.SerializeObject(xmlMsg, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        #endregion

        public void DeleteContact(Plugin.LocalPluginContext localContext, Guid contactId)
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

        public void DeleteCompanyRole(Plugin.LocalPluginContext localContext, Guid companyRoleId)
        {
            //Change status to inactive
            CompanyRoleEntity delCompanyRole = new CompanyRoleEntity()
            {
                Id = companyRoleId
            };

            //XrmHelper.Update(localContext, delCompanyRole);

            //Get CompanyRole By ContactId (then delet Contact)
            FilterExpression getCompRole = new FilterExpression(LogicalOperator.And);
            getCompRole.AddCondition(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, companyRoleId);

            CompanyRoleEntity deleteCompanyRole = XrmRetrieveHelper.RetrieveFirst<CompanyRoleEntity>(localContext, new ColumnSet(false), getCompRole);
            XrmHelper.Delete(localContext, deleteCompanyRole.ToEntityReference());
        }

        //Create Owner for creating a contact

        public ContactEntity CreateFTGContact()
        {
            try
            {
                ContactEntity ftgContact = new ContactEntity()
                {
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    EMailAddress1 = ckEmail,
                    EMailAddress2 = ckEmail,
                    Telephone2 = ck46Mobile,
                    cgi_socialsecuritynumber = xSocialSecurityNrNull,
                    ed_SeniorContact = null,
                    ed_SchoolContact = null,
                    ed_BusinessContact = null
                };

                //Lägg till owner
                //ftgContact.OwnerId = ...;

                return ftgContact;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ContactEntity CreatePrivateContact()
        {
            try
            {
                ContactEntity privateContact = new ContactEntity()
                {
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    //EMailAddress1 = ckEmail, //Change depending on: CreateUpdateContactFromCustomerInfo / UpdateWithWeakCustomerInfo
                    EMailAddress2 = ckEmail,
                    Telephone2 = ck46Mobile,
                    cgi_socialsecuritynumber = xSweSocialSecurityNr,
                    //cgi_socialsecuritynumber = xSocialSecurityNrNull, //Find without social security number
                    ed_SeniorContact = null,
                    ed_SchoolContact = null,
                    ed_BusinessContact = null,
                    ed_PrivateCustomerContact = true //??
                };

                //Lägg till owner
                //ftgContact.OwnerId = ...;

                return privateContact;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region Create CustomerInfo

        public CustomerInfo Create_CustomerInfo_For_PrivateContact() //Fix this
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xPrivateContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    //SocialSecurityNumber = xSocialSecurityNrNull,
                    SocialSecurityNumber = xSocialSecurityNr,
                    //isLockedPortal
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Business
        public CustomerInfo Create_CustomerInfo_For_BusinessContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xBusinessContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    SocialSecurityNumber = xSocialSecurityNrNull,
                    //SocialSecurityNumber = xSocialSecurityNr,
                    //isLockedPortal
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //School
        public CustomerInfo Create_CustomerInfo_For_SchoolContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xSchoolContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    SocialSecurityNumber = xSocialSecurityNrNull,
                    //SocialSecurityNumber = xSocialSecurityNr,
                    //isLockedPortal
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //Senior
        public CustomerInfo Create_CustomerInfo_For_SeniorContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xSeniorContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    SocialSecurityNumber = xSocialSecurityNrNull,
                    //SocialSecurityNumber = xSocialSecurityNr,
                    //isLockedPortal
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_BusinessContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                //CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                //CustomerInfo contactInfo = new CustomerInfo()
                //{
                //    Source = xBusinessContactSource,
                //    Email = ckEmail,
                //    //NewEmail
                //    Mobile = ck46Mobile,
                //    Telephone = ckTelephone,
                //    FirstName = ckFirstName,
                //    LastName = ckLastName,
                //    SocialSecurityNumber = xSocialSecurityNrNull,
                //    //SocialSecurityNumber = xSocialSecurityNr,
                //    isLockedPortal = true,
                //    //isLockedPortalSpecified
                //    //Telephone
                //    //AddressBlock //Multiple addresse
                //    CompanyRole = role,
                //    AddressBlock = addressBlockData
                //};

                //Customer info from FTG 
                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xBusinessContactSource,
                    //Email = ckEmail,
                    //NewEmail
                    //Mobile = ck46Mobile,
                    //Telephone = ckTelephone,
                    FirstName = "Mattias",
                    LastName = "Verdin",
                    SocialSecurityNumber = "197310108399",
                    SwedishSocialSecurityNumber = true,
                    SwedishSocialSecurityNumberSpecified = true,
                    CreditsafeOkSpecified = false,
                    AvlidenSpecified = false,
                    UtvandradSpecified = false,
                    EmailInvalidSpecified = false,
                    Guid = "00000000-0000-0000-0000-000000000000",

                    //SocialSecurityNumber = xSocialSecurityNr,
                    isLockedPortal = false,
                    isLockedPortalSpecified = true,
                    isAddressEnteredManuallySpecified = false,
                    isAddressInformationCompleteSpecified = false,
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    //AddressBlock = addressBlockData
                };

                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_SchoolContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xSchoolContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    Telephone = ckTelephone,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    SocialSecurityNumber = xSocialSecurityNrNull,
                    //SocialSecurityNumber = xSocialSecurityNr,
                    isLockedPortal = true,
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_SeniorContact()
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xSeniorContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    Telephone = ckTelephone,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    SocialSecurityNumber = xSocialSecurityNrNull,
                    //SocialSecurityNumber = xSocialSecurityNr,
                    isLockedPortal = true,
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_PrivateContact() //Fix
        {
            try
            {
                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

                CustomerInfo contactInfo = new CustomerInfo()
                {
                    Source = xPrivateContactSource,
                    Email = ckEmail,
                    //NewEmail
                    Mobile = ck46Mobile,
                    Telephone = ckTelephone,
                    FirstName = ckFirstName,
                    LastName = ckLastName,
                    //SocialSecurityNumber = xSocialSecurityNrNull,
                    SocialSecurityNumber = xSocialSecurityNr,
                    isLockedPortal = true,
                    //isLockedPortalSpecified
                    //Telephone
                    //AddressBlock //Multiple addresse
                    CompanyRole = role,
                    AddressBlock = addressBlockData
                };
                return contactInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CustomerInfo Create_CustomerInfo_For_KopOchSkickaKund()
        {
            CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

            CustomerInfo contactInfo = new CustomerInfo()
            {
                Source = xKopOchSkickaSource,
                Email = "vancarl@test.com",
                FirstName = ckFirstName,
                LastName = ckLastName,
                AddressBlock = addressBlockData,
            };

            return contactInfo;
        }

        #endregion

        //CompanyRole (Done) (Do we need to create/delete?)
        public CustomerInfoCompanyRole Create_CompanyRole_For_CustomerInfo()
        {
            try
            {
                CustomerInfoCompanyRole companyRole = new CustomerInfoCompanyRole()
                {
                    CompanyRoleSpecified = true,
                    CompanyRole = 899310000,
                    PortalId = "2040",
                    Email = "test190604@test.se",
                    Telephone = "0767453646",
                    deleteCompanyRole = false,
                    isLockedPortal = false,
                    isLockedPortalSpecified = true

                };
                return companyRole;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //AdressBlock (do we need to create/Delete?)
        public CustomerInfoAddressBlock Create_AddressBlock_For_CustomerInfo()
        {
            try
            {
                CustomerInfoAddressBlock addressBlockData = new CustomerInfoAddressBlock()
                {
                    City = xCity,
                    CO = xCO,
                    CountryISO = xCountryISO,
                    PostalCode = xPostalCode,
                    Line1 = xLine1
                };

                return addressBlockData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region FindOrCreateUnvalidatedContact

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForFTG() //DoneIsh
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

                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact(); //Varies
                    CustomerInfo CIR = new CustomerInfo
                    {
                        Source = 5,
                        FirstName = "Marcus",
                        LastName = "Stenswed",
                        AddressBlock = new CustomerInfoAddressBlock
                        {
                            CO = "",
                            Line1 = "Sankt Pauligatan 18 A",
                            PostalCode = "41660",
                            City = "Göteborg",
                            CountryISO = "SE"
                        },
                        Mobile = "0735198846",
                        SocialSecurityNumber = "199102013936",
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                        Email = "marcus.stenswed@endeavor.se",
                        CreditsafeOkSpecified = false,
                        AvlidenSpecified = false,
                        UtvandradSpecified = false,
                        EmailInvalidSpecified = false,
                        isLockedPortalSpecified = false,
                        isAddressEnteredManuallySpecified = false,
                        isAddressInformationCompleteSpecified = false
                    };



                    #region API-Post
                    //string url = $"{WebApiTestHelper.WebApiRootEndpoint}Contact/Post";
                    string url = $"{WebApiTestHelper.WebApiRootEndpoint}Contacts";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    //Send request
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var InputJSON = WebApiTestHelper.GenericSerializer(CIR);
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

                    #endregion

                    //* Remember that these methods are called (all these depending on InformationSource):
                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
                    //ContactEntity contact = CreateFTGContact();
                    //if (contact != null)
                    //{
                    //    Guid createdContactId = XrmHelper.Create(localContext, contact);
                    //    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact(); //Varies

                    //    if (customerInfo != null)
                    //    {
                    //        //Act: trigger FindOrCreateUnvalidatedContact
                    //        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                    //        //Create an API-Request
                    //        //Set up http request


                    //        //Assert: Assert that you found the contact (check the data has uppdated)
                    //        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                    //    }

                    //    //Delete created Customer Info/Customer and Company Role
                    //    //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                    //    //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                    //    DeleteContact(localContext, createdContactId);
                    //}

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForSkolPortal() //DoneIsh
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

                    //* Remember that these methods are called (all these depending on InformationSource):
                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
                    ContactEntity contact = CreateFTGContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SchoolContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindOrCreateUnvalidatedContact
                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact (check the data has uppdated)
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForSeniorPortal() //DoneIsh
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

                    //* Remember that these methods are called (all these depending on InformationSource):
                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
                    ContactEntity contact = CreateFTGContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SeniorContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindOrCreateUnvalidatedContact
                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact (check the data has uppdated)
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForPrivate() //DoneIsh
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

                    ContactEntity contact = CreatePrivateContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_PrivateContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindOrCreateUnvalidatedContact
                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact (check the data has uppdated)
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                            //Should have uppdated data
                        }

                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        //Create Contact, delete the created contact
        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForFTG() //DoneIsh
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

                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

                    //Change social securitynumber to test ed_socialsecuritynumberblock
                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact();

                    if (customerInfo != null)
                    {

                        //API CALL
                        //Set up http request
                        string url = $"{WebApiTestHelper.WebApiRootEndpoint}Contacts/CreateValueCode";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        //WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        //Send request
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            var InputJSON = WebApiTestHelper.GenericSerializer(customerInfo);
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

                        //ContactEntity returnContact = new ContactEntity();
                        //returnContact.FirstName = "FirstTest";
                        //returnContact.LastName = "Test";
                        //returnContact.EMailAddress1 = "Test@test.com";
                        //returnContact.Telephone2 = "070484848";
                        //returnContact.ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund;
                        //returnContact.Id = XrmHelper.Create(localContext, returnContact);
                        //Act: trigger FindOrCreateUnvalidatedContact
                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                        //Assert: Assert that you found the contact (check the data has uppdated)
                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                        if (returnContact != null && returnContact.Id != null)
                        {
                            DeleteContact(localContext, returnContact.Id);
                        }
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForSchool() //DoneIsh
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

                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

                    //Change social securitynumber to test ed_socialsecuritynumberblock
                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SchoolContact();

                    if (customerInfo != null)
                    {
                        //Act: trigger FindOrCreateUnvalidatedContact
                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                        //Assert: Assert that you found the contact (check the data has uppdated)
                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                        if (returnContact != null && returnContact.Id != null)
                        {
                            DeleteContact(localContext, returnContact.Id);
                        }
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForSenior() //DoneIsh
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

                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

                    //Change social securitynumber to test ed_socialsecuritynumberblock
                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SeniorContact();

                    if (customerInfo != null)
                    {
                        //Act: trigger FindOrCreateUnvalidatedContact
                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                        //Assert: Assert that you found the contact (check the data has uppdated)
                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
                        if (returnContact != null && returnContact.Id != null)
                        {
                            DeleteContact(localContext, returnContact.Id);
                        }
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForPrivate() //DoneIsh
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

                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

                    //Change social securitynumber to test ed_socialsecuritynumberblock
                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_PrivateContact();

                    if (customerInfo != null)
                    {
                        //Act: trigger FindOrCreateUnvalidatedContact
                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                        //Assert: Assert that you found the contact (check the data has uppdated)
                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

                        //Delete created Customer Info/Customer and Company Role
                        if (returnContact != null && returnContact.Id != null)
                        {
                            DeleteContact(localContext, returnContact.Id);
                            //Do we need to delete Company role?
                        }
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test, Category("Debug")]
        public void CustomerMatch_FindOrCreateUnvalidatedContact_PASS()
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

                    string json = TestDataHelper.ReadTestFile(@"C:\TFS\Skåne\Main\Skanetrafiken.WebApi\UnitTest.Skanetrafiken.WebApi\TestMessages\Pass_duplicateContactCreated.json");


                    CustomerInfo customerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerInfo>(json);

                    if (customerInfo != null)
                    {
                        //Act: trigger FindOrCreateUnvalidatedContact
                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                        //Assert: Assert that you found the contact (check the data has uppdated)
                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

                        //Delete created Customer Info/Customer and Company Role
                        if (returnContact != null && returnContact.Id != null)
                        {
                            DeleteContact(localContext, returnContact.Id);
                            //Do we need to delete Company role?
                        }
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }


        #endregion

        #region FindActiveContact

        [Test]
        public void CustomerMatch_FindActiveContact_ReturnsMatchForFTG() //DoneIsh
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

                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
                    ContactEntity contact = CreateFTGContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_For_BusinessContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindActiveContact
                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test]
        public void CustomerMatch_FindActiveContact_ReturnsMatchForSkolPortal() //DoneIsh
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

                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
                    ContactEntity contact = CreateFTGContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_For_SchoolContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindActiveContact
                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test]
        public void CustomerMatch_FindActiveContact_ReturnsMatchForSeniorPortal() //DoneIsh
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

                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
                    ContactEntity contact = CreateFTGContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_For_SeniorContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindActiveContact
                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        [Test]
        public void CustomerMatch_FindActiveContact_ReturnsMatchForNonFTG() //DoneIsh
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

                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
                    ContactEntity contact = CreatePrivateContact();
                    if (contact != null)
                    {
                        Guid createdContactId = XrmHelper.Create(localContext, contact);
                        CustomerInfo customerInfo = Create_CustomerInfo_For_PrivateContact(); //Varies

                        if (customerInfo != null)
                        {
                            //Act: trigger FindActiveContact
                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

                            //Assert: Assert that you found the contact
                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
                        }

                        //Delete created Customer Info/Customer and Company Role
                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
                        DeleteContact(localContext, createdContactId);
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        #endregion

        [Test, Category("Debug")]
        public void CreateContactTest()
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

                    var CompanyRole = new CustomerInfoCompanyRole()
                    {
                        PortalId = "547",
                        CompanyRole = (int)Generated.ed_companyrole_ed_role.Administrator,
                        CompanyRoleSpecified = true,
                        Email = "marie-louise.sandberg@skanetrafiken.se",
                        Telephone = "0724671922",
                        deleteCompanyRole = false,
                        isLockedPortal = false,
                        isLockedPortalSpecified = true
                    };

                    var arCom = new CustomerInfoCompanyRole[1];
                    arCom[0] = CompanyRole;

                    CustomerInfo customerInfo = new CustomerInfo()
                    {
                        Source = (int)Generated.ed_informationsource.ForetagsPortal,
                        FirstName = "Ted",
                        LastName = "Backlund",
                        CompanyRole = arCom,
                        SocialSecurityNumber = "197102202236",
                        SwedishSocialSecurityNumber = true,
                        SwedishSocialSecurityNumberSpecified = true,
                        CreditsafeOkSpecified = false,
                        AvlidenSpecified = false,
                        UtvandradSpecified = false,
                        EmailInvalidSpecified = false,
                        isAddressInformationCompleteSpecified = false,
                        isAddressEnteredManuallySpecified = false,
                        isLockedPortalSpecified = false,
                        Guid = "00000000-0000-0000-0000-000000000000"
                    };

                    try
                    {

                        //localContext.TracingService.Trace("\nSkapaUppdateraContact:");
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{WebApiTestHelper.WebApiRootEndpoint}contacts");
                        WebApiTestHelper.CreateTokenForTest(localContext, httpWebRequest);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string InputJSON = WebApiTestHelper.SerializeCustomerInfo(localContext, customerInfo);

                            streamWriter.Write(InputJSON);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            WrapperController.FormatCustomerInfo(ref customerInfo);

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

                }
            }
            catch (Exception ex)
            {

            }
        }

        [Test] //This Method Creates Contact If Not Found
        public void CustomerMatch_CreateNewCompanyRole_ReturnsCreatedForFTG() //DoneIsh
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

                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

                    //Change social securitynumber to test ed_socialsecuritynumberblock
                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact();

                    if (customerInfo != null)
                    {
                        ////Method is internal (make public then switch back)
                        //Guid returnContactGuid = CompanyRoleEntity.CreateNewCompanyRole(localContext, customerInfo);

                        ////Assert: Assert that you found the contact (check the data has uppdated)
                        //Assert.IsNotNull(returnContactGuid); //What happens if test fails? Will my contact get deleted?

                        //if (returnContactGuid != null)
                        //{
                        //    DeleteCompanyRole(localContext, returnContactGuid);
                        //    DeleteContact(localContext, returnContactGuid);
                        //}
                    }

                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public static CompanyRoleEntity GetCompanyRole(Plugin.LocalPluginContext localContext, string companyRoleId)
        {
            CompanyRoleEntity coRole = null;
            Guid coRoleGuid;

            if (!string.IsNullOrWhiteSpace(companyRoleId))
            {
                bool isValidGuid = Guid.TryParse(companyRoleId, out coRoleGuid);

                if (isValidGuid == true)
                {
                    coRole = XrmRetrieveHelper.Retrieve<CompanyRoleEntity>(localContext, coRoleGuid, new ColumnSet(true));
                }
            }

            return coRole;
        }

        [Test, Explicit, Category("Debug")]
        public void MatchCompanyRoleWithContact()
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

                bool checkSingleCompanyRole = true;

                List<CompanyRoleEntity> allCompanyRoles = new List<CompanyRoleEntity>();

                if (checkSingleCompanyRole == true)
                {
                    //Test
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "C8084AEB-D0C4-E911-80F0-005056B61FFF"));

                    //Acceptans
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "6154D44F-D76F-E811-80F0-005056B665EC"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "66C17142-7BF1-E811-80F1-005056B665EC"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "07F7253D-05B0-E811-80F1-005056B665EC"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "AA9F401A-49FD-E811-80F1-005056B665EC"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "43B2EED2-19A5-E811-80F0-005056B665EC"));
                    //allCompanyRoles.Add(GetCompanyRole(localContext, "7250527A-BFF7-E811-80F1-005056B665EC"));
                }
                else
                {
                    //Hämta alla Organisationsroller (kontrollera status aktiv?)
                    FilterExpression getCompanyRole = new FilterExpression(LogicalOperator.And);
                    getCompanyRole.AddCondition(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.NotNull);
                    getCompanyRole.AddCondition(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.NotNull);
                    allCompanyRoles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, new ColumnSet(true), getCompanyRole).ToList();
                }

                if (allCompanyRoles != null && allCompanyRoles.Count > 0)
                {
                    //Handle every CompanyRole Found
                    foreach (CompanyRoleEntity coRole in allCompanyRoles)
                    {
                        //Get the associated Contact
                        ContactEntity coContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, coRole.ed_Contact.Id, new ColumnSet(
                                ContactEntity.Fields.EMailAddress1,
                                ContactEntity.Fields.EMailAddress2,
                                ContactEntity.Fields.Telephone2,
                                ContactEntity.Fields.FirstName,
                                ContactEntity.Fields.LastName,
                                ContactEntity.Fields.AccountRoleCode,
                                ContactEntity.Fields.ed_SocialSecurityNumber2,
                                ContactEntity.Fields.ed_SocialSecurityNumberBlock,
                                ContactEntity.Fields.ed_BusinessContact));

                        //Get Associated Account
                        AccountEntity coAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, coRole.ed_Account.Id, new ColumnSet(true));

                        //Make sure CompanyRole has email and telephone
                        if (!string.IsNullOrWhiteSpace(coRole.ed_EmailAddress))
                        {
                            //Make sure we found contact and account
                            if (coContact != null && coAccount != null)
                            {
                                //Compare Email1/Email2 to cRole_Email
                                if ((!string.IsNullOrWhiteSpace(coContact.EMailAddress1) || !string.IsNullOrWhiteSpace(coContact.EMailAddress2)))
                                {

                                    //Check if related contacts data (email) matches with CompanyRoles data (email), if not... enter.
                                    if ((string.Compare(coContact.EMailAddress1, coRole.ed_EmailAddress, true) != 0 &&
                                        string.Compare(coContact.EMailAddress2, coRole.ed_EmailAddress, true) != 0))
                                    {
                                        //Create CompanyRole Object that will be updated
                                        CompanyRoleEntity uppCompanyRole = new CompanyRoleEntity();

                                        //if not the same, search for a contact in the system with cRole_Email + cRole_Telephone
                                        List<ContactEntity> emMoContacts = new List<ContactEntity>();
                                        ContactEntity newContact = null;

                                        #region Email Filter (emailFilter)

                                        FilterExpression emailFilter = new FilterExpression(LogicalOperator.Or)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, coRole.ed_EmailAddress),
                                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, coRole.ed_EmailAddress)
                                            }
                                        };

                                        #endregion

                                        emMoContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                            },
                                            Filters =
                                            {
                                                emailFilter,
                                            }

                                        }).ToList();

                                        if (emMoContacts.Count == 1)
                                        {
                                            //Found Contact, update CompanyRole with new Contact
                                            newContact = emMoContacts[0];

                                            #region Handle CompanyRole

                                            uppCompanyRole.Id = coRole.Id;
                                            uppCompanyRole.ed_Contact = newContact.ToEntityReference();

                                            if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                                !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                string firstnameNewContact = Regex.Replace(newContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                                string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                                string lastnameNewContact = Regex.Replace(newContact.LastName, @"[^\p{L}\p{N}]+", "");
                                                string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                                // Compare first- and lastname
                                                if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                }

                                                if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_LastName = newContact.LastName;
                                                }
                                            }
                                            else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                                string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                uppCompanyRole.ed_LastName = newContact.LastName;
                                            }

                                            //Update Company Role
                                            XrmHelper.Update(localContext, uppCompanyRole); //UPDATE COMPANY ROLE

                                            #endregion

                                            #region Uppdate Contact With Information Fom CompanyRole

                                            //Uppdate Contact with some information
                                            ContactEntity uppdateContact = new ContactEntity();
                                            uppdateContact.Id = newContact.Id;
                                            uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                            uppdateContact.ed_BusinessContact = true;
                                            uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            uppdateContact.ed_isLockedPortal = false;

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                uppdateContact.Company = coRole.ed_Telephone;
                                            }

                                            //handle social security number (change)
                                            if (!String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }

                                                if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    if (String.Compare(newContact.ed_SocialSecurityNumberBlock, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                    }
                                                }
                                                else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.ed_SocialSecurityNumberBlock) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            XrmHelper.Update(localContext, uppdateContact); //UPDATE CONTACT

                                            #endregion

                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //add "AssociateEntitiesRequest" with the entities.
                                                // Connect Contact to Accounts (level 1 and 2)
                                                // Create an AssociateEntities request.

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestCostSiteRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestCostSiteRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                            //Check if association between parent account and contact exists
                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestOrgRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestOrgRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestOrgRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                        }
                                        else if (emMoContacts.Count > 1)
                                        {

                                            //Find the contact with the latest created on
                                            newContact = emMoContacts.Where(i => i.CreatedOn == emMoContacts.Max(x => x.CreatedOn)).FirstOrDefault();

                                            #region Handle CompanyRole

                                            uppCompanyRole.Id = coRole.Id;
                                            uppCompanyRole.ed_Contact = newContact.ToEntityReference();

                                            if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                                !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                                !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                                !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                string firstnameNewContact = Regex.Replace(newContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                                string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                                string lastnameNewContact = Regex.Replace(newContact.LastName, @"[^\p{L}\p{N}]+", "");
                                                string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                                // Compare first- and lastname
                                                if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                }
                                                if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                                {
                                                    uppCompanyRole.ed_LastName = newContact.LastName;
                                                }
                                            }
                                            else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                              string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                              !string.IsNullOrWhiteSpace(newContact.FirstName) &&
                                              !string.IsNullOrWhiteSpace(newContact.LastName))
                                            {
                                                uppCompanyRole.ed_FirstName = newContact.FirstName;
                                                uppCompanyRole.ed_LastName = newContact.LastName;
                                            }

                                            //Update Company Role
                                            XrmHelper.Update(localContext, uppCompanyRole); //UPDATE COMPANY ROLE

                                            #endregion

                                            //Update Contact with information
                                            #region Update Contact with information from CompanyRole

                                            ContactEntity uppdateContact = new ContactEntity();
                                            uppdateContact.Id = newContact.Id;
                                            uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                            uppdateContact.ed_BusinessContact = true;
                                            uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            uppdateContact.ed_isLockedPortal = false;

                                            if (!String.IsNullOrWhiteSpace(uppCompanyRole.ed_Telephone))
                                            {
                                                uppdateContact.Company = uppCompanyRole.ed_Telephone;
                                            }

                                            //Socialsecuritynumber??
                                            //if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock) && !String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            //{
                                            //    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            //}

                                            //handle social security number (change)
                                            if (!String.IsNullOrWhiteSpace(newContact.cgi_socialsecuritynumber))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }

                                                if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    if (String.Compare(newContact.ed_SocialSecurityNumberBlock, newContact.cgi_socialsecuritynumber) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                    }
                                                }
                                                else if (String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = newContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (!String.IsNullOrWhiteSpace(newContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                                {
                                                    if (String.Compare(coRole.ed_SocialSecurityNumber, newContact.ed_SocialSecurityNumberBlock) != 0)
                                                    {
                                                        uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                        //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            XrmHelper.Update(localContext, uppdateContact);

                                            #endregion

                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //add "AssociateEntitiesRequest" with the entities.
                                                // Connect Contact to Accounts (level 1 and 2)
                                                // Create an AssociateEntities request.

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestCostSiteRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestCostSiteRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                            //Check if association between parent account and contact exists
                                            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newContact.ContactId.Value))
                                            {
                                                #region AssociateEntities request

                                                //Namespace is Microsoft.Crm.Sdk.Messages
                                                AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                // Set the ID of Moniker1 to the ID of the lead.
                                                requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                // Set the ID of Moniker2 to the ID of the contact.
                                                requestOrgRel.Moniker2 = new EntityReference { Id = newContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                // Set the relationship name to associate on.
                                                requestOrgRel.RelationshipName = "cgi_account_contact";

                                                // Execute the request.
                                                localContext.OrganizationService.Execute(requestOrgRel);

                                                DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false);

                                                #endregion
                                            }

                                        }
                                        else if (emMoContacts.Count == 0) //Did not find any contact: Create new
                                        {
                                            //if not found, create new contact with, cRole_F.Name + cRole_L.Name + cRole_Email + cRole_Telephone

                                            #region Create New Contact With Info. From Company Role

                                            newContact = new ContactEntity();

                                            //Create a new contact and place it in the company role
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_EmailAddress))
                                            {
                                                newContact.EMailAddress1 = coRole.ed_EmailAddress;
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                newContact.Telephone2 = coRole.ed_Telephone; //Add number to new field
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                            {
                                                newContact.Company = coRole.ed_Telephone; //Add number to new field
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_FirstName))
                                            {
                                                newContact.FirstName = coRole.ed_FirstName;
                                            }

                                            if (!String.IsNullOrWhiteSpace(coRole.ed_LastName))
                                            {
                                                newContact.LastName = coRole.ed_LastName;
                                            }

                                            //coRole PN -> Contact PersonNR (Social security Block)
                                            if (!string.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                newContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                            }

                                            newContact.ed_BusinessContact = true;
                                            newContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;
                                            newContact.ed_isLockedPortal = false;
                                            newContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;

                                            Guid newCreatedContact = XrmHelper.Create(localContext, newContact); //CREATE CONTACT

                                            #endregion

                                            ContactEntity newlyCreatedContact = null;

                                            if (newCreatedContact != null && newCreatedContact != Guid.Empty)
                                            {
                                                newlyCreatedContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newCreatedContact, new ColumnSet(true));
                                            }
                                            ////Retrieve contact (null control)
                                            //ContactEntity newlyCreatedContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newCreatedContact, new ColumnSet(true));

                                            if (newlyCreatedContact != null && newlyCreatedContact.ContactId != null)
                                            {

                                                #region Handle Company Role

                                                //Update CompanyRole with the created contact
                                                uppCompanyRole.Id = coRole.Id;
                                                uppCompanyRole.ed_Contact = newlyCreatedContact.ToEntityReference();

                                                XrmHelper.Update(localContext, uppCompanyRole);

                                                #endregion

                                                //3. Check so that there isn't already an association placed!!
                                                if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, newlyCreatedContact.ContactId.Value))
                                                {
                                                    #region AssociateEntities request

                                                    //add "AssociateEntitiesRequest" with the entities.
                                                    // Connect Contact to Accounts (level 1 and 2)
                                                    // Create an AssociateEntities request.

                                                    //Namespace is Microsoft.Crm.Sdk.Messages
                                                    AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                                    // Set the ID of Moniker1 to the ID of the lead.
                                                    requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                                    // Set the ID of Moniker2 to the ID of the contact.
                                                    requestCostSiteRel.Moniker2 = new EntityReference { Id = newlyCreatedContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                    // Set the relationship name to associate on.
                                                    requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                                    // Execute the request.
                                                    localContext.OrganizationService.Execute(requestCostSiteRel);

                                                    DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.Id, false); //??

                                                    #endregion
                                                }

                                                //Check if association between parent account and contact exists
                                                if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, newlyCreatedContact.ContactId.Value))
                                                {
                                                    #region AssociateEntities request

                                                    //Namespace is Microsoft.Crm.Sdk.Messages
                                                    AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                                    // Set the ID of Moniker1 to the ID of the lead.
                                                    requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                                    // Set the ID of Moniker2 to the ID of the contact.
                                                    requestOrgRel.Moniker2 = new EntityReference { Id = newlyCreatedContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                                    // Set the relationship name to associate on.
                                                    requestOrgRel.RelationshipName = "cgi_account_contact";

                                                    // Execute the request.
                                                    localContext.OrganizationService.Execute(requestOrgRel);

                                                    DisassociateEntities(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.Id, false); //??

                                                    #endregion
                                                }

                                                //Uppdate account (type of account) (both parent and child account)

                                            }

                                            //throw new Exception(string.Format("Multiple Contacts found with the same Email: {0}, and Mobile Number: {1}", customerInfo.Email, customerInfo.Mobile)); //revise
                                        }
                                    }
                                    else //Contacts email + telephone is same as company roles, create association to account
                                    {
                                        //2. Uppdatera kontakt med rollen Administratör
                                        #region Update Contact with information from CompanyRole

                                        ContactEntity uppdateContact = new ContactEntity();
                                        uppdateContact.Id = coContact.Id;
                                        uppdateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministrator;
                                        uppdateContact.ed_BusinessContact = true;
                                        uppdateContact.ed_InformationSource = Generated.ed_informationsource.ForetagsPortal;

                                        if (!String.IsNullOrWhiteSpace(coRole.ed_Telephone))
                                        {
                                            uppdateContact.Company = coRole.ed_Telephone; //Update phone nu,ber with in new field
                                        }

                                        //handle social security number (change)
                                        if (!String.IsNullOrWhiteSpace(coContact.cgi_socialsecuritynumber))
                                        {
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                if (String.Compare(coRole.ed_SocialSecurityNumber, coContact.cgi_socialsecuritynumber) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                    uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                }
                                            }

                                            if (!String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                            {
                                                if (String.Compare(coContact.ed_SocialSecurityNumberBlock, coContact.cgi_socialsecuritynumber) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coContact.cgi_socialsecuritynumber;
                                                }
                                            }
                                            else if (String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                            {
                                                uppdateContact.ed_SocialSecurityNumberBlock = coContact.cgi_socialsecuritynumber;
                                            }
                                        }
                                        else if (!String.IsNullOrWhiteSpace(coContact.ed_SocialSecurityNumberBlock))
                                        {
                                            if (!String.IsNullOrWhiteSpace(coRole.ed_SocialSecurityNumber))
                                            {
                                                if (String.Compare(coRole.ed_SocialSecurityNumber, coContact.ed_SocialSecurityNumberBlock) != 0)
                                                {
                                                    uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                                    //uppdateContact.cgi_socialsecuritynumber = coRole.ed_SocialSecurityNumber;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            uppdateContact.ed_SocialSecurityNumberBlock = coRole.ed_SocialSecurityNumber;
                                        }


                                        XrmHelper.Update(localContext, uppdateContact);

                                        #endregion

                                        #region Handle CompanyRole

                                        //Create CompanyRole Object that will be updated
                                        CompanyRoleEntity uppCompanyRole = new CompanyRoleEntity();
                                        uppCompanyRole.Id = coRole.Id;
                                        bool toBeUpdated = false;

                                        if (!string.IsNullOrWhiteSpace(coRole.ed_FirstName) &&
                                            !string.IsNullOrWhiteSpace(coRole.ed_LastName) &&
                                            !string.IsNullOrWhiteSpace(coContact.FirstName) &&
                                            !string.IsNullOrWhiteSpace(coContact.LastName))
                                        {
                                            string firstnameNewContact = Regex.Replace(coContact.FirstName, @"[^\p{L}\p{N}]+", "");
                                            string firstnameCoRole = Regex.Replace(coRole.ed_FirstName, @"[^\p{L}\p{N}]+", "");

                                            string lastnameNewContact = Regex.Replace(coContact.LastName, @"[^\p{L}\p{N}]+", "");
                                            string lastnameCoRole = Regex.Replace(coRole.ed_LastName, @"[^\p{L}\p{N}]+", "");

                                            // Compare first- and lastname
                                            if (String.Compare(firstnameNewContact, firstnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                            {
                                                uppCompanyRole.ed_FirstName = coContact.FirstName;
                                                toBeUpdated = true;
                                            }
                                            if (String.Compare(lastnameNewContact, lastnameCoRole, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) != 0)
                                            {
                                                uppCompanyRole.ed_LastName = coContact.LastName;
                                                toBeUpdated = true;
                                            }
                                        }
                                        else if ((string.IsNullOrWhiteSpace(coRole.ed_FirstName) ||
                                            string.IsNullOrWhiteSpace(coRole.ed_LastName)) &&
                                            !string.IsNullOrWhiteSpace(coContact.FirstName) &&
                                            !string.IsNullOrWhiteSpace(coContact.LastName))
                                        {
                                            uppCompanyRole.ed_FirstName = coContact.FirstName;
                                            uppCompanyRole.ed_LastName = coContact.LastName;
                                            toBeUpdated = true;
                                        }

                                        if (toBeUpdated == true)
                                        {
                                            XrmHelper.Update(localContext, uppCompanyRole);
                                        }

                                        #endregion

                                        //3. Check so that there isn't already an association placed!!
                                        if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.AccountId.Value, ContactEntity.EntityLogicalName, coContact.ContactId.Value))
                                        {
                                            #region AssociateEntities request

                                            //add "AssociateEntitiesRequest" with the entities.
                                            // Connect Contact to Accounts (level 1 and 2)
                                            // Create an AssociateEntities request.

                                            //Namespace is Microsoft.Crm.Sdk.Messages
                                            AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                                            // Set the ID of Moniker1 to the ID of the lead.
                                            requestCostSiteRel.Moniker1 = new EntityReference { Id = coAccount.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                                            // Set the ID of Moniker2 to the ID of the contact.
                                            requestCostSiteRel.Moniker2 = new EntityReference { Id = coContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                            // Set the relationship name to associate on.
                                            requestCostSiteRel.RelationshipName = "cgi_account_contact";

                                            // Execute the request.
                                            localContext.OrganizationService.Execute(requestCostSiteRel);

                                            #endregion
                                        }

                                        //Check if association between parent account and contact exists
                                        if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, coAccount.ParentAccountId.Id, ContactEntity.EntityLogicalName, coContact.ContactId.Value))
                                        {
                                            #region AssociateEntities request

                                            //Namespace is Microsoft.Crm.Sdk.Messages
                                            AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                                            // Set the ID of Moniker1 to the ID of the lead.
                                            requestOrgRel.Moniker1 = new EntityReference { Id = coAccount.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                                            // Set the ID of Moniker2 to the ID of the contact.
                                            requestOrgRel.Moniker2 = new EntityReference { Id = coContact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                                            // Set the relationship name to associate on.
                                            requestOrgRel.RelationshipName = "cgi_account_contact";

                                            // Execute the request.
                                            localContext.OrganizationService.Execute(requestOrgRel);

                                            #endregion
                                        }
                                    }

                                }//What to do if the found contat has null email / telephone (this wont happen according to them)
                            } // What to do if contact or account is null
                        } //What to do if companyRole Email/Telephone is null
                    }
                }

                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
        }

        //Create method that checks association
        public static bool DoesRelationshipExist(Plugin.LocalPluginContext localContext, string relationshipSchemaName, string entity1Schema, Guid entity1GuidValue, string entity2Schema, Guid entity2GuidValue)
        {
            string fetchXml = "<fetch mapping='logical'> <entity name='" + relationshipSchemaName + "'>"
              + "<all-attributes />"
              + "<filter>"
              + "<condition attribute='" + entity1Schema + "id' operator='eq' value ='" + entity1GuidValue.ToString() + "' />"
              + "<condition attribute='" + entity2Schema + "id' operator='eq' value='" + entity2GuidValue.ToString() + "' />"
              + "</filter>"
              + "</entity>"
              + "</fetch>";

            //string fetchResult = localContact.
            var fetchResult = localContext.OrganizationService.RetrieveMultiple(new FetchExpression(fetchXml));

            if (fetchResult.Entities.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //remove association from contact
        public static void DisassociateEntities(Plugin.LocalPluginContext localContext, string relationshipSchema, string entity1Schema, Guid entity1Guid, string entity2Schema, Guid entity2Guid, bool skipCheck)
        {
            if (skipCheck || DoesRelationshipExist(localContext, relationshipSchema, entity1Schema, entity1Guid, entity2Schema, entity2Guid))
            {
                EntityReference moniker1 = new EntityReference { Id = entity1Guid, LogicalName = entity1Schema };
                EntityReference moniker2 = new EntityReference { Id = entity2Guid, LogicalName = entity2Schema };

                DisassociateEntitiesRequest request = new DisassociateEntitiesRequest();
                request.Moniker1 = moniker1;
                request.Moniker2 = moniker2;
                request.RelationshipName = relationshipSchema;

                localContext.OrganizationService.Execute(request);
            }
        }

        /// <summary>
        /// This test creates a new customer with data from Buy and Send
        /// </summary>
        [Test]
        public void TestAPI_KopOchSkickaKund_SkapaKund()
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


                    var query = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(ContactEntity.Fields.ed_PrivateCustomerContact),
                        Criteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, "vancarl@test.com")
                            }
                        }
                    };

                    var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);
                    if (contact != null)
                        DeleteContact(localContext, contact.Id);


                    CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/Contacts",
                        Create_CustomerInfo_For_KopOchSkickaKund(), "POST", typeof(CustomerInfo),
                        delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h)
                        {
                            contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);
                            Assert.AreEqual(contact.Id.ToString(), s);
                            Assert.AreEqual(HttpStatusCode.OK, h.StatusCode);
                        });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Test Dupilcated Contacts
        [Test]
        public void Test_Duplicated_Contact()
        {
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                DeleteTestData(localContext);

                var leadInfo = CreateTestData_LeadInfo();

                //Create contact from lead - SkapaMittKonto
                CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/Leads", leadInfo, "POST",
                    typeof(LeadInfo), delegate (Plugin.LocalPluginContext l, string s, HttpWebResponse h)
                    {


                    });

                leadInfo.Guid = GetLeadFromTestData(localContext, c_email).Id.ToString();
                leadInfo.Source = 1;

                //Create contact from lead
                CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/Leads/{leadInfo.Guid}", leadInfo, "PUT", typeof(LeadInfo),
                    delegate (Plugin.LocalPluginContext ll, string ss, HttpWebResponse hh)
                    {

                    });

                var query = new QueryExpression()
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(true),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, c_socialSec),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, c_email)
                        }
                    }
                };

                var t = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);
                Assert.NotNull(t, "Could not find contact based on email and socialsecuritynumber");

                var rgol_Contact = CreateTestData_CustomerInfo();
                CallApi($"{WebApiTestHelper.WebApiRootEndpoint}/Contacts", rgol_Contact, "POST",
                    typeof(CustomerInfo), delegate (Plugin.LocalPluginContext lx, string sx, HttpWebResponse hx)
                    {
                        var c = ContactEntity.FindActiveContact(lx, rgol_Contact);
                    });
            }

        }

        #region Inner Helper Methods

        const string c_socialSec = "199001019091";
        const string c_email = "vancarlnguyen@hotmail.se";

        private LeadInfo CreateTestData_LeadInfo()
        {
            var lead = new LeadInfo()
            {
                IsCampaignActiveSpecified = false,
                CampaignDiscountPercentSpecified = false,
                ServiceType = 0,
                Source = 0,
                FirstName = "Van Carl",
                LastName = "Nguyen",
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    Line1 = "Grönegatan 1",
                    PostalCode = "25121",
                    City = "Landskrona",
                    CountryISO = "SE"
                },
                Mobile = "0700158181",
                SocialSecurityNumber = c_socialSec,
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
                Email = c_email,
                CreditsafeOkSpecified = false,
                AvlidenSpecified = false,
                UtvandradSpecified = false,
                EmailInvalidSpecified = false,
                isLockedPortalSpecified = false,
                isAddressEnteredManuallySpecified = false,
                isAddressInformationCompleteSpecified = false
            };

            return lead;
        }

        private CustomerInfo CreateTestData_CustomerInfo()
        {
            var customer = new CustomerInfo()
            {
                ServiceType = 0,
                Source = 5,
                FirstName = "Van Carl",
                LastName = "Nguyen",
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    Line1 = "Grönegatan 1",
                    PostalCode = "25121",
                    City = "Landskrona",
                    CountryISO = "SE",
                    CO = ""
                },
                Mobile = "0700158181",
                SocialSecurityNumber = c_socialSec,
                SwedishSocialSecurityNumber = true,
                SwedishSocialSecurityNumberSpecified = true,
                Email = c_email,
                CreditsafeOkSpecified = false,
                AvlidenSpecified = false,
                UtvandradSpecified = false,
                EmailInvalidSpecified = false,
                isLockedPortalSpecified = false,
                isAddressEnteredManuallySpecified = false,
                isAddressInformationCompleteSpecified = false
            };

            return customer;
        }

        private LeadEntity GetLeadFromTestData(Plugin.LocalPluginContext localContext, string leadEmail)
        {

            var query = new QueryExpression()
            {
                EntityName = LeadEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, leadEmail)
                    }
                }
            };

            var lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);
            if (lead == null)
                throw new Exception($"Could not find lead with email '{leadEmail}'");

            return lead;
        }

        private void DeleteTestData(Plugin.LocalPluginContext localContext)
        {

            var query = new QueryExpression()
            {
                EntityName = LeadEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, c_email)
                    }
                }
            };

            var lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, query);
            if (lead != null)
                XrmHelper.Delete(localContext, lead.ToEntityReference());

            query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, c_socialSec),
                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, c_email),
                    }
                }
            };

            var contact = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);
            foreach (var item in contact)
            {
                XrmHelper.Delete(localContext, item.ToEntityReference());
            }
        }

        #endregion

        #endregion

        #region Helpers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">Api endpoint</param>
        /// <param name="body"></param>
        /// <param name="destination">Specifies the type of which the body shall be converted to.</param>
        /// <param name="assert">Pass assertion code here</param>
        /// <param name="xrmAction"></param>
        public void CallApi(string endpoint, object body, string httpRequestMethod, Type destination, Action<Plugin.LocalPluginContext, string, HttpWebResponse> assert = null, Action<Plugin.LocalPluginContext> clearDataAction = null)
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
    }
    //#endregion
}