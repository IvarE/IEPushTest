using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SyncTravelCardPlugin.Common;
using Microsoft.Xrm.Sdk.Query;

namespace SyncTravelCardPlugin
{
    public class SyncTravelCard : IPlugin
    {
        

        #region public Methods

        public void Execute(IServiceProvider serviceProvider)
        {
           
            PluginData data = new PluginData(serviceProvider);
            data.InitPreImage("preImage");

            Entity target;

            string filePath = GetFilepath(data.Service);

            if (data.Context.InputParameters.TryGetTargetEntity(out target))
            {
                data.Target = target;

                object contactId, accountId;

                if (data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(data.PreImage, "cgi_contactid", out contactId))
                {
                    // Sync with e-handel. 
                    ExecuteTravelcardSync.Sync(((EntityReference)contactId).Id.ToString(), filePath);
                }
                else if (data.Target.Attributes.TryGetUpdatedOrPreImageAttributeValue(data.PreImage, "cgi_accountid", out accountId))
                {
                    // Sync with e-handel.
                    ExecuteTravelcardSync.Sync(((EntityReference)accountId).Id.ToString(), filePath);
                }

            }
        }

        private string GetFilepath(IOrganizationService service)
        {
            string filePath = "";

            string now = DateTime.Now.ToString("s");
            string xml = "";
            xml += "<fetch version='1.0' mapping='logical' distinct='false'>";
            xml += "   <entity name='cgi_setting'>";
            xml += "       <attribute name='cgi_travelcardfilepath' />";
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
            EntityCollection ents = service.RetrieveMultiple(f);
            if (ents != null && ents.Entities.Any())
            {
                Entity ent = ents[0];

                if (ent.Attributes.Contains("cgi_travelcardfilepath"))
                {
                    filePath = ent.Attributes["cgi_travelcardfilepath"].ToString();
                }


            }
            else
            {
                throw new InvalidPluginExecutionException("Settings is missing!");
            }


            return filePath;
        }
        #endregion
    }

}
