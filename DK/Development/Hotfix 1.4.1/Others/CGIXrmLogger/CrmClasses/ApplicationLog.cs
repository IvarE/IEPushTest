using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("cgi_applicationlog")]
internal class ApplicationLog
{
    #region Public Properties
    public int LogTypeValue { get; set; }

    [Xrm("cgi_applicationname")]
    public string ApplicationName { get; set; }

    [Xrm("cgi_method")]
    public string MethodName { get; set; }

    [Xrm("cgi_logtype")]
    public OptionSetValue LogType { get; set; }

    [Xrm("cgi_logmessage")]
    public string Message { get; set; }

    [Xrm("cgi_systemexception")]
    public string SystemExceptionDetail { get; set; }
    #endregion
}