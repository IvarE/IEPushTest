using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class PingController : WrapperController
    {
        private string _prefix = "Ping";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        [HttpGet]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.OK);
            return nrm;
        }
#if !PRODUKTION
        
        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                try
                {
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                    }

                    // TOKEN VERIFICATION

                    HttpResponseMessage tokenResp = TokenValidation(id);
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        return tokenResp;
                    }

                    //return new HttpResponseMessage(HttpStatusCode.OK);

                    HttpResponseMessage resp = CrmPlusControl.GetContactTroubleshooting(threadId, id, _prefix);

                    return resp;

                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetWithId", string.Format(Resources.UnexpectedException), _logger);
                }
            }
        }
#endif
    }
}
