using log4net.Repository.Hierarchy;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class SalesOrdersController : WrapperController
    {
        private string _prefix = "SalesOrder";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Get", Resources.GenericGetNotSupported, _logger);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] SalesOrderInfo salesOrderInfo)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (salesOrderInfo == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncomingDataCannotBeNull, _logger);
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
                //        return tokenResp;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));s
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
                            rm = CrmPlusControl.CompanySalesOrderPost(threadId, salesOrderInfo, _prefix);
                            break;
                        case (int)Schema.Generated.ed_informationsource.Tillagg:
                            rm = CrmPlusControl.CompanySalesOrderPost(threadId, salesOrderInfo, _prefix);
                            break;
                        case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                            rm = CrmPlusControl.KopOchSkickaSalesOrderPost(threadId, salesOrderInfo, false, _prefix);
                            break;
                        case (int)Schema.Generated.ed_informationsource.KopOchSkickaFTG:
                            rm = CrmPlusControl.KopOchSkickaSalesOrderPost(threadId, salesOrderInfo, true, _prefix);
                            break;
                        default:
                            //rm = CrmPlusControl.SalesOrderPost(threadId, salesOrderInfo);
                            rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource), _logger);
                            break;
                    }

                    return rm;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "Post", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }


        [HttpPut]
        public HttpResponseMessage Put([FromBody] SalesOrderInfo salesOrderInfo)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (salesOrderInfo == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.IncomingDataCannotBeNull, _logger);
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
                            rm = CrmPlusControl.KopOchSkickaSalesOrderPut(threadId, salesOrderInfo, false, _prefix);
                            break;
                        case (int)Schema.Generated.ed_informationsource.KopOchSkickaFTG:
                            rm = CrmPlusControl.KopOchSkickaSalesOrderPut(threadId, salesOrderInfo, true, _prefix);
                            break;
                        default:
                            //rm = CrmPlusControl.SalesOrderPut(threadId, salesOrderInfo);
                            rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", string.Format(Resources.InvalidSource, salesOrderInfo.InformationSource), _logger);
                            break;
                    }

                    return rm;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "Put", string.Format(Resources.UnexpectedException), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

    }
}