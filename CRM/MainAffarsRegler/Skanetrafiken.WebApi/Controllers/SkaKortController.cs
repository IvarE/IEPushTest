using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Threading;
using System.Collections.Generic;

namespace Skanetrafiken.Crm.Controllers
{
    /// <summary>
    /// Handles registration and revocation of SkaKort.
    /// </summary>
    public class SkaKortController : WrapperController
    {
        private string _prefix = "SkaKort";
        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };

        /// <summary>
        /// Registers or updates a SkaKort connected to a Contact or Company. Here operation must be equal to Register (0) even when updating an existing card.
        /// </summary>
        /// <param name="skaKortInfo">Contains information of SkaKort</param>
        /// <response code="200">SkaKort was registered or updated correctly</response>
        /// <response code="400">Bad information was received</response>
        /// <response code="500">Something unexpected happened</response>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] SkaKortInfo skaKortInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (skaKortInfo == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncomingDataCannotBeNull, _logger);
                }

                if (skaKortInfo.Operation != Operation.Register)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Post", Resources.IncorrectOperation, _logger);
                }

                try
                {
                    if (skaKortInfo.InformationSource == 1)
                        skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka; //ändra till skakort hantering (ResKortPrivat - ReskortFöretag) - Då måste det finnas en contactid
                    else if (skaKortInfo.InformationSource == 2)
                        skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal; // skickar de med en 2a så måste det finnas en accountID


                    HttpResponseMessage rm = null;
                    switch (skaKortInfo.InformationSource)
                    {
                        case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                            rm = CrmPlusControl.RegisterCompanySkaKortPost(threadId, skaKortInfo, _prefix);
                            break;
                        case (int)Schema.Generated.ed_informationsource.KopOchSkicka:
                            rm = CrmPlusControl.RegisterBuyAndSendSkaKortPost(threadId, skaKortInfo, _prefix);
                            break;
                        default:
                            rm = new HttpResponseMessage(HttpStatusCode.BadRequest);
                            rm.Content = new StringContent(string.Format(Resources.InvalidSource, skaKortInfo.InformationSource));
                            break;
                    }

                    return rm;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "Post", string.Format(Resources.UnexpectedException, ex.Message), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }

        /// <summary>
        /// Deletes or revokes a Skakort. These are the only operations accepted by this endpoint, any other value will yield a bad request.
        /// </summary>
        /// <param name="id">Card Number</param>
        /// <param name="skaKortInfo">Contains information of SkaKort</param>
        /// <response code="200">SkaKort was correctly deleted or revoked</response>
        /// <response code="400">Bad information was received</response>
        /// <response code="500">Something unexpected happened</response>
        [HttpPut]
        public HttpResponseMessage Put([FromUri] string id, [FromBody] SkaKortInfo skaKortInfo)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                if (skaKortInfo == null)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.IncomingDataCannotBeNull, _logger);
                }

                if (skaKortInfo.Operation != Operation.Delete && skaKortInfo.Operation != Operation.Revoke)
                {
                    return CreateErrorResponseWithStatusCode(HttpStatusCode.BadRequest, "Put", Resources.IncorrectOperation, _logger);
                }

                try
                {
                    if (skaKortInfo.InformationSource == 1)
                        skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.KopOchSkicka; //ändra till skakort hantering (ResKortPrivat - ReskortFöretag) - Då måste det finnas en contactid
                    else if (skaKortInfo.InformationSource == 2)
                        skaKortInfo.InformationSource = (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal; // skickar de med en 2a så måste det finnas en accountID

                    HttpResponseMessage rm = new HttpResponseMessage();

                    if (skaKortInfo.Operation == Operation.Revoke)
                    {
                        rm = CrmPlusControl.SkaKortInactivate(threadId, skaKortInfo, _prefix);
                    }
                    else if (skaKortInfo.Operation == Operation.Delete)
                    {
                        rm = CrmPlusControl.RemoveSkaKortContactOrAccount(threadId, skaKortInfo, _prefix);
                    }

                    return rm;
                }
                catch (Exception ex)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(ex, _exceptionCustomProperties);

                    return CreateErrorResponseWithStatusCode(HttpStatusCode.InternalServerError, "Put", string.Format(Resources.UnexpectedException, ex.Message), _logger);
                }
                finally
                {
                    ConnectionCacheManager.ReleaseConnection(threadId);
                }
            }
        }
    }
}