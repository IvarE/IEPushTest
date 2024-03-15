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

        [HttpGet]
        public HttpResponseMessage GetExcelBase64(string fromDate, string toDate)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            using (var _logger = new AppInsightsLogger())
            {
                if (string.IsNullOrWhiteSpace(fromDate))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetExcelBase64", "Could not find a 'fromDate' parameter in url", _logger);
                }

                if (string.IsNullOrWhiteSpace(toDate))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetExcelBase64", "Could not find a 'toDate' parameter in url", _logger);
                }

                HttpResponseMessage resp = CrmPlusControl.CreateExcelBase64(threadId, fromDate, toDate, _prefix);

                return resp;
            }
        }
    }
}
