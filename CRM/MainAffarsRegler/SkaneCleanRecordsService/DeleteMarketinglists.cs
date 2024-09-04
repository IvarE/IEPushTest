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
    public class DeleteMarketinglists : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Delete Marketing Lists Schedule";
        public const string TriggerDescription = "DeleteMarketinglists Schedule Trigger";
        public const string JobDescription = "DeleteMarketinglists Schedule Job";
        public const string TriggerName = "DeleteMarketinglistsTrigger";
        public const string JobName = "Delete Marketing Lists";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(AuditsService.CredentialFilePath, AuditsService.Entropy));
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
            _log.Info($"DeleteMarketingLists Starting");

            try
            {
                localContext = GenerateLocalContext();

                if (localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }


                var deletionThresholdMonths = 3;
                var deletionDateThreshold = DateTime.Now.Subtract(TimeSpan.FromDays(deletionThresholdMonths * 30));

                var queryExpression = new QueryExpression("list")
                {
                    ColumnSet = new ColumnSet(new String[] { "modifiedon", "status" }),
                    Criteria = new FilterExpression(LogicalOperator.And)
                    {
                        Conditions =
                    {
                        new ConditionExpression("statuscode", ConditionOperator.Equal, 1),
                        new ConditionExpression("modifiedon", ConditionOperator.LessThan, deletionDateThreshold)
                    }
                    }
                };

                var entityList = XrmRetrieveHelper.RetrieveMultiple<MarketingListEntity>(localContext, queryExpression);
                var listEntityReferences = entityList.Select(entity => new EntityReference("list", entity.GetAttributeValue<Guid>("listid"))).ToList();

                var deleteRequests = listEntityReferences.Select((reference, index) => (OrganizationRequest)new DeleteRequest { Target = reference }).ToList();

                Helper.ExecuteMultipleRequests(localContext, deleteRequests, true, true, 100);

                _log.Info($"DeleteMarketingLists Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in DeleteMarketingLists.ExecuteJob():\n{e.Message}\n\n");
            }
        }
    }
}
