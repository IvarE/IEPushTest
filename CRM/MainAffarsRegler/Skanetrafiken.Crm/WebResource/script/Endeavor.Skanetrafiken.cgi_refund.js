
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
FORM_TYPE_QUICKCREATE_DEPRECATED = 5;
FORM_TYPE_BULKEDIT = 6;


// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {
    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.cgi_refund) == "undefined") {
    Endeavor.Skanetrafiken.cgi_refund = {

        createAndSendValueCode: function (formContext) {

            var idRecord = formContext.data.entity.getId();
            formContext.ui.setFormNotification("Skapar värdekod. Vänligen vänta.", "INFO");

            Endeavor.formscriptfunctions.callAction("ed_CreateAndSendValueCodeFromRefund7be826f88b9fe811827600155d010b00", "cgi_refund", idRecord, null,
                function () {
                    // Success
                    formContext.data.refresh();
                    formContext.ui.setFormNotification("Värdekod skickad.", "INFO");
                },
                function (e, t) {
                    // Error
                    formContext.ui.setFormNotification("Någonting gick fel: " + e, "INFO");

                    // Write the trace log to the dev console
                    if (window.console && console.error) {
                        console.error(e + "\n" + t);
                    }
                });
        },

        printRefundVoucherReport: function (executionContext) {
            /// <summary>
            /// Print report. Hardcoded reports.
            /// </summary>
            try {
                var formContext = executionContext.getFormContext();

                var refundtypePrint = formContext.getAttribute("ed_refundtype_print").getValue();

                if (refundtypePrint == 0)
                    return;

                var refundPrintedDate = formContext.getAttribute("ed_voucher_printed").getValue();

                var msg = "Värdebeviset redan utskrivet. Skriva ut på nytt? (Ja/Nej)";

                if (refundPrintedDate != null) {
                    var answer = prompt(msg, "Ja");

                    if (answer == "Nej" || answer == "nej")
                        return;

                    // User pressed cancel or entered no value.
                    if (!answer)
                        return;

                    if (isNaN(answer))
                        return;
                }

                var refundId = formContext.data.entity.getId();
                refundId = refundId.substring(1, refundId.length - 1);

                // Generate Report as PDF
                var arrReportSession = Endeavor.Skanetrafiken.cgi_refund.executeReport();
                Endeavor.Skanetrafiken.cgi_refund.convertResponseToPDF(arrReportSession);
            }
            catch (error) {
                alert("Exception caught in printRefundVoucherReport.\r\n\r\n" + error.message);
            }
        },

        bulkPrintRefundVoucherReport: function () {
            /// <summary>
            /// Print report. Hardcoded reports.
            /// </summary>
            try {

                var yesterday = new Date();
                yesterday.setDate(yesterday.getDate() - 1);

                if (!yesterday.getDay())
                    yesterday.setDate(yesterday.getDate() - 2);

                var today = new Date();

                var dateFromString = prompt("Datum från:", yesterday.getFullYear() + "-" + ('0' + (yesterday.getMonth() + 1)).slice(-2) + "-" + ('0' + yesterday.getDate()).slice(-2));
                var dateToString = prompt("Datum till:", today.getFullYear() + "-" + ('0' + (today.getMonth() + 1)).slice(-2) + "-" + ('0' + today.getDate()).slice(-2));

                var dateFrom = Endeavor.Skanetrafiken.cgi_refund.getDateFromString(dateFromString);
                var dateTo = Endeavor.Skanetrafiken.cgi_refund.getDateFromString(dateToString);

                if (dateFrom && dateTo) {

                    var guids = [];
                    Xrm.WebApi.retrieveMultipleRecords("cgi_refund", "?$select=cgi_refundid&$filter=createdon gt '" + dateFrom.toISOString() + "' and createdon le '" + dateTo.toISOString() + "'").then(
                        function success(results) {

                            if (!results || results.entities.length == 0) {
                                Endeavor.formscriptfunctions.AlertCustomDialog("Inga värdebevis hittades.");
                            }
                            else {
                                for (var i = 0; i < results.entities.length; i++)
                                    guids.push(results.entities[i].cgi_refundid);

                                //var arrReportSession = Endeavor.Skanetrafiken.cgi_refund.executeReport();
                                //Endeavor.Skanetrafiken.cgi_refund.convertResponseToPDF(arrReportSession);

                                Endeavor.formscriptfunctions.AlertCustomDialog(i + " värdebevis utskrivna!")
                            }

                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
                else {
                    Endeavor.formscriptfunctions.AlertCustomDialog("Felaktigt datumformat.")
                }
            }
            catch (error) {
                alert("Exception caught in bulkPrintRefundVoucherReport.\r\n\r\n" + error.message);
            }
        },

        getDateFromString: function (datestring) {

            var formattedstring = "";

            for (var i = 0; i < datestring.length; i++) {
                if (Number.isNaN(parseInt(datestring[i])) == false) {
                    formattedstring = formattedstring + datestring[i];
                }
            }

            if (formattedstring.length < 8) {
                return null;
            }
            else {

                var yyyy = parseInt(formattedstring.substring(0, 4));
                var mm = parseInt(formattedstring.substring(4, 6)) - 1;
                var dd = parseInt(formattedstring.substring(6, 8));

                var date = new Date(yyyy, mm, dd, 0, 0, 0, 0);
                if (Number.isNaN(date.getTime())) {
                    date = new Date();
                }

                return date;
            }
        },

        executeReport: function () {
            // GUID of SSRS report in CRM.
            var reportGuid = "8a3e7a71-6504-e511-80dc-0050569010ad";

            //Name of the report. Note: .RDL needs to be specified.
            var reportName = "Beslut Vidarefakturering.rdl";

            var globalContext = Xrm.Utility.getGlobalContext();
            var orgUniqueName = globalContext.organizationSettings.uniqueName;

            // URL of the report server which will execute report and generate response.
            var pth = globalContext.getClientUrl() + "/CRMReports/rsviewer/QuirksReportViewer.aspx";

            //This is the filter that is passed to pre-filtered report. It passes GUID of the record using Xrm.Page.data.entity.getId() method.
            //This filter shows example for quote report. If you want to pass ID of any other entity, you will need to specify respective entity name.
            var reportPrefilter = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'><entity name='cgi_refund'><all-attributes /><filter type='and'><condition attribute='cgi_refundid' operator='eq' value='" + Xrm.Page.data.entity.getId() + "' /></filter></entity></fetch>";

            //Prepare query to execute report.
            var query = "id=%7B" + reportGuid +
                "%7D&uniquename=" + orgUniqueName
                + "&iscustomreport=true&reportnameonsrs=&reportName=" + reportName
                //+ "&isScheduledReport=false&p:cgi_refund=" + reportPrefilter;
                + "&isScheduledReport=false";

            //Prepare request object to execute the report.
            var retrieveEntityReq = new XMLHttpRequest();
            retrieveEntityReq.open("POST", pth, false);
            retrieveEntityReq.setRequestHeader("Accept", "*/*");
            retrieveEntityReq.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            //This statement runs the query and executes the report synchronously.
            retrieveEntityReq.send(query);

            //These variables captures the response and returns the response in an array.
            var x = retrieveEntityReq.responseText.lastIndexOf("ReportSession=");
            var y = retrieveEntityReq.responseText.lastIndexOf("ControlID=");

            var ret = new Array();
            ret[0] = retrieveEntityReq.responseText.substr(x + 14, 24);
            ret[1] = retrieveEntityReq.responseText.substr(x + 10, 32);

            //Returns the response as an Array.
            return ret;
        },

        convertResponseToPDF: function (arrResponseSession) {

            var globalContext = Xrm.Utility.getGlobalContext();

            //Create query string that will be passed to Report Server to generate PDF version of report response.
            var pth = globalContext.getClientUrl() + "/Reserved.ReportViewerWebControl.axd?ReportSession=" + arrResponseSession[0] + "&Culture=1033&CultureOverrides=True&UICulture=1033&UICultureOverrides=True&ReportStack=1&ControlID=" + arrResponseSession[1] + "&OpType=Export&FileName=Public&ContentDisposition=OnlyHtmlInline&Format=PDF";

            //Create request object that will be called to convert the response in PDF base 64 string.
            var retrieveEntityReq = new XMLHttpRequest();
            retrieveEntityReq.open("GET", pth, true);
            retrieveEntityReq.setRequestHeader("Accept", "*/*");
            retrieveEntityReq.responseType = "arraybuffer";

            retrieveEntityReq.onreadystatechange = function () { // This is the callback function.

                if (retrieveEntityReq.readyState == 4 && retrieveEntityReq.status == 200) {

                    var binary = "";
                    var bytes = new Uint8Array(this.response);

                    for (var i = 0; i < bytes.byteLength; i++)
                        binary += String.fromCharCode(bytes[i]);

                    //This is the base 64 PDF formatted string and is ready to pass to the action as an input parameter.
                    var base64PDFString = btoa(binary);

                    //4. Call Action and pass base 64 string as an input parameter. That’s it.
                    if (window.navigator && window.navigator.msSaveOrOpenBlob) { // IE workaround
                        var byteCharacters = atob(base64PDFString);
                        var byteNumbers = new Array(byteCharacters.length);
                        for (var i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        var byteArray = new Uint8Array(byteNumbers);
                        var blob = new Blob([byteArray], { type: 'application/pdf' });

                        Endeavor.Skanetrafiken.cgi_refund.openPrintDialogue(blob);

                        //// Saves the PDF to disk
                        //window.navigator.msSaveOrOpenBlob(blob, "testfile.pdf");
                    }
                    else { // much easier if not IE
                        var byteCharacters = atob(base64PDFString);
                        var byteNumbers = new Array(byteCharacters.length);
                        for (var i = 0; i < byteCharacters.length; i++) {
                            byteNumbers[i] = byteCharacters.charCodeAt(i);
                        }
                        var byteArray = new Uint8Array(byteNumbers);
                        var blob = new Blob([byteArray], { type: 'application/pdf' });

                        Endeavor.Skanetrafiken.cgi_refund.openPrintDialogue(blob);
                    }
                }
            };

            //This statement sends the request for execution asynchronously. Callback function will be called on completion of the request.
            retrieveEntityReq.send();
        },

        openPrintDialogue: function (blob) {

            var file = window.URL.createObjectURL(blob);

            // create new printframe
            var iFrame = $('<iframe></iframe>');
            iFrame
                .attr("id", "printframe")
                .attr("name", "printframe")
                .attr("src", "about:blank")
                .css("width", "0")
                .css("height", "0")
                .css("position", "absolute")
                .css("left", "-9999px")
                .appendTo($("body:first"));

            // load printframe
            if (iFrame != null && file != null) {
                iFrame.attr('src', file);
                iFrame.load(function () {

                    // print the frame
                    var tempFrame = $('#printframe')[0];
                    var tempFrameWindow = tempFrame.contentWindow ? tempFrame.contentWindow : tempFrame.contentDocument.defaultView;

                    tempFrameWindow.onbeforeprint = function () {
                        alert('This will be called before the user prints.');
                    };
                    tempFrameWindow.onafterprint = function () {
                        alert('This will be called after the user prints');
                    };

                    tempFrameWindow.focus();
                    tempFrameWindow.print();
                });
            }
        },

        //Form Methods CGI Refund (from refundLibrary.js)
        onFormLoad: function (executionContext) {
            var formContext = executionContext.getFormContext();

            if (formContext.ui.getFormType() == 0) {
                setTimeout(Endeavor.Skanetrafiken.cgi_refund.onFormLoad(), 1000);
                return;
            }

            switch (formContext.ui.getFormType()) {
                case FORM_TYPE_CREATE:

                    Endeavor.Skanetrafiken.cgi_refund.getCaseNumber(formContext);
                    Endeavor.Skanetrafiken.cgi_refund.setDefaultValues(formContext);
                    Endeavor.Skanetrafiken.cgi_refund.ctrlAttest(formContext);
                    Endeavor.Skanetrafiken.cgi_refund.ctrlAmountLimit(formContext);

                    break;
                case FORM_TYPE_UPDATE:
                    Endeavor.Skanetrafiken.cgi_refund.refund_Onload(formContext);
                    Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnLoad(formContext);
                    //Endeavor.Skanetrafiken.cgi_refund.ctrlAttest();
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

            Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundtypeproductnotrequiredidSetting(formContext);
        },

        onFormSave: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var eventArgs = executionContext.getEventArgs();

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
                var _check_soc = Endeavor.Skanetrafiken.cgi_refund.SocSecNoOnChange(executionContext);
                if (_check_soc == false) {
                    eventArgs.preventDefault();
                }
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        getContactAccount: function (formContext) {
            try {
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                if (_caseid != null) {
                    var caseidClean = Endeavor.formscriptfunctions.cleanIdField(_caseid);
                    Endeavor.OData_Querys.GetContactAccount(caseidClean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.GetContactAccount\n\n" + e.message);
            }
        },

        GetContactAccount_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Du kan inte spara beslut utan kopplling till ärende!");
                }
                else {
                    var _cgi_app_value = "0";
                    formContext.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                    var _cgi_contact = result["_cgi_contactid_value"];
                    var _cgi_account = result["_cgi_accountid_value"];

                    if (_cgi_account == null && _cgi_contact == null) {
                        _cgi_app_value = "1";
                        formContext.getAttribute("cgi_mandcust").setValue(_cgi_app_value);
                        alert("Ni kan inte skapa ett beslut utan att ange privatkund eller organisationskund.");
                    }
                }
            }
            catch (e) {
                alert("Fel i GetContactAccount_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ctrlAttest: function (formContext) {
            try {
                var globalContext = Xrm.Utility.getGlobalContext();

                var userId = globalContext.userSettings.userId;
                var userIdClean = null;
                if (userId != null)
                {
                    userIdClean = Endeavor.formscriptfunctions.cleanIdField(userId);
                    Endeavor.OData_Querys.GetRSID(userIdClean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.ctrlAttest\n\n" + e.message);
            }
        },

        ctrlAttest_callback: function (result, formContext) {
            try {
                if (result == null) {
                    alert("Användaren hittas inte!");
                }
                else {
                    var _cgi_RSID = result["cgi_rsid"];
                    if (_cgi_RSID == null)
                        formContext.getAttribute("cgi_auth_approved").setValue(false);
                    else
                        formContext.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
            catch (e) {
                alert("Fel i ctrlAttest_callback\n\n" + e.message);
            }
        },

        ctrlAmountLimit: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetAmountLimitFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.ctrlAmountLimit\n\n" + e.message);
            }
        },

        ctrlAmountLimit_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Inställning hittas inte!");
                }
                else {
                    var _cgi_amount_limit = result.entities[0]["cgi_amount_limit"];
                    var _cgi_amount_limit_warn = result.entities[0]["cgi_amount_limit_warn"];

                    var __cgi_amount_limit = parseFloat(_cgi_amount_limit);
                    var __cgi_amount_limit_warn = parseFloat(_cgi_amount_limit_warn);

                    formContext.getAttribute("cgi_amount_above_limit").setValue(__cgi_amount_limit);
                    formContext.getAttribute("cgi_amount_limit_warn").setValue(__cgi_amount_limit_warn);
                }
            }
            catch (e) {
                alert("Fel i ctrlAmountLimit_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        SetProductFieldRequired: function (formContext) {

            var ProductRequired = false;

            if (_ReimbursementFormIdCache != null) {
                if (_ReimbursementFormIdCache["cgi_useproduct"] != null) {
                    var _useproduct = _ReimbursementFormIdCache["cgi_useproduct"];
                    if (_useproduct == false) {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                        formContext.getAttribute("cgi_productid").setValue(null);
                    }
                    else {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", true, formContext);
                        ProductRequired = true;
                        var _Currentcgi_accountid = Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                        var _Currentcgi_refundtypeid = Endeavor.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);

                        if (_Currentcgi_refundtypeid != null) {//Check that cgi_refundtypeid is set
                            if (_Currentcgi_accountid != null) {//Check that cgi_accountid is set
                                //both cgi_refundtypeid and cgi_accountid is set. Then Check the following conditions
                                if (_RefundTypeProductNotrequiredIdCache != null) { //ensure the setting is loaded
                                    if ('{' + _RefundTypeProductNotrequiredIdCache.toUpperCase() + '}' == _Currentcgi_refundtypeid) {//check if cgi_refundtypeid matches with setting
                                        if (_RefundAccountIdCache != null) {//ensure refund account number is loaded for _Currentcgi_accountid
                                            if (_Currentcgi_accountid == _RefundAccountIdCache["cgi_refundaccountid"] || _Currentcgi_accountid == '{' + _RefundAccountIdCache["cgi_refundaccountid"].toUpperCase() + '}') {//ensure that the loaded refund account number matches _Currentcgi_accountid
                                                if (_RefundAccountIdCache["cgi_account"].substring(0, 1) == "2")//check if cgi_account starts with 2
                                                    ProductRequired = false;
                                            }
                                            else
                                                Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundaccountNumber(formContext);
                                        }
                                        else
                                            Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundaccountNumber(formContext);
                                    }
                                }
                                else
                                    Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundtypeproductnotrequiredidSetting(formContext);
                            }
                            else
                                _RefundAccountIdCache = null; //ensure the cached value is deleted
                        }
                    }
                }
                else {
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                    formContext.getAttribute("cgi_productid").setValue(null);
                }
            }
            else {
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                formContext.getAttribute("cgi_productid").setValue(null);
            }

            if (ProductRequired)
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_productid", "required", formContext);
            else
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_productid", "none", formContext);
        },

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

        Getcgi_refundaccountNumber: function (formContext) {

            try {
                var _refundaccountid = Endeavor.formscriptfunctions.GetLookupid("cgi_accountid", formContext);
                if (_refundaccountid != null) {
                    var refundaccountidClean = Endeavor.formscriptfunctions.cleanIdField(_refundaccountid);
                    Endeavor.OData_Querys.Getcgi_refundaccountNumber(refundaccountidClean, formContext);
                }
                else {
                    _RefundAccountIdCache = null;
                    Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundaccountNumber\n\n" + e.message);
            }
        },

        Getcgi_refundaccountNumber_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Failed to retrieve RefundAccountNumber!");
                }
                else {
                    _RefundAccountIdCache = result.entities[0];
                    Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);
                }
            }
            catch (e) {
                alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        cgi_accountidOnChange: function (executionContext) {
            var formContext = executionContext.getFormContext();
            Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundaccountNumber(formContext);
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Getcgi_refundtypeproductnotrequiredidSetting: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.Getcgi_refundtypeproductnotrequiredidSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.Getcgi_refundtypeproductnotrequiredidSetting\n\n" + e.message);
            }
        },

        Getcgi_refundtypeproductnotrequiredidSetting_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Required setting is missing: cgi_refundtypeproductnotrequiredid!");
                }
                else {
                    if (result.entities[0]["_cgi_refundtypeproductnotrequiredid_value"] == null)
                        alert('Required setting is missing: cgi_refundtypeproductnotrequiredid!');
                    else {
                        _RefundTypeProductNotrequiredIdCache = result.entities[0]["_cgi_refundtypeproductnotrequiredid_value"];
                        Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);
                    }
                }
            }
            catch (e) {
                alert("Fel i Getcgi_refundtypeproductnotrequiredidSetting_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        SocSecNoOnChange: function (executionContext) {
            var formContext = executionContext.getFormContext();

            var _soc = formContext.getAttribute("cgi_soc_sec_number").getValue();
            var _sectrue = Endeavor.Skanetrafiken.cgi_refund.validatePersonalNumber(_soc);
            var _return_save = true;

            if (_sectrue == false) {
                alert("Personnummer är inte giltigt.");
                formContext.getControl("cgi_soc_sec_number").setFocus();
                _return_save = false;
            }
            else {
                var _soc_trim = _soc.replace('-', '');
                var _soc_trim_length = _soc_trim.length;
                if (_soc_trim_length != 12) {
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
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                if (_caseid != null) {
                    var _caseid_clean = Endeavor.formscriptfunctions.cleanIdField(_caseid);
                    Endeavor.OData_Querys.GetCaseNumber(_caseid_clean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.getCaseNumber\n\n" + e.message);
            }
        },

        getCaseNumber_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Hittar inga ärendenummer!");
                }
                else {
                    if (result.entities[0]["ticketnumber"] != null) {
                        var _caseidvalue = result.entities[0]["ticketnumber"];
                        Endeavor.formscriptfunctions.SetValue("cgi_refundnumber", _caseidvalue, formContext);
                    }
                    _travelcardnumber = result.entities[0]["cgi_unregisterdtravelcard"];
                    //MaxP 2016-03-24 
                    var _caseorigincode = result.entities[0]["caseorigincode"];
                    var _casetypecode = result.entities[0]["casetypecode"];
                    var _iscompleted = result.entities[0]["cgi_iscompleted"];
                    var _taxiclaimedamount = result.entities[0]["cgi_taxiclaimedamount"];
                    var _milagekilometers = result.entities[0]["cgi_milagekilometers"];
                    var _compensationclaimfromrgol = result.entities[0]["cgi_compensationclaimfromrgol"];

                    if (_caseorigincode == 285050007 && _casetypecode == 285050003 && _iscompleted == false) {

                        var _id = result.entities[0]["_cgi_refundtypes_value"];
                        var _logicalname = result.entities[0]["_cgi_refundtypes_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result.entities[0]["_cgi_refundtypes_value@OData.Community.Display.V1.FormattedValue"];
                        Endeavor.formscriptfunctions.SetLookup("cgi_refundtypeid", _logicalname, _id, _name, formContext);
                        Endeavor.Skanetrafiken.cgi_refund.refund_Onload(formContext);

                        if (_milagekilometers != null && _milagekilometers != 0) {
                            var _milagekilometers_decimal = parseFloat(_milagekilometers / 10);
                            formContext.getAttribute("cgi_milage").setValue(_milagekilometers_decimal);

                            var _cgi_amount = parseFloat(Math.round(_milagekilometers_decimal * 18.50));
                            var _rgolclaimedamount = result.entities[0]["cgi_compensationclaimfromrgol"];
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

                        var _id = result.entities[0]["_cgi_refundreimbursementform_value"];
                        var _logicalname = result.entities[0]["_cgi_refundreimbursementform_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result.entities[0]["_cgi_refundreimbursementform_value@OData.Community.Display.V1.FormattedValue"];
                        Endeavor.formscriptfunctions.SetLookup("cgi_reimbursementformid", _logicalname, _id, _name, formContext);

                        Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnChange(null, formContext);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.getCaseNumber_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        reimbursementformid_OnLoad: function (formContext) {

            try {

                var _refundtypeid = Endeavor.formscriptfunctions.GetLookupid("cgi_reimbursementformid", formContext);
                
                if (_refundtypeid != null) {
                    var _refundtypeidclean = Endeavor.formscriptfunctions.cleanIdField(_refundtypeid);
                    Endeavor.OData_Querys.GetReimbursementFormOnLoad(_refundtypeidclean, formContext);
                }
                else {
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_Onload\n\n" + e.message);
            }
        },

        reimbursementformid_OnLoad_callback: function (result, formContext) {

            try {

                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Hittar inga inställningar ()!");
                }
                else {
                    _ReimbursementFormIdCache = result.entities[0];
                    //Use account
                    if (result.entities[0]["cgi_useaccount"] != null) {
                        var _useaccount = result.entities[0]["cgi_useaccount"];

                        if (_useaccount == false) {
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                        }
                        else {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true, formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required", formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required", formContext);
                        }
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                    }

                    //Use responsible 
                    if (result.entities[0]["cgi_useresponsible"] != null) {
                        var _usereponsible = result.entities[0]["cgi_useresponsible"];
                        if (!_usereponsible) {
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                        }
                        else {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true, formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required", formContext);
                        }
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                    }

                    //Product
                    Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);

                    //Field Travelcard Number          
                    var _loadcard = result.entities[0]["cgi_loadcard"];

                    if (_loadcard) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
                    }

                    //Social Security Number
                    var _cgi_payment = result.entities[0]["cgi_payment"];

                    if (_cgi_payment) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true, formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                    }

                    //IBAN and SWIFT and accountnumber
                    var _cgi_payment_abroad = result.entities[0]["cgi_payment_abroad"];

                    if (_cgi_payment_abroad) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "required", formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                    }

                    //Last valid
                    var _cgi_last_valid = result.entities[0]["cgi_time_valid"];

                    if (_cgi_last_valid) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnLoad_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        reimbursementformid_OnChange: function (executionContext, formContext) {
            try {
                if (executionContext != null)
                    formContext = executionContext.getFormContext();

                var _refundtypeid = Endeavor.formscriptfunctions.GetLookupid("cgi_reimbursementformid", formContext);

                if (_refundtypeid != null) {
                    var _refundtypeidClean = Endeavor.formscriptfunctions.cleanIdField(_refundtypeid);
                    Endeavor.OData_Querys.GetReimbursementForm(_refundtypeidClean, formContext);
                }
                else {
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                    formContext.getAttribute("cgi_accountid").setValue(null);

                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                    formContext.getAttribute("cgi_responsibleid").setValue(null);

                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                    formContext.getAttribute("cgi_productid").setValue(null);

                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                    formContext.getAttribute("cgi_vat_code").setValue(null);

                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnChange\n\n" + e.message);
            }
        },

        reimbursementformid_OnChange_callback: function (result, formContext) {
            try {

                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Hittar inga inställningar ()!");
                }
                else {
                    _ReimbursementFormIdCache = result.entities[0];

                    //Use attestation
                    if (result.entities[0]["cgi_attestation"] != null) {
                        var _useattestation = result.entities[0]["cgi_attestation"];

                        if (_useattestation)
                            formContext.getAttribute("cgi_attest_req").setValue("1");
                        else
                            formContext.getAttribute("cgi_attest_req").setValue("0");
                    }
                    else
                        formContext.getAttribute("cgi_attest_req").setValue("0");

                    //Use account
                    if (result.entities[0]["cgi_useaccount"] != null) {
                        var _useaccount = result.entities[0]["cgi_useaccount"];

                        if (!_useaccount) {
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                            formContext.getAttribute("cgi_accountid").setValue(null);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                            formContext.getAttribute("cgi_vat_code").setValue(null);
                        }
                        else {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", true, formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", true, formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "required", formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "required", formContext);
                        }
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                        formContext.getAttribute("cgi_accountid").setValue(null);
                    }

                    //Use responsible 
                    if (result.entities[0]["cgi_useresponsible"] != null) {
                        var _usereponsible = result.entities[0]["cgi_useresponsible"];
                        if (!_usereponsible) {
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                            formContext.getAttribute("cgi_responsibleid").setValue(null);
                        }
                        else {
                            Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", true, formContext);
                            Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "required", formContext);
                        }
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                        formContext.getAttribute("cgi_responsibleid").setValue(null);
                    }

                    //Product
                    Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);

                    //Field Email
                    Endeavor.Skanetrafiken.cgi_refund.SetEmailVisibilityAndRequirement(result, formContext);

                    //Field Travelcard Number          
                    var _loadcard = result.entities[0]["cgi_loadcard"];

                    if (_loadcard ) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_travelcard_number", _travelcardnumber, formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
                    }

                    //Social Security Number with get social security number from incident
                    var _cgi_payment = result.entities[0]["cgi_payment"];

                    if (_cgi_payment) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", true, formContext);
                        Endeavor.Skanetrafiken.cgi_refund.setSocSecNumber(formContext);
                        Endeavor.Skanetrafiken.cgi_refund.ctrlAmountLimit(formContext);
                        _show_soc_sec = 1;

                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                        _show_soc_sec = 0;
                    }

                    //IBAN and SWIFT and accountnumber
                    var _cgi_payment_abroad = result.entities[0]["cgi_payment_abroad"];

                    if (_cgi_payment_abroad) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "required", formContext);

                        //MaxP 2016-04-20
                        Endeavor.Skanetrafiken.cgi_refund.SetBicIban(formContext);
                        Endeavor.Skanetrafiken.cgi_refund.ctrlAmountLimit(formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                    }

                    //Last valid
                    var _cgi_last_valid = result.entities[0]["cgi_time_valid"];

                    if (_cgi_last_valid) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "required", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
                        Endeavor.Skanetrafiken.cgi_refund.SetDateValidField(formContext);
                    }
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.reimbursementformid_OnChange_callback\n\n" + e.message);
            }
        },

        SetEmailVisibilityAndRequirement: function (result, formContext) {
            debugger;
            var _emailRequiredAndVisible = result.entities[0].cgi_couponemail;
            var _phoneRequired = result.entities[0].cgi_couponsms;
            //var _reimbursementName = result.entities[0].cgi_reimbursementname;

            //switch (_reimbursementName) {
            //    case "Laddning reskassa":
            //    case "Rabattkod Jojo Online": //The record's name in UTV
            //    case "Rabattkod e-handel": //The record's name in PROD
            //    //case "Värdekod - E-post":
            //    case "Värdekod E-post - Försening":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    case "Värdekod E-post - Försening (Saldo)":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    //case "Värdekod - E-post (ersättning)":
            //    case "Värdekod E-post - Ersättning":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    case "Värdekod E-post - Ersättning (Saldo)":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    //case "Värdekod - E-post (byta reskassa)":
            //    case "Värdekod E-post - Saldo":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    case "Värdekod SMS - Försening":
            //        _phoneRequired = true;
            //        break;
            //    case "Värdekod SMS - Försening (Saldo)":
            //        _phoneRequired = true;
            //        break;
            //    case "Värdekod SMS - Ersättning":
            //        _phoneRequired = true;
            //        break;
            //    case "Värdekod SMS - Ersättning (Saldo)":
            //        _phoneRequired = true;
            //        break;
            //    case "Värdekod SMS - Saldo":
            //        _phoneRequired = true;
            //        break;
            //    case "Värdebevis - SMS":
            //    case "Värdebevis - e-post":
            //        _emailRequiredAndVisible = true;
            //        break;
            //    default:
            //        _emailRequiredAndVisible = false;
            //}

            var _sendtostralfors = result.entities[0].cgi_sendtostralfors;

            if (_sendtostralfors)
                _emailRequiredAndVisible = true;

            if (_phoneRequired) {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);
                Endeavor.Skanetrafiken.cgi_refund.setMobileNumber(formContext);
            }
            else {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);
            }

            if (_emailRequiredAndVisible) {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);
                Endeavor.Skanetrafiken.cgi_refund.setEmail(formContext);
            }
            else {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
            }
        },

        ReInvoice_OnChange: function (executionContext) {
            var formContext = executionContext.getFormContext();
            _show_reinvoice = formContext.getAttribute("cgi_reinvoicing").getValue()

            if (_show_reinvoice) {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true, formContext);
            }
            else {
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_invoicerecipient", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
            }
        },

        ShowTravelcardNumber: function (executionContext) {

            try {
                var formContext = executionContext.getFormContext();
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.ShowTravelcardNumber\n\n" + e.message);
            }
        },

        SetReinvoicingFALSE: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reference", false, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.SetReinvoicingFALSE\n\n" + e.message);
            }
        },

        SetReinvoicingTRUE: function (executionContext) {
            try {
                var formContext = executionContext.getFormContext();

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", true, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reference", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.SetReinvoicingTRUE\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        setFieldToEditable: function (formContext) {
            try {
                Endeavor.formscriptfunctions.SetState("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_calculated_amount", false, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setFieldToEditable\n\n" + e.message);
            }
        },

        refund_Onload: function (formContext) {
            try {
                var _refundtypeid = Endeavor.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);
                if (_refundtypeid != null) {
                    var refundidClean = Endeavor.formscriptfunctions.cleanIdField(_refundtypeid);
                    Endeavor.OData_Querys.GetRefundSettingOnLoad(refundidClean, formContext);
                }
                else 
                    Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeid_Onload\n\n" + e.message);
            }
        },

        refundtypeid_Onload_callback: function (result, formContext) {
            try {
                if (result == null || result.statecode != 0) {
                    alert("Hittar inga inställningar (ersättningsbeslut)!");
                }
                else {

                    //Field amount editable
                    Endeavor.Skanetrafiken.cgi_refund.setFieldToEditable(formContext);

                    //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                    var _cgi_FinancialTransaction = result["cgi_financialtransaction"];
                    if (!_cgi_FinancialTransaction) {
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);
                    }
                    else {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
                    }

                    //Set default account
                    if (result["_cgi_refundaccountid_value"] != null) {
                        var _id = result["_cgi_refundaccountid_value"];
                        var _logicalname = result["_cgi_refundaccountid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundaccountid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name, formContext);
                    }

                    if (result["_cgi_refundresponsibleid_value"] != null) {
                        //Set default responsible
                        var _id = result["_cgi_refundresponsibleid_value"];
                        var _logicalname = result["_cgi_refundresponsibleid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundresponsibleid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name, formContext);
                    }

                    if (result["_cgi_refundproductid_value"] != null) {
                        //Set default product
                        var _id = result["_cgi_refundproductid_value"];
                        var _logicalname = result["_cgi_refundproductid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundproductid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name, formContext);
                    }

                    if (result["cgi_refundoption"] != null) {
                        var _refundoptionvalue = result["cgi_refundoption"];

                        if (_refundoptionvalue == 285050000)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);

                        if (_refundoptionvalue == 285050001)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeQUANTITY(formContext);

                        if (_refundoptionvalue == 285050002)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeMONEY(formContext);

                        if (_refundoptionvalue == 285050003)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeMILAGE(formContext);

                        if (_refundoptionvalue == 285050004)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeTAXI(formContext);

                        if (_refundoptionvalue == 285050005)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeOTHER(formContext);

                        if (_refundoptionvalue == 285050006)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeFORWARD(formContext);

                        ////// MOSCGI
                        if (_refundoptionvalue == 285050008)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPONSMS(formContext);
                        if (_refundoptionvalue == 285050009)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPONEMAIL(formContext);
                        ////// END MOSCGI

                        if (_refundoptionvalue == 285050010 || _refundoptionvalue == 285050012 || _refundoptionvalue == 285050014)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODESMS(formContext);

                        if (_refundoptionvalue == 285050011 || _refundoptionvalue == 285050013 || _refundoptionvalue == 285050015)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODEEMAIL(formContext);
                    }
                    else
                        Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);

                    //Exeption onload event
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", true, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeid_Onload_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        refundtypeid_OnChange: function (executionContext) {

            var formContext = executionContext.getFormContext();
            Endeavor.Skanetrafiken.cgi_refund.SetProductFieldRequired(formContext);

            try {
                var _refundtypeid = Endeavor.formscriptfunctions.GetLookupid("cgi_refundtypeid", formContext);
                if (_refundtypeid != null) {
                    var refundidClean = Endeavor.formscriptfunctions.cleanIdField(_refundtypeid);
                    Endeavor.OData_Querys.GetRefundSetting(refundidClean, formContext);
                }
                else
                    Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);

                var _refundtypename = Endeavor.formscriptfunctions.GetLookupName("cgi_refundtypeid", formContext);
                if (_refundtypename == "Byte reskassa")
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", true, formContext);
                else
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeid_OnChange\n\n" + e.message);
            }
        },

        refundtypeid_OnChange_callback: function (result, formContext) {
            try {
                if (result == null || result.statecode != 0) {
                    alert("Hittar inga inställningar (ersättningsbeslut)!");
                }
                else {
                    //The field cgi_FinancialTransaction on refundtype defines if reimbursementformid shall show
                    var _cgi_FinancialTransaction = result["cgi_financialtransaction"];
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);

                    if (_cgi_FinancialTransaction == false) {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);
                        //MaxP 2016-07-25 The field cgi_FinancialTransaction on refundtype defines that authorization-rights is not needed
                        formContext.getAttribute("cgi_attest_req").setValue("0");
                        //Social security is not needed
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                        _show_soc_sec = 0;
                        // Iban and BIC is not needed
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                        Endeavor.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                        //Product is not needed
                        //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_productid", "none");
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                        formContext.getAttribute("cgi_productid").setValue(null);
                    }
                    else {
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true, formContext);
                        Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "none", formContext);
                    }

                    //The field cgi_reinvoice defines if its possible to reinvoice reimbursement
                    var _cgi_reinvoice = result["cgi_reinvoice"];
                    if (_cgi_reinvoice)
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", true, formContext);
                    else
                        Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);

                    //Set no attestation
                    formContext.getAttribute("cgi_attestation").setValue(285050004);

                    //Emty values in all fields
                    Endeavor.Skanetrafiken.cgi_refund.setEmptyAllValues(formContext);

                    //Field amount editable
                    Endeavor.Skanetrafiken.cgi_refund.setFieldToEditable(formContext);

                    //Hide cgi_iban, cgi_swift, cgi_accountno, cgi_foreign_account, if refundtype changes
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);

                    Endeavor.formscriptfunctions.SetValue("cgi_soc_sec_number", "", formContext);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_soc_sec_number", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);

                    Endeavor.formscriptfunctions.SetValue("cgi_accountno", "", formContext);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountno", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);

                    Endeavor.formscriptfunctions.SetValue("cgi_iban", "", formContext);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_iban", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);

                    Endeavor.formscriptfunctions.SetValue("cgi_swift", "", formContext);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_swift", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);

                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_accountid", "none", formContext);
                    formContext.getAttribute("cgi_accountid").setValue(null);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);

                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_vat_code", "none", formContext);
                    formContext.getAttribute("cgi_vat_code").setValue(null);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);

                    formContext.getAttribute("cgi_responsibleid").setValue(null);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_responsibleid", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);

                    formContext.getAttribute("cgi_productid").setValue(null);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_productid", "none", formContext);
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);

                    //Set default account
                    if (result["_cgi_refundaccountid_value"] != null) {
                        var _id = result["_cgi_refundaccountid_value"];
                        var _logicalname = result["_cgi_refundaccountid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundaccountid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_accountid", _logicalname, _id, _name, formContext);
                    }

                    //Set default responsible
                    if (result["_cgi_refundresponsibleid_value"] != null) {
                        var _id = result["_cgi_refundresponsibleid_value"];
                        var _logicalname = result["_cgi_refundresponsibleid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundresponsibleid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_responsibleid", _logicalname, _id, _name, formContext);
                    }

                    //Set default product
                    if (result["_cgi_refundproductid_value"] != null) {
                        var _id = result["_cgi_refundproductid_value"];
                        var _logicalname = result["_cgi_refundproductid_value@Microsoft.Dynamics.CRM.lookuplogicalname"];
                        var _name = result["_cgi_refundproductid_value@OData.Community.Display.V1.FormattedValue"];

                        Endeavor.formscriptfunctions.SetLookup("cgi_productid", _logicalname, _id, _name, formContext);
                    }

                    if (result["cgi_refundoption"] != null) {
                        var _refundoptionvalue = result["cgi_refundoption"];

                        if (_refundoptionvalue == 285050000)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);

                        if (_refundoptionvalue == 285050001)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeQUANTITY(formContext);

                        if (_refundoptionvalue == 285050002)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeMONEY(formContext);

                        if (_refundoptionvalue == 285050003)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeMILAGE(formContext);

                        if (_refundoptionvalue == 285050004)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeTAXI(formContext);

                        if (_refundoptionvalue == 285050005)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeOTHER(formContext);

                        if (_refundoptionvalue == 285050006)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeFORWARD(formContext);

                        if (_refundoptionvalue == 285050007)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeTRAVEL(formContext);

                        ////// MOSCGI
                        if (_refundoptionvalue == 285050008)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPONSMS(formContext);
                        if (_refundoptionvalue == 285050009)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPONEMAIL(formContext);
                        ////// END MOSCGI

                        if (_refundoptionvalue == 285050010)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODESMS(formContext);

                        if (_refundoptionvalue == 285050011)
                            Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODEEMAIL(formContext);
                    }
                    else
                        Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE(formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeid_OnChange_callback\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        setDefaultValues: function (formContext) {

            try {

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_productid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_responsibleid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_vat_code", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_soc_sec_number", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_accountno", false, formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_foreign_payment", false);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_iban", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_swift", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                //GISweden.formscriptfunctions.HideOrDisplayField("cgi_value_code", false);

                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);

                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_refundnumber", false, formContext);

                ////////// MOSCGI
                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
                ////////// END MOSCGI

                Endeavor.Skanetrafiken.cgi_refund.getContactAccount(formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setDefaultValues\n\n" + e.message);
            }
        },

        refundtypeNONE: function (formContext) {

            try {

                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reinvoicing", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_invoicerecipient", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);

                ////////// MOSCGI
                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
                ////////// END MOSCGI
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeNONE\n\n" + e.message);
            }
        },

        refundtypeQUANTITY: function (formContext) {

            try {

                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", true, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeQUANTITY\n\n" + e.message);
            }
        },

        refundtypeMILAGE: function (formContext) {

            try {

                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "required", formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", true, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");

                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);

                Endeavor.Skanetrafiken.cgi_refund.setMilageCompensation(formContext);

                Endeavor.formscriptfunctions.SetState("cgi_milage_compensation", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_calculated_amount", true, formContext);
                //Endeavor.formscriptfunctions.SetSubmitModeAlways("cgi_amount"); Emma removed
                Endeavor.formscriptfunctions.SetSubmitModeAlways("cgi_calculated_amount", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeMILAGE\n\n" + e.message);
            }
        },

        refundtypeTAXI: function (formContext) {

            try {

                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeTAXI\n\n" + e.message);
            }
        },

        refundtypeOTHER: function (formContext) {
            try {
                //Biocheck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);

                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");

                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeOTHER\n\n" + e.message);
            }
        },

        refundtypeFORWARD: function (formContext) {

            try {

                //Biockeck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", true, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeFORWARD\n\n" + e.message);
            }
        },

        refundtypeMONEY: function (formContext) {

            try {

                //Biockeck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                var _refundtypename = Endeavor.formscriptfunctions.GetLookupName("cgi_refundtypeid", formContext);
                if (_refundtypename != "Byte reskassa") {
                    Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);
                    Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                }

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);

                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_reimbursementformid", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeMONEY\n\n" + e.message);
            }
        },

        refundtypeTRAVEL: function (formContext) {

            try {

                //Biockeck
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_checknumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_checknumber", false, formContext);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", true);
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amountwithtax", "required");
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_reimbursementformid", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeMONEY\n\n" + e.message);
            }
        },

        ////////// MOSCGI
        refundtypeCOUPONSMS: function (formContext) {
            try {

                //Biocheck
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPON\n\n" + e.message);
            }
        },

        refundtypeVALUECODESMS: function (formContext) {
            try {

                //Biocheck
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", true, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODESMS\n\n" + e.message);
            }
        },

        refundtypeCOUPONEMAIL: function (formContext) {
            try {

                //Biocheck
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeCOUPONEMAIL\n\n" + e.message);
            }
        },

        refundtypeVALUECODEEMAIL: function (formContext) {
            try {

                //Biocheck
                //Endeavor.formscriptfunctions.SetRequiredLevel("cgi_check_number", "none");
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_check_number", false);

                //Kortnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_travelcard_number", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_travelcard_number", false, formContext);

                //Vidarebefordran
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_transportcompanyid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_transportcompanyid", false, formContext);

                //Taxi
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_taxi_company", false, formContext);

                //Milers�ttning
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_car_reg", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_car_reg", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_milage", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_last_valid", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_last_valid", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_milage_compensation", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_quantity", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_quantity", false, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_amount", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_calculated_amount", false, formContext);
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_calculated_amount", "none", formContext);
                //Endeavor.formscriptfunctions.HideOrDisplayField("cgi_amountwithtax", false);

                //Mobilnummer
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_mobilenumber", "none", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_mobilenumber", false, formContext);

                //Epost
                Endeavor.formscriptfunctions.SetRequiredLevel("cgi_email", "required", formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_email", true, formContext);
                Endeavor.formscriptfunctions.HideOrDisplayField("cgi_comments", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.refundtypeVALUECODEEMAIL\n\n" + e.message);
            }
        },
        ////////// END MOSCGI

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Set milage compensation
        setMilageCompensation: function (formContext) {
            try {
                var _currentdate = Endeavor.formscriptfunctions.GetDateTime();
                Endeavor.OData_Querys.GetDefaultMilageContributionFromSetting(_currentdate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setMilageCompensation\n\n" + e.message);
            }
        },

        setMilageContributionOnLoad_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Det finns ingen milersättning definierad!");
                }
                else {
                    var _milage_contribution = result.entities[0]["cgi_milage_contribution"];
                    _milage_contribution_decimal = parseFloat(_milage_contribution);
                    Endeavor.formscriptfunctions.SetValue("cgi_milage_compensation", _milage_contribution_decimal, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setMilageCompensation_callback\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Set valid date for compensation
        SetDateValidField: function (formContext) {
            try {
                //Changed to add 24 moths instead of 12 (DevOps - 8059)
                var futureDate = Endeavor.Skanetrafiken.cgi_refund.getFutureDateByAddingMonths(24);
                Endeavor.formscriptfunctions.SetValue("cgi_last_valid", futureDate, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.SetDateValidField\n\n" + e.message);
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
            return Endeavor.Skanetrafiken.cgi_refund.AddMonthsToDate(todaysDate, months);
        },

        AddMonthsToDate: function (date, months) {
            //create new date by adding the months
            return new Date(new Date(date).setMonth(date.getMonth() + months));
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Set BIC and IBAN
        SetBicIban: function (formContext) {
            try {
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                var _caseidClean = null;

                if (_caseid != null)
                {
                    _caseidClean = Endeavor.formscriptfunctions.cleanIdField(_caseid);
                    Endeavor.OData_Querys.GetBicIban(_caseidClean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.SetBicIban\n\n" + e.message);
            }
        },

        setBicIban_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    //alert("Det finns inget bankkonto angivet på ärendet");
                }
                else {
                    var _cgi_bic = result.entities[0]["cgi_bic"];
                    var _cgi_iban = result.entities[0]["cgi_iban"];

                    Endeavor.formscriptfunctions.SetValue("cgi_iban", _cgi_iban, formContext);
                    Endeavor.formscriptfunctions.SetValue("cgi_swift", _cgi_bic, formContext);
                }
            }
            catch (e) {
                alert("Fel i SetBicIbanOnLoad_callback\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Set social security number
        setSocSecNumber: function (formContext) {
            try {
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                var caseidClean = null;
                if (_caseid != null) {
                    caseidClean = Endeavor.formscriptfunctions.cleanIdField(_caseid);
                    Endeavor.OData_Querys.GetSocSecNumber(caseidClean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.SocSecNumber\n\n" + e.message);
            }
        },

        setSocSecOnLoad_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    //alert("Det finns inget bankkonto angivet p� �rendet");
                }
                else {
                    var _cgi_soc_sec_number = result.entities[0]["cgi_soc_sec_number"];
                    var _cgi_rgol_socialsecuritynumber = result.entities[0]["cgi_rgol_socialsecuritynumber"];
                    var _cgi_iscompleted = result.entities[0]["cgi_iscompleted"];

                    if (_cgi_rgol_socialsecuritynumber != null && _cgi_iscompleted == false)
                        Endeavor.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_rgol_socialsecuritynumber, formContext);
                    else
                        Endeavor.formscriptfunctions.SetValue("cgi_soc_sec_number", _cgi_soc_sec_number, formContext);
                }
            }
            catch (e) {
                alert("Fel i setSocSecOnLoad_callback\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Get email
        setEmail: function (formContext) {
            try {
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                var _caseid_clean = Endeavor.formscriptfunctions.cleanIdField(_caseid);

                Endeavor.OData_Querys.GetEmailAddress(_caseid_clean, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.getEmail\n\n" + e.message);
            }
        },

        getGetEmailAddress_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    //alert("Det finns ingen e-postadress angivet på ärendet");
                }
                else {
                    //MaxP 2016-05-11 Om det är ett manuellt ärende från RGOL, så sätt epost = incident.cgi_rgol_delivery_email
                    var _cgi_customer_email = result.entities[0]["cgi_customer_email"];
                    var _cgi_delivery_email = result.entities[0]["cgi_rgol_delivery_email"];
                    var _cgi_iscompleted = result.entities[0]["cgi_iscompleted"];

                    if (_cgi_delivery_email != null && _cgi_iscompleted == false)
                        Endeavor.formscriptfunctions.SetValue("cgi_email", _cgi_delivery_email, formContext);
                    else
                        Endeavor.formscriptfunctions.SetValue("cgi_email", _cgi_customer_email, formContext);
                }
            }
            catch (e) {
                alert("Fel i getEmail_callback\n\n" + e.message);
            }
        },

        //Set mobilenumber
        setMobileNumber: function (formContext) {
            try {
                var _caseid = Endeavor.formscriptfunctions.GetLookupid("cgi_caseid", formContext);
                var caseidClean = null;

                if (_caseid != null) {
                    caseidClean = Endeavor.formscriptfunctions.cleanIdField(_caseid);
                    Endeavor.OData_Querys.GetMobileNumber(caseidClean, formContext);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.getMobileNumber\n\n" + e.message);
            }
        },

        setMobileNumber_callback: function (result, formContext) {
            try {
                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    //alert("Det finns inget mobilnummer angivet på ärendet");
                }
                else {
                    var _cgi_rgol_telephonenumber = result.entities[0]["cgi_rgol_telephonenumber"];
                    var _cgi_customer_telephonenumber = result.entities[0]["cgi_customer_telephonenumber"];
                    var _cgi_iscompleted = result.entities[0]["cgi_iscompleted"];

                    if (_cgi_rgol_telephonenumber != null && _cgi_iscompleted == false)
                        Endeavor.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_rgol_telephonenumber, formContext);
                    else
                        Endeavor.formscriptfunctions.SetValue("cgi_mobilenumber", _cgi_customer_telephonenumber, formContext);
                }
            }
            catch (e) {
                alert("Fel i getMobileNumber_callback\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Set all values to 0 or null
        setEmptyAllValues: function (formContext) {

            try {

                //S�tt belopp fältet till editerbart
                Endeavor.formscriptfunctions.SetState("cgi_amount", "true", formContext);
                Endeavor.formscriptfunctions.SetState("cgi_calculated_amount", "true", formContext);

                //Ers�ttningsformen
                formContext.getAttribute("cgi_reimbursementformid").setValue(null);

                //Konto uppgifter
                formContext.getAttribute("cgi_accountno").setValue();
                formContext.getAttribute("cgi_iban").setValue();
                formContext.getAttribute("cgi_swift").setValue();

                //Konteringssträngen
                formContext.getAttribute("cgi_accountid").setValue(null);
                formContext.getAttribute("cgi_responsibleid").setValue(null);
                formContext.getAttribute("cgi_productid").setValue(null);
                formContext.getAttribute("cgi_vat_code").setValue(null);

                //Biocheck
                formContext.getAttribute("cgi_checknumber").setValue(null);

                ////Vidarebefordran
                formContext.getAttribute("cgi_transportcompanyid").setValue(null);

                ////Taxi
                formContext.getAttribute("cgi_taxi_company").setValue();

                ////Milers�tttning
                formContext.getAttribute("cgi_car_reg").setValue();
                Endeavor.formscriptfunctions.SetValue("cgi_milage", 0, formContext);
                formContext.getAttribute("cgi_last_valid").setValue();
                Endeavor.formscriptfunctions.SetValue("cgi_milage_compensation", 0, formContext);

                ////Kvantitet och belopp
                formContext.getAttribute("cgi_quantity").setValue();
                formContext.getAttribute("cgi_amount").setValue();
                formContext.getAttribute("cgi_calculated_amount").setValue();

                ////Vidarefakturering
                formContext.getAttribute("cgi_reinvoicing").setValue(false);
                formContext.getAttribute("cgi_invoicerecipient").setValue(null);

                //Kommentarer
                formContext.getAttribute("cgi_comments").setValue();
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setEmptyAllValues\n\n" + e.message);
            }
        },

        // Milage compensation number of mile on_change
        cgi_milage_on_change: function (executionContext) {
            var formContext = executionContext.getFormContext();
            Endeavor.Skanetrafiken.cgi_refund.calculateMilageCompensation(formContext);
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
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.calculateCompensation\n\n" + e.message);
            }

        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        setLimitandAuth: function (executionContext) {
            var formContext = executionContext.getFormContext();
            var globalContext = Xrm.Utility.getGlobalContext();

            //Check security role
            var currentUserRoles = globalContext.userSettings.securityRoles;
            for (var i = 0; i < currentUserRoles.length; i++) {
                var userRoleId = currentUserRoles[i];
                var userRoleIdClean = null;

                if (userRoleId != null)
                {
                    userRoleIdClean = Endeavor.formscriptfunctions.cleanIdField(userRoleId);
                    Endeavor.OData_Querys.GetSecRolesNameRefund(userRoleIdClean, formContext);
                }
            }
        },

        CheckUserRoleOnchange_callback: function (result, formContext) {

            try {

                if (result == null || result.entities == null || result.entities.length < 1 || result.entities[0] == null) {
                    alert("Ingen säkerhetsroll definierad!");
                }
                else {
                    var userRoleName = "Attest"
                    var roleName = result.entities[0]["name"];

                    //Om användaren har rollen Attest ovan, så är användaren godkänd för att 
                    if (roleName == userRoleName)
                        formContext.getAttribute("cgi_auth_approved").setValue(true);
                }
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.CheckUserRoleOnchange_callback\n\n" + e.message);
            }
        },

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        checkAuthorization: function (executionContext) {

            var formContext = executionContext.getFormContext();

            var _attestation_code = formContext.getAttribute("cgi_attestation").getValue();
            var _auth_approved = formContext.getAttribute("cgi_auth_approved").getValue();

            try {
                //Om attest kod �r skilt fr�n Pendning (285050000) och �r skilt fr�n No Financial transaction (285050003) anv�ndaren inte har attestbeh�righet
                //, s� sp�rra samtliga f�lt f�r editering.
                if (_attestation_code != 285050000 && _attestation_code != 285050003 && !_auth_approved)
                    Endeavor.Skanetrafiken.cgi_refund.setFieldsToReadOnly(formContext);

                //Om anv�ndaren inte har attestbeh�righet, s� sp�rra f�ltet attestkod.
                if (!_auth_approved)
                    Endeavor.formscriptfunctions.SetState("cgi_attestation", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.checkAuthorization\n\n" + e.message);
            }
        },

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        setFieldsToReadOnly: function (formContext) {

            try {

                //Generella f�lt
                Endeavor.formscriptfunctions.SetState("cgi_caseid", true, formContext);
                Endeavor.formscriptfunctions.SetState("ownerid", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_reimbursementformid", true, formContext);

                //Biocheck
                Endeavor.formscriptfunctions.SetState("cgi_checknumber", true, formContext);

                ////Konteringsstr�ngen
                Endeavor.formscriptfunctions.SetState("cgi_accountid", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_productid", true, formContext);

                //////Vidarebefordran
                Endeavor.formscriptfunctions.SetState("cgi_transportcompanyid", true, formContext);

                //////Taxi
                Endeavor.formscriptfunctions.SetState("cgi_taxi_company", true, formContext);

                //////Milers�tttning
                Endeavor.formscriptfunctions.SetState("cgi_car_reg", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_milage", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_last_valid", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_milage_compensation", true, formContext);

                //////Kvantitet och belopp
                Endeavor.formscriptfunctions.SetState("cgi_quantity", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_amount", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_calculated_amount", true, formContext);

                //////Vidarefakturering
                Endeavor.formscriptfunctions.SetState("cgi_reinvoicing", true, formContext);
                Endeavor.formscriptfunctions.SetState("cgi_invoicerecipient", true, formContext);
            }
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.setFieldsToReadOnly\n\n" + e.message);
            }
        },
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        format_phonenumber: function (executionContext) {

            try {

                var formContext = executionContext.getFormContext();
                var formContext = executionContext.getFormContext();

                var phoneNumberStr = executionContext.getEventSource();
                var control = formContext.getControl(phoneNumberStr.getName());

                // Verify that the field is valid
                if (typeof (phoneNumberStr) != "undefined" && phoneNumberStr != null && phoneNumberStr.getValue() != null) {

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
            catch (e) {
                alert("Fel i Endeavor.Skanetrafiken.cgi_refund.format_phonenumber\n\n" + e.message);
            }
        }
    };
}