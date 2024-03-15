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
                try
                {
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                    }


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

                    //return new HttpResponseMessage(HttpStatusCode.OK);

                    HttpResponseMessage resp = CrmPlusControl.GetContactTroubleshooting(threadId, id, _prefix);

                    return resp;

                }
                catch (Exception e)
                {
                    HttpResponseMessage errResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    errResp.Content = new StringContent(e.Message);

                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    return errResp;
                }
            }
        }
#endif
    }
}
