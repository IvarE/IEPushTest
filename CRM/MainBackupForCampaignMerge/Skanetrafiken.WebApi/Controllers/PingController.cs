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
    public class PingController : WrapperController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage nrm = new HttpResponseMessage(HttpStatusCode.OK);
            return nrm;
        }
#if !PRODUKTION
        
        [HttpGet]
        public HttpResponseMessage GetWithId(string id)
        {
            try
            {
                _log.DebugFormat("PING called with parameter: id = {0}", id);
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

                //return new HttpResponseMessage(HttpStatusCode.OK);

                HttpResponseMessage resp = CrmPlusControl.GetContactTroubleshooting(id);
                //_log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", resp.StatusCode, resp.Content.ReadAsStringAsync().Result);
                return resp;

            }
            catch (Exception e)
            {
                HttpResponseMessage errResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errResp.Content = new StringContent(e.Message);
                _log.DebugFormat("Returning statuscode = {0}, Content = {1}\n", errResp.StatusCode, errResp.Content.ReadAsStringAsync().Result);
                return errResp;
            }
        }

#endif
    }
}
