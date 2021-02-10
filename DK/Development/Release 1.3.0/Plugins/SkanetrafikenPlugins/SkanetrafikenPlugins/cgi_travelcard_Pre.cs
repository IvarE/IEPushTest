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
     * This plugin runs on cgi_travelcard in a pre event sync on the update message.
     */
    public class cgi_travelcard_Pre : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);
            _data.InitPreImage("preImage");

            Entity target;
            if (_data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                _data.Target = target;
                
                object contactId, accountId;
                if (_data.Target.Attributes.TryGetValue("cgi_contactid", out contactId))
                {
                    if (contactId == null)
                    {
                        ResetCurrentTravelCardName(_data);
                    }
                }
                else if (_data.Target.Attributes.TryGetValue("cgi_accountid", out accountId))
                {
                    if (accountId == null)
                    {
                        ResetCurrentTravelCardName(_data);
                    }
                }
                
            }
        }

        private static void ResetCurrentTravelCardName(plugindata _data)
        {
            _data.Target.Attributes["cgi_travelcardname"] = null;
        }

    }

}
