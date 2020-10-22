// References:
/// <reference path="jquery.1.11.1.min.js" />
/// <reference path="SDK.Rest.js" />
/// <reference path="CrmFetchKit.js" />
/// <reference path="Endeavor.Common.Data.js" />

var Xrm = Xrm || window.parent.Xrm;

// Begin scoping 
if (typeof (Endeavor) == "undefined") {
    var Endeavor = {
    };
}

if (typeof (Endeavor.CreditSafe) == "undefined") {
    Endeavor.CreditSafe = {
    };
}

//$("#dialog").dialog({
//    autoOpen: false
//});

if (typeof (Endeavor.CreditSafe.SolutionConfigurationPage) == "undefined") {
    Endeavor.CreditSafe.SolutionConfigurationPage = {

        LicenseName: "EndeavorCreditSafe",

        LicenseFetchXml: function () {
            // Load license from database
            var fetchxml =
                ['<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">',
                    ' <entity name="edp_endeavorlicense">',
                    '   <attribute name="edp_endeavorlicenseid" />',
                    '   <attribute name="edp_name" />',
                    '   <attribute name="edp_licensexml" />',
                    '   <attribute name="edp_licensenumber" />',
                    '   <order attribute="edp_licensenumber" descending="false" />',
                    '   <filter type="and">',
                    '     <condition attribute="edp_name" operator="eq" value="' + Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName + '" />',
                    '   </filter>',
                    ' </entity>',
                    '</fetch>'].join('');

            return fetchxml;
        },

        clearNotifications: function () {
            $("#licenseNoNotification").text("");
            $("#controlNoNotification").text("");
            $("#licenseXmlNotification").text("");
            $("#licenseNotification").text("");
        },

        onLoad: function () {
            try {
                document.getElementById('configFileInput').addEventListener('change', function (e) { Endeavor.CreditSafe.SolutionConfigurationPage.continueImportComfiguration(e); });

                Endeavor.CreditSafe.SolutionConfigurationPage.clearNotifications();

                var fetchxml = Endeavor.CreditSafe.SolutionConfigurationPage.LicenseFetchXml();

                CrmFetchKit.Fetch(fetchxml)
                    .fail(function (xhr, status, errorThrown) {

                        // get the error-message
                        var msg = $(xhr.responseXML).find('Message').text();

                        alert('Error occured while getting license: ' + msg);
                    })
                    .done(function (results) {

                        if (results.length === 0) {
                            // No License found. Do nothing
                            return;
                        }

                        if (results.length > 1) {
                            // More than one License found. Something is wrong. Display Warning.
                            alert('Multiple licenses matching ' + Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName + ' found. The first one will be used.')
                        }

                        // Fill fields (not Control Number)
                        //var licenseId = results[0].getValue('edp_endeavorlicenseid');
                        var licenseNo = results[0].getValue('edp_licensenumber');
                        var licenseXml = results[0].getValue('edp_licensexml');

                        $("#licenseNo").val(licenseNo);
                        $("#licenseXml").val(licenseXml);

                    });

                Endeavor.CreditSafe.SolutionConfigurationPage.displayCredentialFields();
            }
            catch (error) {
                alert("Exception caught in fx:onLoad.\r\n\r\n" + error);
            }
        },

        displayCredentialFields: function () {
            // -- Show only credentials for the service you have configurations for -- //
            $("#757550000").hide();
            $("#757550001").hide();
            iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?";
            var configResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
            for (var i = 0; i < configResultSet.length; i++) {
                if (configResultSet[i].edp_SearchEngine.Value == "757550000") {
                    $("#757550000").show();
                    $("#757550000configId").val(configResultSet[i].edp_CreditsafeConfigurationId);
                    $("#sweCredUsernameTextField").val(configResultSet[i].edp_UserName);
                    $("#sweCredPasswordTextField").val(configResultSet[i].edp_Password);
                    $("#sweCredUsernameTextField").change(function () { Endeavor.CreditSafe.SolutionConfigurationPage.onSweCredentialFieldChange() });
                    $("#sweCredPasswordTextField").change(function () { Endeavor.CreditSafe.SolutionConfigurationPage.onSweCredentialFieldChange() });
                }
                if (configResultSet[i].edp_SearchEngine.Value == "757550001") {
                    $("#757550001").show();
                    $("#757550001configId").val(configResultSet[i].edp_CreditsafeConfigurationId);
                    $("#ggsCredUsernameTextField").val(configResultSet[i].edp_UserName);
                    $("#ggsCredPasswordTextField").val(configResultSet[i].edp_Password);
                    $("#ggsCredUsernameTextField").change(function () { Endeavor.CreditSafe.SolutionConfigurationPage.onGGSCredentialFieldChange() });
                    $("#ggsCredPasswordTextField").change(function () { Endeavor.CreditSafe.SolutionConfigurationPage.onGGSCredentialFieldChange() });
                }
            }
        },

        onSweCredentialFieldChange: function () {
            var newData = {
                edp_UserName: $("#sweCredUsernameTextField").val(),
                edp_Password: $("#sweCredPasswordTextField").val()
            }
            SDK.REST.updateRecord(
                $("#757550000configId").val(),
                newData,
                'edp_CreditsafeConfiguration',
                function (config) { },
                function (e) {
                    alert("Something went wrong when updating configuration credentials for Creditsafe Sweden:\n" + e.toString());
                }
            )
        },

        onGGSCredentialFieldChange: function () {
            var newData = {
                edp_UserName: $("#ggsCredUsernameTextField").val(),
                edp_Password: $("#ggsCredPasswordTextField").val()
            }
            SDK.REST.updateRecord(
                $("#757550001configId").val(),
                newData,
                'edp_CreditsafeConfiguration',
                function (config) { },
                function (e) {
                    alert("Something went wrong when updating configuration credentials for Creditsafe GGS:\n" + e.toString());
                }
            )
        },

        onImportDefaultConfiguration: function () {
            try {
                var xmlHTTP = new XMLHttpRequest();
                try {
                    xmlHTTP.open("GET", "../data/DefaultConfiguration.xml", false);  //place of default configuration
                    xmlHTTP.send(null);
                }
                catch (e) {
                    alert("Unable to load the DefaultConfiguration xml-file.");
                    return;
                }
                var jsonString = xmlHTTP.responseXML.getElementsByTagName("configuration")[0].textContent;
                Endeavor.CreditSafe.SolutionConfigurationPage.ImportConfigurationFromJsonString(jsonString);
            } catch (e) {
                alert("Exception caught in onImportDefaultConfiguration.\r\n\r\n" + e);
            }
        },

        //onImportConfiguration: function () {
        //    try {
        //        var ieVersion = Endeavor.CreditSafe.SolutionConfigurationPage.detectIE();

        //        var fileSelector = document.createElement('input');
        //        fileSelector.setAttribute('type', 'file');
        //        fileSelector.setAttribute('accept', 'text/xml');
        //        fileSelector.addEventListener('change', function (e) { Endeavor.CreditSafe.SolutionConfigurationPage.continueImportComfiguration(e); });
        //        document.body.appendChild(fileSelector);
        //        fileSelector.click();

        //    } catch (e) {
        //        alert("Exception caught in onImportConfiguration.\r\n\r\n" + e);
        //    }
        //},

        continueImportComfiguration: function (e) {     //helper function to 'pause' process when choosing file
            try {
                var file = e.target.files[0];
                if (!file) {
                    return;
                }
                var reader = new FileReader();
                reader.onload = function (e) {
                    var xmlString = e.target.result;
                    var xmlFile = new DOMParser().parseFromString(xmlString, 'text/xml');
                    var jsonString = xmlFile.getElementsByTagName("configuration")[0].textContent;
                    Endeavor.CreditSafe.SolutionConfigurationPage.ImportConfigurationFromJsonString(jsonString);
                };
                reader.readAsText(file);
            } catch (e) {
                alert("Exception caught in continueImportConfiguration.\r\n\r\n" + e);
            }
        },

        ImportConfigurationFromJsonString: function (jsonString) {
            try {
                var sdkObject = new Sdk.edp_ImportCreditsafeConfigurationRequest(jsonString);
                Sdk.Sync.execute(sdkObject);
                Endeavor.CreditSafe.SolutionConfigurationPage.displayCredentialFields();
                alert("Configuration imported.");
            } catch (error) {
                alert("Exception caught in ImportConfigurationFromJsonString.\r\n\r\n" + error);
            }
        },

        onExportConfiguration: function () {
            try {
                var sdkObject = new Sdk.edp_ExportCreditsafeConfigurationRequest();
                var response = Sdk.Sync.execute(sdkObject);

                if (response == null) {
                    throw new DOMException();
                }
                else {
                    var jsonString = response.getExportConfiguration();

                    var xmlString = "<configuration>" + jsonString +
                        "</configuration>";
                    var parser = new DOMParser();
                    var xmlDoc = parser.parseFromString(xmlString, "text/xml");
                    //alert("xmldoc toString =\n" + xmlDoc.toString);
                    var xmlText = new XMLSerializer().serializeToString(xmlDoc);

                    var ieVersion = Endeavor.CreditSafe.SolutionConfigurationPage.detectIE();
                    // false == 0
                    if (ieVersion < 1) {
                        var pom = document.createElement('a');
                        //console.log(response);
                        //alert("response =\n" + response + "\n\nJsonString =\n" + jsonString);
                        pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(xmlText));
                        pom.setAttribute('download', 'config.xml');
                        if (document.createEvent) {
                            var event = document.createEvent('MouseEvents');
                            event.initEvent('click', true, true);
                            pom.dispatchEvent(event);
                        }
                        else {
                            pom.click();
                        }
                    } else { // If the user is running Internet Explorer or Egde.
                        var blobObject = new Blob([xmlText], { type: 'text/xml' });
                        var retVal = navigator.msSaveBlob(blobObject, 'config.xml');
                        if (!retVal) {
                            alert("download failed");
                        }
                    }
                }
            } catch (e) {
                alert("Exception caught in onExportConfiguration.\r\n\r\n" + e);
            }
        },

        onValidateLicense: function () {
            var sdkObject = new Sdk.edp_GetLicenseInformationRequest();
            //var sdkIbject = new Sdk.edp_ExportCreditsafeConfigurationRequest();
            var response = Sdk.Sync.execute(sdkObject);

            if (response == null) {
                throw new DOMException();
            }
            else {

                var licenseInformation = response.getLicenseInformation();
                alert(licenseInformation);


                alert('d' + jsonString);
                $("#button").on("click", function () {
                    $("#dialog").dialog("open");
                });
            }
        },


        onApplyLicense: function () {
            /// <summary>
            /// Applies license and stores it in the database.
            /// </summary>
            try {

                Endeavor.CreditSafe.SolutionConfigurationPage.clearNotifications();

                // Check that license no has value
                if ($("#licenseNo").val().length == 0) {
                    $("#licenseNoNotification").text("The license number must be entered.");
                    return;
                }
                // Check that control no has value
                if ($("#controlNo").val().length == 0) {
                    $("#controlNoNotification").text("The control number must be entered.");
                    return;
                }
                if ($("#licenseXml").val().length == 0) {
                    $("#licenseXmlNotification").text("The license Xml must be entered.");
                    return;
                }

                // Create/Update entity.
                var fetchxml = Endeavor.CreditSafe.SolutionConfigurationPage.LicenseFetchXml();

                CrmFetchKit.Fetch(fetchxml)
                    .fail(function (xhr, status, errorThrown) {

                        // get the error-message
                        var msg = $(xhr.responseXML).find('Message').text();

                        alert('Error occured while getting license: ' + msg);
                    })
                    .done(function (results) {

                        if (results.length === 0) {
                            // No License found. Create New Item
                            var license = {};
                            license.edp_Name = Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName;
                            license.edp_LicenseNumber = $("#licenseNo").val();
                            license.edp_ControlNumber = parseInt($("#controlNo").val().replace(/\s/g, ""));
                            license.edp_LicenseXml = $("#licenseXml").val();

                            SDK.REST.createRecord(
                                license,
                                "edp_EndeavorLicense",
                                function (license) {
                                    $("#licenseNotification").val("License saved.")

                                }, function (licenseError) {
                                    alert("CreateRecord (license) generated an error: " + licenseError);
                                });
                            return;
                        }

                        if (results.length > 1) {
                            // More than one License found. Something is wrong. Display Warning.
                            alert('Multiple licences matching ' + Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName + ' found. The first one will be used.')
                        }

                        var licenseId = results[0].getValue('edp_endeavorlicenseid');
                        var license = {};
                        license.edp_LicenseNumber = $("#licenseNo").val();
                        license.edp_ControlNumber = parseInt($("#controlNo").val().replace(/\s/g, ""));
                        license.edp_LicenseXml = $("#licenseXml").val();

                        SDK.REST.updateRecord(
                            licenseId,
                            license,
                            "edp_EndeavorLicense",
                            function (license) {
                                $("#licenseNotification").text("License saved.")

                            }, function (licenseError) {
                                alert("UpdateRecord (license) generated an error: " + licenseError);
                            });
                    });
            }
            catch (error) {
                alert("Exception caught in fx:onLoad.\r\n\r\n" + error);
            }
        },

        onUpdateLicense: function () {
            /// <summary>
            /// Update License from central License Server.
            /// </summary>
            try {
                Endeavor.CreditSafe.SolutionConfigurationPage.clearNotifications();

                alert("Update License is not implemented at this time.\r\n\r\nPlease contact Endeavor Support to update your license.");
            }
            catch (error) {
                alert("Exception caught in fx:onUpdateLicense. Message:\r\n\r\n" + error);
            }
        },

        onDeleteLicense: function () {
            /// <summary>
            /// Cancel changes.
            /// </summary>
            try {
                // Restore data from database (if exist), otherwise clear.
                Endeavor.CreditSafe.SolutionConfigurationPage.clearNotifications();

                var message = "Do you really want to delete the license " + Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName + " from the system?\r\n\The solution might become unusable.";

                if (confirm(message)) {

                    // Delete entity.
                    var fetchxml = Endeavor.CreditSafe.SolutionConfigurationPage.LicenseFetchXml();

                    CrmFetchKit.Fetch(fetchxml)
                        .fail(function (xhr, status, errorThrown) {

                            // get the error-message
                            var msg = $(xhr.responseXML).find('Message').text();

                            alert('Error occured while getting license: ' + msg);
                        })
                        .done(function (results) {

                            if (results.length === 0) {
                                // No License found. Do nothing
                                return;
                            }

                            if (results.length > 1) {
                                // More than one License found. Something is wrong. Display Warning.
                                alert('Multiple licences matching ' + Endeavor.CreditSafe.SolutionConfigurationPage.LicenseName + ' found. The first one will be deleted.')
                            }

                            var licenseId = results[0].getValue('edp_endeavorlicenseid');

                            SDK.REST.deleteRecord(
                                licenseId,
                                "edp_EndeavorLicense",
                                function (license) {
                                    $("#licenseNotification").text("License deleted.")
                                    $("#licenseNo").val("");
                                    $("#controlNo").val("");
                                    $("#licenseXml").val("");
                                }, function (licenseError) {
                                    alert("DeleteRecord (license) generated an error: " + licenseError);
                                });
                        });
                }
            }
            catch (error) {
                alert("Exception caught in fx:onDeleteLicense. Message:\r\n\r\n" + error);
            }
        },

        initiateGrid: function () {

            Endeavor.CreditSafe.SolutionConfigurationPage.headLoad(Endeavor.CreditSafe.SolutionConfigurationPage.loadSuccessCallback);
            table = this.getDataFromDB();
            Endeavor.CreditSafe.SolutionConfigurationPage.createGrid(table);
        },

        headLoad: function (successCallback) {
            var jsUrls = [];
            var jsUrl;

            if (typeof SDK == "undefined" || typeof SDK.REST == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/SDK.Rest.js";
                jsUrls.push(jsUrl);
            }
            if (typeof Endeavor == "undefined" || typeof Endeavor.Common == "undefined" || typeof Endeavor.Common.Data == "undefined") {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/Endeavor.Common.Data.js";
                jsUrls.push(jsUrl);
            }
            if (typeof jQuery == 'undefined') {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/jquery.1.11.1.min.js";
                jsUrls.push(jsUrl);
            }

            try {
                $("#tabs").tabs();
            } catch (e) {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/jquery.ui.1.12.1.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof typeof jqGridUtils == 'undefined') {
                jsUrl = Xrm.Page.context.getClientUrl() + "/WebResources/edp_/script/jquery.jqGrid.min.js";
                jsUrls.push(jsUrl);
            }

            if (typeof head.load != "function") {
                console.error("head.load function is not defined.");
                throw new Error("head.load function is not defined.");
            }

            if (jsUrls.length > 0) {
                // Load required JavaScripts
                head.load(jsUrls, successCallback);
            }
            else {
                successCallback();
            }
        },

        loadSuccessCallback: function () {
            console.log("Everything loaded!");
            $("#tabs").tabs();
        },

        getDataFromDB: function () {
            /* -- Retrieving User Data -- */
            var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "SystemUserSet?$select=FullName, SystemUserId, BusinessUnitId&$filter=(IsDisabled eq false) and (AccessMode/Value ne 3) and (AccessMode/Value ne 5)&$orderby=FullName";
            var usersResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
            /* -- Retrieving Roles Data -- */
            iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "RoleSet?$select=Name, RoleId, BusinessUnitId&$filter=(Name eq 'Creditsafe Admin User') or (Name eq 'Creditsafe Basic User')";
            var rolesResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
            if (rolesResultSet == null || rolesResultSet.length < 1) {
                throw new Error("No roles in Table");
            }
            var userRolefilter = "";
            for (var i = 0; i < rolesResultSet.length; i++) {
                if (i > 0) {
                    userRolefilter.concat(" or ");
                }
                userRolefilter.concat("(RoleId/Id eq (guid'" + rolesResultSet[i].RoleId + "')");
            }
            /* -- Retrieving Users in Roles Data -- */
            iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "SystemUserRolesSet?$select=RoleId, SystemUserId, SystemUserRoleId&$filter=" + userRolefilter;
            var userAndRolesResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
            /* -- Retrieving Login Details -- */
            iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeConfigurationSet?$filter=edp_CreditsafeConfigurationId eq (guid'3da0c9b2-40ee-e411-80f9-d4ae52cd182c')";
            var configResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
            this.sweConfig = configResultSet[0];

            iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "edp_CreditsafeUserConfigurationSet?$select=*";
            var userLoginDataResultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);


            /* -- Order the data and create grid -- */
            var table = Endeavor.CreditSafe.SolutionConfigurationPage.makeOrderedTable(usersResultSet, rolesResultSet, userAndRolesResultSet, userLoginDataResultSet);
            return table;
        },

        makeOrderedTable: function (usersResultSet, rolesResultSet, userAndRolesResultSet, userLoginDataResultSet) {
            for (var i = 0; i < usersResultSet.length; i++) {
                Endeavor.CreditSafe.SolutionConfigurationPage.fillExistingRoleIds(usersResultSet[i], rolesResultSet);
                Endeavor.CreditSafe.SolutionConfigurationPage.fillUserBURoleIds(usersResultSet[i], userAndRolesResultSet);
                Endeavor.CreditSafe.SolutionConfigurationPage.fillLoginData(usersResultSet[i], userLoginDataResultSet);
            }
            return usersResultSet;
        },

        fillExistingRoleIds: function (user, rolesResultSet) {
            for (var j = 0; j < rolesResultSet.length; j++) {
                if (rolesResultSet[j].BusinessUnitId.Id == user.BusinessUnitId.Id) {
                    if (rolesResultSet[j].Name == "Creditsafe Admin User") {
                        user.MyUnitAdminId = rolesResultSet[j].RoleId;
                    } else if (rolesResultSet[j].Name == "Creditsafe Basic User") {
                        user.MyUnitBasicId = rolesResultSet[j].RoleId;
                    }
                }
                if (user.MyUnitAdminId != null && user.MyUnitBasicId != null) {
                    break;
                }
            }
        },

        fillUserBURoleIds: function (user, userAndRolesResultSet) {
            user.MyUserAdminId = null;
            user.isAdmin = false;
            user.MyUserBasicId = null;
            user.isBasic = false;
            for (var j = 0; j < userAndRolesResultSet.length; j++) {
                if (userAndRolesResultSet[j].SystemUserId == user.SystemUserId) {
                    if (userAndRolesResultSet[j].RoleId == user.MyUnitAdminId) {
                        user.MyUserAdminId = userAndRolesResultSet[j].SystemUserRoleId;
                        user.isAdmin = true;
                    } else if (userAndRolesResultSet[j].RoleId == user.MyUnitBasicId) {
                        user.MyUserBasicId = userAndRolesResultSet[j].SystemUserRoleId;
                        user.isBasic = true;
                    }
                }
                if (user.MyUserAdminId != null && user.MyUserBasicId != null) {
                    break;
                }
            }

        },

        fillLoginData: function (user, userLoginDataResultSet) {
            try {
                var result;
                user.Username = null;
                user.Password = null;
                user.UserConfig = null;
                for (var j = 0; j < userLoginDataResultSet.length; j++) {
                    result = userLoginDataResultSet[j];
                    if (result.edp_SystemUserId.Id == user.SystemUserId) {
                        user.UserConfig = result;
                        if (result.edp_Username != null) {
                            user.Username = result.edp_Username;
                            if (result.edp_Password == null) {
                                throw Error('No password for user: ' + user.FullName);
                            }
                            user.Password = result.edp_Password;
                        } else {
                            throw Error("No Username for " + user.FullName);
                        }
                    }
                }
            } catch (e) {
                alert("Error in fillLoginData:\n" + e.toString());
            }
        },

        createGrid: function (table) {
            try {
                this.usersData = Endeavor.CreditSafe.SolutionConfigurationPage.serializeData(table);
                $('#jqGrid').jqGrid({
                    data: this.usersData,
                    datatype: 'local',
                    rowNum: 8,
                    //rowheight: 12,
                    colNames: ['Name', 'User', 'Admin', 'Username', 'Password', 'SystemUserId', 'MyUnitAdminId', 'MyUnitBasicId', 'MyUserAdminId', 'MyUserBasicId', 'OldUsername', 'OldPassword', 'UserConfig'],
                    colModel: [
                        { name: 'FullName', index: 'FullName', sorttype: 'string', width: 300, editable: false, },
                        { name: 'isBasic', index: 'isBasic', editable: true, edittype: 'checkbox', editoptions: { value: 'True:False' }, width: 50, formatter: "checkbox", formatoptions: { disabled: false }, align: 'center', classes: 'check' },
                        { name: 'isAdmin', index: 'isAdmin', editable: true, edittype: 'checkbox', editoptions: { value: 'True:False' }, width: 50, formatter: "checkbox", formatoptions: { disabled: false }, align: 'center', classes: 'check' },
                        { name: 'Username', editable: true, edittype: 'text', formatoptions: { disabled: false }, width: 150, align: 'left', sortable: false },
                        { name: 'Password', editable: true, edittype: 'text', formatoptions: { disabled: false }, width: 150, align: 'left', sortable: false },
                        /* hidden internal columns */
                        { name: 'SystemUserId', width: 10, sortable: false, hidden: true },
                        { name: 'MyUnitAdminId', width: 10, sortable: false, hidden: true },
                        { name: 'MyUnitBasicId', width: 10, sortable: false, hidden: true },
                        { name: 'MyUserAdminId', width: 10, sortable: false, hidden: true },
                        { name: 'MyUserBasicId', width: 10, sortable: false, hidden: true },
                        { name: 'OldUsername', width: 10, sortable: false, hidden: true },
                        { name: 'OldPassword', width: 10, sortable: false, hidden: true },
                        { name: 'UserConfig', width: 10, sortable: false, hidden: true }
                    ],
                    loadonce: false,
                    pager: '#pager',
                    pagerpos: "right",
                    rowList: [],          // disable page size dropdown
                    pgbuttons: true,     // disable page control like next, back button
                    pgtext: '{0} of {1}',       // disable pager text like 'Page 0 of 10'
                    pginput: true,
                    viewrecords: false,
                    cellEdit: false,
                    cellsubmit: 'clientArray',
                    sortname: 'FullName',
                    height: '65%',
                    footerrow: false,
                    forceFit: true,         /* the adjacent column (to the right) will resize */
                    shrinkToFit: true,
                    hidegrid: false,
                    gridview: true,
                    loadComplete: function () {
                        var iColb = Endeavor.CreditSafe.SolutionConfigurationPage.getColumnIndexByName($(this), 'isBasic'),
                            iCola = Endeavor.CreditSafe.SolutionConfigurationPage.getColumnIndexByName($(this), 'isAdmin'),
                            rows = this.rows, i, c = rows.length;

                        for (i = 1; i < c; i += 1) {
                            $(('input[type="checkbox"]'), rows[i].cells[iColb]).click(function (e) {
                                var id = $(e.target).closest('tr')[0].id,
                                    isChecked = $(e.target).is(':checked');
                                var rowId = parseInt(id) + $('#jqGrid').jqGrid('getGridParam', 'rowNum') * ($('#jqGrid').jqGrid('getGridParam', 'page') - 1);
                                Endeavor.CreditSafe.SolutionConfigurationPage.setData(rowId - 1, 'isBasic', isChecked);
                            });
                            $(('input[type="checkbox"]'), rows[i].cells[iCola]).click(function (e) {
                                var id = $(e.target).closest('tr')[0].id,
                                    isChecked = $(e.target).is(':checked');
                                var cellUpdate = new Object();
                                cellUpdate['isAdmin'] = isChecked;
                                var rowId = parseInt(id) + $('#jqGrid').jqGrid('getGridParam', 'rowNum') * ($('#jqGrid').jqGrid('getGridParam', 'page') - 1);
                                Endeavor.CreditSafe.SolutionConfigurationPage.setData(rowId - 1, 'isAdmin', isChecked);
                            });
                        }
                    },
                })

                jQuery("#jqGrid")
                    .jqGrid('navGrid', "#pager", { edit: false, add: false, del: false, search: false, refresh: false })
                    .jqGrid('navButtonAdd', "#pager", {
                        caption: 'Save Changes',
                        buttonicon: 'ui-icon-disk',
                        onClickButton: function () {
                            Endeavor.CreditSafe.SolutionConfigurationPage.onApplyUserPrivileges()
                        },
                        position: 'first'
                    });

            } catch (e) {
                alert("Error cought in CreateGrid!: \n\n" + e.toString());
            }
        },

        setData: function (rowId, column, value) {
            var allData = $('#jqGrid').jqGrid('getGridParam', 'data');
            var row = allData[rowId];
            row[column] = value;
            allData[rowId] = row;
            var retVal = $('#jqGrid').jqGrid('setGridParam', 'data', allData);
        },

        refreshGrid: function () {
            this.usersData = this.getDataFromDB();
            $('#jqGrid').trigger("reloadGrid");
        },

        getColumnIndexByName: function (grid, columnName) {
            var cm = grid.jqGrid('getGridParam', 'colModel'), i, l;
            for (i = 1, l = cm.length; i < l; i += 1) {
                if (cm[i].name === columnName) {
                    return i; // return the index
                }
            }
            return -1;
        },

        /* Modify the JSON to fit the 'Local' datatype */
        serializeData: function (table) {
            var myData = [];
            for (var i = 0; i < table.length; i++) {
                var u = table[i];
                myData[i] = {
                    FullName: u.FullName,
                    isBasic: u.isBasic,
                    isAdmin: u.isAdmin,
                    Username: u.Username,
                    Password: u.Password,
                    SystemUserId: u.SystemUserId,
                    MyUnitAdminId: u.MyUnitAdminId,
                    MyUnitBasicId: u.MyUnitBasicId,
                    MyUserAdminId: u.MyUserAdminId,
                    MyUserBasicId: u.MyUserBasicId,
                    OldUsername: u.Username,
                    OldPassword: u.Password,
                    UserConfig: u.UserConfig
                };
            }
            return myData;
        },

        onApplyUserPrivileges: function () {
            var grid = $('#jqGrid');
            var Data = grid.jqGrid('getGridParam', 'data');
            for (var i = 0; i < Data.length; i++) {
                var rowData = Data[i];
                /* Is Basic modified? */
                if (rowData.isBasic != (rowData.MyUserBasicId != null)) {
                    if (rowData.MyUserBasicId != null) { // If there exists an association, remove
                        Endeavor.CreditSafe.SolutionConfigurationPage.removePrivilege(rowData, rowData.MyUnitBasicId);
                        rowData.MyUserBasicId = null;
                    } else { //If no association exists, create one
                        Endeavor.CreditSafe.SolutionConfigurationPage.addPrivilege(rowData, rowData.MyUnitBasicId);
                    }
                }
                /* Is Admin modified? */
                if (rowData.isAdmin != (rowData.MyUserAdminId != null)) {
                    if (rowData.MyUserAdminId != null) {
                        Endeavor.CreditSafe.SolutionConfigurationPage.removePrivilege(rowData, rowData.MyUnitAdminId);
                        rowData.MyUserAdminId = null;
                    } else {
                        Endeavor.CreditSafe.SolutionConfigurationPage.addPrivilege(rowData, rowData.MyUnitAdminId);
                    }
                }
                /* Is Username or Password modified? */
                if (rowData.Username == "")
                    rowData.Username = null;
                if (rowData.Password == "")
                    rowData.Password = null;
                if (rowData.Username != null && rowData.Password == null) {
                    alert("Password needed for user: " + rowData.FullName);
                    break;
                }
                if (rowData.Username != rowData.OldUsername || rowData.Password != rowData.OldPassword) {
                    Endeavor.CreditSafe.SolutionConfigurationPage.updateUsernamePassword(rowData);
                    rowData.OldUsername = rowData.Username;
                    rowData.OldPassword = rowData.Password;
                }
            }
            this.refreshGrid();
        },

        removePrivilege: function (row, unitRoleId) {
            var userId = row.SystemUserId;
            try {
                var roleEntRef = Sdk.EntityReference('role', unitRoleId);
                var collection = Sdk.Collection(Sdk.EntityReference, new Array(roleEntRef));
                Sdk.Sync.disassociate(
                    'systemuser',
                    userId,
                    'systemuserroles_association',
                    collection
                );
            } catch (e) {
                alert("Error in removePrivilege: \n" + e.toString());
            }
        },

        addPrivilege: function (row, unitRoleId) {
            var userId = row.SystemUserId;
            try {
                var roleEntRef = Sdk.EntityReference('role', unitRoleId);
                var collection = Sdk.Collection(Sdk.EntityReference, new Array(roleEntRef));
                Sdk.Sync.associate(
                    'systemuser',
                    userId,
                    'systemuserroles_association',
                    collection
                );
                var iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "SystemUserRolesSet?$select=SystemUserRoleId&$filter=(RoleId eq (guid'" + unitRoleId + "')) and (SystemUserId eq (guid'" + userId + "'))";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                var res = resultSet[0];
                iMUrl = Endeavor.Common.Data.getOrganizationServiceEndpoint() + "RoleSet?$filter=RoleId eq (guid'" + unitRoleId + "')";
                var resultSet = Endeavor.Common.Data.fetchJSONResults(iMUrl);
                if (resultSet[0].Name == 'Creditsafe Admin User')
                    row.MyUserAdminId = res.SystemUserRoleId;
                else
                    row.MyUserBasicId = res.SystemUserRoleId;
            } catch (e) {
                alert("Error in addPrivilege: \n" + e.toString());
            }
        },

        errorHandler: function (error) {
            alert(error.message);
        },

        updateUsernamePassword: function (row) {
            try {
                if (row.UserConfig == null) {
                    row.UserConfig = {
                        edp_SystemUserId: {
                            Id: row.SystemUserId,
                            LogicalName: 'systemuser',
                            Name: row.FullName
                        },
                        OwnerId: {
                            Id: row.SystemUserId,
                            LogicalName: 'systemuser',
                            Name: row.FullName
                        },
                        edp_CreditsafeConfigurationId: {
                            Id: this.sweConfig.edp_CreditsafeConfigurationId,
                            LogicalName: 'edp_creditsafeconfiguration',
                            Name: this.sweConfig.edp_Name
                        },
                        edp_Name: row.FullName,
                        edp_Username: row.Username,
                        edp_Password: row.Password

                    }
                    SDK.REST.createRecord(
                        row.UserConfig,
                        'edp_CreditsafeUserConfiguration',
                        function (config) { },
                        function (e) {
                            alert("Something went wrong when creating config:\n" + e.toString());
                        }
                    )
                } else {
                    if (row.Username == null) {
                        if (row.OldUsername == null) { throw Error("Config without username exists.") }
                        SDK.REST.deleteRecord(
                            row.UserConfig.edp_CreditsafeUserConfigurationId,
                            'edp_CreditsafeUserConfiguration',
                            function () { },
                            function (e) {
                                alert("Failed when trying to delete record:" + e.toString());
                            }
                        )
                    } else {
                        if (row.Password == null)
                            throw Error("No password for user: " + row.FullName);
                        if (row.UserConfig == null)
                            throw Error("No UserConfig for user: " + row.FullName);
                        var newData = {
                            edp_Username: row.Username,
                            edp_Password: row.Password
                        }
                        SDK.REST.updateRecord(
                            row.UserConfig.edp_CreditsafeUserConfigurationId,
                            newData,
                            'edp_CreditsafeUserConfiguration',
                            function (config) { },
                            function (e) {
                                alert("Something went wrong when updating config:\n" + e.toString());
                            }
                        )
                    }
                }
            } catch (e) {
                alert("Error in updateUsernamePassword:\n" + e.toString());
            }
        },

        /**
         * detect IE
         * returns version of IE or false, if browser is not Internet Explorer or Edge
         */
        detectIE: function () {
            var ua = window.navigator.userAgent;

            var msie = ua.indexOf('MSIE ');
            if (msie > 0) {
                // IE 10 or older => return version number
                return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
            }

            var trident = ua.indexOf('Trident/');
            if (trident > 0) {
                // IE 11 => return version number
                var rv = ua.indexOf('rv:');
                return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
            }

            var edge = ua.indexOf('Edge/');
            if (edge > 0) {
                // Edge (IE 12+) => return version number
                return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
            }

            // other browser
            return false;
        },
    };
}