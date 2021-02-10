
// This file is maintained through Endeavor NuGet. Please do not modify it directly in your project.

// Common functions for CRM.
// Be very strict with new functions in this file

// Dependencies:
/// <reference path="SDK.Rest.js" />
/// <reference path="jquery-1.11.1.min.js" />

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Common) == "undefined") {
    Endeavor.Common = {
    };
}

if (typeof (Endeavor.Common.Data) == "undefined") {
    Endeavor.Common.Data = {

        fetchJSONResults: function (url, max_records) {
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
                    var nextRecords = Endeavor.Common.Data.fetchJSONResults(nextUrl, max_records);
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
            /// <summary>
            /// Gets the Organization Service Endpoint for OData.
            /// </summary>
            baseOrganizationServiceEndpoint = SDK.REST._ODataPath();
            return baseOrganizationServiceEndpoint;
        },

        dateToString: function (oDate, sFormat, sSeparator) {
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
                        sDate += Endeavor.Common.Data.zeroFill(oDate.getMonth() + 1, 2) + sSeparator;
                        break;
                    case "d":
                        sDate += oDate.getDate() + sSeparator;
                        break;
                    case "dd":
                        sDate += Endeavor.Common.Data.zeroFill(oDate.getDate(), 2) + sSeparator;
                        break;
                    default:
                        sDate += dateFormatPart + sSeparator;
                }
            }
            sDate = sDate.substring(0, sDate.length - 1);
            return sDate;
        },

        stringToDate: function (sDate, sFormat, sSeparator) {
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
            /// <summary>
            /// Check if date is valid
            /// </summary>
            /// <returns type="">true / false</returns>

            var aDate = sDate.split(sDateSeparator);
            var aDateFormat = sDateFormat.split("/");
            return Endeavor.Common.Data.isDate(aDate, aDateFormat);
        },

        isDate: function (aDate, aFormat) {
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

        isGuid: function (sGUID) {
            /// <summary>
            /// Verify that string is a valid GUID
            /// </summary>
            /// <param name="sGUID">String to test.</param>

            if (sGUID.length == 38) // 36 + {}
                return true;
            else
                return false;
        },

        zeroFill: function (iNumber, iLength) {
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

        trimString: function (str) {
            /// <summary>
            /// Trim whitespaces from start and end.
            /// </summary>
            /// <param name="str">String to trim.</param>
            /// <returns type="string">Trimmed string.</returns>

            return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        },

        getSystemParameterValue: function (parameterName) {
            /// <summary>
            /// Get edp_SystemParameter value.
            /// </summary>
            /// <param name="parameterName">System parameter name.</param>
            /// <returns type="">System parameter value or null.</returns>

            var jsUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_SystemParameterSet?$select=" + parameterName + "&$top=1";
            var jsResult = Endeavor.Common.Data.fetchJSONResults(jsUrl);
            if (jsResult == null || jsResult.length == 0) {
                alert("Parameter not found");
                return null;
            }

            var values = jsResult[0];
            if (values != null)
                return values;
            else
                return null;
        }
    }
}