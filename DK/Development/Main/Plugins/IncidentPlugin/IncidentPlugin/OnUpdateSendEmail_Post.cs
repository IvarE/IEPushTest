using System;
using System.Linq;
using System.ServiceModel;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace CGI.CRM2013.Skanetrafiken.IncidentPlugin
{
    public class OnUpdateSendEmail_Post : IPlugin
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
                    data.InitPostImage("postimage");
                    _sendEmail(data);
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new FaultException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
           
            }
        }
        #endregion

        #region Private Methods
        private void _sendEmail(PluginData data)
        {
            if (data.Target.Attributes.Contains("cgi_sendmailaction"))
            {
                string action = data.Target.Attributes["cgi_sendmailaction"].ToString();
                if (!string.IsNullOrEmpty(action))
                {
                    if (action == "attension")
                        _sendAttentionMail(data);
                    else if (action == "remittance")
                        _sendRemittancemail();

                    data.Target.Attributes["cgi_sendmailaction"] = "";
                    data.Service.Update(data.Target);
                }
            }
        }

        private void _sendAttentionMail(PluginData data)
        {
            // Creating Email 'from' recipient activity party entity object
            Entity wodEmailFromReciepent = new Entity("activityparty");

            // from queue
            Guid queueid = _getDefaultSendingQueue(data);
            wodEmailFromReciepent["partyid"] = new EntityReference("queue", queueid);

            // Creating Email entity object 
            Entity wodEmailEntity = new Entity("email");

            // Creating Email 'to' recipient activity party entity object
            Entity wodEmailToReciepent = new Entity("activityparty");

            // Assigning receiver email address to activity party addressused attribute
            wodEmailToReciepent["addressused"] = "reine.rosqvist@telia.com";   // prmToRecipientEmailAddress;

            // Setting email entity 'to' attribute value
            wodEmailEntity["to"] = new[] { wodEmailToReciepent };

            // Setting email entity 'from' attribute value
            wodEmailEntity["from"] = new[] { wodEmailFromReciepent };
        }

        private void _sendRemittancemail()
        {

        }

        private Guid _getDefaultSendingQueue(PluginData data)
        {
            Guid returnvalue = Guid.Empty;

            //SKA_OUTGOING_FROM_CASE
            QueryByAttribute query = new QueryByAttribute("queue")
            {
                ColumnSet = new ColumnSet("queueid")
            };
            query.Attributes.Add("name");
            query.Values.Add("SKA_OUTGOING_FROM_CASE");
            EntityCollection queues = data.Service.RetrieveMultiple(query);
            if (queues != null && queues.Entities.Any())
            {
                string value = queues.Entities[0].Attributes["queueid"].ToString();
                returnvalue = new Guid(value);
            }

            return returnvalue;
        }
        #endregion
    }
}
