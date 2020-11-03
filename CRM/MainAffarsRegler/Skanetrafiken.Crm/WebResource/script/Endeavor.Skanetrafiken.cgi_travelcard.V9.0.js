FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) === "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.cgi_travelcard) === "undefined") {

    Endeavor.Skanetrafiken.cgi_travelcard  = {

        //Form Methods CGI Travelcard (from travelCardLibrary.js)
        onFormLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                case FORM_TYPE_UPDATE:
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                case FORM_TYPE_QUICKCREATE:
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    alert("Form type error!");
                    break;
            }

        },

        getTravelCardNumber: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _cardNumber = Endeavor.formscriptfunctions.GetValue("cgi_travelcardnumber", formContext);
                if (_cardNumber != null)
                    return _cardNumber;
                else
                    return null;
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setVisibilityOnLoad\n\n" + e.message);
            }
        }
    }
}