
// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.FrontOffice) == "undefined") {
    Endeavor.Skanetrafiken.FrontOffice = {

        _urlRelativeFrontOffice: "https://sttravelconfiguratorweb.azurewebsites.net/ticketdetails/",
        _listOfFormsFrontOffice: ["Contact (3 - Skånetrafiken)", "Kund (3 - Skånetrafiken)", "RGOL", "SalesOrderLIne_Admin",
            "Information (Skånetrafiken)", "Köp och Sälj", "Information", "Köp och Sälj"],

        onFrontOfficeDisplay: function (formContext) {
            
            var formItem = formContext.ui.formSelector.getCurrentItem();

            if (formItem == null)
                return false;

            var formName = formItem.getLabel();

            for (var i = 0; i < Endeavor.Skanetrafiken.FrontOffice._listOfFormsFrontOffice.length; i++) {
                var valueList = Endeavor.Skanetrafiken.FrontOffice._listOfFormsFrontOffice[i];
                if (formName == valueList) {
                    return true;
                }
            }

            return false;
        },

        findValueFrontOffice: function (allSelectedRows, fieldName) {
            for (var i = 0; i < allSelectedRows.getAll().length; i++) {
                var selectedRow = allSelectedRows.getAll()[i];

                var selectedAttribute = selectedRow.getAttribute != null ? selectedRow.getAttribute() : selectedRow.getData().getEntity().getAttributes().getAll();

                for (var j = 0; j < selectedAttribute.length; j++) {

                    var selectAttribute = selectedAttribute[j];
                    if (selectAttribute.getName() == fieldName) {
                        var value = selectAttribute.getValue();
                        return Endeavor.Skanetrafiken.FrontOffice._urlRelativeFrontOffice + value;
                    }
                }
            }

            return null;
        },

        onFrontOfficeIntegrationContact: function (formContext) {

            //var fieldNameReskort = "ed_cardnumber";
            //var gridContextReskort = formContext.getControl("Relaterat_Reskort");
            //var allSelectedRowsReskort = gridContextReskort.getGrid().getSelectedRows();

            //var openUrlReskort = Endeavor.Skanetrafiken.FrontOffice.findValueFrontOffice(allSelectedRowsReskort, fieldNameReskort);
            //if (openUrlReskort != null)
            //    window.open(openUrlReskort, "_reskort");

            //var fieldNameValuecodes = "ed_mobilenumber";
            //var gridContextValuecodes = formContext.getControl("related_valuecodes");
            //var allSelectedRowsValuecodes = gridContextValuecodes.getGrid().getSelectedRows();

            //var openUrlValueCode = Endeavor.Skanetrafiken.FrontOffice.findValueFrontOffice(allSelectedRowsValuecodes, fieldNameValuecodes);
            //if (openUrlValueCode != null)
            //    window.open(openUrlValueCode, "_valuecode");

            var fieldNameSingapore = "st_ticketid";
            var gridContextSingapore = formContext.getControl("Singapore_Biljetter");
            var allSelectedRowsSingapore = gridContextSingapore.getGrid().getSelectedRows();

            var openUrlSingapore = Endeavor.Skanetrafiken.FrontOffice.findValueFrontOffice(allSelectedRowsSingapore, fieldNameSingapore);
            if (openUrlSingapore != null)
                window.open(openUrlSingapore, "_singapore");
        },

        onFrontOfficeIntegrationCase: function (formContext) {

            var ticketNumber1Control = formContext.getAttribute("cgi_ticketnumber1");

            if (ticketNumber1Control == null) {
                Endeavor.formscriptfunctions.AlertCustomDialog("The field 'cgi_ticketnumber1' is not on the form.");
                return;
            }

            var ticketNumber1 = ticketNumber1Control.getValue();

            if (ticketNumber1 == null || ticketNumber1 == "") {
                Endeavor.formscriptfunctions.AlertCustomDialog("The field 'cgi_ticketnumber1' is empty or null.");
                return;
            }

            var openUrlCase = Endeavor.Skanetrafiken.FrontOffice._urlRelativeFrontOffice + ticketNumber1;
            if (openUrlCase != null)
                window.open(openUrlCase, "_ticketNumber");
        },

        onFrontOfficeIntegrationSalesOrder: function (formContext) {

            
            var fieldNameTicket = "ed_ticketid";
            var gridContextTicketId = formContext.getControl("SalesOrderLines");
            var allSelectedRowsTicketId = gridContextTicketId.getGrid().getSelectedRows();

            var openUrlTicketId = Endeavor.Skanetrafiken.FrontOffice.findValueFrontOffice(allSelectedRowsTicketId, fieldNameTicket);
            if (openUrlTicketId != null)
                window.open(openUrlTicketId, "_salesorder");
        },

        onFrontOfficeIntegrationSalesOrderLine: function (formContext) {

            var ticketIdControl = formContext.getAttribute("ed_ticketid");

            if (ticketIdControl == null) {
                Endeavor.formscriptfunctions.AlertCustomDialog("The field 'ed_ticketid' is not on the form.");
                return;
            }

            var ticketId = ticketIdControl.getValue();

            if (ticketId == null || ticketId == "") {
                Endeavor.formscriptfunctions.AlertCustomDialog("The field 'ed_ticketid' is empty or null.");
                return;
            }

            var openUrlOrderLine = Endeavor.Skanetrafiken.FrontOffice._urlRelativeFrontOffice + ticketId;
            if (openUrlOrderLine != null)
                window.open(openUrlOrderLine, "_ticketId");
        }
    };
}