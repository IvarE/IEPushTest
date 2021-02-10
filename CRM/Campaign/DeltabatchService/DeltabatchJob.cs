using Common.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using System;
using System.IO;
using Tamir.SharpSsh;
using System.Xml.Linq;
using Endeavor.Crm.UnitTest;

namespace Endeavor.Crm.DeltabatchService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class DeltabatchJob : IJob
    {
        public const string JobDescription = "Schedule Job";
        public const string TriggerName = "ScheduleTrigger1";
        public const string TriggerDescription = "Schedule Trigger";
        public const string GroupName = "Schedule";
        public const string DataMapModifiedAfter = "ModifiedAfter";
        
        protected Sftp sftp;

        protected ILog _log;
        public static readonly ColumnSet deltabatchQueueColumnSet = new ColumnSet(
               DeltabatchQueueEntity.Fields.ed_ContactNumber,
               DeltabatchQueueEntity.Fields.ed_DeltabatchOperation,
               DeltabatchQueueEntity.Fields.ed_DeltabatchQueueId,
               DeltabatchQueueEntity.Fields.ed_name,
               DeltabatchQueueEntity.Fields.CreatedOn
            );

        public void Execute(IJobExecutionContext context)
        {
            throw new Exception("Cannot execute a clean DeltabatchJob. Please instantiate an inherited class");
        }

        protected Sftp CreateSftpConnectionToCreditsafe()
        {
            string username = null, password = null;
            string credentialFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Endeavor\\CreditsafeCredential.xml");
            if (File.Exists(credentialFile))
            {
                XDocument doc = XDocument.Load(credentialFile);
                username = doc.Root.Element("Username").Value;
                password = Configuration.ToInsecureString(Configuration.DecryptString(doc.Root.Element("Password").Value));

                return new Sftp(Properties.Settings.Default.CreditsafeIP, username, password);
            }
            else
            {
                throw new Exception($"Could not find Credentials file at: {credentialFile}. Please create with Endeavor NUnit Credentials Manager");
            }
        }

        protected Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(DeltabatchService.CredentialFilePath, DeltabatchService.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }
    }
}
