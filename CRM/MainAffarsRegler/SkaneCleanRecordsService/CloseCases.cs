﻿using System;
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

                string runFullData = ConfigurationManager.AppSettings["runFullData"];

                if (runFullData == "true")
                {
                    _log.Debug($"This is a Production Run.");
                    RunCloseCasesProduction(localContext);
                }
                else if (runFullData == "false")
                {
                    _log.Debug($"This is a Test Run.");
                    RunCloseCasesTest(localContext);
                }
                else
                {
                    _log.Debug($"The App Setting 'runFullData' is neither 'true' or 'false'.");
                }

                _log.Info($"CloseCases Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in CloseCases.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public static void RunCloseCasesProduction(Plugin.LocalPluginContext localContext)
        {
            QueryExpression queryCases = new QueryExpression(IncidentEntity.EntityLogicalName);
            queryCases.ColumnSet = new ColumnSet(IncidentEntity.Fields.TicketNumber);
            queryCases.NoLock = true;
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.StateCode, ConditionOperator.Equal, (int)IncidentState.Active);
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.IncidentStageCode, ConditionOperator.Equal, (int)incident_incidentstagecode.NotAnswered);
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.OlderThanXYears, 1); //Cases Older than 1 Year

            #if DEBUG
            queryCases.ColumnSet.AddColumns(IncidentEntity.Fields.StateCode, IncidentEntity.Fields.IncidentStageCode, IncidentEntity.Fields.CreatedOn);
            #endif

            List<IncidentEntity> lUnansweredCases = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, queryCases);
            _log.Info($"Found " + lUnansweredCases.Count + " unanswered Cases.");

            List<OrganizationRequest> requestsList = new List<OrganizationRequest>();
            foreach (IncidentEntity incident in lUnansweredCases)
            {
                #if DEBUG
                _log.Debug($"Processing Incident: {incident.TicketNumber} CreatedOn: {incident.CreatedOn} StateCode: {incident.StateCode} IncidentStageCode: {incident.IncidentStageCode}");
                #endif
                try
                {
                    UpdateRequest updateCase = new UpdateRequest()
                    {
                        Target = new IncidentEntity()
                        {
                            IncidentId = incident.Id,
                            IncidentStageCode = incident_incidentstagecode.Resolved
                        }
                    };

                    CloseIncidentRequest closeIncidentRequest = new CloseIncidentRequest
                    {
                        IncidentResolution = new IncidentResolution
                        {
                            Subject = "Resolved Incident " + incident.Id,
                            IncidentId = new EntityReference(Incident.EntityLogicalName, incident.Id)
                        },
                        Status = new OptionSetValue((int)incident_statuscode.ProblemSolved)
                    };

                    requestsList.Add(new ExecuteTransactionRequest
                    {
                        Requests = new OrganizationRequestCollection() { updateCase, closeIncidentRequest },
                        ReturnResponses = false
                    });

                    _log.InfoFormat($"Incident: {incident.TicketNumber} added to Request Batch to be Resolved.");
                }
                catch (Exception e)
                {
                    _log.Error($"Exception caught in Close Case {incident.Id}\n{e.Message}\n\n");
                }
            }

            
            Helper.ExecuteMultipleRequests(localContext, requestsList, true, true, 100);
        }

        public static void RunCloseCasesTest(Plugin.LocalPluginContext localContext)
        {
            bool runSpecificDate = true;

            string startDate = Properties.Settings.Default.StartDate;
            string endDate = Properties.Settings.Default.EndDate;

            string specificDate = Properties.Settings.Default.SpecificDate;

            QueryExpression queryCases = new QueryExpression(IncidentEntity.EntityLogicalName);
            queryCases.NoLock = true;
            queryCases.ColumnSet = new ColumnSet(IncidentEntity.Fields.StateCode, IncidentEntity.Fields.IncidentStageCode, Incident.Fields.CreatedOn, IncidentEntity.Fields.TicketNumber);

            if (runSpecificDate)
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.On, specificDate);
            else
            {
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.OnOrAfter, startDate);
                queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.OnOrBefore, endDate);
            }

            List<IncidentEntity> lCases = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, queryCases);

            string casesFound = $"Found " + lCases.Count + " Cases";

            if (runSpecificDate)
                casesFound += $" on {specificDate}";
            else
                casesFound += "between " + startDate + " and " + endDate + ".";

            _log.Debug(casesFound);

            int unansweredCases = 0;
            List<OrganizationRequest> requestsLst = new List<OrganizationRequest>();

            foreach (IncidentEntity incident in lCases)
            {
                try
                {
                    if (incident.StateCode.Value != IncidentState.Active || incident.IncidentStageCode.Value != incident_incidentstagecode.NotAnswered ||
                        (DateTime.Now - incident.CreatedOn.Value).TotalDays < 365)
                    {
                        _log.Debug($"Irrelevant Incident: {incident.TicketNumber} CreatedOn: {incident.CreatedOn} StateCode: {incident.StateCode} IncidentStageCode: {incident.IncidentStageCode}");
                        continue;
                    }

                    unansweredCases++;
                    _log.Debug($"Processing Incident: {incident.TicketNumber} CreatedOn: {incident.CreatedOn} StateCode: {incident.StateCode} IncidentStageCode: {incident.IncidentStageCode}");

                    IncidentEntity nIncident = new IncidentEntity();
                    nIncident.Id = incident.Id;
                    nIncident.IncidentStageCode = incident_incidentstagecode.Resolved;

                    UpdateRequest updateCase = new UpdateRequest()
                    {
                        Target = nIncident
                    };

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

                    requestsLst.Add(updateCase);
                    requestsLst.Add(closeIncidentRequest);

                    _log.InfoFormat($"Incident: {incident.TicketNumber} added to Request Batch to be Resolved.");
                }
                catch (Exception e)
                {
                    _log.Error($"Exception caught in Close Case {incident.Id}\n{e.Message}\n\n");
                }
            }

            _log.Debug($"Added {unansweredCases} Cases to be Resolved.");

            Helper.ExecuteMultipleRequestsTransaction(localContext, requestsLst, true);
        }
    }
}
