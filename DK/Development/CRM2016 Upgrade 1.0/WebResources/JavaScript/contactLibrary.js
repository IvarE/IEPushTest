if (typeof (CGISweden) == "undefined") { CGISweden = {}; }
if (typeof (CGISweden.contact) == "undefined") { CGISweden.contact = {}; }


// *******************************************************
// Entity: contact
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;

CGISweden.contact =
{

    onFormLoad: function () {
        try {


            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    break;
                case FORM_TYPE_UPDATE:
                    CGISweden.contact.checkIfUserHasSecRole();
                    CGISweden.contact.timerfunction_eHandel();
                    //CGISweden.contact.checkIfAdmForm();
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
            alert("Fel i CGISweden.contact.onFormLoad\n\n" + e.Message);
        }
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    onsave: function (context) {

        var eventArgs = context.getEventArgs();
        var _check_soc = CGISweden.contact.SocSecNoOnChange();

        if (_check_soc == false) {
            eventArgs.preventDefault();
        }

        //CGISweden.contact.checkIfAdmForm();
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    checkIfUserHasSecRole: function () {
        try {
            var currentUserRoles = Xrm.Page.context.getUserRoles();
            for (var i = 0; i < currentUserRoles.length; i++) {
                var userRoleId = currentUserRoles[i];
                CGISweden.odata.GetSecRolesName(userRoleId, CGISweden.contact.checkIfUserHasRole_callback, CGISweden.contact.checkIfUserHasRole_complete);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.contact.checkIfUserHasRole\n\n" + e.Message);
        }
    },

    //checkIfAdmForm: function () {
    //    var currForm = Xrm.Page.ui.formSelector.getCurrentItem();
    //    var currFormId = currForm.getId()

    //    if (currFormId == "aa39956c-0a06-4963-873a-2b3e574dbea5" || currFormId == "4b94250e-b88f-4439-9184-750d56a84fcf") { //"Tre Kolumner (Test)" or "Labbvy Admin"
    //        Xrm.Page.getAttribute("ed_isadmform").setValue(true);
    //    }
    //    else {
    //        Xrm.Page.getAttribute("ed_isadmform").setValue(false);
    //    }
    //},

    checkIfUserHasRole_complete: function () {
    },

    checkIfUserHasRole_callback: function (result) {
        try {
            if (result == null) {
                alert("Inga säkerhetsroller definierade!");
            }
            else {
                var _handlingOfficer = "Skånetrafiken Handläggare";
                var _handlingOfficerPlus = "Skånetrafiken Handläggare plus";

                var _roleName = result[0].Name;

                
                    try {
                        var emailField = Xrm.Page.getAttribute("emailaddress1").getValue();

                        var currForm = Xrm.Page.ui.formSelector.getCurrentItem();

                        var currFormId = currForm.getId()

                        if (currFormId !== "aa39956c-0a06-4963-873a-2b3e574dbea5") {
                            if (currFormId !== "4b94250e-b88f-4439-9184-750d56a84fcf") { //dont lock email field when using form "Tre Kolumner (Test)" or "Labbvy Admin"
                                if (emailField && emailField.Length !== 0) {
                                    if (_roleName.indexOf("Handläggare") > 0) {
                                        CGISweden.formscriptfunctions.SetState("emailaddress1", "true"); //The field should be editable until it has content
                                    }
                                }
                            }
                        }
                    }
                    catch (ex) {
                        if (emailField === undefined) {

                        } else {
                            alert("Fel i CGISweden.account.checkIfUserHasRole_callback\n\n" + e.Message);
                        }
                    }
                
            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.checkIfUserHasRole_callback\n\n" + e.Message);
        }
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    SocSecNoOnChange: function () {
        // Johan Andersson - Endeavor
        // Check Personnummer if swedish nationality
        var _return_save = true;
        var hasswedish = true;
        var obj = Xrm.Page.getAttribute("ed_hasswedishsocialsecuritynumber");
        if(obj != null){
            if(obj.getValue() == false)
                hasswedish = false;
        }

        // Perform only if swedish (default to swedish if missing)
        if (hasswedish == true) {
            var _soc = Xrm.Page.getAttribute("cgi_socialsecuritynumber");
            if (_soc != "" && _soc != null) {
                _soc = _soc.getValue();
                if (_soc == null)
                    _return_save = true;
                else {
                    var _sectrue = CGISweden.contact.validatePersonalNumber(_soc);
                    if (_sectrue == false) {
                        alert("Personnummer är inte giltigt. Privatkunden kan inte sparas.");
                        _return_save = false;
                    }
                    else {
                        var _soc_trim = _soc.replace('-', '');
                        var _soc_trim_length = _soc_trim.length;
                        if (_soc_trim_length != 12) {
                            alert("Personnummer har fel längd. Privatkunden kan inte sparas");
                            _return_save = false;
                        }
                        else {
                            if (_soc.length == 13) {
                                Xrm.Page.getAttribute("cgi_socialsecuritynumber").setValue(_soc_trim);
                            }
                        }
                    }
                }
            }
        }

        return (_return_save);
    },

    validatePersonalNumber: function (input) {
        // Check valid length & form
        if (!input) return false;
        if (input.indexOf('-') == -1) {
            if (input.length === 10) {
                input = input.slice(0, 6) + "-" + input.slice(6);
            } else {
                input = input.slice(0, 8) + "-" + input.slice(8);
            }
        }
        if (input.indexOf('-') == -1) {
            if (input.length === 10) {
                input = input.slice(0, 6) + "-" + input.slice(6);
            } else {
                input = input.slice(0, 8) + "-" + input.slice(8);
            }
        }
        if (!input.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})|(\d{4})(\d{2})(\d{2})\-(\d{4})$/)) return false;

        // Clean input
        input = input.replace('-', '');
        if (input.length == 12) {
            input = input.substring(2);
        }

        // Declare variables
        var d = new Date(((!!RegExp.$1) ? RegExp.$1 : RegExp.$5), (((!!RegExp.$2) ? RegExp.$2 : RegExp.$6) - 1), ((!!RegExp.$3) ? RegExp.$3 : RegExp.$7)),
                sum = 0,
                numdigits = input.length,
                parity = numdigits % 2,
                i,
                digit;

        // Check valid date
        if (Object.prototype.toString.call(d) !== "[object Date]" || isNaN(d.getTime())) return false;

        // Check luhn algorithm
        for (i = 0; i < numdigits; i = i + 1) {
            digit = parseInt(input.charAt(i))
            if (i % 2 == parity) digit *= 2;
            if (digit > 9) digit -= 9;
            sum += digit;
        }
        return (sum % 10) == 0;
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setContactLastname_Onload: function () {
        CGISweden.formscriptfunctions.SetValue("lastname", "");
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    timerfunction_eHandel: function () {
        try {
            var arg = 'WebResource_eHandelOrders';
            var obj = Xrm.Page.getControl(arg).getObject();
            var entid = Xrm.Page.data.entity.getId();

            try {
                obj.contentWindow.SetID(entid);
            }
            catch (e) {
                setTimeout(CGISweden.contact.timerfunction_eHandel, TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.contact.timerfunction_eHandel\n\n" + e.Message);
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
            var ZIPCodeNumberStr = Xrm.Page.getAttribute("address1_postalcode");

            // Verify that the field is valid
            if (typeof (ZIPCodeNumberStr) != "undefined" && ZIPCodeNumberStr != null) {

                if (ZIPCodeNumberStr.getValue() != null && ZIPCodeNumberStr.getValue().length > 3) {
                    var oldNumberStr = ZIPCodeNumberStr.getValue();
                    oldNumberStr = oldNumberStr.replace(/ /g, "");
                    //var newNumberStr = oldNumberStr.substring(0, 3) + " " + oldNumberStr.substring(3);
                    ZIPCodeNumberStr.setValue(oldNumberStr);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.account.format_ZIPCodeNumber\n\n" + e.Message);
        }

    },

    save_format_ZIPCode: function () {
        try {
            var ZIPCodeNumberStr = Xrm.Page.getAttribute("address1_postalcode");

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
            alert("Fel i CGISweden.account.format_ZIPCodeNumber\n\n" + e.Message);
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
};

// Functions calls
//
// CGISweden.contact.onFormLoad
// 
// 
// 
// 
// 
