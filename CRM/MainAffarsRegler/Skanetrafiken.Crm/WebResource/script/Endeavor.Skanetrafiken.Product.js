// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Product) == "undefined") {
    Endeavor.Skanetrafiken.Product = {

        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var productId = formContext.getAttribute("productnumber");

            if (productId.getValue() == null) {
                formContext.getAttribute("productnumber").setValue("AUTOGENERERAS NÄR PRODUKTEN SPARAS");
            }

        },

        onSave: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var productId = formContext.getAttribute("productnumber");

            if (productId.getValue() == null) {
                formContext.getAttribute("productnumber").setValue("AUTOGENERERAS NÄR PRODUKTEN SPARAS");
            }

        },

        nameOnChange: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var productId = formContext.getAttribute("productnumber");

            if(productId.getValue() == null) {
                formContext.getAttribute("productnumber").setValue("AUTOGENERERAS NÄR PRODUKTEN SPARAS");
            }

        }

    };
}