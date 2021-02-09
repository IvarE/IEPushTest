using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;

using Microsoft.Crm.Sdk.Messages;
using CGI.CRM2013.Skanetrafiken.IncidentPlugin.Common;


namespace CGI.CRM2013.Skanetrafiken.IncidentPlugin
{
    public class OnUpdateSendEmail_Post : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                    _data.InitPostImage("postimage");
                    _sendEmail(_data);
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
           
            }
        }
        #endregion

        #region Private Methods
        private void _sendEmail(PluginData data)
        {
            if (data.Target.Attributes.Contains("cgi_sendmailaction"))
            {
                string _action = data.Target.Attributes["cgi_sendmailaction"].ToString();
                if (!string.IsNullOrEmpty(_action))
                {
                    if (_action == "attension")
                        _sendAttentionMail(data);
                    else if (_action == "remittance")
                        _sendRemittancemail();

                    data.Target.Attributes["cgi_sendmailaction"] = "";
                    data.Service.Update(data.Target);
                }
            }
        }

        private void _sendAttentionMail(PluginData data)
        {
            // Email record id
            Guid wod_EmailId = Guid.Empty;

            // Creating Email 'from' recipient activity party entity object
            Entity wod_EmailFromReciepent = new Entity("activityparty");

            // from queue
            Guid _queueid = _getDefaultSendingQueue(data);
            wod_EmailFromReciepent["partyid"] = new EntityReference("queue", _queueid);

            // Creating Email entity object 
            Entity wod_EmailEntity = new Entity("email");

            // Creating Email 'to' recipient activity party entity object
            Entity wod_EmailToReciepent = new Entity("activityparty");
            // Assigning receiver email address to activity party addressused attribute
            wod_EmailToReciepent["addressused"] = "reine.rosqvist@telia.com";   // prmToRecipientEmailAddress;

            // Setting email entity 'to' attribute value
            wod_EmailEntity["to"] = new Entity[] { wod_EmailToReciepent };

            // Setting email entity 'from' attribute value
            wod_EmailEntity["from"] = new Entity[] { wod_EmailFromReciepent };
        }

        private void _sendRemittancemail()
        {

        }

        private Guid _getDefaultSendingQueue(PluginData data)
        {
            Guid _returnvalue = Guid.Empty;

            //SKA_OUTGOING_FROM_CASE
            QueryByAttribute _query = new QueryByAttribute("queue");
            _query.ColumnSet = new ColumnSet("queueid");
            _query.Attributes.Add("name");
            _query.Values.Add("SKA_OUTGOING_FROM_CASE");
            EntityCollection _queues = data.Service.RetrieveMultiple(_query);
            if (_queues != null && _queues.Entities.Count() > 0)
            {
                string _value = _queues.Entities[0].Attributes["queueid"].ToString();
                _returnvalue = new Guid(_value);
            }

            return _returnvalue;
        }

        private Guid _getDefaultEmailTemplate(PluginData data)
        {
            Guid _returnvalue = Guid.Empty;

            //SKA_OUTGOING_FROM_CASE
            QueryByAttribute _query = new QueryByAttribute("template");
            _query.ColumnSet = new ColumnSet("templateid");
            _query.Attributes.Add("title");
            _query.Values.Add("DEFAULT_EMAIL_TEMPLATE");
            EntityCollection _queues = data.Service.RetrieveMultiple(_query);
            if (_queues != null && _queues.Entities.Count() > 0)
            {
                string _value = _queues.Entities[0].Attributes["templateid"].ToString();
                _returnvalue = new Guid(_value);
            }

            return _returnvalue;
        }
        #endregion
    }
}
