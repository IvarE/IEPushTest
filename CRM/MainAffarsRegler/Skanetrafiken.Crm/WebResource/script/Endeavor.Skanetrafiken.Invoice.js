
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Invoice) == "undefined") {
    Endeavor.Skanetrafiken.Invoice = {

        infotainmentForm: ["ST Invoice"],

        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Invoice.setInfotainmentFilters(formContext);

        },

        setInfotainmentFilters: function (formContext) {
            var formItem = formContext.ui.formSelector.getCurrentItem();

            if (formItem == null)
                return;

            var formName = formItem.getLabel();


            var valueList = Endeavor.Skanetrafiken.Invoice.infotainmentForm[0];
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


        }

       

    };
}