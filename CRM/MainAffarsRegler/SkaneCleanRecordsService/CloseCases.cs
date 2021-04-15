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

                bool testRun = true;
                ExecuteMultipleResponse responseWithResults = null;

                if (!testRun)
                    responseWithResults = RunCloseCasesProduction(localContext);
                else
                {
                    _log.Debug($"This is a Test Run.");
                    responseWithResults = RunCloseCasesTest(localContext);
                }

                // process the results returned in the responses
                if (responseWithResults != null && responseWithResults.IsFaulted == true)
                {
                    _log.Error(String.Join("; ", responseWithResults.Responses.ToList().FindAll(x => x.Fault != null).Select(x => x.Fault.Message)));
                }
                else if (responseWithResults != null && responseWithResults.IsFaulted == false)
                {

                }

                _log.Info($"CloseCases Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in CloseCases.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public static ExecuteMultipleResponse RunCloseCasesProduction(Plugin.LocalPluginContext localContext)
        {
            QueryExpression queryCases = new QueryExpression(IncidentEntity.EntityLogicalName);
            queryCases.NoLock = true;
            queryCases.ColumnSet = new ColumnSet(IncidentEntity.Fields.StateCode, IncidentEntity.Fields.IncidentStageCode, IncidentEntity.Fields.CreatedOn, IncidentEntity.Fields.TicketNumber);
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.StateCode, ConditionOperator.Equal, (int)IncidentState.Active);
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.IncidentStageCode, ConditionOperator.Equal, (int)incident_incidentstagecode.NotAnswered);
            queryCases.Criteria.AddCondition(IncidentEntity.Fields.CreatedOn, ConditionOperator.OlderThanXYears, 1); //Cases Older than 1 Year

            List<IncidentEntity> lUnansweredCases = XrmRetrieveHelper.RetrieveMultiple<IncidentEntity>(localContext, queryCases);
            _log.Info($"Found " + lUnansweredCases.Count + " Unanswered Cases.");

            List<OrganizationRequest> requestsLst = new List<OrganizationRequest>();

            foreach (IncidentEntity incident in lUnansweredCases)
            {
                _log.Debug($"Processing Incident: {incident.TicketNumber} CreatedOn: {incident.CreatedOn} StateCode: {incident.StateCode} IncidentStageCode: {incident.IncidentStageCode}");
                try
                {
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

            return ExecuteMultipleRequests(localContext, requestsLst, true, true);
        }

        public static ExecuteMultipleResponse RunCloseCasesTest(Plugin.LocalPluginContext localContext)
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
                    if(incident.StateCode.Value != IncidentState.Active || incident.IncidentStageCode.Value != incident_incidentstagecode.NotAnswered ||
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

            return ExecuteMultipleRequests(localContext, requestsLst, true, true);
        }

        /// <summary>
        /// Saves the changes that the OrganizationServic is tracking to Microsoft Dynamics CRM.
        /// </summary>
        /// <param name="continueOnError">Indicates whether further execution of message requests should continue if a fault is returned for the current request being processed.</param>
        /// <param name="returnResponses">Indicates if a response for each message request processed should be returned. </param>
        /// <returns></returns>
        public static ExecuteMultipleResponse ExecuteMultipleRequests(Plugin.LocalPluginContext localContext, List<OrganizationRequest> requestsLst, Boolean continueOnError, Boolean returnResponses, int defaultBatchSize = 250)
        {
            // return reponses
            ExecuteMultipleResponse responseWithResults = null;

            // list of OrganizationRequests
            var organizationRequestsList = new List<OrganizationRequest>();
            // Add the custom requests to the request collection.
            organizationRequestsList.AddRange(requestsLst);

            // if no request is to be sent, exit
            if (organizationRequestsList.Count == 0)
            {
                return null;
            }

            // Create an ExecuteMultipleRequest object.
            var requestWithResults = new ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = continueOnError,
                    ReturnResponses = returnResponses
                },
            };

            // split requests according to BatchSize
            int batchSize = defaultBatchSize;
            int offset = 0;
            while (organizationRequestsList.Count > offset)
            {
                var tempRequestSet = organizationRequestsList.Skip(offset).Take(batchSize);

                // Create an empty organization request collection.
                requestWithResults.Requests = new OrganizationRequestCollection();
                requestWithResults.Requests.AddRange(tempRequestSet);

                // Execute all the requests in the request collection using a single web method call.
                try
                {
                    responseWithResults =
                        (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);
                }
                catch (FaultException<OrganizationServiceFault> fault)
                {
                    // Check if the maximum batch size has been exceeded. The maximum batch size is only included in the fault if it
                    // the input request collection count exceeds the maximum batch size.
                    if (fault.Detail.ErrorDetails.Contains("MaxBatchSize"))
                    {
                        int maxBatchSize = Convert.ToInt32(fault.Detail.ErrorDetails["MaxBatchSize"]);
                        if (maxBatchSize < requestWithResults.Requests.Count)
                        {
                            // Here you could reduce the size of your request collection and re-submit the ExecuteMultiple request.
                            // For this sample, that only issues a few requests per batch, we will just print out some info. However,
                            // this code will never be executed because the default max batch size is 1000.
                            batchSize = maxBatchSize;
                            continue;
                        }
                    }
                }

                // increment requests counter
                offset += tempRequestSet.Count();
            }

            // return responses
            return responseWithResults;
        }
    }
}
