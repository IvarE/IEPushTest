using Endeavor.Crm;
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

namespace Skanetrafiken.Crm.Entities
{
    public class UnitEntity : Generated.UoM
    {
        public static string OneDayUnitName = "1 dag";
        public static bool IsOneDayUnit(Plugin.LocalPluginContext localContext,EntityReference unitER)
        {
            if(unitER != null && unitER.Id != Guid.Empty)
            {
                UnitEntity unit = XrmRetrieveHelper.Retrieve<UnitEntity>(localContext, unitER.Id, new ColumnSet(UnitEntity.Fields.Name));

                if(unit != null && !string.IsNullOrEmpty(unit.Name) && unit.Name == "1 dag")
                {
                    return true;
                }
            }

            return false;
        }
    }

    
}