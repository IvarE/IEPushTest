using Skanetrafiken.Crm.Entities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Skanetrafiken.Crm.Controllers
{
    public class ContactsController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage GetLatestLinkGuid(string email)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - RetrieveLatestLinkGuid called for '{email}'.");
            if (string.IsNullOrWhiteSpace(email))
            {
                HttpResponseMessage returnMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                returnMessage.Content = new StringContent(Resources.ParameterMissingEmail);
                _log.Warn($"Th={threadId} - Returning statuscode = {returnMessage.StatusCode}, Content = {returnMessage.Content.ReadAsStringAsync().Result}\n");
                return returnMessage;
            }

            HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveContactLinkGuid(threadId, email);
            //Return Logg
            if (linkGuidResponse.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}, Content = {linkGuidResponse.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {linkGuidResponse.StatusCode}, Content = {linkGuidResponse.Content.ReadAsStringAsync().Result}\n");
            }

            return linkGuidResponse;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Error($"Th={threadId} - Unsupported generic GET called.");

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
            _log.Error($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET called with parameter: id = {id}");
            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage guidResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                guidResp.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {guidResp.StatusCode}, Content = {guidResp.Content.ReadAsStringAsync().Result}\n");
                return guidResp;
            }

            if (this.Request.Headers == null) 
            {
                _log.Warn($"Th={threadId} - Exception caught: {Resources.HeadersMissing}\n");
                throw new Exception(Resources.HeadersMissing);
            }

            string MKLHeaderName = ConfigurationManager.AppSettings["MKLTokenHeaderName"];

            // TODO :
            // If call is made from Company Portal they're using MKLCertificate. It is gonna change. but for now use this.
            if (!(this.Request.Headers.Contains(MKLHeaderName)))
            {
                // TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(id);
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        _log.Warn($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    _log.Error($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
                    return rm;
                }
            }

            HttpResponseMessage resp = CrmPlusControl.GetContact(threadId, id);

            //Return Logg
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {resp.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            }

            return resp;
        }
        
        /// <summary>
        /// Controller used for create customers of different types
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CustomerInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - POST called.\n");
            _log.DebugFormat($"Th={threadId} - POST called with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

            if (info == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
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

            if (info.ServiceType == 1)
            {
                _log.Info($"Th={threadId} - Setting InformationSource to KöpOchSkicka.\n");
                info.Source = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka;
                if (info != null && info.AddressBlock != null && !String.IsNullOrEmpty(info.AddressBlock.PostalCode))
                {
                    info.AddressBlock.PostalCode = Regex.Replace(info.AddressBlock.PostalCode, @"\s+", "");
                }
            }
            else if (info.ServiceType == 2) 
            {
                _log.Info($"Th={threadId} - Setting InformationSource to ForetagsPortal.\n");
                info.Source = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal;
            }
            
            // Format Customer info
            FormatCustomerInfo(ref info);
            

            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKundArende:
                    rm = CrmPlusControl.NonLoginCustomerIncident(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.RGOL:
                    rm = CrmPlusControl.RGOL(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.PASS:
                    rm = CrmPlusControl.PASS(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.OinloggatKop:
                    rm = CrmPlusControl.NonLoginPurchase(threadId, info); // --- Private Contact
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal: // Business Contact 
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal: // School Contact
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal: // Senior Contact
                    rm = CrmPlusControl.CreatePortalCustomer(threadId, info);
                    break;
                case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                    rm = CrmPlusControl.KopOchSkickaKund(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
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
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PUT called.\n");
            _log.DebugFormat($"Th={threadId} - PUT called for id: '{id}' with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

            if (info == null)
            {
                HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                erm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
                _log.Warn($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                return erm;
            }
            Guid guid = Guid.Empty;
            if (info.Guid == null || !Guid.TryParse(info.Guid, out guid))
            {
                HttpResponseMessage verm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                verm.Content = new StringContent(Resources.GuidNotValid);
                _log.Warn($"Th={threadId} - Returning statuscode = {verm.StatusCode}, Content = {verm.Content.ReadAsStringAsync().Result}\n");
                return verm;
            }

            if (this.Request.Headers == null) 
            {
                _log.Warn($"Th={threadId} - Header Exception caught: {Resources.HeadersMissing}\n");
                throw new Exception(Resources.HeadersMissing);
            }

            string MKLHeaderName = ConfigurationManager.AppSettings["MKLTokenHeaderName"];

            // TODO :
            // If call is made from Company Portal they're using MKLCertificate. It is gonna change. but for now use this.
            if (!(this.Request.Headers.Contains(MKLHeaderName)))
            {
                // TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(guid.ToString());
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        _log.Warn($"Th={threadId} - Returning statuscode = {tokenResp.StatusCode}, Content = {tokenResp.Content.ReadAsStringAsync().Result}\n");
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage erm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    erm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                    _log.Error($"Th={threadId} - Returning statuscode = {erm.StatusCode}, Content = {erm.Content.ReadAsStringAsync().Result}\n");
                    return erm;
                }
            }
            if (info.Source != (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto)
            {
                if (!id.Equals(info.Guid))
                {
                    HttpResponseMessage rm1 = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm1.Content = new StringContent(Resources.GuidMismatchBodyAndUrl);
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm1.StatusCode}, Content = {rm1.Content.ReadAsStringAsync().Result}\n");
                    return rm1;
                }
            }

            FormatCustomerInfo(ref info);
            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.UppdateraMittKonto:
                    rm = CrmPlusControl.UpdateContact(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.BytEpost:
                    rm = CrmPlusControl.ChangeEmailAddress(threadId, info);
                    break;
                    //case (int)CustomerUtility.Source.LosenAterstallningSkickat:
                    //    rm = CrmPlusControl.NotifyMKLSent(info);
                case (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto:
                    rm = CrmPlusControl.ValidateEmail(threadId, guid, ContactEntity.EntityTypeCode, id, info.MklId);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SkolPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal:
                    rm = CrmPlusControl.UpdatePortalCustomer(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
        }


        ////INFO: teo - Is not part of HttpRouting in Production-releases
        //public HttpResponseMessage AngeNamnDebugPost([FromBody] CustomerInfo info)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    _log.Info($"Th={threadId} - AngeNamnDebugPost called with Payload:\n{CrmPlusControl.SerializeNoNull(info)}");

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    HttpResponseMessage resp1 = CrmPlusControl.CreateAngeNamn(threadId, info);
        //    _log.Info($"Th={threadId} - Returning statuscode = {resp1.StatusCode}, Content = {resp1.Content.ReadAsStringAsync().Result}\n");
        //    return resp1;

        //}
    }
}
