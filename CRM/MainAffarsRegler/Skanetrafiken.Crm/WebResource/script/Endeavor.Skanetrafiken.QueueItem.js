if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) === "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.QueueItem) === "undefined") {

    //Form Methods CGI QueItem (from queueitemRibbon.js)
    CreateNewCase: function () {
        try {
            // Open the window.
            Xrm.Navigation.openForm("incident");
        }
        catch (e) {
            alert("Error in Endeavor.Skanetrafiken.QueueItem.CreateNewCase\n\n", e.message);
        }
    }
}