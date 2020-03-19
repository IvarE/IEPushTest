if (typeof (CGISweden) == "undefined") { CGISweden = {}; }
if (typeof (CGISweden.address) == "undefined") { CGISweden.address = {}; }


// *******************************************************
// Entity: address
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;

CGISweden.address =
{

    onFormLoad: function () {
        try {
            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    break;
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                    break;
                case FORM_TYPE_QUICKCREATE:
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    alert("Form type error!");
                    break;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.address.onFormLoad\n\n" + e.Message);
        }
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

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    format_ZIPCode: function () {
        try {
            var ZIPCodeNumberStr = Xrm.Page.getAttribute("postalcode");

            // Verify that the field is valid
            if (typeof (ZIPCodeNumberStr) != "undefined" && ZIPCodeNumberStr != null) {

                if (ZIPCodeNumberStr.getValue() != null && ZIPCodeNumberStr.getValue().length > 3) {
                    var oldNumberStr = ZIPCodeNumberStr.getValue();
                    oldNumberStr = oldNumberStr.replace(/ /g, "");
                    var newNumberStr = oldNumberStr.substring(0, 3) + " " + oldNumberStr.substring(3);
                    ZIPCodeNumberStr.setValue(newNumberStr);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.address.format_ZIPCode\n\n" + e.Message);
        }

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    save_format_ZIPCode: function () {
        try {
            var ZIPCodeNumberStr = Xrm.Page.getAttribute("postalcode");

            // Verify that the field is valid
            if (typeof (ZIPCodeNumberStr) != "undefined" && ZIPCodeNumberStr != null) {

                if (ZIPCodeNumberStr.getValue() != null) {
                    var oldNumberStr = ZIPCodeNumberStr.getValue();
                    var newNumberStr = oldNumberStr.replace(/ /g, "");
                    ZIPCodeNumberStr.setValue(newNumberStr);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.address.format_ZIPCode\n\n" + e.Message);
        }

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    check_address_type: function () {
        if (Xrm.Page.getAttribute("addresstypecode").getValue() == null) {
            Xrm.Page.getAttribute("addresstypecode").setValue(1);
        }

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    removeOptionSetValue: function(context) {
        var pickListField = Xrm.Page.getAttribute("addresstypecode");
        var options = pickListField.getOptions();
        var OptionControl = Xrm.Page.getControl("addresstypecode");
        var optionvalue;

        for (var i = 0; i < options.length; i++) {

            optionvalue = options[i].value;

            if (optionvalue == 1 || optionvalue == 2) {

            }
            else {
                //alert('Value :' + options[i].value);
                //alert('Text :' + options[i].text);
                OptionControl.removeOption(options[i].value);
            }
        }


    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////

};
