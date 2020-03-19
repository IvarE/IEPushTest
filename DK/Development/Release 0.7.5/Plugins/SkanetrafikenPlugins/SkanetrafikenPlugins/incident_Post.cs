using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using System.ServiceModel;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Post : IPlugin
    {
        private class plugindata : PlugindataBase
        {
            public plugindata(IServiceProvider serviceProvider) : base(serviceProvider) { }
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            plugindata _data = new plugindata(serviceProvider);
            
            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                }

                //_data.InitPostImage("postimage");

                //Update queueitem
                _updateQueueItem(_data);

                if (_data.Target.Attributes.Contains("cgi_casesolved"))
                {
                    string _casesolved = _data.Target.Attributes["cgi_casesolved"].ToString();
                    if (_casesolved == "2")
                    {
                        _data.Target.Attributes["cgi_casesolved"] = "0";
                        _data.Service.Update(_data.Target);

                        //check if case has any email that is to be sent.
                        if (_checkEmail(_data) == true)
                        {
                        bool _ok = _resolveCase(_data);
                        if (_ok == false)
                        {
                            throw new InvalidPluginExecutionException("Det gick inte att avsluta ärendet!");
                        }
                    }
                        else
                        {
                            throw new InvalidPluginExecutionException("Det finns ett e-post meddelnade som väntar på att skickas. Vänligen vänta med att avsluta ärendet tills att e-posten har skickats!");
                }
                    }
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

        private bool _checkEmail(plugindata data)
        {
            bool _returnValue = true;

            try
            {
                /*

                <fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">
                <entity name="activitypointer">
                <attribute name="activitytypecode" />
                <attribute name="subject" />
                <attribute name="statecode" />
                <attribute name="prioritycode" />
                <attribute name="modifiedon" />
                <attribute name="activityid" />
                <attribute name="instancetypecode" />
                <attribute name="community" />
                <order attribute="modifiedon" descending="false" />
                <filter type="and">
                  <condition attribute="activitytypecode" operator="eq" value="4202" />
                  <condition attribute="regardingobjectid" operator="eq" uiname="" uitype="incident" value="{73BE1CA8-87BD-E411-80D7-005056903A38}" />
                </filter>
                </entity>
                </fetch>
                 
                */

                // a.statuscode,
                // a.statuscodename,
                // a.statecode,
                // a.statecodename

                QueryByAttribute _query = new QueryByAttribute("activitypointer");
                _query.ColumnSet = new ColumnSet("statuscode", "statecode");
                _query.Attributes.Add("activitytypecode");
                _query.Values.Add(4202);
                _query.Attributes.Add("regardingobjectid");
                _query.Values.Add(data.Target.Id);
                EntityCollection _emails = data.Service.RetrieveMultiple(_query);
                if (_emails != null && _emails.Entities.Count() > 0)
                {
                    foreach (Entity _ent in _emails.Entities)
                    {
                        if (_ent.Attributes.Contains("statuscode"))
                        {
                            OptionSetValue _o = _ent.Attributes["statuscode"] as OptionSetValue;
                            if (_o.Value == 7)
                            {
                                _returnValue = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    _returnValue = true;
        }

            }
            catch (Exception ex)
            {
                throw;
            }

            return _returnValue;
        }

        private bool _resolveCase(plugindata data)
        {
            bool _returnValue = false;
            try
            {
                Guid _incidentid = data.Target.Id;

                Entity _caseresolution = new Entity("incidentresolution");
                _caseresolution.Attributes.Add("incidentid", new EntityReference("incident", _incidentid));
                _caseresolution.Attributes.Add("subject", "Problemet löst");

                CloseIncidentRequest _request = new CloseIncidentRequest();
                _request.IncidentResolution = _caseresolution;
                _request.RequestName = "CloseIncident";
                OptionSetValue _o = new Microsoft.Xrm.Sdk.OptionSetValue();
                _o.Value = 5;
                _request.Status = _o;

                CloseIncidentResponse _response = (CloseIncidentResponse)data.Service.Execute(_request);
                _returnValue = true;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
            return _returnValue;
        }

        // WARNING: this almost the same code also exist in queueitem_Post, there attributes are fetched.
        // TODO: refactor to common
        private void _updateQueueItem(plugindata data)
        {
            try
            {
                var q = new QueryByAttribute("queueitem") { ColumnSet = new ColumnSet("objectid") };
                q.AddAttributeValue("objectid", data.Target.Id);
                q.AddAttributeValue("statecode", 0);

                var queueitems = data.Service.RetrieveMultiple(q);

                if (queueitems == null || queueitems.Entities == null || queueitems.Entities.Count <= 0) return;

                traceEntity(data.TracingService, data.Target);

                foreach (var queueitem in queueitems.Entities)
                {
                    foreach (var a in data.Target.Attributes)
                    {
                        OptionSetValue o;
                        EntityReference reference;
                        ColumnSet _c;
                        Entity _a;
                        switch (a.Key)
                        {
                            case "cgi_casdet_row1_cat3id":
                                reference = data.Target.Attributes["cgi_casdet_row1_cat3id"] as EntityReference;
                                _c = new ColumnSet(true);
                                _a = data.Service.Retrieve(reference.LogicalName, reference.Id, _c);
                                string _name = string.Empty;
                                if (_a.Attributes.Contains("cgi_categorydetailname"))
                                    _name = _a.Attributes["cgi_categorydetailname"].ToString();

                                queueitem.Attributes["cgi_casdet_row1_cat3"] = _name;
                                break;
                            case "caseorigincode":
                                o = a.Value as OptionSetValue;
                                queueitem.Attributes["cgi_caseorigincode"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                                break;
                            case "ticketnumber":
                                queueitem.Attributes["cgi_ticketnumber"] = a.Value;
                                break;
                            case "cgi_arrival_date":
                                queueitem.Attributes["cgi_arrival_date"] = a.Value;
                                break;
                            case "cgi_actiondate":
                                queueitem.Attributes["cgi_action_date"] = _convertDate(a.Value.ToString());
                                break;
                            case "cgi_case_remittance":
                                o = a.Value as OptionSetValue;
                                queueitem.Attributes["cgi_case_remittance"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                                break;
                            case "casetypecode":
                                o = a.Value as OptionSetValue;
                                queueitem.Attributes["cgi_case_type"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                                break;
                            case "customerid":
                                //case "cgi_full_name":
                                //MaxP 20150318
                                reference = data.Target.Attributes["customerid"] as EntityReference;
                                _c = new ColumnSet(true);
                                _a = data.Service.Retrieve(reference.LogicalName, reference.Id, _c);
                                _name = "aaa";
                                if (_a.Attributes.Contains("fullname"))
                                    _name = _a.Attributes["fullname"].ToString();

                                if (_a.Attributes.Contains("name"))
                                    _name = _a.Attributes["name"].ToString();

                                queueitem.Attributes["cgi_customer"] = _name;
                                break;
                            case "incidentstagecode":
                                o = a.Value as OptionSetValue;
                                queueitem.Attributes["cgi_incidentstagecode"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                                break;
                            case "prioritycode":
                                o = a.Value as OptionSetValue;
                                queueitem.Attributes["cgi_priority"] = GetOptionSetValueLabel("incident", a.Key, o.Value, 1053, data);
                                break;
                            case "resolveby":
                                queueitem.Attributes["cgi_resolve_by"] = _convertDate(a.Value.ToString());
                                break;
                            case "cgi_customer_number":
                                queueitem.Attributes["cgi_customer_number"] = a.Value as String;
                                break;
                            case "cgi_customer_email":
                                queueitem.Attributes["cgi_customer_email"] = a.Value as String;
                                break;
                            case "cgi_soc_sec_number":
                                queueitem.Attributes["cgi_soc_sec_number"] = a.Value as String;
                                break;
                            case "cgi_customer_telephonenumber":
                                queueitem.Attributes["cgi_customer_telephonenumber"] = a.Value as String;
                                break;
                            case "cgi_customer_telephonenumber_work":
                                queueitem.Attributes["cgi_customer_telephonenumber_work"] = a.Value as String;
                                break;
                            case "cgi_customer_telephonenumber_mobile":
                                queueitem.Attributes["cgi_customer_telephonenumber_mobile"] = a.Value as String;
                                break;
                            case "ownerid":

                                // SPECIFICATION:
                                // check if owner is systemuser or team
                                // if systemuser, update queue post worked by to username
                                // if team, update queue post worked by to null

                                EntityReference owner = a.Value as EntityReference;

                                if (owner.LogicalName.ToUpperInvariant() == "TEAM")
                                {
                                    queueitem.Attributes["workerid"] = null;
                                }
                                else if (owner.LogicalName.ToUpperInvariant() == "SYSTEMUSER")
                                {
                                    queueitem.Attributes["workerid"] = a.Value;
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    traceEntity(data.TracingService, queueitem);
                    // throw new InvalidPluginExecutionException("Intentional!");
                    data.Service.Update(queueitem);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Helper code

        private string _convertDate(string date)
        {
            string _returnvalue = "";
            
            try
            {
                DateTime _date;
                DateTime.TryParse(date, out _date);
                string _year = _date.Year.ToString("0000");
                string _month = _date.Month.ToString("00");
                string _day = _date.Day.ToString("00");
                _returnvalue = string.Format("{0}-{1}-{2}", _year, _month, _day);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return _returnvalue;
        }

        // MOSCGI 2015-03-09
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
        // END

        //This method will return the label for the specified LCID
        private string GetOptionSetValueLabel(string entityName, string fieldName, int optionSetValue, int lcid, plugindata data)
        {
            var attReq = new RetrieveAttributeRequest();
            attReq.EntityLogicalName = entityName;
            attReq.LogicalName = fieldName;
            attReq.RetrieveAsIfPublished = true;

            var attResponse = (RetrieveAttributeResponse)data.Service.Execute(attReq);
            var attMetadata = (EnumAttributeMetadata)attResponse.AttributeMetadata;

            // return attMetadata.OptionSet.Options.Where(x => x.Value == optionSetValue).FirstOrDefault().Label.LocalizedLabels.Where(l => l.LanguageCode == lcid).FirstOrDefault().Label;
            // MOSCGI 2015-03-09
            var optionMetadata = attMetadata.OptionSet.Options.FirstOrDefault(x => x.Value == optionSetValue);
            if (optionMetadata == null) return "missing metadata";
            var localized = optionMetadata.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
            return localized != null ? localized.Label : optionMetadata.Label.UserLocalizedLabel.Label;
            // MOSCGI 2015-03-09
        }        

    }
}
