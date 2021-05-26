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

        updateValueCodeToCanceled: function (formContext, RECORD_ID, fromForm, cancelValueCode) {

            debugger;
            var returnValue = false;
            var globalContext = Xrm.Utility.getGlobalContext();
            var systemUserId = globalContext.userSettings.userId.replace('{', '').replace('}', '');

            if (systemUserId != null && systemUserId != "undefined") {

                var today = new Date();
                var systemUserProp = "/systemusers(" + systemUserId + ")";

                if (cancelValueCode == true) {
                    data = {
                        "ed_CanceledBy@odata.bind": systemUserProp,
                        "ed_canceledon": today,
                    }
                }
                else {
                    data = {
                        "ed_CanceledBy@odata.bind": null,
                        "ed_canceledon": null,
                    }
                }

                data = {
                    "ed_CanceledBy@odata.bind": systemUserProp,
                    "ed_canceledon": today,
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
                        formContext.ui.setFormNotification("Någonting gick fel vid uppdatering: " + error.message, "UpdateNotification");
                    }
                );
            }
        },

        cancelValueCode: async function (formContext) {

            debugger;
            console.log("Cancel Button Clicked!");
            var fromForm = true;

            //Kontrollera attt värdekoden är av typen "Inlösen Reskassa/Presentkort"
            //var valueCodeTypeAtr = formContext.getAttribute("ed_valuecodetypeglobal");
            //var valueCodeType = 0;
            //if (valueCodeTypeAtr != null && valueCodeTypeAtr != "undefined") {
            //    valueCodeType = valueCodeTypeAtr.getValue();
            //}

            var vcVoucherAtr = formContext.getAttribute("ed_valuecodevoucherid");
            var vcVoucherId = "";
            if (vcVoucherAtr != null && vcVoucherAtr != "undefined") {
                vcVoucherId = vcVoucherAtr.getValue();
            }

            //if (valueCodeType == null || valueCodeType == "undefined") {
            //    console.log("Cancel Button Clicked: Found no ValueCode Type on form!");
            //    alert("Värdekoden kan inte Makuleras: Hittade inte Värdekodstyp på värdekoden.");
            //}
            //else if (valueCodeType != 2 && valueCodeType != 5) {
            //    console.log("Cancel Button Clicked: Värdekoden måste vara av typen 'Inlösen Reskassa' eller 'Presentkort'!");
            //    alert("Värdekoden kan inte Makuleras: Värdekoden måste vara av typen 'Inlösen Reskassa' eller 'Presentkort'!");
            //}
            if (vcVoucherId == null || vcVoucherId == "undefined" || vcVoucherId == 0) {
                console.log("Cancel Button Clicked: Found no ValueCode Code-ID on form!");
                alert("Värdekoden kan inte Makuleras: Hittade inte en Voucher kod för Värdekoden.");
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
                    var valueCodeEntityName = formContext.data.entity.getEntityName();
                    var valueCodeCRMId = formContext.data.entity.getId().replace('{', '').replace('}', '');

                    var inputParameters = [{ "Field": "ValueCodeId", "Value": vcVoucherId, "TypeName": "Edm.String", "StructuralProperty": 1 }];

                    debugger;
                    Endeavor.Skanetrafiken.ValueCode.callGlobalAction("ed_CancelValueCode", inputParameters,
                        function (result) {
                            debugger;
                            if (result != null && result != "undefined") {
                                console.log("Result: " + result.responseText);
                            }

                            var parsedResult = JSON.parse(result.responseText);

                            if (parsedResult.Result.startsWith("200")) {

                                formContext.ui.clearFormNotification("värdekodInfo");
                                formContext.ui.clearFormNotification("UpdateNotification");
                                alert("Värdekod makulerad.");

                                //Update ValueCode as inactive - Makulerad - Makulerad av (Current user)
                                var globalContext = Xrm.Utility.getGlobalContext();
                                var systemUserId = globalContext.userSettings.userId.replace('{', '').replace('}', '');
                                var today = new Date();
                                var systemUserProp = "/systemusers(" + systemUserId + ")";

                                var data = {
                                    "ed_CanceledBy@odata.bind": systemUserProp,
                                    "ed_canceledon": today,
                                }

                                Xrm.WebApi.updateRecord("ed_valuecode", valueCodeCRMId, data).then(
                                    function success(result) {

                                        debugger;


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
                                        formContext.ui.setFormNotification("Någonting gick fel vid uppdatering: " + error.message, "UpdateNotification");
                                    }
                                );
                            }
                            else {

                                formContext.ui.clearFormNotification("värdekodInfo");
                                formContext.ui.clearFormNotification("UpdateNotification");
                                console.log("Execution returned: " + parsedResult.Result);

                                var confirmationMakulerad = confirm("Värdekod kunde ej makuleras. Execution returned: " + parsedResult.Result);

                                if (confirmationMakulerad) {
                                    console.log("Execution End");
                                }
                                else {
                                    console.log("Execution End");
                                }
                            }
                        },
                        function (e) {
                            // Error
                            formContext.ui.clearFormNotification("värdekodInfo");
                            formContext.ui.clearFormNotification("UpdateNotification");
                            //formContext.ui.setFormNotification("Någonting gick fel: " + e.message, "UpdateNotification");
                            //alert("Någonting gick fel: " + e);
                            // Write the trace log to the dev console

                            var confirmationMakulerad = confirm("Värdekod kunde ej makuleras. Execution returned: " + e.message);

                            if (confirmationMakulerad) {
                                Xrm.Utility.openEntityForm(valueCodeEntityName, valueCodeCRMId);
                                formContext.ui.setFormNotification("Värdekod kunde ej makuleras.", "UpdateNotification");
                            }
                            else {
                                Xrm.Utility.openEntityForm(valueCodeEntityName, valueCodeCRMId);
                                formContext.ui.setFormNotification("Värdekod kunde ej makuleras.", "UpdateNotification");
                            }

                            if (window.console && console.error)
                                console.error(e.message + "\n" + t);
                        });
                }
                else {
                    console.log("Canceled execution!");
                }
            }
        },

        cancelSingleOrMultipleValueCode: async function (valueCodeArg) {
            debugger;
            console.log("Cancel Multiple Button Clicked!");
            var fromForm = false;

            if (valueCodeArg != null && valueCodeArg != "undefined" && valueCodeArg.length > 0) {
                debugger;
                var confirmation = confirm("Är du säker på att du vill makulera (dessa) värdekod?");

                if (confirmation) {
                    debugger;
                    console.log("Cancel Multiple Button Confirmed!");

                    //Create Vlaue Code String
                    var valueCodeIds = "";

                    //New Code
                    if (valueCodeArg.length == 1) {
                        debugger;
                        valueCodeIds = valueCodeArg[0].Id.replace('{', '').replace('}', '');
                        console.log("ValueCode(s): " + valueCodeIds);

                        //Get current valuecode and check that it is of type "Inlösen reskassa" or "Presentkort"
                        //var validValueCode = false;
                        var validValueCode = "";

                        validValueCode = await Endeavor.Skanetrafiken.ValueCode.checkIfValueCodeIsValidSync(valueCodeIds); //DevOps: 3888
                        debugger;

                        if (validValueCode == "NO-inactive") {
                            //Meddela att den är inaktive
                            alert("Värdekoden kan inte Makuleras: Värdekoden måste vara aktiv för att makuleras!");
                        }
                        else if (validValueCode == "NO-voucherid")
                        {
                            //Meddela att den inte har en Vouchercode Id
                            console.log("Cancel Button Clicked: Found no VoucherCode-ID");
                            alert("Värdekoden kan inte Makuleras: Hittade inte en Voucher kod för Värdekoden.");
                        }
                        else if (validValueCode == "YES" && valueCodeIds != "") {
                            console.log("ValueCode(s): " + valueCodeIds);
                            valueCodeIds += ";";
                            console.log("Changed ValueCode(s): " + valueCodeIds);

                            var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeIds, "TypeName": "Edm.String", "StructuralProperty": 1 }];

                            Endeavor.Skanetrafiken.ValueCode.callGlobalAction("ed_CancelValueCode", inputParameters,
                                function (result) {
                                    //Success
                                    if (result != null && result != "undefined") {
                                        console.log("Result: " + result.responseText);
                                    }

                                    var parsedResult = JSON.parse(result.responseText);

                                    debugger;
                                    if (parsedResult.Result.startsWith("200")) {
                                        debugger;
                                        var confirmation = confirm("Värdekod(erna) makulerad(e).");
                                        if (confirmation) {
                                            console.log("Execution Done!");
                                        }

                                    } else {
                                        console.log("Execution returned: " + parsedResult.Result);
                                        alert("Execution returned: " + parsedResult.Result);
                                    }
                                },
                                function (e) {
                                    // Error
                                    alert("Någonting gick fel: " + e.message);
                                    // Write the trace log to the dev console
                                    if (window.console && console.error)
                                        console.error(e.message + "\n" + t);
                                });

                        }
                        else {
                            alert("Värdekoden kan inte Makuleras: Värdekoden måste vara aktiv samt innehålla en VoucherCodeId för att makuleras!");
                        }
                    }
                    else if (valueCodeArg.length > 1) {
                        var nrOfValueCodes = valueCodeArg.length;
                        var invalideValidValueCodeCount = 0;
                        var inactiveValueCodeCount = 0;
                        var noVouchercodeValuecodeCount = 0;
                        var validValueCodeCount = 0;
                        var warningMessage = "";

                        for (var i = 0; i < nrOfValueCodes; i++) {
                            debugger;
                            var validValueCode = "";

                            //(OLD) Get current valuecode and check that it is of type "Inlösen reskassa" (OLD)
                            //We only need to check that its an active ValueCode (DevOps: 3888)
                            var checkValueCode = valueCodeArg[i].Id.replace('{', '').replace('}', '');

                            //Async
                            validValueCode = await Endeavor.Skanetrafiken.ValueCode.checkIfValueCodeIsValidSync(checkValueCode); //DevOps: 3888

                            //Populate string with valid Value Code Id:s
                            if (validValueCode == "YES") {
                                valueCodeIds += checkValueCode + ";";
                            }
                            else {
                                invalideValidValueCodeCount++;
                                if (validValueCode == "NO-inactive")
                                {
                                    inactiveValueCodeCount++;
                                }
                                else if (validValueCode == "NO-voucherid")
                                {
                                    noVouchercodeValuecodeCount++;
                                }
                            }
                        }

                        debugger;
                        if (valueCodeIds != "") {

                            if (invalideValidValueCodeCount > 0) {
                                //Found invalid ValueCodes -> Provide warning before canceling
                                debugger;
                                validValueCodeCount = nrOfValueCodes - invalideValidValueCodeCount;
                                if (inactiveValueCodeCount > 0 && noVouchercodeValuecodeCount > 0) {
                                    //Message for both.
                                    warningMessage = invalideValidValueCodeCount + " värdekod(er) av " + nrOfValueCodes + " kan inte makuleras. " + inactiveValueCodeCount + " värdekod(er) är inte aktiv(a) och " + noVouchercodeValuecodeCount + " saknar Voucher kod. Vill du fortsätta makulera resterande " + validValueCodeCount + " värdekod(er)?";
                                }
                                else if (inactiveValueCodeCount > 0) {
                                    //Message for inactive.
                                    warningMessage = invalideValidValueCodeCount + " värdekod(er) av " + nrOfValueCodes + " kan inte makuleras då Värdekoden inte är aktiv(a). Vill du fortsätta makulera resterande " + validValueCodeCount + " värdekod(er)?";
                                }
                                else if (noVouchercodeValuecodeCount > 0)
                                {
                                    //Message for VoucherCode.
                                    warningMessage = invalideValidValueCodeCount + " värdekod(er) av " + nrOfValueCodes + " kan inte makuleras då Värdekoden saknar Voucher kod. Vill du fortsätta makulera resterande " + validValueCodeCount + " värdekod(er)?";
                                }
                                
                                var cancelConfirmation = confirm(warningMessage);

                                if (cancelConfirmation) {
                                    console.log("ValueCode(s): " + valueCodeIds);

                                    //Cancel ValueCode
                                    var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeIds, "TypeName": "Edm.String", "StructuralProperty": 1 }];

                                    Endeavor.Skanetrafiken.ValueCode.callGlobalAction("ed_CancelValueCode", inputParameters,
                                        function (result) {
                                            //Success
                                            if (result != null && result != "undefined") {
                                                console.log("Result: " + result.responseText);
                                            }

                                            var parsedResult = JSON.parse(result.responseText);

                                            debugger;
                                            if (parsedResult.Result.startsWith("200")) {
                                                debugger;
                                                var confirmation = confirm("Värdekod(erna) makulerad(e).");
                                                if (confirmation) {
                                                    console.log("Execution Done!");
                                                }

                                            } else {
                                                console.log("Execution returned: " + parsedResult.Result);
                                                alert("Execution returned: " + parsedResult.Result);
                                            }
                                        },
                                        function (e) {
                                            // Error
                                            alert("Någonting gick fel: " + e);
                                            // Write the trace log to the dev console
                                            if (window.console && console.error)
                                                console.error(e.message + "\n" + t);
                                        });
                                }
                                else {
                                    alert("Inga Värdekoder Makulerades.");
                                }
                            }
                            else {
                                //Found no invalid ValueCodes -> Continue with process
                                console.log("ValueCode(s): " + valueCodeIds);
                                //Cancel ValueCode
                                var inputParameters = [{ "Field": "ValueCodeId", "Value": valueCodeIds, "TypeName": "Edm.String", "StructuralProperty": 1 }];

                                Endeavor.Skanetrafiken.ValueCode.callGlobalAction("ed_CancelValueCode", inputParameters,
                                    function (result) {
                                        //Success
                                        if (result != null && result != "undefined") {
                                            console.log("Result: " + result.responseText);
                                        }

                                        var parsedResult = JSON.parse(result.responseText);

                                        debugger;
                                        if (parsedResult.Result.startsWith("200")) {
                                            debugger;
                                            var confirmation = confirm("Värdekod(erna) makulerad(e).");
                                            if (confirmation) {
                                                console.log("Execution Done!");
                                            }

                                        } else {
                                            console.log("Execution returned: " + parsedResult.Result);
                                            alert("Execution returned: " + parsedResult.Result);
                                        }
                                    },
                                    function (e) {
                                        // Error
                                        alert("Någonting gick fel: " + e.message);
                                        // Write the trace log to the dev console
                                        if (window.console && console.error)
                                            console.error(e.message + "\n" + t);
                                    });
                            }
                        }
                        else {
                            alert("Värdekod(erna) kan inte Makuleras: Värdekod(erna) måste vara aktiv(a) samt innehålla en Voucher Kod för att makuleras (" + inactiveValueCodeCount + " är inaktiv(a) och " + noVouchercodeValuecodeCount + " saknar Voucher Kod)!");
                        }
                    }
                    else {
                        alert("Inga värdekoder kunde hittas för Makulering!");
                    }
                }
                else
                    console.log("Canceled execution!");
            }
            else {
                console.log("Argument returned null (Cancel Multiple Value Vodes)");
                alert("Du måste välje minst 1 värdekod att makulera!");
            }
        },

        checkIfValueCodeIsValidSync: async function (valueCodeIds) {

            console.log("Executing: retrieveMultipleRecordsFunction1!");
            debugger;
            //var validValueCode = false;
            var hasVoucherId = "";
            var isValidValueCode = "";

            var fetchXml = [
                "<fetch mapping='logical'>",
                "<entity name='ed_valuecode'>",
                "<attribute name='ed_valuecodetypeglobal' />",
                "<attribute name='ed_valuecodeid' />",
                "<attribute name='ed_valuecodevoucherid' />",
                "<attribute name='statuscode' />", //Makulerad, Inlöst...
                "<attribute name='statecode' />", //Aktiv, Inactive...
                "<filter type='and'>",
                "<condition attribute='ed_valuecodeid' operator='eq' value='", valueCodeIds, "' />",
                "<condition attribute='statecode' operator='eq' value='0' />",
                //"<filter type='or'>",
                //"<condition attribute='ed_valuecodetypeglobal' operator='eq' value='2' />", //DevOps: 3888
                //"<condition attribute='ed_valuecodetypeglobal' operator='eq' value='5' />",
                //"</filter>",
                "</filter>",
                "</entity>",
                "</fetch>",
            ].join("");

            var fetchXmltoUse = "?fetchXml=" + encodeURIComponent(fetchXml);
            var apiResult = await Xrm.WebApi.retrieveMultipleRecords("ed_valuecode", fetchXmltoUse);
            if (apiResult != null && apiResult != "undefined") {
                debugger;
                if (apiResult.entities != null && apiResult.entities != "undefined") {
                    if (apiResult.entities.length != null && apiResult.entities.length != "undefined") {
                        var count = apiResult.entities.length;
                        if (count > 0) {
                            //validValueCode = true;
                            //Check that the entity has a VoucherID
                            hasVoucherId = apiResult.entities[0].ed_valuecodevoucherid;
                            if (hasVoucherId != "" && hasVoucherId != "undefined" && hasVoucherId != null) {
                                isValidValueCode = "YES";
                            }
                            else {
                                isValidValueCode = "NO-voucherid";
                            }
                        }
                        else {
                            //Check that the entity has a VoucherID
                            isValidValueCode = "NO-inactive";
                        }
                    }
                }
            }

            return isValidValueCode;
        },

        hideCancelValueCodeButton: function () {
            return true;
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
    }
}