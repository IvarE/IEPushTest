using Endeavor.Crm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Skanetrafiken.Crm
{
    public class CrmPlusUtility
    {

        internal static StatusBlock GetConfigBoolSetting(string settingName)
        {
            // Make sure we have config parameter.
            string settingString = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(settingString))
            {
                return new StatusBlock()
                {
                    TransactionOk = false,
                    ErrorMessage = $"Konfiguration {settingName} saknas. vänligen kontakta administratör"
                };
            }
            bool settingBool = false;

            if (bool.TryParse(settingString, out settingBool))
                return new StatusBlock
                {
                    TransactionOk = true,
                    Information = settingBool.ToString()
                };
            else
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    ErrorMessage = $"Ogiltigt värde i inställning {settingName}: {settingString}. Vänligen kontakta administratör"
                };
            }
        }

        internal static string GetConfigStringSetting(string settingName)
        {

            string settingString = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrEmpty(settingString))
            {
#if DEBUG
                if (settingString.Equals("InternalTokenCertificateName"))
                {
                    return "SE111111111111-F11111";
                }
#endif
                throw new MissingFieldException($"Hittade inte inställning: {settingName}");
            }
            return settingString;
        }

        public static SendEmailResponse SendConfirmationEmail(Plugin.LocalPluginContext localContext, int threadId, ContactEntity contact)
        {
            string emailCofnfirmationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailConfirmation);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, emailCofnfirmationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, emailCofnfirmationTemplateName));

            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, contact.ToEntityReference());

            email.RegardingObjectId = contact.ToEntityReference();

            return SetToFromAndSendEmail(localContext, threadId, contact.ToEntityReference(), email);
        }

        public static SendEmailResponse SendValidationEmail(Plugin.LocalPluginContext localContext, int threadId, ContactEntity to)
        {
            string contactValidationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailValidationContact);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, contactValidationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, contactValidationTemplateName));

            CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, EmailEntity.CgiSettingColumnSet);
            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, to.ToEntityReference(), new List<Entity> { (Entity)to, (Entity)setting });

            email.RegardingObjectId = to.ToEntityReference();

            return SetToFromAndSendEmail(localContext, threadId, to, email);
        }

        public static SendEmailResponse SendValidationEmail(Plugin.LocalPluginContext localContext, int threadId, LeadEntity to)
        {
            string leadValidationTemplateName = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_TemplateTitleEmailValidationLead);
            QueryExpression query = new QueryExpression()
            {
                EntityName = TemplateEntity.EntityLogicalName,
                ColumnSet = new ColumnSet(TemplateEntity.Fields.Title),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(TemplateEntity.Fields.Title, ConditionOperator.Equal, leadValidationTemplateName)
                    }
                }
            };

            TemplateEntity template = XrmRetrieveHelper.RetrieveFirst<TemplateEntity>(localContext, query);
            if (template == null)
                throw new Exception(string.Format(Resources.CouldNotFindEmailTemplate, leadValidationTemplateName));

            CgiSettingEntity setting = XrmRetrieveHelper.RetrieveFirst<CgiSettingEntity>(localContext, EmailEntity.CgiSettingColumnSet);

            EmailEntity email = EmailEntity.CreateEmailFromTemplate(localContext, template, to.ToEntityReference(), new List<Entity> { (Entity)to, (Entity)setting });

            email.RegardingObjectId = to.ToEntityReference();

            return SetToFromAndSendEmail(localContext, threadId, to.ToEntityReference(), email);
        }

        private static SendEmailResponse SetToFromAndSendEmail(Plugin.LocalPluginContext localContext, int threadId, ContactEntity to, EmailEntity email)
        {
            using (var _logger = new AppInsightsLogger())
            {
                EntityReference defaultQueue;
                try
                {
                    defaultQueue = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue);
                }
                catch (MissingFieldException e)
                {
                    _logger.LogError($"MissingFieldException caught when calling GetSettingEntRef() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                    throw e;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Exception caught when calling GetSettingEntRef() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                    throw e;
                }

                // Create the 'From:' activity party for the email
                ActivityPartyEntity fromParty = new ActivityPartyEntity
                {
                    PartyId = defaultQueue
                };

                // Create the 'To:' activity party for the email
                ActivityPartyEntity toParty = new ActivityPartyEntity
                {
                    PartyId = to.ToEntityReference(),
                    AddressUsed = to.ed_EmailToBeVerified
                };

                email.To = new ActivityPartyEntity[] { toParty };
                email.From = new ActivityPartyEntity[] { fromParty };

                email.Id = localContext.OrganizationService.Create(email);

                // Use the SendEmail message to send an e-mail message.
                SendEmailRequest sendEmailreq = new SendEmailRequest
                {
                    EmailId = email.Id,
                    IssueSend = true
                };

                SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(sendEmailreq);
                return sendEmailresp;
            }
        }

        private static SendEmailResponse SetToFromAndSendEmail(Plugin.LocalPluginContext localContext, int threadId, EntityReference to, EmailEntity email)
        {
            using (var _logger = new AppInsightsLogger())
            {
                EntityReference defaultQueue;
                try
                {
                    defaultQueue = CgiSettingEntity.GetSettingEntRef(localContext, CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue);
                }
                catch (MissingFieldException e)
                {
                    _logger.LogError($"MissingFieldException caught when calling GetSettingInt() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                    throw e;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Exception caught when calling GetSettingInt() for {CgiSettingEntity.Fields.cgi_Defaultoutgoingemailqueue}. Message = {e.Message}");
                    throw e;
                }

                // Create the 'From:' activity party for the email
                ActivityPartyEntity fromParty = new ActivityPartyEntity
                {
                    PartyId = defaultQueue
                };

                // Create the 'To:' activity party for the email
                ActivityPartyEntity toParty = new ActivityPartyEntity
                {
                    PartyId = to
                };

                email.To = new ActivityPartyEntity[] { toParty };
                email.From = new ActivityPartyEntity[] { fromParty };

                email.Id = localContext.OrganizationService.Create(email);

                // Use the SendEmail message to send an e-mail message.
                SendEmailRequest sendEmailreq = new SendEmailRequest
                {
                    EmailId = email.Id,
                    IssueSend = true
                };

                SendEmailResponse sendEmailresp = (SendEmailResponse)localContext.OrganizationService.Execute(sendEmailreq);

                return sendEmailresp;
            }
        }

        public static void CombineLeadInfos(Plugin.LocalPluginContext localContext, LeadInfo target, LeadInfo existingLead)
        {
            if (existingLead == null)
                return;
            #region boolean values
            #region Avliden
            if (!target.AvlidenSpecified && existingLead.AvlidenSpecified)
            {
                target.Avliden = existingLead.Avliden;
                target.AvlidenSpecified = true;
            }
            #endregion
            #region CampaignDiscountPercent
            if (!target.CampaignDiscountPercentSpecified && existingLead.CampaignDiscountPercentSpecified)
            {
                target.CampaignDiscountPercent = existingLead.CampaignDiscountPercent;
                target.CampaignDiscountPercentSpecified = true;
            }
            #endregion
            #region CreditsafeOk
            if (!target.CreditsafeOkSpecified && existingLead.CreditsafeOkSpecified)
            {
                target.CreditsafeOk = existingLead.CreditsafeOk;
                target.CreditsafeOkSpecified = true;
            }
            #endregion
            #region EmailInvalid
            if (!target.EmailInvalidSpecified && existingLead.EmailInvalidSpecified)
            {
                target.EmailInvalid = existingLead.EmailInvalid;
                target.EmailInvalidSpecified = true;
            }
            #endregion
            #region isAddressEnteredManually
            if (!target.isAddressEnteredManuallySpecified && existingLead.isAddressEnteredManuallySpecified)
            {
                target.isAddressEnteredManually = existingLead.isAddressEnteredManually;
                target.isAddressEnteredManuallySpecified = true;
            }
            #endregion
            #region isAddressInformationComplete
            if (!target.isAddressInformationCompleteSpecified && existingLead.isAddressInformationCompleteSpecified)
            {
                target.isAddressInformationComplete = existingLead.isAddressInformationComplete;
                target.isAddressInformationCompleteSpecified = true;
            }
            #endregion
            #region IsCampaignActive
            if (!target.IsCampaignActiveSpecified && existingLead.IsCampaignActiveSpecified)
            {
                target.IsCampaignActive = existingLead.IsCampaignActive;
                target.IsCampaignActiveSpecified = true;
            }
            #endregion
            #region SwedishSocialSecurityNumber
            if (!target.SwedishSocialSecurityNumberSpecified && existingLead.SwedishSocialSecurityNumberSpecified)
            {
                target.SwedishSocialSecurityNumber = existingLead.SwedishSocialSecurityNumber;
                target.SwedishSocialSecurityNumberSpecified = true;
            }
            #endregion
            #region Utvandrad
            if (!target.UtvandradSpecified && existingLead.UtvandradSpecified)
            {
                target.Utvandrad = existingLead.Utvandrad;
                target.UtvandradSpecified = true;
            }
            #endregion
            #endregion
            #region AddressBlock
            if (target.AddressBlock == null && existingLead.AddressBlock != null)
                target.AddressBlock = new CustomerInfoAddressBlock
                {
                    CO = existingLead.AddressBlock.CO,
                    Line1 = existingLead.AddressBlock.Line1,
                    PostalCode = existingLead.AddressBlock.PostalCode,
                    City = existingLead.AddressBlock.City,
                    CountryISO = existingLead.AddressBlock.CountryISO
                };
            else if (target.AddressBlock != null)
            {
                if (string.IsNullOrWhiteSpace(target.AddressBlock.CO) && !string.IsNullOrWhiteSpace(existingLead.AddressBlock.CO))
                {
                    target.AddressBlock.CO = existingLead.AddressBlock.CO;
                }
                if (string.IsNullOrWhiteSpace(target.AddressBlock.Line1) && !string.IsNullOrWhiteSpace(existingLead.AddressBlock.Line1))
                {
                    target.AddressBlock.Line1 = existingLead.AddressBlock.Line1;
                }
                if (string.IsNullOrWhiteSpace(target.AddressBlock.PostalCode) && !string.IsNullOrWhiteSpace(existingLead.AddressBlock.PostalCode))
                {
                    target.AddressBlock.PostalCode = existingLead.AddressBlock.PostalCode;
                }
                if (string.IsNullOrWhiteSpace(target.AddressBlock.City) && !string.IsNullOrWhiteSpace(existingLead.AddressBlock.City))
                {
                    target.AddressBlock.City = existingLead.AddressBlock.City;
                }
                if (string.IsNullOrWhiteSpace(target.AddressBlock.CountryISO) && !string.IsNullOrWhiteSpace(existingLead.AddressBlock.CountryISO))
                {
                    target.AddressBlock.CountryISO = existingLead.AddressBlock.CountryISO;
                }
            }
            #endregion
            #region CampaignCode
            if (string.IsNullOrWhiteSpace(target.CampaignCode) && !string.IsNullOrWhiteSpace(existingLead.CampaignCode))
            {
                target.CampaignCode = existingLead.CampaignCode;
            }
            #endregion
            #region CampaignId
            if (string.IsNullOrWhiteSpace(target.CampaignId) && !string.IsNullOrWhiteSpace(existingLead.CampaignId))
            {
                target.CampaignId = existingLead.CampaignId;
            }
            #endregion
            #region CampaignProducts
            if (target.CampaignProducts == null && existingLead.CampaignProducts != null)
            {
                target.CampaignProducts = existingLead.CampaignProducts;
            }
            #endregion
            #region Email
            if (string.IsNullOrWhiteSpace(target.Email) && !string.IsNullOrWhiteSpace(existingLead.Email))
            {
                target.Email = existingLead.Email;
            }
            #endregion
            #region FirstName
            if (string.IsNullOrWhiteSpace(target.FirstName) && !string.IsNullOrWhiteSpace(existingLead.FirstName))
            {
                target.FirstName = existingLead.FirstName;
            }
            #endregion
            #region Guid
            if (string.IsNullOrWhiteSpace(target.Guid) && !string.IsNullOrWhiteSpace(existingLead.Guid))
            {
                target.Guid = existingLead.Guid;
            }
            #endregion
            #region LastName
            if (string.IsNullOrWhiteSpace(target.LastName) && !string.IsNullOrWhiteSpace(existingLead.LastName))
            {
                target.LastName = existingLead.LastName;
            }
            #endregion
            #region MklId
            if (string.IsNullOrWhiteSpace(target.MklId) && !string.IsNullOrWhiteSpace(existingLead.MklId))
            {
                target.MklId = existingLead.MklId;
            }
            #endregion
            #region Mobile
            if (string.IsNullOrWhiteSpace(target.Mobile) && !string.IsNullOrWhiteSpace(existingLead.Mobile))
            {
                target.Mobile = existingLead.Mobile;
            }
            #endregion
            #region NewEmail
            if (string.IsNullOrWhiteSpace(target.NewEmail) && !string.IsNullOrWhiteSpace(existingLead.NewEmail))
            {
                target.NewEmail = existingLead.NewEmail;
            }
            #endregion
            #region SocialSecurityNumber
            if (string.IsNullOrWhiteSpace(target.SocialSecurityNumber) && !string.IsNullOrWhiteSpace(existingLead.SocialSecurityNumber))
            {
                target.SocialSecurityNumber = existingLead.SocialSecurityNumber;
            }
            #endregion
            #region Telephone
            if (string.IsNullOrWhiteSpace(target.Telephone) && !string.IsNullOrWhiteSpace(existingLead.Telephone))
            {
                target.Telephone = existingLead.Telephone;
            }
            #endregion
        }

        internal static string GetEnumString(string listName, Type enumType)
        {
            string returnString = $"Enum out of range. Valid values for {listName}: ";

            bool firstValue = true;
            foreach (var value in Enum.GetValues(enumType))
            {
                if (firstValue)
                {
                    returnString = $"{returnString} {value} ({(int)value})";
                }
                else
                {
                    returnString = $"{returnString}, {value} ({(int)value})";
                }

                firstValue = false;
            }
            return $"{returnString}.";
        }
    }
}