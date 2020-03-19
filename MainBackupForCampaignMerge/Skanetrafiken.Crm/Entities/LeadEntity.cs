using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm.Entities
{
    public class LeadEntity : Generated.Lead
    {
        //private static string CreateAccountSubject = "Skapa Mitt Konto";

        public static ColumnSet LeadInfoBlock = new ColumnSet(
            LeadEntity.Fields.LeadId,
            LeadEntity.Fields.ed_InformationSource,
            LeadEntity.Fields.FirstName,
            LeadEntity.Fields.LastName,
            LeadEntity.Fields.Address1_Line1,
            LeadEntity.Fields.Address1_Line2,
            LeadEntity.Fields.Address1_PostalCode,
            LeadEntity.Fields.Address1_City,
            LeadEntity.Fields.ed_Address1_Country,
            LeadEntity.Fields.Telephone1,
            LeadEntity.Fields.MobilePhone,
            LeadEntity.Fields.ed_Personnummer,
            LeadEntity.Fields.ed_HasSwedishSocialSecurityNumber,
            LeadEntity.Fields.EMailAddress1);

        public LeadEntity()
        {
        }

        public LeadEntity(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
            FirstName = customerInfo.FirstName;
            LastName = customerInfo.LastName;
            Address1_Line1 = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.CO : null;
            Address1_Line2 = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.Line1 : null;
            Address1_PostalCode = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.PostalCode : null;
            Address1_City = customerInfo.AddressBlock != null ? customerInfo.AddressBlock.City : null;
            ed_Address1_Country = customerInfo.AddressBlock != null ? string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ? null : CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO) : null;
            Telephone1 = customerInfo.Telephone;
            MobilePhone = customerInfo.Mobile;
            ed_Personnummer = customerInfo.SocialSecurityNumber;
            ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
            EMailAddress1 = customerInfo.Email;
        }

        public static StatusBlock CanLeadBeCreated(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            // Check inData
            if (customerInfo == null)
            {
                return new StatusBlock()
                {
                    TransactionOk = false,
                    ErrorMessage = Resources.IncomingDataCannotBeNull,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput
                };
            }
            bool filterAdded = false;
            // Check for conflicting Contact
            FilterExpression contactFilter = new FilterExpression(LogicalOperator.Or);
            if (customerInfo.SocialSecurityNumber != null && customerInfo.SwedishSocialSecurityNumberSpecified && customerInfo.SwedishSocialSecurityNumber)
            {
                FilterExpression persNrFilter = new FilterExpression(LogicalOperator.And);
                persNrFilter.AddCondition(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customerInfo.SocialSecurityNumber);
                persNrFilter.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                persNrFilter.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.NotNull);
                contactFilter.AddFilter(persNrFilter);
                filterAdded = true;
            }
            if (customerInfo.Email != null)
            {
                FilterExpression emailFilter = new FilterExpression(LogicalOperator.And);
                emailFilter.AddCondition(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
                emailFilter.AddCondition(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email);
                contactFilter.AddFilter(emailFilter);
                filterAdded = true;
            }
            IList<ContactEntity> existingConflictingContacts = null;
            if (filterAdded)
                existingConflictingContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.Id), contactFilter);

            if (existingConflictingContacts != null && existingConflictingContacts.Count > 0)
            {
                if (customerInfo.Email == null || !customerInfo.Email.Equals(existingConflictingContacts[0].EMailAddress1, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (customerInfo.SwedishSocialSecurityNumber)
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherContactFound,
                            ErrorMessage = string.Format(Resources.CouldNotCreateCustomerSocSec)
                        };
                }
                else
                {
                    return new StatusBlock()
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherContactFound,
                        ErrorMessage = string.Format(Resources.CouldNotCreateCustomerEmail)
                    };
                }
            }

            // Check for conflicting Lead
            filterAdded = false;
            FilterExpression leadFilter = new FilterExpression(LogicalOperator.Or);
            if (customerInfo.SocialSecurityNumber != null && customerInfo.SwedishSocialSecurityNumberSpecified && customerInfo.SwedishSocialSecurityNumber)
            {
                FilterExpression persNrFilter = new FilterExpression();
                persNrFilter.AddCondition(LeadEntity.Fields.ed_Personnummer, ConditionOperator.Equal, customerInfo.SocialSecurityNumber);
                persNrFilter.AddCondition(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open);
                leadFilter.AddFilter(persNrFilter);
                filterAdded = true;
            }
            if (customerInfo.Email != null)
            {
                FilterExpression leadEmailFilter = new FilterExpression(LogicalOperator.And);
                leadEmailFilter.AddCondition(LeadEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.LeadState.Open);
                leadEmailFilter.AddCondition(LeadEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email);
                //leadEmailNameFilter.AddCondition(LeadEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName);
                //leadEmailNameFilter.AddCondition(LeadEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName);
                leadFilter.AddFilter(leadEmailFilter);
                filterAdded = true;
            }
            IList<LeadEntity> existingConflictingLeads = null;
            if (filterAdded)
                existingConflictingLeads = XrmRetrieveHelper.RetrieveMultiple<LeadEntity>(localContext, LeadInfoBlock, leadFilter);

            if (existingConflictingLeads != null && existingConflictingLeads.Count > 0)
            {
                if (existingConflictingLeads.Count == 1)
                {
                    IEnumerator<LeadEntity> enumerator = existingConflictingLeads.GetEnumerator();
                    enumerator.MoveNext();
                    return new StatusBlock()
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                        Information = enumerator.Current.Id.ToString()
                    };
                }
                else
                {
                    LeadEntity personnummerMatch = null, emailMatch = null;
                    foreach (LeadEntity lead in existingConflictingLeads)
                    {
                        if (lead.ed_Personnummer.Equals(customerInfo.SocialSecurityNumber))
                            personnummerMatch = lead;
                        if (lead.EMailAddress1.Equals(customerInfo.Email, StringComparison.InvariantCultureIgnoreCase))
                            emailMatch = lead;
                    }
                    if (personnummerMatch != null)
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                            Information = personnummerMatch.Id.ToString()
                        };
                    }
                    else if (emailMatch != null)
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.OtherLeadFound,
                            Information = emailMatch.Id.ToString()
                        };
                    }
                    else
                    {
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)CustomerUtility.StatusBlockCode.GenericError,
                            ErrorMessage = Resources.InconsistencyInDatabase
                        };
                    }
                }
            }
            return new StatusBlock()
            {
                TransactionOk = true,
                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.NoConflictingEntity
            };
        }

        public void HandlePreLeadUpdate(Plugin.LocalPluginContext localContext, LeadEntity preImage)
        {
            LeadEntity combined = new LeadEntity();
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

            StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, combined.ToCustomerInfo(localContext));

            if (!validateStatus.TransactionOk)
            {
                throw new Exception(string.Format("Could not Update Lead.\nError message:\n\n{0}", validateStatus.ErrorMessage));
            }
        }

        public void HandlePreLeadCreate(Plugin.LocalPluginContext localContext) { 
            CustomerInfo info = this.ToCustomerInfo(localContext);

            StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, info);

            if (!validateStatus.TransactionOk)
            {
                throw new Exception(string.Format("Could not Create Lead.\nError message:\n\n{0}", validateStatus.ErrorMessage));
            }
        }

        public CustomerInfo ToCustomerInfo(Plugin.LocalPluginContext localContext)
        {
            return new CustomerInfo()
            {
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? null : this.EMailAddress1,
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? null : this.FirstName,
                Guid = this.Id.ToString(),
                LastName = string.IsNullOrWhiteSpace(this.LastName) ? null : this.LastName,
                Mobile = string.IsNullOrWhiteSpace(this.MobilePhone) ? null : this.MobilePhone,
                SocialSecurityNumber = string.IsNullOrWhiteSpace(this.ed_Personnummer) ? null : this.ed_Personnummer,
                Telephone = string.IsNullOrWhiteSpace(this.Telephone1) ? null : this.Telephone1,
                AddressBlock = (
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                this.ed_Address1_Country == null &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode)) ? null : new CustomerInfoAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address1_City) ? null : this.Address1_City,
                    CO = string.IsNullOrWhiteSpace(this.Address1_Line1) ? null : this.Address1_Line1,
                    CountryISO = this.ed_Address1_Country == null ? null : CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id),
                    Line1 = string.IsNullOrWhiteSpace(this.Address1_Line2) ? null : this.Address1_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address1_PostalCode) ? null : this.Address1_PostalCode
                },
                Source = this.ed_InformationSource.HasValue ? (int)this.ed_InformationSource.Value : -1,
                isAddressEnteredManually = true,
                isAddressEnteredManuallySpecified = true,
                isAddressInformationComplete = this.IsCustomerInformationComplete(localContext),
                isAddressInformationCompleteSpecified = true,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? this.ed_HasSwedishSocialSecurityNumber.Value : false,
            };
        }

        public bool UpdateWithCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            bool updated = false;
            if (customerInfo == null)
                return false;
            if (!string.IsNullOrWhiteSpace(customerInfo.FirstName) && !customerInfo.FirstName.Equals(FirstName))
            {
                this.FirstName = customerInfo.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.LastName) && !customerInfo.LastName.Equals(LastName))
            {
                this.LastName = customerInfo.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !customerInfo.Email.Equals(EMailAddress1))
            {
                this.EMailAddress1 = customerInfo.Email;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Telephone) && !customerInfo.Telephone.Equals(Telephone1))
            {
                this.Telephone1 = customerInfo.Telephone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Mobile) && !customerInfo.Mobile.Equals(MobilePhone))
            {
                this.MobilePhone = customerInfo.Mobile;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SocialSecurityNumber.Equals(ed_Personnummer))
            {
                this.ed_Personnummer = customerInfo.SocialSecurityNumber;
                this.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                updated = true;
            }
            if (customerInfo.AddressBlock != null && 
                    (
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CO) && !customerInfo.AddressBlock.CO.Equals(Address1_Line2)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) && !customerInfo.AddressBlock.Line1.Equals(Address1_Line2)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) && !customerInfo.AddressBlock.PostalCode.Equals(Address1_PostalCode)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) && !customerInfo.AddressBlock.City.Equals(Address1_City)) ||
                    (!string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) && (this.ed_Address1_Country == null || !CountryEntity.GetIsoCodeForCountry(localContext, ed_Address1_Country.Id).Equals(customerInfo.AddressBlock.CountryISO)))
                    )
                )
            {
                this.Address1_Line1 = customerInfo.AddressBlock.CO;
                this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                this.Address1_City = customerInfo.AddressBlock.City;
                this.ed_Address1_Country = CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);
                updated = true;
            }
            return updated;
        }

        private bool IsCustomerInformationComplete(Plugin.LocalPluginContext localContext)
        {
            StatusBlock validated = CustomerUtility.ValidateCustomerInfo(localContext, new CustomerInfo()
            {
                AddressBlock = new CustomerInfoAddressBlock()
                {
                    CO = this.Address1_Line1,
                    Line1 = this.Address1_Line2,
                    PostalCode = this.Address1_PostalCode,
                    City = this.Address1_City,
                    CountryISO = this.ed_Address1_Country != null ? CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id): null
                },
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? this.EMailAddress2 : this.EMailAddress1,
                Telephone = this.Telephone1,
                Mobile = this.MobilePhone,
                SocialSecurityNumber = this.ed_Personnummer,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? this.ed_HasSwedishSocialSecurityNumber.Value : false,
                Source = (int)CustomerUtility.Source.SkapaMittKonto
            });
            return validated.TransactionOk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static LeadEntity CreateLead(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo, string LeadSubject)
        {
            if (customerInfo == null)
                return null;
            
            LeadEntity lead = new LeadEntity(localContext, customerInfo);
            lead.Subject = LeadSubject;

            lead.Id = localContext.OrganizationService.Create(lead);
            lead.LeadId = lead.Id;
            return lead;

        }
    }


}