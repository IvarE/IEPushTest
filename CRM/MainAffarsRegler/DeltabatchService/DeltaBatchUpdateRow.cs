using System;

namespace Endeavor.Crm.DeltabatchService
{
    public class DeltaBatchUpdateRow
    {
        public string TransactionGuid { get; set; }
        public string UpdateName { get; set; }
        public string ChangeCodesString { get; set; }
        public string RejectCode { get; set; }
        public string RejectText { get; set; }
        public string RejectComment { get; set; }
        public string GivenName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CoAddress { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Community { get; set; }
        public string SpecialCo { get; set; }
        public string SpecialCountry { get; set; }
        public string SpecialCity { get; set; }
        public string SpecialPostalCode { get; set; }
        public string SpecialRegAddr { get; set; }
        public string DateString { get; set; }
        public string County { get; set; }
        public string CountyNumberString { get; set; }
        public string CommunityNumberString { get; set; }
    }
}
