using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Import_Customers
{
    public class xRMConnection
    {
        #region Declarations
        private readonly string _uri;
        private string _sufix = "/XRMServices/2011/Organization.svc";
        #endregion

        #region Public Properties
        public CRMType Crmtype { get; set; }

        public AuthenticationType Authenticationtype { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }
        #endregion

        #region Constructors
        public xRMConnection(string url)
        {
            _uri = url;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get OrganizationServiceProxy
        /// </summary>
        /// <returns></returns>
        public OrganizationServiceProxy GetService()
        {
            OrganizationServiceProxy service;

            try
            {
                string discoveryService = string.Format("{0}{1}", this._uri, this._sufix);
                Uri uri = new Uri(discoveryService);

                AuthenticationCredentials credentials = _getAuthenticationCredentials();
                IServiceManagement<IOrganizationService> _o = ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(uri.ToString()));
                service = GetProxy<IOrganizationService, OrganizationServiceProxy>(_o, credentials);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return service;
        }
        #endregion

        #region Private Methods
        private AuthenticationCredentials _getAuthenticationCredentials()
        {
            AuthenticationCredentials credentials = new AuthenticationCredentials();

            try
            {
                if (Crmtype == CRMType.OnLine)
                {
                    credentials.ClientCredentials.Windows.ClientCredential = new NetworkCredential();
                    credentials.ClientCredentials.UserName.UserName = Username;
                    credentials.ClientCredentials.UserName.Password = Password;
                }
                else if (Crmtype == CRMType.OnPremis && Authenticationtype == AuthenticationType.User)
                {
                    credentials.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(Username, Password, Domain);
                }
                else if (Crmtype == CRMType.OnPremis && Authenticationtype == AuthenticationType.Default)
                {
                    credentials.ClientCredentials.Windows.ClientCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return credentials;
        }

        private TProxy GetProxy<TService, TProxy>(IServiceManagement<TService> serviceManagement, AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType !=
                AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials =
                    serviceManagement.Authenticate(authCredentials);
                // Obtain discovery/organization service proxy for Federated, LiveId and OnlineFederated environments. 
                // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and SecurityTokenResponse.
                var constructorInfo = classType
                    .GetConstructor(new[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) });
                if (constructorInfo != null)
                    return (TProxy)constructorInfo
                        .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            var memberInfo = classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) });
            if (memberInfo != null)
                return (TProxy)memberInfo
                    .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });

            return null;
        }
        #endregion
    }
}
