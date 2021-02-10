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

CGISweden.travelcard =
{

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

            var _cardNumber = CGISweden.formscriptfunctions.GetValue("cgi_travelcardnumber", formContext);
            if (_cardNumber != null) {
                return _cardNumber;
            }
            else {
                return null;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setVisibilityOnLoad\n\n" + e.Message);
        }
    }

};

// Functions calls
//
// 
// 
// 
// 
// 
// 
