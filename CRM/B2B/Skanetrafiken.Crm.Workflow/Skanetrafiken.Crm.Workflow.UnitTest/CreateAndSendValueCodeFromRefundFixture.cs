using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using Skanetrafiken.Crm.Entities;
using Skanetrafiken.Crm.ValueCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endeavor.Crm.UnitTest
{
    public class CreateAndSendValueCodeFromRefundFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

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

        [Test]
        public void CreateValueCodeForCase()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                // Id to contact Marcus Stenswed
                Guid contactId = new Guid("7FDA72A0-8689-E811-80EF-005056B61FFF");
                ContactEntity contact = XrmRetrieveHelper.Retrieve<ContactEntity>(localContext, contactId, new ColumnSet(false));

                IncidentEntity incident = new IncidentEntity()
                {
                    //cgi_Contactid = contact.ToEntityReference(),
                    CustomerId = contact.ToEntityReference()
                };
                incident.Id = XrmHelper.Create(localContext, incident);


                RefundEntity refund = new RefundEntity
                {
                    cgi_Caseid = new EntityReference(IncidentEntity.EntityLogicalName, incident.Id),
                    cgi_RefundTypeid = new EntityReference(Skanetrafiken.Crm.Schema.Generated.cgi_refundtype.EntityLogicalName, new Guid("D52504D8-9E26-E611-80DF-005056903A38")),
                    cgi_ReimbursementFormid = new EntityReference(Skanetrafiken.Crm.Schema.Generated.cgi_reimbursementform.EntityLogicalName, new Guid("1B0079DE-7BB7-E411-80D3-005056904F1C")),
                    cgi_Amount = new Money(50),
                };

                refund.Id = XrmHelper.Create(localContext, refund);

                

                //CreateValueCodeGeneric

                //Guid valueCodeId = ValueCodeHandler.CreateMobileValueCode(localContext, 100, contact.MobilePhone, templateNumber: 161, contact: contact, incident: incident, refund: refund);
                //Guid valueCodeId = ValueCodeHandler.CreateMobileValueCodeGeneric(localContext, DateTime.Now.AddDays(365), 100, "0735198846", 1, contact: contact, refund: refund);
                //Assert.NotNull(valueCodeId);

                //ValueCodeEntity valueCode = XrmRetrieveHelper.Retrieve<ValueCodeEntity>(localContext, valueCodeId, new ColumnSet(true));

                //Assert.NotNull(valueCode.ed_Link);

                //XrmHelper.Delete(localContext, valueCode.ToEntityReference());
                //XrmHelper.Delete(localContext, refund.ToEntityReference());
                //XrmHelper.Delete(localContext, incident.ToEntityReference());

            }
        }
    }
}
