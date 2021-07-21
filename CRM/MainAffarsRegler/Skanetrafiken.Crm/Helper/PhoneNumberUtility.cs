using System.Linq;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;

namespace Skanetrafiken.Crm
{
    public class PhoneNumberUtility
    {
        public static List<CountryEntity> GetAllowedCountriesPhoneCodes(Plugin.LocalPluginContext localContext)
        {
            QueryExpression queryCountries = new QueryExpression(CountryEntity.EntityLogicalName);
            queryCountries.NoLock = true;
            queryCountries.ColumnSet = new ColumnSet(CountryEntity.Fields.ed_PhoneCode, CountryEntity.Fields.ed_PhoneRegex);
            queryCountries.Criteria.AddCondition(CountryEntity.Fields.ed_AllowedCountryPhone, ConditionOperator.Equal, true);

            return XrmRetrieveHelper.RetrieveMultiple<CountryEntity>(localContext, queryCountries);
        }

        public static bool CheckRegexPhoneNumber(Plugin.LocalPluginContext localContext, string phoneNumber)
        {
            List<CountryEntity> lCountries = GetAllowedCountriesPhoneCodes(localContext);
            localContext.Trace($"(CheckRegexPhoneNumber) Found {lCountries.Count} Allowed Countries Phone Codes.");

            foreach (CountryEntity country in lCountries)
            {
                if (System.Text.RegularExpressions.Regex.Match(phoneNumber, country.ed_PhoneRegex).Success)
                    return true;
            }

            return false;
        }

        public static string CheckPhoneFormatCreateValueCodeGeneric(Plugin.LocalPluginContext localContext, string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            string uPhoneNumber = phoneNumber;

            List<CountryEntity> lCountries = GetAllowedCountriesPhoneCodes(localContext);
            localContext.Trace($"(CheckPhoneFormatCreateValueCodeGeneric) Found {lCountries.Count} Allowed Countries Phone Codes.");

            foreach (CountryEntity country in lCountries)
            {
                string phoneCode = country.ed_PhoneCode;

                if (string.IsNullOrEmpty(phoneCode))
                    continue;

                if (phoneNumber.StartsWith("00" + phoneCode))
                {
                    uPhoneNumber = "+" + phoneCode + phoneNumber.Substring(4);
                    break;
                }
                else if (phoneNumber.StartsWith(phoneCode) && phoneNumber.Length == 11)
                {
                    uPhoneNumber = "+" + phoneCode + phoneNumber.Substring(2);
                    break;
                }
                else if (phoneNumber.StartsWith("07") && phoneNumber.Length == 10)
                {
                    uPhoneNumber = "+" + phoneCode + phoneNumber.Substring(1);
                    break;
                }
            }

            return uPhoneNumber;
        }
    }
}
