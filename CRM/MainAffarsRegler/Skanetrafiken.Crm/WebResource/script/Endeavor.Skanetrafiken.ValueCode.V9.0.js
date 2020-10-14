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

        onLoad: function () {

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

            var inputParameters = [];
            var parameterAmount = { "Field": "Amount", "Value": amountInValueCodesFloat, "TypeName": "Edm.Decimal", "StructuralProperty": 1 };
            var parameterCount = { "Field": "Count", "Value": numberOfValueCodesInt, "TypeName": "Edm.Int32", "StructuralProperty": 1 };

            inputParameters.push(parameterAmount);
            inputParameters.push(parameterCount);

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

            Endeavor.formscriptfunctions.callAction("ed_SendValueCode", "ed_valuecode", , null,
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
    }
}