using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;

namespace CRM2013.SkanetrafikenPlugins
{
    public class contact_Post : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);

            Entity entity;
            if (_data.Context.InputParameters.TryGetTargetEntity(out entity))
            {
                _data.Target = entity;

                HandleFullNameUpdate_UpdateQueueItems(_data);

                HandleEmailAdress1Update_SyncWithEhandel(_data);
            }
        }

        private static void HandleEmailAdress1Update_SyncWithEhandel(plugindata _data)
        {
            if (_data.Target.Contains("emailaddress1") || _data.Target.Contains("statuscode"))
            {
                // Sync with e-handel. 
                CustomerHandler.ExecuteCustomerSyncronization(_data.Service, _data.Target.Id.ToString());
            }
        }

        private static void HandleFullNameUpdate_UpdateQueueItems(plugindata _data)
        {
            if (_data.Target.Contains("fullname"))
            {
                string contactId = _data.Target.Id.ToString(),
                    contactFullName = _data.Target["fullname"].ToString();

                UpdateQueueItems(contactId, contactFullName, _data.Service);
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
            /// Query gets:
            /// 1. All active queueitems
            /// 2. whos activie incidents
            /// 3. has the attribute cgi_contactid of value contactId
            string fetchXML =
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

            FetchExpression query = new FetchExpression(fetchXML);
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

            for (int i = 0; i < records.Entities.Count; i++)
            {
                Entity queueItemToUpdate = new Entity("queueitem");
                queueItemToUpdate.Id = records.Entities[i].Id;
                queueItemToUpdate["cgi_customer"] = fullName;
                //service.Update(queueItemToUpdate);
                Microsoft.Xrm.Sdk.Messages.UpdateRequest updateRequest = new Microsoft.Xrm.Sdk.Messages.UpdateRequest { Target = queueItemToUpdate };
                requestWithResults.Requests.Add(updateRequest);
            }

            // Using this multiple update for speed, if in the future, we have cases that are in many queues at once.
            Microsoft.Xrm.Sdk.Messages.ExecuteMultipleResponse responseWithResults = (Microsoft.Xrm.Sdk.Messages.ExecuteMultipleResponse)service.Execute(requestWithResults);

            // TODO handle error if we should
        }

    }
}
