// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ValueCodeTransaction) == "undefined") {
    Endeavor.Skanetrafiken.ValueCodeTransaction = {

        onLoad: function () {

        },

        showAdminButtons: function () {

            debugger;
            var roles = [];
            roles[0] = "System Administrator";
            var isUserCheck = Endeavor.Skanetrafiken.ValueCodeTransaction.currentUserHasSecurityRole(roles);

            return isUserCheck;
        },

        currentUserHasSecurityRole: function (roles) {

            var fetchXml =
                "<fetch mapping='logical'>" +
                "<entity name='systemuser'>" +
                "<attribute name='systemuserid' />" +
                "<filter type='and'>" +
                "<condition attribute='systemuserid' operator='eq-userid' />" +
                "</filter>" +
                "<link-entity name='systemuserroles' from='systemuserid' to='systemuserid' visible='false' intersect='true'>" +
                "<link-entity name='role' from='roleid' to='roleid' alias='r'>" +
                "<filter type='or'>";

            for (var i = 0; i < roles.length; i++) {
                fetchXml += "<condition attribute='name' operator='eq' value='" + roles[i] + "' />";
            }

            fetchXml += "            </filter>" +
                "</link-entity>" +
                "</link-entity>" +
                "</entity>" +
                "</fetch>";
            var modifiedFetchXml = fetchXml.replace("&", "&amp;");
            var users = Endeavor.formscriptfunctions.executeFetchGetCount(modifiedFetchXml, "systemusers");
            if (users > 0)
                return true;
            else
                return false;
        },
    }
}