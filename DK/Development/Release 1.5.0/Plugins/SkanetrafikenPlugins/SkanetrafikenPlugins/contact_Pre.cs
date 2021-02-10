using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            /*
            CREATE UNIQUE NONCLUSTERED INDEX idx_cgi_contactnumber_notnull
            ON ContactBase(cgi_contactnumber)
            WHERE cgi_contactnumber IS NOT NULL;
            */

            Entity _autonumber = null;
            EntityCollection _autonumbers = null;
            Boolean _updateEntity = false;
            Int32 _nextnumber = 0;
            var exceptions = new List<Exception>();
            Int32 retryCount = 3;

            QueryByAttribute query = new QueryByAttribute("cgi_autonumber")
            {
                ColumnSet = new ColumnSet("cgi_lastused")
            };
            query.Attributes.Add("cgi_entity");
            query.Values.Add(data.Target.LogicalName);
            query.Attributes.Add("statecode");
            query.Values.Add(0);

            try
            {
                _autonumbers = data.Service.RetrieveMultiple(query);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            if (_autonumbers != null && _autonumbers.Entities.Any())
            {
                _autonumber = _autonumbers[0];
                _updateEntity = true;
                string _lastused = _autonumber.Attributes["cgi_lastused"].ToString();
                _nextnumber = Convert.ToInt32(_lastused);
            }
            else
            {
                _autonumber = new Entity
                {
                    LogicalName = "cgi_autonumber"
                };
                _autonumber.Attributes["cgi_entity"] = data.Target.LogicalName;
            }
            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if (retry > 0)
                        Thread.Sleep(100);

                    _nextnumber++;
                    data.Target.Attributes["cgi_contactnumber"] = _nextnumber.ToString();
                    _autonumber.Attributes["cgi_lastused"] = _nextnumber.ToString();

                    if (_updateEntity)
                        data.Service.Update(_autonumber);
                    else
                        data.Service.Create(_autonumber);

                    return;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException(exceptions);

        }
        #endregion

        #region Private Methods
  
        #endregion
    }
}
