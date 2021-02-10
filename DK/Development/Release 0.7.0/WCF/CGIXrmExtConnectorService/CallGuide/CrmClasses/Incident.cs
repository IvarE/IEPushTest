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

    EntityReference _Account;
    [Xrm("cgi_accountid")]
    public EntityReference Account
    {
        get { return _Account; }
        set { _Account = value; }
    }
    

    EntityReference _DefaultCustomer;
    [Xrm("customerid")]
    public EntityReference DefaultCustomer
    {
        get { return _DefaultCustomer; }
        set { _DefaultCustomer = value; }
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
    [Xrm("casetypecode",DecodePart=XrmDecode.Formatted)]
    public string CaseTypeText
    {
        get { return _CaseTypeText; }
        set { _CaseTypeText = value; }
    }

    string _CaseTypeValue;
    [Xrm("casetypecode",DecodePart=XrmDecode.Value)]
    public string CaseTypeValue
    {
        get { return _CaseTypeValue; }
        set { _CaseTypeValue = value; }
    }
    
    
    OptionSetValue _CaseOrigin;
    [Xrm("caseorigincode")]
    public OptionSetValue CaseOrigin
    {
        get { return _CaseOrigin; }
        set { _CaseOrigin = value; }
    }
    
    string _CaseOriginText;
    [Xrm("caseorigincode",DecodePart=XrmDecode.Formatted)]
    public string CaseOriginText
    {
        get { return _CaseOriginText; }
        set { _CaseOriginText = value; }
    }
    
    string _CaseOriginValue;
    [Xrm("caseorigincode", DecodePart = XrmDecode.Value)]
    public string CaseOriginValue
    {
        get { return _CaseOriginValue; }
        set { _CaseOriginValue = value; }
    }
    
    string _TelephoneNumber;
    [Xrm("cgi_telephonenumber")]
    public string TelephoneNumber
    {
        get { return _TelephoneNumber; }
        set { _TelephoneNumber = value; }
    }

    EntityReference _SourceFBActivity;
    [Xrm("cgi_facebookpostid")]
    public EntityReference SourceFBActivity
    {
        get { return _SourceFBActivity; }
        set { _SourceFBActivity = value; }
    }

    EntityReference _SourceChatActivity;
    [Xrm("cgi_chatid")]
    public EntityReference SourceChatActivity
    {
        get { return _SourceChatActivity; }
        set { _SourceChatActivity = value; }
    }
    EntityReference _CallGuideInfo;
    [Xrm("cgi_callguideinfo")]
    public EntityReference CallGuideInfo
    {
        get { return _CallGuideInfo; }
        set { _CallGuideInfo = value; }
    }
    
}