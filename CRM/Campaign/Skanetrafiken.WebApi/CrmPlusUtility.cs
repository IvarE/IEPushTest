using System;
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
    }
}