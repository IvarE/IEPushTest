using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{
    public class contact_Create_Pre : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];

                    if (!_data.Target.Contains("cgi_contactnumber"))
                    {
                        //Update if exists.
                        if (_checkIfAutonumberExists(_data) == true)
                        {
                            Entity _autonumber = _getAutoNumber(_data);
                            if (_autonumber != null)
                            {
                                string _lastused = _autonumber.Attributes["cgi_lastused"].ToString();
                                Int32 _nextnumber = Convert.ToInt32(_lastused) + 1;
                                _data.Target.Attributes["cgi_contactnumber"] = _nextnumber.ToString();
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
                            _data.Target.Attributes["cgi_contactnumber"] = "1";
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
        #endregion

        #region Private Methods
        private bool _checkIfAutonumberExists(PluginData data)
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

        private Entity _getAutoNumber(PluginData data)
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
        #endregion
    }
}
