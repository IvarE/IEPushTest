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
using log4net;
using System.Reflection;

namespace Endeavor.Crm.CleanRecordsService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class DeleteQueueItems : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Delete QueueItems Schedule";
        public const string TriggerDescription = "DeleteQueueItems Schedule Trigger";
        public const string JobDescription = "DeleteQueueItems Schedule Job";
        public const string TriggerName = "DeleteQueueItemsTrigger";
        public const string JobName = "Delete QueueItems";

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


                var deletionDateThreshold = DateTime.Now.Subtract(TimeSpan.FromDays(365));

                var queryExpression = new QueryExpression("queueitem")
                {
                    ColumnSet = new ColumnSet(new String[] { "createdon" }),
                    Criteria = new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                    {
                        new ConditionExpression("createdon", ConditionOperator.LessThan, deletionDateThreshold)
                    }
                    }
                };

                var entityList = XrmRetrieveHelper.RetrieveMultiple<QueueItemEntity>(localContext, queryExpression);
                var listEntityReferences = entityList.Select(entity => new EntityReference("queueitem", entity.GetAttributeValue<Guid>("queueitemid"))).ToList();

                var deleteRequests = listEntityReferences.Select((reference, index) => (OrganizationRequest)new DeleteRequest { Target = reference }).ToList();

                var batchSize = 500;

                foreach (var batch in SplitIntoBatches(deleteRequests, batchSize))
                {
                    var response = Helper.ExecuteMultipleRequests(localContext, batch, true, true, 100);
                    HandleBatchResponse(response);
                }


            }
            catch (Exception e) 
            {
                _log.Error($"Exception caught in DeleteQueueItems.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        private static IEnumerable<List<T>> SplitIntoBatches<T>(List<T> source, int batchSize)
        {
            for (int i = 0; i < source.Count; i += batchSize)
            {
                yield return source.GetRange(i, Math.Min(batchSize, source.Count - i));
            }
        }

        private static void HandleBatchResponse(ExecuteMultipleResponse response)
        {
            foreach (var result in response.Responses)
            {
                if (result.Fault != null)
                {
                    _log.Error($"Error in batch response: {result.Fault.Message}");
                }
                else
                {
                    _log.Info("Batch request processed successfully.");
                }
            }
        }
    }
}

