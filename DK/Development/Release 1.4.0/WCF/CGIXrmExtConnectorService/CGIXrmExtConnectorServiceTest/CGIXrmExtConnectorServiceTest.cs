using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGIXrmExtConnectorServiceTest
{
    [TestClass]
    public class CGIXrmExtConnectorServiceTest
    {
        private readonly CGIXrmExtConnectorService.ExtConnectorService _service;

        public CGIXrmExtConnectorServiceTest()
        {
            _service = new CGIXrmExtConnectorService.ExtConnectorService(); 
        }

        [TestMethod]
        public void GetAgentUserIdTest()
        {
        }
    }
}
