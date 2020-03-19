using System;
using Generic=System.Collections.Generic;
using System.Linq;
using System.Web;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections.ObjectModel;
using System.Threading;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace CGICRMPortalService
{
    public class XrmHelper
    {
              

        //public XrmHelper()
        //{
        //    xrmMgr = GetXrmManagerFromAppSettings(Guid.Empty);
        //}

        //public XrmHelper(Guid callerId)
        //{
        //    xrmMgr = GetXrmManagerFromAppSettings(callerId);
        //}

        internal XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {
                
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"].ToString();
                string domain = ConfigurationManager.AppSettings["Domain"].ToString();
                string username = ConfigurationManager.AppSettings["Username"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    if (callerId != Guid.Empty)
                        ((OrganizationServiceProxy)xrmMgr.Service.InnerService).CallerId = callerId;
                    return xrmMgr;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        //internal Entity EncodeToEntity<TEntity>(TEntity objEntity, Entity entity)
        //{
        //    XrmReadOnlyAttribute _readOnlyAttrib;
        //    XrmAttribute _xrmAttrib;
        //    XrmPrimaryKeyAttribute _primaryKey;

        //    if (entity == null)
        //        entity = new Entity();

        //    Type _typeOfObject = objEntity.GetType();
        //    if (string.IsNullOrEmpty(entity.LogicalName))
        //    {
        //        entity.LogicalName = _typeOfObject.Name;
        //        XrmEntityAttribute _entAtt = _typeOfObject.GetCustomAttributes(typeof(XrmEntityAttribute), true).FirstOrDefault() as XrmEntityAttribute;
        //        if (_entAtt != null)
        //            entity.LogicalName = _entAtt.LogicalName;
        //    }

        //    List<PropertyInfo> _props = objEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
        //    foreach (PropertyInfo prop in _props)
        //    {
        //        _readOnlyAttrib = prop.GetCustomAttributes(typeof(XrmReadOnlyAttribute), true).FirstOrDefault() as XrmReadOnlyAttribute;
        //        if (_readOnlyAttrib != null)
        //            continue;

        //        _xrmAttrib = prop.GetCustomAttributes(typeof(XrmAttribute), true).FirstOrDefault() as XrmAttribute;
        //        if (_xrmAttrib == null)
        //            continue;

        //        if (_xrmAttrib != null && _xrmAttrib.DecodePart != XrmDecode.Default)
        //            continue;

        //        if (string.IsNullOrEmpty(_xrmAttrib.Path) || _xrmAttrib.Path.Contains("."))
        //            continue;

        //        _primaryKey = prop.GetCustomAttributes(typeof(XrmPrimaryKeyAttribute), true).FirstOrDefault() as XrmPrimaryKeyAttribute;
        //        if (_primaryKey != null)
        //        {
        //            Guid _id;
        //            if (Guid.TryParse(prop.GetValue(objEntity, null).ToString(), out _id))
        //                entity.Id = _id;
        //            continue;
        //        }

        //        if (prop.GetValue(objEntity, null) == null)
        //            continue;

        //        if (!entity.Attributes.ContainsKey(_xrmAttrib.Path))
        //        {
        //            if (prop.GetValue(objEntity, null) == (object)XRMConstants.NullValue)
        //            {

        //            }
        //            entity.Attributes.Add(_xrmAttrib.Path, prop.GetValue(objEntity, null));
        //        }
        //        else
        //        {
        //            entity.Attributes[_xrmAttrib.Path] = prop.GetValue(objEntity, null);
        //        }
        //    }
        //    return entity;
        //}


        internal Guid GetIdByValue(string searchValue,string searchAttribute,string entityName,XrmManager xrmMgr,bool includeStateCode=true)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName =entityName;
            queryByAttribute.ColumnSet = new ColumnSet();
            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            if (includeStateCode)
            {
                queryByAttribute.AddAttributeValue("statecode", 0);
            }
            EntityCollection entities = xrmMgr.Service.RetrieveMultiple(queryByAttribute);
            if (entities != null & entities.Entities.Count > 0)
            {
                return entities[0].Id;
            }
            else
            {
                return Guid.Empty;
            }
        }
        
        
        
    }
}
