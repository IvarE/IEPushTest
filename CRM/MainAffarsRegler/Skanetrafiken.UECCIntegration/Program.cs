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
using Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk.Messages;

namespace Skanetrafiken.UECCIntegration
{
    class Program
    {
        //private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ILog _log = LogManager.GetLogger("FileAppenderLog");

        public static IOrganizationService ConnectToMSCRM(string UserName, string Password, string SoapOrgServiceUri)
        {
            try
            {
                ClientCredentials credentials = new ClientCredentials();
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
                Uri serviceUri = new Uri(SoapOrgServiceUri);
                OrganizationServiceProxy proxy = new OrganizationServiceProxy(serviceUri, null, credentials, null);
                proxy.EnableProxyTypes();
                return (IOrganizationService)proxy;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "Error while connecting to CRM " + ex.Message);
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
                return null;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string domainUser = string.Empty;
                string passWord = string.Empty;
                string urlOrganization = string.Empty;

                Console.WriteLine("Split the Contacts in Production? (y)");
                string input = Console.ReadLine();

                if (input == "y")
                {
                    domainUser = ConfigurationManager.AppSettings["domainUserPROD"];
                    passWord = ConfigurationManager.AppSettings["passWordPROD"];
                    urlOrganization = ConfigurationManager.AppSettings["urlOrganizationPROD"];
                }
                else
                {
                    domainUser = ConfigurationManager.AppSettings["domainUserTST"];
                    passWord = ConfigurationManager.AppSettings["passWordTST"];
                    urlOrganization = ConfigurationManager.AppSettings["urlOrganizationTST"];
                }

                IOrganizationService _service = ConnectToMSCRM(domainUser, passWord, urlOrganization);

                if (_service == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, "The CRM Service is null.");
                    return;
                }

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());
                CrmContext crmContext = new CrmContext(_service);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"Running Logic for Split Contacts.");
                LogicHelper.RunLogic(localContext, crmContext);
            }
            catch (Exception e)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Exception Main Error. Details: " + e.Message);
                Console.WriteLine($"Exception Main Error. Details: " + e.Message);
                Console.ReadLine();
            }
        }
    }
}
