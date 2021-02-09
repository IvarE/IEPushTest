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
            Xrm.Utility.openEntityForm("incident");

        }
        catch (e) {
            alert("Error in CGISweden.queueitemRibbon.CreateNewCase\n\n", e.Message);
        }
    }

};



