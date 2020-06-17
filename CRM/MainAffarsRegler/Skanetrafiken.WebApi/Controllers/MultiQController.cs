using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Skanetrafiken.Crm.Models;

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
        /// Returns a List of Orders full information.
        /// </remarks>
        /// <param name="probability">Input data.</param>
        /// <returns>A complete List of Orders.</returns>
        /// <seealso cref="AccountInfo"/>
        [HttpGet]
        public HttpResponseMessage GetOrders(int probability)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetValidOrders called");

            // TOKEN VERIFICATION - TO CHECK
            //try
            //{
            //    HttpResponseMessage tokenResp = TokenValidation();
            //    if (tokenResp.StatusCode != HttpStatusCode.OK)
            //    {
            //        _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
            //        return tokenResp;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
            //    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            //    return rm;
            //}

            HttpResponseMessage resp = CrmPlusControl.GetOrders(threadId, probability);
            _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostDeliveryReport([FromBody] FileInfoMQ fileInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(fileInfo)}");

            if (fileInfo == null || fileInfo.OrderId == null || fileInfo.FileName == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            // TOKEN VERIFICATION - TO CHECK
            //try
            //{
            //    HttpResponseMessage tokenResp = TokenValidation();
            //    if (tokenResp.StatusCode != HttpStatusCode.OK)
            //    {
            //        _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
            //        return tokenResp;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    tokenResponse.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
            //    _log.Info($"Th={threadId} - Returning statuscode = {tokenResponse.StatusCode}, Content = {tokenResponse.Content.ReadAsStringAsync().Result}\n");
            //    return tokenResponse;
            //}

            HttpResponseMessage rm = CrmPlusControl.PostDeliveryReport(threadId, fileInfo);
            _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            return rm;
        }
    }
}
