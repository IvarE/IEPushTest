using System.Runtime.Serialization;
using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("customeraddress")]
public class CustomerAddress
{
    #region Public Properties
    private Guid _CustomerAddressId;
    [XrmPrimaryKey]
    [Xrm("customeraddressid")]
    public Guid CustomerAddressId
    {
        get { return _CustomerAddressId; }
        set { _CustomerAddressId = value; }
    }
    
    private EntityReference _ParentId;
    [Xrm("parentid")]
    public EntityReference ParentId
    {
        get { return _ParentId; }
        set { _ParentId = value; }
    }

    public Guid AddressId
    {
        get { return _CustomerAddressId; }
    }
    
    private string _CompanyName;
    [Xrm("name")]
    public string CompanyName
    {
        get { return _CompanyName; }
        set { _CompanyName = value; }
    }   

    private string _Street;
    [Xrm("line2")]
    public string Street
    {
        get { return _Street; }
        set { _Street = value; }
    }
    private string _PostalCode;
    [Xrm("postalcode")]
    public string PostalCode
    {
        get { return _PostalCode; }
        set { _PostalCode = value; }
    }
    private string _City;
    [Xrm("city")]
    public string City
    {
        get { return _City; }
        set { _City = value; }
    }
    private string _County;
    [Xrm("county")]
    public string County
    {
        get { return _County; }
        set { _County = value; }
    }
    private string _Country;
    [Xrm("country")]
    public string Country
    {
        get { return _Country; }
        set { _Country = value; }
    }
    private string _CareOff;
    [Xrm("line1")]
    public string CareOff
    {
        get { return _CareOff; }
        set { _CareOff = value; }
    }

    private string _ContactPerson;
    [Xrm("primarycontactname")]
    public string ContactPerson
    {
        get { return _ContactPerson; }
        set { _ContactPerson = value; }
    }

    private string _ContactPhoneNumber;
    [Xrm("telephone1")]
    public string ContactPhoneNumber
    {
        get { return _ContactPhoneNumber; }
        set { _ContactPhoneNumber = value; }
    }

    private string _SMSNotificationNumber;
    [Xrm("telephone2")]
    public string SMSNotificationNumber
    {
        get { return _SMSNotificationNumber; }
        set { _SMSNotificationNumber = value; }
    }

    private OptionSetValue _AddressType;
    [Xrm("addresstypecode")]
    public OptionSetValue AddressType
    {
        get { return _AddressType; }
        set { _AddressType = value; }
    }

    [Xrm("cgi_rsid")]
    public string RsID { get; set; }

    [Xrm("cgi_organization_sub_number")]
    public string OrganizationNumber { get; set; }
    #endregion
}