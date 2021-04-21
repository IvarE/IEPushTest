using Endeavor.Crm;
using Endeavor.Crm.Extensions;
using Microsoft.Crm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using Generated = Skanetrafiken.Crm.Schema.Generated;

namespace Skanetrafiken.Crm.Entities
{
    public class OrderProductEntity : Generated.SalesOrderDetail
    {
        public static void HandleOrderProductEntityCreate (Plugin.LocalPluginContext localContext, OrderProductEntity orderProduct)
        {

        }
    }
}
