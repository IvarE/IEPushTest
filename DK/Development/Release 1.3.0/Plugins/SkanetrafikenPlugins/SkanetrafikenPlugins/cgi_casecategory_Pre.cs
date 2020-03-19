using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{
    public class cgi_casecategory_Pre : IPlugin
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
                    _setCaseCategoryName(_data);
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

        private void _setCaseCategoryName(plugindata data)
        {
            try
            {
                if (data.Target.Attributes.Contains("cgi_casecategoryname"))
                    return;

                EntityReference _cat1 = null;
                EntityReference _cat2 = null;
                EntityReference _cat3 = null;

                string _caption = "";

                if (data.Target.Attributes.Contains("cgi_casecategory1id"))
                {
                    _cat1 = data.Target.Attributes["cgi_casecategory1id"] as EntityReference;
                    _caption += _cat1.Name + " ";
                }

                if (data.Target.Attributes.Contains("cgi_casecategory2id"))
                {
                    _cat2 = data.Target.Attributes["cgi_casecategory2id"] as EntityReference;
                    _caption += _cat2.Name + " ";
                }

                if (data.Target.Attributes.Contains("cgi_casecategory3id"))
                {
                    _cat3 = data.Target.Attributes["cgi_casecategory3id"] as EntityReference;
                    _caption += _cat3.Name + " ";
                }

                if (_caption != "")
                {
                    _caption = _caption.Substring(1, _caption.Length - 1);
                }

                data.Target.Attributes["cgi_casecategoryname"] = _caption;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
