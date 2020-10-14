if (typeof (Endeavor) === "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) === "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.cgi_representative) === "undefined") {

    FORM_TYPE_CREATE = 1;
    FORM_TYPE_UPDATE = 2;
    FORM_TYPE_READONLY = 3;
    FORM_TYPE_DISABLED = 4;
    FORM_TYPE_QUICKCREATE = 5;
    FORM_TYPE_BULKEDIT = 6;

    Endeavor.Skanetrafiken.cgi_representative = {

        //Form Methods CGI Representative (from representativLibrary.js)
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

        firstName_OnChange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _name = Endeavor.Skanetrafiken.cgi_representative.getFirstAndLastName(formContext);
                Endeavor.formscriptfunctions.SetValue("cgi_name", _name, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.firstName_OnChange\n\n" + e.message);
            }
        },

        lastName_OnChange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _name = Endeavor.Skanetrafiken.cgi_representative.getFirstAndLastName(formContext);
                Endeavor.formscriptfunctions.SetValue("cgi_name", _name, formContext);
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.lastName_OnChange\n\n" + e.message);
            }
        },

        getFirstAndLastName: function (formContext) {
            var _name = "";
            try {
                var _firstName = Endeavor.formscriptfunctions.GetValue("cgi_firstname", formContext);
                var _lastName = Endeavor.formscriptfunctions.GetValue("cgi_lastname", formContext);

                if (_firstName != null)
                    _name = _firstName;

                if (_lastName != null)
                    _name = _name + " " + _lastName;

            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.getFirstAndLastName\n\n" + e.message);
            }
            return _name;
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        format_phonenumber: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var phoneNumberStr = formContext.getEventSource();
                var control = formContext.getControl(phoneNumberStr.getName());

                // Verify that the field is valid
                if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null) {

                    if (phoneNumberStr.getValue() != null) {

                        // replace any "-" with a blank space
                        var oldNumberStr = phoneNumberStr.getValue();
                        var newNumberStr = oldNumberStr.replace(/-/g, "");
                        newNumberStr = newNumberStr.replace(/ /g, "");
                        phoneNumberStr.setValue(newNumberStr);
                        if (newNumberStr.indexOf("+") > -1) {
                            control.setNotification("Ange telefonnummer utan landsprefix.");
                        }
                        else {
                            control.clearNotification();
                        }
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.format_phonenumber\n\n" + e.message);
            }
        }
    }
}