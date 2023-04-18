if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.ContactAnnons) == "undefined") {
    Endeavor.Skanetrafiken.ContactAnnons = {


        onLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.ContactAnnons.setInfotainmentValues(executionContext);

        },

        setInfotainmentValues: function (executionContext) {

            var formContext = executionContext.getFormContext();
            var roles = [];
            roles[0] = "Skånetrafiken Annons";

            var isUserCheck = Endeavor.Skanetrafiken.ContactAnnons.currentUserHasSecurityRole(roles);

            if (isUserCheck) {

                Endeavor.formscriptfunctions.SetValue("ed_informationsource", 899310000, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_infotainmentcontact", true, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_privatecustomercontact", false, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_agentcontact", false, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_kontaktperson", false, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_epostmottagare", false, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_collaborationcontact", false, formContext);
                Endeavor.formscriptfunctions.SetValue("ed_businesscontact", false, formContext);


                //formContext.getControl("ed_privatecustomercontact").setDisabled(true);
                // ed_informationsource = Annons
                //ed_infotainmentcontact = Yes
            }

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
        }


    }
}