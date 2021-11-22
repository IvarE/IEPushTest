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
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Globalization;
using Skanetrafiken.Crm;
using System.Linq;
using Skanetrafiken.Crm.Entities;
using System.Net.Http;
using System.IdentityModel;

namespace Skanetrafiken.Crm.Entities
{
    public class ContactEntity : Generated.Contact
    {
        //private static string MklEndpoint = "http://stmkltest-staging.azurewebsites.net";
        //private static string MklEndpoint = "http://stmkltest.azurewebsites.net";
        public static string _NEWEMAILDONE = "_NEWEMAILDONE_";

        public static ColumnSet ContactInfoBlock = new ColumnSet(
            ContactEntity.Fields.ed_InformationSource,
            ContactEntity.Fields.FirstName,
            ContactEntity.Fields.LastName,
            ContactEntity.Fields.StateCode,
            ContactEntity.Fields.Address1_Line1,
            ContactEntity.Fields.Address1_Line2,
            ContactEntity.Fields.Address1_PostalCode,
            ContactEntity.Fields.Address1_City,
            ContactEntity.Fields.ed_Address1_Country,
            ContactEntity.Fields.Telephone1,
            ContactEntity.Fields.Telephone2,
            ContactEntity.Fields.cgi_socialsecuritynumber,
            ContactEntity.Fields.BirthDate,
            ContactEntity.Fields.ed_HasSwedishSocialSecurityNumber,
            ContactEntity.Fields.EMailAddress1,
            ContactEntity.Fields.EMailAddress2,
            ContactEntity.Fields.ed_MklId,
            ContactEntity.Fields.ed_EmailToBeVerified,
            ContactEntity.Fields.ed_isLockedPortal,
            ContactEntity.Fields.cgi_activated,
            ContactEntity.Fields.cgi_allow_autoload,
            ContactEntity.Fields.ed_LinkExpiryDate,
            ContactEntity.Fields.FullName,
            ContactEntity.Fields.ed_BusinessContact,
            ContactEntity.Fields.ed_InfotainmentContact,
            ContactEntity.Fields.ed_SchoolContact,
            ContactEntity.Fields.ed_PrivateCustomerContact,
            ContactEntity.Fields.ed_SeniorContact,
            ContactEntity.Fields.ed_AgentContact,
            ContactEntity.Fields.ed_SocialSecurityNumberBlock,
            ContactEntity.Fields.CreatedOn,
            ContactEntity.Fields.ed_CollaborationContact,
            ContactEntity.Fields.ed_Reseller,
            ContactEntity.Fields.ContactId
                );


        public ContactEntity()
        {

        }

        /// <summary>
        /// Additional constructor to speed up object creation.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        public ContactEntity(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo, FeatureTogglingEntity feature = null)
        {

            if (customerInfo != null)
            {
                if (customerInfo.Source > -1)
                    this.ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
                if (!string.IsNullOrWhiteSpace(customerInfo.FirstName))
                    this.FirstName = customerInfo.FirstName;
                if (!string.IsNullOrWhiteSpace(customerInfo.LastName))
                    this.LastName = customerInfo.LastName;

                if (customerInfo.isLockedPortalSpecified)
                    this.ed_isLockedPortal = customerInfo.isLockedPortal;

                if (customerInfo.Source != (int)Generated.ed_informationsource.ForetagsPortal &&
                    customerInfo.Source != (int)Generated.ed_informationsource.SkolPortal &&
                    customerInfo.Source != (int)Generated.ed_informationsource.SeniorPortal)
                {
                    if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                    {
                        this.cgi_socialsecuritynumber = customerInfo.SocialSecurityNumber;
                        this.BirthDate = ContactEntity.UpdateBirthDateOnContact(customerInfo.SocialSecurityNumber); //DevOps 9168
                    }

                    if (customerInfo.SwedishSocialSecurityNumberSpecified)
                        this.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;

                    if (!string.IsNullOrWhiteSpace(customerInfo.Telephone))
                        this.Telephone1 = customerInfo.Telephone;
                    if (!string.IsNullOrWhiteSpace(customerInfo.Mobile))
                        this.Telephone2 = customerInfo.Mobile;
                    if (!string.IsNullOrWhiteSpace(customerInfo.Email))
                        this.EMailAddress1 = customerInfo.Email;
                    if (!string.IsNullOrWhiteSpace(customerInfo.MklId))
                        this.ed_MklId = customerInfo.MklId;
                    if (customerInfo.AddressBlock?.CO != null ||
                        customerInfo.AddressBlock?.Line1 != null ||
                        customerInfo.AddressBlock?.PostalCode != null ||
                        customerInfo.AddressBlock?.City != null ||
                        customerInfo.AddressBlock?.CountryISO != null)
                    {
                        this.Address1_Line1 = customerInfo.AddressBlock.CO;
                        this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                        this.Address1_City = customerInfo.AddressBlock.City;
                        this.ed_Address1_Country = string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ? null : CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);
                        this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                    }
                }
                else if ((customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal ||
                    customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal ||
                    customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal) && (feature == null || (feature != null && feature.ed_SplittCompany == false))) //Old Logic
                {
                    if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone))
                        this.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email))
                        this.EMailAddress1 = customerInfo.CompanyRole[0].Email;
                    if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        this.ed_SocialSecurityNumberBlock = customerInfo.SocialSecurityNumber;
                }
                else if ((customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal ||
                    customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal ||
                    customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal) && feature != null && feature.ed_SplittCompany == true) //New Logic (2020-05-06) : Should we use?
                {
                    if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone))
                        this.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email))
                        this.EMailAddress1 = customerInfo.CompanyRole[0].Email;
                    if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        this.ed_SocialSecurityNumberBlock = customerInfo.SocialSecurityNumber;

                    if (customerInfo.AddressBlock?.CO != null ||
                        customerInfo.AddressBlock?.Line1 != null ||
                        customerInfo.AddressBlock?.PostalCode != null ||
                        customerInfo.AddressBlock?.City != null ||
                        customerInfo.AddressBlock?.CountryISO != null)
                    {
                        this.Address1_Line1 = customerInfo.AddressBlock.CO;
                        this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                        this.Address1_City = customerInfo.AddressBlock.City;
                        this.ed_Address1_Country = string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ? null : CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);
                        this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                    }
                }
            }
        }

        internal void HandlePostContactCreateAsync(Plugin.LocalPluginContext localContext)
        {
            try
            {
                if (Generated.ContactState.Active.Equals(this.StateCode) && !string.IsNullOrWhiteSpace(this.cgi_socialsecuritynumber) && this.ed_HasSwedishSocialSecurityNumber == true)
                {
                    // Retrieve existing Contacts with same Social Security Number and statecode active
                    IList<DeltabatchQueueEntity> activeDeltabatchPlusQueues = DeltabatchQueueEntity.RetrieveDeltabatchQueuesWithSameSocialSecurityNumber(
                        localContext,
                        this.cgi_socialsecuritynumber,
                        Generated.ed_DeltabatchQueueState.Active,
                        Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus);

                    // If there are no active matches. Add a deltabatch plus queue entity
                    if (activeDeltabatchPlusQueues == null || activeDeltabatchPlusQueues.Count() == 0)
                    {
                        DeltabatchQueueEntity queue = new DeltabatchQueueEntity
                        {
                            ed_Contact = this.ToEntityReference(),
                            ed_ContactGuid = this.ContactId?.ToString(),
                            ed_ContactNumber = this.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                            ed_name = $"Create: {this.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Create: {this.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Create: {this.FullName}, {DateTime.Now.ToString()}"
                        };
                        queue.Id = XrmHelper.Create(localContext.OrganizationService, queue);
                    }
                }
            }
            catch (Exception e)
            {
                localContext.Trace($"HandlePostContactCreateAsync threw an unexpected exception: {e.Message}");
                throw e;
            }
        }

        internal void HandlePostContactDeleteAsync(Plugin.LocalPluginContext localContext, EntityReference target)
        {
            try
            {
                if (Generated.ContactState.Active.Equals(this.StateCode) && !string.IsNullOrWhiteSpace(this.cgi_socialsecuritynumber) && this.ed_HasSwedishSocialSecurityNumber == true)
                {

                    // Retrieve existing Contacts with same Social Security Number and statecode active
                    IList<ContactEntity> activeMatches = ContactEntity.RetrieveContactsWithSameSocialSecurityNumber(localContext, this.Id, this.cgi_socialsecuritynumber, Generated.ContactState.Active);

                    // If there are no active matches. Add a deltabatch minus queue entity
                    if (activeMatches == null || activeMatches.Count() == 0)
                    {

                        IList<DeltabatchQueueEntity> existing = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, new ColumnSet(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation,
                            DeltabatchQueueEntity.Fields.CreatedOn),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, this.cgi_socialsecuritynumber)
                                }
                            });
                        if (existing?.Count > 0)
                        {
                            DeltabatchQueueEntity latest = null;
                            foreach (DeltabatchQueueEntity q in existing)
                            {
                                if (latest == null || q.CreatedOn.Value.CompareTo(latest.CreatedOn.Value) > 0)
                                {
                                    latest = q;
                                }
                            }
                            if (latest.ed_DeltabatchOperation == Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus)
                            {
                                return;
                            }
                        }

                        DeltabatchQueueEntity queue = new DeltabatchQueueEntity
                        {
                            ed_Contact = this.ToEntityReference(),
                            ed_ContactGuid = this.ContactId?.ToString(),
                            ed_ContactNumber = this.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus,
                            ed_name = $"Delete!: {this.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Delete!: {this.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Delete!: {this.FullName}, {DateTime.Now.ToString()}"
                        };
                        queue.Id = XrmHelper.Create(localContext.OrganizationService, queue);
                    }
                    // If there are existing Contacts with same Social Security Number =>
                    // Update existing delta queue entity record with new Contact.
                    else if (activeMatches != null && activeMatches.Count() > 0)
                    {
                        IList<DeltabatchQueueEntity> existing = XrmRetrieveHelper.RetrieveMultiple<DeltabatchQueueEntity>(localContext, new ColumnSet(DeltabatchQueueEntity.Fields.ed_DeltabatchOperation, DeltabatchQueueEntity.Fields.CreatedOn),
                            new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression(DeltabatchQueueEntity.Fields.ed_ContactNumber, ConditionOperator.Equal, this.cgi_socialsecuritynumber)
                                }
                            });

                        if (existing?.Count > 0)
                        {
                            DeltabatchQueueEntity latest = null;
                            foreach (DeltabatchQueueEntity q in existing)
                            {
                                if (latest == null || q.CreatedOn.Value.CompareTo(latest.CreatedOn.Value) > 0)
                                {
                                    latest = q;
                                }
                            }
                            if (latest.ed_DeltabatchOperation == Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus)
                            {
                                return;
                            }
                            else
                            {
                                DeltabatchQueueEntity deltaBatchUpd = new DeltabatchQueueEntity();
                                deltaBatchUpd.Id = latest.Id;
                                deltaBatchUpd.ed_Contact = this.ToEntityReference();
                                XrmHelper.Update(localContext, deltaBatchUpd);
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void HandlePostContactUpdateAsync(Plugin.LocalPluginContext localContext, ContactEntity preImage)
        {
            ContactEntity combined = new ContactEntity();
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);
            combined.Id = this.Id;

            try
            {
                if (combined.ed_DoNotUpdateFB == true)
                {
                    if (preImage.ed_DoNotUpdateFB != true && combined.ed_HasSwedishSocialSecurityNumber == true)
                    {
                        DeltabatchQueueEntity minusQueue1 = new DeltabatchQueueEntity
                        {
                            ed_Contact = combined.ToEntityReference(),
                            ed_ContactGuid = combined.ContactId?.ToString(),
                            ed_ContactNumber = combined.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus,
                            ed_name = $"StopUpdate - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"StopUpdate - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"StopUpdate - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}"
                        };
                        minusQueue1.Id = XrmHelper.Create(localContext.OrganizationService, minusQueue1);

                        if (combined.cgi_socialsecuritynumber != preImage.cgi_socialsecuritynumber && preImage.ed_HasSwedishSocialSecurityNumber == true)
                        {
                            DeltabatchQueueEntity minusQueue = new DeltabatchQueueEntity
                            {
                                ed_Contact = preImage.ToEntityReference(),
                                ed_ContactGuid = preImage.ContactId?.ToString(),
                                ed_ContactNumber = preImage.cgi_socialsecuritynumber,
                                ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus,
                                ed_name = $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}"
                            };
                            minusQueue.Id = XrmHelper.Create(localContext.OrganizationService, minusQueue);
                        }
                    }
                }
                else if (combined.cgi_socialsecuritynumber != preImage.cgi_socialsecuritynumber)
                {
                    if (combined.StateCode.Equals(Generated.ContactState.Active) && !string.IsNullOrWhiteSpace(combined.cgi_socialsecuritynumber) && combined.ed_HasSwedishSocialSecurityNumber == true)
                    {
                        DeltabatchQueueEntity plusQueue = new DeltabatchQueueEntity
                        {
                            ed_Contact = combined.ToEntityReference(),
                            ed_ContactGuid = combined.ContactId?.ToString(),
                            ed_ContactNumber = combined.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                            ed_name = $"Update - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Update - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Update - SocSecNr: {combined.FullName}, {DateTime.Now.ToString()}"
                        };
                        plusQueue.Id = XrmHelper.Create(localContext.OrganizationService, plusQueue);
                    }
                    if (!string.IsNullOrWhiteSpace(preImage.cgi_socialsecuritynumber) && preImage.ed_HasSwedishSocialSecurityNumber == true)
                    {
                        DeltabatchQueueEntity minusQueue = new DeltabatchQueueEntity
                        {
                            ed_Contact = preImage.ToEntityReference(),
                            ed_ContactGuid = preImage.ContactId?.ToString(),
                            ed_ContactNumber = preImage.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus,
                            ed_name = $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Update - SocSecNr: {preImage.FullName}, {DateTime.Now.ToString()}"
                        };
                        minusQueue.Id = XrmHelper.Create(localContext.OrganizationService, minusQueue);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="leadInfo"></param>
        /// <returns></returns>
        internal ContactEntity CreateUpdateEntityFromAuthorityInfo(Plugin.LocalPluginContext localContext, LeadInfo leadInfo)
        {
            ContactEntity updateEntity = new ContactEntity
            {
                Id = this.Id,
                ContactId = this.Id
            };
            bool update = false;

            if (!string.IsNullOrWhiteSpace(leadInfo.FirstName) && !leadInfo.FirstName.Equals(FirstName))
            {
                updateEntity.FirstName = leadInfo.FirstName;
                update = true;
            }
            if (!string.IsNullOrWhiteSpace(leadInfo.LastName) && !leadInfo.LastName.Equals(LastName))
            {
                updateEntity.LastName = leadInfo.LastName;
                update = true;
            }
            if (!string.IsNullOrWhiteSpace(leadInfo.Telephone) && !leadInfo.Telephone.Equals(Telephone1))
            {
                updateEntity.Telephone1 = leadInfo.Telephone;
                update = true;
            }
            if (!string.IsNullOrWhiteSpace(leadInfo.Mobile) && !leadInfo.Mobile.Equals(Telephone2))
            {
                updateEntity.Telephone2 = leadInfo.Mobile;
                update = true;
            }

            if (leadInfo.AddressBlock != null &&
                !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.Line1) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.PostalCode) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.City) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CountryISO)))
            {
                if ((!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO) && !leadInfo.AddressBlock.CO.Equals(Address1_Line1)) ||
                    !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO) && string.IsNullOrWhiteSpace(Address1_Line1)))
                {
                    updateEntity.Address1_Line1 = leadInfo.AddressBlock.CO;
                    update = true;
                }
                if ((!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.Line1) && !leadInfo.AddressBlock.Line1.Equals(Address1_Line2)) ||
                    !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.Line1) && string.IsNullOrWhiteSpace(Address1_Line2)))
                {
                    updateEntity.Address1_Line2 = leadInfo.AddressBlock.Line1;
                    update = true;
                }
                if ((!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.PostalCode) && !leadInfo.AddressBlock.PostalCode.Equals(Address1_PostalCode)) ||
                    !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.PostalCode) && string.IsNullOrWhiteSpace(Address1_PostalCode)))
                {
                    updateEntity.Address1_PostalCode = leadInfo.AddressBlock.PostalCode;
                    update = true;
                }

                if ((!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO) && !leadInfo.AddressBlock.CO.Equals(Address1_City)) ||
                    !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.City) && string.IsNullOrWhiteSpace(Address1_City)))
                {
                    updateEntity.Address1_City = leadInfo.AddressBlock.City;
                    update = true;
                }

                if (!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CountryISO))
                {
                    EntityReference countryRef = CountryUtility.GetEntityRefForCountryCode(localContext, leadInfo.AddressBlock.CountryISO);
                    if ((countryRef != null && countryRef != ed_Address1_Country) ||
                        (countryRef == null && ed_Address1_Country != null))
                    {
                        updateEntity.ed_Address1_Country = countryRef;
                        update = true;
                    }
                }

            }

            if (!string.IsNullOrWhiteSpace(leadInfo.Email) && !string.IsNullOrWhiteSpace(EMailAddress1))
            {
                if (!leadInfo.Email.Equals(EMailAddress2))
                {
                    updateEntity.EMailAddress2 = leadInfo.Email;
                    update = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(leadInfo.SocialSecurityNumber) && !leadInfo.SocialSecurityNumber.Equals(cgi_socialsecuritynumber))
            {
                updateEntity.cgi_socialsecuritynumber = leadInfo.SocialSecurityNumber;
                updateEntity.BirthDate = ContactEntity.UpdateBirthDateOnContact(leadInfo.SocialSecurityNumber); //DevOps 9168
                update = true;
                if (leadInfo.SwedishSocialSecurityNumberSpecified && leadInfo.SwedishSocialSecurityNumber != ed_HasSwedishSocialSecurityNumber)
                {
                    updateEntity.ed_HasSwedishSocialSecurityNumber = leadInfo.SwedishSocialSecurityNumber;
                }
            }

            if (update)
                return updateEntity;
            return null;
        }

        internal ContactEntity CreateUpdateEntityFromInfo(Plugin.LocalPluginContext localContext, LeadInfo leadInfo)
        {
            ContactEntity updateEntity = new ContactEntity
            {
                Id = this.Id,
                ContactId = this.Id
            };
            bool update = false;

            if (string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(leadInfo.FirstName))
            {
                updateEntity.FirstName = leadInfo.FirstName;
                update = true;
            }
            if (string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(leadInfo.LastName))
            {
                updateEntity.LastName = leadInfo.LastName;
                update = true;
            }
            if (string.IsNullOrWhiteSpace(Telephone1) && !string.IsNullOrWhiteSpace(leadInfo.Telephone))
            {
                updateEntity.Telephone1 = leadInfo.Telephone;
                update = true;
            }
            if (string.IsNullOrWhiteSpace(Telephone2) && !string.IsNullOrWhiteSpace(leadInfo.Mobile))
            {
                updateEntity.Telephone2 = leadInfo.Mobile;
                update = true;
            }

            /*
            if (string.IsNullOrWhiteSpace(Address1_Line1) &&
                string.IsNullOrWhiteSpace(Address1_Line2) &&
                string.IsNullOrWhiteSpace(Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(Address1_City) &&
                ed_Address1_Country == null &&
                leadInfo.AddressBlock != null &&
                !(string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.Line1) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.PostalCode) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.City) &&
                string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CountryISO)))
            {
                if (!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.CO))
                {
                    updateEntity.Address1_Line1 = leadInfo.AddressBlock.CO;
                    update = true;
                }
                if (!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.Line1))
                {
                    updateEntity.Address1_Line2 = leadInfo.AddressBlock.Line1;
                    update = true;
                }
                if (!string.IsNullOrWhiteSpace(leadInfo.AddressBlock.PostalCode))
                {
                    updateEntity.Address1_PostalCode = leadInfo.AddressBlock.PostalCode;
                    update = true;
                }
                if (string.IsNullOrWhiteSpace(leadInfo.AddressBlock.City))
                {
                    updateEntity.Address1_City = leadInfo.AddressBlock.City;
                    update = true;
                }
                EntityReference countryRef = CountryUtility.GetEntityRefForCountryCode(localContext, leadInfo.AddressBlock.CountryISO);
                if (countryRef != null)
                {
                    updateEntity.ed_Address1_Country = countryRef;
                    update = true;
                }
            }*/

            if (leadInfo.AddressBlock != null)
            {
                if (string.IsNullOrWhiteSpace(this.Address1_Line1) || !this.Address1_Line1.Equals(leadInfo.AddressBlock.CO))
                {
                    updateEntity.Address1_Line1 = leadInfo.AddressBlock.CO;
                    update = true;
                }

                if (string.IsNullOrWhiteSpace(this.Address1_Line1) || !this.Address1_Line1.Equals(leadInfo.AddressBlock.Line1))
                {
                    updateEntity.Address1_Line2 = leadInfo.AddressBlock.Line1;
                    update = true;
                }

                if (string.IsNullOrWhiteSpace(this.Address1_PostalCode) || !this.Address1_PostalCode.Equals(leadInfo.AddressBlock.PostalCode))
                {
                    updateEntity.Address1_PostalCode = leadInfo.AddressBlock.PostalCode;
                    update = true;
                }

                if (string.IsNullOrWhiteSpace(this.Address1_City) || !this.Address1_City.Equals(leadInfo.AddressBlock.City))
                {
                    updateEntity.Address1_City = leadInfo.AddressBlock.City;
                    update = true;
                }

                if (leadInfo.AddressBlock.CountryISO != null)
                {
                    EntityReference countryRef = CountryUtility.GetEntityRefForCountryCode(localContext, leadInfo.AddressBlock.CountryISO);
                    if (countryRef != null && (this.ed_Address1_Country == null || !this.ed_Address1_Country.Equals(countryRef)))
                    {
                        updateEntity.ed_Address1_Country = countryRef;
                        update = true;
                    }
                }
                else if (this.ed_Address1_Country != null)
                {
                    updateEntity.ed_Address1_Country = null;
                    update = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(EMailAddress1) && !string.IsNullOrWhiteSpace(EMailAddress2) && !string.IsNullOrWhiteSpace(leadInfo.Email))
            {
                updateEntity.EMailAddress2 = leadInfo.Email;
                update = true;
            }
            if (string.IsNullOrWhiteSpace(cgi_socialsecuritynumber) && !string.IsNullOrWhiteSpace(leadInfo.SocialSecurityNumber))
            {
                updateEntity.cgi_socialsecuritynumber = leadInfo.SocialSecurityNumber;
                updateEntity.BirthDate = ContactEntity.UpdateBirthDateOnContact(leadInfo.SocialSecurityNumber); //DevOps 9168

                update = true;
                if (leadInfo.SwedishSocialSecurityNumberSpecified && leadInfo.SwedishSocialSecurityNumber != ed_HasSwedishSocialSecurityNumber)
                {
                    updateEntity.ed_HasSwedishSocialSecurityNumber = leadInfo.SwedishSocialSecurityNumber;
                }
            }

            if (update)
                return updateEntity;
            return null;
        }

        internal static void HandlePostContactSetStateAsync(Plugin.LocalPluginContext localContext, EntityReference myEntityRef, OptionSetValue state, OptionSetValue status)
        {
            try
            {
                ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, myEntityRef, new ColumnSet(ContactEntity.Fields.StateCode, ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.FullName, ContactEntity.Fields.ContactId, ContactEntity.Fields.ed_HasSwedishSocialSecurityNumber));

                if (!string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber) && contact.ed_HasSwedishSocialSecurityNumber == true)
                {
                    switch (contact.StateCode)
                    {
                        case Generated.ContactState.Active:

                            // Retrieve existing Contacts with same Social Security Number and statecode active
                            IList<DeltabatchQueueEntity> activeDeltabatchPlusQueues = DeltabatchQueueEntity.RetrieveDeltabatchQueuesWithSameSocialSecurityNumber(
                                localContext,
                                contact.cgi_socialsecuritynumber,
                                Generated.ed_DeltabatchQueueState.Active,
                                Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus);

                            // If there are no active matches. Add a deltabatch plus queue entity
                            if (activeDeltabatchPlusQueues == null || activeDeltabatchPlusQueues.Count() == 0)
                            {

                                DeltabatchQueueEntity plusQueue = new DeltabatchQueueEntity
                                {
                                    ed_Contact = contact.ToEntityReference(),
                                    ed_ContactGuid = contact.ContactId?.ToString(),
                                    ed_ContactNumber = contact.cgi_socialsecuritynumber,
                                    ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                                    ed_name = $"Update - Activated: {contact.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Update - Activated: {contact.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Update - Activated: {contact.FullName}, {DateTime.Now.ToString()}"
                                };
                                plusQueue.Id = XrmHelper.Create(localContext.OrganizationService, plusQueue);

                            }
                            break;
                        case Generated.ContactState.Inactive:

                            // Retrieve existing Contacts with same Social Security Number and statecode inactive
                            IList<ContactEntity> activeMatches = ContactEntity.RetrieveContactsWithSameSocialSecurityNumber(localContext, myEntityRef.Id, contact.cgi_socialsecuritynumber, Generated.ContactState.Active);

                            // If there are no inactive matches. Add a deltabatch minus queue entity
                            if (activeMatches == null || activeMatches.Count() == 0)
                            {

                                DeltabatchQueueEntity minusQueue = new DeltabatchQueueEntity
                                {
                                    ed_Contact = contact.ToEntityReference(),
                                    ed_ContactGuid = contact.ContactId?.ToString(),
                                    ed_ContactNumber = contact.cgi_socialsecuritynumber,
                                    ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Minus,
                                    ed_name = $"Update - Deactivated: {contact.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"Update - Deactivated: {contact.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"Update - Deactivated: {contact.FullName}, {DateTime.Now.ToString()}"
                                };
                                minusQueue.Id = XrmHelper.Create(localContext.OrganizationService, minusQueue);

                            }
                            break;
                        default:
                            throw new Exception($"Unrecognised StateCode for Contact {contact.StateCode}");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static IList<ContactEntity> RetrieveContactsWithSameSocialSecurityNumber(Plugin.LocalPluginContext localContext, Guid contactId, string socialSecurityNumber, Generated.ContactState state)
        {
            IList<ContactEntity> contactMatches = new List<ContactEntity>();

            if (socialSecurityNumber != "")
            {
                contactMatches = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.Id),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, socialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)state),
                            new ConditionExpression(ContactEntity.Fields.Id, ConditionOperator.NotEqual, contactId)
                        }
                    });
            }

            return contactMatches;
        }

        public ContactInfo ToContactInfo(Plugin.LocalPluginContext localContext)
        {
            return new ContactInfo()
            {
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? (string.IsNullOrWhiteSpace(this.EMailAddress2) ? null : this.EMailAddress2) : this.EMailAddress1,
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? null : this.FirstName,
                Guid = this.Id.ToString(),
                MklId = this.ed_MklId,
                LastName = string.IsNullOrWhiteSpace(this.LastName) ? null : this.LastName,
                Mobile = string.IsNullOrWhiteSpace(this.Telephone2) ? null : this.Telephone2,
                SocialSecurityNumber = string.IsNullOrWhiteSpace(this.cgi_socialsecuritynumber) ? null : this.cgi_socialsecuritynumber,
                Telephone = string.IsNullOrWhiteSpace(this.Telephone1) ? null : this.Telephone1,
                AddressBlock = (
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode) &&
                this.ed_Address1_Country == null) ? null : new CustomerInfoAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address1_City) ? null : this.Address1_City,
                    CO = string.IsNullOrWhiteSpace(this.Address1_Line1) ? null : this.Address1_Line1,
                    CountryISO = this.ed_Address1_Country == null ? null : CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id),
                    Line1 = string.IsNullOrWhiteSpace(this.Address1_Line2) ? null : this.Address1_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address1_PostalCode) ? null : this.Address1_PostalCode
                },
                SpecAddressBlock = (
                string.IsNullOrWhiteSpace(this.Address2_City) &&
                string.IsNullOrWhiteSpace(this.Address2_Line1) &&
                string.IsNullOrWhiteSpace(this.Address2_Line2) &&
                string.IsNullOrWhiteSpace(this.Address2_PostalCode) &&
                string.IsNullOrWhiteSpace(this.Address2_Country)
                ) ? null : new ContactInfoSpecAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address2_City) ? null : this.Address2_City,
                    CO = string.IsNullOrWhiteSpace(this.Address2_Line1) ? null : this.Address2_Line1,
                    CountryName = string.IsNullOrWhiteSpace(this.Address2_Country) ? null : this.Address2_Country,
                    Line1 = string.IsNullOrWhiteSpace(this.Address2_Line2) ? null : this.Address2_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address2_PostalCode) ? null : this.Address2_PostalCode
                },
                isEmailChangeInProgress = this.IsEmailChangeInProgress(localContext),
                isEmailChangeInProgressSpecified = true,
                isLockedPortal = this.ed_isLockedPortal.HasValue ? this.ed_isLockedPortal.Value : false,
                isLockedPortalSpecified = this.ed_isLockedPortal.HasValue,
                isAddressInformationComplete = this.IsCustomerInformationComplete(localContext),
                isAddressInformationCompleteSpecified = true,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? ed_HasSwedishSocialSecurityNumber.Value : false,
                SwedishSocialSecurityNumberSpecified = this.ed_HasSwedishSocialSecurityNumber.HasValue,
                isAddressEnteredManually = this.ed_InformationSource.HasValue && this.ed_InformationSource == Generated.ed_informationsource.Folkbokforing ? false : true,
                isAddressEnteredManuallySpecified = this.ed_InformationSource.HasValue,
                Source = this.ed_InformationSource.HasValue ? (int)this.ed_InformationSource.Value : -1,
                NewEmail = this.ed_EmailToBeVerified,
                Utvandrad = Generated.ed_creditsaferejectcodes.Emigrated == this.ed_CreditsafeRejectionCode,
                UtvandradSpecified = true,
                Avliden = Generated.ed_creditsaferejectcodes.Deceased == this.ed_CreditsafeRejectionCode,
                AvlidenSpecified = true,
                CreditsafeOk = this.ed_CreditsafeRejectionCode != null,
                CreditsafeOkSpecified = true,
            };
        }

        public CustomerInfo ToCustomerInfo(Plugin.LocalPluginContext localContext)
        {
            return new CustomerInfo()
            {
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? (string.IsNullOrWhiteSpace(this.EMailAddress2) ? null : this.EMailAddress2) : this.EMailAddress1,
                FirstName = string.IsNullOrWhiteSpace(this.FirstName) ? null : this.FirstName,
                Guid = this.Id.ToString(),
                LastName = string.IsNullOrWhiteSpace(this.LastName) ? null : this.LastName,
                Mobile = string.IsNullOrWhiteSpace(this.Telephone2) ? null : this.Telephone2,
                MklId = string.IsNullOrWhiteSpace(this.ed_MklId) ? null : this.ed_MklId,
                SocialSecurityNumber = string.IsNullOrWhiteSpace(this.cgi_socialsecuritynumber) ? null : this.cgi_socialsecuritynumber,
                Telephone = string.IsNullOrWhiteSpace(this.Telephone1) ? null : this.Telephone1,
                AddressBlock = (
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode) &&
                this.ed_Address1_Country == null) ? null : new CustomerInfoAddressBlock()
                {
                    City = string.IsNullOrWhiteSpace(this.Address1_City) ? null : this.Address1_City,
                    CO = string.IsNullOrWhiteSpace(this.Address1_Line1) ? null : this.Address1_Line1,
                    CountryISO = this.ed_Address1_Country == null ? null : CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id),
                    Line1 = string.IsNullOrWhiteSpace(this.Address1_Line2) ? null : this.Address1_Line2,
                    PostalCode = string.IsNullOrWhiteSpace(this.Address1_PostalCode) ? null : this.Address1_PostalCode
                },
                isAddressInformationComplete = this.IsCustomerInformationComplete(localContext),
                isAddressInformationCompleteSpecified = true,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? ed_HasSwedishSocialSecurityNumber.Value : false,
                SwedishSocialSecurityNumberSpecified = this.ed_HasSwedishSocialSecurityNumber.HasValue,
                isAddressEnteredManually = this.ed_InformationSource.HasValue && this.ed_InformationSource == Generated.ed_informationsource.Folkbokforing ? false : true,
                isAddressEnteredManuallySpecified = this.ed_InformationSource.HasValue,
                Source = this.ed_InformationSource.HasValue ? (int)this.ed_InformationSource.Value : -1,
                NewEmail = this.ed_EmailToBeVerified,
            };
        }

        public void HandlePreValidationContactDelete(Plugin.LocalPluginContext localContext)
        {
            localContext.Trace("Entered HandlePreContactUpdate() Tracing preImage");
            this.Trace(localContext.TracingService);

            this.ThrowMissingProperties(localContext.TracingService, ContactEntity.Fields.StateCode);

            if (!this.StateCode.Equals(Generated.ContactState.Inactive))
            {
                throw new InvalidPluginExecutionException("Kund måste vara inaktiverad för att få bli raderad");
            }
        }

        private bool IsEmailChangeInProgress(Plugin.LocalPluginContext localContext)
        {
            int validityHours = 0;
            try
            {
                validityHours = CgiSettingEntity.GetSettingInt(localContext, CgiSettingEntity.Fields.ed_LeadValidityHours);
            }
            catch (Exception e)
            {
                throw new Exception("Could not-...", e);
            }

            if (this.ed_LinkExpiryDate != null && this.ed_LinkExpiryDate > DateTime.Now && this.ed_EmailToBeVerified != null && this.ed_EmailToBeVerified != _NEWEMAILDONE)
                return true;
            else
                return false;
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
                    CountryISO = this.ed_Address1_Country != null ? CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id) : null
                },
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = string.IsNullOrWhiteSpace(this.EMailAddress1) ? this.EMailAddress2 : this.EMailAddress1,
                Telephone = this.Telephone1,
                Mobile = this.Telephone2,
                SocialSecurityNumber = this.cgi_socialsecuritynumber,
                SwedishSocialSecurityNumber = this.ed_HasSwedishSocialSecurityNumber.HasValue ? this.ed_HasSwedishSocialSecurityNumber.Value : false,
                Source = (int)Crm.Schema.Generated.ed_informationsource.SkapaMittKonto
            });
            return validated.TransactionOk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        public void HandlePreContactCreate(Plugin.LocalPluginContext localContext)
        {
            this.Trace(localContext.TracingService);

            localContext.Trace($"Entered HandlePreContactCreate() Postal Code value: {Address1_PostalCode}");
            if (!string.IsNullOrEmpty(Address1_PostalCode))
            {
                QueryExpression queryPostalCodes = new QueryExpression(PostalCodesEntity.EntityLogicalName);
                queryPostalCodes.NoLock = true;
                queryPostalCodes.ColumnSet.AddColumns(PostalCodesEntity.Fields.ed_Kommun, PostalCodesEntity.Fields.ed_Kommunkod, PostalCodesEntity.Fields.ed_Lan,
                    PostalCodesEntity.Fields.ed_Lanskod, PostalCodesEntity.Fields.ed_name, PostalCodesEntity.Fields.ed_Postort);
                queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postnummer, ConditionOperator.Equal, Address1_PostalCode);

                List<PostalCodesEntity> lPostalCodes = XrmRetrieveHelper.RetrieveMultiple<PostalCodesEntity>(localContext, queryPostalCodes);

                if (lPostalCodes.Count > 1)
                    localContext.Trace($"HandlePreContactCreate(): Found multiple Postal Codes with ZIP Code: {Address1_PostalCode}");
                else if (lPostalCodes.Count == 0)
                    localContext.Trace($"HandlePreContactCreate(): No Postal Codes with ZIP Code: {Address1_PostalCode}");
                else
                {
                    PostalCodesEntity ePostalCode = lPostalCodes.FirstOrDefault();

                    this.ed_Address1_Community = ePostalCode.ed_Kommun;
                    this.ed_Address1_CommunityNumber = int.Parse(ePostalCode.ed_Kommunkod);
                    this.Address1_County = ePostalCode.ed_Lan;
                    this.ed_Address1_CountyNumber = int.Parse(ePostalCode.ed_Lanskod);
                    this.Address1_Name = ePostalCode.ed_name;
                    this.Address1_City = ePostalCode.ed_Postort;
                }
            }

            localContext.Trace($"Entered HandlePreContactCreate() Social Security Number: { cgi_socialsecuritynumber }");
            if (!string.IsNullOrEmpty(cgi_socialsecuritynumber) && cgi_socialsecuritynumber.Length == 12 && BirthDate == null)
            {
                int year = int.Parse(cgi_socialsecuritynumber.Substring(0, 4));
                int month = int.Parse(cgi_socialsecuritynumber.Substring(4, 2));
                int day = int.Parse(cgi_socialsecuritynumber.Substring(6, 2));

                if(year != 0 && month != 0 && day != 0)
                    this.BirthDate = new DateTime(year, month, day);
                else
                    localContext.Trace($"Year, Month or Day is 0. Birthday is still null.");
            }

            if (this.ed_Address1_Country != null)
                localContext.Trace(CountryEntity.GetIsoCodeForCountry(localContext, this.ed_Address1_Country.Id));

            localContext.Trace("Formatting address fields, phone numbers and mail addresses");
            FirstName = Capitalise(FirstName);
            LastName = Capitalise(LastName);
            Address1_City = Capitalise(Address1_City);
            Address1_Line2 = Capitalise(Address1_Line2);
            Address2_City = Capitalise(Address2_City);
            Address2_Line2 = Capitalise(Address2_Line2);

            if (!string.IsNullOrWhiteSpace(EMailAddress1))
                EMailAddress1 = EMailAddress1.ToLower().Trim(" ".ToCharArray());
            if (!string.IsNullOrWhiteSpace(EMailAddress2))
                EMailAddress2 = EMailAddress2.ToLower().Trim(" ".ToCharArray());
            if (!string.IsNullOrWhiteSpace(Telephone1))
                Telephone1 = Telephone1.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(Telephone2))
                Telephone2 = Telephone2.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(Telephone3))
                Telephone3 = Telephone3.Replace(" ", "");

            CustomerInfo info = this.ToContactInfo(localContext);

            StatusBlock validateStatus = CustomerUtility.ValidateCustomerInfo(localContext, info);

            if (!validateStatus.TransactionOk)
            {
                string errorMess = string.Format(Properties.Resources.CouldNotCreateContact, validateStatus.ErrorMessage);
                localContext.Trace(errorMess);
                throw new InvalidPluginExecutionException(errorMess);
            }

            //[2020-09-06]: Added validaton by contact Type
            bool wasAlreadyValidated = ValidateDuplicatesByContactType(localContext, true);

            if (this.ed_InformationSource == Generated.ed_informationsource.AdmSkapaKund && wasAlreadyValidated == false)
            {
                if (!Contains(ContactEntity.Fields.EMailAddress1) && !string.IsNullOrWhiteSpace(this.EMailAddress1))
                    throw new Exception(Properties.Resources.AdminCantCreateValidatedContact);
                VerifyNoDuplicates(localContext, Generated.ed_informationsource.AdmSkapaKund);
            }

            //This is not used anymore?
            if (Contains(ContactEntity.Fields.OriginatingLeadId) && OriginatingLeadId != null && !Guid.Empty.Equals(OriginatingLeadId.Id))
                this.SetLeadSourceCodeIfNeeded(localContext);
        }

        public void HandlePreContactUpdate(Plugin.LocalPluginContext localContext, ContactEntity preImage)
        {
            if (preImage == null)
                throw new InvalidPluginExecutionException("PreImage not registered correctly");

            localContext.Trace($"Entered HandlePreContactUpdate() Postal Code value: {Address1_PostalCode}");
            if (!string.IsNullOrEmpty(Address1_PostalCode))
            {
                QueryExpression queryPostalCodes = new QueryExpression(PostalCodesEntity.EntityLogicalName);
                queryPostalCodes.NoLock = true;
                queryPostalCodes.ColumnSet.AddColumns(PostalCodesEntity.Fields.ed_Kommun, PostalCodesEntity.Fields.ed_Kommunkod, PostalCodesEntity.Fields.ed_Lan,
                    PostalCodesEntity.Fields.ed_Lanskod, PostalCodesEntity.Fields.ed_name, PostalCodesEntity.Fields.ed_Postort);
                queryPostalCodes.Criteria.AddCondition(PostalCodesEntity.Fields.ed_Postnummer, ConditionOperator.Equal, Address1_PostalCode);

                List<PostalCodesEntity> lPostalCodes = XrmRetrieveHelper.RetrieveMultiple<PostalCodesEntity>(localContext, queryPostalCodes);

                if (lPostalCodes.Count > 1)
                    localContext.Trace($"HandlePreContactUpdate(): Found multiple Postal Codes with ZIP Code: {Address1_PostalCode}");
                else if (lPostalCodes.Count == 0)
                    localContext.Trace($"HandlePreContactUpdate(): No Postal Codes with ZIP Code: {Address1_PostalCode}");
                else
                {
                    PostalCodesEntity ePostalCode = lPostalCodes.FirstOrDefault();

                    this.ed_Address1_Community = ePostalCode.ed_Kommun;
                    this.ed_Address1_CommunityNumber = int.Parse(ePostalCode.ed_Kommunkod);
                    this.Address1_County = ePostalCode.ed_Lan;
                    this.ed_Address1_CountyNumber = int.Parse(ePostalCode.ed_Lanskod);
                    this.Address1_Name = ePostalCode.ed_name;
                    this.Address1_City = ePostalCode.ed_Postort;
                }
            }

            localContext.Trace("Formatting address fields, phone numbers and mail addresses");
            if (Contains(ContactEntity.Fields.FirstName))
                FirstName = Capitalise(FirstName);
            if (Contains(ContactEntity.Fields.LastName))
                LastName = Capitalise(LastName);
            if (Contains(ContactEntity.Fields.Address1_City))
                Address1_City = Capitalise(Address1_City);
            if (Contains(ContactEntity.Fields.Address1_Line2))
                Address1_Line2 = Capitalise(Address1_Line2);
            if (Contains(ContactEntity.Fields.Address2_City))
                Address2_City = Capitalise(Address2_City);
            if (Contains(ContactEntity.Fields.Address2_Line2))
                Address2_Line2 = Capitalise(Address2_Line2);

            if (!string.IsNullOrWhiteSpace(EMailAddress1))
                EMailAddress1 = EMailAddress1.ToLower().Trim(' ');
            if (!string.IsNullOrWhiteSpace(EMailAddress2))
                EMailAddress2 = EMailAddress2.ToLower().Trim(' ');
            if (!string.IsNullOrWhiteSpace(Telephone1))
                Telephone1 = Telephone1.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(Telephone2))
                Telephone2 = Telephone2.Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(Telephone3))
                Telephone3 = Telephone3.Replace(" ", "");

            ContactEntity combined = new ContactEntity();
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

            localContext.Trace("Entered HandlePreContactUpdate() Tracing target and preImage");
            this.Trace(localContext.TracingService);
            preImage.Trace(localContext.TracingService);

            SyncSwedishSocialSecurityNumber(combined.ed_HasSwedishSocialSecurityNumber);

            // ---- Validering av information ----
            // Är något av de fält som valideras med i target?       
            bool doValidation = false;
            foreach (string column in ContactEntity.ContactInfoBlock.Columns)
            {
                if (column.Equals(ContactEntity.Fields.StateCode))
                    continue;
                if (Contains(column))
                {
                    doValidation = true;
                    break;
                }
            }
            if (!doValidation)
                return;


            if (combined.StateCode.HasValue && combined.StateCode.Value == Generated.ContactState.Inactive)
            {
                localContext.Trace("Contact inactive, returning");
                return;
            }

            if (ed_InformationSource == Generated.ed_informationsource.AdmAndraKund && !string.IsNullOrWhiteSpace(EMailAddress1))
                throw new ApplicationException(Properties.Resources.AdminCantValidateContact);

            CustomerInfo info = combined.ToContactInfo(localContext);
            StatusBlock status = CustomerUtility.ValidateCustomerInfo(localContext, info);

            if (!status.TransactionOk)
            {
                string errorMess = string.Format(Properties.Resources.CouldNotUpdateContact, status.ErrorMessage);
                localContext.Trace(errorMess);
                throw new InvalidPluginExecutionException(errorMess);
            }

            if (ed_InformationSource == Generated.ed_informationsource.BytEpost && ed_EmailToBeVerified == ContactEntity._NEWEMAILDONE)
                ed_EmailToBeVerified = null;

            //[2020-09-06]: Added validaton by contact Type
            bool wasAlreadyValidated = ValidateDuplicatesByContactType(localContext, false, combined);

            // Validera dubletter.
            if (combined.ed_InformationSource == Generated.ed_informationsource.AdmAndraKund && wasAlreadyValidated == false)
                combined.VerifyNoDuplicates(localContext, Generated.ed_informationsource.AdmAndraKund);

            if (combined.ed_InformationSource == Generated.ed_informationsource.Folkbokforing && wasAlreadyValidated == false)
                combined.VerifyNoDuplicates(localContext, Generated.ed_informationsource.Folkbokforing);
        }

        private void SetLeadSourceCodeIfNeeded(Plugin.LocalPluginContext localContext)
        {
            LeadEntity originLead = XrmRetrieveHelper.RetrieveFirst<LeadEntity>(localContext, new ColumnSet(LeadEntity.Fields.LeadSourceCode),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(LeadEntity.Fields.LeadId, ConditionOperator.Equal, OriginatingLeadId.Id)
                    }
                });
            if (originLead != null && originLead.LeadSourceCode != null && originLead.LeadSourceCode.HasValue)
            {
                this.LeadSourceCode = new OptionSetValue((int)originLead.LeadSourceCode.Value);
            }
        }

        public bool ValidateDuplicatesByContactType(Plugin.LocalPluginContext localContext, bool isCreate, ContactEntity combined = null)
        {
            bool wasAlreadyValidated = false;
            localContext.Trace("Start ValidateDuplicatesByContactType");
            ContactEntity contactToProcess = combined != null ? combined : this;

            ColumnSet contactTypeFields = new ColumnSet(
                ContactEntity.Fields.ed_CollaborationContact,
                ContactEntity.Fields.ed_AgentContact,
                ContactEntity.Fields.ed_Reseller,
                ContactEntity.Fields.ed_BusinessContact,
                ContactEntity.Fields.ed_SchoolContact,
                ContactEntity.Fields.ed_SeniorContact,
                ContactEntity.Fields.ed_InfotainmentContact,
                ContactEntity.Fields.ed_PrivateCustomerContact,
                ContactEntity.Fields.EMailAddress1,
                ContactEntity.Fields.EMailAddress2,
                ContactEntity.Fields.cgi_socialsecuritynumber
                );

            if (!this.Attributes.Any(x => contactTypeFields.Columns.Any(y => y == x.Key)))
            {
                localContext.Trace("Target does not contains any Type field. Return...");
                return wasAlreadyValidated;
            }

            if (
                contactToProcess.ed_PrivateCustomerContact == true &&
                (!string.IsNullOrEmpty(contactToProcess.EMailAddress1)) // || !string.IsNullOrEmpty(contactToProcess.EMailAddress2)
                )
            {
                localContext.Trace("CheckPrivateContactsByEmail");
                CheckPrivateContactsByEmail(localContext, contactToProcess.EMailAddress1, isCreate); // , contactToProcess.EMailAddress2
                //Should not validate again through old functionality
                wasAlreadyValidated = true;
            }
            //25/01/2021 - Chris: New rule where we can have multiple COntacts with same SSN in cgi_socialsecuritynumber (DevOps: 3292)
            //else if (!string.IsNullOrEmpty(contactToProcess.cgi_socialsecuritynumber) && 
            //    (contactToProcess.ed_CollaborationContact == true || contactToProcess.ed_AgentContact == true ||
            //    contactToProcess.ed_Reseller == true || contactToProcess.ed_BusinessContact == true ||
            //    contactToProcess.ed_SchoolContact == true || contactToProcess.ed_SeniorContact == true ||
            //    contactToProcess.ed_InfotainmentContact == true))
            //{
            //    localContext.Trace("CheckContactsBySocialSecurityNumber");
            //    CheckContactsBySocialSecurityNumber(localContext, contactToProcess.cgi_socialsecuritynumber, isCreate);
            //    //Should still validate through old functionality
            //    wasAlreadyValidated = false;
            //}

            localContext.Trace("End ValidateDuplicatesByContactType");
            return wasAlreadyValidated;
        }

        private void CheckPrivateContactsByEmail(Plugin.LocalPluginContext localContext, string email1, bool isCreate) // string email2,
        {
            QueryExpression query = new QueryExpression(LogicalName);
            query.NoLock = true;
            query.ColumnSet.AddColumn(Fields.ContactId);
            query.Criteria.AddCondition(Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
            query.Criteria.AddCondition(Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true);

            if (!isCreate)
                query.Criteria.AddCondition(Fields.ContactId, ConditionOperator.NotEqual, this.Id);

            var filterByEmail = new FilterExpression();
            query.Criteria.AddFilter(filterByEmail);
            filterByEmail.FilterOperator = LogicalOperator.Or;

            if (!string.IsNullOrEmpty(email1))
            {
                var filterByEmail1 = new FilterExpression();
                filterByEmail.AddFilter(filterByEmail1);

                // Define filter QEcontact_Criteria_0_0
                filterByEmail1.FilterOperator = LogicalOperator.Or;
                filterByEmail1.AddCondition(Fields.EMailAddress1, ConditionOperator.Equal, email1);
                //filterByEmail1.AddCondition(Fields.EMailAddress2, ConditionOperator.Equal, email1);
            }

            //if (!string.IsNullOrEmpty(email2))
            //{
            //    var filterByEmail2 = new FilterExpression();
            //    filterByEmail.AddFilter(filterByEmail2);

            //    filterByEmail2.FilterOperator = LogicalOperator.Or;
            //    filterByEmail2.AddCondition(Fields.EMailAddress1, ConditionOperator.Equal, email2);
            //    filterByEmail2.AddCondition(Fields.EMailAddress2, ConditionOperator.Equal, email2);
            //}

            ContactEntity duplicate = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);



            if (duplicate != null) //temporarily removed to allow duplicate contacts
            {

                throw new InvalidPluginExecutionException(
                    isCreate ?
                    string.Format(Properties.Resources.CouldNotCreateCustomerEmail) :
                    string.Format(Properties.Resources.CouldNotUpdateCustomerEmail));
            }
        }

        private void CheckContactsBySocialSecurityNumber(Plugin.LocalPluginContext localContext, string socialNumber, bool isCreate)
        {
            QueryExpression query = new QueryExpression(LogicalName);
            query.NoLock = true;
            query.ColumnSet.AddColumn(Fields.ContactId);
            query.Criteria.AddCondition(Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active);
            query.Criteria.AddCondition(Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, socialNumber);

            ContactEntity duplicate = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

            if (duplicate != null)
            {

                throw new InvalidPluginExecutionException(
                    isCreate ?
                    string.Format(Properties.Resources.CouldNotCreateCustomerSocSec) :
                    string.Format(Properties.Resources.CouldNotUpdateContactSocialSecurityConflict));
            }
        }

        private void SyncSwedishSocialSecurityNumber(bool? combinedHasSweSocSecNr)
        {
            if (Contains(ContactEntity.Fields.cgi_socialsecuritynumber))
            {
                if (string.IsNullOrWhiteSpace(cgi_socialsecuritynumber))
                {
                    if (combinedHasSweSocSecNr != null)
                        ed_HasSwedishSocialSecurityNumber = null;
                }
                else
                {
                    Regex regexSweSocSec = new Regex("^\\d{12}$");
                    Regex regexDateLong = new Regex("^\\d{8}$");
                    Regex regexDateShort = new Regex("^\\d{6}$");
                    if (regexSweSocSec.IsMatch(cgi_socialsecuritynumber))
                    {
                        if (cgi_socialsecuritynumber.Substring(11).Equals(CustomerUtility.calculateCheckDigit(cgi_socialsecuritynumber.Substring(2, 9))))
                        {
                            if (combinedHasSweSocSecNr != true)
                                ed_HasSwedishSocialSecurityNumber = true;
                        }
                        else
                        {
                            throw new InvalidPluginExecutionException("Ogiltigt personnummer.");
                        }
                    }
                    else if (regexDateLong.IsMatch(cgi_socialsecuritynumber))
                    {
                        DateTime date = DateTime.Now;
                        if (DateTime.TryParseExact(cgi_socialsecuritynumber, "yyyyMMdd", new CultureInfo("sv-SE"), DateTimeStyles.None, out date))
                        {
                            if (combinedHasSweSocSecNr != false)
                                ed_HasSwedishSocialSecurityNumber = false;
                        }
                        else
                        {
                            throw new InvalidPluginExecutionException("Ogiltigt födelsedatum.");
                        }
                    }
                    else if (regexDateShort.IsMatch(cgi_socialsecuritynumber))
                    {
                        DateTime date = DateTime.Now;
                        if (DateTime.TryParseExact(cgi_socialsecuritynumber, "yyMMdd", new CultureInfo("sv-SE"), DateTimeStyles.None, out date))
                        {
                            if (combinedHasSweSocSecNr != false)
                                ed_HasSwedishSocialSecurityNumber = false;
                        }
                        else
                        {
                            throw new InvalidPluginExecutionException("Ogiltigt födelsedatum.");
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Ogiltigt format på personnummer.<BR>Giltiga format: ååmmdd, ååååmmdd, ååååmmddxxxx, ååååmmdd-xxxx");
                    }
                }
            }
        }

        public void HandlePreContactMerge(Plugin.LocalPluginContext localContext, ContactEntity subordinate, Entity updateContentData)
        {
            if (this.ed_MklId != null && subordinate.ed_MklId != null)
            {
                if (this.CreatedOn < subordinate.CreatedOn && !string.IsNullOrEmpty(this.st_originating_source))
                    updateContentData.Attributes[ContactEntity.Fields.st_originating_source] = this.st_originating_source;
                else if (!string.IsNullOrEmpty(subordinate.st_originating_source))
                    updateContentData.Attributes[ContactEntity.Fields.st_originating_source] = subordinate.st_originating_source;
            }
        }

        public void HandlePostContactUpdate(Plugin.LocalPluginContext localContext, ContactEntity preImage)
        {
            ContactEntity combined = new ContactEntity();
            if (preImage != null)
                combined.CombineAttributes(preImage);
            combined.CombineAttributes(this);

            if (Contains(ContactEntity.Fields.cgi_socialsecuritynumber) &&
                combined.Contains(ContactEntity.Fields.EMailAddress1) && !string.IsNullOrWhiteSpace(combined.EMailAddress1) &&
                combined.Contains(ContactEntity.Fields.cgi_socialsecuritynumber) && !string.IsNullOrWhiteSpace(combined.cgi_socialsecuritynumber))
            {
                IList<ContactEntity> mergeContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(false),
                    new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_SocialSecurityNumber2, ConditionOperator.Equal, combined.cgi_socialsecuritynumber)
                        }
                    });
                if (mergeContacts.Count > 0)
                {
                    combined.CombineContacts(localContext, mergeContacts);
                }
            }
        }

        private void VerifyNoDuplicates(Plugin.LocalPluginContext localContext, Generated.ed_informationsource infoSource)
        {
            localContext.Trace($"Entered VerifyNoDuplicates.");
            if (infoSource != Generated.ed_informationsource.AdmAndraKund && infoSource != Generated.ed_informationsource.AdmSkapaKund && infoSource != Generated.ed_informationsource.Folkbokforing)
            {
                localContext.Trace($"Wrong information source for this duplicateVerification.");
                return;
            }

            if (!Contains(ContactEntity.Fields.EMailAddress2) /*&& !(Contains(ContactEntity.Fields.cgi_socialsecuritynumber) && ed_HasSwedishSocialSecurityNumber.HasValue && ed_HasSwedishSocialSecurityNumber.Value)*/)
            {
                localContext.Trace($"Contact does not contain EMailAddress2. No duplicate check will be done.");
                return;
            }

            FilterExpression conflictFilter = new FilterExpression(LogicalOperator.Or);
            //25/01/2021 - Chris: New rule where we can have multiple COntacts with same SSN in cgi_socialsecuritynumber (DevOps: 3292)
            //if (Contains(ContactEntity.Fields.cgi_socialsecuritynumber) && ed_HasSwedishSocialSecurityNumber.HasValue && ed_HasSwedishSocialSecurityNumber.Value)
            //{
            //    FilterExpression socSecFilter = new FilterExpression(LogicalOperator.And)
            //    {
            //        Conditions =
            //            {
            //                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
            //                new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, cgi_socialsecuritynumber),
            //                new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.NotEqual, ContactId)
            //            }
            //    };
            //    conflictFilter.AddFilter(socSecFilter);
            //}
            if (Contains(ContactEntity.Fields.EMailAddress2))
            {
                FilterExpression emailFilter = new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, EMailAddress2),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, EMailAddress2)
                        }
                };
                FilterExpression nameFilter = new FilterExpression(LogicalOperator.Or)
                {
                    Filters =
                        {
                            new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, FirstName),
                                    new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, LastName)
                                }
                            }
                        },
                    Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Ange"),
                            new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                        }
                };
                FilterExpression emailNameFilter = new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                        new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.NotEqual, ContactId)
                    },
                    Filters =
                        {
                            emailFilter,
                            nameFilter
                        }
                };
                conflictFilter.AddFilter(emailNameFilter);
            }

            ContactEntity conflict = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.EMailAddress2, ContactEntity.Fields.EMailAddress1, ContactEntity.Fields.FirstName, ContactEntity.Fields.LastName), conflictFilter);
            if (conflict != null)
            {
                if (infoSource.Equals(Generated.ed_informationsource.Folkbokforing))
                {
                    // Merge Contacts
                    if (!string.IsNullOrWhiteSpace(conflict.EMailAddress1))
                    {
                        ContactEntity updateConflictEntity = new ContactEntity
                        {
                            Id = conflict.Id,
                            cgi_socialsecuritynumber = null,
                            BirthDate = null, //DevOps 9168
                            ed_ConflictConnectionGuid = new Guid().ToString()
                        };
                        XrmHelper.Update(localContext, updateConflictEntity);
                        this.ed_ConflictConnectionGuid = updateConflictEntity.ed_ConflictConnectionGuid;
                    }
                    else
                    {
                        CombineContacts(localContext, new List<ContactEntity> { conflict });
                    }
                }
                //25/01/2021 - Chris: New rule where we can have multiple COntacts with same SSN in cgi_socialsecuritynumber (DevOps: 3292)
                //else if (conflict.Contains(ContactEntity.Fields.cgi_socialsecuritynumber) && conflict.cgi_socialsecuritynumber.Equals(cgi_socialsecuritynumber))
                //{
                //    if (infoSource.Equals(Generated.ed_informationsource.AdmSkapaKund))
                //        throw new ApplicationException(string.Format(Properties.Resources.CouldNotCreateCustomerSocSec));
                //    else
                //        throw new ApplicationException(string.Format(Properties.Resources.CouldNotUpdateContactSocialSecurityConflict));
                //}
                else
                {
                    // TODO - Marcus Stenswed - Krockar när man skapar nytt konto via Webben och skickar felmeddelande
                    if (infoSource.Equals(Generated.ed_informationsource.AdmSkapaKund))
                        throw new ApplicationException(string.Format(Properties.Resources.CouldNotCreateCustomerEmailName));
                    else
                        throw new ApplicationException(string.Format(Properties.Resources.CouldNotUpdateCustomerEmail));
                }
            }
        }

        /// <summary>
        /// Fetches pre-defined contact. Used for users that aren't registered.
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public static ContactEntity GetAnonymousContact(Plugin.LocalPluginContext localContext)
        {

            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ContactEntity.Fields.cgi_ContactNumber),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_ContactNumber, ConditionOperator.Equal, "1")
                        }
                    }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

            return contact;
        }

        public static void HandlePreContacSetState(Plugin.LocalPluginContext localContext, EntityReference entityId)
        {
            localContext.Trace("entered HandlePreContacSetState()");

            // Read contact
            ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, entityId.Id, new ColumnSet(ContactEntity.Fields.EMailAddress1,
                                                                                                                        ContactEntity.Fields.ed_PrivateCustomerContact));

            // Must be a valid email
            if (string.IsNullOrWhiteSpace(contact.EMailAddress1))
            {
                localContext.Trace("The Contact in question is not a validated Contact. No validation required.");
                return;
            }

            if (contact.ed_PrivateCustomerContact != null && contact.ed_PrivateCustomerContact != true)
            {
                localContext.Trace("MKL verification are only to be done on private customers, aborting");
                return;
            }

            if (localContext?.PluginExecutionContext?.InitiatingUserId == null || localContext?.PluginExecutionContext?.InitiatingUserId == Guid.Empty)
            {
                localContext.Trace($"Could not identify an initiating user when trying to check SystemUserRole privileges");
                throw new Exception($"Could not identify an initiating user when trying to check SystemUserRole privileges");
            }

            // Check if callingUser has privileges to inactivate no matter what.
            if (!DoesUserHaveInactivationRight(localContext))
            {
                localContext.Trace($"Calling user does not have privileges to inactivate Contacts even if they have a balance in MKL, proceeding to check balance.");
                // Else call MKL to see if customer has an amount connected.
                if (HasMKLBalance(localContext, entityId))
                {
                    // If so prevent the inactivation
                    localContext.Trace($"Target still has funds. No inactivation will take place.");
                    throw new Exception(Properties.Resources.CustomerHasBalanceMKL);
                }
            }

            SendDeleteMessageToMKL(localContext, entityId);
        }


        /// <summary>
        /// Perform merge on all the conflicting contacts
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="conflicts">Contacts to merge to context contact</param>
        public void CombineContacts(Plugin.LocalPluginContext localContext, IList<ContactEntity> conflicts)
        {
            localContext.Trace("Entrered CombineContacts()");
            if (this == null)
                new Exception("Invalid indata: No contact to merge with list of contacts.");
            else if (conflicts == null)
            {
                localContext.Trace("No contacts in list to merge.");
                return;
            }
            localContext.Trace($"Combining {conflicts.Count} Contacts");

            foreach (ContactEntity c in conflicts)
            {
                // Create 'UpdateContent' - Fields that are null in Target but have information in Subordinate.
                ContactEntity updateContent = GenerateUpdateContentContact(c);

                MergeRequest merge = new MergeRequest()
                {
                    SubordinateId = c.Id,
                    Target = this.ToEntityReference(),
                    PerformParentingChecks = false,
                    UpdateContent = updateContent
                };

                // Execute the request.
                MergeResponse merged = (MergeResponse)localContext.OrganizationService.Execute(merge);
            }
        }

        public IList<CompanyRoleEntity> GetCompanyRoles(Plugin.LocalPluginContext localContext)
        {
            localContext.Trace("Entrered GetCompanyRoles()");

            IList<CompanyRoleEntity> contactRoles = XrmRetrieveHelper.RetrieveMultiple<CompanyRoleEntity>
                (localContext,
                new ColumnSet(
                    CompanyRoleEntity.Fields.ed_Role,
                    CompanyRoleEntity.Fields.ed_EmailAddress,
                    CompanyRoleEntity.Fields.ed_Telephone,
                    CompanyRoleEntity.Fields.ed_Account),
                    new FilterExpression()
                    {
                        Conditions =
                        {
                        new ConditionExpression(CompanyRoleEntity.Fields.ed_Contact, ConditionOperator.Equal, this.Id)
                        }
                    });

            return contactRoles;

        }

        private ContactEntity GenerateUpdateContentContact(ContactEntity subordinate)
        {
            ContactEntity updateContent = new ContactEntity();

            if ((string.IsNullOrWhiteSpace(FirstName) || "Ange".Equals(FirstName)) && !string.IsNullOrWhiteSpace(subordinate.FirstName))
            {
                updateContent.FirstName = subordinate.FirstName;
                FirstName = subordinate.FirstName;
            }

            if ((string.IsNullOrWhiteSpace(LastName) || "Namn".Equals(LastName)) && !string.IsNullOrWhiteSpace(subordinate.LastName))
            {
                updateContent.LastName = subordinate.LastName;
                LastName = subordinate.LastName;
            }

            if ((string.IsNullOrWhiteSpace(Telephone1)) && !string.IsNullOrWhiteSpace(subordinate.Telephone1))
            {
                updateContent.Telephone1 = subordinate.Telephone1;
                Telephone1 = subordinate.Telephone1;
            }

            if ((string.IsNullOrWhiteSpace(Telephone2)) && !string.IsNullOrWhiteSpace(subordinate.Telephone2))
            {
                updateContent.Telephone2 = subordinate.Telephone2;
                Telephone2 = subordinate.Telephone2;
            }

            if ((string.IsNullOrWhiteSpace(cgi_socialsecuritynumber)) && !string.IsNullOrWhiteSpace(subordinate.cgi_socialsecuritynumber))
            {
                updateContent.cgi_socialsecuritynumber = subordinate.cgi_socialsecuritynumber;
                updateContent.BirthDate = subordinate.BirthDate; //DevOps 9168
                cgi_socialsecuritynumber = subordinate.cgi_socialsecuritynumber;
                BirthDate = subordinate.BirthDate; //DevOps 9168
            }

            return updateContent;
        }

        private static void InactivateContact(Plugin.LocalPluginContext localContext, EntityReference entityId)
        {
            if (entityId == null || entityId.Id == null || entityId.Id.Equals(Guid.Empty))
                return;
            if (entityId.LogicalName == null || !entityId.LogicalName.Equals(ContactEntity.EntityLogicalName))
                return;
            SetStateRequest req = new SetStateRequest
            {
                EntityMoniker = entityId,
                State = new OptionSetValue((int)Generated.ContactState.Inactive),
                Status = new OptionSetValue((int)Generated.contact_statuscode.Inactive)
            };
            try
            {
                SetStateResponse resp = (SetStateResponse)localContext.OrganizationService.Execute(req);
            }
            catch (Exception e)
            {
                localContext.Trace($"Unexpected exception caught when attempting to inactivate Contact with Id: {entityId.Id}, exception message:\n{e.Message}");
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        private static bool SendDeleteMessageToMKL(Plugin.LocalPluginContext localContext, EntityReference contactId)
        {
            try
            {
                string MklEndpoint = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_MklEndpoint);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{MklEndpoint}/admin/users/{contactId.Id}");
                string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "DELETE";

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode != HttpStatusCode.NoContent)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        // Result is 
                        var result = streamReader.ReadToEnd();
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public static bool HasMKLBalance(Plugin.LocalPluginContext localContext, EntityReference contactId)
        {
            try
            {
                string MklEndpoint = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_MklEndpoint);

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{MklEndpoint}/admin/users/{contactId.Id}/value");
                //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{MklEndpoint}/admin/users/{new Guid("9A9D0B3B-0BBE-E611-8114-00155D0A6B01")}/value");
                string clientCertName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_ClientCertName);
                httpWebRequest.ClientCertificates.Add(Identity.GetCertToUse(clientCertName));
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MklValuePlaceholder));
                    MklValuePlaceholder mklValue = (MklValuePlaceholder)serializer.ReadObject(httpResponse.GetResponseStream());
                    return mklValue.valueExists;
                }
                catch (Exception e)
                {
                    throw new Exception($"Exception caught when trying to parse return value from MKL to MklValuePlaceholder", e);
                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response == null)
                {
                    localContext.TracingService.Trace($"MKL returned an exeption. Content: {we.Message}");
                    throw we;
                }

                //using (var streamReader = new StreamReader(response.GetResponseStream()))
                //{
                //    var result = streamReader.ReadToEnd();
                //}
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MklErrorObject));
                    MklErrorObject mklErr = (MklErrorObject)serializer.ReadObject(response.GetResponseStream());

                    if (MklErrorObject.UserEntityNotFound.Equals(mklErr.code))
                    {
                        return false;
                    }
                    //serializer.WriteObject

                    localContext.TracingService.Trace($"MKL returned an exeption httpCode: {response.StatusCode} Content: {(mklErr.localizedMessage ?? mklErr.message) ?? mklErr.httpStatus}\nSerialised object:\n{ mklErr}");

                    throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {(mklErr.localizedMessage ?? mklErr.message) ?? mklErr.httpStatus}", mklErr.innerException);
                }
                catch (Exception e)
                {
                    throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {e.Message}", e);
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException($"Fel vid kommunikation med externt system: {e.Message}", e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        private static bool DoesUserHaveInactivationRight(Plugin.LocalPluginContext localContext)
        {
            // Retrieve current user
            SystemUserEntity sysUser = null;
            try
            {
                sysUser = XrmRetrieveHelper.Retrieve<SystemUserEntity>(localContext, localContext.PluginExecutionContext.InitiatingUserId, new ColumnSet(SystemUserEntity.Fields.FullName));
            }
            catch (Exception e)
            {
                localContext.Trace($"Exception caught when retrieving systemuser with localContext.PluginExecutionContext.InitiatingUserId:\n{e.Message}");
                throw e;
            }

            EntityReference allowedToInactivateRoleRef = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.ed_AllowedToInactivate);
            Role allowedToInactivateRole = XrmRetrieveHelper.Retrieve<Role>(localContext, allowedToInactivateRoleRef.Id, new ColumnSet(Role.Fields.Name));
            IList<Role> allowedToInactivateRoles = XrmRetrieveHelper.RetrieveMultiple<Role>(localContext, new ColumnSet(false),
                new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Fields.Name, ConditionOperator.Equal, allowedToInactivateRole.Name)
                    }
                });
            FilterExpression roleFilter = new FilterExpression(LogicalOperator.Or);
            foreach (Role r in allowedToInactivateRoles)
            {
                roleFilter.AddCondition(new ConditionExpression(SystemUserRoles.Fields.RoleId, ConditionOperator.Equal, r.RoleId));
            }

            SystemUserRoles userInRole = XrmRetrieveHelper.RetrieveFirst<SystemUserRoles>(localContext, new ColumnSet(false),
                new FilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new ConditionExpression(SystemUserRoles.Fields.SystemUserId, ConditionOperator.Equal, sysUser.SystemUserId)
                    },
                    Filters =
                    {
                        roleFilter
                    }
                });
            return userInRole != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static ContactEntity FindOrCreateUnvalidatedContact(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo, AccountEntity account = null, FeatureTogglingEntity feature = null)
        {

            if (customerInfo == null)
                return null;

            ContactEntity contact = FindActiveContact(localContext, customerInfo, account);

            //PRIVATKUND
            if (customerInfo.Source != (int)Generated.ed_informationsource.ForetagsPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SkolPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SeniorPortal)
            {
                if (contact == null)
                {
                    contact = new ContactEntity(localContext, customerInfo);
                    contact.EMailAddress2 = contact.EMailAddress1;
                    contact.EMailAddress1 = null;

                    //Mark Contact as PrivateCustomer
                    if (contact.ed_PrivateCustomerContact == null || contact.ed_PrivateCustomerContact != true)
                    {
                        contact.ed_PrivateCustomerContact = true;
                    }

                    contact.Id = localContext.OrganizationService.Create(contact);
                }
                else //Vi hittade en match (Kontakt)
                {
                    // Uppdatera inte MittKonto kunder!
                    if (string.IsNullOrWhiteSpace(contact.EMailAddress1) && customerInfo.Source != (int)Generated.ed_informationsource.SkapaMittKonto) //Lagt till skapa mitt konto
                    {
                        // update with all data in customerInfo - Update so that contact is PrivatKund (OptionSet)
                        ContactEntity updateContact = contact.CreateUpdateContactFromCustomerInfo(localContext, customerInfo); //CHECK
                        if (updateContact != null)
                            localContext.OrganizationService.Update(updateContact);
                    }
                    else // ska man kontrollera skapa mitt konto?
                    {
                        // update only missing fields from customerInfo - Update so that contact is PrivatKund (OptionSet)
                        ContactEntity updateContact = contact.UpdateWithWeakCustomerInfo(localContext, customerInfo); //CHECK
                        if (updateContact != null)
                            localContext.OrganizationService.Update(updateContact);
                    }
                }
            }
            //FÖRETAGSKUND FTG/SKOLA
            else if (customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal ||
                customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SkolPortal ||
                customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal)
            {
                //4. Kontrollera ifall cotact har en "Role" värde, om inte - uppdatera till antingen admin.FTG/admin.SKOL/Admin.Senior

                // "contact" is null means no existing matching Contact was found
                if (contact == null)
                {
                    contact = new ContactEntity(localContext, customerInfo, feature); //New Code (2020-05-06)

                    //Lägg till Contact.AccountRoleCode på contacten för ftg (administratör-ftg)/ SKOL värde 9 (portaladministrator-skol) / seniorvärde 10 (portaladministrator-senior)
                    if (customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal)
                    {
                        contact.ed_SeniorContact = true;
                        contact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSenior;
                    }
                    else if (customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal)
                    {
                        contact.ed_SchoolContact = true;
                        contact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSchool;
                    }
                    else if (customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal)
                    {
                        contact.ed_BusinessContact = true;
                        contact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorFTG;
                    }

                    // Create new Portal Customer
                    contact.Id = XrmHelper.Create(localContext.OrganizationService, contact);
                }
                // "contact" is not null means existing matching Contact was found
                else
                {
                    if (feature == null || (feature != null && feature.ed_SplittCompany == false)) //Old Logic
                    {
                        // Update existing Contact with information from CustomerInfo
                        ContactEntity updateContact = contact.UpdateWithWeakCustomerInfo(localContext, customerInfo);
                        if (updateContact != null)
                            localContext.OrganizationService.Update(updateContact);
                    }

                    if (feature != null && feature.ed_SplittCompany == true) //New Logic (2020-05-06)
                    {
                        //Skapa inte en ny kontakt. Behöver inte kontrollera association då vi gör det senare. (2021-02-23 CK)
                        return contact;
                    }
                }
            }

            return contact;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        private ContactEntity UpdateWithWeakCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            ContactEntity updateContact = new ContactEntity
            {
                Id = this.Id,
                ContactId = this.ContactId,
                ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source,
            };
            bool updated = false;
            if (customerInfo == null)
                return null;

            if (string.IsNullOrWhiteSpace(this.FirstName) && !string.IsNullOrWhiteSpace(customerInfo.FirstName))
            {
                this.FirstName = customerInfo.FirstName;
                updateContact.FirstName = customerInfo.FirstName;
                updated = true;
            }
            if (string.IsNullOrWhiteSpace(this.LastName) && !string.IsNullOrWhiteSpace(customerInfo.LastName))
            {
                this.LastName = customerInfo.LastName;
                updateContact.LastName = customerInfo.LastName;
                updated = true;
            }


            if (customerInfo.isLockedPortalSpecified && !this.ed_isLockedPortal.HasValue)
            {
                this.ed_isLockedPortal = customerInfo.isLockedPortal;
                updateContact.ed_isLockedPortal = customerInfo.isLockedPortal; //Added
                updated = true;
            }

            //Flyttats upp då detta inte bara gäller privatpersoner
            if (string.IsNullOrWhiteSpace(this.Telephone2) && !string.IsNullOrWhiteSpace(customerInfo.Mobile))
            {
                this.Telephone2 = customerInfo.Mobile;
                updateContact.Telephone2 = customerInfo.Mobile;
                updated = true;
            }

            // Handle e-mail
            if (string.IsNullOrWhiteSpace(this.EMailAddress1))
            {
                // Assign e-mail2 if email is supplied.
                if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !customerInfo.Email.Equals(this.EMailAddress2))
                {
                    this.EMailAddress2 = customerInfo.Email;
                    updateContact.EMailAddress2 = customerInfo.Email;
                    updated = true;
                }
            }

            if (string.IsNullOrWhiteSpace(this.Telephone1) && !string.IsNullOrWhiteSpace(customerInfo.Telephone))
            {
                this.Telephone1 = customerInfo.Telephone;
                updateContact.Telephone1 = customerInfo.Telephone;
                updated = true;
            }

            if (string.IsNullOrWhiteSpace(this.Address1_Line1) &&
                string.IsNullOrWhiteSpace(this.Address1_Line2) &&
                string.IsNullOrWhiteSpace(this.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(this.Address1_City) &&
                this.ed_Address1_Country == null)
            {
                if (customerInfo.AddressBlock != null && !(
                string.IsNullOrEmpty(customerInfo.AddressBlock.CO) &&
                string.IsNullOrEmpty(customerInfo.AddressBlock.Line1) &&
                string.IsNullOrEmpty(customerInfo.AddressBlock.PostalCode) &&
                string.IsNullOrEmpty(customerInfo.AddressBlock.City) &&
                string.IsNullOrEmpty(customerInfo.AddressBlock.CountryISO)))
                {
                    EntityReference countryRef = CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);

                    this.Address1_Line1 = customerInfo.AddressBlock.CO;
                    this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                    this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                    this.Address1_City = customerInfo.AddressBlock.City;
                    this.ed_Address1_Country = countryRef;
                    updateContact.Address1_Line1 = customerInfo.AddressBlock.CO;
                    updateContact.Address1_Line2 = customerInfo.AddressBlock.Line1;
                    updateContact.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                    updateContact.Address1_City = customerInfo.AddressBlock.City;
                    updateContact.ed_Address1_Country = countryRef;
                    updated = true;
                }
            }

            if (customerInfo.Source != (int)Generated.ed_informationsource.ForetagsPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SkolPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SeniorPortal) // Privatkund
            {
                //SSN moved from above and placed to only be handled when Privatkund
                if (string.IsNullOrWhiteSpace(this.cgi_socialsecuritynumber) && !string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                {
                    this.cgi_socialsecuritynumber = customerInfo.SocialSecurityNumber;
                    this.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                    this.BirthDate = ContactEntity.UpdateBirthDateOnContact(customerInfo.SocialSecurityNumber); //DevOps 91658
                    updateContact.cgi_socialsecuritynumber = customerInfo.SocialSecurityNumber;
                    updateContact.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                    updateContact.BirthDate = ContactEntity.UpdateBirthDateOnContact(customerInfo.SocialSecurityNumber); //DevOps 91658
                    updated = true;
                }

                //Flyttats upp då detta inte bara gäller privatpersoner
                if (string.IsNullOrWhiteSpace(this.Telephone2) && !string.IsNullOrWhiteSpace(customerInfo.Mobile))
                {
                    this.Telephone2 = customerInfo.Mobile;
                    updateContact.Telephone2 = customerInfo.Mobile;
                    updated = true;
                }

                // Handle e-mail
                if (string.IsNullOrWhiteSpace(this.EMailAddress1))
                {
                    // Assign e-mail2 if email is supplied.
                    if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !customerInfo.Email.Equals(this.EMailAddress2))
                    {
                        this.EMailAddress2 = customerInfo.Email;
                        updateContact.EMailAddress2 = customerInfo.Email;
                        updated = true;
                    }
                }

                //Handle ed_PrivateCustomerContact
                if (this.ed_PrivateCustomerContact == null || this.ed_PrivateCustomerContact != true)
                {
                    this.ed_PrivateCustomerContact = true;
                    updateContact.ed_PrivateCustomerContact = true;
                    updated = true;
                }
            }
            else if (customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal ||
                customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal ||
                customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal)
            {
                // Has Telephone changed?
                if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone) && this.Telephone2 != customerInfo.CompanyRole[0].Telephone)
                {
                    this.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    updateContact.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    updated = true;
                }

                // Has Email changed?
                if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email) && this.EMailAddress1 != customerInfo.CompanyRole[0].Email)
                {
                    this.EMailAddress1 = customerInfo.CompanyRole[0].Email;
                    updateContact.EMailAddress1 = customerInfo.CompanyRole[0].Email;
                    updated = true;
                }

                if (customerInfo.isLockedPortalSpecified && this.ed_isLockedPortal != customerInfo.isLockedPortal)
                {
                    this.ed_isLockedPortal = customerInfo.isLockedPortal;
                    updateContact.ed_PrivateCustomerContact = true;
                    updated = true;
                }

                //Handle ed_SchoolContact
                if (this.ed_SchoolContact != true && customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal)
                {
                    this.ed_SchoolContact = true;
                    updateContact.ed_SchoolContact = true;
                    this.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSchool;
                    updateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSchool;
                    updated = true;
                }

                //Handle ed_SeniorContact
                if (this.ed_SeniorContact != true && customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal)
                {
                    this.ed_SeniorContact = true;
                    updateContact.ed_SeniorContact = true;
                    this.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSenior;
                    updateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorSenior;
                    updated = true;
                }

                //Handle ed_BusinessContact
                if (this.ed_BusinessContact != true && customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal)
                {
                    this.ed_BusinessContact = true;
                    updateContact.ed_BusinessContact = true;
                    this.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorFTG;
                    updateContact.AccountRoleCode = Generated.contact_accountrolecode.PortalAdministratorFTG;
                    updated = true;
                }

                if (this.ed_SocialSecurityNumberBlock == string.Empty)
                {
                    this.ed_SocialSecurityNumberBlock = customerInfo.SocialSecurityNumber;
                    updateContact.ed_SocialSecurityNumberBlock = customerInfo.SocialSecurityNumber;
                    updated = true;
                }
            }

            if (this.ed_InformationSource == null && customerInfo.Source > -1 && customerInfo.Source < Enum.GetValues(typeof(Generated.ed_informationsource)).Length)
            {
                this.ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
                updateContact.ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
                updated = true;
            }
            if (updated)
                return updateContact;
            else
                return null;
        }

        /// <summary>
        /// Create a private contact if it does not exists already
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static ContactEntity CreateUnvalidatedPrivateContact(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            if (customerInfo == null)
                return null;

            ContactEntity contact = FindActiveContact(localContext, customerInfo);
            if (contact == null)
            {
                contact = new ContactEntity(localContext, customerInfo)
                {
                    EMailAddress1 = null,
                    EMailAddress2 = customerInfo.Email,     // Make sure the email address goes into the unvalidated field.
                    ed_PrivateCustomerContact = true,
                    ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund,
                };
                contact.Id = localContext.OrganizationService.Create(contact);
            }

            if (contact != null && contact.ed_PrivateCustomerContact != true)
            {
                ContactEntity updateContact = new ContactEntity
                {
                    ed_PrivateCustomerContact = true,
                    Id = contact.Id
                };

                localContext.OrganizationService.Update(updateContact);
            }

            return contact;
        }

        /// <summary>
        /// Finds contact in CRM based on Social security number (primary key) and fallback on FirstName + LastName + Email (secondary key as specified by Håkan Tingström (Skånetrafiken)).
        /// Company/School/Senior matching criteria:
        /// Exact match on Social Security Number (ed_SocialSecurityNumberBlock) and Email (EMailAddress1). Also check for each flag (Business-, School- or Senior Contact)
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static ContactEntity FindActiveContact(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo, AccountEntity account = null)
        {
            //Feature Toggle Entity
            FeatureTogglingEntity feature = FeatureTogglingEntity.GetFeatureToggling(localContext, FeatureTogglingEntity.Fields.ed_SplittCompany);

            if (customerInfo == null)
                return null;

            ContactEntity contact = null;

            #region New Code

            //If "CustomerInfo" indicates that customer is PrivatKund (OptionSet) - Handle with social security Number, email + för.Namn + eft.Nman, address + namn
            if (customerInfo.Source != (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal &&
                customerInfo.Source != (int)Crm.Schema.Generated.ed_informationsource.SkolPortal &&
                customerInfo.Source != (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal)
            {
                // Find by social security number first. (NOTE: we are not using social security number for FTG/SCHOOL)
                //Privat
                if (contact == null /* && !string.IsNullOrWhiteSpace(customerInfo.Mobile) */ && !string.IsNullOrWhiteSpace(customerInfo.Email))
                {

                    //FilterExpression mobileFilter = new FilterExpression(LogicalOperator.And)
                    //{
                    //    Conditions =
                    //    {
                    //        new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, customerInfo.Mobile)
                    //    }
                    //};
                    FilterExpression emailFilterNew = new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                            new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, customerInfo.Email)
                        }
                    };

                    FilterExpression privateCustomerFilter = new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                            }
                    };

                    QueryExpression queryEmailAndMobile = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                                },
                                Filters =
                                {
                                    //mobileFilter,
                                    emailFilterNew,
                                    privateCustomerFilter
                                }
                            }
                    };

                    contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, queryEmailAndMobile);

                    bool update = false;
                    if (contact != null)
                    {
                        ContactEntity updateEntity = new ContactEntity
                        {
                            ContactId = contact.ContactId,
                            Id = contact.Id
                        };
                        if (contact != null && "Ange".Equals(contact.FirstName) && !customerInfo.FirstName.Equals(contact.FirstName))
                        {
                            contact.FirstName = customerInfo.FirstName;
                            updateEntity.FirstName = customerInfo.FirstName;
                            update = true;
                        }
                        if (contact != null && "Namn".Equals(contact.LastName) && !customerInfo.LastName.Equals(contact.LastName))
                        {
                            contact.LastName = customerInfo.LastName;
                            updateEntity.LastName = customerInfo.LastName;
                            update = true;
                        }
                        if (update)
                            XrmHelper.Update(localContext.OrganizationService, updateEntity);
                    }
                    //Match by email + mobile END
                }

                if (contact == null && !string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && customerInfo.SwedishSocialSecurityNumberSpecified && customerInfo.SwedishSocialSecurityNumber)
                {
                    FilterExpression privateCustomerFilter = new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                            }
                    };
                    IList<ContactEntity> socseccontacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                        new FilterExpression()
                        {
                            Conditions =
                            {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.Equal, customerInfo.SocialSecurityNumber),
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                        },
                            Filters =
                            {
                                privateCustomerFilter
                            }
                        }
                    );
                    if (socseccontacts.Count == 1)
                    {
                        contact = socseccontacts[0];
                        localContext.Trace($"Found contact by SocialSecurityNumber");
                    }
                    else if (socseccontacts.Count > 1)
                    {
                        contact = socseccontacts[0];
                        localContext.Trace($"Found multiple contacts by SocialSecurityNumber");
                        //throw new Exception(string.Format("multiple contacts found with the same social security number: {0}", customerInfo.SocialSecurityNumber));
                    }
                    // Match by social security number END
                }

                // Contact still not found?
                if (contact == null && !string.IsNullOrWhiteSpace(customerInfo.FirstName) && !string.IsNullOrWhiteSpace(customerInfo.LastName) && !string.IsNullOrWhiteSpace(customerInfo.Email))
                {
                    //ska vi även här gå efter companyrole - mail + telefon?
                    //Match by email + firstNmae + Last Name START
                    FilterExpression nameFilter = new FilterExpression(LogicalOperator.Or)
                    {
                        Filters =
                            {
                                new FilterExpression(LogicalOperator.And)
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, customerInfo.FirstName),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, customerInfo.LastName)
                                    }
                                },
                                new FilterExpression(LogicalOperator.And)
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, "Ange"),
                                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, "Namn")
                                    }
                                }
                            }
                    };
                    FilterExpression emailFilterNew = new FilterExpression(LogicalOperator.Or)
                    {
                        Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, customerInfo.Email),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, customerInfo.Email)
                            }
                    };

                    //FilterExpression privateCustomerFilter = new FilterExpression(LogicalOperator.Or)
                    //{
                    //    Conditions =
                    //    {
                    //        // Null == true, fallback if no data
                    //        new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Equal, true),
                    //        new ConditionExpression(ContactEntity.Fields.ed_PrivateCustomerContact, ConditionOperator.Null),
                    //    }
                    //};


                    QueryExpression queryEmailAndName = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria =
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                            },
                            Filters =
                            {
                                nameFilter,
                                emailFilterNew
                                //,
                                //privateCustomerFilter
                            }
                        }
                    };

                    IList<ContactEntity> possibleMatches = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, queryEmailAndName);
                    localContext.Trace($"FindActiveContact found {possibleMatches.Count} matches on e-mail and name.");

                    // Check if list of possible matches contains any contact
                    if (possibleMatches.Count() > 0)
                    {
                        // Loop trough possible matches of contacs
                        foreach (var possibleContactMatch in possibleMatches)
                        {
                            string firstnameContactMatch = Regex.Replace(possibleContactMatch.FirstName, @"[^\p{L}\p{N}]+", "");
                            string firstnameCustomerInfo = Regex.Replace(customerInfo.FirstName, @"[^\p{L}\p{N}]+", "");

                            string lastnameContactMatch = Regex.Replace(possibleContactMatch.LastName, @"[^\p{L}\p{N}]+", "");
                            string lastnameCustomerInfo = Regex.Replace(customerInfo.LastName, @"[^\p{L}\p{N}]+", "");

                            // Compare first- and lastname
                            if (String.Compare(firstnameContactMatch, firstnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0
                                && String.Compare(lastnameContactMatch, lastnameCustomerInfo, CultureInfo.CreateSpecificCulture("sv-SE"), CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                            {
                                // First- and Lastname Match (Emailaddress matched when retrieving)
                                contact = possibleContactMatch;
                            }
                            else if ("Ange".Equals(possibleContactMatch.FirstName) && "Namn".Equals(possibleContactMatch.LastName))
                            {
                                contact = possibleContactMatch;
                            }
                        }
                    }

                    bool update = false;
                    if (contact != null)
                    {
                        ContactEntity updateEntity = new ContactEntity
                        {
                            ContactId = contact.ContactId,
                            Id = contact.Id
                        };
                        if (contact != null && "Ange".Equals(contact.FirstName) && !customerInfo.FirstName.Equals(contact.FirstName))
                        {
                            contact.FirstName = customerInfo.FirstName;
                            updateEntity.FirstName = customerInfo.FirstName;
                            update = true;
                        }
                        if (contact != null && "Namn".Equals(contact.LastName) && !customerInfo.LastName.Equals(contact.LastName))
                        {
                            contact.LastName = customerInfo.LastName;
                            updateEntity.LastName = customerInfo.LastName;
                            update = true;
                        }
                        if (update)
                            XrmHelper.Update(localContext.OrganizationService, updateEntity);
                    }
                    //Match by email + firstNmae + Last Name END
                }
            }
            //If "CustomerInfo" indicates that customer is FTG/SKOL (OptionSet) - Handle with Email + Mobile
            else if (customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal ||
                customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SkolPortal ||
                customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal)
            {

                if (feature.ed_SplittCompany == false) //Old Logic
                {
                    string email = string.Empty;
                    if (customerInfo.CompanyRole != null && customerInfo.CompanyRole[0] != null && !string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email))
                    {
                        email = customerInfo.CompanyRole[0].Email;
                    }
                    else if (customerInfo.Email != null)
                    {
                        email = customerInfo.Email;
                    }

                    if (contact == null && !string.IsNullOrWhiteSpace(email) /*&& !string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone)*/) //Email / Telephone found in CompanyRole Object
                    {
                        // ONLY CHECK FOR VALIDATED EMAILS (emailaddress1)
                        FilterExpression emailFilterNew = new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email)
                                //new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, email)
                            }
                        };

                        List<ContactEntity> emMoContacts = new List<ContactEntity>();

                        emMoContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, ContactEntity.ContactInfoBlock,
                            new FilterExpression(LogicalOperator.And)
                            {
                                Conditions =
                                {
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                                },
                                Filters =
                            {
                                emailFilterNew,
                            }

                            }).ToList();


                        // TODO - 2020-01-31 Marcus Stenswed
                        // How should we handle if there are more hits than one?
                        if (emMoContacts.Count == 1)
                        {
                            contact = emMoContacts[0];
                        }
                        else if (emMoContacts.Count > 1)
                        {
                            // 2020-01-31 - Marcus Stenswed
                            // For now we're picking the first one
                            contact = emMoContacts[0];

                            //throw new Exception(string.Format("Multiple Contacts found with the same Email: {0}, and Mobile Number: {1}", email, customerInfo.CompanyRole[0]?.Telephone)); //revise
                        }
                    }
                }

                if (feature.ed_SplittCompany == true) //New Logic (2020-05-06)
                {
                    //string email = customerInfo.CompanyRole[0].Email;
                    string socialSecurityNumber = customerInfo.SocialSecurityNumber;

                    /* 
                     Check for matching Contacts (Company, School or Senior marked)
                     Based on Social Security Number
                     */

                    QueryExpression portalCustomerFilter = new QueryExpression()
                    {
                        EntityName = ContactEntity.EntityLogicalName,
                        ColumnSet = ContactEntity.ContactInfoBlock,
                        Criteria = new FilterExpression(LogicalOperator.And)
                        {
                            Conditions =
                            {
                                //new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email),
                                new ConditionExpression(ContactEntity.Fields.ed_SocialSecurityNumberBlock, ConditionOperator.Equal, socialSecurityNumber), //Add check for company contact
                                new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                            }
                        }
                    };

                    if (customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal)
                    {
                        ConditionExpression companyCustomerExpression = new ConditionExpression(ContactEntity.Fields.ed_BusinessContact, ConditionOperator.Equal, true);
                        portalCustomerFilter.Criteria.AddCondition(companyCustomerExpression);
                    }
                    else if (customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SkolPortal)
                    {
                        ConditionExpression schoolCustomerExpression = new ConditionExpression(ContactEntity.Fields.ed_SchoolContact, ConditionOperator.Equal, true);
                        portalCustomerFilter.Criteria.AddCondition(schoolCustomerExpression);
                    }
                    else if (customerInfo.Source == (int)Crm.Schema.Generated.ed_informationsource.SeniorPortal)
                    {
                        ConditionExpression seniorCustomerExpression = new ConditionExpression(ContactEntity.Fields.ed_SeniorContact, ConditionOperator.Equal, true);
                        portalCustomerFilter.Criteria.AddCondition(seniorCustomerExpression);
                    }

                    List<ContactEntity> matchingContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, portalCustomerFilter);

                    // TODO - 2020-04-08 (Marcus Stenswed)
                    // How should we handle if there are more hits than one?
                    // Return first match (temporary)
                    //TODO - 2020-09-04 (Christian) - Använd accountets Parentaccount. Kolla relation med contact som vi hittar. Använd den som har en relation med Org.
                    if (matchingContacts != null && matchingContacts.Count() > 0)
                    {
                        if (matchingContacts.Count == 1)
                        {
                            contact = matchingContacts[0];
                        }
                        else if (matchingContacts.Count > 1)
                        {
                            //TODO - 2020-09-04 (Christian) - Använd accountets Parentaccount. Kolla relation med contact som vi hittar. Använd den som har en relation med Org.
                            if (account != null && account.ParentAccountId != null)
                            {
                                bool foundContact = false;
                                for (int i = 0; i < matchingContacts.Count; i++)
                                {
                                    if (CompanyRoleEntity.DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, account.ParentAccountId.Id, ContactEntity.EntityLogicalName, matchingContacts[i].ContactId.Value))
                                    {
                                        contact = matchingContacts[i];
                                        foundContact = true;
                                        break;
                                    }
                                }

                                //If none of the listed Contacts has an association with the Accounts Parent account - Return the first Contact.
                                if (foundContact == false)
                                {
                                    contact = matchingContacts[0];
                                }
                            }
                            else if (account != null && account.Id != null)
                            {
                                bool foundContact2 = false;
                                for (int i = 0; i < matchingContacts.Count; i++)
                                {
                                    if (CompanyRoleEntity.DoesRelationshipExist(localContext, "cgi_account_contact", AccountEntity.EntityLogicalName, account.Id, ContactEntity.EntityLogicalName, matchingContacts[i].ContactId.Value))
                                    {
                                        contact = matchingContacts[i];
                                        foundContact2 = true;
                                        break;
                                    }
                                }

                                //If none of the listed Contacts has an association with the Parent account - Return the first Contact.
                                if (foundContact2 == false)
                                {
                                    contact = matchingContacts[0];
                                }
                            }
                            else 
                            {
                                contact = matchingContacts[0];
                            }
                        }
                    }
                }
            }
            #endregion

            return contact;
        }

        /// <summary>
        /// Updates contact with ALL info in customerInfo
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public ContactEntity CreateUpdateContactFromCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            ContactEntity updateContact = new ContactEntity
            {
                ContactId = this.ContactId,
                Id = this.Id
            };
            bool updated = false;
            if (customerInfo == null)
                return null;

            if (!string.IsNullOrWhiteSpace(customerInfo.FirstName) && !customerInfo.FirstName.Equals(this.FirstName))
            {
                this.FirstName = customerInfo.FirstName;
                updateContact.FirstName = customerInfo.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.LastName) && !customerInfo.LastName.Equals(this.LastName))
            {
                this.LastName = customerInfo.LastName;
                updateContact.LastName = customerInfo.LastName;
                updated = true;
            }

            if (customerInfo.isLockedPortalSpecified && this.ed_isLockedPortal != customerInfo.isLockedPortal)
            {
                this.ed_isLockedPortal = customerInfo.isLockedPortal;
                updateContact.ed_isLockedPortal = customerInfo.isLockedPortal; //Added, should it be here?
                updated = true;
            }

            // Företag
            if (customerInfo.Source == (int)Generated.ed_informationsource.ForetagsPortal ||
                customerInfo.Source == (int)Generated.ed_informationsource.SkolPortal ||
                customerInfo.Source == (int)Generated.ed_informationsource.SeniorPortal)
            {
                //FTG
                if (string.IsNullOrWhiteSpace(this.EMailAddress1))
                {
                    // Assign e-mail2 if email is supplied.
                    if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Email) && !customerInfo.CompanyRole[0].Email.Equals(this.EMailAddress2))
                    {
                        this.EMailAddress2 = customerInfo.CompanyRole[0].Email;
                        updateContact.EMailAddress2 = customerInfo.CompanyRole[0].Email;
                        updated = true;
                    }
                }

                //FTG
                if (!string.IsNullOrWhiteSpace(customerInfo.CompanyRole[0].Telephone) && !customerInfo.CompanyRole[0].Telephone.Equals(this.Telephone2)) //Telephone2 is Home, should it be MobilePhone?
                {
                    this.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    updateContact.Telephone2 = customerInfo.CompanyRole[0].Telephone;
                    updated = true;
                }
            }
            else if (customerInfo.Source != (int)Generated.ed_informationsource.ForetagsPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SkolPortal &&
                customerInfo.Source != (int)Generated.ed_informationsource.SeniorPortal) //Privatperson
            {

                //Moved telephone
                if (!string.IsNullOrWhiteSpace(customerInfo.Mobile) && !customerInfo.Mobile.Equals(this.Telephone2)) //Telephone2 is Home, should it be MobilePhone?
                {
                    this.Telephone2 = customerInfo.Mobile;
                    updateContact.Telephone2 = customerInfo.Mobile;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(customerInfo.Telephone) && !customerInfo.Telephone.Equals(this.Telephone1))
                {
                    this.Telephone1 = customerInfo.Telephone;
                    updateContact.Telephone1 = customerInfo.Telephone;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SocialSecurityNumber.Equals(this.cgi_socialsecuritynumber))
                {
                    this.cgi_socialsecuritynumber = customerInfo.SocialSecurityNumber;
                    this.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                    this.BirthDate = ContactEntity.UpdateBirthDateOnContact(customerInfo.SocialSecurityNumber); //DevOps 9168
                    updateContact.cgi_socialsecuritynumber = customerInfo.SocialSecurityNumber;
                    updateContact.ed_HasSwedishSocialSecurityNumber = customerInfo.SwedishSocialSecurityNumber;
                    updateContact.BirthDate = ContactEntity.UpdateBirthDateOnContact(customerInfo.SocialSecurityNumber); //DevOps 9168
                    updated = true;
                }

                if (customerInfo.AddressBlock != null && !(
                    string.IsNullOrEmpty(customerInfo.AddressBlock.CO) &&
                    string.IsNullOrEmpty(customerInfo.AddressBlock.Line1) &&
                    string.IsNullOrEmpty(customerInfo.AddressBlock.PostalCode) &&
                    string.IsNullOrEmpty(customerInfo.AddressBlock.City) &&
                    string.IsNullOrEmpty(customerInfo.AddressBlock.CountryISO)))
                {
                    EntityReference countryRef = CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);

                    if (!(string.Equals(customerInfo.AddressBlock.CO, this.Address1_Line1) &&
                        string.Equals(customerInfo.AddressBlock.Line1, this.Address1_Line2) &&
                        string.Equals(customerInfo.AddressBlock.PostalCode, this.Address1_PostalCode) &&
                        string.Equals(customerInfo.AddressBlock.City, this.Address1_City) &&
                        countryRef.Equals(this.ed_Address1_Country)))
                    {
                        this.Address1_Line1 = customerInfo.AddressBlock.CO;
                        this.Address1_Line2 = customerInfo.AddressBlock.Line1;
                        this.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                        this.Address1_City = customerInfo.AddressBlock.City;
                        this.ed_Address1_Country = countryRef;
                        updateContact.Address1_Line1 = customerInfo.AddressBlock.CO;
                        updateContact.Address1_Line2 = customerInfo.AddressBlock.Line1;
                        updateContact.Address1_PostalCode = customerInfo.AddressBlock.PostalCode;
                        updateContact.Address1_City = customerInfo.AddressBlock.City;
                        updateContact.ed_Address1_Country = countryRef;
                        updated = true;
                    }
                }

                // Handle e-mail (Gäller detta för företag också?)
                if (string.IsNullOrWhiteSpace(this.EMailAddress1))
                {
                    // Assign e-mail2 if email is supplied.
                    if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !customerInfo.Email.Equals(this.EMailAddress2))
                    {
                        this.EMailAddress2 = customerInfo.Email;
                        updateContact.EMailAddress2 = customerInfo.Email;
                        updated = true;
                    }
                }

                //Customer infor har ingen OptionSet (PrivatePerson) NEW
                if (this.ed_PrivateCustomerContact == null || this.ed_PrivateCustomerContact != true)
                {
                    this.ed_PrivateCustomerContact = true;
                    updateContact.ed_PrivateCustomerContact = true;
                    updated = true;
                }
            }
            //else
            //{
            //    this.ed_VerifiedSSN = true;
            //    updateContact.ed_VerifiedSSN = true;
            //    updated = true;
            //}

            if (this.ed_InformationSource == null || customerInfo.Source != (int)this.ed_InformationSource)
            {
                this.ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
                updateContact.ed_InformationSource = (Generated.ed_informationsource)customerInfo.Source;
                updated = true;
            }
            if (updated)
                return updateContact;
            else
                return null;
        }

        /// <summary>
        /// Verify if contact object needs to be updated from lead. No database verification.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="updContact">A contact object that will only contain the fields that should be updated</param>
        /// <param name="lead">Lead to verify against</param>
        /// <returns></returns>
        public static bool UpdateContactWithLead(ref ContactEntity contact, ref ContactEntity updContact, LeadEntity lead)
        {
            bool updated = false;

            // If dummy lastname has been used, Clear it!
            if (string.Equals(lead.LastName, LeadEntity.CreateLead_LastNameToUseIfEmpty))
            {
                contact.LastName = null;
                updContact.LastName = null;
                lead.LastName = null;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(lead.FirstName) && string.IsNullOrWhiteSpace(contact.FirstName))
            {
                contact.FirstName = lead.FirstName;
                updContact.FirstName = lead.FirstName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.LastName)
                    && string.IsNullOrWhiteSpace(contact.LastName))
            {
                contact.LastName = lead.LastName;
                updContact.LastName = lead.LastName;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.Telephone1) && string.IsNullOrWhiteSpace(contact.Telephone1))
            {
                contact.Telephone1 = lead.Telephone1;
                updContact.Telephone1 = lead.Telephone1;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.MobilePhone) && string.IsNullOrWhiteSpace(contact.Telephone2))
            {
                contact.Telephone2 = lead.MobilePhone;
                updContact.Telephone2 = lead.MobilePhone;
                updated = true;
            }
            if (!string.IsNullOrWhiteSpace(lead.ed_Personnummer) && string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber)) //Change this solcialsecurity number field (ed_socialsecuritynumberblock)
            {
                contact.cgi_socialsecuritynumber = lead.ed_Personnummer; //ed_socialsecuritynumberblock
                updContact.cgi_socialsecuritynumber = lead.ed_Personnummer; //ed_socialsecuritynumberblock
                contact.BirthDate = ContactEntity.UpdateBirthDateOnContact(lead.ed_Personnummer); //DevOps 9168
                updContact.BirthDate = ContactEntity.UpdateBirthDateOnContact(lead.ed_Personnummer); //DevOps 9168
                contact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
                updContact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
                updated = true;
            }

            //If any data in address fields in lead and none in Contact - replace everything
            if ((string.IsNullOrWhiteSpace(contact.Address1_Line1) &&
                string.IsNullOrWhiteSpace(contact.Address1_Line2) &&
                string.IsNullOrWhiteSpace(contact.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(contact.Address1_City) &&
                string.IsNullOrWhiteSpace(contact.Address1_Country) &&
                contact.ed_Address1_Country == null)
                &&
                (!string.IsNullOrWhiteSpace(lead.Address1_Line1) ||
                !string.IsNullOrWhiteSpace(lead.Address1_Line2) ||
                !string.IsNullOrWhiteSpace(lead.Address1_PostalCode) ||
                !string.IsNullOrWhiteSpace(lead.Address1_City) ||
                lead.ed_Address1_Country != null))
            {
                updated = true;
                contact.Address1_Line1 = lead.Address1_Line1;
                updContact.Address1_Line1 = lead.Address1_Line1;
                contact.Address1_Line2 = lead.Address1_Line2;
                updContact.Address1_Line2 = lead.Address1_Line2;
                contact.Address1_PostalCode = lead.Address1_PostalCode;
                updContact.Address1_PostalCode = lead.Address1_PostalCode;
                contact.Address1_City = lead.Address1_City;
                updContact.Address1_City = lead.Address1_City;
                contact.Address1_Country = lead.Address1_Country;
                updContact.Address1_Country = lead.Address1_Country;
                contact.ed_Address1_Country = lead.ed_Address1_Country;
                updContact.ed_Address1_Country = lead.ed_Address1_Country;
            }

            if ((contact.ed_InformationSource == null || !contact.ed_InformationSource.HasValue) && lead.ed_InformationSource != null && lead.ed_InformationSource.HasValue)
            {
                contact.ed_InformationSource = lead.ed_InformationSource;
                updContact.ed_InformationSource = lead.ed_InformationSource;
                updated = true;
            }

            return updated;
        }

        /// <summary>
        /// Verify if contact object needs to be updated from lead. No database verification.
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="lead">Lead to verify against</param>
        /// <returns></returns>
        public static void UpdateContactWithLeadKampanj(ref ContactEntity contact, LeadEntity lead)
        {
            // If dummy lastname has been used, Clear it!
            if (string.Equals(lead.LastName, LeadEntity.CreateLead_LastNameToUseIfEmpty))
            {
                contact.LastName = null;
                lead.LastName = null;
            }

            if (!string.IsNullOrWhiteSpace(lead.FirstName) && string.IsNullOrWhiteSpace(contact.FirstName))
                contact.FirstName = lead.FirstName;

            if (!string.IsNullOrWhiteSpace(lead.LastName) && string.IsNullOrWhiteSpace(contact.LastName))
                contact.LastName = lead.LastName;

            if (!string.IsNullOrWhiteSpace(lead.Telephone1) && string.IsNullOrWhiteSpace(contact.Telephone1))
                contact.Telephone1 = lead.Telephone1;

            if (!string.IsNullOrWhiteSpace(lead.MobilePhone) && string.IsNullOrWhiteSpace(contact.Telephone2))
                contact.Telephone2 = lead.MobilePhone;

            if (!string.IsNullOrWhiteSpace(lead.ed_Personnummer) && string.IsNullOrWhiteSpace(contact.cgi_socialsecuritynumber)) //Change this solcialsecurity number field (ed_socialsecuritynumberblock)
            {
                contact.cgi_socialsecuritynumber = lead.ed_Personnummer; //ed_socialsecuritynumberblock
                contact.BirthDate = ContactEntity.UpdateBirthDateOnContact(lead.ed_Personnummer); //DevOps 9168
                contact.ed_HasSwedishSocialSecurityNumber = lead.ed_HasSwedishSocialSecurityNumber;
            }

            //If any data in address fields in lead and none in Contact - replace everything
            if ((string.IsNullOrWhiteSpace(contact.Address1_Line1) &&
                string.IsNullOrWhiteSpace(contact.Address1_Line2) &&
                string.IsNullOrWhiteSpace(contact.Address1_PostalCode) &&
                string.IsNullOrWhiteSpace(contact.Address1_City) &&
                string.IsNullOrWhiteSpace(contact.Address1_Country) &&
                contact.ed_Address1_Country == null)
                &&
                (!string.IsNullOrWhiteSpace(lead.Address1_Line1) ||
                !string.IsNullOrWhiteSpace(lead.Address1_Line2) ||
                !string.IsNullOrWhiteSpace(lead.Address1_PostalCode) ||
                !string.IsNullOrWhiteSpace(lead.Address1_City) ||
                lead.ed_Address1_Country != null))
            {
                contact.Address1_Line1 = lead.Address1_Line1;
                contact.Address1_Line2 = lead.Address1_Line2;
                contact.Address1_PostalCode = lead.Address1_PostalCode;
                contact.Address1_City = lead.Address1_City;
                contact.Address1_Country = lead.Address1_Country;
                contact.ed_Address1_Country = lead.ed_Address1_Country;
            }

            if (contact.ed_InformationSource == null || !contact.ed_InformationSource.HasValue)
                contact.ed_InformationSource = Generated.ed_informationsource.Kampanj;

            if(lead.CampaignId != null)
                contact.ed_SourceCampaignId = lead.CampaignId;

            if (!string.IsNullOrWhiteSpace(contact.EMailAddress2))
            {
                contact.EMailAddress1 = contact.EMailAddress2;
                contact.EMailAddress2 = null;
            }
        }

        //public static ContactEntity CreateContactFromLead(Plugin.LocalPluginContext localContext, LeadEntity lead)
        //{
        //    if (lead == null)
        //    {
        //        return null;
        //    }
        //    // Mandatory Fields
        //    if (!lead.Contains(LeadEntity.Fields.FirstName) || string.IsNullOrWhiteSpace(lead.FirstName) ||
        //        !lead.Contains(LeadEntity.Fields.LastName) || string.IsNullOrWhiteSpace(lead.LastName) ||
        //        !lead.Contains(LeadEntity.Fields.Address1_Line2) || string.IsNullOrWhiteSpace(lead.Address1_Line2) ||
        //        !lead.Contains(LeadEntity.Fields.Address1_PostalCode) || string.IsNullOrWhiteSpace(lead.Address1_PostalCode) ||
        //        !lead.Contains(LeadEntity.Fields.Address1_City) || string.IsNullOrWhiteSpace(lead.Address1_City) ||
        //        !lead.Contains(LeadEntity.Fields.ed_Address1_Country) || lead.ed_Address1_Country == null ||
        //        !lead.Contains(LeadEntity.Fields.ed_Personnummer) || string.IsNullOrWhiteSpace(lead.ed_Personnummer) ||
        //        !lead.Contains(LeadEntity.Fields.EMailAddress1) || string.IsNullOrWhiteSpace(lead.EMailAddress1))
        //        throw new Exception(string.Format("Data missing from Lead when trying to create a Contact from Lead."));

        //    ContactEntity contactToBeCreated = new ContactEntity()
        //    {
        //        FirstName = lead.FirstName,
        //        LastName = lead.LastName,
        //        Address1_Line2 = lead.Address1_Line2,
        //        Address1_PostalCode = lead.Address1_PostalCode,
        //        Address1_City = lead.Address1_City,
        //        ed_Address1_Country = lead.ed_Address1_Country,
        //        cgi_socialsecuritynumber = lead.ed_Personnummer,
        //        EMailAddress1 = lead.EMailAddress1
        //    };
        //    if (!string.IsNullOrWhiteSpace(lead.Address1_Line1))
        //    {
        //        contactToBeCreated.Address1_Line1 = lead.Address1_Line1;
        //    }
        //    if (!string.IsNullOrWhiteSpace(lead.Telephone1))
        //    {
        //        contactToBeCreated.Telephone1 = lead.Telephone1;
        //    }
        //    if (!string.IsNullOrWhiteSpace(lead.MobilePhone))
        //    {
        //        contactToBeCreated.Telephone2 = lead.MobilePhone;
        //    }
        //    // lead ska kvalificeras, inte tas bort. 
        //    localContext.OrganizationService.Delete(LeadEntity.EntityLogicalName, lead.Id);

        //    contactToBeCreated.Id = lead.Id;

        //    Guid guid = localContext.OrganizationService.Create(contactToBeCreated);
        //    if (contactToBeCreated.Id != guid)
        //    {
        //        throw new Exception("Guid mismatch when upgrading customer.");
        //    }
        //    return contactToBeCreated;
        //}

        public static ContactEntity GetValidatedContactFromEmail(Plugin.LocalPluginContext localContext, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            FilterExpression emailFilterNew = new FilterExpression(LogicalOperator.Or)
            {
                Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email),
                        new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, email)
                    }
            };

            QueryExpression query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = ContactEntity.ContactInfoBlock,
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active),
                        //new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email)
                    },
                    Filters =
                    {
                        emailFilterNew //??
                    }
                }
            };

            IList<ContactEntity> contacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);

            if (contacts.Count == 0)
            {
                return null;
            }
            else if (contacts.Count == 1)
            {
                return contacts[0];
            }
            else
            {
                throw new Exception(string.Format("Inconsistency in database, more than one validated contact found for the same EmailAddress: {0}\nPlease contact administrator.", email));
            }
        }

        public static DateTime? UpdateBirthDateOnContact(string ssn) 
        {
            //DevOps 9168
            DateTime birthDate = new DateTime();
            bool failedParse = false;

            //Check length of the sent in ssn
            Regex regexSweSocSecLong = new Regex("^\\d{12}$"); //YYYY MM DD XXXX
            Regex regexSweSocSecShort = new Regex("^\\d{10}$"); //YY MM DD XXXX
            Regex regexDateLong = new Regex("^\\d{8}$"); //YYYY MM DD
            Regex regexDateShort = new Regex("^\\d{6}$"); //YY MM DD

            if (regexSweSocSecLong.IsMatch(ssn)) //YYYY MM DD XXXX
            {
                if (!DateTime.TryParse(ssn.Substring(0, 4) + "-" + ssn.Substring(4, 2) + "-" + ssn.Substring(6, 2), out birthDate))
                {
                    //The sent in date could not be parsed
                    failedParse = true;
                }
            }
            else if (regexSweSocSecShort.IsMatch(ssn)) //YY MM DD XXXX
            {
                var dateToParse = ssn.Substring(0, 2) + "-" + ssn.Substring(2, 2) + "-" + ssn.Substring(4, 2);
                if (!DateTime.TryParseExact(dateToParse, "yy-MM-dd", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out birthDate))
                {
                    //The sent in date could not be parsed
                    failedParse = true;
                }
            }
            else if (regexDateLong.IsMatch(ssn)) //YYYY MM DD
            {
                var dateToParse = ssn.Substring(0, 4) + "-" + ssn.Substring(4, 2) + "-" + ssn.Substring(6, 2);
                if (!DateTime.TryParse(dateToParse, out birthDate))
                {
                    //The sent in date could not be parsed
                    failedParse = true;
                }
            }
            else if (regexDateShort.IsMatch(ssn)) //YY MM DD
            {
                var dateToParse = ssn.Substring(0, 2) + "-" + ssn.Substring(2, 2) + "-" + ssn.Substring(4, 2);
                if (!DateTime.TryParseExact(dateToParse, "yy-MM-dd", null, System.Globalization.DateTimeStyles.AdjustToUniversal, out birthDate))
                {
                    //The sent in date could not be parsed
                    failedParse = true;
                }
            }
            else
            {
                failedParse = true;
            }

            if (failedParse == true)
            {
                return null;
            }
            return (DateTime?)birthDate;
        }

        [Obsolete]
        public static ContactEntity GetContactFromMkl_Id(Plugin.LocalPluginContext localContext, string mkl_id)
        {
            //Check if user exists in CRM before generating value code.
            var contactQuery_mklId = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(ContactEntity.Fields.ed_MklId, ContactEntity.Fields.ContactId),
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ed_MklId, ConditionOperator.Equal, mkl_id)
                        }
                    }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery_mklId);

            return contact;
        }

        /// <summary>
        /// Return a contact
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="columnSet"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static ContactEntity GetContactByNameEmailPhone(Plugin.LocalPluginContext localContext, ColumnSet columnSet, string firstName, string lastName, string email = null, string mobile = null)
        {

            var query = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(ContactEntity.Fields.FirstName, ConditionOperator.Equal, firstName),
                        new ConditionExpression(ContactEntity.Fields.LastName, ConditionOperator.Equal, lastName),
                        new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, email),
                        new ConditionExpression(ContactEntity.Fields.Telephone2, ConditionOperator.Equal, mobile),
                    }
                }
            };

            return XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, query);

        }

        /// <summary>
        /// Creates a contact if one doesn't exist in CRM or just bind existing contact with an existing travelcard.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="contact"></param>
        /// <param name="travelCard"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        public static ContactEntity CreateContactWithTravelCard(Plugin.LocalPluginContext localContext, TravelCardEntity travelCard, string firstName, string lastName, string email, string mobile)
        {
            localContext.TracingService.Trace($"Running CreateContactWithTravelCard");
            #region Validation
            if (localContext == null)
                throw new Exception(string.Format("Failed to connect to CRM API. Please check connection string. Localcontext is null."));

            else if (travelCard == null)
                throw new Exception($"Argument <{nameof(travelCard)}> is null.");

            else if (string.IsNullOrWhiteSpace(firstName))
                throw new Exception($"Argument <{nameof(firstName)}> is null.");

            else if (string.IsNullOrWhiteSpace(lastName))
                throw new Exception($"Argument <{nameof(lastName)}> is null.");

            else if (string.IsNullOrWhiteSpace(email))
                throw new Exception($"Argument <{nameof(email)}> is null.");

            else if (string.IsNullOrWhiteSpace(mobile))
                throw new Exception($"Argument <{nameof(mobile)}> is null.");
            #endregion

            /*
             *
             * If card is passes validation, then create a contact for customer in CRM if he/she doesn't already exist.
             * Check if first/lastname + mobile is in CRM, if so then associate travel card with that contact and if already associated with
             * card then don't skip.
             * Otherwise create a new contact in CRM.
             */
            try
            {
                var existingContact = ContactEntity.GetContactByNameEmailPhone(localContext, new ColumnSet(),
                                                                                firstName
                                                                                , lastName
                                                                                , email
                                                                                , mobile);

                //If no contact were found, create a new one in CRM.
                if (existingContact == null)
                {
                    // TODO - use common function to create a unvalidated contact?
                    var newContact = new ContactEntity()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        EMailAddress2 = email,
                        Telephone1 = mobile,
                        ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund
                    };

                    newContact.Id = XrmHelper.Create(localContext, newContact);

                    var contactTravelCard = new TravelCardEntity()
                    {
                        Id = travelCard.Id,
                        cgi_Contactid = newContact.ToEntityReference()
                    };

                    XrmHelper.Update(localContext, contactTravelCard);

                    return XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, newContact.ToEntityReference(), new ColumnSet());
                }
                else
                {
                    var contactTravelCard = new TravelCardEntity()
                    {
                        Id = travelCard.Id,
                        cgi_Contactid = existingContact.ToEntityReference()
                    };

                    XrmHelper.Update(localContext, contactTravelCard);

                    return XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, existingContact.ToEntityReference(), new ColumnSet());
                }
            }
            catch (Exception ex)
            {
                localContext.TracingService.Trace($"Error in ContactEntity.CreateContactWithTravelCard. Error: {ex}");
                throw;
            }
        }

        public static ContactEntity GetContactById(Plugin.LocalPluginContext localContext, ColumnSet columnSet, Guid contactId)
        {
            //Check if user exists in CRM before generating value code.
            var contactQuery = new QueryExpression()
            {
                EntityName = ContactEntity.EntityLogicalName,
                ColumnSet = columnSet,
                Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.ContactId, ConditionOperator.Equal, contactId)
                        }
                    }
            };

            var contact = XrmRetrieveHelper.RetrieveFirst<ContactEntity>(localContext, contactQuery);

            // Make sure contact is active
            if (contact.StateCode != Generated.ContactState.Active)
                return null;

            return contact;
        }

        /// <summary>
        /// Get contect from email. Searches both active and inactive contacts. Actice are preferred.
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="emailFrom"></param>
        /// <returns></returns>
        public static ContactEntity GetContactFromEmail(Plugin.LocalPluginContext localContext, string emailFrom)
        {
            IList<ContactEntity> intressenter = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, new ColumnSet(ContactEntity.Fields.StateCode),
                new FilterExpression(LogicalOperator.Or)
                {
                    Conditions =
                    {
                                new ConditionExpression(ContactEntity.Fields.EMailAddress1, ConditionOperator.Equal, emailFrom),
                                new ConditionExpression(ContactEntity.Fields.EMailAddress2, ConditionOperator.Equal, emailFrom),
                    }
                });
            ContactEntity intressent = null;
            foreach (ContactEntity c in intressenter)
            {
                if (c.StateCode == Generated.ContactState.Active)
                {
                    intressent = c;
                    break;
                }
                intressent = c;
            }

            return intressent;
        }

        /// <summary>
        /// Create dummy contact for Cases where no contact could be found using e-mail address.
        /// </summary>
        /// <param name="localContext"></param>
        /// <returns></returns>
        public static ContactEntity CreateUnvalidatedContactFromEmailToCaseProcess(Plugin.LocalPluginContext localContext, string emailFrom)
        {
            //IList<ContactEntity> dummyMatches = FindContactsOnContactNo(localContext, DefaultContactNumberEmailToCaseProcess);

            //if (dummyMatches.Count > 0)
            //{
            //    return dummyMatches[0];
            //}
            //else
            //{

            // Create new contact
            ContactEntity newDefaultContact = new ContactEntity();
            newDefaultContact.FirstName = "Inkommande";
            newDefaultContact.EMailAddress2 = emailFrom;
            newDefaultContact.LastName = "Oidentifierad intressent";
            newDefaultContact.ed_InformationSource = Generated.ed_informationsource.AdmSkapaKund;
            newDefaultContact.Id = localContext.OrganizationService.Create(newDefaultContact);
            return newDefaultContact;

            //}
        }

        [DataContract]
        public class MklValuePlaceholder
        {
            [DataMember]
            public bool valueExists { get; set; }
        }

        public static string Capitalise(string input)
        {
            if (input != null)
            {
                TextInfo textInfo = new CultureInfo("sv-SE").TextInfo;
                return textInfo.ToTitleCase(input.ToLower().Trim(" ".ToCharArray()));
            }
            return null;
        }
    }

    [DataContract]
    public class MklErrorObject
    {
        #region Static values
        //Authentication
        public const string CouldNotVerifyUser = "E0100";
        public const string NoUserFound = "E0101";
        public const string CantRefreshToken = "IE0102";
        public const string UserNotLoggedIn = "E0103";
        public const string TokenExpired = "IE0104";
        public const string TokenExpireTimeToLarge = "IE0105";
        public const string UserNotVerified = "E0106";
        public const string TokenInvalid = "IE0107";
        public const string NoAppId = "IE0108";
        public const string NonceNotValid = "IE0109";
        public const string NonceNotSet = "IE0110";
        public const string ExpireTimeNotSet = "IE0111";
        public const string UserNameEmpty = "E0112";
        public const string WrongResetToken = "E0113";
        public const string NoMobile = "E0114";
        public const string NoPassword = "E0115";
        public const string NoNewPassword = "E0116";
        public const string WrongMobileNumber = "IE0117";
        public const string NoMobileInCrm = "IE0118";
        public const string ExpiredResetToken = "E0119";
        public const string UnknownAuthenticationVersion = "IE0120";
        public const string UnknownEntityType = "IE0121";
        public const string UnableToValidateUser = "E0122";
        public const string CertificateNotFound = "IE0124";
        public const string NoCertStoreController = "IE0125";
        public const string ResetAttemptLimit = "E0126";
        public const string ResetTokenExists = "E0127";
        public const string WrongOrNonexistingCertificate = "IE0128";
        public const string NoClientCertificate = "IE0129";
        public const string InvalidClientCertificate = "IE0130";

        //Entity
        public const string AccountEntityNotFound = "IE0201";
        public const string CarrierEntityNotFound = "IE0202";
        public const string UserEntityNotFound = "IE0203";
        public const string UnknownUserEntity = "IE0204";
        public const string NoIdentifierOnUser = "IE0205";
        public const string NewUserNameError = "E0206";
        public const string NewUserNameIdenticalError = "E0207";
        public const string AccountEntityDelete = "IE0209";
        public const string UserEntityDelete = "IE0210";
        public const string UserEntityCreate = "E0211";
        public const string NoUserWithUsernameExists = "IE0212";
        public const string CarrierEntityDelete = "IE0213";
        public const string UserAlreadyExists = "E0214";
        public const string MissingCrmId = "IE0215";
        public const string CrmIdMissmatch = "IE0216";
        public const string AccountByUserIdNotFound = "IE0217";
        public const string UserParseValidateUserToken = "IE0218";
        public const string NoCountryCodeOnMobile = "E0219";

        //Identitfier
        public const string CarrierBadIdentifier = "IE0301";
        public const string AccountBadIdentifier = "IE0302";
        public const string CarrierParseGuidError = "IE0303";
        public const string UserBadIdentifier = "IE0304";
        public const string UserParseCrmIdError = "IE0305";

        //Swagger
        public const string FailedToGenerate = "IE0400";

        //Schema
        public const string SchemaValidationError = "E0500";
        public const string SchemaValidationBodyEmptyError = "E0501";
        public const string SchemaValidationBodyInvalid = "E0502";
        public const string SchemaValidationPasswordToShort = "E0503";
        public const string SchemaValidationPasswordNotValidNumeric = "E0504";
        public const string SchemaValidationPasswordNotValidAlphabetic = "E0505";

        //ByteArrayConverting
        public const string UnexpectedToken = "IE0600";
        public const string UnexpectedEnd = "IE0601";
        public const string UnexpectedParsing = "IE0602";

        //Integartion
        public const string CrmPost = "IE0700";
        public const string ConvertFromBase64 = "IE0701";
        public const string CrmGet = "IE0702";
        public const string EmailSend = "E0703";
        public const string NoPhoneNumber = "E0704";
        public const string NoSmsMessage = "E0705";
        public const string SendSms = "E0706";
        public const string CrmPut = "IE0707";

        //Failed Responses
        public const string Unauthorized = "IE0800";
        public const string Forbidden = "IE0801";
        public const string NotFound = "IE0802";
        public const string MethodNotAllowed = "IE0803";
        public const string InternalServerError = "E0804";
        public const string OnlyHttpsAllowed = "IE0805";

        //Internal errors
        public const string DbInconsistencyUser = "IE0900";
        public const string ArgumentNullOrEmpty = "IE0901";
        public const string KeyNullOrEmpty = "IE0902";
        public const string IvNullOrEmpty = "IE0903";

        #endregion

        #region  properties

        [DataMember]
        public string body { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string db { get; set; }
        [DataMember]
        public dynamic errors { get; set; }
        [DataMember]
        public Uri externalUrl { get; set; }
        //[DataMember]
        //public HttpStatusCode httpStatus { get; set; }
        [DataMember]
        public string httpStatus { get; set; }
        [DataMember]
        public string innerErrorMessage { get; set; }
        [DataMember]
        public Exception innerException { get; set; }
        [DataMember]
        public string localizedMessage { get; set; }
        [DataMember]
        public Guid logId { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string operation { get; set; }
        [DataMember]
        public Guid requestId { get; set; }
        [DataMember]
        public string route { get; set; }
        [DataMember]
        public string schema { get; set; }
        [DataMember]
        public string stackTrace { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public IDictionary<string, string> headers { get; set; }
        [DataMember]
        public IDictionary<object, object> data { get; set; }
        //[DataMember]
        //public DateTime timeStamp { get; set; }
        [DataMember]
        public string timeStamp { get; set; }
        [DataMember]
        public string environment { get; set; }
        [DataMember]
        public string serviceName { get; set; }
        [DataMember]
        public bool mklError { get; set; }

        #endregion
    }
}