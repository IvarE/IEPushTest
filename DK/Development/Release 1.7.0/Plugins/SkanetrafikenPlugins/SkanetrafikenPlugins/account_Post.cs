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
                data.InitPostImage("PostImage");


                if (data.PostImage != null && data.PostImage.Attributes.Contains("cgi_myaccount") && data.PostImage["cgi_myaccount"] != null && ((bool)data.PostImage["cgi_myaccount"]))
                {
                    if (data.Target.Contains("emailaddress1")
              || data.Target.Contains("statuscode")
              || data.Target.Contains("name")
              || data.Target.Contains("cgi_firstname")
              || data.Target.Contains("cgi_lastname")
              || data.Target.Contains("address1_line1")
              || data.Target.Contains("address1_line2")
              || data.Target.Contains("address1_postalcode")
              || data.Target.Contains("address1_city")
              || data.Target.Contains("address1_country")
              || data.Target.Contains("telephone1")
              || data.Target.Contains("telephone2")
              || data.Target.Contains("telephone3"))
                    {
                        // Sync with e-handel. 
                        CustomerHandler.ExecuteCustomerSyncronization(data.Service, data.Target.Id.ToString());
                    }
                }
            }
        }
        #endregion
    }
}
