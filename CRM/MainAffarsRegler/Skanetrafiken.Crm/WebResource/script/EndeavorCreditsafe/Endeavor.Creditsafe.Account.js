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

        onLoad: function () {
            var searchEngineAttribute = Xrm.Page.getAttribute("edp_creditsafesearchengine");
            if (searchEngineAttribute != null) {
                var searchEngineValue = searchEngineAttribute.getValue();
                if (searchEngineValue == "757550000") {
                    Endeavor.Creditsafe.Account.showHideFields(true);
                    return;
                }
            }
            Endeavor.Creditsafe.Account.showHideFields(false);
        },

        showHideFields: function (visibleFlag) {
            try {
                var lastCreditReportCtrl = Xrm.Page.getControl("edp_lastcreditreport");
                var creditRatingCtrl = Xrm.Page.getControl("edp_creditrating");
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
