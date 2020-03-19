using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Configuration;
using System.Threading;

namespace Skanetrafiken.Crm.Controllers
{
    public class SalesOrdersController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Debug($"Th={threadId} - Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.DebugFormat($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}");
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] SalesOrderInfo salesOrderInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(salesOrderInfo)}");

            if (salesOrderInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
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
            //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
            //        return tokenResp;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
            //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
            //    return rm;
            //}

            //FormatSalesOrderInfo(ref salesOrderInfo);

            HttpResponseMessage rm = null;
            switch (salesOrderInfo.InformationSource)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    rm = CrmPlusControl.CompanySalesOrderPost(threadId, salesOrderInfo);
                    break;
                default:
                    //rm = CrmPlusControl.SalesOrderPost(threadId, salesOrderInfo);
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource));
                    break;
            }
            _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            return rm;
        }


        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] SalesOrderInfo salesOrderInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Put called with Payload:\n {CrmPlusControl.SerializeNoNull(salesOrderInfo)}");

            if (salesOrderInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
            }

            HttpResponseMessage rm;
            switch (salesOrderInfo.InformationSource)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    rm = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                    rm.Content = new StringContent(Resources.InvalidSource);
                    break;
                default:
                    //rm = CrmPlusControl.SalesOrderPut(threadId, salesOrderInfo);
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource));
                    break;
            }
            _log.DebugFormat($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            return rm;
        }

    }
}