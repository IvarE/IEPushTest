using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class MultiQController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        /// <summary>
        /// Retrieves List of Orders.
        /// </summary>
        /// <remarks>
        /// Returns an Account-object with full information and all the associated complex types.
        /// </remarks>
        /// <param name="id">Input data to be exactly matched with an organization number.</param>
        /// <returns>A complete Member object.</returns>
        /// <seealso cref="AccountInfo"/>
        [HttpGet]
        public HttpResponseMessage GetOrders()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetValidOrders called");

            // TOKEN VERIFICATION - TO CHECK
            try
            {
                HttpResponseMessage tokenResp = TokenValidation();
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
            }

            HttpResponseMessage resp = CrmPlusControl.GetOrders(threadId);
            _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage PostDeliveryReport([FromBody] string fileBase64)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(fileBase64)}");

            if (fileBase64 == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            // TOKEN VERIFICATION - TO CHECK
            try
            {
                HttpResponseMessage tokenResp = TokenValidation();
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                tokenResponse.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Info($"Th={threadId} - Returning statuscode = {tokenResponse.StatusCode}, Content = {tokenResponse.Content.ReadAsStringAsync().Result}\n");
                return tokenResponse;
            }

            HttpResponseMessage rm = CrmPlusControl.PostDeliveryReport(threadId, fileBase64);
            _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            return rm;
        }
    }
}
