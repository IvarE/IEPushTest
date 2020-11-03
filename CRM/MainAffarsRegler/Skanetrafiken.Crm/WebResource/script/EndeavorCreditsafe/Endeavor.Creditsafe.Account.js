if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Creditsafe) == "undefined") {
    Endeavor.Creditsafe = {
    };
}

if (typeof (Endeavor.Creditsafe.Account) == "undefined") {
    Endeavor.Creditsafe.Account = {

        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var searchEngineAttribute = formContext.getAttribute("edp_creditsafesearchengine");
            if (searchEngineAttribute != null) {
                var searchEngineValue = searchEngineAttribute.getValue();
                if (searchEngineValue == "757550000") {
                    Endeavor.Creditsafe.Account.showHideFields(formContext, true);
                    return;
                }
            }
            Endeavor.Creditsafe.Account.showHideFields(formContext, false);
        },

        showHideFields: function (formContext, visibleFlag) {
            try {
                var lastCreditReportCtrl = formContext.getControl("edp_lastcreditreport");
                var creditRatingCtrl = formContext.getControl("edp_creditrating");
                if (lastCreditReportCtrl != null) {
                    lastCreditReportCtrl.setVisible(visibleFlag);
                }
                if (creditRatingCtrl != null) {
                    creditRatingCtrl.setVisible(visibleFlag);
                }
            } catch (error) {
                //do nothing
            }
        }
    };
}
