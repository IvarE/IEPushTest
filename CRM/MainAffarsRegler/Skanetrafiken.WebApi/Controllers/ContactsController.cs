using Skanetrafiken.Crm.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Skanetrafiken.Crm.Controllers
{
    public class ContactsController : WrapperController
    {
        private string _prefix = "Contact";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        [Route("api/Contacts/GetAccessToken/{process}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAccessToken(string process)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            if (string.IsNullOrWhiteSpace(process))
            {
                HttpResponseMessage badReq = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badReq.Content = new StringContent("Could not find value in 'process' parameter. Please provide a valid 'process'.");
                return badReq;
            }

            Plugin.LocalPluginContext localContext = null;

            try
            {
                CrmServiceClient serviceClient = ConnectionCacheManager.GetAvailableConnection(threadId, true);
                // Cast the proxy client to the IOrganizationService interface.
                using (OrganizationServiceProxy serviceProxy = (OrganizationServiceProxy)serviceClient.OrganizationServiceProxy)
                {
                    localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

                    if (localContext.OrganizationService == null)
                        throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

                    var clientId = string.Empty;
                    var clientSecret = string.Empty;
                    var tenentId = string.Empty;
                    var audience = string.Empty;

                    CgiSettingEntity settings = null;
                    FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);


                    if (process == "mkl")
                    {

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
                        clientSecret = settings.st_CrmAppRegistrationClientSecret;
                        tenentId = settings.st_CrmAppRegistrationTenantId;
                        audience = settings.st_crmappmklaudience;
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
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        private static HttpResponseMessage ReturnApiMessage(int threadId, string errorMessage, HttpStatusCode code)
        {

            HttpResponseMessage response = new HttpResponseMessage(code);
            response.Content = new StringContent(errorMessage);
            return response;
        }

        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage GetLatestLinkGuid(string email)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (string.IsNullOrWhiteSpace(email))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetLatestLinkGuid", Resources.ParameterMissingEmail, _logger);
                }

                HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveContactLinkGuid(threadId, email);

                return linkGuidResponse;
            }
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

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
                return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Get", Resources.GenericGetNotSupported, _logger);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrWhiteSpace(id))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                }

                if (this.Request.Headers == null)
                {
                    //log?
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
                            return tokenResp;
                        }
                    }
                    catch (Exception ex)
                    {
                        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));

                        _exceptionCustomProperties["source"] = _prefix;
                        _logger.LogException(ex, _exceptionCustomProperties);

                        return rm;
                    }
                }

                HttpResponseMessage resp = CrmPlusControl.GetContact(threadId, id, _prefix);

                return resp;
            }
        }

        [HttpPost]
        public HttpResponseMessage ClearMKLId(string mklId)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrWhiteSpace(mklId))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "ClearMKLId", Resources.MklIdMissing, _logger);
                }

                return CrmPlusControl.ClearMKLIdContact(threadId, mklId);
            }

        }

        /// <summary>
        /// Controller used for create customers of different types
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CustomerInfo info)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (info == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncomingDataCannotBeNull, _logger);
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
                //        return tokenResp;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                //    return rm;
                //}

                if (info.ServiceType == 1)
                {
                    info.Source = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                    if (info != null && info.AddressBlock != null && !String.IsNullOrEmpty(info.AddressBlock.PostalCode))
                    {
                        info.AddressBlock.PostalCode = Regex.Replace(info.AddressBlock.PostalCode, @"\s+", "");
                    }
                }
                else if (info.ServiceType == 2)
                {
                    info.Source = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
                }

                // Format Customer info
                FormatCustomerInfo(ref info);


                HttpResponseMessage rm = null;
                switch (info.Source)
                {
                    case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKundArende:
                        rm = CrmPlusControl.NonLoginCustomerIncident(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.RGOL:
                        rm = CrmPlusControl.RGOL(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.PASS:
                        rm = CrmPlusControl.PASS(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKop:
                        rm = CrmPlusControl.NonLoginPurchase(threadId, info, _prefix); // --- Private Contact
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal: // Business Contact 
                        rm = CrmPlusControl.CreatePortalCustomer(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal: // School Contact
                        rm = CrmPlusControl.CreatePortalCustomer(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal: // Senior Contact
                        rm = CrmPlusControl.CreatePortalCustomer(threadId, info, _prefix);
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                        rm = CrmPlusControl.KopOchSkickaKund(threadId, info, _prefix);
                        break;
                    default:
                        rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", string.Format(Resources.InvalidSource, info.Source), _logger);
                        break;
                }

                return rm;
            }
        }

        //[HttpPost]
        //public HttpResponseMessage Post2Param([FromUri] string time, [FromUri] int price, [FromBody] CustomerInfo info)
        //{
        //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    resp.Content = new StringContent(Resources.ParameteredPostNotSupported);
        //    return resp;
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] CustomerInfo info)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (info == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.IncomingDataCannotBeNull, _logger);
                }
                Guid guid = Guid.Empty;
                if (info.Guid == null || !Guid.TryParse(info.Guid, out guid))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.GuidNotValid, _logger);
                }

                if (this.Request.Headers == null)
                {
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
                            return tokenResp;
                        }
                    }
                    catch (Exception ex)
                    {
                        HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        erm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));

                        _exceptionCustomProperties["source"] = _prefix;
                        _logger.LogException(ex, _exceptionCustomProperties);

                        return erm;
                    }
                }
                if (info.Source != (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto)
                {
                    if (!id.Equals(info.Guid))
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.GuidMismatchBodyAndUrl, _logger);
                    }
                }

                FormatCustomerInfo(ref info);
                HttpResponseMessage rm = null;
                switch (info.Source)
                {
                    case (int)Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto:
                        rm = CrmPlusControl.UpdateContact(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.BytEpost:
                        rm = CrmPlusControl.ChangeEmailAddress(threadId, info, _prefix);
                        break;
                    //case (int)CustomerUtility.Source.LosenAterstallningSkickat:
                    //    rm = CrmPlusControl.NotifyMKLSent(info);
                    case (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto:
                        rm = CrmPlusControl.ValidateEmail(threadId, guid, ContactEntity.EntityTypeCode, id, info.MklId, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                        rm = CrmPlusControl.UpdatePortalCustomer(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal:
                        rm = CrmPlusControl.UpdatePortalCustomer(threadId, info, _prefix);
                        break;
                    case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal:
                        rm = CrmPlusControl.UpdatePortalCustomer(threadId, info, _prefix);
                        break;
                    default:
                        rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", string.Format(Resources.InvalidSource,info.Source), _logger);
                        break;
                }

                return rm;
            }
        }


        ////INFO: teo - Is not part of HttpRouting in Production-releases
        //public HttpResponseMessage AngeNamnDebugPost([FromBody] CustomerInfo info)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    HttpResponseMessage resp1 = CrmPlusControl.CreateAngeNamn(threadId, info);
        //    return resp1;

        //}
    }
}
