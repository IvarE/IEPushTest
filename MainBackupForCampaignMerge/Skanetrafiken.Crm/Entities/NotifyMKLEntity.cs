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
    public class NotifyMKLEntity : Generated.ed_notifymkl
    {
        internal void HandlePostNotifyMKLCreateAsync(Plugin.LocalPluginContext localContext)
        {
            SetStateRequest req = new SetStateRequest
            {
                EntityMoniker = ToEntityReference(),
                State = new OptionSetValue((int)Generated.ed_notifymklState.Completed),
                Status = new OptionSetValue((int)Generated.ed_notifymkl_statuscode.Completed)
            };

            localContext.OrganizationService.Execute(req);
        }
    }
}