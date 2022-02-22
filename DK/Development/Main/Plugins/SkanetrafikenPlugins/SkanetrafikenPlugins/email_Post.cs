using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Text.RegularExpressions;
using System.Net;

namespace CRM2013.SkanetrafikenPlugins
{
    public class email_Post : IPlugin
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

                    if (data.Target.Attributes.Contains("regardingobjectid"))
                    {
                        EntityReference regarding = data.Target.Attributes["regardingobjectid"] as EntityReference;
                        string activitytype = data.Target.LogicalName;
                        
                        if (regarding != null && regarding.LogicalName.Equals("incident"))
                        {
                            Entity incident = _getIncident(data, regarding.Id);
                            string description = "";
                            if (incident.Attributes.Contains("description"))
                            {
                                description = incident.Attributes["description"].ToString();
                            }

                            if (activitytype == "email" && description == null && description == "")
                            {
                                Entity activity = _getActivity(data, data.Target.Id);
                                string activityDescription = "";

                                if (activity.Attributes.Contains("description"))
                                    activityDescription = activity.Attributes["description"].ToString();

                                string newdescription = description + "\n\n" + activityDescription;
                                newdescription = Html2Plain(newdescription);

                                EntityReference refcustomerid = null;
                                if (incident.Attributes.Contains("customerid"))
                                    refcustomerid = incident.Attributes["customerid"] as EntityReference;

                                if (refcustomerid != null)
                                {
                                    string typeaccountcontact = refcustomerid.LogicalName;

                                    if (typeaccountcontact == "contact")
                                    {
                                        incident.Attributes["cgi_contactid"] = refcustomerid;
                                        incident.Attributes["description"] = newdescription;
                                        data.Service.Update(incident);
                                    }

                                    if (typeaccountcontact == "account")
                                    {
                                        incident.Attributes["cgi_accountid"] = refcustomerid;
                                        incident.Attributes["description"] = newdescription;
                                        data.Service.Update(incident);
                                    }
                                }
                                else
                                {
                                    Entity setting = _getSetting(data);
                                    if (setting != null)
                                    {
                                        if (setting.Attributes.Contains("cgi_defaultcustomeroncase"))
                                        {
                                            EntityReference defaultcustomer = setting.Attributes["cgi_defaultcustomeroncase"] as EntityReference;
                                            incident.Attributes["customerid"] = defaultcustomer;
                                            incident.Attributes["cgi_accountid"] = defaultcustomer;
                                            incident.Attributes["description"] = newdescription;
                                            data.Service.Update(incident);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
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
            desc = desc.Trim(' ', '\n', '\r', '\t');
            while (desc.IndexOf("\n\n\n", StringComparison.Ordinal) != -1)
            {
                desc = desc.Replace("\n\n\n", "\n\n");
            }



            return desc;
        }

        private Entity _getIncident(PluginData data, Guid caseid)
        {
            Entity returnValue = null;

            try
            {
                QueryByAttribute query = new QueryByAttribute("incident")
                {
                    ColumnSet = new ColumnSet(true)
                };
                query.Attributes.Add("incidentid");
                query.Values.Add(caseid);
                EntityCollection incidents = data.Service.RetrieveMultiple(query);
                if (incidents != null && incidents.Entities.Any())
                {
                    returnValue = incidents[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;
        }

        private Entity _getActivity(PluginData data, Guid activityid)
        {
            Entity returnValue = null;

            try
            {
                QueryByAttribute query = new QueryByAttribute("email")
                {
                    ColumnSet = new ColumnSet(true)
                };
                query.Attributes.Add("activityid");
                query.Values.Add(activityid);
                EntityCollection activities = data.Service.RetrieveMultiple(query);
                if (activities != null && activities.Entities.Any())
                {
                    returnValue = activities[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;
        }

        private Entity _getSetting(PluginData data)
        {
            Entity returnValue = null;

            try
            {
                string now = DateTime.Now.ToString("s");
                string xml = "";
                xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
                xml += "   <entity name='cgi_setting'>";
                xml += "       <attribute name='cgi_defaultcustomeroncase' />";
                xml += "       <filter type='and'>";
                xml += "           <condition attribute='statecode' operator='eq' value='0' />";
                xml += "           <condition attribute='cgi_validfrom' operator='on-or-before' value='" + now + "' />";
                xml += "           <filter type='or'>";
                xml += "               <condition attribute='cgi_validto' operator='on-or-after' value='" + now + "' />";
                xml += "               <condition attribute='cgi_validto' operator='null' />";
                xml += "           </filter>";
                xml += "       </filter>";
                xml += "   </entity>";
                xml += "</fetch>";

                FetchExpression f = new FetchExpression(xml);
                EntityCollection settings = data.Service.RetrieveMultiple(f);
                if (settings != null && settings.Entities.Any())
                {
                    returnValue = settings[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnValue;

        }
        #endregion
    }
}
