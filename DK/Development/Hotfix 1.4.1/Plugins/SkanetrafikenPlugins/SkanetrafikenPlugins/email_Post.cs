using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Net;

namespace CRM2013.SkanetrafikenPlugins
{
    public class email_Post : IPlugin
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

                    if (_data.Target.Attributes.Contains("regardingobjectid"))
                    {
                        EntityReference _regarding = _data.Target.Attributes["regardingobjectid"] as EntityReference;
                        string _activitytype = _data.Target.LogicalName.ToString();
                        
                        if (_regarding != null && _regarding.LogicalName.Equals("incident"))
                        {
                            Entity _incident = _getIncident(_data, _regarding.Id);
                            string _description = "";
                            if (_incident.Attributes.Contains("description"))
                            {
                                _description = _incident.Attributes["description"].ToString();
                            }

                            if (_activitytype == "email" )
                            {
                                Entity _activity = _getActivity(_data, _data.Target.Id);
                                string _activityDescription = "";

                                if (_activity.Attributes.Contains("description"))
                                    _activityDescription = _activity.Attributes["description"].ToString();

                                string _newdescription = _description + "\n\n" + _activityDescription;
                                _newdescription = Html2Plain(_newdescription);

                                EntityReference _refcustomerid = null;
                                if (_incident.Attributes.Contains("customerid"))
                                    _refcustomerid = _incident.Attributes["customerid"] as EntityReference;

                                if (_refcustomerid != null)
                                {
                                    Guid _accountcontactid = _refcustomerid.Id;
                                    string _typeaccountcontact = _refcustomerid.LogicalName;

                                    if (_typeaccountcontact == "contact")
                                    {
                                        _incident.Attributes["cgi_contactid"] = _refcustomerid;
                                        _incident.Attributes["description"] = _newdescription;
                                        _data.Service.Update(_incident);
                                    }

                                    if (_typeaccountcontact == "account")
                                    {
                                        _incident.Attributes["cgi_accountid"] = _refcustomerid;
                                        _incident.Attributes["description"] = _newdescription;
                                        _data.Service.Update(_incident);
                                    }
                                }
                                else
                                {
                                    Entity _setting = _getSetting(_data);
                                    if (_setting != null)
                                    {
                                        if (_setting.Attributes.Contains("cgi_defaultcustomeroncase"))
                                        {
                                            EntityReference _defaultcustomer = _setting.Attributes["cgi_defaultcustomeroncase"] as EntityReference;
                                            _incident.Attributes["customerid"] = _defaultcustomer;
                                            _incident.Attributes["cgi_accountid"] = _defaultcustomer;
                                            _incident.Attributes["description"] = _newdescription;
                                            _data.Service.Update(_incident);
                                        }
                                    }
                                }
                            }
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
        private static string Html2Plain(string desc)
        {
            desc = desc.Replace(@"v\:* {behavior:url(#default#VML);}", "\n");
            desc = desc.Replace(@"o\:* {behavior:url(#default#VML);}", "\n");
            desc = desc.Replace(@"w\:* {behavior:url(#default#VML);}", "\n");
            desc = desc.Replace(@".shape {behavior:url(#default#VML);}", "\n");

            desc = desc.Replace("<br>", "\n");
            desc = desc.Replace("<BR>", "\n");
            desc = desc.Replace("<br/>", "\n");
            desc = desc.Replace("<BR/>", "\n");
            desc = desc.Replace("<div>", "\n");
            Regex myRegEx = new Regex("<[^>]+>"); // strip HTML encoding
            desc = myRegEx.Replace(desc, "");
            desc = WebUtility.HtmlDecode(desc);
            desc = desc.Replace("\n\r", "\n");
            desc = desc.Replace("\n \n", "\n\n");
            desc = desc.Trim(new char[] { ' ', '\n', '\r', '\t' });
            while (desc.IndexOf("\n\n\n", StringComparison.Ordinal) != -1)
            {
                desc = desc.Replace("\n\n\n", "\n\n");
            }



            return desc;
        }

        private Entity _getIncident(PluginData data, Guid caseid)
        {
            Entity _returnValue = null;

            try
            {
                QueryByAttribute _query = new QueryByAttribute("incident");
                _query.ColumnSet = new ColumnSet(true);
                _query.Attributes.Add("incidentid");
                _query.Values.Add(caseid);
                EntityCollection _incidents = data.Service.RetrieveMultiple(_query);
                if (_incidents != null && _incidents.Entities.Count() > 0)
                {
                    _returnValue = _incidents[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _returnValue;
        }

        private Entity _getActivity(PluginData data, Guid activityid)
        {
            Entity _returnValue = null;

            try
            {
                QueryByAttribute _query = new QueryByAttribute("email");
                _query.ColumnSet = new ColumnSet(true);
                _query.Attributes.Add("activityid");
                _query.Values.Add(activityid);
                EntityCollection _activities = data.Service.RetrieveMultiple(_query);
                if (_activities != null && _activities.Entities.Count() > 0)
                {
                    _returnValue = _activities[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _returnValue;
        }

        private Entity _getSetting(PluginData data)
        {
            Entity _returnValue = null;

            try
            {
                string _now = DateTime.Now.ToString("s");
                string _xml = "";
                _xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
                _xml += "   <entity name='cgi_setting'>";
                _xml += "       <attribute name='cgi_defaultcustomeroncase' />";
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

                FetchExpression _f = new FetchExpression(_xml);
                EntityCollection _settings = data.Service.RetrieveMultiple(_f);
                if (_settings != null && _settings.Entities.Count() > 0)
                {
                    _returnValue = _settings[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _returnValue;

        }
        #endregion
    }
}
