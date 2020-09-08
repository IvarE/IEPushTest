using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using Endeavor.Crm;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;

using Skanetrafiken.CloseOldCasesService;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;

namespace Skanetrafiken.CloseOldCasesService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class CaseClosing : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Case Closing Schedule";
        public const string TriggerDescription = "Case Closing Schedule Trigger";
        public const string JobDescription = "Case Closing Schedule Job";
        public const string TriggerName = "CaseClosingTrigger";
        public const string JobName = "CaseClosing";

        private ILog _log = LogManager.GetLogger(typeof(CaseClosing));

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(CasesService.CredentialFilePath, CasesService.Entropy));

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

                QueryExpression queryCases = new QueryExpression(IncidentEntity.EntityLogicalName);
                queryCases.NoLock = true;
                queryCases.ColumnSet.AddColumns(IncidentEntity.Fields.CreatedOn);

                List<IncidentEntity> lCases = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, queryCases);
                _log.Info($"Found " + lCases.Count + " Cases that are Old and Unaswered.");


                foreach (IncidentEntity entityCase in lCases)
                {
                    IncidentEntity uCase = new IncidentEntity();
                    uCase.Id = entityCase.Id;
                    uCase.StateCode = Crm.Schema.Generated.IncidentState.Canceled;
                    uCase.StatusCode = new OptionSetValue(2); //COmpleted---??

                    XrmHelper.Update(localContext, uCase);
                }

                _log.Info($"Case Closing Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in CaseClosing.ExecuteJob():\n{e.Message}\n\n");
            }
        }
    }
}
