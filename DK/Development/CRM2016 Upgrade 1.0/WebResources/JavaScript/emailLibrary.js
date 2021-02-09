if (typeof (CGISweden) == "undefined") { CGISweden = {}; }

// *******************************************************
// Entity: activity email 
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

CGISweden.email =
    {

        onFormLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();

            CGISweden.email.setRegardingObjectidFromQuerystringParam(formContext);
            CGISweden.email.setToFromQuerystringParam(formContext);
            CGISweden.email.addLookupFilter(formContext);

            var _form_type = formContext.ui.getFormType();

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    CGISweden.email.removeMailToOnLoad(formContext);
                    CGISweden.email.SetSenderEmail(_form_type, formContext);
                    break;
                case FORM_TYPE_UPDATE:
                    CGISweden.email.SetSenderEmail(_form_type, formContext);
                    break;
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                case FORM_TYPE_QUICKCREATE:
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    alert("Form type error!");
                    break;
            }

        },

        setEmailRecipientFocus: function (formContext) {
            formContext.getControl("cgi_email_recipient_id").setFocus();
        },

        removeMailToOnLoad: function (formContext) {
            try {
                if (CGISweden.formscriptfunctions.GetValue("cgi_attention", formContext) == true) {
                    //CGISweden.formscriptfunctions.SetValue("from", null);
                    CGISweden.formscriptfunctions.SetValue("to", null, formContext);
                    CGISweden.formscriptfunctions.SetValue("cc", null, formContext);
                    CGISweden.formscriptfunctions.SetValue("bcc", null, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("to", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
                    window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
                }
                else if (CGISweden.formscriptfunctions.GetValue("cgi_remittance", formContext) == true) {
                    CGISweden.formscriptfunctions.SetValue("from", null, formContext);
                    CGISweden.formscriptfunctions.SetValue("to", null, formContext);
                    CGISweden.formscriptfunctions.SetValue("cc", null, formContext);
                    CGISweden.formscriptfunctions.SetValue("bcc", null, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("to", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
                    window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.removeMailToOnLoad\n\n" + e.Message);
            }
        },

        set_filelinks_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _incidentid = CGISweden.formscriptfunctions.GetLookupid("regardingobjectid");
                var _act_value = formContext.data.entity.attributes.get("cgi_getfilelink").getValue();
                var _attribute = formContext.getAttribute("regardingobjectid");
                var _lookup = _attribute.getValue();

                if (_lookup != null) {
                    var entity_name = _lookup[0].entityType;
                }

                if (entity_name == "incident" && _act_value == true) {
                    CGISweden.odata.GetFilelinks(_incidentid, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.article.set_filelinks_onchange\n\n" + e.Message);
            }
        },

        set_filelinks_onchange_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Det finns inga bifogade filer kopplade till ärendet");
                }
                else if (result.length == 0) {
                    alert("Det finns inga bifogade filer kopplade till ärendet");
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        if (i == 0) {
                            var _cgi_Url1 = formContext.getAttribute("description").getValue() + '<br />' + result[i].cgi_URL;
                        }
                        else {
                            _cgi_Url1 = _cgi_Url1 + '<br />' + result[i].cgi_URL;
                        }
                    }
                    formContext.getAttribute("description").setValue(_cgi_Url1);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.set_filelinks_onchange_callback\n\n" + e.Message);
            }
        },


        set_to_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _id = CGISweden.formscriptfunctions.GetLookupid("cgi_email_recipient_id", formContext);
                var _name = CGISweden.formscriptfunctions.GetLookupName("cgi_email_recipient_id", formContext);
                var _logicalname = "cgi_emailrecipient";

                CGISweden.formscriptfunctions.SetLookup("to", _logicalname, _id, _name, formContext);
                formContext.getControl("subject").setFocus();
            }
            catch (e) {
                alert("Fel i CGISweden.email.set_to_onchange\n\n" + e.Message);
            }
        },

        set_cc_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var id = CGISweden.formscriptfunctions.GetLookupid("cgi_cc_emailrecipient", formContext),
                    name = CGISweden.formscriptfunctions.GetLookupName("cgi_cc_emailrecipient", formContext),
                    logicalName = "cgi_emailrecipient";

                CGISweden.formscriptfunctions.SetLookup("cc", logicalName, id, name, formContext);
            } catch (e) {
                alert("Fel i CGISweden.email.set_cc_onchange\n\n" + e.Message);
            }
        },

        set_bcc_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var id = CGISweden.formscriptfunctions.GetLookupid("cgi_bcc_emailrecipient", formContext),
                    name = CGISweden.formscriptfunctions.GetLookupName("cgi_bcc_emailrecipient", formContext),
                    logicalName = "cgi_emailrecipient";

                CGISweden.formscriptfunctions.SetLookup("bcc", logicalName, id, name, formContext);
            } catch (e) {
                alert("Fel i CGISweden.email.set_bcc_onchange\n\n" + e.Message);
            }
        },

        addLookupFilter: function (formContext) {

            try {
                //////// Email Recipient

                // use randomly generated GUID Id for the view
                var viewId = "{0CBC820C-7033-4AFF-9CE8-FB610464DBD3}";
                var entityName = "cgi_emailrecipient";

                // give the custom view a name
                var viewDisplayName = "Epost";

                var _fetch = "";
                _fetch += "<fetch version='1.0' mapping='logical' distinct='false'>";
                _fetch += "<entity name='cgi_emailrecipient'>";
                _fetch += "<attribute name='cgi_emailrecipientid' />";
                _fetch += "<attribute name='cgi_emailrecipientname' />";
                _fetch += "<attribute name='cgi_emailgroupid' />";
                _fetch += "<attribute name='cgi_role' />";
                _fetch += "<attribute name='cgi_emailaddress' />";
                _fetch += "</entity>";
                _fetch += "</fetch>";

                // build Grid Layout
                var _layoutXml = "<grid name='resultset' " +
                    "object='10014' " +
                    "jump='cgi_emailrecipientid' " +
                    "select='1' " +
                    "icon='0' " +
                    "preview='0'>" +
                    "<row name='result' id='cgi_emailrecipientid'>" +
                    "<cell name='cgi_emailrecipientname' width='200' />" +
                    "<cell name='cgi_emailgroupid' width='200' />" +
                    "<cell name='cgi_role' width='200' />" +
                    "<cell name='cgi_emailaddress' width='200' />" +
                    "</row>" +
                    "</grid>";

                // add the Custom View to the indicated [lookupFieldName] Control
                formContext.getControl("to").addCustomView(viewId, entityName, viewDisplayName, _fetch, _layoutXml, true);

            } catch (e) {
                alert("Fel i CGISweden.email.addLookupFilter\n\n" + e.Message);
            }

        },

        setRegardingObjectidFromQuerystringParam: function (formContext) {
            try {
                var _xrmObject = formContext.context.getQueryStringParameters();
                var _caseId = _xrmObject["parameter_regardingid"];
                var _caseName = _xrmObject["parameter_regardingname"];
                var _entityType = _xrmObject["parameter_regardingtype"];

                if (_caseId == "undefined")
                    return;

                if (_caseId != null && _caseName != null && _entityType != null) {
                    CGISweden.formscriptfunctions.SetLookup("regardingobjectid", _entityType, _caseId, _caseName, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.getQuerystringParam\n\n" + e.Message);
            }
        },

        setToFromQuerystringParam: function (formContext) {
            try {
                var _xrmObject = formContext.context.getQueryStringParameters();
                var _accountId = _xrmObject["parameter_customerid"];
                var _accountName = _xrmObject["parameter_customername"];
                var _entityType = _xrmObject["parameter_customertype"];

                if (_accountId == "undefined")
                    return;

                if (_accountId != null && _accountName != null && _entityType != null) {
                    CGISweden.formscriptfunctions.SetLookup("to", _entityType, _accountId, _accountName, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.getQuerystringParam\n\n" + e.Message);
            }
        },

        SetSenderEmail: function (_form_type, formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var _directioncode = formContext.getAttribute("directioncode").getValue();
                var _emailstatus = formContext.data.entity.attributes.get('statuscode').getValue();
                var _infomail = formContext.data.entity.attributes.get('cgi_attention').getValue();
                var _tocustomer = formContext.data.entity.attributes.get('cgi_button_customer').getValue();

                if (_infomail == false && _tocustomer != 1) {
                    if (_form_type == 1 || (_form_type == 2 && _emailstatus == 1 && _directioncode == true)) {
                        //if (_form_type == 1) {
                        var userId = globalContext.userSettings.userId();
                        CGISweden.odata.GetDefaultQueue(userId, formContext);
                        //}
                    }
                }

                if (_tocustomer == '1' && !CGISweden.formscriptfunctions.GetValue("cgi_ask_customer", formContext)) {
                    var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
                    CGISweden.odata.GetDefaultUserFromSetting(_currentdate, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.SetSenderEmail\n\n" + e.Message);
            }
        },

        SetSenderEmail_callback: function (result, formContext) {
            try {
                debugger;
                if (result == null || result[0] == null) {
                    alert("Ingen default kö är definierad på användaren!");
                }
                else {
                    _queueId = result[0].QueueId.Id;
                    _queueLogicalName = result[0].QueueId.LogicalName
                    _queueName = result[0].QueueId.Name

                    // Make sure field is changed.
                    // Unchanged = Do nothing
                    var fromId = CGISweden.formscriptfunctions.GetLookupid("from", formContext);
                    if (fromId != null)
                        if (fromId.toUpperCase() == ("{" + _queueId + "}").toUpperCase())
                            return;

                    CGISweden.formscriptfunctions.SetLookup("from", _queueLogicalName, _queueId, _queueName, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.SetSenderEmail_callback\n\n" + e.Message);
            }
        },

        SetSenderEmailNoReply_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Ingen default användare för noreplay adress är definierad!");
                }
                else {
                    _userId = result[0].cgi_userid.Id;
                    _userLogicalName = result[0].cgi_userid.LogicalName
                    _userName = result[0].cgi_userid.Name
                    CGISweden.formscriptfunctions.SetLookup("from", _userLogicalName, _userId, _userName, formContext);
                }
            }
            catch (e) {
                alert("Fel i CGISweden.email.SetSenderEmailNoReply_callback\n\n" + e.Message);
            }
        },

        AskCustomer_onchange: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var globalContext = Xrm.Utility.getGlobalContext();

            var _form_type = formContext.ui.getFormType();
            var _askcustomer = formContext.data.entity.attributes.get('cgi_ask_customer').getValue();
            if (_askcustomer == true) {
                var _form_type = formContext.ui.getFormType();
                var userId = globalContext.userSettings.userId();
                CGISweden.odata.GetDefaultQueue(userId, formContext);
            }
            else {
                var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
                CGISweden.odata.GetDefaultUserFromSetting(_currentdate, formContext);
            }
        }
    };