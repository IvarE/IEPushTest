using System;
using Microsoft.Xrm.Sdk;

namespace CRM2013.SkanetrafikenPlugins
{
    public class cgi_casecategory_Pre : IPlugin
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
                    _setCaseCategoryName(data);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private void _setCaseCategoryName(PluginData data)
        {
            try
            {
                if (data.Target.Attributes.Contains("cgi_casecategoryname"))
                    return;

                string caption = "";

                if (data.Target.Attributes.Contains("cgi_casecategory1id"))
                {
                    var cat1 = data.Target.Attributes["cgi_casecategory1id"] as EntityReference;
                    if (cat1 != null)
                    {
                        caption += cat1.Name + " ";
                    }
                }

                if (data.Target.Attributes.Contains("cgi_casecategory2id"))
                {
                    var cat2 = data.Target.Attributes["cgi_casecategory2id"] as EntityReference;
                    if (cat2 != null)
                    {
                        caption += cat2.Name + " ";
                    }
                }

                if (data.Target.Attributes.Contains("cgi_casecategory3id"))
                {
                    var cat3 = data.Target.Attributes["cgi_casecategory3id"] as EntityReference;
                    if (cat3 != null)
                    {
                        caption += cat3.Name + " ";
                    }
                }

                if (caption != "")
                {
                    caption = caption.Substring(1, caption.Length - 1);
                }

                data.Target.Attributes["cgi_casecategoryname"] = caption;

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion
    }
}
