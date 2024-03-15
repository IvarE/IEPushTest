using Endeavor.Crm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Skanetrafiken.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Skanetrafiken.Crm.Properties;
using System.Configuration;
using System.Threading;

namespace Skanetrafiken.Crm.Controllers
{
    public class PingConnectionController : WrapperController
    {
        private string _prefix = "PingConnection";

        [HttpGet]
        public HttpResponseMessage Get()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            return CrmPlusControl.PingConnection(threadId, _prefix);
        }
    }
}
