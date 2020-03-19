using System;
using System.Configuration;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CGICRMPortalService.Shared
{
    public class XrmHelper
    {
        #region Internal Methods
        /// <summary>
        /// Method returning an instance of XRM Manager based on settings in configuration.
        /// </summary>
        /// <param name="callerId">Unique Identifier of CRM User</param>
        /// <returns></returns>
        internal XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {                
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                else
                {
                    XrmManager xrmMgr = new XrmManager(crmServerUrl, domain, username, password);
                    if (callerId != Guid.Empty)
                        ((OrganizationServiceProxy)xrmMgr.Service).CallerId = callerId;
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
            QueryByAttribute queryByAttribute = new QueryByAttribute
            {
                EntityName = entityName,
                ColumnSet = new ColumnSet()
            };
            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            if (includeStateCode)
            {
                queryByAttribute.AddAttributeValue("statecode", 0); // 0 active
            }
            EntityCollection entities = xrmMgr.Service.RetrieveMultiple(queryByAttribute);
            if (entities != null && entities.Entities != null && entities.Entities.Count > 0)
            {
                return entities[0].Id;
            }
            return Guid.Empty;
        }
        #endregion
    }
}
