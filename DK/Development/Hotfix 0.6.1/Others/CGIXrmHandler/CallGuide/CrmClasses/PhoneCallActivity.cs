using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.Generic;
using System;

[XrmEntity("phonecall")]
public class PhoneCallActivity
{
    Guid _PhoneCallId;
    [XrmPrimaryKey]
    [Xrm("activityid")]
    public Guid PhoneCallId
    {
        get { return _PhoneCallId; }
        set { _PhoneCallId = value; }
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
    string _PhoneNumber;
    [Xrm("phonenumber")]
    public string PhoneNumber
    {
        get { return _PhoneNumber; }
        set { _PhoneNumber = value; }
    }

    bool _Direction;
    [Xrm("directioncode")]
    public bool Direction
    {
        get { return _Direction; }
        set { _Direction = value; }
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