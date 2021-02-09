using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
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
            PluginData _data = new PluginData(serviceProvider);
            _data.InitPreImage("preImage");

            Entity target;
            if (_data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                _data.Target = target;

                object contactId, accountId;

                if (_data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(_data.PreImage, "cgi_contactid", out contactId))
                {
                    // Sync with e-handel. 
                    TravelCardHandler.ExecuteTravelCardSyncronization(_data.Service, ((EntityReference)contactId).Id.ToString());
                }
                else if (_data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(_data.PreImage, "cgi_accountid", out accountId))
                {
                    // Sync with e-handel.
                    TravelCardHandler.ExecuteTravelCardSyncronization(_data.Service, ((EntityReference)accountId).Id.ToString());
                }

            }
        }
        #endregion
    }
}
