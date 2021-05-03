using System;
using System.Configuration;
using System.Globalization;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;

using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace Skanetrafiken.Crm.ConsoleSDKCreations
{
    class Program
    {
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
                Console.WriteLine("Error while connecting to CRM " + ex.Message);
                return null;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                // THIS SHOULD ONLY BE EXECUTED ONCE TO CREATE FIELDS/ELEMENTS THAT YOU WANT
                string domainUser = string.Empty;
                string passWord = string.Empty;
                string urlOrganization = string.Empty;

                domainUser = ConfigurationManager.AppSettings["domainUserTST"];
                urlOrganization = ConfigurationManager.AppSettings["urlOrganizationTST"];

                Console.WriteLine("Please introduce the password: ");
                passWord = Console.ReadLine();

                IOrganizationService _service = ConnectToMSCRM(domainUser, passWord, urlOrganization);

                if (_service == null)
                {
                    Console.WriteLine("The CRM Service is null.");
                    return;
                }
                
                CreateAttributeRequest widgetSerialNumberAttributeRequest = new CreateAttributeRequest
                {
                    EntityName = "ed_slots",
                    Attribute = new StringAttributeMetadata
                    {

                        //Define the format of the attribute
                        AutoNumberFormat = "S-{SEQNUM:10}",
                        LogicalName = "ed_slotidentifier",
                        SchemaName = "ed_SlotIdentifier",
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None),
                        MaxLength = 100, // The MaxLength defined for the string attribute must be greater than the length of the AutoNumberFormat value, that is, it should be able to fit in the generated value.
                        DisplayName = new Label("Slot ID", 1033),
                        Description = new Label("Auto Generator Slot ID", 1033)
                    }
                };
                _service.Execute(widgetSerialNumberAttributeRequest);

                SetAutoNumberSeedRequest req = new SetAutoNumberSeedRequest();
                req.EntityName = "ed_slots";
                req.AttributeName = "ed_slotidentifier";
                req.Value = 1;
                _service.Execute(req);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Main Error. Details: " + e.Message);
                Console.ReadLine();
            }
        }
    }
}
