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

            var parameters = {};
            parameters = CGISweden.incident.Ribbon.setParameters();
            //type of email
            parameters["cgi_attention"] = "true";

            // Open the window.
            Xrm.Utility.openEntityForm("email", null, parameters);

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

            var parameters = {};
            parameters = CGISweden.incident.Ribbon.setParameters();
            //type of email
            parameters["cgi_remittance"] = "true";

            // Open the window.
            Xrm.Utility.openEntityForm("email", null, parameters);

        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.sendRemitteceEmail\n\n" + e.Message);
        }
    },

    sendCustomermail: function () {
        try {

            if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty() == true) {
                alert("Spara ärendet innan mail skickas!");
                return;
            }

            var parameters = {};
            parameters = CGISweden.incident.Ribbon.setParameters();
            if (CGISweden.formscriptfunctions.GetValue("cgi_accountid") == null && CGISweden.formscriptfunctions.GetValue("cgi_contactid") == null) {
                alert("Ingen kund är kopplad till ärendet!");
                return;
            } else {
                parameters = CGISweden.incident.Ribbon.setParametersCustomer(parameters);
                //type of email
                parameters["cgi_button_customer"] = "1";
            }

            if (CGISweden.incident.Ribbon.validateEmail() == false) {
                alert("Ärendet saknar giltig epost!");
                return;
            }

            // Open the window.
            Xrm.Utility.openEntityForm("email", null, parameters);

        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.sendRemitteceEmail\n\n" + e.Message);
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

        try {
            var email = CGISweden.formscriptfunctions.GetValue("cgi_customer_email");

            if (email == "" || !re.test(email)) {
                return false;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentRibbon.validateEmail\n\n" + e.Message);
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


