using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Xml;

namespace SyncCustomerPlugin
{
    public class SyncAccount : IPlugin
    {

        #region Declarations
        public string UnsecureConfig;
        public XmlDocument PluginConfigXml;
        public IOrganizationService _service;
        #endregion

        #region public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // get reference to CRM Web ServiceIOrganizationServiceFactory 
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            _service = service;

            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                if (entity.LogicalName != "account")
                    return;

                if (context.PostEntityImages.Contains("PostImage") && context.PostEntityImages["PostImage"] is Entity)

                {
                    Entity postImageEntity = context.PostEntityImages["PostImage"];

                    if (postImageEntity.Contains("cgi_myaccount") && postImageEntity["cgi_myaccount"] != null && ((bool)postImageEntity["cgi_myaccount"]))
                    {
                        if (entity.Contains("emailaddress1")
                            || entity.Contains("statuscode")
                            || entity.Contains("firstname")
                            || entity.Contains("lastname")
                            || entity.Contains("address1_line1")
                            || entity.Contains("address1_line2")
                            || entity.Contains("address1_postalcode")
                            || entity.Contains("address1_city")
                            || entity.Contains("address1_country")
                            || entity.Contains("telephone1")
                            || entity.Contains("telephone2")
                            || entity.Contains("telephone3")
                            || entity.Contains("cgi_socialsecuritynumber"))
                        {
                            // Create files to e-handel. 
                            string filePath = GetFilepath(service);
                            //throw new InvalidPluginExecutionException(filePath);
                            ExecuteCustomerSync.Sync(entity.Id.ToString(), filePath);

                        }


                    }
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
            xml += "       <attribute name='cgi_customerfilepath' />";
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

                if (ent.Attributes.Contains("cgi_customerfilepath"))
                {
                    filePath = ent.Attributes["cgi_customerfilepath"].ToString();
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
