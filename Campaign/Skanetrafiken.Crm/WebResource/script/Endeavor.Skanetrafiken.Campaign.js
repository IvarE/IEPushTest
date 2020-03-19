// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Campaign) == "undefined") {
    Endeavor.Skanetrafiken.Campaign = {

        onLoad: function () {
            debugger;
            Endeavor.Skanetrafiken.Campaign.lockFieldsBasedOnProcessStage();
        },

        lockFieldsBasedOnProcessStage: function() {
            debugger;
            var processAttr = Xrm.Page.getAttribute("processid");
            if (processAttr) {
                var stageAttr = Xrm.Page.getAttribute("stageid");
                var stageId = stageAttr.getValue();
                var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "ProcessStageSet?$select=StageCategory&$filter=ProcessStageId eq (guid'" + stageId + "')";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(url);

                if (resultSet) {
                    // If not in 'Propose'-state, lock all fields needed
                    if (resultSet[0].StageCategory.Value != 2) {
                        //var fieldsToLock = Endeavor.Skanetrafiken.Campaign.getFieldsToLock();
                        //for (var i = 0; i < fieldsToLock.length; i++) {
                        //    fieldsToLock[i].setDisabled(true);
                        //}
                    }
                }
            }
        },

        getFieldsToLock: function () {
            var fields = {
                // no fields needing to be locked, Products and Leads blocked by plugin
            };
        },

        //Used by the button in CRM- Account overview
        openSearchWindow: function () {
            Process.callWorkflow("1A68FEA1-7C7E-E611-8103-00155D0A6B01",
                Xrm.Page.data.entity.getId(),
                function () {
                    alert("Workflow executed successfully");
                },
                function () {
                    alert("Error executing workflow");
                });
        },

        onExportLeads: function () {
            var tryOutPeriod = Endeavor.Skanetrafiken.Campaign.compileTryoutString();
            if (tryOutPeriod == "")
                return;

            var lastAnswerDate = Endeavor.Skanetrafiken.Campaign.compileLastAnswerDate();
            if (lastAnswerDate == "")
                return;

            var DR = lastAnswerDate.substr(lastAnswerDate.length - 3, 3);
            lastAnswerDate = lastAnswerDate.substr(0, lastAnswerDate.length - 3);

            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "LeadSet?$select=FirstName,LastName,Address1_Line1,Address1_Line2,Address1_Line3,Address1_PostalCode,Address1_City,ed_AddressSourceName,ed_NearestStop,ed_CampaignCode&$filter=(CampaignId/Id eq (guid'" + Xrm.Page.data.entity.getId() + "')) and (StateCode/Value eq 0)";
            var leadsResultSet = Endeavor.Common.Data.fetchJSONResults(url);

            if (!leadsResultSet || leadsResultSet.length < 1) {
                alert("No Leads found");
                return;
            }

            var csv = "Tilltalsnamn;Efternamn;C/O-adress;Adress;Fortsättningsadress;Postnr;Ort;Adresskälla;Närmaste Hållplats;Kampanjkoder;Prova på-period DR;Svara senast DR";

            for (var i = 0; i < leadsResultSet.length; i++) {
                var tilltalsnamn = leadsResultSet[i].FirstName;
                var efternamn = leadsResultSet[i].LastName;
                var coAdress = leadsResultSet[i].Address1_Line1;
                var adress = leadsResultSet[i].Address1_Line2;

                var fortsättningsadress = leadsResultSet[i].Address1_Line3;
                var postnr = leadsResultSet[i].Address1_PostalCode;
                var ort = leadsResultSet[i].Address1_City;
                var adresskalla = leadsResultSet[i].ed_AddressSourceName;

                var narmasteHallplats = leadsResultSet[i].ed_NearestStop;
                var kampanjkoder = leadsResultSet[i].ed_CampaignCode;
                var provaPaPeriodDR = tryOutPeriod;
                var svaraSenastDR = lastAnswerDate;

                var csvLine = "\n" + tilltalsnamn + ";" + efternamn + ";" + coAdress + ";" + adress + ";"
                    + fortsättningsadress + ";" + postnr + ";" + ort + ";" + adresskalla + ";"
                    + narmasteHallplats + ";" + kampanjkoder + ";" + provaPaPeriodDR + ";" + svaraSenastDR;

                csv += csvLine;
            }
            var remindStr = "";
            if (DR == "DR2") {
                remindStr = ".Reminder"
            }
            // Fix FileDownload
            debugger;
            var downloadLink = document.createElement("a");
            var blob = new Blob(["\ufeff", csv]);
            var url = URL.createObjectURL(blob);
            downloadLink.href = url;
            downloadLink.download = "Skanetrafiken." + Xrm.Page.getAttribute("name").getValue() + ".Leads" + remindStr + ".csv";

            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        },

        compileTryoutString: function () {
            var tryOutFromAttr = Xrm.Page.getAttribute("ed_tryoutfromphase1");
            var tryOutToAttr = Xrm.Page.getAttribute("ed_tryouttophase1");
            if (!tryOutFromAttr && !tryOutToAttr) {
                alert("Please add the 'Try out Period - From'- and 'Try out Period - To'-field to the form and try again");
                return "";
            }
            if (!tryOutFromAttr) {
                alert("Please add the 'Try out Period - From'-field to the form and try again");
                return "";
            }
            if (!tryOutToAttr) {
                alert("Please add the 'Try out Period - To'-field to the form and try again");
                return "";
            }
            var tryOutFrom = tryOutFromAttr.getValue();
            var tryOutTo = tryOutToAttr.getValue();

            if (!tryOutFrom || tryOutFrom == "") {
                alert("Please enter data in the 'Try out Period - From'-field.");
                return "";
            }
            if (!tryOutTo || tryOutTo == "") {
                alert("Please enter data in the 'Try out Period - To'-field.");
                return "";
            }

            var retVal;
            if (tryOutFrom.getMonth() == tryOutTo.getMonth()) {
                retVal = tryOutFrom.getDate() + " - " + tryOutTo.getDate() + "/" + tryOutFrom.getMonth();
            } else {
                retVal = tryOutFrom.getDate() + "/" + tryOutFrom.getMonth() + " - " + tryOutTo.getDate() + "/" + tryOutTo.getMonth();
            }

            return retVal;
        },

        compileLastAnswerDate: function () {
            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "CampaignSet?$select=processstage_campaigns/StageCategory&$expand=processstage_campaigns&$filter=CampaignId eq (guid'" + Xrm.Page.data.entity.getId() + "')";
            var campaignResultSet = Endeavor.Common.Data.fetchJSONResults(url);

            if (!campaignResultSet || campaignResultSet.length < 1 || !campaignResultSet[0].processstage_campaigns || !campaignResultSet[0].processstage_campaigns.StageCategory) {
                alert("Something went wrong. Please try again")
                return "";
            }

            var lastRespVal;
            var DR;
            if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                DR = "DR1";
                var lastResponseAttr = Xrm.Page.getAttribute("ed_validtophase1");
                if (!lastResponseAttr || lastResponseAttr == "") {
                    alert("Please add the 'Valid To Phase 1'-field to the form and try again")
                    return "";
                }
                lastRespVal = lastResponseAttr.getValue();
                if (!lastRespVal) {
                    alert("Please enter data in the 'Valid To Phase 1'-field")
                    return "";
                }
            }
            else if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                DR = "DR2";
                var lastResponseAttr = Xrm.Page.getAttribute("ed_validtophase2");
                if (!lastResponseAttr || lastResponseAttr == "") {
                    alert("Please add the 'Valid To Phase 2'-field to the form and try again")
                    return "";
                }
                lastRespVal = lastResponseAttr.getValue();
                if (!lastRespVal) {
                    alert("Please enter data in the 'Valid To Phase 2'-field")
                    return "";
                }
            }
            if (!lastRespVal) {
                alert("Campaign needs to be in an active stage.")
                return "";
            }
            var lastRespStr = lastRespVal.getDate() + "/" + lastRespVal.getMonth() + DR;

            return lastRespStr;
        }
    }
}