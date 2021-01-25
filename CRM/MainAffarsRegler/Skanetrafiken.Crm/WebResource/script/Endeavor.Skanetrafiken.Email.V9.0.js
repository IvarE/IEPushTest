FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Email) == "undefined") {
    Endeavor.Skanetrafiken.Email = {

        saveAndSendEmail: function (formContext) {
            debugger;
            formContext.data.save().then(function () { Endeavor.Skanetrafiken.Email.SendEmail(formContext); }, function () { Endeavor.Skanetrafiken.Email.ErrorOnSave(formContext); });
        },

        SendEmail: function (formContext) {
            debugger;
            formContext.ui.setFormNotification("Skickar e-post. Vänligen vänta.", "INFO");

            var idRecord = formContext.data.entity.getId().replace("{", "").replace("}", "");

            Endeavor.formscriptfunctions.callAction("new_SendEmailFromRibbon", "email", idRecord, null,
                function () {
                    formContext.ui.close();
                },
                function (e) {
                    formContext.ui.setFormNotification("Någonting gick fel: " + e.message, "INFO");
                    console.error(e.message);
                });
        },

        ErrorOnSave: function (formContext, error) {
            debugger;
            formContext.ui.setFormNotification("Fel vid mejlutskick: " + error);
        },

        //Form Methods CGI Email (from emailLibrary.js)
        onFormLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();

            
            Endeavor.Skanetrafiken.Email.addLookupFilter(formContext);

            var _form_type = formContext.ui.getFormType();

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    Endeavor.Skanetrafiken.Email.removeMailToOnLoad(formContext);
                    Endeavor.Skanetrafiken.Email.SetSenderEmail(_form_type, formContext);
                    Endeavor.Skanetrafiken.Email.setToFromQuerystringParam(formContext);
                    Endeavor.Skanetrafiken.Email.setRegardingObjectidFromQuerystringParam(formContext);
                    break;
                case FORM_TYPE_UPDATE:
                    Endeavor.Skanetrafiken.Email.SetSenderEmail(_form_type, formContext);
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
                if (Endeavor.formscriptfunctions.GetValue("cgi_attention", formContext) == true) {
                    //Endeavor.formscriptfunctions.SetValue("from", null);
                    Endeavor.formscriptfunctions.SetValue("to", null, formContext);
                    Endeavor.formscriptfunctions.SetValue("cc", null, formContext);
                    Endeavor.formscriptfunctions.SetValue("bcc", null, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("to", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
                    window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
                }
                else if (Endeavor.formscriptfunctions.GetValue("cgi_remittance", formContext) == true) {
                    Endeavor.formscriptfunctions.SetValue("from", null, formContext);
                    Endeavor.formscriptfunctions.SetValue("to", null, formContext);
                    Endeavor.formscriptfunctions.SetValue("cc", null, formContext);
                    Endeavor.formscriptfunctions.SetValue("bcc", null, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_ask_customer", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("to", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cc", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("bcc", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email_recipient_id", true, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_cc_emailrecipient", true, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_bcc_emailrecipient", true, formContext);
                    window.setTimeout(this.setEmailRecipientFocus(formContext), 50);
                }
                else {
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_attention", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_remittance", false, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.removeMailToOnLoad\n\n" + e.message);
            }
        },

        set_filelinks_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _incidentid = Endeavor.formscriptfunctions.GetLookupid("regardingobjectid");
                var _act_value = formContext.data.entity.attributes.get("cgi_getfilelink").getValue();
                var _attribute = formContext.getAttribute("regardingobjectid");
                var _lookup = _attribute.getValue();

                if (_lookup != null) {
                    var entity_name = _lookup[0].entityType;

                    if (entity_name == "incident" && _act_value == true)
                        Endeavor.OData_Querys.GetFilelinks(_incidentid, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.set_filelinks_onchange\n\n" + e.message);
            }
        },

        set_filelinks_onchange_callback: function (result, formContext) {
            try {
                if (result == null)
                    alert("Det finns inga bifogade filer kopplade till ärendet");
                else if (result.entities.length == 0)
                    alert("Det finns inga bifogade filer kopplade till ärendet");
                else {
                    var _cgi_Url1 = "";
                    for (var i = 0; i < result.entities.length; i++) {
                        if (i == 0)
                            _cgi_Url1 = formContext.getAttribute("description").getValue() + '<br />' + result.entities[i].cgi_url;
                        else
                            _cgi_Url1 += '<br />' + result.entities[i].cgi_url;
                    }
                    formContext.getAttribute("description").setValue(_cgi_Url1);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.set_filelinks_onchange_callback\n\n" + e.message);
            }
        },

        set_to_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _id = Endeavor.formscriptfunctions.GetLookupid("cgi_email_recipient_id", formContext);
                var _name = Endeavor.formscriptfunctions.GetLookupName("cgi_email_recipient_id", formContext);
                var _logicalname = "cgi_emailrecipient";

                Endeavor.formscriptfunctions.SetLookup("to", _logicalname, _id, _name, formContext);
                formContext.getControl("subject").setFocus();
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.set_to_onchange\n\n" + e.message);
            }
        },

        set_cc_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var id = Endeavor.formscriptfunctions.GetLookupid("cgi_cc_emailrecipient", formContext),
                    name = Endeavor.formscriptfunctions.GetLookupName("cgi_cc_emailrecipient", formContext),
                    logicalName = "cgi_emailrecipient";

                Endeavor.formscriptfunctions.SetLookup("cc", logicalName, id, name, formContext);
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.set_cc_onchange\n\n" + e.message);
            }
        },

        set_bcc_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var id = Endeavor.formscriptfunctions.GetLookupid("cgi_bcc_emailrecipient", formContext),
                    name = Endeavor.formscriptfunctions.GetLookupName("cgi_bcc_emailrecipient", formContext),
                    logicalName = "cgi_emailrecipient";

                Endeavor.formscriptfunctions.SetLookup("bcc", logicalName, id, name, formContext);
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.set_bcc_onchange\n\n" + e.message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.addLookupFilter\n\n" + e.message);
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

                if (_caseId != null && _caseName != null && _entityType != null)
                    Endeavor.formscriptfunctions.SetLookup("regardingobjectid", _entityType, _caseId, _caseName, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.message);
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

                if (_accountId != null && _accountName != null && _entityType != null)
                    Endeavor.formscriptfunctions.SetLookup("to", _entityType, _accountId, _accountName, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.message);
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
                        var userId = globalContext.userSettings.userId.replace("{", "").replace("}", "");
                        Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
                    }
                }

                if (_tocustomer == '1' && !Endeavor.formscriptfunctions.GetValue("cgi_ask_customer", formContext)) {
                    var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                    Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail\n\n" + e.message);
            }
        },

        SetSenderEmail_callback: function (result, formContext) {
            try {
                debugger;
                if (result == null || result.entities == null || result.entities[0] == null) {
                    alert("Ingen default kö är definierad på användaren!");
                }
                else {
                    _queueId = result.entities[0]["_queueid_value"];
                    _queueLogicalName = result.entities[0]["_queueid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                    _queueName = result.entities[0]["_queueid_value@OData.Community.Display.V1.FormattedValue"];

                    // Make sure field is changed.
                    // Unchanged = Do nothing
                    var fromId = Endeavor.formscriptfunctions.GetLookupid("from", formContext);
                    if (fromId != null)
                        if (fromId.toUpperCase() == ("{" + _queueId + "}").toUpperCase())
                            return;

                    Endeavor.formscriptfunctions.SetLookup("from", _queueLogicalName, _queueId, _queueName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail_callback\n\n" + e.message);
            }
        },

        SetSenderEmailNoReply_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities[0] == null) {
                    alert("Ingen default användare för noreplay adress är definierad!");
                }
                else {
                    _userId = result.entities[0]["_cgi_userid_value"];
                    _userLogicalName = result.entities[0]["_cgi_userid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                    _userName = result.entities[0]["_cgi_userid_value@OData.Community.Display.V1.FormattedValue"];

                    Endeavor.formscriptfunctions.SetLookup("from", _userLogicalName, _userId, _userName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmailNoReply_callback\n\n" + e.message);
            }
        },

        AskCustomer_onchange: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var globalContext = Xrm.Utility.getGlobalContext();

            var _askcustomer = formContext.data.entity.attributes.get('cgi_ask_customer').getValue();
            if (_askcustomer == true) {
                var userId = globalContext.userSettings.userId.replace("{", "").replace("}", "");;
                Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
            }
            else {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
            }
        }
    }
}