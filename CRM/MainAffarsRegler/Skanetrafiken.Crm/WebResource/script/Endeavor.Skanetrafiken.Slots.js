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

        getExtension: function (str) {
            var strArr = str.split('.')
            return strArr[strArr.length - 1];
        },

        generateElement: function (window, filename, base64) {
            var el;
            if (window) {
                el = window.document.createElement("a");
            }
            else {
                el = document.createElement("a");
            }
            el.innerText = filename;
            el.id = "1";
            el.download = filename;
            el.href = "data:application/octet-stream;base64," + base64;
            return el;
        },

        downloadFile: function (filename, base64) {

            var el = Endeavor.Skanetrafiken.Slots.generateElement(null, filename, base64);

            var byteString = atob(base64.replace("\"", "").replace("\"", ""));
            var ab = new ArrayBuffer(byteString.length);
            var ia = new Uint8Array(ab);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            var ext = Endeavor.Skanetrafiken.Slots.getExtension(filename);
            var blob = new Blob([ia], { type: "application/" + ext });

            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
                navigator.msSaveBlob(blob, filename);
            else {
                var url = URL.createObjectURL(blob);
                el.href = url;
                el.click();
            }
        },

        getExcelData: function (inputParameters) {
            debugger;
            Endeavor.formscriptfunctions.callGlobalAction("ed_GetExcelDatafromSlots", inputParameters,

                function (result) {
                    if (result.responseText != null && result.responseText != "undefined" && result.responseText != "") {
                        var response = JSON.parse(result.responseText);
                        if (response.OK == null || response.OK == false) {
                            if (response.ExcelBase64 != null && response.ExcelBase64 != "") {
                                debugger;
                                Endeavor.Skanetrafiken.Slots.downloadFile("ExcelSlots.xlsx", response.ExcelBase64);
                            }
                            else {
                                debugger;
                                alert("error");
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
        },

        onClickExportSlots: function () {
            debugger;
            var windowOptions = { height: 800, width: 800 };
            Xrm.Navigation.openWebResource("ed_/html/Endeavor.Skanetrafiken.SlotsExcelExport.html", windowOptions);
        }
    };
}