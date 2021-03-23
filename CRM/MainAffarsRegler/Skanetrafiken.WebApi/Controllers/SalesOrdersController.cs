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
            _log.Error($"Th={threadId} - Unsupported generic GET called.");
            
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.Error($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}");
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] SalesOrderInfo salesOrderInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - Post called.\n");
            _log.DebugFormat($"Th={threadId} - Post called with Payload:\n {CrmPlusControl.SerializeNoNull(salesOrderInfo)}");

            if (salesOrderInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
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

            try
            {
                if (salesOrderInfo.InformationSource == 1)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                else if (salesOrderInfo.InformationSource == 2)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
                else if (salesOrderInfo.InformationSource == 3)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkickaFTG;
                else if (salesOrderInfo.InformationSource == 4)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.Tillagg;
                else if (salesOrderInfo.InformationSource == null)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;


                HttpResponseMessage rm = null;
                switch (salesOrderInfo.InformationSource)
                {
                    case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                        rm = CrmPlusControl.CompanySalesOrderPost(threadId, salesOrderInfo);
                        break;
                    case (int)Schema.Generated.ed_informationsource.Tillagg:
                        rm = CrmPlusControl.CompanySalesOrderPost(threadId, salesOrderInfo);
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                        rm = CrmPlusControl.KopOchSkickaSalesOrderPost(threadId, salesOrderInfo, false);
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkickaFTG:
                        rm = CrmPlusControl.KopOchSkickaSalesOrderPost(threadId, salesOrderInfo, true);
                        break;
                    default:
                        //rm = CrmPlusControl.SalesOrderPost(threadId, salesOrderInfo);
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource));
                        break;
                }
                //if(rm.Content != null)
                //_log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
                //else _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}");

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
            catch(Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }


        [HttpPut]
        public HttpResponseMessage Put([FromBody] SalesOrderInfo salesOrderInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PUT called.\n");
            _log.DebugFormat($"Th={threadId} - PUT called with Payload:\n {CrmPlusControl.SerializeNoNull(salesOrderInfo)}");

            if (salesOrderInfo == null)
            {
                HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                nrm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {nrm.StatusCode}, Content = {nrm.Content.ReadAsStringAsync().Result}\n");
                return nrm;
            }

            //      Mkl = 0,
            //      BuynSend = 1,
            //      Företag = 2,
            //      Service = 3,
            //      Skola = 4,
            //      Senior = 5

            try
            {
                if (salesOrderInfo.InformationSource == 1)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                else if (salesOrderInfo.InformationSource == 2)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
                else if (salesOrderInfo.InformationSource == 3)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkickaFTG;
                else if (salesOrderInfo.InformationSource == null)
                    salesOrderInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;

                HttpResponseMessage rm;
                switch (salesOrderInfo.InformationSource)
                {
                    case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                        rm = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                        rm.Content = new StringContent(Resources.InvalidSource);
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                        rm = CrmPlusControl.KopOchSkickaSalesOrderPut(threadId, salesOrderInfo, false);
                        break;
                    case (int)Schema.Generated.ed_informationsource.KopOchSkickaFTG:
                        rm = CrmPlusControl.KopOchSkickaSalesOrderPut(threadId, salesOrderInfo, true);
                        break;
                    default:
                        //rm = CrmPlusControl.SalesOrderPut(threadId, salesOrderInfo);
                        rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        rm.Content = new StringContent(string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource));
                        break;
                }

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
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content?.ReadAsStringAsync()?.Result}\n");
                return rm;
            }
            finally
            {
                ConnectionCacheManager.ReleaseConnection(threadId);
            }
        }

    }
}