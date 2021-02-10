using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Endeavor.Crm.Extensions;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Generated = Skanetrafiken.Crm.Schema.Generated;
using Endeavor.Crm;

namespace Skanetrafiken.Crm.Entities
{
    public class CustomerAddressEntity : Generated.CustomerAddress
    {

        internal static ColumnSet CustomerAddressInfoBlock = new ColumnSet(
            CustomerAddressEntity.Fields.AddressTypeCode,
            CustomerAddressEntity.Fields.Name,
            CustomerAddressEntity.Fields.Line2,
            CustomerAddressEntity.Fields.PostalCode,
            CustomerAddressEntity.Fields.City, 
            CustomerAddressEntity.Fields.Country
            );

        internal static List<CustomerAddressEntity> GetCustomerAddresses_ByParentId(Plugin.LocalPluginContext localContext, Guid parentId, ColumnSet column = null, FilterExpression filter = null)
        {

            var filterAddress = new QueryExpression()
            {
                EntityName = CustomerAddressEntity.EntityLogicalName,
                ColumnSet = column,
                Criteria = filter != null ? filter : new FilterExpression { Conditions = { new ConditionExpression(CustomerAddressEntity.Fields.ParentId, ConditionOperator.Equal, parentId) } }
            };

            var addresses = XrmRetrieveHelper.RetrieveMultiple<CustomerAddressEntity>(localContext, filterAddress);
            return addresses;
        }
    }

    
}