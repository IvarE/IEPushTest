
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

// This file is deprecated and should not be updated.

// Dependencies:
/// <reference path="SDK.Rest.js" />
/// <reference path="jquery-1.11.1.min.js" />

// Begin scoping 
var edp_Common = {
    /// <deprecated>Use Endeavor.Common instead.</deprecated>

    stopAutoSave: function (context) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Stop the auto save feature.
        /// Put on the OnSave event
        /// Use command: edp_Common.stopAutoSave and enable "Pass execution context as the first parameter"
        /// </summary>
        /// <param name="context"></param>
        var saveEvent = context.getEventArgs();
        if (saveEvent.getSaveMode() == 70 ||    // Form AutoSave Event
            saveEvent.getSaveMode() == 2) {     // Form AutoSave when form is closed.
            saveEvent.preventDefault(); //Stops the Save Event
        }
    },

    fetchJSONResults: function (url, max_records) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get records from server
        /// </summary>
        /// <param name="url">The Query to execute</param>
        /// <param name="max_records">Nr of records to process. If null max 5000 will be retrieved.</param>
        /// <returns type="">The Records.</returns>

        var value = null;
        var request = new XMLHttpRequest();
        var nextUrl = null;
        var totalRecords = new Array();
        request.open("GET", url, false);
        request.setRequestHeader("Accept", "application/json");
        request.setRequestHeader("contentType", "application/json; charset=utf-8");
        request.send();
        try {
            if (max_records == null)
                max_records = 5000;

            var object = eval("[" + request.responseText + "]");
            value = object[0].d.results;
            nextUrl = object[0].d.__next;

            // Add records to total records
            for (var i = 0; i < value.length; i++) {

                if (totalRecords.length >= max_records)
                    return totalRecords;
                totalRecords.push(value[i]);
            }

            // Is there more records to fetch?
            if (nextUrl != null) {
                var nextRecords = edp_Common.fetchJSONResults(nextUrl, max_records);
                if (nextRecords != null && nextRecords.length > 0) {
                    for (var i = 0; i < nextRecords.length; i++) {
                        if (totalRecords.length >= max_records)
                            return totalRecords;
                        totalRecords.push(nextRecords[i]);
                    }
                }
            }

            return totalRecords;
        }
        catch (error) {
            value = null;
            alert("There was an error trying to access the CRM server. Please contact your administrator. URL:" + url);
            return null;
        }
    },

    getOrganizationServiceEndpoint: function () {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Gets the Organization Service Endpoint for OData.
        /// </summary>
        baseOrganizationServiceEndpoint = SDK.REST._ODataPath();
        return baseOrganizationServiceEndpoint;
    },

    setLookup: function (fieldName, entRef) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Set value in a lookup field
        /// </summary>
        /// <param name="fieldName">The field to Set</param>
        /// <param name="entRef">Entity reference object</param>
        if (entRef != undefined || null) {
            var lookup = new Object();
            var lookupValue = new Array();
            if ("Id" in entRef) {
                lookup.id = entRef.Id;
            }
            else if ("getId" in entRef) {
                lookup.id = entRef.getId();
            }
            if ("LogicalName" in entRef) {
                lookup.entityType = entRef.LogicalName;
            }
            else if ("getType" in entRef) {
                lookup.entityType = entRef.getType();
            }
            if ("Name" in entRef) {
                lookup.name = entRef.Name;
            }
            else if ("getName" in entRef) {
                lookup.name = entRef.getName();
            }
            lookupValue[0] = lookup;
            Xrm.Page.getAttribute(fieldName).setValue(lookupValue);
        }
        else {
            Xrm.Page.getAttribute(fieldName).setValue();
        }
        // Problem with fields always beeing sent (Problem in Timereporting entity)
        //Xrm.Page.getAttribute(field).setSubmitMode("always");
    },

    getLookupId: function (fieldName) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get Lookup field Id.
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        var lookupid = null;
        var lookupObject = Xrm.Page.getAttribute(fieldName);

        if (lookupObject != null) {
            var lookUpObjectValue = lookupObject.getValue();
            if ((lookUpObjectValue != null)) {
                lookupid = lookUpObjectValue[0].id;
            }
        }

        return lookupid;
    },

    getLookupName: function (fieldName) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get Lookup field Name.
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        var name = null;
        var lookupObject = Xrm.Page.getAttribute(fieldName);

        if (lookupObject != null) {
            var lookUpObjectValue = lookupObject.getValue();
            if ((lookUpObjectValue != null)) {
                name = lookUpObjectValue[0].name;
            }
        }

        return name;
    },

    dateToString: function (oDate, sFormat, sSeparator) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Convert date to string
        /// </summary>
        /// <param name="oDate">A date time variable</param>
        /// <param name="sFormat">Format. I.e: yyyy/MM/dd (Must be "/")</param>
        /// <param name="sSeparator">Use "-"</param>
        /// <returns type=""></returns>

        var sDate = "";
        var dateFormatParts = sFormat.split("/");
        for (i = 0; i < dateFormatParts.length; i++) {
            var dateFormatPart = dateFormatParts[i];
            switch (dateFormatPart) {
                case "yyyy":
                    sDate += oDate.getFullYear() + sSeparator;
                    break;
                case "yy":
                    sDate += oDate.getFullYear().toString().substring(2, 4) + sSeparator;
                    break;
                case "M":
                    sDate += oDate.getMonth() + 1 + sSeparator;
                    break;
                case "MM":
                    sDate += edp_Common.zeroFill(oDate.getMonth() + 1, 2) + sSeparator;
                    break;
                case "d":
                    sDate += oDate.getDate() + sSeparator;
                    break;
                case "dd":
                    sDate += edp_Common.zeroFill(oDate.getDate(), 2) + sSeparator;
                    break;
                default:
                    sDate += dateFormatPart + sSeparator;
            }
        }
        sDate = sDate.substring(0, sDate.length - 1);
        return sDate;
    },

    stringToDate: function (sDate, sFormat, sSeparator) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Convert string to date according to format
        /// </summary>

        // Verify input data
        if (sDate == null)
            return null;

        var oDate = new Date(1900, 1, 1);
        var dateFormatParts = sFormat.split("/");
        var dateParts = sDate.split(sSeparator);
        var datePartYear;
        var datePartMonth;
        var datePartDay;

        for (i = 0; i < dateFormatParts.length; i++) {
            var dateFormatPart = dateFormatParts[i];
            var datePart = dateParts[i];
            switch (dateFormatPart) {
                case "yy":
                    datePart = "20" + String(datePart);
                case "yyyy":
                    //oDate.setYear(datePart);
                    datePartYear = datePart;
                    break;
                case "M":
                case "MM":
                    //oDate.setMonth(parseInt(datePart) - 1);
                    datePartMonth = parseInt(datePart, 10) - 1;
                    break;
                case "d":
                case "dd":
                    //oDate.setDate(parseInt(datePart));
                    datePartDay = parseInt(datePart);
                    break;
            }
        }

        // Compose new date
        oDate.setYear(datePartYear);
        oDate.setMonth(datePartMonth);
        oDate.setDate(datePartDay);

        sDate = sDate.substring(0, sDate.length - 1);
        return oDate;
    },


    isValidDate: function (sDate, sDateFormat, sDateSeparator) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Check if date is valid
        /// </summary>
        /// <returns type="">true / false</returns>

        var aDate = sDate.split(sDateSeparator);
        var aDateFormat = sDateFormat.split("/");
        return edp_Common.isDate(aDate, aDateFormat);
    },

    isDate: function (aDate, aFormat) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        var m, d, y, yearLength
        for (var i = 0, len = aFormat.length; i < len; i++) {
            if (/M/.test(aFormat[i])) m = aDate[i]
            if (/d/.test(aFormat[i])) d = aDate[i]
            if (/y/.test(aFormat[i])) {
                y = aDate[i];
                yearLength = aFormat[i].length;
            }
        }
        return (
            m > 0 && m < 13 &&
            y && y.length === yearLength &&
            d > 0 && d <= (new Date(y, m, 0)).getDate()
            )
    },

    getWeekFromDate: function (date) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get the ISO week number.
        /// </summary>
        /// <param name="date">Date to get week for.</param>
        /// <returns type="">Week number.</returns>

        // ISO week date weeks start on monday  
        // so correct the day number  
        var dayNr = (date.getDay() + 6) % 7;

        // ISO 8601 states that week 1 is the week  
        // with the first thursday of that year.  
        // Set the target date to the thursday in the target week  
        date.setDate(date.getDate() - dayNr + 3);

        // Store the millisecond value of the target date  
        var firstThursday = date.valueOf();

        // Set the target to the first thursday of the year  
        // First set the target to january first  
        date.setMonth(0, 1);
        // Not a thursday? Correct the date to the next thursday  
        if (date.getDay() != 4) {
            date.setMonth(0, 1 + ((4 - date.getDay()) + 7) % 7);
        }

        // The weeknumber is the number of weeks between the   
        // first thursday of the year and the thursday in the target week  
        return 1 + Math.ceil((firstThursday - date) / 604800000); // 604800000 = 7 * 24 * 3600 * 1000  
    },

    getWeekYearFromDate: function (date) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get the ISO weekyear for a date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns type="">Year</returns>

        // Create a new date object for the thursday of this week  
        var target = new Date(date.valueOf());
        target.setDate(target.getDate() - ((date.getDay() + 6) % 7) + 3);

        return target.getFullYear();
    },

    isGUID: function (sGUID) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Verify that string is a valid GUID
        /// </summary>
        /// <param name="sGUID">String to test.</param>

        if (sGUID.length == 38) // 36 + {}
            return true;
        else
            return false;
    },

    // 
    zeroFill: function (iNumber, iLength) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Return iNumber with iLength initial zero(s)
        /// </summary>
        var sNumber = String(iNumber);

        for (var i = sNumber.length; i < iLength; i++) {
            sNumber = "0" + sNumber;
        }

        return sNumber;
    },

    createCookie: function (name, value, days) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Create cookie (session of permanent)
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <param name="value">String value of cookie</param>
        /// <param name="days">null = session cookie, 1 = Valid for 24h</param>

        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";
        document.cookie = name + "=" + value + expires + "; path=/";
    },

    readCookie: function (name) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Reads Cookie value
        /// </summary>
        /// <param name="name">Name of cookie</param>
        /// <returns type="">Cookie Value or null</returns>
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1, c.length);
            }
            if (c.indexOf(nameEQ) == 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
        return null;
    },

    getSubGridSelectedRow: function (subGridName) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get selected row ID from subGrid
        /// </summary>
        /// <param name="subGridName"></param>
        /// <returns type="">Returns a Row object .Id and .Name</returns>

        var grid = document.getElementById(subGridName).control;
        // Other way to get the control
        // grid = Xrm.Page.getControl("AgreementsToAddToJournal");
        // grid.$Y_0
        for (var rowNo = 0; rowNo < grid.get_selectedRecords().length; rowNo++) {
            //alert(grid.get_selectedRecords()[rowNo].Id);
            //alert(grid.get_selectedRecords()[rowNo].Name);
            return grid.get_selectedRecords()[rowNo];
        }
        return null;
    },

    // Convert a field to a button
    ConvertToButton: function (fieldname, buttontext, buttonwidth, clickevent, title) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>

        // CRM2013 Syntax
        if (document.getElementById(fieldname) != null) {
            var fieldId = "field" + fieldname;
            var width = "100px";                    // Standard width
            if (isNaN(buttonwidth) == false)
                width = buttonwidth + "px";
            if (document.getElementById(fieldId) == null) {
                var elementId = document.getElementById(fieldname + "_d");
                var div = document.createElement("div");
                div.style.width = width;
                div.style.textAlign = "right";
                div.style.display = "inline";
                elementId.appendChild(div, elementId);
                //div.innerHTML = '<button id="' + fieldId + '"  type="button" style="margin-left: 3px; width: 100%;" >' + buttontext + '</button>';
                div.innerHTML = '<button id="' + fieldId + '"  type="button" style="margin-left: 3px;" >' + buttontext + '</button>';
                document.getElementById(fieldname).style.width = "0%";
                document.getElementById(fieldId).onclick = function () { clickevent(); };
            }
        }
    },

    trimString: function (str) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Trim whitespaces from start and end.
        /// </summary>
        /// <param name="str">String to trim.</param>
        /// <returns type="string">Trimmed string.</returns>

        return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
    },

    getSystemParameterValue: function (parameterName) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        /// <summary>
        /// Get edp_SystemParameter value.
        /// </summary>
        /// <param name="parameterName">System parameter name.</param>
        /// <returns type="">System parameter value or null.</returns>

        var jsUrl = edp_Common.getOrganizationServiceEndpoint() + "edp_SystemParameterSet?$select=" + parameterName + "&$top=1";
        var jsResult = edp_Common.fetchJSONResults(jsUrl);
        if (jsResult == null || jsResult.length == 0) {
            alert("Parameter not found");
            return null;
        }

        var values = jsResult[0];
        if (values != null)
            return values;
        else
            return null;
    },

    initializeFormWait: function () {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        if (!jQuery.isFunction(jQuery.fn.center)) {
            jQuery.fn.center = function () {
                this.css("position", "absolute");
                this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                            $(window).scrollTop()) + "px");
                this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                            $(window).scrollLeft()) + "px");
                return this;
            }
        }

        var divContent = ['<div id="edp_formWaitDiv" style="background: url(/_imgs/AdvFind/progress.gif) no-repeat left #ffffca;background-position:10px; left: 650px; top: 162px; width: 300px; height: 100px; display: block; position: absolute; border:1px solid black; position: relative;z-index: 100;">',
        '<p id="edp_formWaitText" style="font: 18px/normal Segoe UI, Tahoma, Arial; text-align: left; font-size-adjust: none; font-stretch: normal; word-wrap: break-word; margin-left: 50px;">',
        'Working on it...',
        '</p>',
        '<a id="edp_formWaitHideLink" href="#" style="position: absolute; bottom: 0px; right: 0px;">hide</a>',
        '</div>'].join('');

        // Don't add div if it already exists.
        if ($('#edp_formWaitDiv').length <= 0) {
            $('body').append(divContent);
        }

        // center, and hide it initially 
        $('#edp_formWaitDiv').center().hide();

        $('#edp_formWaitHideLink').click(function () { edp_Common.hideFormWait(); return false; });
    },

    setFormWaitText: function (text) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        $('#formWaitText').text(text);
    },

    setFormWaitImage: function (imageUrl) {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        $('#edp_formWaitDiv').css('background', 'url(' + imageUrl + ') no-repeat center');
    },

    showFormWait: function () {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        if ($('#edp_formWaitDiv').length > 0) {
            $('#edp_formWaitDiv').show();
        }
    },

    hideFormWait: function () {
        /// <deprecated>Use Endeavor.Common instead.</deprecated>
        if ($('#edp_formWaitDiv').length > 0) {
            $('#edp_formWaitDiv').hide();
        }
    }
} // End edp_Common