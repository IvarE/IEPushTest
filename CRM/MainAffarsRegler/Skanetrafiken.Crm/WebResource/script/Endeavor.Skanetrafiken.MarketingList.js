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

            var accessButton = false;
            accessButton = Endeavor.Skanetrafiken.MarketingList.showSendMarketingListButton();

            if (formContext?.getAttribute("st_ticket_sent")?.getValue() == true) {
                confirm("Biljett har redan skickats för den här marknadslistan.");
                return;
            }

            if (accessButton == false) {
                var validationConfirmation = confirm("Du har inte rätt rättigheter till att skicka Marknadslista. Kontakta administratör.");
                if (validationConfirmation) {
                    console.log("Endeavor.Skanetrafiken.MarketingList: showSendMarketingListButton returned false.");
                } else {
                    console.log("Endeavor.Skanetrafiken.MarketingList: showSendMarketingListButton returned false.");
                }
            }
            else
            {
                var validationConfirmation = confirm("Är du säker på att du vill skicka Marknadslista?");
                if (validationConfirmation) {
                    console.log("Endeavor.Skanetrafiken.MarketingList: Sending Marketing List.");
                    var marketingListId = formContext.data.entity.getId();

                    if (marketingListId != null && marketingListId != "") {

                        marketingListId = marketingListId.replace("{", "").replace("}", "");

                        var inputParameters = [
                            { "Field": "MarketingListId", "Value": marketingListId, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }
                        ];

                        //Add loading indicator
                        Xrm.Utility.showProgressIndicator("Skickar Marknadslista...");

                        Endeavor.formscriptfunctions.callGlobalAction("ed_SendMarketingInfo", inputParameters,
                            function (result) {
                                //Close loading indicator
                                Xrm.Utility.closeProgressIndicator();

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
                                    else {
                                        if (response.Ok == false) {

                                            var confirmation = confirm("Det gick inte att skicka Marknadslista: " + response.Message);
                                            if (confirmation) {
                                                console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                            } else {
                                                console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                                            }
                                        }
                                        else if (response.Ok == true) {
                                            formContext?.getAttribute("st_ticket_sent")?.setValue(true);
                                            formContext?.getAttribute("st_ticket_sent")?.setSubmitMode("always");
                                            formContext?.data?.save();
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
                                //Close loading indicator
                                Xrm.Utility.closeProgressIndicator();

                                var errorMessage = "Error in  Sending Marketing Info." + error.message;
                                console.log(errorMessage);
                                Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                            });
                    }

                } else {
                    console.log("Endeavor.Skanetrafiken.MarketingList: Canceled sending Marketing List.");
                }
            }
        },

        showSendMarketingListButton: function () {

            var roles = [];
            roles[0] = "Marketing List Sender"; //Security role for access to sending Marketing Lists
            var isUserCheck = Endeavor.Skanetrafiken.MarketingList.currentUserHasSecurityRole(roles);

            return isUserCheck;
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
    }
}