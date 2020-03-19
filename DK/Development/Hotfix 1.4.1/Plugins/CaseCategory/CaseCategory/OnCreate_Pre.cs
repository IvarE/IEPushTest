using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;

using Microsoft.Crm.Sdk.Messages;

namespace CGI.CRM2013.Skanetrafiken.CaseCategory
{
    public class OnCreate_Pre : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") &&
                    _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target =
                        (Entity)_data.Context.InputParameters["Target"];
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
        #endregion

        #region Private Methods
        private void _setCaseCategoryName(PluginData data)
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
                    _cat1 = data.Target.Attributes["cgi_casecategory1id"]
                        as EntityReference;
                    _caption += _cat1.Name + " "; 
                }

                if (data.Target.Attributes.Contains("cgi_casecategory2id"))
                {
                    _cat2 = data.Target.Attributes["cgi_casecategory2id"]
                        as EntityReference;
                    _caption += _cat2.Name + " ";
                }

                if (data.Target.Attributes.Contains("cgi_casecategory3id"))
                {
                    _cat3 = data.Target.Attributes["cgi_casecategory3id"]
                        as EntityReference;
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
        #endregion
    }
}
