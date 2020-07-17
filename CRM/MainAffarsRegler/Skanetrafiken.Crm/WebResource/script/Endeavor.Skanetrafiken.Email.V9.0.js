
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

        saveAndSendEmail: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            formContext.data.save().then(function () { Endeavor.Skanetrafiken.Email.SendEmail(formContext); }, function () { Endeavor.Skanetrafiken.Email.ErrorOnSave(formContext); });

            //var emailId = parent.Xrm.Page.data.entity.save();


            ////parent.Xrm.Page.ui.close();

            //Process.callWorkflow("AA7332EA-4124-448F-A36D-BD0039130F6C",
            //    emailId,
            //    function () {
            //        //alert("E-post skickat");
            //        parent.Xrm.Page.ui.close();
            //    },
            //    function () {
            //        alert("Fel vid utskick av e-post");
            //    });
        },

        SendEmail: function (formContext) {
            debugger;
            formContext.ui.setFormNotification("Skickar e-post. Vänligen vänta.", "INFO");

            var idRecord = formContext.data.entity.getId();

            //TODO ACTIONS
            Process.callAction("new_SendEmailFromRibbon",
                [{
                    key: "Target",
                    type: Process.Type.EntityReference,
                    value: new Process.EntityReference("email", idRecord)
                }],
                function () {
                    //DEBUG TODO
                    parent.formContext.ui.close();

                    //Xrm.Page.data.refresh();
                    //Xrm.Page.ui.setFormNotification("Text Message successfully delivered.", "INFO");
                },
                function (e, t) {
                    // Error
                    formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                })
        },
        ErrorOnSave: function (formContext, error) {
            debugger;
            formContext.ui.setFormNotification("Fel vid mejlutskick: " + error);
        },

        /*
         * 
         * CGI Email (From emailLibrary.js)
         * 
         */

        onFormLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Email.setRegardingObjectidFromQuerystringParam(formContext);
            Endeavor.Skanetrafiken.Email.setToFromQuerystringParam(formContext);
            Endeavor.Skanetrafiken.Email.addLookupFilter(formContext);

            var _form_type = formContext.ui.getFormType();

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    Endeavor.Skanetrafiken.Email.removeMailToOnLoad(formContext);
                    Endeavor.Skanetrafiken.Email.SetSenderEmail(_form_type, formContext);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.removeMailToOnLoad\n\n" + e.Message);
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
                }

                if (entity_name == "incident" && _act_value == true) {
                    Endeavor.OData_Querys.GetFilelinks(_incidentid, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.set_filelinks_onchange\n\n" + e.Message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.set_filelinks_onchange_callback\n\n" + e.Message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.set_to_onchange\n\n" + e.Message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.set_cc_onchange\n\n" + e.Message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.set_bcc_onchange\n\n" + e.Message);
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
                alert("Fel i Endeavor.Skanetrafiken.Email.addLookupFilter\n\n" + e.Message);
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
                    Endeavor.formscriptfunctions.SetLookup("regardingobjectid", _entityType, _caseId, _caseName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.Message);
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
                    Endeavor.formscriptfunctions.SetLookup("to", _entityType, _accountId, _accountName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.getQuerystringParam\n\n" + e.Message);
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
                        Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
                        //}
                    }
                }

                if (_tocustomer == '1' && !Endeavor.formscriptfunctions.GetValue("cgi_ask_customer", formContext)) {
                    var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                    Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail\n\n" + e.Message);
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
                    var fromId = Endeavor.formscriptfunctions.GetLookupid("from", formContext);
                    if (fromId != null)
                        if (fromId.toUpperCase() == ("{" + _queueId + "}").toUpperCase())
                            return;

                    Endeavor.formscriptfunctions.SetLookup("from", _queueLogicalName, _queueId, _queueName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmail_callback\n\n" + e.Message);
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
                    Endeavor.formscriptfunctions.SetLookup("from", _userLogicalName, _userId, _userName, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Email.SetSenderEmailNoReply_callback\n\n" + e.Message);
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
                Endeavor.OData_Querys.GetDefaultQueue(userId, formContext);
            }
            else {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetDefaultUserFromSetting(_currentdate, formContext);
            }
        }

    }

}

var Process = Process || {};

// Supported Action input parameter types
Process.Type = {
    Bool: "c:boolean",
    Float: "c:double", // Not a typo
    Decimal: "c:decimal",
    Int: "c:int",
    String: "c:string",
    DateTime: "c:dateTime",
    Guid: "c:guid",
    EntityReference: "a:EntityReference",
    OptionSet: "a:OptionSetValue",
    Money: "a:Money",
    Entity: "a:Entity",
    EntityCollection: "a:EntityCollection"
}

// inputParams: Array of parameters to pass to the Action. Each param object should contain key, value, and type.
// successCallback: Function accepting 1 argument, which is an array of output params. Access values like: params["key"]
// errorCallback: Function accepting 1 argument, which is the string error message. Can be null.
// Unless the Action is global, you must specify a 'Target' input parameter as EntityReference
// actionName is required
Process.callAction = function (actionName, inputParams, successCallback, errorCallback, url) {
    var ns = {
        "": "http://schemas.microsoft.com/xrm/2011/Contracts/Services",
        ":s": "http://schemas.xmlsoap.org/soap/envelope/",
        ":a": "http://schemas.microsoft.com/xrm/2011/Contracts",
        ":i": "http://www.w3.org/2001/XMLSchema-instance",
        ":b": "http://schemas.datacontract.org/2004/07/System.Collections.Generic",
        ":c": "http://www.w3.org/2001/XMLSchema",
        ":d": "http://schemas.microsoft.com/xrm/2011/Contracts/Services",
        ":e": "http://schemas.microsoft.com/2003/10/Serialization/",
        ":f": "http://schemas.microsoft.com/2003/10/Serialization/Arrays",
        ":g": "http://schemas.microsoft.com/crm/2011/Contracts",
        ":h": "http://schemas.microsoft.com/xrm/2011/Metadata",
        ":j": "http://schemas.microsoft.com/xrm/2011/Metadata/Query",
        ":k": "http://schemas.microsoft.com/xrm/2013/Metadata",
        ":l": "http://schemas.microsoft.com/xrm/2012/Contracts",
        //":c": "http://schemas.microsoft.com/2003/10/Serialization/" // Conflicting namespace for guid... hardcoding in the _getXmlValue bit
    };

    var requestXml = "<s:Envelope";

    // Add all the namespaces
    for (var i in ns) {
        requestXml += " xmlns" + i + "='" + ns[i] + "'";
    }

    requestXml += ">" +
        "<s:Body>" +
        "<Execute>" +
        "<request>";

    if (inputParams != null && inputParams.length > 0) {
        requestXml += "<a:Parameters>";

        // Add each input param
        for (var i = 0; i < inputParams.length; i++) {
            var param = inputParams[i];

            var value = Process._getXmlValue(param.key, param.type, param.value);

            requestXml += value;
        }

        requestXml += "</a:Parameters>";
    }
    else {
        requestXml += "<a:Parameters />";
    }

    requestXml += "<a:RequestId i:nil='true' />" +
        "<a:RequestName>" + actionName + "</a:RequestName>" +
        "</request>" +
        "</Execute>" +
        "</s:Body>" +
        "</s:Envelope>";

    Process._callActionBase(requestXml, successCallback, errorCallback, url);
}

Process._emptyGuid = "00000000-0000-0000-0000-000000000000";

// This can be used to execute custom requests if needed - useful for me testing the SOAP :)
Process._callActionBase = function (requestXml, successCallback, errorCallback, url) {
    if (url == null) {
        url = Xrm.Page.context.getClientUrl();
    }

    var req = new XMLHttpRequest();
    req.open("POST", url + "/XRMServices/2011/Organization.svc/web", true);
    req.setRequestHeader("Accept", "application/xml, text/xml, */*");
    req.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
    req.setRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/Execute");

    req.onreadystatechange = function () {
        if (req.readyState == 4) {
            if (req.status == 200) {
                // If there's no successCallback we don't need to check the outputParams
                if (successCallback) {
                    // Yucky but don't want to risk there being multiple 'Results' nodes or something
                    var resultsNode = req.responseXML.childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[1]; // <a:Results>

                    // Action completed successfully - get output params
                    var responseParams = Process._getChildNodes(resultsNode, "a:KeyValuePairOfstringanyType");

                    var outputParams = {};
                    for (i = 0; i < responseParams.length; i++) {
                        var attrNameNode = Process._getChildNode(responseParams[i], "b:key");
                        var attrValueNode = Process._getChildNode(responseParams[i], "b:value");

                        var attributeName = Process._getNodeTextValue(attrNameNode);
                        var attributeValue = Process._getValue(attrValueNode);

                        // v1.0 - Deprecated method using key/value pair and standard array
                        //outputParams.push({ key: attributeName, value: attributeValue.value });

                        // v2.0 - Allows accessing output params directly: outputParams["Target"].attributes["new_fieldname"];
                        outputParams[attributeName] = attributeValue.value;

                        /*
                        RETURN TYPES:
                            DateTime = Users local time (JavaScript date)
                            bool = true or false (JavaScript boolean)
                            OptionSet, int, decimal, float, etc = 1 (JavaScript number)
                            guid = string
                            EntityReference = { id: "guid", name: "name", entityType: "account" }
                            Entity = { logicalName: "account", id: "guid", attributes: {}, formattedValues: {} }
                            EntityCollection = [{ logicalName: "account", id: "guid", attributes: {}, formattedValues: {} }]
    
                        Attributes for entity accessed like: entity.attributes["new_fieldname"].value
                        For entityreference: entity.attributes["new_fieldname"].value.id
                        Make sure attributes["new_fieldname"] is not null before using .value
                        Or use the extension method entity.get("new_fieldname") to get the .value
                        Also use entity.formattedValues["new_fieldname"] to get the string value of optionsetvalues, bools, moneys, etc
                        */
                    }

                    // Make sure the callback accepts exactly 1 argument - use dynamic function if you want more
                    successCallback(outputParams);
                }
            }
            else {
                // Error has occured, action failed
                if (errorCallback) {
                    var message = null;
                    var traceText = null;
                    try {
                        message = Process._getNodeTextValueNotNull(req.responseXML.getElementsByTagName("Message"));
                        traceText = Process._getNodeTextValueNotNull(req.responseXML.getElementsByTagName("TraceText"));
                    } catch (e) { }
                    if (message == null) { message = "Error executing Action. Check input parameters or contact your CRM Administrator"; }
                    errorCallback(message, traceText);
                }
            }
        }
    };

    req.send(requestXml);
}

// Get only the immediate child nodes for a specific tag, otherwise entitycollections etc mess it up
Process._getChildNodes = function (node, childNodesName) {
    var childNodes = [];

    for (var i = 0; i < node.childNodes.length; i++) {
        if (node.childNodes[i].tagName == childNodesName) {
            childNodes.push(node.childNodes[i]);
        }
    }

    // Chrome uses just 'Results' instead of 'a:Results' etc
    if (childNodes.length == 0 && childNodesName.indexOf(":") !== -1) {
        childNodes = Process._getChildNodes(node, childNodesName.substring(childNodesName.indexOf(":") + 1));
    }

    return childNodes;
}

// Get a single child node for a specific tag
Process._getChildNode = function (node, childNodeName) {
    var nodes = Process._getChildNodes(node, childNodeName);

    if (nodes != null && nodes.length > 0) { return nodes[0]; }
    else { return null; }
}

// Gets the first not null value from a collection of nodes
Process._getNodeTextValueNotNull = function (nodes) {
    var value = "";

    for (var i = 0; i < nodes.length; i++) {
        if (value === "") {
            value = Process._getNodeTextValue(nodes[i]);
        }
    }

    return value;
}

// Gets the string value of the XML node
Process._getNodeTextValue = function (node) {
    if (node != null) {
        var textNode = node.firstChild;
        if (textNode != null) {
            return textNode.textContent || textNode.nodeValue || textNode.data || textNode.text;
        }
    }

    return "";
}

// Gets the value of a parameter based on its type, can be recursive for entities
Process._getValue = function (node) {
    var value = null;
    var type = null;

    if (node != null) {
        type = node.getAttribute("i:type") || node.getAttribute("type");

        // If the parameter/attribute is null, there won't be a type either
        if (type != null) {
            // Get the part after the ':' (since Chrome doesn't have the ':')
            var valueType = type.substring(type.indexOf(":") + 1).toLowerCase();

            if (valueType == "entityreference") {
                // Gets the lookup object
                var attrValueIdNode = Process._getChildNode(node, "a:Id");
                var attrValueEntityNode = Process._getChildNode(node, "a:LogicalName");
                var attrValueNameNode = Process._getChildNode(node, "a:Name");

                var lookupId = Process._getNodeTextValue(attrValueIdNode);
                var lookupName = Process._getNodeTextValue(attrValueNameNode);
                var lookupEntity = Process._getNodeTextValue(attrValueEntityNode);

                value = new Process.EntityReference(lookupEntity, lookupId, lookupName);
            }
            else if (valueType == "entity") {
                // Gets the entity data, and all attributes
                value = Process._getEntityData(node);
            }
            else if (valueType == "entitycollection") {
                // Loop through each entity, returns each entity, and all attributes
                var entitiesNode = Process._getChildNode(node, "a:Entities");
                var entityNodes = Process._getChildNodes(entitiesNode, "a:Entity");

                value = [];
                if (entityNodes != null && entityNodes.length > 0) {
                    for (var i = 0; i < entityNodes.length; i++) {
                        value.push(Process._getEntityData(entityNodes[i]));
                    }
                }
            }
            else if (valueType == "aliasedvalue") {
                // Gets the actual data type of the aliased value
                // Key for these is "alias.fieldname"
                var aliasedValue = Process._getValue(Process._getChildNode(node, "a:Value"));
                if (aliasedValue != null) {
                    value = aliasedValue.value;
                    type = aliasedValue.type;
                }
            }
            else {
                // Standard fields like string, int, date, money, optionset, float, bool, decimal
                // Output will be string, even for number fields etc
                var stringValue = Process._getNodeTextValue(node);

                if (stringValue != null) {
                    switch (valueType) {
                        case "datetime":
                            value = new Date(stringValue);
                            break;
                        case "int":
                        case "money":
                        case "optionsetvalue":
                        case "double": // float
                        case "decimal":
                            value = Number(stringValue);
                            break;
                        case "boolean":
                            value = stringValue.toLowerCase() === "true";
                            break;
                        default:
                            value = stringValue;
                    }
                }
            }
        }
    }

    return new Process.Attribute(value, type);
}

Process._getEntityData = function (entityNode) {
    var value = null;

    var entityAttrsNode = Process._getChildNode(entityNode, "a:Attributes");
    var entityIdNode = Process._getChildNode(entityNode, "a:Id");
    var entityLogicalNameNode = Process._getChildNode(entityNode, "a:LogicalName");
    var entityFormattedValuesNode = Process._getChildNode(entityNode, "a:FormattedValues");

    var entityLogicalName = Process._getNodeTextValue(entityLogicalNameNode);
    var entityId = Process._getNodeTextValue(entityIdNode);
    var entityAttrs = Process._getChildNodes(entityAttrsNode, "a:KeyValuePairOfstringanyType");

    value = new Process.Entity(entityLogicalName, entityId);

    // Attribute values accessed via entity.attributes["new_fieldname"]
    if (entityAttrs != null && entityAttrs.length > 0) {
        for (var i = 0; i < entityAttrs.length; i++) {

            var attrNameNode = Process._getChildNode(entityAttrs[i], "b:key")
            var attrValueNode = Process._getChildNode(entityAttrs[i], "b:value");

            var attributeName = Process._getNodeTextValue(attrNameNode);
            var attributeValue = Process._getValue(attrValueNode);

            value.attributes[attributeName] = attributeValue;
        }
    }

    // Formatted values accessed via entity.formattedValues["new_fieldname"]
    for (var j = 0; j < entityFormattedValuesNode.childNodes.length; j++) {
        var foNode = entityFormattedValuesNode.childNodes[j];

        var fNameNode = Process._getChildNode(foNode, "b:key")
        var fValueNode = Process._getChildNode(foNode, "b:value");

        var fName = Process._getNodeTextValue(fNameNode);
        var fValue = Process._getNodeTextValue(fValueNode);

        value.formattedValues[fName] = fValue;
    }

    return value;
}

Process._getXmlValue = function (key, dataType, value) {
    var xml = "";
    var xmlValue = "";

    var extraNamespace = "";

    // Check the param type to determine how the value is formed
    switch (dataType) {
        case Process.Type.String:
            xmlValue = Process._htmlEncode(value) || ""; // Allows fetchXml strings etc
            break;
        case Process.Type.DateTime:
            xmlValue = value.toISOString() || "";
            break;
        case Process.Type.EntityReference:
            xmlValue = "<a:Id>" + (value.id || "") + "</a:Id>" +
                "<a:LogicalName>" + (value.entityType || "") + "</a:LogicalName>" +
                "<a:Name i:nil='true' />";
            break;
        case Process.Type.OptionSet:
        case Process.Type.Money:
            xmlValue = "<a:Value>" + (value || 0) + "</a:Value>";
            break;
        case Process.Type.Entity:
            xmlValue = Process._getXmlEntityData(value);
            break;
        case Process.Type.EntityCollection:
            if (value != null && value.length > 0) {
                var entityCollection = "";
                for (var i = 0; i < value.length; i++) {
                    var entityData = Process._getXmlEntityData(value[i]);
                    if (entityData !== null) {
                        entityCollection += "<a:Entity>" + entityData + "</a:Entity>";
                    }
                }
                if (entityCollection !== null && entityCollection !== "") {
                    xmlValue = "<a:Entities>" + entityCollection + "</a:Entities>" +
                        "<a:EntityName i:nil='true' />" +
                        "<a:MinActiveRowVersion i:nil='true' />" +
                        "<a:MoreRecords>false</a:MoreRecords>" +
                        "<a:PagingCookie i:nil='true' />" +
                        "<a:TotalRecordCount>0</a:TotalRecordCount>" +
                        "<a:TotalRecordCountLimitExceeded>false</a:TotalRecordCountLimitExceeded>";
                }
            }
            break;
        case Process.Type.Guid:
            // I don't think guid fields can even be null?
            xmlValue = value || Process._emptyGuid;

            // This is a hacky fix to get guids working since they have a conflicting namespace :(
            extraNamespace = " xmlns:c='http://schemas.microsoft.com/2003/10/Serialization/'";
            break;
        default: // bool, int, double, decimal
            xmlValue = value != undefined ? value : null;
            break;
    }

    xml = "<a:KeyValuePairOfstringanyType>" +
        "<b:key>" + key + "</b:key>" +
        "<b:value i:type='" + dataType + "'" + extraNamespace;

    // nulls crash if you have a non-self-closing tag
    if (xmlValue === null || xmlValue === "") {
        xml += " i:nil='true' />";
    }
    else {
        xml += ">" + xmlValue + "</b:value>";
    }

    xml += "</a:KeyValuePairOfstringanyType>";

    return xml;
}

Process._getXmlEntityData = function (entity) {
    var xml = null;

    if (entity != null) {
        var attrXml = "";

        for (field in entity.attributes) {
            var a = entity.attributes[field];
            var aXml = Process._getXmlValue(field, a.type, a.value);

            attrXml += aXml;
        }

        if (attrXml !== "") {
            xml = "<a:Attributes>" + attrXml + "</a:Attributes>";
        }
        else {
            xml = "<a:Attributes />";
        }

        xml += "<a:EntityState i:nil='true' />" +
            "<a:FormattedValues />" +
            "<a:Id>" + entity.id + "</a:Id>" +
            "<a:KeyAttributes />" +
            "<a:LogicalName>" + entity.logicalName + "</a:LogicalName>" +
            "<a:RelatedEntities />" +
            "<a:RowVersion i:nil='true' />";
    }

    return xml;
}

Process._htmlEncode = function (s) {
    if (typeof s !== "string") { return s; }

    return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
}

Process.Entity = function (logicalName, id, attributes) {
    this.logicalName = logicalName || "";
    this.attributes = attributes || {};
    this.formattedValues = {};
    this.id = id || Process._emptyGuid;
}

// Gets the value of the attribute without having to check null
Process.Entity.prototype.get = function (key) {
    var a = this.attributes[key];
    if (a != null) {
        return a.value;
    }

    return null;
}

Process.EntityReference = function (entityType, id, name) {
    this.id = id || Process._emptyGuid;
    this.name = name || "";
    this.entityType = entityType || "";
}

Process.Attribute = function (value, type) {
    this.value = value != undefined ? value : null;
    this.type = type || "";
}