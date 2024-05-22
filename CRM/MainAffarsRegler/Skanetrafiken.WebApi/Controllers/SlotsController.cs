using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class SlotsController : WrapperController
    {
        private string _prefix = "Slot";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        [HttpGet]
        public HttpResponseMessage GetExcelBase64(string fromDate, string toDate)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (string.IsNullOrWhiteSpace(fromDate))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetExcelBase64", "Could not find a 'fromDate' parameter in url", _logger);
                }

                if (string.IsNullOrWhiteSpace(toDate))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetExcelBase64", "Could not find a 'toDate' parameter in url", _logger);
                }

                try
                {
                    HttpResponseMessage resp = CrmPlusControl.CreateExcelBase64(threadId, fromDate, toDate, _prefix);

                    return resp;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetExcelBase64", string.Format(Resources.UnexpectedException), _logger);
                }
            }
        }
    }
}
