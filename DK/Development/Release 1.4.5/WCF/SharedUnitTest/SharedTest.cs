using System;
using System.Configuration;
using System.Linq;
using CGIXrmWin;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace SharedUnitTests
{
    public static class SharedTest
    {
        private static readonly XrmManager XrmMgr = GetXrmManagerFromAppSettings(Guid.Empty);

        /// <summary>
        /// Validate the security code given against the user in CRM
        /// </summary>
        /// <param name="username">CRM User Name</param>
        /// <returns>CRM User Guid</returns>
        public static Guid ValidateLogin(string username)
        {
            //Get user details
            string fetchXmlQuery = string.Format(@"<fetch mapping='logical'>
              <entity name='systemuser'>
                <attribute name='fullname' />
                <attribute name='businessunitid' />
                <attribute name='title' />
                <attribute name='address1_telephone1' />
                <attribute name='systemuserid' />
                <attribute name='domainname' />
                <attribute name='cgi_attestid' />
                <order attribute='fullname' descending='false' />
                <filter type='and'>
                  <condition attribute='domainname' operator='eq' value='{0}' />
                </filter>
              </entity>
            </fetch>", username);

            var users = XrmMgr.Service.RetrieveMultiple(new FetchExpression(fetchXmlQuery));


            if (users != null && users.Entities != null && users.Entities.Count > 0)
            {
                var userEntity = users.Entities[0];

                return userEntity.Id;
            }

            return Guid.Empty;
        }


        /// <summary>
        /// Creates an instance towards CRM based on given parameters.
        /// </summary>
        /// <param name="callerId">CRM User Id</param>
        /// <returns></returns>
        public static XrmManager GetXrmManagerFromAppSettings(Guid callerId)
        {
            try
            {
                var crmServerUrl = ConfigurationManager.AppSettings["CrmServerUrl"];
                var domain = ConfigurationManager.AppSettings["Domain"];
                var username = ConfigurationManager.AppSettings["Username"];
                var password = ConfigurationManager.AppSettings["Password"];
                if (String.IsNullOrEmpty(crmServerUrl) || String.IsNullOrEmpty(domain) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
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

        public static Guid? GetFirstEntityId(string entityName, string entityIdName)
        {
            var query = new QueryExpression(entityName);
            query.ColumnSet.AddColumns(entityIdName);
            query.TopCount = 1;
            var results = XrmMgr.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities[0].Id;
            }
            return null;
        }

        public static DataCollection<Entity> GetMultipleEntityId(string entityName, string[] columnNames, int count = 0)
        {
            var query = new QueryExpression(entityName);
            foreach (string column in columnNames)
            {
                query.ColumnSet.AddColumns(column);
            }
            if (count > 0)
            {
                query.TopCount = count;
            }
            var results = XrmMgr.Service.RetrieveMultiple(query);
            if (results != null && results.Entities.Any())
            {
                return results.Entities;
            }
            return null;
        }
    }
}
