using System;
using System.Configuration;
using System.Globalization;

using log4net;
using Endeavor.Crm;
using Endeavor.Crm.UECCIntegration;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

using Skanetrafiken.Crm.Entities;
using Skanetrafiken.UECCIntegration.Logic;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;

namespace Skanetrafiken.UECCIntegration
{
    class Program
    {
        public static IOrganizationService _service = null;

        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("BiffigasteIntegrationen");

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        public static void ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                _service = (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "Error while connecting to CRM " + ex.Message);
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
            }
        }

        private static Plugin.LocalPluginContext GetCrmConnection()
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, "Building CRM connection string");
            //_log.InfoFormat(CultureInfo.InvariantCulture, $"Found connectionstring {CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy)}");

            // Connect to the CRM web service using a connection string.
            //CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            CrmServiceClient conn = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CrmConnection"].ConnectionString);
            _log.InfoFormat(CultureInfo.InvariantCulture, $"Service client created, Ready:{conn.IsReady}");

            if (conn.IsReady == false)
                throw new Exception("Failed to connect to Microsoft CRM. IsReady = false");

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            // Obtain information about the logged on user from the web service.
            {
                Guid userId = ((WhoAmIResponse)serviceProxy.Execute(new WhoAmIRequest())).UserId;

                SystemUserEntity systemUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(
                    localContext,
                    userId,
                    new ColumnSet(
                        SystemUserEntity.Fields.FullName));

                _log.InfoFormat(CultureInfo.InvariantCulture, "The logged on user is \"{0}\" with id \"{1}\".", systemUser.FullName, userId.ToString());
            }

            // Retrieve the version of Microsoft Dynamics CRM.
            {
                RetrieveVersionRequest versionRequest = new RetrieveVersionRequest();
                RetrieveVersionResponse versionResponse = (RetrieveVersionResponse)serviceProxy.Execute(versionRequest);

                _log.InfoFormat(CultureInfo.InvariantCulture, "Microsoft Dynamics CRM version \"{0}\".", versionResponse.Version);
            }

            return localContext;
        }

        static void Main(string[] args)
        {
            try
            {
                ConnectToMSCRM("D1\\CRMAdmin", "uSEme2!nstal1", "https://sekundtst.skanetrafiken.se/DKCRM/XRMServices/2011/Organization.svc");

                if (_service == null)
                    _log.ErrorFormat(CultureInfo.InvariantCulture, "The CRM Service is null.");

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());
                LogicHelper.RunLogic(localContext);
            }
            catch (Exception e)
            {
                Console.WriteLine("Global Exception: " + e.Message);
                return;
            }
        }
    }
}
