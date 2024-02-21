using Skanetrafiken.Crm.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Globalization;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Skanetrafiken.Crm.Controllers
{
    public class ContactsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("api/Contacts/GetAccessToken/{process}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccessToken(string process)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetAccessToken called with parameter: {process}");

            if (string.IsNullOrWhiteSpace(process))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find value in 'process' parameter. Please provide a valid 'process'.");
                _log.Warn($"Th={threadId} - Returning statuscode = {badReq.StatusCode}, Content = {badReq.Content.ReadAsStringAsync().Result}\n");
                return badReq;
            }

            Plugin.LocalPluginContext localContext = null;

            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                _log.DebugFormat($"Th={threadId} - GetAccessToken: Creating serviceProxy");
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    _log.Info($"Th={threadId} - GetAccessToken: ServiceProxy and LocalContext created Successfully. Getting Info from Settings.");

                    var clientId = string.Empty;
                    var clientSecret = string.Empty;
                    var tenentId = string.Empty;
                    var audience = string.Empty;

                    CgiSettingEntity settings = null;
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);


                    if (process == "mkl")
                    {
                        _log.Info($"Th={threadId} - GetAccessToken: Process is - {process}");

                        settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationClientId, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationClientSecret, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.st_CrmAppRegistrationTenantId, ConditionOperator.NotNull);
                        settingFilter.AddCondition(CgiSettingEntity.Fields.st_crmappmklaudience, ConditionOperator.NotNull);

                        settings = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, new ColumnSet(
                        CgiSettingEntity.Fields.st_CrmAppRegistrationClientId,
                        CgiSettingEntity.Fields.st_CrmAppRegistrationClientSecret,
                        CgiSettingEntity.Fields.st_CrmAppRegistrationTenantId,
                        CgiSettingEntity.Fields.st_crmappmklaudience), settingFilter);

                        clientId = settings.st_CrmAppRegistrationClientId;
                        _log.Debug($"Th={threadId} - GetAccessToken: Client Id - {clientId}");
                        clientSecret = settings.st_CrmAppRegistrationClientSecret;
                        _log.Debug($"Th={threadId} - GetAccessToken: Client Secret - {clientSecret}");
                        tenentId = settings.st_CrmAppRegistrationTenantId;
                        _log.Debug($"Th={threadId} - GetAccessToken: TenentId - {tenentId}");
                        audience = settings.st_crmappmklaudience;
                        _log.Debug($"Th={threadId} - GetAccessToken: Audience - {audience}");
                    }
                    else
                    {
                        //Throw an exception
                        throw new Exception(string.Format("GetAccessToken: Provided 'Process' argument does not match any available processes. Please provide a valid 'Process'."));
                    }

                    if (settings == null || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tenentId) || string.IsNullOrWhiteSpace(audience))
                    {
                        //Throw an exception
                        throw new Exception(string.Format("GetAccessToken: Failed to aquire settings from CRM."));
                    }
                    else
                    {
                        _log.Debug($"Th={threadId} - GetAccessToken: Fetching Access Token.");
                        
                        var tokenResp = await CrmPlusControl.GetAccessToken(clientId, clientSecret, tenentId, audience);

                        //Create a return OK with token in string
                        return ReturnApiMessage(threadId, tokenResp, HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        private static HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {
            _log.DebugFormat($"Th={threadId} - Returning statuscode = {code}, Content = {errorMessage}\n");

            HttpResponseMessage response = new HttpResponseMessage(code);
            response.Content = new StringContent(errorMessage);
            return response;
        }

        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage GetLatestLinkGuid(string email)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - RetrieveLatestLinkGuid called for '{email}'.");
            if (string.IsNullOrWhiteSpace(email))
            {
                HttpResponseMessage returnMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                returnMessage.Content = new StringContent(Resources.ParameterMissingEmail);
                _log.Warn($"Th={threadId} - Returning statuscode = {returnMessage.StatusCode}, Content = {returnMessage.Content.ReadAsStringAsync().Result}\n");
                return returnMessage;
            }

            HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveContactLinkGuid(threadId, email);
            //Return Logg
            if (linkGuidResponse.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}, Content = {linkGuidResponse.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}, Content = {linkGuidResponse.Content.ReadAsStringAsync().Result}\n");
            }

            return linkGuidResponse;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Error($"Th={threadId} - Unsupported generic GET called.");

            //System.Diagnostics.Stopwatch counter = new System.Diagnostics.Stopwatch();
            //counter.Restart();
            //string mess = "";

            //for (int i = 0; i < 15; i++)
            //{
            //    string messAdd = CrmPlusControl.util();
            //    mess = string.Format("{0}{1}", mess, messAdd);
            //    mess = string.Format("{0}{1}", mess, string.Format("\nUtilCount = {0}, counter = {1}\n", i, counter.ElapsedMilliseconds));
            //}

            //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            //resp.Content = new StringContent(mess);
            //return resp;

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.Error($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET called with parameter: id = {id}");
            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage guidResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                guidResp.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {guidResp.StatusCode}, Content = {guidResp.Content.ReadAsStringAsync().Result}\n");
                return guidResp;
            }

            if (this.Request.Headers == null) 
            {
                _log.Warn($"Th={threadId} - Exception caught: {Resources.HeadersMissing}\n");
                throw new Exception(Resources.HeadersMissing);
            }

            string MKLHeaderName = ConfigurationManager.AppSettings["MKLTokenHeaderName"];

            // TODO :
            // If call is made from Company Portal they're using MKLCertificate. It is gonna change. but for now use this.
            if (!(this.Request.Headers.Contains(MKLHeaderName)))
            {
                // TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(id);
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        _log.Warn($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                    return rm;
                }
            }

            HttpResponseMessage resp = CrmPlusControl.GetContact(threadId, id);

            //Return Logg
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }

            return resp;
        }

        [HttpPost]
        public HttpResponseMessage ClearMKLId(string mklId)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - POST called with parameter: mklId = {mklId}");

            if (string.IsNullOrWhiteSpace(mklId))
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.MklIdMissing);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            return CrmPlusControl.ClearMKLIdContact(threadId, mklId);
        }

        /// <summary>
        /// Controller used for create customers of different types
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CustomerInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - POST called.\n");
            _log.DebugFormat($"Th={threadId} - POST called with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

            if (info == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            //// *-* INFO: *-*

            // Token verification inactivated for calls that didn't require Guid.

            //// *-* /INFO *-*

            //// TOKEN VERIFICATION
            //try
            //{
            //    HttpResponseMessage tokenResp = TokenValidation();
            //    if (tokenResp.StatusCode != HttpStatusCode.OK)
            //    {
            //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
            //        return tokenResp;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
            //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
            //    return rm;
            //}

            if (info.ServiceType == 1)
            {
                _log.Info($"Th={threadId} - Setting InformationSource to KöpOchSkicka.\n");
                info.Source = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                if (info != null && info.AddressBlock != null && !String.IsNullOrEmpty(info.AddressBlock.PostalCode))
                {
                    info.AddressBlock.PostalCode = Regex.Replace(info.AddressBlock.PostalCode, @"\s+", "");
                }
            }
            else if (info.ServiceType == 2) 
            {
                _log.Info($"Th={threadId} - Setting InformationSource to ForetagsPortal.\n");
                info.Source = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
            }
            
            // Format Customer info
            FormatCustomerInfo(ref info);
            

            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKundArende:
                    rm = CrmPlusControl.NonLoginCustomerIncident(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.RGOL:
                    rm = CrmPlusControl.RGOL(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.PASS:
                    rm = CrmPlusControl.PASS(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKop:
                    rm = CrmPlusControl.NonLoginPurchase(threadId, info); // --- Private Contact
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal: // Business Contact 
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal: // School Contact
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal: // Senior Contact
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                    rm = CrmPlusControl.KopOchSkickaKund(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
        }

        //[HttpPost]
        //public HttpResponseMessage Post2Param([FromUri] string time, [FromUri] int price, [FromBody] CustomerInfo info)
        //{
        //    _log.DebugFormat("Post called with 2 parameters");
        //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    resp.Content = new StringContent(Resources.ParameteredPostNotSupported);
        //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
        //    return resp;
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] CustomerInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PUT called.\n");
            _log.DebugFormat($"Th={threadId} - PUT called for id: '{id}' with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

            if (info == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }
            Guid guid = Guid.Empty;
            if (info.Guid == null || !Guid.TryParse(info.Guid, out guid))
            {
                HttpResponseMessage verm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                verm.Content = new StringContent(Resources.GuidNotValid);
                _log.Warn($"Th={threadId} - Returning statuscode = {verm.StatusCode}, Content = {verm.Content.ReadAsStringAsync().Result}\n");
                return verm;
            }

            if (this.Request.Headers == null) 
            {
                _log.Warn($"Th={threadId} - Header Exception caught: {Resources.HeadersMissing}\n");
                throw new Exception(Resources.HeadersMissing);
            }

            string MKLHeaderName = ConfigurationManager.AppSettings["MKLTokenHeaderName"];

            // TODO :
            // If call is made from Company Portal they're using MKLCertificate. It is gonna change. but for now use this.
            if (!(this.Request.Headers.Contains(MKLHeaderName)))
            {
                // TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(guid.ToString());
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        _log.Warn($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    erm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    _log.Error($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                    return erm;
                }
            }
            if (info.Source != (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto)
            {
                if (!id.Equals(info.Guid))
                {
                    HttpResponseMessage rm1 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm1.Content = new StringContent(Resources.GuidMismatchBodyAndUrl);
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm1.StatusCode}, Content = {rm1.Content.ReadAsStringAsync().Result}\n");
                    return rm1;
                }
            }

            FormatCustomerInfo(ref info);
            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto:
                    rm = CrmPlusControl.UpdateContact(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.BytEpost:
                    rm = CrmPlusControl.ChangeEmailAddress(threadId, info);
                    break;
                    //case (int)CustomerUtility.Source.LosenAterstallningSkickat:
                    //    rm = CrmPlusControl.NotifyMKLSent(info);
                case (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto:
                    rm = CrmPlusControl.ValidateEmail(threadId, guid, ContactEntity.EntityTypeCode, id, info.MklId);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
        }


        ////INFO: teo - Is not part of HttpRouting in Production-releases
        //public HttpResponseMessage AngeNamnDebugPost([FromBody] CustomerInfo info)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    _log.Info($"Th={threadId} - AngeNamnDebugPost called with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    HttpResponseMessage resp1 = CrmPlusControl.CreateAngeNamn(threadId, info);
        //    _log.Info($"Th={threadId} - Returning statuscode = {resp1.StatusCode}, Content = {resp1.Content.ReadAsStringAsync().Result}\n");
        //    return resp1;

        //}
    }
}
