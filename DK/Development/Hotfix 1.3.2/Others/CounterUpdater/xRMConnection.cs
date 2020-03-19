using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel.Description;
using Microsoft.Crm;
using Microsoft.Crm.Sdk;
using Microsoft.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Query;
using System.Net;

namespace CRM_Connection
{
    public class xRMConnection
    {

        private string _uri;
        private string _sufix = "/XRMServices/2011/Organization.svc";

        private CRMType _crmtype;
        public CRMType Crmtype
        {
            get { return _crmtype; }
            set { _crmtype = value; }
        }

        private AuthenticationType _authenticationtype;
        public AuthenticationType Authenticationtype
        {
            get { return _authenticationtype; }
            set { _authenticationtype = value; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _domain;
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public xRMConnection(string Url)
        {
            this._uri = Url;
        }

        /// <summary>
        /// Get OrganizationServiceProxy
        /// </summary>
        /// <returns></returns>
        public OrganizationServiceProxy GetService()
        {
            OrganizationServiceProxy _service = null;

            try
            {
                string _discoveryService = string.Format("{0}{1}", this._uri, this._sufix);
                Uri _uri = new Uri(_discoveryService);

                AuthenticationCredentials credentials = _getAuthenticationCredentials();
                IServiceManagement<IOrganizationService> _o = ServiceConfigurationFactory.CreateManagement<IOrganizationService>(new Uri(_uri.ToString()));
                _service = GetProxy<IOrganizationService, OrganizationServiceProxy>(_o, credentials);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _service;
        }

        private AuthenticationCredentials _getAuthenticationCredentials()
        {
            AuthenticationCredentials credentials = new AuthenticationCredentials();

            try
            {
                if (_crmtype == CRMType.OnLine)
                {
                    credentials.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential();
                    credentials.ClientCredentials.UserName.UserName = this._username;
                    credentials.ClientCredentials.UserName.Password = this.Password;
                }
                else if (_crmtype == CRMType.OnPremis && _authenticationtype == AuthenticationType.User)
                {
                    credentials.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(this._username, this.Password, this.Domain);
                }
                else if (_crmtype == CRMType.OnPremis && _authenticationtype == AuthenticationType.Default)
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
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }

    }

    public enum CRMType
    {
        OnLine,
        OnPremis
    }

    public enum AuthenticationType
    {
        User,
        Default
    }
}
