namespace Skanetrafiken.Crm.ColumnBlockSchemas
{
    public static class CustomerInfoExtensions
    {
        public static bool IsEmigrated(this CustomerInfo customerInfo)
        {
            return customerInfo.UtvandradSpecified && customerInfo.Utvandrad;
        }

        public static bool IsProtected(this CustomerInfo customerInfo)
        {
            return customerInfo.SkyddadSpecified && customerInfo.Skyddad;
        }

        public static bool IsDeceased(this CustomerInfo customerInfo)
        {
            return customerInfo.AvlidenSpecified && customerInfo.Avliden;
        }

        /// <summary>
        /// Checks if any of the rejection code related fields are set, and returns true in that case.
        /// </summary>
        /// <returns></returns>
        public static bool IsRejected(this CustomerInfo customerInfo)
        {
            return customerInfo.IsEmigrated() || customerInfo.IsProtected() || customerInfo.IsDeceased();
        }

        public static bool AreNameFieldsEmpty(this CustomerInfo customerInfo)
        {
            return string.IsNullOrWhiteSpace(customerInfo.FirstName) || string.IsNullOrWhiteSpace(customerInfo.LastName);
        }

        public static bool IsSocialSecurityNumberEmpty(this CustomerInfo customerInfo)
        {
            return string.IsNullOrWhiteSpace(customerInfo.SocialSecurityNumber);
        }

        public static bool IsNullOrEmptyAddress(this CustomerInfo customerInfo)
        {
            return customerInfo.AddressBlock == null ||
                    string.IsNullOrWhiteSpace(customerInfo.AddressBlock.Line1) ||
                    string.IsNullOrWhiteSpace(customerInfo.AddressBlock.PostalCode) ||
                    string.IsNullOrWhiteSpace(customerInfo.AddressBlock.City) ||
                    string.IsNullOrWhiteSpace(customerInfo.AddressBlock.CountryISO);
        }
    }
}
