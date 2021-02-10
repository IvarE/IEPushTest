using System;
using Microsoft.Xrm.Sdk;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    /*
     * Description:
     * This plugin runs on cgi_travelcard in a pre event sync on the update message.
     */
    public class cgi_travelcard_Pre : IPlugin
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
                if (data.Target.Attributes.TryGetValue("cgi_contactid", out contactId))
                {
                    if (contactId == null)
                    {
                        ResetCurrentTravelCardName(data);
                    }
                }
                else if (data.Target.Attributes.TryGetValue("cgi_accountid", out accountId))
                {
                    if (accountId == null)
                    {
                        ResetCurrentTravelCardName(data);
                    }
                }
                
            }
        }
        #endregion

        #region Private Methods
        private static void ResetCurrentTravelCardName(PluginData data)
        {
            data.Target.Attributes["cgi_travelcardname"] = null;
        }
        #endregion
    }

}
