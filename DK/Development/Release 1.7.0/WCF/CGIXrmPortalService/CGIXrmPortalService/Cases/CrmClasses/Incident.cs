using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;

[XrmEntity("incident")]
public class Incident:XrmBaseEntity
{
    Guid _IncidentId;
    [XrmPrimaryKey]
    [Xrm("incidentid")]    
    public Guid IncidentId  
    {
        get { return _IncidentId; }
        set { _IncidentId = value; }
    }
    string _CaseNumber;
    [Xrm("ticketnumber")]
    public string CaseNumber
    {
        get { return _CaseNumber; }
        set { _CaseNumber = value; }
    }
    string _Title;
    [Xrm("title")]
    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }

    OptionSetValue _CaseType;
    [Xrm("casetypecode")]
    public OptionSetValue CaseType
    {
        get { return _CaseType; }
        set { _CaseType = value; }
    }

    string _CaseTypeText;
    [Xrm("casetypecode", DecodePart = XrmDecode.Formatted)]
    public string CaseTypeText
    {
        get { return _CaseTypeText; }
        set { _CaseTypeText = value; }
    }

    int _CaseTypeValue;
    [Xrm("casetypecode", DecodePart = XrmDecode.Value)]
    public int CaseTypeValue
    {
        get { return _CaseTypeValue; }
        set { _CaseTypeValue = value; }
    }
    
    string _AccountNumber;
    [Xrm("cgi_accountnumber")]
    public string AccountNumber
    {
        get { return _AccountNumber; }
        set { _AccountNumber = value; }
    }


    EntityReference _Account;
    [Xrm("cgi_accountid")]
    public EntityReference Account
    {
        get { return _Account; }
        set { _Account = value; }
    }


    string _Email;
    [Xrm("cgi_emailaddress")]
    public string Email
    {
        get { return _Email; }
        set { _Email = value; }
    }

   
    

    
    string _TelephoneNumber;
    [Xrm("cgi_telephonenumber")]
    public string TelephoneNumber
    {
        get { return _TelephoneNumber; }
        set { _TelephoneNumber = value; }
    }


    EntityReference _TravelCard;
    [Xrm("cgi_travelcardid")]
    public EntityReference TravelCard
    {
        get { return _TravelCard; }
        set { _TravelCard = value; }
    }
    string _TravelCardValue;
    [Xrm("cgi_travelcardid", DecodePart = XrmDecode.Value)]
    public string TravelCardValue
    {
        get { return _TravelCardValue; }
        set { _TravelCardValue = value; }
    }
    string _TravelCardNumber;
    [Xrm("cgi_travelcardid",DecodePart=XrmDecode.Name)]
    public string TravelCardNumber
    {
        get { return _TravelCardNumber; }
        set { _TravelCardNumber = value; }
    }
    string _Description;
    [Xrm("description")]
    public string Description
    {
        get { return _Description; }
        set { _Description = value; }
    }
    OptionSetValue _CaseStatus;
    [Xrm("statuscode")]
    public OptionSetValue CaseStatus
    {
        get { return _CaseStatus; }
        set { _CaseStatus = value; }
    }

    string _CaseStatusText;
    [Xrm("statuscode", DecodePart = XrmDecode.Formatted)]
    public string CaseStatusText
    {
        get { return _CaseStatusText; }
        set { _CaseStatusText = value; }
    }

    int _CaseStatusValue;
    [Xrm("statuscode", DecodePart = XrmDecode.Value)]
    public int CaseStatusValue
    {
        get { return _CaseStatusValue; }
        set { _CaseStatusValue = value; }
    }
    OptionSetValue _CaseOrigin;
    [Xrm("caseorigincode")]
    public OptionSetValue CaseOrigin
    {
        get { return _CaseOrigin; }
        set { _CaseOrigin = value; }
    }

    string _CaseOriginText;
    [Xrm("caseorigincode", DecodePart = XrmDecode.Formatted)]
    public string CaseOriginText
    {
        get { return _CaseOriginText; }
        set { _CaseOriginText = value; }
    }

    int _CaseOriginValue;
    [Xrm("caseorigincode", DecodePart = XrmDecode.Value)]
    public int CaseOriginValue
    {
        get { return _CaseOriginValue; }
        set { _CaseOriginValue = value; }
    }
    


    
}