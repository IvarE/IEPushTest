
using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Schema.Generated;
using System;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class CompanyRoleEntity : ed_CompanyRole
    {
        public static ColumnSet CompanyRoleInfoBlock = new ColumnSet(
                CompanyRoleEntity.Fields.ed_name,
                CompanyRoleEntity.Fields.ed_Account,
                CompanyRoleEntity.Fields.ed_Contact,
                CompanyRoleEntity.Fields.ed_FirstName,
                CompanyRoleEntity.Fields.ed_LastName,
                CompanyRoleEntity.Fields.ed_EmailAddress,
                CompanyRoleEntity.Fields.ed_Telephone,
                CompanyRoleEntity.Fields.ed_Role,
                CompanyRoleEntity.Fields.ed_SocialSecurityNumber,
                CompanyRoleEntity.Fields.ed_isLockedPortal
            );

        private static string GetCompanyRoleName(AccountEntity account, ContactEntity contact, CustomerInfo customerInfo)
        {
            string roleName = $"{contact.FirstName} {contact.LastName}";

            return roleName;
        }

        internal static CompanyRoleEntity GetUpdateCompanyRole(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            bool updated = false;
            if (customerInfo == null)
                return null;

            if (customerInfo.CompanyRole == null)
                return null;

            if (customerInfo.CompanyRole.Length == 0)
                return null;

            if (customerInfo.CompanyRole[0] == null)
                return null;

            CustomerInfoCompanyRole companyRole = customerInfo.CompanyRole[0];
            
            // 2020-01-31 - Marcus Stenswed - We have a guid to the existing Contact, use that instead of matching on email. It's coming from PUT Contact
            //ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); //Check - Match

            ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, new Guid(customerInfo.Guid), new ColumnSet(false));


            AccountEntity account = AccountEntity.FindAccount(localContext, companyRole.PortalId, AccountEntity.AccountInfoBlock);

            IList<CompanyRoleEntity> roles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, CompanyRoleEntity.CompanyRoleInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.Equal, account.AccountId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.ContactId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Role, ConditionOperator.Equal, companyRole.CompanyRole),
                                        new ConditionExpression(CompanyRoleEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_CompanyRoleState.Active)
                            }
                        });

            if (roles == null || roles.Count == 0)
            {
                throw new Exception($"Role {Generated.ed_companyrole_ed_role.Administrator.ToString()} on {account.Name} for contact {contact.FullName} does not exist.");
            }

            if (roles != null && roles.Count > 1)
            {
                throw new Exception($"Found multiple roles {companyRole.CompanyRole} on {account.Name} for contact {contact.FullName}.");
            }

            CompanyRoleEntity oldRole = roles[0];

            CompanyRoleEntity updateRole = new CompanyRoleEntity
            {
                ed_CompanyRoleId = oldRole.ed_CompanyRoleId,
                Id = oldRole.Id
            };
            
            string roleName = GetCompanyRoleName(account, contact, customerInfo);
            if (!string.IsNullOrWhiteSpace(roleName) && !roleName.Equals(oldRole.ed_name))
            {
                localContext.TracingService.Trace("Updating Role Name");
                updateRole.ed_name = roleName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(companyRole.PortalId) && !companyRole.PortalId.Equals(account.AccountNumber))
            {
                localContext.TracingService.Trace("Updating Account");
                updateRole.ed_Account = account.ToEntityReference();
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SocialSecurityNumber.Equals(contact.cgi_socialsecuritynumber))
            {
                localContext.TracingService.Trace("Updating Contact");
                updateRole.ed_Contact = contact.ToEntityReference();
                updated = true;
            }
            //if (!string.IsNullOrWhiteSpace(contact.FirstName) && !contact.FirstName.Equals(oldRole.ed_FirstName))
            //{
            //    updateRole.ed_FirstName = customerInfo.FirstName;
            //    updated = true;
            //}
            //if (!string.IsNullOrWhiteSpace(contact.LastName) && !contact.LastName.Equals(oldRole.ed_LastName))
            //{
            //    updateRole.ed_LastName = customerInfo.LastName;
            //    updated = true;
            //}
            if (!string.IsNullOrWhiteSpace(companyRole.Email) && !companyRole.Email.Equals(oldRole.ed_EmailAddress))
            {
                localContext.TracingService.Trace("Updating Email");
                updateRole.ed_EmailAddress = companyRole.Email;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(companyRole.Telephone) && !companyRole.Telephone.Equals(oldRole.ed_Telephone))
            {
                localContext.TracingService.Trace("Updating Telephone");
                updateRole.ed_Telephone = companyRole.Telephone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SocialSecurityNumber.Equals(oldRole.ed_SocialSecurityNumber))
            {
                localContext.TracingService.Trace("Updating Social Security Number");
                updateRole.ed_SocialSecurityNumber = customerInfo.SocialSecurityNumber;
                updated = true;
            }

            if (companyRole.isLockedPortalSpecified && !companyRole.isLockedPortal.Equals(oldRole.ed_isLockedPortal))
            {
                localContext.TracingService.Trace("Updating IsLockedPortal");
                updateRole.ed_isLockedPortal = companyRole.isLockedPortal;
                updated = true;
            }

            if (oldRole.ed_Role == null && companyRole.CompanyRole > -1 && companyRole.CompanyRole < Enum.GetValues(typeof(Generated.ed_companyrole_ed_role)).Length)
            {
                updateRole.ed_Role = (Generated.ed_companyrole_ed_role)companyRole.CompanyRole;
                updated = true;
            }

            if (companyRole.deleteCompanyRole == true)
            {
                SetStateRequest req = new SetStateRequest
                {
                    EntityMoniker = updateRole.ToEntityReference(),
                    State = new OptionSetValue((int)Generated.ed_CompanyRoleState.Inactive),
                    Status = new OptionSetValue(2)
                };
                try
                {
                    SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
                    updated = false;
                }
                catch (Exception e)
                {
                    localContext.Trace($"Unexpected exception caught when attempting to inactivate CompanyRole with Id: {updateRole.Id}, exception message:\n{e.Message}");
                    throw e;
                }
            }
            
            if (updated)
                return updateRole;
            else
                return null;
        }

        /// <summary>
        /// Creates contact if SSN is not found. Gets existing account or returns an error if account does not exist.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        internal static Guid CreateNewCompanyRole(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            CustomerInfoCompanyRole role = customerInfo.CompanyRole[0];

            ContactEntity contact = ContactEntity.FindOrCreateUnvalidatedContact(localContext, customerInfo); //Matchning sker här!!
            AccountEntity account = AccountEntity.FindAccount(localContext, role.PortalId, new ColumnSet(AccountEntity.Fields.Name, AccountEntity.Fields.ParentAccountId));

            IList<CompanyRoleEntity> roles;

            if (contact.ContactId != null)
            {
                roles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, new ColumnSet(false),
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.Equal, account.AccountId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.ContactId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Role, ConditionOperator.Equal, role.CompanyRole),
                                        new ConditionExpression(CompanyRoleEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_CompanyRoleState.Active)
                                }
                            });
            }
            else
            {
                roles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>(localContext, new ColumnSet(false),
                            new FilterExpression()
                            {
                                Conditions =
                                {
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Account, ConditionOperator.Equal, account.AccountId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, contact.ContactId),
                                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Role, ConditionOperator.Equal, role.CompanyRole),
                                        new ConditionExpression(CompanyRoleEntity.Fields.statecode, ConditionOperator.Equal, (int)Generated.ed_CompanyRoleState.Active)
                                }
                            });
            }

            if (roles != null && roles.Count > 0)
            {
                // 2020-01-31 - TODO Marcus Stenswed
                // This throw exception returns a 500 error, maybe return a 400 BadRequest instead?
                throw new Exception($"Role {Generated.ed_companyrole_ed_role.Administrator.ToString()} on {account.Name} already exists.");
            }
            
            CompanyRoleEntity newRole = new CompanyRoleEntity()
            {
                ed_name = GetCompanyRoleName(account, contact, customerInfo),
                ed_Account = account.ToEntityReference(),
                ed_Contact = contact.ToEntityReference(),
                ed_FirstName = customerInfo.FirstName,
                ed_LastName = customerInfo.LastName,
                ed_EmailAddress = role.Email,
                ed_Telephone = role.Telephone,
                ed_Role = role.CompanyRoleSpecified ? (Crm.Schema.Generated.ed_companyrole_ed_role?)role.CompanyRole : null,
                ed_SocialSecurityNumber = customerInfo.SocialSecurityNumber
            };

            XrmHelper.Create(localContext, newRole);


            // Connect Contact to Accounts (level 1 and 2)
            // Create an AssociateEntities request.

            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, account.AccountId.Value, ContactEntity.EntityLogicalName, contact.ContactId.Value))
            {
                //Namespace is Microsoft.Crm.Sdk.Messages
                AssociateEntitiesRequest requestCostSiteRel = new AssociateEntitiesRequest();

                // Set the ID of Moniker1 to the ID of the lead.
                requestCostSiteRel.Moniker1 = new EntityReference { Id = account.AccountId.Value, LogicalName = AccountEntity.EntityLogicalName };

                // Set the ID of Moniker2 to the ID of the contact.
                requestCostSiteRel.Moniker2 = new EntityReference { Id = contact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                // Set the relationship name to associate on.
                requestCostSiteRel.RelationshipName = "cgi_account_contact";

                // Execute the request.
                localContext.OrganizationService.Execute(requestCostSiteRel);
            }

            if (!DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, account.ParentAccountId.Id, ContactEntity.EntityLogicalName, contact.ContactId.Value))
            {
                //Namespace is Microsoft.Crm.Sdk.Messages
                AssociateEntitiesRequest requestOrgRel = new AssociateEntitiesRequest();

                // Set the ID of Moniker1 to the ID of the lead.
                requestOrgRel.Moniker1 = new EntityReference { Id = account.ParentAccountId.Id, LogicalName = AccountEntity.EntityLogicalName };

                // Set the ID of Moniker2 to the ID of the contact.
                requestOrgRel.Moniker2 = new EntityReference { Id = contact.ContactId.Value, LogicalName = ContactEntity.EntityLogicalName };

                // Set the relationship name to associate on.
                requestOrgRel.RelationshipName = "cgi_account_contact";

                // Execute the request.
                localContext.OrganizationService.Execute(requestOrgRel);
            }
            //// Execute the request.
            //localContext.OrganizationService.Execute(requestOrgRel);

            return contact.Id;
        }

        public static bool DoesRelationshipExist(Plugin.LocalPluginContext localContext, string relationshipSchemaName, string entity1Schema, Guid entity1GuidValue, string entity2Schema, Guid entity2GuidValue)
        {
            string fetchXml = "<fetch mapping='logical'> <entity name='" + relationshipSchemaName + "'>"
              + "<all-attributes />"
              + "<filter>"
              + "<condition attribute='" + entity1Schema + "id' operator='eq' value ='" + entity1GuidValue.ToString() + "' />"
              + "<condition attribute='" + entity2Schema + "id' operator='eq' value='" + entity2GuidValue.ToString() + "' />"
              + "</filter>"
              + "</entity>"
              + "</fetch>";

            //string fetchResult = localContact.
            var fetchResult = localContext.OrganizationService.RetrieveMultiple(new FetchExpression(fetchXml));

            if (fetchResult.Entities.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal static CustomerInfoCompanyRole GetRoleInfoFromCompanyRole(Plugin.LocalPluginContext localContext, CompanyRoleEntity companyRole)
        {
            IList<AccountEntity> account = XrmRetrieveHelper.RetrieveMultiple<AccountEntity>(localContext, new ColumnSet(AccountEntity.Fields.AccountNumber),
                        new FilterExpression()
                        {
                            Conditions =
                            {
                                        new ConditionExpression(AccountEntity.Fields.AccountId, ConditionOperator.Equal, companyRole.ed_Account.Id)
                            }
                        });

            if (account == null || account.Count != 1)
            {
                throw new Exception($"Multiple accounts found for company role.");
            }

            CustomerInfoCompanyRole companyRoleInfo = new CustomerInfoCompanyRole()
            {
                PortalId = account[0].AccountNumber,
                CompanyRole = companyRole.ed_Role != null ? (int)companyRole.ed_Role : -1,
                CompanyRoleSpecified = companyRole.ed_Role.HasValue,
                Email = companyRole.ed_EmailAddress,
                Telephone = companyRole.ed_Telephone,
                isLockedPortal = companyRole.ed_isLockedPortal.HasValue ? companyRole.ed_isLockedPortal.Value : false,
                isLockedPortalSpecified = companyRole.ed_isLockedPortal.HasValue
            };
            
            return companyRoleInfo;
        }

        internal void HandlePostCompanyRoleCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                LeadEntity companyLead = new LeadEntity();
                companyLead.EMailAddress1 = this.ed_EmailAddress;
                companyLead.MobilePhone = this.ed_Telephone;
                companyLead.ed_Personnummer = this.ed_SocialSecurityNumber;
                companyLead.FirstName = this.ed_FirstName;
                companyLead.LastName = this.ed_LastName;
                companyLead.LeadSourceCode = lead_leadsourcecode.Foretagslead;

                CompanyRoleEntity cre = new CompanyRoleEntity();
                cre.Id = this.Id;

                companyLead.ed_CompanyEngagement = cre.ToEntityReference();

                XrmHelper.Create(localContext.OrganizationService, companyLead);
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostContactCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

    }

}