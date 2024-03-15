using Skanetrafiken.Crm.Properties;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

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
        private string _prefix = "SyncData";

        //[HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;

        //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    resp.Content = new StringContent(Resources.GenericGetNotSupported);
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

        //    // TOKEN VERIFICATION - TO CHECK
        //    try
        //    {
        //        HttpResponseMessage tokenResp = TokenValidation();
        //        if (tokenResp.StatusCode != HttpStatusCode.OK)
        //        {
        //            return tokenResp;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //        rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //        return rm;
        //    }

        //    HttpResponseMessage resp = CrmPlusControl.GetOrders(threadId, probability);
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
            using (var _logger = new AppInsightsLogger())
            {


                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrEmpty(syncContact.SocialSecurityNumber) || string.IsNullOrEmpty(syncContact.PortalId) || string.IsNullOrEmpty(syncContact.EmailAddress))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "SyncContact", Resources.IncomingDataCannotBeNull, _logger);
                }

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

                HttpResponseMessage rm = CrmPlusControl.SynchronizeContactData(threadId, syncContact.SocialSecurityNumber, syncContact.PortalId, syncContact.EmailAddress, _prefix);

                return rm;
            }
        }
    }
}
