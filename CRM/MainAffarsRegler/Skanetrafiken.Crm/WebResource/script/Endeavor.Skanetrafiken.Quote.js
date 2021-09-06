
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Quote) == "undefined") {
    Endeavor.Skanetrafiken.Quote = {

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
                function () { alert(errorCode + message) },)
            //formContext.data.refresh();

        }

        //TotalAmountOnChange: function (executionContext) {

        //    var formContext = executionContext.getFormContext();

        //    var quoteAttribute = formContext.getAttribute("quoteid");

        //    var quoteId = quoteAttribute.getValue()[0].id.replace("{", "").replace("}", "");
        //    var columnSet = "extendedamount";
        //    Xrm.WebApi.retrieveMultipleRecords("quotedetail", "?$select=" + columnSet + "&$filter=quoteId eq " + quoteId).then(
        //        function success(result) {

        //            if (result && result.entities.length > 0) {

        //                result.forEach()
        //            }

        //        },
        //        function (error) {
        //            console.log(error.message);
        //            Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
        //        }
        //    );
        //}
    };
}