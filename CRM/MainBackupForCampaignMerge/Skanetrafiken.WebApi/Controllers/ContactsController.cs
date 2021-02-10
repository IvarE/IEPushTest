using Skanetrafiken.Crm.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm.Controllers
{
    public class ContactsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage GetLatestLinkGuid(string email)
        {
            _log.Debug($"RetrieveLatestLinkGuid called for '{email}'.");
            if (string.IsNullOrWhiteSpace(email))
            {
                HttpResponseMessage returnMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                returnMessage.Content = new StringContent(Resources.ParameterMissingEmail);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", returnMessage.StatusCode, returnMessage.Content.ReadAsStringAsync().Result);
                return returnMessage;
            }

            HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveContactLinkGuid(email);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", linkGuidResponse.StatusCode, linkGuidResponse.Content.ReadAsStringAsync().Result);
            return linkGuidResponse;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            _log.Debug("Unsupported generic GET called.");
            
            //System.Diagnostics.Stopwatch counter = new System.Diagnostics.Stopwatch();
            //counter.Restart();
            //string mess = "";

            //for (int i = 0; i < 15; i++)
            //{
            //    string messAdd = CrmPlusControl.util();
            //    mess = string.Format("{0}{1}", mess, messAdd);
            //    mess = string.Format("{0}{1}", mess, string.Format("\nUtilCount = {0}, counter = {1}\n", i, counter.ElapsedMilliseconds));
            //}

            //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            //resp.Content = new StringContent(mess);
            //return resp;

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            _log.DebugFormat("GET called with parameter: id = {0}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage guidResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                guidResp.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", guidResp.StatusCode, guidResp.Content.ReadAsStringAsync().Result);
                return guidResp;
            }

            // TOKEN VERIFICATION
            try
            {
                HttpResponseMessage tokenResp = TokenValidation(id);
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }

            HttpResponseMessage resp = CrmPlusControl.GetContact(id);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
            return resp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CustomerInfo info)
        {
            _log.DebugFormat("POST called with Payload:\n{0}", CrmPlusControl.SerializeNoNull(info));
            
            if (info == null)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
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

            FormatCustomerInfo(ref info);

            switch (info.Source)
            {
                case (int)CustomerUtility.Source.OinloggatKundArende:
                    HttpResponseMessage resp1 = CrmPlusControl.NonLoginCustomerIncident(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
                    return resp1;
                case (int)CustomerUtility.Source.RGOL:
                    HttpResponseMessage resp2 = CrmPlusControl.RGOL(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp2.StatusCode, resp2.Content.ReadAsStringAsync().Result);
                    return resp2;
                case (int)CustomerUtility.Source.PASS:
                    HttpResponseMessage resp3 = CrmPlusControl.PASS(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp3.StatusCode, resp3.Content.ReadAsStringAsync().Result);
                    return resp3;
                case (int)CustomerUtility.Source.OinloggatKop:
                    HttpResponseMessage resp4 = CrmPlusControl.NonLoginPurchase(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}", resp4.StatusCode, resp4.Content.ReadAsStringAsync().Result);
                    return resp4;
                default:
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                    return rm;
            }
        }

        //[HttpPost]
        //public HttpResponseMessage Post2Param([FromUri] string time, [FromUri] int price, [FromBody] CustomerInfo info)
        //{
        //    _log.DebugFormat("Post called with 2 parameters");
        //    HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    resp.Content = new StringContent(Resources.ParameteredPostNotSupported);
        //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
        //    return resp;
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] CustomerInfo info)
        {
            _log.DebugFormat("PUT called for id: '{0}' with Payload:\n{1}", id, CrmPlusControl.SerializeNoNull(info));
            
            if (info == null)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }
            Guid guid = Guid.Empty;
            if (info.Guid == null || !Guid.TryParse(info.Guid, out guid))
            {
                HttpResponseMessage verm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                verm.Content = new StringContent(Resources.GuidNotValid);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", verm.StatusCode, verm.Content.ReadAsStringAsync().Result);
                return verm;
            }

            // TOKEN VERIFICATION
            try
            {
                HttpResponseMessage tokenResp = TokenValidation(guid.ToString());
                if (tokenResp.StatusCode != HttpStatusCode.OK)
                {
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
                    return tokenResp;
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }
            if (info.Source != (int)CustomerUtility.Source.ValideraEpost)
            {
                if (!id.Equals(info.Guid))
                {
                    HttpResponseMessage rm1 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm1.Content = new StringContent(Resources.GuidMismatchBodyAndUrl);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm1.StatusCode, rm1.Content.ReadAsStringAsync().Result);
                    return rm1;
                }
            }

            FormatCustomerInfo(ref info);

            switch (info.Source)
            {
                case (int)CustomerUtility.Source.UppdateraMittKonto:
                    HttpResponseMessage resp1 = CrmPlusControl.UpdateContact(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
                    return resp1;
                case (int)CustomerUtility.Source.BytEpost:
                    HttpResponseMessage resp2 = CrmPlusControl.ChangeEmailAddress(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp2.StatusCode, resp2.Content.ReadAsStringAsync().Result);
                    return resp2;
                //case (int)CustomerUtility.Source.LosenAterstallningSkickat:
                //    HttpResponseMessage resp3 = CrmPlusControl.NotifyMKLSent(info);
                //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp3.StatusCode, resp3.Content.ReadAsStringAsync().Result);
                //    return resp3;
                case (int)CustomerUtility.Source.ValideraEpost:
                    HttpResponseMessage resp4 = CrmPlusControl.ValidateEmail(guid, ContactEntity.EntityTypeCode, id, info.MklId);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp4.StatusCode, resp4.Content.ReadAsStringAsync().Result);
                    return resp4;
                default:
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                    return rm;
            }
        }
        
        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage AngeNamnDebugPost([FromBody] CustomerInfo info)
        {
            _log.DebugFormat("AngeNamnDebugPost called with Payload:\n{0}", CrmPlusControl.SerializeNoNull(info));

            if (info == null)
            {
                HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                return rm;
            }

            FormatCustomerInfo(ref info);

            HttpResponseMessage resp1 = CrmPlusControl.CreateAngeNamn(info);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
            return resp1;

        }
    }
}
