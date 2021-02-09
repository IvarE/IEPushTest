using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using CGIXrmEAIConnectorService.Shared.CrmClasses;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmEAIConnectorService.Shared
{
    public class XrmHelper
    {
        #region Global Variables
        private readonly object _lockSql = new object();
        #endregion

        #region Public Properties
        Settings _xrmSettings;
        public Settings XrmSettings
        {
            get { return _xrmSettings ?? (_xrmSettings = GetXrmSettings()); }
            set { _xrmSettings = value; }
        }
        #endregion

        #region Private Methods
        Settings GetXrmSettings()
        {
            XrmManager xrmManager = GetXrmManagerFromAppSettings(Guid.Empty);


            string fetchXml = string.Format(@"<fetch mapping='logical' distinct='false'>
                                            <entity name='cgi_setting'>
                                            <all-attributes/>
                                            <order attribute='cgi_name' descending='false'/>
                                            <filter type='and'>
                                            <filter type='or'>
                                            <condition attribute='cgi_validto' operator='on-or-before' value='{0}'/>
                                            <condition attribute='cgi_validto' operator='null'/>
                                            </filter>
                                            <condition attribute='statecode' operator='eq' value='0'/>
                                            </filter>
                                            </entity>
                                            </fetch>", DateTime.Today.ToString(CultureInfo.GetCultureInfo("sv-SE").DateTimeFormat.ShortDatePattern));

            ObservableCollection<Settings> lstSettings = xrmManager.Get<Settings>(fetchXml);
            if (lstSettings != null && lstSettings.Count > 0)
                return lstSettings[0];
            throw new Exception("No matching setting was found");
        }

        #endregion

        #region Public Methods
        public XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {
                string crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                string domain = ConfigurationManager.AppSettings["Domain"];
                string username = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                    throw new Exception();
                
                XrmManager xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                if (callerId != Guid.Empty)
                    ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                return xrmManager;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings", ex);
            }
        }

        public Guid GetId(string searchValue, string searchAttribute, string entityName, XrmManager xrmManager)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute
            {
                EntityName = entityName,
                ColumnSet = new ColumnSet()
            };
            queryByAttribute.ColumnSet.AddColumn(entityName + "id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            EntityCollection entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                return entityCollection[0].Id;
            else
                throw new Exception("No Matching User Found");
        }
        #endregion
    }
}
