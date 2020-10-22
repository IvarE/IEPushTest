// References:
/// <reference path="Endeavor.Common.Page.js" />
/// <reference path="Endeavor.Common.Data.js" />
/// <reference path="vsdoc/XrmPage-vsdoc.js" />
/// <reference path="vsdoc/Sdk.Soap.vsdoc.js" />
/// <reference path="vsdoc/Sdk.edp_SearchCompany.vsdoc.js" />
/// <reference path="vsdoc/Sdk.edp_HandleCompanyData.vsdoc.js" />
var inputSearchCriteria = {};
var selectedRowsCount = 0;
var selectedRowsJson = [];
var parameterCRMId;
var parameterCountryCode;
var parameterAccountName;
var parameterOrgNo;
var compabilityMode = null;

// format function :-)
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
                ;
        });
    };
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}


function getURLParameter(name) {
    // Remove Data=
    var parameters = location.search.substring(6);
    var decodeStr = "?" + decodeURIComponent(parameters);

    return (new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(decodeStr) || [, ""])[1] || null;
}

if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.Creditsafe) == "undefined") {
    Endeavor.Creditsafe = {
    };
}


if (typeof (Endeavor.Creditsafe.SearchCreditsafe) == "undefined") {
    Endeavor.Creditsafe.SearchCreditsafe = {

        init: function (localisation) {
            var language = 1033;
            try {
                //Get user language from CRM
                language = Xrm.Page.context.getUserLcid();

                // Set english if no support for language.
                if (language != 1031 &&
                    language != 1033 &&
                    language != 1053)
                    language = 1033;

            } catch (error) {
                alert(error + "\n\nException caught in localisation")
            }

            //Load relevant language file
            var filename = "../script/Endeavor.Creditsafe.Resources." + language + ".js";
            var fileref = document.createElement('script')
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", filename);

            document.getElementsByTagName("head")[0].appendChild(fileref);

            //Need to wait for language file to be loaded before continuing
            window.setTimeout(localisation, 200);
        },

        initialLocalisation: function () {

            try {
                if (typeof (Endeavor.Creditsafe.Resources) == "undefined" ||
                    typeof (Endeavor.Creditsafe.Resources.PageTitle) == "undefined" ||
                    typeof $.jgrid == "undefined") {
                    window.setTimeout("Endeavor.Creditsafe.SearchCreditsafe.initialLocalisation", 200);
                    return;
                }

                // Get params from URL
                parameterCRMId = getURLParameter('CRMId');
                parameterCountryCode = getURLParameter('CountryCode');
                parameterAccountName = getURLParameter('AccountName');
                parameterOrgNo = getURLParameter('OrgNo');
                if (parameterCRMId != null && parameterCRMId.length > 36) {
                    // Remove {}
                    parameterCRMId = parameterCRMId.replace("{", "").replace("}", "");
                }


                //Only run if inside the correct document
                if ($(".LabelCompanyName").length) {
                    document.title = Endeavor.Creditsafe.Resources.PageTitle;
                    $(".LabelCompanyName").append(Endeavor.Creditsafe.Resources.LabelCompanyName);
                    $(".LabelOrganisationNumber").append(Endeavor.Creditsafe.Resources.LabelOrganisationNumber);
                    $(".LabelAdress").append(Endeavor.Creditsafe.Resources.LabelAdress);
                    $(".LabelPostalCode").append(Endeavor.Creditsafe.Resources.LabelPostalCode);
                    $(".LabelCity").append(Endeavor.Creditsafe.Resources.LabelCity);
                    $(".LabelCountry").append(Endeavor.Creditsafe.Resources.LabelCountry);
                    $(".Label-de-reason-code").append(Endeavor.Creditsafe.Resources.DE_ReasonCode);
                    $('#breadcrumbResult').append(Endeavor.Creditsafe.Resources.ResultLink);
                    $('#breadcrumbSearch').append(Endeavor.Creditsafe.Resources.SearchLink);
                    $('#search-title').append(Endeavor.Creditsafe.Resources.SearchTitle);
                    $('#search-desc').append(Endeavor.Creditsafe.Resources.SearchDesc);
                    $('#search').append(Endeavor.Creditsafe.Resources.SearchButton);
                    $('#button-lead-crm').append(Endeavor.Creditsafe.Resources.LeadButton);
                    if (parameterCRMId != null && parameterCRMId != "") {
                        $('#button-create-crm').append(Endeavor.Creditsafe.Resources.UpdateAccountButton);      // Change label on search button
                        $('#button-lead-crm').hide();
                    }
                    else {
                        $('#button-create-crm').append(Endeavor.Creditsafe.Resources.AccountButton);
                    }

                    // Add localization
                    // ***************
                    $('#Label-de-reason-code').text(Endeavor.Creditsafe.Resources.DE_ReasonCode);
                    // Build reasoncode 
                    $("#de-reason-code").append('<option value="' + 0 + '">' + '--</option>');
                    $("#de-reason-code").append('<option value="' + 1 + '">' + Endeavor.Creditsafe.Resources.Credit_Decisioning + '</option>');
                    $("#de-reason-code").append('<option value="' + 2 + '">' + Endeavor.Creditsafe.Resources.Credit_Assessment_Future_business + '</option>');
                    $("#de-reason-code").append('<option value="' + 3 + '">' + Endeavor.Creditsafe.Resources.Credit_Assessment_Existing_business + '</option>');
                    $("#de-reason-code").append('<option value="' + 4 + '">' + Endeavor.Creditsafe.Resources.Realisation_check_receivables_collection + '</option>');
                    $("#de-reason-code").append('<option value="' + 5 + '">' + Endeavor.Creditsafe.Resources.Purchase_Contract_Intention_of_signature + '</option>');
                    $("#de-reason-code").append('<option value="' + 6 + '">' + Endeavor.Creditsafe.Resources.Goods_Credit_Insurance + '</option>');
                    $("#de-reason-code").append('<option value="' + 7 + '">' + Endeavor.Creditsafe.Resources.Contract_Intention_of_signature + '</option>');
                    $("#de-reason-code").append('<option value="' + 8 + '">' + Endeavor.Creditsafe.Resources.Insurance_Contract_Intention_of_signature + '</option>');

                    // Set tooltip
                    $("#CompanyName").attr("title", Endeavor.Creditsafe.Resources.ToolTipSearch);

                    //Localisation for the grid
                    $.extend($.jgrid.defaults, {
                        pgtext: Endeavor.Creditsafe.Resources.GridPgText,
                        recordtext: Endeavor.Creditsafe.Resources.GridRecordText
                    });

                    //hide pager
                    $("#jqGridPager").hide();

                    //Getting countries from the CRM
                    try {
                        $("#countries").empty();
                        var options = "$select=edp_Name,edp_CountryCode";
                        SDK.REST.retrieveMultipleRecords("edp_Country"
                            , options
                            , Endeavor.Creditsafe.SearchCreditsafe.addCountriesToList
                            , function (error) { alert(error.message); }
                            , Endeavor.Creditsafe.SearchCreditsafe.search);
                    } catch (error) {
                        alert(error + "\n\Error caught in initialLocalisation while fetching countries");
                    }

                    // Get compatibility mode from config entity
                    Endeavor.Creditsafe.SearchCreditsafe.getCompabilityMode();
                }
            } catch (error) {
                alert(error + "\n\Error caught in fx:initialLocalisation");
            }
        },

        // Get compatibility mode from config entity
        getCompabilityMode: function () {
            try {
                var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$select=edp_CompabilityMode";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                if (resultSet.length > 0) {
                    compabilityMode = resultSet[0].edp_CompabilityMode
                }
            } catch (error) {
                alert(error + "\n\Error caught in fx:getCompabilityMode");
            }

            // Default to 2013 if no hit.
            if (compabilityMode == null)
                compabilityMode = 2013;

        },


        //Adds countries to the input list in the search view. Value of countries is set to country code in order to work for all localisations
        addCountriesToList: function (retrievedCountries) {

            // Get last used country from cookie
            var lastUsedCountry = getCookie("lastCountryCode");

            for (var i = 0; i < retrievedCountries.length; i++) {
                var selectedOption = "";
                if (lastUsedCountry == retrievedCountries[i].edp_CountryCode) {
                    selectedOption = " selected=" + '"' + "selected" + '"';
                }
                $("#countries").append('<option value="' + retrievedCountries[i].edp_CountryCode + '"' + selectedOption + '>' + retrievedCountries[i].edp_Name + ' - ' + retrievedCountries[i].edp_CountryCode + '</option>');
            }

        },


        //
        search: function () {
            $("#resultGrid").hide();
            $("#searchView").show();
            $("#resultLink").click(Endeavor.Creditsafe.SearchCreditsafe.showResultView);


            $("#search").click(function () {

                var checkNumber = 0;
                inputSearchCriteria = {};

                inputSearchCriteria.organisationNo = $("#OrganisationNumber").val();
                if ((inputSearchCriteria.organisationNo === "--")) {
                    inputSearchCriteria.organisationNo = "";
                    checkNumber += 1;
                }

                inputSearchCriteria.name = $("#CompanyName").val();
                if ((inputSearchCriteria.name === "--")) {
                    inputSearchCriteria.name = "";
                    checkNumber += 1;
                }

                inputSearchCriteria.address1_line1 = $("#Address").val();
                if (inputSearchCriteria.address1_line1 === "--") {
                    inputSearchCriteria.address1_line1 = "";
                    checkNumber += 1;
                }

                inputSearchCriteria.address1_city = $("#City").val();
                if (inputSearchCriteria.address1_city === "--") {
                    inputSearchCriteria.address1_city = "";
                    checkNumber += 1;
                }

                inputSearchCriteria.address1_postalcode = $("#PostalCode").val();
                if (inputSearchCriteria.address1_postalcode === "--") {
                    inputSearchCriteria.address1_postalcode = "";
                    checkNumber += 1;
                }

                inputSearchCriteria.country = $("#countries").val();
                setCookie("lastCountryCode", inputSearchCriteria.country, 365);         // Save value in cookie...

                if (checkNumber < 5) {
                    //Loading wait cursor
                    Endeavor.Creditsafe.SearchCreditsafe.run_Waitme("#searchView", Endeavor.Creditsafe.Resources.SearchWaitCursorLabel);

                    // Run async to populate wait cursor
                    setTimeout(
                        'Endeavor.Creditsafe.SearchCreditsafe.searchInCreditsafe(inputSearchCriteria, Endeavor.Creditsafe.SearchCreditsafe.createGrid)', 20);

                } else {
                    alert(Endeavor.Creditsafe.Resources.SearchErrorMsg)
                    return;
                }
            })

            // Do we have data as input?
            if (parameterCRMId != null && parameterCRMId != "") { // && ) {

                if (parameterOrgNo != null) {
                    $("#OrganisationNumber").val(parameterOrgNo);
                }
                else
                    $("#CompanyName").val(parameterAccountName);

                // Do autosearch if we have a country code.
                if (parameterCountryCode != null) {
                    // Set countrycode
                    $("#countries").val(parameterCountryCode);
                    $('#search').trigger('click');
                }
                // Clear parameter to avoid autosearch beeing called again
                parameterCountryCode = null;
            }

        },

        run_Waitme: function (div, displayText) {
            $(div).waitMe({
                effect: 'win8',
                text: displayText,
                bg: 'rgba(255,255,255,0.7)',
                color: '#000'
            });
        },

        searchInCreditsafe: function (inputCompany, createGrid) {
            /// <summary>
            /// Search for company with Creditsafe.
            /// </summary>
            try {

                // Fetch Compabilitymode if empty
                if (compabilityMode == null)
                    Endeavor.Creditsafe.SearchCreditsafe.getCompabilityMode();

                // CRM 2011?
                if (compabilityMode.Value == 2011) {
                    // Trigger Transaction via plugintrigger
                    var pluginTrigger = {};
                    pluginTrigger.edp_Name = "Search Creditsafe";
                    pluginTrigger.edp_PluginCommandJSON = encodeURIComponent(inputCompany.name + ";" + inputCompany.organisationNo + ";" + inputCompany.address1_line1 + ";" + inputCompany.address1_city + ";" + inputCompany.address1_postalcode + ";" + inputCompany.country);
                    SDK.REST.createRecord(
                        pluginTrigger,
                        "edp_PluginTrigger",
                        function (pluginTrigger) {
                            var response = pluginTrigger.edp_Message;

                            if (response == "undefined" || response === null) {
                                throw new Error("The request did not return anything.");
                            }
                            createGrid(response);

                        }, function (pluginTriggerError) {
                            $("#resultGrid").waitMe('hide');
                            alert("CreateRecord (plugin trigger) generated an error: " + pluginTriggerError);
                        });
                }
                // CRM 2013 and up ?
                else {
                    var request = new Sdk.edp_SearchCompanyRequest(inputCompany.organisationNo
                        , encodeURIComponent(inputCompany.name)
                        , encodeURIComponent(inputCompany.address1_line1)
                        , encodeURIComponent(inputCompany.address1_city)
                        , encodeURIComponent(inputCompany.address1_postalcode)
                        , inputCompany.country);
                    var response = Sdk.Sync.execute(request);

                    if (response === null) {
                        throw new Error("The request did not return anything.");
                    }
                    var jsonString = response.getOutAccounts();
                    createGrid(jsonString);
                }
            }
            catch (error) {
                alert("Creditsafe configuration could not be found. Please make sure a valid configuration is available");
                location.reload();
            }
        },

        createGrid: function (responseAsJson) {
            //Stop loading splash
            $("#searchView").waitMe('hide');

            var selectedCountry = $("#countries").val();

            //Set links of breadcrumbs
            $("#breadcrumbSearch").removeClass("active");
            $('#breadcrumbSearch').empty();
            //            $('#breadcrumbSearch').append("<a id='searchLink' href='#'>" + Endeavor.Creditsafe.Resources.SearchLink + "</a>");
            $('#breadcrumbSearch').append("<a id='searchLink' href='#'>" + Endeavor.Creditsafe.Resources.SearchLink + "</a>");
            $("#breadcrumbResult").addClass("active");
            $("#breadcrumbResult").empty();
            $('#breadcrumbResult').append("<a id='resultLink' href='#'>" + Endeavor.Creditsafe.Resources.ResultLink + "</a>");

            Endeavor.Creditsafe.SearchCreditsafe.showResultView();

            if (selectedCountry == 'DE') {
                $("#localized-footer").show();
            }
            else {
                $("#localized-footer").hide();
            }

            //check for old grid and reload it
            $("#jqGrid").jqGrid('setGridParam', { datatype: "jsonstring", datastr: responseAsJson }).trigger("reloadGrid");

            $("#jqGrid").jqGrid({
                datatype: "jsonstring",
                datastr: responseAsJson,
                colModel: [
                    //Resultset (hidden) -- Used to create json for return set
                    { label: 'edp_OrganisationNumber', name: 'edp_OrganisationNumber', width: 90, hidden: true },
                    { label: 'Name', name: 'Name', width: 150, hidden: true },
                    { label: 'Street1', name: 'Street1', width: 110, hidden: true },
                    { label: 'Address1_City', name: 'Address1_City', width: 80, hidden: true },
                    { label: 'Address1_PostalCode', name: 'Address1_PostalCode', width: 40, hidden: true },
                    { label: 'edp_CountryName.Id', name: 'edp_CountryName.Id', width: 80, hidden: true },
                    { label: 'edp_CountryName.Name', name: 'edp_CountryName.Name', width: 80, hidden: true },
                    { label: 'edp_CountryName.LogicalName', name: 'edp_CountryName.LogicalName', width: 80, hidden: true },
                    { label: 'edp_Address1_CountryCode', name: 'edp_Address1_CountryCode', width: 80, hidden: true },
                    { label: 'Address1_Lattitude', name: 'Address1_Lattitude', width: 80, hidden: true },
                    { label: 'Address1_Longitude', name: 'Address1_Longitude', width: 80, hidden: true },
                    { label: 'CRMId', name: 'CRMId', width: 9, hidden: true },
                    { label: 'ReportType', name: 'ReportType', width: 80, hidden: true },
                    { label: 'ggsId', name: 'ggsId', width: 80, hidden: true, key: true },

                    //Visible columns
                    { label: 'CRM', name: 'CRMId', width: 30, formatter: Endeavor.Creditsafe.SearchCreditsafe.existInCrmFormat },
                    { label: Endeavor.Creditsafe.Resources.LabelOrganisationNumber, name: 'edp_OrganisationNumber', width: 90 },
                    { label: Endeavor.Creditsafe.Resources.LabelCompanyName, name: 'Name', width: 150 },
                    { label: Endeavor.Creditsafe.Resources.LabelCompanyType, name: 'CompanyType', width: 30 },
                    { label: Endeavor.Creditsafe.Resources.LabelAdress, name: 'Street1', width: 110 },
                    { label: Endeavor.Creditsafe.Resources.LabelPostalCode, name: 'Address1_PostalCode', width: 40 },
                    { label: Endeavor.Creditsafe.Resources.LabelCity, name: 'Address1_City', width: 80 },
                    { label: Endeavor.Creditsafe.Resources.LabelStatus, name: 'Status', width: 80 },
                    { label: Endeavor.Creditsafe.Resources.LabelCountry, name: 'edp_CountryName.Name', width: 80 },
                ],
                loadonce: true,
                viewrecords: true,
                width: 1080,
                height: 240,
                rowNum: 50,
                rowList: [20, 30, 50],
                rownumbers: false,
                rownumWidth: 25,
                multiselect: true,
                pager: "#jqGridPager",
                jsonReader: {
                    repeatitems: false,
                    id: "edp_OrganisationNumber",
                    root: function (obj) { return obj; },
                    page: function (obj) { return 1; },
                    total: function (obj) { return 1; },
                    records: function (obj) { return obj.length; }
                },
                onSelectRow: function (rowid, status, e) {
                    //if company exist in crm
                    if (($('#jqGrid').jqGrid('getCell', rowid, 'CRMId')) != '00000000-0000-0000-0000-000000000000') {
                        //when selected
                        var rowID = '#' + rowid.split("/").join("");       // Remove all "/"
                        if (status) {
                            //make sure that "exist in crm" colorcode dissapears
                            $(rowID).css("background-color", "");
                            $(rowID).unbind('mouseover mouseout');
                        } else {
                            //put back colorcoding when deselected
                            $(rowID).css("background-color", "#E0FFE0");
                            $(rowID).on('mouseout', function () {
                                $(this).css("background-color", "#E0FFE0");
                            });
                            $(rowID).on('mouseover', function () {
                                $(this).css("background-color", "#D7EBF9");
                            });
                        }
                    }
                },
                gridComplete: function () {
                    Endeavor.Creditsafe.SearchCreditsafe.changeGridCheckbox();
                    Endeavor.Creditsafe.SearchCreditsafe.colorCodeRows();
                }
            });

            // Make sure old events are removed! Use cloning
            var old_element = document.getElementById("button-create-crm");
            var new_element = old_element.cloneNode(true);
            old_element.parentNode.replaceChild(new_element, old_element);

            var old_element = document.getElementById("button-lead-crm");
            var new_element = old_element.cloneNode(true);
            old_element.parentNode.replaceChild(new_element, old_element);

            var old_element = document.getElementById("searchLink");
            var new_element = old_element.cloneNode(true);
            old_element.parentNode.replaceChild(new_element, old_element);

            //Add event listeners to the buttons 
            document.getElementById("button-create-crm").addEventListener("click", function () {
                Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows("Account")
            });
            document.getElementById("button-lead-crm").addEventListener("click", function () {
                Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows("Lead")
            });
            document.getElementById("searchLink").addEventListener("click", Endeavor.Creditsafe.SearchCreditsafe.search);
        },

        showResultView: function () {
            $("#searchView").hide();
            $("#resultGrid").show();
        },

        showSearchView: function () {
            $("#searchView").show();
            $("#resultGrid").hide();
        },


        existInCrmFormat: function (cellvalue, options, rowObject) {
            if (cellvalue == "00000000-0000-0000-0000-000000000000") {
                return ""
            } else {
                return '<img class="crmDisplay" src="../image/crmlogo_16.png" />'
            }
        },

        changeGridCheckbox: function () {
            $(".cbox").each(function (index) {
                $(this).after('<img id="img_' + index + '" class="checkbox_light" src="../image/transparent_spacer.gif">');
                $(this).attr("onClick", "Endeavor.Creditsafe.SearchCreditsafe.checked()");
            })
        },

        //Colorcode rows for accounts already existing in CRM
        colorCodeRows: function () {
            var ids = $("#jqGrid").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                if (($('#jqGrid').jqGrid('getCell', ids[i], 'CRMId')) != '00000000-0000-0000-0000-000000000000') {
                    var rowID = '#' + ids[i].split("/").join("");       // Remove all "/"
                    $(rowID).css("background-color", "#E0FFE0");
                    $(rowID).on('mouseover', function () {
                        $(this).css("background-color", "#D7EBF9");
                    }).on('mouseout', function () {
                        $(this).css("background-color", "#E0FFE0"); // old value#D7EBF9
                    });
                }
            }
        },

        //Gets all selected rows and creates a json structure with all the values to be sent to the backend.
        //Requires entity name, as a string,"Account" or "Lead" to be passed in
        getSelectedRows: function (entityName) {
            var jsonArray = [];
            var grid = $("#jqGrid");
            var rowKey = grid.getGridParam("selrow");
            var reasonCode = '0';                       // Default DE reason code.

            var selectedCountry = $("#countries").val();
            if (selectedCountry == 'DE') {
                reasonCode = $("#de-reason-code").val();
                if (reasonCode == null || reasonCode == '' || reasonCode == '0') {
                    alert(Endeavor.Creditsafe.Resources.EnterCreditReasonCode);
                    return;
                }
            }

            //Check that at least one row is selected
            if (!rowKey)
                alert(Endeavor.Creditsafe.Resources.SelectedRowsErrorMsg);
            else {
                var selectedIDs = grid.getGridParam("selarrrow");

                //Create a json-like array with all the values to be passed to the backend
                for (var i = 0; i < selectedIDs.length; i++) {

                    var name = grid.jqGrid('getCell', selectedIDs[i], 'Name');
                    var companyType = grid.jqGrid('getCell', selectedIDs[i], 'CompanyType');
                    var getCreditInfo = "";         // Default to empty string

                    // Make sure only one row is selected if we are in update mode
                    var rowCRMID = grid.jqGrid('getCell', selectedIDs[i], 'CRMId');
                    if (parameterCRMId != null && parameterCRMId != "") {
                        if (selectedIDs.length != 1) {
                            alert(Endeavor.Creditsafe.Resources.OnlyOneSelectedRowErrorMsg);
                            return;
                        }
                        // Set ID from CRM to update existing record.
                        rowCRMID = parameterCRMId;
                    }

                    // If Sweden and EF, KB or HB, ask if credit
                    if (companyType === "EF" || companyType === "HB_KB") {
                        var msg = String.format(Endeavor.Creditsafe.Resources.QuestionUseCreditBlockName, name, companyType);   // "Account " + Name + " is not mapped in Creditsafe. Do mapping now?"
                        if (confirm(msg)) {
                            getCreditInfo = "YES";
                        }
                    }

                    var item = {
                        "edp_OrganisationNumber": grid.jqGrid('getCell', selectedIDs[i], 'edp_OrganisationNumber'),
                        "Name": encodeURIComponent(name),
                        "Street1": encodeURIComponent(grid.jqGrid('getCell', selectedIDs[i], 'Street1')),
                        "Address1_City": encodeURIComponent(grid.jqGrid('getCell', selectedIDs[i], 'Address1_City')),
                        "Address1_PostalCode": encodeURIComponent(grid.jqGrid('getCell', selectedIDs[i], 'Address1_PostalCode')),
                        "edp_CountryName": {
                            "Id": grid.jqGrid('getCell', selectedIDs[i], 'edp_CountryName.Id'),
                            "LogicalName": grid.jqGrid('getCell', selectedIDs[i], 'edp_CountryName.LogicalName'),
                            "Name": grid.jqGrid('getCell', selectedIDs[i], 'edp_CountryName.Name')
                        },
                        "edp_Address1_CountryCode": grid.jqGrid('getCell', selectedIDs[i], 'edp_Address1_CountryCode'),
                        "Address1_Lattitude": grid.jqGrid('getCell', selectedIDs[i], 'Address1_Lattitude'),
                        "Address1_Longitude": grid.jqGrid('getCell', selectedIDs[i], 'Address1_Longitude'),
                        "Status": grid.jqGrid('getCell', selectedIDs[i], 'Status'),
                        "CRMId": rowCRMID,
                        "ReportType": grid.jqGrid('getCell', selectedIDs[i], 'ReportType'),
                        "ggsId": grid.jqGrid('getCell', selectedIDs[i], 'ggsId'),
                        "EntityName": entityName,
                        "ReasonCode": reasonCode,
                        "CompanyType": companyType,
                        "GetCreditInfo": getCreditInfo,
                    };
                    jsonArray.push(item);
                }
                selectedRowsJson = [];
                selectedRowsJson = jsonArray;
                selectedRowsCount = selectedIDs.length
                Endeavor.Creditsafe.SearchCreditsafe.run_Waitme("#resultGrid", Endeavor.Creditsafe.Resources.ProcessingWaitCursorLabel);
                // Must call async for wait cursor to work
                setTimeout('Endeavor.Creditsafe.SearchCreditsafe.updateCRM(selectedRowsJson, selectedRowsCount, null, "' + entityName + '")', 20);
            }
        },

        updateCRM: function (json, noOfRecords, noGUI, entityName) {
            try {
                // Fetch Compabilitymode if empty
                if (compabilityMode == null)
                    Endeavor.Creditsafe.SearchCreditsafe.getCompabilityMode();

                // CRM 2011?
                if (compabilityMode.Value == 2011) {
                    // Trigger Transaction via plugintrigger
                    var pluginTrigger = {};
                    pluginTrigger.edp_Name = "Creditsafe:Update CRM";
                    pluginTrigger.edp_PluginCommandJSON = JSON.stringify(json);
                    SDK.REST.createRecord(
                        pluginTrigger,
                        "edp_PluginTrigger",
                        function (pluginTrigger) {
                            if (noGUI == null) {
                                $("#resultGrid").waitMe('hide');
                                alert(String.format(Endeavor.Creditsafe.Resources.SuccessCreateUpdateMessage, noOfRecords));
                                window.close();
                            }
                        }, function (pluginTriggerError) {
                            if (noGUI == null) {
                                $("#resultGrid").waitMe('hide');
                            }
                            alert("updateCRM-CreateRecord (plugin trigger) generated an error: " + pluginTriggerError);
                        });

                } else {
                    var sdkObject = new Sdk.edp_HandleCompanyDataRequest(JSON.stringify(json));
                    var response = Sdk.Sync.execute(sdkObject);
                    if (noGUI == null) {
                        $("#resultGrid").waitMe('hide');
                        Xrm.Utility.alertDialog(response.getOutMessage() + "\n\rNumber of " + entityName + "s processed: " + noOfRecords);
                        window.close();
                        try {
                            if (window != null && window.parent != null && window.parent.opener != null) {
                                window.parent.opener.location.reload();
                            }
                        }
                        catch (error) {
                            //do nothing
                        }
                    }
                    return response.getOutMessage();
                }
                return true;

            } catch (error) {
                if (noGUI == null) {
                    $("#resultGrid").waitMe('hide');
                }
                alert(error + "\n\nException caught in updateCRM.");
                return false;
            }
        },

        //Removes the default value of the input field when focused
        inputOnFocus: function (ths) {
            var elem = document.getElementById(ths.id);
            if (elem.value == '--') {
                elem.value = '';
            }
        },

        //Enters the default value of the input field if nothing is entered
        inputOnBlur: function (ths) {
            var elem = document.getElementById(ths.id);
            if (elem.value == '') {
                elem.value = '--';
            }
        },

        //Used by the button in CRM- Account overview
        openSearchWindow: function (AccountName, CRMId, countryCodeISO, orgNo) {
            var encodedParams = "";

            if (CRMId != null) {
                if (orgNo == null)
                    orgNo = "";
                var params = String.format("CRMId={0}&CountryCode={1}&AccountName={2}&OrgNo={3}", CRMId, countryCodeISO, AccountName, orgNo);
                encodedParams = encodeURIComponent(params);
                //encodedParams = "?Data=" + encodeURIComponent(params);
            }

            //var url = "WebResources/edp_/html/CreditsafeSearchView.html" + encodedParams;
            Xrm.Utility.openWebResource("edp_/html/CreditsafeSearchView.html", encodedParams, 1200, 450);
            //window.open(url, "SearchWindow", "width=1200, height=450");
        },

    }
};
if (typeof ($) != "undefined") {
    $(document).ready(Endeavor.Creditsafe.SearchCreditsafe.init(Endeavor.Creditsafe.SearchCreditsafe.initialLocalisation));
}