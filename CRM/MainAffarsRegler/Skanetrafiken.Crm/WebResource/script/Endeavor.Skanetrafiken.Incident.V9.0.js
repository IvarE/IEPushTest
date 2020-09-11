FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;

TIMEOUT_COUNTER = 400;

CASE_ORIGIN_EMAIL = 2;
CASE_ORIGIN_LETTER = 285050002;
CASE_ORIGIN_PHONE = 1;
CASE_ORIGIN_CUSTOMERCENTER = 285050004;
CASE_ORIGIN_INTERNAL = 285050005;
CASE_ORIGIN_WEBSKANETRAFIKEN = 3;
CASE_ORIGIN_WEBORESUNDSTRAIN = 285050006;
CASE_ORIGIN_CHAT = 285050001;
CASE_ORIGIN_TWITTER = 3986;
CASE_ORIGIN_FACEBOOK = 285050000;
CASE_ORIGIN_FACEBOOKPERSONALMESSAGE = 2483;
CASE_ORIGIN_FAX = 285050003;
CASE_ORIGIN_RESEGARANTIONLINE = 285050007;

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.Incident) == "undefined") {
    Endeavor.Skanetrafiken.Incident = {

        alertCustomDialog: function (msgText) {

            var message = { confirmButtonLabel: "Ok", text: msgText };
            var alertOptions = { height: 150, width: 280 };

            Xrm.Navigation.openAlertDialog(message, alertOptions).then(
                function success(result) {
                    console.log("Alert dialog closed");
                },
                function (error) {
                    console.log(error.message);
                }
            );
        },

        onLoad: function (executionContext) {
            debugger;
            var formContext = executionContext.getFormContext();

            var contactAttribute = formContext.getAttribute("cgi_contactid")
            var customerEmailAttribute = formContext.getAttribute("cgi_customer_email");

            if (contactAttribute && customerEmailAttribute) {
                if (customerEmailAttribute.getValue && !customerEmailAttribute.getValue()) {
                    if (contactAttribute.getValue && contactAttribute.getValue() && contactAttribute.getValue().length > 0) {

                        var columnSet = "emailaddress1,emailaddress2";
                        Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=" + columnSet + "&$filter=contactid eq " + contactAttribute.getValue()[0].id + ")").then(
                            function success(contactResult) {

                                if (contactResult && contactResult.entities.length > 0) {
                                    if (contactResult.entities[0].emailaddress1)
                                        customerEmailAttribute.setValue(contactResult.entities[0].emailaddress1);
                                    else if (contactResult.entities[0].emailaddress2)
                                        customerEmailAttribute.setValue(contactResult.entities[0].emailaddress2);
                                }

                            },
                            function (error) {
                                console.log(error.message);
                                Endeavor.Skanetrafiken.Incident.alertCustomDialog(error.message);
                            }
                        );
                    }
                }
            }
        },

        /*
         * 
         * CGI Incident (From incidentLibrary.js)
         * 
         */

        onFormLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var wrControl = formContext.getControl("cgi_/CRM.GetBIFFTransactionsPage.html");
            if (wrControl) {
                wrControl.getContentWindow().then(
                    function (contentWindow) {
                        contentWindow.setClientApiContext(Xrm, formContext);
                    }
                )
            }


            Endeavor.Skanetrafiken.Incident.setVisibilityOnLoad(formContext);

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:
                    Endeavor.Skanetrafiken.Incident.setCustomerOnLoad(formContext);
                    Endeavor.Skanetrafiken.Incident.setDefaultOnCreate(formContext);
                    //Endeavor.Skanetrafiken.Incident.setAccountOrContactVisibility(formContext);
                    break;
                case FORM_TYPE_UPDATE:
                    Endeavor.Skanetrafiken.Incident.setOnUpdate(formContext);
                case FORM_TYPE_READONLY:
                case FORM_TYPE_DISABLED:
                    Endeavor.Skanetrafiken.Incident.onLoad(formContext);
                    //Endeavor.Skanetrafiken.Incident.setAccountOrContactVisibility(formContext);
                    break;
                case FORM_TYPE_QUICKCREATE:
                case FORM_TYPE_BULKEDIT:
                    break;
                default:
                    alert("Form type error!");
                    break;
            }
        },

        onSave: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var _case_stage = Endeavor.formscriptfunctions.GetValue("incidentstagecode", formContext);
            if (_case_stage == 1 || _case_stage == 285050003) {
                formContext.getAttribute("incidentstagecode").setValue(285050000);
            }

            var _cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();
            var _cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

            if (_cgi_accountid != null) {
                Endeavor.Skanetrafiken.Incident.setCustomerFromAccount();
            }

            if (_cgi_contactid != null) {
                Endeavor.Skanetrafiken.Incident.setCustomerFromContact();
            }



            //if (prmContext != null && prmContext.getEventArgs() != null) {

            //    var SaveMode = prmContext.getEventArgs().getSaveMode();
            //    alert(SaveMode);

            //    if (SaveMode == 5) {

            //        alert("Write your validation code here");
            //        if (true) {
            //            if (confirm("Ärende av kategori X förväntas innehålla trafikinformation, vilket saknas för detta ärende. Vill du avsluta ärendet ändå? ") == true) {
            //                alert("Svar Ja!");
            //            }
            //            else {
            //                alert("Svar Nej!");
            //                // Use the code line below only if validation is failed then abort function save event
            //                prmContext.getEventArgs().preventDefault();
            //                return;
            //            }
            //        }

            //    }
            //}
            //else {
            //    alert("Context should be sent as a parameter to onSave function!");
            //}



        },

        onLoadHideShowTypeOfContactFields: function () {
            try {
                setTimeout(Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields, TIMEOUT_COUNTER);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onLoadHideShowTypeOfContactFields\n\n" + e.Message);
            }
        },

        onLoad: function (formContext) {
            try {
                setTimeout(Endeavor.Skanetrafiken.Incident.timerfunction_BIFF(formContext), TIMEOUT_COUNTER);
                //setTimeout(Endeavor.Skanetrafiken.Incident.timerfunction_Travel(formContext), TIMEOUT_COUNTER);
                //setTimeout(Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields, TIMEOUT_COUNTER);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onLoad\n\n" + e.Message);
            }
        },

        onSendToQueue: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Incident.onSave();
            Endeavor.formscriptfunctions.SaveAndCloseEntity(formContext);
        },

        setAccountOrContactVisibility: function (formContext) {
            try {
                var _cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();
                var _cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

                if (_cgi_accountid == null && _cgi_contactid == null) {
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
                }

                if (_cgi_accountid != null && _cgi_contactid == null) {
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", false, formContext);
                }

                if (_cgi_accountid == null && _cgi_contactid != null) {
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
                }

            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setAccountOrContactVisibility\n\n" + e.Message);
            }
        },

        onChangeDate: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var datetime = formContext.getAttribute("cgi_actiondate").getValue();
        },

        onChangeOrigin: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var origin = formContext.getAttribute("caseorigincode").getValue();
                var priority = 0;

                switch (origin) {
                    case CASE_ORIGIN_EMAIL:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_LETTER:
                        priority = 3;
                        break;

                    case CASE_ORIGIN_CHAT:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_FACEBOOKPERSONALMESSAGE:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_FACEBOOK:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_FAX:
                        priority = 3;
                        break;

                    case CASE_ORIGIN_CUSTOMERCENTER:
                        priority = 2;
                        break;

                    case CASE_ORIGIN_INTERNAL:
                        priority = 3;
                        break;

                    case CASE_ORIGIN_WEBSKANETRAFIKEN:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_WEBORESUNDSTRAIN:
                        priority = 1;
                        break;

                    case CASE_ORIGIN_PHONE:
                        priority = 2;
                        break;

                    default:
                        priority = 2;
                }

                formContext.getAttribute("prioritycode").setValue(priority);
                formContext.getAttribute("cgi_runpriorityworkflow").setValue(0);

            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onChangeOrigin\n\n" + e.Message);
            }
        },

        onChangeAccount: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var __cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();

                if (__cgi_accountid != null) {
                    //Endeavor.formscriptfunctions.SetValue("cgi_contactid", null);
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", false);
                }
                else {
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", true);
                }

                var __customerid = formContext.getAttribute("customerid").getValue();
                //formContext.getAttribute("cgi_contactid").setValue(null);

                if (__cgi_accountid != null) {
                    //if (__customerid != null) {
                    Endeavor.Skanetrafiken.Incident.setCustomerFromAccount(formContext);
                    //}
                }

                if (__cgi_accountid == null) {
                    Endeavor.Skanetrafiken.Incident.setCustomerOnLoad(formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onChangeAccount\n\n" + e.Message);
            }
        },

        onChangeContact: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var __cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

                if (__cgi_contactid != null) {
                    //Endeavor.formscriptfunctions.SetValue("cgi_accountid", null, formContext);
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                    Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields(formContext);

                }
                else {
                    //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
                }

                var __customerid = formContext.getAttribute("customerid").getValue();
                //formContext.getAttribute("cgi_accountid").setValue(null);

                if (__cgi_contactid != null) {
                    //if (__customerid != null) {
                    Endeavor.Skanetrafiken.Incident.setCustomerFromContact(formContext);

                    //}
                }

                if (__cgi_contactid == null) {
                    Endeavor.Skanetrafiken.Incident.setCustomerOnLoad(formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onChangeAccount\n\n" + e.Message);
            }
        },

        onChangeTravelCard: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                // MaxP 2016-04-28 Har lagt till så att kortnummer inte sätts i BIFF kontrollen när den är kollapsad
                var _tab_biffinformation = formContext.ui.tabs.get("tab_BIFFInformation").getDisplayState()
                if (_tab_biffinformation == "expanded") {
                    var arg = 'WebResource_BIFFTransactions';
                    var obj = formContext.getControl(arg).getObject();
                    var travelcardid = Endeavor.formscriptfunctions.GetValue("cgi_unregisterdtravelcard", formContext);
                    obj.contentWindow.SetTravelCardNumber(travelcardid);
                }
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onChangeTravelCard\n\n" + e.Message);
            }
        },

        /**
         * This function runs on onLoad.
         * It hides fields from 'Type Of Contact' quick view form if fields are 'false', otherwise show it.
         * */
        hideOrShowTypeOfContactFields: function (formContext) {
            try {
                var contactId = formContext.getAttribute("cgi_contactid");
                if (contactId) {
                    var contactValue = contactId.getValue();
                    if (contactValue) {

                        var query = "contacts?$select=ed_privatecustomercontact,ed_businesscontact,ed_infotainmentcontact,ed_schoolcontact,ed_seniorcontact,ed_agentcontact" +
                            "&$filter=contactid eq " + contactValue[0].id.replace(/[{}]/g, '');;
                        var serverUrl = Xrm.Page.context.getClientUrl();
                        var oDataEndpointUrl = serverUrl + "/api/data/v8.1/";
                        oDataEndpointUrl += query;
                        var ODataRequest = new XMLHttpRequest();
                        ODataRequest.open("GET", oDataEndpointUrl, false); // false = synchronous request
                        ODataRequest.setRequestHeader("Accept", "application/json");
                        ODataRequest.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                        ODataRequest.send();

                        if (ODataRequest.status === 200) {
                            var parsedResults = JSON.parse(ODataRequest.responseText);
                            if (parsedResults != null && parsedResults.value != null) {

                                var typeOfContact_tab = Xrm.Page.ui.quickForms.get("type_of_contact");
                                //If cannot find quick view form, rerun method.
                                if (typeOfContact_tab) {
                                    //If quick view form is not loaded rerun method.
                                    if (typeOfContact_tab.isLoaded()) {

                                        //Fetches all attributes from quick view form.
                                        var fieldArr = typeOfContact_tab.data.entity.attributes.getAll();
                                        if (fieldArr) {
                                            for (var i = 0; i < fieldArr.length; i++) {
                                                var fieldName = fieldArr[i].getName(); //Fetch the name of the attribute
                                                var attr_ctrl = typeOfContact_tab.getControl(fieldName);
                                                if (attr_ctrl) {
                                                    if (fieldName == "ed_privatecustomercontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_privatecustomercontact || false);
                                                    else if (fieldName == "ed_businesscontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_businesscontact || false);
                                                    else if (fieldName == "ed_infotainmentcontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_infotainmentcontact || false);
                                                    else if (fieldName == "ed_schoolcontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_schoolcontact || false);
                                                    else if (fieldName == "ed_seniorcontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_seniorcontact || false);
                                                    else if (fieldName == "ed_agentcontact")
                                                        attr_ctrl.setVisible(parsedResults.value[0].ed_agentcontact || false);
                                                }

                                            }
                                        }
                                        return;

                                    } else {
                                        setTimeout(Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields, 300);
                                    }
                                } else {
                                    setTimeout(Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields, 300);
                                }
                            }
                        } else {
                            console.log("Could not fetch values. Contact Admin.");
                        }
                    }
                }

            } catch (e) {
                alert("Exception caught in Endeavor.Skanetrafiken.Incident.hideOrShowTypeOfContactFields. Error: " + e.message);
            }
        },

        hideFields: function (parsedResult) {
            try {

            } catch (e) {
                alert("Something went wrong.");
            }
        },

        timerfunction_BIFF: function (formContext) {
            try {
                var arg = 'WebResource_BIFFTransactions';
                var obj = formContext.getControl(arg).getObject();
                var entid = formContext.data.entity.getId();

                try {
                    obj.contentWindow.SetID(entid);
                }
                catch (e) {
                    setTimeout(Endeavor.Skanetrafiken.Incident.timerfunction_BIFF(formContext), TIMEOUT_COUNTER);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.timerfunction_BIFF\n\n" + e.Message);
            }
        },

        timerfunction_Travel: function (formContext) {
            try {
                var arg = 'WebResource_TravelInfo';
                var obj = formContext.getControl(arg).getObject();
                var entid = formContext.data.entity.getId();

                try {
                    obj.contentWindow.SetID(entid);
                }
                catch (e) {
                    setTimeout(Endeavor.Skanetrafiken.Incident.timerfunction_Travel(formContext), TIMEOUT_COUNTER);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.timerfunction_Travel\n\n" + e.Message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Set customer to account attribute
        setCustomerFromAccount: function (formContext) {
            var __customerid = formContext.getAttribute("customerid").getValue();
            var _cgi_accountid_logicalname = "account" //formContext.getAttribute("cgi_accountid").getValue()[0].logicalname;
            var _cgi_accountid_id = formContext.getAttribute("cgi_accountid").getValue()[0].id;
            var _cgi_accountid_name = formContext.getAttribute("cgi_accountid").getValue()[0].name;
            Endeavor.formscriptfunctions.SetLookup("customerid", _cgi_accountid_logicalname, _cgi_accountid_id, _cgi_accountid_name, formContext);
        },


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Set customer to contact attribute
        setCustomerFromContact: function (formContext) {
            var __customerid = formContext.getAttribute("customerid").getValue();
            var _cgi_contactid_logicalname = "contact"; //formContext.getAttribute("cgi_contactid").getValue()[0].logicalname;
            var _cgi_contactid_id = formContext.getAttribute("cgi_contactid").getValue()[0].id;
            var _cgi_contactid_name = formContext.getAttribute("cgi_contactid").getValue()[0].name;

            Endeavor.formscriptfunctions.SetLookup("customerid", _cgi_contactid_logicalname, _cgi_contactid_id, _cgi_contactid_name, formContext);

        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Set default values on create
        setDefaultOnCreate: function (formContext) {
            try {
                var _currentdate = new Date();
                formContext.getAttribute("cgi_arrival_date").setValue(_currentdate);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setCaseDefaultOnLoad\n\n" + e.Message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Show fields on update
        setOnUpdate: function (formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var lookupObject = formContext.getAttribute("cgi_casdet_row2_cat3id");

                if (lookupObject != null) {
                    var lookUpObjectValue = lookupObject.getValue();

                    if ((lookUpObjectValue != null)) {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true, formContext);
                    }
                }

                var entityId = formContext.data.entity.getId();
                var currentUserId = globalContext.userSettings.userId();
                var entityType = "Incident";

                // We check paas fields, and hidden field to find out if incident has been opened from paas more then once.
                var isPaasIncident = formContext.getAttribute("cgi_sfn").getValue() !== null;
                // NOTE: should be lowercase name we use in getAttribute
                var cgi_passhasbeenopenedatleastonce_value = formContext.getAttribute("cgi_passhasbeenopenedatleastonce".toLowerCase()).getValue();
                var isOpenedFromPaasFirstTime = cgi_passhasbeenopenedatleastonce_value === null;

                if (isPaasIncident && isOpenedFromPaasFirstTime) {

                    // Function signature:
                    //assignRequest: function (Assignee, Target, Type, successCallback, errorCallback)
                    SDK.SOAPAssign.assignRequest(currentUserId, entityId, entityType,
                        function (p1, p2, p3) {

                            formContext.data.refresh(false);

                            // NOTE: schema name in object properties, with possible upper case letters
                            var updateDTO = {
                                cgi_passhasbeenopenedatleastonce: true
                            };

                            // Function signature:
                            //updateRecord: function (id, object, type, successCallback, errorCallback) {
                            SDK.REST.updateRecord(entityId, updateDTO, entityType,
                                function (p1, p2, p3) { /*silent success*/ },
                                function (p1, p2, p3) {
                                    alert("Fel i Endeavor.Skanetrafiken.Incident.setOnUpdate\n\n" + p1);
                                    console.log("fail p1 " + p1 + " fail p2 " + p2 + "fail p3 " + p3);
                                });

                        },
                        function (p1, p2, p3) {
                            alert("Fel i Endeavor.Skanetrafiken.Incident.setOnUpdate\n\n" + p1);
                            console.log("fail p1 " + p1 + " fail p2 " + p2 + "fail p3 " + p3);
                        }
                    );

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setOnUpdate\n\n" + e.Message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Set tab epanded/collapsed and visible/not visible
        toggleTabDisplayState: function (tabName, tabDisplayState, tabIsVisible formContext) {
            //Hide/Show and/or Expand/Collapse tabs     
            var tabs = formContext.ui.tabs.get();
            for (var i in tabs) {
                var tab = tabs[i];
                //alert("Namn p� tab " + tab.getName());
                if (tab.getName() == tabName) {
                    tab.setVisible(tabIsVisible);
                    tab.setDisplayState(tabDisplayState);
                }
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Set default customer to customerid attribute
        setCustomerOnLoad: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetDefaultCustomerFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setCustomerOnLoad\n\n" + e.Message);
            }
        },

        setCustomerOnLoad_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen default kund definerad!");
                }
                else {
                    var _id = result[0].cgi_DefaultCustomerOnCase.Id;
                    var _logicalname = result[0].cgi_DefaultCustomerOnCase.LogicalName;
                    var _name = result[0].cgi_DefaultCustomerOnCase.Name;
                    Endeavor.formscriptfunctions.SetLookup("customerid", _logicalname, _id, _name, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setCustomerOnLoad_callback\n\n" + e.Message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Show facebook or chat section
        setVisibilityOnLoad: function (formContext) {
            try {
                //formContext.getControl("description").setFocus();

                var _origin = Endeavor.formscriptfunctions.GetValue("caseorigincode", formContext);
                if (_origin != null) {

                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_chatid", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", false, formContext);

                    if (_origin == 285050001) {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_chatid", true, formContext);
                    }
                    else if (_origin == 285050000) {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", true, formContext);
                    }
                }

            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setVisibilityOnLoad\n\n" + e.Message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        getStateofCase: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                return Endeavor.formscriptfunctions.GetValue("statecode", formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setVisibilityOnLoad\n\n" + e.Message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        category2_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _attribute = formContext.getEventSource();
                var _fieldName = _attribute.getName();
                var _case_category = formContext.getControl(_fieldName);

                var _split = _fieldName.split("_");
                var _row = _split[2];
                var _col = _split[3];

                _category2_onchange_rownr = _row.substring(3, 4)
                var lookupObject = formContext.getAttribute(_fieldName);
                if (lookupObject != null) {
                    var lookUpObjectValue = lookupObject.getValue();
                    if ((lookUpObjectValue != null)) {
                        var _categoryid = formContext.getAttribute(_fieldName).getValue()[0].id;
                        Endeavor.OData_Querys.GetParentCategory(_categoryid, formContext);
                    }
                    else {
                        var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat2id";
                        formContext.getAttribute(_update_fieldname).setValue(null);
                    }
                }
                else {
                    alert("F�ltet finns inte. Hur kan du �ndra i det?");
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category2_onchange\n\n" + e.Message);
            }
        },

        category2_onchange_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Hittar ingen kategori 2");
                }
                else {
                    var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat1id";
                    //alert("Uppdatera detta f�lt " + _update_fieldname);
                    var _id = result[0].cgi_Parentid.Id;
                    var _logicalname = result[0].cgi_Parentid.LogicalName;
                    var _name = result[0].cgi_Parentid.Name;
                    Endeavor.formscriptfunctions.SetLookup(_update_fieldname, _logicalname, _id, _name, formContext);

                    //Always clear category 3 then category 2 is changed
                    var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat3id";
                    formContext.getAttribute(_update_fieldname).setValue(null);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category2_onchange_callback\n\n" + e.Message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        category3_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _attribute = formContext.getEventSource();
                var _fieldName = _attribute.getName();
                Endeavor.Skanetrafiken.Incident.category3_onchange_nocontext(_fieldName, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category3_onchange\n\n" + e.Message);
            }
        },

        category3_onchange_nocontext: function (_fieldName, formContext) {
            try {
                var _case_category = formContext.getControl(_fieldName);

                var _split = _fieldName.split("_");
                var _row = _split[2];
                _category3_onchange_rownr = _row.substring(3, 4)

                var lookupObject = formContext.getAttribute(_fieldName);


                if (lookupObject != null) {
                    var lookUpObjectValue = lookupObject.getValue();

                    if ((lookUpObjectValue != null)) {
                        if (_row == "row2") {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true, formContext);
                        }
                        if (_row == "row3") {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true, formContext);
                        }

                        var _categoryid = formContext.getAttribute(_fieldName).getValue()[0].id;
                        Endeavor.OData_Querys.GetParentCategory3(_categoryid, formContext);
                    }
                    else {
                        var _update_fieldname_cat1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                        formContext.getAttribute(_update_fieldname_cat1).setValue(null);
                        var _update_fieldname_cat2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                        formContext.getAttribute(_update_fieldname_cat2).setValue(null);

                        Endeavor.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat1, formContext);
                        Endeavor.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat2, formContext);
                    }
                }
                else {
                    alert("F�ltet finns inte. Hur kan du �ndra i det?");
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category3_onchange_nocontext\n\n" + e.Message);
            }
        },

        category3_onchange_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Hittar ingen kategori 3");
                }
                else {

                    var _update_fieldname1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                    var _id = result[0].cgi_Parentid.Id;
                    var _logicalname = result[0].cgi_Parentid.LogicalName;
                    var _name = result[0].cgi_Parentid.Name;
                    Endeavor.formscriptfunctions.SetLookup(_update_fieldname1, _logicalname, _id, _name, formContext);

                    Endeavor.formscriptfunctions.SetSubmitModeAlways(_update_fieldname1, formContext);

                    var _update_fieldname2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                    var _id = result[0].cgi_parentid2.Id;
                    var _logicalname = result[0].cgi_parentid2.LogicalName;
                    var _name = result[0].cgi_parentid2.Name;
                    Endeavor.formscriptfunctions.SetLookup(_update_fieldname2, _logicalname, _id, _name, formContext);


                    Endeavor.formscriptfunctions.SetSubmitModeAlways(_update_fieldname2, formContext);

                    //Endeavor.Skanetrafiken.Incident.category3_onchange_get_cat1(_id);

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category3_onchange_callback\n\n" + e.Message);
            }
        },
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        onChangeHandelseDatum: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var handelsedatum = formContext.getAttribute("cgi_handelsedatum").getValue();

                //var handelsedatum = "15-01/03 15g27";
                handelsedatum = handelsedatum.replace(/\D/g, '');

                if (handelsedatum.length == 12 || handelsedatum.length == 10 || handelsedatum.length == 8 || handelsedatum.length == 6) {
                    dto = new Date();

                    year = month = day = hour = minute = 0;

                    if (handelsedatum.length == 12 || handelsedatum.length == 8)
                        centAdd = 2;
                    else
                        centAdd = 0;

                    yearStr = handelsedatum.substring(0, 2 + centAdd);
                    monthStr = handelsedatum.substring(2 + centAdd, 4 + centAdd);
                    dayStr = handelsedatum.substring(4 + centAdd, 6 + centAdd);

                    if (centAdd == 0)
                        year = 2000 + parseInt(yearStr);
                    else
                        year = parseInt(yearStr);

                    month = parseInt(monthStr) - 1;
                    day = parseInt(dayStr);
                    dto.setFullYear(year);
                    dto.setMonth(month);
                    dto.setDate(day);

                    if (handelsedatum.length == 12 || handelsedatum.length == 10) {
                        hourStr = handelsedatum.substring(6 + centAdd, 8 + centAdd);
                        minuteStr = handelsedatum.substring(8 + centAdd, 10 + centAdd);
                        hour = parseInt(hourStr);
                        minute = parseInt(minuteStr);
                        dto.setHours(hour);
                        dto.setMinutes(minute);
                        dto.setSeconds(0);
                    }
                    else {
                        dto.setHours(0);
                        dto.setMinutes(0);
                        dto.setSeconds(0);
                    }
                    formContext.getAttribute("cgi_actiondate").setValue(dto);
                }
            } catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.onChangeHandelseDatum\n\n" + e.Message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // function not used. Commented out
        //  Endeavor.OData_Querys.GetParentCategory2 - does not exist
        category3_onchange_get_cat1: function (_id) {
            try {
                Endeavor.OData_Querys.GetParentCategory2(_id, Endeavor.Skanetrafiken.Incident.category3_sub_onchange_callback, Endeavor.Skanetrafiken.Incident.category3_sub_onchange_completed);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category3_onchange_get_cat1\n\n" + e.Message);
            }
        },

        category3_sub_onchange_completed: function () { },

        category3_sub_onchange_callback: function (result) {
            try {
                if (result == null || result[0] == null) {
                    alert("Hittar ingen kategori 2 i subrutinen");
                }
                else {
                    var _update_fieldname_cat1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                    var _id = result[0].cgi_parentid2.Id;
                    var _logicalname = result[0].cgi_parentid2.LogicalName;
                    var _name = result[0].cgi_parentid2.Name;

                    Endeavor.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat1);

                    Endeavor.formscriptfunctions.SetLookup(_update_fieldname_cat1, _logicalname, _id, _name);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.category3_sub_onchange_callback\n\n" + e.Message);
            }
        },
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        letter_template_onchange: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                var _id = Endeavor.formscriptfunctions.GetLookupid("cgi_letter_templateid", formContext);

                if (_id != null) {
                    Endeavor.OData_Querys.GetLetterTemplate(_id, formContext);
                }
                else {
                    Endeavor.formscriptfunctions.SetValue("cgi_letter_title", "", formContext);
                    Endeavor.formscriptfunctions.SetValue("cgi_letter_body", "", formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.letter_template_onchange\n\n" + e.Message);
            }
        },

        letter_template_onchange_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Hittar inte brevmallen.");
                }
                else {
                    var _title = result[0].cgi_title;
                    var _letter_body = result[0].cgi_template_body;

                    formContext.getAttribute("cgi_letter_title").setValue(_title);
                    formContext.getAttribute("cgi_letter_body").setValue(_letter_body);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.letter_template_onchange_callback\n\n" + e.Message);
            }
        },
        //Körs istället för affärsregel. Observera att funktionen även sätter nivå 2 och nivå 1
        casetypecode_onchange: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var cgi_casdet_row1_cat3id = formContext.getAttribute('cgi_casdet_row1_cat3id').getValue();
            if (cgi_casdet_row1_cat3id == null) {
                var casetypecode = formContext.getAttribute('casetypecode').getValue();
                if (casetypecode == 285050003) {//travel waranty 
                    try {
                        var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                        Endeavor.OData_Querys.GetDefaultCaseCategory3Setting(_currentdate, formContext);
                    }
                    catch (e) {
                        alert("Fel i Endeavor.Skanetrafiken.Incident.casetypecode_onchange\n\n" + e.Message);
                    }
                }
            }


        },


        getRGOLapiurl: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetRGOLUrlFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.IncidentLibrary.getRGOLapiurl\n\n" + e.Message);
            }
        },

        getRGOLapiurl_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen url definierad för RGOL!");
                }
                else {

                    var _url = result[0].cgi_rgolurl;
                    var rgolId = formContext.getAttribute("cgi_rgolissueid").getValue();
                    var rgolPath = "https://" + _url + "/web/index.html?data=issueId%3D" + rgolId + "%26environment%3D" + _url;


                    if (formContext.ui.tabs.get("rgol_info_new").getDisplayState() == "expanded") {
                        formContext.ui.controls.get("IFRAME_RGOLinfo_new").setSrc(rgolPath);

                    }

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.IncidentLibrary.getRGOLapiurl\n\n" + e.Message);
            }
        },

        //laddar rgolinfo i en iframe
        rgoliframe_onchange: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Incident.getRGOLapiurl(formContext);
        },

        getRGOLapiurlNew: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetRGOLUrlFromSettingNew(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.IncidentLibrary.getRGOLapiurl\n\n" + e.Message);
            }
        },

        getRGOLapiurl_callbackNew: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen url definierad för RGOL!");
                }
                else {

                    var _url = result[0].cgi_rgolurl;
                    var splitUrl = _url.split("/");
                    _url = splitUrl[0];

                    var rgolId = formContext.getAttribute("cgi_rgolissueid").getValue();
                    //var rgolPath = "http://" + _url + "/web/index.html?data=issueId%3D" + rgolId + "%26environment%3D" + _url;
                    var rgolPath = "https://" + _url + "/Pages/IssueSimple.aspx?id=" + rgolId;


                    if (formContext.ui.tabs.get("rgol_info_new").getDisplayState() == "expanded") {
                        formContext.ui.controls.get("IFRAME_RGOLinfo_new").setSrc(rgolPath);

                    }

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.IncidentLibrary.getRGOLapiurl\n\n" + e.Message);
            }
        },

        rgoliframe_onchange_new: function (executionContext) {
            var formContext = executionContext.getFormContext();

            Endeavor.Skanetrafiken.Incident.getRGOLapiurlNew(formContext);
        },

        casetypecode_onchange_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen default kategori 3 definerad!");
                }
                else {
                    var _id = result[0].cgi_category_detail3id.Id;
                    var _logicalname = result[0].cgi_category_detail3id.LogicalName;
                    var _name = result[0].cgi_category_detail3id.Name;
                    Endeavor.formscriptfunctions.SetLookup("cgi_casdet_row1_cat3id", _logicalname, _id, _name, formContext);
                    Endeavor.Skanetrafiken.Incident.category3_onchange_nocontext("cgi_casdet_row1_cat3id", formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.casetypecode_onchange_callback\n\n" + e.Message);
            }
        },

        /*
         * 
         * CGI Incident Ribbon (From incidentRibbon.js)
         * 
         */

        trafficInfoExists: function () {

        },

        resolveCase: function (caseid, primaryControl) {
            try {
                var formContext = primaryControl;
                if (Endeavor.formscriptfunctions.MandatoryPopulated(formContext) == false) {
                    alert("Ett eller flera obligatoriska fält saknar värde!");
                    return;
                }

                //START validera trafikinfo START
                //fortsätt valideringen endast ifall användaren INTE explicit angivit att ingen trafikinformation ska registreras
                if (formContext.getAttribute("cgi_notravelinfo").getValue() != 1) {

                    //fortsätt med valideringen endast om trafikinfo saknas
                    Endeavor.OData_Querys.GetTravelInfoForCase(caseid, formContext);
                }
                //END validera trafikinfo END


                if (Endeavor.formscriptfunctions.GetDisabledField("cgi_casesolved", formContext) == true) {
                    alert("Du saknar behörighet att avsluta detta ärende!");
                    return;
                }


                var _incidentstagecode = Endeavor.formscriptfunctions.GetValue("incidentstagecode", formContext);

                if (_incidentstagecode != 285050002) {
                    formContext.getAttribute("incidentstagecode").setValue(285050004);
                    Endeavor.formscriptfunctions.SetValue("cgi_case_reopen", "1", formContext);
                    Endeavor.formscriptfunctions.SetValue("cgi_casesolved", "2", formContext);
                    Endeavor.formscriptfunctions.SaveAndCloseEntity(formContext);
                }
                else {
                    if (confirm("Ärendet har status Obesvarad kund. Vill du avsluta ärendet ändå?") == true) {
                        formContext.getAttribute("incidentstagecode").setValue(285050004);
                        Endeavor.formscriptfunctions.SetValue("cgi_case_reopen", "1", formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_casesolved", "2", formContext);
                        Endeavor.formscriptfunctions.SaveAndCloseEntity(formContext);
                    }
                }


            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.resolveCase\n\n" + e.Message);
            }
        },

        sendAttentionEmail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (Endeavor.Skanetrafiken.Incident.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan mail skickas!");
                    return;
                }

                var parameters = Endeavor.Skanetrafiken.Incident.setArgs(formContext);
                //type of email
                parameters += "&cgi_attention=true";

                // Open the window.
                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(parameters);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);

            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.sendAttentionEmail\n\n" + e.Message);
            }
        },

        sendRemitteceEmail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (Endeavor.Skanetrafiken.Incident.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan mail skickas!");
                    return;
                }

                var parameters = Endeavor.Skanetrafiken.Incident.setArgs(formContext);
                //type of email
                parameters += "&cgi_remittance=true";

                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(parameters);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);

            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.sendRemitteceEmail\n\n" + e.Message);
            }
        },

        sendCustomermail: function (primaryControl) {
            try {
                var formContext = primaryControl;
                var globalContext = Xrm.Utility.getGlobalContext();

                if (Endeavor.Skanetrafiken.Incident.chechIfAnyFieldIsDirty(formContext) == true) {
                    alert("Spara ärendet innan e-post kan skickas!");
                    return;
                }


                var argValues = "";
                argValues += Endeavor.Skanetrafiken.Incident.setArgs(formContext);
                if (Endeavor.formscriptfunctions.GetValue("cgi_accountid", formContext) == null && Endeavor.formscriptfunctions.GetValue("cgi_contactid", formContext) == null) {
                    alert("Ingen kund är kopplad till ärendet!");
                    return;
                }
                else {
                    argValues += "&" + Endeavor.Skanetrafiken.Incident.setArgsCustomer(formContext);
                    //type of email
                    argValues += "&cgi_button_customer=1";
                }

                //if (Endeavor.Skanetrafiken.Incident.validateEmail() == false) {
                //    return;
                //}

                debugger;
                var hasRep = null;
                var cgi_representativ = formContext.getAttribute("cgi_representativid").getValue();

                if (cgi_representativ !== null) {
                    hasRep = Endeavor.Skanetrafiken.Incident.hasRepresentativEmail(cgi_representativ, formContext);

                    if (hasRep == false) {
                        return;
                    }
                }


                if (cgi_representativ == null)
                    if (Endeavor.Skanetrafiken.Incident.hasContactEmail(formContext) == false)
                        return;

                // Open the window.
                var url = "/main.aspx?etn=email&pagetype=entityrecord";
                var features = "location=no,menubar=no,status=no,toolbar=no,resizable=1";

                url = globalContext.userSettings.prependOrgName(url);
                url = url + "&extraqs=" + encodeURIComponent(argValues);
                url = url + "&histKey=" + Math.floor(Math.random() * 10000) + "&newWindow=true"
                window.open(url, "_blank", features, false);
            }
            catch (e) {
                alert("Ett fel inträffade i Endeavor.Skanetrafiken.Incident.sendCustomermail\n\n" + e.Message);
            }
        },

        hasContactEmail: function (formContext) {
            try {
                var contact = formContext.getAttribute("cgi_contactid").getValue();
                if (contact == null)
                    return;

                var contactId = contact[0].id;

                Xrm.WebApi.retrieveRecord("contact", contactId, "?$select=emailaddress1,emailaddress2,yomifullname")
                    .then(function (result) {
                        if (result.EMailAddress1 == null && result.EMailAddress2 == null) {
                            formContext.ui.setFormNotification(result.YomiFullName + " har ingen e-post kopplad till sig.", "Info", "1");
                            return false;
                        }
                        formContext.ui.clearFormNotification("1");
                        return true;
                    });
            }
            catch (e) {
                alert("Ett fel inträffade i Endeavor.Skanetrafiken.Incident.hasContactEmail\n\n" + e.Message);
            }

        },

        hasRepresentativEmail: function (cgi_representativ, formContext) {
            try {
                var cgi_representativId = cgi_representativ[0].id;

                Xrm.WebApi.retrieveRecord("cgi_representative", cgi_representativId, "?$select=cgi_name,emailaddress")
                    .then(function (result) {
                        if (result.EmailAddress == null) {
                            formContext.ui.setFormNotification(userRepresentativ.cgi_name + " har ingen e-post kopplad till sig.", "Info", "1");
                            return false;
                        }
                        formContext.ui.clearFormNotification("1");
                        return true;
                    });
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.hasRepresentativEmail\n\n" + e.Message);
            }
        },

        setParameters: function (primaryControl) {
            var _returnValue = {};
            try {
                var formContext = primaryControl;
                //regardingobjectid
                _returnValue["parameter_regardingid"] = Endeavor.formscriptfunctions.GetObjectID(formContext);
                _returnValue["parameter_regardingname"] = Endeavor.formscriptfunctions.GetValue("title", formContext);
                _returnValue["parameter_regardingtype"] = "incident";
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setParameters\n\n" + e.Message);
            }
            return _returnValue;
        },

        setArgs: function (formContext) {
            try {
                var _returnValue = "parameter_regardingid=" + Endeavor.formscriptfunctions.GetObjectID(formContext);
                var title = Endeavor.formscriptfunctions.GetValue("title", formContext);
                debugger;
                title = title.replace("%", " procent");
                _returnValue += "&parameter_regardingname=" + title;
                _returnValue += "&parameter_regardingtype=incident";

            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setArgs\n\n" + e.Message);
            }
            return _returnValue;
        },

        setArgsCustomer: function (formContext) {
            try {
                var parameterValue = "";
                var _cgi_accountid = Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext),
                    _representative = Endeavor.formscriptfunctions.GetLookupid("cgi_representativid", formContext);

                // If the customer as a representative all communication should go through the representative.
                if (_representative != null) {
                    parameterValue += "parameter_customerid=" + Endeavor.formscriptfunctions.GetLookupid("cgi_representativid", formContext);
                    parameterValue += "&parameter_customername=" + Endeavor.formscriptfunctions.GetLookupName("cgi_representativid", formContext);
                    parameterValue += "&parameter_customertype=cgi_representative";
                }
                else if (_cgi_accountid != null) {
                    //cgi_accountid
                    parameterValue += "parameter_customerid=" + Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                    parameterValue += "&parameter_customername=" + Endeavor.formscriptfunctions.GetLookupName("cgi_accountid", formContext);
                    parameterValue += "&parameter_customertype=account";
                }
                else {
                    parameterValue += "parameter_customerid=" + Endeavor.formscriptfunctions.GetLookupid("cgi_contactid", formContext);
                    parameterValue += "&parameter_customername=" + Endeavor.formscriptfunctions.GetLookupName("cgi_contactid", formContext);
                    parameterValue += "&parameter_customertype=contact";
                }

                return parameterValue;
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setArgsCustomer\n\n" + e.Message);
            }
            return param;
        },

        setParametersCustomer: function (param, primaryControl) {
            try {
                var formContext = primaryControl;

                var _cgi_accountid = Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext),
                    _representative = Endeavor.formscriptfunctions.GetLookupid("cgi_representativid", formContext);

                // If the customer as a representative all communication should go through the representative.
                if (_representative != null) {
                    param["parameter_customerid"] = _representative;
                    param["parameter_customername"] = Endeavor.formscriptfunctions.GetLookupName("cgi_representativid", formContext);
                    param["parameter_customertype"] = "cgi_representative";
                }
                else if (_cgi_accountid != null) {
                    //cgi_accountid
                    param["parameter_customerid"] = Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                    param["parameter_customername"] = Endeavor.formscriptfunctions.GetLookupName("cgi_accountid", formContext);
                    param["parameter_customertype"] = "account";
                }
                else {
                    param["parameter_customerid"] = Endeavor.formscriptfunctions.GetLookupid("cgi_contactid", formContext);
                    param["parameter_customername"] = Endeavor.formscriptfunctions.GetLookupName("cgi_contactid", formContext);
                    param["parameter_customertype"] = "contact";
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.setParameters\n\n" + e.Message);
            }
            return param;
        },

        setParametersUser: function () {
            alert("Namnet på kön " + queueName);
        },

        validateEmail: function (primaryControl) {
            var formContext = primaryControl;

            var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;

            var email = "";
            var email2 = "";
            var cgi_representativid = "";

            try {
                email = Endeavor.formscriptfunctions.GetValue("cgi_customer_email", formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.validateEmail:1\n\n" + e.Message);
                return false;
            }

            try {
                cgi_representativid = Endeavor.formscriptfunctions.GetLookupid("cgi_representativid", formContext);
            }
            catch (e) {

            }

            if (cgi_representativid != null && cgi_representativid != "") {
                try {
                    Xrm.WebApi.retrieveRecord("cgi_representativ", cgi_representativid, "?$select=cgi_email")
                        .then(function (result) {
                            var cgi_representative = result.results[0];
                            if (cgi_representative["cgi_Email"] == null || cgi_representative["cgi_Email"] == "")
                                email2 = cgi_representative["cgi_Email"];

                        });
                }
                catch (e) {
                    alert("Fel i Endeavor.Skanetrafiken.Incident.validateEmail:2\n\n" + e.Message);
                }
            }

            if (re.test(email) || re.test(email2)) {
                return false;
            }

            return true;
        },

        openBombApp: function (primaryControl) {
            try {
                var formContext = primaryControl;

                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetBOMBUrlFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.openBombApp\n\n" + e.Message);
            }
        },

        openBombApp_callback: function (result, formContext) {
            try {
                if (result == null || result[0] == null) {
                    alert("Det finns ingen url definierad för BOMB!");
                }
                else {
                    var _features = "status=1,toolbar=1,location=1,menubar=1,directories=1,resizable=1,scrollbars=1";
                    var _url = result[0].cgi_BOMBUrl;
                    var _param = Endeavor.formscriptfunctions.GetValue("cgi_bombmobilenumber", formContext)
                    if (_param == null) {
                        alert("Inget mobilnummer är angivet!");
                        return;
                    }

                    var _openurl = _url + _param;
                    window.open(_openurl, "_blank", _features, true);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.openBombApp_callback\n\n" + e.Message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        chechIfAnyFieldIsDirty: function (formContext) {
            try {
                return formContext.data.entity.getIsDirty();
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.Incident.chechIfAnyFieldIsDirty\n\n" + e.Message);
            }
        }

    };
}