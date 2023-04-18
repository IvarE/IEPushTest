using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Linq;

using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Crm.Sdk.Messages;

using CGIXrmHandler.CrmClasses;
using CGIXrmHandler.Shared;
using Endeavor.Extensions;
using CGIXrmCreateCaseService.Entities;         // To be able to share objects.
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using CGIXrmHandler.CRMPlusAPI;
using System.Configuration;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace CGIXrmHandler
{
    public class PASSHandler : IDisposable
    {
        #region Declarations
        private XrmManager _xrmManager;
        private string _tokenCertName = "";
        // Flag: Has Dispose already been called? 
        bool _disposed;

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
        #endregion

        #region Constructors
        public PASSHandler()
        {
            var xrmHelper = new XrmHelper();
            _xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
            _tokenCertName = ConfigurationManager.AppSettings["TokenCertificateName"];
        }

        #endregion


        #region Endeavor Helper Functions

        /// <summary>
        /// Retrieve first record from a entity
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
            var xrmHelper = new XrmHelper();
            xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
            
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

            //var xrmHelper = new XrmHelper();
            //xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
            
            //if(xrmManager.Service == null)
            //{
            //    throw new Exception("XrmManager.Service is null...");
            //}

            try
            {
                
                EntityCollection result = xrmManager.Service.RetrieveMultiple(query);

                if (result.Entities.Count > 0)
                    return (result.Entities[0]).ToEntityNull<T>();


            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving. Ex: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SettingsEntity GetSettings()
        {
            SettingsEntity settings = null;
            //try
            //{
            //    var xrmHelper = new XrmHelper();
            //    _xrmManager = xrmHelper.GetXrmManagerFromAppSettings();
            //}
            //catch(Exception e)
            //{
            //    throw new Exception("Error with XrmManager. Ex: " + e.ToString());
            //}

            try
            {
                // Get settings, first available.
                settings = RetrieveFirst<SettingsEntity>(_xrmManager, SettingsEntity.EntityLogicalName
                        , new ColumnSet(SettingsEntity.Fields.ed_CRMPlusService));

                if (settings == null)
                    throw new Exception("Settings data is missing.");

                if (string.IsNullOrEmpty(settings.ed_CRMPlusService))
                    throw new Exception("URL to CRMPlus service is missing in settings.");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected exception from GetSettings(). Err: " + ex.ToString());
            }
            return settings;
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

            return Skanetrafiken.Crm.Controllers.Identity.EncodeTokenEncryption(tokenClass, _tokenCertName);
        }

        /// <summary>
        /// RGOL: Anropa Fasad för att hämta eller skapa kund.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Guid GetOrCreateContactPASSIncident(Incident incident)
        {
            // Get settings, first available.
            SettingsEntity settings = GetSettings();

            string City = null;
            string PostalCode = null;

            GetCityAndPostalCodeFromField(incident.sPA, out City, out PostalCode);

            // Call API to get or create new contact.
            CRMPlusAPI.Models.CustomerInfo customer = new CRMPlusAPI.Models.CustomerInfo();
            customer.Source = (int)CRMPlusAPISource.PASS;
            customer.FirstName = incident.sFN;
            customer.LastName = incident.sLN; ;
            customer.Telephone = incident.sPW;
            //INFO: Missing mapping for telefone2 (sPH)
            // According to Håkan T (2016-11-29) is phone2 not used.
            customer.Mobile = incident.sPM;
            customer.Email = incident.sEM;
            customer.SocialSecurityNumber = incident.sSSN;
            if (customer.SocialSecurityNumber.Length == 8)
                customer.SwedishSocialSecurityNumber = false;
            else
                customer.SwedishSocialSecurityNumber = true;

            customer.AddressBlock = new CRMPlusAPI.Models.CustomerInfoAddressBlock();
            customer.AddressBlock.City = City;
            customer.AddressBlock.CountryISO = "SE";            // Always use SE, PASS is only for swedish customers.
            customer.AddressBlock.Line1 = incident.sA;
            customer.AddressBlock.PostalCode = PostalCode;

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
                obj.HttpClient.DefaultRequestHeaders.Add("X-CRMPlusToken", CreateTokenForWEBApi());

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
                string msg = ex.Response.Content.ContentToString();

                throw new Exception(string.Format("Kan inte skapa kund i SeKund. Orsak:{0}", msg), ex.InnerException);
            }
            // Catch any serialization exeptions
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                throw new Exception(string.Format("Kunde inte omvandla resultat från CRMPlus till ett objekt. Detaljerat fel:{0}", ex.Message), ex.InnerException);
            }
            catch (Exception ex)
            {
                string msg = string.Format($"Inner exeption:{ex.InnerException}, exeption:{ex.Message}");
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// Created method from existing code.
        /// JohanA, Endeavor.
        /// </summary>
        /// <param name="cityAndPostalCodeCombined"></param>
        /// <param name="city"></param>
        /// <param name="postalCode"></param>
        private void GetCityAndPostalCodeFromField(string cityAndPostalCodeCombined, out string city, out string postalCode)
        {
            city = "";
            postalCode = "";
            if (!string.IsNullOrEmpty(cityAndPostalCodeCombined))
            {
                string sPa = cityAndPostalCodeCombined;

                // Tar bort ev. mellanslag i postnr
                if (cityAndPostalCodeCombined.Length > 4)
                {
                    if (cityAndPostalCodeCombined[3].Equals(' '))
                        sPa = cityAndPostalCodeCombined.Remove(3, 1);
                }

                if (sPa.Contains(" "))
                {
                    string[] split = Regex.Split(sPa, " ");
                    if (split.Length == 1)
                    {
                        postalCode = split[0];
                        city = "";
                    }

                    if (split.Length == 2)
                    {
                        postalCode = split[0];
                        city = split[1];
                    }

                    if (split.Length > 2)
                    {
                        postalCode = split[0];
                        bool breakLoop = false;
                        // Take all parts until we reach a ","
                        for (int i = 1; i < split.Length; i++)
                        {
                            if (split[i].Contains(","))
                            {
                                split[i] = split[i].Replace(",", "");
                                breakLoop = true;
                            }
                            // Add to city, to support Västa Frölunda...
                            city += split[i] + " ";

                            if (breakLoop)
                                break;
                        }

                        city.Trim();
                    }

                }
                else
                {
                    postalCode = sPa;
                    city = "";
                }
            }
        }


        #endregion


        #region Private Methods

        private EntityReference CreateIncident(Incident incident)
        {
            incident.CaseOrigin = new OptionSetValue((int)CaseOrgin.PhoneCall);
            Entity entity = _xrmManager.Create(incident);

            EntityReference team = GetDefaultTem();
            if (team != null)
            {
                AssignRequest assign = new AssignRequest
                {
                    Assignee = new EntityReference(team.LogicalName, team.Id),
                    Target = new EntityReference(entity.LogicalName, entity.Id)
                };
                _xrmManager.Service.Execute(assign);
                LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Assign to team.");
            }

            return new EntityReference(entity.LogicalName, entity.Id);
        }

        private Guid CreatePASSTravelInformation(PASSTravelInformation passTravelInformation, EntityReference IncidentReference)
        {
            LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "CreatePASSTravelInformation");
            passTravelInformation.Case = IncidentReference;
            Entity entity = _xrmManager.Create<PASSTravelInformation>(passTravelInformation);
            return entity.Id;
        }


        private string _returnFullname(string fname, string lname)
        {
            string fullname = "okänd";

            if (!string.IsNullOrEmpty(fname))
                fullname = fname;

            if (!string.IsNullOrEmpty(lname))
                fullname = string.Format("{0} {1}", fullname, lname);

            return fullname;
        }

        private string _formatSSN(string ssn)
        {
            if (string.IsNullOrEmpty(ssn))
                return "";

            string socialsecuritynumber;

            try
            {
                socialsecuritynumber = ssn.Replace("-", "");
                string first;
                string last;

                if (socialsecuritynumber.Length == 10)
                {
                    first = socialsecuritynumber.Substring(0, 6);
                    last = socialsecuritynumber.Substring(6, 4);
                    socialsecuritynumber = string.Format("19{0}{1}", first, last);
                }

                if (socialsecuritynumber.Length == 12)
                {
                    first = socialsecuritynumber.Substring(0, 8);
                    last = socialsecuritynumber.Substring(8, 4);
                    socialsecuritynumber = string.Format("{0}{1}", first, last);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return socialsecuritynumber;
        }

        private string _formatPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "";

            string phonenumber;

            try
            {
                phonenumber = number;

                if (number.Substring(0, 1) == "+")
                {
                    phonenumber = phonenumber.Substring(3, (number.Length - 3));
                }

                int first = 0;
                int.TryParse(phonenumber.Substring(0, 1), out first);
                if (first > 0)
                {
                    phonenumber = string.Format("0{0}", phonenumber);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return phonenumber;
        }


        #endregion

        #region Public Methods

        public Guid ExecutePASSRequest(Incident incident, PASSTravelInformation[] passTravelInformations)
        {
            LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Create incident");
            EntityReference incidentReference = CreateIncident(incident);

            foreach (PASSTravelInformation passTravelInformation in passTravelInformations)
            {
                CreatePASSTravelInformation(passTravelInformation, incidentReference);
            }

            return incidentReference.Id;
        }

        public ObservableCollection<Contact> FetchContacts(Incident incident)
        {
            ObservableCollection<Contact> contacts;
            try
            {
                LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Fetch contact");

                Guid g = GetOrCreateContactPASSIncident(incident);

                if (g != Guid.Empty)
                {
                    LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Fetch incident is not empty");
                    contacts = _xrmManager.Get<Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                    LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", "Fetch contact finished");
                    return contacts;
                }

                //contacts = _xrmManager.Get<Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                //if (contacts == null || !contacts.Any())
                //{
                //    LogMessage("E:\\Logs\\CGIXrmHandlers\\PASSLog.txt", "Fetch contact 2");
                //    Guid g = _createContact(incident);
                //    LogMessage("E:\\Logs\\CGIXrmHandlers\\PASSLog.txt", "Fetch contact 3");
                //    if (g != Guid.Empty)
                //    {
                //        LogMessage("E:\\Logs\\CGIXrmHandlers\\PASSLog.txt", "Fetch contact 4");
                //        contacts = _xrmManager.Get<Contact>(new FetchExpression(FetchContactsXML(incident.sSSN, incident.sEM)));
                //        LogMessage("E:\\Logs\\CGIXrmHandlers\\PASSLog.txt", "Fetch contact 5");
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogMessage("E:\\Logs\\CRM\\CRMExtIntegrationPortal\\PASSLog.txt", ex.Message);
                throw new Exception(ex.Message);
            }

            return null;
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

        #endregion

        #region GettSettings

        public EntityReference GetAnonymousCustomer()
        {
            #region FetchXML

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_defaultcustomeroncase' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            #endregion

            EntityCollection settings = _xrmManager.Service.RetrieveMultiple(new FetchExpression(xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultcustomeroncase");
        }

        public EntityReference GetDefaultTem()
        {
            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_defaultteamonpasscase' />";
            xml += "       <filter type='and'>";
            xml += "           <condition attribute='statecode' operator='eq' value='0' />";
            xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
            xml += "           <filter type='or'>";
            xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
            xml += "               <condition attribute='cgi_validto' operator='null' />";
            xml += "           </filter>";
            xml += "       </filter>";
            xml += "   </entity>";
            xml += "</fetch>";

            EntityCollection settings = _xrmManager.Service.RetrieveMultiple(new FetchExpression(xml));

            return settings.Entities.First().GetAttributeValue<EntityReference>("cgi_defaultteamonpasscase");

        }

        #endregion

        #region [FetchXML]

        // TODO : email parameter not used
        private string FetchContactsXML(string ssn, string email)
        {
            return string.Format(
            "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
              "<entity name='contact'>" +
                "<attribute name='contactid' />" +
                "<attribute name='cgi_socialsecuritynumber' />" +
                "<attribute name='emailaddress1' />" +
                "<filter type='and'>" +
                  "<filter type='or'>" +
                    "<condition attribute='cgi_socialsecuritynumber' operator='eq' value='{0}' />" +
                  "</filter>" +
                "</filter>" +
              "</entity>" +
            "</fetch>", ssn);
        }

        #endregion

        #region [IDispose]


        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        ~PASSHandler()
        {
            Dispose(false);
        }
        #endregion
    }
}
