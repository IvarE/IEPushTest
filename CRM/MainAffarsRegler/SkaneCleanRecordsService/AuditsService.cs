using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Quartz.Impl;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.CleanRecordsService
{
    public partial class AuditsService : ServiceBase
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IScheduler _quartzScheduler;

        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("AuditsService");

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePathAudits);
            }
        }

        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[DeleteAudits.DataMapModifiedAfter] = DateTime.Now;

            _log.Info($"Scheduling UploadJob");

            IJobDetail scheduleUploadJob = JobBuilder.Create<DeleteAudits>()
            .WithIdentity(DeleteAudits.JobName, DeleteAudits.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(DeleteAudits.JobDescription)
            .Build();

            ITrigger scheduleUploadTrigger = TriggerBuilder.Create()
                  .WithIdentity(DeleteAudits.TriggerName, DeleteAudits.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.DeleteAuditScheduleCronExpression)
                  .WithDescription(DeleteAudits.TriggerDescription)
                  .ForJob(scheduleUploadJob)
                  .Build();

            scheduler.ScheduleJob(scheduleUploadJob, scheduleUploadTrigger);
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
            _log.InfoFormat(CultureInfo.InvariantCulture, "Test Connection");
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));
            _log.InfoFormat(CultureInfo.InvariantCulture, "Connection done");

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            // Obtain information about the logged on user from the web service.
            {
                Guid userId = ((WhoAmIResponse)serviceProxy.Execute(new WhoAmIRequest())).UserId;

                SystemUserEntity systemUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(
                    localContext,
                    userId,
                    new ColumnSet(SystemUserEntity.Fields.FullName));

                _log.InfoFormat(CultureInfo.InvariantCulture, $"The logged on user is \"{systemUser.FullName}\" with id \"{userId.ToString()}\".");
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


        public AuditsService()
        {
            InitializeComponent();
        }

        public AuditsService(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
