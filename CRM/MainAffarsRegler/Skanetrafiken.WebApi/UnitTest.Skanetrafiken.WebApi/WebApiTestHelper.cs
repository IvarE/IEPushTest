using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Controllers;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Models;
using System;
using System.Net;

namespace Endeavor.Crm.UnitTest
{
    public class WebApiTestHelper
    {
        //const string tokenCert = "SE162321000255-F16679";           // SE162321000255-F16679 - mittkontotest.skanetrafiken.se
        public const string clientCert = "SE162321000255-F16675";          // SE162321000255-F16675 - sekundtest1.skanetrafiken.se

        /*****************************************************************************
                Cert. För test. KlientCert 
             Fyll på med beskrivning för att sätta upp tester här.....



         *****************************************************************************/

        public static string WebApiRootEndpoint_LocalTest = @"http://localhost:37909/api/";
#if DEV
        //public static string WebApiRootEndpoint = @"https://crmwebapi-tst.skanetrafiken.se/api/";              // ED UTV med token

#endif
#if TEST
        //public static string WebApiRootEndpoint = @"https://crmwebapi-tst.skanetrafiken.se/api/";    // DK test med kabel, token och CERT
        public static string WebApiRootEndpoint = @"http://localhost:37909/api/";
#endif
#if ACCEPTANS
        public static string WebApiRootEndpoint = @"https://crmwebapi-acc.skanetrafiken.se/api/";    // DK Acc med kabel, token och CERT
        //public static string WebApiRootEndpoint = @"http://localhost:37909/api/";
#endif
#if PRODUKTION
        public static string WebApiRootEndpoint = @"https://crmwebapi-n1.skanetrafiken.se/api/";     // DK prod:1 med kabel, token och CERT
        //public static string WebApiRootEndpoint = @"https://crmwebapi-n2.skanetrafiken.se/api/";     // DK prod:2 med kabel, token och CERT
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="crmId">Optional guid to create token for</param>
        public static void CreateTokenForTest(Plugin.LocalPluginContext localContext, HttpWebRequest httpWebRequest, Guid? crmId = null)
        {
            const long tokenLifetimeSeconds = 30 * 60 + 60 * 60 * 1;  // Add 1h for Summertime UTC

            Int32 unixTimeStamp;
            DateTime currentTime = DateTime.Now;
            DateTime zuluTime = currentTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;

            long validTo = unixTimeStamp + tokenLifetimeSeconds;

            Identity tokenClass = new Identity();
            if (crmId != null)
                tokenClass.crmId = (Guid)crmId;
            tokenClass.exp = validTo;
#if !DEV
            //string tokenCertName = "SE162321000255-F16679";
            string token = Identity.EncodeTokenEncryption(tokenClass, WebApiTestHelper.clientCert);
            httpWebRequest.Headers["X-CRMPlusToken"] = token;

            //string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
            // Add client cert
            //string clientCertName = "SE162321000255-F16675";
            httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCert));
#endif

        }


        /// <summary>
        /// Get unique string YYMMDD.HHSS
        /// </summary>
        /// <returns></returns>
        public static string GetUnitTestID()
        {
            DateTime today = DateTime.Now;

            return today.ToString("yyyyMMdd.hhmm");
        }

        public static string SerializeCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customer)
        {
            return JsonConvert.SerializeObject(customer, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeAccountInfo(Plugin.LocalPluginContext localContext, AccountInfo customer)
        {
            return JsonConvert.SerializeObject(customer, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>
        /// Serializes model to json string
        /// </summary>
        /// <param name="obj">The model you wish to serialize.</param>
        /// <returns></returns>
        public static string GenericSerializer(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeNotificationInfo(Plugin.LocalPluginContext localContext, NotificationInfo notification)
        {
            return JsonConvert.SerializeObject(notification, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public static string SerializeNotificationInfos(Plugin.LocalPluginContext localContext, NotificationInfo[] notifications)
        {
            return JsonConvert.SerializeObject(notifications, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        internal static LeadInfo ToLeadInfo(CustomerInfo customerInfo)
        {
            LeadInfo leadInfo = new LeadInfo
            {
                Avliden = customerInfo.Avliden,
                AvlidenSpecified = customerInfo.AvlidenSpecified,
                CreditsafeOk = customerInfo.CreditsafeOk,
                CreditsafeOkSpecified = customerInfo.CreditsafeOkSpecified,
                Email = customerInfo.Email,
                EmailInvalid = customerInfo.EmailInvalid,
                EmailInvalidSpecified = customerInfo.EmailInvalidSpecified,
                FirstName = customerInfo.FirstName,
                Guid = customerInfo.Guid,
                isAddressEnteredManually = customerInfo.isAddressEnteredManually,
                isAddressEnteredManuallySpecified = customerInfo.isAddressEnteredManuallySpecified,
                isAddressInformationComplete = customerInfo.isAddressInformationComplete,
                isAddressInformationCompleteSpecified = customerInfo.isAddressInformationCompleteSpecified,
                LastName = customerInfo.LastName,
                MklId = customerInfo.MklId,
                Mobile = customerInfo.Mobile,
                NewEmail = customerInfo.NewEmail,
                SocialSecurityNumber = customerInfo.SocialSecurityNumber,
                Source = customerInfo.Source,
                SwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber,
                SwedishSocialSecurityNumberSpecified = customerInfo.SwedishSocialSecurityNumberSpecified,
                Telephone = customerInfo.Telephone,
                Utvandrad = customerInfo.Utvandrad,
                UtvandradSpecified = customerInfo.UtvandradSpecified
            };
            if (customerInfo.AddressBlock != null)
            {
                leadInfo.AddressBlock = new CustomerInfoAddressBlock
                {
                    City = customerInfo.AddressBlock.City,
                    CO = customerInfo.AddressBlock.CO,
                    CountryISO = customerInfo.AddressBlock.CountryISO,
                    Line1 = customerInfo.AddressBlock.Line1,
                    PostalCode = customerInfo.AddressBlock.PostalCode
                };
            }
            return leadInfo;
        }
    }
}
