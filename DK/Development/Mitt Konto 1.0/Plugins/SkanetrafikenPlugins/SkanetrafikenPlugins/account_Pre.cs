using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class account_Create_Pre : IPlugin
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

                    if (!data.Target.Contains("accountnumber"))
                    {
                        data.Target.Attributes["accountnumber"] = _formatNumber(Common.CustomerHandler.getNextCustomerNumber("CustomerNumberAccount", data), data);

                        ////Update if exists.
                        //if (_checkIfAutonumberExists(data))
                        //{
                        //    Entity autonumber = _getAutoNumber(data);
                        //    if (autonumber != null)
                        //    {
                        //        string lastused = autonumber.Attributes["cgi_lastused"].ToString();
                        //        Int32 nextnumber = Convert.ToInt32(lastused) + 1;
                        //        data.Target.Attributes["accountnumber"] = _formatNumber(nextnumber, data);
                        //        autonumber.Attributes["cgi_lastused"] = nextnumber.ToString();
                        //        data.Service.Update(autonumber);
                        //    }
                        //}
                        //else //Create autonumber if not exists.
                        //{
                        //    Entity autonumber = new Entity
                        //    {
                        //        LogicalName = "cgi_autonumber"
                        //    };

                        //    autonumber.Attributes["cgi_entity"] = data.Target.LogicalName;
                        //    autonumber.Attributes["cgi_lastused"] = "1";
                        //    data.Service.Create(autonumber);
                        //    data.Target.Attributes["accountnumber"] = _formatNumber(1, data);
                        //}
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
                throw new InvalidPluginExecutionException(ex.Message);
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

        private string _formatNumber(int input, PluginData data)
        {
            string returnValue;
            try
            {

                #region fetch

                string now = DateTime.Now.ToString("s");
                string xml = "";
                xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
                xml += "   <entity name='cgi_setting'>";
                xml += "       <attribute name='cgi_settingid' />";
                xml += "       <attribute name='cgi_organizationprefix' />";
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

                #endregion fetch

                FetchExpression f = new FetchExpression(xml);
                EntityCollection settings = data.Service.RetrieveMultiple(f);

                if (settings != null)
                {
                    Entity ent = settings[0];
                    if (ent.Attributes.Contains("cgi_organizationprefix"))
                    {
                        // Ex ST000042
                        string prefix = ent.Attributes["cgi_organizationprefix"].ToString();
                        int length = 8 - prefix.Length;
                        string number = input.ToString().PadLeft(length, '0');
                        returnValue = string.Format("{0}{1}", prefix, number);
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
            return returnValue;
        }
        #endregion
    }
}
