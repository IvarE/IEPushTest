using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGIXrmExtConnectorServiceTest
{
    [ExcludeFromCodeCoverage]
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
