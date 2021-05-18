using System;
using Quartz;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk.Messages;
using System.Linq;
using System.ServiceModel;
using System.Configuration;

namespace Endeavor.Crm.CleanRecordsService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class DeleteAudits : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Delete Audits Schedule";
        public const string TriggerDescription = "DeleteAudits Schedule Trigger";
        public const string JobDescription = "DeleteAudits Schedule Job";
        public const string TriggerName = "DeleteAuditsTrigger";
        public const string JobName = "Delete Audits";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(AuditsService.CredentialFilePath, AuditsService.Entropy));

            // Cast the proxy client to the IOrganizationService interface.
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());

            return localContext;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));

            ExecuteJob();

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        public void ExecuteJob()
        {
            Plugin.LocalPluginContext localContext = null;

            try
            {
                localContext = GenerateLocalContext();

                if (localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }

                // Get the list of audit partitions.
                RetrieveAuditPartitionListResponse partitionRequest = (RetrieveAuditPartitionListResponse)localContext.OrganizationService.Execute(new RetrieveAuditPartitionListRequest());
                AuditPartitionDetailCollection partitions = partitionRequest.AuditPartitionDetailCollection;

                // Create a delete request with an end date earlier than possible.
                DeleteAuditDataRequest deleteRequest = new DeleteAuditDataRequest();
                deleteRequest.EndDate = DateTime.MinValue; // new DateTime(2000, 1, 1);

                DateTime dt6MonthsAgo = DateTime.Now.AddMonths(-6);

                // Check if partitions are not supported as is the case with SQL Server Standard edition.
                if (partitions.IsLogicalCollection)
                {
                    // Delete all audit records created up until now.
                    deleteRequest.EndDate = dt6MonthsAgo;
                }

                // Otherwise, delete all partitions that are older than the current partition.
                // Hint: The partitions in the collection are returned in sorted order where the 
                // partition with the oldest end date is at index 0.
                else
                {
                    for (int n = partitions.Count - 1; n >= 0; --n)
                    {
                        if (partitions[n].EndDate < dt6MonthsAgo && partitions[n].EndDate > deleteRequest.EndDate)
                        {
                            deleteRequest.EndDate = (DateTime)partitions[n].EndDate;
                            break;
                        }
                    }
                }

                // Delete the audit records.
                if (deleteRequest.EndDate != DateTime.MinValue)
                {
                    localContext.OrganizationService.Execute(deleteRequest);
                    _log.Info("Audit records have been deleted.");
                }
                else
                    _log.Info("There were no audit records that could be deleted.");

                _log.Info($"DeleteAudits Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in DeleteAudits.ExecuteJob():\n{e.Message}\n\n");
            }
        }
    }
}
