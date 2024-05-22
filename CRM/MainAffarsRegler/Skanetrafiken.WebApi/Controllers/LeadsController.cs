using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class LeadsController : WrapperController
    {
        private string _prefix = "Lead";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        //INFO: teo - Is not part of HttpRouting in Production-releases
        public HttpResponseMessage GetLatestLinkGuid(string email)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (string.IsNullOrWhiteSpace(email))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetLatestLinkGuid", Resources.ParameterMissingEmail, _logger);
                }

                try
                {
                    HttpResponseMessage linkGuidResponse = CrmPlusControl.RetrieveLeadLinkGuid(threadId, email);
                    return linkGuidResponse;
                }
                catch (Exception e)
                {
                    // Log to Application Insights
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);
                    // Handle the exception
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetLatestLinkGuid", "An error occurred", _logger);
                }
            }
        }


        [HttpGet]
        public HttpResponseMessage GetLeadInfo(string campaignCode)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (string.IsNullOrWhiteSpace(campaignCode))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.ParameterMissingCampaignCode, _logger);
                }

                try
                {
                    HttpResponseMessage leadInformationResponse = CrmPlusControlCampaign.RetrieveLeadInfo(threadId, campaignCode, _prefix);

                    return leadInformationResponse;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "GetLeadInfo", string.Format(Resources.UnexpectedException), _logger);
                }
            }
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            using (var _logger = new AppInsightsLogger())
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;

                return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Get", Resources.GenericGetNotSupported, _logger);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                int threadId = Thread.CurrentThread.ManagedThreadId;

                if (string.IsNullOrWhiteSpace(id))
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "GetWithId", "Could not find an 'id' parameter in url", _logger);
                }
                //TOKEN VERIFICATION
                try
                {
                    HttpResponseMessage tokenResp = TokenValidation(id);
                    if (tokenResp.StatusCode != HttpStatusCode.OK)
                    {
                        return tokenResp;
                    }
                }
                catch (Exception ex)
                {
                    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));

                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return rm;
                }

                HttpResponseMessage resp = CrmPlusControl.GetLead(threadId, id, _prefix);

                return resp;
            }
        }

        //[HttpPost]
        //public HttpResponseMessage LeadInfoPost([FromBody] LeadInfo leadInfo)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;

        //    if (leadInfo == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
        //        return rm;
        //    }


        //    HttpResponseMessage resp1 = CrmPlusControlCampaign.PostLeadAndQualifyToContact(leadInfo);
        //    return resp1;

        //}



        [HttpPost]
        public HttpResponseMessage Post([FromBody] LeadInfo info)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (info == null)
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
                //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
                //    return rm;
                //}

                try
                {
                    FormatLeadInfo(ref info);
                    HttpResponseMessage rm = null;
                    switch (info.Source)
                    {
                        case (int)Crm.Schema.Generated.ed_informationsource.SkapaMittKonto:
                            rm = CrmPlusControl.CreateCustomerLead(threadId, info, _prefix);
                            break;
                        case (int)Crm.Schema.Generated.ed_informationsource.Kampanj:
                            rm = CrmPlusControlCampaign.PostLeadAndQualifyToContact(threadId, info, _prefix);
                            break;
                        case (int)Crm.Schema.Generated.ed_informationsource.OinloggatLaddaKort:
                            rm = CrmPlusControl.NonLoginRefill(threadId, info, _prefix);
                            break;
                        case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                            break;
                        default:
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", string.Format(Resources.InvalidSource, info.Source), _logger);
                    }
                    return rm;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "Post", string.Format(Resources.UnexpectedException), _logger);

                }
            }
        }

        //[HttpPost]
        //public HttpResponseMessage PostParam([FromBody] CustomerInfo info)
        //{
        //    int threadId = Thread.CurrentThread.ManagedThreadId;

        //    if (info == null)
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(Resources.IncomingDataCannotBeNull);
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
        //    //        return tokenResp;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //    //    rm.Content = new StringContent(string.Format(Resources.UnexpectedException, ex.Message));
        //    //    return rm;
        //    //}

        //    DateTime dt = new DateTime();
        //    if (!DateTime.TryParse(time, out dt))
        //    {
        //        HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //        rm.Content = new StringContent(string.Format(Resources.DateTimeWrongFormat, time));
        //        return rm;
        //    }

        //    FormatCustomerInfo(ref info);

        //    switch (info.Source)
        //    {
        //        default:
        //            HttpResponseMessage rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
        //            rm.Content = new StringContent(string.Format(Resources.InvalidSource, info.Source));
        //            return rm;
        //    }
        //}

        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] LeadInfo info)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                try
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;

                    if (info == null)
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.IncomingDataCannotBeNull, _logger);
                    }
                    Guid guid = Guid.Empty;
                    if (info.Guid == null || !Guid.TryParse(info.Guid, out guid))
                    {
                        return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.GuidNotValid, _logger);
                    }
                    // TOKEN VERIFICATION WITH GUID
#if !DEV
                    else
                    {
#if !DEV
                        // TOKEN VERIFICATION WITH GUID
                        HttpResponseMessage tokenResp = TokenValidation(guid.ToString());
                        if (tokenResp.StatusCode != HttpStatusCode.OK)
                        {
                            return tokenResp;
                        }
#endif
                    }
#endif

                    if (info.Source != (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto)
                    {
                        if (!id.Equals(info.Guid))
                        {
                            return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.GuidMismatchBodyAndUrl, _logger);
                        }
                    }

                    FormatLeadInfo(ref info);
                    HttpResponseMessage rm = null;
                    switch (info.Source)
                    {
                        case (int)Crm.Schema.Generated.ed_informationsource.LoggaInMittKonto:
                            rm = CrmPlusControl.ValidateEmail(threadId, guid, LeadEntity.EntityTypeCode, id, info.MklId, _prefix);
                            break;
                        case (int)Crm.Schema.Generated.ed_informationsource.Kampanj:
                            //rm = CrmPlusControl.ValidateEmailKampanj(threadId, guid);
                            //rm = CrmPlusControlCampaign.QualifyLeadToUnvalidatedCustomer(threadId, info);
                            break;
                        default:
                            rm = CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", string.Format(Resources.IncomingDataCannotBeNull, info.Source), _logger);
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
                
            }
        }
    }
}
