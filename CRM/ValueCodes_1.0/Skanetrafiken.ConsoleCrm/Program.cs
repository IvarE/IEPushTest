using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

using Endeavor.Crm;
using Endeavor.Crm.Extensions;

using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.ConsoleCrm
{
    class Program
    {
        private static ServerConnection.Configuration _config;
        private static ServerConnection _serverConnection;

        private static ServerConnection.Configuration Config
        {
            get 
            {
                return _config;
            }
        }

        private static ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        static void Main(string[] args)
        {
            // Setup Connection to CRM
            Setup();

            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (OrganizationServiceProxy _serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());


                IList<MergeRecordsEntity> recordsToMerge = XrmRetrieveHelper.RetrieveMultiple<MergeRecordsEntity>(localContext, new ColumnSet(true),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(MergeRecordsEntity.Fields.ed_Message, ConditionOperator.Null)
                    }
                });

                localContext.TracingService.Trace($"Found {recordsToMerge.Count} records to merge. Starting....");

                int iProcessed = 0;
                foreach (var recordToMerge in recordsToMerge)
                {
                    recordToMerge.PerformeMerge(localContext);

                    iProcessed++;
                    localContext.TracingService.Trace($"Processed {iProcessed} records.");
                }

                localContext.TracingService.Trace($"! Done ! :-)");

            }
            // Wait for user.
            Console.ReadKey();
        }

        private static void Setup()
        {
            _config = new ServerConnection.Configuration();
            _config.DiscoveryUri = new Uri(Properties.Settings.Default.DiscoveryUri);
            _config.EndpointType = (AuthenticationProviderType)Enum.Parse(typeof(AuthenticationProviderType), Properties.Settings.Default.EndpointType);
            if (!string.IsNullOrEmpty(Properties.Settings.Default.HomeRealmUri))
                _config.HomeRealmUri = new Uri(Properties.Settings.Default.HomeRealmUri);
            _config.OrganizationName = Properties.Settings.Default.OrganizationName;
            _config.OrganizationUri = new Uri(Properties.Settings.Default.OrganizationUri);
            _config.ServerAddress = Properties.Settings.Default.ServerAddress;

            // This is only for testing so I'm not using secure string.
            string password = string.Empty;
            if (!Properties.Settings.Default.UseDefaultCredential)
            {
                Console.Write(string.Format(@"Please enter password for {0}\{1}: ", Properties.Settings.Default.Domain, Properties.Settings.Default.UserName));
                password = Console.ReadLine();
            }


            if (_config.EndpointType == AuthenticationProviderType.ActiveDirectory)
            {
                _config.Credentials = new ClientCredentials();
                if (Properties.Settings.Default.UseDefaultCredential)
                {
                    _config.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    _config.Credentials.Windows.ClientCredential = new System.Net.NetworkCredential(Properties.Settings.Default.UserName, password, Properties.Settings.Default.Domain);
                }
                
            }
            else if (_config.EndpointType == AuthenticationProviderType.Federation)
            {
                _config.Credentials = new ClientCredentials();
                if (Properties.Settings.Default.UseDefaultCredential)
                {
                    _config.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    _config.Credentials.UserName.UserName = Properties.Settings.Default.UserName;
                    _config.Credentials.UserName.Password = password;
                }
            }
            else if (_config.EndpointType == AuthenticationProviderType.OnlineFederation)
            {
                _config.Credentials = new ClientCredentials();
                if (Properties.Settings.Default.UseDefaultCredential)
                {
                    _config.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    _config.Credentials.UserName.UserName = Properties.Settings.Default.UserName;
                    _config.Credentials.UserName.Password = password;
                }
            }
            else
                throw new NotImplementedException(string.Format("AuthenticationProviderType {0} is supported for now.", _config.EndpointType.ToString()));

            Console.WriteLine(string.Format("Connecting to:{0}", _config.OrganizationUri));
        }

    }
}
