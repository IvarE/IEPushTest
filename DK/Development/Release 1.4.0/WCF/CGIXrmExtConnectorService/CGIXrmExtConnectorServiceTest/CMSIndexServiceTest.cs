using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGIXrmExtConnectorService.CMSIndex.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CGIXrmExtConnectorServiceTest
{
    class CMSIndexServiceTest
    {
        private readonly CGIXrmExtConnectorService.CMSIndex.ExtConnectorService _service;

        public CMSIndexServiceTest()
        {
            _service = new CGIXrmExtConnectorService.CMSIndex.ExtConnectorService(); 
        }

        [TestMethod]
        public void CreateIndexTest()
        {

        }

        [TestMethod]
        public void RemoveIndex()
        {
            
        }
    }
}
