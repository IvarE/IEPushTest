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

        onLoad: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();
            Endeavor.Skanetrafiken.Campaign.lockFieldsBasedOnProcessStage(formContext);
        },

        lockFieldsBasedOnProcessStage: function (formContext) {
            debugger;
            var processAttr = formContext.getAttribute("processid");
            if (processAttr) {
                var stageAttr = formContext.getAttribute("stageid");
                var stageId = stageAttr.getValue();
                if (!stageId)
                    return;
            }
        },

        //Used by the button in CRM- Account overview
        openSearchWindow: function (formContext) {

            var idRecord = formContext.data.entity.getId();

            Process.callWorkflow("1A68FEA1-7C7E-E611-8103-00155D0A6B01",
                idRecord,
                function () {
                    alert("Workflow executed successfully");
                },
                function () {
                    alert("Error executing workflow");
                });
        },

        onExportLeads: function (formContext) {
            debugger;
            var globalContext = Xrm.Utility.getGlobalContext();
            var clientUrl = globalContext.getClientUrl();

            var tryOutPeriodFrom = Endeavor.Skanetrafiken.Campaign.compileTryoutFromString(formContext, clientUrl);
            if (tryOutPeriodFrom == "")
                return;

            var tryOutPeriodTo = Endeavor.Skanetrafiken.Campaign.compileTryoutToString(formContext, clientUrl);
            if (tryOutPeriodTo == "")
                return;

            var lastAnswerDate = Endeavor.Skanetrafiken.Campaign.compileLastAnswerDate(formContext, clientUrl);
            if (lastAnswerDate == "")
                return;

            var DR = lastAnswerDate.substr(lastAnswerDate.length - 3, 3);
            lastAnswerDate = lastAnswerDate.substr(0, lastAnswerDate.length - 3);

            var columnSet = "firstname,lastname,address1_line1,address1_line2,address1_line3,address1_postalcode,address1_city,leadsourcecode,ed_neareststop,ed_campaigncode";
            var idRecord = formContext.data.entity.getId().replace("{", "").replace("}", "");

            Xrm.WebApi.retrieveMultipleRecords("lead", "?$select=" + columnSet + "&$filter=campaignid eq " + idRecord + " and statecode eq 0").then(
                function success(results) {

                    if (!results || results.entities.length < 1) {
                        alert("No Leads found");
                        return;
                    }

                    var csv = "Tilltalsnamn;Efternamn;C/O-adress;Adress;Fortsättningsadress;Postnr;Ort;Adresskälla;Närmaste Hållplats;Kampanjkoder;Prova på - Från;Prova på - Till;Svara senast DR";

                    for (var i = 0; i < results.entities.length; i++) {
                        var tilltalsnamn = results.entities[i].firstname;
                        var efternamn = results.entities[i].lastname;
                        var coAdress = results.entities[i].address1_line1;
                        var adress = results.entities[i].address1_line2;

                        var fortsättningsadress = results.entities[i].address1_line3;
                        var postnr = results.entities[i].address1_postalcode;
                        var ort = results.entities[i].address1_city;
                        for (var j = 0; j < formContext.getAttribute("ed_leadsource").getOptions().length; j++) {
                            if (formContext.getAttribute("ed_leadsource").getOptions()[j].value == results.entities[i].leadsourcecode)
                                var adresskalla = formContext.getAttribute("ed_leadsource").getOptions()[j].text;
                        }

                        var narmasteHallplats = results.entities[i].ed_neareststop;
                        var kampanjkoder = results.entities[i].ed_campaigncode;
                        var provaPaFran = tryOutPeriodFrom;
                        var provaPaTill = tryOutPeriodTo;
                        var svaraSenastDR = lastAnswerDate;

                        var csvLine = "\n" + tilltalsnamn + ";" + efternamn + ";" + coAdress + ";" + adress + ";"
                            + fortsättningsadress + ";" + postnr + ";" + ort + ";" + adresskalla + ";"
                            + narmasteHallplats + ";" + kampanjkoder + ";" + provaPaFran + ";" + provaPaTill + ";" + svaraSenastDR;

                        csv += csvLine;
                    }
                    var remindStr = "";
                    if (DR == "DR2")
                        remindStr = ".Reminder"

                    debugger;
                    Endeavor.Skanetrafiken.Campaign.downloadFile("Skanetrafiken." + formContext.getAttribute("name").getValue() + ".Leads" + remindStr + ".csv", csv);
                },
                function (error) {
                    console.log(error.message);
                    Endeavor.formscriptfunctions.ErrorCustomDialog(error.message, "Retrieve Multiple Records Error");
                });
        },

        compileTryoutFromString: function (formContext, clientUrl) {

            var idRecord = formContext.data.entity.getId();

            var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>"+
                    "<entity name='campaign'>"+
                        "<attribute name='campaignid' />"+
                        "<filter type='and'>"+
                            "<condition attribute='campaignid' operator='eq' value='" + idRecord + "' />" +
                        "</filter>" +
                        "<link-entity name='processstage' from='processstageid' to='stageid' alias='aa'>" +
                            "<attribute name='stagecategory' />" +
                        "</link-entity>" +
                    "</entity>" +
                "</fetch>";

            var url = clientUrl + "/api/data/v9.0/campaigns?fetchXml=" + encodeURIComponent(fetchxml);
            var campaignResultSet = Endeavor.formscriptfunctions.fetchJSONResults(url);

            var tryOutFromVal;
            if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                var tryOutFromAttr = formContext.getAttribute("ed_tryoutfromphase1");
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
            else if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                var tryOutFromAttr = formContext.getAttribute("ed_tryoutfromphase2");
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

        compileTryoutToString: function (formContext, clientUrl) {

            var idRecord = formContext.data.entity.getId();

            var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>" +
                    "<entity name='campaign'>" +
                        "<attribute name='campaignid' />" +
                        "<filter type='and'>" +
                            "<condition attribute='campaignid' operator='eq' value='" + idRecord + "' />" +
                        "</filter>" +
                        "<link-entity name='processstage' from='processstageid' to='stageid' alias='aa'>" +
                            "<attribute name='stagecategory' />" +
                        "</link-entity>" +
                    "</entity>" +
                "</fetch>";

            var url = clientUrl + "/api/data/v9.0/campaigns?fetchXml=" + encodeURIComponent(fetchxml);
            var campaignResultSet = Endeavor.formscriptfunctions.fetchJSONResults(url);

            if (!campaignResultSet || campaignResultSet.length < 1 || !campaignResultSet[0]["aa_x002e_stagecategory"]) {
                alert("Something went wrong. Please try again");
                return "";
            }

            var tryOutToVal;
            if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                var tryOutToAttr = formContext.getAttribute("ed_tryouttophase1");
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
            else if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                var tryOutToAttr = formContext.getAttribute("ed_tryouttophase2");
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

        compileLastAnswerDate: function (formContext, clientUrl) {

            var idRecord = formContext.data.entity.getId();

            var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>" +
                    "<entity name='campaign'>" +
                        "<attribute name='campaignid' />" +
                        "<filter type='and'>" +
                            "<condition attribute='campaignid' operator='eq' value='" + idRecord + "' />" +
                        "</filter>" +
                        "<link-entity name='processstage' from='processstageid' to='stageid' alias='aa'>" +
                            "<attribute name='stagecategory' />" +
                        "</link-entity>" +
                    "</entity>" +
                "</fetch>";

            var url = clientUrl + "/api/data/v9.0/campaigns?fetchXml=" + encodeURIComponent(fetchxml);
            var campaignResultSet = Endeavor.formscriptfunctions.fetchJSONResults(url);

            if (!campaignResultSet || campaignResultSet.length < 1 || !campaignResultSet[0]["aa_x002e_stagecategory"]) {
                alert("Something went wrong. Please try again")
                return "";
            }

            var lastRespVal;
            var DR;
            if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Kvalificera) {
                DR = "DR1";
                var lastResponseAttr = formContext.getAttribute("ed_validtophase1");
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
            else if (campaignResultSet[0]["aa_x002e_stagecategory"] == Endeavor.Skanetrafiken.OptionSet.processstage_category.Tafram) {
                DR = "DR2";
                var lastResponseAttr = formContext.getAttribute("ed_validtophase2");
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