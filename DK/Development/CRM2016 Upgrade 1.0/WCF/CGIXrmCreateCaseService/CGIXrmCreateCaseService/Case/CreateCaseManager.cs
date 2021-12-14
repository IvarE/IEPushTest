using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Xml.Serialization;
using CGIXrmCreateCaseService.Case.Models;
using CGIXrmWin;
using CRM2013.SkanetrafikenPlugins.Common;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.KeyVault;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.Generic;
using CGIXrmCreateCaseService.Entities;
using CGIXrmCreateCaseService.CRMPlusAPI;
using System.Runtime.Serialization.Json;
using Endeavor.Extensions;
using System.Text;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Azure.Identity;

namespace CGIXrmCreateCaseService.Case
{
    public class CreateCaseManager
    {
        #region Global Variables --------------------------------------------------------------------------------------

        readonly XrmManager _xrmManager;
        private readonly object _lockSql = new object();
        readonly Entity _incidentEntity = new Entity("incident");
        private const string CgiTravelInformation = "cgi_travelinformation";
        private const string CgiBiffTransaction = "cgi_travelcardtransaction";
        private string _tokenCertName = "";
        private string _logLocation = @"E:\Logs\CRM\CGIXrmCreateCaseService\createcase.log";
        private bool useFetchXml = true;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Endeavor Helper Functions

        /// <summary>
        /// Sources to use when calling CRMPlus fasad
        /// </summary>
        public enum CRMPlusAPISource
        {
            SkapaMittKonto = 0,
            ValideraEpost = 1,
            BytEpost = 2,
            OinloggatKop = 3,
            OinloggatKundArende = 4,
            RGOL = 5,
            PASS = 6,
            Kampanj = 7,
            AdmSkapaKund = 8,
            OinloggatLaddaKort = 9,
            UppdateraMittKonto = 10,
            Folkbokforing = 11,
            AdmAndraKund = 12,
            LosenAterstallningSkickat = 13
        }


        /// <summary>
        /// Retrieve fiest record from a entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xrmManager"></param>
        /// <param name="entityLogicalName"></param>
        /// <param name="columns"></param>
        /// <param name="filter"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public static T RetrieveFirst<T>(XrmManager xrmManager, string entityLogicalName, ColumnSet columns, FilterExpression filter = null, Dictionary<string, OrderType> orderby = null) where T : Entity
        {
            QueryExpression query = new QueryExpression(entityLogicalName);
            query.ColumnSet = columns;
            if (filter != null)
                query.Criteria = filter;
            if (orderby != null)
            {
                foreach (string attribute in orderby.Keys)
                {
                    query.AddOrder(attribute, orderby[attribute]);
                }
            }

            PagingInfo page = new PagingInfo();
            page.Count = 1;
            page.PageNumber = 1;
            query.PageInfo = page;

            EntityCollection result = xrmManager.Service.RetrieveMultiple(query);

            if (result.Entities.Count > 0)
                return (result.Entities[0]).ToEntityNull<T>();
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="crmId">Optional guid to create token for</param>
        public string CreateTokenForWEBApi(Guid? crmId = null)
        {
            const long tokenLifetimeSeconds = 30 * 60 + 60 * 60 * 2;  // Add 2h for Summertime UTC

            Int32 unixTimeStamp;
            DateTime currentTime = DateTime.Now;
            DateTime zuluTime = currentTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;

            long validTo = unixTimeStamp + tokenLifetimeSeconds;

            Skanetrafiken.Crm.Controllers.Identity tokenClass = new Skanetrafiken.Crm.Controllers.Identity();

            tokenClass.exp = validTo;

            return Skanetrafiken.Crm.Controllers.Identity.EncodeTokenEncryption(tokenClass, (string)_tokenCertName);
        }

        /// <summary>
        /// RGOL: Anropa Fasad för att hämta eller skapa kund.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Guid GetOrCreateContactRGOLCase(AutoRgCaseRequest request, SettingsEntity settings = null)
        {
            
            // Get settings, first available.
            if (settings == null)
                settings = GetSettings();

            // RGOL Changed, now they send ISO-code.
            //string countryISO = CountryEntity.GetCountryCodeFromName(_xrmManager, request.CustomerAddress1Country);
            string countryISO = request.CustomerAddress1Country != null ? request.CustomerAddress1Country : "SE";       // Default to SE.
            if (countryISO == null)
            {
                _log.Error($"RGOL:Warning:Failed to find countryISO code for country {request.CustomerAddress1Country}");
                //LogMessage(_logLocation, string.Format($"RGOL:Warning:Failed to find countryISO code for country {request.CustomerAddress1Country}"));
            }

            // Call API to get or create new contact.
            CRMPlusAPI.Models.CustomerInfo customer = new CRMPlusAPI.Models.CustomerInfo();
            customer.FirstName = request.CustomerFirstName;
            customer.LastName = request.CustomerLastName;
            customer.Mobile = request.MobileNo;
            //customer.Telephone = request.CustomerTelephonenumber;     // Beslut 170404-Åsa/HåkanT, skall inte fylla i hemtelefonnr.
            customer.Email = request.EmailAddress;

            //Check if contact with given social security number already exits.


            customer.SocialSecurityNumber = request.CustomerSocialSecurityNumber;
            if (request.CustomerSocialSecurityNumber.Length == 8)
                customer.SwedishSocialSecurityNumber = false;
            else
                customer.SwedishSocialSecurityNumber = true;

            customer.AddressBlock = new CRMPlusAPI.Models.CustomerInfoAddressBlock();
            customer.AddressBlock.City = request.CustomerAddress1City;
            customer.AddressBlock.CountryISO = countryISO;
            customer.AddressBlock.CO = request.CustomerAddress1Line1;
            customer.AddressBlock.Line1 = request.CustomerAddress1Line2;
            customer.AddressBlock.PostalCode = request.CustomerAddress1Postalcode;

            customer.Source = (int)CRMPlusAPISource.RGOL;

            try
            {
                // Add client Cert to to the request
                string certName = ConfigurationManager.AppSettings["ClientCertificateName"];
                WebRequestHandler handler = new WebRequestHandler();
                X509Certificate2 certificate = Skanetrafiken.Crm.Controllers.Identity.GetCertToUse(certName);
                handler.ClientCertificates.Add(certificate);

                // Get URL from settings
                CRMPlusAPI.SkanetrafikenCrm obj = new CRMPlusAPI.SkanetrafikenCrm(handler);
                obj.BaseUri = new System.Uri(settings.ed_CRMPlusService);

                // Add token header to request
                string token = CreateTokenForWEBApi();
                obj.HttpClient.DefaultRequestHeaders.Add("X-CRMPlusToken", token);

                // Do debug-tracing. Johan Endeavor
                //if (DateTime.Now < new DateTime(2017, 04, 18))
                //{
                //    _log.Debug(string.Format("Accuired token:{0}", token));
                //    //LogMessage(_logLocation, string.Format("Accuired token:{0}", token));
                //}
                _log.Debug(string.Format("Accuired token:{0}", token));

                // Skapa kundobjekt
                string jsonResult = obj.Contacts.Post(customer);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CRMPlusAPI.Models.CustomerInfo));
                MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(jsonResult));
                CRMPlusAPI.Models.CustomerInfo createdCustomer = (CRMPlusAPI.Models.CustomerInfo)ser.ReadObject(ms);

                // Returnera GUID
                return new Guid(createdCustomer.Guid);
            }
            // Catch HTTP Exceptions from CRMPlus
            catch (Microsoft.Rest.HttpOperationException ex)
            {
                string msg = ex.Response?.Content ?? "Failed to provide Response Context";
                _log.Error(string.Format("Exception catched in GetOrCreateContactRGOLCase:{0}. Country:{1}", msg, request.CustomerAddress1Country));

                string errorMessage = ex.Message ?? "No generic Error";
                string innerMessage = ex.InnerException?.Message ?? "No inner exception";
                _log.Error(string.Format("Exception {0}, inner:{1}", errorMessage, innerMessage));

                throw new Exception(string.Format("Kan inte skapa kund i SeKund. Orsak:{0}", msg), ex.InnerException);
            }
            // Catch any serialization exeptions
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                _log.Error(string.Format("Kunde inte omvandla resultat från CRMPlus till ett objekt. Detaljerat fel:{0}, inner:{1}", ex.Message, ex.InnerException));
                if (ex.Message != null && ex.InnerException != null)
                    _log.Error(string.Format("Exception {0}, inner:{1}", ex.Message, ex.InnerException.Message));

                throw new Exception(string.Format("Kunde inte omvandla resultat från CRMPlus till ett objekt. Detaljerat fel:{0}", ex.Message), ex.InnerException);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="EmailAddress"></param>
        /// <param name="MobilePhoneNumber"></param>
        /// <returns></returns>
        private Guid GetOrCreateContactCase(string FirstName, string LastName, string EmailAddress, string MobilePhoneNumber)
        {
            // input available?
            if (string.IsNullOrWhiteSpace(FirstName) &&
                string.IsNullOrWhiteSpace(LastName) &&
                string.IsNullOrWhiteSpace(EmailAddress) &&
                string.IsNullOrWhiteSpace(MobilePhoneNumber))
            {
                return Guid.Empty;
            }



            // Get settings, first available.
            SettingsEntity settings = GetSettings();


            // Call API to get or create new contact.
            CRMPlusAPI.Models.CustomerInfo customer = new CRMPlusAPI.Models.CustomerInfo();
            customer.FirstName = FirstName;
            customer.LastName = LastName;
            customer.Email = EmailAddress;
            customer.Mobile = MobilePhoneNumber;

            customer.Source = (int)CRMPlusAPISource.OinloggatKundArende;

            try
            {
                // Add client Cert to to the request
                string certName = ConfigurationManager.AppSettings["ClientCertificateName"];
                WebRequestHandler handler = new WebRequestHandler();
                X509Certificate2 certificate = Skanetrafiken.Crm.Controllers.Identity.GetCertToUse(certName);
                handler.ClientCertificates.Add(certificate);

                // Get URL from settings
                CRMPlusAPI.SkanetrafikenCrm obj = new CRMPlusAPI.SkanetrafikenCrm(handler);
                obj.BaseUri = new System.Uri(settings.ed_CRMPlusService);

                // Add token header to request
                string token = CreateTokenForWEBApi();
                obj.HttpClient.DefaultRequestHeaders.Add("X-CRMPlusToken", token);

                // Do debug-tracing. Johan Endeavor
                //if (DateTime.Now < new DateTime(2017, 03, 02))
                //{
                //    _log.Debug(string.Format($"Accuired token:{token}, accessing URL:{settings.ed_CRMPlusService}"));
                //    //LogMessage(_logLocation, string.Format($"Accuired token:{token}, accessing URL:{settings.ed_CRMPlusService}"));
                //}
                _log.Debug(string.Format($"Accuired token:{token}, accessing URL:{settings.ed_CRMPlusService}"));

                string jsonResult = obj.Contacts.Post(customer);

                // Skapa kundobjekt
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CRMPlusAPI.Models.CustomerInfo));
                MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(jsonResult));
                CRMPlusAPI.Models.CustomerInfo createdCustomer = (CRMPlusAPI.Models.CustomerInfo)ser.ReadObject(ms);

                // Returnera GUID
                return new Guid(createdCustomer.Guid);
            }
            // Catch HTTP Exceptions from CRMPlus
            catch (Microsoft.Rest.HttpOperationException ex)
            {
                //string msg = ex.Response.Content.ContentToString();
                string msg = ex.Response.Content;
                string fullMessage = string.Format("Kan inte skapa kund i SeKund. Orsak:{0}", msg);
                _log.Error(fullMessage);
                if (ex.Message != null)
                    _log.Error(string.Format("Exception {0}, inner:{1}", ex.Message, ex.InnerException.Message));

                throw new Exception(fullMessage, ex.InnerException);
            }
            // Catch any serialization exeptions
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                string fullMessage = string.Format("Kunde inte omvandla resultat från CRMPlus till ett objekt. Detaljerat fel:{0}", ex.Message);
                _log.Error(fullMessage);
                if (ex.Message != null)
                    _log.Error(string.Format("Exception {0}, inner:{1}", ex.Message, ex.InnerException.Message));

                throw new Exception(fullMessage, ex.InnerException);
            }
        }


        private SettingsEntity GetSettings()
        {
            // Get settings, first available.
            SettingsEntity settings = RetrieveFirst<SettingsEntity>(_xrmManager, SettingsEntity.EntityLogicalName
                    , new ColumnSet(SettingsEntity.Fields.ed_CRMPlusService));

            if (settings == null)
                throw new Exception("Settings data is missing.");

            if (string.IsNullOrEmpty(settings.ed_CRMPlusService))
                throw new Exception("URL to CRMPlus service is missing in settings.");

            return settings;
        }

        #endregion

        #region Public Methods ----------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public CreateCaseManager()
        {
            _xrmManager = InitializeManager();
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private XrmManager InitializeManager()
        {
            try
            {
                // Read config file.
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                _tokenCertName = ConfigurationManager.AppSettings["TokenCertificateName"];

                if (string.IsNullOrEmpty(crmServerUrl) ||
                    string.IsNullOrEmpty(domain) ||
                    string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(password))
                    throw new Exception();

                XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);

                return xrmMgr;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings. Ex: " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SqlConnection OpenSql()
        {
            //Removed to check that we can close the integrationDB....
            /* 
            lock (_lockSql)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["IntegrationDB"].ConnectionString;
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;
            }
            */
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        private void CloseSql(IDbConnection connection)
        {
            lock (_lockSql)
            {
                connection?.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileLink"></param>
        /// <param name="incidentid"></param>
        private void CreateFileLink(FileLink fileLink, Guid incidentid)
        {
            if (string.IsNullOrEmpty(fileLink.Url)) return;

            if (DateTime.Now < new DateTime(2017, 11, 25))
            {
                _log.Debug("FileLink URL: " + fileLink.Url);
            }
            var entity = new Entity("cgi_filelink")
            {
                Attributes =
                {
                    ["cgi_incidentid"] = new EntityReference("incident", incidentid),
                    ["cgi_url"] = fileLink.Url.Replace(@"\", @"/")
                }
            };

            _xrmManager.Create(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="accountOrContact"></param>
        /// <returns></returns>
        private bool TryIdentifyAccountOrContactFromAutoRg(AutoRgCaseRequest request,
            out Account accountOrContact,
            out string errorMessage)
        {
            accountOrContact = null;
            Guid customerIdGuid;
            bool isContactOrAccountInCrm;
            Account account = null;
            Account contact = null;
            errorMessage = string.Empty;

            if (request.IsOrganisation)
            {
                if (Guid.TryParse(request.CustomerId, out customerIdGuid) == false)
                {
                    return false;
                }

                account = GetAccountFromId(customerIdGuid, (CustomerType)request.CustomerType);
                if (account == null)
                {
                    errorMessage = $"Kunden med id {request.CustomerId} hittas inte i databasen!";
                    isContactOrAccountInCrm = false;
                }
                else
                {
                    isContactOrAccountInCrm = true;
                }
            }
            else if (request.IsOrganisation == false && request.IsLoggedIn)
            {
                if (Guid.TryParse(request.CustomerId, out customerIdGuid) == false)
                {
                    return false;
                }

                contact = GetAccountFromId(customerIdGuid, (CustomerType)request.CustomerType);
                if (contact == null)
                {
                    errorMessage = $"Kunden med id {request.CustomerId} hittas inte i databasen!";
                    isContactOrAccountInCrm = false;
                }
                else
                {
                    isContactOrAccountInCrm = true;
                }
            }
            else
            {
                isContactOrAccountInCrm =
                    TryGetAccountOrContactByEMail(request.EmailAddress, (CustomerType)request.CustomerType, out contact)
                    || TryGetContactBySocialSecurityNumber(request.CustomerSocialSecurityNumber, out contact)
                    || TryGetContactByNameAndAddress(request.CustomerFirstName, request.CustomerLastName, request.CustomerAddress1Line2, request.CustomerAddress1Postalcode, request.CustomerAddress1City, out contact);
            }
            accountOrContact = contact ?? account;
            return isContactOrAccountInCrm;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool TryGetContactBySocialSecurityNumber(string socialSecurityNumber, out Account contact)
        {
            contact = null;

            if (string.IsNullOrEmpty(socialSecurityNumber)) return false;

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='contact'>";
                _fx += "<attribute name='contactid' />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "<condition attribute='cgi_socialsecuritynumber' operator='eq' value='" + socialSecurityNumber + "' />";
                //_fx += "<condition attribute='ed_socialsecuritynumber2' operator='eq' value='234' />";
                //_fx += "<condition attribute='ed_hasswedishsocialsecuritynumber' operator='eq' value='1' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return false;

                contact = TransformEntityToAccount(_xrmManager.Service.Retrieve(_result.Entities[0].LogicalName, _result.Entities[0].Id, new ColumnSet(true)));
                contact.RecordCount = _result.Entities.Count;

                return true;
            }
            else
            {
                // TODO regex check could be here
                var sqlCon = OpenSql();
                using (var command = new SqlCommand("sp_GetContactFromSocialSecurityNumber", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@SocialSecurityNumber",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = socialSecurityNumber
                    });

                    var reader = command.ExecuteXmlReader();

                    AccountSqlList accounts;
                    {
                        var ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                    contact = accounts.Accounts[0];
                    contact.RecordCount = accounts.Accounts.Count;

                    return true;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="addressLine2"></param>
        /// <param name="addressPostalCode"></param>
        /// <param name="addressCity"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        private bool TryGetContactByNameAndAddress(
            string firstName,
            string lastName,
            string addressLine2,
            string addressPostalCode,
            string addressCity,
            out Account account)
        {
            account = null;

            //Avoid searching for empty valuees
            if (string.IsNullOrEmpty(firstName)) return false;
            if (string.IsNullOrEmpty(lastName)) return false;
            if (string.IsNullOrEmpty(addressLine2)) return false;
            if (string.IsNullOrEmpty(addressPostalCode)) return false;
            if (string.IsNullOrEmpty(addressCity)) return false;


            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='contact'>";
                _fx += "<attribute name='contactid' />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "<condition attribute='firstname' operator='like' value='%" + firstName + "%' />";
                _fx += "<condition attribute='lastname' operator='like' value='%" + lastName + "%' />";
                _fx += "<condition attribute='address1_line2' operator='like' value='%" + addressLine2 + "%' />";
                _fx += "<condition attribute='address1_postalcode' operator='like' value='%" + addressPostalCode + "%' />";
                _fx += "<condition attribute='address1_city' operator='like' value='%" + addressCity + "%' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return false;

                account = TransformEntityToAccount(_xrmManager.Service.Retrieve(_result.Entities[0].LogicalName, _result.Entities[0].Id, new ColumnSet(true)));
                account.RecordCount = _result.Entities.Count;

                return true;
            }
            else
            {

                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetContactFromNameAndAddress", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = firstName
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@LastName",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = lastName
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@Address_Line2",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = addressLine2
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@Address_PostalCode",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = addressPostalCode
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@Address_City",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = addressCity
                    });

                    var reader = command.ExecuteXmlReader();

                    AccountSqlList accounts;
                    {
                        var ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                    account = accounts.Accounts[0];
                    account.RecordCount = accounts.Accounts.Count;

                }

                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromEmail(string emailAddress, CustomerType customerType)
        {
            Account returnValue = new Account();

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='account'>";
                _fx += "<attribute name='accountid' />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='emailaddress1' operator='eq' value='" + emailAddress + "' />";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return null;

                returnValue = TransformEntityToAccount(_xrmManager.Service.Retrieve(_result.Entities[0].LogicalName, _result.Entities[0].Id, new ColumnSet(true)));
                returnValue.RecordCount = _result.Entities.Count;
            }
            #region oldStoredprocedure
            else
            {

                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@emailaddress", SqlDbType = SqlDbType.VarChar, SqlValue = emailAddress });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", SqlDbType = SqlDbType.Int, SqlValue = (int)customerType });

                    var reader = command.ExecuteXmlReader();
                    AccountSqlList accounts;
                    {
                        var ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }
                    reader.Close();
                    CloseSql(sqlCon);

                    if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                    returnValue = accounts.Accounts[0];
                    returnValue.RecordCount = accounts.Accounts.Count;
                }
            }
            #endregion  
            return returnValue;
        }


        /// <summary>
        /// Deletes two possible entities:
        /// * TravelInformation
        /// * TravelCardTransaction ( BIFF )
        /// if they are attached to a Incident.
        /// </summary>
        /// <param name="entityLogicalName"></param>
        /// <param name="caseId"></param>
        /// <param name="guardErrorMessage"></param>
        private void DeleteCurrentEntitiesAttachedToCase(string entityLogicalName, Guid caseId, string guardErrorMessage)
        {
            if (caseId == Guid.Empty)
            {
                throw new Exception("Parameter caseId is empty guid.");
            }
            var columns = new string[] { };

            var attributes = new[]
            {
                "cgi_caseid"
            };

            object[] values =
            {
                caseId.ToString()
            };

            QueryBase query = CreateQueryAttribute(entityLogicalName, columns, attributes, values);
            var entityResults = _xrmManager.Service.RetrieveMultiple(query);

            if (entityResults?.Entities == null || entityResults.Entities.Count <= 0) return;

            if (entityResults.Entities.Count > 50)
            {
                throw new Exception(guardErrorMessage);
            }

            foreach (var entity in entityResults.Entities)
            {
                _xrmManager.Service.Delete(entityLogicalName, entity.Id);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromId(Guid customerid, CustomerType customerType)
        {
            Account returnValue;

            if (useFetchXml)
            {
                Entity _customer = null;
                if (customerType == CustomerType.Organisation)
                {

                    _customer = _xrmManager.Service.Retrieve("account", customerid, new ColumnSet(true));

                }
                else if (customerType == CustomerType.Private)
                {

                    _customer = _xrmManager.Service.Retrieve("contact", customerid, new ColumnSet(true));
                }

                if (_customer == null) return null;

                returnValue = TransformEntityToAccount(_customer);
            }
            else
            {
                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@accountid",
                        SqlDbType = SqlDbType.UniqueIdentifier,
                        SqlValue = customerid
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@type",
                        SqlDbType = SqlDbType.Int,
                        SqlValue = (int)customerType
                    });

                    var reader = command.ExecuteXmlReader();

                    AccountSqlList accounts;
                    {
                        var ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                    returnValue = accounts.Accounts[0];
                    returnValue.RecordCount = accounts.Accounts.Count;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customer"></param>
        /// <returns></returns>
        private Account TransformEntityToAccount(Entity _customer)
        {
            try
            {
                Account returnValue = new Account();

                returnValue.AccountId = _customer.Id;

                if (_customer.Contains("cgi_contactnumber"))
                    returnValue.AccountNumber = _customer.GetAttributeValue<string>("cgi_contatnumber");

                if (_customer.Contains("cgi_accountnumber"))
                    returnValue.AccountNumber = _customer.GetAttributeValue<string>("cgi_accountnumber");

                if (_customer.Contains("firstname"))
                    returnValue.FirstName = _customer.GetAttributeValue<string>("firstname");

                if (_customer.Contains("lastname"))
                    returnValue.LastName = _customer.GetAttributeValue<string>("lastname");

                if (_customer.Contains("cgi_socialsecuritynumber"))
                    returnValue.SocialSecurityNumber = _customer.GetAttributeValue<string>("cgi_socialsecuritynumber");

                if (_customer.Contains("emailaddress"))
                    returnValue.EmailAddress = _customer.GetAttributeValue<string>("emailaddress");

                if (_customer.Contains("telephone2"))
                    returnValue.Telephone2 = _customer.GetAttributeValue<string>("telephone2");

                if (_customer.Contains("address1_line1"))
                    returnValue.Address1_line1 = _customer.GetAttributeValue<string>("address1_line1");

                if (_customer.Contains("address1_line2"))
                    returnValue.Address1_line2 = _customer.GetAttributeValue<string>("address1_line2");

                if (_customer.Contains("address1_postalcode"))
                    returnValue.Address1_postalcode = _customer.GetAttributeValue<string>("address1_postalcode");

                if (_customer.Contains("address1_city"))
                    returnValue.Address1_city = _customer.GetAttributeValue<string>("address1_city");

                if (_customer.Contains("address1_country"))
                    returnValue.Address1_country = _customer.GetAttributeValue<string>("address1_country");


                returnValue.RecordCount = 1;

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new Exception("TransformEntityToAccount throw an exception!", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customer"></param>
        /// <returns></returns>
        private Setting TransformEntityToSetting(Entity _customer)
        {
            Setting returnValue = new Setting();

            if (_customer.Contains("cgi_defaultcustomeroncase"))
                returnValue.DefaultCustomer = _customer.GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase").Id.ToString("D");

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customer"></param>
        /// <returns></returns>
        private RgolSetting TransformEntityToRgolSetting(Entity _customer)
        {
            RgolSetting returnValue = new RgolSetting();

            if (_customer.Contains("cgi_rgolsettingid"))
            {
                returnValue.RgolSettingId = _customer.Id;
            }

            if (_customer.Contains("cgi_rgolsettingno"))
            {
                returnValue.RgolSettingNo = _customer.GetAttributeValue<string>("cgi_rgolsettingno");
            }

            if (_customer.Contains("cgi_name"))
            {
                returnValue.Name = _customer.GetAttributeValue<string>("cgi_name");
            }

            if (_customer.Contains("cgi_refundtypeid"))
            {
                EntityReference tempRef = _customer.GetAttributeValue<EntityReference>("cgi_refundtypeid");
                returnValue.RefundTypeId = tempRef.Id.ToString("D");
                returnValue.RefundTypeIdName = tempRef.Name;
            }

            if (_customer.Contains("cgi_reimbursementformid"))
            {
                EntityReference tempRef = _customer.GetAttributeValue<EntityReference>("cgi_reimbursementformid");
                returnValue.ReImBursementFormId = tempRef.Id.ToString("D");
                returnValue.ReImBursementFormIdName = tempRef.Name;
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_customer"></param>
        /// <returns></returns>
        private BaseCurrency TransformEntityToBaseCurrency(Entity _customer)
        {
            BaseCurrency returnValue = new BaseCurrency();

            if (_customer.Contains("basecurrencyid"))
            {
                returnValue.BaseCurrencyId = _customer.GetAttributeValue<EntityReference>("basecurrencyid").Id.ToString("D");
                returnValue.BaseCurrencyIdName = _customer.GetAttributeValue<EntityReference>("basecurrencyid").Name;
            }

            return returnValue;
        }

        #region GetAccountFromIdRgol Not Used
        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        private Account GetAccountFromIdRgol(Guid customerid, CustomerType customerType) //TODO: This private method is never used? Delete (Last check 2016-03-13)
        {
            Account returnValue = null;

            var sqlCon = OpenSql();

            using (var command = new SqlCommand("sp_GetAccountFromAccountId", sqlCon))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@accountid",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    SqlValue = customerid
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@type",
                    SqlDbType = SqlDbType.Int,
                    SqlValue = (int)customerType
                });

                var reader = command.ExecuteXmlReader();

                AccountSqlList accounts;
                {
                    var ser = new XmlSerializer(typeof(AccountSqlList));
                    accounts = ser.Deserialize(reader) as AccountSqlList;
                }
                reader.Close();
                CloseSql(sqlCon);

                if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                returnValue = accounts.Accounts[0];
                returnValue.RecordCount = accounts.Accounts.Count;
            }

            return returnValue;
        }
        */
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="customerType"></param>
        /// <param name="contactOrAccount"></param>
        /// <returns></returns>
        private bool TryGetAccountOrContactByEMail(string emailAddress, CustomerType customerType, out Account contactOrAccount)
        {
            contactOrAccount = null;
            if (string.IsNullOrEmpty(emailAddress)) return false;

            // TODO could do a regex check here

            if (useFetchXml)
            {
                var _fx = "";

                if (customerType == CustomerType.Organisation)
                {
                    _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                    _fx += "<entity name='account'>";
                    _fx += "<attribute name='accountid' />";
                    _fx += "<filter type='and'>";
                    _fx += "<filter type='or'>";
                    _fx += "<condition attribute='emailaddress1' operator='eq' value= '" + emailAddress + "' />";
                    _fx += "<condition attribute='emailaddress2' operator='eq' value= '" + emailAddress + "' />";
                    _fx += "</filter>";
                    _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                    _fx += "</filter>";
                    _fx += "</entity>";
                    _fx += "</fetch>";

                    //_fx += "<filter type='and'>";
                    //_fx += "<condition attribute='emailaddress1' operator='eq' value='" + emailAddress + "' />";
                    //_fx += "<condition attribute='statecode' operator='eq' value='0' />";
                    //_fx += "</filter>";

                }
                else if (customerType == CustomerType.Private)
                {
                    _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                    _fx += "<entity name='contact'>";
                    _fx += "<attribute name='contactid' />";
                    _fx += "<filter type='and'>";
                    _fx += "<filter type='or'>";
                    _fx += "<condition attribute='emailaddress1' operator='eq' value= '" + emailAddress + "' />";
                    _fx += "<condition attribute='emailaddress2' operator='eq' value= '" + emailAddress + "' />";
                    _fx += "</filter>";
                    _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                    _fx += "</filter>";
                    _fx += "</entity>";
                    _fx += "</fetch>";

                    //_fx += "<filter type='and'>";
                    //_fx += "<condition attribute='emailaddress1' operator='eq' value='" + emailAddress + "' />";
                    //_fx += "<condition attribute='statecode' operator='eq' value='0' />";
                    //_fx += "</filter>";
                }

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return false;

                contactOrAccount = TransformEntityToAccount(_xrmManager.Service.Retrieve(_result.Entities[0].LogicalName, _result.Entities[0].Id, new ColumnSet(true)));
                contactOrAccount.RecordCount = _result.Entities.Count;

                return true;

            }
            else
            {

                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@emailaddress",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = emailAddress
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@type",
                        SqlDbType = SqlDbType.Int,
                        SqlValue = (int)customerType
                    });

                    var reader = command.ExecuteXmlReader();

                    AccountSqlList accounts;
                    {
                        var ser = new XmlSerializer(typeof(AccountSqlList));
                        accounts = ser.Deserialize(reader) as AccountSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (accounts?.Accounts == null || !accounts.Accounts.Any()) return false;

                    contactOrAccount = accounts.Accounts[0];
                    contactOrAccount.RecordCount = accounts.Accounts.Count;

                    return true;
                }
            }
        }

        #region Remove unused funciton

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="emailcount"></param>
        /// <returns></returns>
        /* private Account GetAccountFromEmailRgol(AutoRgCaseRequest request, out string emailcount) //TODO: This private method is never used? Delete (Last check 2016-03-13)
         {
             Account returnValue;
             emailcount = "0";

             var sqlCon = OpenSql();

             using (var command = new SqlCommand("sp_GetAccountFromEmail", sqlCon))
             {
                 command.CommandType = CommandType.StoredProcedure;

                 command.Parameters.Add(new SqlParameter
                 {
                     ParameterName = "@emailaddress",
                     SqlDbType = SqlDbType.VarChar,
                     SqlValue = request.EmailAddress
                 });

                 command.Parameters.Add(new SqlParameter
                 {
                     ParameterName = "@type",
                     SqlDbType = SqlDbType.Int,
                     SqlValue = request.CustomerType
                 });

                 var reader = command.ExecuteXmlReader();

                 AccountSqlList accounts;
                 {
                     var ser = new XmlSerializer(typeof(AccountSqlList));
                     accounts = ser.Deserialize(reader) as AccountSqlList;
                 }

                 reader.Close();
                 CloseSql(sqlCon);

                 if (accounts?.Accounts == null || !accounts.Accounts.Any()) return null;

                 returnValue = accounts.Accounts[0];
                 emailcount = accounts.Accounts.Count.ToString();
             }

             return returnValue;
         }
         */
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Setting GetDefaultCustomer()
        {
            Setting returnValue = new Setting();

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='cgi_setting'>";
                _fx += "<all-attributes />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "<filter type='or'>";
                _fx += "<condition attribute='cgi_validto' operator='next-x-years' value='100' />";
                _fx += "<condition attribute='cgi_validto' operator='null' />";
                _fx += "</filter>";
                _fx += "<condition attribute='cgi_validfrom' operator='last-x-years' value='100' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return null;

                returnValue = TransformEntityToSetting(_result.Entities[0]);
            }
            #region oldStoredProcedure
            else
            {

                var sqlCon = OpenSql();
                using (var command = new SqlCommand("sp_GetDefaultCustomer", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var reader = command.ExecuteXmlReader();

                    SettingSqlList settings;
                    {
                        var ser = new XmlSerializer(typeof(SettingSqlList));
                        settings = ser.Deserialize(reader) as SettingSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (settings?.Settings != null && settings.Settings.Any())
                    {
                        returnValue = settings.Settings[0];
                    }
                }
            }
            #endregion

            return returnValue;

        }


        /// <summary>
        /// Get incident by ID. Rewritten by JohanA, Endeavor.
        /// </summary>
        /// <param name="incidentid"></param>
        /// <returns></returns>
        private Incident GetIncidentFromId(Guid incidentid)
        {
            Entity ent = _xrmManager.Service.Retrieve(IncidentEntity.EntityLogicalName, incidentid, new ColumnSet(IncidentEntity.Fields.TicketNumber,
                                                                                                                            IncidentEntity.Fields.AccountId,
                                                                                                                            IncidentEntity.Fields.ContactId,
                                                                                                                            IncidentEntity.Fields.Title,
                                                                                                                            IncidentEntity.Fields.cgi_TravelCardNo,
                                                                                                                            IncidentEntity.Fields.cgi_Accountid,
                                                                                                                            IncidentEntity.Fields.cgi_Contactid,
                                                                                                                            IncidentEntity.Fields.cgi_TelephoneNumber,
                                                                                                                            IncidentEntity.Fields.cgi_rgol_delivery_email,
                                                                                                                            IncidentEntity.Fields.cgi_rgol_telephonenumber));

            IncidentEntity incident = ent.ToEntity<IncidentEntity>();
            return new Incident
            {
                IncidentId = (Guid)incident.IncidentId,
                TicketNumber = incident.TicketNumber,
                Title = incident.Title,
                TravelCardNo = incident.cgi_TravelCardNo,
                AccountId = incident.cgi_Accountid?.Id.ToString(),
                AccountIdName = incident.cgi_Accountid?.Name,
                ContactId = incident.cgi_Contactid?.Id.ToString(),
                ContactIdName = incident.cgi_Contactid?.Name,
                MobileNumber = incident.cgi_TelephoneNumber,
                DeliveryEmailAddress = incident.cgi_rgol_delivery_email,
                DeliveryTelephoneNumber = incident.cgi_rgol_telephonenumber
            };

            //Incident returnValue = null;

            //var sqlCon = OpenSql();

            //using (var command = new SqlCommand("sp_GetIncidentFromIncidentId", sqlCon))
            //{
            //    command.CommandType = CommandType.StoredProcedure;

            //    command.Parameters.Add(new SqlParameter
            //    {
            //        ParameterName = "@incidentid",
            //        SqlDbType = SqlDbType.UniqueIdentifier,
            //        SqlValue = incidentid
            //    });

            //    var reader = command.ExecuteXmlReader();
            //    IncidentSqlList incidents;
            //    {
            //        var ser = new XmlSerializer(typeof(IncidentSqlList));
            //        incidents = ser.Deserialize(reader) as IncidentSqlList;
            //    }
            //    reader.Close();
            //    CloseSql(sqlCon);

            //    if (incidents?.Incidents != null && incidents.Incidents.Any())
            //    {
            //        returnValue = incidents.Incidents[0];
            //    }
            //}

            //return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundType"></param>
        /// <returns></returns>
        private RgolSetting GetRgolSettingFromRefundType(int refundType)
        {
            RgolSetting returnValue = new RgolSetting();

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='cgi_rgolsetting'>";
                _fx += "<all-attributes />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "<condition attribute='cgi_rgolsettingno' operator='eq' value='" + refundType + "' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return null;

                returnValue = TransformEntityToRgolSetting(_result.Entities[0]);
            }
            else
            {

                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetRGOLSetting", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@rgolsetting",
                        SqlDbType = SqlDbType.Int,
                        SqlValue = refundType,
                    });

                    var reader = command.ExecuteXmlReader();

                    RgolSettingSqlList rgolsettings;
                    {
                        var ser = new XmlSerializer(typeof(RgolSettingSqlList));
                        rgolsettings = ser.Deserialize(reader) as RgolSettingSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (rgolsettings?.RgolSettings != null && rgolsettings.RgolSettings.Any())
                    {
                        returnValue = rgolsettings.RgolSettings[0];
                    }
                    else
                    {
                        throw new Exception($"RGOL setting with refundType number {refundType} do not exist.");
                    }
                }
            }
            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardnumber"></param>
        /// <returns></returns>
        private string GetTravelCardIdFromCardNumber(string cardnumber) //TODO: This private method is never used? Delete (Last check 2016-03-13)
        {
            string returnValue = null;

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='cgi_travelcard'>";
                _fx += "<all-attributes />";
                _fx += "<filter type='and'>";
                _fx += "<condition attribute='statecode' operator='eq' value='0' />";
                _fx += "<condition attribute='cgi_travelcardnumber' operator='eq' value='" + cardnumber + "' />";
                _fx += "</filter>";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return null;

                if (_result.Entities[0].Contains("cgi_travelcardid"))
                    returnValue = _result.Entities[0].GetAttributeValue<EntityReference>("cgi_travelcardid").Id.ToString("D");
            }
            else
            {
                var sqlCon = OpenSql();

                using (var command = new SqlCommand("sp_GetTravelCard", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@cardnumber",
                        SqlDbType = SqlDbType.VarChar,
                        SqlValue = cardnumber
                    });

                    var reader = command.ExecuteXmlReader();

                    CardSqlList cards;
                    {
                        var ser = new XmlSerializer(typeof(CardSqlList));
                        cards = ser.Deserialize(reader) as CardSqlList;
                    }

                    reader.Close();
                    CloseSql(sqlCon);

                    if (cards?.Cards != null && cards.Cards.Any())
                    {
                        returnValue = cards.Cards[0].TravelCardId;
                    }
                }
            }

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Guid CreateIncident(CreateCaseRequest request, Account customer)
        {
            _incidentEntity.Attributes = new AttributeCollection
            {
                {"caseorigincode", new OptionSetValue(3)}
            };

            if (!string.IsNullOrEmpty(request.CustomersCategory))
                _incidentEntity.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                _incidentEntity.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            var sendtoqueue = GetSendToQueue(request.CustomersCategory, request.CustomersSubcategory);
            _incidentEntity.Attributes.Add("cgi_sendtoqueue", new OptionSetValue(sendtoqueue));

            // Handle title.
            var titleText = string.Empty;
            var descriptionText = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.Title))
                titleText = request.Title;

            if (titleText.Contains("JoJo") && titleText.Contains("kort") && titleText.Contains("och") && titleText.Contains("priser"))
                titleText = "Kort, appar, biljetter och priser";

            if (sendtoqueue == 285050008)
            {
                if (!string.IsNullOrEmpty(request.ControlFeeNumber))
                    titleText = "" + request.ControlFeeNumber + "";
                else
                    titleText = "Bestridan kontrollavgift";
            }

            if (string.IsNullOrWhiteSpace(titleText))
                titleText = "Övrigt";

            _incidentEntity.Attributes.Add("title", titleText);

            if (!string.IsNullOrEmpty(request.Description))
                descriptionText = $"{request.Description}{Environment.NewLine}";

            if (customer.AccountName == "CGI_DEFAULT")
                _incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            else
            {
                if (request.CustomerType == CustomerType.Organisation)
                {
                    _incidentEntity.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    _incidentEntity.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if (request.CustomerType == CustomerType.Private)
                {
                    _incidentEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    _incidentEntity.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                _incidentEntity.Attributes.Add("cgi_invoiceno", request.InvoiceNumber);
                descriptionText += $"{Environment.NewLine}Fakturanummer: {request.InvoiceNumber}";
            }

            if (!string.IsNullOrEmpty(request.ControlFeeNumber))
            {
                _incidentEntity.Attributes.Add("cgi_controlfeeno", request.ControlFeeNumber);
                descriptionText += $"{Environment.NewLine}Kontrollavgiftsnummer: {request.ControlFeeNumber}";
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                _incidentEntity.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);

                if(request.CardNumber.All(char.IsDigit))
                    _incidentEntity.Attributes.Add("ed_unregisterdskacard", request.CardNumber);
                else
                    _incidentEntity.Attributes.Add("cgi_ticketnumber1", request.CardNumber);
            }

            if (!string.IsNullOrEmpty(request.WayOfTravel))
                _incidentEntity.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);

            if (!string.IsNullOrEmpty(request.Line))
            {
                _incidentEntity.Attributes.Add("cgi_line", request.Line);
                descriptionText += $"{Environment.NewLine}Linje: {request.Line}";
            }

            if (!string.IsNullOrEmpty(request.Train))
            {
                _incidentEntity.Attributes.Add("cgi_train", request.Train);
                descriptionText += $"{Environment.NewLine}Linje: {request.Train}";
            }

            if (!string.IsNullOrEmpty(request.County))
            {
                _incidentEntity.Attributes.Add("cgi_county", request.County);
                descriptionText += $"{Environment.NewLine}Kommun: {request.County}";
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                _incidentEntity.Attributes.Add("cgi_city", request.City);
                descriptionText += $"{Environment.NewLine}Stad: {request.City}";
            }

            if (!string.IsNullOrEmpty(request.DriverId))
            {
                _incidentEntity.Attributes.Add("ed_driverid", request.DriverId);
                descriptionText += $"{Environment.NewLine}Förar Id: {request.DriverId}";
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
                _incidentEntity.Attributes.Add("cgi_emailaddress", request.EmailAddress);

            _incidentEntity.Attributes.Add("cgi_contactcustomer", request.ContactCustomer);

            if (customer.RecordCount != null)
                _incidentEntity.Attributes.Add("cgi_emailcount", customer.RecordCount.ToString());

            if (request.ActionDate != null)
                _incidentEntity.Attributes.Add("cgi_actiondate", request.ActionDate.Value);

            _incidentEntity.Attributes.Add("description", descriptionText);

            //EndeavorDoTrace(_incidentEntity);

            var g = _xrmManager.Create(_incidentEntity);
            return g;
        }

        private void EndeavorDoTrace(Entity _incidentEntity)
        {
            StringBuilder traceString = new StringBuilder();
            traceString.AppendLine();
            traceString.AppendLine("----------" + _incidentEntity.LogicalName + "----------");
            foreach (KeyValuePair<string, object> attribute in _incidentEntity.Attributes)
            {
                try
                {
                    traceString.Append("- " + attribute.Key + " = ");
                    if (attribute.Value == null)
                        traceString.Append("null;");
                    else if (attribute.Value is Microsoft.Xrm.Sdk.EntityReference)
                        traceString.Append("(EntityReference) " + ((EntityReference)attribute.Value).LogicalName + " (" + ((EntityReference)attribute.Value).Id + ")");
                    else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                        traceString.Append("(OptionSetValue) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                    else if (attribute.Value is Microsoft.Xrm.Sdk.Money)
                        traceString.Append("(Money) " + "(" + ((Money)attribute.Value).Value.ToString() + ")");
                    else if (attribute.Value is Microsoft.Xrm.Sdk.OptionSetValue)
                        traceString.Append("(Entity) " + ((OptionSetValue)attribute.Value).ExtensionData + " (" + ((OptionSetValue)attribute.Value).Value.ToString() + ")");
                    else if (attribute.Value is EntityCollection)
                        traceString.Append("(EntityCollection) " + ((EntityCollection)attribute.Value).EntityName + " (" + ((EntityCollection)attribute.Value).Entities.Count() + ")");
                    else
                        traceString.Append("(" + attribute.Value.GetType() + ") " + attribute.Value.ToString());
                    traceString.AppendLine(string.Empty);
                }
                catch (Exception ex)
                {
                    traceString.AppendLine(ex.Message);
                }
            }

            _log.Debug(traceString.ToString());
            //LogMessage(_logLocation, traceString.ToString());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private Guid CreateIncidentRgol(AutoRgCaseRequest request, Account customer)
        {
            var incident = new Entity("incident")
            {
                Attributes = new AttributeCollection
                {
                    {"caseorigincode", new OptionSetValue(285050007)},//resegaranti online
                    {"casetypecode", new OptionSetValue(285050003)},//travel warranty
                    {"cgi_contactcustomer", false},
                }
            };

            // 2019-04-05 - Added new description field
            if (!string.IsNullOrEmpty(request.RGOLExtendedDescription))
                incident.Attributes.Add("ed_description", request.RGOLExtendedDescription);

            if (!string.IsNullOrEmpty(request.CustomersCategory))
                incident.Attributes.Add("cgi_customers_category", request.CustomersCategory);

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
                incident.Attributes.Add("cgi_customers_subcategory", request.CustomersSubcategory);

            if (!string.IsNullOrEmpty(request.Title))
                incident.Attributes.Add("title", request.Title);

            incident.Attributes.Add("description",
                !string.IsNullOrEmpty(request.Description) ? request.Description : "Ej angivet");

            if (customer.AccountName == "CGI_DEFAULT")
            {
                incident.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
            }
            else
            {
                if ((CustomerType)request.CustomerType == CustomerType.Organisation)
                {
                    incident.Attributes.Add("cgi_accountid", new EntityReference("account", customer.AccountId));
                    incident.Attributes.Add("customerid", new EntityReference("account", customer.AccountId));
                }

                if ((CustomerType)request.CustomerType == CustomerType.Private)
                {
                    incident.Attributes.Add("cgi_contactid", new EntityReference("contact", customer.AccountId));
                    incident.Attributes.Add("customerid", new EntityReference("contact", customer.AccountId));
                }
            }

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                incident.Attributes.Add("cgi_unregisterdtravelcard", request.CardNumber);
            }

            if(!string.IsNullOrEmpty(request.SKACardNumber))
            {
                incident.Attributes.Add("ed_unregisterdskacard", request.SKACardNumber);
            }

            if (!string.IsNullOrEmpty(request.WayOfTravel))
            {
                incident.Attributes.Add("cgi_way_of_transport", request.WayOfTravel);
            }

            if (!string.IsNullOrEmpty(request.Line))
            {
                incident.Attributes.Add("cgi_line", request.Line);
            }

            if (!string.IsNullOrEmpty(request.DriverId))
            {
                incident.Attributes.Add("ed_driverid", request.DriverId);
            }

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                incident.Attributes.Add("cgi_emailaddress", request.EmailAddress);
            }

            if (!string.IsNullOrEmpty(request.DeliveryEmailAddress))
            {
                incident.Attributes.Add("cgi_rgol_delivery_email", request.DeliveryEmailAddress);
            }
            // 2017-04-04, make sure email is present in Delivery section
            // JoAn-Endeavor. Från Åsa
            else
            {
                if (!string.IsNullOrEmpty(request.EmailAddress))
                {
                    incident.Attributes.Add("cgi_rgol_delivery_email", request.EmailAddress);
                }
            }

            if (!string.IsNullOrEmpty(request.MobileNo))
            {
                incident.Attributes.Add("cgi_customer_telephonenumber_mobile", request.MobileNo);
                incident.Attributes.Add("cgi_bombmobilenumber", request.MobileNo);
                incident.Attributes.Add("cgi_rgol_telephonenumber", request.MobileNo);
            }

            if (!string.IsNullOrEmpty(request.RGOLIssueID))
            {
                incident.Attributes.Add("cgi_rgolissueid", request.RGOLIssueID);
            }

            if (request.DepartureDateTime != null)
            {
                incident.Attributes.Add("cgi_departuredatetime", request.DepartureDateTime);
            }

            if (request.DepartureDateTime != null)
            {
                incident.Attributes.Add("cgi_actiondate", request.DepartureDateTime);
            }

            if (customer.RecordCount != null)
            {
                incident.Attributes.Add("cgi_emailcount", customer.RecordCount.ToString());
            }

            if (string.IsNullOrEmpty(request.TicketType1) == false)
            {
                incident.Attributes.Add("cgi_tickettype_1", request.TicketType1);
            }

            if (string.IsNullOrEmpty(request.TicketType2) == false)
            {
                incident.Attributes.Add("cgi_tickettype_2", request.TicketType2);
            }

            if (string.IsNullOrEmpty(request.TicketNumber1) == false)
            {
                incident.Attributes.Add("cgi_ticketnumber1", request.TicketNumber1);
            }

            if (string.IsNullOrEmpty(request.TicketNumber2) == false)
            {
                incident.Attributes.Add("cgi_ticketnumber2", request.TicketNumber2);
            }

            if (string.IsNullOrEmpty(request.MileageFrom) == false)
            {
                incident.Attributes.Add("cgi_milagefrom", request.MileageFrom);
            }

            if (string.IsNullOrEmpty(request.MileageTo) == false)
            {
                incident.Attributes.Add("cgi_milageto", request.MileageTo);
            }

            if (string.IsNullOrEmpty(request.MileageKilometers) == false)
            {
                incident.Attributes.Add("cgi_milagekilometers", request.MileageKilometers);
            }

            if (string.IsNullOrEmpty(request.MileageLicencePlateNumber) == false)
            {
                incident.Attributes.Add("cgi_milagelicenseplatenumber", request.MileageLicencePlateNumber);
            }

            if (string.IsNullOrEmpty(request.TaxiFrom) == false)
            {
                incident.Attributes.Add("cgi_taxifrom", request.TaxiFrom);
            }

            if (string.IsNullOrEmpty(request.TaxiTo) == false)
            {
                incident.Attributes.Add("cgi_taxito", request.TaxiTo);
            }

            if (string.IsNullOrEmpty(request.Iban) == false)
            {
                incident.Attributes.Add("cgi_iban", request.Iban);
            }

            if (string.IsNullOrEmpty(request.Bic) == false)
            {
                incident.Attributes.Add("cgi_bic", request.Bic);
            }

            incident.Attributes.Add("cgi_description2",
                !string.IsNullOrEmpty(request.Description) ? request.Description : "Ej angivet");

            incident.Attributes.Add("cgi_taxiclaimedamount", new Money(request.TaxiClaimedAmount));

            incident.Attributes.Add("ownerid", GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultowner"));

            if (string.IsNullOrEmpty(request.Address_Line1) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_line1", request.Address_Line1);
            }

            if (string.IsNullOrEmpty(request.Address_Line2) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_line2", request.Address_Line2);
            }

            if (string.IsNullOrEmpty(request.Address_PostalCode) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_postalcode", request.Address_PostalCode);
            }

            if (string.IsNullOrEmpty(request.Address_City) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_city", request.Address_City);
            }

            if (string.IsNullOrEmpty(request.Address_Country) == false)
            {
                incident.Attributes.Add("cgi_rgol_address1_country", request.Address_Country);
            }

            if (string.IsNullOrEmpty(request.FirstName) == false && string.IsNullOrEmpty(request.LastName) == false)
            {
                incident.Attributes.Add("cgi_rgol_fullname", $"{request.FirstName} {request.LastName}");
            }

            if (string.IsNullOrEmpty(request.CustomerSocialSecurityNumber) == false)
            {
                incident.Attributes.Add("cgi_soc_sec_number", request.CustomerSocialSecurityNumber);
            }

            if (string.IsNullOrEmpty(request.SocialSecurityNumber) == false)
            {
                incident.Attributes.Add("cgi_rgol_socialsecuritynumber", request.SocialSecurityNumber);
            }

            if (!string.IsNullOrEmpty(request.CustomerTelephonenumber))
            {
                incident.Attributes.Add("cgi_telephonenumber", request.CustomerTelephonenumber);
            }


            incident.Attributes.Add("cgi_notravelinfo", true);

            var incidentId = _xrmManager.Create(incident);

            //add filelinks from collection
            if (request.FileLinks != null && request.FileLinks.Any())
            {
                foreach (var fileLink in request.FileLinks)
                {
                    CreateFileLink(fileLink, incidentId);
                }
            }

            return incidentId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundAmount"></param>
        /// <param name="rgolsetting"></param>
        /// <param name="incident"></param>
        /// <param name="currencyid"></param>
        /// <param name="internalMessage"></param>
        /// <param name="customerMessage"></param>
        /// <returns></returns>
        private Guid CreateDecisionRgol(
            decimal refundAmount,
            RgolSetting rgolsetting,
            Incident incident,
            string currencyid,
            string internalMessage,
            string customerMessage)
        {
            _log.Debug($"Entering {nameof(CreateDecisionRgol)}.");

            var refundEntity = new Entity("cgi_refund")
            {
                Attributes = new AttributeCollection()
            };

            if (!string.IsNullOrEmpty(rgolsetting.RefundTypeId))
                refundEntity.Attributes.Add("cgi_refundtypeid", new EntityReference("cgi_refundtype", new Guid(rgolsetting.RefundTypeId)));

            if (!string.IsNullOrEmpty(rgolsetting.ReImBursementFormId))
                refundEntity.Attributes.Add("cgi_reimbursementformid", new EntityReference("cgi_reimbursementform", new Guid(rgolsetting.ReImBursementFormId)));

            if (!string.IsNullOrEmpty(incident.TravelCardNo))
                refundEntity.Attributes.Add("cgi_travelcard_number", incident.TravelCardNo);

            if (!string.IsNullOrEmpty(incident.ContactId))
                refundEntity.Attributes.Add("cgi_contactid", new EntityReference("contact", new Guid(incident.ContactId)));

            if (!string.IsNullOrEmpty(incident.TicketNumber))
                refundEntity.Attributes.Add("cgi_refundnumber", incident.TicketNumber);

            if (!string.IsNullOrEmpty(internalMessage))
                refundEntity.Attributes.Add("cgi_comments", internalMessage);

            if (!string.IsNullOrEmpty(customerMessage))
                refundEntity.Attributes.Add("cgi_customermessage", customerMessage);

            refundEntity.Attributes.Add("cgi_isautogenerated", true);

            refundEntity.Attributes.Add("cgi_caseid", new EntityReference("incident", incident.IncidentId));

            if (!string.IsNullOrEmpty(incident.DeliveryTelephoneNumber))
                refundEntity.Attributes.Add("cgi_mobilenumber", incident.DeliveryTelephoneNumber);

            if (!string.IsNullOrEmpty(incident.DeliveryEmailAddress))
                refundEntity.Attributes.Add("cgi_email", incident.DeliveryEmailAddress);

            if (refundAmount > 0)
            {
                var money = new Money
                {
                    Value = refundAmount
                };

                refundEntity.Attributes.Add("cgi_amount", money);
            }

            if (!string.IsNullOrEmpty(currencyid))
                refundEntity.Attributes.Add("transactioncurrencyid", new EntityReference("transactioncurrency", new Guid(currencyid)));

            refundEntity.Attributes.Add("cgi_attestation", new OptionSetValue(285050004));

            refundEntity.Attributes.Add("ownerid", GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultowner"));

            var valueCodeValidForMonths = Ed_GetSetting<int?>(_xrmManager.Service, "cgi_valuecodevalidformonths");
            var valueCodeValidDate = Ed_GetSetting<DateTime?>(_xrmManager.Service, "ed_valuecodevaliddate");

            if (valueCodeValidForMonths == null && valueCodeValidDate == null)
                throw new Exception("'valueCodeValidForMonths' and 'valueCodeValidDate' cannot be empty at the same time. Go to settings to enter one of the fields.");

            if (valueCodeValidForMonths != null && valueCodeValidDate == null)
                refundEntity.Attributes.Add("cgi_last_valid", DateTime.Now.AddMonths(valueCodeValidForMonths.Value));
            else if (valueCodeValidForMonths == null && valueCodeValidDate != null)
                refundEntity.Attributes.Add("cgi_last_valid", valueCodeValidDate.Value);




            _log.Debug($"Creating refund.");
            var returnValue = _xrmManager.Create(refundEntity);
            _log.Debug($"Refund id: {returnValue.ToString()}");

            _log.Debug($"Exiting {nameof(CreateDecisionRgol)}.");
            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="incidentid"></param>
        private void CreateAnnotation(Document document, Guid incidentid)
        {
            var encodedData = Convert.ToBase64String(document.DocumentBody);

            var annotation = new Entity("annotation")
            {
                Attributes =
                {
                    ["objectid"] = new EntityReference("incident", incidentid),
                    ["objecttypecode"] = "incident",
                    ["subject"] = document.Subject,
                    ["documentbody"] = encodedData,
                    ["mimetype"] = @"text/plain",
                    ["notetext"] = document.NoteText,
                    ["filename"] = document.FileName
                }
            };

            _xrmManager.Create(annotation);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customercategory"></param>
        /// <param name="custumersubcategory"></param>
        /// <returns></returns>
        private static int GetSendToQueue(string customercategory, string custumersubcategory)
        {
            // 285,050,006  Kundtjänst 1 Linjen
            var returnValue = 285050000;

            if (customercategory == "Synpunkter trafikinformation" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Tankar och önskemål om ny trafik" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Skador och vandalisering" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Regionbuss")
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Stadsbuss")
                returnValue = 285050000;
            else if (customercategory == "Min resa" && custumersubcategory == "Tåg")
                returnValue = 285050000;
            else if (customercategory == "Övrigt" && (string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006; // 285,050,006  Kundtjänst 1 Linjen
            else if (customercategory == "Överklaga kontroll avgift" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050008;
            else if (customercategory == "Överklaga kontrollavgift" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))//Same as previous with alternative spelling on customercategory
                returnValue = 285050008;
            else if (customercategory == "JoJo kort och priser" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050004;
            else if (customercategory == "Min resa" && custumersubcategory == "Närtrafik")
                returnValue = 285050001;
            else if (customercategory == "Min resa" && custumersubcategory == "Färdtjänst/sjukresa")
                returnValue = 285050001;
            else if (customercategory == "Boka gruppresa" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006;
            else if (customercategory == "Boka ledsagning" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050006;
            else if (customercategory == "Kundtjänst Serviceresor" && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050001;
            else if ((customercategory == "" || string.IsNullOrEmpty(customercategory)) && (custumersubcategory == "" || string.IsNullOrEmpty(custumersubcategory)))
                returnValue = 285050000;

            return returnValue;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private BaseCurrency GetCurrency()
        {
            BaseCurrency returnValue = new BaseCurrency();

            if (useFetchXml)
            {
                var _fx = "";
                _fx += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                _fx += "<entity name='transactioncurrency'>";
                _fx += "<all-attributes />";
                _fx += "<order attribute='createdon' descending='false' />";
                _fx += "</entity>";
                _fx += "</fetch>";

                EntityCollection _result = _xrmManager.Service.RetrieveMultiple(new FetchExpression(_fx));

                if (_result == null || _result.Entities.Count <= 0) return null;

                returnValue = TransformEntityToBaseCurrency(_result.Entities[0]);

            }
            else
            {

                SqlConnection sqlCon = OpenSql();
                using (var command = new SqlCommand("sp_GetBaseCurrency", sqlCon))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var reader = command.ExecuteXmlReader();
                    BaseCurrencySqlList currencies;
                    {
                        var ser = new XmlSerializer(typeof(BaseCurrencySqlList));
                        currencies = ser.Deserialize(reader) as BaseCurrencySqlList;
                    }
                    reader.Close();
                    CloseSql(sqlCon);

                    if (currencies?.BaseCurrencies != null && currencies.BaseCurrencies.Any())
                    {
                        returnValue = currencies.BaseCurrencies[0];
                    }
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void CreateLogFileAutoRGCase(int threadId, AutoRgCaseRequest request)
        {
            if (request == null) return;
            //var sw = new StreamWriter("E:\\Logs\\CRM\\CGIXrmCreateCaseService\\RequestCreateCase_Log\\CreateLogFile_Log.txt", true);
            //sw.WriteLine("======================================================================================================================");
            //sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));

            _log.Info($"Th={threadId} - ===================== Checking Missing Parameters. =====================");

            //if (!string.IsNullOrEmpty(request.CustomersCategory))
            //    sw.WriteLine("CustomersCategory : " + request.CustomersCategory);
            //else
            //    sw.WriteLine("CustomersCategory is missing");

            if (!string.IsNullOrEmpty(request.CustomersCategory))
            {
                _log.Debug($"Th={threadId} - CustomersCategory : {request.CustomersCategory}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomersCategory is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomersSubcategory))
            //    sw.WriteLine("CustomersSubcategory : " + request.CustomersSubcategory);
            //else
            //    sw.WriteLine("CustomersSubcategory is missing");

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
            {
                _log.Debug($"Th={threadId} - CustomersSubcategory : {request.CustomersSubcategory}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomersSubcategory is missing");
            }

            //if (!string.IsNullOrEmpty(request.Title))
            //    sw.WriteLine("Title : " + request.Title);
            //else
            //    sw.WriteLine("Title is missing");

            if (!string.IsNullOrEmpty(request.Title))
            {
                _log.Debug($"Th={threadId} - Title : {request.Title}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Title is missing");
            }

            //if (!string.IsNullOrEmpty(request.Description))
            //    sw.WriteLine("Description : " + request.Description);
            //else
            //    sw.WriteLine("Description is missing");

            if (!string.IsNullOrEmpty(request.Description))
            {
                _log.Debug($"Th={threadId} - Description : {request.Description}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Description is missing");
            }

            //if (!string.IsNullOrEmpty(request.RGOLExtendedDescription))
            //    sw.WriteLine("RGOLExtendedDescription : " + request.RGOLExtendedDescription);
            //else
            //    sw.WriteLine("RGOLExtendedDescription is missing");

            if (!string.IsNullOrEmpty(request.RGOLExtendedDescription))
            {
                _log.Debug($"Th={threadId} - RGOLExtendedDescription : {request.RGOLExtendedDescription}");
            }
            else
            {
                _log.Warn($"Th={threadId} - RGOLExtendedDescription is missing");
            }

            //if (!string.IsNullOrEmpty(request.QueueId))
            //    sw.WriteLine("QueueId : " + request.QueueId);
            //else
            //    sw.WriteLine("QueueId is missing");

            if (!string.IsNullOrEmpty(request.QueueId))
            {
                _log.Debug($"Th={threadId} - QueueId : {request.QueueId}");
            }
            else
            {
                _log.Warn($"Th={threadId} - QueueId is missing");
            }

            //if (request.CustomerId != null )
            //    sw.WriteLine("CustomerId : " + request.CustomerId);
            //else
            //    sw.WriteLine("CustomerId is missing");

            if (request.CustomerId != null)
            {
                _log.Debug($"Th={threadId} - CustomerId : {request.CustomerId}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerId is missing");
            }

            switch (request.CustomerType)
            {
                case 0:// CustomerType.Private:
                    //sw.WriteLine("Customer : Private");
                    _log.Debug($"Th={threadId} - Customer : Private");
                    break;
                case 1: // CustomerType.Organisation:
                    //sw.WriteLine("Customer : Organisation");
                    _log.Debug($"Th={threadId} - Customer : Organisation");
                    break;
                default:
                    //sw.WriteLine("Customer is missing");
                    _log.Warn($"Th={threadId} - Customer is missing");
                    break;
            }

            //if (!string.IsNullOrEmpty(request.CardNumber))
            //    sw.WriteLine("CardNumber : " + request.CardNumber);
            //else
            //    sw.WriteLine("CardNumber is missing");

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                _log.Debug($"Th={threadId} - CardNumber : {request.CardNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CardNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.SKACardNumber))
            //    sw.WriteLine("SKACardNumber : " + request.SKACardNumber);
            //else
            //    sw.WriteLine("SKACardNumber is missing");

            if (!string.IsNullOrEmpty(request.SKACardNumber))
            {
                _log.Debug($"Th={threadId} - SKACardNumber : {request.SKACardNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - SKACardNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.WayOfTravel))
            //    sw.WriteLine("WayOfTravel : " + request.WayOfTravel);
            //else
            //    sw.WriteLine("WayOfTravel is missing");

            if (!string.IsNullOrEmpty(request.WayOfTravel))
            {
                _log.Debug($"Th={threadId} - WayOfTravel : {request.WayOfTravel}");
            }
            else
            {
                _log.Warn($"Th={threadId} - WayOfTravel is missing");
            }

            //if (!string.IsNullOrEmpty(request.Line))
            //    sw.WriteLine("Line : " + request.Line);
            //else
            //    sw.WriteLine("Line is missing");

            if (!string.IsNullOrEmpty(request.Line))
            {
                _log.Debug($"Th={threadId} - Line : {request.Line}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Line is missing");
            }

            //if (!string.IsNullOrEmpty(request.DriverId))
            //    sw.WriteLine("DriverId : " + request.DriverId);
            //else
            //    sw.WriteLine("DriverId is missing");

            if (!string.IsNullOrEmpty(request.DriverId))
            {
                _log.Debug($"Th={threadId} - DriverId : {request.DriverId}");
            }
            else
            {
                _log.Warn($"Th={threadId} - DriverId is missing");
            }

            //if (!string.IsNullOrEmpty(request.FirstName))
            //    sw.WriteLine("FirstName : " + request.FirstName);
            //else
            //    sw.WriteLine("FirstName is missing");

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                _log.Debug($"Th={threadId} - FirstName : {request.FirstName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - FirstName is missing");
            }

            //if (!string.IsNullOrEmpty(request.LastName))
            //    sw.WriteLine("LastName : " + request.LastName);
            //else
            //    sw.WriteLine("LastName is missing");

            if (!string.IsNullOrEmpty(request.LastName))
            {
                _log.Debug($"Th={threadId} - LastName : {request.LastName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - LastName is missing");
            }

            //if (!string.IsNullOrEmpty(request.EmailAddress))
            //    sw.WriteLine("EmailAddress : " + request.EmailAddress);
            //else
            //    sw.WriteLine("EmailAddress is missing");

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                _log.Debug($"Th={threadId} - EmailAddress : {request.EmailAddress}");
            }
            else
            {
                _log.Warn($"Th={threadId} - EmailAddress is missing");
            }

            //if (!string.IsNullOrEmpty(request.DeliveryEmailAddress))
            //    sw.WriteLine("DeliveryEmailAddress : " + request.DeliveryEmailAddress);
            //else
            //    sw.WriteLine("DeliveryEmailAddress is missing");

            if (!string.IsNullOrEmpty(request.DeliveryEmailAddress))
            {
                _log.Debug($"Th={threadId} - DeliveryEmailAddress : {request.DeliveryEmailAddress}");
            }
            else
            {
                _log.Warn($"Th={threadId} - DeliveryEmailAddress is missing");
            }

            //if (!string.IsNullOrEmpty(request.MobileNo))
            //    sw.WriteLine("MobileNo : " + request.MobileNo);
            //else
            //    sw.WriteLine("MobileNo is missing");

            if (!string.IsNullOrEmpty(request.MobileNo))
            {
                _log.Debug($"Th={threadId} - MobileNo : {request.MobileNo}");
            }
            else
            {
                _log.Warn($"Th={threadId} - MobileNo is missing");
            }

            //if (!string.IsNullOrEmpty(request.RGOLIssueID))
            //    sw.WriteLine("RGOLIssueID : " + request.RGOLIssueID);
            //else
            //    sw.WriteLine("RGOLIssueID is missing");

            if (!string.IsNullOrEmpty(request.RGOLIssueID))
            {
                _log.Debug($"Th={threadId} - RGOLIssueID : {request.RGOLIssueID}");
            }
            else
            {
                _log.Warn($"Th={threadId} - RGOLIssueID is missing");
            }

            //if (request.DepartureDateTime == null)
            //    sw.WriteLine("DepartureDateTime : " + request.DepartureDateTime);
            //else
            //    sw.WriteLine("DepartureDateTime is missing");

            if (request.DepartureDateTime == null)
            {
                _log.Debug($"Th={threadId} - DepartureDateTime : {request.DepartureDateTime}");
            }
            else
            {
                _log.Warn($"Th={threadId} - DepartureDateTime is missing");
            }

            //if (!string.IsNullOrEmpty(request.ExperiencedDelay))
            //    sw.WriteLine("ExperiencedDelay : " + request.ExperiencedDelay);
            //else
            //    sw.WriteLine("ExperiencedDelay is missing");

            if (!string.IsNullOrEmpty(request.ExperiencedDelay))
            {
                _log.Debug($"Th={threadId} - ExperiencedDelay : {request.ExperiencedDelay}");
            }
            else
            {
                _log.Warn($"Th={threadId} - ExperiencedDelay is missing");
            }

            //if (!string.IsNullOrEmpty(request.SocialSecurityNumber))
            //    sw.WriteLine("SocialSecurityNumber : " + request.SocialSecurityNumber);
            //else
            //    sw.WriteLine("SocialSecurityNumber is missing");

            if (!string.IsNullOrEmpty(request.SocialSecurityNumber))
            {
                _log.Debug($"Th={threadId} - SocialSecurityNumber : {request.SocialSecurityNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - SocialSecurityNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.Address_Line1))
            //    sw.WriteLine("Address_Line1 : " + request.Address_Line1);
            //else
            //    sw.WriteLine("Address_Line1 is missing");

            if (!string.IsNullOrEmpty(request.Address_Line1))
            {
                _log.Debug($"Th={threadId} - Address_Line1 : {request.Address_Line1}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Address_Line1 is missing");
            }

            //if (!string.IsNullOrEmpty(request.Address_Line2))
            //    sw.WriteLine("Address_Line2 : " + request.Address_Line2);
            //else
            //    sw.WriteLine("Address_Line2 is missing");

            if (!string.IsNullOrEmpty(request.Address_Line2))
            {
                _log.Debug($"Th={threadId} - Address_Line2 : {request.Address_Line2}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Address_Line2 is missing");
            }

            //if (!string.IsNullOrEmpty(request.Address_PostalCode))
            //    sw.WriteLine("Address_PostalCode : " + request.Address_PostalCode);
            //else
            //    sw.WriteLine("Address_PostalCode is missing");

            if (!string.IsNullOrEmpty(request.Address_PostalCode))
            {
                _log.Debug($"Th={threadId} - Address_PostalCode : {request.Address_PostalCode}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Address_PostalCode is missing");
            }

            //if (!string.IsNullOrEmpty(request.Address_City))
            //    sw.WriteLine("Address_City : " + request.Address_City);
            //else
            //    sw.WriteLine("Address_City is missing");

            if (!string.IsNullOrEmpty(request.Address_City))
            {
                _log.Debug($"Th={threadId} - Address_City : {request.Address_City}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Address_City is missing");
            }

            //if (!string.IsNullOrEmpty(request.Address_Country))
            //    sw.WriteLine("Address_Country : " + request.Address_Country);
            //else
            //    sw.WriteLine("Address_Country is missing");

            if (!string.IsNullOrEmpty(request.Address_Country))
            {
                _log.Debug($"Th={threadId} - Address_Country : {request.Address_Country}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Address_Country is missing");
            }

            //if (!string.IsNullOrEmpty(request.TicketType1))
            //    sw.WriteLine("TicketType1 : " + request.TicketType1);
            //else
            //    sw.WriteLine("TicketType1 is missing");

            if (!string.IsNullOrEmpty(request.TicketType1))
            {
                _log.Debug($"Th={threadId} - TicketType1 : {request.TicketType1}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TicketType1 is missing");
            }

            //if (!string.IsNullOrEmpty(request.TicketNumber1))
            //    sw.WriteLine("TicketNumber1 : " + request.TicketNumber1);
            //else
            //    sw.WriteLine("TicketNumber1 is missing");

            if (!string.IsNullOrEmpty(request.TicketNumber1))
            {
                _log.Debug($"Th={threadId} - TicketNumber1 : {request.TicketNumber1}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TicketNumber1 is missing");
            }

            //if (!string.IsNullOrEmpty(request.TicketType2))
            //    sw.WriteLine("TicketType2 : " + request.TicketType2);
            //else
            //    sw.WriteLine("TicketType2 is missing");

            if (!string.IsNullOrEmpty(request.TicketType2))
            {
                _log.Debug($"Th={threadId} - TicketType2 : {request.TicketType2}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TicketType2 is missing");
            }

            //if (!string.IsNullOrEmpty(request.TicketNumber2))
            //    sw.WriteLine("TicketNumber2 : " + request.TicketNumber2);
            //else
            //    sw.WriteLine("TicketNumber2 is missing");

            if (!string.IsNullOrEmpty(request.TicketNumber2))
            {
                _log.Debug($"Th={threadId} - TicketNumber2 : {request.TicketNumber2}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TicketNumber2 is missing");
            }

            //if (!string.IsNullOrEmpty(request.MileageFrom))
            //    sw.WriteLine("MileageFrom : " + request.MileageFrom);
            //else
            //    sw.WriteLine("MileageFrom is missing");

            if (!string.IsNullOrEmpty(request.MileageFrom))
            {
                _log.Debug($"Th={threadId} - MileageFrom : {request.MileageFrom}");
            }
            else
            {
                _log.Warn($"Th={threadId} - MileageFrom is missing");
            }

            //if (!string.IsNullOrEmpty(request.MileageTo))
            //    sw.WriteLine("MileageTo : " + request.MileageTo);
            //else
            //    sw.WriteLine("MileageTo is missing");

            if (!string.IsNullOrEmpty(request.MileageTo))
            {
                _log.Debug($"Th={threadId} - MileageTo : {request.MileageTo}");
            }
            else
            {
                _log.Warn($"Th={threadId} - MileageTo is missing");
            }

            //if (!string.IsNullOrEmpty(request.MileageKilometers))
            //    sw.WriteLine("MileageKilometers : " + request.MileageKilometers);
            //else
            //    sw.WriteLine("MileageKilometers is missing");

            if (!string.IsNullOrEmpty(request.MileageKilometers))
            {
                _log.Debug($"Th={threadId} - MileageKilometers : {request.MileageKilometers}");
            }
            else
            {
                _log.Warn($"Th={threadId} - MileageKilometers is missing");
            }

            //if (!string.IsNullOrEmpty(request.MileageLicencePlateNumber))
            //    sw.WriteLine("MileageLicencePlateNumber : " + request.MileageLicencePlateNumber);
            //else
            //    sw.WriteLine("MileageLicencePlateNumber is missing");

            if (!string.IsNullOrEmpty(request.MileageLicencePlateNumber))
            {
                _log.Debug($"Th={threadId} - MileageLicencePlateNumber : {request.MileageLicencePlateNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - MileageLicencePlateNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.TaxiFrom))
            //    sw.WriteLine("TaxiFrom : " + request.TaxiFrom);
            //else
            //    sw.WriteLine("TaxiFrom is missing");

            if (!string.IsNullOrEmpty(request.TaxiFrom))
            {
                _log.Debug($"Th={threadId} - TaxiFrom : {request.TaxiFrom}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TaxiFrom is missing");
            }

            //if (!string.IsNullOrEmpty(request.TaxiTo))
            //    sw.WriteLine("TaxiTo : " + request.TaxiTo);
            //else
            //    sw.WriteLine("TaxiTo is missing");

            if (!string.IsNullOrEmpty(request.TaxiTo))
            {
                _log.Debug($"Th={threadId} - TaxiTo : {request.TaxiTo}");
            }
            else
            {
                _log.Warn($"Th={threadId} - TaxiTo is missing");
            }

            //sw.WriteLine("TaxiClaimedAmount : " + request.TaxiClaimedAmount);
            _log.Debug($"Th={threadId} - TaxiClaimedAmount : {request.TaxiClaimedAmount}");

            if (request.FileLinks != null && request.FileLinks.Length > 0)
            {
                foreach (var link in request.FileLinks)
                {
                    if (link.Url != null && link.Url != "")
                    {
                        //sw.WriteLine("FileLink.Url : " + link.Url);
                        _log.Debug($"Th={threadId} - FileLink.Url : {link.Url}");
                    }
                    else if (link.Url == null)
                    {
                        //sw.WriteLine("FileLink.Url is null");
                        _log.Debug($"Th={threadId} - FileLink.Url is null");
                    }
                    else
                    {
                        //sw.WriteLine("FileLink.Url is empty");
                        _log.Debug($"Th={threadId} - FileLink.Url is empty");
                    }
                }
            }
            else if (request.FileLinks == null)
            {
                //sw.WriteLine("FileLinks list is null");
                _log.Debug($"Th={threadId} - FileLinks list is null");
            }
            else if (request.FileLinks.Length <= 0)
            {
                //sw.WriteLine("FileLinks.Length <= 0");
                _log.Debug($"Th={threadId} - FileLinks.Length <= 0");
            }
            else 
            {
                //sw.WriteLine("Unexpected value in FileLinks");
                _log.Debug($"Th={threadId} - Unexpected value in FileLinks");
            }


            //if (!string.IsNullOrEmpty(request.Iban))
            //    sw.WriteLine("Iban : " + request.Iban);
            //else
            //    sw.WriteLine("Iban is missing");

            if (!string.IsNullOrEmpty(request.Iban))
            {
                _log.Debug($"Th={threadId} - Iban : {request.Iban}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Iban is missing");
            }

            //if (!string.IsNullOrEmpty(request.Bic))
            //    sw.WriteLine("Bic : " + request.Bic);
            //else
            //    sw.WriteLine("Bic is missing");

            if (!string.IsNullOrEmpty(request.Bic))
            {
                _log.Debug($"Th={threadId} - Bic : {request.Bic}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Bic is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerSocialSecurityNumber))
            //    sw.WriteLine("CustomerSocialSecurityNumber : " + request.CustomerSocialSecurityNumber);
            //else
            //    sw.WriteLine("CustomerSocialSecurityNumber is missing");

            if (!string.IsNullOrEmpty(request.CustomerSocialSecurityNumber))
            {
                _log.Debug($"Th={threadId} - CustomerSocialSecurityNumber : {request.CustomerSocialSecurityNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerSocialSecurityNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerAddress1Line1))
            //    sw.WriteLine("CustomerAddress1Line1 : " + request.CustomerAddress1Line1);
            //else
            //    sw.WriteLine("CustomerAddress1Line1 is missing");

            if (!string.IsNullOrEmpty(request.CustomerAddress1Line1))
            {
                _log.Debug($"Th={threadId} - CustomerAddress1Line1 : {request.CustomerAddress1Line1}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerAddress1Line1 is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerAddress1Line2))
            //    sw.WriteLine("CustomerAddress1Line2 : " + request.CustomerAddress1Line2);
            //else
            //    sw.WriteLine("CustomerAddress1Line2 is missing");

            if (!string.IsNullOrEmpty(request.CustomerAddress1Line2))
            {
                _log.Debug($"Th={threadId} - CustomerAddress1Line2 : {request.CustomerAddress1Line2}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerAddress1Line2 is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerAddress1Postalcode))
            //    sw.WriteLine("CustomerAddress1Postalcode : " + request.CustomerAddress1Postalcode);
            //else
            //    sw.WriteLine("CustomerAddress1Postalcode is missing");

            if (!string.IsNullOrEmpty(request.CustomerAddress1Postalcode))
            {
                _log.Debug($"Th={threadId} - CustomerAddress1Postalcode : {request.CustomerAddress1Postalcode}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerAddress1Postalcode is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerAddress1City))
            //    sw.WriteLine("CustomerAddress1City : " + request.CustomerAddress1City);
            //else
            //    sw.WriteLine("CustomerAddress1City is missing");

            if (!string.IsNullOrEmpty(request.CustomerAddress1City))
            {
                _log.Debug($"Th={threadId} - CustomerAddress1City : {request.CustomerAddress1City}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerAddress1City is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerAddress1Country))
            //    sw.WriteLine("CustomerAddress1Country : " + request.CustomerAddress1Country);
            //else
            //    sw.WriteLine("CustomerAddress1Country is missing");

            if (!string.IsNullOrEmpty(request.CustomerAddress1Country))
            {
                _log.Debug($"Th={threadId} - CustomerAddress1Country : {request.CustomerAddress1Country}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerAddress1Country is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerTelephonenumber))
            //    sw.WriteLine("CustomerTelephonenumber : " + request.CustomerTelephonenumber);
            //else
            //    sw.WriteLine("CustomerTelephonenumber is missing");

            if (!string.IsNullOrEmpty(request.CustomerTelephonenumber))
            {
                _log.Debug($"Th={threadId} - CustomerTelephonenumber : {request.CustomerTelephonenumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerTelephonenumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerFirstName))
            //    sw.WriteLine("CustomerFirstName : " + request.CustomerFirstName);
            //else
            //    sw.WriteLine("CustomerFirstName is missing");

            if (!string.IsNullOrEmpty(request.CustomerFirstName))
            {
                _log.Debug($"Th={threadId} - CustomerFirstName : {request.CustomerFirstName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerFirstName is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomerLastName))
            //    sw.WriteLine("CustomerLastName : " + request.CustomerLastName);
            //else
            //    sw.WriteLine("CustomerLastName is missing");

            if (!string.IsNullOrEmpty(request.CustomerLastName))
            {
                _log.Debug($"Th={threadId} - CustomerLastName : {request.CustomerLastName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomerLastName is missing");
            }

            _log.Info($"Th={threadId} - ===================== Finished Checking Parameters. =====================");
            //sw.Flush();
            //sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private void CreateLogFile(int threadId, CreateCaseRequest request)
        {
            if (request == null) return;
            //var sw = new StreamWriter("E:\\Logs\\CRM\\CGIXrmCreateCaseService\\RequestCreateCase_Log\\CreateLogFile_Log.txt", true);
            //sw.WriteLine("======================================================================================================================");
            //sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));

            _log.Info($"Th={threadId} - ===================== Checking Missing Parameters. =====================");
            
            //if (!string.IsNullOrEmpty(request.CustomersCategory))
            //    sw.WriteLine("CustomersCategory : " + request.CustomersCategory);

            //else
            //    sw.WriteLine("CustomersCategory is missing");

            if (!string.IsNullOrEmpty(request.CustomersCategory))
            {
                _log.Debug($"Th={threadId} - CustomersCategory : {request.CustomersCategory}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomersCategory is missing");
            }

            //if (!string.IsNullOrEmpty(request.CustomersSubcategory))
            //    sw.WriteLine("CustomersSubcategory : " + request.CustomersSubcategory);
            //else
            //    sw.WriteLine("CustomersSubcategory is missing");

            if (!string.IsNullOrEmpty(request.CustomersSubcategory))
            {
                _log.Debug($"Th={threadId} - CustomersSubcategory : {request.CustomersSubcategory}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CustomersSubcategory is missing");
            }

            //if (!string.IsNullOrEmpty(request.Title))
            //    sw.WriteLine("Title : " + request.Title);
            //else
            //    sw.WriteLine("Title is missing");

            if (!string.IsNullOrEmpty(request.Title))
            {
                _log.Debug($"Title : {request.Title}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Title is missing");
            }

            //if (!string.IsNullOrEmpty(request.Description))
            //    sw.WriteLine("Description : " + request.Description);
            //else
            //    sw.WriteLine("Description is missing");

            if (!string.IsNullOrEmpty(request.Description))
            {
                _log.Debug($"Th={threadId} - Description : {request.Description}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Description is missing");
            }

            //if (request.Customer != null && request.Customer != Guid.Empty)
            //    sw.WriteLine("Customer : " + request.Customer);
            //else
            //    sw.WriteLine("Customer is missing");

            if (request.Customer != null && request.Customer != Guid.Empty)
            {
                _log.Debug($"Th={threadId} - Customer : {request.Customer}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Customer is missing");
            }

            switch (request.CustomerType)
            {
                case CustomerType.Private:
                    //sw.WriteLine("Customer : Private");
                    _log.Debug($"Th={threadId} - Customer : Private");
                    break;
                case CustomerType.Organisation:
                    //sw.WriteLine("Customer : Organisation");
                    _log.Debug($"Th={threadId} - Customer : Organisation");
                    break;
                default:
                    //sw.WriteLine("Customer is missing");
                    _log.Warn($"Th={threadId} - CustomerType is missing");
                    break;
            }

            //if (!string.IsNullOrEmpty(request.InvoiceNumber))
            //    sw.WriteLine("InvoiceNumber : " + request.InvoiceNumber);
            //else
            //    sw.WriteLine("InvoiceNumber is missing");

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                _log.Debug($"Th={threadId} - InvoiceNumber : {request.InvoiceNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - InvoiceNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.ControlFeeNumber))
            //    sw.WriteLine("ControlFeeNumber : " + request.ControlFeeNumber);
            //else
            //    sw.WriteLine("ControlFeeNumber is missing");

            if (!string.IsNullOrEmpty(request.InvoiceNumber))
            {
                _log.Debug($"Th={threadId} - InvoiceNumber : {request.InvoiceNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - InvoiceNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.CardNumber))
            //    sw.WriteLine("CardNumber : " + request.CardNumber);
            //else
            //    sw.WriteLine("CardNumber is missing");

            if (!string.IsNullOrEmpty(request.CardNumber))
            {
                _log.Debug($"Th={threadId} - CardNumber : {request.CardNumber}");
            }
            else
            {
                _log.Warn($"Th={threadId} - CardNumber is missing");
            }

            //if (!string.IsNullOrEmpty(request.WayOfTravel))
            //    sw.WriteLine("WayOfTravel : " + request.WayOfTravel);
            //else
            //    sw.WriteLine("WayOfTravel is missing");

            if (!string.IsNullOrEmpty(request.WayOfTravel))
            {
                _log.Debug($"Th={threadId} - WayOfTravel : {request.WayOfTravel}");
            }
            else
            {
                _log.Warn($"Th={threadId} - WayOfTravel is missing");
            }

            //if (!string.IsNullOrEmpty(request.Line))
            //    sw.WriteLine("Line : " + request.Line);
            //else
            //    sw.WriteLine("Line is missing");

            if (!string.IsNullOrEmpty(request.Line))
            {
                _log.Debug($"Th={threadId} - Line : {request.Line}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Line is missing");
            }

            //if (!string.IsNullOrEmpty(request.City))
            //    sw.WriteLine("City : " + request.City);
            //else
            //    sw.WriteLine("City is missing");

            if (!string.IsNullOrEmpty(request.City))
            {
                _log.Debug($"Th={threadId} - City : {request.City}");
            }
            else
            {
                _log.Warn($"Th={threadId} - City is missing");
            }

            //if (!string.IsNullOrEmpty(request.Train))
            //    sw.WriteLine("Train : " + request.Train);
            //else
            //    sw.WriteLine("Train is missing");

            if (!string.IsNullOrEmpty(request.Train))
            {
                _log.Debug($"Th={threadId} - Train : {request.Train}");
            }
            else
            {
                _log.Warn($"Th={threadId} - Train is missing");
            }

            //if (!string.IsNullOrEmpty(request.County))
            //    sw.WriteLine("County : " + request.County);
            //else
            //    sw.WriteLine("County is missing");

            if (!string.IsNullOrEmpty(request.County))
            {
                _log.Debug($"Th={threadId} - County : {request.County}");
            }
            else
            {
                _log.Warn($"Th={threadId} - County is missing");
            }

            //if (!string.IsNullOrEmpty(request.FirstName))
            //    sw.WriteLine("firstName : " + request.FirstName);
            //else
            //    sw.WriteLine("firstName is missing");

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                _log.Debug($"Th={threadId} - FirstName : {request.FirstName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - FirstName is missing");
            }

            //if (!string.IsNullOrEmpty(request.LastName))
            //    sw.WriteLine("lastName : " + request.LastName);
            //else
            //    sw.WriteLine("lastName is missing");

            if (!string.IsNullOrEmpty(request.LastName))
            {
                _log.Debug($"Th={threadId} - LastName : {request.LastName}");
            }
            else
            {
                _log.Warn($"Th={threadId} - LastName is missing");
            }

            //if (!string.IsNullOrEmpty(request.EmailAddress))
            //    sw.WriteLine("emailAddress : " + request.EmailAddress);
            //else
            //    sw.WriteLine("emailAddress is missing");

            if (!string.IsNullOrEmpty(request.EmailAddress))
            {
                _log.Debug($"Th={threadId} - EmailAddress : {request.EmailAddress}");
            }
            else
            {
                _log.Warn($"Th={threadId} - EmailAddress is missing");
            }

            switch (request.ContactCustomer)
            {
                case true:
                    //sw.WriteLine("ContactCustomer : true");
                    _log.Debug($"Th={threadId} - ContactCustomer : true");
                    break;
                case false:
                    //sw.WriteLine("ContactCustomer : false");
                    _log.Debug($"Th={threadId} - ContactCustomer : false");
                    break;
                default:
                    //sw.WriteLine("ContactCustomer is missing");
                    _log.Warn($"Th={threadId} - ContactCustomer is missing");
                    break;
            }

            if (request.FileLinks != null && request.FileLinks.Count > 0)
            {
                foreach (var link in request.FileLinks)
                {
                    if (link.Url != null && link.Url != "")
                    {
                        //sw.WriteLine("FileLink.Url : " + link.Url);
                        _log.Debug($"Th={threadId} - FileLink.Url : {link.Url}");
                    }
                    else if (link.Url == null)
                    {
                        //sw.WriteLine("FileLink.Url is null");
                        _log.Debug($"Th={threadId} - FileLink.Url is null");
                    }
                    else 
                    {
                        //sw.WriteLine("FileLink.Url is empty");
                        _log.Debug($"Th={threadId} - FileLink.Url is empty");
                    }
                }
            }
            else if (request.FileLinks == null)
            {
                //sw.WriteLine("FileLinks list is null");
                _log.Debug($"Th={threadId} - FileLinks list is null");
            }
            else if (request.FileLinks.Count <= 0)
            {
                //sw.WriteLine("FileLinks.Count <= 0");
                _log.Debug($"Th={threadId} - FileLinks.Count <= 0");
            }
            else
            {
                //sw.WriteLine("Unexpected value in FileLinks");
                _log.Debug($"Th={threadId} - Unexpected value in FileLinks");
            }

            _log.Info($"Th={threadId} - ===================== Finished Checking Parameters. =====================");
            //sw.Flush();
            //sw.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundId"></param>
        /// <param name="state"></param>
        /// <param name="status"></param>
        private void SetRefundDescisionStatus(Guid refundId, int state, int status)
        {
            //State value: 0 = active, corresponding Status values: 1 = New
            //State value: 1 = inactive, corresponding Status values: 2 = Declined, 285050000 = Approved, 285050001 =Approved -Transaction failed

            _log.Debug($"Entering {nameof(SetRefundDescisionStatus)}.");

            _log.Debug($"RefundId: {refundId}.");
            _log.Debug($"State: {state}.");
            _log.Debug($"Status: {status}.");

            var statusrequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference("cgi_refund", refundId),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };

            _xrmManager.Service.Execute(statusrequest);

            _log.Debug($"Exiting {nameof(SetRefundDescisionStatus)}.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private QueryByAttribute CreateQueryAttribute(string entityName, string[] columns, string[] attributes, object[] values)
        {
            var query = new QueryByAttribute(entityName)
            {
                ColumnSet = new ColumnSet()
            };

            query.ColumnSet.AddColumns(columns);
            query.Attributes.AddRange(attributes);
            query.Values.AddRange(values);

            return query;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="state"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private static SetStateRequest CreateStateRequest(string entityName, Guid entityId, int state, int status)
        {
            var stateRequest = new SetStateRequest
            {
                EntityMoniker = new EntityReference(entityName, entityId),
                State = new OptionSetValue(state),
                Status = new OptionSetValue(status)
            };

            return stateRequest;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="incidentId"></param>
        private void CloseIncident(Guid incidentId)
        {
            _log.Debug($"Entering {nameof(CloseIncident)}");

            SetCaseReopen(incidentId);

            var caseresolution = new Entity("incidentresolution");
            caseresolution.Attributes.Add("incidentid", new EntityReference("incident", incidentId));
            caseresolution.Attributes.Add("subject", "Problemet löst.");

            var closerequest = new CloseIncidentRequest()
            {
                IncidentResolution = caseresolution,
                Status = new OptionSetValue(5)
            };

            _log.Debug($"Closing incident '{incidentId}' with status 5.");

            _xrmManager.Service.Execute(closerequest);

            _log.Debug($"Exiting {nameof(CloseIncident)}");
        }

        /// <summary>
        /// Adds RGOL data to case in fields and annotation.
        /// This is used for queueitems, views and quickforms.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="incident"></param>
        /// <param name="setting"></param>
        private void UpdateCaseWithRgolData(UpdateAutoRgCaseRequest request, Incident incident, RgolSetting setting, int incidentStageCode)
        {
            _log.Debug($"Entering {nameof(UpdateCaseWithRgolData)}");

            Entity cgiSetting = GetSettings(_xrmManager.Service, new[] { "cgi_category_detail1id", "cgi_category_detail2id", "cgi_category_detail3id" });
            Entity incidentEntity = new Entity("incident");

            incidentEntity.Id = incident.IncidentId;
            incidentEntity["cgi_iscompleted"] = request.IsCompleted;
            incidentEntity["cgi_rgolcaselog"] = request.RGOLCaseLog;
            incidentEntity["cgi_compensationclaimfromrgol"] = new Money(request.CompensationClaimFromRGOL);
            incidentEntity["cgi_refundtypes"] = new EntityReference("cgi_refundtype", new Guid(setting.RefundTypeId));
            incidentEntity["cgi_casdet_row1_cat1id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail1id");
            incidentEntity["cgi_casdet_row1_cat2id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail2id");
            incidentEntity["cgi_casdet_row1_cat3id"] = cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail3id");
            incidentEntity["incidentstagecode"] = new OptionSetValue(incidentStageCode);

            _log.Debug($"cgi_iscompleted: {request.IsCompleted}.");
            _log.Debug($"cgi_rgolcaselog: {request.RGOLCaseLog}.");
            _log.Debug($"cgi_compensationclaimfromrgol: {new Money(request.CompensationClaimFromRGOL)}.");
            _log.Debug($"cgi_refundtypes: {setting.RefundTypeId}.");
            _log.Debug($"cgi_casdet_row1_cat1id: {cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail1id")}.");
            _log.Debug($"cgi_casdet_row1_cat2id: {cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail2id")}.");
            _log.Debug($"cgi_casdet_row1_cat3id: {cgiSetting.GetAttributeValue<EntityReference>("cgi_category_detail3id")}.");
            _log.Debug($"incidentstagecode: {incidentStageCode}.");


            // Could be null in 'Avslag' as it doesnt have a reimbursement form
            if (string.IsNullOrEmpty(setting.ReImBursementFormId) == false)
            {
                incidentEntity["cgi_refundreimbursementform"] = new EntityReference("cgi_reimbursementform", new Guid(setting.ReImBursementFormId));
                _log.Debug($"cgi_refundreimbursementform: {setting.ReImBursementFormId}.");
            }
            if (request.IsCompleted == true)
            {
                incidentEntity["cgi_refundapprovaltype"] = request.Approved ? "Bifall" : "Avslag";
                _log.Debug($"cgi_refundapprovaltype: {request.Approved}.");
            }

            if (!string.IsNullOrEmpty(request.CustomerDemand))
            {
                incidentEntity["cgi_customer_demand"] = request.CustomerDemand;
                _log.Debug($"cgi_customer_demand: {request.CustomerDemand}.");
            }

            _xrmManager.Service.Update(incidentEntity);



            _log.Debug($"Exiting {nameof(UpdateCaseWithRgolData)}");
        }
        /// <summary>
        /// Sätter cgi_case_reopen till 1
        /// </summary>
        /// <param name="IncidentId"></param>
        private void SetCaseReopen(Guid IncidentId)
        {
            _log.Debug($"Entering {nameof(SetCaseReopen)}");
            Entity incidentEntity = new Entity("incident");
            incidentEntity.Id = IncidentId;

            incidentEntity["cgi_case_reopen"] = "1";

            _xrmManager.Service.Update(incidentEntity);
            _log.Debug($"Updating incident '{IncidentId}' cgi_case_reopen with value 1.");

            _log.Debug($"Exiting {nameof(SetCaseReopen)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseId"></param>
        private void RouteIncidentToRgolQueue(Guid caseId)
        {
            EntityReference reference = GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultqueue");

            AddToQueueRequest addToSourceQueue = new AddToQueueRequest
            {
                DestinationQueueId = reference.Id,
                Target = new EntityReference("incident", caseId),
            };

            OrganizationResponse response = _xrmManager.Service.Execute(addToSourceQueue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseId"></param>
        private void RouteIncidentToRgolQueue(Guid caseId, string queueId)
        {
            _log.Debug($"Entering {nameof(RouteIncidentToRgolQueue)}");
            // Try getting Queue passed from RGOL
            Entity queue = _xrmManager.Service.Retrieve("queue", new Guid(queueId), new ColumnSet(false));

            // Use Queue from RGOL if exists
            if (queue == null)
            {
                // Get Default Queue if it does not exists
                EntityReference queueDefault = GetSetting<EntityReference>(_xrmManager.Service, "cgi_case_rgol_defaultqueue");

                AddToQueueRequest addToSourceQueueDefault = new AddToQueueRequest
                {
                    DestinationQueueId = queueDefault.Id,
                    Target = new EntityReference("incident", caseId),
                };

                OrganizationResponse responseDefault = _xrmManager.Service.Execute(addToSourceQueueDefault);
            }
            else
            {
                AddToQueueRequest addToSourceQueue = new AddToQueueRequest
                {
                    DestinationQueueId = queue.Id,
                    Target = new EntityReference("incident", caseId),
                };

                OrganizationResponse response = _xrmManager.Service.Execute(addToSourceQueue);
            }

            _log.Debug($"Exiting {nameof(RouteIncidentToRgolQueue)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="serviceAttributeName"></param>
        /// <see cref="GetSetting{T}(IOrganizationService, string)"/>
        /// <returns></returns>
        private static T Ed_GetSetting<T>(IOrganizationService service, string serviceAttributeName)
        {
            //Endeavor VC - Edited for handling attributes that doesn't get fetched due to null value in CRM.
            var settings = Ed_GetSettings(service, new[] { serviceAttributeName });
            if (settings == null)
                return default(T);

            if (settings.Contains(serviceAttributeName))
                return settings.GetAttributeValue<T>(serviceAttributeName);
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceAttributeNames"></param>
        /// <see cref="GetSettings(IOrganizationService, string[])"/>
        /// <returns></returns>
        private static Entity Ed_GetSettings(IOrganizationService service, string[] serviceAttributeNames)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            for (int i = 0; i < serviceAttributeNames.Length; i++)
            {
                _xml += "       <attribute name='" + serviceAttributeNames[i] + "' />";
            }
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            //for (int i = 0; i < serviceAttributeNames.Length; i++)
            //{
            //    string serviceAttributeName = serviceAttributeNames[i];

            //    for (int j = 0; j < settingscollection.Entities.Count; j++)
            //    {
            //        Entity setting = settingscollection.Entities[j];

            //        //Endeavor VC - Changed && to || and assign null value to settingscollection, 2019-04-11
            //        if (!setting.Contains(serviceAttributeName) || setting[serviceAttributeName] == null)
            //        {

            //            //throw new Exception("Required setting is missing: " + serviceAttributeName);
            //        }
            //    }
            //}
            return settingscollection.Entities[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="serviceAttributeName"></param>
        /// <returns></returns>
        private static T GetSetting<T>(IOrganizationService service, string serviceAttributeName)
        {
            return GetSettings(service, new[] { serviceAttributeName }).GetAttributeValue<T>(serviceAttributeName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="serviceAttributeNameInLowerCase"></param>
        /// <returns></returns>
        private static Entity GetSettings(IOrganizationService service, string[] serviceAttributeNames)
        {
            #region FetchXML

            string _now = DateTime.Now.ToString("s");
            string _xml = "";
            _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            _xml += "   <entity name='cgi_setting'>";
            _xml += "       <attribute name='cgi_settingid' />";
            for (int i = 0; i < serviceAttributeNames.Length; i++)
            {
                _xml += "       <attribute name='" + serviceAttributeNames[i] + "' />";
            }
            _xml += "       <filter type='and'>";
            _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
            _xml += "           <filter type='or'>";
            _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
            _xml += "               <condition attribute='cgi_validto' operator='null' />";
            _xml += "           </filter>";
            _xml += "       </filter>";
            _xml += "   </entity>";
            _xml += "</fetch>";

            #endregion

            FetchExpression _f = new FetchExpression(_xml);
            EntityCollection settingscollection = service.RetrieveMultiple(_f);

            for (int i = 0; i < serviceAttributeNames.Length; i++)
            {
                string serviceAttributeName = serviceAttributeNames[i];

                for (int j = 0; j < settingscollection.Entities.Count; j++)
                {
                    Entity setting = settingscollection.Entities[j];

                    if (!setting.Contains(serviceAttributeName) || setting[serviceAttributeName] == null)
                    {
                        throw new Exception("Required setting is missing: " + serviceAttributeName);
                    }
                }
            }
            return settingscollection.Entities[0];
        }


        //Create Incident from web.
        internal void RequestCreateCase(int threadId, CreateCaseRequest request)
        {
            _log.Info($"Th={threadId} - Entered {nameof(RouteIncidentToRgolQueue)}");
            CreateLogFile(threadId, request);
            Account account = null;
            try
            {

                if (request.Customer != null && request.Customer.Value != null && request.Customer.Value != Guid.Empty)
                {
                    account = GetAccountFromId((Guid)request.Customer, request.CustomerType);
                    if (account == null)
                        throw new Exception($"Kunden med id {request.Customer} hittas inte i databasen!");

                    account.RecordCount = account.RecordCount;
                }
                else
                {
                    if (request.ContactCustomer)
                    {
                        // If a contact, call Fasad.
                        if (request.CustomerType == CustomerType.Private)
                        {
                            //Create private customer (contact) from firstname, lastname and emailaddress
                            account = new Account();
                            Guid contactGuid = GetOrCreateContactCase(request.FirstName, request.LastName, request.EmailAddress, request.MobilePhoneNumber);

                            // Default to anonymous customer
                            if (contactGuid == Guid.Empty)
                            {
                                var setting = GetDefaultCustomer();

                                if (setting != null)
                                    contactGuid = new Guid(setting.DefaultCustomer);
                                // Change request to organisation, default is an account...
                                request.CustomerType = CustomerType.Organisation;
                            }

                            account.AccountId = contactGuid;
                            account.RecordCount = 1;
                        }
                        else
                        {
                            //var acc = GetAccountFromEmail(request.EmailAddress, request.CustomerType);
                            var acc = GetAccountFromEmail(request.EmailAddress, request.CustomerType);
                            if (acc != null)
                            {
                                if (acc.RecordCount == 1)
                                {
                                    account = new Account
                                    {
                                        AccountId = acc.AccountId,
                                        RecordCount = acc.RecordCount
                                    };
                                }
                                else
                                {
                                    var setting = GetDefaultCustomer();

                                    if (setting != null)
                                    {
                                        account = new Account
                                        {
                                            AccountId = new Guid(setting.DefaultCustomer),
                                            AccountName = "CGI_DEFAULT",
                                            RecordCount = acc.RecordCount
                                        };
                                    }
                                    else
                                        throw new Exception("Det finns ingen default kund definierad!");
                                }
                            }
                            else
                            {
                                //Create private customer (contact) from firstname, lastname and emailaddress
                                account = new Account();
                                Guid contactGuid = GetOrCreateContactCase(request.FirstName, request.LastName, request.EmailAddress, request.MobilePhoneNumber);
                                // Default to anonymous customer
                                if (contactGuid == Guid.Empty)
                                {
                                    var setting = GetDefaultCustomer();

                                    if (setting != null)
                                        contactGuid = new Guid(setting.DefaultCustomer);
                                }
                                account.AccountId = contactGuid;
                                account.RecordCount = 1;
                            }
                        }
                    }
                    else
                    {
                        // Connect to defualt customer
                        account = new Account
                        {
                            RecordCount = 1
                        };
                        var setting = GetDefaultCustomer();

                        if (setting != null)
                            account.AccountId = new Guid(setting.DefaultCustomer);
                        // Change request to organisation, default is an account...
                        request.CustomerType = CustomerType.Organisation;
                    }
                }

                //create case from request
                var incidentId = CreateIncident(request, account);

                //add documents from collection
                if (request.DocumentList != null && request.DocumentList.Any())
                {
                    foreach (var doc in request.DocumentList)
                    {
                        CreateAnnotation(doc, incidentId);
                    }
                }
                //add filelinks from collection
                //Change - Handle Decryption of link
                if (request.FileLinks == null || !request.FileLinks.Any()) return;

                foreach (var fileLink in request.FileLinks)
                {
                    CreateFileLink(fileLink, incidentId);
                }
                _log.Info($"Th={threadId} - Exeting {nameof(RouteIncidentToRgolQueue)}");
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    //_log.Error(string.Format("Exception {0}, inner:{1}", ex.Message, ex.InnerException.Message));
                    _log.Error($"Th={threadId} - Exception {ex.Message}, inner:{ex.InnerException.Message}");
                }

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal AutoRgCaseResponse RequestCreateAutoRgCase(int threadId, AutoRgCaseRequest request)
        {
            // Do debug-tracing. Johan Endeavor
            //_log.Debug(string.Format("Entered RequestCreateAutoRgCase"));
            _log.Info($"Th={threadId} - Entered {nameof(RequestCreateAutoRgCase)}");
            CreateLogFileAutoRGCase(threadId, request);
            

            var response = new AutoRgCaseResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            //dublettkontroll, finns redan ett ärende för detta RGOLIssueID?
            QueryBase query = CreateQueryAttribute("incident", new string[] { "incidentid", "ticketnumber" }, new string[] { "cgi_rgolissueid" }, new object[] { request.RGOLIssueID });
            EntityCollection entityResults = _xrmManager.Service.RetrieveMultiple(query);
            if (entityResults.Entities.Count() > 0)
            {
                response.CaseId = (Guid)entityResults.Entities[0]["incidentid"];

                if (entityResults.Entities[0].Contains("ticketnumber") && entityResults.Entities[0]["ticketnumber"] != null)
                    response.CaseNumber = (string)entityResults.Entities[0]["ticketnumber"];

                response.Success = false;
                response.ErrorMessage = "Duplicate RGOLIssueID";//Do not change this error message. Service consumer code may use it to identify duplicates.

                return response;
            }
             
            try
            {
                Account accountOrContact;
                string errorMessage;

                //För organisationskunder krävs att användaren är inloggad och således krävs ett kundid 
                if (request.IsOrganisation && request.IsLoggedIn == false)
                    throw new Exception("CustomerId, which was not supplied, is required to handle caseregistration for CustomerType 1 = Organization!");

                // Skall account sökas?
                // **********************************
                if (request.IsOrganisation || request.CustomerType == (int)CustomerType.Organisation)
                {
                    bool isAccountOrContactInCRM = TryIdentifyAccountOrContactFromAutoRg(request, out accountOrContact, out errorMessage);
                    if (string.IsNullOrEmpty(errorMessage) == false)
                        throw new Exception(errorMessage);
                }
                else
                {
                    // Hämta kunduppgifter från fasad (contact)
                    Guid contactId = GetOrCreateContactRGOLCase(request);

                    // These values are used in CreateIncidentRgol since oldtimes: TODO check for refac possibility
                    Account contact = new Account()
                    {
                        AccountId = contactId,
                        AccountName = string.Empty,
                        RecordCount = 1,
                    };
                    accountOrContact = contact;
                }

                var caseId = CreateIncidentRgol(request, accountOrContact);

                if (!string.IsNullOrEmpty(request.QueueId))
                {
                    RouteIncidentToRgolQueue(caseId, request.QueueId);
                }
                else
                {
                    RouteIncidentToRgolQueue(caseId);
                }
                response.CaseId = caseId;

                Incident incidentCase = GetIncidentFromId(caseId);
                response.CaseNumber = incidentCase.TicketNumber;

                response.Success = true;
            }
            catch (FaultException faultex)
            {
                _log.Debug($"Th={threadId} - Fel vid skapande av contact: {faultex.Message}");
                if (faultex.Message != null)
                    _log.Error($"Th={threadId} - Exception {faultex.Message}, inner:{faultex.InnerException?.Message}");

                response.CaseId = Guid.Empty;
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                    _log.Error($"Th={threadId} - Exception {ex.Message}, inner:{ex.InnerException?.Message}");

                response.CaseId = Guid.Empty;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// Create Incident from AutoRG
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal UpdateAutoRgCaseResponse RequestCreateAutoRgRefundDecision(UpdateAutoRgCaseRequest request)
        {
            UpdateAutoRgCaseResponse response = new UpdateAutoRgCaseResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    response.Success = false;
                    response.ErrorMessage = "Guid is guid empty format";
                    return response;
                }

                // Do debug-tracing. Johan Endeavor
                _log.Debug(string.Format("============================================"));
                _log.Debug(string.Format("Start RequestCreateAutoRgRefundDecision"));


                Incident incidentCase = GetIncidentFromId(request.CaseID);
                // TODO guard rule för fler än ett anrop till denna metod

                RgolSetting setting = GetRgolSettingFromRefundType(request.RefundType);

                BaseCurrency currency = GetCurrency();

                if (!string.IsNullOrEmpty(request.QueueId))
                {
                    RouteIncidentToRgolQueue(request.CaseID, request.QueueId);
                }

                if (request.IsCompleted == false && request.ReqReceipt)
                {
                    _log.Debug(string.Format("1. RequestCreateAutoRgRefundDecision"));

                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050002); // 285050002 | Not answered
                    // Incident status  1    In Progress | 3    Waiting for details
                    var stateRequest = CreateStateRequest("incident", incidentCase.IncidentId, 0, 3);
                    _xrmManager.Service.Execute(stateRequest);
                }
                else if (request.IsCompleted == false && request.ReqReceipt == false)
                {
                    _log.Debug(string.Format("2. RequestCreateAutoRgRefundDecision"));

                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050000); // 285050000 | Ongoing

                    // Incident status  1    In Progress | 3    Waiting for details
                    var stateRequest = CreateStateRequest("incident", incidentCase.IncidentId, 0, 1);

                    _xrmManager.Service.Execute(stateRequest);
                }
                else if (request.IsCompleted && request.Approved)
                {
                    _log.Debug(string.Format("3. RequestCreateAutoRgRefundDecision"));

                    response.RefundID = CreateDecisionRgol(request.Value, setting, incidentCase, currency.BaseCurrencyId, request.InternalMessage, request.CustomerMessage);
                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050006); // 285050006 | Resolved-Approved
                    SetRefundDescisionStatus(response.RefundID, 0, 285050005);// 285050005 = Approved
                    ReimbursementHandler rh = new ReimbursementHandler();
                    rh.ExecuteRefundAndUpdatesStatus(response.RefundID, _xrmManager.Service);
                }
                else if (request.IsCompleted && request.Approved == false)
                {
                    _log.Debug(string.Format("4. RequestCreateAutoRgRefundDecision"));

                    response.RefundID = CreateDecisionRgol(request.Value, setting, incidentCase, currency.BaseCurrencyId, request.InternalMessage, request.CustomerMessage);

                    // update case with information needed for queueitem, views and quick form + incidentstagecode
                    UpdateCaseWithRgolData(request, incidentCase, setting, 285050007); // 285050007 | Resolved-Denied
                    SetRefundDescisionStatus(response.RefundID, 0, 285050006);// 0 = Active, Declined = 285 050 006
                    CloseIncident(incidentCase.IncidentId);
                }
                else
                {
                    throw new Exception($"IsCompleted {request.IsCompleted}, request.ReqReceipt {request.ReqReceipt}, IsApproved {request.Approved} is not a valid combination.");
                }

                response.Success = true;
            }
            catch (FaultException faultex)
            {
                response.RefundID = Guid.Empty;
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.RefundID = Guid.Empty;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal UpdateAutoRgResponse RequestCreateBiffTransactionsLinkedToCase(UpdateAutoRgCaseBiffTransactionsRequest request)
        {
            var response = new UpdateAutoRgResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    throw new Exception("Request parameter CaseId is guid empty format.");
                }

                // Delete all current biff transactions as this method could be called several times.
                var guardErrorMessage = "More then 50 bifftransactions attached to case. Something is proberbly wrong.";
                DeleteCurrentEntitiesAttachedToCase(CgiBiffTransaction, request.CaseID, guardErrorMessage);

                foreach (var biffTrans in request.BiffTransactions)
                {
                    var biffTransEntity = new Entity(CgiBiffTransaction)
                    {
                        Attributes = new AttributeCollection
                        {
                            {"cgi_caseid", new EntityReference("incident", request.CaseID)}
                        }
                    };


                    if (string.IsNullOrEmpty(biffTrans.Amount) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_amount", biffTrans.Amount);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Cardsect) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_cardsect", biffTrans.Cardsect);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Date) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_date", biffTrans.Date);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Deviceid) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_deviceid", biffTrans.Deviceid);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Origzone) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_origzone", biffTrans.Origzone);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Origzonename) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_origzonename", biffTrans.Origzonename);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Rectype) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_rectype", biffTrans.Rectype);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Route) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_route", biffTrans.Route);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Time) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_time", biffTrans.Time);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Travelcard) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcard", biffTrans.Travelcard);
                    }
                    if (string.IsNullOrEmpty(biffTrans.TravelcardId) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcardid", biffTrans.TravelcardId);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Travelcardtransaction) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_travelcardtransaction", biffTrans.Travelcardtransaction);
                    }
                    if (string.IsNullOrEmpty(biffTrans.Txntype) == false)
                    {
                        biffTransEntity.Attributes.Add("cgi_txntype", biffTrans.Txntype);
                    }

                    _xrmManager.Create(biffTransEntity);
                }
                response.Success = true;

            }
            catch (FaultException faultex)
            {
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        public void LogMessage(string path, string message)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8);
                message = string.Format("{0} : {1}", DateTime.Now, message);
                sw.WriteLine(message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region OldRequestCreateTravelInformationLinkedToCase

        /*
        internal UpdateAutoRgResponse OldRequestCreateTravelInformationLinkedToCase(UpdateAutoRgCaseTravelInformationsRequest request)
        {
            var response = new UpdateAutoRgResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    throw new Exception("Request parameter CaseId is guid empty format.");
                }

                // Delete all current travel informations as this method could be called several times.
                var guardErrorMessage = "More then 50 travelinformations attached to case. Something is proberbly wrong.";
                DeleteCurrentEntitiesAttachedToCase(CgiTravelInformation, request.CaseID, guardErrorMessage);

                foreach (var travelInfo in request.TravelInformations)
                {
                    var travelInformation = new Entity(CgiTravelInformation)
                    {
                        Attributes = new AttributeCollection
                        {
                            {"cgi_caseid", new EntityReference("incident", request.CaseID)}
                        }
                    };

                    if (!string.IsNullOrEmpty(travelInfo.Line))
                    {
                        travelInformation.Attributes.Add("cgi_line", travelInfo.Line);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Company))
                    {
                        travelInformation.Attributes.Add("cgi_contractor", travelInfo.Company);
                    }

                    if (travelInfo.StartPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_startplanned", travelInfo.StartPlanned);
                    }

                    if (travelInfo.ArrivalPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_arivalplanned", travelInfo.ArrivalPlanned);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Title))
                    {
                        travelInformation.Attributes.Add("cgi_travelinformation", travelInfo.Title);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.ArrivalActual))
                    {
                        travelInformation.Attributes.Add("cgi_arivalactual", travelInfo.ArrivalActual);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.City))
                    {
                        travelInformation.Attributes.Add("cgi_city", travelInfo.City);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.DeviationMessage))
                    {
                        travelInformation.Attributes.Add("cgi_deviationmessage", travelInfo.DeviationMessage);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.DirectionText))
                    {
                        travelInformation.Attributes.Add("cgi_directiontext", travelInfo.DirectionText);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                    {
                        //Start time

                        var plannedStartTime = string.Empty;
                        string actualStartTime;
                        var diffStartTime = string.Empty;

                        try
                        {
                            actualStartTime = Convert.ToDateTime(travelInfo.StartActual).ToString("H:mm:ss");
                            plannedStartTime = travelInfo.StartPlanned.Value.ToString("H:mm:ss");
                            var diffStartTimeSpan = Convert.ToDateTime(travelInfo.StartActual) - travelInfo.StartPlanned.Value;

                            if (diffStartTimeSpan.TotalMinutes > 0)
                            {
                                diffStartTime += "+";
                            }

                            if (diffStartTimeSpan.TotalDays > 364 || diffStartTimeSpan.TotalDays < -364)
                            {
                                diffStartTime = "X";
                                actualStartTime = string.Empty;
                            }
                            else
                            {
                                diffStartTime += diffStartTimeSpan.TotalMinutes.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        catch
                        {
                            diffStartTime = "X";
                            actualStartTime = string.Empty;
                        }


                        //Arrive time
                        var plannedEndTime = string.Empty;
                        string actualEndTime;
                        var diffEndTime = string.Empty;

                        try
                        {
                            plannedEndTime = travelInfo.ArrivalPlanned.Value.ToString("H:mm:ss");
                            actualEndTime = Convert.ToDateTime(travelInfo.ArrivalActual).ToString("H:mm:ss");
                            var diffEndTimeSpan = Convert.ToDateTime(travelInfo.ArrivalActual) - travelInfo.ArrivalPlanned.Value;

                            if (diffEndTimeSpan.TotalMinutes > 0)
                            {
                                diffEndTime += "+";
                            }

                            if (diffEndTimeSpan.TotalDays > 364 || diffEndTimeSpan.TotalDays < -364)
                            {
                                diffEndTime = "X";
                                actualEndTime = "";
                            }
                            else
                            {
                                diffEndTime += diffEndTimeSpan.TotalMinutes.ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        catch
                        {
                            diffEndTime = "X";
                            actualEndTime = "";
                        }


                        string travelCompany = "(ingen operatör)";
                        if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                        {
                            travelCompany = travelInfo.Company;
                        }

                        travelInformation.Attributes.Add("cgi_displaytext",
                            $"Linje: [{travelInfo.Line}] Tur:{plannedStartTime}[{actualStartTime}({diffStartTime})] {travelInfo.Start} - {plannedEndTime}[{actualEndTime}({diffEndTime})] {travelInfo.Stop} Entreprenör: {travelCompany}");
                    }
                    else if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                    {

                    }

                    if (!string.IsNullOrEmpty(travelInfo.Start))
                    {
                        travelInformation.Attributes.Add("cgi_start", travelInfo.Start);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.StartActual))
                    {
                        travelInformation.Attributes.Add("cgi_startactual", travelInfo.StartActual);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.Stop))
                    {
                        travelInformation.Attributes.Add("cgi_stop", travelInfo.Stop);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Tour))
                    {
                        travelInformation.Attributes.Add("cgi_tour", travelInfo.Tour);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Transport))
                    {
                        travelInformation.Attributes.Add("cgi_transport", travelInfo.Transport);
                    }

                    _xrmManager.Create(travelInformation);
                }

                response.Success = true;

            }
            catch (FaultException faultex)
            {
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
        */
        #endregion OldRequestCreateTravelInformationLinkedToCase

        internal UpdateAutoRgResponse RequestCreateTravelInformationLinkedToCase(UpdateAutoRgCaseTravelInformationsRequest request)
        {

            _log.Debug($"--------------------------------Body content of UpdateAutoRgCaseTravelInformationsRequest-----------------------------");
            _log.Debug($"{nameof(request.CaseID)}: {request.CaseID}");

            for (int i = 0; i < request.TravelInformations.Length; i++)
            {
                _log.Debug($"Travel information loop: {i + 1}");
                _log.Debug($"ArrivalActual: {request.TravelInformations[i].ArrivalActual}");
                if (request.TravelInformations[i].ArrivalActualDateTime.HasValue)
                    _log.Debug($"ArrivalActualDateTime: {request.TravelInformations[i].ArrivalActualDateTime}");
                else _log.Debug($"ArrivalActualDateTime: NULL");

                _log.Debug($"ArrivalDifference: {request.TravelInformations[i].ArrivalDifference}");

                if (request.TravelInformations[i].ArrivalPlanned.HasValue)
                    _log.Debug($"ArrivalPlanned: {request.TravelInformations[i].ArrivalPlanned}");
                else _log.Debug($"ArrivalPlanned: NULL");

                _log.Debug($"City: {request.TravelInformations[i].City}");
                _log.Debug($"Company: {request.TravelInformations[i].Company}");
                _log.Debug($"DeviationMessage: {request.TravelInformations[i].DeviationMessage}");
                _log.Debug($"DirectionText: {request.TravelInformations[i].DirectionText}");
                _log.Debug($"DisplayText: {request.TravelInformations[i].DisplayText}");
                _log.Debug($"FailedToArrive: {request.TravelInformations[i].FailedToArrive}");
                _log.Debug($"FailedToStart: {request.TravelInformations[i].FailedToStart}");
                _log.Debug($"Line: {request.TravelInformations[i].Line}");
                _log.Debug($"LineDesignation: {request.TravelInformations[i].LineDesignation}");
                _log.Debug($"Start: {request.TravelInformations[i].Start}");
                _log.Debug($"StartActual: {request.TravelInformations[i].StartActual}");

                if (request.TravelInformations[i].StartActualDateTime.HasValue)
                    _log.Debug($"StartActualDateTime: {request.TravelInformations[i].StartActualDateTime}");
                else _log.Debug($"StartActualDateTime: NULL");

                _log.Debug($"StartDifference: {request.TravelInformations[i].StartDifference}");

                if (request.TravelInformations[i].StartPlanned.HasValue)
                    _log.Debug($"StartPlanned: {request.TravelInformations[i].StartPlanned}");
                else _log.Debug($"StartPlanned: NULL");

                _log.Debug($"Stop: {request.TravelInformations[i].Stop}");
                _log.Debug($"Title: {request.TravelInformations[i].Title}");
                _log.Debug($"Tour: {request.TravelInformations[i].Tour}");
                _log.Debug($"TrainNumber: {request.TravelInformations[i].TrainNumber}");
                _log.Debug($"Transport: {request.TravelInformations[i].Transport}");
                _log.Debug($"-------------------------------------------------------------");

            }

            _log.Debug($"----------------End of travel information----------------");


            var response = new UpdateAutoRgResponse()
            {
                Success = false,
                ErrorMessage = string.Empty
            };

            try
            {
                if (request.CaseID == Guid.Empty)
                {
                    throw new Exception("Request parameter CaseId is guid empty format.");
                }

                // Delete all current travel informations as this method could be called several times.
                var guardErrorMessage = "More then 50 travelinformations attached to case. Something is proberbly wrong.";
                DeleteCurrentEntitiesAttachedToCase(CgiTravelInformation, request.CaseID, guardErrorMessage);

                foreach (var travelInfo in request.TravelInformations)
                {
                    var travelInformation = new Entity(CgiTravelInformation)
                    {
                        Attributes = new AttributeCollection
                        {
                            {"cgi_caseid", new EntityReference("incident", request.CaseID)}
                        }
                    };

                    if (!string.IsNullOrEmpty(travelInfo.Line))
                    {
                        travelInformation.Attributes.Add("cgi_line", travelInfo.Line);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Company))
                    {
                        travelInformation.Attributes.Add("cgi_contractor", travelInfo.Company);
                    }

                    if (travelInfo.StartPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_startplanned", travelInfo.StartPlanned);
                    }

                    if (travelInfo.ArrivalPlanned != null)
                    {
                        travelInformation.Attributes.Add("cgi_arivalplanned", travelInfo.ArrivalPlanned);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Title))
                    {
                        travelInformation.Attributes.Add("cgi_travelinformation", travelInfo.Title);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.City))
                    {
                        travelInformation.Attributes.Add("cgi_city", travelInfo.City);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.DeviationMessage))
                    {
                        travelInformation.Attributes.Add("cgi_deviationmessage", travelInfo.DeviationMessage);
                    }
                    if (!string.IsNullOrEmpty(travelInfo.DirectionText))
                    {
                        travelInformation.Attributes.Add("cgi_directiontext", travelInfo.DirectionText);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.ArrivalActual))
                    {
                        DateTime t = Convert.ToDateTime(travelInfo.ArrivalActual);
                        if (t < new DateTime(9000, 1, 1))
                            travelInformation.Attributes.Add("cgi_arivalactual", travelInfo.ArrivalActual);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.StartActual))
                    {
                        DateTime t = Convert.ToDateTime(travelInfo.StartActual);
                        if (t < new DateTime(9000, 1, 1))
                            travelInformation.Attributes.Add("cgi_startactual", travelInfo.StartActual);
                    }

                    if (travelInfo.ArrivalActualDateTime != null)
                    {
                        if (travelInfo.ArrivalActualDateTime.Value.Date < new DateTime(2090, 1, 1))
                            travelInformation.Attributes.Add("cgi_arrivalactualdatetime", travelInfo.ArrivalActualDateTime);
                    }

                    if (travelInfo.StartActualDateTime != null)
                    {
                        if (travelInfo.StartActualDateTime.Value < new DateTime(2090, 1, 1))
                            travelInformation.Attributes.Add("cgi_startactualdatetime", travelInfo.StartActualDateTime);
                    }

                    //#region Times
                    //var plannedStartTime = string.Empty;
                    //string actualStartTime = string.Empty;
                    //var diffStartTime = string.Empty;
                    //bool failedToStart = true;

                    //var plannedArriveTime = string.Empty;
                    //string actualArriveTime = string.Empty;
                    //var diffArriveTime = string.Empty;
                    //bool failedToArrive = true;

                    //#region Start time
                    //try
                    //{

                    //    DateTime t_SA = Convert.ToDateTime(travelInfo.StartActual);
                    //    DateTime t_SP = Convert.ToDateTime(travelInfo.StartPlanned);

                    //    if (t_SA < new DateTime(9000, 1, 1))
                    //    {
                    //        var diffStartTimeSpan = t_SA - t_SP;

                    //        actualStartTime = Convert.ToDateTime(travelInfo.StartActual).ToString("HH:mm:ss");
                    //        plannedStartTime = travelInfo.StartPlanned.Value.ToString("HH:mm:ss");
                    //        //var diffStartTimeSpan = Convert.ToDateTime(travelInfo.StartActual) - travelInfo.StartPlanned.Value;

                    //        if (diffStartTimeSpan.Minutes == 0)
                    //        {
                    //            diffStartTime = string.Format("{0}", diffStartTimeSpan.Minutes.ToString());
                    //        }

                    //        else if (diffStartTimeSpan.Minutes > 0)
                    //        {
                    //            diffStartTime = string.Format("+{0}", diffStartTimeSpan.Minutes.ToString());
                    //        }
                    //        else if (diffStartTimeSpan.Minutes < 0)
                    //        {
                    //            diffStartTime = string.Format("{0}", diffStartTimeSpan.Minutes.ToString());
                    //        }
                    //        failedToStart = false;
                    //    }
                    //    else
                    //    {
                    //        failedToStart = true;
                    //    }
                    //}
                    //catch
                    //{
                    //    diffStartTime = string.Empty;
                    //    actualStartTime = string.Empty;
                    //    failedToStart = true;
                    //}

                    //#endregion Start time

                    //#region Arrive time
                    //try
                    //{

                    //    DateTime t_AA = Convert.ToDateTime(travelInfo.ArrivalActual);
                    //    DateTime t_AP = Convert.ToDateTime(travelInfo.ArrivalPlanned);

                    //    if (t_AA < new DateTime(9000, 1, 1))
                    //    {
                    //        var diffStartTimeSpan = t_AA - t_AP;

                    //        plannedArriveTime = travelInfo.ArrivalPlanned.Value.ToString("HH:mm:ss");
                    //        actualArriveTime = Convert.ToDateTime(travelInfo.ArrivalActual).ToString("HH:mm:ss");
                    //        var diffArriveTimeSpan = t_AA - t_AP;

                    //        if (diffArriveTimeSpan.Minutes == 0)
                    //        {
                    //            diffArriveTime = string.Format("{0}", diffArriveTimeSpan.Minutes.ToString());
                    //        }
                    //        else if (diffArriveTimeSpan.Minutes > 0)

                    //        {
                    //            diffArriveTime = string.Format("+{0}", diffArriveTimeSpan.Minutes.ToString());

                    //        }
                    //        else if (diffArriveTimeSpan.Minutes < 0)
                    //        {
                    //            diffArriveTime = string.Format("{0}", diffArriveTimeSpan.Minutes.ToString());
                    //        }

                    //        failedToArrive = false;
                    //        failedToStart = false;
                    //    }
                    //    else
                    //    {
                    //        failedToArrive = true;
                    //    }
                    //}
                    //catch
                    //{
                    //    diffArriveTime = string.Empty;
                    //    actualArriveTime = string.Empty;
                    //    failedToArrive = true;
                    //}


                    //#endregion Arrive time
                    //#endregion Times

                    string travelCompany = "(ingen operatör)";
                    if (!string.IsNullOrEmpty(travelInfo.DisplayText))
                    {
                        travelCompany = travelInfo.Company;
                    }

                    //travelInformation.Attributes.Add("cgi_displaytext",
                    //    $"Linje: [{travelInfo.Line}] Tur:{plannedStartTime}[{actualStartTime}({diffStartTime})] {travelInfo.Start} - {plannedArriveTime}[{actualArriveTime}({diffArriveTime})] {travelInfo.Stop} Entreprenör: {travelCompany}");


                    if (!string.IsNullOrEmpty(travelInfo.Start))
                    {
                        travelInformation.Attributes.Add("cgi_start", travelInfo.Start);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Stop))
                    {
                        travelInformation.Attributes.Add("cgi_stop", travelInfo.Stop);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Tour))
                    {
                        travelInformation.Attributes.Add("cgi_tour", travelInfo.Tour);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.Transport))
                    {
                        travelInformation.Attributes.Add("cgi_transport", travelInfo.Transport);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.ArrivalDifference))
                    {
                        travelInformation.Attributes.Add("cgi_arrivaldifference", travelInfo.ArrivalDifference);
                    }
                    //else if (!string.IsNullOrEmpty(diffArriveTime))
                    //{
                    //    travelInformation.Attributes.Add("cgi_arrivaldifference", diffArriveTime);
                    //}

                    if (!string.IsNullOrEmpty(travelInfo.StartDifference))
                    {
                        travelInformation.Attributes.Add("cgi_startdifference", travelInfo.StartDifference);
                    }
                    //else if (!string.IsNullOrEmpty(diffStartTime))
                    //{
                    //    travelInformation.Attributes.Add("cgi_startdifference", diffStartTime);
                    //}

                    travelInformation.Attributes.Add("cgi_failedtoarrive", travelInfo.FailedToArrive);
                    travelInformation.Attributes.Add("cgi_failedtodepart", travelInfo.FailedToStart);


                    /*
                    if (!string.IsNullOrEmpty(travelInfo.lineType))
                    {
                        travelInformation.Attributes.Add("cgi_linetype", lineType); //Not in use
                    }
                    */

                    if (!string.IsNullOrEmpty(travelInfo.TrainNumber))
                    {
                        travelInformation.Attributes.Add("cgi_trainnumber", travelInfo.TrainNumber);
                    }

                    if (!string.IsNullOrEmpty(travelInfo.LineDesignation))
                    {
                        travelInformation.Attributes.Add("cgi_linedesignation", travelInfo.LineDesignation);
                    }

                    string journeyNumber = string.IsNullOrEmpty(travelInfo.Tour) ? "" : travelInfo.Tour;
                    if (!string.IsNullOrEmpty(journeyNumber))
                    {
                        travelInformation.Attributes.Add("cgi_journeynumber", journeyNumber);
                    }

                    _log.Debug($"Creating travel information.");
                    _xrmManager.Create(travelInformation);

                }

                response.Success = true;



            }
            catch (FaultException faultex)
            {
                response.ErrorMessage = faultex.Message;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        #endregion
    }
}