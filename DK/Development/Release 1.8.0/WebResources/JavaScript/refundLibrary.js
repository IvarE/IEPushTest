if (typeof (CGISweden) == "undefined")
{ CGISweden = {}; }

//

// *******************************************************
// Entity: cgi_refund
// *******************************************************

var _travelcardnumber;  //Laddas på form_type_create getCaseNumber
var _show_soc_sec = 0;  //Laddas genom reimbursementformid_OnChange
var _milage_contribution_decimal;

var _RefundTypeProductNotrequiredIdCache;
var _RefundAccountIdCache;
var _ReimbursementFormIdCache;

FORM_TYPE_CREATE = 1;
FORM_TYPE_UPDATE = 2;
FORM_TYPE_READONLY = 3;
FORM_TYPE_DISABLED = 4;
FORM_TYPE_QUICKCREATE = 5;
FORM_TYPE_BULKEDIT = 6;

CGISweden.refund =
{
    onFormLoad: function () {

        switch (Xrm.Page.ui.getFormType()) {
            case FORM_TYPE_CREATE:
                CGISweden.refund.getCaseNumber();
                CGISweden.refund.setDefaultValues();
                CGISweden.refund.ctrlAttest();
                CGISweden.refund.ctrlAmountLimit();
                break;
            case FORM_TYPE_UPDATE:
                CGISweden.refund.refund_Onload();
                //CGISweden.refund.reimbursementformid_OnLoad();
                //CGISweden.refund.ctrlAttest();
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
        CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting();
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    onsave: function (context) {

        var eventArgs = context.getEventArgs();

        var _auth_approved = Xrm.Page.getAttribute("cgi_auth_approved").getValue();
        var _attest_req = Xrm.Page.getAttribute("cgi_attest_req").getValue();

        if (_auth_approved == false && _attest_req == "1") {
            alert("Ni är inte behörig att göra utbetalningar till bankkonto.");
            eventArgs.preventDefault();
        }

        var _amount_above_limit = Xrm.Page.getAttribute("cgi_amount_above_limit").getValue();
        var _amount = Xrm.Page.getAttribute("cgi_amount").getValue();
        var _amount_limit_warn = Xrm.Page.getAttribute("cgi_amount_limit_warn").getValue();

        if (_amount > _amount_above_limit) {
            alert("Beloppet du försöker betala ut överstiger maxbeloppet: " + _amount_above_limit + " kr.");
            eventArgs.preventDefault();
        } else if (_amount > _amount_limit_warn) {
            var r = confirm("Du vill betala ut " + _amount + " kronor - är du säker?");
            if (r == false) {
                eventArgs.preventDefault();
            }
        }

        var _check_cust = Xrm.Page.getAttribute("cgi_mandcust").getValue();

        if (_check_cust == 1) {
            alert("Ni kan inte skapa ett beslut utan att ange privatkund eller organisationskund.");
            eventArgs.preventDefault();
        }

        //MaxP 2016-06-07 Kontrollerar om personnummer är angivet
        var _socialsecuritynumber = Xrm.Page.getAttribute("cgi_soc_sec_number").getValue();

        if (_show_soc_sec != 0 && _socialsecuritynumber != null && _socialsecuritynumber != "") {
            var _check_soc = CGISweden.refund.SocSecNoOnChange();
            if (_check_soc == false) {
                eventArgs.preventDefault();
            }
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    getContactAccount: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            if (_caseid != null) {
                CGISweden.odata.GetContactAccount(_caseid, CGISweden.refund.GetContactAccount_callback, CGISweden.refund.GetContactAccount_complete);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.GetContactAccount\n\n" + e.Message);
        }
    },

    GetContactAccount_complete: function () { },

    GetContactAccount_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Du kan inte spara beslut utan kopplling till ärende!");
            }
            else {
                var _cgi_app_value = "0";
                Xrm.Page.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                var _cgi_contact = result[0].cgi_Contactid.Id;
                var _cgi_account = result[0].cgi_Accountid.Id;

                if (_cgi_account == null && _cgi_contact == null) {
                    _cgi_app_value = "1";
                    Xrm.Page.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                    alert("Ni kan inte skapa ett beslut utan att ange privatkund eller organisationskund.");
                }
            }
        }
        catch (e) {
            alert("Fel i GetContactAccount_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ctrlAttest: function () {
        try {
            var userId = Xrm.Page.context.getUserId();
            CGISweden.odata.GetRSID(userId, CGISweden.refund.ctrlAttest_callback, CGISweden.refund.ctrlAttest_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ctrlAttest\n\n" + e.Message);
        }
    },

    ctrlAttest_complete: function () { },

    ctrlAttest_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Användaren hittas inte!");
            }
            else {
                var _cgi_RSID = result[0].cgi_RSID;
                if (_cgi_RSID == null) {
                    Xrm.Page.getAttribute("cgi_auth_approved").setValue(false);
                }
                else {
                    Xrm.Page.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
        }
        catch (e) {
            alert("Fel i ctrlAttest_callback\n\n" + e.Message);
        }
    },

    ctrlAmountLimit: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetAmountLimitFromSetting(_currentdate, CGISweden.refund.ctrlAmountLimit_callback, CGISweden.refund.ctrlAmountLimit_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ctrlAmountLimit\n\n" + e.Message);
        }
    },

    ctrlAmountLimit_complete: function () { },

    ctrlAmountLimit_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Inställning hittas inte!");
            }
            else {
                var _cgi_amount_limit = result[0].cgi_amount_limit.Value;
                var _cgi_amount_limit_warn = result[0].cgi_amount_limit_warn.Value;

                var __cgi_amount_limit = parseFloat(_cgi_amount_limit);
                var __cgi_amount_limit_warn = parseFloat(_cgi_amount_limit_warn);

                Xrm.Page.getAttribute("cgi_amount_above_limit").setValue(__cgi_amount_limit);
                Xrm.Page.getAttribute("cgi_amount_limit_warn").setValue(__cgi_amount_limit_warn);

            }
        }
        catch (e) {
            alert("Fel i ctrlAmountLimit_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    cgi_accountidOnChange: function () {
        CGISweden.refund.Getcgi_refundaccountNumber();
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Getcgi_refundaccountNumber: function () {
        try {
            var _refundaccountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid");
            if (_refundaccountid != null) {
                CGISweden.odata.Getcgi_refundaccountNumber(_refundaccountid, CGISweden.refund.Getcgi_refundaccountNumber_callback, CGISweden.refund.Getcgi_refundaccountNumber_complete);
            }
            else {
                _RefundAccountIdCache = null;


                CGISweden.refund.SetProductFieldRequired();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.Getcgi_refundaccountNumber\n\n" + e.Message);
        }
    },

    Getcgi_refundaccountNumber_complete: function () { },

    Getcgi_refundaccountNumber_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Failed to retrieve RefundAccountNumber!");
            }
            else {
                _RefundAccountIdCache = result[0];

                CGISweden.refund.SetProductFieldRequired();
            }
        }
        catch (e) {
            alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.Message);
        }
    },


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Getcgi_refundtypeproductnotrequiredidSetting: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.Getcgi_refundtypeproductnotrequiredidSetting(_currentdate, CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting_callback, CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.Message);
        }
    },

    Getcgi_refundtypeproductnotrequiredidSetting_complete: function () { },

    Getcgi_refundtypeproductnotrequiredidSetting_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Required setting is missing: cgi_refundtypeproductnotrequiredid!");
            }
            else {
                if (result[0].cgi_refundtypeproductnotrequiredid.Id == null) {
                    alert('Required setting is missing: cgi_refundtypeproductnotrequiredid!');
                }
                _RefundTypeProductNotrequiredIdCache = result[0].cgi_refundtypeproductnotrequiredid;

                CGISweden.refund.SetProductFieldRequired();

            }
        }
        catch (e) {
            alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    SetProductFieldRequired: function () {

        var ProductRequired = false;

        if (_ReimbursementFormIdCache != null) {
            if (_ReimbursementFormIdCache.cgi_UseProduct != null) {
                var _useproduct = _ReimbursementFormIdCache.cgi_UseProduct;
                if (_useproduct == false) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
                    Xrm.Page.getAttribute("cgi_productid").setValue(null);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", true);
                    ProductRequired = true;
                    var _Currentcgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid");
                    var _Currentcgi_refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid");

                    if (_Currentcgi_refundtypeid != null) {//Check that cgi_refundtypeid is set
                        if (_Currentcgi_accountid != null) {//Check that cgi_accountid is set
                            //both cgi_refundtypeid and cgi_accountid is set. Then Check the following conditions
                            if (_RefundTypeProductNotrequiredIdCache != null) { //ensure the setting is loaded
                                if ('{' + _RefundTypeProductNotrequiredIdCache.Id.toUpperCase() + '}' == _Currentcgi_refundtypeid) {//check if cgi_refundtypeid matches with setting
                                    if (_RefundAccountIdCache != null) {//ensure refund account number is loaded for _Currentcgi_accountid
                                        if (_Currentcgi_accountid == _RefundAccountIdCache.cgi_refundaccountId || _Currentcgi_accountid == '{' + _RefundAccountIdCache.cgi_refundaccountId.toUpperCase() + '}') {//ensure that the loaded refund account number matches _Currentcgi_accountid
                                            if (_RefundAccountIdCache.cgi_Account.substring(0, 1) == "2") {//check if cgi_account starts with 2
                                                ProductRequired = false;
                                            }
                                        }
                                        else {
                                            CGISweden.refund.Getcgi_refundaccountNumber();
                                        }
                                    }
                                    else {
                                        CGISweden.refund.Getcgi_refundaccountNumber();
                                    }
                                }
                            }
                            else {
                                CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting();
                            }
                        }
                        else {
                            _RefundAccountIdCache = null; //ensure the cached value is deleted
                        }
                    }
                }
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
                Xrm.Page.getAttribute("cgi_productid").setValue(null);
            }
        }
        else {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
            Xrm.Page.getAttribute("cgi_productid").setValue(null);
        }


        if (ProductRequired) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "required");
        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none");
        }

    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    validatePersonalNumber: function (input) {

        // Check valid length & form
        if (!input) return false;
        if (input.indexOf('-') == -1) {
            if (input.length === 10) {
                input = input.slice(0, 6) + "-" + input.slice(6);
            } else {
                input = input.slice(0, 8) + "-" + input.slice(8);
            }
        }
        if (input.indexOf('-') == -1) {
            if (input.length === 10) {
                input = input.slice(0, 6) + "-" + input.slice(6);
            } else {
                input = input.slice(0, 8) + "-" + input.slice(8);
            }
        }
        if (!input.match(/^(\d{2})(\d{2})(\d{2})\-(\d{4})|(\d{4})(\d{2})(\d{2})\-(\d{4})$/)) return false;

        // Clean input
        input = input.replace('-', '');
        if (input.length == 12) {
            input = input.substring(2);
        }

        // Declare variables
        var d = new Date(((!!RegExp.$1) ? RegExp.$1 : RegExp.$5), (((!!RegExp.$2) ? RegExp.$2 : RegExp.$6) - 1), ((!!RegExp.$3) ? RegExp.$3 : RegExp.$7)),
                sum = 0,
                numdigits = input.length,
                parity = numdigits % 2,
                i,
                digit;

        // Check valid date
        if (Object.prototype.toString.call(d) !== "[object Date]" || isNaN(d.getTime())) return false;

        // Check luhn algorithm
        for (i = 0; i < numdigits; i = i + 1) {
            digit = parseInt(input.charAt(i))
            if (i % 2 == parity) digit *= 2;
            if (digit > 9) digit -= 9;
            sum += digit;
        }
        return (sum % 10) == 0;
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Tömd och flyttad till onsave.
    AmountOnChange: function () {
        //    var _amount_limit_warn = Xrm.Page.getAttribute("cgi_amount_limit_warn").getValue();
        //    var _amount = Xrm.Page.getAttribute("cgi_amount").getValue();
        //	
        //    if (_amount > _amount_limit_warn) {
        //        alert("Du vill betala ut " + _amount + " kronor - är du säker?");
        //    }
    },

    SocSecNoOnChange: function () {
        var _soc = Xrm.Page.getAttribute("cgi_soc_sec_number").getValue();
        var _sectrue = CGISweden.refund.validatePersonalNumber(_soc);
        var _return_save = true;

        if (_sectrue == false) {
            //Xrm.Page.getAttribute("cgi_soc_sec_number").setValue();
            alert("Personnummer är inte giltigt.");
            Xrm.Page.getControl("cgi_soc_sec_number").setFocus();
            _return_save = false;
        }
        else {
            var _soc_trim = _soc.replace('-', '');
            var _soc_trim_length = _soc_trim.length;
            if (_soc_trim_length != 12) {
                //Xrm.Page.getAttribute("cgi_soc_sec_number").setValue();
                alert("Personnummer måste anges med sekel-siffra");
                Xrm.Page.getControl("cgi_soc_sec_number").setFocus();
                _return_save = false;
            }
            else {
                if (_soc.length == 13) {
                    Xrm.Page.getAttribute("cgi_soc_sec_number").setValue(_soc_trim);
                    _return_save = false;
                }
            }
        }
        return (_return_save);
    },

    getCaseNumber: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            if (_caseid != null) {
                CGISweden.odata.GetCaseNumber(_caseid, CGISweden.refund.getCaseNumber_callback, CGISweden.refund.getCaseNumber_complete);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getCaseNumber\n\n" + e.Message);
        }
    },

    getCaseNumber_complete: function () { },

    getCaseNumber_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga ärendenummer!");
            }
            else {
                if (result[0].TicketNumber != null) {
                    var _caseidvalue = result[0].TicketNumber;
                    CGISweden.formscriptfunctions.SetValue("cgi_refundnumber", _caseidvalue);
                }
                _travelcardnumber = result[0].cgi_UnregisterdTravelCard;
                //MaxP 2016-03-24 
                var _caseorigincode = result[0].CaseOriginCode.Value;
                var _casetypecode = result[0].CaseTypeCode.Value;
                var _iscompleted = result[0].cgi_iscompleted;
                var _taxiclaimedamount = result[0].cgi_taxiclaimedamount.Value;
                var _milagekilometers = result[0].cgi_milagekilometers;
                var _compensationclaimfromrgol = result[0].cgi_compensationclaimfromrgol.Value;

                if (_caseorigincode == 285050007 && _casetypecode == 285050003 && _iscompleted == false) {

                    var _id = result[0].cgi_RefundTypes.Id;
                    var _logicalname = result[0].cgi_RefundTypes.LogicalName;
                    var _name = result[0].cgi_RefundTypes.Name;
                    CGISweden.formscriptfunctions.SetLookup("cgi_refundtypeid", _logicalname, _id, _name);
                    CGISweden.refund.refund_Onload();

                    if (_milagekilometers != null && _milagekilometers != 0) {
                        var _milagekilometers_decimal = parseFloat(_milagekilometers / 10);
                        Xrm.Page.getAttribute("cgi_milage").setValue(_milagekilometers_decimal);

                        var _cgi_amount = parseFloat(Math.round(_milagekilometers_decimal * 18.50));
                        //var _cgi_amount_1 = parseFloat(Math.round(_milagekilometers_decimal * 18.50));
                        var _rgolclaimedamount = result[0].cgi_compensationclaimfromrgol.Value;
                        var _rgolclaimedamountdecimal = parseFloat(_rgolclaimedamount);
                        var _cgi_amount_2 = _rgolclaimedamountdecimal;

                        Xrm.Page.getAttribute("cgi_calculated_amount").setValue(_cgi_amount);
                        Xrm.Page.getAttribute("cgi_amount").setValue(_cgi_amount_2);
                    }
                    else if (_taxiclaimedamount != null && _taxiclaimedamount != 0) {
                        var _taxiclaimedamount_decimal = parseFloat(_taxiclaimedamount);
                        Xrm.Page.getAttribute("cgi_amount").setValue(_taxiclaimedamount_decimal);
                    }
                    else {
                        var _rgolclaimedamountdecimal_1 = parseFloat(_compensationclaimfromrgol);
                        Xrm.Page.getAttribute("cgi_amount").setValue(_rgolclaimedamountdecimal_1);
                    }

                    var _id = result[0].cgi_RefundReimbursementForm.Id;
                    var _logicalname = result[0].cgi_RefundReimbursementForm.LogicalName;
                    var _name = result[0].cgi_RefundReimbursementForm.Name;
                    CGISweden.formscriptfunctions.SetLookup("cgi_reimbursementformid", _logicalname, _id, _name);

                    CGISweden.refund.reimbursementformid_OnChange();
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getCaseNumber_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    reimbursementformid_OnLoad: function () {

        try {
            //debugger;
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_reimbursementformid");
            if (_refundtypeid != null) {
                CGISweden.odata.GetReimbursementForm(_refundtypeid, CGISweden.refund.reimbursementformid_OnLoad_callback, CGISweden.refund.reimbursementformid_OnLoad_complete);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_Onload\n\n" + e.Message);
        }
    },

    reimbursementformid_OnLoad_complete: function () { },

    reimbursementformid_OnLoad_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar ()!");
            }
            else {
                _ReimbursementFormIdCache = result[0];
                //Use account
                if (result[0].cgi_UseAccount != null) {
                    var _useaccount = result[0].cgi_UseAccount;

                    if (_useaccount == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required");
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required");
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                }

                //Use responsible 
                if (result[0].cgi_useresponsible != null) {
                    var _usereponsible = result[0].cgi_useresponsible;
                    if (_usereponsible == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required");
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                }

                //Product

                CGISweden.refund.SetProductFieldRequired();

                //Field Travelcard Number          
                var _loadcard = result[0].cgi_loadcard;
                var _sendtostralfors = result[0].cgi_sendtostralfors;

                //if (_loadcard == true || _sendtostralfors == true) {
                if (_loadcard == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);
                }

                //Social Security Number
                var _cgi_payment = result[0].cgi_payment;

                if (_cgi_payment == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);
                }

                //IBAN and SWIFT and accountnumber
                var _cgi_payment_abroad = result[0].cgi_payment_abroad;

                if (_cgi_payment_abroad == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "required");
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
                }
                //Last valid
                var _cgi_last_valid = result[0].cgi_time_valid;

                if (_cgi_last_valid == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnLoad_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    reimbursementformid_OnChange: function () {
        try {
            //debugger;
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_reimbursementformid");
            if (_refundtypeid != null) {
                CGISweden.odata.GetReimbursementForm(_refundtypeid, CGISweden.refund.reimbursementformid_OnChange_callback, CGISweden.refund.reimbursementformid_OnChange_complete);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                Xrm.Page.getAttribute("cgi_accountid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                Xrm.Page.getAttribute("cgi_responsibleid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
                Xrm.Page.getAttribute("cgi_productid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);
                Xrm.Page.getAttribute("cgi_vat_code").setValue(null);

            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnChange\n\n" + e.Message);
        }
    },

    reimbursementformid_OnChange_complete: function () { },

    reimbursementformid_OnChange_callback: function (result) {
        try {

            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar ()!");
            }
            else {
                _ReimbursementFormIdCache = result[0];

                //Use attestation
                if (result[0].cgi_attestation != null) {
                    var _useattestation = result[0].cgi_attestation;

                    if (_useattestation == true) {
                        Xrm.Page.getAttribute("cgi_attest_req").setValue("1");
                    }
                    else {
                        Xrm.Page.getAttribute("cgi_attest_req").setValue("0");
                    }
                }
                else {
                    Xrm.Page.getAttribute("cgi_attest_req").setValue("0");
                }

                //Use account
                if (result[0].cgi_UseAccount != null) {
                    var _useaccount = result[0].cgi_UseAccount;

                    if (_useaccount == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                        Xrm.Page.getAttribute("cgi_accountid").setValue(null);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);
                        Xrm.Page.getAttribute("cgi_vat_code").setValue(null);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required");
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required");
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
                    Xrm.Page.getAttribute("cgi_accountid").setValue(null);
                }

                //Use responsible 
                if (result[0].cgi_useresponsible != null) {
                    var _usereponsible = result[0].cgi_useresponsible;
                    if (_usereponsible == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none");
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                        Xrm.Page.getAttribute("cgi_responsibleid").setValue(null);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required");
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
                    Xrm.Page.getAttribute("cgi_responsibleid").setValue(null);
                }

                //Product

                CGISweden.refund.SetProductFieldRequired();

                //Field Email
                CGISweden.refund.SetEmailVisibilityAndRequirement(result);


                //Field Travelcard Number          
                var _loadcard = result[0].cgi_loadcard;

                if (_loadcard == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true);
                    CGISweden.formscriptfunctions.SetValue("cgi_travelcard_number", _travelcardnumber);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);
                    //CGISweden.formscriptfunctions.SetValue("cgi_travelcard_number", "");
                }

                //Social Security Number with get social security number from incident

                var _cgi_payment = result[0].cgi_payment;

                if (_cgi_payment == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true);
                    CGISweden.refund.setSocSecNumber();
                    CGISweden.refund.ctrlAmountLimit();
                    _show_soc_sec = 1;

                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "");
                    _show_soc_sec = 0;
                }

                //IBAN and SWIFT and accountnumber
                var _cgi_payment_abroad = result[0].cgi_payment_abroad;

                if (_cgi_payment_abroad == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);

                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "required");

                    //MaxP 2016-04-20
                    CGISweden.refund.SetBicIban();

                    CGISweden.refund.ctrlAmountLimit();
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_accountno", "");
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_iban", "");
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_swift", "");
                }

                //Last valid
                var _cgi_last_valid = result[0].cgi_time_valid;

                if (_cgi_last_valid == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true);
                    CGISweden.refund.SetDateValidField();
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_last_valid", "");
                }

                //SMS Coupon
                var _cgi_couponsms = result[0].cgi_couponsms;

                if (_cgi_couponsms == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true);
                    CGISweden.refund.setMobileNumber();
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnChange_callback\n\n" + e.Message);
        }
    },

    SetEmailVisibilityAndRequirement: function (result) {

        var _emailRequiredAndVisible = false;

        var _reimbursementName = result[0].cgi_reimbursementname;

        switch (_reimbursementName) {
            case "Laddning reskassa":
            case "Rabattkod Jojo Online": //The record's name in UTV
            case "Rabattkod e-handel": //The record's name in PROD
            case "Värdebevis - SMS":
            case "Värdebevis - e-post":
                _emailRequiredAndVisible = true;
                break;
            default:
                _emailRequiredAndVisible = false;
        }

        var _sendtostralfors = result[0].cgi_sendtostralfors;

        if (_sendtostralfors) {
            _emailRequiredAndVisible = true;
        }


        if (_emailRequiredAndVisible) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", true);
            CGISweden.refund.setEmail();
        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false);
        }

    },

    ReInvoice_OnChange: function () {
        _show_reinvoice = Xrm.Page.getAttribute("cgi_reinvoicing").getValue()

        if (_show_reinvoice == true) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true);

        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false);
        }

    },

    ShowTravelcardNumber: function () {
        try {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ShowTravelcardNumber\n\n" + e.Message);
        }
    },

    SetReinvoicingFALSE: function () {
        try {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", false);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetReinvoicingFALSE\n\n" + e.Message);
        }
    },

    SetReinvoicingTRUE: function () {
        try {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetReinvoicingTRUE\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    refund_Onload: function () {
        try {
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid");
            if (_refundtypeid != null) {
                CGISweden.odata.GetRefundSetting(_refundtypeid, CGISweden.refund.refundtypeid_Onload_callback, CGISweden.refund.refundtypeid_Onload_complete);
            }
            else {
                CGISweden.refund.refundtypeNONE();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_Onload\n\n" + e.Message);
        }
    },

    refundtypeid_Onload_complete: function () { },

    refundtypeid_Onload_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar (ersättningsbeslut)!");
            }
            else {

                //Field amount editable
                CGISweden.refund.setFieldToEditable();

                //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                var _cgi_FinancialTransaction = result[0].cgi_FinancialTransaction;
                if (_cgi_FinancialTransaction == false) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none");
                }

                //The field cgi_reinvoice defines if its possible to reinvoice reimbursement
                //var _cgi_reinvoice = result[0].cgi_reinvoice;
                //if (_cgi_reinvoice == true) {
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true);
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true);
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", true);
                //}
                //else {
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false);
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false);
                //    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", false);
                //}

                //Set default account
                var _id = result[0].cgi_refundaccountid.Id;
                var _logicalname = result[0].cgi_refundaccountid.LogicalName;
                var _name = result[0].cgi_refundaccountid.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name);

                //Set default responsible
                var _id = result[0].cgi_refundresponsibleId.Id;
                var _logicalname = result[0].cgi_refundresponsibleId.LogicalName;
                var _name = result[0].cgi_refundresponsibleId.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name);

                //Set default product
                var _id = result[0].cgi_refundproductid.Id;
                var _logicalname = result[0].cgi_refundproductid.LogicalName;
                var _name = result[0].cgi_refundproductid.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name);


                if (result[0].cgi_RefundOption != null) {
                    var _refundoptionvalue = result[0].cgi_RefundOption.Value;
                    if (_refundoptionvalue == 285050000) {
                        CGISweden.refund.refundtypeNONE();
                    }

                    if (_refundoptionvalue == 285050001) {
                        CGISweden.refund.refundtypeQUANTITY(_cgi_FinancialTransaction);
                    }

                    if (_refundoptionvalue == 285050002) {
                        CGISweden.refund.refundtypeMONEY();
                    }

                    if (_refundoptionvalue == 285050003) {
                        CGISweden.refund.refundtypeMILAGE();
                    }

                    if (_refundoptionvalue == 285050004) {
                        CGISweden.refund.refundtypeTAXI();
                    }

                    if (_refundoptionvalue == 285050005) {
                        CGISweden.refund.refundtypeOTHER();
                    }

                    if (_refundoptionvalue == 285050006) {
                        CGISweden.refund.refundtypeFORWARD();
                    }

                    ////// MOSCGI
                    if (_refundoptionvalue == 285050008) {
                        CGISweden.refund.refundtypeCOUPONSMS();
                    }
                    if (_refundoptionvalue == 285050009) {
                        CGISweden.refund.refundtypeCOUPONEMAIL();
                    }
                    ////// END MOSCGI
                }
                else {
                    CGISweden.refund.refundtypeNONE();
                }
                //Exeption onload event
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_Onload_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    refundtypeid_OnChange: function () {


        CGISweden.refund.SetProductFieldRequired();
        try {
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid");
            if (_refundtypeid != null) {
                CGISweden.odata.GetRefundSetting(_refundtypeid, CGISweden.refund.refundtypeid_OnChange_callback, CGISweden.refund.refundtypeid_OnChange_complete);
            }
            else {
                CGISweden.refund.refundtypeNONE();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_OnChange\n\n" + e.Message);
        }
    },

    refundtypeid_OnChange_complete: function () { },

    refundtypeid_OnChange_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar (ersättningsbeslut)!");
            }
            else {
                //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                var _cgi_FinancialTransaction = result[0].cgi_FinancialTransaction;
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none");

                if (_cgi_FinancialTransaction == false) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);
                    //MaxP 2016-07-25 The field cgi_FinancialTransaction on refundtype defines that authorization-rights is not needed
                    Xrm.Page.getAttribute("cgi_attest_req").setValue("0");
                    //Social security is not needed
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "");
                    _show_soc_sec = 0;
                    // Iban and BIC is not needed
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_accountno", "");
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_iban", "");
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
                    CGISweden.formscriptfunctions.SetValue("cgi_swift", "");
                    //Product is not needed
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
                    Xrm.Page.getAttribute("cgi_productid").setValue(null);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none");
                }

                //The field cgi_reinvoice defines if its possible to reinvoice reimbursement
                var _cgi_reinvoice = result[0].cgi_reinvoice;
                if (_cgi_reinvoice == true) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false);
                }

                //Set no attestation
                Xrm.Page.getAttribute("cgi_attestation").setValue(285050004);

                //Emty values in all fields
                CGISweden.refund.setEmptyAllValues();

                //Field amount editable
                CGISweden.refund.setFieldToEditable();

                //Hide cgi_iban, cgi_swift, cgi_accountno, cgi_foreign_account, if refundtype changes
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);

                CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "");
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);

                CGISweden.formscriptfunctions.SetValue("cgi_accountno", "");
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);

                CGISweden.formscriptfunctions.SetValue("cgi_iban", "");
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);

                CGISweden.formscriptfunctions.SetValue("cgi_swift", "");
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);

                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none");
                Xrm.Page.getAttribute("cgi_accountid").setValue(null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);

                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none");
                Xrm.Page.getAttribute("cgi_vat_code").setValue(null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);

                Xrm.Page.getAttribute("cgi_responsibleid").setValue(null);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);

                Xrm.Page.getAttribute("cgi_productid").setValue(null);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none");
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);

                //Set default account
                var _id = result[0].cgi_refundaccountid.Id;
                var _logicalname = result[0].cgi_refundaccountid.LogicalName;
                var _name = result[0].cgi_refundaccountid.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name);

                //Set default responsible
                var _id = result[0].cgi_refundresponsibleId.Id;
                var _logicalname = result[0].cgi_refundresponsibleId.LogicalName;
                var _name = result[0].cgi_refundresponsibleId.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name);

                //Set default product
                var _id = result[0].cgi_refundproductid.Id;
                var _logicalname = result[0].cgi_refundproductid.LogicalName;
                var _name = result[0].cgi_refundproductid.Name;

                CGISweden.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name);

                if (result[0].cgi_RefundOption != null) {
                    var _refundoptionvalue = result[0].cgi_RefundOption.Value;

                    if (_refundoptionvalue == 285050000) {
                        CGISweden.refund.refundtypeNONE();
                    }

                    if (_refundoptionvalue == 285050001) {
                        CGISweden.refund.refundtypeQUANTITY();
                    }

                    if (_refundoptionvalue == 285050002) {
                        CGISweden.refund.refundtypeMONEY();
                    }

                    if (_refundoptionvalue == 285050003) {
                        CGISweden.refund.refundtypeMILAGE();
                    }

                    if (_refundoptionvalue == 285050004) {
                        CGISweden.refund.refundtypeTAXI();
                    }

                    if (_refundoptionvalue == 285050005) {
                        CGISweden.refund.refundtypeOTHER();
                    }

                    if (_refundoptionvalue == 285050006) {
                        CGISweden.refund.refundtypeFORWARD();
                    }

                    if (_refundoptionvalue == 285050007) {
                        CGISweden.refund.refundtypeTRAVEL();
                    }

                    ////// MOSCGI
                    if (_refundoptionvalue == 285050008) {
                        CGISweden.refund.refundtypeCOUPONSMS();
                    }
                    if (_refundoptionvalue == 285050009) {
                        CGISweden.refund.refundtypeCOUPONEMAIL();
                    }
                    ////// END MOSCGI
                }
                else {
                    CGISweden.refund.refundtypeNONE();
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_OnChange_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setDefaultValues: function () {
        try {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_foreign_payment", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            //GISweden.formscriptfunctions.HideOrDisplayField("cgi_value_code", false);

            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_refundnumber", false);

            ////////// MOSCGI
            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false);
            ////////// END MOSCGI

            CGISweden.refund.getContactAccount();

        }
        catch (e) {
            alert("Fel i CGISweden.refund.setDefaultValues\n\n" + e.Message);
        }
    },

    refundtypeNONE: function () {
        try {
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);

            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);


            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);

            ////////// MOSCGI
            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false);
            ////////// END MOSCGI

            // MaxP 2016-06-07
            // Utbetalning inom Sverige
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false);
            //// Utbetalning till utlandet
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeNONE\n\n" + e.Message);
        }
    },

    refundtypeQUANTITY: function (_cgi_FinancialTransaction) {
        try {
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", true);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeQUANTITY\n\n" + e.Message);
        }
    },

    refundtypeMILAGE: function () {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "required");
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", true);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required");

            CGISweden.refund.setMilageCompensation();

            CGISweden.formscriptfunctions.SetState("cgi_milage_compensation", true);
            CGISweden.formscriptfunctions.SetState("cgi_amount", false);
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", true);
            //CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_amount"); Emma removed
            CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_calculated_amount");

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);


        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMILAGE\n\n" + e.Message);
        }
    },

    refundtypeTAXI: function () {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required");

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeTAXI\n\n" + e.Message);
        }
    },

    refundtypeOTHER: function () {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");

            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required");

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeOTHER\n\n" + e.Message);
        }
    },

    refundtypeFORWARD: function () {
        try {
            //Biockeck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", true);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeFORWARD\n\n" + e.Message);
        }
    },

    refundtypeMONEY: function () {
        try {
            //Biockeck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");


            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMONEY\n\n" + e.Message);
        }
    },


    refundtypeTRAVEL: function () {
        try {
            //Biockeck

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required");

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMONEY\n\n" + e.Message);
        }
    },
    ////////// MOSCGI
    refundtypeCOUPONSMS: function () {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeCOUPON\n\n" + e.Message);
        }
    },

    refundtypeCOUPONEMAIL: function () {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "required");
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", true);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeCOUPONEMAIL\n\n" + e.Message);
        }
    },
    ////////// END MOSCGI
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set milage compensation
    setMilageCompensation: function () {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetDefaultMilageContributionFromSetting(_currentdate, CGISweden.refund.setMilageContributionOnLoad_callback, CGISweden.refund.setMilageContributionOnLoad_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setMilageCompensation\n\n" + e.Message);
        }
    },

    setMilageContributionOnLoad_complete: function () { },

    setMilageContributionOnLoad_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen milersättning definierad!");
            }
            else {
                var _milage_contribution = result[0].cgi_milage_contribution.Value;
                _milage_contribution_decimal = parseFloat(_milage_contribution);
                CGISweden.formscriptfunctions.SetValue("cgi_milage_compensation", _milage_contribution_decimal);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setMilageCompensation_callback\n\n" + e.Message);
        }
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set valid date for compensation
    SetDateValidField: function () {
        try {
            var today = new Date();

            var futureDate = CGISweden.refund.getFutureDateByAddingMonths(6);
            Xrm.Page.data.entity.attributes.get("cgi_last_valid").setValue(futureDate);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetDateValidField\n\n" + e.Message);
        }
    },

    getFutureDate: function (days) {

        // Convert 'days' to milliseconds
        var millies = 1000 * 60 * 60 * 24 * days;

        // Get the current date/time
        var todaysDate = new Date();

        // Get 'todaysDate' as Epoch Time, then add 'days' number of mSecs to it
        var futureMillies = todaysDate.getTime() + millies;

        // Use the Epoch time of the targeted future date to create
        //   a new Date object, and then return it.
        return new Date(futureMillies);
    },
    getFutureDateByAddingMonths: function (months) {
        // Get the current date/time
        var todaysDate = new Date();
        //create new date by adding the months
        return CGISweden.refund.AddMonthsToDate(todaysDate, months);
    },
    AddMonthsToDate: function (date, months) {
        //create new date by adding the months
        return new Date(new Date(date).setMonth(date.getMonth() + months));
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set BIC and IBAN
    SetBicIban: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            //alert("Ärendenr: " + _caseid);
            CGISweden.odata.GetBicIban(_caseid, CGISweden.refund.setBicIban_callback, CGISweden.refund.setBicIban_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetBicIban\n\n" + e.Message);
        }
    },

    setBicIban_complete: function () { },

    setBicIban_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget bankkonto angivet på ärendet");
            }
            else {
                var _cgi_bic = result[0].cgi_bic;
                var _cgi_iban = result[0].cgi_iban;
                
                CGISweden.formscriptfunctions.SetValue("cgi_iban", _cgi_iban);
                CGISweden.formscriptfunctions.SetValue("cgi_swift", _cgi_bic);
            }
        }
        catch (e) {
            alert("Fel i SetBicIbanOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set social security number
    setSocSecNumber: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            CGISweden.odata.GetSocSecNumber(_caseid, CGISweden.refund.setSocSecOnLoad_callback, CGISweden.refund.setSocSecOnLoad_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SocSecNumber\n\n" + e.Message);
        }
    },

    setSocSecOnLoad_complete: function () { },

    setSocSecOnLoad_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget bankkonto angivet p� �rendet");
            }
            else {
                var _cgi_soc_sec_number = result[0].cgi_soc_sec_number;
                var _cgi_rgol_socialsecuritynumber = result[0].cgi_rgol_socialsecuritynumber;
                var _cgi_iscompleted = result[0].cgi_iscompleted;

                if (_cgi_rgol_socialsecuritynumber != null && _cgi_iscompleted == false)
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_rgol_socialsecuritynumber);
                else
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_soc_sec_number);

            }
        }
        catch (e) {
            alert("Fel i setSocSecOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get email
    setEmail: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            CGISweden.odata.GetEmailAddress(_caseid, CGISweden.refund.getGetEmailAddress_callback, CGISweden.refund.getEmailAddress_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getEmail\n\n" + e.Message);
        }
    },

    getEmailAddress_complete: function () { },

    getGetEmailAddress_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns ingen e-postadress angivet på ärendet");
            }
            else {
                //MaxP 2016-05-11 Om det är ett manuellt ärende från RGOL, så sätt epost = incident.cgi_rgol_delivery_email
                var _cgi_customer_email = result[0].cgi_customer_email;
                var _cgi_delivery_email = result[0].cgi_rgol_delivery_email;
                var _cgi_iscompleted = result[0].cgi_iscompleted;

                    if (_cgi_delivery_email != null && _cgi_iscompleted == false) {
                        CGISweden.formscriptfunctions.SetValue("cgi_email", _cgi_delivery_email);
                    }
                    else {
                        CGISweden.formscriptfunctions.SetValue("cgi_email", _cgi_customer_email);
                    }
                }
            }
      
        catch (e) {
            alert("Fel i getEmail_callback\n\n" + e.Message);
        }
    },

    //Set mobilenumber
    setMobileNumber: function () {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid");
            CGISweden.odata.GetMobileNumber(_caseid, CGISweden.refund.setMobileNumber_callback, CGISweden.refund.setMobileNumber_complete);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getMobileNumber\n\n" + e.Message);
        }
    },

    setMobileNumber_complete: function () { },

    setMobileNumber_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget mobilnummer angivet på ärendet");
            }
            else {
                var _cgi_rgol_telephonenumber = result[0].cgi_rgol_telephonenumber;
                var _cgi_customer_telephonenumber = result[0].cgi_customer_telephonenumber;
                var _cgi_iscompleted = result[0].cgi_iscompleted;

                if (_cgi_rgol_telephonenumber != null && _cgi_iscompleted == false)
                    CGISweden.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_rgol_telephonenumber);
                else
                    CGISweden.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_customer_telephonenumber);
            }
        }
        catch (e) {
            alert("Fel i getMobileNumber_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set all values to 0 or null
    setEmptyAllValues: function () {
        try {
            //S�tt belopp fältet till editerbart
            CGISweden.formscriptfunctions.SetState("cgi_amount", "true");
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", "true");

            //Ers�ttningsformen
            Xrm.Page.getAttribute("cgi_reimbursementformid").setValue(null);

            //Konto uppgifter
            Xrm.Page.getAttribute("cgi_accountno").setValue();
            //Xrm.Page.getAttribute("cgi_foreign_payment").setValue();
            Xrm.Page.getAttribute("cgi_iban").setValue();
            Xrm.Page.getAttribute("cgi_swift").setValue();

            //Konteringssträngen
            Xrm.Page.getAttribute("cgi_accountid").setValue(null);
            Xrm.Page.getAttribute("cgi_responsibleid").setValue(null);
            Xrm.Page.getAttribute("cgi_productid").setValue(null);
            Xrm.Page.getAttribute("cgi_vat_code").setValue(null);

            //Biocheck
            Xrm.Page.getAttribute("cgi_checknumber").setValue(null);
            //CGISweden.formscriptfunctions.SetValue("cgi_check_number", "");

            ////Vidarebefordran
            Xrm.Page.getAttribute("cgi_transportcompanyid").setValue(null);

            ////Taxi
            Xrm.Page.getAttribute("cgi_taxi_company").setValue();

            ////Milers�tttning
            Xrm.Page.getAttribute("cgi_car_reg").setValue();
            CGISweden.formscriptfunctions.SetValue("cgi_milage", 0);
            Xrm.Page.getAttribute("cgi_last_valid").setValue();
            CGISweden.formscriptfunctions.SetValue("cgi_milage_compensation", 0);

            ////Kvantitet och belopp
            Xrm.Page.getAttribute("cgi_quantity").setValue();
            Xrm.Page.getAttribute("cgi_amount").setValue();
            Xrm.Page.getAttribute("cgi_calculated_amount").setValue();
            //Xrm.Page.getAttribute("cgi_amountwithtax").setValue();

            ////Vidarefakturering
            Xrm.Page.getAttribute("cgi_reinvoicing").setValue(false);
            Xrm.Page.getAttribute("cgi_invoicerecipient").setValue(null);

            //Kommentarer
            Xrm.Page.getAttribute("cgi_comments").setValue();

        }
        catch (e) {
            alert("Fel i CGISweden.refund.setEmptyAllValues\n\n" + e.Message);
        }
    },

    // Milage compensation number of mile on_change
    cgi_milage_on_change: function () {
        CGISweden.refund.calculateMilageCompensation();
    },

    //Calculate milage compensation
    calculateMilageCompensation: function () {
        try {
            var _cgi_milage = Xrm.Page.getAttribute("cgi_milage").getValue();
            var _cgi_milage_compensation = Xrm.Page.getAttribute("cgi_milage_compensation").getValue();
            var _cgi_amount = Math.round(_cgi_milage * _cgi_milage_compensation);

            Xrm.Page.getAttribute("cgi_amount").setValue(_cgi_amount);
            Xrm.Page.getAttribute("cgi_calculated_amount").setValue(_cgi_amount);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.calculateCompensation\n\n" + e.Message);
        }

    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    setLimitandAuth: function () {
        //Check security role
        var currentUserRoles = Xrm.Page.context.getUserRoles();
        for (var i = 0; i < currentUserRoles.length; i++) {
            var userRoleId = currentUserRoles[i];
            CGISweden.odata.GetSecRolesName(userRoleId, CGISweden.refund.CheckUserRoleOnchange_callback, CGISweden.refund.CheckUserRoleOnCange_complete);
            //alert("Antal roller " + i);
        }
        //Check amount limit
        //CGISweden.refund.setAmountLimit();
    },

    CheckUserRoleOnCange_complete: function () { },

    CheckUserRoleOnchange_callback: function (result) {
        try {
            if (result == null || result[0] == null) {
                alert("Ingen säkerhetsroll definierad!");
            }
            else {
                var userRoleName = "Attest"
                var roleName = result[0].Name;

                //Om användaren har rollen Attest ovan, så är användaren godkänd för att 
                if (roleName == userRoleName) {
                    Xrm.Page.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.CheckUserRoleOnchange_callback\n\n" + e.Message);
        }
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    checkAuthorization: function () {
        var _attestation_code = Xrm.Page.getAttribute("cgi_attestation").getValue();
        var _auth_approved = Xrm.Page.getAttribute("cgi_auth_approved").getValue();

        //alert("Attestkod" + _attestation_code);
        //alert("Attest godkänd" + _auth_approved);

        try {
            //Om attest kod �r skilt fr�n Pendning (285050000) och �r skilt fr�n No Financial transaction (285050003) anv�ndaren inte har attestbeh�righet
            //, s� sp�rra samtliga f�lt f�r editering.
            if (_attestation_code != 285050000 && _attestation_code != 285050003 && _auth_approved == false) {
                CGISweden.refund.setFieldsToReadOnly();
            }
            //Om anv�ndaren inte har attestbeh�righet, s� sp�rra f�ltet attestkod.
            if (_auth_approved == false) {
                CGISweden.refund.setAttestationToReadOnly();
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.checkAuthorization\n\n" + e.Message);
        }
    },

    setAttestationToReadOnly: function () {
        try {
            CGISweden.formscriptfunctions.SetState("cgi_attestation", "true");
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setAttestationToReadOnly\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setFieldsToReadOnly: function () {
        try {

            //Generella f�lt
            CGISweden.formscriptfunctions.SetState("cgi_caseid", "true");
            CGISweden.formscriptfunctions.SetState("ownerid", "true");
            CGISweden.formscriptfunctions.SetState("cgi_reimbursementformid", "true");


            //Biocheck
            CGISweden.formscriptfunctions.SetState("cgi_checknumber", "true");


            ////Konteringsstr�ngen
            CGISweden.formscriptfunctions.SetState("cgi_accountid", "true");
            CGISweden.formscriptfunctions.SetState("cgi_productid", "true");


            //////Vidarebefordran
            CGISweden.formscriptfunctions.SetState("cgi_transportcompanyid", "true");

            //////Taxi
            CGISweden.formscriptfunctions.SetState("cgi_taxi_company", "true");


            //////Milers�tttning
            CGISweden.formscriptfunctions.SetState("cgi_car_reg", "true");
            CGISweden.formscriptfunctions.SetState("cgi_milage", "true");
            CGISweden.formscriptfunctions.SetState("cgi_last_valid", "true");
            CGISweden.formscriptfunctions.SetState("cgi_milage_compensation", "true");

            //////Kvantitet och belopp
            CGISweden.formscriptfunctions.SetState("cgi_quantity", "true");
            CGISweden.formscriptfunctions.SetState("cgi_amount", "true");
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", "true");
            //CGISweden.formscriptfunctions.SetState("cgi_amountwithtax", "true");


            //////Vidarefakturering
            CGISweden.formscriptfunctions.SetState("cgi_reinvoicing", "true");
            CGISweden.formscriptfunctions.SetState("cgi_invoicerecipient", "true");
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setFieldsToReadOnly\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setFieldToEditable: function () {
        try {
            //CGISweden.formscriptfunctions.SetState("cgi_amount", "false");
            var _field = Xrm.Page.ui.controls.get("cgi_amount");
            _field.setDisabled(false);
            var _field = Xrm.Page.ui.controls.get("cgi_calculated_amount");
            _field.setDisabled(false);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setFieldToEditable\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    format_phonenumber: function (context) {
        try {
            var phoneNumberStr = context.getEventSource();
            var control = Xrm.Page.getControl(phoneNumberStr.getName());

            // Verify that the field is valid
            if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null) {

                if (phoneNumberStr.getValue() != null) {

                    // replace any "-" with a blank space
                    var oldNumberStr = phoneNumberStr.getValue();
                    var newNumberStr = oldNumberStr.replace(/-/g, "");
                    newNumberStr = newNumberStr.replace(/ /g, "");
                    phoneNumberStr.setValue(newNumberStr);
                    if (newNumberStr.indexOf("+") > -1) {
                        control.setNotification("Ange telefonnummer utan landsprefix.");
                    }
                    else {
                        control.clearNotification();
                    }
                }

            }

        }
        catch (e) {
            alert("Fel i CGISweden.account.format_phonenumber\n\n" + e.Message);
        }

    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
};

// Functions calls
//
// CGISweden.refund.onFormLoad
// CGISweden.refund.refundtypeid_OnChange
// CGISweden.refund.reimbursementformid_OnChange
// 
// 
// 



