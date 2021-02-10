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

        private static void HandleDefaultBehaviourOnReopen(plugindata _data)
        {
            try
            {
                var _modifiedbyref = _data.Target.Attributes["modifiedby"] as EntityReference;
                
                _data.TracingService.Trace("_modifiedbyref: " + _modifiedbyref.Id.ToString() + ".");
                
                var _c = new ColumnSet(true);
                var _modifiedby = _data.Service.Retrieve(_modifiedbyref.LogicalName, _modifiedbyref.Id, _c);
                var _modifiedbyname = _modifiedby["fullname"].ToString();
                _data.TracingService.Trace("_modifiedbyname: " + _modifiedbyname + ".");
                bool _privateQueue = false;
                string _queuename = "";
                bool _isMemberOfQueue = false;

                traceEntity(_data.TracingService, _modifiedby);

                if (_modifiedby.Attributes.Contains("queueid"))
                {
                    var _queueref = _modifiedby.Attributes["queueid"] as EntityReference;
                    Guid _queueid = _queueref.Id;

                    var _queue = _data.Service.Retrieve(_queueref.LogicalName, _queueref.Id, _c);
                    traceEntity(_data.TracingService, _queue);
                    _privateQueue = !(_queue.Attributes["queueviewtype"].ToString() == "1");
                    _queuename = _queue.Attributes["name"].ToString();
                    _data.TracingService.Trace("_queuename: " + _queuename + ".");
                    _data.TracingService.Trace("_privateQueue: " + _privateQueue.ToString() + ".");

                    if (_modifiedbyref != null && _queueref != null)
                    {
                        var qm = new QueryByAttribute("queuemembership") { ColumnSet = new ColumnSet("queueid") };
                        qm.AddAttributeValue("queueid", _queueref.Id);
                        qm.AddAttributeValue("systemuserid", _modifiedbyref.Id);

                        var queuemembership = _data.Service.RetrieveMultiple(qm);

                        _isMemberOfQueue = (queuemembership != null && queuemembership.Entities != null && queuemembership.Entities.Count > 0);

                        _data.TracingService.Trace("_isMemberOfQueue: " + _isMemberOfQueue.ToString() + ".");
                    }

                    EntityReference queueowner = _queue.Attributes["ownerid"] as EntityReference;

                    AddToQueueRequest addToSourceQueue = new AddToQueueRequest
                    {
                        DestinationQueueId = _queueid,
                        Target = new EntityReference(_data.Target.LogicalName, _data.Target.Id),
                    };

                    string _cgi_noqueueassign = "";

                    if (_modifiedby.Attributes.Contains("cgi_noqueueassign") && _modifiedby.Attributes["cgi_noqueueassign"] != null)
                        _cgi_noqueueassign = _modifiedby.Attributes["cgi_noqueueassign"].ToString();

                    _data.TracingService.Trace("cgi_noqueueassign: " + _cgi_noqueueassign + ".");               

                    AddToQueueResponse _res = _data.Service.Execute(addToSourceQueue) as AddToQueueResponse;

                    if ((_res != null) && (_res.QueueItemId != null) && (queueowner.LogicalName.ToUpperInvariant() == "TEAM") && (_cgi_noqueueassign != "1"))
                    {
                        if (_privateQueue && _isMemberOfQueue != true)
                            throw new InvalidPluginExecutionException(String.Format("Användaren ({0}) är inte medlem i den privata kön ({1})!", _modifiedbyname, _queuename));

                        Entity _queueitem = new Entity("queueitem");
                        _queueitem.Attributes["queueitemid"] = _res.QueueItemId;
                        _queueitem.Attributes["workerid"] = new EntityReference("systemuser", _modifiedbyref.Id);

                        //addToSourceQueue.QueueItemProperties = _queueitem;
                        _data.Service.Update(_queueitem);
                    }               
                }

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("Message: " + ex.Message + "StackTrace:" + ex.StackTrace);
            }
        }

        private static void HandleOptionalBehaviorOnReopen(plugindata _data)
        {
            try
            {
                // Activate queueitem so that case once agains returns to 'previoius' queueS
                string incidentId = _data.Target.Id.ToString();

                // statecode    0  active. 1   inactive // statuscode   1   active. 2   inactive
                QueryByAttribute query = Common.CreateQueryAttribute("queueitem", new string[] { "queueitemid", "statuscode", "statecode" }, new string[] { "objectid", "statecode", "statuscode" }, new object[] { incidentId, 1, 2 });
                EntityCollection result = _data.Service.RetrieveMultiple(query);

                if (Common.EntityCollectionHasItems(result))
                {
                    for (int i = 0; i < result.Entities.Count; i++)
                    {
                        if (i > 20) break;// guard condition for safety. case will never be in more then 20 queues..
                        Entity queueItem = (Entity)result.Entities[i];
                        SetStateRequest stateRequest = Common.CreateStateRequest(queueItem, 0, 1);
                        _data.Service.Execute(stateRequest);
                    }
                }
            }
            catch
            {
                // Do nothing. Could write to internal CRM Application Log.
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
                var q = new QueryByAttribute("queueitem") { ColumnSet = new ColumnSet("objectid", "queueid") };
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
                            /*case "ownerid":

                                // SPECIFICATION:
                                // check if owner is systemuser or team
                                // if systemuser, update queue post worked by to username
                                // if team, update queue post worked by to null

                                EntityReference owner = a.Value as EntityReference; // incident owner

                                // Lookup queue record
                                reference = queueitem.Attributes["queueid"] as EntityReference;
                                data.TracingService.Trace("queueid: " + reference.Id.ToString() + ".");
                                _c = new ColumnSet(true);
                                _a = data.Service.Retrieve(reference.LogicalName, reference.Id, _c);

                                bool _privateQueue = false;
                                _privateQueue = !(_a.Attributes["queueviewtype"].ToString() == "1");
                                string _queuename = "";
                                _queuename = _a.Attributes["name"].ToString();
                                data.TracingService.Trace("_queuename: " + _queuename + ".");
                                data.TracingService.Trace("_privateQueue: " + _privateQueue.ToString() + ".");

                                traceEntity(data.TracingService, _a);
                                EntityReference queueowner = _a.Attributes["ownerid"] as EntityReference;
                                data.TracingService.Trace("queueowner: " + queueowner.LogicalName.ToUpperInvariant() + ".");

                                traceEntity(data.TracingService, queueitem);

                                if (queueowner.LogicalName.ToUpperInvariant() == "SYSTEMUSER") // Inverted since we don't want workerid on user queues
                                {
                                    queueitem.Attributes["workerid"] = null;
                                }
                                else if (queueowner.LogicalName.ToUpperInvariant() == "TEAM") // Inverted
                                {
                                    _c = new ColumnSet("cgi_noqueueassign");
                                    _a = data.Service.Retrieve(owner.LogicalName, owner.Id, _c);

                                    EntityReference _workerid = null;
                                    string _workername = null;

                                    if (data.Target.Attributes.Contains("modifiedby"))
                                    {
                                        _workerid = data.Target.Attributes["modifiedby"] as EntityReference;
                                        _workername = data.Target.GetAttributeValue<EntityReference>("modifiedby").Name;
                                        data.TracingService.Trace("_workerid: " + _workerid.Id.ToString() + ".");
                                        data.TracingService.Trace("_workername: " + _workername + ".");
                                    }

                                    bool _isMemberOfQueue = false;

                                    if (_workerid != null)
                                    {
                                        var qm = new QueryByAttribute("queuemembership") { ColumnSet = new ColumnSet("queueid") };
                                        qm.AddAttributeValue("systemuserid", _workerid.Id);
                                        var queuemembership = data.Service.RetrieveMultiple(qm);

                                        _isMemberOfQueue = !(queuemembership != null && queuemembership.Entities != null && queuemembership.Entities.Count > 0); 

                                        data.TracingService.Trace("_isMemberOfQueue: " + _isMemberOfQueue.ToString() + ".");
                                    }

                                    if (_a.Attributes.Contains("cgi_noqueueassign"))
                                    {
                                        data.TracingService.Trace("cgi_noqueueassign: " + _a.Attributes["cgi_noqueueassign"].ToString() + ".");

                                        if (_a.Attributes["cgi_noqueueassign"].ToString() == "1")
                                            queueitem.Attributes["workerid"] = null;
                                        else
                                        {
                                            if (_privateQueue && _isMemberOfQueue != true)
                                                throw new InvalidPluginExecutionException(String.Format("Användaren ({0}) är inte medlem i den privata kön ({1})!", _workername, _queuename));

                                            queueitem.Attributes["workerid"] = _workerid; //a.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (_privateQueue && _isMemberOfQueue != true)
                                            throw new InvalidPluginExecutionException(String.Format("Användaren ({0}) är inte medlem i den privata kön ({1})!", _workername, _queuename));

                                        queueitem.Attributes["workerid"] = _workerid;//a.Value;
                                    }
                                }

                                break;
                            */
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
                throw new InvalidPluginExecutionException("Message: " + ex.Message + "StackTrace:" + ex.StackTrace);
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
