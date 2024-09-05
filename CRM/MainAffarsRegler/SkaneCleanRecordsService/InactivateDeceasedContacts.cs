using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Quartz;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Endeavor.Crm.CleanRecordsService
{
    [DisallowConcurrentExecution, PersistJobDataAfterExecution]
    public class InactivateDeceasedContacts : IJob
    {
        public const string DataMapModifiedAfter = "ModifiedAfterUpload";
        public const string GroupName = "Inactivate Deceased Contacts Schedule";
        public const string TriggerDescription = "InactivateDeceasedContacts Schedule Trigger";
        public const string JobDescription = "InactivateDeceasedContacts Schedule Job";
        public const string TriggerName = "InactivateDeceasedContactsTrigger";
        public const string JobName = "InactivateDeceasedContacts";

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Plugin.LocalPluginContext GenerateLocalContext()
        {
            CrmServiceClient conn = new CrmServiceClient(CrmConnection.GetCrmConnectionString(ContactsService.CredentialFilePath, ContactsService.Entropy));
            IOrganizationService serviceProxy = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
            Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), serviceProxy, null, new TracingService());
            return localContext;
        }

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug(string.Format(Properties.Resources.TriggerExecuting, context.Trigger.Description ?? context.Trigger.Key.Name));
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuting, context.JobDetail.Description ?? context.JobDetail.Key.Name ?? "NULL"));
            ExecuteJob();
            _log.Debug(string.Format(Properties.Resources.ScheduleJobExecuted, context.JobDetail.Description ?? context.JobDetail.Key.Name));
        }

        public void ExecuteJob()
        {
            Plugin.LocalPluginContext localContext = null;

            try
            {
                _log.Info($"Starting Inactivate Deceased Contacts Job....");
                localContext = InactivateDeceasedContacts.GenerateLocalContext();

                if (localContext == null)
                {
                    _log.Error($"Connection to CRM was not possible.\n LocalContext is null.\n\n");
                    return;
                }

                _log.Info($"Local Context is not null....");
                string runFullData = ConfigurationManager.AppSettings["runFullData"];

                if (runFullData == null || (runFullData != "true" && runFullData != "false"))
                {
                    _log.Debug($"The App Setting 'runFullData' is neither 'true' or 'false'.");
                    return;
                }

                _log.Info($"runFullData Flag is {runFullData}");
                bool bRunFullData = runFullData == "true" ? true : false;

                ExecuteMultipleResponse responseWithResults = RunInactivateDeceasedContacts(localContext, bRunFullData);

                // Used for debugging the results
                if (responseWithResults != null)
                {
                    var faultMessages = new List<string>();
                    var successMessages = new List<string>();

                    foreach (var responseItem in responseWithResults.Responses)
                    {
                        if (responseItem.Fault != null)
                        {
                            faultMessages.Add(responseItem.Fault.Message);
                        }
                        else
                        {
                            if (responseItem.Response is UpdateResponse updateResponse)
                            {
                                var targetEntity = (Entity)updateResponse.Results["Target"];
                                successMessages.Add($"Successfully updated entity with ID: {targetEntity.Id}");
                            }
                            else
                            {
                                successMessages.Add("Successfully processed request.");
                            }
                        }
                    }

                    if (faultMessages.Any())
                    {
                        _log.Error(string.Join("; ", faultMessages));
                    }

                    if (successMessages.Any())
                    {
                        _log.Info(string.Join("; ", successMessages));
                    }
                }

                _log.Info($"Deceased Contacts Service Done");
            }
            catch (Exception e)
            {
                _log.Error($"Exception caught in InactivateDeceasedContacts.ExecuteJob():\n{e.Message}\n\n");
            }
        }

        public static QueryExpression GetQueryInactivateDeceasedContacts(bool bRunFullData)
        {

            string testSpecificDate = Properties.Settings.Default.SpecificDate;

            var queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
            queryContacts.Distinct = true;
            queryContacts.NoLock = true;
            queryContacts.ColumnSet.AddColumns(ContactEntity.Fields.ContactId, ContactEntity.Fields.ed_DeceasedDate, ContactEntity.Fields.ed_Deceased,
                                            ContactEntity.Fields.ed_PrivateCustomerContact, ContactEntity.Fields.ed_Serviceresor, ContactEntity.Fields.EMailAddress1, ContactEntity.Fields.EMailAddress2);

            var queryFilter0 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter0);

            if (bRunFullData)
            {
                _log.Info($"Contacts filtered by Deceased Date Older than 6 months");
                queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_DeceasedDate, ConditionOperator.OlderThanXMonths, 6);
            }
            else if (!bRunFullData)
            {
                // For testing / debugging
                // Add your alternate query conditions here
            }

            queryFilter0.AddCondition(ContactEntity.Fields.ed_MklId, ConditionOperator.Null);
            queryFilter0.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)ContactState.Active);
            queryFilter0.AddCondition(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true);

            var queryEpost = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryEpost);
            queryEpost.FilterOperator = LogicalOperator.Or;
            queryEpost.AddCondition(ContactEntity.Fields.ed_Epostmottagare, ConditionOperator.Null);
            queryEpost.AddCondition(ContactEntity.Fields.ed_Epostmottagare, ConditionOperator.Equal, false);

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
            queryFilter6.FilterOperator = LogicalOperator.Or;
            queryFilter6.AddCondition("au", TravelCardEntity.Fields.cgi_Contactid, ConditionOperator.Null);
            queryFilter6.AddCondition("au", TravelCardEntity.Fields.cgi_Blocked, ConditionOperator.Equal, true);

            var queryFilter7 = new FilterExpression();
            queryContacts.Criteria.AddFilter(queryFilter7);
            queryFilter7.AddCondition("av", SkaKortEntity.Fields.ed_Contact, ConditionOperator.Null);

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

        public static QueryExpression GetQueryValuesCodes(Guid gContact)
        {
            var queryValueCodes = new QueryExpression(ValueCodeEntity.EntityLogicalName);
            queryValueCodes.NoLock = true;
            queryValueCodes.ColumnSet.AddColumns(ValueCodeEntity.Fields.statecode);
            queryValueCodes.Criteria.AddCondition(ValueCodeEntity.Fields.ed_Contact, ConditionOperator.Equal, gContact);

            return queryValueCodes;
        }

        public static UpdateRequest GetUpdateRequest(Guid gContact)
        {
            ContactEntity nContact = new ContactEntity();
            nContact.Id = gContact;
            nContact.StateCode = ContactState.Inactive;
            nContact.StatusCode = contact_statuscode.Inactive;

            return new UpdateRequest()
            {
                Target = nContact
            };
        }

        public static ExecuteMultipleResponse RunInactivateDeceasedContacts(Plugin.LocalPluginContext localContext, bool bRunFullData)
        {
            _log.Info($"'RunFullData': {bRunFullData}");
            QueryExpression queryContacts = InactivateDeceasedContacts.GetQueryInactivateDeceasedContacts(bRunFullData);
            List<ContactEntity> lContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);

            _log.Info($"Found {lContacts.Count} Contacts to be checked.");

            List<OrganizationRequest> requestsLst = new List<OrganizationRequest>();

            foreach (ContactEntity contact in lContacts)
            {
                try
                {
                    QueryExpression queryValueCodes = GetQueryValuesCodes(contact.Id);
                    List<ValueCodeEntity> lValueCodes = XrmRetrieveHelper.RetrieveMultiple<ValueCodeEntity>(localContext, queryValueCodes);

                    if (lValueCodes.Count == 0)
                    {
                        UpdateRequest updateContact = GetUpdateRequest(contact.Id);
                        requestsLst.Add(updateContact);
                    }
                    else
                    {
                        bool checkActive = false;
                        foreach (ValueCodeEntity valueCode in lValueCodes)
                        {
                            if (valueCode.statecode == ed_ValueCodeState.Active)
                            {
                                checkActive = true;
                                break;
                            }
                        }

                        if (!checkActive)
                        {
                            HandleDeleteContactMessageMKL(localContext, contact.Id, contact);
                            UpdateRequest updateContact = InactivateDeceasedContacts.GetUpdateRequestDeceasedContact(contact.Id);
                            requestsLst.Add(updateContact);
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.Error($"Exception caught in Inactivate Deceased Contact {contact.FirstName} {contact.LastName}\n{e.Message}\n\n");
                }
            }

            _log.Info($"Found {requestsLst.Count} Deceased Contacts to be Inactivated.");
            return Helper.ExecuteMultipleRequests(localContext, requestsLst, true, true);
        }

        public static bool HandleDeleteContactMessageMKL(Plugin.LocalPluginContext localContext, Guid contactId, ContactEntity contact)
        {

            FilterExpression settingFilter = new FilterExpression(LogicalOperator.And);
            settingFilter.AddCondition(CgiSettingEntity.Fields.st_MklEndpointToken, ConditionOperator.NotNull);
            settingFilter.AddCondition(CgiSettingEntity.Fields.ed_CRMPlusService, ConditionOperator.NotNull);
            CgiSettingEntity cgiSetting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext,
                new ColumnSet(CgiSettingEntity.Fields.st_MklEndpointToken, CgiSettingEntity.Fields.ed_CRMPlusService), settingFilter);

            string MklEndpoint = cgiSetting?.st_MklEndpointToken;
            string fasadEndpoint = cgiSetting.ed_CRMPlusService;

            if (cgiSetting != null && !string.IsNullOrWhiteSpace(MklEndpoint) && !string.IsNullOrWhiteSpace(fasadEndpoint))
            {

                var requestToken = string.Empty;

                #region AccessToken From Fasad

                var tokenHttpWebReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/Contacts/GetAccessToken/mkl", fasadEndpoint));
                tokenHttpWebReq.ContentType = "text/plain; charset=utf-8";
                tokenHttpWebReq.Method = "GET";

                try
                {
                    var tokenHttpResponse = (HttpWebResponse)tokenHttpWebReq.GetResponse();
                    if (tokenHttpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(string.Format("400 - Could not access required token for MKL endpoint. Response: " + tokenHttpResponse.StatusCode.ToString()));
                    }
                    else
                    {
                        using (var streamReader = new StreamReader(tokenHttpResponse.GetResponseStream()))
                        {
                            //Read response token
                            requestToken = streamReader.ReadToEnd();
                        }

                        if (string.IsNullOrWhiteSpace(requestToken))
                        {
                            throw new Exception(string.Format("400 - Could not access required token for MKL endpoint. Returned token was null."));
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidPluginExecutionException($"Error while retrieving Token through Fasaden : {e.Message}", e);
                }

                #endregion

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Trigger call to MKL
                //Using the admin endpoint that uses a JSON Object in the body
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format($"{MklEndpoint}/api/v1/admin/users/{contactId}"));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";
                httpWebRequest.Headers.Add("channel", "crm");
                httpWebRequest.Headers.Add("Authorization", "Bearer " + requestToken);

                //Create stream so that you can send a JSON object as body of content 
                ContactInactivateRequest inactivateContactRequestObj = new ContactInactivateRequest();
                inactivateContactRequestObj.contactId = contactId.ToString();
                DataContractJsonSerializer js = null;
                MemoryStream msObj = new MemoryStream();
                js = new DataContractJsonSerializer(typeof(ContactInactivateRequest));
                js.WriteObject(msObj, inactivateContactRequestObj);

                msObj.Position = 0;
                StreamReader sr = new StreamReader(msObj);
                var InputJSON = sr.ReadToEnd(); //Create a JSON for CancelledBy and VoucherId
                sr.Close();
                msObj.Close();

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(InputJSON);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    var result = string.Empty;

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    if (httpResponse != null && httpResponse.StatusCode != HttpStatusCode.NoContent)
                    {
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                            localContext.TracingService.Trace($"DeleteMessage to MKL returned StatusCode: {httpResponse.StatusCode} and results: {result}");
                            throw new Exception($"Fel vid kommunikation med externt system: {result}");
                        }
                    }
                    return true;

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (HttpWebResponse)we.Response;
                    if (response == null)
                    {
                        localContext.TracingService.Trace($"Attempted Delete-message to MKL returned an exeption. Content: {we.Message}");
                        throw we;
                    }

                    try
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MklErrorObject));
                        MklErrorObject mklErr = (MklErrorObject)serializer.ReadObject(response.GetResponseStream());

                        if (response.StatusCode == HttpStatusCode.NotFound && contact.ed_OverrideMerge == true)
                        {
                            return true;
                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new InvalidPluginExecutionException($"Kontakten hittades ej hos Mitt Konto, vill du fortfarande inaktivera kontakten, sätt 'Mitt Konto Saknas' till Ja och utför åtgärden igen.");
                        }


                        if (MklErrorObject.UserEntityNotFound.Equals(mklErr.code))
                        {
                            return true;
                        }

                        localContext.TracingService.Trace("Attempted Delete-message to MKL returned an exeption httpCode: {0} Content: {1}", response.StatusCode, mklErr.localizedMessage);

                        throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {(mklErr.localizedMessage ?? mklErr.message) ?? mklErr.httpStatus.ToString()}", mklErr.innerException ?? null);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {e.Message}", e);
                    }

                }
                catch (Exception e)
                {
                    throw new Exception("400 - Failed to Execute: " + e.Message);
                }

            }
            else
            {
                throw new InvalidPluginExecutionException("400 - Did not find URI for inactivate contact!");
            }
        }

        public static UpdateRequest GetUpdateRequestDeceasedContact(Guid gContact)
        {
            ContactEntity nContact = new ContactEntity();
            nContact.Id = gContact;
            nContact.StateCode = ContactState.Inactive;
            nContact.StatusCode = contact_statuscode.Inactive;
            nContact.ed_DoNotUpdateFB = true;

            return new UpdateRequest()
            {
                Target = nContact
            };
        }
    }
}