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
    public class cgi_travelcard_Post : IPlugin
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

                object contactId;
                if (_data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(_data.PreImage, "cgi_contactid", out contactId))
                {
                    // Sync with e-handel. This is in post update sync, transaction is commited to database at this stage.
                    // A exception will roll back the transaction.
                    TravelCardHandler.ExecuteTravelCardSyncronization(_data.Service, ((EntityReference)contactId).Id.ToString());

                    // If sync with e-handel did not throw a exeption, go ahead and remove the card name
                    ResetCurrentTravelCardName(_data);

                }
                
            }
        }

        private static void ResetCurrentTravelCardName(plugindata _data)
        {
            Entity travelCard = new Entity();
            travelCard.LogicalName = _data.Target.LogicalName;
            travelCard.Id = _data.Target.Id;
            travelCard["cgi_travelcardname"] = null;
            _data.Service.Update(travelCard);
        }
    }

}
