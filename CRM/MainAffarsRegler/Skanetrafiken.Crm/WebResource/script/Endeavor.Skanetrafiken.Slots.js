// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Slots) == "undefined") {
    Endeavor.Skanetrafiken.Slots = {

        onClickExportSlots: function (formContext) {
            debugger;
            var dateFrom = new Date();
            var dateTo = new Date();

            var inputParameters = [
                { "Field": "FromDate", "Value": dateFrom, "TypeName": Endeavor.formscriptfunctions.getParameterType("datetime"), "StructuralProperty": 1 },
                { "Field": "ToDate", "Value": dateTo, "TypeName": Endeavor.formscriptfunctions.getParameterType("datetime"), "StructuralProperty": 1 }
            ];

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetExcelDatafromSlots", inputParameters,

                function (result) {
                    if (result.responseText != null && result.responseText != "undefined" && result.responseText != "") {
                        var response = JSON.parse(result.responseText);
                        if (response.OK == null || response.OK == false) {
                            if (response.ExcelBase64 != null && response.ExcelBase64 != "") {
                                debugger;
                            }
                            else {
                                debugger;
                            }
                        }
                    }
                    else {
                        alert("Error: Unknown problem generating Excel Slots.");
                        return;
                    }
                },
                function (error) {
                    var errorMessage = "Error in  Excel Slots creation." + error.message;
                    console.log(errorMessage);
                    Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
        }
    };
}