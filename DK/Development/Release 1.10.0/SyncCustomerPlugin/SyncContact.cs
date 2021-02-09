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
    public class SyncContact : IPlugin
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
                if (entity.LogicalName != "contact")
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
                            ExecuteCustomerSync.Sync(entity.Id.ToString(), filePath);

                        }


                    }
                }
            }
        }
        #endregion
    
        




        private static void HandleFullNameUpdate_UpdateQueueItems(Entity contact, IOrganizationService service)
        {
            if (contact.Contains("fullname"))
            {
                string contactId = contact.Id.ToString(),
                    contactFullName = contact["fullname"].ToString();

                UpdateQueueItems(contactId, contactFullName, service);
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

        /// <summary>
        /// Updates queueitems
        /// </summary>
        /// <param name="contactId">Id of contact whows incdidents queueitems should be updated</param>
        /// <param name="fullName">Full name of contact for queueitem cgi_customer attribute to be updated</param>
        /// <param name="service">IOrganizationService</param>
        private static void UpdateQueueItems(string contactId, string fullName, IOrganizationService service)
        {
            // Query gets:
            // 1. All active queueitems
            // 2. whos activie incidents
            // 3. has the attribute cgi_contactid of value contactId
            string fetchXml =
                        "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                          "<entity name='queueitem'>" +
                            "<attribute name='queueid' />" +
                            "<filter type='and'>" +
                              "<condition attribute='statecode' operator='eq' value='0' />" +
                            "</filter>" +
                            "<link-entity name='incident' from='incidentid' to='objectid' alias='aa'>" +
                              "<filter type='and'>" +
                                "<condition attribute='statecode' operator='eq' value='0' />" +
                                "<condition attribute='cgi_contactid' operator='eq' uitype='contact' value='{" + contactId + "}' />" +
                              "</filter>" +
                            "</link-entity>" +
                          "</entity>" +
                        "</fetch>";

            FetchExpression query = new FetchExpression(fetchXml);
            EntityCollection records = service.RetrieveMultiple(query);
            if (records.Entities == null || records.Entities.Count <= 0) return;


            Microsoft.Xrm.Sdk.Messages.ExecuteMultipleRequest requestWithResults = new Microsoft.Xrm.Sdk.Messages.ExecuteMultipleRequest()
            {
                // Assign settings that define execution behavior: continue on error, return responses. 
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                },
                // Create an empty organization request collection.
                Requests = new OrganizationRequestCollection()
            };

            foreach (Entity entity in records.Entities)
            {
                Entity queueItemToUpdate = new Entity("queueitem")
                {
                    Id = entity.Id
                };
                queueItemToUpdate["cgi_customer"] = fullName;
                Microsoft.Xrm.Sdk.Messages.UpdateRequest updateRequest = new Microsoft.Xrm.Sdk.Messages.UpdateRequest { Target = queueItemToUpdate };
                requestWithResults.Requests.Add(updateRequest);
            }

            // Using this multiple update for speed, if in the future, we have cases that are in many queues at once.
            service.Execute(requestWithResults);

            // TODO handle error if we should
        }
       
    }
}
