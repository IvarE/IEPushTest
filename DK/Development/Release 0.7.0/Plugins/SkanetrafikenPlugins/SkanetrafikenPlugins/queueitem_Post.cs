using System;
using System.Linq;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace CRM2013.SkanetrafikenPlugins
{
    public class queueitem_Post : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            var _data = new plugindata(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                }

                _updateQueueItem(_data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private static Entity GetEntity(plugindata data, Guid objectId)
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
        private void _updateQueueItem(plugindata data)
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
                    ColumnSet _c;
                    Entity _a;
                    switch (a.Key)
                    {
                        case "cgi_casdet_row1_cat3id":
                            reference = incident["cgi_casdet_row1_cat3id"] as EntityReference;
                            _c = new ColumnSet(true);
                            _a = data.Service.Retrieve(reference.LogicalName, reference.Id, _c);
                            string _name = string.Empty;
                            if (_a.Attributes.Contains("cgi_categorydetailname"))
                                _name = _a.Attributes["cgi_categorydetailname"].ToString();

                            queueItem.Attributes["cgi_casdet_row1_cat3"] = _name;
                            break;
                        case "caseorigincode":
                            o = a.Value as OptionSetValue;
                            queueItem.Attributes["cgi_caseorigincode"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
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
                            queueItem.Attributes["cgi_case_remittance"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                            break;
                        case "casetypecode":
                            o = a.Value as OptionSetValue;
                            queueItem.Attributes["cgi_case_type"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                            break;
                        //MaxP 2015-03-18
                        case "customerid":
                            reference = incident["customerid"] as EntityReference;
                            _c = new ColumnSet(true);
                            _a = data.Service.Retrieve(reference.LogicalName, reference.Id, _c);
                            _name = "aaa";
                            if (_a.Attributes.Contains("fullname"))
                                _name = _a.Attributes["fullname"].ToString();

                            if (_a.Attributes.Contains("name"))
                                _name = _a.Attributes["name"].ToString();

                            queueItem.Attributes["cgi_customer"] = _name;
                            break;
                        case "incidentstagecode":
                            o = a.Value as OptionSetValue;
                            queueItem.Attributes["cgi_incidentstagecode"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                            break;
                        case "prioritycode":
                            o = a.Value as OptionSetValue;
                            queueItem.Attributes["cgi_priority"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
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

                            if (owner.LogicalName.ToUpperInvariant() == "TEAM")
                            {
                                queueItem.Attributes["workerid"] = null;
                            }
                            else if (owner.LogicalName.ToUpperInvariant() == "SYSTEMUSER")
                            {
                                queueItem.Attributes["workerid"] = a.Value;
                            }

                            break;

                        default:
                            break;
                    }
                }
                traceEntity(data.TracingService, queueItem);
                //throw new InvalidPluginExecutionException("Intentional!");
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
                if (a.Value is OptionSetValue)
                {
                    tracingService.Trace("{0}: {1}", a.Key, ((OptionSetValue)a.Value).Value);
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
            var returnvalue = "";

            try
            {
                DateTime _date;
                DateTime.TryParse(date, out _date);
                var _year = _date.Year.ToString("0000");
                var _month = _date.Month.ToString("00");
                var _day = _date.Day.ToString("00");
                returnvalue = string.Format("{0}-{1}-{2}", _year, _month, _day);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return returnvalue;
        }

        //This method will return the label for the specified LCID
        private string GetOptionSetValueLabel(string entityName, string fieldName, int optionSetValue, int lcid, plugindata data)
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
    }
}
