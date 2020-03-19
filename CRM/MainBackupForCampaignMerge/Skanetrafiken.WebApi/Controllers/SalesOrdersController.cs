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

namespace Skanetrafiken.Crm.Controllers
{
    public class SalesOrdersController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            _log.Debug("Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] SalesOrderInfo salesOrderInfo)
        {
            _log.DebugFormat("Post called with Payload:\n {0}", CrmPlusControl.SerializeNoNull(salesOrderInfo));

            if (salesOrderInfo == null)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }

            HttpResponseMessage nrm = CrmPlusControl.SalesOrderPost(salesOrderInfo);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", nrm.StatusCode, nrm.Content.ReadAsStringAsync().Result);
            return nrm;
        }


        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] SalesOrderInfo info)
        {
            _log.DebugFormat("Put called with Payload:\n {0}", CrmPlusControl.SerializeNoNull(info));

            if (info == null)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }

            HttpResponseMessage nrm = CrmPlusControl.SalesOrderPut(info);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", nrm.StatusCode, nrm.Content.ReadAsStringAsync().Result);
            return nrm;
        }

    }
}