using Skanetrafiken.Crm.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Query;
using System.Threading;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm.Controllers
{
    public class AccountsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Error($"Th={threadId} - Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.Error($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        /// <summary>
        /// Retrieves all information about an organization.
        /// </summary>
        /// <remarks>
        /// Returns an Account-object with full information and all the associated complex types.
        /// </remarks>
        /// <param name="id">Input data to be exactly matched with an organization number.</param>
        /// <returns>A complete Member object.</returns>
        /// <seealso cref="AccountInfo"/>
        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetOrganization called with: {id}");
            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage guidResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                guidResp.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {guidResp.StatusCode}, Content = {guidResp.Content.ReadAsStringAsync().Result}\n");
                return guidResp;
            }

            /*
            // TOKEN VERIFICATION
            try
            {
                HttpResponseMessage tokenResp = TokenValidation(id);
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }*/

            HttpResponseMessage resp = CrmPlusControl.GetAccount(threadId, id);
            //Return Logg
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }
            else {
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }
            
            return resp;
                        
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AccountInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Post called.\n");
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(info)}");

            if (info == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }
            
            HttpResponseMessage rm = null;
            switch (info.InformationSource)
            {
                case (int)Generated.ed_informationsource.ForetagsPortal:
                    rm = CrmPlusControl.AccountPost(threadId, info);
                    break;
                case (int)Generated.ed_informationsource.SkolPortal:
                    rm = CrmPlusControl.AccountPost(threadId, info);
                    break;
                case (int)Generated.ed_informationsource.SeniorPortal:
                    rm = CrmPlusControl.AccountPost(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.InformationSource));
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

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] AccountInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Put called.\n");
            _log.DebugFormat($"Th={threadId} - Put called with Payload:\n {CrmPlusControl.SerializeNoNull(info)}");

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

            /*
            // TOKEN VERIFICATION
            try
            {
                HttpResponseMessage tokenResp = TokenValidation(guid.ToString());
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                erm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            } */

            if (!id.Equals(info.Guid))
            {
                HttpResponseMessage rm1 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm1.Content = new StringContent(Resources.GuidMismatchBodyAndUrl);
                _log.Warn($"Th={threadId} - Returning statuscode = {rm1.StatusCode}, Content = {rm1.Content.ReadAsStringAsync().Result}\n");
                return rm1;
            }
            
            HttpResponseMessage rm = null;
            switch (info.InformationSource)
            {
                case (int)Generated.ed_informationsource.ForetagsPortal:
                    rm = CrmPlusControl.AccountPut(threadId, info);
                    break;
                case (int)Generated.ed_informationsource.SkolPortal:
                    rm = CrmPlusControl.AccountPut(threadId, info);
                    break;
                case (int)Generated.ed_informationsource.SeniorPortal:
                    rm = CrmPlusControl.AccountPut(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.InformationSource));
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
        
        [NonAction]
        private CrmServiceClient GetCrmConnection()
        {
            string connectionString = CrmConnection.GetCrmConnectionString(CredentialFilePath);
            //  Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(connectionString);
            return conn;
        }

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
    }
}
