// JavaScript source code
// Begin scoping 
// References:
/// <reference path="Endeavor.Common.Page.js" />
/// <reference path="Endeavor.Common.Data.js" />
/// <reference path="vsdoc/XrmPage-vsdoc.js" />
/// <reference path="vsdoc/Sdk.Soap.vsdoc.js" />
/// <reference path="vsdoc/Sdk.edp_GetCredentials.vsdoc.js" />
/// <reference path="SDK.Rest.js" />
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Creditsafe) == "undefined") {
    Endeavor.Creditsafe = {
    };
}

if (typeof (Endeavor.Creditsafe.CreditsafeConfiguration) == "undefined") {
    Endeavor.Creditsafe.CreditsafeConfiguration = {

        testCreditsafeConnection: function () {

            var request = new Sdk.edp_GetCredentialsRequest();
            var response = Sdk.Sync.execute(request);

            if (response === null) {
                window.alert("response was null");
                throw new Error("The request did not return anything.");
            } else {

                var jsonString = response.getConfigcredentials();
                var data = jsonString.split(';');

                var username = data[1];
                var password = data[2];

                var url = "https://login.creditsafe.com/?P1=" + username + "&P2=" + password;
                window.open(url);

            }
        }

    }
};

