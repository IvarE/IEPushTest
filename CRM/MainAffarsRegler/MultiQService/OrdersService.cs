using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System.Globalization;
using Skanetrafiken.Crm.Entities;
using Quartz.Impl;
using Microsoft.Xrm.Tooling.Connector;

namespace Endeavor.Crm.MultiQService
{
    public partial class OrdersService : ServiceBase
    {
        private static ILog _log = LogManager.GetLogger(typeof(OrdersService));
        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("OrdersService");
        private IScheduler _quartzScheduler;

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }

        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[ReportUploader.DataMapModifiedAfter] = DateTime.Now;

            _log.Info($"Scheduling UploadJob");

            IJobDetail scheduleUploadJob = JobBuilder.Create<ReportUploader>()
            .WithIdentity(ReportUploader.JobName, ReportUploader.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(ReportUploader.JobDescription)
            .Build();

            ITrigger scheduleUploadTrigger = TriggerBuilder.Create()
                  .WithIdentity(ReportUploader.TriggerName, ReportUploader.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.FileUploadScheduleCronExpression)
                  .WithDescription(ReportUploader.TriggerDescription)
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

        public OrdersService()
        {
            InitializeComponent();
        }

        public OrdersService(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
