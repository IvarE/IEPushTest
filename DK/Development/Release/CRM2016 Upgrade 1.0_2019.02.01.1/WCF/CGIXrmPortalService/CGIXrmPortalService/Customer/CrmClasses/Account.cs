using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

namespace CGICRMPortalService.Customer.CrmClasses
{
    [XrmEntity("account")]
    public class Account : XrmBaseEntity
    {
        #region Public Properties
        [XrmPrimaryKey]
        [Xrm("accountid")]
        public Guid AccountId { get; set; }

        [Xrm("name")]
        public string Name { get; set; }


        [Xrm("cgi_firstname")]
        public string AccountFirstName { get; set; }


        [Xrm("cgi_lastname")]
        public string AccountLastName { get; set; }


        [Xrm("primarycontact.firstname")]
        public string PrimaryContactFirstName { get; set; }


        [Xrm("primarycontact.lastname")]
        public string PrimaryContactLastName { get; set; }


        [Xrm("accountnumber")]
        public string AccountNumber { get; set; }


        [Xrm("cgi_socialsecuritynumber")]
        public string SocialSecurtiyNumber { get; set; }


        [Xrm("telephone1")]
        public string MainPhone { get; set; }


        [Xrm("telephone2")]
        public string OtherPhone { get; set; }


        [Xrm("emailaddress1")]
        public string Email { get; set; }


        [Xrm("address1_addressid")]
        public Guid AddressId { get; set; }


        [Xrm("address1_name")]
        public string Address1_Name { get; set; }


        [Xrm("address1_line1")]
        public string Address1_CareOff { get; set; }


        [Xrm("address1_line2")]
        public string Address1_Street1 { get; set; }


        [Xrm("address1_postalcode")]
        public string Address1_PostalCode { get; set; }


        [Xrm("address1_city")]
        public string Address1_City { get; set; }


        [Xrm("address1_county")]
        public string Address1_County { get; set; }


        [Xrm("address1_country")]
        public string Address1_Country { get; set; }


        [Xrm("address1_primarycontactname")]
        public string Address1_ContactPerson { get; set; }


        [Xrm("address1_telephone1")]
        public string Address1_ContactPersonPhoneNumber { get; set; }


        [Xrm("address1_telephone2")]
        public string Address1_SMSNotificationNumber { get; set; }


        [Xrm("donotbulkemail")]
        public bool AllowBulkEmail { get; set; }


        [Xrm("donotemail")]
        public bool AllowEmail { get; set; }


        [Xrm("donotphone")]
        public bool AllowPhone { get; set; }


        [Xrm("donotpostalmail")]
        public bool AllowMail { get; set; }


        [Xrm("cgi_allow_autoload")]
        public bool AllowAutoLoad { get; set; }


        [Xrm("cgi_max_cards_autoload")]
        public int MaxCardsAutoLoad { get; set; }


        [Xrm("accountcategorycode")]
        public OptionSetValue CustomerType { get; set; }


        [Xrm("cgi_debtcollection")]
        public bool OrganizationCreditApproved { get; set; }


        [Xrm("cgi_organizational_number")]
        public string OrganizationNumber { get; set; }


        [Xrm("cgi_organization_sub_number")]
        public string OrganizationSubNumber { get; set; }


        [Xrm("cgi_activated")]
        public bool InActive { get; set; }


        [Xrm("statuscode")]
        public OptionSetValue Deleted { get; set; }

        [Xrm("cgi_rsid")]
        public string CareOff { get; set; }
        #endregion
    }
}