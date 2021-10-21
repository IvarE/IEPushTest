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
    public class LeadsController : WrapperController
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

            HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveLeadLinkGuid(threadId, email);
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
        public HttpResponseMessage GetLeadInfo(string campaignCode)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GetLeadInfo called for '{campaignCode}'.");
            if (string.IsNullOrWhiteSpace(campaignCode))
            {
                HttpResponseMessage returnMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                returnMessage.Content = new StringContent(Resources.ParameterMissingCampaignCode);
                _log.Warn($"Th={threadId} - Returning statuscode = {returnMessage.StatusCode}, Content = {returnMessage.Content.ReadAsStringAsync().Result}\n");
                return returnMessage;
            }

            HttpResponseMessage leadInformationResponse = CrmPlusControlCampaign.RetrieveLeadInfo(threadId, campaignCode);
            //Return Logg
            if (leadInformationResponse.StatusCode != HttpStatusCode.OK)
            {
                _log.Warn($"Th={threadId} - Returning statuscode = {leadInformationResponse.StatusCode}, Content = {leadInformationResponse.Content.ReadAsStringAsync().Result}\n");
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {leadInformationResponse.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {leadInformationResponse.StatusCode}, Content = {leadInformationResponse.Content.ReadAsStringAsync().Result}\n");
            }

            return leadInformationResponse;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Error($"Th={threadId} - Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.Error($"Th={threadId} - Returning statuscode = {resp.StatusCode}, Content = {resp.Content.ReadAsStringAsync().Result}\n");
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - GET called with parameter: {id}");

            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResponseMessage guidResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                guidResp.Content = new StringContent("Could not find an 'id' parameter in url");
                _log.Warn($"Th={threadId} - Returning statuscode = {guidResp.StatusCode}, Content = {guidResp.Content.ReadAsStringAsync().Result}\n");
                return guidResp;
            }
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


            HttpResponseMessage resp = CrmPlusControl.GetLead(threadId, id);

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

        //[HttpPost]
        //public HttpResponseMessage LeadInfoPost([FromBody] LeadInfo leadInfo)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;

        //    if (leadInfo == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }


        //    HttpResponseMessage resp1 = CrmPlusControlCampaign.PostLeadAndQualifyToContact(leadInfo);
        //    _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
        //    return resp1;

        //}



        [HttpPost]
        public HttpResponseMessage Post([FromBody] LeadInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - POST called with Payload:\n");
            _log.DebugFormat($"Th={threadId} - POST called with Payload:\n {CrmPlusControl.SerializeNoNull(info)}");

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

            FormatLeadInfo(ref info);
            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.SkapaMittKonto:
                    rm = CrmPlusControl.CreateCustomerLead(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.Kampanj:
                    rm = CrmPlusControlCampaign.PostLeadAndQualifyToContact(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.OinloggatLaddaKort:
                    rm = CrmPlusControl.NonLoginRefill(threadId, info);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                //_log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");

                if (rm.StatusCode == HttpStatusCode.Created || rm.StatusCode == HttpStatusCode.Accepted)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}).\n");
                }
                else {
                    // Controller loggingn too much information when status returns "Created"
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}), Content = {rm.Content.ReadAsStringAsync().Result}\n");
                }
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode}.\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode}, Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
        }

        //[HttpPost]
        //public HttpResponseMessage PostParam([FromBody] CustomerInfo info)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;
        //    _log.Info($"Th={threadId} - POST called with parameters: time = {0}, price = {1}\nPayload:\n {2}", time, price, CrmPlusControl.SerializeNoNull(info));

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }

        //    //// *-* INFO: *-*

        //    // Token verification inactivated for calls that didn't require Guid.

        //    //// *-* /INFO *-*

        //    //// TOKEN VERIFICATION
        //    //try
        //    //{
        //    //    HttpResponseMessage tokenResp = TokenValidation();
        //    //    if (tokenResp.StatusCode != HttpStatusCode.OK)
        //    //    {
        //    //        _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
        //    //        return tokenResp;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //    //    _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //    //    return rm;
        //    //}

        //    DateTime dt = new DateTime();
        //    if (!DateTime.TryParse(time, out dt))
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(string.Format(Resources.DateTimeWrongFormat, time));
        //        _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    switch (info.Source)
        //    {
        //        default:
        //            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //            rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
        //            _log.Info($"Th={threadId} - Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //            return rm;
        //    }
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] LeadInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _log.Info($"Th={threadId} - PUT called with Payload:\n");
            _log.DebugFormat($"Th={threadId} - PUT called for id = {id}\nPayload:\n {CrmPlusControl.SerializeNoNull(info)}");

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
//            #if !DEV
//            else {
//#if !DEV
//                // TOKEN VERIFICATION WITH GUID
            #if !DEV
            else
            {
#if !DEV
                // TOKEN VERIFICATION WITH GUID

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
#endif
            }
#endif

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

            FormatLeadInfo(ref info);
            HttpResponseMessage rm = null;
            switch (info.Source)
            {
                case (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto:
                    rm = CrmPlusControl.ValidateEmail(threadId, guid, LeadEntity.EntityTypeCode, id, info.MklId);
                    break;
                case (int)Crm.Schema.Generated.ed_informationsource.Kampanj:
                    rm = CrmPlusControl.ValidateEmailKampanj(threadId, guid);
                    //rm = CrmPlusControlCampaign.QualifyLeadToUnvalidatedCustomer(threadId, info);
                    break;
                default:
                    rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    break;
            }

            //Return Logg
            if (rm.StatusCode != HttpStatusCode.OK)
            {
                if (rm.StatusCode == HttpStatusCode.Created || rm.StatusCode == HttpStatusCode.Accepted)
                {
                    _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}).\n");
                }
                
                if (rm.StatusCode == HttpStatusCode.BadRequest || rm.StatusCode == HttpStatusCode.InternalServerError || rm.StatusCode == HttpStatusCode.NotFound) {
                    // Controller loggingn too much information when status returns "Created"
                    _log.Warn($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}), Content = {rm.Content.ReadAsStringAsync().Result}\n");
                }
            }
            else
            {
                _log.Info($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}).\n");
                _log.Debug($"Th={threadId} - Returning statuscode = {rm.StatusCode} ({(int)rm.StatusCode}), Content = {rm.Content.ReadAsStringAsync().Result}\n");
            }

            return rm;
        }
    }
}
