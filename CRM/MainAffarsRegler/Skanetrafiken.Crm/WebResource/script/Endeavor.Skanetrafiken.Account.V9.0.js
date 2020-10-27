FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 500;

// Begin scoping
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Account) == "undefined") {
    Endeavor.Skanetrafiken.Account = {

        onLoad: function (executionContext) {

            var formContext = executionContext.getFormContext();

            //Check if this is a Quick Create Form
            var stateCode = formContext.getAttribute("statecode");
            if (stateCode == null) {
                var businessCustomer = formContext.getAttribute("ed_businesscustomer");
                var agent = formContext.getAttribute("ed_agent");
                var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

                if (businessCustomer != null && agent != null && infotainment != null) {
                    businessCustomer.setRequiredLevel("required");
                    agent.setRequiredLevel("required");
                    infotainment.setRequiredLevel("required");
                }
            }
            else 
                Endeavor.Skanetrafiken.Account.showInfoAccountPortal(formContext);

            //Hide/Show Õvrig Information
            Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
        },

        resetRequiredLevel: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var stateCode = formContext.getAttribute("statecode");
            if (stateCode == null) {
                var businessCustomer = formContext.getAttribute("ed_businesscustomer");
                var agent = formContext.getAttribute("ed_agent");
                var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

                if (businessCustomer != null && agent != null && infotainment != null) {

                    var businessValue = businessCustomer.getValue();
                    var agentValue = agent.getValue();
                    var infotainmentValue = infotainment.getValue();

                    if (businessValue != false || agentValue != false || infotainmentValue != false) {
                        businessCustomer.setRequiredLevel("none");
                        agent.setRequiredLevel("none");
                        infotainment.setRequiredLevel("none");
                    }
                    else if (businessValue == false && agentValue == false && infotainmentValue == false) {
                        businessCustomer.setRequiredLevel("required");
                        agent.setRequiredLevel("required");
                        infotainment.setRequiredLevel("required");
                    }
                }
            }
        },

        onChangeTypeOfAccount: function (executionContext) {

            var formContext = executionContext.getFormContext();

            //Hide/Show Õvrig Information
            Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Block button
        onBlockAccountShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == false && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Unblock button
        onUnblockAccountShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            } else 
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == true && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Block button
        onBlockCostSiteShow: function (formContext) {

            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == false && showButton != true)
                return true;
            // If Account = Cost Site (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Unblock button
        onUnblockCostSiteShow: function (formContext) {
            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount");
            if (accountType != null) {
                //Get OptionSet Val
                var optionSetValue = accountType.getValue();

                // if Account Portal (Ny företagskund)
                if (optionSetValue == null || optionSetValue == 899310001)
                    return false;
            }
            else
                return false;

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = false;

            showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == true && showButton != true)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        showOvrigInformation: function (formContext) {

            var infotaimentCustomer = formContext.getAttribute("ed_infotainmentcustomer");
            var customer = formContext.getAttribute("ed_customer");
            var agent = formContext.getAttribute("ed_agent");
            var samarbete = formContext.getAttribute("ed_collaborationcustomer");
            var reseller = formContext.getAttribute("ed_reseller");
            var school = formContext.getAttribute("ed_schoolcustomer");
            var senior = formContext.getAttribute("ed_seniorcustomer");
            var portal = formContext.getAttribute("ed_portalcustomer");

            if (infotaimentCustomer == null || customer == null || agent == null || samarbete == null || reseller == null || school == null || senior == null || portal == null)
                return;

            if (infotaimentCustomer.getValue() && !customer.getValue() && !agent.getValue() && !samarbete.getValue() &&
                !reseller.getValue() && !school.getValue() && !senior.getValue() && !portal.getValue()) {

                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(false);
                formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(false);
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(false);
                formContext.ui.tabs.get("Cost Sites").setVisible(false);

            }
            else {
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(true);
                formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(true);
                formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(true);
                formContext.ui.tabs.get("Cost Sites").setVisible(true);
            }
        },

        showInfoAccountPortal: function (formContext) {

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            if (parentAccount === null) {
                if (formContext.ui.tabs.get("Cost Sites") !== null && formContext.ui.tabs.get("Cost Sites") !== 'undefined')
                    formContext.ui.tabs.get("Cost Sites").setVisible(true);

                if (formContext.ui.tabs.get("Sales History") !== null && formContext.ui.tabs.get("Sales History") !== 'undefined')
                    formContext.ui.tabs.get("Sales History").setVisible(false);
            }
            else {
                if (formContext.ui.tabs.get("Cost Sites") !== null && formContext.ui.tabs.get("Cost Sites") !== 'undefined')
                    formContext.ui.tabs.get("Cost Sites").setVisible(false);

                if (formContext.ui.tabs.get("Sales History") !== null && formContext.ui.tabs.get("Sales History") !== 'undefined')
                    formContext.ui.tabs.get("Sales History").setVisible(true);
            }

            var isLocked = formContext.getAttribute("ed_islockedportal");
            if (isLocked != null) {
                var isLockedBool = isLocked.getValue();
                if (isLockedBool == true) 
                    formContext.ui.setFormNotification("Detta företag är låst från företagssidan", "WARNING", "2");
            }

            var AllowCreate = formContext.getAttribute("ed_allowcreate");
            if (AllowCreate != null) {
                var allowCreateBool = AllowCreate.getValue();
                if (allowCreateBool == false)
                    formContext.ui.setFormNotification("OBS - Detta företag kan inte skapa upp nya administratörer via företagssidan", "INFO", "1");
            }
        },

        blockAccountPortal: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var blocked = formContext.getAttribute("ed_islockedportal").getValue();
            var msgText = "";

            if (parentAccount == null && blocked === false) {
                msgText = "Är du säker på att du vill spärra hela organisationen från åtkomst till hemsidan?";
            }
            else if (parentAccount == null && blocked === true) {
                msgText = "Är du säker på att du vill avblockera hela organisationen och ge åtkomst till hemsidan?";
            } else if (parentAccount != null && blocked === false) {
                msgText = "Är du säker på att du vill spärra detta kostnadsställe från åtkomst till hemsidan?";
            }
            else {
                msgText = "Är du säker på att du vill avblockera detta kostnadsställe och ge åtkomst till hemsidan?";
            }

            Endeavor.formscriptfunctions.ConfirmCustomDialog(msgText,

                function () {
                    try {
                        Endeavor.Skanetrafiken.Account.blockAccountPortalSuccessCallback(formContext);
                    } catch (e) {
                        Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
                    }
                });
        },

        blockAccountPortalSuccessCallback: function (formContext) {

            var portalID = formContext.getAttribute("accountnumber").getValue();
            var blocked = formContext.getAttribute("ed_islockedportal").getValue();
            var parentID = "";
            var organizationNumber = "";

            if (formContext.getAttribute("parentaccountid").getValue()) {

                var inputParameters = [{ "Field": "PortalID", "Value": portalID, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "ParentID", "Value": parentID, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "OrganizationNumber", "Value": organizationNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "Blocked", "Value": !blocked, "TypeName": Endeavor.formscriptfunctions.getParameterType("bool"), "StructuralProperty": 1 }];

                Endeavor.formscriptfunctions.callGlobalAction("ed_BlockAccountPortal", inputParameters,
                    function (result) {
                        if (blocked)
                            Endeavor.formscriptfunctions.AlertCustomDialog("Företag avblockerat!");
                        else
                            Endeavor.formscriptfunctions.AlertCustomDialog("Företag spärrat!");
                    },
                    function (error) {
                        if (blocked)
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte avblockera företag. Var god försök igen senare.");
                        else
                            Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte spärra företag. Var god försök igen senare.");
                    });
            }
            else {
                var ID = formContext.data.entity.getId();
                ID = ID.substring(1, ID.length - 1);

                Xrm.WebApi.retrieveMultipleRecords("account", "?$select=accountnumber&$filter=parentaccountid eq " + ID).then(
                    function success(results) {

                        for (var i = 0; i < results.entities.length; i++) {

                            var account = results.entities[i];
                            if (account.AccountNumber) {

                                try {

                                    var inputParameters = [{ "Field": "PortalID", "Value": account.AccountNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "ParentID", "Value": parentID, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "OrganizationNumber", "Value": organizationNumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                                    { "Field": "Blocked", "Value": !blocked, "TypeName": Endeavor.formscriptfunctions.getParameterType("bool"), "StructuralProperty": 1 }];

                                    Endeavor.formscriptfunctions.callGlobalAction("ed_BlockAccountPortal", inputParameters,
                                        function (result) {

                                            if (blocked)
                                                Endeavor.formscriptfunctions.AlertCustomDialog("Företag avblockerat.");
                                            else
                                                Endeavor.formscriptfunctions.AlertCustomDialog("Företag spärrat.");
                                        },
                                        function (error) {
                                            if (blocked)
                                                Endeavor.formscriptfunctions.AlertCustomDialog("Avblockering av kostnadsställe(n) misslyckades" + account.AccountNumber);
                                            else
                                                Endeavor.formscriptfunctions.AlertCustomDialog("Spärr av kostnadsställe(n) misslyckades" + account.AccountNumber);
                                        });
                                }
                                catch (e) {
                                    console.log(e.message);
                                    Endeavor.formscriptfunctions.ErrorCustomDialog(e.message, "Retrieve Multiple Records Error");
                                }
                            }
                        }
                    },
                    function (error) {
                        console.log(error.message);
                        Endeavor.formscriptfunctions.ErrorCustomDialog(error.message, "Retrieve Multiple Records Error");
                    }
                );
            }
        },

        //Senare fix med säkerhetsroller
        showBlockButton: function () {

            var roles = [];
            roles[0] = "Block Portal Account"; //Borde heta "access block button" eller liknande
            var isUserCheck = Endeavor.Skanetrafiken.Account.currentUserHasSecurityRole(roles);

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
                return false;
            else
                return true;
        },

        //Form Methods CGI Account (from accountLibrary.js)
        onFormLoad: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                switch (formContext.ui.getFormType()) {
                    case FORM_TYPE_CREATE:
                        break;
                    case FORM_TYPE_UPDATE:
                        Endeavor.Skanetrafiken.Account.checkIfUserHasSecRole(formContext);
                        Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext);
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
                alert("Fel i Endeavor.Skanetrafiken.Account.onFormLoad\n\n" + e.message);
            }
        },

        checkIfUserHasSecRole: function (formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var currentUserRoles = globalContext.userSettings.securityRoles();
                for (var i = 0; i < currentUserRoles.length; i++) {
                    var userRoleId = currentUserRoles[i];
                    Endeavor.OData_Querys.GetSecRolesNameAccount(userRoleId, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.checkIfUserHasRole\n\n" + e.message);
            }
        },

        checkIfUserHasRole_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Inga säkerhetsroller definierade!");
                }
                else {
                    var _roleName = result[0].name;
                    var emailField = formContext.getAttribute("emailaddress1").getValue();

                    if (emailField && emailField.Length !== 0 && _roleName.indexOf("Handläggare") > 0)
                        Endeavor.formscriptfunctions.SetState("emailaddress1", "true", formContext); //The field should only be editable until it has content
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.checkIfUserHasRole_callback\n\n" + e.message);
            }
        },

        timerfunction_eHandel: function (formContext) {
            try {
                var arg = 'WebResource_eHandelOrders';
                var obj = formContext.getControl(arg).getObject();
                var entid = formContext.data.entity.getId();

                try {
                    obj.contentWindow.SetID(entid);
                }
                catch (e) {
                    setTimeout(function () { Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext); }, TIMEOUT_COUNTER);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.timerfunction_eHandel\n\n" + e.message);
            }
        },

        format_phonenumber: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var phoneNumberStr = executionContext.getEventSource();
                var control = formContext.getControl(phoneNumberStr.getName());

                // Verify that the field is valid
                if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null) {

                    if (phoneNumberStr.getValue() != null) {

                        // replace any "-" with a blank space
                        var oldNumberStr = phoneNumberStr.getValue();
                        var newNumberStr = oldNumberStr.replace(/-/g, "");
                        newNumberStr = newNumberStr.replace(/ /g, "");
                        phoneNumberStr.setValue(newNumberStr);
                        if (newNumberStr.indexOf("+") > -1)
                            control.setNotification("Ange telefonnummer utan landsprefix.");
                        else
                            control.clearNotification();
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.format_phonenumber\n\n" + e.message);
            }
        },

        //--------------------------------------------------------------RIBBON-------------------------------------------------------------

        //Ribbon Method CGI Account Ribbon (from accountRibbon.js)
        createNewCase: function (accountid) {
            try {
                var _accountid = accountid;

                var parameters = {};
                parameters["entityName"] = "incident";
                parameters["cgi_accountid"] = _accountid;

                // Open the window.
                Xrm.Navigation.openForm(parameters).then(

                    function (success) {
                        console.log(success);
                    },
                    function (error) {
                        console.log(error);
                    });
            }
            catch (e) {
                alert("Error in Endeavor.Skanetrafiken.Account.CreateNewCase\n\n", e.message);
            }
        },

        openMigration_WebResource: function () {
            try {

                var windowOptions = { height: 400, width: 400 };
                Xrm.Navigation.openWebResource("ed_/html/Endeavor.Skanetrafiken.AccountMigration.html", windowOptions);

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Account.openMigration_WebResource");
            }
        },

        hideMigration_Button: function () {
            try {

                return true;

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Account.hideMigration_Button. Error: +" + e.message);
            }
        },
    };
}