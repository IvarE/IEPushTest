using Skanetrafiken.Crm.Models;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class MultiQController : WrapperController
    {
        private string _prefix = "MultiQ";
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

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                try
                {
                    // TOKEN VERIFICATION - TO CHECK
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
                    //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    //    return rm;
                    //}

                    HttpResponseMessage resp = CrmPlusControl.GetOrders(threadId, probability, _prefix);

                    return resp;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetOrders", string.Format(Resources.UnexpectedException), _logger);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostDeliveryReport([FromBody] FileInfoMQ fileInfo)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "PostDeliveryReport", "This method is no longer Implemented.It's Deprecated since 14/01/2021", _logger);

                //if (fileInfo == null || fileInfo.OrderId == null || fileInfo.FileName == null)
                //{
                //    HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                //    erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                //    return erm;
                //}

                //// TOKEN VERIFICATION - TO CHECK
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
                //    HttpResponseMessage tokenResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //    tokenResponse.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                //    return tokenResponse;
                //}

                //HttpResponseMessage rm = CrmPlusControl.PostDeliveryReport(threadId, fileInfo);
                //return rm;
            }
        }
    }
}
