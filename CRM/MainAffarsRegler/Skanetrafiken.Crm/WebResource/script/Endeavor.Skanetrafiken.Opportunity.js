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
            //Endeavor.Skanetrafiken.Opportunity.SubGridFilterExecution(executionContext);
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
        },

        SubGridFilterExecution: function (executionContext) {
        //Create a Form Context.
            var formContext = executionContext.getFormContext();
            //Step 1 - Get the subgrid control.
                var gridContext = formContext.getControl("STAKEHOLDERS");
            //Step 2 - Retrieving Form Attribute Value.
            var company = formContext.getAttribute("parentaccountid");
                //var companyGuid = company[0].id;
                if (company == null) {
                    return;
                }

                company = formContext.getAttribute("parentaccountid").getValue();

            //Step 3 - Recall the execution method if the subgrid context is null or empty.
            if (gridContext == null) {
                setTimeout(Endeavor.Skanetrafiken.Opportunity.SubGridFilterExecution, 3000);
                return;
            }
            else {
                //Set grid with query A based fetch XML.
                if (company != null) {
                    //Step 4 - Build a fetch XML in a variable.
                    var FetchXmlA = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                        "<entity name='connection'>" +
                        "<filter type='and'>" +
                        "<condition attribute='parentcustomerid' operator='eq' uitype='account' value='" + company[0].id + "' /></filter>" +
                        "</entity>" +
                        "</fetch>";
                    //Step 5 - Update The Subrid Context
                    gridContext.setFilterXml(FetchXmlA);
                    //Step 6 - Refresh grid to show filtered records only.
                    formContext.getControl("STAKEHOLDERS").refresh();
                }
            }
        },

        DiscountPercentageOnChange: function (executionContext) {

            var formContext = executionContext.getFormContext();


            var totalAmount = formContext.getAttribute("ed_totalamountforcalculation").getValue();
            var detailAmount = formContext.getAttribute("totallineitemamount").getValue();
            var discountPercentage = formContext.getAttribute("ed_discountpercentage").getValue();

            if (discountPercentage == null || discountPercentage == 0) {
                formContext.getAttribute("discountpercentage").setValue(null);
            }
            else {
                var decimalPercentage = discountPercentage / 100;

                var calculateAmount = decimalPercentage * totalAmount;

                var discountPercentageToUpdate = (calculateAmount / detailAmount) * 100;

                formContext.getAttribute("discountpercentage").setValue(discountPercentageToUpdate);

            }

        },

        DetailAmountOnChange: function (executionContext) {

            var formContext = executionContext.getFormContext();
            var id = formContext.data.entity.getId();
            var entity = formContext.data.entity.getEntityName();

            formContext.data.save().then(
                function () { Xrm.Utility.openEntityForm(entity, id); },
                function () { alert(errorCode + message) })
            //formContext.data.refresh();

        }

    };
}