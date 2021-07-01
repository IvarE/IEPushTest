using System.Linq;
using Endeavor.Crm;
using Skanetrafiken.Crm.Entities;
using System.Collections.Generic;

namespace Skanetrafiken.Crm
{
    public class PhoneNumberUtility
    {
        public static string CheckPhoneFormatCreateValueCodeGeneric(Plugin.LocalPluginContext localContext, string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return phoneNumber;

            string uPhoneNumber = phoneNumber;

            string countriesAllowed = CgiSettingEntity.GetSettingString(localContext, CgiSettingEntity.Fields.ed_CountriesAllowed);
            localContext.Trace($"(CheckPhoneFormatCreateValueCodeGeneric) API: {countriesAllowed}");

            List<string> lCountries = countriesAllowed.Split(';').ToList();

            foreach (var country in lCountries)
            {
                if (phoneNumber.StartsWith("00" + country))
                {
                    phoneNumber = "+" + country + phoneNumber.Substring(4);
                    break;
                }
                else if (phoneNumber.StartsWith(country) && phoneNumber.Length == 11)
                {
                    phoneNumber = "+" + country + phoneNumber.Substring(2);
                    break;
                }
                else if (phoneNumber.StartsWith("07") && phoneNumber.Length == 10)
                {
                    phoneNumber = "+" + country + phoneNumber.Substring(1);
                    break;
                }
            }

            return phoneNumber;
        }
    }
}
