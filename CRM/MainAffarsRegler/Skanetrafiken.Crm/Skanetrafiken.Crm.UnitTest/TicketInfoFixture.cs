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
using Microsoft.Crm.Sdk.Samples;

namespace Endeavor.Crm.IntegrationTests
{
    [TestFixture]
    public class TicketInfoFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Explicit, Category("Debug")]
        public void UpdateClassificationForAllCustomers()
        {
            // The using statement assures that the service proxy will be properly disposed.
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, null, new TracingService());

                bool live = false; 

                QueryExpression query1 = new QueryExpression()
                {
                    EntityName = ContactEntity.EntityLogicalName,
                    ColumnSet = new ColumnSet(ContactEntity.Fields.ed_MklId, ContactEntity.Fields.ed_Kundresan),
                    LinkEntities =
                    {
                        new LinkEntity()
                        {
                            LinkFromEntityName = ContactEntity.EntityLogicalName,
                            LinkToEntityName = TicketInfoEntity.EntityLogicalName,
                            LinkFromAttributeName = ContactEntity.Fields.ed_MklId,
                            LinkToAttributeName = TicketInfoEntity.Fields.ed_MklId,
                            EntityAlias = TicketInfoEntity.EntityLogicalName,
                            JoinOperator = JoinOperator.Inner
                        }
                    }
                };
                //828k
                // Denna lista hämtar duplicates (finns det mer än 1 ticket purchase på kunden hamnar kunden här 1 gång för varje ticket purchase) 
                IList<ContactEntity> contactsWithTicketPurchase = XrmRetrieveHelper.RetrieveMultiple<ContactEntity>(localContext, query1);

                var numberOfContacts = 0;

                if (live)
                {
                    foreach (ContactEntity contact in contactsWithTicketPurchase)
                    {
                        numberOfContacts++;
                        TicketInfoEntity.CalculateClassification(localContext, contact.ed_MklId, null);
                        localContext.TracingService.Trace($"Handled {numberOfContacts}/{contactsWithTicketPurchase.Count}");
                    }
                }
                


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
