using System;
using Quartz;
using System.Collections.Generic;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;

using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;

namespace Endeavor.Crm.CloseCasesService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class CloseCases : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Close Cases Schedule";
        public const string TriggerDescription = "CloseCases Schedule Trigger";
        public const string JobDescription = "CloseCases Schedule Job";
        public const string TriggerName = "CloseCasesTrigger";
        public const string JobName = "CloseCases";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.StateCode, ConditionOperator.Equal, (int)IncidentState.Active);
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.IncidentStageCode, ConditionOperator.Equal, (int)incident_incidentstagecode.NotAnswered);
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.OlderThanXYears, 1); //Cases Older than 1 Year

                List<IncidentEntity> lUnansweredCases = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, queryCases);
                _log.Info($"Found " + lUnansweredCases.Count + " Unanswered Cases.");

                foreach (IncidentEntity incident in lUnansweredCases)
                {
                    try
                    {
                        IncidentEntity nIncident = new IncidentEntity();
                        nIncident.Id = incident.Id;
                        nIncident.IncidentStageCode = incident_incidentstagecode.Resolved;

                        XrmHelper.Update(localContext, nIncident);
                        _log.Info($"Incident {incident.Id} was updated with StageCode Resolved.");

                        IncidentResolution incidentResolution = new IncidentResolution
                        {
                            Subject = "Resolved Incident " + incident.Id,
                            IncidentId = new EntityReference(Incident.EntityLogicalName, incident.Id)
                        };

                        CloseIncidentRequest closeIncidentRequest = new CloseIncidentRequest
                        {
                            IncidentResolution = incidentResolution,
                            Status = new OptionSetValue((int)incident_statuscode.ProblemSolved)
                        };

                        localContext.OrganizationService.Execute(closeIncidentRequest);
                        _log.Info($"Unanswered Incident {incident.Id} closed with Status 'Problem Solved'.");
                    }
                    catch (Exception e)
                    {
                        _log.Error($"Exception caught in Close Case {incident.Id}\n{e.Message}\n\n");
                    }
                }

                _log.Info($"CloseCases Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in CloseCases.ExecuteJob():\n{e.Message}\n\n");
            }
        }
    }
}
