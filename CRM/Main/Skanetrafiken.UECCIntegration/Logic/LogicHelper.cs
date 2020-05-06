using Endeavor.Crm;
using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
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

        public static List<ContactEntity> GetFilteredContacts(Plugin.LocalPluginContext localContext, int active, bool privateCustomer)
        {
            QueryExpression queryContacts = new QueryExpression(ContactEntity.EntityLogicalName);
            queryContacts.NoLock = true;
            queryContacts.ColumnSet.AddColumns(ContactEntity.Fields.ContactId);
            queryContacts.Criteria.AddCondition(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, privateCustomer);
            queryContacts.Criteria.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, active);

            return XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryContacts);
        }


        public static List<Guid> GetContactsCriteriaOneTwo(IOrganizationService organizationService, int active, int privateCustomer, int countOfCompanyRoles)
        {
            List<Guid> lGContacts = new List<Guid>();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true' no-lock='true' >
                              <entity name='ed_companyrole' >
                                <attribute name='ed_companyroleid' alias='companyrole_count' aggregate='countcolumn' />
                                <link-entity name='contact' from='contactid' to='ed_contact' alias='ab' >
                                  <attribute name='fullname' alias='contactid' groupby='true' />
                                  <filter type='and' >
                                    <condition attribute='ed_privatecustomercontact' operator='eq' value='{0}' />
                                    <condition attribute='statecode' operator='eq' value='{1}' />
                                  </filter>
                                </link-entity>
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
                                  <attribute name='fullname' alias='contactid' groupby='true' />
                                  <filter type='and' >
                                    <condition attribute='statecode' operator='eq' value='{0}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            string getCompanyRoles = string.Format(fetch, active);
            List<Entity> lContacts = organizationService.RetrieveMultiple(new FetchExpression(getCompanyRoles)).Entities.ToList();

            lC1.AddRange(lC2);
            foreach (Guid contactid in lC1)
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

            lGContactsC1.AddRange(lGContactsC2);
            lGContactsC1.AddRange(lGContactsC3);

            foreach (Guid contactid in lGContactsC1)
            {

            }
        }
    }
}
