//using NUnit.Framework;
//using Microsoft.Crm.Sdk.Samples;
//using Skanetrafiken.Crm;
//using System;
//using System.Net;
//using System.IO;
//using Skanetrafiken.Crm.Entities;
//using Microsoft.Xrm.Sdk.Query;
//using System.Collections.Generic;
//using Endeavor.Crm.Extensions;
//using Generated = Skanetrafiken.Crm.Schema.Generated;
//using Skanetrafiken.Crm.Controllers;
//using System.Xml;
//using System.Xml.Linq;
//using System.Xml.XPath;
//using System.Xml.Serialization;
//using System.Text;
//using Skanetrafiken.Crm.Models;
//using Skanetrafiken.Crm.ValueCodes;
//using System.Globalization;
//using System.Linq;
//using System.Web;
//using System.Runtime.Serialization.Json;
//using Microsoft.Xrm.Sdk;
//using Newtonsoft.Json;
//using System.Runtime.Serialization;

//namespace Endeavor.Crm.UnitTest
//{
//    [TestFixture]
//    public class AccountApiFixture : PluginFixtureBase
//    {
//        #region Configs
//        private ServerConnection _serverConnection;

//        internal ServerConnection ServerConnection
//        {
//            get
//            {
//                if (_serverConnection == null)
//                {
//                    _serverConnection = new ServerConnection();
//                }
//                return _serverConnection;
//            }
//        }

//        internal ServerConnection.Configuration Config
//        {
//            get
//            {
//                return TestSetup.Config;
//            }
//        }
//        #endregion

//        #region General - Test values

//        //AccountInfo Object
//        private const string ckFirstName = "Chris";
//        private const string ckLastName = "Kangaji";
//        private const string ckEmail = "yyy@yyy.com";
//        private const string ckMobile = "0700158181";
//        private const string ck46Mobile = "+46700158181";
//        private const string ckTelephone = "0857970740";

//        private const string xFirstName = "TestCK";
//        private const string xLastName = "TestCK";


//        private const string xEmailNull = "";
//        private const string xMobile = "0735198846";
//        private const string x46Mobile = "+46735198846";
//        private const string xMobileNull = "";

//        //AccountInfoData
//        private const string xGuidField = "900601-1655";
//        private const string xAccountNumber = "";
//        private const string xOrganizationName = "900601-1655";
//        private const string xCgi_Organizational_Number = "";
//        private const int xPaymentMethod = 0;
//        private bool yIsLockedPortal = false;
//        private const string xEmail = "o1046308@nwytg.com";
//        private const string xCostSite = "";
//        private const string xBillingEmailAddress = "900601-1655";
//        private const int xBillingMethod = 0;
//        private bool xAllowCreate = false;
//        private const string xReferencePortal = "900601-1655";
//        private const string xSuborgname = "";

//        private int xPrivateContactSource = (int)Generated.ed_informationsource.OinloggatLaddaKort;
//        private int xBusinessContactSource = (int)Generated.ed_informationsource.ForetagsPortal;
//        private int xSchoolContactSource = (int)Generated.ed_informationsource.SkolPortal;
//        private int xSeniorContactSource = (int)Generated.ed_informationsource.SeniorPortal;

//        private CustomerInfoCompanyRole[] xCompanyRole = null;

//        //CustomerInfoCompanyRole
//        private const string yPortalId = "";
//        private const string yCompanyRole = "";
//        private const string yEmail = "";
//        private const string yTelephone = "";
//        private const string yDeleteCompanyRole = "";


//        private bool yCompanyRoleSpecified = false;
//        private bool yIsLockedPortalSpecified = false;


//        //CustomerInfoAddressBlock
//        private const string xCity = "Stockholm";
//        private const string xCO = "";
//        private const string xCountryISO = "SE";
//        private const string xPostalCode = "11824";
//        private const string xLine1 = "Tavastgatan";

//        #endregion

//        [SetUp]
//        public void SetUp()
//        {

//        }

//        [TearDown]
//        public void TearDown()
//        {

//        }

//        #region API

//        //Create Request
//        private static HttpWebRequest CreateRequest(Uri uri)
//        {
//            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
//            httpWebRequest.ContentType = "application/json";
//            return httpWebRequest;
//        }
//        //Create Response
//        private static T GetAPIResponse<T>(HttpWebRequest httpWebRequest) where T : class, new()
//        {
//            HttpWebResponse httpResponse;

//            try
//            {
//                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }

//            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
//            {
//                string responseJson = streamReader.ReadToEnd();

//                T responseObj = new T();
//                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseJson)))
//                {
//                    DataContractJsonSerializer ser = new DataContractJsonSerializer(responseObj.GetType());
//                    responseObj = ser.ReadObject(ms) as T;
//                    ms.Close();
//                }
//                return responseObj;
//            }
//        }

//        private string SerializeXmlMessage(Plugin.LocalPluginContext localContext, string xmlMsg)
//        {
//            return JsonConvert.SerializeObject(xmlMsg, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
//        }

//        #endregion

//        public void DeleteAccount(Plugin.LocalPluginContext localContext, Guid accountId)
//        {
//            AccountEntity deleteAccount = XrmRetrieveHelper.Retrieve<AccountEntity>(localContext, accountId, new ColumnSet(false));
//            XrmHelper.Delete(localContext, deleteAccount.ToEntityReference());
//        }

//        //Create Owner for creating a contact

//        public AccountEntity CreateFTGAccount()
//        {
//            try
//            {
//                AccountEntity ftgAccount = new AccountEntity()
//                {

//                };

//                //Lägg till owner
//                //ftgContact.OwnerId = ...;

//                return ftgAccount;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        #region Create CustomerInfo
//        //Business
//        public CustomerInfo Create_CustomerInfo_For_BusinessContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xBusinessContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    //isLockedPortal
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        //School
//        public CustomerInfo Create_CustomerInfo_For_SchoolContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xSchoolContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    //isLockedPortal
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        //Senior
//        public CustomerInfo Create_CustomerInfo_For_SeniorContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xSeniorContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    //isLockedPortal
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_BusinessContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xBusinessContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    Telephone = ckTelephone,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    isLockedPortal = true,
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_SchoolContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xSchoolContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    Telephone = ckTelephone,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    isLockedPortal = true,
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        public CustomerInfo Create_CustomerInfo_FindOrCreate_For_SeniorContact()
//        {
//            try
//            {
//                var companyRoleData = Create_CompanyRole_For_CustomerInfo(); //Kontrollera
//                CustomerInfoCompanyRole[] role = new CustomerInfoCompanyRole[] { companyRoleData };

//                CustomerInfoAddressBlock addressBlockData = Create_AddressBlock_For_CustomerInfo();

//                CustomerInfo contactInfo = new CustomerInfo()
//                {
//                    Source = xSeniorContactSource,
//                    Email = ckEmail,
//                    //NewEmail
//                    Mobile = ck46Mobile,
//                    Telephone = ckTelephone,
//                    FirstName = ckFirstName,
//                    LastName = ckLastName,
//                    //SocialSecurityNumber = xSocialSecurityNrNull,
//                    //SocialSecurityNumber = xSocialSecurityNr,
//                    isLockedPortal = true,
//                    //isLockedPortalSpecified
//                    //Telephone
//                    //AddressBlock //Multiple addresse
//                    CompanyRole = role,
//                    AddressBlock = addressBlockData
//                };
//                return contactInfo;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        #endregion

//        //CompanyRole (Done) (Do we need to create/delete?)
//        public CustomerInfoCompanyRole Create_CompanyRole_For_CustomerInfo()
//        {
//            try
//            {
//                CustomerInfoCompanyRole companyRole = new CustomerInfoCompanyRole()
//                {
//                    //CompanyRoleSpecified
//                    //CompanyRole should be defined
//                    //PortalId
//                    Email = xEmailNull,
//                    Telephone = xMobileNull,
//                    CompanyRoleSpecified = false

//                };
//                return companyRole;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }
//        //AdressBlock (do we need to create/Delete?)
//        public CustomerInfoAddressBlock Create_AddressBlock_For_CustomerInfo()
//        {
//            try
//            {
//                CustomerInfoAddressBlock addressBlockData = new CustomerInfoAddressBlock()
//                {
//                    City = xCity,
//                    CO = xCO,
//                    CountryISO = xCountryISO,
//                    PostalCode = xPostalCode,
//                    Line1 = xLine1
//                };

//                return addressBlockData;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//        #region FindOrCreateUnvalidatedContact

//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForFTG() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();


//                    //* Remember that these methods are called (all these depending on InformationSource):
//                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindOrCreateUnvalidatedContact
//                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact (check the data has uppdated)
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForSkolPortal() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //* Remember that these methods are called (all these depending on InformationSource):
//                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SchoolContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindOrCreateUnvalidatedContact
//                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact (check the data has uppdated)
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsMatchForSeniorPortal() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //* Remember that these methods are called (all these depending on InformationSource):
//                    //* CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateCustomerFirstLastNameInfo(localContext, customerInfo);
//                    //* CustomerUtility.ValidateRoleInfo(localContext, customerInfo);
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SeniorContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindOrCreateUnvalidatedContact
//                            ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact (check the data has uppdated)
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }


//        //Create Contact, delete the created contact
//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForFTG() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

//                    //Change social securitynumber to test ed_socialsecuritynumberblock
//                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_BusinessContact();

//                    if (customerInfo != null)
//                    {
//                        //Act: trigger FindOrCreateUnvalidatedContact
//                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                        //Assert: Assert that you found the contact (check the data has uppdated)
//                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        if (returnContact != null && returnContact.Id != null)
//                        {
//                            DeleteContact(localContext, returnContact.Id);
//                        }
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForSchool() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

//                    //Change social securitynumber to test ed_socialsecuritynumberblock
//                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SchoolContact();

//                    if (customerInfo != null)
//                    {
//                        //Act: trigger FindOrCreateUnvalidatedContact
//                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                        //Assert: Assert that you found the contact (check the data has uppdated)
//                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        if (returnContact != null && returnContact.Id != null)
//                        {
//                            DeleteContact(localContext, returnContact.Id);
//                        }
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test] //This Method Creates Contact If Not Found
//        public void CustomerMatch_FindOrCreateUnvalidatedContact_ReturnsCreatedForSenior() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //FindActiveContact will return Null because we wont creat a contact here (Guid createdContactId = XrmHelper.Create(localContext, contact);)

//                    //Change social securitynumber to test ed_socialsecuritynumberblock
//                    CustomerInfo customerInfo = Create_CustomerInfo_FindOrCreate_For_SeniorContact();

//                    if (customerInfo != null)
//                    {
//                        //Act: trigger FindOrCreateUnvalidatedContact
//                        ContactEntity returnContact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

//                        //Assert: Assert that you found the contact (check the data has uppdated)
//                        Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        //XrmHelper.Delete(localContext, deleteContact.ToEntityReference());
//                        if (returnContact != null && returnContact.Id != null)
//                        {
//                            DeleteContact(localContext, returnContact.Id);
//                        }
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        #endregion

//        #region FindActiveContact

//        [Test]
//        public void CustomerMatch_FindActiveContact_ReturnsMatchForFTG() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_For_BusinessContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindActiveContact
//                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test]
//        public void CustomerMatch_FindActiveContact_ReturnsMatchForSkolPortal() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_For_SchoolContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindActiveContact
//                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        [Test]
//        public void CustomerMatch_FindActiveContact_ReturnsMatchForSeniorPortal() //DoneIsh
//        {
//            try
//            {
//                // Connect to the Organization service. 
//                // The using statement assures that the service proxy will be properly disposed.
//                using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
//                {
//                    // This statement is required to enable early-bound type support.
//                    _serviceProxy.EnableProxyTypes();

//                    Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());
//                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//                    stopwatch.Start();

//                    //Arrange: Create your contact and customerInfo (without social security number), Create your company role
//                    ContactEntity contact = CreateFTGContact();
//                    if (contact != null)
//                    {
//                        Guid createdContactId = XrmHelper.Create(localContext, contact);
//                        CustomerInfo customerInfo = Create_CustomerInfo_For_SeniorContact(); //Varies

//                        if (customerInfo != null)
//                        {
//                            //Act: trigger FindActiveContact
//                            ContactEntity returnContact = ContactEntity.FindActiveContact(localContext, customerInfo);

//                            //Assert: Assert that you found the contact
//                            Assert.IsNotNull(returnContact); //What happens if test fails? Will my contact get deleted?
//                        }

//                        //Delete created Customer Info/Customer and Company Role
//                        //ContactEntity deleteContact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, createdContactId, new ColumnSet(false));
//                        DeleteContact(localContext, createdContactId);
//                    }

//                    localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
//                }
//            }
//            catch (WebException ex)
//            {
//                throw ex;
//            }
//        }

//        #endregion

//        //---- Create test for the validations -- Create test that calls on API and sends "CustomerInfo" as parameters


//    }
//    //#endregion
//}