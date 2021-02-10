using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;

namespace CRM2013.SkanetrafikenPlugins
{
    public class incident_Post : IPlugin
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
                    data.InitPreImage("preImage");
                    data.InitPostImage("postImage");

                    /* 160129 - Ny kod för att tilldela kö på återöppnat till användarens standardkö START*/
                    if (data.Target.Attributes.Contains("statuscode") && data.Target.Attributes.Contains("modifiedby")) // TODO -kan hantera utan statuscode, för 1 2 3.
                    {
                        Entity preIncidentImage = data.PreImage;
                        Entity postIncidentImage = data.PostImage;

                        if (preIncidentImage.Attributes.Contains("statuscode") && postIncidentImage.Attributes.Contains("statuscode"))
                        {
                            int statusCodePreImageValue = ((OptionSetValue)preIncidentImage.Attributes["statuscode"]).Value;
                            int statusCodePostImageValue = ((OptionSetValue)postIncidentImage.Attributes["statuscode"]).Value;

                            // In Progress  1
                            // Resolved     5
                            if (statusCodePreImageValue == 5 && statusCodePostImageValue == 1)
                            {
                                // If executed from form these values will be null and 1, ie its a crm user working with the form that has resolved the case
                                bool isCurrentPluginExecutedByPluginOrRealTimeWorkFlow = data.Context.ParentContext != null && data.Context.Depth >= 2;

                                if (isCurrentPluginExecutedByPluginOrRealTimeWorkFlow == false)
                                {
                                    HandleDefaultBehaviourOnReopen(data); // User reactivated case from form
                                }
                                else
                                {
                                    HandleOptionalBehaviorOnReopen(data); // Case was reactivated from other plugin or realtime workflow, ie in this case a incoming email that triggered a rt workflow
                                }
                            }
                        }
                    }
                    /* 160129 - Ny kod för att tilldela kö på återöppnat till användarens standardkö SLUT*/

                    _updateQueueItem(data);

                    if (data.Target.Attributes.Contains("cgi_casesolved"))
                    {
                        string casesolved = data.Target.Attributes["cgi_casesolved"].ToString();
                        if (casesolved == "2")
                        {
                            data.Target.Attributes["cgi_casesolved"] = "0";
                            data.Service.Update(data.Target);

                            //check if case has any email that is to be sent.
                            if (_checkEmail(data))
                            {
                                bool ok = _resolveCase(data);
                                if (ok == false)
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
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
        #endregion

        #region Private Methods
        private static void HandleDefaultBehaviourOnReopen(PluginData data)
        {
            var modifiedbyref = data.Target.Attributes["modifiedby"] as EntityReference;

            //assign the case/incident to the user reopening the case
            AssignRequest IncidentAssignReq = new AssignRequest
            {
                Assignee = modifiedbyref,
                Target = data.Target.ToEntityReference()

            };
            data.Service.Execute(IncidentAssignReq);

            var _c = new ColumnSet(true);
            var modifiedby = data.Service.Retrieve(modifiedbyref.LogicalName, modifiedbyref.Id, _c);

            if (modifiedby.Attributes.Contains("queueid"))
            {
                Guid queueid = ((EntityReference)modifiedby.Attributes["queueid"]).Id;

                AddToQueueRequest addToSourceQueue = new AddToQueueRequest
                {
                    DestinationQueueId = queueid,
                    Target = new EntityReference(data.Target.LogicalName, data.Target.Id)
                };

                data.Service.Execute(addToSourceQueue);
            }
        }

        private static void HandleOptionalBehaviorOnReopen(PluginData data)
        {
            try
            {
                // Activate queueitem so that case once agains returns to 'previoius' queueS
                string incidentId = data.Target.Id.ToString();

                // statecode    0  active. 1   inactive // statuscode   1   active. 2   inactive
                QueryByAttribute query = Common.Common.CreateQueryAttribute("queueitem", new[] { "queueitemid", "statuscode", "statecode" }, new[] { "objectid", "statecode", "statuscode" }, new object[] { incidentId, 1, 2 });
                EntityCollection result = data.Service.RetrieveMultiple(query);

                if (Common.Common.EntityCollectionHasItems(result))
                {
                    for (int i = 0; i < result.Entities.Count; i++)
                    {
                        if (i > 20) break;// guard condition for safety. case will never be in more then 20 queues..
                        Entity queueItem = result.Entities[i];
                        SetStateRequest stateRequest = Common.Common.CreateStateRequest(queueItem, 0, 1);
                        data.Service.Execute(stateRequest);
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
            bool returnValue = true;

            try
            {
                QueryByAttribute query = new QueryByAttribute("activitypointer")
                {
                    ColumnSet = new ColumnSet("statuscode", "statecode")
                };
                query.Attributes.Add("activitytypecode");
                query.Values.Add(4202);
                query.Attributes.Add("regardingobjectid");
                query.Values.Add(data.Target.Id);
                EntityCollection emails = data.Service.RetrieveMultiple(query);
                if (emails != null && emails.Entities.Any())
                {
                    foreach (Entity ent in emails.Entities)
                    {
                        if (ent.Attributes.Contains("statuscode"))
                        {
                            OptionSetValue o = ent.Attributes["statuscode"] as OptionSetValue;
                            if (o != null && o.Value == 7)
                            {
                                returnValue = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return returnValue;
        }

        private bool _resolveCase(PluginData data)
        {
            try
            {
                Guid incidentid = data.Target.Id;

                Entity caseresolution = new Entity("incidentresolution");
                caseresolution.Attributes.Add("incidentid", new EntityReference("incident", incidentid));
                caseresolution.Attributes.Add("subject", "Problemet löst");

                CloseIncidentRequest request = new CloseIncidentRequest
                {
                    IncidentResolution = caseresolution,
                    RequestName = "CloseIncident"
                };
                OptionSetValue o = new OptionSetValue
                {
                    Value = 5
                };
                request.Status = o;

                data.Service.Execute(request);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
            return true;
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
                        ColumnSet c;
                        Entity incidentEntity;
                        string name;
                        switch (a.Key)
                        {
                            case "cgi_casdet_row1_cat3id":
                                reference = data.Target.Attributes["cgi_casdet_row1_cat3id"] as EntityReference;
                                if (reference != null)
                                {
                                    c = new ColumnSet(true);
                                    incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                    name = string.Empty;
                                    if (incidentEntity.Attributes.Contains("cgi_categorydetailname"))
                                        name = incidentEntity.Attributes["cgi_categorydetailname"].ToString();

                                    queueitem.Attributes["cgi_casdet_row1_cat3"] = name;
                                }
                                break;
                            case "caseorigincode":
                                o = a.Value as OptionSetValue;
                                if (o != null)
                                {
                                    queueitem.Attributes["cgi_caseorigincode"] = GetOptionSetValueLabel("incident",
                                        a.Key,
                                        o.Value, 1053, data);
                                }
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
                                if (o != null)
                                {
                                    queueitem.Attributes["cgi_case_remittance"] = GetOptionSetValueLabel("incident",
                                        a.Key,
                                        o.Value, 1053, data);
                                }
                                break;
                            case "casetypecode":
                                o = a.Value as OptionSetValue;
                                if (o != null)
                                {
                                    queueitem.Attributes["cgi_case_type"] = GetOptionSetValueLabel("incident", a.Key,
                                        o.Value, 1053, data);
                                }
                                break;
                            case "customerid":
                                //case "cgi_full_name":
                                //MaxP 20150318
                                reference = data.Target.Attributes["customerid"] as EntityReference;
                                if (reference != null)
                                {
                                    c = new ColumnSet(true);
                                    incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                    name = "aaa";
                                    if (incidentEntity.Attributes.Contains("fullname"))
                                        name = incidentEntity.Attributes["fullname"].ToString();

                                    if (incidentEntity.Attributes.Contains("name"))
                                        name = incidentEntity.Attributes["name"].ToString();

                                    queueitem.Attributes["cgi_customer"] = name;
                                }
                                break;
                            case "incidentstagecode":
                                o = a.Value as OptionSetValue;
                                if (o != null)
                                {
                                    queueitem.Attributes["cgi_incidentstagecode"] = GetOptionSetValueLabel("incident",
                                        a.Key,
                                        o.Value, 1053, data);
                                }
                                break;
                            case "prioritycode":
                                o = a.Value as OptionSetValue;
                                if (o != null)
                                {
                                    queueitem.Attributes["cgi_priority"] = GetOptionSetValueLabel("incident", a.Key,
                                        o.Value,
                                        1053, data);
                                }
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

                                if (owner != null)
                                {
                                    if (owner.LogicalName.ToUpperInvariant() == "TEAM")
                                    {
                                        queueitem.Attributes["workerid"] = null;
                                    }
                                    else if (owner.LogicalName.ToUpperInvariant() == "SYSTEMUSER")
                                    {
                                        c = new ColumnSet("cgi_noqueueassign");
                                        incidentEntity = data.Service.Retrieve(owner.LogicalName, owner.Id, c);

                                        if (incidentEntity.Attributes.Contains("cgi_noqueueassign"))
                                        {
                                            if (incidentEntity.Attributes["cgi_noqueueassign"].ToString() == "1")
                                            {
                                                queueitem.Attributes["workerid"] = null;
                                            }
                                            else
                                            {
                                                //queueitem.Attributes["workerid"] = a.Value;
                                            }
                                        }
                                        else
                                        {
                                            //queueitem.Attributes["workerid"] = a.Value;
                                        }
                                    }
                                }
                                break;
                            case "cgi_travelinformationline":
                                queueitem.Attributes["cgi_travelinformationline"] = a.Value as String;
                                break;
                            case "cgi_refundreimbursementform":
                                reference = data.Target.Attributes["cgi_refundreimbursementform"] as EntityReference;
                                if (reference != null)
                                {
                                    c = new ColumnSet("cgi_reimbursementname");
                                    incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                    if (incidentEntity.Attributes.Contains("cgi_reimbursementname"))
                                    {
                                        queueitem.Attributes["cgi_reimbursement_name"] =
                                            incidentEntity.Attributes["cgi_reimbursementname"].ToString();
                                    }
                                }
                                break;
                            case "cgi_refundtypes":
                                reference = data.Target.Attributes["cgi_refundtypes"] as EntityReference;
                                if (reference != null)
                                {
                                    c = new ColumnSet("cgi_refundtypename");
                                    incidentEntity = data.Service.Retrieve(reference.LogicalName, reference.Id, c);
                                    if (incidentEntity.Attributes.Contains("cgi_refundtypename"))
                                    {
                                        queueitem.Attributes["cgi_refund_typename"] =
                                            incidentEntity.Attributes["cgi_refundtypename"].ToString();
                                    }
                                }
                                break;
                            case "cgi_refundapprovaltype":
                                queueitem.Attributes["cgi_refund_approvaltype"] = a.Value as String;
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
            string returnvalue;

            try
            {
                DateTime dateTime;
                DateTime.TryParse(date, out dateTime);
                string year = dateTime.Year.ToString("0000");
                string month = dateTime.Month.ToString("00");
                string day = dateTime.Day.ToString("00");
                returnvalue = string.Format("{0}-{1}-{2}", year, month, day);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return returnvalue;
        }

        // MOSCGI 2015-03-09
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
        // END

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
