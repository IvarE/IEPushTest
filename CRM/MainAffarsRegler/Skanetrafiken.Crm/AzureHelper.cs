using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm
{
    [DataContract]
    public class AzureHelper
    {
        /// <summary>
        /// DELETE IF NOT USED
        /// </summary>
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public int expires_in { get; set; }
        [DataMember]
        public string token_type { get; set; }

        public static string GetAccessToken(string apiTokenUrl, string clientId, string clientSecret, string resource = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestParameters = new Dictionary<string, string>();
                requestParameters.Add("Content-Type", "application/x-www-form-urlencoded");
                requestParameters.Add("grant_type", "client_credentials");

                requestParameters.Add("client_id", clientId);
                requestParameters.Add("client_secret", clientSecret);

                if (resource != null)
                {
                    requestParameters.Add("resource", resource);
                }

                var content = new FormUrlEncodedContent(requestParameters);
                var httpResponse = httpClient.PostAsync(apiTokenUrl, content);
                var resp = httpResponse.Result.Content.ReadAsStringAsync().Result;

                AzureHelper token = JsonHelper.JsonDeserialize<AzureHelper>(resp);

                return $"{token.token_type} {token.access_token}";
            }

        }
    }
}
