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
            var marketingListId = formContext.data.entity.getId().slice(1, -1);
            Endeavor.Skanetrafiken.MarketingList.dataValidationBeforeSMI(formContext, marketingListId, (responseMsg) => {
                if (responseMsg) {
                    Xrm.Navigation.openAlertDialog({ text: responseMsg });
                    return;
                }
                var confirmStrings = { text: "Är du säker på att du vill skicka marknadslista?", title: "Bekräfta att skicka marknadslista", confirmButtonLabel: "Ja", cancelButtonLabel: "Nej" };
                var confirmOptions = { height: 220, width: 450 };
                Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                    async function (success) {
                        if (success.confirmed) {
                            
                            var inputParameters = [{ "Field": "MarketingListId", "Value": marketingListId, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                            //Add loading indicator
                            var message = "Skickar marknadslista.";
                            Xrm.Utility.showProgressIndicator(message);
                            var messageInterval = setInterval(() => {
                                if (message == "Skickar marknadslista.") message = "Skickar marknadslista..";
                                else {
                                    if (message == "Skickar marknadslista..") message = "Skickar marknadslista...";
                                    else { if (message == "Skickar marknadslista...") message = "Skickar marknadslista."; }
                                }
                                Xrm.Utility.showProgressIndicator(message);
                            }, 500);

                            Endeavor.Skanetrafiken.MarketingList.callActionSendMarketingInfo(inputParameters, async (toClearMember, responseMessage) => {
                                //Close loading indicator
                                clearInterval(messageInterval);
                                Xrm.Utility.closeProgressIndicator();

                                if (toClearMember == null) { //something is wrong from integration
                                    Xrm.Navigation.openAlertDialog({ text: responseMessage }, { height: 220, width: 480 });
                                    return;
                                }                    
                                
                                confirmStrings.text = responseMessage +"\r\n\r\nVill du ta bort alla medlemmar från listan?\r\n\r\nEtt bakgrundsjobb kommer att ta bort medlemmar och uppdatera statusen för fältet \"Rensa medlemmar\" när det är klart.";
                                confirmStrings.title = "Bekräfta radering av medlemmar";
                                confirmOptions = { height: 280, width: 500 };
                                Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                                    async function (success) {
                                        if (success.confirmed) {

                                            if (formContext.getAttribute("st_clearmembers")) {
                                                formContext.getAttribute("st_clearmembers").setValue(toClearMember);//To Do: 206290000, In Progress: 206290002, Done: 206290003
                                                formContext.getAttribute("st_clearmembers").setSubmitMode("never");
                                                //await formContext.data.entity.save();                                    
                                            }
                                            await Xrm.WebApi.updateRecord("list", marketingListId, { st_clearmembers: toClearMember });
                                             
                                        }
                                    });
                            });
                        }
                        else
                            console.log("Endeavor.Skanetrafiken.MarketingList: Canceled or X sending Marketing List.");
                    });
            });
        },

        dataValidationBeforeSMI: function (formContext, listId, callBack) {
            formContext.data.save().then(async function done() {
                var accessButton = Endeavor.Skanetrafiken.MarketingList.showSendMarketingListButton();
                if (!accessButton) {
                    callBack("Du har inte rätt rättigheter till att skicka narknadslista. Kontakta administratör.");
                    return;
                }
                if (formContext.getAttribute("type").getValue() !== false) {
                    callBack("Skickar marknadslista fungerar bara för statisk listtypen.");
                    return;
                }

                if (formContext.getAttribute("createdfromcode").getValue() != 2) {
                    callBack("Skickar marknadslista fungerar bara för målgruppen som är kontakt.");
                    return;
                }

                if (formContext.getAttribute("st_clearmembers") && (formContext.getAttribute("st_clearmembers").getValue() == 206290000 || formContext.getAttribute("st_clearmembers").getValue() == 206290001)) {
                    callBack("Det går inte att skicka narknadslista när listans medlemmar håller att tömmas.");
                    return;
                }

                let checkCounter = formContext.getAttribute("membercount") && formContext.getAttribute("membercount").getValue() != null ? formContext.getAttribute("membercount").getValue() : 0;
                if (checkCounter == 0) {
                    checkCounter = await Xrm.WebApi.retrieveMultipleRecords("listmember", "?$select=_entityid_value&$filter=_listid_value eq " + listId, 1);
                    if (checkCounter.entities.length == 0) {
                        callBack("Vänligen lägg till listmedlemmar för att fortsätta.");
                        return;
                    }
                }

                let alertMessage = "";
                var checkCampaignList = await Xrm.WebApi.online.retrieveRecord("list", listId, "?$select=listid,st_clearmembers&$expand=campaignlist_association($select=campaignid)")
                if (checkCampaignList.st_clearmembers == 206290002) {
                    //reset flagg
                    await Xrm.WebApi.updateRecord("list", listId, { st_clearmembers: null });
                }

                if (checkCampaignList.st_clearmembers == 206290000 || checkCampaignList.st_clearmembers == 206290001) ////To Do: 206290000, In Progress: 206290001, Done: 206290002
                    alertMessage = "Det går inte att skicka narknadslista när listans medlemmar håller att tömmas.";
                else {
                    if (checkCampaignList.campaignlist_association && (checkCampaignList.campaignlist_association.length == 0 || checkCampaignList.campaignlist_association.length > 1)) {
                        alertMessage = "Det gick inte att skicka narknadslista: ";
                        if (checkCampaignList.campaignlist_association.length > 1) alertMessage += "Det finns flera än ett kampanjer kopplad till marknadslista";// "There is more than one Campaign connected to the Marketing List.";                                                
                        else alertMessage += "Det finns ingen kampanjer kopplad till marknadslista";// "There is no Campaigns connected to the Marketing List.";
                    }
                }
                callBack(alertMessage);

            }, function error(error) {
                callBack(error.message);
            });
        },

        callActionSendMarketingInfo: function (inputParameters, callBack) {
            
            Endeavor.formscriptfunctions.callGlobalAction("ed_SendMarketingInfo", inputParameters,
                function (result) {
                    var clearMemberStatus = null;
                    var resultMessage = "";
                    if (result.responseText != null && result.responseText != "undefined" && result.responseText != "") {
                        var response = JSON.parse(result.responseText);

                        if (response == null || response == undefined) {
                            resultMessage = "Det gick inte att skicka narknadslista. Kontakta administratör.";
                        }
                        else {
                            if (response.Ok == false) {
                                resultMessage = "Det gick inte att skicka narknadslista: " + response.Message;
                            }
                            else if (response.Ok == true) {

                                resultMessage = "Marknadslista skickad.";
                                clearMemberStatus = 206290000;//To Do: 206290000,
                                console.log("Endeavor.Skanetrafiken.MarketingList: " + response.Message);
                            }
                        }
                    }
                    else {
                        resultMessage = "Error: Unknown problem sending Marketing Info.";
                    }

                    callBack(clearMemberStatus, resultMessage);
                },
                function (error) {
                    //Close loading indicator
                    var errorMessage = "Error in  Sending Marketing Info." + error.message;
                    callBack(null, errorMessage);
                });
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