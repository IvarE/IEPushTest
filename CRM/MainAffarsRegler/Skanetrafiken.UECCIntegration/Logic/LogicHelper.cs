using Endeavor.Crm;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.UECCIntegration.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.UECCIntegration.Logic
{
    public class LogicHelper
    {
        //private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ILog _log = LogManager.GetLogger("FileAppenderLog");
        private static ILog _logC3 = LogManager.GetLogger("Criteria3Contacts");

        public static bool CheckInterceptions(List<Guid> lGContactsC1, List<Guid> lGContactsC2, List<Guid> lGContactsC3)
        {
            bool hasInterceptions = false;

            foreach (Guid contactId in lGContactsC1)
            {
                List<Guid> match2 = lGContactsC2.Where(x => x.Equals(contactId)).ToList();
                List<Guid> match3 = lGContactsC3.Where(x => x.Equals(contactId)).ToList();

                if (match2.Count > 0 || match3.Count > 0)
                {
                    hasInterceptions = true;
                    _log.InfoFormat(CultureInfo.InvariantCulture, match2?.FirstOrDefault().ToString());
                }
            }

            foreach (Guid contactId in lGContactsC2)
            {
                List<Guid> match1 = lGContactsC1.Where(x => x.Equals(contactId)).ToList();
                List<Guid> match3 = lGContactsC3.Where(x => x.Equals(contactId)).ToList();

                if (match1.Count > 0 || match3.Count > 0)
                {
                    hasInterceptions = true;
                    _log.InfoFormat(CultureInfo.InvariantCulture, match1?.FirstOrDefault().ToString());
                }
            }

            foreach (Guid contactId in lGContactsC3)
            {
                List<Guid> match1 = lGContactsC1.Where(x => x.Equals(contactId)).ToList();
                List<Guid> match2 = lGContactsC2.Where(x => x.Equals(contactId)).ToList();

                if (match1.Count > 0 || match2.Count > 0)
                    hasInterceptions = true;
            }

            return hasInterceptions;
        }

        public static List<Guid> GetContactsCriteriaOneTwo(IOrganizationService organizationService, int active, int privateCustomer, int countOfCompanyRoles)
        {
            List<Guid> lGContacts = new List<Guid>();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true' no-lock='true' >
                              <entity name='ed_companyrole' >
                                <attribute name='ed_companyroleid' alias='companyrole_count' aggregate='countcolumn' />
                                <link-entity name='contact' from='contactid' to='ed_contact' alias='ab' >
                                  <attribute name='contactid' alias='contactid' groupby='true' />";

            if(privateCustomer == 0) //Is Criteria 1
            {
                fetch += @"<filter type='and'>
                              <filter type='or'>
                                <condition attribute='ed_privatecustomercontact' operator='eq' value='{0}' />
                                <condition attribute='ed_privatecustomercontact' operator='null' />
                              </filter>
                              <condition attribute='statecode' operator='eq' value='{1}' />
                            </filter>";
            }
            else
            {
                fetch += @"<filter type='and'>
                                <condition attribute='ed_privatecustomercontact' operator='eq' value='{0}' />
                                <condition attribute='statecode' operator='eq' value='{1}' />
                            </filter>";
                                
            }

            fetch += @"</link-entity>
                        </entity>
                        </fetch>";

            string getCompanyRoles = string.Format(fetch, privateCustomer, active);
            List<Entity> lContacts = organizationService.RetrieveMultiple(new FetchExpression(getCompanyRoles)).Entities.ToList();

            foreach (Entity contact in lContacts)
            {
                if (contact["companyrole_count"] == null || contact["contactid"] == null)
                    continue;

                try
                {
                    int companyRole_Count = (int)(((AliasedValue)contact["companyrole_count"]).Value);

                    if (companyRole_Count >= countOfCompanyRoles)
                        lGContacts.Add((Guid)(((AliasedValue)contact["contactid"]).Value));
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, "The Contact " + contact["contactid"] + " was not processed. Details: " + e.Message);
                }
            }
            return lGContacts;
        }

        public static List<Guid> GetContactsCriteriaThree(IOrganizationService organizationService, int active, int countOfCompanyRoles, List<Guid> lC1, List<Guid> lC2)
        {
            List<Guid> lGContacts = new List<Guid>();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true' no-lock='true' >
                              <entity name='ed_companyrole' >
                                <attribute name='ed_companyroleid' alias='companyrole_count' aggregate='countcolumn' />
                                <link-entity name='contact' from='contactid' to='ed_contact' alias='ab' >
                                  <attribute name='contactid' alias='contactid' groupby='true' />
                                  <filter type='and' >
                                    <condition attribute='statecode' operator='eq' value='{0}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            string getCompanyRoles = string.Format(fetch, active);
            List<Entity> lContacts = organizationService.RetrieveMultiple(new FetchExpression(getCompanyRoles)).Entities.ToList();

            List<Guid> lToRemove = new List<Guid>();
            lToRemove.AddRange(lC1);
            lToRemove.AddRange(lC2);

            foreach (Guid contactid in lToRemove)
            {
                int index = lContacts.FindIndex(x => ((Guid)(((AliasedValue)x["contactid"]).Value)).Equals(contactid));
                lContacts.RemoveAt(index);
            }

            foreach (Entity contact in lContacts)
            {
                if (contact["companyrole_count"] == null || contact["contactid"] == null)
                    continue;

                try
                {
                    int companyRole_Count = (int)(((AliasedValue)contact["companyrole_count"]).Value);

                    if (companyRole_Count >= countOfCompanyRoles)
                        lGContacts.Add((Guid)(((AliasedValue)contact["contactid"]).Value));
                }
                catch (Exception e)
                {
                    _logC3.ErrorFormat(CultureInfo.InvariantCulture, "The Contact " + contact["contactid"] + " was not processed. Details: " + e.Message);
                }
            }

            return lGContacts;
        }

        public static List<ed_CompanyRole> GetCompanyRolesFromContact(Plugin.LocalPluginContext localContext, Guid contactId)
        {
            QueryExpression queryCompanyRoles = new QueryExpression(ed_CompanyRole.EntityLogicalName);
            queryCompanyRoles.NoLock = true;
            queryCompanyRoles.ColumnSet.AddColumns(ed_CompanyRole.Fields.ed_CompanyRoleId, ed_CompanyRole.Fields.ed_EmailAddress, ed_CompanyRole.Fields.ed_Telephone,
                                                    ed_CompanyRole.Fields.ed_SocialSecurityNumber);
            queryCompanyRoles.AddOrder(ed_CompanyRole.Fields.CreatedOn, OrderType.Ascending);
            queryCompanyRoles.Criteria.AddCondition(ed_CompanyRole.Fields.ed_Contact, ConditionOperator.Equal, contactId);

            return XrmRetrieveHelper.RetrieveMultiple<ed_CompanyRole>(localContext, queryCompanyRoles);
        }

        public static List<Contact> GetListContactFromLimitedList(Plugin.LocalPluginContext localContext, List<Guid> lGContacts)
        {
            QueryExpression queryContacts = new QueryExpression(Contact.EntityLogicalName);
            queryContacts.NoLock = true;
            queryContacts.ColumnSet.AddColumns(Contact.Fields.ContactId, Contact.Fields.EMailAddress1, Contact.Fields.FirstName, Contact.Fields.LastName,
                                                Contact.Fields.Telephone2, Contact.Fields.ed_SocialSecurityNumberBlock, Contact.Fields.cgi_socialsecuritynumber);

            FilterExpression filter = new FilterExpression();
            queryContacts.Criteria.AddFilter(filter);

            filter.FilterOperator = LogicalOperator.Or;

            foreach (Guid contactId in lGContacts)
                filter.AddCondition(Contact.Fields.ContactId, ConditionOperator.Equal, contactId);

            return XrmRetrieveHelper.RetrieveMultiple<Contact>(localContext, queryContacts);
        }

        public static List<Contact> GetContactsFromListGuids(Plugin.LocalPluginContext localContext, List<Guid> lGContacts)
        {
            int maxConditions = int.Parse(ConfigurationManager.AppSettings["maxConditions"]); ;

            List<Contact> lFinalContacts = new List<Contact>();

            List<List<Guid>> lAux = lGContacts.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / maxConditions)
                                                .Select(x => x.Select(v => v.Value).ToList()).ToList();

            foreach (List<Guid> listGuids in lAux)
            {
                List<Contact> lAuxContact = GetListContactFromLimitedList(localContext, listGuids);
                lFinalContacts.AddRange(lAuxContact);
            }

            return lFinalContacts;
        }

        public static SkaneRequests HandleContactsC1C2(Plugin.LocalPluginContext localContext, List<Guid> lGContacts, bool isC1)
        {
            SkaneRequests finalRequests = new SkaneRequests();

            List<Contact> lFinalContacts = GetContactsFromListGuids(localContext, lGContacts);

            foreach (Contact contact in lFinalContacts)
            {
                List<ed_CompanyRole> lCompanyRoles = GetCompanyRolesFromContact(localContext, (Guid)contact.ContactId);

                if (lCompanyRoles.FirstOrDefault() == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, "The Contact " + contact.ContactId + " does not have Company Roles.");
                    continue;
                }

                if (isC1)
                {
                    ed_CompanyRole firstCompanyRole = lCompanyRoles.FirstOrDefault();

                    string contactEmailAddress = contact.EMailAddress1;
                    string companyRoleEmailAddress = firstCompanyRole.ed_EmailAddress;

                    if (contactEmailAddress != companyRoleEmailAddress)
                    {
                        Contact uContact = new Contact();
                        uContact.Id = (Guid)contact.ContactId;
                        uContact.EMailAddress1 = companyRoleEmailAddress;

                        UpdateRequest updateRequest = new UpdateRequest { Target = uContact };
                        finalRequests.lUpdateRequests.Add(updateRequest);
                        _log.InfoFormat("Update Request Added for Contact: " + (Guid)contact.ContactId);

                        //XrmHelper.Update(localContext, uContact);
                        //_log.InfoFormat(CultureInfo.InvariantCulture, "The Contact " + (Guid)contact.ContactId + " of Criteria 1 was updated with email address: " + companyRoleEmailAddress + ".");
                    }

                    lCompanyRoles.Remove(firstCompanyRole);
                }

                foreach (ed_CompanyRole companyRole in lCompanyRoles)
                {
                    Guid gNewContact = Guid.Empty;

                    Contact nContact = new Contact();
                    nContact.FirstName = contact.FirstName;
                    nContact.LastName = contact.LastName;
                    nContact.EMailAddress1 = companyRole.ed_EmailAddress;
                    nContact.Telephone2 = companyRole.ed_Telephone;
                    nContact.ed_SocialSecurityNumberBlock = companyRole.ed_SocialSecurityNumber;
                    nContact.ed_InformationSource = ed_informationsource.ForetagsPortal;
                    nContact.Description = contact.ContactId.ToString();

                    CreateRequest createRequest = new CreateRequest { Target = nContact };
                    finalRequests.lCreateRequests.Add(createRequest);
                    _log.InfoFormat("Create Request Added for Contact: " + contact.FirstName + ", " + contact.LastName);

                    //gNewContact = XrmHelper.Create(localContext, nContact);
                    //_log.InfoFormat(CultureInfo.InvariantCulture, "New Contact created");

                    if(companyRole.ed_EmailAddress != null)
                    {
                        ed_CompanyRole uCompanyRole = new ed_CompanyRole();
                        uCompanyRole.Id = (Guid)companyRole.ed_CompanyRoleId;
                        uCompanyRole.ed_Contact = new EntityReference(Contact.EntityLogicalName)
                        {
                            KeyAttributes = new KeyAttributeCollection
                            {
                                {Contact.Fields.EMailAddress1, companyRole.ed_EmailAddress}
                            }
                        };
                        //uCompanyRole.ed_Contact = new EntityReference(Contact.EntityLogicalName, gNewContact);

                        UpdateRequest updateRequest = new UpdateRequest { Target = uCompanyRole };
                        finalRequests.lUpdateRequests.Add(updateRequest);
                        _log.InfoFormat("Update Request Added for Company Role: " + (Guid)companyRole.ed_CompanyRoleId);

                        //XrmHelper.Update(localContext, uCompanyRole);
                        //_log.InfoFormat(CultureInfo.InvariantCulture, "The Company Role " + (Guid)companyRole.ed_CompanyRoleId + " was updated with contact: " + gNewContact + ".");
                    }
                }
            }

            return finalRequests;
        }

        public static void HandleContactsC3(Plugin.LocalPluginContext localContext, List<Guid> lGContacts)
        {
            if (lGContacts.Count == 0)
            {
                _logC3.InfoFormat(CultureInfo.InvariantCulture, "No Contacts found for Criteria 3.");
                return;
            }

            List<Contact> lFinalContacts = GetContactsFromListGuids(localContext, lGContacts);

            foreach (Contact contact in lFinalContacts)
            {
                string logC3 = contact.cgi_socialsecuritynumber != null ? "Social Security Number: " + contact.cgi_socialsecuritynumber + ", " : "";
                logC3 += contact.ed_SocialSecurityNumberBlock != null ? "Social Security Number Block: " + contact.ed_SocialSecurityNumberBlock + ", " : "";
                logC3 += contact.FirstName != null ? "First Name: " + contact.FirstName + ", " : "";
                logC3 += contact.LastName != null ? "Last Name: " + contact.LastName + ", " : "";
                logC3 += contact.EMailAddress1 != null ? "Email: " + contact.EMailAddress1 + ", " : "";
                logC3 += contact.ContactId != null ? "ContactId: " + contact.ContactId + ", " : "";

                _logC3.InfoFormat(CultureInfo.InvariantCulture, logC3);
            }
        }

        public static List<Guid> ReadLogFileForErrorContacts()
        {
            List<Guid> lContacts = new List<Guid>();

            string[] lines = System.IO.File.ReadAllLines(@"C:\S\CRM\MainAffarsRegler\Skanetrafiken.UECCIntegration\Endeavor.ErrorContacts.log");

            foreach (string line in lines)
            {
                Guid gContact = Guid.Empty;
                if(Guid.TryParse(line, out gContact))
                    lContacts.Add(gContact);
                else
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"The string: " + line + " is not a valid Guid.");
            }

            return lContacts;
        }

        public static void LogExecuteMultipleResponses(ExecuteMultipleRequest requestWithResults, ExecuteMultipleResponse responseWithResults)
        {
            List<string> lStringFile = new List<string>();

            foreach (ExecuteMultipleResponseItem responseItem in responseWithResults.Responses)
            {
                // A valid response.
                if (responseItem.Response != null)
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Success Response: " + requestWithResults.Requests[responseItem.RequestIndex] + " : Response: " + responseItem.Response);

                // An error has occurred.
                else if (responseItem.Fault != null)
                {
                    try
                    {
                        _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed Response: " + requestWithResults.Requests[responseItem.RequestIndex] + " : Response: " + responseItem.Fault);

                        if (requestWithResults.Requests[responseItem.RequestIndex].RequestName == "Create")
                        {
                            Contact createContact = (Contact)requestWithResults.Requests[responseItem.RequestIndex].Parameters.Values.FirstOrDefault();

                            if (createContact != null && createContact.Description != null)
                                lStringFile.Add(createContact.Description);
                        }
                        else if (requestWithResults.Requests[responseItem.RequestIndex].RequestName == "Update")
                        {
                            Entity target = (Entity)requestWithResults.Requests[responseItem.RequestIndex].Parameters.Values.FirstOrDefault();

                            if (target == null)
                            {
                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Target Value from Responses is null. Contact your administrator.");
                                continue;
                            }

                            if (target.LogicalName == ed_CompanyRole.EntityLogicalName)
                            {
                                KeyAttributeCollection keyAttribute = target.GetAttributeValue<EntityReference>("ed_contact")?.KeyAttributes;
                                string keyContactEmail = (string)keyAttribute.Values.FirstOrDefault();
                                Guid keyCompanyRoleId = target.GetAttributeValue<Guid>("ed_companyroleid");

                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to Update Company Role: " + keyContactEmail + " with Contact: " + keyContactEmail + ". Response: " + responseItem.Fault);
                            }
                            else if (target.LogicalName == Contact.EntityLogicalName)
                            {
                                string emailAddress = target.GetAttributeValue<string>("emailaddress1");
                                Guid contactId = target.GetAttributeValue<Guid>("contactid");

                                _log.ErrorFormat(CultureInfo.InvariantCulture, $"Failed to Update Contact: " + contactId + " with Email Address: " + emailAddress);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _log.InfoFormat(CultureInfo.InvariantCulture, $"Error logging error message: Details " + e.Message);
                    }
                }
            }

            if(lStringFile.Count == 0)
            {
                List<string> lFinalString = lStringFile.Distinct().ToList();
                if(lFinalString.Count != 0)
                    System.IO.File.WriteAllLines(@"C:\S\CRM\MainAffarsRegler\Skanetrafiken.UECCIntegration\Endeavor.ErrorContacts.log", lFinalString);
            }
        }

        public static void HandleMultipleRequests(Plugin.LocalPluginContext localContext, SkaneRequests requestsC1, SkaneRequests requestsC2)
        {
            int maxLimitCalls = int.Parse(ConfigurationManager.AppSettings["maxLimitCalls"]);

            SkaneRequests finalRequest = new SkaneRequests();
            finalRequest.lCreateRequests.AddRange(requestsC1.lCreateRequests);
            finalRequest.lCreateRequests.AddRange(requestsC2.lCreateRequests);
            finalRequest.lUpdateRequests.AddRange(requestsC1.lUpdateRequests);
            finalRequest.lUpdateRequests.AddRange(requestsC2.lUpdateRequests);

            ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            if (finalRequest.lUpdateRequests.Count + finalRequest.lCreateRequests.Count > maxLimitCalls)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"The total Requests is greater than the max allowed.");

                List<List<CreateRequest>> lAuxCreate = finalRequest.lCreateRequests.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / maxLimitCalls)
                                                .Select(x => x.Select(v => v.Value).ToList()).ToList();

                foreach (List<CreateRequest> listCreate in lAuxCreate)
                {
                    requestWithResults.Requests = new OrganizationRequestCollection();
                    requestWithResults.Requests.AddRange(listCreate);

                    ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                    LogExecuteMultipleResponses(requestWithResults, responseWithResults);
                }

                List<List<UpdateRequest>> lAuxUpdate = finalRequest.lUpdateRequests.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / maxLimitCalls)
                                                .Select(x => x.Select(v => v.Value).ToList()).ToList();

                foreach (List<UpdateRequest> listUpdate in lAuxUpdate)
                {
                    requestWithResults.Requests = new OrganizationRequestCollection();
                    requestWithResults.Requests.AddRange(listUpdate);

                    ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                    LogExecuteMultipleResponses(requestWithResults, responseWithResults);
                }

            }
            else
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"The total Requests is less than the max allowed.");

                requestWithResults.Requests = new OrganizationRequestCollection();
                requestWithResults.Requests.AddRange(finalRequest.lCreateRequests);
                requestWithResults.Requests.AddRange(finalRequest.lUpdateRequests);

                ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)localContext.OrganizationService.Execute(requestWithResults);

                LogExecuteMultipleResponses(requestWithResults, responseWithResults);
            }
        }

        public static bool? CheckIfContactisC1(Plugin.LocalPluginContext localContext, Guid gContact)
        {
            int numberOfCompanyRolesC1 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC1"]);
            int numberOfCompanyRolesC2 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC2"]);

            EntityReference erContact = new EntityReference(Contact.EntityLogicalName, gContact);

            Contact eContact = XrmRetrieveHelper.Retrieve<Contact>(localContext, erContact, new ColumnSet(Contact.Fields.ed_PrivateCustomerContact));
            List<ed_CompanyRole> lCompanyRoles = GetCompanyRolesFromContact(localContext, gContact);

            if(eContact != null) {

                if ((eContact.ed_PrivateCustomerContact == null || eContact.ed_PrivateCustomerContact == false) && lCompanyRoles.Count >= numberOfCompanyRolesC1)
                {
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Contact: " + gContact + " is Criteria 1.");
                    return true;
                }
                else if (eContact.ed_PrivateCustomerContact == true && lCompanyRoles.Count >= numberOfCompanyRolesC2)
                {
                    _log.InfoFormat(CultureInfo.InvariantCulture, $"Contact: " + gContact + " is Criteria 2.");
                    return false;
                }
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Contact: " + gContact + " is not Criteria 1 or Criteria 2.");
            return null;
        }

        public static void RunLogic(Plugin.LocalPluginContext localContext)
        {
            int active = 0;
            int privateCustomerC1 = 0;
            int numberOfCompanyRolesC1 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC1"]);
            List<Guid> lGContactsC1 = GetContactsCriteriaOneTwo(localContext.OrganizationService, active, privateCustomerC1, numberOfCompanyRolesC1);

            int privateCustomerC2 = 1;
            int numberOfCompanyRolesC2 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC2"]);
            List<Guid> lGContactsC2 = GetContactsCriteriaOneTwo(localContext.OrganizationService, active, privateCustomerC2, numberOfCompanyRolesC2);

            List<Guid> lGContactsC3 = GetContactsCriteriaThree(localContext.OrganizationService, active, numberOfCompanyRolesC2, lGContactsC1, lGContactsC2);

            _log.InfoFormat(CultureInfo.InvariantCulture, "Number of Contacts Criteria 1: " + lGContactsC1.Count);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Number of Contacts Criteria 2: " + lGContactsC2.Count);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Number of Contacts Criteria 3: " + lGContactsC3.Count);

            bool hasInterceptions = CheckInterceptions(lGContactsC1, lGContactsC2, lGContactsC3);

            if (hasInterceptions)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, "There is Interceptions between the two lists. Please review Logic");
                Console.WriteLine("There is Interceptions between the two lists. Please review Logic");
                Console.ReadLine();
                return;
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, "There is no Interceptions between the two lists.");

            _log.InfoFormat(CultureInfo.InvariantCulture, "Getting Criteria 1 Requests.");
            SkaneRequests requestsC1 = HandleContactsC1C2(localContext, lGContactsC1, true);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Found " + requestsC1.lCreateRequests.Count + " Create Requests and " + requestsC1.lUpdateRequests.Count + " Update Requests");

            _log.InfoFormat(CultureInfo.InvariantCulture, "Getting Criteria 2 Logic.");
            SkaneRequests requestsC2 = HandleContactsC1C2(localContext, lGContactsC2, false);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Found " + requestsC2.lCreateRequests.Count + " Create Requests and " + requestsC2.lUpdateRequests.Count + " Update Requests");

            _log.InfoFormat(CultureInfo.InvariantCulture, "Starting to Send Requests to CRM.");
            HandleMultipleRequests(localContext, requestsC1, requestsC2);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Finished sending Requests to CRM.");

            _logC3.InfoFormat(CultureInfo.InvariantCulture, "Logging Criteria 3 Contacts.");
            HandleContactsC3(localContext, lGContactsC3);
        }

        public static void RunErrorContacts(Plugin.LocalPluginContext localContext)
        {
            List<Guid> errorContacts = ReadLogFileForErrorContacts();

            if(errorContacts == null || errorContacts.Count == 0)
            {
                _log.InfoFormat(CultureInfo.InvariantCulture, $"No more Contacts to handle.");
                Console.ReadLine();
                return;
            }

            List<Guid> lGContactsC1 = new List<Guid>();
            List<Guid> lGContactsC2 = new List<Guid>();

            foreach (Guid gContact in errorContacts)
            {
                bool? isC1 = CheckIfContactisC1(localContext, gContact);

                if (isC1 == null)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Critical Error: This Contact is neither Criteria 1 or Criteria 2. Contact you Administrator.");
                    continue;
                }

                if ((bool)isC1)
                    lGContactsC1.Add(gContact);
                else if(!(bool)isC1)
                    lGContactsC2.Add(gContact);
            }

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Getting Criteria 1 Requests.");
            SkaneRequests requestsC1 = HandleContactsC1C2(localContext, lGContactsC1, true);
            _log.InfoFormat(CultureInfo.InvariantCulture, $"Found " + requestsC1.lCreateRequests.Count + " Create Requests and " + requestsC1.lUpdateRequests.Count + " Update Requests");

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Getting Criteria 2 Logic.");
            SkaneRequests requestsC2 = HandleContactsC1C2(localContext, lGContactsC2, false);
            _log.InfoFormat(CultureInfo.InvariantCulture, $"Found " + requestsC2.lCreateRequests.Count + " Create Requests and " + requestsC2.lUpdateRequests.Count + " Update Requests");

            _log.InfoFormat(CultureInfo.InvariantCulture, $"RunErrorContacts--------------------Starting to Send Requests to CRM.");
            HandleMultipleRequests(localContext, requestsC1, requestsC2);
            _log.InfoFormat(CultureInfo.InvariantCulture, $"RunErrorContacts--------------------Finished sending Requests to CRM.");
        }
    }
}
