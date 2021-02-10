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
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Microsoft.Xrm.Sdk;
using Skanetrafiken.Crm.Properties;

namespace Skanetrafiken.Crm
{
    public class AccountUtility
    {

        public enum StatusBlockCode
        {
            GenericError = 0,
            InvalidInput = 1,
            OtherLeadFound = 2,
            OtherAccountFound = 3,
            NoConflictingEntity = 4,
            DataValid = 5,
            NoAccountFound = 6
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localContext"></param>
        /// <param name="customerInfo"></param>
        /// <returns></returns>
        public static StatusBlock ValidateAccountInfo(Plugin.LocalPluginContext localContext, AccountInfo accountInfo, bool isPostMessage)
        {
            if (accountInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                    ErrorMessage = Properties.Resources.IncomingDataCannotBeNull
                };
            }

            List<string> messages = new List<string>();
            bool error = false;

            if (isPostMessage == true)
            {
                switch (accountInfo.InformationSource)
                {
                    #region ForetagsPortal
                    case (int)Schema.Generated.ed_informationsource.ForetagsPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationName) //Validera type of account?
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    case (int)Schema.Generated.ed_informationsource.SkolPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationName) //Validera type of account?
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    case (int)Schema.Generated.ed_informationsource.SeniorPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber) ||
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationName) //Validera type of account?
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    #endregion
                    default:
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                            ErrorMessage = string.Format(Resources.InvalidSource, accountInfo.InformationSource)
                        };
                }
            }
            else
            {
                switch (accountInfo.InformationSource)
                {
                    #region ForetagsPortal
                    case (int)Schema.Generated.ed_informationsource.ForetagsPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) &&
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber)
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    case (int)Schema.Generated.ed_informationsource.SkolPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) &&
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber)
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    case (int)Schema.Generated.ed_informationsource.SeniorPortal:
                        if (string.IsNullOrWhiteSpace(accountInfo.PortalId) &&
                            string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber)
                            )
                        {
                            error = true;
                            messages.Add(ReturnMissingFields(localContext, accountInfo).ErrorMessage);
                        }
                        break;
                    #endregion
                    default:
                        return new StatusBlock()
                        {
                            TransactionOk = false,
                            StatusBlockCode = (int)StatusBlockCode.InvalidInput,
                            ErrorMessage = string.Format(Resources.InvalidSource, accountInfo.InformationSource)
                        };
                }
            }
            #region Format Checks

            if(accountInfo.PaymentMethod.HasValue && !Enum.IsDefined(typeof(Generated.ed_account_ed_paymentmethod), accountInfo.PaymentMethod))
            {
                error = true;
                messages.Add(CrmPlusUtility.GetEnumString("PaymentMethod", typeof(Generated.ed_account_ed_paymentmethod)));
            }

            if (accountInfo.BillingMethod.HasValue && !Enum.IsDefined(typeof(Generated.ed_account_ed_billingmethod), accountInfo.BillingMethod))
            {
                error = true;
                messages.Add(CrmPlusUtility.GetEnumString("BillingMethod", typeof(Generated.ed_account_ed_billingmethod)));
            }

            if (accountInfo.Addresses != null && accountInfo.Addresses.Count > 0)
            {
                foreach (AddressInfo addressInfo in accountInfo.Addresses)
                {
                    EntityReference countryRef = null;
                    if (addressInfo.CountryISO != null)
                    {
                        countryRef = CountryEntity.GetEntityRefForCountryCode(localContext, addressInfo.CountryISO);

                        if ("SE".Equals(addressInfo?.CountryISO?.ToUpper()) && !string.IsNullOrWhiteSpace(addressInfo.PostalCode) && !CheckPostalCodeFormat(addressInfo.PostalCode))
                        {
                            error = true;
                            messages.Add(string.Format(Resources.InvalidFormatForPostalCode, addressInfo.PostalCode));
                        }

                        if (!string.IsNullOrWhiteSpace(addressInfo.CountryISO) && countryRef == null)
                        {
                            error = true;
                            messages.Add(string.Format(Resources.CouldNotFindAValidCountryForISOCode, addressInfo.CountryISO));
                        }
                    }

                    if (addressInfo.TypeCode.HasValue && !Enum.IsDefined(typeof(Generated.customeraddress_addresstypecode), addressInfo.TypeCode))
                    {
                        error = true;
                        messages.Add(CrmPlusUtility.GetEnumString("TypeCode", typeof(Generated.customeraddress_addresstypecode)));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(accountInfo.EMail) && !CheckEmailFormat(accountInfo.EMail))
            {
                error = true;
                messages.Add(string.Format(Resources.InvalidFormatForEmail, accountInfo.EMail));
            }

            //if (!string.IsNullOrWhiteSpace(accountInfo.) && !CheckEmailFormat(accountInfo.EMail))
            //{
            //    error = true;
            //    messages.Add(string.Format(Resources.InvalidFormatForEmail, accountInfo.EMail));
            //}
            //if (customerInfo.AddressBlock != null && !string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO) && countryRef == null)
            //{
            //error = true;
            //messages.Add(string.Format(Resources.CouldNotFindAValidCountryForISOCode, customerInfo.AddressBlock.CountryISO));
            //}
            #endregion

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

        private static StatusBlock ReturnMissingFields(Plugin.LocalPluginContext localContext, AccountInfo accountInfo)
        {
            if (accountInfo == null)
            {
                return new StatusBlock
                {
                    TransactionOk = false,
                    StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                    ErrorMessage = Resources.IncomingDataCannotBeNull
                };
            }
            List<string> missingFields = new List<string>();

            switch (accountInfo.InformationSource)
            {
                #region ForetagsPortal
                case (int)Crm.Schema.Generated.ed_informationsource.ForetagsPortal:
                    if (string.IsNullOrWhiteSpace(accountInfo.PortalId))
                        missingFields.Add("PortalId");
                    if (string.IsNullOrWhiteSpace(accountInfo.OrganizationNumber))
                        missingFields.Add("Organisationsnummer");
                    if (string.IsNullOrWhiteSpace(accountInfo.OrganizationName))
                        missingFields.Add("Företagsnamn");
                    break;
                #endregion
                default:
                    return new StatusBlock
                    {
                        TransactionOk = false,
                        StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                        ErrorMessage = string.Format(Resources.InvalidSource, accountInfo.InformationSource)
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
            sb.Remove(sb.Length - 2, 2);

            return new StatusBlock
            {
                TransactionOk = false,
                StatusBlockCode = (int)CustomerUtility.StatusBlockCode.InvalidInput,
                ErrorMessage = sb.ToString()
            };
        }

        public static bool CheckEmailFormat(string email)
        {
            if (email == ContactEntity._NEWEMAILDONE)
                return true;
            // Also accepts the letters [á, é, å, ä, ö].
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~\-áéåäö]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~\-áéåäö]+)*@(?:[a-z0-9áéåäö](?:[a-z0-9\-áéåäö]*[a-z0-9áéåäö])?\.)+[a-z0-9áéåäö](?:[a-z0-9\-áéåäö]*[a-z0-9áéåäö])?)\Z", RegexOptions.IgnoreCase);
            //return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~\-éåäö]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~\-éåäö]+)*@(?:[a-z0-9éåäö](?:[a-z0-9\-éåäö]*[a-z0-9éåäö])?\.)+[a-z0-9éåäö](?:[a-z0-9\-éåäö]*[a-z0-9éåäö])?)\Z", RegexOptions.IgnoreCase);
            //return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
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

        private static bool CheckPostalCodeFormat(string postalcode)
        {
            return Regex.Match(postalcode, @"^([0-9]{5})$").Success;
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
    }
}
