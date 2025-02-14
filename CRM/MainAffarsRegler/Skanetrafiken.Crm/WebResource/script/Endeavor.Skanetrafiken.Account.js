﻿FORM_TYPE_CREATE = 1;
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


        onSave: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var seniorCustomer = formContext.getAttribute("ed_seniorcustomer");
            var schoolCustomer = formContext.getAttribute("ed_schoolcustomer");
            var reseller = formContext.getAttribute("ed_reseller");
            var collaborationCustomer = formContext.getAttribute("ed_collaborationcustomer");
            var customer = formContext.getAttribute("ed_customer");
            var portalCustomer = formContext.getAttribute("ed_portalcustomer");
            var agent = formContext.getAttribute("ed_agent");
            var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

            if (seniorCustomer.getValue() == false && schoolCustomer.getValue() == false && reseller.getValue() == false && collaborationCustomer.getValue() == false && customer.getValue() == false && portalCustomer.getValue() == false && agent.getValue() == false && infotainment.getValue() == false) {
                seniorCustomer.setRequiredLevel("required");
                schoolCustomer.setRequiredLevel("required");
                reseller.setRequiredLevel("required");
                collaborationCustomer.setRequiredLevel("required");
                customer.setRequiredLevel("required");
                portalCustomer.setRequiredLevel("required");
                agent.setRequiredLevel("required");
                infotainment.setRequiredLevel("required");

                Xrm.Navigation.openAlertDialog({ text: "Vänligen ange sammanhang innan ni sparar" });
            }

        },

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
            else {
                Endeavor.Skanetrafiken.Account.setCorrectFormForAccountPortal(formContext);
                Endeavor.Skanetrafiken.Account.showInfoAccountPortal(formContext);
            }
            var seniorCustomer = formContext.getAttribute("ed_seniorcustomer");
            var schoolCustomer = formContext.getAttribute("ed_schoolcustomer");
            var reseller = formContext.getAttribute("ed_reseller");
            var collaborationCustomer = formContext.getAttribute("ed_collaborationcustomer");
            var customer = formContext.getAttribute("ed_customer");
            var portalCustomer = formContext.getAttribute("ed_portalcustomer");
            var agent = formContext.getAttribute("ed_agent");
            var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

            if ((seniorCustomer.getValue() == false || seniorCustomer.getValue() == null) && (schoolCustomer.getValue() == false || schoolCustomer.getValue() == null) && (reseller.getValue() == false || reseller.getValue() == null) && (collaborationCustomer.getValue() == false || collaborationCustomer.getValue() == null) && (customer.getValue() == false || customer.getValue() == null) && (portalCustomer.getValue() == false || portalCustomer.getValue() == null) && (agent.getValue() == false || agent.getValue() == null) && (infotainment.getValue() == false || infotainment.getValue() == null)) {
                seniorCustomer.setRequiredLevel("required");
                schoolCustomer.setRequiredLevel("required");
                reseller.setRequiredLevel("required");
                collaborationCustomer.setRequiredLevel("required");
                customer.setRequiredLevel("required");
                portalCustomer.setRequiredLevel("required");
                agent.setRequiredLevel("required");
                infotainment.setRequiredLevel("required");
            }
            else {
                seniorCustomer.setRequiredLevel("none");
                schoolCustomer.setRequiredLevel("none");
                reseller.setRequiredLevel("none");
                collaborationCustomer.setRequiredLevel("none");
                customer.setRequiredLevel("none");
                portalCustomer.setRequiredLevel("none");
                agent.setRequiredLevel("none");
                infotainment.setRequiredLevel("none");
            }

            var roles = [];
            roles[0] = "Skånetrafiken Annons";

            var isUserCheck = Endeavor.Skanetrafiken.Account.currentUserHasSecurityRole(roles);

            if (isUserCheck) {

                if ((seniorCustomer.getValue() == false || seniorCustomer.getValue() == null) && (schoolCustomer.getValue() == false || schoolCustomer.getValue() == null) && (reseller.getValue() == false || reseller.getValue() == null) && (collaborationCustomer.getValue() == false || collaborationCustomer.getValue() == null) && (customer.getValue() == false || customer.getValue() == null) && (portalCustomer.getValue() == false || portalCustomer.getValue() == null) && (agent.getValue() == false || agent.getValue() == null) && (infotainment.getValue() == false || infotainment.getValue() == null)) {

                    if (infotainment != null) {
                        infotainment.setValue(true);
                    }
                }
            }

            Endeavor.Skanetrafiken.Account.hideTabIfInfotainment(executionContext);


            //Hide/Show Õvrig Information
            //Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
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


        hideTabIfInfotainment: function (executionContext) {

            var formContext = executionContext.getFormContext();
            var currForm = formContext.ui.formSelector.getCurrentItem();
            var currFormId = currForm.getId();

            if (currFormId == "7a53340c-a6c5-468a-9ed2-f365da63e74f") { // Annons Form
                setTimeout(Endeavor.Skanetrafiken.Account.hideTab(formContext), 300)
            }

        },

        hideTab: function (formContext) {

            var tabActivities = formContext.ui.tabs.get("Aktiviteter");
            var tabContacts = formContext.ui.tabs.get("tab_3");
            var tabOpp = formContext.ui.tabs.get("tab_4");
            var tabLeadAccount = formContext.ui.tabs.get("tab_5");
            var tabOrders = formContext.ui.tabs.get("Cost Sites");
            var tabQuotes = formContext.ui.tabs.get("tab_7");
            var tabInvoices = formContext.ui.tabs.get("tab_8");
            var tabCompanyInfo = formContext.ui.tabs.get("SUMMARY_TAB");
            var tabEndeavorDev = formContext.ui.tabs.get("tab_6");

            tabActivities.setDisplayState('collapsed');
            tabContacts.setDisplayState('collapsed');
            tabOpp.setDisplayState('collapsed');
            tabLeadAccount.setDisplayState('collapsed');
            tabOrders.setDisplayState('collapsed');
            tabQuotes.setDisplayState('collapsed');
            tabInvoices.setDisplayState('collapsed');
            tabCompanyInfo.setDisplayState('collapsed');
            tabEndeavorDev.setDisplayState('collapsed');
        },


        resetRequiredLevelForAllTypes: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var seniorCustomer = formContext.getAttribute("ed_seniorcustomer");
            var schoolCustomer = formContext.getAttribute("ed_schoolcustomer");
            var reseller = formContext.getAttribute("ed_reseller");
            var collaborationCustomer = formContext.getAttribute("ed_collaborationcustomer");
            var customer = formContext.getAttribute("ed_customer");
            var portalCustomer = formContext.getAttribute("ed_portalcustomer");
            var agent = formContext.getAttribute("ed_agent");
            var infotainment = formContext.getAttribute("ed_infotainmentcustomer");

            if (seniorCustomer != null && schoolCustomer != null && reseller != null && collaborationCustomer != null && customer != null && portalCustomer != null && agent != null && infotainment != null) {

                var seniorValue = seniorCustomer.getValue();
                var schoolValue = schoolCustomer.getValue();
                var resellerValue = reseller.getValue();
                var collaborationValue = collaborationCustomer.getValue();
                var customerValue = customer.getValue();
                var portalValue = portalCustomer.getValue();
                var agentValue = agent.getValue();
                var infotainmentValue = infotainment.getValue();


                if ((seniorValue != false && seniorValue != null) || (schoolValue != false && schoolValue != null) || (resellerValue != false && resellerValue != null) || (collaborationValue != false && collaborationValue != null) || (customerValue != false && customerValue != null) || (portalValue != false && portalValue != null) || (agentValue != false && agentValue != null) || (infotainmentValue != false && infotainmentValue != null)) {
                    seniorCustomer.setRequiredLevel("none");
                    schoolCustomer.setRequiredLevel("none");
                    reseller.setRequiredLevel("none");
                    collaborationCustomer.setRequiredLevel("none");
                    customer.setRequiredLevel("none");
                    portalCustomer.setRequiredLevel("none");
                    agent.setRequiredLevel("none");
                    infotainment.setRequiredLevel("none");
                }
                else if ((seniorValue == false || seniorValue == null) && (schoolValue == false || schoolValue == null) && (resellerValue == false || resellerValue == null) && (collaborationValue == false || collaborationValue == null) && (customerValue == false || customerValue == null) && (portalValue == false || portalValue == null) && (agentValue == false || agentValue == null) && (infotainmentValue == false || infotainmentValue == null)) {
                    seniorCustomer.setRequiredLevel("required");
                    schoolCustomer.setRequiredLevel("required");
                    reseller.setRequiredLevel("required");
                    collaborationCustomer.setRequiredLevel("required");
                    customer.setRequiredLevel("required");
                    portalCustomer.setRequiredLevel("required");
                    agent.setRequiredLevel("required");
                    infotainment.setRequiredLevel("required");
                }
            }

        },

        onChangeTypeOfAccount: function (executionContext) {

            var formContext = executionContext.getFormContext();

            //Hide/Show Õvrig Information
            //Endeavor.Skanetrafiken.Account.showOvrigInformation(formContext);
        },

        onChangePostalCodeCity: function (executionContext, postalCodeLogicalName, cityLogicalName) {

            var formContext = executionContext.getFormContext();
            var postalCode = Endeavor.formscriptfunctions.GetValue(postalCodeLogicalName, formContext);
            var city = Endeavor.formscriptfunctions.GetValue(cityLogicalName, formContext);

            if ((postalCode == null || postalCode == "") && (city == null || city == ""))
                return;

            var fetchXml =
                "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' no-lock='true'>" +
                "  <entity name='ed_postnummer'>" +
                "    <attribute name='ed_postnummerid' />" +
                "    <attribute name='ed_name' />" +
                "    <attribute name='ed_postort' />" +
                "    <attribute name='ed_postnummer' />" +
                "    <attribute name='ed_lanskod' />" +
                "    <attribute name='ed_lan' />" +
                "    <attribute name='ed_kommunkod' />" +
                "    <attribute name='ed_kommun' />" +
                "    <filter type='and'>";

            if (postalCode != null && postalCode != "")
                fetchXml += "      <condition attribute='ed_postnummer' operator='eq' value='" + postalCode + "'/>";

            if (city != null && city != "")
                fetchXml += "      <condition attribute='ed_postort' operator='eq' value='" + city + "'/>";

            fetchXml += "    </filter>" + "  </entity>" + "</fetch>";
            fetchXml = "?fetchXml=" + fetchXml;

            Xrm.WebApi.retrieveMultipleRecords("ed_postnummer", fetchXml).then(
                function success(result) {

                    if (result.entities.length == 1) {
                        var name = result.entities[0].ed_name;
                        var postNummer = result.entities[0].ed_postnummer;
                        var postOrt = result.entities[0].ed_postort;
                        var lansKod = result.entities[0].ed_lanskod;
                        var lan = result.entities[0].ed_lan;
                        var kommunKod = result.entities[0].ed_kommunkod;
                        var kommun = result.entities[0].ed_kommun;

                        if (postalCodeLogicalName.indexOf('1') > -1 && cityLogicalName.indexOf('1') > -1) {
                            Endeavor.formscriptfunctions.SetValue("address1_name", name, formContext);
                            Endeavor.formscriptfunctions.SetValue("address1_postalcode", postNummer, formContext);
                            Endeavor.formscriptfunctions.SetValue("address1_city", postOrt, formContext);
                            Endeavor.formscriptfunctions.SetValue("ed_address1_countynumber", parseInt(lansKod), formContext);
                            Endeavor.formscriptfunctions.SetValue("address1_county", lan, formContext);
                            Endeavor.formscriptfunctions.SetValue("ed_address1_communitynumber", parseInt(kommunKod), formContext);
                            Endeavor.formscriptfunctions.SetValue("address1_stateorprovince", kommun, formContext);
                        }
                        else if (postalCodeLogicalName.indexOf('2') > -1 && cityLogicalName.indexOf('2') > -1) {
                            Endeavor.formscriptfunctions.SetValue("address2_name", name, formContext);
                            Endeavor.formscriptfunctions.SetValue("address2_postalcode", postNummer, formContext);
                            Endeavor.formscriptfunctions.SetValue("address2_city", postOrt, formContext);
                            Endeavor.formscriptfunctions.SetValue("ed_address1_countynumber", parseInt(lansKod), formContext);
                            Endeavor.formscriptfunctions.SetValue("address2_county", lan, formContext);
                            Endeavor.formscriptfunctions.SetValue("ed_address1_communitynumber", parseInt(kommunKod), formContext);
                            Endeavor.formscriptfunctions.SetValue("address2_stateorprovince", kommun, formContext);
                        }
                    }
                    else if (result.entities.length == 0)
                        Endeavor.formscriptfunctions.AlertCustomDialog("Inget postnummer kommun hittades med postnummer: " + postalCode + " och stad: " + city + ". Vänligen överväg att ange stad och postnummer.");
                    else if (result.entities.length > 1)
                        Endeavor.formscriptfunctions.AlertCustomDialog("Flera postnummer kommun hittades med postnummer: " + postalCode + " och stad: " + city + ". Vänligen överväg att ange stad och postnummer.");
                },
                function (error) {
                    Endeavor.formscriptfunctions.ErrorCustomDialog(error.message, "Generic Error");
                    console.log(error.message);
                }
            );
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Block button
        onBlockAccountShow: function (formContext) {
            var currFormId = formContext.ui.formSelector.getCurrentItem().getId();
            /*Account (Skånetrafiken)/Företag (Skånetrafiken) "003f08c2-06d9-494b-926a-42020901c6b2"
                Account (Portal)/"Företag" "c88de51d-55b5-42ff-80c7-1774646b3b70" */
            if ( currFormId != "003f08c2-06d9-494b-926a-42020901c6b2")/*currFormId != "c88de51d-55b5-42ff-80c7-1774646b3b70" &&*/ //Show only Block organization for organization form
                return false;
            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount") != null ? formContext.getAttribute("ed_typeofaccount").getValue() : -1;
            if (accountType == -1 || (accountType == null || accountType == 899310001)) return false;
             
            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = Endeavor.Skanetrafiken.Account.showBlockButton();//Block Portal Account role

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == false && showButton != false)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // ORGANISATION (TOP LEVEL) - Show/hide Unblock button
        onUnblockAccountShow: function (formContext) {
            var currFormId = formContext.ui.formSelector.getCurrentItem().getId();
            /*Account (Skånetrafiken)/Företag (Skånetrafiken) "003f08c2-06d9-494b-926a-42020901c6b2"
                Account (Portal)/"Företag" "c88de51d-55b5-42ff-80c7-1774646b3b70" */
            if (currFormId != "003f08c2-06d9-494b-926a-42020901c6b2")/*currFormId != "c88de51d-55b5-42ff-80c7-1774646b3b70" && */ //Show only Block organization for organization form
                return false;
            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount") != null ? formContext.getAttribute("ed_typeofaccount").getValue() : -1;
            if (accountType == -1 || (accountType == null || accountType == 899310001)) return false;
             
            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = Endeavor.Skanetrafiken.Account.showBlockButton(); //Block Portal Account role

            // If Account = Organization (top level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Organization"
            if (parentAccount == null && lockedPortal == true && showButton != false) return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Block button
        onBlockCostSiteShow: function (formContext) {
            var currFormId = formContext.ui.formSelector.getCurrentItem().getId();

            if (currFormId != "c88de51d-55b5-42ff-80c7-1774646b3b70") //Account (Portal)/"Företag"
                return false;
            // Only show button if TypeOfAccount = Företagskund (Portal)
            var accountType = formContext.getAttribute("ed_typeofaccount") != null ? formContext.getAttribute("ed_typeofaccount").getValue() : -1;
            if (accountType == -1 || (accountType == null || accountType == 899310001)) return false;
             
            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = Endeavor.Skanetrafiken.Account.showBlockButton(); 

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == false && showButton != false)
                return true;
            // If Account = Cost Site (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        // COST SITE (BOTTOM LEVEL) - Show/hide Unblock button
        onUnblockCostSiteShow: function (formContext) {
            var currFormId = formContext.ui.formSelector.getCurrentItem().getId();

            if (currFormId != "c88de51d-55b5-42ff-80c7-1774646b3b70") //Account (Portal)/"Företag"
                return false;

            // Only show button if TypeOfAccount = Företagskund (Portal)            
            var accountType = formContext.getAttribute("ed_typeofaccount") != null ? formContext.getAttribute("ed_typeofaccount").getValue() : -1;
            if (accountType == -1 || (accountType == null || accountType == 899310001)) return false;
            
            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var lockedPortal = formContext.getAttribute("ed_islockedportal").getValue();
            var showButton = Endeavor.Skanetrafiken.Account.showBlockButton();

            // If Account = Cost Site (bottom level) and 
            // if ed_isLockedPortal = false
            // => Show button "Block Cost Site"
            if (parentAccount != null && lockedPortal == true && showButton != false)
                return true;
            // If Account = CostSite (bottom level) and
            // if ed_isLockedPortal = true
            else
                return false;
        },

        setCorrectFormForAccountPortal: function (formContext) {
            // Only for TypeOfAccount = Företagskund (Portal)
           
            var accountType = formContext.getAttribute("ed_typeofaccount") != null ? formContext.getAttribute("ed_typeofaccount").getValue() : null;
            if (accountType == 899310000) {
                var parentAccount = formContext.getAttribute("parentaccountid") ? formContext.getAttribute("parentaccountid").getValue(): -1;
                var setFormId = "003f08c2-06d9-494b-926a-42020901c6b2"; //Account (Skånetrafiken)
                if (parentAccount != -1 && parentAccount != null) //account has parent is Företagskonto/Cost site and should have this form
                    setFormId = "c88de51d-55b5-42ff-80c7-1774646b3b70"; //Account(Portal) / "Företag"
                     
                var currFormId = formContext.ui.formSelector.getCurrentItem().getId();
                var forms = formContext.ui.formSelector.items.get();
                for (var f = 0; f < forms.length; f++) {
                    if (currFormId != setFormId && forms[f].getId() == setFormId)
                        formContext.ui.formSelector.items.get(f).navigate();  
                }
            }
        },
        //showOvrigInformation: function (formContext) {

        //    var infotaimentCustomer = formContext.getAttribute("ed_infotainmentcustomer");
        //    var customer = formContext.getAttribute("ed_customer");
        //    var agent = formContext.getAttribute("ed_agent");
        //    var samarbete = formContext.getAttribute("ed_collaborationcustomer");
        //    var reseller = formContext.getAttribute("ed_reseller");
        //    var school = formContext.getAttribute("ed_schoolcustomer");
        //    var senior = formContext.getAttribute("ed_seniorcustomer");
        //    var portal = formContext.getAttribute("ed_portalcustomer");

        //    if (infotaimentCustomer == null || customer == null || agent == null || samarbete == null || reseller == null || school == null || senior == null || portal == null)
        //        return;

        //    if (infotaimentCustomer.getValue() && !customer.getValue() && !agent.getValue() && !samarbete.getValue() &&
        //        !reseller.getValue() && !school.getValue() && !senior.getValue() && !portal.getValue()) {

        //        formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(false);
        //        formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(false); 
        //        formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(false);
        //        formContext.ui.tabs.get("Cost Sites").setVisible(false);

        //    }
        //    else {
        //        formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_12").setVisible(true);
        //        formContext.ui.tabs.get("tab_3").sections.get("tab_3_section_1").setVisible(true); 
        //        formContext.ui.tabs.get("SUMMARY_TAB").sections.get("SUMMARY_TAB_section_9").setVisible(true);
        //        formContext.ui.tabs.get("Cost Sites").setVisible(true);
        //    }
        //},

        showInfoAccountPortal: function (formContext) {
            var xMsg = "företagskonto"; //for cost sites
            var parentAccount = formContext.getAttribute("parentaccountid") ? formContext.getAttribute("parentaccountid").getValue():null;
            if (parentAccount === null) {
                xMsg = "företag"; //Organisation = no parent
                if (formContext.ui.tabs.get("Cost Sites") !== null)
                    formContext.ui.tabs.get("Cost Sites").setVisible(true);

                if (formContext.ui.tabs.get("Sales History") !== null)
                    formContext.ui.tabs.get("Sales History").setVisible(false);
            }
            else {
                if (formContext.ui.tabs.get("Cost Sites") !== null)
                    formContext.ui.tabs.get("Cost Sites").setVisible(false);

                if (formContext.ui.tabs.get("Sales History") !== null)
                    formContext.ui.tabs.get("Sales History").setVisible(true);
            } 

            if (formContext.getControl("ed_islockedportal")) formContext.getControl("ed_islockedportal").setDisabled(true);

            var isLocked = formContext.getAttribute("ed_islockedportal") ? formContext.getAttribute("ed_islockedportal").getValue() : false;
            if (isLocked == true) {
                formContext.ui.setFormNotification("Detta " + xMsg + " är låst från företagssidan", "WARNING", "2");
            }

            var allowCreate = formContext.getAttribute("ed_allowcreate") ? formContext.getAttribute("ed_allowcreate").getValue() : null;
            if (allowCreate != null && allowCreate == false) {
                formContext.ui.setFormNotification("OBS - Detta företag kan inte skapa upp nya administratörer via företagssidan", "INFO", "1");
            }
        },

        blockAccountPortal: function (formContext) {

            var parentAccount = formContext.getAttribute("parentaccountid").getValue();
            var blocked = formContext.getAttribute("ed_islockedportal").getValue();
            var msgText = "";

            if (parentAccount == null && blocked === false)
                msgText = "Är du säker på att du vill spärra hela organisationen från åtkomst till företagsportalen?";
            else if (parentAccount == null && blocked === true)
                msgText = "Är du säker på att du vill avblockera hela organisationen och ge åtkomst till företagsportalen?";
            else if (parentAccount != null && blocked === false)
                msgText = "Är du säker på att du vill spärra detta företagskonto från åtkomst till företagsportalen?";
            else
                msgText = "Är du säker på att du vill avblockera detta företagskonto och ge åtkomst till företagsportalen?";

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

                        //Handle error message
                        var errorMessage = "";
                        var devider = "Message: ";
                        var parsedMessage = "";
                        var printMessageAvblockera = "";
                        var printMessageSparra = "";

                        if (error.innerError) {
                            if (error.innerError.message) {
                                errorMessage = error.innerError.message;
                            }
                        }

                        if (errorMessage != "") {
                            parsedMessage = errorMessage.split(devider).pop();
                        }

                        if (blocked) {
                            if (parsedMessage != "") {
                                printMessageAvblockera = "Kunde inte avblockera företag. Var god försök igen senare. (Fel => " + parsedMessage + ").";
                                Endeavor.formscriptfunctions.AlertCustomDialog(printMessageAvblockera);
                            }
                            else {
                                Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte avblockera företag. Var god försök igen senare.");
                            }
                        }
                        else {
                            if (parsedMessage != "") {
                                printMessageSparra = "Kunde inte spärra företag. Var god försök igen senare. (Fel => " + parsedMessage + ").";
                                Endeavor.formscriptfunctions.AlertCustomDialog(printMessageSparra);
                            }
                            else {
                                Endeavor.formscriptfunctions.AlertCustomDialog("Kunde inte spärra företag. Var god försök igen senare.");
                            }
                        }
                    });
            }
            else {

                var ID = formContext.data.entity.getId();
                ID = ID.substring(1, ID.length - 1);

                Xrm.WebApi.retrieveMultipleRecords("account", "?$select=accountnumber&$filter=(_parentaccountid_value eq " + ID + ")").then(
                    function success(results) {

                        for (var i = 0; i < results.entities.length; i++) {

                            var account = results.entities[i];
                            if (account.accountnumber) {

                                try {

                                    var inputParameters = [{ "Field": "PortalID", "Value": account.accountnumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
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

                                            //Handle error message
                                            var errorMessage = "";
                                            var devider = "Message: ";
                                            var parsedMessage = "";
                                            var printMessageAvblockera = "";
                                            var printMessageSparra = "";

                                            if (error.innerError) {
                                                if (error.innerError.message) {
                                                    errorMessage = error.innerError.message;
                                                }
                                            }

                                            if (errorMessage != "") {
                                                parsedMessage = errorMessage.split(devider).pop();
                                            }

                                            if (blocked) {
                                                if (parsedMessage != "") {
                                                    //printMessageAvblockera = "Kunde inte avblockera företag. Var god försök igen senare. (Fel => " + parsedMessage + ").";
                                                    printMessageAvblockera = "Avblockering av företagskonto(n) misslyckades " + account.accountnumber + " (Fel => " + parsedMessage + ").";
                                                    Endeavor.formscriptfunctions.AlertCustomDialog(printMessageAvblockera);
                                                }
                                                else {
                                                    Endeavor.formscriptfunctions.AlertCustomDialog("Avblockering av företagskonto(n) misslyckades " + account.accountnumber);
                                                }
                                            }
                                            else {
                                                if (parsedMessage != "") {
                                                    //printMessageSparra = "Kunde inte spärra företag. Var god försök igen senare. (Fel => " + parsedMessage + ").";
                                                    printMessageSparra = "Spärr av företagskonto(n) misslyckades " + account.accountnumber + " (Fel => " + parsedMessage + ").";
                                                    Endeavor.formscriptfunctions.AlertCustomDialog(printMessageSparra);
                                                }
                                                else {
                                                    Endeavor.formscriptfunctions.AlertCustomDialog("Spärr av företagskonto(n) misslyckades " + account.accountnumber);
                                                }
                                            }
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
                var formType = formContext.ui.getFormType();
                var currFormId = formContext.ui.formSelector.getCurrentItem().getId();
                switch (formType) {
                    case FORM_TYPE_CREATE:

                        break;
                    case FORM_TYPE_UPDATE:
                        Endeavor.Skanetrafiken.Account.checkIfUserHasSecRole(formContext);
                        Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext);

                        Endeavor.Skanetrafiken.Account.setCorrectFormForAccountPortal(formContext);
                        if (currFormId == "003f08c2-06d9-494b-926a-42020901c6b2") //Account (Skånetrafiken)/Företag (Skånetrafiken)
                        {
                            Endeavor.Skanetrafiken.Account.showInfoAccountPortal(formContext);
                        }

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

        checkIfUserHasSecRole: function (executionContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();
                var formContext = executionContext.getFormContext != null ? executionContext.getFormContext() : executionContext;

                var currentUserRoles = globalContext.userSettings.securityRoles;
                for (var i = 0; i < currentUserRoles.length; i++) {
                    var userRoleId = currentUserRoles[i];

                    var globalContext = Xrm.Utility.getGlobalContext();
                    var clientURL = globalContext.getClientUrl();

                    var url = clientURL + "/api/data/v9.0/roles?$select=name&$filter=roleid eq " + userRoleId;
                    var results = Endeavor.formscriptfunctions.fetchJSONResults(url);
                    Endeavor.Skanetrafiken.Account.checkIfUserHasRole_callback(results, formContext);
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
                    var emailField = formContext.getAttribute("emailaddress1");

                    if (emailField == null)
                        return;

                    var emailValue = emailField.getValue();

                    if (emailValue && emailValue.Length !== 0 && _roleName.indexOf("Handläggare") > 0)
                        Endeavor.formscriptfunctions.SetState("emailaddress1", "true", formContext); //The field should only be editable until it has content
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Account.checkIfUserHasRole_callback\n\n" + e.message);
            }
        },

        timerfunction_eHandel: function (formContext) {
            try {
                /*TODO - this is commented out, until the Silverlight page is coded as an HTML if needed - 03/11/2020*/
                //var arg = 'WebResource_eHandelOrders';
                //var obj = formContext.getControl(arg).getObject();
                //var entid = formContext.data.entity.getId();

                //try {
                //    obj.contentWindow.SetID(entid);
                //}
                //catch (e) {
                //    setTimeout(function () { Endeavor.Skanetrafiken.Account.timerfunction_eHandel(formContext); }, TIMEOUT_COUNTER);
                //}
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

        showCreateChildAccountButton: function (formContext) {
            try {

                var formItem = formContext.ui.formSelector.getCurrentItem();
                if (formItem != null) {
                    var formLabel = formItem.getLabel();
                    return formLabel == "Annons";
                }
                return false;

            } catch (e) {
                alert("Error in Endeavor.Skanetrafiken.Account.showCreateChildAccountButton\n\n", e.message);
            }
        },

        createChildAccount: function (formContext) {
            try {

                var id = formContext.data.entity.getId();

                if (id == null || id == "")
                    return;

                var name = Endeavor.formscriptfunctions.GetValue("name", formContext);

                var entityFormOptions = {};
                entityFormOptions["entityName"] = "account";

                // Set default values for the Child Account
                var formParameters = {};
                formParameters["name"] = name;
                formParameters["ed_infotainmentcustomer"] = true;
                formParameters["parentaccountid"] = id;
                formParameters["parentaccountidname"] = name;

                // Open the form.
                Xrm.Navigation.openForm(entityFormOptions, formParameters).then(
                    function (success) {
                        console.log(success);
                    },
                    function (error) {
                        console.log(error); parentaccountid
                    });

            } catch (e) {
                alert("Error in Endeavor.Skanetrafiken.Account.createChildAccount\n\n", e.message);
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

        hideMigration_Button: function () {
            try {

                return true;

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Account.hideMigration_Button. Error: +" + e.message);
            }
        },
    };
}