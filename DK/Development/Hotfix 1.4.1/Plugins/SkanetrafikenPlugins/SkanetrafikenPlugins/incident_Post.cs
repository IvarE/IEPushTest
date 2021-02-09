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
        #region Public Methods
        public void Execute(IServiceProvider serviceProvider)
        {
            PluginData _data = new PluginData(serviceProvider);

            try
            {
                if (_data.Context.InputParameters.Contains("Target") && _data.Context.InputParameters["Target"] is Entity)
                {
                    _data.Target = (Entity)_data.Context.InputParameters["Target"];
                    _data.InitPreImage("preImage");
                    _data.InitPostImage("postImage");

                    /* 160129 - Ny kod för att tilldela kö på återöppnat till användarens standardkö START*/
                    if (_data.Target.Attributes.Contains("statuscode") && _data.Target.Attributes.Contains("modifiedby")) // TODO -kan hantera utan statuscode, för 1 2 3.
                    {
                        Entity preIncidentImage = (Entity)_data.PreImage;
                        Entity postIncidentImage = (Entity)_data.PostImage;

                        if (preIncidentImage.Attributes.Contains("statuscode") && postIncidentImage.Attributes.Contains("statuscode"))
                        {
                            int statusCodePreImageValue = ((OptionSetValue)preIncidentImage.Attributes["statuscode"]).Value;
                            int statusCodePostImageValue = ((OptionSetValue)postIncidentImage.Attributes["statuscode"]).Value;

                            // In Progress  1
                            // Resolved     5
                            if (statusCodePreImageValue == 5 && statusCodePostImageValue == 1)
                            {
                                // If executed from form these values will be null and 1, ie its a crm user working with the form that has resolved the case
                                bool isCurrentPluginExecutedByPluginOrRealTimeWorkFlow = _data.Context.ParentContext != null && _data.Context.Depth >= 2;

                                if (isCurrentPluginExecutedByPluginOrRealTimeWorkFlow == false)
                                {
                                    HandleDefaultBehaviourOnReopen(_data); // User reactivated case from form
                                }
                                else
                                {
                                    HandleOptionalBehaviorOnReopen(_data); // Case was reactivated from other plugin or realtime workflow, ie in this case a incoming email that triggered a rt workflow
                                }
                            }
                        }
                    }
                    /* 160129 - Ny kod för att tilldela kö på återöppnat till användarens standardkö SLUT*/

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
        private static void HandleDefaultBehaviourOnReopen(PluginData _data)
        {
            var _modifiedbyref = _data.Target.Attributes["modifiedby"] as EntityReference;

            //assign the case/incident to the user reopening the case
            AssignRequest IncidentAssignReq = new AssignRequest
            {
                Assignee = _modifiedbyref,
                Target = _data.Target.ToEntityReference()

            };
            _data.Service.Execute(IncidentAssignReq);

            var _c = new ColumnSet(true);
            var _modifiedby = _data.Service.Retrieve(_modifiedbyref.LogicalName, _modifiedbyref.Id, _c);

            if (_modifiedby.Attributes.Contains("queueid"))
            {
                Guid _queueid = ((EntityReference)_modifiedby.Attributes["queueid"]).Id;

                AddToQueueRequest addToSourceQueue = new AddToQueueRequest
                {
                    DestinationQueueId = _queueid,
                    Target = new EntityReference(_data.Target.LogicalName, _data.Target.Id)
                };

                _data.Service.Execute(addToSourceQueue);
            }
        }

        private static void HandleOptionalBehaviorOnReopen(PluginData _data)
        {
            try
            {
                // Activate queueitem so that case once agains returns to 'previoius' queueS
                string incidentId = _data.Target.Id.ToString();

                // statecode    0  active. 1   inactive // statuscode   1   active. 2   inactive
                QueryByAttribute query = Common.Common.CreateQueryAttribute("queueitem", new string[] { "queueitemid", "statuscode", "statecode" }, new string[] { "objectid", "statecode", "statuscode" }, new object[] { incidentId, 1, 2 });
                EntityCollection result = _data.Service.RetrieveMultiple(query);

                if (Common.Common.EntityCollectionHasItems(result))
                {
                    for (int i = 0; i < result.Entities.Count; i++)
                    {
                        if (i > 20) break;// guard condition for safety. case will never be in more then 20 queues..
                        Entity queueItem = (Entity)result.Entities[i];
                        SetStateRequest stateRequest = Common.Common.CreateStateRequest(queueItem, 0, 1);
                        _data.Service.Execute(stateRequest);
                    }
                }
            }
            catch
            {
                // Do nothing. Could write to internal CRM Application Log.
            }
        }

        private bool _checkEmail(PluginData data)
        {
            bool _returnValue = true;

            try
            {
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
            catch (Exception)
            {
                throw;
            }

            return _returnValue;
        }

        private bool _resolveCase(PluginData data)
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
        private void _updateQueueItem(PluginData data)
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
                                    _c = new ColumnSet("cgi_noqueueassign");
                                    _a = data.Service.Retrieve(owner.LogicalName, owner.Id, _c);

                                    if (_a.Attributes.Contains("cgi_noqueueassign"))
                                        if (_a.Attributes["cgi_noqueueassign"].ToString() == "1")
                                            queueitem.Attributes["workerid"] = null;
                                        else
                                            queueitem.Attributes["workerid"] = a.Value;
                                    else
                                        queueitem.Attributes["workerid"] = a.Value;
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    traceEntity(data.TracingService, queueitem);
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
        private string GetOptionSetValueLabel(string entityName, string fieldName, int optionSetValue, int lcid, PluginData data)
        {
            var attReq = new RetrieveAttributeRequest();
            attReq.EntityLogicalName = entityName;
            attReq.LogicalName = fieldName;
            attReq.RetrieveAsIfPublished = true;

            var attResponse = (RetrieveAttributeResponse)data.Service.Execute(attReq);
            var attMetadata = (EnumAttributeMetadata)attResponse.AttributeMetadata;

            // MOSCGI 2015-03-09
            var optionMetadata = attMetadata.OptionSet.Options.FirstOrDefault(x => x.Value == optionSetValue);
            if (optionMetadata == null) return "missing metadata";
            var localized = optionMetadata.Label.LocalizedLabels.FirstOrDefault(l => l.LanguageCode == lcid);
            return localized != null ? localized.Label : optionMetadata.Label.UserLocalizedLabel.Label;
            // MOSCGI 2015-03-09
        }
        #endregion
    }
}
