using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("cgi_applicationlog")]
internal class ApplicationLog
{
    string _ApplicationName;
    [Xrm("cgi_applicationname")]
    public string ApplicationName
    {
        get { return _ApplicationName; }
        set { _ApplicationName = value; }
    }

    string _MethodName;
    [Xrm("cgi_method")]
    public string MethodName
    {
        get { return _MethodName; }
        set { _MethodName = value; }
    }

    OptionSetValue _LogType;
    [Xrm("cgi_logtype")]
    public OptionSetValue LogType
    {
        get { return _LogType; }
        set { _LogType = value; }
    }

    int _LogTypeValue;
    public int LogTypeValue
    {
        get { return _LogTypeValue; }
        set { _LogTypeValue = value; }
    }

    string _Message;
    [Xrm("cgi_logmessage")]
    public string Message
    {
        get { return _Message; }
        set { _Message = value; }
    }

    string _SystemExceptionDetail;
    [Xrm("cgi_systemexception")]
    public string SystemExceptionDetail
    {
        get { return _SystemExceptionDetail; }
        set { _SystemExceptionDetail = value; }
    }

}