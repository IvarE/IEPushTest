using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Xml;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;

using Generated = Skanetrafiken.Crm.Schema.Generated;
using Skanetrafiken.Crm;
using Skanetrafiken.Crm.Entities;
using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Globalization;
using System.Net;
using Skanetrafiken.Crm;
using System.Text.RegularExpressions;

namespace Endeavor.Crm.UnitTest
{
    [TestFixture(Category = "Plugin")]
    public class DeltabatchQueueFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void GeneratePlusFilesForAllActiveContacts()
        {
            #region Test Setup
            // Connect to the Organization service. 
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                #endregion

                QueryExpression query = new QueryExpression
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(ContactEntity.Fields.ContactId, ContactEntity.Fields.cgi_socialsecuritynumber, ContactEntity.Fields.FullName),
                    Criteria =
                    {
                        Conditions =
                        {
                            new ConditionExpression(ContactEntity.Fields.cgi_socialsecuritynumber, ConditionOperator.NotNull),
                            new ConditionExpression(ContactEntity.Fields.StateCode, ConditionOperator.Equal, (int)Generated.ContactState.Active)
                        }
                    }
                };

                IList<ContactEntity> allContacts = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query);

                Regex regEx = new Regex("^[0-9]{12}$");
                foreach (ContactEntity c in allContacts)
                {
                    if (regEx.IsMatch(c.cgi_socialsecuritynumber))
                    {
                        DeltabatchQueueEntity q = new DeltabatchQueueEntity
                        {
                            ed_Contact = c.ToEntityReference(),
                            ed_ContactGuid = c.ContactId?.ToString(),
                            ed_ContactNumber = c.cgi_socialsecuritynumber,
                            ed_DeltabatchOperation = Generated.ed_deltabatchqueue_ed_deltabatchoperation.Plus,
                            ed_name = $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}".Length > 99 ? $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}".Substring(0, 99) : $"FirstMegaPlusFile: {c.FullName}, {DateTime.Now.ToString()}"
                        };

                        XrmHelper.Create(localContext, q);
                    }
                }


                #region Test Cleanup
                localContext.TracingService.Trace("Stop Sequences, ElapsedMilliseconds: {0}.", stopwatch.ElapsedMilliseconds);
            }
            #endregion
        }
        
        internal ServerConnection ServerConnection
        {
            get
            {
                if (_serverConnection == null)
                {
                    _serverConnection = new ServerConnection();
                }
                return _serverConnection;
            }
        }

        internal ServerConnection.Configuration Config
        {
            get
            {
                return TestSetup.Config;
            }
        }

    }
}
