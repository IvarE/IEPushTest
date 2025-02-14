﻿using log4net;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Skanetrafiken.Crm.Controllers
{
    public class WrapperController : ApiController
    {
        /// <summary>
        /// Method to easily validate a token.
        /// </summary>
        /// <param name="id">A verified id, Cannot be null.</param>
        /// <returns>If the token is valid a httpResponseMessage with httpCode = 200 (OK) is returned.
        /// In every other case the resulting httpResponseMessage should be passed along to the caller as an error.</returns>
        protected HttpResponseMessage TokenValidation(string id = null)
        {
            StatusBlock tokenValidationStatusBlock = CrmPlusUtility.GetConfigBoolSetting("TokenValidation");
            if (!tokenValidationStatusBlock.TransactionOk)
            {
                HttpResponseMessage configError = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                configError.Content = new StringContent(tokenValidationStatusBlock.ErrorMessage);
                return configError;
            }
            if (bool.Parse(tokenValidationStatusBlock.Information))
            {
                return this.VerifyMyAccountToken(id);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// Method to be used by apiControllers in order to verify that the "X-StMklToken" is present and valid in terms of encoding and content.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage VerifyMyAccountToken(string id = null)
        {
            Identity identity = null;
            string token = "";
            string certName = "";

            using (var _logger = new AppInsightsLogger())
            {
                if (this.Request.Headers == null)
                    throw new Exception(Resources.HeadersMissing);

                string MKLHeaderName = ConfigurationManager.AppSettings["MKLTokenHeaderName"];
                string InternalHeaderName = ConfigurationManager.AppSettings["InternalTokenHeaderName"];
                string EcommerceHeaderName = ConfigurationManager.AppSettings["EcommerceTokenHeaderName"];
                string SeKundFasadenHeaderName = ConfigurationManager.AppSettings["SeKundFasadenHeaderName"];

                if (!(this.Request.Headers.Contains(MKLHeaderName) || this.Request.Headers.Contains(InternalHeaderName) || this.Request.Headers.Contains(EcommerceHeaderName) || this.Request.Headers.Contains(SeKundFasadenHeaderName)))
                {
                    HttpResponseMessage hdrResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var errMsg = string.Format(Resources.TokenHeaderMissing, MKLHeaderName, InternalHeaderName, EcommerceHeaderName, SeKundFasadenHeaderName);
                    hdrResp.Content = new StringContent(errMsg);

                    _logger.LogError(errMsg);

                    return hdrResp;
                }

                if (this.Request.Headers.Contains(MKLHeaderName))
                {
                    certName = ConfigurationManager.AppSettings["MKLTokenCertificateName"];
                    IEnumerable<string> headerValues = this.Request.Headers.GetValues(MKLHeaderName);
                    token = headerValues.FirstOrDefault();
                }
                else if (this.Request.Headers.Contains(InternalHeaderName))
                {
                    IEnumerable<string> headerValues = this.Request.Headers.GetValues(InternalHeaderName);
                    token = headerValues.FirstOrDefault();
                    certName = ConfigurationManager.AppSettings["InternalTokenCertificateName"];
                }
                else if (this.Request.Headers.Contains(EcommerceHeaderName))
                {
                    IEnumerable<string> headerValues = this.Request.Headers.GetValues(EcommerceHeaderName);
                    token = headerValues.FirstOrDefault();
                    certName = ConfigurationManager.AppSettings["EcommerceTokenCertificateName"];
                }
                else if (this.Request.Headers.Contains(SeKundFasadenHeaderName))
                {
                    IEnumerable<string> headerValues = this.Request.Headers.GetValues(SeKundFasadenHeaderName);
                    token = headerValues.FirstOrDefault();
                    certName = ConfigurationManager.AppSettings["SeKundFasadenCertificateName"];
                }

                StatusBlock tokenEncryptionStatusBlock = CrmPlusUtility.GetConfigBoolSetting("TokenEncryptionEnabled");
                if (!tokenEncryptionStatusBlock.TransactionOk)
                {
                    HttpResponseMessage encrResp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    var errMsg = tokenEncryptionStatusBlock.ErrorMessage;
                    encrResp.Content = new StringContent(errMsg);

                    _logger.LogError(errMsg);

                    return encrResp;
                }

                if (bool.Parse(tokenEncryptionStatusBlock.Information))
                {
                    identity = Identity.DecodeTokenEncryption(token, certName);
                }
                else
                    identity = Identity.DecodeToken(token);

                if (identity == null)
                {
                    HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var errMsg = "Ogiltig Token. Kunde inte läsa någon data.";
                    errorResp.Content = new StringContent(errMsg);

                    _logger.LogError(errMsg);

                    return errorResp;
                }

                if (identity.expirationTime == null)
                {
                    HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var errMsg = "Ogiltig token. Ingen giltighetstid funnen.";
                    errorResp.Content = new StringContent(errMsg);

                    _logger.LogError(errMsg);

                    return errorResp;
                }

                if (DateTime.Now.ToUniversalTime().CompareTo(identity.expirationTime) > 0)
                {
                    HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    var errMsg = $"Ogiltig Token. Giltighetstiden gick ut {identity.expirationTime}.";
                    errorResp.Content = new StringContent(errMsg);

                    _logger.LogError(errMsg);

                    return errorResp;
                }

                // If a GUID is present, make sure it matches.
                // *******************************************
                if (id != null)
                {
                    Guid guid = Guid.Empty;
                    if (!Guid.TryParse(id, out guid))
                    {
                        HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        var errMsg = $"Kunde inte tolka {id} som ett giltigt CRM Guid.";
                        errorResp.Content = new StringContent(errMsg);

                        _logger.LogError(errMsg);

                        return errorResp;
                    }
                    if (identity.crmId == null)
                    {
                        HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        var errMsg = $"Ogiltig Token. Parametern 'crmId' saknas.";
                        errorResp.Content = new StringContent(errMsg);

                        _logger.LogError(errMsg);

                        return errorResp;
                    }
                    if (!guid.Equals(identity.crmId))
                    {
                        HttpResponseMessage errorResp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        var errMsg = $"Ogiltig Token. Parametern 'crmId' matchade inte entitetens guid.";
                        errorResp.Content = new StringContent(errMsg);

                        _logger.LogError(errMsg);

                        return errorResp;
                    }
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        public static void FormatCustomerInfo(ref CustomerInfo customerInfo)
        {
            if (customerInfo != null)
            {
                customerInfo.FirstName = ContactEntity.Capitalise(customerInfo.FirstName);
                customerInfo.LastName = ContactEntity.Capitalise(customerInfo.LastName);
                if (customerInfo.AddressBlock != null)
                {
                    customerInfo.AddressBlock.Line1 = ContactEntity.Capitalise(customerInfo.AddressBlock.Line1);
                    customerInfo.AddressBlock.City = ContactEntity.Capitalise(customerInfo.AddressBlock.City);
                }
                if (!string.IsNullOrEmpty(customerInfo.Email))
                    customerInfo.Email = customerInfo.Email.ToLower().Trim(" ".ToCharArray());
                if (!string.IsNullOrEmpty(customerInfo.NewEmail))
                    customerInfo.NewEmail = customerInfo.NewEmail.ToLower().Trim(" ".ToCharArray());
                if (!string.IsNullOrEmpty(customerInfo.Telephone))
                    customerInfo.Telephone = customerInfo.Telephone.Replace(" ", "");
                if (!string.IsNullOrEmpty(customerInfo.Mobile))
                    customerInfo.Mobile = customerInfo.Mobile.Replace(" ", "");
            }
        }

        public static void FormatLeadInfo(ref LeadInfo leadInfo)
        {
            if (leadInfo != null)
            {
                leadInfo.FirstName = ContactEntity.Capitalise(leadInfo.FirstName);
                leadInfo.LastName = ContactEntity.Capitalise(leadInfo.LastName);
                if (leadInfo.AddressBlock != null)
                {
                    leadInfo.AddressBlock.City = ContactEntity.Capitalise(leadInfo.AddressBlock.City);
                    leadInfo.AddressBlock.Line1 = ContactEntity.Capitalise(leadInfo.AddressBlock.Line1);
                }
                if (!string.IsNullOrEmpty(leadInfo.Email))
                    leadInfo.Email = leadInfo.Email.ToLower().Trim(" ".ToCharArray());
                if (!string.IsNullOrEmpty(leadInfo.NewEmail))
                    leadInfo.NewEmail = leadInfo.NewEmail.ToLower().Trim(" ".ToCharArray());
                if (!string.IsNullOrEmpty(leadInfo.Telephone))
                    leadInfo.Telephone = leadInfo.Telephone.Replace(" ", "");
                if (!string.IsNullOrEmpty(leadInfo.Mobile))
                    leadInfo.Mobile = leadInfo.Mobile.Replace(" ", "");
            }
        }

        protected HttpResponseMessage CreateErrorResponseWithStatusCode(HttpStatusCode statusCode, string methodName, string message, AppInsightsLogger logger)
        {
            HttpResponseMessage response = new HttpResponseMessage(statusCode);
            response.Content = new StringContent(message);

            logger.LogError($"{methodName}: Failed - {message}");

            return response;
        }
    }
}
