using System.Runtime.Serialization;
using System;
using System.Xml.Serialization;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("contact")]
public class Contact:XrmBaseEntity
{
    [XrmPrimaryKey]
    [Xrm("contactid")]
    public Guid ContactId { get; set; }


    [Xrm("fullname")]
    public string FullName { get; set; }


    [Xrm("firstname")]
    public string FirstName { get; set; }
   

    [Xrm("lastname")]
    public string LastName { get; set; }
    

    [Xrm("cgi_contactnumber")]
    public string ContactNumber { get; set; }
   

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


    public OptionSetValue CustomerType { get; set; }


    [Xrm("cgi_activated")]
    public bool InActive { get; set; }


    [Xrm("statuscode")]
    public OptionSetValue Deleted { get; set; }
}