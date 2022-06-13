
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Order) == "undefined") {
    Endeavor.Skanetrafiken.Order = {

        infotainmentForm: ["Annons Order"],

        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Order.setInfotainmentFilters(formContext);

        },

        setInfotainmentFilters: function (formContext) {
            var formItem = formContext.ui.formSelector.getCurrentItem();

            if (formItem == null)
                return;

            var formName = formItem.getLabel();


            var valueList = Endeavor.Skanetrafiken.Order.infotainmentForm[0];
            if (formName == valueList) {

                var company = formContext.getAttribute("customerid");
                //var companyGuid = company[0].id;
                if (company == null) {
                    return;
                }

                company = formContext.getAttribute("customerid").getValue();
                //formContext.getControl("ed_contact").addPreSearch(Endeavor.Skanetrafiken.Quote.addActionTypeLookupFilter(formContext, company));


                formContext.getControl("ed_contact").addPreSearch(function () {

                    var fetchXml = "<filter type='and'><condition attribute='ed_infotainmentcontact' operator='eq' value='1' /><condition attribute='parentcustomerid' operator='eq' uitype='account' value='" + company[0].id + "' /></filter>";

                    formContext.getControl("ed_contact").addCustomFilter(fetchXml);

                });

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