using Endeavor.Crm;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skanetrafiken.UECCIntegration.Logic
{
    public class LogicHelper
    {
        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                    _logC3.InfoFormat(CultureInfo.InvariantCulture, match2?.FirstOrDefault().ToString());
                }
            }

            foreach (Guid contactId in lGContactsC2)
            {
                List<Guid> match1 = lGContactsC1.Where(x => x.Equals(contactId)).ToList();
                List<Guid> match3 = lGContactsC3.Where(x => x.Equals(contactId)).ToList();

                if (match1.Count > 0 || match3.Count > 0)
                {
                    hasInterceptions = true;
                    _logC3.InfoFormat(CultureInfo.InvariantCulture, match1?.FirstOrDefault().ToString());
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
                    _log.ErrorFormat(CultureInfo.InvariantCulture, "The Contact " + contact["contactid"] + " was not processed. Details: " + e.Message);
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

        public static List<Contact> GetContactsFromListGuids(Plugin.LocalPluginContext localContext, List<Guid> lGContacts)
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

        public static void HandleContactsC1C2(Plugin.LocalPluginContext localContext, List<Guid> lGContacts, bool isC1)
        {
            List<Contact> lFinalContacts = GetContactsFromListGuids(localContext, lGContacts);

            foreach (Contact contact in lFinalContacts)
            {
                List<ed_CompanyRole> lCompanyRoles = GetCompanyRolesFromContact(localContext, (Guid)contact.ContactId);

                if (lCompanyRoles.FirstOrDefault() == null)
                {
                    _log.InfoFormat(CultureInfo.InvariantCulture, "The Contact " + contact.ContactId + " does not have Company Roles.");
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

                        XrmHelper.Update(localContext, uContact);
                    }

                    lCompanyRoles.Remove(firstCompanyRole);
                }

                foreach (ed_CompanyRole companyRole in lCompanyRoles)
                {
                    Contact nContact = new Contact();
                    nContact.FirstName = contact.FirstName;
                    nContact.LastName = contact.LastName;
                    nContact.EMailAddress1 = companyRole.ed_EmailAddress;
                    nContact.Telephone2 = companyRole.ed_Telephone;
                    nContact.ed_SocialSecurityNumberBlock = companyRole.ed_SocialSecurityNumber;
                    nContact.ed_InformationSource = ed_informationsource.ForetagsPortal;

                    Guid gNewContact = XrmHelper.Create(localContext, nContact);

                    ed_CompanyRole uCompanyRole = new ed_CompanyRole();
                    uCompanyRole.Id = (Guid)companyRole.ed_CompanyRoleId;
                    uCompanyRole.ed_Contact = new EntityReference(Contact.EntityLogicalName, gNewContact);

                    XrmHelper.Update(localContext, uCompanyRole);
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

        public static void RunLogic(Plugin.LocalPluginContext localContext)
        {
            int active = 0;
            int privateCustomerC1 = 0;
            int numberOfCompanyRolesC1 = 2;
            List<Guid> lGContactsC1 = GetContactsCriteriaOneTwo(localContext.OrganizationService, active, privateCustomerC1, numberOfCompanyRolesC1);

            int privateCustomerC2 = 1;
            int numberOfCompanyRolesC2 = 1;
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

            _log.InfoFormat(CultureInfo.InvariantCulture, "Starting Criteria 1 Logic.");
            HandleContactsC1C2(localContext, lGContactsC1, true);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Ending Criteria 1 Logic.");

            _log.InfoFormat(CultureInfo.InvariantCulture, "Starting Criteria 2 Logic.");
            HandleContactsC1C2(localContext, lGContactsC2, false);
            _log.InfoFormat(CultureInfo.InvariantCulture, "Ending Criteria 2 Logic.");

            _logC3.InfoFormat(CultureInfo.InvariantCulture, "Starting Criteria 3 Logic.");
            HandleContactsC3(localContext, lGContactsC3);
            _logC3.InfoFormat(CultureInfo.InvariantCulture, "Ending Criteria 3 Logic.");
        }
    }
}
