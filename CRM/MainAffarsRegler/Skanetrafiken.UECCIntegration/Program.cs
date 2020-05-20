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
        public static IOrganizationService _service = null;

        //private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ILog _log = LogManager.GetLogger("FileAppenderLog");

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

                ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
                {
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = true,
                        ReturnResponses = true
                    },
                    Requests = new OrganizationRequestCollection()
                };

                ed_CompanyRole uCompanyRole = new ed_CompanyRole();
                uCompanyRole.Id = new Guid("1038d3bb-bb9a-ea11-80f8-005056b61fff");
                uCompanyRole.ed_Contact = new EntityReference(Contact.EntityLogicalName)
                {
                    KeyAttributes = new KeyAttributeCollection
                    {
                        {Contact.Fields.EMailAddress1, "vahidaz2@msn.com"}
                    }
                };

                UpdateRequest updateRequest = new UpdateRequest { Target = uCompanyRole };
                requestWithResults.Requests.Add(updateRequest);

                ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                LogicHelper.LogExecuteMultipleResponses(requestWithResults, responseWithResults); //TODO TESTAR ISTO - PERGUNTAR SE POSSO CRIAR UMA KEY NA ENTIDADE CONTACT
                //bool firstRun = true; //Change this flag to run for the remaining Contacts that throw errors

                //if(firstRun)
                //    LogicHelper.RunLogic(localContext);
                //else
                //    LogicHelper.RunErrorContacts(localContext);
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
