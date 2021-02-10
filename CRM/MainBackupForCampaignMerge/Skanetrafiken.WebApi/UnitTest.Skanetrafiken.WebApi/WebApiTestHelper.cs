using Microsoft.Crm.Sdk.Samples;
using Skanetrafiken.Crm.Controllers;
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

        //public static string WebApiRootEndpoint = @"https://crm.endeavor.se:8091/api/";              // ED UTV med token
        //public static string WebApiRootEndpoint = @"https://crm.endeavor.se:8092/api/";              // ED UTV Gammal, anv ej!

        //public static string WebApiRootEndpoint = @"https://crmwebapi-tst.skanetrafiken.se/api/";    // DK test med kabel, token och CERT
        public static string WebApiRootEndpoint = @"https://crmwebapi-acc.skanetrafiken.se/api/";    // DK Acc med kabel, token och CERT

        //public static string WebApiRootEndpoint = @"https://crmwebapi-n1.skanetrafiken.se/api/";     // DK prod:1 med kabel, token och CERT
        //public static string WebApiRootEndpoint = @"https://crmwebapi-n2.skanetrafiken.se/api/";     // DK prod:2 med kabel, token och CERT

        //public static string WebApiRootEndpoint = @"http://localhost:37909/api/";                    // Egen maskin
    }

    public static class PluginFixtureBaseExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="crmId">Optional guid to create token for</param>
        public static void CreateTokenForTest(this PluginFixtureBase fixture, Plugin.LocalPluginContext localContext, HttpWebRequest httpWebRequest, Guid? crmId = null)
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

            //string tokenCertName = "SE162321000255-F16679";
            string token = Identity.EncodeTokenEncryption(tokenClass, WebApiTestHelper.clientCert);
            httpWebRequest.Headers["X-CRMPlusToken"] = token;

            //string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
            // Add client cert
            //string clientCertName = "SE162321000255-F16675";
            httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(WebApiTestHelper.clientCert));

        }
    }
}
