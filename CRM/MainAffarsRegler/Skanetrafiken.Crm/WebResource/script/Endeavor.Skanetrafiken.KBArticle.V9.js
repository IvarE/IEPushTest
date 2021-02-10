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

if (typeof (Endeavor.Skanetrafiken.KBArticle) === "undefined") {

    //Form Methods CGI Article (from articleLibrary.js)
    Endeavor.Skanetrafiken.KBArticle = {
        onFormLoad: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();
                Endeavor.Skanetrafiken.KBArticle.setPublishAllowed(0, formContext);

                switch (formContext.ui.getFormType()) {
                    case FORM_TYPE_CREATE:
                        Endeavor.Skanetrafiken.KBArticle.checkIfUserHasMarketingRole(formContext);
                        break;
                    case FORM_TYPE_UPDATE:
                        Endeavor.Skanetrafiken.KBArticle.checkIfUserHasMarketingRole(formContext);
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
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.onFormLoad\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        checkIfUserHasMarketingRole: function (formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var currentUserRoles = globalContext.userSettings.securityRoles;
                for (var i = 0; i < currentUserRoles.length; i++) {
                    var _roleId = currentUserRoles[i];
                    Endeavor.OData_Querys.GetSecRolesNameArticle(_roleId, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.checkIfUserHasMarketingRole\n\n" + e.message);
            }
        },

        checkIfUserHasMarketingRole_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Inga säkerhetsroller definierade!");
                }
                else {
                    var _marketingRoleName = "Skånetrafiken Marketing Professional";
                    var _roleName = result.entities[0].name;

                    //if has role eq _marketingRoleName, user can change field cgi_auth_approved
                    if (_roleName == _marketingRoleName)
                        Endeavor.Skanetrafiken.KBArticle.setPublishAllowed(1, formContext);

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.checkIfUserHasMarketingRole_callback\n\n" + e.message);
            }
        },

        setPublishAllowed: function (publishallowed, formContext) {
            try {
                var _setstate = true;

                if (publishallowed == 1)
                    _setstate = false;

                Endeavor.formscriptfunctions.SetState("cgi_appoval", _setstate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.setPublishAllowed\n\n" + e.message);
            }
        },

        cgi_publishonweb_OnChange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _cgi_publishonweb = Endeavor.formscriptfunctions.GetValue("cgi_publishonweb", formContext);

                if (_cgi_publishonweb == true) {
                    Endeavor.formscriptfunctions.SetValue("cgi_appoval", 285050001, formContext);
                    Endeavor.formscriptfunctions.SetSubmitModeAlways("cgi_appoval", formContext);
                }

                if (_cgi_publishonweb == false) {
                    Endeavor.formscriptfunctions.SetValue("cgi_appoval", 285050002, formContext);
                    Endeavor.formscriptfunctions.SetSubmitModeAlways("cgi_appoval", formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.KBArticle.cgi_publishonweb_OnChange\n\n" + e.message);
            }
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}