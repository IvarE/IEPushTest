FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Contact) == "undefined") {
    Endeavor.Skanetrafiken.Contact = {

        _ticketMovesErrorHolder: "ticketMovesErrorHolder",

        _urlRelativeFrontOffice: "https://sttravelconfiguratorweb.azurewebsites.net/ticketdetails/",
        _listOfFormsFrontOffice: ["Contact (3 - Skånetrafiken)"],

        onLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();
            var formType = formContext.ui.getFormType();

            var formIsOnLoad = true;
            Endeavor.Skanetrafiken.Contact.resetRequiredLevel(executionContext, formIsOnLoad);

            switch (formType) {
                case FORM_TYPE_CREATE:

                    debugger;
                    //Check if quick create
                    var stateCode = formContext.getAttribute("statecode");
                    if (stateCode == null) {
                        var privateContact = formContext.getAttribute("ed_privatecustomercontact");
                        if (privateContact != null) {
                            privateContact.setValue(true);
                        }

                        Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName(formContext);
                    }

                    //debugger;
                    var emailaddress1 = formContext.getAttribute("emailaddress1");
                    var emailaddress2 = formContext.getAttribute("emailaddress2");
                    // Objects in form?
                    if (emailaddress1 != null && emailaddress2 != null) {
                        // email1 present? Move to email2...
                        if (emailaddress1.getValue() != null) {
                            emailaddress2.setValue(emailaddress1.getValue());
                            emailaddress1.setValue(null);
                        }
                    }

                    Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName(formContext);

                    // getOrders prevent load
                    if (formContext.ui.tabs.get('tab_5'))
                        formContext.ui.tabs.get('tab_5').setVisible(false);

                    break;
                case FORM_TYPE_UPDATE:
                    Endeavor.Skanetrafiken.Contact.showHideCompanyEngagementTab(formContext);
                    Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact(formContext);
                    break;
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                    break;
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    break;
            }
        },

        // Install on onSave
        onSave: function (executionContext) {

            var formContext = executionContext.getFormContext();
            formContext.ui.clearFormNotification("IsMoreThanPrivate");

            var formType = formContext.ui.getFormType();
            if (formType == 1) { //Create
                Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact(formContext);
                formContext.getAttribute("ed_informationsource").setValue(8); // 8-AdmSkapaKund
            } else if (formType == 2) { //Update
                Endeavor.Skanetrafiken.Contact.isMoreThanPrivateContact(formContext);
                formContext.getAttribute("ed_informationsource").setValue(12); // 12-AdmAndraKund
            } else {
                // TODO teo - Vad gör vi om formuläret sparas och det varken är Create eller Update?
            }
        },

        resetRequiredLevel: function (executionContext, formIsOnLoad) {
            
            var formContext = executionContext.getFormContext();
            debugger;
            var stateCode = formContext.getAttribute("statecode");
            if (stateCode == null || (stateCode != null && stateCode.getValue() == 0)) {

                var businessContact = formContext.getAttribute("ed_businesscontact");
                var agentContact = formContext.getAttribute("ed_agentcontact");
                var seniorContact = formContext.getAttribute("ed_seniorcontact");
                var schoolContact = formContext.getAttribute("ed_schoolcontact");
                var infotainmentContact = formContext.getAttribute("ed_infotainmentcontact");
                var privateContact = formContext.getAttribute("ed_privatecustomercontact");
                var resellerContact = formContext.getAttribute("ed_reseller");
                var collaborationContact = formContext.getAttribute("ed_collaborationcontact");

                if (businessContact != null && agentContact != null && seniorContact != null &&
                    schoolContact != null && infotainmentContact != null && privateContact != null
                    && resellerContact != null && collaborationContact != null) {

                    var businessValue = businessContact.getValue();
                    var agentValue = agentContact.getValue();
                    var seniorValue = seniorContact.getValue();
                    var schoolValue = schoolContact.getValue();
                    var infotainmentValue = infotainmentContact.getValue();
                    var privateValue = privateContact.getValue();
                    var resellerValue = resellerContact.getValue();
                    var collaborationValue = collaborationContact.getValue();

                    if (stateCode == null && formIsOnLoad == true)
                    {
                        privateContact.setValue(true);
                        privateValue = true;
                    }

                    if (businessValue != false || agentValue != false || seniorValue != false ||
                        schoolValue != false || infotainmentValue != false || privateValue != false
                        || resellerValue != false || collaborationValue != false) {

                        businessContact.setRequiredLevel("none");
                        agentContact.setRequiredLevel("none");
                        seniorContact.setRequiredLevel("none");
                        schoolContact.setRequiredLevel("none");
                        infotainmentContact.setRequiredLevel("none");
                        privateContact.setRequiredLevel("none");
                        resellerContact.setRequiredLevel("none");
                        collaborationContact.setRequiredLevel("none");

                        if (privateValue != false) {

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (businessValue != false)
                        {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (schoolValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (seniorValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (infotainmentValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (agentValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (resellerValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            collaborationContact.setValue(false);
                            formContext.getControl("ed_collaborationcontact").setDisabled(true);

                        }
                        else if (collaborationValue != false) {

                            privateContact.setValue(false);
                            formContext.getControl("ed_privatecustomercontact").setDisabled(true);

                            businessContact.setValue(false);
                            formContext.getControl("ed_businesscontact").setDisabled(true);

                            schoolContact.setValue(false);
                            formContext.getControl("ed_schoolcontact").setDisabled(true);

                            seniorContact.setValue(false);
                            formContext.getControl("ed_seniorcontact").setDisabled(true);

                            infotainmentContact.setValue(false);
                            formContext.getControl("ed_infotainmentcontact").setDisabled(true);

                            agentContact.setValue(false);
                            formContext.getControl("ed_agentcontact").setDisabled(true);

                            resellerContact.setValue(false);
                            formContext.getControl("ed_reseller").setDisabled(true);

                        }
                    }
                    else if (businessValue == false && agentValue == false
                        && seniorValue == false && schoolValue == false
                        && infotainmentValue == false && privateValue == false
                        && resellerValue == false && collaborationValue == false) {

                        formContext.getControl("ed_businesscontact").setDisabled(false);
                        formContext.getControl("ed_agentcontact").setDisabled(false);
                        formContext.getControl("ed_seniorcontact").setDisabled(false);
                        formContext.getControl("ed_schoolcontact").setDisabled(false);
                        formContext.getControl("ed_infotainmentcontact").setDisabled(false);
                        formContext.getControl("ed_privatecustomercontact").setDisabled(false);
                        formContext.getControl("ed_reseller").setDisabled(false);
                        formContext.getControl("ed_collaborationcontact").setDisabled(false);

                        businessContact.setRequiredLevel("required");
                        agentContact.setRequiredLevel("required");
                        seniorContact.setRequiredLevel("required");
                        schoolContact.setRequiredLevel("required");
                        infotainmentContact.setRequiredLevel("required");
                        privateContact.setRequiredLevel("required");
                        resellerContact.setRequiredLevel("required");
                        collaborationContact.setRequiredLevel("required");
                    }
                }
            }
        },

        isMoreThanPrivateContact: function (formContext) {

            debugger;
            formContext.ui.clearFormNotification("IsMoreThanPrivate")

            var checkPrivateAttribute = formContext.getAttribute("ed_privatecustomercontact");
            var checkSchoolAttribute = formContext.getAttribute("ed_schoolcontact");
            var checkSeniorAttribute = formContext.getAttribute("ed_seniorcontact");
            var checkAgentAttribute = formContext.getAttribute("ed_agentcontact");
            var checkInfotainmentAttribute = formContext.getAttribute("ed_infotainmentcontact");
            var checkBusinessAttribute = formContext.getAttribute("ed_businesscontact");

            if (checkPrivateAttribute != null && checkSchoolAttribute != null && checkSeniorAttribute != null && checkAgentAttribute != null && checkInfotainmentAttribute != null && checkBusinessAttribute != null) {
                formContext.ui.clearFormNotification("IsMoreThanPrivate")

                var notOnlyPrivateContact = false;
                var privateContact = formContext.getAttribute("ed_privatecustomercontact").getValue();
                var schoolContact = formContext.getAttribute("ed_schoolcontact").getValue();
                var seniorContact = formContext.getAttribute("ed_seniorcontact").getValue();
                var agentContact = formContext.getAttribute("ed_agentcontact").getValue();
                var infotainmentContact = formContext.getAttribute("ed_infotainmentcontact").getValue();
                var businessContact = formContext.getAttribute("ed_businesscontact").getValue();

                if (privateContact != null && (schoolContact != null ||
                    seniorContact != null || agentContact != null ||
                    infotainmentContact != null || businessContact != null)) {
                    if (privateContact == true && (schoolContact == true || seniorContact == true ||
                        agentContact == true || infotainmentContact == true || businessContact == true)) {
                        notOnlyPrivateContact = true;
                    }
                }

                if (notOnlyPrivateContact == true) {
                    var additionalTraits = "";
                    var school = "School Contact";
                    var senior = "Senior Contact";
                    var agent = "Agent Contact";
                    var infotainment = "Infotainment Contact";
                    var business = "Business Contact";
                    if (schoolContact == true) {
                        //additionalTraits = additionalTraits + ", " + school;
                        additionalTraits = additionalTraits + ", " + "Skolkontakt";
                    }
                    if (seniorContact == true) {
                        //additionalTraits = additionalTraits + ", " + senior;
                        additionalTraits = additionalTraits + ", " + "Seniorkontakt";
                    }
                    if (agentContact == true) {
                        //additionalTraits = additionalTraits + ", " + agent;
                        additionalTraits = additionalTraits + ", " + "Agentkontakt";
                    }
                    if (infotainmentContact == true) {
                        //additionalTraits = additionalTraits + ", " + infotainment;
                        additionalTraits = additionalTraits + ", " + "Infotainmentkontakt";
                    }
                    if (businessContact == true) {
                        //additionalTraits = additionalTraits + ", " + business;
                        additionalTraits = additionalTraits + ", " + "Företagskontakt";
                    }

                    formContext.ui.setFormNotification("Denna kontakt är en Privatkontakt. Utöver detta så är kontakten även" + additionalTraits + ".", "INFO", "IsMoreThanPrivate");
                }
            }
        },

        showHideCompanyEngagementTab: function (formContext) {
            var contactId = formContext.data.entity.getId().replace("{", "").replace("}", "");

            Xrm.WebApi.retrieveMultipleRecords("ed_companyrole", "?$filter=_ed_contact_value eq " + contactId).then(
                function success(results) {

                    if (results == null || results.entities.length == 0) {
                        // Hide Company Engagement tab
                        formContext.ui.tabs.get("Portal roles").setVisible(false);
                        formContext.ui.controls.get("ed_islockedportal").setVisible(false);
                    } else {
                        // Show Company Engagement tab
                        formContext.ui.tabs.get("Portal roles").setVisible(true);
                    }
                },
                function (error) {
                    console.log(error.message);
                    Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                }
            );
        },

        onBlockContactShow: function (formContext) {

            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Contact.showBlockButton(formContext);

            if (lockedPortal == false && showButton == true) {
                return true;
            } else {
                return false;
            }
        },

        onUnblockContactShow: function (formContext) {

            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Contact.showBlockButton(formContext);

            if (lockedPortal == true && showButton == true) {
                return true;
            } else {
                return false;
            }
        },

        onFrontOfficeDisplay: function (formContext) {
            debugger;
            var formItem = formContext.ui.formSelector.getCurrentItem();

            if (formItem == null)
                return false;

            var formName = formItem.getLabel();

            for (var i = 0; i < Endeavor.Skanetrafiken.Contact._listOfFormsFrontOffice.length; i++) {
                var valueList = Endeavor.Skanetrafiken.Contact._listOfFormsFrontOffice[i];
                if (formName == valueList) {
                    return true;
                }
            }

            return false;
        },

        findValueFrontOffice: function (allSelectedRows, fieldName) {
            for (var i = 0; i < allSelectedRows.getAll().length; i++) {
                var selectedRow = allSelectedRows.getAll()[i];

                var selectedAttribute = selectedRow.getAttribute != null ? selectedRow.getAttribute() : selectedRow.getData().getEntity().getAttributes().getAll();

                for (var j = 0; j < selectedAttribute.length; j++) {

                    var selectedAttribute = selectedAttribute[j];
                    if (selectedAttribute.getName() == fieldName) {
                        var value = selectedAttribute.getValue();
                        return Endeavor.Skanetrafiken.Contact._urlRelativeFrontOffice + value;
                    }
                }
            }

            return null;
        },

        onFrontOfficeIntegration: function (formContext) {

            var fieldNameReskort = "ed_cardnumber";
            var gridContextReskort = formContext.getControl("Relaterat_Reskort");
            var allSelectedRowsReskort = gridContextReskort.getGrid().getSelectedRows();

            var openUrlReskort = Endeavor.Skanetrafiken.Contact.findValueFrontOffice(allSelectedRowsReskort, fieldNameReskort);
            if (openUrlReskort != null)
                window.open(openUrlReskort, "_reskort");

            var fieldNameSingapore = "st_ticketid";
            var gridContextSingapore = formContext.getControl("Singapore_Biljetter");
            var allSelectedRowsSingapore = gridContextSingapore.getGrid().getSelectedRows();

            var openUrlSingapore = Endeavor.Skanetrafiken.Contact.findValueFrontOffice(allSelectedRowsSingapore, fieldNameSingapore);
            if (openUrlSingapore != null)
                window.open(openUrlSingapore, "_singapore");

            var fieldNameValuecodes = "ed_mobilenumber";
            var gridContextValuecodes = formContext.getControl("related_valuecodes");
            var allSelectedRowsValuecodes = gridContextValuecodes.getGrid().getSelectedRows();

            var openUrlValueCode = Endeavor.Skanetrafiken.Contact.findValueFrontOffice(allSelectedRowsValuecodes, fieldNameValuecodes);
            if (openUrlValueCode != null)
                window.open(openUrlValueCode, "_valuecode");
        },

        onSocialSecurityNumberChange: function (executionContext) {

            var formContext = executionContext.getFormContext();
            var contactNumberControlTag = "cgi_socialsecuritynumber";

            var contactNumber = formContext.getAttribute(contactNumberControlTag);
            if (!(!contactNumber.getValue() || 0 === contactNumber.getValue().length)) {
                if (!Endeavor.Skanetrafiken.Contact.checkSocSecNumber(formContext, contactNumber.getValue()))
                    formContext.getControl(contactNumberControlTag).setNotification("Ogiltigt Persnonnummer<BR>(giltiga format: ååmmdd, ååååmmdd, ååååmmddxxxx, ååååmmdd-xxxx)");
                else 
                    formContext.getControl(contactNumberControlTag).clearNotification();
                
            }
            else
                formContext.getControl(contactNumberControlTag).clearNotification();

        },

        onMarkForCreditsafeUpdate: function (formContext) {

            var guid = formContext.data.entity.getId().replace("{", "").replace("}", "");

            if (!formContext.getAttribute("cgi_socialsecuritynumber")) {
                Endeavor.formscriptfunctions.AlertCustomDialog("Inget personnummer funnet på formuläret. Vänligen lägg till.");
                return;
            }
            var socSecNr = formContext.getAttribute("cgi_socialsecuritynumber").getValue();

            if (formContext.getAttribute("firstname") && formContext.getAttribute("lastname"))
                var fullName = formContext.getAttribute("firstname").getValue() + " " + formContext.getAttribute("lastname").getValue();
            else if (formContext.getAttribute("lastname"))
                var fullName = formContext.getAttribute("lastname").getValue();
            else if (formContext.getAttribute("firstname"))
                var fullName = formContext.getAttribute("firstname").getValue();
            else
                var fullName = "Namn saknas";

            var deltabatchQueue =
            {
                "ed_contactguid": guid,
                "ed_contactnumber": socSecNr,
                "ed_deltabatchoperation": 899310000,
                "ed_name": "ForceUpdate: " + fullName + ", " + Endeavor.Common.Data.dateToString(new Date(), "yyyy/MM/dd", "-"),
            }

            Xrm.WebApi.createRecord("ed_deltabatchqueue", deltabatchQueue).then(
                function success(result) {
                    if (!result)
                        Endeavor.formscriptfunctions.AlertCustomDialog("Uppdatering kanske inte är schemalagd. Inget returvärde då köpost skapades");
                },
                function (error) {
                    Endeavor.formscriptfunctions.AlertCustomDialog("Något gick fel när uppdatering skulle schemaläggas:\n\n" + error.message);
                }
            );
        },

        onDisplayTicketMoves: function (formContext) {

            formContext.ui.clearFormNotification(Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder);
            try {
                Endeavor.Skanetrafiken.Contact.displayTicketMovesSuccessCallback(formContext);
            } catch (e) {
                if (!formContext.ui.setFormNotification(e.message, "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                    Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
            }
        },

        displayTicketMovesSuccessCallback: function (formContext) {
            var guid = formContext.data.entity.getId().replace("{", "").replace("}", "");

            // Om kunden inte har något värde i fältet "Mitt Konto-ID" så visar vi ett varningsmeddelande
            var mittKontoField = formContext.getAttribute("ed_mklid");
            if (mittKontoField && mittKontoField.getValue) {
                if (!mittKontoField.getValue()) {
                    if (!formContext.ui.setFormNotification("Denna kund har inte 'Mitt Konto'", "WARNING", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                        Endeavor.formscriptfunctions.AlertCustomDialog(results);
                    return;
                }
            } else {

                Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=ed_mklid&$filter=contactid eq " + guid).then(
                    function success(results) {

                        if (!results || !results.entities.length > 0 || !results.entities[0] || !results.entities[0].ed_mklid) {
                            if (!formContext.ui.setFormNotification("Denna kund har inte 'Mitt Konto'", "WARNING", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                                Endeavor.formscriptfunctions.AlertCustomDialog(results);
                            return;
                        }
                    },
                    function (error) {
                        console.log(error.message);
                        Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                    }
                );
            }

            var fullName = "";
            if (formContext.getAttribute("firstname") && formContext.getAttribute("lastname"))
                fullName = formContext.getAttribute("firstname").getValue() + " " + formContext.getAttribute("lastname").getValue();
            else if (formContext.getAttribute("lastname"))
                fullName = formContext.getAttribute("lastname").getValue();
            else if (formContext.getAttribute("firstname"))
                fullName = formContext.getAttribute("firstname").getValue();
            else
                fullName = "Namn saknas";

            var inputParameters = [{ "Field": "ContactGuid", "Value": guid, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetTicketMoveDataFromMKL", inputParameters,
                function (result) {

                    var object = JSON.parse(result.responseText);
                    var objectMKLResponse = object.GetTicketMoveDataFromMKLResponse;
                    objectMKLResponse = objectMKLResponse.replace("Ett fel uppstod vid kommunikation med MKL: ", "");
                    objectMKLResponse = JSON.parse(objectMKLResponse);
                    debugger;
                    if (objectMKLResponse.message != null) {
                        if (!formContext.ui.setFormNotification(objectMKLResponse.message, "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                            Endeavor.formscriptfunctions.AlertCustomDialog(objectMKLResponse.message);
                        return;
                    }

                    var movesDone = objectMKLResponse.user.accountMoved;

                    window.mklData = fullName + ";" + guid + ";" + movesDone;
                    var windowOptions = { height: 300, width: 600 };
                    Xrm.Navigation.openWebResource("ed_/html/Endeavor.Skanetrafiken.TicketMoveManager.html", windowOptions, (fullName + ";" + guid + ";" + movesDone));

                }, function (error) {
                    if (!formContext.ui.setFormNotification(error.message, "ERROR", Endeavor.Skanetrafiken.Contact._ticketMovesErrorHolder))
                        Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                    return;
                });
        },

        checkSocSecNumber: function (formContext, nr) {
            if (Endeavor.Skanetrafiken.Contact.checkPersonnummer(nr)) {
                var sweSocSec = formContext.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(true);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            } else if (Endeavor.Skanetrafiken.Contact.checkNonSwedishSocSecNumber(nr)) {
                var sweSocSec = formContext.getAttribute("ed_hasswedishsocialsecuritynumber");
                if (sweSocSec != null) {
                    sweSocSec.setValue(false);
                    sweSocSec.setSubmitMode("always");
                } else {
                    alert("Fel i formulär, dolt fält saknas. Vänligen kontakta Administratör");
                }
                return true;
            }
            return false;
        },

        checkPersonnummer: function (nr) {
            this.valid = false;
            //if(!nr.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})$/)){ return false; }
            if (nr.match(/^(\d{4})(\d{2})(\d{2})-(\d{4})$/)) {
                nr = nr.replace("-", "");
            }
            if (!nr.match(/^(\d{4})(\d{2})(\d{2})(\d{4})$/)) {
                return false;
            }

            this.fullYear = RegExp.$1;
            this.year = this.fullYear.substring(2, 4);
            this.month = RegExp.$2;
            this.day = RegExp.$3;
            this.controldigits = RegExp.$4;

            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }

            this.alldigits = this.year + this.month + this.day + this.controldigits;

            var nn = "";
            for (var n = 0; n < this.alldigits.length; n++) {
                nn += ((((n + 1) % 2) + 1) * this.alldigits.substring(n, n + 1));
            }
            this.checksum = 0;

            for (var n = 0; n < nn.length; n++) {
                this.checksum += nn.substring(n, n + 1) * 1;
            }
            this.valid = (this.checksum % 10 == 0) ? true : false;
            this.sex = parseInt(this.controldigits.substring(2, 3)) % 2;
            return this.valid;
        },

        checkNonSwedishSocSecNumber: function (nr) {
            if (nr.match(/^(\d{4})(\d{2})(\d{2})$/)) {
                this.fullYear = RegExp.$1;
                this.year = this.fullYear.substring(2, 4);
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else if (nr.match(/^(\d{2})(\d{2})(\d{2})$/)) {
                this.year = RegExp.$1;
                this.fullYear = parseInt(this.year > (new Date()).getFullYear() % 100) ? "19" : "20" + this.year;
                this.month = RegExp.$2;
                this.day = RegExp.$3;
            } else {
                return false;
            }
            if (!Endeavor.Skanetrafiken.Contact.checkDateFormat()) {
                return false;
            }
            return true;
        },

        checkDateFormat: function () {
            var months = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
            if (this.fullYear % 400 == 0 || this.fullYear % 4 == 0 && this.fullYear % 100 != 0) {
                months[1] = 29;
            }

            if (this.month * 1 < 1 || this.month * 1 > 12 || this.day * 1 < 1 || this.day * 1 > months[this.month * 1 - 1]) {
                return false;
            }
            return true;
        },

        blockCustomerPortal: function (formContext) {

            try {
                Endeavor.Skanetrafiken.Contact.blockCustomerPortalSuccessCallback(formContext);
            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
            }
        },

        blockCustomerPortalSuccessCallback: function (formContext) {
            var SSN = "";
            var blocked = "";

            //Använda ed_socialsecuritynumberblock
            if (formContext.getAttribute("ed_socialsecuritynumberblock"))
                SSN = formContext.getAttribute("ed_socialsecuritynumberblock").getValue();

            if (formContext.getAttribute("ed_islockedportal"))
                blocked = formContext.getAttribute("ed_islockedportal").getValue();

            if (SSN) {

                var inputParameters = [{ "Field": "SSN", "Value": SSN, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "Blocked", "Value": !blocked, "TypeName": Endeavor.formscriptfunctions.getParameterType("bool"), "StructuralProperty": 1 }];

                Endeavor.formscriptfunctions.callGlobalAction("ed_BlockCustomerPortal", inputParameters,
                    function (result) {
                        if (blocked)
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kund avblockerad!");
                        else
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kund spärrad!");

                        formContext.data.refresh();
                    },
                    function (error) {
                        if (blocked)
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte avblockera kund. Var god försök igen senare.");
                        else
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte spärra kund. Var god försök igen senare.");

                        formContext.data.refresh();
                    });
            }
            else
                Endeavor.formscriptfunctions.AlertCustomDialog("Kunden saknar personnummer.");
        },

        setFocusQuickCreateContactFirstName: function (formContext) {
            try {
                formContext.getControl("firstname").setFocus();
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.setFocusQuickCreateContactFirstName\n\n" + e.message);
            }
        },

        //Senare fix med säkerhetsroller
        showBlockButton: function (formContext) {

            debugger;
            var roles = [];
            roles[0] = "Skånetrafiken Handläggare"; //Borde heta "access block button" eller liknande
            roles[1] = "Skånetrafiken Handläggare Minus";
            roles[2] = "Skånetrafiken handläggare plus NY";
            roles[3] = "Skånetrafiken Handläggare Serviceresor";
            var isUserCheck = Endeavor.Skanetrafiken.Contact.currentUserHasSecurityRole(roles);

            var currForm = formContext.ui.formSelector.getCurrentItem();

            var isContactOrgForm = false;
            if (currForm && currForm.getId() == "c2cd73d5-94d8-428e-8efe-4fa7d9ba05b4") //Contact (Organisation) form.
                isContactOrgForm = true;

            return isUserCheck && isContactOrgForm;
        },

        currentUserHasSecurityRole: function (roles) {
            var fetchXml =
                "<fetch mapping='logical'>" +
                "<entity name='systemuser'>" +
                "<attribute name='systemuserid' />" +
                "<filter type='and'>" +
                "<condition attribute='systemuserid' operator='eq-userid' />" +
                "</filter>" +
                "<link-entity name='systemuserroles' from='systemuserid' to='systemuserid' visible='false' intersect='true'>" +
                "<link-entity name='role' from='roleid' to='roleid' alias='r'>" +
                "<filter type='or'>";

            for (var i = 0; i < roles.length; i++) {
                fetchXml += "<condition attribute='name' operator='eq' value='" + roles[i] + "' />";
            }

            fetchXml += "            </filter>" +
                "</link-entity>" +
                "</link-entity>" +
                "</entity>" +
                "</fetch>";
            var modifiedFetchXml = fetchXml.replace("&", "&amp;");

            var users = Endeavor.formscriptfunctions.executeFetchGetCount(modifiedFetchXml, "systemusers");
            if (users > 0)
                return true;
            else
                return false;
        },
        // Senare fix

        //Form Methods CGI Contact (from contactLibrary.js)
        onFormLoad: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var formIsOnLoad = true;
                Endeavor.Skanetrafiken.Contact.resetRequiredLevel(executionContext, formIsOnLoad);

                switch (formContext.ui.getFormType()) {
                    case FORM_TYPE_CREATE:
                        break;
                    case FORM_TYPE_UPDATE:
                        Endeavor.Skanetrafiken.Contact.checkIfUserHasSecRole(executionContext);
                        Endeavor.Skanetrafiken.Contact.timerfunction_eHandel(formContext);
                    case FORM_TYPE_READONLY:
                    case FORM_TYPE_DISABLED:
                        break;
                    case FORM_TYPE_QUICKCREATE:
                    case FORM_TYPE_BULKEDIT:
                        break;
                    default:
                        alert("Form type error!");
                        break;
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.onFormLoad\n\n" + e.message);
            }
        },
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        onsave: function (executionContext) {
            
            var eventArgs = executionContext.getEventArgs();
            var _check_soc = Endeavor.Skanetrafiken.Contact.SocSecNoOnChange(executionContext);

            if (_check_soc == false)
                eventArgs.preventDefault();
        },
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        checkIfUserHasSecRole: function (executionContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();
                var formContext = executionContext.getFormContext();

                var currentUserRoles = globalContext.userSettings.securityRoles;
                for (var i = 0; i < currentUserRoles.length; i++) {
                    var userRoleId = currentUserRoles[i];

                    var globalContext = Xrm.Utility.getGlobalContext();
                    var clientURL = globalContext.getClientUrl();

                    var url = clientURL + "/api/data/v9.0/roles?$select=name&$filter=roleid eq " + userRoleId;
                    var results = Endeavor.formscriptfunctions.fetchJSONResults(url);
                    Endeavor.Skanetrafiken.Contact.checkIfUserHasRole_callback(results, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.checkIfUserHasRole\n\n" + e.message);
            }
        },

        checkIfUserHasRole_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Inga säkerhetsroller definierade!");
                }
                else {
                    var _handlingOfficer = "Skånetrafiken Handläggare";
                    var _handlingOfficerPlus = "Skånetrafiken Handläggare plus";

                    var _roleName = result[0].name;

                    try {
                        var emailField = formContext.getAttribute("emailaddress1");

                        if (emailField == null)
                            return;

                        var emailValue = emailField.getValue();

                        var currForm = formContext.ui.formSelector.getCurrentItem();
                        var currFormId = currForm.getId()

                        if (currFormId !== "aa39956c-0a06-4963-873a-2b3e574dbea5" && currFormId !== "4b94250e-b88f-4439-9184-750d56a84fcf") //dont lock email field when using form "Tre Kolumner (Test)" or "Labbvy Admin"
                            if (emailValue && emailValue.Length !== 0 && _roleName.indexOf("Handläggare") > 0)
                                Endeavor.formscriptfunctions.SetState("emailaddress1", "true", formContext); //The field should be editable until it has content
                    }
                    catch (ex) {
                        alert("Fel i Endeavor.Skanetrafiken.Contact.checkIfUserHasRole_callback\n\n" + ex.message);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.checkIfUserHasRole_callback\n\n" + e.message);
            }
        },
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        SocSecNoOnChange: function (executionContext) {

            var formContext = executionContext.getFormContext();

            // Johan Andersson - Endeavor
            // Check Personnummer if swedish nationality
            var _return_save = true;
            var hasswedish = true;
            var obj = formContext.getAttribute("ed_hasswedishsocialsecuritynumber");
            if (obj != null)
                if (obj.getValue() == false)
                    hasswedish = false;

            // Perform only if swedish (default to swedish if missing)
            if (hasswedish == true) {
                var _soc = formContext.getAttribute("cgi_socialsecuritynumber");
                if (_soc != "" && _soc != null) {
                    _soc = _soc.getValue();
                    if (_soc == null)
                        _return_save = true;
                    else {
                        var _sectrue = Endeavor.Skanetrafiken.Contact.validatePersonalNumber(_soc);
                        if (_sectrue == false) {
                            alert("Personnummer är inte giltigt. Privatkunden kan inte sparas.");
                            _return_save = false;
                        }
                        else {
                            var _soc_trim = _soc.replace('-', '');
                            var _soc_trim_length = _soc_trim.length;
                            if (_soc_trim_length != 12) {
                                alert("Personnummer har fel längd. Privatkunden kan inte sparas");
                                _return_save = false;
                            }
                            else
                                if (_soc.length == 13)
                                    formContext.getAttribute("cgi_socialsecuritynumber").setValue(_soc_trim);
                        }
                    }
                }
            }

            return (_return_save);
        },

        validatePersonalNumber: function (input) {
            // Check valid length & form
            if (!input) return false;
            if (input.indexOf('-') == -1) {
                if (input.length === 10) {
                    input = input.slice(0, 6) + "-" + input.slice(6);
                } else {
                    input = input.slice(0, 8) + "-" + input.slice(8);
                }
            }
            if (input.indexOf('-') == -1) {
                if (input.length === 10) {
                    input = input.slice(0, 6) + "-" + input.slice(6);
                } else {
                    input = input.slice(0, 8) + "-" + input.slice(8);
                }
            }
            if (!input.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})|(\d{4})(\d{2})(\d{2})\-(\d{4})$/)) return false;

            // Clean input
            input = input.replace('-', '');
            if (input.length == 12) {
                input = input.substring(2);
            }

            // Declare variables
            var d = new Date(((!!RegExp.$1) ? RegExp.$1 : RegExp.$5), (((!!RegExp.$2) ? RegExp.$2 : RegExp.$6) - 1), ((!!RegExp.$3) ? RegExp.$3 : RegExp.$7)),
                sum = 0,
                numdigits = input.length,
                parity = numdigits % 2,
                i,
                digit;

            // Check valid date
            if (Object.prototype.toString.call(d) !== "[object Date]" || isNaN(d.getTime())) return false;

            // Check luhn algorithm
            for (i = 0; i < numdigits; i = i + 1) {
                digit = parseInt(input.charAt(i))
                if (i % 2 == parity) digit *= 2;
                if (digit > 9) digit -= 9;
                sum += digit;
            }
            return (sum % 10) == 0;
        },
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        setContactLastname_Onload: function (executionContext) {
            var formContext = executionContext.getFormContext();
            Endeavor.formscriptfunctions.SetValue("lastname", "", formContext);
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        timerfunction_eHandel: function (formContext) {
            try {
                /*TODO - this is commented out, until the Silverlight page is coded as an HTML if needed - 03/11/2020*/
                //var arg = 'WebResource_eHandelOrders';
                //var obj = formContext.getControl(arg).getObject();
                //var entid = formContext.data.entity.getId();

                //try {
                //    obj.contentWindow.SetID(entid);
                //}
                //catch (e) {
                //    setTimeout(function () { Endeavor.Skanetrafiken.Contact.timerfunction_eHandel(formContext); }, TIMEOUT_COUNTER);
                //}
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.timerfunction_eHandel\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        format_phonenumber: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();
                var phoneNumberStr = executionContext.getEventSource();
                var control = formContext.getControl(phoneNumberStr.getName());

                // Verify that the field is valid
                if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null) {

                    if (phoneNumberStr.getValue() != null) {

                        // replace any "-" with a blank space
                        var oldNumberStr = phoneNumberStr.getValue();
                        var newNumberStr = oldNumberStr.replace(/-/g, "");
                        newNumberStr = newNumberStr.replace(/ /g, "");
                        phoneNumberStr.setValue(newNumberStr);
                        if (newNumberStr.indexOf("+") > -1)
                            control.setNotification("Ange telefonnummer utan landsprefix.");
                        else
                            control.clearNotification();
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.format_phonenumber\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        format_ZIPCode: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var ZIPCodeNumberStr = formContext.getAttribute("address1_postalcode");

                // Verify that the field is valid
                if (typeof (ZIPCodeNumberStr) != "undefined" && ZIPCodeNumberStr != null) {

                    if (ZIPCodeNumberStr.getValue() != null && ZIPCodeNumberStr.getValue().length > 3) {
                        var oldNumberStr = ZIPCodeNumberStr.getValue();
                        oldNumberStr = oldNumberStr.replace(/ /g, "");
                        //var newNumberStr = oldNumberStr.substring(0, 3) + " " + oldNumberStr.substring(3);
                        ZIPCodeNumberStr.setValue(oldNumberStr);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.format_ZIPCodeNumber\n\n" + e.message);
            }
        },

        save_format_ZIPCode: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var ZIPCodeNumberStr = formContext.getAttribute("address1_postalcode");

                // Verify that the field is valid
                if (typeof (ZIPCodeNumberStr) != "undefined" && ZIPCodeNumberStr != null) {

                    if (ZIPCodeNumberStr.getValue() != null) {
                        var oldNumberStr = ZIPCodeNumberStr.getValue();
                        var newNumberStr = oldNumberStr.replace(/ /g, "");
                        ZIPCodeNumberStr.setValue(newNumberStr);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Contact.format_ZIPCodeNumber\n\n" + e.message);
            }
        }
    };
}