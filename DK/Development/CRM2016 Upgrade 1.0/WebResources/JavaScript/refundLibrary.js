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
    onFormLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

        if (formContext.ui.getFormType() == 0) {
            setTimeout(CGISweden.refund.onFormLoad(), 1000);
            return;
        }

        switch (formContext.ui.getFormType()) {
            case FORM_TYPE_CREATE:

                CGISweden.refund.getCaseNumber(formContext);
                CGISweden.refund.setDefaultValues(formContext);
                CGISweden.refund.ctrlAttest(formContext);
                CGISweden.refund.ctrlAmountLimit(formContext);

                break;
            case FORM_TYPE_UPDATE:
                CGISweden.refund.refund_Onload(formContext);
                //CGISweden.refund.reimbursementformid_OnLoad(formContext);
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
        CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting(formContext);
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    onsave: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var eventArgs = formContext.getEventArgs();

        var _auth_approved = formContext.getAttribute("cgi_auth_approved").getValue();
        var _attest_req = formContext.getAttribute("cgi_attest_req").getValue();

        if (_auth_approved == false && _attest_req == "1") {
            alert("Ni är inte behörig att göra utbetalningar till bankkonto.");
            eventArgs.preventDefault();
        }

        var _amount_above_limit = formContext.getAttribute("cgi_amount_above_limit").getValue();
        var _amount = formContext.getAttribute("cgi_amount").getValue();
        var _amount_limit_warn = formContext.getAttribute("cgi_amount_limit_warn").getValue();

        if (_amount > _amount_above_limit) {
            alert("Beloppet du försöker betala ut överstiger maxbeloppet: " + _amount_above_limit + " kr.");
            eventArgs.preventDefault();
        } else if (_amount > _amount_limit_warn) {
            var r = confirm("Du vill betala ut " + _amount + " kronor - är du säker?");
            if (r == false) {
                eventArgs.preventDefault();
            }
        }

        var _check_cust = formContext.getAttribute("cgi_mandcust").getValue();

        if (_check_cust == 1) {
            alert("Ni kan inte skapa ett beslut utan att ange privatkund eller organisationskund.");
            eventArgs.preventDefault();
        }

        //MaxP 2016-06-07 Kontrollerar om personnummer är angivet
        var _socialsecuritynumber = formContext.getAttribute("cgi_soc_sec_number").getValue();

        if (_show_soc_sec != 0 && _socialsecuritynumber != null && _socialsecuritynumber != "") {
            var _check_soc = CGISweden.refund.SocSecNoOnChange(formContext);
            if (_check_soc == false) {
                eventArgs.preventDefault();
            }
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    getContactAccount: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            if (_caseid != null) {
                CGISweden.odata.GetContactAccount(_caseid, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.GetContactAccount\n\n" + e.Message);
        }
    },

    GetContactAccount_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Du kan inte spara beslut utan kopplling till ärende!");
            }
            else {
                var _cgi_app_value = "0";
                formContext.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                var _cgi_contact = result[0].cgi_Contactid.Id;
                var _cgi_account = result[0].cgi_Accountid.Id;

                if (_cgi_account == null && _cgi_contact == null) {
                    _cgi_app_value = "1";
                    formContext.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                    alert("Ni kan inte skapa ett beslut utan att ange privatkund eller organisationskund.");
                }
            }
        }
        catch (e) {
            alert("Fel i GetContactAccount_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ctrlAttest: function (formContext) {
        try {
           var globalContext = Xrm.Utility.getGlobalContext();

            var userId = globalContext.userSettings.userId();
            CGISweden.odata.GetRSID(userId, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ctrlAttest\n\n" + e.Message);
        }
    },

    ctrlAttest_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Användaren hittas inte!");
            }
            else {
                var _cgi_RSID = result[0].cgi_RSID;
                if (_cgi_RSID == null) {
                    formContext.getAttribute("cgi_auth_approved").setValue(false);
                }
                else {
                    formContext.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
        }
        catch (e) {
            alert("Fel i ctrlAttest_callback\n\n" + e.Message);
        }
    },

    ctrlAmountLimit: function (formContext) {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetAmountLimitFromSetting(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ctrlAmountLimit\n\n" + e.Message);
        }
    },
    
    ctrlAmountLimit_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Inställning hittas inte!");
            }
            else {
                var _cgi_amount_limit = result[0].cgi_amount_limit.Value;
                var _cgi_amount_limit_warn = result[0].cgi_amount_limit_warn.Value;

                var __cgi_amount_limit = parseFloat(_cgi_amount_limit);
                var __cgi_amount_limit_warn = parseFloat(_cgi_amount_limit_warn);

                formContext.getAttribute("cgi_amount_above_limit").setValue(__cgi_amount_limit);
                formContext.getAttribute("cgi_amount_limit_warn").setValue(__cgi_amount_limit_warn);

            }
        }
        catch (e) {
            alert("Fel i ctrlAmountLimit_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Cant find function in CRM TODO
    cgi_accountidOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.refund.Getcgi_refundaccountNumber(formContext);
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Getcgi_refundaccountNumber: function (formContext) {
        try {
            var _refundaccountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
            if (_refundaccountid != null) {
                CGISweden.odata.Getcgi_refundaccountNumber(_refundaccountid, formContext);
            }
            else {
                _RefundAccountIdCache = null;


                CGISweden.refund.SetProductFieldRequired(formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.Getcgi_refundaccountNumber\n\n" + e.Message);
        }
    },

    Getcgi_refundaccountNumber_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Failed to retrieve RefundAccountNumber!");
            }
            else {
                _RefundAccountIdCache = result[0];

                CGISweden.refund.SetProductFieldRequired(formContext);
            }
        }
        catch (e) {
            alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.Message);
        }
    },


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    Getcgi_refundtypeproductnotrequiredidSetting: function (formContext) {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.Getcgi_refundtypeproductnotrequiredidSetting(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.Message);
        }
    },

    Getcgi_refundtypeproductnotrequiredidSetting_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Required setting is missing: cgi_refundtypeproductnotrequiredid!");
            }
            else {
                if (result[0].cgi_refundtypeproductnotrequiredid.Id == null) {
                    alert('Required setting is missing: cgi_refundtypeproductnotrequiredid!');
                }
                _RefundTypeProductNotrequiredIdCache = result[0].cgi_refundtypeproductnotrequiredid;

                CGISweden.refund.SetProductFieldRequired(formContext);

            }
        }
        catch (e) {
            alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    SetProductFieldRequired: function (formContext) {

        var ProductRequired = false;

        if (_ReimbursementFormIdCache != null) {
            if (_ReimbursementFormIdCache.cgi_UseProduct != null) {
                var _useproduct = _ReimbursementFormIdCache.cgi_UseProduct;
                if (_useproduct == false) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                    formContext.getAttribute("cgi_productid").setValue(null);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", true, formContext);
                    ProductRequired = true;
                    var _Currentcgi_accountid = CGISweden.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                    var _Currentcgi_refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);

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
                                            CGISweden.refund.Getcgi_refundaccountNumber(formContext);
                                        }
                                    }
                                    else {
                                        CGISweden.refund.Getcgi_refundaccountNumber(formContext);
                                    }
                                }
                            }
                            else {
                                CGISweden.refund.Getcgi_refundtypeproductnotrequiredidSetting(formContext);
                            }
                        }
                        else {
                            _RefundAccountIdCache = null; //ensure the cached value is deleted
                        }
                    }
                }
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                formContext.getAttribute("cgi_productid").setValue(null);
            }
        }
        else {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
            formContext.getAttribute("cgi_productid").setValue(null);
        }


        if (ProductRequired) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "required", formContext);
        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none", formContext);
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

    SocSecNoOnChange: function (formContext) {
        var _soc = formContext.getAttribute("cgi_soc_sec_number").getValue();
        var _sectrue = CGISweden.refund.validatePersonalNumber(_soc);
        var _return_save = true;

        if (_sectrue == false) {
            //formContext.getAttribute("cgi_soc_sec_number").setValue();
            alert("Personnummer är inte giltigt.");
            formContext.getControl("cgi_soc_sec_number").setFocus();
            _return_save = false;
        }
        else {
            var _soc_trim = _soc.replace('-', '');
            var _soc_trim_length = _soc_trim.length;
            if (_soc_trim_length != 12) {
                //formContext.getAttribute("cgi_soc_sec_number").setValue();
                alert("Personnummer måste anges med sekel-siffra");
                formContext.getControl("cgi_soc_sec_number").setFocus();
                _return_save = false;
            }
            else {
                if (_soc.length == 13) {
                    formContext.getAttribute("cgi_soc_sec_number").setValue(_soc_trim);
                    _return_save = false;
                }
            }
        }
        return (_return_save);
    },

    getCaseNumber: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            if (_caseid != null) {
                CGISweden.odata.GetCaseNumber(_caseid, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getCaseNumber\n\n" + e.Message);
        }
    },

    getCaseNumber_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga ärendenummer!");
            }
            else {
                if (result[0].TicketNumber != null) {
                    var _caseidvalue = result[0].TicketNumber;
                    CGISweden.formscriptfunctions.SetValue("cgi_refundnumber", _caseidvalue, formContext);
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
                    CGISweden.formscriptfunctions.SetLookup("cgi_refundtypeid", _logicalname, _id, _name, formContext);
                    CGISweden.refund.refund_Onload(formContext);

                    if (_milagekilometers != null && _milagekilometers != 0) {
                        var _milagekilometers_decimal = parseFloat(_milagekilometers / 10);
                        formContext.getAttribute("cgi_milage").setValue(_milagekilometers_decimal);

                        var _cgi_amount = parseFloat(Math.round(_milagekilometers_decimal * 18.50));
                        //var _cgi_amount_1 = parseFloat(Math.round(_milagekilometers_decimal * 18.50));
                        var _rgolclaimedamount = result[0].cgi_compensationclaimfromrgol.Value;
                        var _rgolclaimedamountdecimal = parseFloat(_rgolclaimedamount);
                        var _cgi_amount_2 = _rgolclaimedamountdecimal;

                        formContext.getAttribute("cgi_calculated_amount").setValue(_cgi_amount);
                        formContext.getAttribute("cgi_amount").setValue(_cgi_amount_2);
                    }
                    else if (_taxiclaimedamount != null && _taxiclaimedamount != 0) {
                        var _taxiclaimedamount_decimal = parseFloat(_taxiclaimedamount);
                        formContext.getAttribute("cgi_amount").setValue(_taxiclaimedamount_decimal);
                    }
                    else {
                        var _rgolclaimedamountdecimal_1 = parseFloat(_compensationclaimfromrgol);
                        formContext.getAttribute("cgi_amount").setValue(_rgolclaimedamountdecimal_1);
                    }

                    var _id = result[0].cgi_RefundReimbursementForm.Id;
                    var _logicalname = result[0].cgi_RefundReimbursementForm.LogicalName;
                    var _name = result[0].cgi_RefundReimbursementForm.Name;
                    CGISweden.formscriptfunctions.SetLookup("cgi_reimbursementformid", _logicalname, _id, _name, formContext);

                    CGISweden.refund.reimbursementformid_OnChange(formContext);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getCaseNumber_callback\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    reimbursementformid_OnLoad: function (formContext) {

        try {
            //debugger;
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_reimbursementformid", formContext);
            if (_refundtypeid != null) {
                CGISweden.odata.GetReimbursementFormOnLoad(_refundtypeid, formContext);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_Onload\n\n" + e.Message);
        }
    },

    reimbursementformid_OnLoad_callback: function (result, formContext) {
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
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true, formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required", formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required", formContext);
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                }

                //Use responsible 
                if (result[0].cgi_useresponsible != null) {
                    var _usereponsible = result[0].cgi_useresponsible;
                    if (_usereponsible == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true, formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required", formContext);
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                }

                //Product

                CGISweden.refund.SetProductFieldRequired(formContext);

                //Field Travelcard Number          
                var _loadcard = result[0].cgi_loadcard;
                var _sendtostralfors = result[0].cgi_sendtostralfors;

                //if (_loadcard == true || _sendtostralfors == true) {
                if (_loadcard == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
                }

                //Social Security Number
                var _cgi_payment = result[0].cgi_payment;

                if (_cgi_payment == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                }

                //IBAN and SWIFT and accountnumber
                var _cgi_payment_abroad = result[0].cgi_payment_abroad;

                if (_cgi_payment_abroad == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "required", formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                }
                //Last valid
                var _cgi_last_valid = result[0].cgi_time_valid;

                if (_cgi_last_valid == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnLoad_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    reimbursementformid_OnChange: function (formContext) {
        try {
            //debugger;
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_reimbursementformid", formContext);
            if (_refundtypeid != null) {
                CGISweden.odata.GetReimbursementForm(_refundtypeid, formContext);
            }
            else {
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                formContext.getAttribute("cgi_accountid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                formContext.getAttribute("cgi_responsibleid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                formContext.getAttribute("cgi_productid").setValue(null);

                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                formContext.getAttribute("cgi_vat_code").setValue(null);

            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnChange\n\n" + e.Message);
        }
    },

    reimbursementformid_OnChange_callback: function (result, formContext) {
        try {
            debugger;
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar ()!");
            }
            else {
                _ReimbursementFormIdCache = result[0];

                //Use attestation
                if (result[0].cgi_attestation != null) {
                    var _useattestation = result[0].cgi_attestation;

                    if (_useattestation == true) {
                        formContext.getAttribute("cgi_attest_req").setValue("1");
                    }
                    else {
                        formContext.getAttribute("cgi_attest_req").setValue("0");
                    }
                }
                else {
                    formContext.getAttribute("cgi_attest_req").setValue("0");
                }

                //Use account
                if (result[0].cgi_UseAccount != null) {
                    var _useaccount = result[0].cgi_UseAccount;

                    if (_useaccount == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                        formContext.getAttribute("cgi_accountid").setValue(null);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                        formContext.getAttribute("cgi_vat_code").setValue(null);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true, formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required", formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required", formContext);
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                    formContext.getAttribute("cgi_accountid").setValue(null);
                }

                //Use responsible 
                if (result[0].cgi_useresponsible != null) {
                    var _usereponsible = result[0].cgi_useresponsible;
                    if (_usereponsible == false) {
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                        formContext.getAttribute("cgi_responsibleid").setValue(null);
                    }
                    else {
                        CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true, formContext);
                        CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required", formContext);
                    }
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                    formContext.getAttribute("cgi_responsibleid").setValue(null);
                }

                //Product

                CGISweden.refund.SetProductFieldRequired(formContext);

                //Field Email
                CGISweden.refund.SetEmailVisibilityAndRequirement(result, formContext);


                //Field Travelcard Number          
                var _loadcard = result[0].cgi_loadcard;

                if (_loadcard == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_travelcard_number", _travelcardnumber, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
                    //CGISweden.formscriptfunctions.SetValue("cgi_travelcard_number", "");
                }

                //Social Security Number with get social security number from incident

                var _cgi_payment = result[0].cgi_payment;

                if (_cgi_payment == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true, formContext);
                    CGISweden.refund.setSocSecNumber(formContext);
                    CGISweden.refund.ctrlAmountLimit(formContext);
                    _show_soc_sec = 1;

                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                    _show_soc_sec = 0;
                }

                //IBAN and SWIFT and accountnumber
                var _cgi_payment_abroad = result[0].cgi_payment_abroad;

                if (_cgi_payment_abroad == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);

                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "required", formContext);

                    //MaxP 2016-04-20
                    CGISweden.refund.SetBicIban(formContext);

                    CGISweden.refund.ctrlAmountLimit(formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                }

                //Last valid

                var _cgi_last_valid = result[0].cgi_time_valid;

                if (_cgi_last_valid == true) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
                    CGISweden.refund.SetDateValidField(formContext);
                }
                /*
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
                */
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.reimbursementformid_OnChange_callback\n\n" + e.Message);
        }
    },

    SetEmailVisibilityAndRequirement: function (result, formContext) {

        var _emailRequiredAndVisible = false;
        var _phoneRequired = false;

        var _reimbursementName = result[0].cgi_reimbursementname;

        debugger;

        switch (_reimbursementName) {
            case "Laddning reskassa":
            case "Rabattkod Jojo Online": //The record's name in UTV
            case "Rabattkod e-handel": //The record's name in PROD
            //case "Värdekod - E-post":
            case "Värdekod E-post - Försening":
                _emailRequiredAndVisible = true;
                break;
            case "Värdekod E-post - Försening (Saldo)":
                _emailRequiredAndVisible = true;
                break;
            //case "Värdekod - E-post (ersättning)":
            case "Värdekod E-post - Ersättning":
                _emailRequiredAndVisible = true;
                break;
            case "Värdekod E-post - Ersättning (Saldo)":
                _emailRequiredAndVisible = true;
                break;
            //case "Värdekod - E-post (byta reskassa)":
            case "Värdekod E-post - Saldo":
                _emailRequiredAndVisible = true;
                break;
            case "Värdekod SMS - Försening":
                _phoneRequired = true;
                break;
            case "Värdekod SMS - Försening (Saldo)":
                _phoneRequired = true;
                break;
            case "Värdekod SMS - Ersättning":
                _phoneRequired = true;
                break;
            case "Värdekod SMS - Ersättning (Saldo)":
                _phoneRequired = true;
                break;
            case "Värdekod SMS - Saldo":
                _phoneRequired = true;
                break;
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

        if (_phoneRequired) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);
            CGISweden.refund.setMobileNumber(formContext);
        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);
        }

        if (_emailRequiredAndVisible) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);
            CGISweden.refund.setEmail(formContext);
        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
        }

    },

    ReInvoice_OnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        _show_reinvoice = formContext.getAttribute("cgi_reinvoicing").getValue()

        if (_show_reinvoice == true) {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true, formContext);

        }
        else {
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
        }

    },

    ShowTravelcardNumber: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.ShowTravelcardNumber\n\n" + e.Message);
        }
    },

    SetReinvoicingFALSE: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", false, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetReinvoicingFALSE\n\n" + e.Message);
        }
    },

    //verkar inte användas TODO
    SetReinvoicingTRUE: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reference", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetReinvoicingTRUE\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    refund_Onload: function (formContext) {
        try {
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);
            if (_refundtypeid != null) {
                CGISweden.odata.GetRefundSettingOnLoad(_refundtypeid, formContext);
            }
            else {
                CGISweden.refund.refundtypeNONE(formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_Onload\n\n" + e.Message);
        }
    },

    refundtypeid_Onload_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar (ersättningsbeslut)!");
            }
            else {

                //Field amount editable
                CGISweden.refund.setFieldToEditable(formContext);

                //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                var _cgi_FinancialTransaction = result[0].cgi_FinancialTransaction;
                if (_cgi_FinancialTransaction == false) {
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
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
                if (result[0].cgi_refundaccountid.Id != null) {
                    var _id = result[0].cgi_refundaccountid.Id;
                    var _logicalname = result[0].cgi_refundaccountid.LogicalName;
                    var _name = result[0].cgi_refundaccountid.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name, formContext);
                }

                if (result[0].cgi_refundresponsibleId.Id != null) {
                    //Set default responsible
                    var _id = result[0].cgi_refundresponsibleId.Id;
                    var _logicalname = result[0].cgi_refundresponsibleId.LogicalName;
                    var _name = result[0].cgi_refundresponsibleId.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name, formContext);
                }

                if (result[0].cgi_refundproductid.Id != null) {
                    //Set default product
                    var _id = result[0].cgi_refundproductid.Id;
                    var _logicalname = result[0].cgi_refundproductid.LogicalName;
                    var _name = result[0].cgi_refundproductid.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name, formContext);
                }


                if (result[0].cgi_RefundOption != null) {
                    var _refundoptionvalue = result[0].cgi_RefundOption.Value;
                    if (_refundoptionvalue == 285050000) {
                        CGISweden.refund.refundtypeNONE(formContext);
                    }

                    if (_refundoptionvalue == 285050001) {
                        CGISweden.refund.refundtypeQUANTITY(_cgi_FinancialTransaction, formContext);
                    }

                    if (_refundoptionvalue == 285050002) {
                        CGISweden.refund.refundtypeMONEY(formContext);
                    }

                    if (_refundoptionvalue == 285050003) {
                        CGISweden.refund.refundtypeMILAGE(formContext);
                    }

                    if (_refundoptionvalue == 285050004) {
                        CGISweden.refund.refundtypeTAXI(formContext);
                    }

                    if (_refundoptionvalue == 285050005) {
                        CGISweden.refund.refundtypeOTHER(formContext);
                    }

                    if (_refundoptionvalue == 285050006) {
                        CGISweden.refund.refundtypeFORWARD(formContext);
                    }

                    ////// MOSCGI
                    if (_refundoptionvalue == 285050008) {
                        CGISweden.refund.refundtypeCOUPONSMS(formContext);
                    }
                    if (_refundoptionvalue == 285050009) {
                        CGISweden.refund.refundtypeCOUPONEMAIL(formContext);
                    }
                    ////// END MOSCGI

                    debugger;
                    if (_refundoptionvalue == 285050010 || _refundoptionvalue == 285050012 || _refundoptionvalue == 285050014) {
                        CGISweden.refund.refundtypeVALUECODESMS(formContext);
                    }
                    if (_refundoptionvalue == 285050011 || _refundoptionvalue == 285050013 || _refundoptionvalue == 285050015) {
                        CGISweden.refund.refundtypeVALUECODEEMAIL(formContext);
                    }
                }
                else {
                    CGISweden.refund.refundtypeNONE(formContext);
                }
                //Exeption onload event
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_Onload_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    refundtypeid_OnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.refund.SetProductFieldRequired(formContext);
        try {
            var _refundtypeid = CGISweden.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);
            if (_refundtypeid != null) {
                CGISweden.odata.GetRefundSetting(_refundtypeid, formContext);
            }
            else {
                CGISweden.refund.refundtypeNONE(formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_OnChange\n\n" + e.Message);
        }
    },

    refundtypeid_OnChange_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Hittar inga inställningar (ersättningsbeslut)!");
            }
            else {
                //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                var _cgi_FinancialTransaction = result[0].cgi_FinancialTransaction;
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);

                if (_cgi_FinancialTransaction == false) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);
                    //MaxP 2016-07-25 The field cgi_FinancialTransaction on refundtype defines that authorization-rights is not needed
                    formContext.getAttribute("cgi_attest_req").setValue("0");
                    //Social security is not needed
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                    _show_soc_sec = 0;
                    // Iban and BIC is not needed
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                    CGISweden.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                    //Product is not needed
                    //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none");
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                    formContext.getAttribute("cgi_productid").setValue(null);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true, formContext);
                    CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
                }

                //The field cgi_reinvoice defines if its possible to reinvoice reimbursement
                var _cgi_reinvoice = result[0].cgi_reinvoice;
                if (_cgi_reinvoice == true) {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);
                }

                //Set no attestation
                formContext.getAttribute("cgi_attestation").setValue(285050004);

                //Emty values in all fields
                CGISweden.refund.setEmptyAllValues(formContext);

                //Field amount editable
                CGISweden.refund.setFieldToEditable(formContext);

                //Hide cgi_iban, cgi_swift, cgi_accountno, cgi_foreign_account, if refundtype changes
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);

                CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);

                CGISweden.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);

                CGISweden.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);

                CGISweden.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);

                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                formContext.getAttribute("cgi_accountid").setValue(null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);

                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                formContext.getAttribute("cgi_vat_code").setValue(null);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);

                formContext.getAttribute("cgi_responsibleid").setValue(null);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);

                formContext.getAttribute("cgi_productid").setValue(null);
                CGISweden.formscriptfunctions.SetRequiredLevel("cgi_productid", "none", formContext);
                CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);

                //Set default account
                if (result[0].cgi_refundaccountid.Id != null) {
                    var _id = result[0].cgi_refundaccountid.Id;
                    var _logicalname = result[0].cgi_refundaccountid.LogicalName;
                    var _name = result[0].cgi_refundaccountid.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name, formContext);
                }

                //Set default responsible
                if (result[0].cgi_refundresponsibleId.Id != null) {
                    var _id = result[0].cgi_refundresponsibleId.Id;
                    var _logicalname = result[0].cgi_refundresponsibleId.LogicalName;
                    var _name = result[0].cgi_refundresponsibleId.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name, formContext);
                }

                //Set default product
                if (result[0].cgi_refundproductid.Id != null) {
                    var _id = result[0].cgi_refundproductid.Id;
                    var _logicalname = result[0].cgi_refundproductid.LogicalName;
                    var _name = result[0].cgi_refundproductid.Name;

                    CGISweden.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name, formContext);
                }

                if (result[0].cgi_RefundOption != null) {
                    var _refundoptionvalue = result[0].cgi_RefundOption.Value;

                    if (_refundoptionvalue == 285050000) {
                        CGISweden.refund.refundtypeNONE(formContext);
                    }

                    if (_refundoptionvalue == 285050001) {
                        CGISweden.refund.refundtypeQUANTITY(formContext);
                    }

                    if (_refundoptionvalue == 285050002) {
                        CGISweden.refund.refundtypeMONEY(formContext);
                    }

                    if (_refundoptionvalue == 285050003) {
                        CGISweden.refund.refundtypeMILAGE(formContext);
                    }

                    if (_refundoptionvalue == 285050004) {
                        CGISweden.refund.refundtypeTAXI(formContext);
                    }

                    if (_refundoptionvalue == 285050005) {
                        CGISweden.refund.refundtypeOTHER(formContext);
                    }

                    if (_refundoptionvalue == 285050006) {
                        CGISweden.refund.refundtypeFORWARD(formContext);
                    }

                    if (_refundoptionvalue == 285050007) {
                        CGISweden.refund.refundtypeTRAVEL(formContext);
                    }

                    ////// MOSCGI
                    if (_refundoptionvalue == 285050008) {
                        CGISweden.refund.refundtypeCOUPONSMS(formContext);
                    }
                    if (_refundoptionvalue == 285050009) {
                        CGISweden.refund.refundtypeCOUPONEMAIL(formContext);
                    }
                    ////// END MOSCGI

                    if (_refundoptionvalue == 285050010) {
                        CGISweden.refund.refundtypeVALUECODESMS(formContext);
                    }
                    if (_refundoptionvalue == 285050011) {
                        CGISweden.refund.refundtypeVALUECODEEMAIL(formContext);
                    }
                }
                else {
                    CGISweden.refund.refundtypeNONE(formContext);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeid_OnChange_callback\n\n" + e.Message);
        }
    },

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setDefaultValues: function (formContext) {
        try {
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_foreign_payment", false);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            //GISweden.formscriptfunctions.HideOrDisplayField("cgi_value_code", false);

            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_refundnumber", false, formContext);

            ////////// MOSCGI
            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
            ////////// END MOSCGI

            CGISweden.refund.getContactAccount(formContext);

        }
        catch (e) {
            alert("Fel i CGISweden.refund.setDefaultValues\n\n" + e.Message);
        }
    },

    refundtypeNONE: function (formContext) {
        try {
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);

            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);


            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);

            ////////// MOSCGI
            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
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

    refundtypeQUANTITY: function (_cgi_FinancialTransaction, formContext) {
        try {
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false);
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", true, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeQUANTITY\n\n" + e.Message);
        }
    },

    refundtypeMILAGE: function (formContext) {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "required", formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", true, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);

            CGISweden.refund.setMilageCompensation(formContext);

            CGISweden.formscriptfunctions.SetState("cgi_milage_compensation", true, formContext);
            CGISweden.formscriptfunctions.SetState("cgi_amount", false, formContext);
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", true, formContext);
            //CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_amount"); Emma removed
            CGISweden.formscriptfunctions.SetSubmitModeAlways("cgi_calculated_amount", formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);


        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMILAGE\n\n" + e.Message);
        }
    },

    refundtypeTAXI: function (formContext) {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeTAXI\n\n" + e.Message);
        }
    },

    refundtypeOTHER: function (formContext) {
        try {
            //Biocheck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);

            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeOTHER\n\n" + e.Message);
        }
    },

    refundtypeFORWARD: function (formContext) {
        try {
            //Biockeck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", true, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeFORWARD\n\n" + e.Message);
        }
    },

    refundtypeMONEY: function (formContext) {
        try {
            //Biockeck
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");


            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMONEY\n\n" + e.Message);
        }
    },


    refundtypeTRAVEL: function (formContext) {
        try {
            //Biockeck

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeMONEY\n\n" + e.Message);
        }
    },
    ////////// MOSCGI
    refundtypeCOUPONSMS: function (formContext) {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeCOUPON\n\n" + e.Message);
        }
    },

    refundtypeVALUECODESMS: function (formContext) {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeVALUECODESMS\n\n" + e.Message);
        }
    },

    refundtypeCOUPONEMAIL: function (formContext) {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeCOUPONEMAIL\n\n" + e.Message);
        }
    },

    refundtypeVALUECODEEMAIL: function (formContext) {
        try {

            //Biocheck
            //CGISweden.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

            //Kortnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

            //Vidarebefordran
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

            //Taxi
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

            //Milers�ttning
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
            //CGISweden.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

            //Mobilnummer
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

            //Epost
            CGISweden.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);

            CGISweden.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.refundtypeVALUECODEEMAIL\n\n" + e.Message);
        }
    },
    ////////// END MOSCGI
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set milage compensation
    setMilageCompensation: function (formContext) {
        try {
            var _currentdate = CGISweden.formscriptfunctions.GetDateTime();
            CGISweden.odata.GetDefaultMilageContributionFromSetting(_currentdate, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setMilageCompensation\n\n" + e.Message);
        }
    },

    setMilageContributionOnLoad_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Det finns ingen milersättning definierad!");
            }
            else {
                var _milage_contribution = result[0].cgi_milage_contribution.Value;
                _milage_contribution_decimal = parseFloat(_milage_contribution);
                CGISweden.formscriptfunctions.SetValue("cgi_milage_compensation", _milage_contribution_decimal, formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setMilageCompensation_callback\n\n" + e.Message);
        }
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set valid date for compensation
    SetDateValidField: function (formContext) {
        try {
            var futureDate = CGISweden.refund.getFutureDateByAddingMonths(12);
            CGISweden.formscriptfunctions.SetValue("cgi_last_valid", futureDate, formContext);
            console.log(futureDate);
            console.log(formContext.getAttribute("cgi_last_valid").getValue());
            //formContext.data.entity.attributes.get("cgi_last_valid").setValue(futureDate);
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
    SetBicIban: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            //alert("Ärendenr: " + _caseid);
            CGISweden.odata.GetBicIban(_caseid, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SetBicIban\n\n" + e.Message);
        }
    },

    setBicIban_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget bankkonto angivet på ärendet");
            }
            else {
                var _cgi_bic = result[0].cgi_bic;
                var _cgi_iban = result[0].cgi_iban;

                CGISweden.formscriptfunctions.SetValue("cgi_iban", _cgi_iban, formContext);
                CGISweden.formscriptfunctions.SetValue("cgi_swift", _cgi_bic, formContext);
            }
        }
        catch (e) {
            alert("Fel i SetBicIbanOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set social security number
    setSocSecNumber: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            CGISweden.odata.GetSocSecNumber(_caseid, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.SocSecNumber\n\n" + e.Message);
        }
    },

    setSocSecOnLoad_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget bankkonto angivet p� �rendet");
            }
            else {
                var _cgi_soc_sec_number = result[0].cgi_soc_sec_number;
                var _cgi_rgol_socialsecuritynumber = result[0].cgi_rgol_socialsecuritynumber;
                var _cgi_iscompleted = result[0].cgi_iscompleted;

                if (_cgi_rgol_socialsecuritynumber != null && _cgi_iscompleted == false)
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_rgol_socialsecuritynumber, formContext);
                else
                    CGISweden.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_soc_sec_number, formContext);

            }
        }
        catch (e) {
            alert("Fel i setSocSecOnLoad_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Get email
    setEmail: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            CGISweden.odata.GetEmailAddress(_caseid, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getEmail\n\n" + e.Message);
        }
    },

    getGetEmailAddress_callback: function (result, formContext) {
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
                    CGISweden.formscriptfunctions.SetValue("cgi_email", _cgi_delivery_email, formContext);
                }
                else {
                    CGISweden.formscriptfunctions.SetValue("cgi_email", _cgi_customer_email, formContext);
                }
            }
        }

        catch (e) {
            alert("Fel i getEmail_callback\n\n" + e.Message);
        }
    },

    //Set mobilenumber
    setMobileNumber: function (formContext) {
        try {
            var _caseid = CGISweden.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
            CGISweden.odata.GetMobileNumber(_caseid, formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.getMobileNumber\n\n" + e.Message);
        }
    },

    setMobileNumber_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                //alert("Det finns inget mobilnummer angivet på ärendet");
            }
            else {
                var _cgi_rgol_telephonenumber = result[0].cgi_rgol_telephonenumber;
                var _cgi_customer_telephonenumber = result[0].cgi_customer_telephonenumber;
                var _cgi_iscompleted = result[0].cgi_iscompleted;

                if (_cgi_rgol_telephonenumber != null && _cgi_iscompleted == false)
                    CGISweden.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_rgol_telephonenumber, formContext);
                else
                    CGISweden.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_customer_telephonenumber, formContext);
            }
        }
        catch (e) {
            alert("Fel i getMobileNumber_callback\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Set all values to 0 or null
    setEmptyAllValues: function (formContext) {
        try {
            //S�tt belopp fältet till editerbart
            CGISweden.formscriptfunctions.SetState("cgi_amount", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", "true", formContext);

            //Ers�ttningsformen
            formContext.getAttribute("cgi_reimbursementformid").setValue(null);

            //Konto uppgifter
            formContext.getAttribute("cgi_accountno").setValue();
            //formContext.getAttribute("cgi_foreign_payment").setValue();
            formContext.getAttribute("cgi_iban").setValue();
            formContext.getAttribute("cgi_swift").setValue();

            //Konteringssträngen
            formContext.getAttribute("cgi_accountid").setValue(null);
            formContext.getAttribute("cgi_responsibleid").setValue(null);
            formContext.getAttribute("cgi_productid").setValue(null);
            formContext.getAttribute("cgi_vat_code").setValue(null);

            //Biocheck
            formContext.getAttribute("cgi_checknumber").setValue(null);
            //CGISweden.formscriptfunctions.SetValue("cgi_check_number", "", formContext);

            ////Vidarebefordran
            formContext.getAttribute("cgi_transportcompanyid").setValue(null);

            ////Taxi
            formContext.getAttribute("cgi_taxi_company").setValue();

            ////Milers�tttning
            formContext.getAttribute("cgi_car_reg").setValue();
            CGISweden.formscriptfunctions.SetValue("cgi_milage", 0, formContext);
            formContext.getAttribute("cgi_last_valid").setValue();
            CGISweden.formscriptfunctions.SetValue("cgi_milage_compensation", 0, formContext);

            ////Kvantitet och belopp
            formContext.getAttribute("cgi_quantity").setValue();
            formContext.getAttribute("cgi_amount").setValue();
            formContext.getAttribute("cgi_calculated_amount").setValue();
            //formContext.getAttribute("cgi_amountwithtax").setValue();

            ////Vidarefakturering
            formContext.getAttribute("cgi_reinvoicing").setValue(false);
            formContext.getAttribute("cgi_invoicerecipient").setValue(null);

            //Kommentarer
            formContext.getAttribute("cgi_comments").setValue();

        }
        catch (e) {
            alert("Fel i CGISweden.refund.setEmptyAllValues\n\n" + e.Message);
        }
    },

    // Milage compensation number of mile on_change
    cgi_milage_on_change: function (executionContext) {
        var formContext = executionContext.getFormContext();

        CGISweden.refund.calculateMilageCompensation(formContext);
    },

    //Calculate milage compensation
    calculateMilageCompensation: function (formContext) {
        try {
            var _cgi_milage = formContext.getAttribute("cgi_milage").getValue();
            var _cgi_milage_compensation = formContext.getAttribute("cgi_milage_compensation").getValue();
            var _cgi_amount = Math.round(_cgi_milage * _cgi_milage_compensation);

            formContext.getAttribute("cgi_amount").setValue(_cgi_amount);
            formContext.getAttribute("cgi_calculated_amount").setValue(_cgi_amount);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.calculateCompensation\n\n" + e.Message);
        }

    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
    setLimitandAuth: function (executionContext) {
        var formContext = executionContext.getFormContext();
        var globalContext = Xrm.Utility.getGlobalContext();

        //Check security role
        var currentUserRoles = globalContext.userSettings.securityRoles();
        for (var i = 0; i < currentUserRoles.length; i++) {
            var userRoleId = currentUserRoles[i];
            CGISweden.odata.GetSecRolesNameRefund(userRoleId, formContext);
            //alert("Antal roller " + i);
        }
        //Check amount limit
        //CGISweden.refund.setAmountLimit();
    },

    CheckUserRoleOnchange_callback: function (result, formContext) {
        try {
            if (result == null || result[0] == null) {
                alert("Ingen säkerhetsroll definierad!");
            }
            else {
                var userRoleName = "Attest"
                var roleName = result[0].Name;

                //Om användaren har rollen Attest ovan, så är användaren godkänd för att 
                if (roleName == userRoleName) {
                    formContext.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.CheckUserRoleOnchange_callback\n\n" + e.Message);
        }
    },
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

   
    checkAuthorization: function (executionContext) {
        var _attestation_code = formContext.getAttribute("cgi_attestation").getValue();
        var _auth_approved = formContext.getAttribute("cgi_auth_approved").getValue();

        //alert("Attestkod" + _attestation_code);
        //alert("Attest godkänd" + _auth_approved);

        try {
            //Om attest kod �r skilt fr�n Pendning (285050000) och �r skilt fr�n No Financial transaction (285050003) anv�ndaren inte har attestbeh�righet
            //, s� sp�rra samtliga f�lt f�r editering.
            if (_attestation_code != 285050000 && _attestation_code != 285050003 && _auth_approved == false) {
                CGISweden.refund.setFieldsToReadOnly(formContext);
            }
            //Om anv�ndaren inte har attestbeh�righet, s� sp�rra f�ltet attestkod.
            if (_auth_approved == false) {
                CGISweden.refund.setAttestationToReadOnly(formContext);
            }
        }
        catch (e) {
            alert("Fel i CGISweden.refund.checkAuthorization\n\n" + e.Message);
        }
    },

    setAttestationToReadOnly: function (formContext) {
        try {
            CGISweden.formscriptfunctions.SetState("cgi_attestation", "true", formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setAttestationToReadOnly\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setFieldsToReadOnly: function (formContext) {
        try {

            //Generella f�lt
            CGISweden.formscriptfunctions.SetState("cgi_caseid", "true", formContext);
            CGISweden.formscriptfunctions.SetState("ownerid", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_reimbursementformid", "true", formContext);


            //Biocheck
            CGISweden.formscriptfunctions.SetState("cgi_checknumber", "true", formContext);


            ////Konteringsstr�ngen
            CGISweden.formscriptfunctions.SetState("cgi_accountid", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_productid", "true", formContext);


            //////Vidarebefordran
            CGISweden.formscriptfunctions.SetState("cgi_transportcompanyid", "true", formContext);

            //////Taxi
            CGISweden.formscriptfunctions.SetState("cgi_taxi_company", "true", formContext);


            //////Milers�tttning
            CGISweden.formscriptfunctions.SetState("cgi_car_reg", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_milage", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_last_valid", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_milage_compensation", "true", formContext);

            //////Kvantitet och belopp
            CGISweden.formscriptfunctions.SetState("cgi_quantity", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_amount", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_calculated_amount", "true", formContext);
            //CGISweden.formscriptfunctions.SetState("cgi_amountwithtax", "true");


            //////Vidarefakturering
            CGISweden.formscriptfunctions.SetState("cgi_reinvoicing", "true", formContext);
            CGISweden.formscriptfunctions.SetState("cgi_invoicerecipient", "true", formContext);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setFieldsToReadOnly\n\n" + e.Message);
        }
    },
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    setFieldToEditable: function (formContext) {
        try {
            //CGISweden.formscriptfunctions.SetState("cgi_amount", "false");
            var _field = formContext.ui.controls.get("cgi_amount");
            _field.setDisabled(false);
            var _field = formContext.ui.controls.get("cgi_calculated_amount");
            _field.setDisabled(false);
        }
        catch (e) {
            alert("Fel i CGISweden.refund.setFieldToEditable\n\n" + e.Message);
        }
    },

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
    format_phonenumber: function (executionContext) {
        try {
            var formContext = executionContext.getFormContext();

            var phoneNumberStr = formContext.getEventSource();
            var control = formContext.getControl(phoneNumberStr.getName());

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