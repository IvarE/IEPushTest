if (typeof(SDK) == "undefined") {
    SDK = {
        __namespace: true
    };
}
SDK.REST = {
    _context: function () {
        if (typeof GetGlobalContext != "undefined") {
            return GetGlobalContext();
        }
        else {
            if (typeof Xrm != "undefined") {
                return Xrm.Page.context;
            }
            else {
                throw new Error("Context is not available.");
            }
        }
    },
    _getServerUrl: function () {
        var url;
        if (this._context().getClientUrl) {    // Post 2011-UR12
            url = this._context().getClientUrl();
        }
        else {// Pre 2011-UR12
            url = this._context().getServerUrl();
        }

        if (url.match(/\/$/)) {
            url = url.substring(0, url.length - 1);
        }

        return url;
    },
    _ODataPath: function () {
        return this._getServerUrl() + "/XRMServices/2011/OrganizationData.svc/";
    },
    _errorHandler: function (req) {
        return new Error("Error : " + req.status + ": " + req.statusText + ": " + JSON.parse(req.responseText).error.message.value);
    },
    _dateReviver: function (key, value) {
        var a;
        if (typeof value === 'string') {
            a = /Date\(([-+]?\d+)\)/.exec(value);
            if (a) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
            }
        }
        return value;
    },
    callformGridRibbon: function (message, value) {
        var sUrl = "/" + SDK.REST._context().getOrgUniqueName() + "/WebResources/ic_TestHTML";
        var w = 350;
        var h = 200;
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        window.open(sUrl, "", "resizable=no,status=no,scrollbars=no,toolbars=no,menubar=no,location=no,width=400,height=150,top=" + top + ",left=" + left);
    },
    _parameterCheck: function (parameter, message) {
        if ((typeof parameter === "undefined") || parameter === null) {
            throw new Error(message);
        }
    },
    _stringParameterCheck: function (parameter, message) {
        if (typeof parameter != "string") {
            throw new Error(message);
        }
    },
    _callbackParameterCheck: function (callbackParameter, message) {
        if (typeof callbackParameter != "function") {
            throw new Error(message);
        }
    },
    createRecord: function (object, type, successCallback, errorCallback) {
        this._parameterCheck(object, "SDK.REST.createRecord requires the object parameter.");
        this._stringParameterCheck(type, "SDK.REST.createRecord requires the type parameter is a string.");
        this._callbackParameterCheck(successCallback, "SDK.REST.createRecord requires the successCallback is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.createRecord requires the errorCallback is a function.");
        var req = new XMLHttpRequest();
        req.open("POST", this._ODataPath() + type + "Set", false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 201) {
                    var object = eval("[" + req.responseText + "]");
                    successCallback(JSON.parse(this.responseText, SDK.REST._dateReviver).d);
                }
                else {
                    errorCallback(SDK.REST._errorHandler(this));
                }
            }
        };

        if (typeof JSON == "undefined") {
            alert("JSON methods are missing. Are you running in compability mode?");
        }
        req.send(JSON.stringify(object));

    },
    retrieveRecord: function (id, type, select, expand, successCallback, errorCallback) {
        this._stringParameterCheck(id, "SDK.REST.retrieveRecord requires the id parameter is a string.");
        this._stringParameterCheck(type, "SDK.REST.retrieveRecord requires the type parameter is a string.");
        if (select != null) this._stringParameterCheck(select, "SDK.REST.retrieveRecord requires the select parameter is a string.");
        if (expand != null) this._stringParameterCheck(expand, "SDK.REST.retrieveRecord requires the expand parameter is a string.");
        this._callbackParameterCheck(successCallback, "SDK.REST.retrieveRecord requires the successCallback parameter is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.retrieveRecord requires the errorCallback parameter is a function.");
        var systemQueryOptions = "";
        if (select != null || expand != null) {
            systemQueryOptions = "?";
            if (select != null) {
                var selectString = "$select=" + select;
                if (expand != null) {
                    selectString = selectString + "," + expand;
                }
                systemQueryOptions = systemQueryOptions + selectString;
            }
            if (expand != null) {
                systemQueryOptions = systemQueryOptions + "&$expand=" + expand;
            }
        }
        var req = new XMLHttpRequest();
        req.open("GET", this._ODataPath() + type + "Set(guid'" + id + "')" + systemQueryOptions, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.send();
        try {
            var object = eval("[" + req.responseText + "]");
            successCallback(object[0].d);
        }
        catch(error) {
            errorCallback(SDK.REST._errorHandler(this));
        }
    },
    retrieveRecordObject: function (id, type, select, expand, successCallback, errorCallback) {
        this._stringParameterCheck(id, "SDK.REST.retrieveRecord requires the id parameter is a string.");
        this._stringParameterCheck(type, "SDK.REST.retrieveRecord requires the type parameter is a string.");
        if (select != null) this._stringParameterCheck(select, "SDK.REST.retrieveRecord requires the select parameter is a string.");
        if (expand != null) this._stringParameterCheck(expand, "SDK.REST.retrieveRecord requires the expand parameter is a string.");
        this._callbackParameterCheck(successCallback, "SDK.REST.retrieveRecord requires the successCallback parameter is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.retrieveRecord requires the errorCallback parameter is a function.");
        var systemQueryOptions = "";
        if (select != null || expand != null) {
            systemQueryOptions = "?";
            if (select != null) {
                var selectString = "$select=" + select;
                if (expand != null) {
                    selectString = selectString + "," + expand;
                }
                systemQueryOptions = systemQueryOptions + selectString;
            }
            if (expand != null) {
                systemQueryOptions = systemQueryOptions + "&$expand=" + expand;
            }
        }
        var req = new XMLHttpRequest();
        req.open("GET", this._ODataPath() + type + "Set(guid'" + id + "')" + systemQueryOptions, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.send();
        try {
            var object = eval("[" + req.responseText + "]");
            successCallback(object[0].d);
            return object[0].d;
        }
        catch(error) {
            errorCallback(SDK.REST._errorHandler(this));
        }
    },
    retrieveRecordFilterObject: function (id, type, filter, expand, successCallback, errorCallback) {
        this._stringParameterCheck(id, "SDK.REST.retrieveRecord requires the id parameter is a string.");
        this._stringParameterCheck(type, "SDK.REST.retrieveRecord requires the type parameter is a string.");
        if (filter != null) this._stringParameterCheck(filter, "SDK.REST.retrieveRecord requires the select parameter is a string.");
        if (expand != null) this._stringParameterCheck(expand, "SDK.REST.retrieveRecord requires the expand parameter is a string.");
        this._callbackParameterCheck(successCallback, "SDK.REST.retrieveRecord requires the successCallback parameter is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.retrieveRecord requires the errorCallback parameter is a function.");
        var systemQueryOptions = "";
        if (filter != null || expand != null) {
            systemQueryOptions = "?";
            if (filter != null) {
                var selectString = "$filter=" + filter + " eq (guid'" + id + "')";
                if (expand != null) {
                    selectString = selectString + "," + expand;
                }
                systemQueryOptions = systemQueryOptions + selectString;
            }
            if (expand != null) {
                systemQueryOptions = systemQueryOptions + "&$expand=" + expand;
            }
        }
        var req = new XMLHttpRequest();
        req.open("GET", this._ODataPath() + type + "Set" + systemQueryOptions, false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.send();
        try {
            var object = eval("[" + req.responseText + "]");
            successCallback(object[0].d);
            return object[0].d.results[0];
        }
        catch(error) {
            errorCallback(SDK.REST._errorHandler(this));
        }
    },
    updateRecord: function (id, object, type, successCallback, errorCallback) {
        this._stringParameterCheck(id, "SDK.REST.updateRecord requires the id parameter.");
        this._parameterCheck(object, "SDK.REST.updateRecord requires the object parameter.");
        this._stringParameterCheck(type, "SDK.REST.updateRecord requires the type parameter.");
        this._callbackParameterCheck(successCallback, "SDK.REST.updateRecord requires the successCallback is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.updateRecord requires the errorCallback is a function.");
        var req = new XMLHttpRequest();
        req.open("POST", this._ODataPath() + type + "Set(guid'" + id + "')", false);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("X-HTTP-Method", "MERGE");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 204 || this.status == 1223) {
                    successCallback();
                }
                else {
                    errorCallback(SDK.REST._errorHandler(this));
                }
            }
        };
        req.send(JSON.stringify(object));
    },
    deleteRecord: function (id, type, successCallback, errorCallback) {
        this._stringParameterCheck(id, "SDK.REST.deleteRecord requires the id parameter.");
        this._stringParameterCheck(type, "SDK.REST.deleteRecord requires the type parameter.");
        this._callbackParameterCheck(successCallback, "SDK.REST.deleteRecord requires the successCallback is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.deleteRecord requires the errorCallback is a function.");
        var req = new XMLHttpRequest();
        req.open("POST", this._ODataPath() + type + "Set(guid'" + id + "')", true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("X-HTTP-Method", "DELETE");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 204 || this.status == 1223) {
                    successCallback();
                }
                else {
                    errorCallback(SDK.REST._errorHandler(this));
                }
            }
        };
        req.send();
    },
    retrieveMultipleRecords: function (type, options, successCallback, errorCallback, OnComplete) {
        this._stringParameterCheck(type, "SDK.REST.retrieveMultipleRecords requires the type parameter is a string.");
        if (options != null) this._stringParameterCheck(options, "SDK.REST.retrieveMultipleRecords requires the options parameter is a string.");
        this._callbackParameterCheck(successCallback, "SDK.REST.retrieveMultipleRecords requires the successCallback parameter is a function.");
        this._callbackParameterCheck(errorCallback, "SDK.REST.retrieveMultipleRecords requires the errorCallback parameter is a function.");
        this._callbackParameterCheck(OnComplete, "SDK.REST.retrieveMultipleRecords requires the OnComplete parameter is a function.");
        var optionsString;
        if (options != null) {
            if (options.charAt(0) != "?") {
                optionsString = "?" + options;
            }
            else {
                optionsString = options;
            }
        }
        var req = new XMLHttpRequest();
        req.open("GET", this._ODataPath() + type + "Set" + optionsString, true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    var returned = JSON.parse(this.responseText, SDK.REST._dateReviver).d;
                    successCallback(returned.results);
                    if (returned.__next != null) {
                        var queryOptions = returned.__next.substring((SDK.REST._ODataPath() + type + "Set").length);
                        SDK.REST.retrieveMultipleRecords(type, queryOptions, successCallback, errorCallback, OnComplete);
                    }
                    else {
                        OnComplete();
                    }
                }
                else {
                    errorCallback(SDK.REST._errorHandler(this));
                }
            }
        };
        req.send();
    },
    createActivityParty: function (account, serviceAppointMent) {
        SDK.REST.parameterCheck(account, "The value passed to the createActivityParty function account parameter is null or undefined.");
        SDK.REST.parameterCheck(serviceAppointMent, "The value passed to the createActivityParty function email parameter is null or undefined.");
        var activityParty = {},
        jsonActivityParty;
        activityParty.PartyId = {
            Id: account.Id,
            LogicalName: "account"
        };
        activityParty.ActivityId = {
            Id: serviceAppointMent.Id,
            LogicalName: "ServiceAppointment"
        };
        activityParty.ParticipationTypeMask = {
            Value: 1
        };
        jsonActivityParty = JSON.stringify(activityParty);
        SDK.REST.performRequest({
            type: "POST",
            url: this._ODataPath + "/ActivityPartySet",
            data: jsonActivityParty,
            success: function (request) {
                var newActivityParty = JSON.parse(request.responseText).d;
                showMessage("ACTION: Created new ActivityParty id:{" + newActivityParty.ActivityPartyId + "}.  The account is now related to the email.");
                deleteAccountAndEmail(account, email);
                showMessage("createActivityParty callback success END");
            },
            error: function (request) {
                errorHandler(request);
                showMessage("createActivityParty callback failure END");
            }
        });
        showMessage("createActivityParty function END");
    },
    parameterCheck: function (parameter, message) {
        if (parameter == null || (typeof parameter == "undefined")) {
            throw new Error(message);
        }
    },
    performRequest: function (settings) {
        SDK.REST.parameterCheck(settings, "The value passed to the performRequest function settings parameter is null or undefined.");
        var request = new XMLHttpRequest();
        request.open(settings.type, settings.url, true);
        request.setRequestHeader("Accept", "application/json");
        if (settings.action != null) {
            request.setRequestHeader("X-HTTP-Method", settings.action);
        }
        request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        request.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 204 || this.status == 1223 || this.status == 201) {
                    settings.success(this);
                }
                else {
                    if (settings.error) {
                        settings.error(this);
                    }
                    else {
                        errorHandler(this);
                    }
                }
            }
        };
        request.send();
    },
    __namespace: true
};