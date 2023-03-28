using Endeavor.Crm.CleanRecordsService.PandoraExtensions.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using SR.Generated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using LocalPluginContext = Endeavor.Crm.Plugin.LocalPluginContext;

namespace Endeavor.Crm.CleanRecordsService.PandoraExtensions
{

    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class InactivatePermits : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Inactivate Permits Schedule";
        public const string TriggerDescription = "InactivatePermits Schedule Trigger";
        public const string JobDescription = "InactivatePermits Schedule Job";
        public const string TriggerName = "InactivatePermitsTrigger";
        public const string JobName = "PermitsContacts";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IHelperWrapper _helper;
        private readonly IRetrieveHelperWrapper _xrmRetrieveHelper;

        public InactivatePermits()
        {
            _helper = new HelperWrapper();
            _xrmRetrieveHelper = new RetrieveHelperWrapper();
        }

        /// <summary>
        /// ctor used by unit tests
        /// </summary>
        public InactivatePermits(IHelperWrapper helper, IRetrieveHelperWrapper retrieveHelper)
        {
            _helper = helper;
            _xrmRetrieveHelper = retrieveHelper;
        }

        public static LocalPluginContext GenerateLocalContext()
        {
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(PermitsService.CredentialFilePath, ContactsService.Entropy));
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            conn.OrganizationServiceProxy?.EnableProxyTypes();
            
            LocalPluginContext localContext = new LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());
            return localContext;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));

            JobDataMap dataMap = context.JobDetail.JobDataMap;

            DateTime modifiedAfter = dataMap.GetDateTime(DataMapModifiedAfter);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL", modifiedAfter.ToString() ?? "NULL"));

            ExecuteJob(context.FireTimeUtc?.UtcDateTime);

            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name, modifiedAfter.ToString()));
        }

        public void ExecuteJob(DateTime? jobExecutionTime)
        {
            try
            {
                LocalPluginContext localContext = GenerateLocalContext();
                if (localContext is null)
                {
                    _log.Error($"Unable to create a local plugin context in {nameof(InactivatePermits)}.{nameof(ExecuteJob)}.");
                    return;
                }

                bool debug = ConfigurationManager.AppSettings["runFullData"] == "true";
                var response = RunInactivatePermits(localContext, jobExecutionTime ?? DateTime.UtcNow, debug);
                if (response != null && response.IsFaulted)
                {
                    _log.Error(string.Join("; ", response.Responses.ToList().FindAll(x => x.Fault != null).Select(x => x.Fault.Message)));
                }
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in {nameof(InactivatePermits)}.{nameof(ExecuteJob)}:\n{e.Message}\n\n");
            }
        }

        public ExecuteMultipleResponse RunInactivatePermits(LocalPluginContext localContext, DateTime beforeDate, bool debug)
        {
            QueryExpression inactivePermitsQuery = GetExpiredPermitsQuery(beforeDate, debug);
            List<abssr_permit> permits = _xrmRetrieveHelper.RetrieveMultiple<abssr_permit>(localContext, inactivePermitsQuery);

            List<OrganizationRequest> inactivationRequests = new List<OrganizationRequest>(permits.Count);
            foreach (var permit in permits)
            {
                if (permit.abssr_Todate is null) { continue; }

                inactivationRequests.Add(new UpdateRequest
                {
                    Target = new abssr_permit
                    {
                        Id = permit.Id,
                        StateCode = abssr_permitState.Inactive,
                        StatusCode = abssr_permit_StatusCode.Inactive
                    }
                });
            }

            return _helper.ExecuteMultipleRequests(localContext, inactivationRequests, true, true);
        }

        private QueryExpression GetExpiredPermitsQuery(DateTime beforeDate, bool debug)
        {
            _log.Info($"Query for permits with to-date before {beforeDate}.");

            var query = new QueryExpression(abssr_permit.EntityLogicalName);
            query.Criteria.AddCondition(abssr_permit.Fields.abssr_Todate, ConditionOperator.OnOrBefore, beforeDate);
            query.Criteria.AddCondition(abssr_permit.Fields.StateCode, ConditionOperator.Equal, (int)abssr_permitState.Active);
            
            query.Distinct = true;
            query.ColumnSet = new ColumnSet(new string[] { abssr_permit.Fields.Id });

            return query;
        }
    }
}
