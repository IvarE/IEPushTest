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
using System.Threading.Tasks;

namespace Skanetrafiken.Crm.Controllers
{
    public class NotificationsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //[HttpPost]
        //public HttpResponseMessage PostDepr([FromBody] NotificationInfo[] notificationInfo)
        //{
        //    _log.DebugFormat("POST called with Payload:\n {0}", CrmPlusControl.SerializeNoNull(notificationInfo));

        //    if (notificationInfo == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }
        //    Guid guid = Guid.Empty;
        //    if (notificationInfo.Guid == null || !Guid.TryParse(notificationInfo.Guid, out guid))
        //    {
        //        HttpResponseMessage verm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        verm.Content = new StringContent(Resources.GuidNotValid);
        //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", verm.StatusCode, verm.Content.ReadAsStringAsync().Result);
        //        return verm;
        //    }

        //    //// *-* INFO: *-*
        //    // Token verification inactivated for calls that didn't require Guid.
        //    //// *-* /INFO *-*
        //    //// TOKEN VERIFICATION
        //    //try
        //    //{
        //    //    HttpResponseMessage tokenResp = TokenValidation(/*guid.ToString()*/);
        //    //    if (tokenResp.StatusCode != HttpStatusCode.OK)
        //    //        return tokenResp;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //    //    return rm;
        //    //}

        //    HttpResponseMessage nrm = CrmPlusControl.NotifyMKLSent(notificationInfo);
        //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", nrm.StatusCode, nrm.Content.ReadAsStringAsync().Result);
        //    return nrm;
        //}

        [HttpPost]
        public HttpResponseMessage Post([FromBody] NotificationInfo[] notificationInfos)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PostArray called.\n");
            _log.Debug($"Th={threadId} - PostArray called with Payload:\n {CrmPlusControl.SerializeNoNull(notificationInfos)}");

            if (notificationInfos == null || notificationInfos.Length == 0)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }

            //var nrm2 = NotifyMKLsSentAsync(threadId, notificationInfos).ConfigureAwait(false);

            HttpResponseMessage nrm = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos);
            //HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.Accepted);

            //Return Logg
            if (nrm.StatusCode != HttpStatusCode.OK)
            {
                if (nrm.StatusCode == HttpStatusCode.Ambiguous || nrm.StatusCode == HttpStatusCode.MultipleChoices)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {nrm.StatusCode}.\n");
                    _log.Debug($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                }
                else 
                {
                    _log.Warn($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                }
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {nrm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
            }

            return nrm;

        }

        async Task<HttpResponseMessage> NotifyMKLsSentAsync(int threadId, NotificationInfo[] notificationInfos)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            await Task.Run(() =>
            {
                response = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos);
            });
            return response;

            //var response = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos);
            //return response;
        }
    }
}
