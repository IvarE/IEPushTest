using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using CGIXrmExtConnectorService.Shared.CrmClasses;
using CGIXrmWin;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CGIXrmExtConnectorService.Shared
{
    public class XrmHelper
    {
        #region Properties --------------------------------------------------------------------------------------------

        private Settings _xrmSettings;

        /// <summary>
        /// 
        /// </summary>
        public Settings XrmSettings
        {
          get { return _xrmSettings ?? (_xrmSettings = GetXrmSettings()); }
            set
            {
                _xrmSettings = value;
            }
        }

        #endregion

        #region Private Methods ---------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Settings GetXrmSettings()
        {
            var xrmManager = GetXrmManagerFromAppSettings(Guid.Empty);

            var fetchXml = string.Format(@"<fetch mapping='logical' distinct='false'>
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

            var lstSettings = xrmManager.Get<Settings>(fetchXml);

            if (lstSettings != null && lstSettings.Count > 0)
                return lstSettings[0];
            
            throw new Exception("No matching setting was found");
        }

        #endregion

        #region Internal Methods -------------------------------------------------------------------------------------

        private readonly object _lockSql = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal SqlConnection OpenSql()
        {
            lock (_lockSql)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["PubTransStaging"].ConnectionString;

                var sqlConnection = new SqlConnection(connectionString);

                sqlConnection.Open();
                
                return sqlConnection;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        internal void CloseSql(SqlConnection connection)
        {
            lock (_lockSql)
            {
                if (connection != null)
                    connection.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callerId"></param>
        /// <returns></returns>
        internal XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {
                
                var crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var username = ConfigurationManager.AppSettings["Username"];
                var password = ConfigurationManager.AppSettings["Password"];
                
                if (string.IsNullOrEmpty(crmServerUrl) || string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new Exception();
                
                var xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                
                if (callerId != Guid.Empty)
                    ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                
                return xrmManager;
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="searchAttribute"></param>
        /// <param name="entityName"></param>
        /// <param name="xrmManager"></param>
        /// <returns></returns>
        internal Guid GetId(string searchValue,string searchAttribute,string entityName,XrmManager xrmManager)
        {
            var queryByAttribute = new QueryByAttribute
            {
                EntityName = entityName,
                ColumnSet = new ColumnSet()
            };

            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
            queryByAttribute.AddAttributeValue(searchAttribute, searchValue);
            
            var entityCollection = xrmManager.Service.RetrieveMultiple(queryByAttribute);
            
            if (entityCollection != null && entityCollection.Entities.Count > 0)
                return entityCollection[0].Id;
            
            throw new Exception("No Matching User Found");
        }         

        #endregion
    }
}
