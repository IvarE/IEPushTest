﻿using Endeavor.Crm;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
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

        public static string rel_contact_companyRole = "ed_contact_ed_companyrole_Contact";

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
            List<Entity> lContacts = new List<Entity>();

            string fetch = @"<fetch {0} version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true' no-lock='true' >
                              <entity name='ed_companyrole' >
                                <attribute name='ed_companyroleid' alias='companyrole_count' aggregate='countcolumn' />
                                <link-entity name='contact' from='contactid' to='ed_contact' alias='ab' >
                                  <attribute name='contactid' alias='contactid' groupby='true' />
                                  <filter type='and' >
                                    <condition attribute='statecode' operator='eq' value='{1}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            bool moreRecords = false;
            int page = 1;
            string cookie = string.Empty;

            do
            {
                string getCompanyRoles = string.Format(fetch, cookie, active);

                _log.InfoFormat(CultureInfo.InvariantCulture, $"Fetchxml: " + getCompanyRoles);

                EntityCollection collection = organizationService.RetrieveMultiple(new FetchExpression(getCompanyRoles));

                if (collection.Entities.Count >= 0)
                    lContacts.AddRange(collection.Entities);

                moreRecords = collection.MoreRecords;
                _log.InfoFormat(CultureInfo.InvariantCulture, $"More Records: " + moreRecords);

                if (moreRecords)
                {
                    page++;
                    cookie = string.Format("paging-cookie='{0}' page='{1}'", System.Security.SecurityElement.Escape(collection.PagingCookie), page);
                }

            } while (moreRecords);

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Found: " + lContacts.Count + " Contact of criteria 3.");
            _log.InfoFormat(CultureInfo.InvariantCulture, $"Removing the Contacts from C1 and C2");

            List<Guid> lToRemove = new List<Guid>();
            lToRemove.AddRange(lC1);
            lToRemove.AddRange(lC2);

            _log.InfoFormat(CultureInfo.InvariantCulture, $"There are " + lToRemove.Count + " contacts to remove.");

            foreach (Guid contactid in lToRemove)
            {
                int index = lContacts.FindIndex(x => ((Guid)(((AliasedValue)x["contactid"]).Value)).Equals(contactid));

                if(index != -1)
                {
                    _log.InfoFormat("Index Found: " + index + " on ContactId: " + contactid);
                    lContacts.RemoveAt(index);
                }
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

        public static void HandleContactsC1C2(Plugin.LocalPluginContext localContext, CrmContext crmContext, List<Guid> lGContacts, bool isC1)
        {
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

                        crmContext.Attach(uContact);
                        crmContext.UpdateObject(uContact);
                        _log.InfoFormat("Update Request Added for Contact: " + (Guid)contact.ContactId);
                    }

                    lCompanyRoles.Remove(firstCompanyRole);
                }
                else
                {
                    Contact uContactC2 = new Contact();
                    uContactC2.Id = (Guid)contact.ContactId;
                    uContactC2.ed_BusinessContact = false;

                    crmContext.Attach(uContactC2);
                    crmContext.UpdateObject(uContactC2);
                    _log.InfoFormat("Update Request Added for Contact: " + (Guid)contact.ContactId);
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
                    nContact.ed_PrivateCustomerContact = false;
                    nContact.ed_BusinessContact = true;

                    crmContext.AddObject(nContact);
                    _log.InfoFormat("Create Request Added for Contact: " + contact.FirstName + ", " + contact.LastName);

                    crmContext.Attach(companyRole);
                    crmContext.AddLink(nContact, new Relationship(rel_contact_companyRole), companyRole);
                    _log.InfoFormat("Update Request Added for Company Role: " + (Guid)companyRole.ed_CompanyRoleId);
                }
            }
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

        public static void LogCrmContextMultipleResponses(SaveChangesResultCollection lResponses)
        {
            foreach (SaveChangesResult response in lResponses)
            {
                try
                {
                    // A valid response.
                    if (response.Error == null)
                    {
                        string message = response.Request.RequestName;
                        Contact contact = (Contact)response.Request["Target"];
                        Guid id = Guid.Empty;

                        if(message == "Create")
                        {
                            id = (Guid)response.Response["id"];
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Contact entity with email: " + contact.EMailAddress1 + " was " + message + "d sucessfully. Resulting Id: " + id);
                        }
                        else
                        {
                            _log.InfoFormat(CultureInfo.InvariantCulture, $"The Contact entity with email: " + contact.EMailAddress1 + " was " + message + "d sucessfully.");
                        }
                            
                    }
                    //An error has occurred.
                    else
                    {
                        Contact contact = (Contact)response.Request["Target"];
                        string message = response.Request.RequestName;

                        if(message == "Create")
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Contact entity with email: " + contact.EMailAddress1 + " from Company Role was not created. The Contact: " + contact.FirstName + " " + contact.LastName + " was not split. Details: " + response.Error.Message);
                        }
                        else if(message == "Update")
                        {
                            _log.ErrorFormat(CultureInfo.InvariantCulture, $"The Contact entity with Id: " + contact.ContactId + " was not updated with email: " + contact.EMailAddress1 + ". Details: " + response.Error.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    _log.ErrorFormat(CultureInfo.InvariantCulture, $"Error logging results. Details: " + e.Message);
                }
            }
        }

        public static void RunLogic(Plugin.LocalPluginContext localContext, CrmContext crmContext)
        {
            SaveChangesOptions optionsChanges = SaveChangesOptions.ContinueOnError;

            int active = 0;
            int privateCustomerC1 = 0;
            int numberOfCompanyRolesC1 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC1"]);
            List<Guid> lGContactsC1 = GetContactsCriteriaOneTwo(localContext.OrganizationService, active, privateCustomerC1, numberOfCompanyRolesC1);

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Found " + lGContactsC1.Count + " Contact of Criteria 1.");

            int privateCustomerC2 = 1;
            int numberOfCompanyRolesC2 = int.Parse(ConfigurationManager.AppSettings["numberOfCompanyRolesC2"]);
            List<Guid> lGContactsC2 = GetContactsCriteriaOneTwo(localContext.OrganizationService, active, privateCustomerC2, numberOfCompanyRolesC2);

            _log.InfoFormat(CultureInfo.InvariantCulture, $"Found " + lGContactsC2.Count + " Contact of Criteria 2.");

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
            HandleContactsC1C2(localContext, crmContext, lGContactsC1, true);

            SaveChangesResultCollection responsesC1 = crmContext.SaveChanges(optionsChanges);
            LogCrmContextMultipleResponses(responsesC1);
            crmContext.ClearChanges();

            _log.InfoFormat(CultureInfo.InvariantCulture, "Getting Criteria 2 Logic.");
            HandleContactsC1C2(localContext, crmContext, lGContactsC2, false);

            SaveChangesResultCollection responsesC2 = crmContext.SaveChanges(optionsChanges);
            LogCrmContextMultipleResponses(responsesC2);
            crmContext.ClearChanges();

            _logC3.InfoFormat(CultureInfo.InvariantCulture, "Logging Criteria 3 Contacts.");
            HandleContactsC3(localContext, lGContactsC3);
        }
    }
}
