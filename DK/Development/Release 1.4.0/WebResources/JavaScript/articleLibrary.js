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

    onFormLoad: function () {
        try {

            CGISweden.article.setPublishAllowed(0);

            switch (Xrm.Page.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    CGISweden.article.checkIfUserHasMarketingRole();
                    break;
                case FORM_TYPE_UPDATE:
                    CGISweden.article.checkIfUserHasMarketingRole();
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

    checkIfUserHasMarketingRole: function () {
        try {
            var currentUserRoles = Xrm.Page.context.getUserRoles();
            for (var i = 0; i < currentUserRoles.length; i++) {
                var _roleId = currentUserRoles[i];
                CGISweden.odata.GetSecRolesNameArticle(_roleId, CGISweden.article.checkIfUserHasMarketingRole_callback, CGISweden.article.checkIfUserHasMarketingRole_complete);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.article.checkIfUserHasMarketingRole\n\n" + e.Message);
        }
    },

    checkIfUserHasMarketingRole_complete: function () {
    },

    checkIfUserHasMarketingRole_callback: function (result) {
        try {
            if (result == null) {
                alert("Inga säkerhetsroller definierade!");
            }
            else {
                var _marketingRoleName = "Skånetrafiken Marketing Professional";
                var _roleName = result[0].Name;

                //if has role eq _marketingRoleName, user can change field cgi_auth_approved
                if (_roleName == _marketingRoleName) {
                    CGISweden.article.setPublishAllowed(1);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.article.checkIfUserHasMarketingRole_callback\n\n" + e.Message);
        }
    },

    setPublishAllowed: function (publishallowed) {
        try {
            var _setstate = true;

            if (publishallowed == 1)
                _setstate = false;

            CGISweden.formscriptfunctions.SetState("cgi_appoval", _setstate);

        }
        catch (e) {
            alert("Fel i CGISweden.article.setPublishAllowed\n\n" + e.Message);
        }
    },

    cgi_publishonweb_OnChange: function () {
        try {
            var _cgi_publishonweb = CGISweden.formscriptfunctions.GetValue("cgi_publishonweb");

            if (_cgi_publishonweb == true) {
                CGISweden.formscriptfunctions.SetValue("cgi_appoval", 285050001);
                CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_appoval");
            }

            if (_cgi_publishonweb == false) {
                CGISweden.formscriptfunctions.SetValue("cgi_appoval", 285050002);
                CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_appoval");
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
