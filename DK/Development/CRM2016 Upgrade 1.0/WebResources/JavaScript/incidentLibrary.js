
if (typeof (CGISweden) == "undefined") { CGISweden = {}; }

// *******************************************************
// Entity: incident 
// *******************************************************

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
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

CGISweden.incident =
{

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


        CGISweden.incident.setVisibilityOnLoad(formContext);

        switch (formContext.ui.getFormType()) {
            case FORM_TYPE_CREATE:
                CGISweden.incident.setCustomerOnLoad(formContext);
                CGISweden.incident.setDefaultOnCreate(formContext);
                //CGISweden.incident.setAccountOrContactVisibility(formContext);
                break;
            case FORM_TYPE_UPDATE:
                CGISweden.incident.setOnUpdate(formContext);
            case FORM_TYPE_READONLY:
            case FORM_TYPE_DISABLED:
                CGISweden.incident.onLoad(formContext);
                //CGISweden.incident.setAccountOrContactVisibility(formContext);
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

        var _case_stage = CGISweden.formscriptfunctions.GetValue("incidentstagecode", formContext);
        if (_case_stage == 1 || _case_stage == 285050003) {
            formContext.getAttribute("incidentstagecode").setValue(285050000);
        }

        var _cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();
        var _cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

        if (_cgi_accountid != null) {
            CGISweden.incident.setCustomerFromAccount();
        }

        if (_cgi_contactid != null) {
            CGISweden.incident.setCustomerFromContact();
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
            setTimeout(CGISweden.incident.hideOrShowTypeOfContactFields, TIMEOUT_COUNTER);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onLoadHideShowTypeOfContactFields\n\n" + e.Message);
        }
    },

    onLoad: function (formContext) {
        try {
            setTimeout(CGISweden.incident.timerfunction_BIFF(formContext), TIMEOUT_COUNTER);
            //setTimeout(CGISweden.incident.timerfunction_Travel(formContext), TIMEOUT_COUNTER);
            //setTimeout(CGISweden.incident.hideOrShowTypeOfContactFields, TIMEOUT_COUNTER);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onLoad\n\n" + e.Message);
        }
    },

    onSendToQueue: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.incident.onSave();
        CGISweden.formscriptfunctions.SaveAndCloseEntity(formContext);
    },

    setAccountOrContactVisibility: function (formContext) {
        try {
            var _cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();
            var _cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

            if (_cgi_accountid == null && _cgi_contactid == null) {
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
            }

            if (_cgi_accountid != null && _cgi_contactid == null) {
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", false, formContext);
            }

            if (_cgi_accountid == null && _cgi_contactid != null) {
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
            }

        } catch (e) {
            alert("Fel i CGISweden.incident.setAccountOrContactVisibility\n\n" + e.Message);
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
            alert("Fel i CGISweden.incident.onChangeOrigin\n\n" + e.Message);
        }
    },

    onChangeAccount: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var __cgi_accountid = formContext.getAttribute("cgi_accountid").getValue();

            if (__cgi_accountid != null) {
                //CGISweden.formscriptfunctions.SetValue("cgi_contactid", null);
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", false);
            }
            else {
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true);
            }

            var __customerid = formContext.getAttribute("customerid").getValue();
            //formContext.getAttribute("cgi_contactid").setValue(null);

            if (__cgi_accountid != null) {
                //if (__customerid != null) {
                CGISweden.incident.setCustomerFromAccount(formContext);
                //}
            }

            if (__cgi_accountid == null) {
                CGISweden.incident.setCustomerOnLoad(formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onChangeAccount\n\n" + e.Message);
        }
    },

    onChangeContact: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var __cgi_contactid = formContext.getAttribute("cgi_contactid").getValue();

            if (__cgi_contactid != null) {
                //CGISweden.formscriptfunctions.SetValue("cgi_accountid", null, formContext);
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                CGISweden.incident.hideOrShowTypeOfContactFields(formContext);

            }
            else {
                //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
            }

            var __customerid = formContext.getAttribute("customerid").getValue();
            //formContext.getAttribute("cgi_accountid").setValue(null);

            if (__cgi_contactid != null) {
                //if (__customerid != null) {
                CGISweden.incident.setCustomerFromContact(formContext);

                //}
            }

            if (__cgi_contactid == null) {
                CGISweden.incident.setCustomerOnLoad(formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onChangeAccount\n\n" + e.Message);
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
                var travelcardid = CGISweden.formscriptfunctions.GetValue("cgi_unregisterdtravelcard", formContext);
                obj.contentWindow.SetTravelCardNumber(travelcardid);
            }
        } catch (e) {
            alert("Fel i CGISweden.incident.onChangeTravelCard\n\n" + e.Message);
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
                                    setTimeout(CGISweden.incident.hideOrShowTypeOfContactFields, 300);
                                }
                            } else {
                                setTimeout(CGISweden.incident.hideOrShowTypeOfContactFields, 300);
                            }
                        }
                    } else {
                        console.log("Could not fetch values. Contact Admin.");
                    }
                }
            }

        } catch (e) {
            alert("Exception caught in CGISweden.incident.hideOrShowTypeOfContactFields. Error: " + e.message);
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
                setTimeout(CGISweden.incident.timerfunction_BIFF(formContext), TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.timerfunction_BIFF\n\n" + e.Message);
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
                setTimeout(CGISweden.incident.timerfunction_Travel(formContext), TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.timerfunction_Travel\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set customer to account attribute
    setCustomerFromAccount: function (formContext) {
        var __customerid = formContext.getAttribute("customerid").getValue();
        var _cgi_accountid_logicalname = "account" //formContext.getAttribute("cgi_accountid").getValue()[0].logicalname;
        var _cgi_accountid_id = formContext.getAttribute("cgi_accountid").getValue()[0].id;
        var _cgi_accountid_name = formContext.getAttribute("cgi_accountid").getValue()[0].name;
        CGISweden.formscriptfunctions.SetLookup("customerid", _cgi_accountid_logicalname, _cgi_accountid_id, _cgi_accountid_name, formContext);
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set customer to contact attribute
    setCustomerFromContact: function (formContext) {
        var __customerid = formContext.getAttribute("customerid").getValue();
        var _cgi_contactid_logicalname = "contact"; //formContext.getAttribute("cgi_contactid").getValue()[0].logicalname;
        var _cgi_contactid_id = formContext.getAttribute("cgi_contactid").getValue()[0].id;
        var _cgi_contactid_name = formContext.getAttribute("cgi_contactid").getValue()[0].name;

        CGISweden.formscriptfunctions.SetLookup("customerid", _cgi_contactid_logicalname, _cgi_contactid_id, _cgi_contactid_name, formContext);

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set default values on create
    setDefaultOnCreate: function (formContext) {
        try {
            var _currentdate = new Date();
            formContext.getAttribute("cgi_arrival_date").setValue(_currentdate);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCaseDefaultOnLoad\n\n" + e.Message);
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
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true, formContext);
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
                                alert("Fel i CGISweden.incident.setOnUpdate\n\n" + p1);
                                console.log("fail p1 " + p1 + " fail p2 " + p2 + "fail p3 " + p3);
                            });

                    },
                    function (p1, p2, p3) {
                        alert("Fel i CGISweden.incident.setOnUpdate\n\n" + p1);
                        console.log("fail p1 " + p1 + " fail p2 " + p2 + "fail p3 " + p3);
                    }
                );

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setOnUpdate\n\n" + e.Message);
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
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetDefaultCustomerFromSetting(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCustomerOnLoad\n\n" + e.Message);
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
                CGISweden.formscriptfunctions.SetLookup("customerid", _logicalname, _id, _name, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCustomerOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Show facebook or chat section
    setVisibilityOnLoad: function (formContext) {
        try {
            //formContext.getControl("description").setFocus();

            var _origin = CGISweden.formscriptfunctions.GetValue("caseorigincode", formContext);
            if (_origin != null) {

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_chatid", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", false, formContext);

                if (_origin == 285050001) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_chatid", true, formContext);
                }
                else if (_origin == 285050000) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", true, formContext);
                }
            }

        }
        catch (e) {
            alert("Fel i CGISweden.incident.setVisibilityOnLoad\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    getStateofCase: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            return CGISweden.formscriptfunctions.GetValue("statecode", formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setVisibilityOnLoad\n\n" + e.Message);
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
                    CGISweden.odata.GetParentCategory(_categoryid, formContext);
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
            alert("Fel i CGISweden.incident.category2_onchange\n\n" + e.Message);
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
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname, _logicalname, _id, _name, formContext);

                //Always clear category 3 then category 2 is changed
                var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat3id";
                formContext.getAttribute(_update_fieldname).setValue(null);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category2_onchange_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    category3_onchange: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var _attribute = formContext.getEventSource();
            var _fieldName = _attribute.getName();
            CGISweden.incident.category3_onchange_nocontext(_fieldName, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange\n\n" + e.Message);
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
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true, formContext);
                    }
                    if (_row == "row3") {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true, formContext);
                    }

                    var _categoryid = formContext.getAttribute(_fieldName).getValue()[0].id;
                    CGISweden.odata.GetParentCategory3(_categoryid, formContext);
                }
                else {
                    var _update_fieldname_cat1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                    formContext.getAttribute(_update_fieldname_cat1).setValue(null);
                    var _update_fieldname_cat2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                    formContext.getAttribute(_update_fieldname_cat2).setValue(null);

                    CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat1, formContext);
                    CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat2, formContext);
                }
            }
            else {
                alert("F�ltet finns inte. Hur kan du �ndra i det?");
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange_nocontext\n\n" + e.Message);
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
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname1, _logicalname, _id, _name, formContext);

                CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname1, formContext);

                var _update_fieldname2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                var _id = result[0].cgi_parentid2.Id;
                var _logicalname = result[0].cgi_parentid2.LogicalName;
                var _name = result[0].cgi_parentid2.Name;
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname2, _logicalname, _id, _name, formContext);


                CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname2, formContext);

                //CGISweden.incident.category3_onchange_get_cat1(_id);

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange_callback\n\n" + e.Message);
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
            alert("Fel i CGISweden.incident.onChangeHandelseDatum\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // function not used. Commented out
    //  CGISweden.odata.GetParentCategory2 - does not exist
    category3_onchange_get_cat1: function (_id) {
        try {
            CGISweden.odata.GetParentCategory2(_id, CGISweden.incident.category3_sub_onchange_callback, CGISweden.incident.category3_sub_onchange_completed);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange_get_cat1\n\n" + e.Message);
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

                CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat1);

                CGISweden.formscriptfunctions.SetLookup(_update_fieldname_cat1, _logicalname, _id, _name);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_sub_onchange_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    letter_template_onchange: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var _id = CGISweden.formscriptfunctions.GetLookupid("cgi_letter_templateid", formContext);

            if (_id != null) {
                CGISweden.odata.GetLetterTemplate(_id, formContext);
            }
            else {
                CGISweden.formscriptfunctions.SetValue("cgi_letter_title", "", formContext);
                CGISweden.formscriptfunctions.SetValue("cgi_letter_body", "", formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.letter_template_onchange\n\n" + e.Message);
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
            alert("Fel i CGISweden.incident.letter_template_onchange_callback\n\n" + e.Message);
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
                    var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
                    CGISweden.odata.GetDefaultCaseCategory3Setting(_currentdate, formContext);
                }
                catch (e) {
                    alert("Fel i CGISweden.incident.casetypecode_onchange\n\n" + e.Message);
                }
            }
        }


    },

    
    getRGOLapiurl: function (formContext) {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetRGOLUrlFromSetting(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
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
                var rgolPath = "http://" + _url + "/web/index.html?data=issueId%3D" + rgolId + "%26environment%3D" + _url;


                if (formContext.ui.tabs.get("rgol_info_new").getDisplayState() == "expanded") {
                    formContext.ui.controls.get("IFRAME_RGOLinfo_new").setSrc(rgolPath);

                }

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    //laddar rgolinfo i en iframe
    rgoliframe_onchange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.incident.getRGOLapiurl(formContext);
    },

    getRGOLapiurlNew: function (formContext) {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetRGOLUrlFromSettingNew(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
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
                var rgolPath = "http://" + _url + "/Pages/IssueSimple.aspx?id=" + rgolId;


                if (formContext.ui.tabs.get("rgol_info_new").getDisplayState() == "expanded") {
                    formContext.ui.controls.get("IFRAME_RGOLinfo_new").setSrc(rgolPath);

                }

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    rgoliframe_onchange_new: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.incident.getRGOLapiurlNew(formContext);
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
                CGISweden.formscriptfunctions.SetLookup("cgi_casdet_row1_cat3id", _logicalname, _id, _name, formContext);
                CGISweden.incident.category3_onchange_nocontext("cgi_casdet_row1_cat3id", formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.casetypecode_onchange_callback\n\n" + e.Message);
        }
    },

};