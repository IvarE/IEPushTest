using System;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    /*
     * Description:
     * This plugin runs on account in a post event async on the update message.
     */
    public class account_Post : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);

            Entity target;
            if (data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                data.Target = target;

                if (data.Target.Contains("emailaddress1") || data.Target.Contains("statuscode"))
                {
                    // Sync with e-handel. 
                    CustomerHandler.ExecuteCustomerSyncronization(data.Service, data.Target.Id.ToString());
                }
            }
        }
        #endregion
    }
}
