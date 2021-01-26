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
var tmpFormContext = null;
var GlobalRelationTypes = [];

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
    decodeStr = decodeURIComponent(decodeStr);

    if (name === 'AccountName') {

        //Special handling of AccountName, since it may contain ampersand (&), causing regexp below to cut string too early:
        try {
            var accountName = decodeStr.substring(
                decodeStr.lastIndexOf("&OrgNo"),
                decodeStr.lastIndexOf("&AccountName")
            );
            accountName = accountName.replace('&AccountName=', '');
            if (accountName.length > 0) {
                return accountName;
            }
        } catch (ex) {
            console.log('AccountName parsing error in getUrlParameter: ' + ex);
        }
    }
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
                language = Xrm.Utility.getGlobalContext().userSettings.languageId;

                // Set english if no support for language.
                if (language != 1031 &&
                    language != 1033 &&
                    language != 1053)
                    language = 1033;

            } catch (error) {
                alert(error + "\n\nException caught in localisation")
            }

            //Load relevant language file


            var filename = Xrm.Utility.getGlobalContext().getClientUrl() + "/WebResources/edp_/script/Endeavor.Creditsafe.Resources." + language + ".js";
            var fileref = document.createElement('script')
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", filename);

            document.getElementsByTagName("head")[0].appendChild(fileref);

            //Need to wait for language file to be loaded before continuing
            window.setTimeout(localisation, 200);
        },

        onWindowResize: function () {
            var newWidth = window.innerWidth;
            newWidth = newWidth - (0.1 * newWidth);
            var newHeight = window.innerHeight;
            newHeight = newHeight - (0.4 * newHeight);
            $('#jqGrid').jqGrid("setGridWidth", newWidth, true);
            $('#jqGrid').jqGrid("setGridHeight", newHeight, true);
            //console.log("Window is resized!");
        },

        hideOrShowRelationTypes: function () {
            var entNames = {};
            entNames["edp_SystemParameter"] = "edp_SystemParameters";
            window.ENTITY_SET_NAMES = JSON.stringify(entNames);
            var primaryKeys = {};
            primaryKeys["edp_SystemParameter"] = "edp_SystemParameterId";
            window.ENTITY_PRIMARY_KEYS = JSON.stringify(primaryKeys);

            var sysparamEntityNameString = "edp_systemparameters";

            if (Xrm.Internal.isUci()) {
                sysparamEntityNameString = "edp_systemparameter";
            }

            Xrm.WebApi.retrieveMultipleRecords(sysparamEntityNameString, "?$select=edp_showrelationtype&$filter=edp_showrelationtype ne null").then(
                function (result) {
                    if (result && result.entities.length > 0 && result.entities[0].edp_showrelationtype) {
                        $("#relation-type-label").text(Endeavor.Creditsafe.Resources.RelationType);
                    } else {
                        $("#relation-type-label").hide();
                        $("#relation-type").hide();
                    }
                },
                function (e) {
                    $("#relation-type-label").hide();
                    $("#relation-type").hide();
                }
            );
        },

        initialLocalisation: function () {

            try {
                if (typeof (Endeavor.Creditsafe.Resources) == "undefined" ||
                    typeof (Endeavor.Creditsafe.Resources.PageTitle) == "undefined" ||
                    typeof $.jgrid == "undefined") {
                    window.setTimeout("Endeavor.Creditsafe.SearchCreditsafe.initialLocalisation", 200);
                    return;
                }

                window.addEventListener("resize", Endeavor.Creditsafe.SearchCreditsafe.onWindowResize);

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

                    Endeavor.Creditsafe.SearchCreditsafe.hideOrShowRelationTypes();


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
                        console.log("BEFORE MULTI RETRIEVE");

                        var entNames = {};
                        entNames["edp_Country"] = "edp_Countries";
                        window.ENTITY_SET_NAMES = JSON.stringify(entNames);
                        var primaryKeys = {};
                        primaryKeys["edp_Country"] = "edp_CountryId";
                        window.ENTITY_PRIMARY_KEYS = JSON.stringify(primaryKeys);

                        var countryEntityNameString = "edp_countries";

                        if (Xrm.Internal.isUci()) {
                            countryEntityNameString = "edp_country";
                        }

                        Xrm.WebApi.retrieveMultipleRecords(countryEntityNameString, "?$select=edp_name,edp_countrycode&$orderby=edp_name").then(
                            function (result) {
                                if (result.entities) {
                                    Endeavor.Creditsafe.SearchCreditsafe.addCountriesToList(result.entities);
                                }
                                Endeavor.Creditsafe.SearchCreditsafe.search();
                                Endeavor.Creditsafe.SearchCreditsafe.getCompabilityMode();
                            },
                            function (error) {
                                alert(error.message);
                            }
                        );
                    } catch (error) {
                        alert(error + "\n\Error caught in initialLocalisation while fetching countries");
                    }
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

        populateCountriesList: function (retrievedCountries, defaultCountryCode) {
            for (var i = 0; i < retrievedCountries.length; i++) {
                var selectedOption = "";
                if (defaultCountryCode == retrievedCountries[i].edp_countrycode) {
                    selectedOption = " selected=" + '"' + "selected" + '"';
                }
                $("#countries").append('<option value="' + retrievedCountries[i].edp_countrycode + '"' + selectedOption + '>' + retrievedCountries[i].edp_name + ' - ' + retrievedCountries[i].edp_countrycode + '</option>');
            }
        },

        //Adds countries to the input list in the search view. Value of countries is set to country code in order to work for all localisations
        addCountriesToList: function (retrievedCountries) {

            debugger;

            // Get last used country from cookie
            //var lastUsedCountry = getCookie("lastCountryCode");
            var lastUsedCountry = Endeavor.Common.Data.readCookie("lastCountryCode");
            var sysparamEntityNameString = "edp_systemparameters";

            if (Xrm.Internal.isUci()) {
                sysparamEntityNameString = "edp_systemparameter";
            }

            if (lastUsedCountry == null || lastUsedCountry == 'undefined' || lastUsedCountry == '') {

                Xrm.WebApi.retrieveMultipleRecords(sysparamEntityNameString, "?$select=edp_DefaultSearchCountryId&$expand=edp_DefaultSearchCountryId($select=edp_countrycode)").then(
                    function (result) {
                        if (result.entities && result.entities[0].edp_DefaultSearchCountryId != null && result.entities[0].edp_DefaultSearchCountryId.edp_countrycode != null) {
                            var defaultCountryCode = result.entities[0].edp_DefaultSearchCountryId.edp_countrycode;
                            Endeavor.Creditsafe.SearchCreditsafe.populateCountriesList(retrievedCountries, defaultCountryCode);
                        }
                        Endeavor.Creditsafe.SearchCreditsafe.populateCountriesList(retrievedCountries, null);
                    },
                    function (error) {
                        console.log("Error occurred when fetching systen parameter edp_defaultsearchcountryid.");
                        Endeavor.Creditsafe.SearchCreditsafe.populateCountriesList(retrievedCountries, null);
                    }
                );

            } else {
                Endeavor.Creditsafe.SearchCreditsafe.populateCountriesList(retrievedCountries, lastUsedCountry);
            }
        },


        //
        search: function () {
            $("#resultGrid").hide();
            $("#searchView").show();
            $("#resultLink").unbind('click');
            $("#resultLink").click(Endeavor.Creditsafe.SearchCreditsafe.showResultView);

            $("#search").unbind('click');
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

                //setCookie("lastCountryCode", inputSearchCriteria.country, 365); // Save value in cookie...
                Endeavor.Common.Data.createCookie("lastCountryCode", inputSearchCriteria.country, 365); // Save value in cookie...

                if (checkNumber < 5) {
                    //Loading wait cursor
                    Endeavor.Creditsafe.SearchCreditsafe.run_Waitme("#searchView", Endeavor.Creditsafe.Resources.SearchWaitCursorLabel);

                    // Run async to populate wait cursor
                    setTimeout(
                        'Endeavor.Creditsafe.SearchCreditsafe.searchInCreditsafe(inputSearchCriteria, Endeavor.Creditsafe.SearchCreditsafe.createGrid)', 20);

                } else {
                    alert(Endeavor.Creditsafe.Resources.SearchErrorMsg);
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

                var request = {
                    OrganisationNo: inputCompany.organisationNo,
                    Name: inputCompany.name,
                    Street1: inputCompany.address1_line1,
                    City: inputCompany.address1_city,
                    PostalCode: inputCompany.address1_postalcode,
                    Country: inputCompany.country,
                    getMetadata: function () {
                        return {
                            boundParameter: null,
                            parameterTypes: {
                                "OrganisationNo": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                },
                                "Name": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                },
                                "Street1": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                },
                                "City": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                },
                                "PostalCode": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                },
                                "Country": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1
                                }
                            },
                            operationType: 0,
                            operationName: "edp_SearchCompany"
                        };
                    }
                };

                Xrm.WebApi.online.execute(request).then(
                    function (result) {
                        if (result.ok) {
                            result.json().then(
                                function (response) {
                                    var jsonString = response.OutAccounts;
                                    createGrid(jsonString);
                                }
                            );
                        } else {
                            alert("Something went wrong. Please try again."); // will this ever happen?
                            location.reload();
                        }
                    },
                    function (error) {
                        alert(error.message);
                        location.reload();
                    });
            }
            catch (error) {
                alert("Creditsafe configuration could not be found. Please make sure a valid configuration is available");
                location.reload();
            }
        },

        //fetchRelationTypesFromRows: function (jsonArray) {
        //    try {
        //        var resultArray = [];
        //        var accountsCrmId = [];
        //        for (var i = 0; i < jsonArray.length; i++) {
        //            var json = jsonArray[i];
        //            if (json && json.CRMId && json.CRMId !== "00000000-0000-0000-0000-000000000000") {
        //                var id = "%27" + json.CRMId.toUpperCase() + "%27";
        //                accountsCrmId.push(id);
        //            } else {
        //                var obj = {
        //                    "_edp_relationtypeid_value": "00000000-0000-0000-0000-000000000000",
        //                    "accountid": "00000000-0000-0000-0000-000000000000"
        //                }
        //                resultArray.push(obj);
        //            }
        //        }
        //        //%2775DEDB81-83CD-E911-A975-000D3A389769%27,%27BA73F57B-DBC3-E911-A96C-000D3A389D2C%27
        //        var properyValues = "";
        //        for (var i = 0; i < accountsCrmId.length - 1; i++) {
        //            properyValues += accountsCrmId[i] + ",";
        //        }
        //        properyValues += accountsCrmId[accountsCrmId.length - 1]; //last item, no comma seperator

        //        var entityNameString = "accounts";

        //        if (Xrm.Internal.isUci()) {
        //            entityNameString = "account";
        //        }

        //        var query = "?$select=accountid,_edp_relationtypeid_value&$filter=Microsoft.Dynamics.CRM.In(PropertyName=%27accountid%27,PropertyValues=[" + properyValues + "])";
        //        return Xrm.WebApi.retrieveMultipleRecords(entityNameString, query, 50).then(
        //            function (r) {
        //                if (r && r.entities && r.entities.length > 0) {
        //                    for (var i = 0; i < r.entities.length; i++) {
        //                        resultArray.push(r.entities[i]);
        //                    }
        //                    return resultArray;
        //                } else {
        //                    return "";
        //                }
        //            },
        //            function (error) {
        //                console.log(error.message);
        //                return "";
        //            }
        //        );
        //    } catch (error) {
        //        console.log("Error in fetchRelationType: " + error)
        //    }
        //},

        createGrid: function (responseAsJson) {
            //Stop loading splash
            Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();

            var selectedCountry = $("#countries").val();

            //Set links of breadcrumbs
            $("#breadcrumbSearch").removeClass("active");
            $('#breadcrumbSearch').empty();
            //            $('#breadcrumbSearch').append("<a id='searchLink' href='#'>" + Endeavor.Creditsafe.Resources.SearchLink + "</a>");
            $('#breadcrumbSearch').append("<a id='searchLink' href='#'>" + Endeavor.Creditsafe.Resources.SearchLink + "</a>");
            $("#breadcrumbResult").addClass("active");
            $("#breadcrumbResult").empty();
            $('#breadcrumbResult').append("<a id='resultLink' href='#'>" + Endeavor.Creditsafe.Resources.ResultLink + "</a>");

            //Empty relation type drop down control:
            $("#relation-type").empty();

            Endeavor.Creditsafe.SearchCreditsafe.showResultView();

            if (selectedCountry == 'DE') {
                $("#Label-de-reason-code").show();
                $("#de-reason-code").show();
            }
            else {
                $("#Label-de-reason-code").hide();
                $("#de-reason-code").hide();
            }

            //check for old grid and reload it
            $("#jqGrid").jqGrid('clearGridData');
            $("#jqGrid").jqGrid('setGridParam', { datatype: "jsonstring", datastr: responseAsJson }).trigger("reloadGrid");

            $("#jqGrid").jqGrid({
                datatype: "jsonstring",
                datastr: responseAsJson,
                colModel: [
                    //Resultset (hidden) -- Used to create json for return set
                    { label: 'RelationType', name: "edp_RelationType", width: 70, hidden: true }, //not present in account fetched from creditsafe, used to store lookup o nrow select
                    { label: 'RelationTypeName', name: "edp_RelationTypeName", width: 70, hidden: true },
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
                    Endeavor.Creditsafe.SearchCreditsafe.onRowsSelect();
                },

                gridComplete: function () {
                    Endeavor.Creditsafe.SearchCreditsafe.changeGridCheckbox();
                    Endeavor.Creditsafe.SearchCreditsafe.colorCodeRows();
                    //Endeavor.Creditsafe.SearchCreditsafe.fetchAllRelationTypes().then(
                    //    function (r) {
                    //        Endeavor.Creditsafe.SearchCreditsafe.addAllOtherRelationTypes();
                    //        console.log("GlobalRelationTypes.length: " + GlobalRelationTypes.length);
                    //    },
                    //    function (error) {
                    //        $("#relation-type").empty();
                    //        $("#relation-type").append("<option value=empty> -- </option>");
                    //    }
                    //);
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

            var old_element = document.getElementById("button-create-crm");
            var new_element = old_element.cloneNode(true);
            old_element.parentNode.replaceChild(new_element, old_element);

            //Add event listeners to the buttons
            document.getElementById("button-create-crm").addEventListener("click", function () {
                Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows("Account")
                //Endeavor.Creditsafe.SearchCreditsafe.confirmRelationTypeChanges("Account");
            });
            document.getElementById("button-lead-crm").addEventListener("click", function () {
                Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows("Lead")
            });
            document.getElementById("searchLink").addEventListener("click", Endeavor.Creditsafe.SearchCreditsafe.search);
            //document.getElementById("button-contact-crm").addEventListener("click", function () {
            //    Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows("Lead")
            //});
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

        //confirmRelationTypeChanges: function (entityName) {
        //    var selectedRelationType = $("#relation-type").val();
        //    var selectedRelationName = $("#relation-type option:selected").text();
        //    var selectedIDs = $("#jqGrid").getGridParam("selarrrow");
        //    var message = Endeavor.Creditsafe.Resources.ChangeRelationType + ":\n"; //"You are about to change the following relation types:\n";
        //    var confirmFlag = false;

        //    for (var i = 0; i < selectedIDs.length; i++) {
        //        var rowRelationId = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'edp_RelationType');
        //        var rowCRMId = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'CRMId');
        //        var bothEmpty = !(rowRelationId === "" && selectedRelationType === "empty");
        //        var notEqual = rowRelationId !== selectedRelationType;
        //        var freshCompany = rowCRMId === "" || rowCRMId === "00000000-0000-0000-0000-000000000000";

        //        if (notEqual && bothEmpty && !freshCompany) {
        //            confirmFlag = true;
        //            var rowRelationName = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'edp_RelationTypeName');
        //            var rowCompanyName = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'Name');

        //            rowRelationName = rowRelationName === "" ? "N/A" : rowRelationName;
        //            rowCompanyName = rowCompanyName === "" ? "N/A" : rowCompanyName;
        //            selectedRelationName = selectedRelationName === " -- " ? "N/A" : selectedRelationName;
        //            message += rowCompanyName + ": " + rowRelationName + " --> " + selectedRelationName + "\n";
        //        }
        //    }
        //    message += Endeavor.Creditsafe.Resources.SureProceed;//"Do you want to proceed?";

        //    var confirmStrings = {
        //        text: message, title: Endeavor.Creditsafe.Resources.DetectedRelationTypeChanges,//"Detected RelationType Change",
        //        confirmButtonLabel: Endeavor.Creditsafe.Resources.Confirm, cancelButtonLabel: Endeavor.Creditsafe.Resources.Cancel
        //    };
        //    var confirmOptions = { height: 250, width: 500 };

        //    if (confirmFlag) {
        //        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        //            function (r) {
        //                if (r.confirmed) {
        //                    Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows(entityName);
        //                }

        //            },
        //            function (error) {
        //                console.log("error: " + error.message);
        //            }
        //        );
        //    } else {
        //        Endeavor.Creditsafe.SearchCreditsafe.getSelectedRows(entityName);
        //    }

        //},

        handleRelationTypeValues: function (result) {
            var setToEmpty = false;

            if (result && result.length > 1) {
                var compareID = result[0]._edp_relationtypeid_value;
                for (var i = 1; i < result.length; i++) {
                    if (result[i]._edp_relationtypeid_value !== compareID || result[i]._edp_relationtypeid_value === "00000000-0000-0000-0000-000000000000") {
                        setToEmpty = true;
                        break;
                    }
                }
            }

            if (result && result.length > 0 && !setToEmpty) {
                $("#relation-type").empty();
                var relationTypeName = " -- ";
                var relationId = "empty";
                if (result[0]._edp_relationtypeid_value) {
                    relationId = result[0]._edp_relationtypeid_value;
                }
                if (result[0]["_edp_relationtypeid_value@OData.Community.Display.V1.FormattedValue"]) {
                    relationTypeName = result[0]["_edp_relationtypeid_value@OData.Community.Display.V1.FormattedValue"];
                }
                $("#relation-type").append('<option value="' + relationId + '">' + relationTypeName + '</option>');
                Endeavor.Creditsafe.SearchCreditsafe.addAllOtherRelationTypes();
            } else {
                $("#relation-type").empty();
                $("#relation-type").append("<option value=empty> -- </option>");
                Endeavor.Creditsafe.SearchCreditsafe.addAllOtherRelationTypes();
            }
        },

        saveRelationTypesToGrid: function (jsonArray, result) {
            var selectedIDs = $("#jqGrid").getGridParam("selarrrow");

            for (var j = 0; j < jsonArray.length; j++) {
                for (var i = 0; i < result.length; i++) {
                    if (result[i].accountid === jsonArray[j].CRMId) {

                        if (result[i]._edp_relationtypeid_value) {
                            $("#jqGrid").jqGrid('setRowData', jsonArray[j].rowId, { edp_RelationType: result[i]._edp_relationtypeid_value });
                        }

                        if (result[i]["_edp_relationtypeid_value@OData.Community.Display.V1.FormattedValue"])
                            $("#jqGrid").jqGrid('setRowData', jsonArray[j].rowId, { edp_RelationTypeName: result[i]["_edp_relationtypeid_value@OData.Community.Display.V1.FormattedValue"] });
                    }
                }
            }
        },

        onRowsSelect: function () {
            var jsonArray = [];
            var selectedIDs = $("#jqGrid").getGridParam("selarrrow");
            for (var i = 0; i < selectedIDs.length; i++) {
                var rowCRMID = $("#jqGrid").jqGrid('getCell', selectedIDs[i], 'CRMId');
                var item = {
                    "rowId": selectedIDs[i],
                    "CRMId": rowCRMID
                };
                jsonArray.push(item);
            }

            //if (jsonArray && jsonArray.length > 0) {
            //    Endeavor.Creditsafe.SearchCreditsafe.fetchRelationTypesFromRows(jsonArray).then(
            //        function (result) {
            //            Endeavor.Creditsafe.SearchCreditsafe.saveRelationTypesToGrid(jsonArray, result);
            //            Endeavor.Creditsafe.SearchCreditsafe.handleRelationTypeValues(result);
            //        },
            //        function (error) {
            //            console.log(error);
            //            Endeavor.Creditsafe.SearchCreditsafe.handleRelationTypeValues(null);

            //            //error just set to relationtype to "--" and print message
            //        }
            //    );
            //} else {
            //    Endeavor.Creditsafe.SearchCreditsafe.handleRelationTypeValues(null);
            //}


        },

        //fetchAllRelationTypes: function () {
        //    var entityNameString = "edp_relationtypes";

        //    if (Xrm.Internal.isUci()) {
        //        entityNameString = "edp_relationtype";
        //    }

        //    var query = "?$select=edp_relationtypeid, edp_name";
        //    return Xrm.WebApi.retrieveMultipleRecords(entityNameString, query, 50).then(
        //        function (r) {
        //            if (r && r.entities && r.entities.length > 0) {
        //                GlobalRelationTypes = r.entities;
        //                return "success";
        //            }
        //        },
        //        function (error) {
        //            console.log(error.message);
        //            return "error";
        //        }
        //    );
        //},

        //addAllOtherRelationTypes: function () {
        //    if (!($("#relation-type option[value=empty").length > 0)) {
        //        $("#relation-type").append("<option value=empty> -- </option>");
        //    }

        //    if (GlobalRelationTypes && GlobalRelationTypes.length > 0) {
        //        for (var i = 0; i < GlobalRelationTypes.length; i++) {
        //            if (!($("#relation-type option[value=" + GlobalRelationTypes[i].edp_relationtypeid + "]").length > 0)) {
        //                $("#relation-type").append('<option value="' + GlobalRelationTypes[i].edp_relationtypeid + '">' + GlobalRelationTypes[i].edp_name + '</option>');
        //            }
        //        }
        //    }
        //},

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
                    // Gets info from selected row cells. jqGrid('method', rowId, columnName);
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
                tmpFormContext = parent.Xrm.Page; //becouse this is called from html form
                setTimeout('Endeavor.Creditsafe.SearchCreditsafe.updateCRM(tmpFormContext, selectedRowsJson, selectedRowsCount, null, "' + entityName + '", 0)', 20);
            }
        },

        isEntityFormContext: function (location) {
            if (location != null) {
                if (location.hash != null && location.hash.indexOf('pagetype=entityrecord') > -1) {
                    return true;
                }

                if (location.search != null && location.search.indexOf('pagetype=entityrecord') > -1) {
                    return true;
                }
            }
            return false;
        },

        closeWindowAndRefreshPage: function (refreshParentFlag) {
            window.top.close();
            try {
                if (window != null && window.parent != null && window.parent.opener != null) {
                    if (refreshParentFlag) {
                        if (Endeavor.Creditsafe.SearchCreditsafe.isEntityFormContext(window.parent.opener.location)) {
                            //only do refresh:
                            window.parent.opener.Xrm.Page.data.refresh();
                        } else {
                            //trigger click on ribbon button 'Refresh'
                            var refreshCompleted = false;
                            var buttonElements = window.parent.opener.document.getElementsByTagName('button');
                            for (var i = 0, len = buttonElements.length; i < len && !refreshCompleted; i++) {
                                if (buttonElements[i] != null && buttonElements[i].dataset != null && buttonElements[i].dataset.id != null &&
                                    (
                                        buttonElements[i].dataset.id.toLowerCase() === 'account|norelationship|homepagegrid|mscrm.homepagegrid.account.refreshmodernbutton'
                                        || buttonElements[i].dataset.id.toLowerCase() === 'lead|norelationship|homepagegrid|mscrm.homepagegrid.lead.refreshmodernbutton'
                                    )
                                ) {
                                    refreshCompleted = true;
                                    buttonElements[i].click();
                                }
                            }
                            if (!refreshCompleted) {
                                //reload list, since this is not entity record page type, but a list view:
                                window.parent.opener.location.reload();
                            }
                        }
                        window.top.close();
                    } else {
                        //Refresh record/form
                        window.parent.opener.Xrm.Page.data.refresh();
                        window.top.close();
                    }
                }
            } catch (error) {
                //do nothing
            }
        },

        exitSearchWindow: function (message, doNotRefreshParentFlag) {
            var alertStrings = { confirmButtonLabel: "Ok", text: message }; // + "\n\rNumber of " + entityName + "s processed: " + noOfRecords
            var alertOptions = { height: 240, width: 360 };
            Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                function success(result) {
                    Endeavor.Creditsafe.SearchCreditsafe.closeWindowAndRefreshPage(!doNotRefreshParentFlag);
                },
                function (error) {
                    Endeavor.Creditsafe.SearchCreditsafe.closeWindowAndRefreshPage(!doNotRefreshParentFlag);
                }
            );
        },

        handleDuplicateDetection: function (duplicateIds, action, entityName, doNotRefreshParentFlag, json, returnMode) {
            var message = action === "create" ? Endeavor.Creditsafe.Resources.DuplicateSearchCreate + "\n\n"
                : Endeavor.Creditsafe.Resources.DuplicateSearchUpdate + "\n\n";

            var splitedMessage = duplicateIds.split(",");
            if (splitedMessage.length > 0) {
                for (var i = 0; i < splitedMessage.length; i++) {
                    message += decodeURIComponent(splitedMessage[i]) + "\n";
                }
            }

            var confirmStrings = {
                text: message, title: "Duplicate Found",
                confirmButtonLabel: "Confirm", cancelButtonLabel: "Cancel"
            };
            var confirmOptions = { height: 250, width: 400 };

            return Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                function (result) {
                    if (result.confirmed) {
                        if (returnMode == "closeHtml") {
                            Endeavor.Creditsafe.SearchCreditsafe.run_Waitme("#resultGrid", Endeavor.Creditsafe.Resources.SearchWaitCursorLabel);
                        }
                        return Endeavor.Creditsafe.SearchCreditsafe.forceUpdateCRM(json, doNotRefreshParentFlag, entityName, returnMode);
                    }
                }
            );
        },

        updateEntityRelationType: function (entityId, entityName) {

            if ($("#relation-type").val() === "empty") {
                var entityPluralName = "";
                if (entityName.toLowerCase() == "account") {
                    entityPluralName = "accounts";
                } else if (entityName.toLowerCase() == "lead") {
                    entityPluralName = "leads";
                }
                var req = new XMLHttpRequest();
                req.open("DELETE", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.0/" + entityPluralName + "(" + entityId + ")/edp_RelationTypeId/$ref");
                req.setRequestHeader("Accept", "application/json");
                req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                req.setRequestHeader("OData-MaxVersion", "4.0");
                req.setRequestHeader("OData-Version", "4.0");
                req.onreadystatechange = function () {
                    if (this.readyState === 4) {
                        req.onreadystatechange = null;
                        if (this.status === 204 || this.status === 1223) {
                            //Success - No Return Data - Do Something
                        }
                    }
                };
                req.send();
            } else {
                if (!Xrm.Internal.isUci()) {
                    entityName = entityName.toLowerCase() + "s";
                } else {
                    entityName = entityName.toLowerCase();
                }
                var data = { "edp_RelationTypeId@odata.bind": "/edp_relationtypes(" + $("#relation-type").val() + ")" };
                Xrm.WebApi.updateRecord(entityName, entityId, data).then(
                    function (result) {
                    },
                    function (error) {
                        console.log("error in updaterecord: " + error.message);
                    });
            }


        },

        handleResultInGuiMode: function (response, noOfRecords, entityName, oldJson) {
            try {
                var json = JSON.parse(response);

                var doNotRefreshParentFlag = false;
                Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                var outMessage = "";
                if (json.entityId) { //successful if an id is present
                    doNotRefreshParentFlag = true;
                    outMessage = json.action === "create" ? Endeavor.Creditsafe.Resources.CreateMessage : Endeavor.Creditsafe.Resources.UpdateMessage;

                    if (json.message != null && json.message.indexOf('Please') > 0 && json.message.indexOf('restricted') > 0 && json.message.indexOf('report') > 0) {
                        outMessage += '\n\n' + Endeavor.Creditsafe.Resources.NoReportMessage;
                    }

                } else {
                    if (json.message != null) {
                        outMessage = decodeURIComponent(json.message);
                    }
                }

                if (json.hasDuplicates) {
                    doNotRefreshParentFlag = false;
                    Endeavor.Creditsafe.SearchCreditsafe.handleDuplicateDetection(json.duplicateIds, json.action, entityName, doNotRefreshParentFlag, oldJson, "closeHtml");
                    return "duplicate";
                } else {
                    if (json.entityId) {
                        Endeavor.Creditsafe.SearchCreditsafe.updateEntityRelationType(json.entityId, entityName);
                    }
                    return outMessage;
                }
            } catch (e) {
                console.log(e.toString());
                Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
            }

        },

        handleResultInNonGuiMode: function (formContext, response, entityName, oldJson) {
            var json = JSON.parse(response);

            var outMessage = "";
            if (json.hasDuplicates) {
                Endeavor.Creditsafe.SearchCreditsafe.handleDuplicateDetection(json.duplicateIds, json.action, entityName, true, oldJson, "alertbox").then(
                    function (r) {
                        Endeavor.Creditsafe.SearchCreditsafe.reloadForm(formContext);
                    });
            } else if (json.entityId) {
                outMessage = json.action === "create" ? Endeavor.Creditsafe.Resources.CreateMessage : Endeavor.Creditsafe.Resources.UpdateMessage;

                if (json.message != null && json.message.indexOf('Please') > 0 && json.message.indexOf('restricted') > 0 && json.message.indexOf('report') > 0) {
                    outMessage += '\n\n' + Endeavor.Creditsafe.Resources.NoReportMessage;
                }

                var alertStrings = { confirmButtonLabel: "Ok", text: outMessage };
                var alertOptions = { height: 240, width: 360 };
                Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                    function success(result) {
                        Endeavor.Creditsafe.SearchCreditsafe.reloadForm(formContext);
                    },
                    function (error) {
                        Endeavor.Creditsafe.SearchCreditsafe.reloadForm(formContext);
                    });
            } else {
                outMessage = "No results at all. Something went wrong when communicating with the server.";
                if (json.message != null) {
                    outMessage = decodeURIComponent(json.message);
                }
                formContext.ui.setFormNotification(outMessage, "ERROR");
            }
        },

        reloadForm: function (formContext) {
            //Work item 155 (DevOps) - previously we used another method for the classical interface, but at this point we assume UCI.
            //Thus, formContext.data.refresh() will be enough.
            formContext.data.refresh();
        },

        forceUpdateCRM: function (json, doNotRefreshParentFlag, entityName, returnMode) {
            var request = {
                //IgnoreDuplicateRules: 1, //1 == true, othernum == false
                InAccounts: JSON.stringify(json),
                getMetadata: function () { //maybe this here?
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            //"IgnoreDuplicateRules": {
                            //    "typeName": "Edm.Int32",
                            //    "structuralProperty": 1
                            //},
                            "InAccounts": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1 // Primitive Type
                            }
                        },
                        operationType: 0,
                        operationName: "edp_HandleCompanyData"
                    };
                }
            };

            return Xrm.WebApi.online.execute(request).then(
                function (result) {
                    if (result.ok) {
                        return result.json().then(
                            function (response) {
                                if (response && response.OutMessage) {
                                    var jsonList = response.OutMessage.split('|');
                                    var message = "";
                                    for (var i = 0; i < jsonList.length; i++) {
                                        try {
                                            var json = JSON.parse(jsonList[i]);
                                            if (json.entityId) {
                                                message += json.action === "create" ? Endeavor.Creditsafe.Resources.CreateMessage : Endeavor.Creditsafe.Resources.UpdateMessage + "\n";
                                            } else {
                                                message += decodeURIComponent(json.message);
                                            }
                                            if (returnMode != "nothing" && $("#relation-type") && $("#relation-type").val()) {
                                                Endeavor.Creditsafe.SearchCreditsafe.updateEntityRelationType(json.entityId, entityName);
                                            }
                                        } catch (e) {
                                            message += " Error when trying to create/update account";
                                        }

                                    }
                                    if (returnMode == "closeHtml") {
                                        return Endeavor.Creditsafe.SearchCreditsafe.exitSearchWindow(message, doNotRefreshParentFlag);
                                    } else if (returnMode == "alertbox") {
                                        var alertStrings = { confirmButtonLabel: "Ok", text: message };
                                        var alertOptions = { height: 240, width: 360 };
                                        return Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                                            function () { }
                                        );
                                    } else {
                                        return message;
                                    }

                                }
                            }
                        );
                    }
                },
                function (error) {
                    if (returnMode != "nothing") {
                        alert("Error when trying to force duplicate: " + error.message);
                    }
                });
        },

        updateCRM: function (formContext, json, noOfRecords, noGUI, entityName /*ignoreDuplicateRules*/) {
            try {

                var doNotRefreshParentFlag = false;

                var request = {
                    //IgnoreDuplicateRules: ignoreDuplicateRules, //1 == true, othernum == false
                    InAccounts: JSON.stringify(json),
                    getMetadata: function () { //maybe this here?
                        return {
                            boundParameter: null,
                            parameterTypes: {
                                //"IgnoreDuplicateRules": {
                                //    "typeName": "Edm.Int32",
                                //    "structuralProperty": 1
                                //},
                                "InAccounts": {
                                    "typeName": "Edm.String",
                                    "structuralProperty": 1 // Primitive Type
                                }
                            },
                            operationType: 0,
                            operationName: "edp_HandleCompanyData"
                        };
                    }
                };

                Xrm.WebApi.online.execute(request).then(
                    function (result) {
                        if (result.ok) {
                            result.json().then(
                                function (response) {
                                    Endeavor.Creditsafe.SearchCreditsafe.handleCreditsafeResponse(formContext, response, entityName, json, noOfRecords, noGUI);
                                },
                                function (error) {
                                    Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                                    alert("Error: " + error);
                                }
                            );
                        }
                    },
                    function (error) {
                        var errorMessage = ".";
                        if (error != null && error.message != null) {
                            errorMessage = ":\n\n" + error.message;
                        }
                        errorMessage = "An error occurred when updating CRM" + errorMessage;

                        var alertStrings = { confirmButtonLabel: "Ok", text: errorMessage };
                        var alertOptions = { height: 240, width: 360 };
                        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                            function () {
                                try {
                                    window.top.close();
                                    if (window != null && window.parent != null && window.parent.opener != null) {
                                        //window.parent.opener.location.reload();
                                        window.parent.opener.Xrm.Page.data.refresh();
                                    }
                                    Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                                } catch (error) {
                                    //do nothing
                                }
                            }
                        );
                    });

            } catch (error) {
                if (!noGUI) {
                    Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                }
                alert(error + "\n\nException caught in updateCRM.");
                return false;
            }
        },

        handleCreditsafeResponse: function (formContext, response, entityName, json, noOfRecords, noGUI) {
            try {
                if (response && response.OutMessage) {
                    var jsonList = response.OutMessage.split('|');
                    var guiMessage = "";

                    for (var i = 0; i < jsonList.length; i++) {
                        if (noGUI) {
                            Endeavor.Creditsafe.SearchCreditsafe.handleResultInNonGuiMode(formContext, jsonList[i], entityName, json);
                        } else {
                            guiMessage += Endeavor.Creditsafe.SearchCreditsafe.handleResultInGuiMode(jsonList[i], noOfRecords, entityName, json) + "\n";
                        }
                    }

                    if (!noGUI && jsonList && jsonList.length > 0 && !(guiMessage.indexOf("duplicate") > -1)) {
                        if (jsonList[0].entityId) {
                            Endeavor.Creditsafe.SearchCreditsafe.exitSearchWindow(guiMessage, true);
                        } else {
                            Endeavor.Creditsafe.SearchCreditsafe.exitSearchWindow(guiMessage, false);
                        }
                    } else if (guiMessage !== "" && !(guiMessage.indexOf("duplicate") > -1)) {
                        Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                        Endeavor.Creditsafe.SearchCreditsafe.exitSearchWindow(guiMessage, false);
                    }
                }
            } catch (e) {
                Endeavor.Creditsafe.SearchCreditsafe.turnOffWaitMe();
                alert("Error in handleCreditsafeResponse: " + e);
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

        isUsingInternetExplorer11: function () {
            try {
                var isIE11 = /Trident.*rv[ :]*11\./.test(navigator.userAgent);
                return isIE11;
            } catch (ex) {
                //do nothing
            }

            return false;
        },

        turnOffWaitMe: function () {
            try {
                $("#resultGrid").waitMe('hide');
            } catch (e) {
                console.log(e);
            }
            try {
                $("#searchView").waitMe('hide');
            } catch (e) {
                console.log(e);
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
            if (Endeavor.Creditsafe.SearchCreditsafe.isUsingInternetExplorer11() && !Xrm.Internal.isUci()) {
                var globalContext = Xrm.Utility.getGlobalContext();
                var url = globalContext.getClientUrl() + "/WebResources/edp_/html/CreditsafeSearchView.html?Data=" + encodedParams;
                window.open(url, "SearchWindow", "width=1200, height=470");
            } else {
                var windowOptions = {
                    height: 470,
                    width: 1200,
                    openInNewWindow: true
                };
                Xrm.Navigation.openWebResource("edp_/html/CreditsafeSearchView.html", windowOptions, encodedParams);
            }

        },

    }
};
if (typeof ($) != "undefined") {
    $(document).ready(Endeavor.Creditsafe.SearchCreditsafe.init(Endeavor.Creditsafe.SearchCreditsafe.initialLocalisation));
}