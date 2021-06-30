var abortProcess = false;
var recordsProcessed = 0;

if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Merge) == "undefined") {
    Endeavor.Skanetrafiken.Merge = {

        startMergeWindow: function () {
            try {

                var globalContext = Xrm.Utility.getGlobalContext();
                var clientURL = globalContext.getClientUrl();

                var url = clientURL + "/WebResources/ed_/html/MergeRecordsStarter.html";

                var left = (screen.width - 300) / 2;
                var top = (screen.height - 150) / 2;
                myCustomWindow = window.open(url, "gsaWindow", "width=400,height=300,left=" + left + ",top=" + top);
                myCustomWindow.focus();
            }
            catch (error) {
                alert(error.message);
            }
        },

        closeWindow: function () {
            window.close();
        },

        startMerge: function () {
            try {
                debugger;
                abortProcess = false;
                recordsProcessed = 0;

                $("#spinnerDiv").text("Starting merge");
                setTimeout(Endeavor.Skanetrafiken.Merge.runRecursive(), 50);
            }
            catch (error) {
                alert("Exception caught in startMerge().\r\n\r\n" + error.message);
            }
            finally {
            }
        },

        runRecursive: function () {
            try {

                Endeavor.formscriptfunctions.callGlobalAction("ed_MergeRecordsActionWorkflow", null,
                    function (result) {

                        debugger;
                        recordsProcessed++;
                        $("#spinnerDiv").text("Merged " + recordsProcessed.toString() + " batches");

                        var remaining = result.RemainingRecords;
                        if (remaining == 0) {
                            $("#spinnerDiv").text("Done!");
                            abortProcess = true;
                            Endeavor.Skanetrafiken.Merge.closeWindow();
                        }

                        if (!abortProcess)
                            setTimeout(Endeavor.Skanetrafiken.Merge.runRecursive(), 50);

                    },
                    function (error) {
                        var errorMessage = "Exception caught in startMerge_inner(). Details: " + error.message;
                        console.log(errorMessage);
                        Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                        $("body").removeClass("wait");
                        abortProcess = true;
                    });                
            }
            catch (error) {
                alert("Exception caught in startMerge_inner().\r\n\r\n" + error);
                $("body").removeClass("wait");
                abortProcess = true;
            }
            finally {
            }
        },
    };
};