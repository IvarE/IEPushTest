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
        /// <summary>
        /// Method returning an instance of XRM Manager based on settings in configuration.
        /// </summary>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
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


        /// <summary>
        /// Method returning an entity identifier based on given parameters.
        /// </summary>
        /// <param name="searchValue">Search Criteria Value</param>
        /// <param name="searchAttribute">Name of field in CRM-entity</param>
        /// <param name="entityName">CRM Entity Name</param>
        /// <param name="xrmMgr">Instance of XRM Manager</param>
        /// <param name="includeStateCode">True: Active Only, False: Inactive Included. Default: Active Only</param>
        /// <returns>Unique Identifier of specific record in CRM</returns>
        internal Guid GetIdByValue(string searchValue, string searchAttribute, string entityName, XrmManager xrmMgr, bool includeStateCode = true)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName =entityName;
            queryByAttribute.ColumnSet = new ColumnSet();
            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            if (includeStateCode)
            {
                queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
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
