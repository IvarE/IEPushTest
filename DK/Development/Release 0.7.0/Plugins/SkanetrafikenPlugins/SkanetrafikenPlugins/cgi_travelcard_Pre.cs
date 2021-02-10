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
                if (_data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(_data.PreImage, "cgi_contactid", out contactId))
                {
                    // This will be rolled back if sync with e-handel throws a exeption in post sync execution of cgi_travelcard_Post
                    ResetCurrentTravelCardName(_data);

                }
                else if (_data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(_data.PreImage, "cgi_accountid", out accountId))
                {
                    // This will be rolled back if sync with e-handel throws a exeption in post sync execution of cgi_travelcard_Post
                    ResetCurrentTravelCardName(_data);

                }
                
            }
        }

        private static void ResetCurrentTravelCardName(plugindata _data)
        {
            _data.Target.Attributes["cgi_travelcardname"] = null;
            //_data.Target.Attributes["cgi_validfrom"] = null;
            //_data.Target.Attributes["cgi_validto"] = null;
            //_data.Target.Attributes["cgi_latestchargedate"] = null;
        }

    }

}
