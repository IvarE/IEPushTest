using System;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    /*
     * Description:
     * This plugin runs on cgi_travelcard in a pre event sync on the update message.
     */
    public class cgi_travelcard_Post : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);
            data.InitPreImage("preImage");

            Entity target;
            if (data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                data.Target = target;

                object contactId, accountId;

                if (data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(data.PreImage, "cgi_contactid", out contactId))
                {
                    // Sync with e-handel. 
                    TravelCardHandler.ExecuteTravelCardSyncronization(data.Service, ((EntityReference)contactId).Id.ToString());
                }
                else if (data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(data.PreImage, "cgi_accountid", out accountId))
                {
                    // Sync with e-handel.
                    TravelCardHandler.ExecuteTravelCardSyncronization(data.Service, ((EntityReference)accountId).Id.ToString());
                }

            }
        }
        #endregion
    }
}
