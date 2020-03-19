using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using CRM2013.SkanetrafikenPlugins.Common;

namespace CRM2013.SkanetrafikenPlugins
{
    public class contact_Post : IPlugin
    {
        #region public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData data = new PluginData(serviceProvider);

            Entity entity;
            if (data.Context.InputParameters.TryGetTargetEntity(out entity))
            {
                data.Target = entity;

                HandleFullNameUpdate_UpdateQueueItems(data);

                HandleEmailAdress1Update_SyncWithEhandel(data);
            }
        }
        #endregion

        #region Private Methods
        private static void HandleEmailAdress1Update_SyncWithEhandel(PluginData data)
        {
            if (data.Target.Contains("emailaddress1") || data.Target.Contains("statuscode"))
            {
                // Sync with e-handel. 
                CustomerHandler.ExecuteCustomerSyncronization(data.Service, data.Target.Id.ToString());
            }
        }

        private static void HandleFullNameUpdate_UpdateQueueItems(PluginData data)
        {
            if (data.Target.Contains("fullname"))
            {
                string contactId = data.Target.Id.ToString(),
                    contactFullName = data.Target["fullname"].ToString();

                UpdateQueueItems(contactId, contactFullName, data.Service);
            }
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
        #endregion
    }
}
