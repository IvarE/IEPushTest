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

        public static void RunAutoNumberLogic()
        {
            string domainV9User = ConfigurationManager.AppSettings["domainV9User"];
            string urlV9Organization = ConfigurationManager.AppSettings["urlV9Organization"];

            Console.WriteLine("Please introduce the password: ");
            string passWordV9 = Console.ReadLine();

            IOrganizationService _service = ConnectToMSCRM(domainV9User, passWordV9, urlV9Organization);

            if (_service == null)
            {
                _log.ErrorFormat(CultureInfo.InvariantCulture, "The CRM Service is null.");
                return;
            }

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _service, null, new TracingService());

            // QueryExpression to Retrieve Organization
            QueryExpression qe = new QueryExpression("organization")
            {
                ColumnSet = new ColumnSet(new string[] { "organizationid", "name", "currentordernumber", "currentkbnumber" })
            };
            EntityCollection orgs = localContext.OrganizationService.RetrieveMultiple(qe);

            if (orgs != null && orgs.Entities.Count > 0)
            {
                var org = orgs[0];
                var organizationId = (Guid)org["organizationid"];

                // Creating a new Object to update. Set the CurrentKbNumber to your desired number
                //Define the seed 
                SetAutoNumberSeedRequest req = new SetAutoNumberSeedRequest();
                req.EntityName = "salesorder";
                req.AttributeName = "ordernumber";
                req.Value = long.Parse(ConfigurationManager.AppSettings["autoNumber"]);
                localContext.OrganizationService.Execute(req);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                string domainUser = string.Empty;
                string passWord = string.Empty;
                string urlOrganization = string.Empty;

                Console.WriteLine("Fix AutoNumber Starting Point? (y)");
                string input = Console.ReadLine();

                if(input == "y")
                {
                    RunAutoNumberLogic();
                    return;
                }

                Console.WriteLine("End of AutoNumber");
                Console.WriteLine("Split the Contacts in Production? (y)");
                input = Console.ReadLine();

                if (input == "y")
                {
                    domainUser = ConfigurationManager.AppSettings["domainUserPROD"];
                    urlOrganization = ConfigurationManager.AppSettings["urlOrganizationPROD"];
                }
                else
                {
                    domainUser = ConfigurationManager.AppSettings["domainUserTST"];
                    urlOrganization = ConfigurationManager.AppSettings["urlOrganizationTST"];
                }

                Console.WriteLine("Please introduce the password: ");
                passWord = Console.ReadLine();

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
