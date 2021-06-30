// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.CompanyRole) == "undefined") {
    Endeavor.Skanetrafiken.CompanyRole = {

        onBlockCompanyRoleShow: function (formContext) {
            debugger;

            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            if (lockedPortal) 
                return false;
            else 
                return true;
        },

        onUnblockCompanyRoleShow: function (formContext) {
            debugger;

            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            if (lockedPortal)
                return true;
            else
                return false;
        },

        blockCompanyRolePortal: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            try {
                Endeavor.Skanetrafiken.CompanyRole.blockCompanyRolePortalSuccessCallback(formContext);
            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
            }
        },

        blockCompanyRolePortalSuccessCallback: function (formContext) {
            var SSN = "";
            var PortalID = "";
            var blocked = "";

            // SSN
            if (formContext.getAttribute("ed_socialsecuritynumber"))
                SSN = formContext.getAttribute("ed_socialsecuritynumber").getValue();

            // PortalID
            var account = formContext.getAttribute("ed_account");
            if (account && account.getValue() && account.getValue()[0] && account.getValue()[0].id) {

                var guid = account.getValue()[0].id;
                guid = guid.substring(1, guid.length - 1);

                Xrm.WebApi.retrieveRecord("account", guid, "?$select=accountnumber").then(
                    function success(result) {

                        PortalID = result.accountnumber;

                        //blocked
                        if (formContext.getAttribute("ed_islockedportal"))
                            blocked = formContext.getAttribute("ed_islockedportal").getValue();

                        var inputParameters = [{ "Field": "SSN", "Value": SSN, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                            { "Field": "PortalID", "Value": PortalID, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                            { "Field": "Blocked", "Value": !blocked, "TypeName": Endeavor.formscriptfunctions.getParameterType("bool"), "StructuralProperty": 1 }];

                        try {

                            Endeavor.formscriptfunctions.callGlobalAction("ed_BlockCompanyRolePortal", inputParameters,
                                function (result) {
                                    if (blocked)
                                        Endeavor.formscriptfunctions.AlertCustomDialog("Företagsroll avblockerad!");
                                    else
                                        Endeavor.formscriptfunctions.AlertCustomDialog("Företagsroll spärrad.");
                                },
                                function (error) {
                                    if (blocked)
                                        Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte avblockera företagsroll. Var god försök igen senare.");
                                    else
                                        Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte spärra företagsroll. Var god försök igen senare.");

                                    console.log(error.message);
                                    Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                                });
                        }
                        catch (e) {
                            console.log(e.message);
                            Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
                        }
                    },
                    function (error) {
                        console.log(error.message);
                        Endeavor.formscriptfunctions.AlertCustomDialog(error.message);
                    }
                );               
            }
        }
    };
}