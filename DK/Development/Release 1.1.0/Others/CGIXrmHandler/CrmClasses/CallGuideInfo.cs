using System;
using CGIXrmWin;

[XrmEntity("cgi_callguideinfo")]
public class CallGuideInfo
{
    Guid _CallGuideInfoId;
    [XrmPrimaryKey]
    [Xrm("cgi_callguideinfoid")]
    public Guid CallGuideInfoId
    {
        get { return _CallGuideInfoId; }
        set { _CallGuideInfoId = value; }
    }

    string _CallGuideInfoName;
    [Xrm("cgi_callguideinfoname")]
    public string CallGuideInfoName
    {
        get { return _CallGuideInfoName; }
        set { _CallGuideInfoName = value; }
    }

    string _CallGuideSessionID;
    [Xrm("cgi_callguidesessionid")]
    public string CallGuideSessionID
    {
        get { return _CallGuideSessionID; }
        set { _CallGuideSessionID = value; }
    }

    string _QueueTime;
    [Xrm("cgi_queuetime")]
    public string QueueTime
    {
        get { return _QueueTime; }
        set { _QueueTime = value; }
    }
        
    string _APhoneNumber;
    [Xrm("cgi_aphonenumber")]
    public string APhoneNumber
    {
        get { return _APhoneNumber; }
        set { _APhoneNumber = value; }
    }


    string _BPhoneNumber;
    [Xrm("cgi_bphonenumber")]
    public string BPhoneNumber
    {
        get { return _BPhoneNumber; }
        set { _BPhoneNumber = value; }
    }


    string _ErrandTaskType;
    [Xrm("cgi_errandtasktype")]
    public string ErrandTaskType
    {
        get { return _ErrandTaskType; }
        set { _ErrandTaskType = value; }
    }

    string _ScreenPopChoice;
    [Xrm("cgi_screenpopchoice")]
    public string ScreenPopChoice
    {
        get { return _ScreenPopChoice; }
        set { _ScreenPopChoice = value; }
    }

    string _ContactSourceType;
    [Xrm("cgi_contactsourcetype")]
    public string ContactSourceType
    {
        get { return _ContactSourceType; }
        set { _ContactSourceType = value; }
    }

    string _CId;
    [Xrm("cgi_cid")]
    public string CId
    {
        get { return _CId; }
        set { _CId = value; }
    }

    string _AgentName;
    [Xrm("cgi_agentname")]
    public string AgentName
    {
        get { return _AgentName; }
        set { _AgentName = value; }
    }

    string _ChatCustomerAlias;
    [Xrm("cgi_chatcustomeralias")]
    public string ChatCustomerAlias
    {
        get { return _ChatCustomerAlias; }
        set { _ChatCustomerAlias = value; }
    }
}