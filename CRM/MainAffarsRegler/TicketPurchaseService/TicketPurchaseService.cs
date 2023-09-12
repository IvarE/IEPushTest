using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Quartz.Impl;
using Skanetrafiken.Crm.Entities;
using System;
using System.Globalization;
using System.ServiceProcess;
using Common.Logging;

namespace TicketPurchaseService
{
    public partial class TicketPurchaseService : ServiceBase
    {

        private static ILog _log = LogManager.GetLogger(typeof(TicketPurchaseService));
        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("Pepper could be a password if used correctly");
        private IScheduler _quartzScheduler;

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        public TicketPurchaseService()
        {
            // Test to make sure we have connection to CRM.


            InitializeComponent();
        }

        public void Execute()
        {
            // Test to make sure we have connection to CRM.
            TestCrmConnection();
            // Initialize scheduling engine.
            InitializeQuartzEngine();
        }

        protected override void OnStart(string[] args)
        {
            _log.Info(this.ServiceName);

            Execute();
        }

        private void TestCrmConnection()
        {
            _log.Info("Test CRM Connection");

            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));

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

                _log.InfoFormat(CultureInfo.InvariantCulture, $"The logged on user is \"{systemUser.FullName}\" with id \"{userId.ToString()}\".");
            }

            // Retrieve the version of Microsoft Dynamics CRM.
            {
                //_log.Debug($"Pre versionRequest");
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

        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[PopulateTPJob.DataMapModifiedAfter] = DateTime.Now;

            _log.Info($"Scheduling UploadJob");

            IJobDetail scheduleJob = JobBuilder.Create<PopulateTPJob>()
            .WithIdentity(PopulateTPJob.JobName, PopulateTPJob.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(PopulateTPJob.JobDescription)
            .Build();

            ITrigger scheduleTrigger = TriggerBuilder.Create()
                  .WithIdentity(PopulateTPJob.TriggerName, PopulateTPJob.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.ScheduleCronExpression)
                  .WithDescription(PopulateTPJob.TriggerDescription)
                  .ForJob(scheduleJob)
                  .Build();

            scheduler.ScheduleJob(scheduleJob, scheduleTrigger);

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
