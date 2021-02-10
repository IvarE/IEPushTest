using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Pre : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                }

                if (_data.Target.Attributes.Contains("cgi_travelcardid"))
                {
                    _setTravelCardOnCase(_data);
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void _setTravelCardOnCase(plugindata data)
        {
            string _travelcard = data.Target.Attributes["cgi_travelcardid"].ToString();
            if (data.Target.Attributes.Contains("cgi_unregisterdtravelcard"))
            {
                data.Target.Attributes["cgi_unregisterdtravelcard"] = _travelcard;
            }
            else
            {
                data.Target.Attributes.Add("cgi_unregisterdtravelcard", _travelcard);
            }
        }

    }
}
