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
