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

namespace Skanetrafiken.Crm.Controllers
{
    public class LeadsController : WrapperController
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

            HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveLeadLinkGuid(email);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", linkGuidResponse.StatusCode, linkGuidResponse.Content.ReadAsStringAsync().Result);
            return linkGuidResponse;
        }

        [HttpGet]
        public HttpResponseMessage GetLeadInfo(string campaignCode)
        {
            _log.Debug($"GetLeadInfo called for '{campaignCode}'.");
            if (string.IsNullOrWhiteSpace(campaignCode))
            {
                HttpResponseMessage returnMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                returnMessage.Content = new StringContent(Resources.ParameterMissingCampaignCode);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", returnMessage.StatusCode, returnMessage.Content.ReadAsStringAsync().Result);
                return returnMessage;
            }

            HttpResponseMessage leadInformationResponse = CrmPlusControlCampaign.RetrieveLeadInfo(campaignCode);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", leadInformationResponse.StatusCode, leadInformationResponse.Content.ReadAsStringAsync().Result);
            return leadInformationResponse;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            _log.Debug("Unsupported generic GET called.");

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
            resp.Content = new StringContent(Resources.GenericGetNotSupported);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            _log.DebugFormat("GET called with parameter: idOrEmail = {0}", id);

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

            HttpResponseMessage resp = CrmPlusControl.GetLead(id);
            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
            return resp;
        }

        //[HttpPost]
        //public HttpResponseMessage LeadInfoPost([FromBody] LeadInfo leadInfo) {

        //    if (leadInfo == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }


        //    HttpResponseMessage resp1 = CrmPlusControlCampaign.PostLeadAndQualifyToContact(leadInfo);
        //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
        //    return resp1;

        //}



        [HttpPost]
        public HttpResponseMessage Post([FromBody] LeadInfo info)
        {
            _log.DebugFormat("POST called with Payload:\n {0}", CrmPlusControl.SerializeNoNull(info));

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

            FormatLeadInfo(ref info);

            switch (info.Source)
            {
                case (int)CustomerUtility.Source.SkapaMittKonto:
                    HttpResponseMessage resp1 = CrmPlusControl.CreateCustomerLead(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
                    return resp1;
                case (int)CustomerUtility.Source.Kampanj:
                    HttpResponseMessage resp2 = CrmPlusControlCampaign.PostLeadAndQualifyToContact(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp2.StatusCode, resp2.Content.ReadAsStringAsync().Result);
                    return resp2;
                case (int)CustomerUtility.Source.OinloggatLaddaKort:
                    HttpResponseMessage resp3 = CrmPlusControl.NonLoginRefill(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp3.StatusCode, resp3.Content.ReadAsStringAsync().Result);
                    return resp3;
                default:
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                    return rm;
            }
        }

        //[HttpPost]
        //public HttpResponseMessage PostParam([FromBody] CustomerInfo info)
        //{
        //    _log.DebugFormat("POST called with parameters: time = {0}, price = {1}\nPayload:\n {2}", time, price, CrmPlusControl.SerializeNoNull(info));

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
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
        //    //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", tokenResp.StatusCode, tokenResp.Content.ReadAsStringAsync().Result);
        //    //        return tokenResp;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //    //    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //    //    return rm;
        //    //}

        //    DateTime dt = new DateTime();
        //    if (!DateTime.TryParse(time, out dt))
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(string.Format(Resources.DateTimeWrongFormat, time));
        //        _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    switch (info.Source)
        //    {
        //        default:
        //            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //            rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
        //            _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
        //            return rm;
        //    }
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] LeadInfo info)
        {
            _log.DebugFormat("PUT called for id = {0}\nPayload:\n {1}", id, CrmPlusControl.SerializeNoNull(info));
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

            FormatLeadInfo(ref info);

            switch (info.Source)
            {
                case (int)CustomerUtility.Source.ValideraEpost:
                    HttpResponseMessage resp1 = CrmPlusControl.ValidateEmail(guid, LeadEntity.EntityTypeCode, id, info.MklId);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp1.StatusCode, resp1.Content.ReadAsStringAsync().Result);
                    return resp1;
                case (int)CustomerUtility.Source.Kampanj:
                    HttpResponseMessage resp2 = CrmPlusControlCampaign.QualifyLeadToUnvalidatedCustomer(info);
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp2.StatusCode, resp2.Content.ReadAsStringAsync().Result);
                    return resp2;
                default:
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
                    _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", rm.StatusCode, rm.Content.ReadAsStringAsync().Result);
                    return rm;
            }
        }
    }
}
