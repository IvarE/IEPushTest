using System;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Data.SqlClient;


namespace CGIXrmExtConnectorService
{
    public class XrmHelper
    {
        #region [Properties]
        Settings _XrmSettings;
        public Settings XrmSettings
        {
          get {
              if (_XrmSettings == null)
                  _XrmSettings = GetXrmSettings();
            
              return _XrmSettings;
            }
          set { _XrmSettings = value; }
        }
        #endregion

        #region [Private Methods]

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

             //EntityCollection entityCollection= xrmManager.Service.RetrieveMultiple(new FetchExpression(fetchXml));
            ObservableCollection<Settings> lstSettings = xrmManager.Get<Settings>(fetchXml);
            if (lstSettings != null && lstSettings.Count > 0)
                return lstSettings[0];
            else
                throw new Exception("No matching setting was found");
        }

        #endregion

        #region [internal Methods]
        private object LockSql = new object();

        public SqlConnection OpenSQL()
        {
            lock (LockSql)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PubTransStaging"].ConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                return sqlConnection;

            }
        }

        public void CloseSQL(SqlConnection connection)
        {
            lock (LockSql)
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public XrmManager GetXrmManagerFromAppSettings(Guid callerId)
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
                    XrmManager xrmManager = new XrmManager(crmServerUrl, domain, username, password);
                    if (callerId != Guid.Empty)
                        ((OrganizationServiceProxy)xrmManager.Service.InnerService).CallerId = callerId;
                    return xrmManager;
                }
            }
            catch
            {
                throw new Exception("Error while initiating XrmManager. Please check the web settings");
            }
        }

        

        public Guid GetId(string searchValue,string searchAttribute,string entityName,XrmManager xrmManager)
        {
            QueryByAttribute queryByAttribute = new QueryByAttribute();
            queryByAttribute.EntityName =entityName;
            queryByAttribute.ColumnSet = new ColumnSet();
            queryByAttribute.ColumnSet.AddColumn(entityName+"id");
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
