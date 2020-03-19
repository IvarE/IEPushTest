if (typeof (CGISweden) == "undefined") { CGISweden = {}; }
if (typeof (CGISweden.account) == "undefined") { CGISweden.account = {}; }


// *******************************************************
// Entity: account
// *******************************************************


FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;


///////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////
CGISweden.account =
{

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    onFormLoad: function () {
        try {


            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    break;
                case FORM_TYPE_UPDATE:
                    CGISweden.account.checkIfUserHasSecRole();
                    CGISweden.account.timerfunction_eHandel();
                    break;
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                case FORM_TYPE_QUICKCREATE:
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    alert("Form type error!");
                    break;
            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.onFormLoad\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    checkIfUserHasSecRole: function () {
        try {
            var currentUserRoles = Xrm.Page.context.getUserRoles();
            for (var i = 0; i < currentUserRoles.length; i++) {
                var userRoleId = currentUserRoles[i];
                CGISweden.odata.GetSecRolesName(userRoleId, CGISweden.account.checkIfUserHasRole_callback, CGISweden.account.checkIfUserHasRole_complete);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.checkIfUserHasRole\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    checkIfUserHasRole_complete: function () {
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    checkIfUserHasRole_callback: function (result) {
        try {
            if (result == null) {
                alert("Inga säkerhetsroller definierade!");
            }
            else {

                var _roleName = result[0].Name;

                var emailField = Xrm.Page.getAttribute("emailaddress1").getValue();

                if (emailField && emailField.Length !== 0) {
                    if (_roleName.indexOf("Handläggare") > 0) {
                        CGISweden.formscriptfunctions.SetState("emailaddress1", "true"); //The field should only be editable until it has content
                    }
                }

            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.checkIfUserHasRole_callback\n\n" + e.Message);
        }
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    timerfunction_eHandel: function () {
        try {
            var arg = 'WebResource_eHandelOrders';
            var obj = Xrm.Page.getControl(arg).getObject();
            var entid = Xrm.Page.data.entity.getId();

            try {
                obj.contentWindow.SetID(entid);
            }
            catch (e) {
                setTimeout(CGISweden.account.timerfunction_eHandel, TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.timerfunction_eHandel\n\n" + e.Message);
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
                    if(newNumberStr.indexOf("+") > -1)
                    {
                        control.setNotification("Ange telefonnummer utan landsprefix.");
                    }
                    else
                    {
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
// CGISweden.account.onFormLoad
// 
// 
// 
// 
// 

