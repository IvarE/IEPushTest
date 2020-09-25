
// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Opportunity) == "undefined") {
    Endeavor.Skanetrafiken.Opportunity = {

        onLoad: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Opportunity.updateEstimatedWeightedeRevenue(executionContext);
            formContext.data.process.addOnStageChange(function () { Endeavor.Skanetrafiken.Opportunity.updateEstimatedWeightedeRevenue(executionContext); });
        },

        updateEstimatedWeightedeRevenue: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var probabilityAttribute = formContext.getAttribute("closeprobability");
            var estimatedValueAttribute = formContext.getAttribute("estimatedvalue");
            var estWeightedRevenueAttribute = formContext.getAttribute("ed_estweightedrevenue");
            var stepNameAttribute = formContext.getAttribute("stepname");

            if ((probabilityAttribute != null && probabilityAttribute != "undefined") &&
                (estimatedValueAttribute != null && estimatedValueAttribute != "undefined") &&
                (estWeightedRevenueAttribute != null && estWeightedRevenueAttribute != "undefined") &&
                (stepNameAttribute != null && stepNameAttribute != "undefined")) {
                debugger;
                var estimatedValue = estimatedValueAttribute.getValue();
                var stepNameValue = stepNameAttribute.getValue();
                var revenueValue = null;

                if (stepNameValue == "2-Develop") {
                    //Set Probability to 25
                    probabilityAttribute.setValue(25);

                    if (estimatedValue != null && estimatedValue > 0) {
                        revenueValue = estimatedValue * 0.25;
                        estWeightedRevenueAttribute.setValue(revenueValue);
                    }
                    else if (estimatedValue == null || estimatedValue == 0)
                        estWeightedRevenueAttribute.setValue(0);
                }
                else if (stepNameValue == "3-Propose") {
                    //Set Probability to 50
                    probabilityAttribute.setValue(50);

                    if (estimatedValue != null && estimatedValue > 0) {
                        revenueValue = estimatedValue * 0.50;
                        estWeightedRevenueAttribute.setValue(revenueValue);
                    }
                    else if (estimatedValue == null || estimatedValue == 0)
                        estWeightedRevenueAttribute.setValue(0);
                }
                else if (stepNameValue == "4-Close") {
                    //Set Probability to 80
                    probabilityAttribute.setValue(80);

                    if (estimatedValue != null && estimatedValue > 0) {
                        revenueValue = estimatedValue * 0.80;
                        estWeightedRevenueAttribute.setValue(revenueValue);
                    }
                    else if (estimatedValue == null || estimatedValue == 0)
                        estWeightedRevenueAttribute.setValue(0);
                }

                formContext.data.entity.save();
            }
        }
    };
}