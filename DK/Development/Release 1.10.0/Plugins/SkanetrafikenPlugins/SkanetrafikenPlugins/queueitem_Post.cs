using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class queueitem_Post : IPlugin
    {
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            var data = new PluginData(serviceProvider);

            try
            {
                if (data.Context.InputParameters.Contains("Target") && data.Context.InputParameters["Target"] is Entity)
                {
                    data.Target = (Entity)data.Context.InputParameters["Target"];
                }

                _updateQueueItem(data);

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private static Entity GetEntity(PluginData data, Guid objectId)
        {
            var fetchXml = "";
            fetchXml += "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>";
            fetchXml += "  <entity name='incident'>";
            fetchXml += "    <attribute name='incidentid' />";
            fetchXml += "    <attribute name='cgi_actiondate' />";
            fetchXml += "    <attribute name='cgi_case_remittance' />";
            fetchXml += "    <attribute name='casetypecode' />";
            fetchXml += "    <attribute name='customerid' />";
            fetchXml += "    <attribute name='cgi_full_name' />";
            fetchXml += "    <attribute name='incidentstagecode' />";
            fetchXml += "    <attribute name='prioritycode' />";
            fetchXml += "    <attribute name='resolveby' />";
            fetchXml += "    <attribute name='cgi_customer_number' />";
            fetchXml += "    <attribute name='cgi_customer_email' />";
            fetchXml += "    <attribute name='cgi_soc_sec_number' />";
            fetchXml += "    <attribute name='cgi_customer_telephonenumber' />";
            fetchXml += "    <attribute name='cgi_customer_telephonenumber_mobile' />";
            fetchXml += "    <attribute name='cgi_casdet_row1_cat3id' />";
            fetchXml += "    <attribute name='caseorigincode' />";
            fetchXml += "    <attribute name='ticketnumber' />";
            fetchXml += "    <attribute name='cgi_arrival_date' />";
            fetchXml += "    <attribute name='ownerid' />";            
            fetchXml += "    <link-entity name='queueitem' from='objectid' to='incidentid' alias='ab'>";
            fetchXml += "      <filter type='and'>";
            fetchXml += "        <condition attribute='queueitemid' operator='eq' value='" + objectId + "' />";
            fetchXml += "      </filter>";
            fetchXml += "    </link-entity>";
            fetchXml += "  </entity>";
            fetchXml += "</fetch>";
            var query = new FetchExpression(fetchXml);
            var records = data.Service.RetrieveMultiple(query);

            if (records.Entities == null || records.Entities.Count <= 0) return null;
            // only one case per queueitem
            traceEntity(data.TracingService, records.Entities[0]);
            return records.Entities[0];
        }

        // WARNING: this almost the same code also exist in incident_Post
        // TODO: refactor to common
        private void _updateQueueItem(PluginData data)
        {
            try
            {
                var objectId = data.Target.Id;
                var queueItem = new Entity("queueitem") { Id = objectId };

                var incident = GetEntity(data, objectId);

                if (incident == null) return;

                foreach (var a in incident.Attributes)
                {
                    OptionSetValue o;
                    EntityReference reference;
                    ColumnSet c;
                    Entity incidentEntity;
                    string name;
                    switch (a.Key)
                    {
                        case "cgi_casdet_row1_cat3id":
                            reference = incident["cgi_casdet_row1_cat3id"] as EntityReference;
                            if (reference != null)
                            {
                                c = new ColumnSet(true);
                                incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                name = string.Empty;
                                if (incidentEntity.Attributes.Contains("cgi_categorydetailname"))
                                    name = incidentEntity.Attributes["cgi_categorydetailname"].ToString();

                                queueItem.Attributes["cgi_casdet_row1_cat3"] = name;
                            }
                            break;
                        case "caseorigincode":
                            o = a.Value as OptionSetValue;
                            if (o != null)
                            {
                                queueItem.Attributes["cgi_caseorigincode"] = GetOptionSetValueLabel("incident", a.Key,
                                    o.Value, 1053, data);
                            }
                            break;
                        case "ticketnumber":
                            queueItem.Attributes["cgi_ticketnumber"] = a.Value;
                            break;
                        case "cgi_arrival_date":
                            queueItem.Attributes["cgi_arrival_date"] = a.Value;
                            break;
                        case "cgi_actiondate":
                            queueItem.Attributes["cgi_action_date"] = _convertDate(a.Value.ToString());
                            break;
                        case "cgi_case_remittance":
                            o = a.Value as OptionSetValue;
                            if (o != null)
                            {
                                queueItem.Attributes["cgi_case_remittance"] = GetOptionSetValueLabel("incident", a.Key,
                                    o.Value, 1053, data);
                            }
                            break;
                        case "casetypecode":
                            o = a.Value as OptionSetValue;
                            if (o != null)
                            {
                                queueItem.Attributes["cgi_case_type"] = GetOptionSetValueLabel("incident", a.Key,
                                    o.Value, 1053, data);
                            }
                            break;
                        //MaxP 2015-03-18
                        case "customerid":
                            reference = incident["customerid"] as EntityReference;
                            c = new ColumnSet(true);
                            if (reference != null)
                            {
                                incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                name = "aaa";
                                if (incidentEntity.Attributes.Contains("fullname"))
                                    name = incidentEntity.Attributes["fullname"].ToString();

                                if (incidentEntity.Attributes.Contains("name"))
                                    name = incidentEntity.Attributes["name"].ToString();

                                queueItem.Attributes["cgi_customer"] = name;
                            }
                            break;
                        case "incidentstagecode":
                            o = a.Value as OptionSetValue;
                            if (o != null)
                            {
                                queueItem.Attributes["cgi_incidentstagecode"] = GetOptionSetValueLabel("incident", a.Key,
                                    o.Value, 1053, data);
                            }
                            break;
                        case "prioritycode":
                            o = a.Value as OptionSetValue;
                            if (o != null)
                            {
                                queueItem.Attributes["cgi_priority"] = GetOptionSetValueLabel("incident", a.Key, o.Value,
                                    1053, data);
                            }
                            break;
                        case "resolveby":
                            queueItem.Attributes["cgi_resolve_by"] = _convertDate(a.Value.ToString());
                            break;
                        case "cgi_customer_number":
                            queueItem.Attributes["cgi_customer_number"] = a.Value as String;
                            break;
                        case "cgi_customer_email":
                            queueItem.Attributes["cgi_customer_email"] = a.Value as String;
                            break;
                        case "cgi_soc_sec_number":
                            queueItem.Attributes["cgi_soc_sec_number"] = a.Value as String;
                            break;
                        case "cgi_customer_telephonenumber":
                            queueItem.Attributes["cgi_customer_telephonenumber"] = a.Value as String;
                            break;
                        case "cgi_customer_telephonenumber_work":
                            queueItem.Attributes["cgi_customer_telephonenumber_work"] = a.Value as String;
                            break;
                        case "cgi_customer_telephonenumber_mobile":
                            queueItem.Attributes["cgi_customer_telephonenumber_mobile"] = a.Value as String;
                            break;
                        case "ownerid":
                            
                        // SPECIFICATION:
                            // check if owner is systemuser or team
                            // if systemuser, update queue post worked by to username
                            // if team, update queue post worked by to null

                            EntityReference owner = a.Value as EntityReference;
                            if (owner != null)
                            {
                                if (owner.LogicalName.ToUpperInvariant() == "TEAM")
                                {
                                    queueItem.Attributes["workerid"] = null;
                                }
                                else if (owner.LogicalName.ToUpperInvariant() == "SYSTEMUSER")
                                {
                                    c = new ColumnSet("cgi_noqueueassign");
                                    incidentEntity = data.Service.Retrieve(owner.LogicalName, owner.Id, c);

                                    if (incidentEntity.Attributes.Contains("cgi_noqueueassign"))
                                        if (incidentEntity.Attributes["cgi_noqueueassign"].ToString() == "1")
                                            queueItem.Attributes["workerid"] = null;
                                        else
                                            queueItem.Attributes["workerid"] = a.Value;
                                    else
                                        queueItem.Attributes["workerid"] = a.Value;
                                }
                            }
                            break;
                    }
                }
                traceEntity(data.TracingService, queueItem);
                data.Service.Update(queueItem);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }

        private static void traceEntity(ITracingService tracingService, Entity entity)
        {
            tracingService.Trace("");
            tracingService.Trace("{0}: {1}", entity.LogicalName, entity.Id);
            tracingService.Trace("------------------------------------------------------------");

            foreach (var a in entity.Attributes)
            {
                var optionSetValue = a.Value as OptionSetValue;
                if (optionSetValue != null)
                {
                    tracingService.Trace("{0}: {1}", a.Key, optionSetValue.Value);
                }
                else if (a.Value is EntityReference)
                {
                    tracingService.Trace("{0}: {1}; {2}; {3}", a.Key, ((EntityReference)a.Value).Id,
                        ((EntityReference)a.Value).LogicalName, ((EntityReference)a.Value).Name);
                }
                else
                {
                    tracingService.Trace("{0}: {1}", a.Key, a.Value);
                }
            }
            tracingService.Trace("");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Helper code

        private string _convertDate(string date)
        {
            string returnvalue;

            try
            {
                DateTime dateTime;
                DateTime.TryParse(date, out dateTime);
                var year = dateTime.Year.ToString("0000");
                var month = dateTime.Month.ToString("00");
                var day = dateTime.Day.ToString("00");
                returnvalue = string.Format("{0}-{1}-{2}", year, month, day);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return returnvalue;
        }

        //This method will return the label for the specified LCID
        private string GetOptionSetValueLabel(string entityName, string fieldName, int optionSetValue, int lcid, PluginData data)
        {
            var attReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = fieldName,
                RetrieveAsIfPublished = true
            };

            var attResponse = (RetrieveAttributeResponse)data.Service.Execute(attReq);
            var attMetadata = (EnumAttributeMetadata)attResponse.AttributeMetadata;

            var optionMetadata = attMetadata.OptionSet.Options.FirstOrDefault(x => x.Value == optionSetValue);
            if (optionMetadata == null) return "missing metadata";
            var localized = optionMetadata.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
            return localized != null ? localized.Label : optionMetadata.Label.UserLocalizedLabel.Label;
        }
        #endregion
    }
}
