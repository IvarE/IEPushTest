// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ValueCode) == "undefined") {
    Endeavor.Skanetrafiken.ValueCode = {

        onLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();
            formContext.ui.clearFormNotification("UpdateNotification");
        },

        createMultipleValueCodes: function (formContext) {

            debugger;
            var numberOfValueCodes = prompt("Ange antal värdekoder du vill skapa", "1");
            var isNum = /^\d+$/.test(numberOfValueCodes);
            if (numberOfValueCodes == null && isNum != true) {
                alert("Ogiltigt värde i antal värdekoder. Försök igen");
                return;
            }
            var numberOfValueCodesInt = parseInt(numberOfValueCodes);

            var amountInValueCodes = prompt("Ange belopp på värdekod(-er)", "50");
            isNum = /^\d+$/.test(amountInValueCodes);
            if (amountInValueCodes == null && isNum != true) {
                alert("Ogiltigt belopp. Försök igen");
                return;
            }
            var amountInValueCodesFloat = parseFloat(amountInValueCodes);

            formContext.ui.setFormNotification("Skapar värdekoder. Vänligen vänta.", "INFO");

            var inputParameters = [{ "Field": "Amount", "Value": amountInValueCodesFloat, "TypeName": "Edm.Decimal", "StructuralProperty": 1 },
                                { "Field": "Count", "Value": numberOfValueCodesInt, "TypeName": "Edm.Int32", "StructuralProperty": 1 }];

            Endeavor.formscriptfunctions.callGlobalAction("ed_CreateMultipleValueCodes", inputParameters,
                function () {
                    alert("3 nya värdekoder skapade.");
                    location.reload();
                },
                function (e, t) {
                    alert("Misslyckades att skapa nya värdekoder.");

                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },

        sendValueCode: function (formContext) {
            debugger;
            formContext.data.save();
            formContext.ui.setFormNotification("Skickar värdekod. Vänligen vänta.", "INFO");

            var valueCodeId = formContext.data.entity.getId().replace("{", "").replace("}", "");

            Endeavor.formscriptfunctions.callAction("ed_SendValueCode", "ed_valuecode", valueCodeId, null,
                function () {
                    formContext.data.refresh();
                    formContext.ui.setFormNotification("Värdekod skickad.", "INFO");
                },
                function (e, t) {
                    // Error
                    formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },

        updateValueCodeToCanceled: function (formContext, RECORD_ID) {

            debugger;
            var globalContext = Xrm.Utility.getGlobalContext();
            var systemUserId = globalContext.userSettings.userId.replace('{', '').replace('}', '');

            if (systemUserId != null && systemUserId != "undefined") {
                var today = new Date();
                var stateCodeInactive = 1;
                var statusReasonMakulerad = 899310004;
                var systemUserProp = "/systemusers(" + systemUserId + ")";

                var data = {

                    "ed_CanceledBy@odata.bind": systemUserProp,
                    "ed_canceledon": today,
                    "statecode": stateCodeInactive,
                    "statuscode": statusReasonMakulerad
                }

                Xrm.WebApi.updateRecord("ed_valuecode", RECORD_ID, data).then(
                    function success(result) {

                        debugger;
                        formContext.ui.clearFormNotification("UpdateNotification");
                        formContext.ui.clearFormNotification("värdekodInfo");

                        //Update ValueCode as inactive - Makulerad - Makulerad av (Current user)
                        formContext.setFormNotification("Värdekod makulerad.", "UpdateNotification");

                        setTimeout(function () {
                            // Call the Open Entity Form method and pass through the current entity name and ID to force CRM to reload the record
                            var entityFormOptions = {};
                            entityFormOptions["entityName"] = formContext.data.entity.getEntityName();
                            entityFormOptions["entityId"] = formContext.data.entity.getId();

                            // Open the form.
                            Xrm.Navigation.openForm(entityFormOptions).then(
                                function (success) {
                                    console.log(success);
                                },
                                function (error) {
                                    console.log(error);
                                });
                        }, 3000);
                    },
                    function (error) {
                        console.log(error.message);
                        debugger;

                        formContext.ui.clearFormNotification("värdekodInfo");
                        formContext.ui.clearFormNotification("UpdateNotification");
                        formContext.ui.setFormNotification("Någonting gick fel: " + error.message, "UpdateNotification");
                    }
                );
            }
        },

        cancelValueCode: function (formContext) {

            debugger;
            //Kontrollera attt värdekoden är av typen "Inlösen Reskassa"
            var valueCodeTypeAtr = formContext.getAttribute("ed_valuecodetypeglobal");
            var valueCodeType = 0;
            if (valueCodeTypeAtr != null && valueCodeTypeAtr != "undefined") {
                valueCodeType = valueCodeTypeAtr.getValue();
            }

            if (valueCodeType == null || valueCodeType == "undefined") {
                console.log("Cancel Button Clicked: Found no ValueCode Type on form!");
                alert("Värdekoden kan inte Makuleras: Hittade inte Värdekodstyp på värdekoden. Typen måste vara 'Inlösen Reskassa'!");
            }
            else if (valueCodeType != 2) {
                console.log("Cancel Button Clicked: Värdekoden måste vara av typen 'Inlösen Reskassa'!");
                alert("Värdekoden kan inte Makuleras: Värdekoden måste vara av typen 'Inlösen Reskassa'!");
            }
            else {

                console.log("Cancel Button Clicked!");
                var confirmation = confirm("Är du säker på att du vill makulera värdekoden?");

                if (confirmation) {

                    debugger;
                    console.log("Cancel Button Confirmed!");
                    //Might not work on View List page
                    formContext.ui.setFormNotification("Makulerar Värdekod(er). Detta kan ta några sekunder...", "värdekodInfo");

                    //Call new API Action
                    var valueCodeId = formContext.data.entity.getId().replace('{', '').replace('}', '');
                    var valueCodeEntityName = formContext.data.entity.getEntityName();

                    var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeId, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                    Endeavor.formscriptfunctions.callGlobalAction("ed_CancelValueCode", inputParameters,
                        function (result) {

                            formContext.ui.clearFormNotification("värdekodInfo");
                            formContext.ui.clearFormNotification("UpdateNotification");

                            if (result != null && result != "undefined") {
                                console.log("Status: " + result.status);
                                console.log("Result: " + result.Result);
                            }

                            Endeavor.Skanetrafiken.ValueCode.updateValueCodeToCanceled(valueCodeId);

                            if (result.status == 204) {
                                debugger;

                                alert("Status returned 204");
                                formContext.ui.clearFormNotification("värdekodInfo");
                                formContext.ui.clearFormNotification("UpdateNotification");

                                //Update ValueCode as inactive - Makulerad - Makulerad av (Current user)
                                //Endeavor.Skanetrafiken.ValueCode.updateValueCodeToCanceled(valueCodeId);

                                formContext.data.refresh();
                                formContext.ui.setFormNotification("Värdekod makulerad.", "UpdateNotification");
                            }
                            else {
                                formContext.ui.clearFormNotification("värdekodInfo");
                                formContext.ui.clearFormNotification("UpdateNotification");
                                console.log("Result: Null/undefined");
                            }
                        },
                        function (e) {
                            // Error
                            formContext.ui.clearFormNotification("värdekodInfo");
                            formContext.ui.clearFormNotification("UpdateNotification");
                            formContext.ui.setFormNotification("Någonting gick fel: " + e.message, "UpdateNotification");
                            //alert("Någonting gick fel: " + e);
                            // Write the trace log to the dev console
                            if (window.console && console.error)
                                console.error(e.message + "\n" + t);
                        });
                }
                else
                    console.log("Canceled execution!");
            }
        },

        cancelSingleOrMultipleValueCode: function (valueCodeArg) {
            debugger;
            console.log("Cancel Multiple Button Clicked!");

            if (valueCodeArg != null && valueCodeArg != "undefined" && valueCodeArg.length > 0) {
                debugger;
                var confirmation = confirm("Är du säker på att du vill makulera (dessa) värdekod?");

                if (confirmation) {
                    debugger;
                    console.log("Cancel Multiple Button Confirmed!");

                    //Create Vlaue Code String
                    var valueCodeIds = "";
                    if (valueCodeArg.length == 1) {
                        valueCodeIds = valueCodeArg[0].Id.replace('{', '').replace('}', '');
                        //Get current valuecode and check that it is of type "Inlösen reskassa"
                    }
                    else {
                        for (var i = 0; i < valueCodeArg.length; i++) {
                            valueCodeIds += valueCodeArg[i].Id.replace('{', '').replace('}', '') + ";";
                            //Get current valuecode and check that it is of type "Inlösen reskassa"
                        }
                    }


                    //If string contains ; make sure to create an array with value codes
                    var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeIds, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                    Endeavor.formscriptfunctions.callGlobalAction("ed_CancelValueCode", inputParameters,
                        function (result) {
                            alert("Värdekod(erna) makulerad(e).");
                        },
                        function (e) {
                            // Error
                            alert("Någonting gick fel: " + e.message);
                            // Write the trace log to the dev console
                            if (window.console && console.error)
                                console.error(e + "\n" + t);
                        });
                }
                else
                    console.log("Canceled execution!");
            }
            else {
                console.log("Argument returned null (Cancel Multiple Value Vodes)");
                alert("Du måste välje minst 1 värdekod att makulera!");
            }
        },

        hideCancelValueCodeButton: function () {
            return true;
        },
    }
}