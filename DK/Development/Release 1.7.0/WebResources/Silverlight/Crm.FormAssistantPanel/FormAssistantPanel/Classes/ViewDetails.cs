using System.Collections.Generic;
using System.Reflection;
using Crm.FormAssistantPanel.Utilities;
using System;

public class View : ICustomTypeProvider
{

    bool _isSelected = false;
    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    string _panelConfigId = string.Empty;
    public string PanelConfigId
    {
        get { return _panelConfigId; }
        set { _panelConfigId = value; }
    }

    string _viewEntityName = string.Empty;
    public string ViewEntityName
    {
        get { return _viewEntityName; }
        set { _viewEntityName = value; }
    }


    Dictionary<string, string> _unFormattedValues = new Dictionary<string, string>();
    public Dictionary<string, string> UnFormattedValues
    {
        get { return _unFormattedValues; }
        set { _unFormattedValues = value; }
    }
    

    Dictionary<string, string> _attributeDetails = new Dictionary<string, string>();
    public Dictionary<string, string> AttributeDetails
    {
        get { return _attributeDetails; }
        set { _attributeDetails = value; }
    }

    Dictionary<string, LookUpDetails> _lookUpDetails = new Dictionary<string, LookUpDetails>();
    public Dictionary<string, LookUpDetails> LookUpDetails
    {
        get { return _lookUpDetails; }
        set { _lookUpDetails = value; }
    }


    private CustomTypeHelper<View> helper =
        new CustomTypeHelper<View>();

    // Redirect all method calls to the helper like shown below
    public Type GetCustomType()
    {
        return helper.GetCustomType();
    }

    public static void AddProperty(string name)
    {
        if(!View.HasProperty(name))
            CustomTypeHelper<View>.AddProperty(name);
    }
    public static void AddProperty(string name, Type propertyType)
    {
        if (!View.HasProperty(name))
            CustomTypeHelper<View>.AddProperty(name,propertyType);
    }
    public static bool HasProperty(string name)
    {
        return CustomTypeHelper<View>.HasProperty(name);
    }
    public void SetPropertyValue(string propertyName, object value)
    {
        helper.SetPropertyValue(propertyName, value);
    }
    public object GetPropertyValue(string propertyName)
    {
        return helper.GetPropertyValue(propertyName);
    }

    public PropertyInfo[] GetProperties()
    {
        return helper.GetProperties();
    }
}


public class LookUpDetails
{
    internal Guid Id
    {
        get;
        set;
    }
    internal string EntityType
    {
        get;
        set;
    }
    internal string Name
    {
        get;
        set;
    }
 

    
}

