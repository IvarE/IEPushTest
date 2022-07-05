using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


using Microsoft.Crm.Sdk.Messages;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Skanetrafiken.Crm.Entities;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using System.Diagnostics;

namespace Endeavor.Crm.UnitTest
{
    class TicketInfoFixture : PluginFixtureBase
    {
        [Test]
        public void CreateClientelingActionAndEmailForContacts()
        {
            //Plugin.LocalPluginContext localContext = GenerateLocalContext();
        }
    }
}
