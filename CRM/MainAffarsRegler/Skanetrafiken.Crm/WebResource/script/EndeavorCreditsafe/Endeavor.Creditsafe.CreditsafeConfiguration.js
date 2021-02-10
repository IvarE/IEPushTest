// JavaScript source code
// Begin scoping 
// References:
/// <reference path="Endeavor.Common.Page.js" />
/// <reference path="Endeavor.Common.Data.js" />
/// <reference path="vsdoc/XrmPage-vsdoc.js" />

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

        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var searchEngineAttribute = formContext.getAttribute("edp_searchengine");
            if (searchEngineAttribute != null) {
                var searchEngineValue = searchEngineAttribute.getValue();
                if (searchEngineValue == "757550000") { //Sweden Company Search
                    Endeavor.Creditsafe.CreditsafeConfiguration.showHideFields(formContext, true);
                    return;
                }
            }
            Endeavor.Creditsafe.CreditsafeConfiguration.showHideFields(formContext, false);
        },

        showHideFields: function (formContext, visibleFlag) {
            try {
                var testEndPointControl = formContext.getControl("edp_endpoint");
                if (testEndPointControl != null) {
                    testEndPointControl.setVisible(visibleFlag);
                }
            } catch (error) {
                //do nothing
            }
        },

        testCreditsafeConnection: function () {

            var request = {
                getMetadata: function () {
                    return {
                        boundParameter: null,
                        operationType: 0,
                        operationName: "edp_GetCredentials"
                    };
                }
            };

            Xrm.WebApi.online.execute(request).then(
                function (result) {
                    if (result.ok) {
                        result.json().then(
                            function (response) {
                                var jsonString = response.configcredentials;
                                var data = jsonString.split(';');

                                var username = data[1];
                                var password = data[2];
                                var url = "https://login.creditsafe.com/?P1=" + username + "&P2=" + password;
                                window.open(url);
                            }
                        );
                    } else {
                        throw new Error("The request failed. Please try again.");
                    }
                }, function (error) {
                    throw new Error("An error occurred. " + error);
                }
            );
        }

    }
};

