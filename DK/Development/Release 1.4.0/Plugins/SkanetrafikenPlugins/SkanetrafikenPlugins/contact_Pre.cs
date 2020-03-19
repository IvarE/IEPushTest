using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class contact_Create_Pre : IPlugin
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

                    if (!data.Target.Contains("cgi_contactnumber"))
                    {
                        //Update if exists.
                        if (_checkIfAutonumberExists(data))
                        {
                            Entity autonumber = _getAutoNumber(data);
                            if (autonumber != null)
                            {
                                string lastused = autonumber.Attributes["cgi_lastused"].ToString();
                                Int32 nextnumber = Convert.ToInt32(lastused) + 1;
                                data.Target.Attributes["cgi_contactnumber"] = nextnumber.ToString();
                                autonumber.Attributes["cgi_lastused"] = nextnumber.ToString();
                                data.Service.Update(autonumber);
                            }
                        }
                        else //Create autonumber if not exists.
                        {
                            Entity autonumber = new Entity
                            {
                                LogicalName = "cgi_autonumber"
                            };
                            autonumber.Attributes["cgi_entity"] = data.Target.LogicalName;
                            autonumber.Attributes["cgi_lastused"] = "1";
                            data.Service.Create(autonumber);
                            data.Target.Attributes["cgi_contactnumber"] = "1";
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
        private bool _checkIfAutonumberExists(PluginData data)
        {
            bool returnvalue = false;

            try
            {
                QueryByAttribute query = new QueryByAttribute("cgi_autonumber")
                {
                    ColumnSet = new ColumnSet("cgi_lastused")
                };
                query.Attributes.Add("cgi_entity");
                query.Values.Add(data.Target.LogicalName);
                query.Attributes.Add("statecode");
                query.Values.Add(0);
                EntityCollection autonumbers = data.Service.RetrieveMultiple(query);
                if (autonumbers != null && autonumbers.Entities.Any())
                {
                    returnvalue = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnvalue;
        }

        private Entity _getAutoNumber(PluginData data)
        {
            Entity returnvalue = null;

            try
            {
                QueryByAttribute query = new QueryByAttribute("cgi_autonumber")
                {
                    ColumnSet = new ColumnSet("cgi_lastused")
                };
                query.Attributes.Add("cgi_entity");
                query.Values.Add(data.Target.LogicalName);
                query.Attributes.Add("statecode");
                query.Values.Add(0);
                EntityCollection autonumbers = data.Service.RetrieveMultiple(query);
                if (autonumbers != null && autonumbers.Entities.Any())
                {
                    returnvalue = autonumbers[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnvalue;
        }
        #endregion
    }
}
