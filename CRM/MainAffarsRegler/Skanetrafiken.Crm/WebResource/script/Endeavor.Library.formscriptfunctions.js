if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

Endeavor.formscriptfunctions = {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Add a onchange function to a field. 
    SetOnChangeFunction: function (name, functionname, formContext) {
        try {
            formContext.data.entity.attributes.get(name).addOnChange(functionname);
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetOnChangeFunction\n\n" + e.message);
        }
    },
    cleanIdField: function (id) {
        id = id.replace("{", "");
        id = id.replace("}", "");
        return id;
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Function to return parameter types for an action

    getParameterType: function (typeField) {
        if (typeField == "string")
            return "Edm.String";
        else if (typeField == "int")
            return "Edm.Int32"
        else if (typeField == "decimal")
            return "Edm.Decimal"
        else if (typeField == "float")
            return "Edm.Double"
        else if (typeField == "bool")
            return "Edm.Boolean"
        else if (typeField == "datetime")
            return "Edm.DateTimeOffset"
        else if (typeField == "optionset")
            return "Edm.Int32"
    },
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Save entity
    SaveEntity: function (formContext) {
        try {
            formContext.data.entity.save();
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SaveEntity\n\n" + e.message);
        }
    },

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Save and close entity
    SaveAndCloseEntity: function (formContext) {
        try {
            formContext.data.entity.save("saveandclose");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SaveAndCloseEntity\n\n" + e.message);
        }
    },

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get objectid of entity
    GetObjectID: function (formContext) {
        try {
            return formContext.data.entity.getId();
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetObjectID\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get formtype.
    GetFormType: function (formContext) {
        var _returnvalue = null;
        try {
            _returnvalue = formContext.ui.getFormType();
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetFormType\n\n" + e.message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display tab.
    HideOrDisplayTab: function (name, visible, formContext) {
        try {
            var _tab = formContext.ui.tabs.get(name);
            if (_tab)
                _tab.setVisible(visible);
            else
                console.log("Endeavor.formscriptfunctions.HideOrDisplayTab: tab " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.HideOrDisplayTab\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //expanded or collapsed
    SetDisplayState: function (name, state, formContext) {
        try {
            var _tab = formContext.ui.tabs.get(name);
            if (_tab)
                _tab.setDisplayState(state);
            else
                console.log("Endeavor.formscriptfunctions.SetDisplayState: tab " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetDisplayState\n\n" + e.message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display section.
    HideOrDisplaySection: function (tabname, sectionname, visible, formContext) {
        try {
            var _tab = formContext.ui.tabs.get(tabname);
            if (_tab) {
                var _section = _tab.sections.get(sectionname);
                if (_section)
                    _section.setVisible(visible);
                else
                    console.log("Endeavor.formscriptfunctions.HideOrDisplaySection: section " + sectionname + " is not on the form.");
            }
            else
                console.log("Endeavor.formscriptfunctions.HideOrDisplaySection: tab " + tabname + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.HideOrDisplaySection\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Hide or display field.
    HideOrDisplayField: function (name, visible, formContext) {
        try {
            var _control = formContext.getControl(name);
            if (_control)
                _control.setVisible(visible);
            else
                console.log("Endeavor.formscriptfunctions.HideOrDisplayField: control " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.HideOrDisplayField\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set value to a field
    SetValue: function (name, value, formContext) {
        try {
            var _attribute = formContext.data.entity.attributes.get(name);
            if (_attribute)
                _attribute.setValue(value);
            else
                console.log("Endeavor.formscriptfunctions.SetValue: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetValue\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get value from a field
    GetValue: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _attribute = formContext.data.entity.attributes.get(name);
            if (_attribute)
                _returnvalue = _attribute.getValue();
            else
                console.log("Endeavor.formscriptfunctions.GetValue: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetValue\n\n" + e.message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set control label.
    SetLabel: function (name, label, formContext) {
        try {
            var _field = formContext.ui.controls.get(name);
            if(_field)
                _field.setLabel(label);
            else
                console.log("Endeavor.formscriptfunctions.SetLabel: control " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetLabel\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set field enabled or disabled.
    SetState: function (name, state, formContext) {
        try {
            var _field = formContext.ui.controls.get(name);
            if (_field)
                _field.setDisabled(state);
            else
                console.log("Endeavor.formscriptfunctions.SetState: control " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetState\n\n" + e.message);
        }
    },

    SetSubmitModeAlways: function (name, formContext) {
        try {
            var _attribute = formContext.getAttribute(name);
            if (_attribute)
                _attribute.setSubmitMode("always");
            else
                console.log("Endeavor.formscriptfunctions.SetSubmitModeAlways: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetSubmitModeAlways\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get control state. Enabled or disabled
    GetState: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _control = formContext.getControl(name);
            if (_control)
                _returnvalue = _control.getDisabled();
            else
                console.log("Endeavor.formscriptfunctions.GetState: control " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetState\n\n" + e.message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // none, required, recommended
    SetRequiredLevel: function (name, state, formContext) {
        try {
            var _attribute = formContext.getAttribute(name);
            if (_attribute)
                _attribute.setRequiredLevel(state);
            else
                console.log("Endeavor.formscriptfunctions.SetRequiredLevel: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetRequiredLevel\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set submitmode for field
    // always, never, dirty
    SetSubmitMode: function (name, state, formContext) {
        try {
            var _attribute = formContext.data.entity.attributes.get(name);
            if (_attribute)
                _attribute.setSubmitMode(state);
            else
                console.log("Endeavor.formscriptfunctions.SetSubmitMode: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetSubmitMode\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set value to lookupfield.
    SetLookup: function (attributeName, entityType, id, name, formContext) {
        try {
            var _attribute = formContext.getAttribute(attributeName);
            if (_attribute) {
                var _item = new Object();
                _item.id = id;
                _item.name = name;
                _item.entityType = entityType;
                var _lookup = new Array();
                _lookup[0] = _item;
                _attribute.setValue(_lookup);
            }
            else
                console.log("Endeavor.formscriptfunctions.SetLookup: attribute " + attributeName + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetLookup\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get id value from lookupfield.
    GetLookupid: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _id = null;
            var _attribute = formContext.getAttribute(name);
            if (_attribute) {
                var _lookup = _attribute.getValue();
                if (_lookup != null)
                    _id = _lookup[0].id;
                return _id;
            }
            else
                console.log("Endeavor.formscriptfunctions.GetLookupid: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetLookupid\n\n" + e.message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get name value from lookupfield.
    GetLookupName: function (name, formContext) {
        var _returnvalue = null;
        try {
            var _idName = null;
            var _attribute = formContext.getAttribute(name);
            if (_attribute) {
                var _lookup = _attribute.getValue();
                if (_lookup != null)
                    _idName = _lookup[0].name;
                return _idName;
            }
            else
                console.log("Endeavor.formscriptfunctions.GetLookupName: attribute " + name + " is not on the form.");
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetLookupName\n\n" + e.message);
        }
        return _returnvalue;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set focus on field.
    SetFocusOnField: function (name, formContext) {
        try {
            formContext.ui.controls.get(name).setFocus();
        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.SetFocusOnField\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Check if field is disabled.
    GetDisabledField: function (arg, formContext) {
        try {
            return formContext.getControl(arg).getDisabled();
        } catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.GetDisabledField\n\n" + e.message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Check if mandatory fields is not empty
    MandatoryPopulated: function (formContext) {
        populated = true;

        formContext.getAttribute(function (attribute, index) {
            if (attribute.getRequiredLevel() == "required") {
                if (attribute.getValue() === null) {
                    populated = false;
                }
            }
        });

        return populated;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //get date of today. Format YYYY-MM-DDTHH:MM:SS
    GetDateTime: function () {
        var now = new Date();
        var year = now.getFullYear();
        var month = now.getMonth() + 1;
        var day = now.getDate();
        var hour = now.getHours();
        var minute = now.getMinutes();
        var second = now.getSeconds();
        if (month.toString().length == 1) {
            var month = '0' + month;
        }
        if (day.toString().length == 1) {
            var day = '0' + day;
        }
        if (hour.toString().length == 1) {
            var hour = '0' + hour;
        }
        if (minute.toString().length == 1) {
            var minute = '0' + minute;
        }
        if (second.toString().length == 1) {
            var second = '0' + second;
        }
        var dateTime = year + '-' + month + '-' + day + 'T' + hour + ':' + minute + ':' + second;
        return dateTime;

    },

    AlertCustomDialog: function (msgText) {

        var message = { confirmButtonLabel: "Ok", text: msgText };
        var alertOptions = { height: 150, width: 280 };

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        Xrm.Navigation.openAlertDialog(message, alertOptions).then(
            function success(result) {
                console.log("Alert dialog closed");
            },
            function (error) {
                console.log(error.message);
            }
        );
    },

    ConfirmCustomDialog: function (msgText, sucesscallBack) {

        var confirmStrings = { text: msgText, title: "Confirmation Dialog", "cancelButtonLabel": "Cancel", confirmButtonLabel: "Confirm" };
        var confirmOptions = { height: 200, width: 500 };

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {

                    console.log("Dialog closed using OK button.");
                    sucesscallBack();
                }
                else
                    console.log("Dialog closed using Cancel button or X.");
            });
    },

    ErrorCustomDialog: function (details, message) {

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        Xrm.Navigation.openErrorDialog({ details: details, message: message }).then(
            function (success) {
                console.log(success);
            },
            function (error) {
                console.log(error);
            });
    },

    OpenCustomForm: function (entityName, entityId) {

        var entityFormOptions = {};
        entityFormOptions["entityName"] = entityName;
        entityFormOptions["entityId"] = entityId;

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        // Open the form.
        Xrm.Navigation.openForm(entityFormOptions).then(
            function (success) {
                console.log(success);
            },
            function (error) {
                console.log(error);
                Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
            });
    },

    LoadXrmExecutionContext: function (webResourceName, formContext) {

        try {
            
            var wrControl = formContext.getControl(webResourceName);
            if (wrControl) {
                wrControl.getContentWindow().then(contentWindow => {
                    let numberOfCalls = 0;
                    let interval = setInterval(() => {
                        if (typeof contentWindow.setClientApiContext !== "undefined") {
                            clearInterval(interval);
                            contentWindow.setClientApiContext(Xrm, formContext);
                        }
                        else
                            //stop interval after 1 minute
                            if (++numberOfCalls > 600) {
                                clearInterval(interval);
                                throw new Error("Content Window failed to initialize.");
                            }
                    }, 100);
                });
            }
            else
                alert("Fel i Endeavor.formscriptfunctions.LoadXrmExecutionContext\n\n Web Resource " + webResourceName + " not found.");

        }
        catch (e) {
            alert("Fel i Endeavor.formscriptfunctions.LoadXrmExecutionContext\n\n" + e.message);
        }
    },

    callAction: function (actionName, entityName, targetId, inputParameters, sucessCallback, errorCallback) {

        var target = {};
        target.entityType = entityName;
        target.id = targetId;

        var parameterTypes = {};
        parameterTypes["entity"] = { "typeName": "mscrm." + entityName, "structuralProperty": 5 };

        if (inputParameters != null)
            for (var i = 0; i < inputParameters.length; i++) {
                var parameter = inputParameters[i];

                req[parameter.Field] = parameter.Value;
                parameterTypes[parameter.Field] = { "typeName": parameter.TypeName, "structuralProperty": parameter.StructuralProperty };
            }

        var req = {};
        req.entity = target;
        req.getMetadata = function () {
            return {
                boundParameter: "entity",
                parameterTypes: parameterTypes,
                operationType: 0,
                operationName: actionName
            };
        };

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
    },

    callGlobalAction: function (actionName, inputParameters, sucessCallback, errorCallback) {

        var req = {};

        var parameterTypes = {};
        if (inputParameters != null)
            for (var i = 0; i < inputParameters.length; i++) {
                var parameter = inputParameters[i];

                req[parameter.Field] = parameter.Value;
                parameterTypes[parameter.Field] = { "typeName": parameter.TypeName, "structuralProperty": parameter.StructuralProperty };
            }

        req.getMetadata = function () {

            return {
                boundParameter: null,
                parameterTypes: parameterTypes,
                operationType: 0,
                operationName: actionName
            };
        };

        if (typeof (Xrm) == "undefined")
            Xrm = parent.Xrm;

        Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
    },

    callGlobalActionRibbon: function (Xrm, actionName, inputParameters, sucessCallback, errorCallback) {

        var req = {};

        var parameterTypes = {};
        if (inputParameters != null)
            for (var i = 0; i < inputParameters.length; i++) {
                var parameter = inputParameters[i];

                req[parameter.Field] = parameter.Value;
                parameterTypes[parameter.Field] = { "typeName": parameter.TypeName, "structuralProperty": parameter.StructuralProperty };
            }

        req.getMetadata = function () {

            return {
                boundParameter: null,
                parameterTypes: parameterTypes,
                operationType: 0,
                operationName: actionName
            };
        };

        Xrm.WebApi.online.execute(req).then(sucessCallback, errorCallback);
    },

    fetchJSONResults: function (url, max_records) {
        /// <summary>
        /// Get records from server
        /// </summary>
        /// <param name="url">The Query to execute</param>
        /// <param name="max_records">Nr of records to process. If null max 5000 will be retrieved.</param>
        /// <returns type="">The Records.</returns>

        var value = null;
        var request = new XMLHttpRequest();
        var nextUrl = null;
        var totalRecords = new Array();
        request.open("GET", url, false);
        request.setRequestHeader("Accept", "application/json");
        request.setRequestHeader("contentType", "application/json; charset=utf-8");
        request.send();

        try {
            if (max_records == null)
                max_records = 5000;

            
            var object = JSON.parse(request.responseText);
            value = object.value;
            nextUrl = object["@odata.nextLink"];

            if (value == null)
                return object;

            // Add records to total records
            for (var i = 0; i < value.length; i++) {

                if (totalRecords.length >= max_records)
                    return totalRecords;
                totalRecords.push(value[i]);
            }

            // Is there more records to fetch?
            if (nextUrl != null) {
                var nextRecords = Endeavor.Common.Data.fetchJSONResults(nextUrl, max_records);
                if (nextRecords != null && nextRecords.length > 0) {
                    for (var i = 0; i < nextRecords.length; i++) {
                        if (totalRecords.length >= max_records)
                            return totalRecords;
                        totalRecords.push(nextRecords[i]);
                    }
                }
            }

            return totalRecords;
        }
        catch (error) {
            value = null;
            alert("There was an error trying to access the CRM server. Please contact your administrator. URL:" + url);
            return null;
        }
    },

    executeFetchGetCount: function (originalFetch, entityname) {
        var count = 0;
        var fetch = encodeURI(originalFetch);
        var query = entityname + "?fetchXml=" + fetch;

        var globalContext = Xrm.Utility.getGlobalContext();
        var serverURL = globalContext.getClientUrl();

        var req = new XMLHttpRequest();
        req.open("GET", serverURL + "/api/data/v9.0/" + query, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.onreadystatechange = function () {
            if (this.readyState == 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status == 200) {
                    var data = JSON.parse(this.response);
                    if (data != null) {
                        count = data.value.length;
                    }
                }
                else {
                    var error = JSON.parse(this.response).error;
                    alert(error.message);
                }
            }
        };
        req.send();
        return count;
    }
}