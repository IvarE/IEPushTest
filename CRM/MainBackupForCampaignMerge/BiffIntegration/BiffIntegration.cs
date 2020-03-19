using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using Quartz.Impl;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;

namespace Endeavor.Crm.BiffIntegration
{
    public partial class BiffIntegration : ServiceBase
    {
        private static ILog _log = LogManager.GetLogger(typeof(BiffIntegration));
        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("BiffigasteIntegrationen");
        private IScheduler _quartzScheduler;

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        internal static string CredentialSQLFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePathSQLPassword);
            }
        }

        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[ScheduleJob.DataMapModifiedAfter] = DateTime.Now;

            IJobDetail scheduleJob = JobBuilder.Create<ScheduleJob>()
            .WithIdentity(ScheduleJob.JobName, ScheduleJob.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(ScheduleJob.JobDescription)
            .Build();

            ITrigger scheduleTrigger = TriggerBuilder.Create()
                  .WithIdentity(ScheduleJob.TriggerName, ScheduleJob.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.ScheduleRefreshCronExpression)
                  .WithDescription(ScheduleJob.TriggerDescription)
                  .ForJob(scheduleJob)
                  .Build();

            scheduler.ScheduleJob(scheduleJob, scheduleTrigger);
        }

        public BiffIntegration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this instance. Used for debug purposes.
        /// </summary>
        public void Execute()
        {
            // Test to make sure we have connection to CRM.
            TestCrmConnection();
            // Initialize scheduling engine.
            InitializeQuartzEngine();
        }

        private void TestCrmConnection()
        {
            _log.InfoFormat(CultureInfo.InvariantCulture, "Building CRM connection string");

            //_log.InfoFormat(CultureInfo.InvariantCulture, $"Found connectionstring {CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy)}");


            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));

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
        }


        private void InitializeQuartzEngine()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _quartzScheduler = sf.GetScheduler();
            _log.Info(Properties.Resources.QuartzInitializationComplete);

            InitializeScheduler(_quartzScheduler);

            _quartzScheduler.Start();
            _log.Info(Properties.Resources.QuartzSchedulerStarted);
        }

        protected override void OnStart(string[] args)
        {
            _log.Info(this.ServiceName);
#if !DEBUG
            //Execute is called from Main in debug.
            Execute();
#endif
        }

        protected override void OnStop()
        {
            if (_quartzScheduler != null)
            {
                // shut down the scheduler
                _log.Info(Properties.Resources.QuartzShuttingDown);
                _quartzScheduler.Shutdown(true);
                _log.Info(Properties.Resources.QuartzShuttingDownCompleted);
            }
        }
    }
}
