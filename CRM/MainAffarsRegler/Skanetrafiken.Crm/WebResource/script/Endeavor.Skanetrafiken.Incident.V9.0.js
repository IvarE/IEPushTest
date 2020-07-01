FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;


// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Incident) == "undefined") {
    Endeavor.Skanetrafiken.Incident = {

        alertCustomDialog: function (msgText) {

            var message = { confirmButtonLabel: "Ok", text: msgText };
            var alertOptions = { height: 150, width: 280 };

            Xrm.Navigation.openAlertDialog(message, alertOptions).then(
                function success(result) {
                    console.log("Alert dialog closed");
                },
                function (error) {
                    console.log(error.message);
                }
            );
        },

        onLoad: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            var contactAttribute = formContext.getAttribute("cgi_contactid")
            var customerEmailAttribute = formContext.getAttribute("cgi_customer_email");

            if (contactAttribute && customerEmailAttribute) {
                if (customerEmailAttribute.getValue && !customerEmailAttribute.getValue()) {
                    if (contactAttribute.getValue && contactAttribute.getValue() && contactAttribute.getValue().length > 0) {

                        var columnSet = "emailaddress1,emailaddress2";
                        Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=" + columnSet + "&$filter=contactid eq " + contactAttribute.getValue()[0].id + ")").then(
                            function success(contactResult) {

                                if (contactResult && contactResult.entities.length > 0) {
                                    if (contactResult.entities[0].emailaddress1)
                                        customerEmailAttribute.setValue(contactResult.entities[0].emailaddress1);
                                    else if (contactResult.entities[0].emailaddress2)
                                        customerEmailAttribute.setValue(contactResult.entities[0].emailaddress2);
                                }

                            },
                            function (error) {
                                console.log(error.message);
                                Endeavor.Skanetrafiken.Incident.alertCustomDialog(error.message);
                            }
                        );                        
                    }
                }
            }
        },
    };
}