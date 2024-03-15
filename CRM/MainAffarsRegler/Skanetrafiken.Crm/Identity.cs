using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jose;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Configuration;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Skanetrafiken.Crm
{
    public class Identity
    {
        public string UserName { get; set; }
        public int accountId { get; set; }
        public string locale { get; set; }
        public int userId { get; set; }
        public long exp { get; set; }
        public DateTime expirationTime => new DateTime(1970, 1, 1).AddSeconds(exp);
        public string token { get; set; }
        public Guid crmId { get; set; }


        private static Dictionary<string, string> _exceptionCustomProperties = new Dictionary<string, string>()
        {
            { "source", "" }
        };
        private static string _prefix = "Identity";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static string EncodeTokenEncryption(Identity identity, string certName)
        {
            string decodedString = null;
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                // Serialize to string
                try
                {
                    decodedString = JsonConvert.SerializeObject(identity);
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception("Exception Caught when trying to serialize identity object to string.", e);
                }
            }

            var privateKey = GetPrivateEncodingKey(certName);
            var publicKey = GetPublicDecryptionKey(certName);
            string b64Key = Convert.ToBase64String(publicKey.ExportCspBlob(false));
            var header = new Dictionary<string, object>()
                    {
                        {"x5u", b64Key}
                    };

            return JWT.Encode(decodedString, privateKey, JwsAlgorithm.RS512, header);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Identity DecodeTokenEncryption(string token, string certName)
        {

            //string exampleToken =
            //    "eyJhbGciOiJSUzUxMiIsIng1dSI6IkJnSUFBQUNrQUFCU1UwRXhBQWdBQUFFQUFRQ3huWUdEYXFqcEx4OEs0OTlKRXhEaWlXbG05aUo3N0t4Y21TS3JpK1oxbzJTQ1o3YnhTMGRnZDVwaklFcUhhQlZDelFGZU5FZlI0VmtBNTcxbFR5NFo3bzhHQlU0OEF4Y245V2J4TWdFa0hzb2xESmNLS3BKa2FKVlZwTE9qVmpWZVRkbkJCcmRvRU1OQ2swZWlwNXoyZTc2aTE0aEh3Q2pmZmkzWjFqUVNCQTBuSVgvUWdrK2FFVXJHNTBBZ1FDSUxPOHg1UmV0THBoblBMSmZubnA0cmlGSytvcGZYeGF1ZmhUenVhRmFFNVQ3NVI4cE1ScjNWYTRoclRnRFFOblZmV21pcGlYVEVwTUtTeFRFMUFDWnlxSkxVSjlzVjVRZk1lQVRuSkZOYVRqbjZMQ2d6UnJtY3JJenZ1K2xuOWFlcHZuM011cE9pbHJ1RDg0cExHc0dpIn0.dGVzdA.kszOJeleckLwkk5ZdSyH5VFuZJNQCVIfdMOzRWJdnt-PfpxIAd3IPb43ec5BihqV-6zMLYgJdb9cnqJ4I-GfOu5Wy2UFFrI6Hivux3Qg9LVQD9rEEyIXqHE90n5hsWNuTQRbXTfoJKmt2S40hnSpjdAS-pDtH4pd3HeQbp5OyfkSNWbPOEAYQJTlvU5L4zy1Hq15s22FImWWUTWosimcpS9cKF3CPwfWyhT6kH1M0sqjtd4weuCXKnuPLMmnSeqW8yXQ3VPWoDpylQgync-U7v62u5RRph9JqtXqTTNtLOLXH7prU2rh8NQsFysyX-sXAo25Fx52cdWZ0KWhB7Bozw";
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                string decodedString = null;
                try
                {
                    var publicKey = GetPublicDecryptionKey(certName);
                    decodedString = JWT.Decode(token, publicKey);
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception caught when trying to decode payload.\n\n{e.Message}", e);
                }

                // Deserialize
                try
                {
                    return JsonConvert.DeserializeObject<Identity>(decodedString);
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);
                    
                    throw new Exception("Exception Caught when trying to deserialize token-string into class identity. Possible cause is faulty token.", e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Identity DecodeToken(string token)
        {
            string payload = null;
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                try
                {
                    payload = JWT.Decode(token);
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception($"Exception caught when trying to decode payload.\n\n{e.Message}", e);
                }

                try
                {
                    return JsonConvert.DeserializeObject<Identity>(payload);
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception("Exception Caught when trying to deserialize Payload into Identity", e);
                }
            }
        }

        /// <summary>
        /// Get public for decoding.
        /// </summary>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPublicDecryptionKey(string certName)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);
                try
                {
                    var cert = GetCertToUse(certName);
                    return cert.PublicKey.Key as RSACryptoServiceProvider;
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception($"Exception caught when trying to access PublicEncodingKey:\n\n{e.Message}", e);
                }
            }
        }

        /// <summary>
        /// Get privateKey for encoding.
        /// </summary>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPrivateEncodingKey(string certName)
        {
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                try
                {
                    var cert = GetCertToUse(certName);

                    if (cert.PrivateKey == null)
                        throw new Exception("Failed to get Private key. Make sure certificate is properly installed");


                    var certPrivateKey = cert.PrivateKey as RSACryptoServiceProvider;


                    // Do we need to rewrite the cert?
                    // Code from Niclas Olofsson
                    if (certPrivateKey.CspKeyContainerInfo.ProviderName == "Microsoft RSA SChannel Cryptographic Provider")
                    {
                        return FixRsaKey(certPrivateKey);
                    }
                    else
                        return certPrivateKey;
                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception($"Exception caught when trying to access PrivateEncodingKey.\n\n{e.Message}", e);
                }
            }
        }

        private static RSACryptoServiceProvider FixRsaKey(RSACryptoServiceProvider csp)
        {
            CspParameters cp = new CspParameters(24);
            var privateKeyBlob = csp.ExportCspBlob(true);
            csp = new RSACryptoServiceProvider(cp);
            csp.ImportCspBlob(privateKeyBlob);
            return csp;
        }


        public static X509Certificate2 GetCertToUse(string certName)
        {
            X509Certificate2 cert = null;

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            //var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            using (var _logger = new AppInsightsLogger())
            {
                _logger.SetGlobalProperty("source", _prefix);

                try
                {
                    store.Open(OpenFlags.ReadOnly);

                    var certCollection = store.Certificates;
                    var currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                    //var currentCerts = certCollection.Find(X509FindType.FindBySubjectName, certName, false);

                    if (certName == null)
                        throw new Exception($"No Cert Name available to decode token with.");

                    var signingCert = currentCerts.Find(X509FindType.FindBySubjectName, certName, false);

                    if (signingCert.Count == 0)
                        throw new Exception($"Could not find Certificate to decode token. Cert Name:{certName}");

                    cert = signingCert.Count == 0 ? null : signingCert[0];

                }
                catch (Exception e)
                {
                    _exceptionCustomProperties["source"] = _prefix;
                    _logger.LogException(e, _exceptionCustomProperties);

                    throw new Exception($"Exception caught when trying to access certificates in local Cert Store.\n\n{e.Message}", e);
                }
                finally
                {
                    store.Close();
                }
                return cert;
            }
        }

    }

}