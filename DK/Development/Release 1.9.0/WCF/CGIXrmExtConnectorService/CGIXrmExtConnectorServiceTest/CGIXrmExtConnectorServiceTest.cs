using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGIXrmExtConnectorService;

namespace CGIXrmExtConnectorServiceTest
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ExtConnectorServiceManagerTest
    {
        private readonly ExtConnectorServiceManager _extConnectorServiceManager;

        public ExtConnectorServiceManagerTest()
        {
            _extConnectorServiceManager = new ExtConnectorServiceManager();
        }

        [TestMethod]
        public void GetAgentUserIdTest()
        {
            Guid userId = _extConnectorServiceManager.GetAgentUserId("skkundtjanstresegaranti@skanetrafiken.se");
            Assert.AreNotEqual(userId, Guid.Empty);
        }
    }
}
