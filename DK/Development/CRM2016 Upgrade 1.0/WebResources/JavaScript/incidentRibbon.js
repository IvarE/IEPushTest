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

        resolveCase: function (caseid, primaryControl) {
            try {
                var formContext = primaryControl;
                if (CGISweden.formscriptfunctions.MandatoryPopulated(formContext) == false) {
                    alert("Ett eller flera obligatoriska fält saknar värde!");
                    return;
                }

                //START validera trafikinfo START
                //fortsätt valideringen endast ifall användaren INTE explicit angivit att ingen trafikinformation ska registreras
                if (formContext.getAttribute("cgi_notravelinfo").getValue() != 1) {

                    //fortsätt med valideringen endast om trafikinfo saknas
                     CGISweden.odata.GetTravelInfoForCase(caseid, formContext);
                }
                //END validera trafikinfo END


                if (CGISweden.formscriptfunctions.GetDisabledField("cgi_casesolved", formContext) == true) {
                    alert("Du saknar behörighet att avsluta detta ärende!");
                    return;
                }


                var _incidentstagecode = CGISweden.formscriptfunctions.GetValue("incidentstagecode", formContext);

                if (_incidentstagecode != 285050002) {
                    formContext.getAttribute("incidentstagecode").setValue(285050004);
                    CGISweden.formscriptfunctions.SetValue("cgi_case_reopen", "1", formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_casesolved", "2", formContext);
                    CGISweden.formscriptfunctions.SaveAndCloseEntity(formContext);
                }
                else {
                    if (confirm("Ärendet har status Obesvarad kund. Vill du avsluta ärendet ändå?") == true) {
                        formContext.getAttribute("incidentstagecode").setValue(285050004);
                        CGISweden.formscriptfunctions.SetValue("cgi_case_reopen", "1", formContext);
                        CGISweden.formscriptfunctions.SetValue("cgi_casesolved", "2", formContext);
                        CGISweden.formscriptfunctions.SaveAndCloseEntity(formContext);
                    }
                }


            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.resolveCase\n\n" + e.Message);
            }
        },

        sendAttentionEmail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan mail skickas!");
                    return;
                }

                var parameters = CGISweden.incident.Ribbon.setArgs(formContext);
                //type of email
                parameters += "&cgi_attention=true";

                // Open the window.
                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(parameters);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);

            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.sendAttentionEmail\n\n" + e.Message);
            }
        },

        sendRemitteceEmail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan mail skickas!");
                    return;
                }

                var parameters = CGISweden.incident.Ribbon.setArgs(formContext);
                //type of email
                parameters += "&cgi_remittance=true";

                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(parameters);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);

            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.sendRemitteceEmail\n\n" + e.Message);
            }
        },

        sendCustomermail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (CGISweden.incident.Ribbon.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan e-post kan skickas!");
                    return;
                }


                var argValues = "";
                argValues += CGISweden.incident.Ribbon.setArgs(formContext);
                if (CGISweden.formscriptfunctions.GetValue("cgi_accountid", formContext) == null && CGISweden.formscriptfunctions.GetValue("cgi_contactid", formContext) == null) {
                    alert("Ingen kund är kopplad till ärendet!");
                    return;
                }
                else {
                    argValues += "&" + CGISweden.incident.Ribbon.setArgsCustomer(formContext);
                    //type of email
                    argValues += "&cgi_button_customer=1";
                }

                //if (CGISweden.incident.Ribbon.validateEmail() == false) {
                //    return;
                //}

                debugger;
                var hasRep = null;
                var cgi_representativ = formContext.getAttribute("cgi_representativid").getValue();

                if (cgi_representativ !== null) {
                    hasRep = CGISweden.incident.Ribbon.hasRepresentativEmail(cgi_representativ, formContext);

                    if (hasRep == false) {
                        return;
                    }
                }

                if (cgi_representativ == null)
                    if (CGISweden.incident.Ribbon.hasContactEmail(formContext) == false)
                        return;

                // Open the window.
                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(argValues);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);
            }
            catch (e) {
                alert("Ett fel inträffade i CGISweden.incidentRibbon.sendCustomermail\n\n" + e.Message);
            }
        },

        hasContactEmail: function (formContext) {
            try {
                var contact = formContext.getAttribute("cgi_contactid").getValue();
                if (contact == null)
                    return;

                var contactId = contact[0].id;

                Xrm.WebApi.retrieveRecord("contact", contactId, "?$select=emailaddress1,emailaddress2,yomifullname")
                    .then(function (result) {
                        if (result.EMailAddress1 == null && result.EMailAddress2 == null) {
                            formContext.ui.setFormNotification(result.YomiFullName + " har ingen e-post kopplad till sig.", "Info", "1");
                            return false;
                        }
                        formContext.ui.clearFormNotification("1");
                        return true;
                    });
            }
            catch (e) {
                alert("Ett fel inträffade i CGISweden.incidentRibbon.hasContactEmail\n\n" + e.Message);
            }
        },

        hasRepresentativEmail: function (cgi_representativ, formContext) {
            try {
                var cgi_representativId = cgi_representativ[0].id;

                Xrm.WebApi.retrieveRecord("cgi_representative", cgi_representativId, "?$select=cgi_name,emailaddress")
                    .then(function (result) {
                        if (result.EmailAddress == null) {
                            formContext.ui.setFormNotification(userRepresentativ.cgi_name + " har ingen e-post kopplad till sig.", "Info", "1");
                            return false;
                        }
                        formContext.ui.clearFormNotification("1");
                        return true;
                    });
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.hasRepresentativEmail\n\n" + e.Message);
            }
        },

        setParameters: function (primaryControl) {
            var _returnValue = {};
            try {
                var formContext = primaryControl;
                //regardingobjectid
                _returnValue["parameter_regardingid"] = CGISweden.formscriptfunctions.GetObjectID(formContext);
                _returnValue["parameter_regardingname"] = CGISweden.formscriptfunctions.GetValue("title", formContext);
                _returnValue["parameter_regardingtype"] = "incident";
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.setParameters\n\n" + e.Message);
            }
            return _returnValue;
        },

        setArgs: function (formContext) {
            try {
                var _returnValue = "parameter_regardingid=" + CGISweden.formscriptfunctions.GetObjectID(formContext);
                var title = CGISweden.formscriptfunctions.GetValue("title", formContext);
                debugger;
                title = title.replace("%", " procent");
                _returnValue += "&parameter_regardingname=" + title;
                _returnValue += "&parameter_regardingtype=incident";

            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.setArgs\n\n" + e.Message);
            }
            return _returnValue;
        },

        setArgsCustomer: function (formContext) {
            try {
                var parameterValue = "";
                var _cgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext),
                    _representative = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid", formContext);

                // If the customer as a representative all communication should go through the representative.
                if (_representative != null) {
                    parameterValue += "parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_representativid", formContext);
                    parameterValue += "&parameter_customername=" + CGISweden.formscriptfunctions.GetLookupName("cgi_representativid", formContext);
                    parameterValue += "&parameter_customertype=cgi_representative";
                }
                else if (_cgi_accountid != null) {
                    //cgi_accountid
                    parameterValue += "parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                    parameterValue += "&parameter_customername=" + CGISweden.formscriptfunctions.GetLookupName("cgi_accountid", formContext);
                    parameterValue += "&parameter_customertype=account";
                }
                else {
                    parameterValue += "parameter_customerid=" + CGISweden.formscriptfunctions.GetLookupid("cgi_contactid", formContext);
                    parameterValue += "&parameter_customername=" + CGISweden.formscriptfunctions.GetLookupName("cgi_contactid", formContext);
                    parameterValue += "&parameter_customertype=contact";
                }

                return parameterValue;
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.setArgsCustomer\n\n" + e.Message);
            }
            return param;
        },

        setParametersCustomer: function (param, primaryControl) {
            try {
                var formContext = primaryControl;

                var _cgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext),
                    _representative = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid", formContext);

                // If the customer as a representative all communication should go through the representative.
                if (_representative != null) {
                    param["parameter_customerid"] = _representative;
                    param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_representativid", formContext);
                    param["parameter_customertype"] = "cgi_representative";
                }
                else if (_cgi_accountid != null) {
                    //cgi_accountid
                    param["parameter_customerid"] = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                    param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_accountid", formContext);
                    param["parameter_customertype"] = "account";
                }
                else {
                    param["parameter_customerid"] = CGISweden.formscriptfunctions.GetLookupid("cgi_contactid", formContext);
                    param["parameter_customername"] = CGISweden.formscriptfunctions.GetLookupName("cgi_contactid", formContext);
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

        validateEmail: function (primaryControl) {
            var formContext = primaryControl;

            var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

            var email = "";
            var email2 = "";
            var cgi_representativid = "";

            try {
                email = CGISweden.formscriptfunctions.GetValue("cgi_customer_email", formContext);
            }
            catch (e) {
                alert("Fel i CGISweden.incident.Ribbon.validateEmail:1\n\n" + e.Message);
                return false;
            }

            try {
                cgi_representativid = CGISweden.formscriptfunctions.GetLookupid("cgi_representativid", formContext);
            }
            catch (e) {

            }

            if (cgi_representativid != null && cgi_representativid != "") {
                try {
                    Xrm.WebApi.retrieveRecord("cgi_representativ", cgi_representativid, "?$select=cgi_email")
                        .then(function (result) {
                            var cgi_representative = result.results[0];
                            if (cgi_representative["cgi_Email"] == null || cgi_representative["cgi_Email"] == "")
                                email2 = cgi_representative["cgi_Email"];

                        });
                }
                catch (e) {
                    alert("Fel i CGISweden.incident.Ribbon.validateEmail:2\n\n" + e.Message);
                }
            }

            if (re.test(email) || re.test(email2)) {
                return false;
            }

            return true;
        },

        openBombApp: function (primaryControl) {
            try {
                var formContext = primaryControl;

                var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
                CGISweden.odata.GetBOMBUrlFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.openBombApp\n\n" + e.Message);
            }
        },

        openBombApp_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen url definierad för BOMB!");
                }
                else {
                    var _features = "status=1,toolbar=1,location=1,menubar=1,directories=1,resizable=1,scrollbars=1";
                    var _url = result[0].cgi_BOMBUrl;
                    var _param = CGISweden.formscriptfunctions.GetValue("cgi_bombmobilenumber", formContext)
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
        chechIfAnyFieldIsDirty: function (formContext) {
            try {
                return formContext.data.entity.getIsDirty();
            }
            catch (e) {
                alert("Fel i CGISweden.incidentRibbon.chechIfAnyFieldIsDirty\n\n" + e.Message);
            }
        }

    };


