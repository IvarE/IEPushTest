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
using System.Collections.Generic;

namespace Skanetrafiken.Crm.Controllers
{
    public class AccountsController : WrapperController
    {
        private string _prefix = "Account";

        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var _logger = new AppInsightsLogger())
            {
                return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Get", Resources.GenericGetNotSupported, _logger);
            }
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
            using (var _logger = new AppInsightsLogger())
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                }

                int threadId = Thread.CurrentThread.ManagedThreadId;

                /*
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
                    return rm;
                }*/

                HttpResponseMessage resp = CrmPlusControl.GetAccount(threadId, id, _prefix);

                return resp;

            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] AccountInfo info)
        {
            using (var _logger = new AppInsightsLogger())
            {
                if (info == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncomingDataCannotBeNull, _logger);
                }

                int threadId = Thread.CurrentThread.ManagedThreadId;

                HttpResponseMessage rm = null;
                switch (info.InformationSource)
                {
                    case (int)Generated.ed_informationsource.ForetagsPortal:
                        rm = CrmPlusControl.AccountPost(threadId, info, _prefix);
                        break;
                    case (int)Generated.ed_informationsource.SkolPortal:
                        rm = CrmPlusControl.AccountPost(threadId, info, _prefix);
                        break;
                    case (int)Generated.ed_informationsource.SeniorPortal:
                        rm = CrmPlusControl.AccountPost(threadId, info, _prefix);
                        break;
                    default:
                        rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", string.Format(Resources.InvalidSource, info.InformationSource), _logger);
                        break;
                }

                return rm;
            }
        }

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] AccountInfo info)
        {
            using (var _logger = new AppInsightsLogger())
            {
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

                /*
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
                    return erm;
                } */

                if (!id.Equals(info.Guid))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.GuidMismatchBodyAndUrl, _logger);
                }

                HttpResponseMessage rm = null;
                switch (info.InformationSource)
                {
                    case (int)Generated.ed_informationsource.ForetagsPortal:
                        rm = CrmPlusControl.AccountPut(threadId, info, _prefix);
                        break;
                    case (int)Generated.ed_informationsource.SkolPortal:
                        rm = CrmPlusControl.AccountPut(threadId, info, _prefix);
                        break;
                    case (int)Generated.ed_informationsource.SeniorPortal:
                        rm = CrmPlusControl.AccountPut(threadId, info, _prefix);
                        break;
                    default:
                        rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", string.Format(Resources.InvalidSource, info.InformationSource), _logger);
                        break;
                }

                return rm;
            }
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
