using Endeavor.Crm;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Skanetrafiken.Crm
{
    public class ApiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="crmId">Optional guid to create token for</param>
        public static void CreateTokenForVoucherService(Plugin.LocalPluginContext localContext, HttpWebRequest httpWebRequest, Guid? crmId = null)
        {
            const long tokenLifetimeSeconds = 30 * 60 + 60 * 60 * 1;  // Add 1h for Summertime UTC

            Int32 unixTimeStamp;
            DateTime currentTime = DateTime.Now;
            DateTime zuluTime = currentTime.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;

            long validTo = unixTimeStamp + tokenLifetimeSeconds;

            //Identity tokenClass = new Identity();
            //if (crmId != null)
            //    tokenClass.crmId = (Guid)crmId;
            //tokenClass.exp = validTo;

            string certName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);

            //string token = Identity.EncodeTokenEncryption(tokenClass, certName);


            var tokenClass = new IdentityContract();
            if (crmId != null)
                tokenClass.crmId = (Guid)crmId;
            tokenClass.exp = validTo;

            var token = EncodeTokenEncryption(tokenClass, certName);

            httpWebRequest.Headers["X-CRMPlusToken"] = token;
            
            
            httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(certName));
        }

        public static string EncodeTokenEncryption(IdentityContract identityContract, string certName)
        {
            DataContractJsonSerializer js = null;
            MemoryStream msObj = new MemoryStream();

            try
            {
                js = new DataContractJsonSerializer(typeof(IdentityContract));
                js.WriteObject(msObj, identityContract);

                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);
                string json = sr.ReadToEnd();
                sr.Close();
                msObj.Close();

                return json;
            }
            catch (Exception e)
            {
                throw new Exception("Exception Caught when trying to serialize identity object to string.", e);
            }
        }
  
    }
}