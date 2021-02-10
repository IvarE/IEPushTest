if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }


// *******************************************************
// Entity: incident ribbon 
// *******************************************************

CGISweden.accountRibbon =
{

    CreateNewCase: function (accountid) {
        try {

            var _accountid = accountid;

            var parameters = {};
            parameters["cgi_accountid"] = _accountid;
            //            parameters["cgi_accountidname"] = "Anders Andersson";
            //            parameters["cgi_accountidtype"] = "account";

            // Open the window.
            Xrm.Utility.openEntityForm("incident", null, parameters);

        }
        catch (e) {
            alert("Error in CGISweden.accountRibbon.CreateNewCase\n\n", e.Message);
        }
    }





};



