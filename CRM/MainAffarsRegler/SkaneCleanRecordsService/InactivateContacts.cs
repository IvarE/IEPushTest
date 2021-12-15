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
    public class InactivateContacts : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Inactivate Contacts Schedule";
        public const string TriggerDescription = "InactivateContacts Schedule Trigger";
        public const string JobDescription = "InactivateContacts Schedule Job";
        public const string TriggerName = "InactivateContactsTrigger";
        public const string JobName = "InactivateContacts";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            // Connect to the CRM web service using a connection string.
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(ContactsService.CredentialFilePath, ContactsService.Entropy));

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
                _log.Info($"Starting Inactivate Contacts Job....");
                localContext = GenerateLocalContext();

                if (localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }

                _log.Info($"Local Context is not null....");
                string runFullData = ConfigurationManager.AppSettings["runFullData"];
                
                if(runFullData == null || (runFullData != "true" && runFullData != "false"))
                {
                    _log.Debug($"The App Setting 'runFullData' is neither 'true' or 'false'.");
                    return;
                }

                _log.Info($"runFullData Flag is {runFullData}");
                bool bRunFullData = runFullData == "true" ? true : false;
                ExecuteMultipleResponse responseWithResults = RunInactivateContacts(localContext, bRunFullData);

                // process the results returned in the responses
                if (responseWithResults != null && responseWithResults.IsFaulted == true)
                {
                    _log.Error(String.Join("; ", responseWithResults.Responses.ToList().FindAll(x => x.Fault != null).Select(x => x.Fault.Message)));
                }
                else if (responseWithResults != null && responseWithResults.IsFaulted == false)
                {

                }

                _log.Info($"ContactsService Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in InactivateContacts.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public static QueryExpression GetQueryInactivateContacts(bool bRunFullData)
        {
            string testSpecificDate = Properties.Settings.Default.SpecificDate;

            var queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
            queryContacts.Distinct = true;
            queryContacts.NoLock = true;
            queryContacts.ColumnSet.AddColumns(ContactEntity.Fields.ContactId, ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName,
                                            ContactEntity.Fields.EMailAddress1, ContactEntity.Fields.EMailAddress2);

            var queryFilter0 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter0);
            queryFilter0.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.Null);
            queryFilter0.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)ContactState.Active);
            queryFilter0.AddCondition(ContactEntity.Fields.ed_Epostmottagare, ConditionOperator.Equal, false);
            queryFilter0.AddCondition(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true);
            
            var queryFilter1 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter1);
            queryFilter1.AddCondition("ap", IncidentEntity.Fields.cgi_Contactid, ConditionOperator.Null);

            var queryFilter2 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter2);
            queryFilter2.AddCondition("aq", IncidentEntity.Fields.PrimaryContactId, ConditionOperator.Null);

            var queryFilter3 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter3);
            queryFilter3.AddCondition("ar", IncidentEntity.Fields.CustomerId, ConditionOperator.Null);

            var queryFilter4 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter4);
            queryFilter4.AddCondition("as", SalesOrderEntity.Fields.ed_ContactId, ConditionOperator.Null);

            var queryFilter5 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter5);
            queryFilter5.AddCondition("at", SingaporeTicketEntity.Fields.st_ContactID, ConditionOperator.Null);

            var queryFilter6 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter6);
            queryFilter6.AddCondition("au", TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.Null);

            var queryFilter7 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter7);
            queryFilter7.AddCondition("av", SkaKortEntity.Fields.ed_Contact, ConditionOperator.Null);

            var al = queryContacts.AddLink(ValueCodeEntity.EntityLogicalName, Contact.Fields.ContactId, ValueCodeEntity.Fields.ed_Contact);
            al.EntityAlias = "al";
            al.LinkCriteria.AddCondition(ValueCodeEntity.Fields.statecode, ConditionOperator.Equal, (int)ed_ValueCodeState.Inactive);

            var ap = queryContacts.AddLink(IncidentEntity.EntityLogicalName, ContactEntity.Fields.ContactId, IncidentEntity.Fields.cgi_Contactid, JoinOperator.LeftOuter);
            ap.EntityAlias = "ap";

            var aq = queryContacts.AddLink(IncidentEntity.EntityLogicalName, ContactEntity.Fields.ContactId, IncidentEntity.Fields.PrimaryContactId, JoinOperator.LeftOuter);
            aq.EntityAlias = "aq";

            var ar = queryContacts.AddLink(IncidentEntity.EntityLogicalName, ContactEntity.Fields.ContactId, IncidentEntity.Fields.CustomerId, JoinOperator.LeftOuter);
            ar.EntityAlias = "ar";

            var _as = queryContacts.AddLink(SalesOrderEntity.EntityLogicalName, ContactEntity.Fields.ContactId, SalesOrderEntity.Fields.ed_ContactId, JoinOperator.LeftOuter);
            _as.EntityAlias = "as";

            var at = queryContacts.AddLink(SingaporeTicketEntity.EntityLogicalName, ContactEntity.Fields.ContactId, SingaporeTicketEntity.Fields.st_ContactID, JoinOperator.LeftOuter);
            at.EntityAlias = "at";

            var au = queryContacts.AddLink(TravelCardEntity.EntityLogicalName, ContactEntity.Fields.ContactId, TravelCardEntity.Fields.cgi_Contactid, JoinOperator.LeftOuter);
            au.EntityAlias = "au";

            var av = queryContacts.AddLink(SkaKortEntity.EntityLogicalName, ContactEntity.Fields.ContactId, SkaKortEntity.Fields.ed_Contact, JoinOperator.LeftOuter);
            av.EntityAlias = "av";

            if (!bRunFullData)
            {
                _log.Info($"Contacts filtered by Created On: {testSpecificDate}");
                queryContacts.Criteria.AddCondition(Contact.Fields.CreatedOn, ConditionOperator.On, testSpecificDate);
            }

            return queryContacts;
        }

        public static ExecuteMultipleResponse RunInactivateContacts(Plugin.LocalPluginContext localContext, bool bRunFullData)
        {
            //Run everyday
            _log.Info($"'RunFullData': {bRunFullData}");
            QueryExpression queryContacts = GetQueryInactivateContacts(bRunFullData);
            List<ContactEntity> lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);
            _log.Info($"Found { lContacts.Count } Contacts to be Inactivated.");

            List<OrganizationRequest> requestsLst = new List<OrganizationRequest>();

            foreach (ContactEntity contact in lContacts)
            {
                try
                {
                    ContactEntity nContact = new ContactEntity();
                    nContact.Id = contact.Id;
                    nContact.StateCode = ContactState.Inactive;
                    nContact.StatusCode = contact_statuscode.Inactive;

                    UpdateRequest updateContact = new UpdateRequest()
                    {
                        Target = nContact
                    };

                    requestsLst.Add(updateContact);
                    _log.InfoFormat($"Contact: {contact.FirstName} {contact.LastName} added to Request Batch to be Inactivated.");
                }
                catch (Exception e)
                {
                    _log.Error($"Exception caught in Inactivate Contact {contact.FirstName} {contact.LastName}\n{e.Message}\n\n");
                }
            }

            return Helper.ExecuteMultipleRequests(localContext, requestsLst, true, true);
        }
    }
}
