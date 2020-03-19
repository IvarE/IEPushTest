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

        lockFieldsBasedOnProcessStage: function () {
            debugger;
            var processAttr = Xrm.Page.getAttribute("processid");
            if (processAttr) {
                var stageAttr = Xrm.Page.getAttribute("stageid");
                var stageId = stageAttr.getValue();
                if (!stageId)
                    return;
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
            debugger;
            var tryOutPeriodFrom = Endeavor.Skanetrafiken.Campaign.compileTryoutFromString();
            if (tryOutPeriodFrom == "")
                return;

            var tryOutPeriodTo = Endeavor.Skanetrafiken.Campaign.compileTryoutToString();
            if (tryOutPeriodTo == "")
                return;

            var lastAnswerDate = Endeavor.Skanetrafiken.Campaign.compileLastAnswerDate();
            if (lastAnswerDate == "")
                return;

            var DR = lastAnswerDate.substr(lastAnswerDate.length - 3, 3);
            lastAnswerDate = lastAnswerDate.substr(0, lastAnswerDate.length - 3);

            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "LeadSet?$select=FirstName,LastName,Address1_Line1,Address1_Line2,Address1_Line3,Address1_PostalCode,Address1_City,LeadSourceCode,ed_NearestStop,ed_CampaignCode&$filter=(CampaignId/Id eq (guid'" + Xrm.Page.data.entity.getId() + "')) and (StateCode/Value eq 0)";
            var leadsResultSet = Endeavor.Common.Data.fetchJSONResults(url, 1000000);

            if (!leadsResultSet || leadsResultSet.length < 1) {
                alert("No Leads found");
                return;
            }

            var csv = "Tilltalsnamn;Efternamn;C/O-adress;Adress;Fortsättningsadress;Postnr;Ort;Adresskälla;Närmaste Hållplats;Kampanjkoder;Prova på - Från;Prova på - Till;Svara senast DR";

            for (var i = 0; i < leadsResultSet.length; i++) {
                var tilltalsnamn = leadsResultSet[i].FirstName;
                var efternamn = leadsResultSet[i].LastName;
                var coAdress = leadsResultSet[i].Address1_Line1;
                var adress = leadsResultSet[i].Address1_Line2;

                var fortsättningsadress = leadsResultSet[i].Address1_Line3;
                var postnr = leadsResultSet[i].Address1_PostalCode;
                var ort = leadsResultSet[i].Address1_City;
                for (var j = 0; j < Xrm.Page.getAttribute("ed_leadsource").getOptions().length; j++) {
                    if (Xrm.Page.getAttribute("ed_leadsource").getOptions()[j].value == leadsResultSet[i].LeadSourceCode.Value)
                        var adresskalla = Xrm.Page.getAttribute("ed_leadsource").getOptions()[j].text;
                }

                var narmasteHallplats = leadsResultSet[i].ed_NearestStop;
                var kampanjkoder = leadsResultSet[i].ed_CampaignCode;
                var provaPaFran = tryOutPeriodFrom;
                var provaPaTill = tryOutPeriodTo;
                var svaraSenastDR = lastAnswerDate;

                var csvLine = "\n" + tilltalsnamn + ";" + efternamn + ";" + coAdress + ";" + adress + ";"
                    + fortsättningsadress + ";" + postnr + ";" + ort + ";" + adresskalla + ";"
                    + narmasteHallplats + ";" + kampanjkoder + ";" + provaPaFran + ";" + provaPaTill + ";" + svaraSenastDR;

                csv += csvLine;
            }
            var remindStr = "";
            if (DR == "DR2") {
                remindStr = ".Reminder"
            }

            debugger;

            Endeavor.Skanetrafiken.Campaign.downloadFile("Skanetrafiken." + Xrm.Page.getAttribute("name").getValue() + ".Leads" + remindStr + ".csv", csv);

            //var downloadLink = document.createElement("a");
            //var blob = new Blob(["\ufeff", csv]);
            //var url = URL.createObjectURL(blob);
            //downloadLink.href = url;
            //downloadLink.download = "Skanetrafiken." + Xrm.Page.getAttribute("name").getValue() + ".Leads" + remindStr + ".csv";

            //document.body.appendChild(downloadLink);
            //downloadLink.click();
            //document.body.removeChild(downloadLink);
        },

        compileTryoutFromString: function () {
            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "CampaignSet?$select=processstage_campaigns/StageCategory&$expand=processstage_campaigns&$filter=CampaignId eq (guid'" + Xrm.Page.data.entity.getId() + "')";
            var campaignResultSet = Endeavor.Common.Data.fetchJSONResults(url);

            if (!campaignResultSet || campaignResultSet.length < 1 || !campaignResultSet[0].processstage_campaigns || !campaignResultSet[0].processstage_campaigns.StageCategory) {
                alert("Something went wrong. Please try again")
                return "";
            }

            var tryOutFromVal;
            if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                var tryOutFromAttr = Xrm.Page.getAttribute("ed_tryoutfromphase1");
                if (!tryOutFromAttr) {
                    alert("Please add the 'Try out Period - From (Primary Sendout)'-field to the form and try again");
                    return "";
                }
                tryOutFromVal = tryOutFromAttr.getValue();

                if (!tryOutFromVal || tryOutFromVal == "") {
                    alert("Please enter data in the 'Try out Period - From (Primary Sendout)'-field.");
                    return "";
                }
            }
            else if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                var tryOutFromAttr = Xrm.Page.getAttribute("ed_tryoutfromphase2");
                if (!tryOutFromAttr) {
                    alert("Please add the 'Try out Period - From (Reminder Sendout)'-field to the form and try again");
                    return "";
                }
                tryOutFromVal = tryOutFromAttr.getValue();

                if (!tryOutFromVal || tryOutFromVal == "") {
                    alert("Please enter data in the 'Try out Period - From (Reminder Sendout)'-field.");
                    return "";
                }
            }
            if (!tryOutFromVal) {
                alert("Campaign needs to be in an active stage.")
                return "";
            }
            var tryOutFromStr = (tryOutFromVal.getYear() + 1900) + "-" + (tryOutFromVal.getMonth() + 1) + "-" + tryOutFromVal.getDate();

            return tryOutFromStr;
        },

        compileTryoutToString: function () {
            var url = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "CampaignSet?$select=processstage_campaigns/StageCategory&$expand=processstage_campaigns&$filter=CampaignId eq (guid'" + Xrm.Page.data.entity.getId() + "')";
            var campaignResultSet = Endeavor.Common.Data.fetchJSONResults(url);

            if (!campaignResultSet || campaignResultSet.length < 1 || !campaignResultSet[0].processstage_campaigns || !campaignResultSet[0].processstage_campaigns.StageCategory) {
                alert("Something went wrong. Please try again")
                return "";
            }

            var tryOutToVal;
            if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                var tryOutToAttr = Xrm.Page.getAttribute("ed_tryouttophase1");
                if (!tryOutToAttr) {
                    alert("Please add the 'Try out Period - To (Primary Sendout)'-field to the form and try again");
                    return "";
                }
                tryOutToVal = tryOutToAttr.getValue();

                if (!tryOutToVal || tryOutToVal == "") {
                    alert("Please enter data in the 'Try out Period - To (Primary Sendout)'-field.");
                    return "";
                }
            }
            else if (campaignResultSet[0].processstage_campaigns.StageCategory.Value == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                var tryOutToAttr = Xrm.Page.getAttribute("ed_tryouttophase2");
                if (!tryOutToAttr) {
                    alert("Please add the 'Try out Period - To (Reminder Sendout)'-field to the form and try again");
                    return "";
                }
                tryOutToVal = tryOutToAttr.getValue();

                if (!tryOutToVal || tryOutToVal == "") {
                    alert("Please enter data in the 'Try out Period - To (Reminder Sendout)'-field.");
                    return "";
                }
            }
            if (!tryOutToVal) {
                alert("Campaign needs to be in an active stage.")
                return "";
            }
            var tryOutToStr = (tryOutToVal.getYear() + 1900) + "-" + (tryOutToVal.getMonth() + 1) + "-" + tryOutToVal.getDate();

            return tryOutToStr;
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
            var lastRespStr = (lastRespVal.getYear() + 1900) + "-" + (lastRespVal.getMonth() + 1) + "-" + lastRespVal.getDate();

            return lastRespStr + DR;
        },


        // Code below copied from https://github.com/angular-ui/ui-grid/issues/2312

        /**
        * @ngdoc function
        * @name isIEBelow10
        * @methodOf  ui.grid.exporter.service:uiGridExporterService
        * @description Checks whether current browser is IE and of version below 10
        */
        isIEBelow10: function () {
            var myNav = navigator.userAgent.toLowerCase();
            return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) < 10 : false;
        },

        /**
        * @ngdoc function
        * @name downloadFile
        * @methodOf  ui.grid.exporter.service:uiGridExporterService
        * @description Triggers download of a csv file.  Logic provided
        * by @cssensei (from his colleagues at https://github.com/ifeelgoods) in issue #2391
        * @param {string} fileName the filename we'd like our file to be
        * given
        * @param {string} csvContent the csv content that we'd like to 
        * download as a file
        */
        downloadFile: function (fileName, csvContent) {
            var D = document;
            var a = D.createElement('a');
            var strMimeType = 'application/octet-stream;charset=utf-8';
            var rawFile;

            if (!fileName) {
                var currentDate = new Date();
                fileName = "CSV Export - " + currentDate.getFullYear() + (currentDate.getMonth() + 1) +
                    currentDate.getDate() + currentDate.getHours() +
                    currentDate.getMinutes() + currentDate.getSeconds() + ".csv";
            }

            if (this.isIEBelow10()) {
                var frame = D.createElement('iframe');
                document.body.appendChild(frame);

                frame.contentWindow.document.open("text/html", "replace");
                frame.contentWindow.document.write('sep=,\r\n' + csvContent);
                frame.contentWindow.document.close();
                frame.contentWindow.focus();
                frame.contentWindow.document.execCommand('SaveAs', true, fileName);

                document.body.removeChild(frame);
                return true;
            }

            // IE10+
            if (navigator.msSaveBlob) {
                return navigator.msSaveBlob(new Blob(["\ufeff", csvContent], {
                    type: strMimeType
                }), fileName);
            }

            //html5 A[download]
            if ('download' in a) {
                var blob = new Blob(["\ufeff", csvContent], {
                    type: strMimeType
                });
                rawFile = URL.createObjectURL(blob);
                a.setAttribute('download', fileName);
            } else {
                rawFile = 'data:' + strMimeType + ',' + encodeURIComponent(csvContent);
                a.setAttribute('target', '_blank');
                a.setAttribute('download', fileName);
            }


            a.href = rawFile;
            a.setAttribute('style', 'display:none;');
            D.body.appendChild(a);
            setTimeout(function () {
                if (a.click) {
                    a.click();
                    // Workaround for Safari 5
                } else if (document.createEvent) {
                    var eventObj = document.createEvent('MouseEvents');
                    eventObj.initEvent('click', true, true);
                    a.dispatchEvent(eventObj);
                }
                D.body.removeChild(a);

            }, 100);
        },

    }
}