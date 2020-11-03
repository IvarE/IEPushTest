if (typeof (Endeavor) == "undefined") {

    var Endeavor = {
    };
}

if (typeof (Endeavor.Skanetrafiken) == "undefined") {

    Endeavor.Skanetrafiken = {
    };
}

if (typeof (Endeavor.Skanetrafiken.BiffTransactions) == "undefined") {

    Endeavor.Skanetrafiken.BiffTransactions = {

        // REFERENCE TO BIFFTRANSACTIONS DOCUMENT TEST
        htmldocument: null,
        // REFERENCE TO ACTIVE CARD (PAGE DEFAULT OR LATEST SEARCH)
        cardnumber: null,
        missingcard: true,
        travelcardid: null,
        globalContext: null,
        formContext: null,

        onLoad: function (executionContext) {
            debugger;

            try {
                Endeavor.Skanetrafiken.BiffTransactions.formContext = executionContext.getFormContext();
                Endeavor.Skanetrafiken.BiffTransactions.globalContext = Xrm.Utility.getGlobalContext();
            } catch (e) {
                Endeavor.formscriptfunctions.AlertCustomDialog(e.message);
            }
        },

        setDocument: function (document) {
            Endeavor.Skanetrafiken.BiffTransactions.htmldocument = document;

            if (Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('cardnr').value)
                Endeavor.Skanetrafiken.BiffTransactions.getDetailsAndTransactions();
            
        },

        // CALLED WHEN SEARCH BUTTON IS PRESSED
        getDetailsAndTransactions: function () {

            Endeavor.Skanetrafiken.BiffTransactions.cardnumber = Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('cardnr').value;

            if (Endeavor.Skanetrafiken.BiffTransactions.cardnumber) {

                try {
                    Endeavor.Skanetrafiken.BiffTransactions.getDetails();
                }
                catch (e) {
                    Endeavor.formscriptfunctions.AlertCustomDialog("Error in TravelCard details: " + e.message);
                }

                try {
                    Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable(Endeavor.Skanetrafiken.BiffTransactions.formContext);
                }
                catch (e) {
                    Endeavor.formscriptfunctions.AlertCustomDialog("Error in saved transactions: " + e.message);
                }
            }
            else
                Endeavor.formscriptfunctions.AlertCustomDialog("Inget angivet kortnummer");
        },

        // REQUESTS AND SETS ALL CARD DETAILS FROM BIZTALK AND CRM
        getDetails: function () {

            var cardnumber = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;
            var inputParameters = [{ "Field": "TravelCardNumber", "Value": cardnumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

            Endeavor.formscriptfunctions.callGlobalAction("ed_GetCardDetails", inputParameters,
                function (result) {

                    var url = Endeavor.Skanetrafiken.BiffTransactions.globalContext.getClientUrl() + "/api/data/v9.0/cgi_travelcards?" +
                        "$select=cgi_travelcardnumber,cgi_travelcardid,cgi_travelcardname,cgi_failedattemptstochargemoney,cgi_creditcardmask,_cgi_contactid_value,cgi_autoloaddisconnectiondate," +
                        "cgi_autoloadconnectiondate,cgi_autoloadstatus,ed_blockedcard,_cgi_accountid_value&$filter=cgi_travelcardnumber eq '" + cardnumber + "'";
                    var detailsresults = Endeavor.formscriptfunctions.fetchJSONResults(url);

                    // CALL FUNCTION
                    Endeavor.Skanetrafiken.BiffTransactions.populateValues(result, detailsresults);

                    // GET OUTSTANDING CHARGES (ENABLES / DISABLES RELOAD BUTTON)
                    if (cardnumber && !Endeavor.Skanetrafiken.BiffTransactions.missingcard) {

                        Endeavor.formscriptfunctions.callGlobalAction("ed_GetOutstandingCharges", inputParameters,
                            function (result) {
                                Endeavor.Skanetrafiken.BiffTransactions.hasOutstandingCharges(result);
                            },
                            function (error) {
                                var errorMessage = "Outstanding charges service unavailable. Please contact your systems administrator. Details: " + error.message;
                                console.log(errorMessage);
                                Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                            });                            
                    }
                },
                function (error) {
                    var errorMessage = "Get Card Details service is unavailable. Please contact your systems administrator. Details: " + error.message;
                    console.log(errorMessage);
                    Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                });
        },

        // POPULATE DETAILS VALUES AND ZONE TABLE
        // detailsresponse = retrieved from biztalk
        // detailsresults = retrieved from oData
        populateValues: function (detailsresponse, detailsresults) {

            var htmldocument = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

            /* Local function for not getting null reference from xml objects */
            function getElementValue(xmlelement, tagname) {

                if (xmlelement && xmlelement.getElementsByTagName(tagname)[0] != null) {
                    if (xmlelement.getElementsByTagName(tagname)[0].firstChild != null) {
                        return xmlelement.getElementsByTagName(tagname)[0].firstChild.nodeValue;
                    }
                    else {
                        return "";
                    }
                }
                else {
                    return "";
                }
            }

            /* Local function for converting true/false to ja/nej */
            function getJaNej(str) {

                if (str && str.toLowerCase() == 'true') {
                    return "Ja";
                }

                if (str && str.toLowerCase() == 'false') {
                    return "Nej";
                }

                return "";
            }

            if (detailsresults.length < 1)
                Endeavor.formscriptfunctions.AlertCustomDialog("Kortet hittades inte");
            else {
                if (detailsresponse.getElementsByTagName("ns0:CardInformation") && detailsresponse.getElementsByTagName("ns0:CardInformation").length > 1)
                    throw new Error("Unexpected response length");
                else {

                    if (detailsresponse.getElementsByTagName("ns0:CardInformation") && detailsresponse.getElementsByTagName("ns0:CardInformation").length < 1) {
                        Endeavor.Skanetrafiken.BiffTransactions.missingcard = true;
                        Endeavor.formscriptfunctions.AlertCustomDialog("Kortnumret saknas i BizTalk.");
                    }
                    else
                        Endeavor.Skanetrafiken.BiffTransactions.missingcard = false;

                    // SET GLOBAL TRAVELCARDID
                    Endeavor.Skanetrafiken.BiffTransactions.travelcardid = detailsresults[0].cgi_travelcardid;

                    // POPULATE INFORMATION DISPLAY
                    var cardinformation = detailsresponse.getElementsByTagName("ns0:CardInformation")[0];
                    var pursedetails = detailsresponse.getElementsByTagName("ns0:PurseDetails")[0];
                    var perioddetails = detailsresponse.getElementsByTagName("ns0:PeriodDetails")[0];

                    var zonelists = [];
                    for (var i = 0; i < detailsresponse.getElementsByTagName("ns0:ZoneLists").length; i++) {
                        zonelists.push(detailsresponse.getElementsByTagName("ns0:ZoneLists")[i]);
                    }

                    var routelists = [];
                    for (var i = 0; i < detailsresponse.getElementsByTagName("ns0:RouteLists").length; i++) {
                        routelists.push(detailsresponse.getElementsByTagName("ns0:RouteLists")[i]);
                    }

                    // CHECK FOR ACCOUNT OR CONTACT
                    if (detailsresults[0]._cgi_contactid_value) {

                        var url = Endeavor.Skanetrafiken.BiffTransactions.globalContext.getClientUrl() + "/api/data/v9.0/contacts?$select=firstname,lastname" +
                            "&$filter=contactid eq " + detailsresults[0]._cgi_contactid_value;
                        var contactresults = Endeavor.formscriptfunctions.fetchJSONResults(url);

                        var link = htmldocument.createElement("a");
                        var name = "";
                        if (contactresults[0].firstname)
                            name += contactresults[0].firstname;
                        if (contactresults[0].lastname)
                            name += " " + contactresults[0].lastname;

                        link.innerHTML = name;
                        link.style.cursor = "pointer";
                        link.onclick = function f() {
                            Endeavor.formscriptfunctions.OpenCustomForm("contact", detailsresults[0]._cgi_contactid_value);
                        };
                        htmldocument.getElementById('name').innerHTML = "";
                        htmldocument.getElementById('name').appendChild(link);
                    }
                    else {

                        var url = Endeavor.Skanetrafiken.BiffTransactions.globalContext.getClientUrl() + "/api/data/v9.0/accounts?$select=name" +
                            "&$filter=accountid eq " + detailsresults[0]._cgi_accountid_value;
                        var accountresults = Endeavor.formscriptfunctions.fetchJSONResults(url);

                        var link = htmldocument.createElement("a");
                        link.innerHTML = accountresults[0].name;
                        link.style.cursor = "pointer";
                        link.onclick = function f() {
                            Endeavor.formscriptfunctions.OpenCustomForm("account", detailsresults[0]._cgi_accountid_value);
                        };
                        htmldocument.getElementById('name').innerHTML = "";
                        htmldocument.getElementById('name').appendChild(link);
                    }

                    htmldocument.getElementById('cardname').innerHTML = detailsresults[0].cgi_travelcardname;

                    var autoloadstatus = detailsresults[0].cgi_autoloadstatus;
                    if (autoloadstatus == 1)
                        htmldocument.getElementById('autoloadstatus').innerHTML = "Ja";
                    else
                        htmldocument.getElementById('autoloadstatus').innerHTML = "Nej";

                    if (detailsresults[0].cgi_autoloadconnectiondate && detailsresults[0].cgi_autoloadconnectiondate.length == 21 && detailsresults[0].cgi_autoloadconnectiondate.charAt(0) == "/")
                        htmldocument.getElementById('autoloadstartdate').innerHTML = (new Date(parseInt(detailsresults[0].cgi_AutoloadConnectionDate.substring(6, 19)))).toISOString().substring(0, 10);
                    else
                        if (detailsresults[0].cgi_autoloadconnectiondate)
                            htmldocument.getElementById('autoloadstartdate').innerHTML = detailsresults[0].cgi_autoloadconnectiondate;

                    if (detailsresults[0].cgi_autoloaddisconnectiondate)
                        htmldocument.getElementById('autoloadenddate').innerHTML = detailsresults[0].cgi_autoloaddisconnectiondate;

                    htmldocument.getElementById('creditcardnr').innerHTML = "";
                    htmldocument.getElementById('failedattempts').innerHTML = detailsresults[0].cgi_failedattemptstochargemoney;

                    htmldocument.getElementById('status').innerHTML = getJaNej(getElementValue(cardinformation, 'ns0:CardHotlisted'));
                    htmldocument.getElementById('reason').innerHTML = getElementValue(cardinformation, 'ns0:HotlistReason');
                    htmldocument.getElementById('hotlisted').innerHTML = getJaNej(getElementValue(perioddetails, 'ns0:Hotlisted'));
                    htmldocument.getElementById('blocked').innerHTML = getJaNej(getElementValue(pursedetails, 'ns0:Hotlisted'));

                    htmldocument.getElementById('loadstatus').innerHTML = getJaNej(getElementValue(pursedetails, 'ns0:OutstandingDirectedAutoload'));
                    htmldocument.getElementById('balance').innerHTML = getElementValue(pursedetails, 'ns0:Balance');

                    htmldocument.getElementById('cardtype').innerHTML = getElementValue(perioddetails, 'ns0:ProductType');
                    htmldocument.getElementById('startdate').innerHTML = getElementValue(perioddetails, 'ns0:PeriodStart').substring(0, 10);
                    htmldocument.getElementById('enddate').innerHTML = getElementValue(perioddetails, 'ns0:PeriodEnd').substring(0, 10);
                    htmldocument.getElementById('pricepaid').innerHTML = getElementValue(perioddetails, 'ns0:PricePaid') + " " + getElementValue(perioddetails, 'ns0:Currency');
                    htmldocument.getElementById('waitingperiods').innerHTML = getElementValue(perioddetails, 'ns0:WaitingPeriods');
                    htmldocument.getElementById('activatedautoload').innerHTML = getJaNej(getElementValue(perioddetails, 'ns0:OutstandingEnableThresholdAutoload'));

                    // POPULATE ZONE TABLE
                    htmldocument.getElementById('zonetable').innerHTML = "";

                    for (var j = 0; j < zonelists.length; j++) {
                        var zonenumber = getElementValue(zonelists[j], 'ns0:Zone');

                        var row = htmldocument.getElementById('zonetable').insertRow();
                        var cell = row.insertCell();
                        cell.innerHTML = zonenumber;
                        cell = row.insertCell();

                        try {

                            var url = Endeavor.Skanetrafiken.BiffTransactions.globalContext.getClientUrl() + "/api/data/v9.0/cgi_zonenames?$select=cgi_name" +
                                "&$filter=cgi_zoneid eq " + zonenumber;
                            var zoneresults = Endeavor.formscriptfunctions.fetchJSONResults(url);
                            cell.innerHTML = zoneresults[0].cgi_name;
                        }
                        catch (e) {
                            Endeavor.formscriptfunctions.AlertCustomDialog("Error when fetching Zone :" + e.message);
                        }
                    }
                }
            }
        },

        transactionSearch: function (formContext) {

            if (Endeavor.Skanetrafiken.BiffTransactions.cardnumber) {

                function parseDateString(datestring) {
                    var formattedstring = "";

                    for (var i = 0; i < datestring.length; i++) {
                        if (isNaN(parseInt(datestring[i])) == false) {
                            formattedstring = formattedstring + datestring[i];
                        }
                    }

                    var yyyy = parseInt(formattedstring.substring(0, 4));
                    var mm = parseInt(formattedstring.substring(4, 6)) - 1;
                    var dd = parseInt(formattedstring.substring(6, 8));

                    var date = new Date(yyyy, mm, dd, 0, 0, 0, 0);
                    if (isNaN(date.getTime())) {
                        date = new Date();
                    }
                    return date.toISOString();
                }

                var cardnr = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;
                var datefrom = parseDateString(Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('datefrom').value);
                var dateto = parseDateString(Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('dateto').value);
                var maxtransactions = '100';

                if (datefrom && dateto) {

                    var inputParameters = [{ "Field": "TravelCardNumber", "Value": cardnr, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "MaxTransactions", "Value": maxtransactions, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "DateFrom", "Value": datefrom, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 },
                        { "Field": "DateTo", "Value": dateto, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                    Endeavor.formscriptfunctions.callGlobalAction("ed_GetCardTransactions", inputParameters,
                        function (result) {
                            Endeavor.Skanetrafiken.BiffTransactions.populateTransactionTable(result, formContext);
                        },
                        function (error) {
                            var errorMessage = "TravelCard Transactions service is unavailable. Please contact your systems administrator. Details: " + error.message;
                            console.log(errorMessage);
                            Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                        });
                }
            }
            else 
                Endeavor.formscriptfunctions.AlertCustomDialog("Inget angivet kortnummer");
        },

        populateTransactionTable: function (transactionsresponse, formContext) {

            var document = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

            /* Local function for not getting null reference from xml objects */
            function getElementValue(xmlelement, tagname) {

                if (xmlelement && xmlelement.getElementsByTagName(tagname)[0] != null) {
                    if (xmlelement.getElementsByTagName(tagname)[0].firstChild != null) {
                        return xmlelement.getElementsByTagName(tagname)[0].firstChild.nodeValue;
                    }
                    else {
                        return "";
                    }
                }
                else {
                    return "";
                }
            }

            var transactionstable = document.getElementById('transactionsbody');

            if (!transactionsresponse.getElementsByTagName("ns0:Transactions") || transactionsresponse.getElementsByTagName("ns0:Transactions").length == 0)
                transactionstable.innerHTML = "<tr  class=\"blankrow\"></tr>";
            else {

                transactionstable.innerHTML = "";
                for (var i = 0; i < transactionsresponse.getElementsByTagName("ns0:Transactions").length; i++) {

                    var transaction = transactionsresponse.getElementsByTagName("ns0:Transactions")[i];

                    // cgi_travelcardid, cgi_cardsect, cgi_date, cgi_time, cgi_amount, cgi_deviceid, cgi_origzone, cgi_origzonename, cgi_rectype, cgi_route, cgi_txntype, cgi_caseid, cgi_travelcard, cgi_travelcardtransaction
                    // cgi_travelcardid, cgi_origzonename, cgi_caseid, cgi_travelcard, cgi_travelcardtransaction

                    var travelcardid = Endeavor.Skanetrafiken.BiffTransactions.travelcardid;
                    var travelcard = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;
                    var date = getElementValue(transaction, 'ns0:Date').substring(0, 10);
                    var time = getElementValue(transaction, 'ns0:Date').substring(11, 16);
                    var cardsect = Endeavor.Skanetrafiken.BiffTransactions.convertCardSect(getElementValue(transaction, 'ns0:CardSect'));
                    var rectype = Endeavor.Skanetrafiken.BiffTransactions.convertRecType(getElementValue(transaction, 'ns0:RecType'));
                    var txntype = Endeavor.Skanetrafiken.BiffTransactions.convertTransactionType(getElementValue(transaction, 'ns0:TxnType'));
                    var amount = getElementValue(transaction, 'ns0:Amount');
                    var origzone = getElementValue(transaction, 'ns0:OrigZone');
                    var origzonename = ""; //???
                    var route = getElementValue(transaction, 'ns0:Route');
                    var deviceid = getElementValue(transaction, 'ns0:DeviceID');

                    var row = transactionstable.insertRow();
                    var buttoncell = row.insertCell();

                    var formtype = formContext.data.entity.getEntityName();

                    if (formtype.toUpperCase() == "INCIDENT") {
                        var caseid = formContext.data.entity.getId();

                        var save = document.createElement("a");
                        save.style = "font-weight: bold";
                        save.style.cursor = "pointer";
                        save.onclick = Endeavor.Skanetrafiken.BiffTransactions.saveTransaction(travelcardid, cardsect, date, time, amount, deviceid, origzone, origzonename, rectype, route, txntype, caseid, travelcard);
                        save.innerHTML = '+';
                        buttoncell.appendChild(save);
                        buttoncell.title = "Lägg till";
                        buttoncell.style = "text-align: center";
                    }
                    else
                        buttoncell.innerHTML = " ";

                    row.insertCell().innerHTML = date;
                    row.insertCell().innerHTML = time;
                    row.insertCell().innerHTML = cardsect;
                    row.insertCell().innerHTML = rectype;
                    var txncell = row.insertCell();
                    txncell.colSpan = "2";
                    txncell.innerHTML = txntype;
                    row.insertCell().innerHTML = amount;
                    row.insertCell().innerHTML = origzone;
                    var routecell = row.insertCell();
                    routecell.colSpan = "3";
                    routecell.innerHTML = route;
                    row.insertCell().innerHTML = deviceid;
                }
            }
        },

        populateSavedTransactionsTable: function (formContext) {

            var document = Endeavor.Skanetrafiken.BiffTransactions.htmldocument;

            var entityName = formContext.data.entity.getEntityName();
            var entityId = formContext.data.entity.getId();

            if (entityName.toUpperCase() == "INCIDENT") {

                entityId = entityId.substring(1, entityId.length - 1);

                var url = Endeavor.Skanetrafiken.BiffTransactions.globalContext.getClientUrl() + "/api/data/v9.0/cgi_travelcardtransactions?$select=cgi_travelcardtransactionid," +
                    "cgi_travelcard,cgi_date,cgi_time,cgi_cardsect,cgi_rectype,cgi_txntype,cgi_amount,cgi_origzone,cgi_route,cgi_deviceid&$filter=cgi_caseid eq " + entityId;
                var savedtransactionsresults = Endeavor.formscriptfunctions.fetchJSONResults(url);

                var savedtransactionstable = document.getElementById('savedtransactionsbody');

                if (!savedtransactionsresults)
                    savedtransactionstable.innerHTML = "<tr  class=\"blankrow\"></tr>";
                else {
                    savedtransactionstable.innerHTML = "";
                    for (var i = 0; i < savedtransactionsresults.length; i++) {

                        var row = savedtransactionstable.insertRow();

                        var cell = row.insertCell(); // EMPTY CELL FOR BUTTON-SPACE
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_travelcard;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_date;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_time;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_cardsect;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_rectype;
                        cell = row.insertCell();
                        cell.colSpan = "2";
                        cell.innerHTML = savedtransactionsresults[i].cgi_txntype;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_amount;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_origzone;
                        cell = row.insertCell();
                        cell.colSpan = "2";
                        cell.innerHTML = savedtransactionsresults[i].cgi_route;
                        cell = row.insertCell();
                        cell.innerHTML = savedtransactionsresults[i].cgi_deviceid;
                        cell = row.insertCell();

                        var del = document.createElement("a");
                        del.style = "font-weight: bold";
                        del.style.cursor = "pointer";
                        del.onclick = Endeavor.Skanetrafiken.BiffTransactions.deleteTransaction(savedtransactionsresults[i].cgi_travelcardtransactionid);
                        del.innerHTML = "x";
                        cell.appendChild(del);
                        cell.title = "Ta bort";
                        cell.style = "text-align: center";

                    }
                }
            }
        },

        /* Function for converting Transaction number to names */
        convertTransactionType: function (type) {
            var _returnValue = "";

            if (!type)
                return _returnValue;

            if (type == "0")
                _returnValue = "Ordinarie";
            else if (type == "1")
                _returnValue = "Utställning";
            else if (type == "2")
                _returnValue = "Refund";
            else if (type == "3")
                _returnValue = "Hämtad laddning Reskassa";
            else if (type == "4")
                _returnValue = "Hämtad Autoladda Period";
            else if (type == "+16")
                _returnValue = "Cancellation";
            else if (type == "+32")
                _returnValue = "Card Write Error";
            else if (type == "+64")
                _returnValue = "Walk Away";
            else if (type == "+128")
                _returnValue = "Info";
            else
                _returnValue = "Ordinarie";

            return _returnValue;
        },

        /* Function for converting Rectype number to names */
        convertRecType: function (recordtype) {

            var returnValue = "";

            if (!recordtype)
                return returnValue;
            if (recordtype == "70")
                returnValue = "Reskassa";
            else if (recordtype == "71")
                returnValue = "Value Payment";
            else if (recordtype == "72")
                returnValue = "Periodkort";
            else if (recordtype == "74")
                returnValue = "Övergång";
            else if (recordtype == "75")
                returnValue = "Övergång";
            else if (recordtype == "78")
                returnValue = "Inspektion";
            else if (recordtype == "79")
                returnValue = "Activating of Waiting Period";
            else if (recordtype == "80")
                returnValue = "Laddning Reskassa";
            else if (recordtype == "81")
                returnValue = "Laddning Periodkort";
            else if (recordtype == "91")
                returnValue = "Use of Card on hotlist";
            else
                returnValue = "Reskassa";

            return returnValue;
        },

        /* Function for converting Cardsect number to names */
        convertCardSect: function (cardsect) {
            var returnValue = "";

            if (!cardsect)
                return returnValue;
            if (cardsect == "0")
                _returnValue = "Hela kortet";
            else if (cardsect == "1")
                _returnValue = "Periodkort";
            else if (cardsect == "2")
                _returnValue = "Reskassa";
            else
                _returnValue = "Hela kortet";

            return _returnValue;
        },

        saveTransaction: function (cgi_travelcardid, cgi_cardsect, cgi_date, cgi_time, cgi_amount, cgi_deviceid, cgi_origzone, cgi_origzonename, cgi_rectype, cgi_route, cgi_txntype, cgi_caseid, cgi_travelcard) {
            return function () {
                var data = {

                    "cgi_caseid@odata.bind": "/incidents(" + cgi_caseid + ")",
                    "cgi_travelcardid@odata.bind": "/cgi_travelcards(" + cgi_travelcardid + ")",
                    "cgi_cardsect": cgi_cardsect,
                    "cgi_amount": cgi_amount,
                    "cgi_currency": "",
                    "cgi_date": cgi_date,
                    "cgi_time": cgi_time,
                    "cgi_deviceid": cgi_deviceid,
                    "cgi_origzone": cgi_origzone,
                    "cgi_OrigZoneName": cgi_origzonename,
                    "cgi_rectype": cgi_rectype,
                    "cgi_route": cgi_route,
                    "cgi_txntype": cgi_txntype,
                    "cgi_txnnum": "",
                    "cgi_travelcardtransaction": cgi_date + cgi_time + cgi_route,
                    "cgi_travelcard": cgi_travelcard
                };

                Xrm.WebApi.createRecord("cgi_travelcardtransaction", data).then(
                    function success(result) {
                        Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable(Endeavor.Skanetrafiken.BiffTransactions.formContext);
                    },
                    function (error) {
                        Endeavor.formscriptfunctions.AlertCustomDialog("Error function called, error:" + error.message);
                    });
            };
        },

        deleteTransaction: function (travelcardtransactionid) {
            return function () {

                Xrm.WebApi.deleteRecord("cgi_travelcardtransaction", travelcardtransactionid).then(
                    function success(result) {
                        Endeavor.Skanetrafiken.BiffTransactions.populateSavedTransactionsTable(Endeavor.Skanetrafiken.BiffTransactions.formContext);
                    },
                    function (error) {
                        Endeavor.formscriptfunctions.AlertCustomDialog("Error function called, error:" + error.message);
                    }
                );
            };
        },

        hasOutstandingCharges: function (outstandingchargesresponse) {

            var hasoutstandingcharges = 'false';

            if (outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0] != null) {
                if (outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0].firstChild != null)
                    hasoutstandingcharges = outstandingchargesresponse.getElementsByTagName('HasExpiredCharge')[0].firstChild.nodeValue;
                else
                    hasoutstandingcharges = "false";
            }
            else
                hasoutstandingcharges = "false";

            if (hasoutstandingcharges == "true") {
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('reloadbutton').disabled = false;
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('outstandingcharges').innerHTML = "Ja";
            }
            else 
                Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('outstandingcharges').innerHTML = "Nej";
        },

        reload: function () {

            var cardnumber = Endeavor.Skanetrafiken.BiffTransactions.cardnumber;

            if (cardnumber) {

                var inputParameters = [{ "Field": "TravelCardNumber", "Value": cardnumber, "TypeName": Endeavor.formscriptfunctions.getParameterType("string"), "StructuralProperty": 1 }];

                Endeavor.formscriptfunctions.callGlobalAction("ed_RechargeCard", inputParameters,
                    function (result) {
                        Endeavor.Skanetrafiken.BiffTransactions.htmldocument.getElementById('reloadbutton').disabled = false;
                        Endeavor.formscriptfunctions.AlertCustomDialog(rechargeresponse.getElementsByTagName('Message')[0].firstChild.nodeValue);
                    },
                    function (error) {
                        var errorMessage = "Recharge Card service is unavailable.Please contact your systems administrator. Details: " + error.message;
                        console.log(errorMessage);
                        Endeavor.formscriptfunctions.AlertCustomDialog(errorMessage);
                    });
            }
            else
                Endeavor.formscriptfunctions.AlertCustomDialog("Inget kortnummer hittat");
        }
    };
}