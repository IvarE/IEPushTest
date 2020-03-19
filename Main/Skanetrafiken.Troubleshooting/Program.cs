using Skanetrafiken.Crm.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Troubleshooting
{
    class Program
    {
        const string clientCert = "SE162321000255-F16673";          // SE162321000255-F16673 - sekund1.skanetrafiken.se
        //const string clientCert = "SE162321000255-F16675";          // SE162321000255-F16675 - sekundtest1.skanetrafiken.se
        //const string clientCert = "SE162321000255-F16679";           // SE162321000255-F16679 - mittkontotest.skanetrafiken.se

        //private static string WebApiRootEndpoint = @"https://crm.endeavor.se:8091/api/";              // ED UTV med token
        //private static string WebApiRootEndpoint = @"https://crmwebapi-acc.skanetrafiken.se/api/";    // DK Acc med kabel, token och CERT
        private static string WebApiRootEndpoint = @"https://crmwebapi.skanetrafiken.se/api/";              //DK Prod


        static void Main(string[] args)
        {
            Console.WriteLine($"Connecting to {WebApiRootEndpoint} with cert {clientCert}");

            //Console.ReadKey();
            int i = 0;
            while (i < 3)
            {
                try
                {
                    i++;

                    string url = $"{WebApiRootEndpoint}ping";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    CreateTokenForTest(httpWebRequest, null);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    Console.WriteLine($"{i} No Error, content length = {httpResponse.ContentLength}");

                    httpWebRequest.Abort();

                    System.Threading.Thread.Sleep(1);


                    //string url = $"{WebApiRootEndpoint}contacts/{new Guid("47CC480C-F6E9-E611-8122-00155D010B02").ToString()}";
                    //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    //CreateTokenForTest(httpWebRequest, new Guid("47CC480C-F6E9-E611-8122-00155D010B02"));
                    //httpWebRequest.ContentType = "application/json";
                    //httpWebRequest.Method = "GET";

                    //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    //Console.WriteLine($"{i} No Error, content length = {httpResponse.ContentLength}");

                    //httpWebRequest.Abort();

                   // System.Threading.Thread.Sleep(1);
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    string responseContent;

                    if(response == null)
                    {
                        Console.WriteLine($"{i} Error catched, message:{we.Message}");
                        Console.WriteLine($"{i} Inner message:{we.InnerException.Message}");
                    }
                    else
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            responseContent = streamReader.ReadToEnd();
                        }
                        Console.WriteLine($"{i} WebError: {responseContent}");
                    }
       
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{i} Error: {e.Message}");
                    //break;
                }

            }


            Console.ReadKey();
        }
        
        
          /// <summary>
          /// 
          /// </summary>
          /// <param name="httpWebRequest"></param>
          /// <param name="crmId">Optional guid to create token for</param>
        private static void CreateTokenForTest(HttpWebRequest httpWebRequest, Guid? crmId = null)
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
            string token = Identity.EncodeTokenEncryption(tokenClass, clientCert);
            httpWebRequest.Headers["X-CRMPlusToken"] = token;

            //string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
            // Add client cert
            //string clientCertName = "SE162321000255-F16675";
            httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCert));

        }
    }
}
