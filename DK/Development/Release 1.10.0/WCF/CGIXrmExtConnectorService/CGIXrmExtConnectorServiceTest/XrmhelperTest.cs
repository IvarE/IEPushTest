using CGIXrmExtConnectorService;
using CGIXrmWin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmExtConnectorServiceTest
{
    [TestClass]
    public class XrmHelperTest
    {
        XrmHelper _xrmHelper;
        private readonly XrmManager _xrmManager;
        public XrmHelperTest()
        {
            _xrmHelper = new XrmHelper();
            var username = ConfigurationManager.AppSettings["Username"];
            var password = ConfigurationManager.AppSettings["Password"];
            var domain = ConfigurationManager.AppSettings["Domain"];
            var serverAdress = ConfigurationManager.AppSettings["CrmServerUrl"];

            _xrmManager = new XrmManager(serverAdress, domain, username, password);
        }
        
        [TestMethod]
        public void OpenCloseSqlTest()
        {
            SqlConnection sqlConnection = _xrmHelper.OpenSQL();
            Assert.IsTrue(sqlConnection.State == System.Data.ConnectionState.Open);

            _xrmHelper.CloseSQL(sqlConnection);
            Assert.IsTrue(sqlConnection.State == System.Data.ConnectionState.Closed);
        }

        [TestMethod]
        public void GetXrmManagerFromAppSettingsTest()
        {
            Guid userGuid = _xrmHelper.GetId("skkundtjanstresegaranti@skanetrafiken.se", "internalemailaddress", "systemuser", _xrmManager);
            Assert.AreNotEqual(userGuid, Guid.Empty);
            XrmManager xrmManager = _xrmHelper.GetXrmManagerFromAppSettings(userGuid);
            Assert.IsNotNull(xrmManager);

        }

        [TestMethod]
        public void GetIdTest()
        {
            Assert.AreNotEqual(_xrmHelper.GetId("skkundtjanstresegaranti@skanetrafiken.se", "internalemailaddress", "systemuser", _xrmManager), Guid.Empty);
        }
    }
}
