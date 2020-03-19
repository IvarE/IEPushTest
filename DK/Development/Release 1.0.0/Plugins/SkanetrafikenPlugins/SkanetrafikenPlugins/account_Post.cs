using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{

    /*
     * Description:
     * This plugin runs on account in a post event async on the update message.
     */
    public class account_Post : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);

            Entity target;
            if (_data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                _data.Target = target;

                if (_data.Target.Contains("emailaddress1") || _data.Target.Contains("statuscode"))
                {
                    // Sync with e-handel. 
                    CustomerHandler.ExecuteCustomerSyncronization(_data.Service, _data.Target.Id.ToString());
                }
            }
        }

    }

}
