if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }

// *******************************************************
// Entity: cgi_travelcard 
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

CGISweden.representativ =
{

    onFormLoad: function () {

        switch (Xrm.Page.ui.getFormType()) {
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

    firstName_OnChange: function () {
        try {
            var _name = CGISweden.representativ.getFirstAndLastName();
            CGISweden.formscriptfunctions.SetValue("cgi_name", _name);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.firstName_OnChange\n\n" + e.Message);
        }
    },

    lastName_OnChange: function () {
        try {
            var _name = CGISweden.representativ.getFirstAndLastName();
            CGISweden.formscriptfunctions.SetValue("cgi_name", _name);
        } catch (e) {
            alert("Fel i CGISweden.incident.lastName_OnChange\n\n" + e.Message);
        }
    },

    getFirstAndLastName: function () {
        var _name = "";
        try {
            var _firstName = CGISweden.formscriptfunctions.GetValue("cgi_firstname");
            var _lastName = CGISweden.formscriptfunctions.GetValue("cgi_lastname");

            if (_firstName != null)
                _name = _firstName;

            if (_lastName != null)
                _name = _name + " " + _lastName;

        } catch (e) {
            alert("Fel i CGISweden.incident.getFirstAndLastName\n\n" + e.Message);
        }
        return _name;
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    format_phonenumber: function (context) {
        try {
            var phoneNumberStr = context.getEventSource();
            var control = Xrm.Page.getControl(phoneNumberStr.getName());

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
            alert("Fel i CGISweden.account.format_phonenumber\n\n" + e.Message);
        }

    }

};

// Functions calls
//
// CGISweden.representativ.firstName_OnChange
// CGISweden.representativ.lastName_OnChange
// 
// 
// 
// 
