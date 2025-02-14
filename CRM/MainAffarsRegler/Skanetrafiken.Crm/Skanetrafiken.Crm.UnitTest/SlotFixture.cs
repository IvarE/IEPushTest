﻿using System;
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
using Microsoft.Xrm.Sdk.Metadata;


namespace Endeavor.Crm.IntegrationTests
{
    public class SlotFixture : PluginFixtureBase
    {
        private ServerConnection _serverConnection;

        [Test, Category("Debug")]
        public void CreateSlot()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                SlotsEntity slot = new SlotsEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    ed_BookingDay = new DateTime(2021, 04, 11),
                    ed_name = "Test UnitTest | DT: " + DateTime.Now.ToString(),
                    ed_Extended = true,
                    ed_StandardPrice = new Money(4000),
                    ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary,
                    //ed_Quote = new EntityReference(QuoteEntity.EntityLogicalName, Guid.Parse("06d8b0ef-af6a-eb11-9479-005056b6fa28")),
                    //ed_QuoteProductID = new EntityReference(QuoteProductEntity.EntityLogicalName, Guid.Parse("dad69739-3f99-eb11-947e-005056b6fa28")),

                    ed_ProductID = new EntityReference(ProductEntity.EntityLogicalName, Guid.Parse("19ffd424-a196-ea11-80f8-005056b61fff")),
                    ed_Order = new EntityReference(OrderEntity.EntityLogicalName, Guid.Parse("13164f39-2ba7-eb11-9480-005056b6fa28")),
                    ed_OrderProductID = new EntityReference(OrderProductEntity.EntityLogicalName, Guid.Parse("1a164f39-2ba7-eb11-9480-005056b6fa28")),
                    ed_Opportunity = new EntityReference(OpportunityEntity.EntityLogicalName, Guid.Parse("0a1c7dd8-13e9-e911-80f0-005056b61fff")),

                    
                };

                Guid slotId = XrmHelper.Create(localContext, slot);

                slot.Id = slotId;
                slot.ed_SlotsId = slotId;

                SlotsEntity.HandleSlotsEntityCreate(localContext, slot);
            }
        }

        [Test, Category("Debug")]
        public void UpdateSlot()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                SlotsEntity slot = XrmRetrieveHelper.Retrieve<SlotsEntity>(localContext, new EntityReference(SlotsEntity.EntityLogicalName, Guid.Parse("1e3fcb1d-5f9c-eb11-947e-005056b6fa28")), new ColumnSet(true));

                SlotsEntity updateSlot = new SlotsEntity()
                {
                    //CaseOriginCode = new OptionSetValue(3),
                    Id = slot.Id,
                    ed_SlotsId = slot.Id,
                    ed_Opportunity = new EntityReference(OpportunityEntity.EntityLogicalName, Guid.Parse("0a1c7dd8-13e9-e911-80f0-005056b61fff")),
                    ed_Quote = new EntityReference(QuoteEntity.EntityLogicalName, Guid.Parse("06d8b0ef-af6a-eb11-9479-005056b6fa28")),
                    ed_QuoteProductID = new EntityReference(QuoteProductEntity.EntityLogicalName, Guid.Parse("4d4bdc3c-709c-eb11-947e-005056b6fa28")),
                    ed_BookingStatus = Generated.ed_slots_ed_bookingstatus.Preliminary//new OptionSetValue(899310001)
                };
                
                SlotsEntity.HandleSlotsEntityUpdate(localContext, updateSlot, slot);
            }
        }

        [Test, Category("Debug")]
        public void UpdateSlotNumber()
        {
            using (_serviceProxy = ServerConnection.GetOrganizationProxy(Config))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();

                Plugin.LocalPluginContext localContext = new Plugin.LocalPluginContext(new ServiceProvider(), _serviceProxy, PluginExecutionContext, new TracingService());

                SlotsEntity slot = XrmRetrieveHelper.Retrieve<SlotsEntity>(localContext, new EntityReference(SlotsEntity.EntityLogicalName, Guid.Parse("836061cf-2aac-eb11-9480-005056b6fa28")), new ColumnSet(true));

                SlotsEntity.updateNumberSlot(localContext, slot);
            }
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
