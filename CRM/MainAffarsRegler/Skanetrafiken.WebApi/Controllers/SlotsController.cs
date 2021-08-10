using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class SlotsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage GetExcelBase64(string fromDate, string toDate)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            _log.Info($"Th={threadId} - GetExcelBase64 called with FromDate: {fromDate} and ToDate: {toDate}");
            if (string.IsNullOrWhiteSpace(fromDate))
            {
                HttpResponseMessage fromResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                fromResp.Content = new StringContent("Could not find a 'fromDate' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {fromResp.StatusCode}, Content = {fromResp.Content.ReadAsStringAsync().Result}\n");
                return fromResp;
            }

            if (string.IsNullOrWhiteSpace(toDate))
            {
                HttpResponseMessage toResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                toResp.Content = new StringContent("Could not find a 'fromDate' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {toResp.StatusCode}, Content = {toResp.Content.ReadAsStringAsync().Result}\n");
                return toResp;
            }

            HttpResponseMessage resp = CrmPlusControl.CreateExcelBase64(threadId, fromDate, toDate);
            _log.Warn($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");

            return resp;
        }
    }
}
