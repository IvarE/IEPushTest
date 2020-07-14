if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }


// *******************************************************
// Entity: queueitem ribbon 
// *******************************************************

CGISweden.queueitemRibbon =
{

    CreateNewCase: function () {
        try {

            // Open the window.
            Xrm.Navigation.openForm("incident");

        }
        catch (e) {
            alert("Error in CGISweden.queueitemRibbon.CreateNewCase\n\n", e.Message);
        }
    }

};



