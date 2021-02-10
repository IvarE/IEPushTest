using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{
    public class account_Create_Pre : IPlugin
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

                    if (!_data.Target.Contains("accountnumber"))
                    {
                        //Update if exists.
                        if (_checkIfAutonumberExists(_data) == true)
                        {
                            Entity _autonumber = _getAutoNumber(_data);
                            if (_autonumber != null)
                            {
                                string _lastused = _autonumber.Attributes["cgi_lastused"].ToString();
                                Int32 _nextnumber = Convert.ToInt32(_lastused) + 1;
                                _data.Target.Attributes["accountnumber"] = _formatNumber(_nextnumber, _data);
                                _autonumber.Attributes["cgi_lastused"] = _nextnumber.ToString();
                                _data.Service.Update(_autonumber);
                            }
                        }
                        else //Create autonumber if not exists.
                        {
                            Entity _autonumber = new Entity();
                            _autonumber.LogicalName = "cgi_autonumber";
                            _autonumber.Attributes["cgi_entity"] = _data.Target.LogicalName;
                            _autonumber.Attributes["cgi_lastused"] = "1";
                            _data.Service.Create(_autonumber);
                            _data.Target.Attributes["accountnumber"] = _formatNumber(1, _data);
                        }
                    }
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
        
        private bool _checkIfAutonumberExists(plugindata data)
        {
            bool _returnvalue = false;

            try
            {
                QueryByAttribute _query = new QueryByAttribute("cgi_autonumber");
                _query.ColumnSet = new ColumnSet("cgi_lastused");
                _query.Attributes.Add("cgi_entity");
                _query.Values.Add(data.Target.LogicalName);
                _query.Attributes.Add("statecode");
                _query.Values.Add(0);
                EntityCollection _autonumbers = data.Service.RetrieveMultiple(_query);
                if (_autonumbers != null && _autonumbers.Entities.Count() > 0)
                {
                    _returnvalue = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _returnvalue;
        }

        private Entity _getAutoNumber(plugindata data)
        {
            Entity _returnvalue = null;

            try
            {
                QueryByAttribute _query = new QueryByAttribute("cgi_autonumber");
                _query.ColumnSet = new ColumnSet("cgi_lastused");
                _query.Attributes.Add("cgi_entity");
                _query.Values.Add(data.Target.LogicalName);
                _query.Attributes.Add("statecode");
                _query.Values.Add(0);
                EntityCollection _autonumbers = data.Service.RetrieveMultiple(_query);
                if (_autonumbers != null && _autonumbers.Entities.Count() > 0)
                {
                    _returnvalue = _autonumbers[0] as Entity;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _returnvalue;
        }

        private string _formatNumber(int input, plugindata data)
        {
            string _returnValue = "";
            try
            {

                #region fetch

                string _now = DateTime.Now.ToString("s");
                string _xml = "";
                _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
                _xml += "   <entity name='cgi_setting'>";
                _xml += "       <attribute name='cgi_settingid' />";
                _xml += "       <attribute name='cgi_organizationprefix' />";
                _xml += "       <filter type='and'>";
                _xml += "           <condition attribute='statecode' operator='eq' value='0' />";
                _xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + _now + "' />";
                _xml += "           <filter type='or'>";
                _xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + _now + "' />";
                _xml += "               <condition attribute='cgi_validto' operator='null' />";
                _xml += "           </filter>";
                _xml += "       </filter>";
                _xml += "   </entity>";
                _xml += "</fetch>";

                #endregion fetch

                FetchExpression _f = new FetchExpression(_xml);
                EntityCollection settings = data.Service.RetrieveMultiple(_f);

                if (settings != null)
                {
                    Entity _ent = settings[0] as Entity;
                    if (_ent.Attributes.Contains("cgi_organizationprefix"))
                    {
                        // Ex ST000042
                        string _prefix = _ent.Attributes["cgi_organizationprefix"].ToString();
                        int _length = 8 - _prefix.Length;
                        string _number = input.ToString().PadLeft(_length, '0');
                        _returnValue = string.Format("{0}{1}", _prefix, _number);
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Hittar inte systeminställningar för kundprefix!");
                    }
                }
                else
                {
                    throw new InvalidPluginExecutionException("Hittar inte systeminställningar!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _returnValue;
        }




    }
}
