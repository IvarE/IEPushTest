﻿using System;
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

namespace Endeavor.Crm.DeltabatchService
{
    public partial class DeltabatchService : ServiceBase
    {
        private static ILog _log = LogManager.GetLogger(typeof(DeltabatchService));
        // INFO: (hest) The entropy should be unique for each application. DON'T COPY THIS VALUE INTO A NEW PROJECT!!!!
        internal static byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("Salt could be a password if used correctly");
        private IScheduler _quartzScheduler;

        internal static string CredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CredentialsFilePath);
            }
        }
        internal static string CreditsafeCredentialFilePath
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CreditsafeCredentialsFilePath);
            }
        }
        internal static string CreditsafeUsername
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CreditsafeLoginUsername);
            }
        }
        internal static string CreditsafeIP
        {
            get
            {
                return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.CreditsafeIP);
            }
        }


        public static void InitializeScheduler(IScheduler scheduler)
        {
            JobDataMap jobDataMap = new JobDataMap();
            jobDataMap[UploadJob.DataMapModifiedAfter] = DateTime.Now;

            _log.Info($"Scheduling UploadJob");

            IJobDetail scheduleUploadJob = JobBuilder.Create<UploadJob>()
            .WithIdentity(UploadJob.JobName, UploadJob.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(UploadJob.JobDescription)
            .Build();

            ITrigger scheduleUploadTrigger = TriggerBuilder.Create()
                  .WithIdentity(UploadJob.TriggerName, UploadJob.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.FileUploadScheduleCronExpression)
                  .WithDescription(UploadJob.TriggerDescription)
                  .ForJob(scheduleUploadJob)
                  .Build();

            scheduler.ScheduleJob(scheduleUploadJob, scheduleUploadTrigger);

            _log.Info($"Scheduling DownloadJob");

            IJobDetail scheduleDownloadJob = JobBuilder.Create<DownloadJob>()
            .WithIdentity(DownloadJob.JobName, DownloadJob.GroupName)
            .UsingJobData(jobDataMap)
            .WithDescription(DownloadJob.JobDescription)
            .Build();

            ITrigger scheduleDownloadTrigger = TriggerBuilder.Create()
                  .WithIdentity(DownloadJob.TriggerName, DownloadJob.GroupName)
                  .WithCronSchedule(Properties.Settings.Default.FileDownloadScheduleCronExpression)
                  .WithDescription(DownloadJob.TriggerDescription)
                  .ForJob(scheduleDownloadJob)
                  .Build();

            scheduler.ScheduleJob(scheduleDownloadJob, scheduleDownloadTrigger);
            
        }

        public DeltabatchService()
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
            //_log.Debug($"Pre conn");
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CredentialFilePath, Entropy));

            //_log.Debug($"Pre serviceProxy");
            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            //_log.Debug($"Pre localContext");
            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            // Obtain information about the logged on user from the web service.
            {
                //_log.Debug($"Pre userId");
                Guid userId = ((WhoAmIResponse)serviceProxy.Execute(new WhoAmIRequest())).UserId;

                //_log.Debug($"Pre systemUser");
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
