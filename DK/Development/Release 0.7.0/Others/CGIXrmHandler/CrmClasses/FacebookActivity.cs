
using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("cgi_callguidefacebook")]
public class FacebookActivity
{
    Guid _ChatId;
    [XrmPrimaryKey]
    [Xrm("activityid")]
    public Guid ChatId
    {
        get { return _ChatId; }
        set { _ChatId = value; }
    }

    string _Subject;
    [Xrm("subject")]
    public string Subject
    {
        get { return _Subject; }
        set { _Subject = value; }
    }

    EntityCollection _CallFrom;
    [Xrm("from")]
    public EntityCollection CallFrom
    {
        get { return _CallFrom; }
        set { _CallFrom = value; }
    }
    EntityCollection _CallTo;
    [Xrm("to")]
    public EntityCollection CallTo
    {
        get { return _CallTo; }
        set { _CallTo = value; }
    }
    

    
    EntityReference _Regarding;
    [Xrm("regardingobjectid")]
    public EntityReference Regarding
    {
        get { return _Regarding; }
        set { _Regarding = value; }
    }
    string _Duration;
    [Xrm("actualdurationminutes")]
    public string Duration
    {
        get { return _Duration; }
        set { _Duration = value; }
    }
    string _Desciption;
    [Xrm("description")]
    public string Desciption
    {
        get { return _Desciption; }
        set { _Desciption = value; }
    }

    EntityReference _CallGuideInfo;
    [Xrm("cgi_callguideinfoid")]
    public EntityReference CallGuideInfo
    {
        get { return _CallGuideInfo; }
        set { _CallGuideInfo = value; }
    }
}