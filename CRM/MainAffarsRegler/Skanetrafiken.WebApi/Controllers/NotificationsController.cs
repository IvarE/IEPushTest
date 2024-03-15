using Skanetrafiken.Crm.Properties;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class NotificationsController : WrapperController
    {
        private string _prefix = "Notification";

        //[HttpPost]
        //public HttpResponseMessage PostDepr([FromBody] NotificationInfo[] notificationInfo)
        //{

        //    if (notificationInfo == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        return rm;
        //    }
        //    Guid guid = Guid.Empty;
        //    if (notificationInfo.Guid == null || !Guid.TryParse(notificationInfo.Guid, out guid))
        //    {
        //        HttpResponseMessage verm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        verm.Content = new StringContent(Resources.GuidNotValid);
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
        //    return nrm;
        //}

        [HttpPost]
        public HttpResponseMessage Post([FromBody] NotificationInfo[] notificationInfos)
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (notificationInfos == null || notificationInfos.Length == 0)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncomingDataCannotBeNull, _logger);
                }

                //var nrm2 = NotifyMKLsSentAsync(threadId, notificationInfos).ConfigureAwait(false);

                HttpResponseMessage nrm = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos, _prefix);
                //HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.Accepted);

                return nrm;
            }

        }

        async Task<HttpResponseMessage> NotifyMKLsSentAsync(int threadId, NotificationInfo[] notificationInfos)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            await Task.Run(() =>
            {
                response = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos, _prefix);
            });
            return response;

            //var response = CrmPlusControl.NotifyMKLsSent(threadId, notificationInfos);
            //return response;
        }
    }
}
