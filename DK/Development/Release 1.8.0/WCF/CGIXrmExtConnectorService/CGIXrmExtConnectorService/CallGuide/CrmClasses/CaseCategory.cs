using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;

[XrmEntity("cgi_casecategory")]
public class CaseCatergory:XrmBaseEntity
{
    Guid _CaseCategoryid;
    [XrmPrimaryKey]
    [Xrm("cgi_casecategoryid")]
    public Guid CaseCategoryid
    {
        get { return _CaseCategoryid; }
        set { _CaseCategoryid = value; }
    }
    string _CaseCategoryName;
    [Xrm("cgi_casecategoryname")]
    public string CaseCategoryName
    {
        get { return _CaseCategoryName; }
        set { _CaseCategoryName = value; }
    }    
    EntityReference _CaseId;
    [Xrm("cgi_caseid")]
    public EntityReference CaseId
    {
        get { return _CaseId; }
        set { _CaseId = value; }
    }

    string _CaseIdName;
    [Xrm("cgi_caseid",DecodePart=XrmDecode.Name)]
    public string CaseIdName
    {
        get { return _CaseIdName; }
        set { _CaseIdName = value; }
    }
    string _CaseIdValue;
    [Xrm("cgi_caseid", DecodePart = XrmDecode.Value)]
    public string CaseIdValue
    {
        get { return _CaseIdValue; }
        set { _CaseIdValue = value; }
    }
    EntityReference _CaseCategory1;
    [Xrm("cgi_category1id")]
    public EntityReference CaseCategory1
    {
        get { return _CaseCategory1; }
        set { _CaseCategory1 = value; }
    }

    string _CaseCategory1Name;
    [Xrm("cgi_category1id", DecodePart = XrmDecode.Name)]
    public string CaseCategory1Name
    {
        get { return _CaseCategory1Name; }
        set { _CaseCategory1Name = value; }
    }
    string _CaseCategory1Value;
    [Xrm("cgi_category1id", DecodePart = XrmDecode.Value)]
    public string CaseCategory1Value
    {
        get { return _CaseCategory1Value; }
        set { _CaseCategory1Value = value; }
    }

    EntityReference _CaseCategory2;
    [Xrm("cgi_category2id")]
    public EntityReference CaseCategory2
    {
        get { return _CaseCategory2; }
        set { _CaseCategory2 = value; }
    }

    string _CaseCategory2Name;
    [Xrm("cgi_category2id", DecodePart = XrmDecode.Name)]
    public string CaseCategory2Name
    {
        get { return _CaseCategory2Name; }
        set { _CaseCategory2Name = value; }
    }
    string _CaseCategory2Value;
    [Xrm("cgi_category2id", DecodePart = XrmDecode.Value)]
    public string CaseCategory2Value
    {
        get { return _CaseCategory2Value; }
        set { _CaseCategory2Value = value; }
    }
    EntityReference _CaseCategory3;
    [Xrm("cgi_category3id")]
    public EntityReference CaseCategory3
    {
        get { return _CaseCategory3; }
        set { _CaseCategory3 = value; }
    }
    string _CaseCategory3Name;
    [Xrm("cgi_category3id", DecodePart = XrmDecode.Name)]
    public string CaseCategory3Name
    {
        get { return _CaseCategory3Name; }
        set { _CaseCategory3Name = value; }
    }
    string _CaseCategory3Value;
    [Xrm("cgi_category3id", DecodePart = XrmDecode.Value)]
    public string CaseCategory3Value
    {
        get { return _CaseCategory3Value; }
        set { _CaseCategory3Value = value; }
    }


}