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
    public class SkaKortController : WrapperController
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

        //Register (Create) SkaKort
        [HttpPost]
        public HttpResponseMessage Post([FromBody] SkaKortInfo skaKortInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(skaKortInfo)}");

            if (skaKortInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
            }

            try
            {
                if (skaKortInfo.InformationSource == 1)
                    skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka; //ändra till skakort hantering (ResKortPrivat - ReskortFöretag) - Då måste det finnas en contactid
                else if (skaKortInfo.InformationSource == 2)
                    skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal; // skickar de med en 2a så måste det finnas en accountID
                else if (skaKortInfo.InformationSource == null)
                    skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal; //Bad request if no information source was sent


                HttpResponseMessage rm = null;
                switch (skaKortInfo.InformationSource)
                {
                    case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                        rm = CrmPlusControl.RegisterCompanySkaKortPost(threadId, skaKortInfo); //Create methods
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                        rm = CrmPlusControl.RegisterBuyAndSendSkaKortPost(threadId, skaKortInfo); //Create methods
                        break;
                    default:
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(string.Format(Resources.InvalidSource, skaKortInfo.InformationSource));
                        break;
                }

                //if(rm.Content != null)
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
                //else _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}");
                return rm;
            }
            catch(Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        //Remove Contact/Account (Update) SkaKort
        [HttpPut]
        public HttpResponseMessage Revoke([FromUri] string id, [FromBody] SkaKortInfo skaKortInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Put called with Payload:\n {CrmPlusControl.SerializeNoNull(skaKortInfo)}");

            if (skaKortInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
            }

            try
            {

                HttpResponseMessage rm = CrmPlusControl.RemoveSkaKortContactOrAccountPut(threadId, skaKortInfo);
                
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

        //Revoke (Delete) SkaKort
        [HttpPut]
        public HttpResponseMessage Disconnect([FromUri] string id, [FromBody] SkaKortInfo skaKortInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.DebugFormat($"Th={threadId} - Disconnect called with Payload:\n {CrmPlusControl.SerializeNoNull(skaKortInfo)}");

            if (skaKortInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
            }

            try
            {

                HttpResponseMessage rm = CrmPlusControl.SkaKortDisconnect(threadId, skaKortInfo);

                _log.DebugFormat($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                return rm;
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

    }
}