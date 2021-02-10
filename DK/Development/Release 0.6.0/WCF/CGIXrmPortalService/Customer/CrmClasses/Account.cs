using System.Runtime.Serialization;
using System;
using System.Xml.Serialization;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;



[XrmEntity("account")]
public class Account:XrmBaseEntity
{

    private Guid _AccountIdId;
    [XrmPrimaryKey]
    [Xrm("accountid")]
    public Guid AccountId
    {
        get { return _AccountIdId; }
        set { _AccountIdId = value; }
    }

    private string _Name;
    [Xrm("name")]
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    private string _AccountFirstName;
    [Xrm("cgi_firstname")]
    public string AccountFirstName
    {
        get { return _AccountFirstName; }
        set { _AccountFirstName = value; }
    }
   
    private string _AccountLastName;
    [Xrm("cgi_lastname")]
    public string AccountLastName
    {
        get { return _AccountLastName; }
        set { _AccountLastName = value; }
    }
    private string _PrimaryContactFirstName;
    [Xrm("primarycontact.firstname")]
    public string PrimaryContactFirstName
    {
        get { return _PrimaryContactFirstName; }
        set { _PrimaryContactFirstName = value; }
    }
    private string _PrimaryContactLastName;
    [Xrm("primarycontact.lastname")]
    public string PrimaryContactLastName
    {
        get { return _PrimaryContactLastName; }
        set { _PrimaryContactLastName = value; }
    }
    private string _AccountNumber;
    [Xrm("accountnumber")]
    public string AccountNumber
    {
        get { return _AccountNumber; }
        set { _AccountNumber = value; }
    }

   

    private string _SocialSecurtiyNumber;
    [Xrm("cgi_socialsecuritynumber")]
    public string SocialSecurtiyNumber
    {
        get { return _SocialSecurtiyNumber; }
        set { _SocialSecurtiyNumber = value; }
    }

    private string _MainPhone;
    [Xrm("telephone1")]
    public string MainPhone
    {
        get { return _MainPhone; }
        set { _MainPhone = value; }
    }
    private string _OtherPhone;
    [Xrm("telephone2")]
    public string OtherPhone
    {
        get { return _OtherPhone; }
        set { _OtherPhone = value; }
    }
    
    private string _Email;
    [Xrm("emailaddress1")]
    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }
    //private string _Telephone3;
    //[Xrm("telephone3")]
    //public string Telephone3
    //{
    //    get { return _Telephone3; }
    //    set { _Telephone3 = value; }
    //}
    private Guid _AddressId;
    //[Xrm("cgi_addressid")]
    [Xrm("address1_addressid")]
    public Guid AddressId
    {
        get { return _AddressId; }
        set { _AddressId = value; }
    }
    private string _Address1_Name;
    [Xrm("address1_name")]
    public string Address1_Name
    {
        get { return _Address1_Name; }
        set { _Address1_Name = value; }
    }
    private string _Address1_CareOff;
    [Xrm("address1_line1")]
    public string Address1_CareOff
    {
        get { return _Address1_CareOff; }
        set { _Address1_CareOff = value; }
    }
    private string _Address1_Street1;
    [Xrm("address1_line2")]
    public string Address1_Street1
    {
        get { return _Address1_Street1; }
        set { _Address1_Street1 = value; }
    }
    private string _Address1_PostalCode;
    [Xrm("address1_postalcode")]
    public string Address1_PostalCode
    {
        get { return _Address1_PostalCode; }
        set { _Address1_PostalCode = value; }
    }
    private string _Address1_City;
    [Xrm("address1_city")]
    public string Address1_City
    {
        get { return _Address1_City; }
        set { _Address1_City = value; }
    }
    private string _Address1_County;
    [Xrm("address1_county")]
    public string Address1_County
    {
        get { return _Address1_County; }
        set { _Address1_County = value; }
    }
    private string _Address1_Country;
    [Xrm("address1_country")]
    public string Address1_Country
    {
        get { return _Address1_Country; }
        set { _Address1_Country = value; }
    }
    private string _Address1_ContactPerson;
    [Xrm("address1_primarycontactname")]
    public string Address1_ContactPerson
    {
        get { return _Address1_ContactPerson; }
        set { _Address1_ContactPerson = value; }
    }

    private string _Address1_ContactPersonPhoneNumber;
    [Xrm("address1_telephone1")]
    public string Address1_ContactPersonPhoneNumber
    {
        get { return _Address1_ContactPersonPhoneNumber; }
        set { _Address1_ContactPersonPhoneNumber = value; }
    }

    private string _Address1_SMSNotificationNumber;
    [Xrm("address1_telephone2")]
    public string Address1_SMSNotificationNumber
    {
        get { return _Address1_SMSNotificationNumber; }
        set { _Address1_SMSNotificationNumber = value; }

    }

    //private string _Address2_Street1;
    //[Xrm("address2_line1")]
    //public string Address2_Street1
    //{
    //    get { return _Address2_Street1; }
    //    set { _Address2_Street1 = value; }
    //}
    //private string _Address2_PostalCode;
    //[Xrm("address2_postalcode")]
    //public string Address2_PostalCode
    //{
    //    get { return _Address2_PostalCode; }
    //    set { _Address2_PostalCode = value; }
    //}
    //private string _Address2_City;
    //[Xrm("address2_city")]
    //public string Address2_City
    //{
    //    get { return _Address2_City; }
    //    set { _Address2_City = value; }
    //}
    //private string _Address2_County;
    //[Xrm("address2_county")]
    //public string Address2_County
    //{
    //    get { return _Address2_County; }
    //    set { _Address2_County = value; }
    //}
    //private string _Address2_Country;
    //[Xrm("address2_country")]
    //public string Address2_Country
    //{
    //    get { return _Address2_Country; }
    //    set { _Address2_Country = value; }
    //}
    
    private bool _AllowBulkEmail;
    [Xrm("donotbulkemail")]
    public bool AllowBulkEmail
    {
        get { return _AllowBulkEmail; }
        set { _AllowBulkEmail = value; }
    }
    private bool _AllowEmail;
    [Xrm("donotemail")]
    public bool AllowEmail
    {
        get { return _AllowEmail; }
        set { _AllowEmail = value; }
    }
    private bool _AllowPhone;
    [Xrm("donotphone")]
    public bool AllowPhone
    {
        get { return _AllowPhone; }
        set { _AllowPhone = value; }
    }
    private bool _AllowMail;
    [Xrm("donotpostalmail")]
    public bool AllowMail
    {
        get { return _AllowMail; }
        set { _AllowMail = value; }
    }
    private bool _AllowAutoLoad;
    [Xrm("cgi_allow_autoload")]
    public bool AllowAutoLoad
    {
        get { return _AllowAutoLoad; }
        set { _AllowAutoLoad = value; }
    }

    private int _MaxCardsAutoLoad;
    [Xrm("cgi_max_cards_autoload")]
    public int MaxCardsAutoLoad
    {
        get { return _MaxCardsAutoLoad; }
        set { _MaxCardsAutoLoad = value; }
    }

    private OptionSetValue _CustomerType;
    [Xrm("accountcategorycode")]
    public OptionSetValue CustomerType
    {
        get { return _CustomerType; }
        set { _CustomerType = value; }
    }

    private bool _OrganizationCreditApproved;
    [Xrm("cgi_debtcollection")]
    public bool OrganizationCreditApproved
    {
        get { return _OrganizationCreditApproved; }
        set { _OrganizationCreditApproved = value; }
    }

    private string _OrganizationNumber;
    [Xrm("cgi_organizational_number")]
    public string OrganizationNumber
    {
        get { return _OrganizationNumber; }
        set { _OrganizationNumber = value; }
    }

    private string _OrganizationSubNumber;
    [Xrm("cgi_organization_sub_number")]
    public string OrganizationSubNumber
    {
        get { return _OrganizationSubNumber; }
        set { _OrganizationSubNumber = value; }
    }

    private bool _InActive;
    [Xrm("cgi_activated")]
    public bool InActive
    {
        get { return _InActive; }
        set { _InActive = value; }
    }

    private bool _Deleted;    
    public bool Deleted
    {
        get { return _Deleted; }
        set { _Deleted = value; }
    }
}