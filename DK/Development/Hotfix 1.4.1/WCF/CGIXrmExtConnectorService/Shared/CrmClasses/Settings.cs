using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;

[XrmEntity("cgi_setting")]
public class Settings
{
    #region Public Properties
    string _ValidFrom;
    [Xrm("cgi_validfrom",DecodePart=XrmDecode.Formatted)]
    public string ValidFrom
    {
        get { return _ValidFrom; }
        set { _ValidFrom = value; }
    }
    string _ValidTo;
    [Xrm("cgi_validto", DecodePart = XrmDecode.Formatted)]
    public string ValidTo
    {
        get { return _ValidTo; }
        set { _ValidTo = value; }
    }
    EntityReference _DefaultCustomerOnCase;    
    [Xrm("cgi_defaultcustomeroncase")]
    public EntityReference DefaultCustomerOnCase
    {
        get { return _DefaultCustomerOnCase; }
        set { _DefaultCustomerOnCase = value; }
    }
    #endregion
}
