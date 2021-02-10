using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGIXrmEAIConnectorService.AllBinary.CrmClasses
{
    [XrmEntity("customeraddress")]
    public class CustomerAddress
    {
        #region Public Properties

        [XrmPrimaryKey]
        [Xrm("customeraddressid")]
        public Guid CustomerAddressId { get; set; }

        [Xrm("parentid")]
        public EntityReference ParentId { get; set; }

        [Xrm("cgi_addressid")]
        public string AddressId { get; set; }

        [Xrm("name")]
        public string CompanyName { get; set; }

        [Xrm("line2")]
        public string Street { get; set; }

        [Xrm("postalcode")]
        public string PostalCode { get; set; }

        [Xrm("city")]
        public string City { get; set; }

        [Xrm("county")]
        public string County { get; set; }

        [Xrm("country")]
        public string Country { get; set; }

        [Xrm("line1")]
        public string CareOff { get; set; }

        [Xrm("primarycontactname")]
        public string ContactPerson { get; set; }

        [Xrm("telephone1")]
        public string ContactPhoneNumber { get; set; }

        [Xrm("telephone2")]
        public string SMSNotificationNumber { get; set; }

        [Xrm("addresstypecode")]
        public OptionSetValue AddressType { get; set; }

        #endregion
    }
}