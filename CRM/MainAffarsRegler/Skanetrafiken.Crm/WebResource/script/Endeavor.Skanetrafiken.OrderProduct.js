﻿
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.OrderProduct) == "undefined") {
    Endeavor.Skanetrafiken.OrderProduct = {

        ManualDiscountOnChange: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var manualDiscount = formContext.getAttribute("ed_manualdiscount").getValue();
            var numberOfSlots = formContext.getAttribute("ed_totalslots").getValue();

            var fromDate = formContext.getAttribute("ed_fromdate").getValue();
            var toDate = formContext.getAttribute("ed_todate").getValue();

            if (numberOfSlots == null || numberOfSlots == 0) {

                if (fromDate != null && toDate != null) {

                    var DayValue = 1000 * 60 * 60 * 24;
                    numberOfSlots = ((toDate.getTime() - fromDate.getTime()) / DayValue) + 1;

                }
                else {
                    numberOfSlots == 0;
                }

            }

            if (manualDiscount == null || manualDiscount == 0) {
                formContext.getAttribute("manualdiscountamount").setValue(null);
            }
            else {
                var newManualDiscount = manualDiscount / numberOfSlots;

                formContext.getAttribute("manualdiscountamount").setValue(newManualDiscount);

            }

        },

            FromAndToDateOnChange: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var fromDate = formContext.getAttribute("ed_fromdate").getValue();
            var numberOfSlots = 0;

                var unit = formContext.getAttribute("uomid").getValue();

                function addDays(date, days) {
                    var result = new Date(date);
                    result.setDate(result.getDate() + parseInt(days));
                    return result;
                }


                if (unit[0].name == "1 vecka") {
                    if (fromDate != null) {

                        formContext.getAttribute("ed_todate").setValue(addDays(fromDate, 6));
                    }
                }

                var toDate = formContext.getAttribute("ed_todate").getValue();

            if (fromDate != null && toDate != null) {

                var DayValue = 1000 * 60 * 60 * 24;
                var numberOfSlots = ((toDate.getTime() - fromDate.getTime()) / DayValue) + 1;


                formContext.getAttribute("ed_totalslots").setValue(numberOfSlots);
            }
            else {
                formContext.getAttribute("ed_totalslots").setValue(numberOfSlots);
            }

        }
    };
}