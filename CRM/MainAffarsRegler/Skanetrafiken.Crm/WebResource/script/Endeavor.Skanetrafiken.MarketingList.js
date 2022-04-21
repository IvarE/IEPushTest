// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.MarketingList) == "undefined") {
    Endeavor.Skanetrafiken.MarketingList = {

        onSendMarketingInfo: function (formContext) {
            debugger;
            var marketingListId = formContext.data.entity.getId();

            if (marketingListId != null && marketingListId != "") {

                marketingListId = marketingListId.replace("{", "").replace("}", "");

                var inputParameters = [
                    { "Field": "MarketingListId", "Value": marketingListId, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }
                ];

                Endeavor.formscriptfunctions.callGlobalAction("ed_SendMarketingInfo", inputParameters,
                    function (result) {
                        if (result.responseText != null && result.responseText != "undefined" && result.responseText != "") {
                            var response = JSON.parse(result.responseText);
                            debugger;
                            if (response == null || response == undefined) {
                                
                                var confirmation = confirm("Det gick inte att skicka Marknadslista. Kontakta administratör.");
                                if (confirmation) {
                                    console.log("Endeavor.Skanetrafiken.MarketingList: Response was null or undefined!");
                                } else {
                                    console.log("Endeavor.Skanetrafiken.MarketingList: Response was null or undefined!");
                                }
                            }
                            else
                            {
                                if (response.Ok == false) {
                                    
                                    var confirmation = confirm("Det gick inte att skicka Marknadslista: " + response.Message);
                                    if (confirmation) {
                                        console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                    } else {
                                        console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                    }
                                }
                                else if (response.Ok == true)
                                {
                                    
                                    var confirmation = confirm("Marknadslista Skickad.");
                                    if (confirmation) {
                                        console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                    } else {
                                        console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                    }
                                }
                            }
                        }
                        else {
                            alert("Error: Unknown problem sending Marketing Info.");
                            return;
                        }
                    },
                    function (error) {
                        var errorMessage = "Error in  Sending Marketing Info." + error.message;
                        console.log(errorMessage);
                        Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
            }
        },
    }
}