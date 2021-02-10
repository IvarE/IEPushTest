using System;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Pre : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);

            try
            {
                if (data.Context.InputParameters.Contains("Target") && data.Context.InputParameters["Target"] is Entity)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];
                }

                if (data.Target.Attributes.Contains("cgi_travelcardid"))
                {
                    _setTravelCardOnCase(data);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private void _setTravelCardOnCase(PluginData data)
        {
            string travelcard = data.Target.Attributes["cgi_travelcardid"].ToString();
            if (data.Target.Attributes.Contains("cgi_unregisterdtravelcard"))
            {
                data.Target.Attributes["cgi_unregisterdtravelcard"] = travelcard;
            }
            else
            {
                data.Target.Attributes.Add("cgi_unregisterdtravelcard", travelcard);
            }
        }
        #endregion
    }
}
