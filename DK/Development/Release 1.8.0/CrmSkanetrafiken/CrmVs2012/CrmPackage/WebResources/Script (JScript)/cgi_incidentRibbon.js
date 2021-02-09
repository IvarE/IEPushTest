if (typeof (CGISweden) == "undefined") { CGISweden = {}; }
if (typeof (CGISweden.incident) == "undefined") { CGISweden.incident = {}; }
if (typeof (CGISweden.incident.Ribbon) == "undefined") { CGISweden.incident.Ribbon = {}; }


// *******************************************************
// Entity: incident ribbon 
// *******************************************************

CGISweden.incident.Ribbon =
{
    trafficInfoExists: function () {

    },

    resolveCase: function (caseid) {
        try {
            if (CGISweden.formscriptfunctions.MandatoryPopulated() == false) {
                alert("Ett eller flera obligatoriska fält saknar värde!");
                return;
            }

            //START validera trafikinfo START
            //fortsätt valideringen endast ifall användaren INTE explicit angivit att ingen trafikinformation ska registreras
            if (Xrm.Page.getAttribute("cgi_notravelinfo").getValue() != 1) {

                //fortsätt med valideringen endast om trafikinfo saknas
                var noOfTI = CGISweden.odata.GetTravelInfoForCase(caseid);
                if (noOfTI === 0) {

                    //fortsätt med valideringen endast ifall trafikinformation krävs för aktuell kategori
                    var IsTravelInformationRequired = false;
                    var cgi_casdet_row1_cat3idLookup = Xrm.Page.getAttribute("cgi_casdet_row1_cat3id").getValue();
                    IsTravelInformationRequired = CGISweden.odata.IsTravelInformationRequired(cgi_casdet_row1_cat3idLookup[0].id);
                    if (IsTravelInformationRequired) {

                        //ge användaren en möljighet att avsluta ärendet ändå utan trafikinfo genom att klicka ok
                        if (confirm("Ärenden i kategori " + cgi_casdet_row1_cat3idLookup[0].name + " förväntas innehålla trafikinformation, vilken saknas i detta ärende. Vill du verkligen avsluta ärendet utan trafikinformation? ") == true) {
                            //ange explicit att ärendet ska sparas utan trafikinfo. Annars kommer en plugin förhindra att det avslutas utan trafikinformation
                            Xrm.Page.getAttribute("cgi_notravelinfo").setValue(1);
                        }
                        else {
                            //genom att avbryta exekveringen undviks att ärendet att avslutas
                            return;
                        }
                    }

                }
            }
            //END validera trafikinfo END


            if (CGISweden.formscriptfunctions.GetDisabledField("cgi_casesolved") == true) {
                alert("Du saknar behörighet att avsluta detta ärende!");
                return;
            }


            var _incidentstagecode = CGISweden.formscriptfunctions.GetValue("incidentstagecode");

            if (_incidentstagecode != 285050002) {
                Xrm.Page.getAttribute("incidentstagecode").setValue(285050004);
                CGISweden.formscriptfunctions.SetValue("cgi_case_reopen", "1");
                CGISweden.formscriptfunctions.SetValue("cgi_casesolved", "2");
                CGISweden.formscriptfunctions.SaveAndCloseEntity();
            }
            else {
                if (confirm("Ärendet har status Obesvarad kund. Vill du avsluta ärendet ändå?") == true) {
                    Xrm.Page.getAttribute("incidentstagecode").setValue(285050004);
                    CGISweden.formscriptfunctions.SetValue("cgi_case_reopen", "1");
                    CGISweden.formscriptfunctions.SetValue("cgi_casesolved", "2");
                    CGISweden.formscriptfunctions.SaveAndCloseEntity();
                }
            }


        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.resolveCase\n\n" + e.Message);
        }
    },

    sendAttentionEmail: function () {
        try {

            if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty() == true) {
                alert("Spara ärendet innan mail skickas!");
                return;
            }

            var parameters = CGISweden.incident.Ribbon.setArgs();
            //type of email
            parameters += "&cgi_attention=true";

            // Open the window.
            var url = "/main.aspx?etn=email&pagetype=entityrecord";
            var features = "location=no,menubar=no,status=no,toolbar=no";

            url = Xrm.Page.context.prependOrgName(url);
            url = url + "&extraqs=" + encodeURIComponent(parameters);
            window.open(url, "_blank", features, false);

        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.sendAttentionEmail\n\n" + e.Message);
        }
    },

    sendRemitteceEmail: function () {
        try {

            if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty() == true) {
                alert("Spara ärendet innan mail skickas!");
                return;
            }

            var parameters = CGISweden.incident.Ribbon.setArgs();
            //type of email
            parameters += "&cgi_remittance=true";

            var url = "/main.aspx?etn=email&pagetype=entityrecord";
            var features = "location=no,menubar=no,status=no,toolbar=no";

            url = Xrm.Page.context.prependOrgName(url);
            url = url + "&extraqs=" + encodeURIComponent(parameters);
            window.open(url, "_blank", features, false);

        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.sendRemitteceEmail\n\n" + e.Message);
        }
    },

    sendCustomermail: function () {
        try {

            if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty() == true) {
                alert("Spara ärendet innan e-post kan skickas!");
                return;
            }

            var argValues = "";
            argValues += CGISweden.incident.Ribbon.setArgs();
            if (CGISweden.formscriptfunctions.GetValue("cgi_accountid") == null && CGISweden.formscriptfunctions.GetValue("cgi_contactid") == null) {
                alert("Ingen kund är kopplad till ärendet!");
                return;
            }
            else
            {
                argValues += CGISweden.incident.Ribbon.setArgsCustomer();
                //type of email
                argValues += "&cgi_button_customer=1";
            }

            // Open the window.
            var url = "/main.aspx?etn=email&pagetype=entityrecord";
            var features = "location=no,menubar=no,status=no,toolbar=no";

            url = Xrm.Page.context.prependOrgName(url);
            url = url + "&extraqs=" + encodeURIComponent(argValues);
            window.open(url, "_blank", features, false);
        }
        catch (e) {
            alert("Ett fel inträffade i CGISweden.incidentRibbon.sendCustomermail\n\n" + e.Message);
        }
    },

    setParameters: function () {
        var _returnValue = {};
        try {
            //regardingobjectid
            _returnValue["parameter_regardingid"] = CGISweden.formscriptfunctions.GetObjectID();
            _returnValue["parameter_regardingname"] = CGISweden.formscriptfunctions.GetValue("title");
            _returnValue["parameter_regardingtype"] = "incident";
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.setParameters\n\n" + e.Message);
        }
        return _returnValue;
    },

    setArgs: function () {
        try {
            var _returnValue = "parameter_regardingid=" + CGISweden.formscriptfunctions.GetObjectID();
            _returnValue += "&parameter_regardingname=" + CGISweden.formscriptfunctions.GetValue("title");
            _returnValue += "&parameter_regardingtype=incident";

        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.setArgs\n\n" + e.Message);
        }
        return _returnValue;
    },

    setArgsCustomer: function () {
        try {
            var parameterValue = "";
            var _cgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid"),
            _representative = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid");

            // If the customer as a representative all communication should go through the representative.
            if (_representative != null) {
                parameterValue += "&parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_representativid");
                parameterValue += "&parameter_customername=" + CGISweden.formscriptfunctions.GetLookupName("cgi_representativid");
                parameterValue += "&parameter_customertype=cgi_representative";
            }
            else if (_cgi_accountid != null) {
                //cgi_accountid
                parameterValue += "&parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_accountid");
                parameterValue += "&parameter_customername" + CGISweden.formscriptfunctions.GetLookupName("cgi_accountid");
                parameterValue += "&parameter_customertype=account";
            }
            else {
                parameterValue += "&parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_contactid");
                parameterValue += "&parameter_customername=" + CGISweden.formscriptfunctions.GetLookupName("cgi_contactid");
                parameterValue += "&parameter_customertype=contact";
            }

            return parameterValue;
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.setArgsCustomer\n\n" + e.Message);
        }
        return param;
    },

    setParametersCustomer: function (param) {
        try {
            var _cgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid"),
                _representative = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid");

            // If the customer as a representative all communication should go through the representative.
            if (_representative != null) {
                param["parameter_customerid"] = _representative;
                param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_representativid");
                param["parameter_customertype"] = "cgi_representative";
            }
            else if (_cgi_accountid != null) {
                //cgi_accountid
                param["parameter_customerid"] = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid");
                param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_accountid");
                param["parameter_customertype"] = "account";
            }
            else {
                param["parameter_customerid"] = CGISweden.formscriptfunctions.GetLookupid("cgi_contactid");
                param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_contactid");
                param["parameter_customertype"] = "contact";
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.setParameters\n\n" + e.Message);
        }
        return param;
    },

    setParametersUser: function () {
        alert("Namnet på kön " + queueName);
    },

    validateEmail: function () {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

        var email = "";
        var email2 = "";
        var cgi_representativid = "";

        try {
            email = CGISweden.formscriptfunctions.GetValue("cgi_customer_email");
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.validateEmail:1\n\n" + e.Message);
            return false;
        }

        try {
            cgi_representativid = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid");
        }
        catch (e) {

        }

        if (cgi_representativid != null && cgi_representativid != "") {
            try {
                //This is done synchronously because it is part of a validation process
                var serverUrl;

                if (Xrm.Page.context.getClientUrl !== undefined) {
                    serverUrl = Xrm.Page.context.getClientUrl();
                } else {
                    serverUrl = Xrm.Page.context.getServerUrl();
                }
                var ODataPath = serverUrl + "/XRMServices/2011/OrganizationData.svc";

                var _options = "$select=cgi_Email&";
                var _filter = "$filter=cgi_representativeId eq guid'" + cgi_representativid + "'";
                var _odata = _options + _filter;

                var cgi_categorydetailQueryUrl = ODataPath + "/cgi_representativeSet?" + _odata;

                var ODataRequest = new XMLHttpRequest();
                ODataRequest.open("GET", cgi_categorydetailQueryUrl, false); // false = synchronous request
                ODataRequest.setRequestHeader("Accept", "application/json");
                ODataRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                ODataRequest.send();

                if (ODataRequest.status === 200) {
                    var parsedResults = JSON.parse(ODataRequest.responseText).d;
                    if (parsedResults != null && parsedResults.results != null && parsedResults.results.length > 0) {
                        var cgi_representative = parsedResults.results[0];
                        if (cgi_representative["cgi_Email"] == null || cgi_representative["cgi_Email"] == "")
                            email2 = cgi_representative["cgi_Email"];
                    }
                }
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.validateEmail:2\n\n" + e.Message);
            }
        }

        if (re.test(email) || re.test(email2)) {
            return false;
        }

        return true;
    },

    openBombApp: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetBOMBUrlFromSetting(_currentdate, CGISweden.incident.Ribbon.openBombApp_callback, CGISweden.incident.Ribbon.openBombApp_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.openBombApp\n\n" + e.Message);
        }
    },

    openBombApp_complete: function () { },

    openBombApp_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen url definierad för BOMB!");
            }
            else {
                var _features = "status=1,toolbar=1,location=1,menubar=1,directories=1,resizable=1,scrollbars=1";
                var _url = result[0].cgi_BOMBUrl;
                var _param = CGISweden.formscriptfunctions.GetValue("cgi_bombmobilenumber")
                if (_param == null) {
                    alert("Inget mobilnummer är angivet!");
                    return;
                }

                var _openurl = _url + _param;
                window.open(_openurl, "_blank", _features, true);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.openBombApp_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    chechIfAnyFieldIsDirty: function () {
        try {
            return Xrm.Page.data.entity.getIsDirty();
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.chechIfAnyFieldIsDirty\n\n" + e.Message);
        }
    }

};


