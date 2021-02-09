using Microsoft.Xrm.Sdk;
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