// Begin scoping
if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}
if (typeof (Endeavor.Skanetrafiken) === "undefined") {
    Endeavor.Skanetrafiken = {
    };
}
if (typeof (Endeavor.Skanetrafiken.ValueCodeApproval) === "undefined") {
    Endeavor.Skanetrafiken.ValueCodeApproval = {

        onLoad: function () {
            debugger;
        },
        cleanIdField = function (id) {
            id = id.replace("{", "");
            id = id.replace("}", "");
            return id;
        },
        approveValueCode: function (formContext) {
            debugger;
            var valueCodeId;
            formContext.data.entity.save();
            formContext.ui.setFormNotification("Skapar och skickar värdekod. Vänligen vänta.", "INFO");
            try {
                //var clearonTemplateId = Xrm.Page.getAttribute("ed_clearontemplateid").getValue();
                var amount = formContext.getAttribute("ed_amount").getValue();
                var mobile = formContext.getAttribute("ed_mobile").getValue();
                var email = formContext.getAttribute("ed_emailaddress").getValue();
                var validTo = formContext.getAttribute("ed_validto").getValue();
                var contact = formContext.getAttribute("ed_contact").getValue()[0].id;
                var travelCardNumber = formContext.getAttribute("ed_travelcardnumber").getValue();
                var typeOfValueCode = formContext.getAttribute("ed_typeofvaluecode").getText();
                var valCodeApprovId;
                var formType = formContext.ui.getFormType();

                var contactIdValue = { entityType: "ed_valuecodeapproval", id: Endeavor.Skanetrafiken.ValueCodeApproval.cleanIdField(contact) };
                var valCodeApprovIdValue = { entityType: "ed_valuecodeapproval", id: Endeavor.Skanetrafiken.ValueCodeApproval.cleanIdField(valCodeApprovId) };

                var inputParameters = [{ "Field": "ClearonTemplateId", "Value": 239, "TypeName": Endeavor.formscriptfunctions.getParameterType("int"), "StructuralProperty": 1 },
                    { "Field": "Amount", "Value": amount, "TypeName": Endeavor.formscriptfunctions.getParameterType("float"), "StructuralProperty": 1 },
                    { "Field": "Mobile", "Value": mobile, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                    { "Field": "Email", "Value": email, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                    { "Field": "ContactId", "Value": contactIdValue, "TypeName": Edm.contact, "StructuralProperty": 5 },
                    { "Field": "ValidTo", "Value": validTo, "TypeName": Endeavor.formscriptfunctions.getParameterType("datetime"), "StructuralProperty": 1 },
                    { "Field": "ValidTo", "Value": validTo, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                    { "Field": "ValueCodeApprovalId", "Value": valCodeApprovIdValue, "TypeName": Edm.ed_valuecodeapproval, "StructuralProperty": 5 }];

                // Create the ValueCode
                Endeavor.formscriptfunctions.callGlobalAction("ed_CreateValueCodeGeneric",
                    inputParameters,
                    function (data) {
                        debugger;
                        valueCodeId = Object.values(data)[0];
                        formContext.ui.setFormNotification("Värdekod skapad.", "INFO");
                        valcodeId = Endeavor.Skanetrafiken.ValueCodeApproval.cleanIdField(valCodeApprovId);

                        var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeId.id, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                            { "Field": "Mobile", "Value": mobile, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                            { "Field": "Email", "Value": email, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                            { "Field": "TravelCardNumber", "Value": travelCardNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                        try {
                            Endeavor.formscriptfunctions.callAction("ed_ApproveValueCode", "ed_valuecodeapproval", valCodeApprovId,
                                inputParameters,
                                function () {
                                    try {
                                        // Sending the created ValueCode
                                        Endeavor.formscriptfunctions.callAction("ed_SendValueCode", ed_valuecode, valueCodeId.id,
                                            null,
                                            function () {
                                                debugger;
                                                formContext.data.refresh();
                                                formContext.ui.setFormNotification("Värdekod skickad.", "INFO");
                                            },
                                            function (e, t) {
                                                debugger;
                                                // Error
                                                formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");
                                                // Write the trace log to the dev console
                                                if (window.console && console.error) {
                                                    console.error(e + "\n" + t);
                                                }
                                            });
                                    } catch (error) {
                                        var err = error;
                                    }
                                    formContext.data.refresh();
                                    formContext.ui.setFormNotification("Värdekod godkänd.", "INFO");
                                },
                                function (er, tr) {
                                    debugger;
                                    // Error
                                    formContext.ui.setFormNotification("Någonting gick fel: " + er, "INFO");
                                    // Write the trace log to the dev console
                                    if (window.console && console.error) {
                                        console.error(er + "\n" + tr);
                                    }
                                });
                        } catch (err) {
                            var error = err;
                        }
                    },
                    function (e, t) {
                        debugger;
                        // Error
                        formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");
                        // Write the trace log to the dev console
                        if (window.console && console.error) {
                            console.error(e + "\n" + t);
                        }
                    });

            }
            catch (ex) {
                var error = ex;
                // Error
            }
        },
        declineValueCode: function (formContext) {
            debugger;
            try {
                var valCodeApprovId = formContext.data.entity.getId();
                Endeavor.formscriptfunctions.callAction("ed_DeclineValueCode", "ed_valuecodeapproval", valCodeApprovId,
                    [],
                    function () {
                        debugger;
                        formContext.data.refresh(true);
                        formContext.ui.setFormNotification("Värdekod nekad.", "INFO");
                    },
                    function (e, t) {
                        debugger;
                        // Error
                        formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");
                        // Write the trace log to the dev console
                        if (window.console && console.error) {
                            console.error(e + "\n" + t);
                        }
                    });
            }
            catch (error) {
                var err = error;
            }
        },
        showHideApproveButton: function (formContext) {
            var stat = formContext.getAttribute("statuscode").getValue();
            if (stat != 899310000)
                return true;
            else return false;
        },
        showHideDeclineButton: function (formContext) {
            var stat = formContext.getAttribute("statuscode").getValue();
            if (stat != 899310001 && stat != 899310000)
                return true;
            else return false;
        }
    };
}