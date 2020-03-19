if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }

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

    onFormLoad: function () {

        CGISweden.incident.setVisibilityOnLoad();

        switch (Xrm.Page.ui.getFormType()) {
            case FORM_TYPE_CREATE:
                CGISweden.incident.setCustomerOnLoad();
                CGISweden.incident.setDefaultOnCreate();
                CGISweden.incident.setAccountOrContactVisibility();
                break;
            case FORM_TYPE_UPDATE:
                CGISweden.incident.setOnUpdate();
            case FORM_TYPE_READONLY:
            case FORM_TYPE_DISABLED:
                CGISweden.incident.onLoad();
                CGISweden.incident.setAccountOrContactVisibility();
                break;
            case FORM_TYPE_QUICKCREATE:
            case FORM_TYPE_BULKEDIT:
                break;
            default:
                alert("Form type error!");
                break;
        }
    },

    onSave: function (prmContext) {
        var _case_stage = CGISweden.formscriptfunctions.GetValue("incidentstagecode");
        if (_case_stage == 1 || _case_stage == 285050003) {
            Xrm.Page.getAttribute("incidentstagecode").setValue(285050000);
        }

        var _cgi_accountid = Xrm.Page.getAttribute("cgi_accountid").getValue();
        var _cgi_contactid = Xrm.Page.getAttribute("cgi_contactid").getValue();

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

    onLoad: function () {
        try {
            setTimeout(CGISweden.incident.timerfunction_BIFF, TIMEOUT_COUNTER);
            setTimeout(CGISweden.incident.timerfunction_Travel, TIMEOUT_COUNTER);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onLoad\n\n" + e.Message);
        }
    },

    onSendToQueue: function () {
        CGISweden.incident.onSave();
        CGISweden.formscriptfunctions.SaveAndCloseEntity();
    },

    setAccountOrContactVisibility: function () {
        try {
            var _cgi_accountid = Xrm.Page.getAttribute("cgi_accountid").getValue();
            var _cgi_contactid = Xrm.Page.getAttribute("cgi_contactid").getValue();

            if (_cgi_accountid == null && _cgi_contactid == null) {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true);
            }

            if (_cgi_accountid != null && _cgi_contactid == null) {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", false);
            }

            if (_cgi_accountid == null && _cgi_contactid != null) {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true);
            }

        } catch (e) {
            alert("Fel i CGISweden.incident.setAccountOrContactVisibility\n\n" + e.Message);
        }
    },

    onChangeDate: function () {
        var datetime = Xrm.Page.getAttribute("cgi_actiondate").getValue();
    },

    onChangeOrigin: function () {
        try {
            var origin = Xrm.Page.getAttribute("caseorigincode").getValue();
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

            Xrm.Page.getAttribute("prioritycode").setValue(priority);
            Xrm.Page.getAttribute("cgi_runpriorityworkflow").setValue(0);

        } catch (e) {
            alert("Fel i CGISweden.incident.onChangeOrigin\n\n" + e.Message);
        }
    },

    onChangeAccount: function () {
        try {
            var __cgi_accountid = Xrm.Page.getAttribute("cgi_accountid").getValue();

            if (__cgi_accountid != null) {
                CGISweden.formscriptfunctions.SetValue("cgi_contactid", null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", false);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_contactid", true);
            }

            var __customerid = Xrm.Page.getAttribute("customerid").getValue();
            Xrm.Page.getAttribute("cgi_contactid").setValue(null);

            if (__cgi_accountid != null) {
                //if (__customerid != null) {
                CGISweden.incident.setCustomerFromAccount();
                //}
            }

            if (__cgi_accountid == null) {
                CGISweden.incident.setCustomerOnLoad();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onChangeAccount\n\n" + e.Message);
        }
    },

    onChangeContact: function () {
        try {
            var __cgi_contactid = Xrm.Page.getAttribute("cgi_contactid").getValue();

            if (__cgi_contactid != null) {
                CGISweden.formscriptfunctions.SetValue("cgi_accountid", null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
            }

            var __customerid = Xrm.Page.getAttribute("customerid").getValue();
            Xrm.Page.getAttribute("cgi_accountid").setValue(null);

            if (__cgi_contactid != null) {
                //if (__customerid != null) {
                CGISweden.incident.setCustomerFromContact();
                //}
            }

            if (__cgi_contactid == null) {
                CGISweden.incident.setCustomerOnLoad();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.onChangeAccount\n\n" + e.Message);
        }
    },

    onChangeTravelCard: function () {
        try {
            // MaxP 2016-04-28 Har lagt till så att kortnummer inte sätts i BIFF kontrollen när den är kollapsad
            var _tab_biffinformation = Xrm.Page.ui.tabs.get("tab_BIFFInformation").getDisplayState()
            if (_tab_biffinformation == "expanded") {
                var arg = 'WebResource_BIFFTransactions';
                var obj = Xrm.Page.getControl(arg).getObject();
                var travelcardid = CGISweden.formscriptfunctions.GetValue("cgi_unregisterdtravelcard");
                obj.contentWindow.SetTravelCardNumber(travelcardid);
            }
        } catch (e) {
            alert("Fel i CGISweden.incident.onChangeTravelCard\n\n" + e.Message);
        }
    },

    timerfunction_BIFF: function () {
        try {
            var arg = 'WebResource_BIFFTransactions';
            var obj = Xrm.Page.getControl(arg).getObject();
            var entid = Xrm.Page.data.entity.getId();

            try {
                obj.contentWindow.SetID(entid);
            }
            catch (e) {
                setTimeout(CGISweden.incident.timerfunction_BIFF, TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.timerfunction_BIFF\n\n" + e.Message);
        }
    },

    timerfunction_Travel: function () {
        try {
            var arg = 'WebResource_TravelInfo';
            var obj = Xrm.Page.getControl(arg).getObject();
            var entid = Xrm.Page.data.entity.getId();

            try {
                obj.contentWindow.SetID(entid);
            }
            catch (e) {
                setTimeout(CGISweden.incident.timerfunction_Travel, TIMEOUT_COUNTER);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.timerfunction_Travel\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set customer to account attribute
    setCustomerFromAccount: function () {
        var __customerid = Xrm.Page.getAttribute("customerid").getValue();
        var _cgi_accountid_logicalname = "account" //Xrm.Page.getAttribute("cgi_accountid").getValue()[0].logicalname;
        var _cgi_accountid_id = Xrm.Page.getAttribute("cgi_accountid").getValue()[0].id;
        var _cgi_accountid_name = Xrm.Page.getAttribute("cgi_accountid").getValue()[0].name;
        CGISweden.formscriptfunctions.SetLookup("customerid", _cgi_accountid_logicalname, _cgi_accountid_id, _cgi_accountid_name);
    },


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set customer to contact attribute
    setCustomerFromContact: function () {
        var __customerid = Xrm.Page.getAttribute("customerid").getValue();
        var _cgi_contactid_logicalname = "contact" //Xrm.Page.getAttribute("cgi_contactid").getValue()[0].logicalname;
        var _cgi_contactid_id = Xrm.Page.getAttribute("cgi_contactid").getValue()[0].id;
        var _cgi_contactid_name = Xrm.Page.getAttribute("cgi_contactid").getValue()[0].name;

        CGISweden.formscriptfunctions.SetLookup("customerid", _cgi_contactid_logicalname, _cgi_contactid_id, _cgi_contactid_name);

    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set default values on create
    setDefaultOnCreate: function () {
        try {
            var _currentdate = new Date();
            Xrm.Page.getAttribute("cgi_arrival_date").setValue(_currentdate);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCaseDefaultOnLoad\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Show fields on update
    setOnUpdate: function () {
        try {
            var lookupObject = Xrm.Page.getAttribute("cgi_casdet_row2_cat3id");

            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();

                if ((lookUpObjectValue != null)) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true);
                }
            }

            var entityId = Xrm.Page.data.entity.getId();
            var currentUserId = Xrm.Page.context.getUserId();
            var entityType = "Incident";

            // We check paas fields, and hidden field to find out if incident has been opened from paas more then once.
            var isPaasIncident = Xrm.Page.getAttribute("cgi_sfn").getValue() !== null;
            // NOTE: should be lowercase name we use in getAttribute
            var cgi_passhasbeenopenedatleastonce_value = Xrm.Page.getAttribute("cgi_passhasbeenopenedatleastonce".toLowerCase()).getValue();
            var isOpenedFromPaasFirstTime = cgi_passhasbeenopenedatleastonce_value === null;

            if (isPaasIncident && isOpenedFromPaasFirstTime) {

                // Function signature:
                //assignRequest: function (Assignee, Target, Type, successCallback, errorCallback)
                SDK.SOAPAssign.assignRequest(currentUserId, entityId, entityType,
                                          function (p1, p2, p3) {

                                              Xrm.Page.data.refresh(false);

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
    toggleTabDisplayState: function (tabName, tabDisplayState, tabIsVisible) {
        //Hide/Show and/or Expand/Collapse tabs     
        var tabs = Xrm.Page.ui.tabs.get();
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
    setCustomerOnLoad: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetDefaultCustomerFromSetting(_currentdate, CGISweden.incident.setCustomerOnLoad_callback, CGISweden.incident.setCustomerOnLoad_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCustomerOnLoad\n\n" + e.Message);
        }
    },

    setCustomerOnLoad_complete: function () { },

    setCustomerOnLoad_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen default kund definerad!");
            }
            else {
                var _id = result[0].cgi_DefaultCustomerOnCase.Id;
                var _logicalname = result[0].cgi_DefaultCustomerOnCase.LogicalName;
                var _name = result[0].cgi_DefaultCustomerOnCase.Name;
                CGISweden.formscriptfunctions.SetLookup("customerid", _logicalname, _id, _name);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setCustomerOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Show facebook or chat section
    setVisibilityOnLoad: function () {
        try {
            //Xrm.Page.getControl("description").setFocus();

            var _origin = CGISweden.formscriptfunctions.GetValue("caseorigincode");
            if (_origin != null) {

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_chatid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", false);

                if (_origin == 285050001) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_chatid", true);
                }
                else if (_origin == 285050000) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_facebookpostid", true);
                }
            }

        }
        catch (e) {
            alert("Fel i CGISweden.incident.setVisibilityOnLoad\n\n" + e.Message);
        }
    },


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    getStateofCase: function () {
        try {
            return CGISweden.formscriptfunctions.GetValue("statecode");
        }
        catch (e) {
            alert("Fel i CGISweden.incident.setVisibilityOnLoad\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    category2_onchange: function (context) {
        try {
            var _attribute = context.getEventSource();
            var _fieldName = _attribute.getName();
            var _case_category = Xrm.Page.getControl(_fieldName);

            var _split = _fieldName.split("_");
            var _row = _split[2];
            var _col = _split[3];

            _category2_onchange_rownr = _row.substring(3, 4)
            var lookupObject = Xrm.Page.getAttribute(_fieldName);
            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();
                if ((lookUpObjectValue != null)) {
                    var _categoryid = Xrm.Page.getAttribute(_fieldName).getValue()[0].id;
                    CGISweden.odata.GetParentCategory(_categoryid, CGISweden.incident.category2_onchange_callback, CGISweden.incident.category2_onchange_completed);
                }
                else {
                    var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat2id";
                    Xrm.Page.getAttribute(_update_fieldname).setValue(null);
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

    category2_onchange_completed: function () { },

    category2_onchange_callback: function (result) {
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
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname, _logicalname, _id, _name);

                //Always clear category 3 then category 2 is changed
                var _update_fieldname = "cgi_casdet_row" + _category2_onchange_rownr + "_cat3id";
                Xrm.Page.getAttribute(_update_fieldname).setValue(null);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category2_onchange_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    category3_onchange: function (context) {
        try {
            var _attribute = context.getEventSource();
            var _fieldName = _attribute.getName();
            CGISweden.incident.category3_onchange_nocontext(_fieldName);
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange\n\n" + e.Message);
        }
    },

    category3_onchange_nocontext: function (_fieldName) {
        try {
            var _case_category = Xrm.Page.getControl(_fieldName);

            var _split = _fieldName.split("_");
            var _row = _split[2];
            _category3_onchange_rownr = _row.substring(3, 4)

            var lookupObject = Xrm.Page.getAttribute(_fieldName);


            if (lookupObject != null) {
                var lookUpObjectValue = lookupObject.getValue();

                if ((lookUpObjectValue != null)) {
                    if (_row == "row2") {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat3id", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat2id", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row3_cat1id", true);
                    }
                    if (_row == "row3") {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat3id", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat2id", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_casdet_row4_cat1id", true);
                    }

                    var _categoryid = Xrm.Page.getAttribute(_fieldName).getValue()[0].id;
                    CGISweden.odata.GetParentCategory(_categoryid, CGISweden.incident.category3_onchange_callback, CGISweden.incident.category3_onchange_completed);
                }
                else {
                    var _update_fieldname_cat1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                    Xrm.Page.getAttribute(_update_fieldname_cat1).setValue(null);
                    var _update_fieldname_cat2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                    Xrm.Page.getAttribute(_update_fieldname_cat2).setValue(null);

                    CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat1);
                    CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname_cat2);
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

    category3_onchange_completed: function () { },

    category3_onchange_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar ingen kategori 3");
            }
            else {

                var _update_fieldname1 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat1id";
                var _id = result[0].cgi_Parentid.Id;
                var _logicalname = result[0].cgi_Parentid.LogicalName;
                var _name = result[0].cgi_Parentid.Name;
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname1, _logicalname, _id, _name);

                CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname1);

                var _update_fieldname2 = "cgi_casdet_row" + _category3_onchange_rownr + "_cat2id";
                var _id = result[0].cgi_parentid2.Id;
                var _logicalname = result[0].cgi_parentid2.LogicalName;
                var _name = result[0].cgi_parentid2.Name;
                CGISweden.formscriptfunctions.SetLookup(_update_fieldname2, _logicalname, _id, _name);


                CGISweden.formscriptfunctions.SetSubmitModeAlways(_update_fieldname2);

                //CGISweden.incident.category3_onchange_get_cat1(_id);

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.category3_onchange_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    onChangeHandelseDatum: function () {
        try {
            var handelsedatum = Xrm.Page.getAttribute("cgi_handelsedatum").getValue();

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
                Xrm.Page.getAttribute("cgi_actiondate").setValue(dto);
            }
        } catch (e) {
            alert("Fel i CGISweden.incident.onChangeHandelseDatum\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
    letter_template_onchange: function () {
        try {
            var _id = CGISweden.formscriptfunctions.GetLookupid("cgi_letter_templateid");

            if (_id != null) {
                CGISweden.odata.GetLetterTemplate(_id, CGISweden.incident.letter_template_onchange_callback, CGISweden.incident.letter_template_onchange_completed);
            }
            else {
                CGISweden.formscriptfunctions.SetValue("cgi_letter_title", "");
                CGISweden.formscriptfunctions.SetValue("cgi_letter_body", "");
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.letter_template_onchange\n\n" + e.Message);
        }
    },

    letter_template_onchange_completed: function () { },

    letter_template_onchange_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inte brevmallen.");
            }
            else {
                var _title = result[0].cgi_title;
                var _letter_body = result[0].cgi_template_body;

                Xrm.Page.getAttribute("cgi_letter_title").setValue(_title);
                Xrm.Page.getAttribute("cgi_letter_body").setValue(_letter_body);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.letter_template_onchange_callback\n\n" + e.Message);
        }
    },
    //Körs istället för affärsregel. Observera att funktionen även sätter nivå 2 och nivå 1
    casetypecode_onchange: function () {

        var cgi_casdet_row1_cat3id = Xrm.Page.getAttribute('cgi_casdet_row1_cat3id').getValue();
        if (cgi_casdet_row1_cat3id == null) {
            var casetypecode = Xrm.Page.getAttribute('casetypecode').getValue();
            if (casetypecode == 285050003) {//travel waranty 
                try {
                    var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
                    CGISweden.odata.GetDefaultCaseCategory3Setting(_currentdate, CGISweden.incident.casetypecode_onchange_callback, CGISweden.incident.casetypecode_onchange_complete);
                }
                catch (e) {
                    alert("Fel i CGISweden.incident.casetypecode_onchange\n\n" + e.Message);
                }
            }
        }


    },


    getRGOLapiurl: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetRGOLUrlFromSetting(_currentdate, CGISweden.incident.getRGOLapiurl_callback, CGISweden.incident.getRGOLapiurl_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    getRGOLapiurl_complete: function () { },

    getRGOLapiurl_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen url definierad för RGOL!");
            }
            else {

                var _url = result[0].cgi_rgolurl;
                var rgolId = Xrm.Page.getAttribute("cgi_rgolissueid").getValue();
                var rgolPath = "https://" + _url + "/web/index.html?data=issueId%3D" + rgolId + "%26environment%3D" + _url;


                if (Xrm.Page.ui.tabs.get("rgol_info").getDisplayState() == "expanded") {
                    Xrm.Page.ui.controls.get("IFRAME_RGOLinfo").setSrc(rgolPath);

                }

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    //laddar rgolinfo i en iframe
    rgoliframe_onchange: function () {
        CGISweden.incident.getRGOLapiurl();
    },

    getRGOLapiurlNew: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetRGOLUrlFromSetting(_currentdate, CGISweden.incident.getRGOLapiurl_callbackNew, CGISweden.incident.getRGOLapiurl_completeNew);
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    getRGOLapiurl_completeNew: function () { },

    getRGOLapiurl_callbackNew: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen url definierad för RGOL!");
            }
            else {

                var _url = result[0].cgi_rgolurl;
                var splitUrl = _url.split("/");
                _url = splitUrl[0];

                var rgolId = Xrm.Page.getAttribute("cgi_rgolissueid").getValue();
                //var rgolPath = "http://" + _url + "/web/index.html?data=issueId%3D" + rgolId + "%26environment%3D" + _url;
                var rgolPath = "https://" + _url + "/Pages/IssueSimple.aspx?id=" + rgolId;


                if (Xrm.Page.ui.tabs.get("rgol_info_new").getDisplayState() == "expanded") {
                    Xrm.Page.ui.controls.get("IFRAME_RGOLinfo_new").setSrc(rgolPath);

                }

            }
        }
        catch (e) {
            alert("Fel i CGISweden.incidentLibrary.getRGOLapiurl\n\n" + e.Message);
        }
    },

    rgoliframe_onchange_new: function () {
        CGISweden.incident.getRGOLapiurlNew();
    },

    casetypecode_onchange_complete: function () { },

    casetypecode_onchange_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen default kategori 3 definerad!");
            }
            else {
                var _id = result[0].cgi_category_detail3id.Id;
                var _logicalname = result[0].cgi_category_detail3id.LogicalName;
                var _name = result[0].cgi_category_detail3id.Name;
                CGISweden.formscriptfunctions.SetLookup("cgi_casdet_row1_cat3id", _logicalname, _id, _name);
                CGISweden.incident.category3_onchange_nocontext("cgi_casdet_row1_cat3id");
            }
        }
        catch (e) {
            alert("Fel i CGISweden.incident.casetypecode_onchange_callback\n\n" + e.Message);
        }
    },

};

// Functions calls
//
// CGISweden.incident.onFormLoad
// CGISweden.incident.onChangeTravelCard
// 
// 
// 
// 
