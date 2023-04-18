using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Quartz.Impl;
using Skanetrafiken.Crm.Entities;
using SR.Generated;
using System;
using System.ComponentModel;
using System.Globalization;
using System.ServiceProcess;

namespace Endeavor.Crm.CleanRecordsService.PandoraExtensions
{
    public partial class PermitsService : ServiceBase
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IScheduler _quartzScheduler;

        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes(nameof(PermitsService));

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePathPermits);
            }
        }

        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[InactivatePermits.DataMapModifiedAfter] = DateTime.Now;

            _log.Info($"Scheduling Inactivate Permits Job");

            IJobDetail scheduleUploadJob = JobBuilder.Create<InactivatePermits>()
                .WithIdentity(InactivatePermits.JobName, InactivatePermits.GroupName)
                .UsingJobData(jobDataMap)
                .WithDescription(InactivatePermits.JobDescription)
                .Build();

            ITrigger scheduleUploadTrigger = TriggerBuilder.Create()
                .WithIdentity(InactivatePermits.TriggerName, InactivatePermits.GroupName)
                .WithCronSchedule(Properties.Settings.Default.InactivatePermitsCronExpression)
                .WithDescription(InactivatePermits.TriggerDescription)
                .ForJob(scheduleUploadJob)
                .Build();

            scheduler.ScheduleJob(scheduleUploadJob, scheduleUploadTrigger);
        }

        /// <summary>
        /// Executes this instance. Used for debug purposes.
        /// </summary>
        public void Execute()
        {
            TestCrmConnection();
            InitializeQuartzEngine();
        }

        private void TestCrmConnection()
        {
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));
            IOrganizationService serviceProxy = conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : conn.OrganizationServiceProxy;
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
            _log.Info(ServiceName);

#if !DEBUG
            //Execute is called from Main in debug.
            Execute();
#endif
        }

        protected override void OnStop()
        {
            if (_quartzScheduler != null)
            {
                _log.Info(Properties.Resources.QuartzShuttingDown);
                _quartzScheduler.Shutdown(true);
                _log.Info(Properties.Resources.QuartzShuttingDownCompleted);
            }
        }

        public PermitsService()
        {
            InitializeComponent();
        }

        public PermitsService(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
