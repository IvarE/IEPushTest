// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ProductPriceListItem) == "undefined") {
    Endeavor.Skanetrafiken.ProductPriceListItem = {

        onLoad: function (executionContext) {
         
            Endeavor.Skanetrafiken.ProductPriceListItem.setPriceList(executionContext);

        },

        setPriceList: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var product = formContext.getAttribute("productid").getValue();


            var productId = product[0].id.replace("{", "").replace("}", "");
            debugger;

            Xrm.WebApi.retrieveMultipleRecords("product", "?$filter=productid eq " + productId).then(
                function success(results) {

                    if (results == null || results.entities.length == 0) {
                        console.log("inget resultat")

                    } else {
                        if (results.entities[0]._pricelevelid_value == "6ba54d56-b6e9-e911-80f0-005056b61fff") {// infotainment
                            var object = new Array();
                            object[0] = new Object();
                            object[0].id = "{c103aeb7-3994-ea11-80f8-005056b64d75}";
                            object[0].name = "Infotainment";
                            object[0].entityType = "pricelevel";
                            formContext.getAttribute("pricelevelid").setValue(object);
                        }
                    }
                },
                function (error) {
                    console.log(error.message);
                    Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                }
            );

        }

        
    };
}