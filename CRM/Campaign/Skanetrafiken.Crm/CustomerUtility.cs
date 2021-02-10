using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skanetrafiken.Crm;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;
using Skanetrafiken.Crm.Entities;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm
{
    public class CustomerUtility
    {
        private static string tempLeadSubject;

        public enum StatusBlockCode
        {
            GenericError = 0,
            InvalidInput = 1,
            OtherLeadFound = 2,
            OtherContactFound = 3,
            NoConflictingEntity = 4,
            DataValid = 5
        }
        public enum StatusFlag
        {
            CreditSafeOk = 0,
            Avliden = 1,
            Utvandrad = 2,
            OgiltigEmail = 3
        }
        
        public enum Source
        {
            SkapaMittKonto = 0,
            ValideraEpost = 1,
            BytEpost = 2,
            OinloggatKop = 3,
            OinloggatKundArende = 4,
            RGOL = 5,
            PASS = 6,
            Kampanj = 7,
            AdmSkapaKund = 8,
            OinloggatLaddaKort = 9,
            UppdateraMittKonto = 10,
            Folkbokforing = 11,
            AdmAndraKund = 12
        }

        public static StatusBlock QuickFaultyStatus(StatusBlockCode code, string message)
        {
            return new StatusBlock()
            {
                TransactionOk = false,
                StatusBlockCode = (int)code,
                ErrorMessage = message
            };
        }
        
        public static StatusBlock ValidateLeadInfo(Plugin.LocalPluginContext localContext, LeadInfo leadInfo, ref CampaignEntity campaign)
        {
            StatusBlock returnStatusBlock = null;
            // Convert Lead to CustomerInfo
            CustomerInfo customerInfo = CustomerUtility.ToCustomerInfo(leadInfo);

            // Run ValidateCustomerInfo And return error if needed
            returnStatusBlock = ValidateCustomerInfo(localContext, customerInfo);
            if (!returnStatusBlock.TransactionOk)
            {
                return returnStatusBlock;
            }

            // Further Lead-specific Checks


            if (leadInfo.CampaignId == null)
            {
                returnStatusBlock = new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = Resources.CampaignIdMissing
                };
                return returnStatusBlock;
            }
            campaign = XrmRetrieveHelper.RetrieveFirst<CampaignEntity>(localContext, new ColumnSet(CampaignEntity.Fields.StateCode, CampaignEntity.Fields.ed_LeadSource, CampaignEntity.Fields.ed_LeadTopic,
                CampaignEntity.Fields.ed_ValidFromPhase1, CampaignEntity.Fields.ed_ValidToPhase1, CampaignEntity.Fields.ed_ValidFromPhase2, CampaignEntity.Fields.ed_ValidToPhase2, CampaignEntity.Fields.CodeName, CampaignEntity.Fields.ed_TotalLeads),
                new FilterExpression
                {
                    Conditions =
                    {
                            new ConditionExpression(CampaignEntity.Fields.CodeName, ConditionOperator.Equal, leadInfo.CampaignId)
                    }
                });
            if (campaign == null)
            {
                returnStatusBlock = new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = string.Format(Resources.CampaignMissing, leadInfo.CampaignCode)
                };
                return returnStatusBlock;
            }

            if (campaign.ed_LeadTopic == null)
            {
                returnStatusBlock = new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = string.Format(Resources.CampaignIncomplete, "Lead Topic (ed_LeadTopic)")
                };
                return returnStatusBlock;
            }

            if (campaign.ed_LeadSource == null)
            {
                returnStatusBlock = new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = string.Format(Resources.CampaignIncomplete, "Lead Source (ed_LeadSource)")
                };
                return returnStatusBlock;
            }

            if (!campaign.IsAcceptingResponse(localContext))
            {
                returnStatusBlock = new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = $"{Resources.CampaignInvalid}"
                };
                return returnStatusBlock;
            }

            return new StatusBlock
            {
                TransactionOk = true,
                StatusBlockCode = (int)StatusBlockCode.DataValid
            };
        }

        private static CustomerInfo ToCustomerInfo(LeadInfo leadInfo)
        {
            if (leadInfo == null)
                return null;

            CustomerInfo info = new CustomerInfo
            {
                Avliden = leadInfo.Avliden,
                AvlidenSpecified = leadInfo.AvlidenSpecified,
                CreditsafeOk = leadInfo.CreditsafeOk,
                CreditsafeOkSpecified = leadInfo.CreditsafeOkSpecified,
                Email = leadInfo.Email,
                EmailInvalid = leadInfo.EmailInvalid,
                EmailInvalidSpecified = leadInfo.EmailInvalidSpecified,
                FirstName = leadInfo.FirstName,
                Guid = leadInfo.Guid,
                isAddressEnteredManually = leadInfo.isAddressEnteredManually,
                isAddressEnteredManuallySpecified = leadInfo.isAddressEnteredManuallySpecified,
                isAddressInformationComplete = leadInfo.isAddressInformationComplete,
                isAddressInformationCompleteSpecified = leadInfo.isAddressInformationCompleteSpecified,
                LastName = leadInfo.LastName,
                MklId = leadInfo.MklId,
                Mobile = leadInfo.Mobile,
                NewEmail = leadInfo.NewEmail,
                SocialSecurityNumber = leadInfo.SocialSecurityNumber,
                Source = leadInfo.Source,
                SwedishSocialSecurityNumber = leadInfo.SwedishSocialSecurityNumber,
                SwedishSocialSecurityNumberSpecified = leadInfo.SwedishSocialSecurityNumberSpecified,
                Telephone = leadInfo.Telephone,
                Utvandrad = leadInfo.Utvandrad,
                UtvandradSpecified = leadInfo.UtvandradSpecified
            };

            if (leadInfo.AddressBlock != null)
            {
                info.AddressBlock = new CustomerInfoAddressBlock
                {
                    City = leadInfo.AddressBlock.City,
                    CO = leadInfo.AddressBlock.CO,
                    CountryISO = leadInfo.AddressBlock.CountryISO,
                    Line1 = leadInfo.AddressBlock.Line1,
                    PostalCode = leadInfo.AddressBlock.PostalCode
                };
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static StatusBlock ValidateCustomerInfo(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            if (customerInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = Properties.Resources.IncomingDataCannotBeNull
                };
            }
            EntityReference countryRef = null;
            if (customerInfo.AddressBlock != null && customerInfo.AddressBlock.CountryISO != null)
            {
                countryRef = CountryEntity.GetEntityRefForCountryCode(localContext, customerInfo.AddressBlock.CountryISO);
            }
            List<string> messages = new List<string>();
            bool error = false;
            switch (customerInfo.Source)
            {
                #region SkapaMittKonto
                case (int)Schema.Generated.ed_informationsource.SkapaMittKonto:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region BytEpost
                case (int)Schema.Generated.ed_informationsource.BytEpost:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email) ||
                        string.IsNullOrWhiteSpace(customerInfo.NewEmail))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region OinloggatKop
                case (int)Schema.Generated.ed_informationsource.OinloggatKop:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region OinloggatKundÄrende
                case (int)Schema.Generated.ed_informationsource.OinloggatKundArende:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region RGOL
                case (int)Schema.Generated.ed_informationsource.RGOL:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) &&
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(Resources.RGOLNeedsEmailOrSocSecNr);
                    }
                    break;
                #endregion
                #region PASS
                case (int)Schema.Generated.ed_informationsource.PASS:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region Kampanj
                case (int)Schema.Generated.ed_informationsource.Kampanj:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region AdmSkapaKund
                case (int)Schema.Generated.ed_informationsource.AdmSkapaKund:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region OinloggatLaddaKort
                case (int)Schema.Generated.ed_informationsource.OinloggatLaddaKort:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region UppdateraMittKonto
                case (int)Schema.Generated.ed_informationsource.UppdateraMittKonto:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.Mobile) ||
                        string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) ||
                        string.IsNullOrWhiteSpace(customerInfo.Email))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region Folkbokföring
                case (int)Schema.Generated.ed_informationsource.Folkbokforing:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName) ||
                        customerInfo.AddressBlock == null ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                        string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) ||
                        string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                #region AdmÄndraKund
                case (int)Schema.Generated.ed_informationsource.AdmAndraKund:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName) ||
                        string.IsNullOrWhiteSpace(customerInfo.LastName))
                    {
                        error = true;
                        messages.Add(ReturnMissingFields(localContext, customerInfo).ErrorMessage);
                    }
                    break;
                #endregion
                default:
                    return new StatusBlock()
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                        ErrorMessage = string.Format(Resources.InvalidSource, customerInfo.Source)
                    };
            }

            if ("SE".Equals(customerInfo.AddressBlock?.CountryISO?.ToUpper()) && !string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode) && !CheckPostalCodeFormat(customerInfo.AddressBlock.PostalCode))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForPostalCode, customerInfo.AddressBlock.PostalCode));
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Telephone) && !CheckPhoneNumberFormat(customerInfo.Telephone))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForTelephone, customerInfo.Telephone));
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Mobile) && !CheckMobilePhoneFormat(customerInfo.Mobile))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForMobile, customerInfo.Mobile));
            }
            //string errorMessage;
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && customerInfo.SwedishSocialSecurityNumber && !CheckPersonnummerFormat(customerInfo.SocialSecurityNumber/*, out errorMessage*/))
            {
                error = true;
                //messages.Add(string.Format(Resources.InvalidFormatForSocialSecurityNumber, $"{customerInfo.SocialSecurityNumber} mess: {errorMessage}"));
                messages.Add(string.Format(Resources.InvalidFormatForSocialSecurityNumber, customerInfo.SocialSecurityNumber));
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber) && !customerInfo.SwedishSocialSecurityNumber && !CheckDateFormat(customerInfo.SocialSecurityNumber))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForIntenationalSocialSecurityNumber, customerInfo.SocialSecurityNumber));
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.Email) && !CheckEmailFormat(customerInfo.Email))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForEmail, customerInfo.Email));
            }
            if (!string.IsNullOrWhiteSpace(customerInfo.NewEmail) && !CheckEmailFormat(customerInfo.NewEmail))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidNewEmail, customerInfo.NewEmail));
            }
            if (customerInfo.AddressBlock != null && !string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) && countryRef == null)
            {
                error = true;
                messages.Add(string.Format(Resources.CouldNotFindAValidCountryForISOCode, customerInfo.AddressBlock.CountryISO));
            }

            if (error)
            {
                return QuickFaultyStatus(StatusBlockCode.InvalidInput, FormatErrorMessages(messages));
            }

            return new StatusBlock()
            {
                TransactionOk = true,
                StatusBlockCode = (int)StatusBlockCode.DataValid
            };
        }

        private static string FormatErrorMessages(List<string> messages)
        {
            if (messages == null || messages.Count == 0)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (string s in messages)
            {
                sb.AppendLine(s);
                sb.Append("<BR>");
            }
            return sb.ToString();
        }

        private static StatusBlock ReturnMissingFields(Plugin.LocalPluginContext localContext, CustomerInfo customerInfo)
        {
            if (customerInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                    ErrorMessage = Resources.IncomingDataCannotBeNull
                };
            }
            List<string> missingFields = new List<string>();

            switch (customerInfo.Source)
            {
                #region SkapaMittKonto
                case (int)CustomerUtility.Source.SkapaMittKonto:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        missingFields.Add("Personnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                #region BytEpost
                case (int)CustomerUtility.Source.BytEpost:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        missingFields.Add("Personnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    if (string.IsNullOrWhiteSpace(customerInfo.NewEmail))
                        missingFields.Add("NewEmail");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                #region OinloggatKop
                case (int)CustomerUtility.Source.OinloggatKop:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                #region OinloggatKundArende, Kampanj, OinloggatLaddaKort
                case (int)CustomerUtility.Source.OinloggatKundArende:
                case (int)CustomerUtility.Source.Kampanj:
                case (int)CustomerUtility.Source.OinloggatLaddaKort:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    break;
                #endregion
                #region RGOL
                case (int)CustomerUtility.Source.RGOL:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        missingFields.Add("Personnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                #region UppdateraMittKonto
                case (int)CustomerUtility.Source.UppdateraMittKonto:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        missingFields.Add("Personnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.Mobile))
                        missingFields.Add("Mobilnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.Email))
                        missingFields.Add("Epost");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                #region AdmSkapaKund, AdmAndraKund
                case (int)CustomerUtility.Source.AdmSkapaKund:
                case (int)CustomerUtility.Source.AdmAndraKund:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    break;
                #endregion
                #region Folkbokforing, PASS
                case (int)CustomerUtility.Source.Folkbokforing:
                case (int)CustomerUtility.Source.PASS:
                    if (string.IsNullOrWhiteSpace(customerInfo.FirstName))
                        missingFields.Add("Förnamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.LastName))
                        missingFields.Add("Efternamn");
                    if (string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber))
                        missingFields.Add("Personnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.Line1))
                        missingFields.Add("Postadress");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.PostalCode))
                        missingFields.Add("Postnummer");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.City))
                        missingFields.Add("Ort");
                    if (string.IsNullOrWhiteSpace(customerInfo.AddressBlock?.CountryISO))
                        missingFields.Add("Land (ISO-kod)");
                    break;
                #endregion
                default:
                    return new StatusBlock
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                        ErrorMessage = string.Format(Resources.InvalidSource, customerInfo.Source)
                    };
            }
            if (missingFields.Count == 0)
            {
                return new StatusBlock
                {
                    TransactionOk = true,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.DataValid,
                    ErrorMessage = Resources.NoMissingFields
                };
            }

            StringBuilder sb = new StringBuilder(Resources.MissingFields);
            foreach (string s in missingFields)
            {
                sb.Append(string.Format("<BR>{0}, ", s));
            }
            sb.Remove(sb.Length-2, 2);

            return new StatusBlock
            {
                TransactionOk = false,
                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                ErrorMessage = sb.ToString()
            };
        }

        public static bool CheckEmailFormat(string email)
        {
            // Also accepts the letters [á, é, å, ä, ö].
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~\-áéåäö]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~\-áéåäö]+)*@(?:[a-z0-9áéåäö](?:[a-z0-9\-áéåäö]*[a-z0-9áéåäö])?\.)+[a-z0-9áéåäö](?:[a-z0-9\-áéåäö]*[a-z0-9áéåäö])?)\Z", RegexOptions.IgnoreCase);
            //return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~\-éåäö]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~\-éåäö]+)*@(?:[a-z0-9éåäö](?:[a-z0-9\-éåäö]*[a-z0-9éåäö])?\.)+[a-z0-9éåäö](?:[a-z0-9\-éåäö]*[a-z0-9éåäö])?)\Z", RegexOptions.IgnoreCase);
            //return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Make sure personnummer is in format YYYYMMDDXXXZ
        /// </summary>
        /// <param name="personnummer"></param>
        /// <returns></returns>
        public static bool CheckPersonnummerFormat(string personnummer/*, out string errorMess*/)
        {
            Regex regEx = new Regex("^[0-9]{12}$");
            if (!regEx.IsMatch(personnummer))
            {
                //errorMess = "Regex Fail";
                return false;
            }
            DateTime dt;
            if (!DateTime.TryParse(personnummer.Substring(0, 4) + "-" + personnummer.Substring(4, 2) + "-" + personnummer.Substring(6, 2), out dt))
            {
                //errorMess = "DateTime-Parse Fail";
                return false;
            }
            string pnr9digits = personnummer.Substring(2, 9);
            if (personnummer.Substring(11, 1).Equals(calculateCheckDigit(pnr9digits)))
            {
                //errorMess = "Checkdigit Fail";
                return true;
            }
            //errorMess = "no Error";
            return false;
        }

        private static bool CheckDateFormat(string socialSecurityNumber)
        {
            Regex regEx = new Regex("^[0-9]{8}$");
            if (!regEx.IsMatch(socialSecurityNumber))
                return false;
            DateTime dt;
            if (DateTime.TryParse(socialSecurityNumber.Substring(0, 4) + "-" + socialSecurityNumber.Substring(4, 2) + "-" + socialSecurityNumber.Substring(6, 2), out dt))
                return true;
            return false;
        }

        /// <summary>
        /// Calulate checkdigit using 9 chars
        /// Example: personnummer.Substring(2, 9);
        /// </summary>
        /// <param name="strDigits"></param>
        /// <returns></returns>
        public static string calculateCheckDigit(string strDigits)
        {
            List<int> digits = new List<int>();
            foreach (char c in strDigits.ToArray())
            {
                digits.Add(int.Parse($"{c}"));
            }
            return String.Format("{0}", calculateCheckDigit(digits));
        }

        private static int calculateCheckDigit(List<int> digits)
        {
            int digitSum = 0;
            int checkDigit = 0;
            int paritet = digits.Count % 2;
            for (int index = digits.Count - 1; index >= 0; index--)
            {
                int digitValue = digits[index];
                // varannan multipliceras med 2 och varannan med 1...
                digitValue = digitValue * (((index + paritet) % 2) + 1);
                if (digitValue > 9)
                {
                    digitSum += digitValue / 10;
                    digitSum += digitValue % 10;
                }
                else
                {
                    digitSum += digitValue;
                }
            }
            checkDigit = (10 - (digitSum % 10)) % 10;
            return checkDigit;
        }

        private static bool CheckMobilePhoneFormat(string mobile)
        {
            return Regex.Match(mobile, @"^([0-9]{6,16})$").Success;
        }

        private static bool CheckPhoneNumberFormat(string telephone)
        {
            return Regex.Match(telephone, @"^([0-9]{6,16})$").Success;
        }

        private static bool CheckPostalCodeFormat(string postalcode)
        {
            return Regex.Match(postalcode, @"^([0-9]{5})$").Success;
        }

        public static string GenerateValidSocialSecurityNumber(DateTime dateTime)
        {
            string datum = dateTime.ToString("yyyyMMdd").Replace("-", "").Substring(0,8);
            StringBuilder personnummer = new StringBuilder(datum);
            personnummer.Append(Random4Digits());
            string pnr9digits = personnummer.ToString().Substring(2, 9);
            personnummer[11] = CustomerUtility.calculateCheckDigit(pnr9digits)[0];
            return personnummer.ToString();
        }

        private static string Random4Digits()
        {
            Random rnd = new Random();
            int next = rnd.Next(0, 10000);
            return (next + 10000).ToString().Substring(1);
        }
    }
}
