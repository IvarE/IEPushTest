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
    public class SyncContact
    {
        public string SocialSecurityNumber { get; set; }
        public string PortalId { get; set; }
        public string EmailAddress { get; set; }
    }

    public class SyncDataController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    _log.Info($"Th={threadId} - Unsupported generic GET called.");

        //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    resp.Content = new StringContent(Resources.GenericGetNotSupported);
        //    _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
        //    return resp;
        //}

        ///// <summary>
        ///// Retrieves List of Orders.
        ///// </summary>
        ///// <remarks>
        ///// Returns a List of Orders full information.
        ///// </remarks>
        ///// <param name="probability">Input data.</param>
        ///// <returns>A complete List of Orders.</returns>
        ///// <seealso cref="AccountInfo"/>
        //[HttpGet]
        //public HttpResponseMessage GetOrders(int probability)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    _log.Info($"Th={threadId} - GetValidOrders called");

        //    // TOKEN VERIFICATION - TO CHECK
        //    try
        //    {
        //        HttpResponseMessage tokenResp = TokenValidation();
        //        if (tokenResp.StatusCode != HttpStatusCode.OK)
        //        {
        //            _log.Info($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
        //            return tokenResp;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //        rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //        _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
        //        return rm;
        //    }

        //    HttpResponseMessage resp = CrmPlusControl.GetOrders(threadId, probability);
        //    _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
        //    return resp;
        //}

        
        /// <summary>
        /// Synchronizes Contacts between Webb and SeKund. Returns the correct Guid (ContactId) and updates email address.
        /// </summary>
        /// <param name="syncContact"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SyncContact([FromBody] SyncContact syncContact)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - POST called.\n");
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(syncContact)}");

            if (string.IsNullOrEmpty(syncContact.SocialSecurityNumber) || string.IsNullOrEmpty(syncContact.PortalId) || string.IsNullOrEmpty(syncContact.EmailAddress))
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }

            //// TOKEN VERIFICATION - TO CHECK
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

            HttpResponseMessage rm = CrmPlusControl.SynchronizeContactData(threadId, syncContact.SocialSecurityNumber, syncContact.PortalId, syncContact.EmailAddress);

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
            }

            return rm;
        }
    }
}
