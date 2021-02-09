using System.Runtime.Serialization;
using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

 [XrmEntity("contact")]
public class Contact:XrmBaseEntity
{
    private Guid _ContactId;
    [XrmPrimaryKey]
    [Xrm("contactid")]
    public Guid ContactId
    {
        get { return _ContactId; }
        set { _ContactId = value; }
    }
    private EntityReference _ParentCustomer;
    [Xrm("parentcustomerid")]
    public EntityReference ParentCustomer
    {
        get { return _ParentCustomer; }
        set { _ParentCustomer = value; }
    }
    private string _FirstName = string.Empty;
    [Xrm("firstname")]
    public string FirstName
    {
        get { return _FirstName; }
        set { _FirstName = value; }
    }

    private string _LastName = string.Empty;
    [Xrm("lastname")]
    public string LastName
    {
        get { return _LastName; }
        set { _LastName = value; }
    }

    private string _Street1;
    [Xrm("address1_line1")]
    public string Street1
    {
        get { return _Street1; }
        set { _Street1 = value; }
    }
    private string _PostalCode;
    [Xrm("address1_postalcode")]
    public string PostalCode
    {
        get { return _PostalCode; }
        set { _PostalCode = value; }
    }
    private string _City;
    [Xrm("address1_city")]
    public string City
    {
        get { return _City; }
        set { _City = value; }
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
    private string _Address1_CareOff;
    [Xrm("address1_name")]
    public string Address1_CareOff
    {
        get { return _Address1_CareOff; }
        set { _Address1_CareOff = value; }
    }

    private string _ContactPerson;
    [Xrm("address1_primarycontactname")]
    public string ContactPerson
    {
        get { return _ContactPerson; }
        set { _ContactPerson = value; }
    }
    
    private string _ContactPhoneNumber;
    [Xrm("address1_telephone1")]
    public string ContactPhoneNumber
    {
        get { return _ContactPhoneNumber; }
        set { _ContactPhoneNumber = value; }
    }

    private string _SMSNotificationNumber;
     [Xrm("address1_telephone2")]
    public string SMSNotificationNumber
    {
        get { return _SMSNotificationNumber; }
        set { _SMSNotificationNumber = value; }
    }

    private string _Email;
    [Xrm("emailaddress1")]
    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }
    private bool _IsPrimaryAddress;
    [Xrm("cgi_isprimaryaddress")]
    public bool IsPrimaryAddress
    {
        get { return _IsPrimaryAddress; }
        set { _IsPrimaryAddress = value; }
    }
    private OptionSetValue _AddressType;
    [Xrm("address1_addresstypecode")] 
    public OptionSetValue AddressType
    {
        get { return _AddressType; }
        set { _AddressType = value; }
    }

    //private string _Fax;
    //[Xrm("fax")]
    //public string Fax
    //{
    //    get { return _Fax; }
    //    set { _Fax = value; }
    //}
    
   

    

        

}
