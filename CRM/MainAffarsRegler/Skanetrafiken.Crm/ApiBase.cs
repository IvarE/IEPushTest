using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.Crm
{
    /// <summary>
    /// DELETE IF NOT USED
    /// </summary>
    public class ResourceContainer
    {
        public string ResourceId { get; set; }
        public string Url { get; set; }
    }

    public class AuthContainer
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public ResourceContainer TicketMaster { get; set; }
    }

    public class ApiBase
    {
        private static string _apiRootUrl;
        private const string Authority = "https://login.microsoftonline.com/92f52389-3f0f-4623-9a3b-957c32d194e5";
        public static AuthenticationResult _authResult;

        public static async Task Authenticate(AuthContainer auth, ResourceContainer resourceContainer)
        {
            _apiRootUrl = resourceContainer.Url;

            var authenticationContext = new AuthenticationContext(Authority);
            _authResult = await authenticationContext.AcquireTokenAsync(resourceContainer.ResourceId, new ClientCredential(auth.ClientId, auth.ClientSecret));
        }

        public static AuthContainer GetAuth()
        {
            return new AuthContainer()
            {
                ClientId = "NEEDED FOR PROD",
                ClientSecret = "NEEDED FOR PROD",
                TicketMaster = new ResourceContainer()
                {
                    ResourceId = "https://stticketmasterprod.net",
                    Url = "https://trafik.skanetrafiken.se/tm"
                }
            };
        }
    }
}
