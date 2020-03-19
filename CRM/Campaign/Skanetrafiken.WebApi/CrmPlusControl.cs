using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Endeavor.Crm.Extensions;
using System.Web;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Newtonsoft.Json;
using System.Web.Caching;
using Skanetrafiken.Crm.Properties;
using Microsoft.Xrm.Sdk.Client;
//using Microsoft.Web.Administration;


namespace Skanetrafiken.Crm.Controllers
{
    public class CrmPlusControl
    {

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string _generateContextString = "localContext";
        //private static readonly string _cacheErrorCountString = "cacheErrorCount";
        //private static readonly string _appPoolName = "CRMPlus";
        private static readonly byte[] _entropyString = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        //private static System.Diagnostics.Stopwatch _stopwatch;
        //private static CrmServiceClient _conn;

        //private static string NonLoginPurchaseSubject = "Oinloggat Köp";
        private static string NonLoginRefillSubject = "Ladda Kort";
        private static string CreateAccountSubject = "Skapa mitt konto";

        internal static HttpResponseMessage RetrieveLeadLinkGuid(string eamil)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.StateCode, LeadEntity.Fields.ed_LatestLinkGuid),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, eamil),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                        }
                    });

                if (leads.Count == 0)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                    rm.Content = new StringContent($"Hittade inget Lead med {eamil}");
                    return rm;
                }
                else if (leads.Count > 1)
                {
                    int actives = 0;
                    LeadEntity activeLead = null;
                    foreach (LeadEntity l in leads)
                    {
                        if (l.StateCode == Generated.LeadState.Open)
                        {
                            activeLead = l;
                            actives++;
                        }
                    }
                    if (actives > 1)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Hittade multipla Lead med {eamil}");
                        return rm;
                    }
                    else if (actives == 0)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent($"Hittade inga aktiva Lead med {eamil}");
                        return rm;
                    }
                    else
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        GuidsPlaceholder guidPlaceholder = new GuidsPlaceholder
                        {
                            CrmGuid = activeLead.Id.ToString(),
                            LinkId = activeLead.ed_LatestLinkGuid
                        };
                        response.Content = new StringContent(JsonConvert.SerializeObject(guidPlaceholder));
                        return response;
                    }
                }
                else if (!leads[0].StateCode.Equals(Generated.LeadState.Open))
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent($"Lead med {eamil} har redan validerats.");
                    return rm;
                }
                if (string.IsNullOrWhiteSpace(leads[0].ed_LatestLinkGuid))
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                    rm.Content = new StringContent($"Hittade inget LänkId för Lead med {eamil}");
                    return rm;
                }
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                GuidsPlaceholder gp = new GuidsPlaceholder
                {
                    CrmGuid = leads[0].Id.ToString(),
                    LinkId = leads[0].ed_LatestLinkGuid
                };
                resp.Content = new StringContent(JsonConvert.SerializeObject(gp));

                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage RetrieveContactLinkGuid(string idOrEmail)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                IList<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.ed_LatestLinkGuid),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_EmailToBeVerified, ConditionOperator.Equal, idOrEmail)
                        }
                    });

                if (contacts.Count == 0)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                    rm.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, idOrEmail));
                    return rm;
                }
                else if (contacts.Count > 1)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent($"Hittade multipla Kunder som försöker byta till {idOrEmail}");
                    return rm;
                }
                if (string.IsNullOrWhiteSpace(contacts[0].ed_LatestLinkGuid))
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.NotFound);
                    rm.Content = new StringContent($"Hittade inget länkId för Kund med {idOrEmail}");
                    return rm;
                }
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                GuidsPlaceholder gp = new GuidsPlaceholder
                {
                    CrmGuid = contacts[0].Id.ToString(),
                    LinkId = contacts[0].ed_LatestLinkGuid
                };
                resp.Content = new StringContent(JsonConvert.SerializeObject(gp));
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        internal static HttpResponseMessage PASS(CustomerInfo customerInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                if (customerInfo == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return respMess;
                }
                
                // Validate info
                _log.DebugFormat("PASS, validating info");
                customerInfo.Source = (int)CustomerUtility.Source.PASS;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(validateStatus.ErrorMessage);
                    return rm;
                }

                // Find/ Create Customer
                _log.DebugFormat("PASS, calling FindOrCreateUnvalidatedContact");
                ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                _log.DebugFormat("PASS, Found/created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                // Attach Lead
                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                        }
                    });
                bool updated = false;
                foreach (LeadEntity lead in leads)
                {
                    if (ContactEntity.UpdateContactWithLead(ref contact, lead))
                        updated = true;
                }
                if (updated)
                {
                    contact.ed_InformationSource = Generated.ed_informationsource.PASS;
                    localContext.OrganizationService.Update(contact);
                }
                
                HttpResponseMessage finalrm = new HttpResponseMessage(HttpStatusCode.OK);
                finalrm.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return finalrm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static string util()
        {
            try
            {
                System.Diagnostics.Stopwatch counter = new System.Diagnostics.Stopwatch();
                counter.Restart();
                Plugin.LocalPluginContext localContext = GenerateContext();

                string mess = "";
                for (int i = 0; i < 15; i++)
                {
                    EntityReference se = CountryUtility.GetEntityRefForCountryCode(localContext, "SE");
                    EntityReference uk = CountryUtility.GetEntityRefForCountryCode(localContext, "UK");
                    EntityReference dk = CountryUtility.GetEntityRefForCountryCode(localContext, "DK");
                    string seIso = CountryUtility.GetIsoCodeForCountry(localContext, se.Id);
                    string ukIso = CountryUtility.GetIsoCodeForCountry(localContext, uk.Id);
                    string dkIso = CountryUtility.GetIsoCodeForCountry(localContext, dk.Id);
                    
                    mess = string.Format("{0}{1}", mess, string.Format("\ni = {0}, counter = {1}", i, counter.ElapsedMilliseconds));
                }

                return mess;  
            }
            catch (Exception ex)
            {
                //HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.OK);
                //rm.Content = new StringContent(string.Format("Unexpected exception caught:\n{0}", ex.Message));
                //return rm;
                throw ex;
            }
        }


        public static HttpResponseMessage ChangeEmailAddress([FromBody] CustomerInfo customer)
        {
            try
            {
                if (customer == null)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return rm;
                }
                Plugin.LocalPluginContext localContext = GenerateContext();
                // Validera att inkommande information är giltig (ValidateCustomerInfo)
                customer.Source = (int)CustomerUtility.Source.BytEpost;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                if (!validateStatus.TransactionOk)
                {
                    // Skicka vidare eventuellt felmeddelande
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(validateStatus.ErrorMessage);
                    return respMess;
                }
                // Kolla så att en eventuell uppdatering inte skulle skapa dublett - Om så, returnera lämpligt felmeddelande
                IList<ContactEntity> mailConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression()
                    {
                        Conditions =
                        {
                             new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                             new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.NewEmail)
                        }
                    });
                if (mailConflicts.Count > 0)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.EmailAlreadyInUse);
                    return rm;
                }
                // Uppdatera Contact's EmailToBeVerified-fält och skicka ett valideringsmail.
                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email)
                        }
                    });
                if (contact == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, string.Format("{0} + {1}", customer.SocialSecurityNumber, customer.Email)));
                    return respMess;
                }

                int validityHours = 0;
                try
                {
                    validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
                }
                catch (MissingFieldException e)
                {
                    _log.Error($"Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    respMess.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                    return respMess;
                }
                catch (Exception)
                {
                    throw;
                }

                contact.ed_EmailToBeVerified = customer.NewEmail;
                contact.ed_LinkExpiryDate = DateTime.Now.AddHours(validityHours);
                contact.ed_LatestLinkGuid = Guid.NewGuid().ToString();
                contact.ed_InformationSource = Generated.ed_informationsource.BytEpost;
                localContext.OrganizationService.Update(contact);
                SendEmailResponse resp = SendValidationEmail(localContext, contact);

                HttpResponseMessage finalMess = new HttpResponseMessage(HttpStatusCode.OK);
                finalMess.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return finalMess;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateCustomerLead([FromBody] CustomerInfo customer)
        {
            try
            {
                customer = (LeadInfo)customer;
                LeadEntity newLead = null;
                Plugin.LocalPluginContext localContext = GenerateContext();
                _log.Debug("localContext generated");
                if (customer == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return response;
                }
                if (!customer.SwedishSocialSecurityNumberSpecified)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Properties.Resources.SwedishSocialSecurityNumberMustBeSpecified);
                    return respMess;
                }
                customer.Source = (int)CustomerUtility.Source.SkapaMittKonto;
                StatusBlock validationStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                if (!validationStatus.TransactionOk)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(validationStatus.ErrorMessage);
                    return response;
                }
                StatusBlock canLeadBeCreated = LeadEntity.CanLeadBeCreated(localContext, customer);
                if (canLeadBeCreated.TransactionOk)
                {
                    // Create a new lead.
                    newLead = LeadEntity.CreateLead(localContext, customer, CreateAccountSubject);
                }
                else if (canLeadBeCreated.StatusBlockCode == (int)CustomerUtility.StatusBlockCode.OtherLeadFound)
                {
                    Guid leadGuid = Guid.Empty;
                    if (Guid.TryParse(canLeadBeCreated.Information, out leadGuid))
                    {
                        try
                        {
                            newLead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, leadGuid, LeadEntity.LeadInfoBlock);
                        }
                        catch (Exception e)
                        {
                            localContext.TracingService.Trace(string.Format("Inconcistency in Database. Unexpected exception caught:\n{0}", e.Message));

                            HttpResponseMessage exceptionResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            exceptionResponse.Content = new StringContent(Resources.InconsistencyInDatabase);
                            return exceptionResponse;
                        }

                        newLead.UpdateWithCustomerInfo(localContext, customer);
                    }
                    else
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(Resources.InconsistencyInDatabase);
                        return response;
                    }
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(canLeadBeCreated.ErrorMessage);
                    return response;
                }

                int validityHours = 0;
                try
                {
                    validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
                }
                catch (MissingFieldException e)
                {
                    _log.Error($"Error caught when calling GetSettingInt() for {CgiSettingEntity.Fields.ed_LeadValidityHours}. Message = {e.Message}");
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    respMess.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                    return respMess;
                }
                catch (Exception)
                {
                    throw;
                }

                newLead.ed_LinkExpiryDate = DateTime.Now.AddHours(validityHours);
                newLead.ed_LatestLinkGuid = Guid.NewGuid().ToString();
                newLead.ed_InformationSource = Generated.ed_informationsource.SkapaMittKonto;
                newLead.LeadSourceCode = Generated.lead_leadsourcecode.MittKonto;
                localContext.OrganizationService.Update(newLead);

                SendEmailResponse mailResponse = SendValidationEmail(localContext, newLead);
                
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.Created);
                rm.Content = new StringContent(SerializeNoNull(newLead.ToCustomerInfo(localContext)));
                return rm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format("Unexpected exception caught:\n{0}", ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage GetContactTroubleshooting(string contactGuidOrEmail)
        {
            //OrganizationServiceProxy serviceProxy = null;
            try
            {

                HttpContext httpContext = HttpContext.Current;
                _log.DebugFormat("httpContext = {0}", httpContext);

                CrmServiceClient _conn = null;
                if (httpContext != null)
                {
                    _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
                }
                if (_conn == null)
                {
                    _log.Debug("Creating new connection from CRM (no cache found)");
                    string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                    _log.DebugFormat("connectionString = {0}", connectionString);
                    //  Connect to the CRM web service using a connection string.
                    _conn = new CrmServiceClient(connectionString);

                    _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
                    if (_conn.IsReady)
                    {
                        httpContext.Cache.Insert(_generateContextString, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                    }
                    else
                    {
                        //ServerManager serverManager = new ServerManager();

                        //ApplicationPoolCollection appPoolCollection = serverManager.ApplicationPools;
                        //foreach (ApplicationPool pool in appPoolCollection)
                        //{
                        //    if (pool.Name != null && pool.Name.Equals(_appPoolName))
                        //    {

                        //    }
                        //}
                            
                        //_log.Debug($"Acessing cache for error count");
                        //int? errorCount = httpContext.Cache.Get(_cacheErrorCountString) as int?;
                    }
                }

                _log.DebugFormat("Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
                OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)_conn.OrganizationServiceProxy;
                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                if (localContext.OrganizationService == null)
                    throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);

                resp.Content = new StringContent("Debug");


                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            //finally
            //{
            //    if (serviceProxy != null)
            //        serviceProxy.Dispose();
            //}
        }

        public static HttpResponseMessage GetContact(string contactGuidOrEmail)
        {
            ContactEntity contact;
            try
            {                
                Plugin.LocalPluginContext localContext = GenerateContext();

                Guid contactId = Guid.Empty;
                // Split contactId / email.
                if (Guid.TryParse(contactGuidOrEmail, out contactId))
                {
                    contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, contactId)
                            }
                        });
                }
                else
                {
                    contact = ContactEntity.GetValidatedContactFromEmail(localContext, contactGuidOrEmail);
                }
                if (contact == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                    respMess.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, contactGuidOrEmail));
                    return respMess;
                }
                if (contact.StateCode != Generated.ContactState.Active)
                {
                    // TODD teo - Verify httpStatusCode usage
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.NotFound);
                    respMess.Content = new StringContent(string.Format(Resources.ContactIsInactive));
                    return respMess;
                }
                CustomerInfo info = contact.ToContactInfo(localContext);

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);

                resp.Content = new StringContent(SerializeNoNull(info));

                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage GetLead(string id)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();

                if (string.IsNullOrWhiteSpace(id))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return response;
                }
                Guid guid = Guid.Empty;
                if (!Guid.TryParse(id, out guid))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(Resources.GuidNotValid);
                    return response;
                }
                LeadEntity lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                        new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, guid)
                        }
                    });
                if (lead == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    response.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, guid));
                    return response;
                }
                
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(lead.ToCustomerInfo(localContext)));
                return resp;

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginCustomerIncident([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                if (customerInfo == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return respMess;
                }

                // Validate info.
                customerInfo.Source = (int)CustomerUtility.Source.OinloggatKundArende;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(validateStatus.ErrorMessage);
                    return respMess;
                }
                // Find customer according to rules
                ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);

                // Find possible Leads and connect possible purchaseInfo.
                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                        }
                    });
                foreach (LeadEntity lead in leads)
                {
                    bool update = false;
                    if (ContactEntity.UpdateContactWithLead(ref contact, lead))
                        update = true;
                    if (update)
                    {
                        contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKundArende;
                        localContext.OrganizationService.Update(contact);
                    }
                }

                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.OK);
                rm.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return rm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginPurchase([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();

                if (customerInfo == null)
                {
                    HttpResponseMessage invalidInputResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    invalidInputResp.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return invalidInputResp;
                }

                // Validate info
                customerInfo.Source = (int)CustomerUtility.Source.OinloggatKop;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(validateStatus.ErrorMessage);
                    return rm;
                }

                // Find/ Create Customer
                _log.DebugFormat("NonLoginPurchase, calling FindOrCreateUnvalidatedContact");
                ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                _log.DebugFormat("NonLoginPurchase, Found/Created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                // Attach Lead
                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                        }
                    });
                bool updated = false;
                foreach (LeadEntity lead in leads)
                {
                    if (ContactEntity.UpdateContactWithLead(ref contact, lead))
                        updated = true;
                }
                if (updated)
                {
                    contact.ed_InformationSource = Generated.ed_informationsource.OinloggatKop;
                    localContext.OrganizationService.Update(contact);
                }

                HttpResponseMessage leadResp = new HttpResponseMessage(HttpStatusCode.OK);
                leadResp.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return leadResp;

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static HttpResponseMessage NonLoginRefill([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();

                if (customerInfo == null)
                {
                    HttpResponseMessage invalidInputResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    invalidInputResp.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return invalidInputResp;
                }

                // Validate info
                customerInfo.Source = (int)CustomerUtility.Source.OinloggatLaddaKort;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(validateStatus.ErrorMessage);
                    return rm;
                }

                ContactEntity contact = ContactEntity.FindActiveContact(localContext, customerInfo);

                if (contact != null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.OK);
                    respMess.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                    return respMess;
                }
                else
                {
                    LeadEntity lead = null;
                    // INFO - har inte refaktoriserats på grund av att felmeddelanden ska vara HttpResponseMessage
                    StatusBlock canLeadBeCreated = LeadEntity.CanLeadBeCreated(localContext, customerInfo);
                    if (canLeadBeCreated.TransactionOk)
                    {
                        lead = new LeadEntity(localContext, customerInfo);
                        lead.LeadSourceCode = Generated.lead_leadsourcecode.LaddaKort;
                        lead.Subject = NonLoginRefillSubject;
                        lead.Id = localContext.OrganizationService.Create(lead);
                    }
                    else if (canLeadBeCreated.StatusBlockCode == (int)CustomerUtility.StatusBlockCode.OtherLeadFound)
                    {
                        try
                        {
                            lead = XrmRetrieveHelper.Retrieve<LeadEntity>(localContext, new Guid(canLeadBeCreated.Information), LeadEntity.LeadInfoBlock);
                            bool update = false;
                            if (lead.UpdateWithCustomerInfo(localContext, customerInfo))
                                update = true;
                            if (!NonLoginRefillSubject.Equals(lead.Subject))
                            {
                                lead.Subject = NonLoginRefillSubject;
                                update = true;
                            }
                            if (!Generated.lead_leadsourcecode.LaddaKort.Equals(lead.LeadSourceCode))
                            {
                                lead.LeadSourceCode = Generated.lead_leadsourcecode.LaddaKort;
                                update = true;
                            }
                            if (update)
                                XrmHelper.Update(localContext.OrganizationService, lead);
                        }
                        catch (Exception e)
                        {
                            _log.DebugFormat("Unexpected Exception caught when retrieving Lead. Error messagee: {0}", e.Message);
                            HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            errorResp.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                            return errorResp;
                        }
                    }
                    else
                    {
                        HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        errorResp.Content = new StringContent(canLeadBeCreated.ErrorMessage);
                        return errorResp;
                    }
                    
                    HttpResponseMessage leadResp = new HttpResponseMessage(HttpStatusCode.OK);
                    leadResp.Content = new StringContent(SerializeNoNull(lead.ToCustomerInfo(localContext)));
                    return leadResp;
                }

            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static HttpResponseMessage RGOL([FromBody] CustomerInfo customerInfo)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                if (customerInfo == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return respMess;
                }

                //// Validate connection to server
                //WhoAmIResponse whoAmI = (WhoAmIResponse)localContext.OrganizationService.Execute(new WhoAmIRequest());
                //_log.DebugFormat("RGOL, whoAmI:{0}", whoAmI.UserId.ToString());

                // Validate info
                _log.DebugFormat("RGOL, validating info");
                customerInfo.Source = (int)CustomerUtility.Source.RGOL;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customerInfo);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(validateStatus.ErrorMessage);
                    return rm;
                }

                // Find/ Create Customer
                _log.DebugFormat("RGOL, calling FindOrCreateUnvalidatedContact");
                ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo);
                _log.DebugFormat("RGOL, Found/created contact {0}", contact == null ? "null" : contact.ContactId.ToString());

                // Attach Lead if contains Email
                if (!string.IsNullOrWhiteSpace(customerInfo.Email))
                {
                IList<LeadEntity> leads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadEntity.LeadInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                            new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                            new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName),
                            new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                        }
                    });
                bool updated = false;
                foreach (LeadEntity lead in leads)
                {
                    if (ContactEntity.UpdateContactWithLead(ref contact, lead))
                        updated = true;
                }
                if (updated)
                {
                    contact.ed_InformationSource = Generated.ed_informationsource.RGOL;
                    localContext.OrganizationService.Update(contact);
                }
                }


                HttpResponseMessage finalrm = new HttpResponseMessage(HttpStatusCode.OK);
                finalrm.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return finalrm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        public static ContactEntity QualifyLeadToContact(Plugin.LocalPluginContext localContext, LeadEntity lead)
        {
            QualifyLeadRequest req = new QualifyLeadRequest()
            {
                LeadId = lead.ToEntityReference(),
                CreateAccount = false,
                CreateContact = true,
                CreateOpportunity = false,
                Status = new OptionSetValue((int)Generated.lead_statuscode.Qualified)
            };

            QualifyLeadResponse resp = (QualifyLeadResponse)localContext.OrganizationService.Execute(req);
            ContactEntity qualifiedContact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, resp.CreatedEntities.FirstOrDefault().Id)
                    }
                });
            if (qualifiedContact == null)
            {
                throw new Exception(string.Format("Lead validation failed. No contact found."));
            }
            if (ContactEntity.UpdateContactWithLead(ref qualifiedContact, lead))
                localContext.OrganizationService.Update(qualifiedContact);
            return qualifiedContact;
        }

        public static HttpResponseMessage NotifyMKLSent(NotificationInfo notificationInfo)
        {
            try
            {
                _log.Debug("Entered NotifyMKLSent");

                HttpResponseMessage verificationRespMess = VerifyNotificationInfo(notificationInfo);
                if (verificationRespMess.StatusCode != HttpStatusCode.OK)
                    return verificationRespMess;

                Plugin.LocalPluginContext localContext = GenerateContext();

                ContactEntity contact = null;
                contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, notificationInfo.Guid)
                        }
                    });
                if (contact == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, notificationInfo.Guid))
                    };
                }

                NotifyMKLEntity notification = new NotifyMKLEntity
                {
                    Subject = $"{notificationInfo.MessageType} - {((Generated.ed_notifymkl_ed_method)notificationInfo.ContactType).ToString()}",
                    ed_MessageType = notificationInfo.MessageType,
                    ed_Method = (Generated.ed_notifymkl_ed_method)notificationInfo.ContactType,
                    ed_TargetAddress = notificationInfo.SendTo,
                    RegardingObjectId = new EntityReference
                    {
                        Id = contact.Id,
                        LogicalName = ContactEntity.EntityLogicalName
                    }
                };
                notification.Id = XrmHelper.Create(localContext.OrganizationService, notification);

                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(Resources.NotificationCreated)
                };
            }
            catch (Exception e)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, e.Message));
                return rm;
            }
        }

        private static HttpResponseMessage VerifyNotificationInfo(NotificationInfo notificationInfo)
        {
            if (string.IsNullOrWhiteSpace(notificationInfo.MessageType))
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.NotificationMissingMessageType);
                return rm;
            }
            if (string.IsNullOrWhiteSpace(notificationInfo.SendTo))
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.NotificationMissingSendTo);
                return rm;
            }
            if (notificationInfo.ContactType != 0 && notificationInfo.ContactType != 1)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(string.Format(Resources.NotificationUnrecognisedContactType, notificationInfo.ContactType));
                return rm;
            }
            if (string.IsNullOrWhiteSpace(notificationInfo.Guid))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Resources.GuidMissing)
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public static SendEmailResponse SendConfirmationEmail(Plugin.LocalPluginContext localContext, ContactEntity contact)
        {
            string emailCofnfirmationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailConfirmation);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, emailCofnfirmationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, emailCofnfirmationTemplateName));

            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, contact.ToEntityReference());

            email.RegardingObjectId = contact.ToEntityReference();

            return SetToFromAndSendEmail(localContext, contact.ToEntityReference(), email);
        }

        public static SendEmailResponse SendValidationEmail(Plugin.LocalPluginContext localContext, ContactEntity to)
        {
            string contactValidationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailValidationContact);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, contactValidationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, contactValidationTemplateName));

            CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, EmailEntity.CgiSettingColumnSet);
            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, to.ToEntityReference(), new List<Entity>{(Entity)to, (Entity)setting });

            email.RegardingObjectId = to.ToEntityReference();

            return SetToFromAndSendEmail(localContext, to , email);
        }

        public static SendEmailResponse SendValidationEmail(Plugin.LocalPluginContext localContext, LeadEntity to)
        {
            string leadValidationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailValidationLead);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, leadValidationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, leadValidationTemplateName));

            CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, EmailEntity.CgiSettingColumnSet);
            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, to.ToEntityReference(), new List<Entity> { (Entity)to, (Entity)setting });

            email.RegardingObjectId = to.ToEntityReference();

            return SetToFromAndSendEmail(localContext, to.ToEntityReference(), email);
        }

        private static SendEmailResponse SetToFromAndSendEmail(Plugin.LocalPluginContext localContext, ContactEntity to, EmailEntity email)
        {
            _log.Debug($"Entered function SetToFromAndSendEmail() with recipientEmail as String param");
            EntityReference defaultQueue;
            try
            {
                defaultQueue = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue);
            }
            catch (MissingFieldException e)
            {
                _log.Error($"MissingFieldException caught when calling GetSettingEntRef() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                throw e;
            }
            catch (Exception)
            {
                throw;
            }

            // Create the 'From:' activity party for the email
            ActivityPartyEntity fromParty = new ActivityPartyEntity
            {
                PartyId = defaultQueue
            };

            // Create the 'To:' activity party for the email
            ActivityPartyEntity toParty = new ActivityPartyEntity
            {
                PartyId = to.ToEntityReference(),
                AddressUsed = to.ed_EmailToBeVerified
            };

            email.To = new ActivityPartyEntity[] { toParty };
            email.From = new ActivityPartyEntity[] { fromParty };

            email.Id = localContext.OrganizationService.Create(email);

            // Use the SendEmail message to send an e-mail message.
            SendEmailRequest sendEmailreq = new SendEmailRequest
            {
                EmailId = email.Id,
                IssueSend = true
            };

            SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(sendEmailreq);
            _log.Debug($"Email sent. Response.Subject = {sendEmailresp.Subject}");
            return sendEmailresp;
        }

        private static SendEmailResponse SetToFromAndSendEmail(Plugin.LocalPluginContext localContext, EntityReference to, EmailEntity email)
        {
            _log.Debug($"Entered function SetToFromAndSendEmail()");
            EntityReference defaultQueue; 
            try
            {
                 defaultQueue = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue);
            }
            catch (MissingFieldException e)
            {
                _log.Error($"MissingFieldException caught when calling GetSettingInt() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                throw e;
            }
            catch (Exception)
            {
                throw;
            }

            // Create the 'From:' activity party for the email
            ActivityPartyEntity fromParty = new ActivityPartyEntity
            {
                PartyId = defaultQueue
            };

            // Create the 'To:' activity party for the email
            ActivityPartyEntity toParty = new ActivityPartyEntity
            {
                PartyId = to
            };

            email.To = new ActivityPartyEntity[] { toParty };
            email.From = new ActivityPartyEntity[] { fromParty };

            email.Id = localContext.OrganizationService.Create(email);

            // Use the SendEmail message to send an e-mail message.
            SendEmailRequest sendEmailreq = new SendEmailRequest
            {
                EmailId = email.Id,
                IssueSend = true
            };

            SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(sendEmailreq);
            _log.Debug($"Email sent. Response.Subject = {sendEmailresp.Subject}");
            return sendEmailresp;
        }

        public static HttpResponseMessage UpdateContact([FromBody] CustomerInfo customer)
        {
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                if (customer == null)
                {
                    HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    respMess.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                    return respMess;
                }

                // Validera inkommande information (validateCustomerInfo)
                customer.Source = (int)CustomerUtility.Source.UppdateraMittKonto;
                StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, customer);
                if (!validateStatus.TransactionOk)
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    responseMessage.Content = new StringContent(validateStatus.ErrorMessage);
                    return responseMessage;
                }

                // Uppdatera och returnera lämpligt meddelande
                ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                    new FilterExpression()
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customer.Email),
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, customer.Guid)
                        }
                    });
                if (contact == null)
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customer.Email + " och " + customer.Guid));
                    return response;
                }
                //customer.Guid = contact.Id.ToString();
                
                // Validera att en eventuell uppdatering inte skulle bryta mot affärsregler (inga dubletter) - Om så returnera lämpligt felmeddelande
                if (customer.SwedishSocialSecurityNumberSpecified && customer.SwedishSocialSecurityNumber) {
                    QueryExpression query = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = new ColumnSet(false),
                        Criteria = new FilterExpression()
                        {
                            Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customer.SocialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.NotEqual, customer.Guid)
                        }
                        }
                    };
                    IList<ContactEntity> possibleConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);
                    if (possibleConflicts.Count > 0)
                    {
                        // Om kunden har fått ett nytt, svenskt personnummer som krockar med existerande kund
                        if (string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber) && customer.SwedishSocialSecurityNumberSpecified && customer.SwedishSocialSecurityNumber && !string.IsNullOrWhiteSpace(customer.SocialSecurityNumber))
                        {
                            if (possibleConflicts.Count > 1)
                            {
                                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                response.Content = new StringContent(Resources.MultipleSocSecConflictsFound);
                                return response;
                            }
                            ContactEntity conflict = possibleConflicts[0];
                            ContactEntity conflictUpdate = new ContactEntity()
                            {
                                ContactId = conflict.ContactId,
                                ed_InformationSource = Generated.ed_informationsource.AdmAndraKund,
                                cgi_socialsecuritynumber = null,
                                ed_ConflictConnectionGuid = Guid.NewGuid().ToString()
                            };
                            XrmHelper.Update(localContext.OrganizationService, conflictUpdate);
                            contact.ed_ConflictConnectionGuid = conflictUpdate.ed_ConflictConnectionGuid;
                        }
                        else {
                            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            response.Content = new StringContent(Resources.CouldNotUpdateCustomerSocialSecurityConflict);
                            return response;
                        }
                    }
                }

                ContactEntity newInfo = new ContactEntity(localContext, customer);
                if (contact.IsAnyAttributeModified(newInfo, ContactEntity.ContactInfoBlock.Columns.ToArray()))
                {
                    contact.CombineAttributes(newInfo);
                    contact.ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto;
                    localContext.OrganizationService.Update(contact);
                }

                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                return resp;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }



        public static HttpResponseMessage ValidateEmail(Guid customerId, int entityTypeCode, string latestLinkGuid, string mklId)
        {
            _log.DebugFormat($"Entered ValidateEmail(), customerId: {customerId}, entityTypeCode: {entityTypeCode}, latestLinkGuid: {latestLinkGuid}");
            try
            {
                Plugin.LocalPluginContext localContext = GenerateContext();
                // validating indata
                if (customerId == null || Guid.Empty.Equals(customerId))
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.GuidMissing);
                    return rm;
                }
                if (string.IsNullOrWhiteSpace(mklId))
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(Resources.MklIdMissing);
                    return rm;
                }

                switch (entityTypeCode)
                {
                    #region case Contact
                    case ContactEntity.EntityTypeCode: // Contact
                        _log.DebugFormat("Retrieving Contact with Id: {0}", customerId);

                        ColumnSet columnSet = ContactEntity.ContactInfoBlock;
                        columnSet.AddColumn(ContactEntity.Fields.ed_LatestLinkGuid);
                        ContactEntity contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, columnSet, new FilterExpression()
                        {
                            Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.Equal, customerId)
                                }
                        });
                        if (contact == null)
                        {
                            _log.Error("Could not find a Contact with the provided Guid.");
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.NotFound);
                            rml.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customerId));
                            return rml;
                        }
                        if (string.IsNullOrWhiteSpace(contact.ed_LatestLinkGuid))
                        {
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(string.Format(Resources.NoLinkGuidFoundOnContact));
                            return rml;
                        }
                        if (contact.ed_LinkExpiryDate.HasValue && contact.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) > 0)
                        {
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(string.Format(Resources.OldLinkUsed));
                            return rml;
                        }
                        if (string.IsNullOrWhiteSpace(contact.ed_EmailToBeVerified))
                        {
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(string.Format(Resources.NoEmailToBeVerifiedFoundOnContact));
                            return rml;
                        }
                        if (!contact.ed_LatestLinkGuid.Equals(latestLinkGuid))
                        {
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(string.Format(Resources.LinkNotMeantForThisContact));
                            return rml;
                        }

                        #region Conflicting Contacts Query
                        FilterExpression conflictFilter = new FilterExpression(LogicalOperator.Or);
                        FilterExpression mailFilter = new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.ed_EmailToBeVerified),
                                //new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                //new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                            }
                        };
                        conflictFilter.AddFilter(mailFilter);
                        if (contact.ed_HasSwedishSocialSecurityNumber.HasValue && contact.ed_HasSwedishSocialSecurityNumber.Value)
                        {
                            FilterExpression socsecFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contact.cgi_socialsecuritynumber),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)

                                }
                            };
                            conflictFilter.AddFilter(socsecFilter);
                        }
                        QueryExpression conflictContactQuery = new QueryExpression()
                        {
                            EntityName = ContactEntity.EntityLogicalName,
                            ColumnSet = new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.EMailAddress1),
                            Criteria = conflictFilter
                        };
                        #endregion
                        IList<ContactEntity> contactConflicts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, conflictContactQuery);
                        if (contactConflicts.Count > 0)
                        {
                            if (contact.ed_EmailToBeVerified.Equals(contactConflicts[0].EMailAddress1))
                            {
                                _log.DebugFormat($"Found existing, validated contact with conflicting email {contact.ed_EmailToBeVerified}. Cannot validate this contact.");
                                HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerEmail);
                                return rm3;
                            }
                            else
                            {
                                _log.DebugFormat($"Found existing, validated contact with Social Security Number {contactConflicts[0].cgi_socialsecuritynumber}. Cannot validate this contact.");
                                HttpResponseMessage rm3 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                                rm3.Content = new StringContent(Resources.CouldNotVerifyCustomerSocSec);
                                return rm3;
                            }
                        }

                        #region MergeContacts Query
                        FilterExpression mergeContactFilter = new FilterExpression(LogicalOperator.Or);
                        FilterExpression nameFilter = new FilterExpression(LogicalOperator.Or)
                        {
                            Filters =
                            {
                                new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName)
                                    }
                                },
                                new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                                    }
                                }
                            }
                        };

                        if (contact.ed_HasSwedishSocialSecurityNumber.HasValue && contact.ed_HasSwedishSocialSecurityNumber.Value)
                        {
                            FilterExpression persNrFilter = new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, contact.cgi_socialsecuritynumber),
                                    new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                                },
                                Filters =
                                {
                                    nameFilter
                                }
                            };
                            mergeContactFilter.AddFilter(persNrFilter);
                        }
                        FilterExpression epostFilter = new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, contact.EMailAddress1),
                                new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contact.Id)
                            },
                            Filters =
                                {
                                    nameFilter
                                }
                        };
                        mergeContactFilter.AddFilter(epostFilter);
                        QueryExpression mergeContactQuery = new QueryExpression()
                        {
                            EntityName = ContactEntity.EntityLogicalName,
                            ColumnSet = ContactEntity.ContactInfoBlock,
                            Criteria = mergeContactFilter
                        };
                        #endregion
                        IList<ContactEntity> mergeContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, mergeContactQuery);
                        _log.DebugFormat("Found {0} contacts to merge with the one to be validated", mergeContacts.Count);
                        #region Conflicting Leads Query
                        QueryExpression conflictLeadQuery = new QueryExpression()
                        {
                            EntityName = LeadEntity.EntityLogicalName,
                            ColumnSet = LeadEntity.LeadInfoBlock,
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, contact.EMailAddress1),
                                    new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, contact.FirstName),
                                    new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, contact.LastName),
                                    new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open),
                                    new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                                }
                            }
                        };
                        #endregion
                        IList<LeadEntity> leadConflicts = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, conflictLeadQuery);
                        _log.DebugFormat("Found {0} conflicting leads to merge with the one to be validated", leadConflicts.Count);
                        
                        if (mergeContacts.Count > 0)
                        {
                            contact.CombineContacts(localContext, mergeContacts);
                        }
                        if (leadConflicts.Count > 0)
                        {
                            foreach (LeadEntity l in leadConflicts)
                                ContactEntity.UpdateContactWithLead(ref contact, l);
                        }

                        contact.EMailAddress1 = contact.ed_EmailToBeVerified;
                        contact.ed_EmailToBeVerified = null;
                        contact.EMailAddress2 = null;
                        contact.ed_LatestLinkGuid = null;
                        contact.ed_MklId = mklId;
                        contact.ed_InformationSource = Generated.ed_informationsource.UppdateraMittKonto;
                        localContext.OrganizationService.Update(contact);

                        SendConfirmationEmail(localContext, contact);
                        
                        HttpResponseMessage rm2 = new HttpResponseMessage(HttpStatusCode.OK);
                        rm2.Content = new StringContent(SerializeNoNull(contact.ToContactInfo(localContext)));
                        return rm2;
                    #endregion
                    #region case Lead
                    case LeadEntity.EntityTypeCode: // Lead
                        #region Find Lead
                        ContactEntity newContact = null;
                        _log.DebugFormat("Retrieving Lead with Id: {0}", customerId);
                        ColumnSet columns = LeadEntity.LeadInfoBlock;
                        columns.AddColumn(LeadEntity.Fields.ed_LatestLinkGuid);
                        columns.AddColumn(LeadEntity.Fields.ed_LinkExpiryDate);
                        LeadEntity lead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, columns, new FilterExpression()
                        {
                            Conditions =
                            {
                                new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.Equal, customerId),
                                new ConditionExpression(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open)
                            }
                        });
                        if (lead == null)
                        {
                            _log.ErrorFormat("Could not find a Lead with the provided Guid: {0}", customerId);
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.NotFound);
                            rml.Content = new StringContent(string.Format(Resources.CouldNotFindContactWithInfo, customerId));
                            return rml;
                        }
                        if (string.IsNullOrWhiteSpace(lead.ed_LatestLinkGuid))
                        {
                            _log.ErrorFormat("Could not find a LatestLinkGuid on the retrieved Lead");
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(Resources.NoLinkGuidFoundOnLead);
                            return rml;
                        }
                        if (!lead.ed_LatestLinkGuid.Equals(latestLinkGuid))
                        {
                            _log.ErrorFormat("LatestLinkGuid used is not the expected one");
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(Resources.LinkNotMeantForThisLead);
                            return rml;
                        }
                        if (lead.ed_LinkExpiryDate.HasValue && lead.ed_LinkExpiryDate.Value.CompareTo(DateTime.Now) < 0)
                        {
                            _log.ErrorFormat("LinkExpiryDate is less that DateTime.Now");
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(Resources.OldLinkUsed);
                            return rml;
                        }
                        if (string.IsNullOrWhiteSpace(lead.EMailAddress1))
                        {
                            _log.ErrorFormat("Lead to be validated does not contain an email address");
                            HttpResponseMessage rml = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rml.Content = new StringContent(Resources.NoEmailToBeVerifiedFoundOnLead);
                            return rml;
                        }
                        #endregion
                        QueryExpression contactConflictQuery = CreateContactConflictQuery(lead);
                        IList<ContactEntity> conflictContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, contactConflictQuery);
                        _log.DebugFormat("Found {0} conflicting Contacts", conflictContacts.Count);
                        #region make Lead Conflict Query
                        QueryExpression leadConflictQuery = new QueryExpression()
                        {
                            EntityName = LeadEntity.EntityLogicalName,
                            ColumnSet = LeadEntity.LeadInfoBlock,
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression(LeadEntity.Fields.Id, ConditionOperator.NotEqual, lead.Id),
                                    new ConditionExpression(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, lead.EMailAddress1),
                                    new ConditionExpression(LeadEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                                    new ConditionExpression(LeadEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName),
                                    new ConditionExpression(LeadEntity.Fields.CampaignId, ConditionOperator.Null)
                                }
                            }
                        };
                        #endregion
                        IList<LeadEntity> conflictLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, leadConflictQuery);
                        _log.DebugFormat("Found {0} conflicting Leads", conflictLeads.Count);

                        #region if Contact conflicts are found
                        if (conflictContacts.Count > 0)
                        {
                            ContactEntity socialSecurityConflict = null;
                            foreach (ContactEntity c in conflictContacts)
                            {
                                if (lead.ed_Personnummer != null && lead.ed_Personnummer.Equals(c.cgi_socialsecuritynumber))
                                {
                                    if (socialSecurityConflict == null)
                                    {
                                        _log.DebugFormat("Found Contact matching on social security number.");
                                        socialSecurityConflict = c;
                                    }
                                    else
                                    {
                                        _log.DebugFormat("Multiple conflicting Contacts with the same socialSecurityNumber were found.");
                                        HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                                        respMess.Content = new StringContent(Resources.MultipleSocSecConflictsFound);
                                        return respMess;
                                    }
                                }
                            }
                            if (socialSecurityConflict != null)
                            {
                                conflictContacts.Remove(socialSecurityConflict);
                                newContact = socialSecurityConflict;
                                UpdateContactWithAuthorityLead(ref newContact, lead);

                                SetStateRequest req = new SetStateRequest()
                                {
                                    EntityMoniker = lead.ToEntityReference(),
                                    State = new OptionSetValue((int)Generated.LeadState.Disqualified),
                                    Status = new OptionSetValue((int)Generated.lead_statuscode.Canceled)
                                };
                                SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);

                                newContact.EMailAddress1 = lead.EMailAddress1;
                                newContact.EMailAddress2 = null;
                            }
                            else
                            {
                                //newContact = new ContactEntity(lead.ToCustomerInfo());
                                //newContact.Id = localContext.OrganizationService.Create(newContact);
                                _log.DebugFormat("No SocialSecurity conflict was found. Qualifying Lead.");
                                newContact = QualifyLeadToContact(localContext, lead);
                            }
                            _log.DebugFormat($"Combining {conflictContacts.Count} conflicting contacts.");
                            newContact.CombineContacts(localContext, conflictContacts);

                            ////inactivate other customers
                            //foreach (ContactEntity c in conflictContacts)
                            //{
                            //    SetStateRequest req = new SetStateRequest()
                            //    {
                            //        EntityMoniker = c.ToEntityReference(),
                            //        State = new OptionSetValue((int)Generated.ContactState.Inactive),
                            //        Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
                            //    };
                            //    localContext.OrganizationService.Execute(req);
                            //}
                        }
                        #endregion
                        else
                        {
                            _log.DebugFormat("No valid existing contact found, creating new from the Lead.");
                            newContact = QualifyLeadToContact(localContext, lead);
                        }

                        if (newContact == null)
                        {
                            HttpResponseMessage respMess = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                            respMess.Content = new StringContent(Resources.UnexpectedErrorWhenValidatingEmail);
                            return respMess;
                        }

                        if (conflictLeads.Count > 0)
                        {
                            _log.DebugFormat("Merging {0} Leads with the new Contact", conflictLeads.Count);
                            foreach (LeadEntity leadConflict in conflictLeads)
                            {
                                ContactEntity.UpdateContactWithLead(ref newContact, leadConflict);
                            }
                        }

                        if (newContact.DoNotEMail == true)
                            newContact.DoNotEMail = false;

                        newContact.ed_MklId = mklId;
                        newContact.ed_InformationSource = Generated.ed_informationsource.SkapaMittKonto;
                        localContext.OrganizationService.Update(newContact);

                        SendConfirmationEmail(localContext, newContact);

                        HttpResponseMessage rm4 = new HttpResponseMessage(HttpStatusCode.OK);
                        rm4.Content = new StringContent(SerializeNoNull(newContact.ToContactInfo(localContext)));
                        return rm4;

                    #endregion
                    default:
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(string.Format(Resources.UnsupportedEntityTypeCode, entityTypeCode));
                        return rm;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
        }

        private static QueryExpression CreateContactConflictQuery(LeadEntity lead)
        {
            #region make Contact Conflict Query
            FilterExpression contactConflictFilter = new FilterExpression(LogicalOperator.Or);
            if (lead.ed_HasSwedishSocialSecurityNumber.HasValue && lead.ed_HasSwedishSocialSecurityNumber.Value)
            {
                FilterExpression SocSecFilter = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                    new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, lead.ed_Personnummer)
                                }
                };
                contactConflictFilter.AddFilter(SocSecFilter);
            }
            FilterExpression nameMailFilter = new FilterExpression(LogicalOperator.And)
            {
                Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, lead.EMailAddress1)
                            },
                Filters =
                            {
                                new FilterExpression(LogicalOperator.Or)
                                {
                                    Filters =
                                    {
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, lead.FirstName),
                                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lead.LastName)
                                            }
                                        },
                                        new FilterExpression(LogicalOperator.And)
                                        {
                                            Conditions =
                                            {
                                                new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                                                new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                                            }
                                        }
                                    }
                                }
                            }
            };
            contactConflictFilter.AddFilter(nameMailFilter);

            ColumnSet contactConflictColumnSet = ContactEntity.ContactInfoBlock;
            contactConflictColumnSet.AddColumn(ContactEntity.Fields.DoNotEMail);

            QueryExpression contactConflictQuery = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = contactConflictColumnSet,
                Criteria = contactConflictFilter
            };
            #endregion
            return contactConflictQuery;
        }

        private static bool UpdateContactWithAuthorityLead(ref ContactEntity contact, LeadEntity lead)
        {
            bool updated = false;
            if (!string.IsNullOrWhiteSpace(lead.FirstName) && !lead.FirstName.Equals(contact.FirstName))
            {
                contact.FirstName = lead.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.LastName) && !lead.LastName.Equals(contact.LastName))
            {
                contact.LastName = lead.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.Telephone1) && !lead.Telephone1.Equals(contact.Telephone1))
            {
                contact.Telephone1 = lead.Telephone1;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.MobilePhone) && !lead.MobilePhone.Equals(contact.Telephone2))
            {
                contact.Telephone2 = lead.MobilePhone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.ed_Personnummer) && !lead.ed_Personnummer.Equals(contact.cgi_socialsecuritynumber))
            {
                contact.cgi_socialsecuritynumber = lead.ed_Personnummer;
                contact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
                updated = true;
            }

            if (!(string.IsNullOrWhiteSpace(lead.Address1_Line1) &&
                string.IsNullOrWhiteSpace(lead.Address1_Line2) &&
                string.IsNullOrWhiteSpace(lead.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(lead.Address1_City) &&
                lead.ed_Address1_Country == null))
            {
                updated = true;
                contact.Address1_Line1 = lead.Address1_Line1;
                contact.Address1_Line2 = lead.Address1_Line2;
                contact.Address1_PostalCode = lead.Address1_PostalCode;
                contact.Address1_City = lead.Address1_City;
                contact.ed_Address1_Country = lead.ed_Address1_Country;
            }

            if (lead.ed_InformationSource.Value != contact.ed_InformationSource.Value)
            {
                contact.ed_InformationSource = lead.ed_InformationSource;
                updated = true;
            }
            // TODO teo - transfer activities from lead to contact.

            return updated;
        }

        public static string SerializeNoNull(CustomerInfo info)
        {
            return JsonConvert.SerializeObject(info, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeNoNull(NotificationInfo info)
        {
            return JsonConvert.SerializeObject(info, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
        
        internal static Plugin.LocalPluginContext GenerateContext()
        {
            HttpContext httpContext = HttpContext.Current;

            CrmServiceClient _conn = null;
            if(httpContext != null)
            {
                _conn = httpContext.Cache.Get(_generateContextString) as CrmServiceClient;
            }
            if (_conn == null)
            {
                _log.Debug("Creating new connection from CRM (no cache found)");
                string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
                _log.DebugFormat("connectionString = {0}", connectionString);
                //  Connect to the CRM web service using a connection string.
                _conn = new CrmServiceClient(connectionString);

                _log.DebugFormat("Connection state: _conn.IsReady = {0}", _conn.IsReady);
                if (_conn.IsReady)
                {
                    httpContext.Cache.Insert(_generateContextString, _conn, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                }
                else
                {

                }
            }
            //string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);

            ////  Connect to the CRM web service using a connection string.
            //CrmServiceClient _conn = new CrmServiceClient(connectionString);
            
            _log.DebugFormat("Creating serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            //IOrganizationService serviceProxy = (IOrganizationService)_conn.OrganizationWebProxyClient != null ? (IOrganizationService)_conn.OrganizationWebProxyClient : (IOrganizationService)_conn.OrganizationServiceProxy;
            OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)_conn.OrganizationServiceProxy;
            _log.DebugFormat("Assembling localContext");
            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            if (localContext.OrganizationService == null)
                throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

            return localContext;
        }

        public class GuidsPlaceholder
        {
            public string LinkId { get; set; }

            public string CrmGuid { get; set; }
        }
    }
}