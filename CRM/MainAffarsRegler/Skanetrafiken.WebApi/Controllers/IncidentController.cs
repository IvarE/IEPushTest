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
    public class IncidentController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET IncidentController called.\n");
            return CrmPlusControl.PingConnection(threadId);
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentFromAzure(string encryptedUrl)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Get called.\n");
            _log.DebugFormat($"Th={threadId} - Get called with Payload:\n {encryptedUrl}");

            if (encryptedUrl == string.Empty)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            HttpResponseMessage rm = null;
            //Decrypt the string

            //var test = IncidentEntity.HandleDecryptAttachment(localContext, encryptedUrl, null);

            //Return Logg
            //if (rm.StatusCode != HttpStatusCode.OK)
            //{
            //    _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            //}
            //else
            //{
            //    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
            //    _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            //}

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
