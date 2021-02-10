using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.InteropServices;

[XrmEntity("cgi_categorydetail")]
[ComVisibleAttribute(false)]
public class CategoryDetail:XrmBaseEntity
{
    Guid _CategoryDetailId;
    [XrmPrimaryKey]
    [Xrm("cgi_categorydetailid")]
    public Guid CategoryDetailId
    {
        get { return _CategoryDetailId; }
        set { _CategoryDetailId = value; }
    }
    string _CategoryDetailName;
    [Xrm("cgi_categorydetailname")]
    public string CategoryDetailName
    {
        get { return _CategoryDetailName; }
        set { _CategoryDetailName = value; }
    }    
    EntityReference _ParentLevel1;
    [Xrm("cgi_parentid")]
    public EntityReference ParentLevel1
    {
        get { return _ParentLevel1; }
        set { _ParentLevel1 = value; }
    }
    EntityReference _ParentLevel2;
    [Xrm("cgi_parentid2")]
    public EntityReference ParentLevel2
    {
        get { return _ParentLevel2; }
        set { _ParentLevel2 = value; }
    }
    string _Level;
    [Xrm("cgi_level")]
    public string Level
    {
        get { return _Level; }
        set { _Level = value; }
    }
    string _CallGuideCategory;
    [Xrm("cgi_callguidecategory")]
    public string CallGuideCategory
    {
        get { return _CallGuideCategory; }
        set { _CallGuideCategory = value; }
    }
}