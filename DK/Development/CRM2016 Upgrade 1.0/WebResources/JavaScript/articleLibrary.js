if (typeof (CGISweden) == "undefined") { CGISweden = {}; }
if (typeof (CGISweden.article) == "undefined") { CGISweden.article = {}; }


// *******************************************************
// Entity: article
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

CGISweden.article =
{

    onFormLoad: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            CGISweden.article.setPublishAllowed(0);

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    CGISweden.article.checkIfUserHasMarketingRole(formContext);
                    break;
                case FORM_TYPE_UPDATE:
                    CGISweden.article.checkIfUserHasMarketingRole(formContext);
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
            alert("Fel i CGISweden.article.onFormLoad\n\n" + e.Message);
        }
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    checkIfUserHasMarketingRole: function (formContext) {
        try {
            var globalContext = Xrm.Utility.getGlobalContext();

            var currentUserRoles = globalContext.userSettings.securityRoles();
            for (var i = 0; i < currentUserRoles.length; i++) {
                var _roleId = currentUserRoles[i];
                CGISweden.odata.GetSecRolesNameArticle(_roleId, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.article.checkIfUserHasMarketingRole\n\n" + e.Message);
        }
    },

    checkIfUserHasMarketingRole_callback: function (result, formContext) {
        try {
            if (result == null) {
                alert("Inga säkerhetsroller definierade!");
            }
            else {
                var _marketingRoleName = "Skånetrafiken Marketing Professional";
                var _roleName = result[0].Name;

                //if has role eq _marketingRoleName, user can change field cgi_auth_approved
                if (_roleName == _marketingRoleName) {
                    CGISweden.article.setPublishAllowed(1, formContext);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.article.checkIfUserHasMarketingRole_callback\n\n" + e.Message);
        }
    },

    setPublishAllowed: function (publishallowed, formContext) {
        try {
            var _setstate = true;

            if (publishallowed == 1)
                _setstate = false;

            CGISweden.formscriptfunctions.SetState("cgi_appoval", _setstate, formContext);

        }
        catch (e) {
            alert("Fel i CGISweden.article.setPublishAllowed\n\n" + e.Message);
        }
    },

    cgi_publishonweb_OnChange: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var _cgi_publishonweb = CGISweden.formscriptfunctions.GetValue("cgi_publishonweb", formContext);

            if (_cgi_publishonweb == true) {
                CGISweden.formscriptfunctions.SetValue("cgi_appoval", 285050001, formContext);
                CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_appoval", formContext);
            }

            if (_cgi_publishonweb == false) {
                CGISweden.formscriptfunctions.SetValue("cgi_appoval", 285050002, formContext);
                CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_appoval", formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.article.cgi_publishonweb_OnChange\n\n" + e.Message);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
};

// Functions calls
//
// CGISweden.article.onFormLoad
// CGISweden.article.cgi_publishonweb_OnChange
// 
// 
// 
// 
