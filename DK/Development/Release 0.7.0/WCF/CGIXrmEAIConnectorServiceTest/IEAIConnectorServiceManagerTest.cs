using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ServiceModel;
using CGIXrmEAIConnectorService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGIXrmWin;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using System.Configuration;
using Microsoft.Xrm.Sdk.Query;
using System.Runtime.Serialization;

namespace CGIXrmEAIConnectorServiceTest
{
    [TestClass]
    public class IEAIConnectorServiceManagerTest
    {
        AllBinaryManager mgr;
        XrmManager xrmMgr;
        Guid callerId;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IEAIConnectorServiceManagerTest()
        {
            xrmMgr = GetXrmManagerFromAppSettings(Guid.Empty);
            callerId = ValidateLogin(@"D1\NHECGI");
            mgr = new AllBinaryManager(callerId);
        }


        /// <summary>
        /// Validate the security code given against the user in CRM
        /// </summary>
        /// <param name="username">CRM User Name</param>
        /// <returns>CRM User Guid</returns>
        internal Guid ValidateLogin(string username)
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

            var users = xrmMgr.Service.RetrieveMultiple(new FetchExpression(fetchXmlQuery));


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
        private XrmManager GetXrmManagerFromAppSettings(Guid callerId)
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

        /// <summary>
        /// Get Customer Entity details.
        /// </summary>
        [TestMethod]
        public void GetCustomerDetails()
        {
            var response = mgr.GetCustomerDetails(new Guid("DA7EB949-1205-E511-80D7-005056906AE2"), AccountCategoryCode.Company);

            Assert.IsNotNull(response);
        }
    }
}
