FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
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

if (typeof (Endeavor.Skanetrafiken.cgi_refund) == "undefined") {
    Endeavor.Skanetrafiken.cgi_refund = {

        createAndSendValueCode: function () {
            debugger;
            Xrm.Page.ui.setFormNotification("Skapar värdekod. Vänligen vänta.", "INFO");
            Process.callAction("ed_CreateAndSendValueCodeFromRefund7be826f88b9fe811827600155d010b00",
                [{
                    key: "Target",
                    type: Process.Type.EntityReference,
                    value: new Process.EntityReference("cgi_refund", Xrm.Page.data.entity.getId())
                }],
                function () {
                    //parent.Xrm.Page.ui.close();
                    //Xrm.Page.ui.clearFormNotification();
                    //Xrm.Page.ui.setFormNotification("Värdekod skickad", "INFO");

                    Xrm.Page.data.refresh();
                    Xrm.Page.ui.setFormNotification("Värdekod skickad.", "INFO");

                    //Xrm.Page.data.refresh();
                    //Xrm.Page.ui.setFormNotification("Text Message successfully delivered.", "INFO");
                },
                function (e, t) {
                    // Error
                    Xrm.Page.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                })
        },
        
        printRefundVoucherReport: function () {
            /// <summary>
            /// Print report. Hardcoded reports.
            /// </summary>
            try {

                debugger;

                var refundtypePrint = Xrm.Page.getAttribute("ed_refundtype_print").getValue();

                if (refundtypePrint == 0) {
                    return;
                }

                var refundPrintedDate = Xrm.Page.getAttribute("ed_voucher_printed").getValue();

                var msg = "Värdebeviset redan utskrivet. Skriva ut på nytt? (Ja/Nej)";

                if (refundPrintedDate != null) {
                    var answer = prompt(msg, "Ja");

                    if (answer == "Nej" || answer == "nej") {
                        return;
                    }

                    // User pressed cancel or entered no value.
                    if (!answer)
                        return;

                    if (isNaN(answer))
                        return;
                }

                var clientUrl = Xrm.Page.context.getClientUrl();
                var url = "";

                var refundId = Xrm.Page.data.entity.getId();
                refundId = refundId.substring(1, refundId.length - 1);


                // TODO :
                // add functionality for printing reports and set values in printed date and printed by (systemuser)

                //url = clientUrl + "/crmreports/viewer/viewer.aspx?action=run&context=records&helpID=Beslut%20Vidarefakturering.rdl&id=%7b8a3e7a71-6504-e511-80dc-0050569010ad%7d&records=" + refundId + "&recordstype=10022";
                //window.open(url, null, 600, 400, true, false, null);



                // Generate Report as PDF

                var arrReportSession = Endeavor.Skanetrafiken.cgi_refund.executeReport();

                Endeavor.Skanetrafiken.cgi_refund.convertResponseToPDF(arrReportSession);






                //// TODO :
                //// update which user executed the report and set value in refund on which datetime it was executed
                //// Set Printed date = Dateime.Now
                //var datetimeNow = new Date();

                //// Set Read-Only fields Disabled = True to update field values
                //Xrm.Page.ui.controls.get("ed_voucher_printed").setDisabled(true);
                //Xrm.Page.ui.controls.get("ed_voucher_printed_by").setDisabled(true);

                //Xrm.Page.getAttribute("ed_voucher_printed").setValue(datetimeNow);

                //// Set SystemUser
                //var setUservalue = new Array();
                //setUservalue[0] = new Object();
                //setUservalue[0].id = Xrm.Page.context.getUserId();
                //setUservalue[0].entityType = "systemuser";
                //setUservalue[0].name = Xrm.Page.context.getUserName();

                //Xrm.Page.getAttribute("ed_voucher_printed_by").setValue(setUservalue)

                //// Set Read-Only fields Disabled = false after field values has been updated
                //Xrm.Page.ui.controls.get("ed_voucher_printed").setDisabled(false);
                //Xrm.Page.ui.controls.get("ed_voucher_printed_by").setDisabled(false);

                //Xrm.Page.data.entity.save();
            }
            catch (error) {
                alert("Exception caught in printRefundVoucherReport.\r\n\r\n" + error);
            }
        },

        bulkPrintRefundVoucherReport: function () {
            /// <summary>
            /// Print report. Hardcoded reports.
            /// </summary>
            try {
                debugger;

                // DEFAULT FROM DATE
                var yesterday = new Date();
                yesterday.setDate(yesterday.getDate() - 1);

                if (!yesterday.getDay()) {
                    yesterday.setDate(yesterday.getDate() - 2);
                }

                // DEFAULT TO DATE
                var today = new Date();

                var dateFromString = prompt("Datum från:", yesterday.getFullYear() + "-" + ('0' + (yesterday.getMonth() + 1)).slice(-2) + "-" + ('0' + yesterday.getDate()).slice(-2));
                var dateToString = prompt("Datum till:", today.getFullYear() + "-" + ('0' + (today.getMonth() + 1)).slice(-2) + "-" + ('0' + today.getDate()).slice(-2));

                var dateFrom = Endeavor.Skanetrafiken.cgi_refund.getDateFromString(dateFromString);
                var dateTo = Endeavor.Skanetrafiken.cgi_refund.getDateFromString(dateToString);

                if (dateFrom && dateTo) {

                    var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "cgi_refundSet?$select=cgi_refundId&$filter=CreatedOn gt datetime'" + dateFrom.toISOString() + "' and CreatedOn le datetime'" + dateTo.toISOString() + "'";
                    var results = Endeavor.Common.Data.fetchJSONResults(url);

                    var guids = [];
                    if (!results || results.length == 0) {
                        Xrm.Utility.alertDialog("Inga värdebevis hittades.")
                    }
                    else {
                        var i = 0;
                        while (i < results.length) {
                            guids.push(results[i].cgi_refundId);
                            i++;
                        }

                        //var arrReportSession = Endeavor.Skanetrafiken.cgi_refund.executeReport();

                        //Endeavor.Skanetrafiken.cgi_refund.convertResponseToPDF(arrReportSession);

                        i++;
                        Xrm.Utility.alertDialog(i + " värdebevis utskrivna!")
                    }
                }
                else {
                    Xrm.Utility.alertDialog("Felaktigt datumformat.")
                }

            }
            catch (error) {
                alert("Exception caught in bulkPrintRefundVoucherReport.\r\n\r\n" + error);
            }
        },

        getDateFromString: function (datestring) {
            
            var formattedstring = "";

            for (var i = 0; i < datestring.length; i++) {
                if (Number.isNaN(parseInt(datestring[i])) == false) {
                    formattedstring = formattedstring + datestring[i];
                }
            }

            if (formattedstring.length < 8) {
                return null;
            }
            else {

                var yyyy = parseInt(formattedstring.substring(0, 4));
                var mm = parseInt(formattedstring.substring(4, 6)) - 1;
                var dd = parseInt(formattedstring.substring(6, 8));

                var date = new Date(yyyy, mm, dd, 0, 0, 0, 0);
                if (Number.isNaN(date.getTime())) {
                    date = new Date();
                }  

                return date;
            }
        },

        executeReport: function () {
            // GUID of SSRS report in CRM.
            var reportGuid = "8a3e7a71-6504-e511-80dc-0050569010ad";

            //Name of the report. Note: .RDL needs to be specified.
            var reportName = "Beslut Vidarefakturering.rdl";

            // URL of the report server which will execute report and generate response.
            var pth = Xrm.Page.context.getClientUrl() + "/CRMReports/rsviewer/QuirksReportViewer.aspx";

            //This is the filter that is passed to pre-filtered report. It passes GUID of the record using Xrm.Page.data.entity.getId() method.
            //This filter shows example for quote report. If you want to pass ID of any other entity, you will need to specify respective entity name.
            var reportPrefilter = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'><entity name='cgi_refund'><all-attributes /><filter type='and'><condition attribute='cgi_refundid' operator='eq' value='" + Xrm.Page.data.entity.getId() + "' /></filter></entity></fetch>";

            //Prepare query to execute report.
            var query = "id=%7B" + reportGuid +
                "%7D&uniquename=" + Xrm.Page.context.getOrgUniqueName()
                + "&iscustomreport=true&reportnameonsrs=&reportName=" + reportName
                //+ "&isScheduledReport=false&p:cgi_refund=" + reportPrefilter;
                + "&isScheduledReport=false";

            //Prepare request object to execute the report.
            var retrieveEntityReq = new XMLHttpRequest();
            retrieveEntityReq.open("POST", pth, false);
            retrieveEntityReq.setRequestHeader("Accept", "*/*");
            retrieveEntityReq.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            //This statement runs the query and executes the report synchronously.
            retrieveEntityReq.send(query);

            //These variables captures the response and returns the response in an array.
            var x = retrieveEntityReq.responseText.lastIndexOf("ReportSession=");
            var y = retrieveEntityReq.responseText.lastIndexOf("ControlID=");

            var ret = new Array();
            ret[0] = retrieveEntityReq.responseText.substr(x + 14, 24);
            ret[1] = retrieveEntityReq.responseText.substr(x + 10, 32);

            //Returns the response as an Array.

            return ret;
        },

        convertResponseToPDF: function (arrResponseSession) {
            //Create query string that will be passed to Report Server to generate PDF version of report response.
            var pth = Xrm.Page.context.getClientUrl() + "/Reserved.ReportViewerWebControl.axd?ReportSession=" + arrResponseSession[0] + "&Culture=1033&CultureOverrides=True&UICulture=1033&UICultureOverrides=True&ReportStack=1&ControlID=" + arrResponseSession[1] + "&OpType=Export&FileName=Public&ContentDisposition=OnlyHtmlInline&Format=PDF";

            //Create request object that will be called to convert the response in PDF base 64 string.
            var retrieveEntityReq = new XMLHttpRequest();
            retrieveEntityReq.open("GET", pth, true);
            retrieveEntityReq.setRequestHeader("Accept", "*/*");
            retrieveEntityReq.responseType = "arraybuffer";

            retrieveEntityReq.onreadystatechange = function () { // This is the callback function.

                if (retrieveEntityReq.readyState == 4 && retrieveEntityReq.status == 200) {

                    var binary = "";

                    var bytes = new Uint8Array(this.response);

                    for (var i = 0; i < bytes.byteLength; i++) {

                        binary += String.fromCharCode(bytes[i]);

                    }

                    //This is the base 64 PDF formatted string and is ready to pass to the action as an input parameter.
                    var base64PDFString = btoa(binary);

                    //4. Call Action and pass base 64 string as an input parameter. That’s it.


                    if (window.navigator && window.navigator.msSaveOrOpenBlob) { // IE workaround
                        var byteCharacters = atob(base64PDFString);
                        var byteNumbers = new Array(byteCharacters.length);
                        for (var i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        var byteArray = new Uint8Array(byteNumbers);
                        var blob = new Blob([byteArray], { type: 'application/pdf' });


                        Endeavor.Skanetrafiken.cgi_refund.openPrintDialogue(blob);


                        //// Saves the PDF to disk
                        //window.navigator.msSaveOrOpenBlob(blob, "testfile.pdf");
                    }
                    else { // much easier if not IE
                        var byteCharacters = atob(base64PDFString);
                        var byteNumbers = new Array(byteCharacters.length);
                        for (var i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        var byteArray = new Uint8Array(byteNumbers);
                        var blob = new Blob([byteArray], { type: 'application/pdf' });


                        Endeavor.Skanetrafiken.cgi_refund.openPrintDialogue(blob);
                    }

                }

            };

            //This statement sends the request for execution asynchronously. Callback function will be called on completion of the request.
            retrieveEntityReq.send();
        },

        openPrintDialogue: function (blob) {

            var file = window.URL.createObjectURL(blob);

            // create new printframe
            var iFrame = $('<iframe></iframe>');
            iFrame
              .attr("id", "printframe")
              .attr("name", "printframe")
              .attr("src", "about:blank")
              .css("width", "0")
              .css("height", "0")
              .css("position", "absolute")
              .css("left", "-9999px")
              .appendTo($("body:first"));

            // load printframe
            if (iFrame != null && file != null) {
                iFrame.attr('src', file);
                iFrame.load(function () {

                    // print the frame
                    var tempFrame = $('#printframe')[0];
                    var tempFrameWindow = tempFrame.contentWindow ? tempFrame.contentWindow : tempFrame.contentDocument.defaultView;

                    tempFrameWindow.onbeforeprint = function () {
                        alert('This will be called before the user prints.');
                    };
                    tempFrameWindow.onafterprint = function () {
                        alert('This will be called after the user prints');
                    };

                    tempFrameWindow.focus();
                    tempFrameWindow.print();
                });
            }
        },

    };
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